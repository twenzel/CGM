using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using codessentials.CGM.Classes;
using codessentials.CGM.Commands;

namespace codessentials.CGM.Import
{
    public class DefaultBinaryReader : IBinaryReader, IDisposable
    {
        private readonly BinaryReader _reader;
        private readonly CGMFile _cgm;
        private int _positionInCurrentArgument;
        private byte[] _arguments;
        private Command _currentCommand;
        private readonly List<Message> _messages = new List<Message>();
        private readonly ICommandFactory _commandFactory;
        private bool _isDisposed;

        internal static readonly double Two_Ex_16 = Math.Pow(2, 16);
        internal static readonly double Two_Ex_32 = Math.Pow(2, 32);

        public int CurrentArg { get; private set; } = 0;
        public byte[] Arguments => _arguments;

        public int ArgumentsCount => _arguments.Length;
        public IEnumerable<Message> Messages => _messages;

        public DefaultBinaryReader(Stream stream, CGMFile cgm, ICommandFactory commandFactory)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            _reader = new BinaryReader(stream);
            _cgm = cgm ?? throw new ArgumentNullException(nameof(cgm));
            _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (disposing)
            {
                _reader.Dispose();
            }

            _isDisposed = true;
        }

        public void ReadCommands()
        {
            while (true)
            {
                var cmd = ReadCommand();

                if (cmd == null)
                    break;

                // get rid of all arguments after we read them
                _arguments = null;
                _cgm.Commands.Add(cmd);
            }
        }

        public int ReadEnum()
        {
            return ReadSignedInt16();
        }

        public string ReadString()
        {
            return ReadString(GetStringCount());
        }

        public int ReadIndex()
        {
            var precision = _cgm.IndexPrecision;
            return ReadInt(precision);
        }

        public int ReadName()
        {
            var precision = _cgm.NamePrecision;
            return ReadInt(precision);
        }

        public int ReadInt()
        {
            var precision = _cgm.IntegerPrecision;
            return ReadInt(precision);
        }

        public string ReadFixedString()
        {
            var length = GetStringCount();
            var c = new char[length];
            for (var i = 0; i < length; i++)
            {
                c[i] = ReadChar();
            }

            return new string(c);
        }

        public string ReadFixedStringWithFallback(int length)
        {
            var textLength = GetStringCount();
            var c = new char[textLength];
            for (var i = 0; i < textLength; i++)
            {
                try
                {
                    c[i] = ReadChar();
                }
                catch (InvalidOperationException)
                {
                    if (length < textLength)
                        return new string(c, 0, length);
                }
            }

            return new string(c);
        }

        public StructuredDataRecord ReadSDR()
        {
            var ret = new StructuredDataRecord();
            var sdrLength = GetStringCount();
            var startPos = CurrentArg;
            while (CurrentArg < (startPos + sdrLength))
            {
                var dataType = (StructuredDataRecord.StructuredDataType)ReadIndex();
                var dataCount = ReadInt();
                var data = new List<object>();
                for (var i = 0; i < dataCount; i++)
                {
                    switch (dataType)
                    {
                        case StructuredDataRecord.StructuredDataType.SDR:
                            data.Add(ReadSDR());
                            break;
                        case StructuredDataRecord.StructuredDataType.CI:
                            data.Add(ReadColorIndex());
                            break;
                        case StructuredDataRecord.StructuredDataType.CD:
                            data.Add(ReadDirectColor());
                            break;
                        case StructuredDataRecord.StructuredDataType.N:
                            data.Add(ReadName());
                            break;
                        case StructuredDataRecord.StructuredDataType.E:
                            data.Add(ReadEnum());
                            break;
                        case StructuredDataRecord.StructuredDataType.I:
                            data.Add(ReadInt());
                            break;
                        case StructuredDataRecord.StructuredDataType.RESERVED:
                            // reserved
                            break;
                        case StructuredDataRecord.StructuredDataType.IF8:
                            data.Add(ReadSignedInt8());
                            break;
                        case StructuredDataRecord.StructuredDataType.IF16:
                            data.Add(ReadSignedInt16());
                            break;
                        case StructuredDataRecord.StructuredDataType.IF32:
                            data.Add(ReadSignedInt32());
                            break;
                        case StructuredDataRecord.StructuredDataType.IX:
                            data.Add(ReadIndex());
                            break;
                        case StructuredDataRecord.StructuredDataType.R:
                            data.Add(ReadReal());
                            break;
                        case StructuredDataRecord.StructuredDataType.S:
                            data.Add(ReadString());
                            break;
                        case StructuredDataRecord.StructuredDataType.SF:
                            data.Add(ReadString());
                            break;
                        case StructuredDataRecord.StructuredDataType.VC:
                            data.Add(ReadVc());
                            break;
                        case StructuredDataRecord.StructuredDataType.VDC:
                            data.Add(ReadVdc());
                            break;
                        case StructuredDataRecord.StructuredDataType.CCO:
                            data.Add(ReadDirectColor());
                            break;
                        case StructuredDataRecord.StructuredDataType.UI8:
                            data.Add(ReadUInt8());
                            break;
                        case StructuredDataRecord.StructuredDataType.UI32:
                            data.Add(ReadUInt32());
                            break;
                        case StructuredDataRecord.StructuredDataType.BS:
                            // bit stream? XXX how do we know how many bits to read?
                            throw new NotImplementedException("ReadSDR- bit stream");
                        case StructuredDataRecord.StructuredDataType.CL:
                            // color list? XXX how to read? -> evtl wie in CellArray
                            throw new NotImplementedException("ReadSDR - color list");
                        case StructuredDataRecord.StructuredDataType.UI16:
                            data.Add(ReadUInt16());
                            break;
                        default:
                            throw new NotSupportedException("ReadSDR()-unsupported dataTypeIndex " + dataType);
                    }
                }
                ret.Add(dataType, dataCount, data);
            }
            return ret;
        }


        protected void EnsureAllArgumentsWereRead()
        {
            Command.Assert(CurrentArg == _arguments.Length || (CurrentArg == 0 && _positionInCurrentArgument > 0), GetErrorMessage());
        }

        public Command ReadEmbeddedCommand()
        {
            var k = ReadUInt16();

            // the element class
            var elementClass = k >> 12;
            var elementId = (k >> 5) & 127;
            var argumentCount = k & 31;

            if (argumentCount == 31)
            {
                // it's a long form command
                argumentCount = ReadUInt(16);

                // note: we don't support partitioned data here
                Command.Assert((argumentCount & (1 << 15)) == 0, "partitioned data is not supported");
            }

            var oldCommand = _currentCommand;
            _currentCommand = _commandFactory.CreateCommand(elementId, elementClass, _cgm);

            // copy all the remaining arguments in an array
            var commandArguments = new byte[argumentCount];
            var c = 0;
            while (c < argumentCount)
                commandArguments[c++] = ReadByte();

            var oldCurrentArg = CurrentArg;
            CurrentArg = 0;
            var oldArguments = _arguments;
            _arguments = commandArguments;

            try
            {
                _currentCommand.ReadFromBinary(this);
            }
            catch (NotSupportedException ex)
            {
                _messages.Add(new Message(Severity.Unsupported, (ClassCode)elementClass, elementId, ex.Message, _currentCommand.ToString()));
            }
            catch (NotImplementedException ex)
            {
                _messages.Add(new Message(Severity.Unimplemented, (ClassCode)elementClass, elementId, ex.Message, _currentCommand.ToString()));
            }
            catch (Exception ex)
            {
                _messages.Add(new Message(Severity.Fatal, (ClassCode)elementClass, elementId, ex.Message, _currentCommand.ToString()));
            }

            var result = _currentCommand;
            _currentCommand = oldCommand;
            _arguments = oldArguments;
            CurrentArg = oldCurrentArg;

            AlignOnWord();

            return result;
        }

        /// <summary>
        /// Reads one command from the given input stream.
        /// </summary>
        /// <param name="reader">The input reader</param>
        /// <param name="messages">The message container</param>
        /// <returns>The command or null if the end of stream was found</returns>
        private Command ReadCommand()
        {

            int k;
            try
            {
                k = _reader.ReadByte();
                k = (k << 8) | _reader.ReadByte();
            }
            catch (EndOfStreamException)
            {
                return null;
            }

            // the element class
            var elementClass = k >> 12;
            var elementId = (k >> 5) & 127;
            var argumentCount = k & 31;

            var oldCommand = _currentCommand;
            var oldArguments = _arguments;
            var oldCurrentArg = CurrentArg;

            _currentCommand = _commandFactory.CreateCommand(elementId, elementClass, _cgm);
            _arguments = null;
            CurrentArg = 0;
            ReadArguments(argumentCount, _reader);
            try
            {
                _currentCommand.ReadFromBinary(this);

            }
            catch (NotSupportedException ex)
            {
                _messages.Add(new Message(Severity.Unsupported, (ClassCode)elementClass, elementId, ex.Message, _currentCommand.ToString()));
            }
            catch (NotImplementedException ex)
            {
                _messages.Add(new Message(Severity.Unimplemented, (ClassCode)elementClass, elementId, ex.Message, _currentCommand.ToString()));
            }
            catch (Exception ex)
            {
                if (ex.Source == "FluentAssertions")
                    throw;

                _messages.Add(new Message(Severity.Fatal, (ClassCode)elementClass, elementId, ex.Message, _currentCommand.ToString()));
            }

            EnsureAllArgumentsWereRead();

            var result = _currentCommand;
            _currentCommand = oldCommand;
            _arguments = oldArguments;
            CurrentArg = oldCurrentArg;

            return result;
        }

        public void Unsupported(string message)
        {
            if (_currentCommand != null)
                _messages.Add(new Message(Severity.Unsupported, _currentCommand.ElementClass, _currentCommand.ElementId, message, _currentCommand.ToString()));
            else
                _messages.Add(new Message(Severity.Unsupported, 0, 0, message, ""));
        }

        private void LogWarning(string message)
        {
            Unsupported(message);
        }


        private string GetErrorMessage([CallerMemberName] string callingMethod = "")
        {
            if (_currentCommand != null)
                return $"Error on {callingMethod} in {_currentCommand.GetType().Name} command for file '{_cgm.Name}'!";
            else
                return $"Error on {callingMethod} command for file '{_cgm.Name}'!";
        }

        private void ReadArguments(int argumentsCount, BinaryReader reader)
        {
            if (argumentsCount != 31)
            {
                _arguments = new byte[argumentsCount];
                for (var i = 0; i < argumentsCount; i++)
                    _arguments[i] = reader.ReadByte();

                if (argumentsCount % 2 == 1)
                {
                    try
                    {
                        reader.ReadByte();
                    }
                    catch (EndOfStreamException)
                    {
                        // we've reached the end of the data input. Since we're only
                        // skipping data here, the exception can be ignored.
                    }
                }
            }
            else
            {
                var a = 0;
                // this is a long form command
                bool done;
                do
                {
                    argumentsCount = ReadInt16Direct(reader);
                    if (argumentsCount == -1)
                        break;

                    if ((argumentsCount & (1 << 15)) != 0)
                    {
                        // data is partitioned and it's not the last partition
                        done = false;
                        // clear bit 15
                        argumentsCount &= ~(1 << 15);
                    }
                    else
                    {
                        done = true;
                    }

                    if (_arguments == null)
                        _arguments = new byte[argumentsCount];

                    else
                    {
                        // resize the args array
                        Array.Resize(ref _arguments, _arguments.Length + argumentsCount);
                    }

                    for (var i = 0; i < argumentsCount; i++)
                        _arguments[a++] = reader.ReadByte();

                    // align on a word if necessary
                    if (argumentsCount % 2 == 1)
                    {
                        reader.ReadByte();
                    }
                }
                while (!done);
            }
        }

        private int ReadInt16Direct(BinaryReader reader)
        {
            return (reader.ReadByte() << 8) | reader.ReadByte();
        }

        protected string ReadString(int length)
        {
            var c = new char[length];
            try
            {
                for (var i = 0; i < length; i++)
                {
                    c[i] = (char)ReadByte();
                }

                return new string(c, 0, length); //"ISO8859-1"
            }
            catch (Exception)
            {
                return new string(c);
            }
        }



        public byte ReadByte()
        {
            SkipBits();
            Command.Assert(CurrentArg < _arguments.Length, GetErrorMessage());
            return _arguments[CurrentArg++];
        }

        protected char ReadChar()
        {
            SkipBits();
            Command.Assert(CurrentArg < _arguments.Length, GetErrorMessage());
            return (char)_arguments[CurrentArg++];
        }

        protected int ReadSignedInt8()
        {
            return ReadByte();
        }

        protected int ReadSignedInt16()
        {
            SkipBits();
            Command.Assert(CurrentArg + 1 < _arguments.Length, GetErrorMessage());
            return ((short)(_arguments[CurrentArg++] << 8) + _arguments[CurrentArg++]);
        }

        protected int ReadSignedInt24()
        {
            SkipBits();
            Command.Assert(CurrentArg + 2 < _arguments.Length, GetErrorMessage());
            return (_arguments[CurrentArg++] << 16) + (_arguments[CurrentArg++] << 8) + _arguments[CurrentArg++];
        }

        protected int ReadSignedInt32()
        {
            SkipBits();
            Command.Assert(CurrentArg + 3 < _arguments.Length, GetErrorMessage());
            return (_arguments[CurrentArg++] << 24) + (_arguments[CurrentArg++] << 16) + (_arguments[CurrentArg++] << 8) + _arguments[CurrentArg++];
        }

        public int SizeOfInt()
        {
            var precision = _cgm.IntegerPrecision;
            return precision / 8;
        }

        private int ReadInt(int precision)
        {
            SkipBits();
            if (precision == 8)
                return ReadSignedInt8();

            if (precision == 16)
                return ReadSignedInt16();

            if (precision == 24)
                return ReadSignedInt24();

            if (precision == 32)
                return ReadSignedInt32();

            LogWarning("unsupported integer precision " + precision);

            // return default
            return ReadSignedInt16();
        }

        public int ReadUInt(int precision)
        {
            if (precision == 1)
                return ReadUInt1();

            if (precision == 2)
                return ReadUInt2();

            if (precision == 4)
                return ReadUInt4();

            if (precision == 8)
                return ReadUInt8();

            if (precision == 16)
                return ReadUInt16();

            if (precision == 24)
                return ReadUInt24();

            if (precision == 32)
                return ReadUInt32();

            LogWarning("unsupported uint precision " + precision);

            // return default
            return ReadUInt8();
        }

        private int ReadUInt32()
        {
            return ReadSignedInt32();
        }

        private int ReadUInt24()
        {
            return ReadSignedInt24();
        }

        private int ReadUInt16()
        {
            SkipBits();

            if (CurrentArg + 1 < _arguments.Length)
            {
                // this is the default, two bytes
                return (_arguments[CurrentArg++] << 8) + _arguments[CurrentArg++];
            }

            // some CGM files request a 16 bit precision integer when there are only 8 bits left
            if (CurrentArg < _arguments.Length)
            {
                return _arguments[CurrentArg++];
            }

            Command.Assert(false, GetErrorMessage());
            return 0;
        }

        private int ReadUInt8()
        {
            return ReadSignedInt8();
        }

        private int ReadUInt4()
        {
            return ReadUIntBit(4);
        }

        private int ReadUInt2()
        {
            return ReadUIntBit(2);
        }

        private int ReadUInt1()
        {
            return ReadUIntBit(1);
        }

        private int ReadUIntBit(int numBits)
        {
            Command.Assert(CurrentArg < _arguments.Length, GetErrorMessage());

            var bitsPosition = 8 - numBits - _positionInCurrentArgument;
            var mask = ((1 << numBits) - 1) << bitsPosition;
            var ret = ((_arguments[CurrentArg] & mask) >> bitsPosition);
            _positionInCurrentArgument += numBits;
            if (_positionInCurrentArgument % 8 == 0)
            {
                // advance to next byte
                _positionInCurrentArgument = 0;
                CurrentArg++;
            }
            return ret;
        }

        public bool ReadBool()
        {
            return ReadEnum() != 0;
        }

        public double ReadVdc()
        {
            if (_cgm.VDCType == VDCType.Type.Real)
            {
                var realPrecision = _cgm.VDCRealPrecision;
                if (realPrecision == Precision.Fixed_32)
                    return ReadFixedPoint32();

                if (realPrecision == Precision.Fixed_64)
                    return ReadFixedPoint64();

                if (realPrecision == Precision.Floating_32)
                    return ReadFloatingPoint32();

                if (realPrecision == Precision.Floating_64)
                    return ReadFloatingPoint64();


                LogWarning("ReadVdc - unsupported precision " + realPrecision);
                return ReadFixedPoint32();
            }

            var precision = _cgm.VDCIntegerPrecision;
            if (precision == 16)
                return ReadSignedInt16();

            if (precision == 24)
                return ReadSignedInt24();

            if (precision == 32)
                return ReadSignedInt32();

            LogWarning("ReadVdc - unsupported precision " + precision);
            return ReadSignedInt16();
        }

        protected int SizeOfVdc()
        {
            if (_cgm.VDCType == VDCType.Type.Integer)
            {
                var precision = _cgm.VDCIntegerPrecision;
                return (precision / 8);
            }

            if (_cgm.VDCType == VDCType.Type.Real)
            {
                var precision = _cgm.VDCRealPrecision;
                if (precision == Precision.Fixed_32)
                    return SizeOfFixedPoint32();

                if (precision == Precision.Fixed_64)
                    return SizeOfFixedPoint64();

                if (precision == Precision.Floating_32)
                    return SizeOfFloatingPoint32();

                if (precision == Precision.Floating_64)
                    return SizeOfFloatingPoint64();

            }
            return 1;
        }

        protected VC ReadVc()
        {
            var result = new VC();

            switch (_cgm.DeviceViewportSpecificationMode)
            {
                case DeviceViewportSpecificationMode.Mode.MM:
                case DeviceViewportSpecificationMode.Mode.PHYDEVCOORD:
                    result.ValueInt = ReadInt();
                    break;
                default:
                    result.ValueReal = ReadReal();
                    break;
            }

            return result;
        }

        public ViewportPoint ReadViewportPoint()
        {
            var result = new ViewportPoint()
            {
                FirstPoint = ReadVc(),
                SecondPoint = ReadVc()
            };
            return result;
        }

        public double ReadReal()
        {
            var precision = _cgm.RealPrecision;
            if (precision == Precision.Fixed_32)
                return ReadFixedPoint32();

            if (precision == Precision.Fixed_64)
                return ReadFixedPoint64();

            if (precision == Precision.Floating_32)
                return ReadFloatingPoint32();

            if (precision == Precision.Floating_64)
                return ReadFloatingPoint64();

            LogWarning("ReadReal unsupported real precision " + precision);
            // return default
            return ReadFixedPoint32();
        }

        protected double ReadFixedPoint()
        {
            var precision = _cgm.RealPrecision;

            if (precision == Precision.Fixed_32)
                return ReadFixedPoint32();

            if (precision == Precision.Fixed_64)
                return ReadFixedPoint64();

            LogWarning("ReadFixedPoint - unsupported real precision " + precision);
            // return default
            return ReadFixedPoint32();
        }

        public double ReadFloatingPoint()
        {
            var precision = _cgm.RealPrecision;

            if (precision == Precision.Floating_32)
                return ReadFloatingPoint32();

            if (precision == Precision.Floating_64)
                return ReadFloatingPoint64();

            return ReadFloatingPoint32();
        }

        private double ReadFixedPoint32()
        {
            var wholePart = ReadSignedInt16();
            var fractionPart = ReadUInt16();

            return wholePart + (fractionPart / Two_Ex_16);
        }

        private int SizeOfFixedPoint32()
        {
            return 2 + 2;
        }

        private double ReadFixedPoint64()
        {
            double wholePart = ReadSignedInt32();
            double fractionPart = ReadUInt32();

            return wholePart + (fractionPart / Two_Ex_32);
        }

        private int SizeOfFixedPoint64()
        {
            return 4 + 4;
        }

        public double ReadFloatingPoint32()
        {
            SkipBits();
            var bits = 0;
            for (var i = 0; i < 4; i++)
            {
                bits = (bits << 8) | ReadChar();
            }

            var bytes = BitConverter.GetBytes(bits);
            var result = BitConverter.ToSingle(bytes, 0);

            if (result == -5.1034731995969196E-12) // quirks mode, this should be zero
                result = 0;

            // first to decimal, because converting from decimal to double remains the correct fractional part
            // when converting from float to double (or cast) the fractional part will be changed
            return System.Convert.ToDouble((decimal)result);
        }

        private int SizeOfFloatingPoint32()
        {
            return 2 * 2;
        }

        private double ReadFloatingPoint64()
        {
            SkipBits();
            long bits = 0;
            for (var i = 0; i < 8; i++)
            {
                bits = (bits << 8) | ReadChar();
            }
            return BitConverter.Int64BitsToDouble(bits);
        }

        private int SizeOfFloatingPoint64()
        {
            return 2 * 4;
        }

        public int SizeOfEnum()
        {
            return 2;
        }

        public CGMPoint ReadPoint()
        {
            return new CGMPoint(ReadVdc(), ReadVdc());
        }

        public int SizeOfPoint()
        {
            return 2 * SizeOfVdc();
        }

        public int ReadColorIndex()
        {
            var precision = _cgm.ColourIndexPrecision;
            return ReadUInt(precision);
        }

        public int ReadColorIndex(int localColorPrecision)
        {
            return ReadUInt(localColorPrecision == -1 ? _cgm.ColourIndexPrecision : localColorPrecision);
        }

        public CGMColor ReadColor()
        {
            return ReadColor(-1);
        }

        public CGMColor ReadColor(int localColorPrecision)
        {
            var result = new CGMColor();

            if (_cgm.ColourSelectionMode == ColourSelectionMode.Type.DIRECT)
                result.Color = ReadDirectColor();
            else if (_cgm.ColourSelectionMode == ColourSelectionMode.Type.INDEXED)
                result.ColorIndex = ReadColorIndex(localColorPrecision);

            return result;
        }

        private void SkipBits()
        {
            if (_positionInCurrentArgument % 8 != 0)
            {
                // we read some bits from the current arg but aren't done, skip the rest
                _positionInCurrentArgument = 0;
                CurrentArg++;
            }
        }

        private int GetStringCount()
        {
            var length = ReadUInt8();
            if (length == 255)
            {
                length = ReadUInt16();
                if ((length & (1 << 16)) != 0)
                {
                    length = (length << 16) | ReadUInt16();
                }
            }
            return length;
        }

        public Color ReadDirectColor()
        {
            var precision = _cgm.ColourPrecision;
            var model = _cgm.ColourModel;

            if (model == ColourModel.Model.RGB)
            {
                var scaled = ScaleColorValueRGB(ReadUInt(precision), ReadUInt(precision), ReadUInt(precision));
                return Color.FromArgb(scaled[0], scaled[1], scaled[2]);
            }

            if (model == ColourModel.Model.CIELAB)
            {
                LogWarning("ReadDirectColor - unsupported CIELAB");
                ReadUInt(precision);
                ReadUInt(precision);
                ReadUInt(precision);
                return Color.Cyan;
            }

            if (model == ColourModel.Model.CIELUV)
            {
                LogWarning("ReadDirectColor - unsupported CIELUV");
                ReadUInt(precision);
                ReadUInt(precision);
                ReadUInt(precision);
                return Color.Cyan;
            }

            if (model == ColourModel.Model.CMYK)
            {
                var components = new float[4];
                components[0] = ReadUInt(precision);
                components[1] = ReadUInt(precision);
                components[2] = ReadUInt(precision);
                components[3] = ReadUInt(precision);
                LogWarning("ReadDirectColor- unsupported CMYK SUPPORT");
                //TODO: CMYK SUPPORT
                //CMYKColorSpace colorSpace = new CMYKColorSpace();
                //return new Color(colorSpace, components, 1.0);

                var r = (int)(255 * (1 - components[0]) * (1 - components[3]));
                var g = (int)(255 * (1 - components[1]) * (1 - components[3]));
                var b = (int)(255 * (1 - components[2]) * (1 - components[3]));
                return Color.FromArgb(r, g, b);
            }

            if (model == ColourModel.Model.RGB_RELATED)
            {
                LogWarning("ReadDirectColor- unsupported RGB_RELATED");
                ReadUInt(precision);
                ReadUInt(precision);
                ReadUInt(precision);
                return Color.Cyan;
            }

            LogWarning($"unsupported color mode {model}");
            return Color.Cyan;
        }


        public int SizeOfDirectColor()
        {
            var precision = _cgm.ColourPrecision;
            var model = _cgm.ColourModel;

            if (model == ColourModel.Model.RGB)
            {
                return 3 * precision / 8;
            }

            LogWarning(GetErrorMessage());
            return 0;
        }

        //final protected int sizeOfDirectColor()
        //{
        //    int precision = ColourPrecision.getPrecision();
        //    Model model = ColourModel.getModel();

        //    if (model.equals(Model.RGB))
        //    {
        //        return 3 * precision / 8;
        //    }

        //    assert false;
        //    return 0;
        //}

        private int[] ScaleColorValueRGB(int r, int g, int b)
        {
            var min = _cgm.ColourValueExtentMinimumColorValueRGB;
            var max = _cgm.ColourValueExtentMaximumColorValueRGB;

            r = Clamp(r, min[0], max[0]);
            g = Clamp(g, min[1], max[1]);
            b = Clamp(b, min[2], max[2]);

            Command.Assert(min[0] != max[0] && min[1] != max[1] && min[2] != max[2], GetErrorMessage());

            return new int[] {
                Scale(r, min[0], max[0]),
                Scale(g, min[1], max[1]),
                Scale(b, min[2], max[2])
            };
        }

        private int Scale(int r, int min, int max)
        {
            return 255 * (r - min) / (max - min);
        }

        /// <summary>
        /// Clamp the given value between the given minimum and maximum
        /// </summary>
        /// <param name="r"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private int Clamp(int r, int min, int max)
        {
            return Math.Max(Math.Min(r, max), min);
        }

        /*Table 11 — Specification mode contolling SS resolution for each affected aspect
Specification Mode                  Affected Parameters 
								Element                         Parameter
LINE WIDTH SPECIFICATION MODE   LINE REPRESENTATION             line width specifier 
								LINE AND EDGE TYPE DEFINITION   dash cycle repeat length
								LINE WIDTH                      line width specifier 
								
MARKER SIZE SPECIFICATION MODE  MARKER REPRESENTATION           marker size specifier
								MARKER SIZE                     marker size specifier
								
EDGE WIDTH SPECIFICATION MODE   EDGE REPRESENTATION             edge width specifier
								EDGE WIDTH                      edge width specifier
			
INTERIOR STYLE SPECIFICATION    HATCH STYLE DEFINITION          hatch direction vectors specifier 
MODE                            HATCH STYLE DEFINITION          duty cycle length                                 
								PATTERN SIZE                    pattern size specifier
								INTERPOLATED INTERIOR           reference geometry
*/

        public double ReadSizeSpecification(SpecificationMode edgeWidthSpecificationMode)
        {
            if (edgeWidthSpecificationMode == SpecificationMode.ABS)
                return ReadVdc();

            return ReadReal();
        }

        protected int ReadLocalColorPrecicion()
        {
            var localColorPrecision = ReadInt();

            if (localColorPrecision == 0)
            {
                if (_cgm.ColourSelectionMode == ColourSelectionMode.Type.INDEXED)
                    localColorPrecision = _cgm.ColourIndexPrecision;
                else
                    localColorPrecision = _cgm.ColourPrecision;
            }

            return localColorPrecision;
        }

        /**
	 * Align on a word boundary
	 */
        public void AlignOnWord()
        {
            if (CurrentArg >= _arguments.Length)
            {
                // we reached the end of the array, nothing to skip
                return;
            }

            if (CurrentArg % 2 == 0 && _positionInCurrentArgument > 0)
            {
                _positionInCurrentArgument = 0;
                CurrentArg += 2;
            }
            else if (CurrentArg % 2 == 1)
            {
                _positionInCurrentArgument = 0;
                CurrentArg++;
            }
        }

    }
}
