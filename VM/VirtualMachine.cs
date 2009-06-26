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
        // memory
        private Dictionary<int, double> _r;

        // input registers
        private Dictionary<int, double> _in;

        // output registers
        private Dictionary<int, double> _out;

    }

    public class VmProgram
    {
        // vector of instructions
        private readonly int[] _instructions = new int[2 ^ 14];

        // initial memory
        private readonly double[] _data = new double[2 ^ 14];

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

    public class VirtualMachine
    {
        private VmProgram _program;
        private double[] _memory = new double[2^14];
        private double[] _input = new double[2 ^ 14];
        private double[] _output = new double[2 ^ 14];

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
            program.Data.CopyTo(_memory, 0);
        }

        public void Step()
        {
            bool status = false;

            for(int ip = 0; ip < _program.Length; ip++)
            {
                int frame = _program.Instructions[ip];
                if((frame & (0xFF000000)) == 0) // S-Type
                {
                    int op = frame >> 24;
                    int imm = (frame & 0x0000FFFF) >> 14;
                    int r1 = frame & 0x3FFF;

                    switch(op)
                    {
                        case 0x0: // noop
                            break;

                        case 0x1: // Cmpz
                            switch(imm)
                            {
                                case 0x0: // <
                                    status = _memory[r1] < 0.0;
                                    break;
                                case 0x1: // <=
                                    status = _memory[r1] <= 0.0;
                                    break;
                                case 0x2: // ==
                                    status = _memory[r1] == 0.0;
                                    break;
                                case 0x3: // >=
                                    status = _memory[r1] >= 0.0;
                                    break;
                                case 0x4: // >
                                    status = _memory[r1] > 0.0;
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
                            _memory[ip] = status ? _memory[r1] : _memory[r2];
                            break;

                        default:
                            Debug.Assert(false, "Illegal D-Type Opcode: " + String.Format("0x{0:x4}", op));
                            break;
                    }
                }
            }
        }
    }
}
