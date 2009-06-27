using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace icfp09
{
    using System.Diagnostics;
    
    public class VmState
    {
        public double[] _memory = new double[1 << 14];
        public double[] _input = new double[1 << 14];
        public double[] _output = new double[1 << 14];
        public bool _status;
        public int _elapsed;

        public VmState(double[] memory, double[] input, double[] output, bool status, int elapsed)
        {
            memory.CopyTo(_memory, 0);
            input.CopyTo(_input, 0);
            output.CopyTo(_output, 0);
            _status = status;
            _elapsed = elapsed;
        }
    }

    public class VmProgram
    {
        // vector of instructions
        private readonly int[] _instructions = new int[1 << 14];

        // initial memory
        private readonly double[] _data = new double[1 << 14];

        // number of instructions
        private readonly int _length;

        public int[] Instructions
        {
            get { return _instructions; }
        }

        public double[] Data
        {
            get { return _data; }
        }

        public int Length
        {
            get { return _length;  }
        }

        public VmProgram(string filename)
        {
            var info = new FileInfo(filename);
            Debug.Assert(info.Length % 12 == 0); // executable is a collection of 12-byte frames
            var framedata = new byte[info.Length];

            using(var f = new FileStream(filename, FileMode.Open))
                f.Read(framedata, 0, (int)info.Length);

            _length = (int)info.Length / 12;

            for(int i = 0; i < _length; i++)
            {
                if (i % 2 == 0) // even frames are |double|int|
                {
                    _data[i] = BitConverter.ToDouble(framedata, i * 12);
                    _instructions[i] = BitConverter.ToInt32(framedata, (i * 12) + 8);
                }
                else // odd frames are |int|double|
                {
                    _instructions[i] = BitConverter.ToInt32(framedata, i * 12);
                    _data[i] = BitConverter.ToDouble(framedata, (i * 12) + 4);
                }
            }
        }
    }

    public class VirtualMachineStepArgs : EventArgs
    {
        public double Score { get; private set; }
        public double Fuel { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double TargetX { get; private set; }
        public double TargetY { get; private set; }
        public int Elapsed { get; private set; }

        public VirtualMachineStepArgs(int elapsed, double score, double fuel, double x, double y, double targetx, double targety)
        {
            Elapsed = elapsed;
            Score = score;
            Fuel = fuel;
            X = x;
            Y = y;
            TargetX = targetx;
            TargetY = targety;
        }

        public double OrbitRadius
        {
            get
            {
                return Position.length();
            }
        }

        public Vector2d Position
        {
            get
            {
                return new Vector2d(X, Y);
            }
        }

        public Vector2d TargetPosition
        {
            get
            {
                return new Vector2d(X - TargetX, Y - TargetY);
            }
        }

        public double TargetOrbitRadius
        {
            get
            {
                return TargetPosition.length();
            }
        }

        public double TargetDistance
        {
            get
            {
                return (Position - TargetPosition).length();
            }
        }
    }

    public class VirtualMachine
    {
        private class InputChange
        {
            public int Address { get; private set; }
            public double Value { get; private set; }
            public InputChange(int address, double value)
            {
                Address = address;
                Value = value;
            }
        }

        public EventHandler<VirtualMachineStepArgs> OnStep;

        private VmProgram _program;
        private double[] _memory = new double[1 << 14];
        private double[] _input = new double[1 << 14];
        private double[] _output = new double[1 << 14];
        private bool _status = false;

        private List<InputChange> _changes = new List<InputChange>();

        private BinaryWriter _traceFile;

        public int Elapsed
        {
            get; private set;
        }
        public double Configuration
        {
            get
            {
                return _input[0x3e80];
            }
            set
            {
                _input[0x3e80] = value;
                if(_traceFile != null)
                    _changes.Add(new InputChange(0x3e80, value));
            }
        }
        public double XVelocity
        {
            set
            {
                _input[0x2] = value;
                if (_traceFile != null)
                    _changes.Add(new InputChange(0x2, value));

            }
        }
        public double YVelocity
        {
            set
            {
                _input[0x3] = value;
                if (_traceFile != null)
                    _changes.Add(new InputChange(0x3, value));

            }
        }

        public double[] InPort
        {
            get { return _input; }
        }
        public double[] OutPort
        {
            get { return _output; }
        }

        public VirtualMachine(VmProgram program)
        {
            _program = program;
            this.Reset();
        }

        public void Reset()
        {
            _input = new double[1 << 14];
            _output = new double[1 << 14];
            _program.Data.CopyTo(_memory, 0);
            _status = false;
            Elapsed = 0;
        }

        public void Reset(VmState state)
        {
            state._memory.CopyTo(_memory, 0);
            state._input.CopyTo(_input, 0);
            state._output.CopyTo(_output, 0);
            _status = state._status;
            Elapsed = state._elapsed;
        }

        public VmState SnapShot()
        {
            return new VmState(_memory, _input, _output, _status, Elapsed);
        }

        public void StartTrace(string filename, uint scenario)
        {
            const uint header = 0xCAFEBABE;
            const uint teamid = 150;

            Debug.WriteLine("saving trace to \"" + filename + "\"");
            _traceFile = new BinaryWriter(File.OpenWrite(filename));
            _traceFile.Write(header);
            _traceFile.Write(teamid);
            _traceFile.Write(scenario);
        }

        public void EndTrace()
        {
            Debug.WriteLine("ending trace (Elapsed = " + Elapsed + ")");
            const uint end = 0;
            _traceFile.Write(Elapsed);
            _traceFile.Write(end);
            _traceFile.Close();
            _traceFile = null;
        }

        public VirtualMachineStepArgs Step()
        {
            Debug.Assert(Configuration != 0.0);

            for (int ip = 0; ip <= _program.Length; ip++)
            {
                #region Interpreter
                int frame = _program.Instructions[ip];
                if((frame & (0xF0000000)) == 0) // S-Type
                {
                    int op = frame >> 24;
                    int imm = (frame >> 21) & 0x07;
                    int r1 = frame & 0x3FFF;

                    switch(op)
                    {
                        case 0x0: // noop
                            break;

                        case 0x1: // Cmpz
                            switch(imm)
                            {
                                case 0x0: // <
                                    _status = _memory[r1] < 0.0;
                                    break;
                                case 0x1: // <=
                                    _status = _memory[r1] <= 0.0;
                                    break;
                                case 0x2: // ==
                                    _status = _memory[r1] == 0.0;
                                    break;
                                case 0x3: // >=
                                    _status = _memory[r1] >= 0.0;
                                    break;
                                case 0x4: // >
                                    _status = _memory[r1] > 0.0;
                                    break;
                                default:
                                    Debug.Assert(false, "Illegal S-Type ImmCode: " + String.Format("0x{0:x4}", imm));
                                    break;
                            }
                            break;

                        case 0x2: // Sqrt
                            _memory[ip] = Math.Sqrt(_memory[r1]);
                            break;

                        case 0x3: // Copy
                            _memory[ip] = _memory[r1];
                            break;

                        case 0x4: // Input
                            _memory[ip] = _input[r1];
                            break;

                        default:
                            Debug.Assert(false, "Illegal S-Type Opcode: " + String.Format("0x{0:x4}", op));
                            break;
                    }
                }
                else // D-Type
                {
                    int op = frame >> 28;
                    int r1 = (frame & 0x0FFFFFFF) >> 14;
                    int r2 = frame & 0x3FFF;

                    switch(op)
                    {
                        case 0x1: // Add
                            _memory[ip] = _memory[r1] + _memory[r2];
                            break;

                        case 0x2: // Subtract
                            _memory[ip] = _memory[r1] - _memory[r2];
                            break;

                        case 0x3: // Multiply
                            _memory[ip] = _memory[r1] * _memory[r2];
                            break;

                        case 0x4: // Divide
                            if (_memory[r2] == 0.0)
                                _memory[ip] = 0.0;
                            else
                                _memory[ip] = _memory[r1] / _memory[r2];
                            break;

                        case 0x5: // Output
                            _output[r1] = _memory[r2];
                            break;

                        case 0x6: // Phi
                            _memory[ip] = _status ? _memory[r1] : _memory[r2];
                            break;

                        default:
                            Debug.Assert(false, "Illegal D-Type Opcode: " + String.Format("0x{0:x4}", op));
                            break;
                    }
                }
                #endregion
            }

            if (_traceFile != null && _changes.Count > 0)
            {
                Debug.Assert(_output[0x0] == 0.0); // blow up if we change something on the last frame
                Debug.WriteLine("recording " + _changes.Count + " changes (Elapsed = " + Elapsed + ")");
                _traceFile.Write(Elapsed);
                _traceFile.Write(_changes.Count);
                foreach (var change in _changes)
                {
                    _traceFile.Write(change.Address);
                    _traceFile.Write(change.Value);
                }

                _changes.Clear();
            }

            if (_traceFile != null && _output[0x0] != 0.0)
                this.EndTrace();

            Elapsed = Elapsed + 1;
            var args = new VirtualMachineStepArgs(Elapsed, _output[0x0], _output[0x1], _output[0x2], _output[0x3], _output[0x4], _output[0x5]);

            if (OnStep != null)
                OnStep(this, args);

            return args;
        } // Step()
    } // VirtualMachine
} // Namespace icfp09
