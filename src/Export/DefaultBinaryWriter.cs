using codessentials.CGM.Classes;
using codessentials.CGM.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Export
{
    public class DefaultBinaryWriter : IBinaryWriter, IDisposable
    {
        private Stream _stream;
        private WriterBucket _bucket = new WriterBucket();
        private CGMFile _cgm;
        private List<Message> _messages = new List<Message>();
        private Command _currentCommand;
        int _positionInCurrentArgument = 0;

        private static double Two_Ex_16 = Math.Pow(2, 16);
        private static double Two_Ex_32 = Math.Pow(2, 32);

        public IEnumerable<Message> Messages => _messages;

        public DefaultBinaryWriter(Stream stream, CGMFile cgm)
        {
            _stream = stream;
            _cgm = cgm;
        }

        public void Dispose()
        {
            _stream = null;
        }

        /// <summary>
        /// Writes the parameter length and the value
        /// </summary>
        /// <param name="data">The string value</param>
        public void WriteString(string data)
        {
            if (data.Length > 0)
            {
                // A string is encoded as a count(unsigned integer) followed by characters

                // The count is a count of octets in the string, not whole character codes
                WriteDataLength(data.Length);

                // value
                _bucket.WriteString(data);
            }
            else
                _bucket.Add(0);
        }

        private void WriteDataLength(int length)
        {
            if (length >= 255)
            {
                WriteUInt8(255);
                while (length > 0)
                {
                    if (length > 32767)
                    {
                        WriteUInt16(32767);
                        length -= 32767;

                        WriteUInt16(UInt16.MaxValue);
                    }
                    else
                    {
                        WriteUInt16(length);
                        length = 0;
                    }
                }
            }
            else
                WriteUInt8(length);
        }

        public void WriteFixedString(string data)
        {
            WriteString(data);
        }

        public void WriteInt(int data)
        {
            WriteInt(data, _cgm.IntegerPrecision);
        }

        public void WriteEnum(int data)
        {
            WriteSignedInt16(data);
        }

        public void WriteBool(bool data)
        {
            WriteEnum(data ? 1 : 0);
        }

        public void WriteIndex(int data)
        {
            WriteInt(data, _cgm.IndexPrecision);
        }

        public void WriteName(int data)
        {
            WriteInt(data, _cgm.NamePrecision);
        }

        public void WriteByte(byte data)
        {
            _bucket.Add(data);
        }

        public void WriteColor(CGMColor color, int localColorPrecision = -1)
        {
            if (_cgm.ColourSelectionMode == ColourSelectionMode.Type.DIRECT)
                WriteDirectColor(color.Color);
            else if (_cgm.ColourSelectionMode == ColourSelectionMode.Type.INDEXED)
                WriteColorIndex(color.ColorIndex, localColorPrecision);
        }

        public void WritePoint(CGMPoint point)
        {
            WriteVdc(point.X);
            WriteVdc(point.Y);
        }

        public void WriteReal(double data)
        {
            Precision precision = _cgm.RealPrecision;
            if (precision == Precision.Fixed_32)
                WriteFixedPoint32(data);
            else if (precision == Precision.Fixed_64)
                WriteFixedPoint64(data);
            else if (precision == Precision.Floating_32)
                WriteFloatingPoint32(data);
            else if (precision == Precision.Floating_64)
                WriteFloatingPoint64(data);
            else
                throw new NotSupportedException($"unsupported RealPrecision {precision}!");
        }

        public void WriteFloatingPoint(double data)
        {
            if (_cgm.RealPrecision == Precision.Floating_64)
                WriteFloatingPoint64(data);
            else
                WriteFloatingPoint32(data);
        }

        public void WriteSDR(StructuredDataRecord data)
        {
            var oldBucket = _bucket;
            _bucket = new WriterBucket();

            var argumentsCount = data.Members.Count;

            foreach (var record in data.Members)
            {
                argumentsCount += 2 + record.Data.Count;
            }


            //WriteDataLength(argumentsCount);

            foreach (var record in data.Members)
            {
                WriteIndex((int)record.Type);
                WriteInt(record.Count);
                foreach (var val in record.Data)
                {
                    switch (record.Type)
                    {
                        case StructuredDataRecord.StructuredDataType.SDR:
                            WriteSDR((StructuredDataRecord)val);
                            break;
                        case StructuredDataRecord.StructuredDataType.CI:
                            WriteColorIndex((int)val, _cgm.ColourIndexPrecision);
                            break;
                        case StructuredDataRecord.StructuredDataType.CD:
                            WriteDirectColor((System.Drawing.Color)val);
                            break;
                        case StructuredDataRecord.StructuredDataType.N:
                            WriteName((int)val);
                            break;
                        case StructuredDataRecord.StructuredDataType.E:
                            WriteEnum((int)val);
                            break;
                        case StructuredDataRecord.StructuredDataType.I:
                            WriteInt((int)val);
                            break;
                        case StructuredDataRecord.StructuredDataType.RESERVED:
                            // reserved
                            break;
                        case StructuredDataRecord.StructuredDataType.IF8:
                            WriteSignedInt8((int)val);
                            break;
                        case StructuredDataRecord.StructuredDataType.IF16:
                            WriteSignedInt16((int)val);
                            break;
                        case StructuredDataRecord.StructuredDataType.IF32:
                            WriteSignedInt32((int)val);
                            break;
                        case StructuredDataRecord.StructuredDataType.IX:
                            WriteIndex((int)val);
                            break;
                        case StructuredDataRecord.StructuredDataType.R:
                            WriteReal((double)val);
                            break;
                        case StructuredDataRecord.StructuredDataType.S:
                            WriteString((string)val);
                            break;
                        case StructuredDataRecord.StructuredDataType.SF:
                            WriteFixedString((string)val);
                            break;
                        case StructuredDataRecord.StructuredDataType.VC:
                            WriteVc((VC)val);
                            break;
                        case StructuredDataRecord.StructuredDataType.VDC:
                            WriteVdc((double)val);
                            break;
                        case StructuredDataRecord.StructuredDataType.CCO:
                            WriteDirectColor((System.Drawing.Color)val);
                            break;
                        case StructuredDataRecord.StructuredDataType.UI8:
                            WriteUInt8((int)val);
                            break;
                        case StructuredDataRecord.StructuredDataType.UI32:
                            WriteUInt32((int)val);
                            break;
                        case StructuredDataRecord.StructuredDataType.BS:
                            _bucket.AddRange((byte[])val);
                            break;
                        case StructuredDataRecord.StructuredDataType.CL:
                            // color list? XXX how to read?  -> evtl wie in CellArray
                            throw new NotImplementedException("Write SDR color list");
                        case StructuredDataRecord.StructuredDataType.UI16:
                            WriteUInt16((int)val);
                            break;
                        default:
                            throw new NotSupportedException($"Data type {record.Type} is not supported by WriteSDR");
                    }
                }
            }

            var sdrBucket = _bucket;
            _bucket = oldBucket;

            WriteDataLength(sdrBucket.Count);
            _bucket.AddRange(sdrBucket);
        }

        public void FillToWord()
        {
            if (_bucket.Count % 2 == 0 && _positionInCurrentArgument > 0)
            {
                _bucket.Add(0);
                _bucket.Add(0);
                _positionInCurrentArgument = 0;
            }
            else if (_bucket.Count % 2 == 1)
            {
                _bucket.Add(0);
                _positionInCurrentArgument = 0;
            }
        }

        public void WriteViewportPoint(ViewportPoint data)
        {
            WriteVc(data.FirstPoint);
            WriteVc(data.SecondPoint);
        }

        public void WriteSizeSpecification(double data, SpecificationMode specificationMode)
        {
            if (specificationMode == SpecificationMode.ABS)
                WriteVdc(data);
            else
                WriteReal(data);
        }

        public void WriteCommand(Commands.Command command)
        {
            var oldCommand = _currentCommand;
            _currentCommand = command;

            // write to bucket
            command.WriteAsBinary(this);

            WriteHeader(command);

            if (_bucket.Count % 2 == 1)
                _bucket.Add(0);

            _bucket.SaveToStream(_stream);
            _bucket.Clear();

            _currentCommand = oldCommand;
        }

        public void Unsupported(string message)
        {
            if (_currentCommand != null)
                _messages.Add(new Message(Severity.Unsupported, _currentCommand.ElementClass, _currentCommand.ElementId, message, _currentCommand.ToString()));
            else
                _messages.Add(new Message(Severity.Unsupported, 0, 0, message, ""));
        }

        public void WriteEmbeddedCommand(Commands.Command command)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new DefaultBinaryWriter(stream, _cgm))
                {
                    writer.WriteCommand(command);
                }
                _bucket.AddRange(stream.ToArray());
            }
        }

        #region internal write helper

        private void WriteInt(int data, int precision)
        {
            if (precision == 8)
                WriteSignedInt8(data);
            else if (precision == 16)
                WriteSignedInt16(data);
            else if (precision == 24)
                WriteSignedInt24(data);
            else if (precision == 32)
                WriteSignedInt32(data);
            else
                throw new NotSupportedException($"Precision of {precision} is not supported!");
        }

        private void WriteSignedInt8(int data)
        {
            _positionInCurrentArgument = 0;
            _bucket.Add((byte)data);
        }

        private void WriteSignedInt16(int data)
        {
            _positionInCurrentArgument = 0;

            _bucket.Add((byte)(data >> 8 & 0xff));
            _bucket.Add((byte)(data & 0xff));
        }

        private void WriteSignedInt16(int data, int index)
        {
            _positionInCurrentArgument = 0;

            _bucket.Insert(index, (byte)(data >> 8 & 0xff));
            _bucket.Insert(index + 1, (byte)(data & 0xff));
        }

        private void WriteSignedInt24(int data)
        {
            _positionInCurrentArgument = 0;
            _bucket.Add((byte)(data >> 16 & 0xff));
            _bucket.Add((byte)(data >> 8 & 0xff));
            _bucket.Add((byte)(data & 0xff));
        }

        private void WriteSignedInt32(int data)
        {
            _positionInCurrentArgument = 0;

            _bucket.Add((byte)(data >> 24 & 0xff));
            _bucket.Add((byte)(data >> 16 & 0xff));
            _bucket.Add((byte)(data >> 8 & 0xff));
            _bucket.Add((byte)(data & 0xff));
        }

        public void WriteUInt(int data, int precision)
        {
            if (precision == 1)
                WriteUInt1(data);
            else if (precision == 2)
                WriteUInt2(data);
            else if (precision == 4)
                WriteUInt4(data);
            else if (precision == 8)
                WriteUInt8(data);
            else if (precision == 16)
                WriteUInt16(data);
            else if (precision == 24)
                WriteUInt24(data);
            else if (precision == 32)
                WriteUInt32(data);
            else
                throw new NotSupportedException($"UInt Precision {precision} not supported!");
        }

        private void WriteUInt32(int data)
        {
            WriteSignedInt32(data);
        }

        private void WriteUInt24(int data)
        {
            WriteSignedInt24(data);
        }

        private void WriteUInt16(int data)
        {
            WriteSignedInt16(data);
        }

        private void WriteUInt8(int data)
        {
            WriteSignedInt8(data);
        }

        private void WriteUInt4(int data)
        {
            WriteUIntBit(data, 4);
        }

        private void WriteUInt2(int data)
        {
            WriteUIntBit(data, 2);
        }

        private void WriteUInt1(int data)
        {
            WriteUIntBit(data, 1);
        }

        private void WriteUIntBit(int data, int numBits)
        {
            if (_bucket.Count == 0 || _positionInCurrentArgument == 0)
            {
                _bucket.Add(0);
                _positionInCurrentArgument = 0;
            }

            int bitsPosition = 8 - numBits - _positionInCurrentArgument;
            int mask = ((1 << numBits) - 1) << bitsPosition;
            byte currentVal = _bucket[_bucket.Count - 1];

            _bucket[_bucket.Count - 1] = (byte)(currentVal | (byte)(data << bitsPosition));

            _positionInCurrentArgument += numBits;
            if (_positionInCurrentArgument % 8 == 0)
            {
                // advance to next byte
                _positionInCurrentArgument = 0;
            }
        }

        public static byte SetBit(byte b, int BitNumber)
        {
            //Kleine Fehlerbehandlung
            if (BitNumber < 8 && BitNumber > -1)
            {
                return (byte)(b | (byte)(0x01 << BitNumber));
            }
            else
            {
                throw new InvalidOperationException(
                "Der Wert für BitNumber " + BitNumber.ToString() + " war nicht im zulässigen Bereich! (BitNumber = (min)0 - (max)7)");
            }
        }

        public static int CheckBitSet(byte b, int BitNumber)
        {
            //Kleine Fehlerbehandlung
            if (BitNumber < 8 && BitNumber > -1)
            {
                return (b & (1 << BitNumber));
            }
            else
            {
                throw new InvalidOperationException(
                "Der Wert für BitNumber " + BitNumber.ToString() + " war nicht im zulässigen Bereich! (BitNumber = (min)0 - (max)7)");
            }

        }

        public void WriteHeader(Commands.Command command)
        {
            // the element class
            uint elementClass = (uint)command.ElementClass << 12;
            uint elementId = (uint)command.ElementId << 5;
            bool isLongForm = _bucket.Count >= 31; // more than 31
            uint argumentCount = isLongForm ? 31 : (uint)_bucket.Count;

            int cmd = (int)(elementClass | elementId | argumentCount);

            WriteInt16Direct(cmd);

            if (isLongForm)
            {
                var c = _bucket.Count;
                int i = 0;
                if (c > 32032)
                {
                    WriteInt16Direct(32032 | (1 << 15)); // inclusive partition flag
                    c -= 32032;
                    i += 32032;

                    while (c > 32000)
                    {
                        WriteSignedInt16(32000 | (1 << 15), i); // inclusive partition flag
                        c -= 32000;
                        i += 32000 + 2; // 2 for the bytes written by WriteSignedInt16 above
                    }

                    WriteSignedInt16(c, i);
                }
                else
                    WriteInt16Direct(c);
            }
        }

        private void WriteInt16Direct(int value)
        {
            _stream.WriteByte((byte)(value >> 8 & 0xff));
            _stream.WriteByte((byte)(value & 0xff));
        }

        public void WriteDirectColor(System.Drawing.Color color)
        {
            int precision = _cgm.ColourPrecision;
            var model = _cgm.ColourModel;

            if (model == ColourModel.Model.RGB)
            {
                int[] scaled = scaleColorValueRGB(color.R, color.G, color.B);
                WriteUInt(scaled[0], precision);
                WriteUInt(scaled[1], precision);
                WriteUInt(scaled[2], precision);
            }

            if (model == ColourModel.Model.CIELAB)
            {
                throw new NotImplementedException("WriteDirectColor CIELAB");
            }

            if (model == ColourModel.Model.CIELUV)
            {
                throw new NotImplementedException("WriteDirectColor CIELUV");
            }

            if (model == ColourModel.Model.CMYK)
            {
                //float[] components = new float[4];
                //components[0] = ReadUInt(precision);
                //components[1] = ReadUInt(precision);
                //components[2] = ReadUInt(precision);
                //components[3] = ReadUInt(precision);
                //Unimplemented("CMYK SUPPORT");
                ////TODO: CMYK SUPPORT
                ////CMYKColorSpace colorSpace = new CMYKColorSpace();
                ////return new Color(colorSpace, components, 1.0);

                //var r = (int)(255 * (1 - components[0]) * (1 - components[3]));
                //var g = (int)(255 * (1 - components[1]) * (1 - components[3]));
                //var b = (int)(255 * (1 - components[2]) * (1 - components[3]));
                //return Color.FromArgb(r, g, b);
                throw new NotImplementedException("WriteDirectColor CMYK");
            }

            if (model == ColourModel.Model.RGB_RELATED)
            {
                throw new NotImplementedException("WriteDirectColor RGB_RELATED");
            }
        }

        public void WriteColorIndex(int index)
        {
            WriteColorIndex(index, _cgm.ColourIndexPrecision);
        }

        public void WriteColorIndex(int index, int precision)
        {
            WriteUInt(index, precision <= 0 ? _cgm.ColourIndexPrecision : precision);
        }

        private int[] scaleColorValueRGB(int r, int g, int b)
        {
            int[] min = _cgm.ColourValueExtentMinimumColorValueRGB;
            int[] max = _cgm.ColourValueExtentMaximumColorValueRGB;

            r = clamp(r, min[0], max[0]);
            g = clamp(g, min[1], max[1]);
            b = clamp(b, min[2], max[2]);

            return new int[] {
                scale(r, min[0], max[0]),
                scale(g, min[1], max[1]),
                scale(b, min[2], max[2])
            };
        }

        private int scale(int r, int min, int max)
        {
            // return 255 * (r - min) / (max - min);

           return (max + min) * (r + min) / 255;
        }

        private int clamp(int r, int min, int max)
        {
            return Math.Max(Math.Min(r, max), min);
        }

        private void WriteVc(VC data)
        {
            switch (_cgm.DeviceViewportSpecificationMode)
            {
                case DeviceViewportSpecificationMode.Mode.MM:
                case DeviceViewportSpecificationMode.Mode.PHYDEVCOORD:
                    WriteInt(data.ValueInt);
                    break;
                case DeviceViewportSpecificationMode.Mode.FRACTION:
                default:
                    WriteReal(data.ValueReal);
                    break;
            }
        }

        public void WriteVdc(double value)
        {
            if (_cgm.VDCType == VDCType.Type.Real)
            {
                var realPrecision = _cgm.VDCRealPrecision;
                if (realPrecision == Precision.Fixed_32)
                    WriteFixedPoint32(value);
                else if (realPrecision == Precision.Fixed_64)
                    WriteFixedPoint64(value);
                else if (realPrecision == Precision.Floating_32)
                    WriteFloatingPoint32(value);
                else if (realPrecision == Precision.Floating_64)
                    WriteFloatingPoint64(value);
                else
                    throw new NotSupportedException($"Unsupported VDCRealPrecision {realPrecision}!");
            }
            else
            {
                int precision = _cgm.VDCIntegerPrecision;
                if (precision == 16)
                    WriteSignedInt16((int)value);
                else if (precision == 24)
                    WriteSignedInt24((int)value);
                else if (precision == 32)
                    WriteSignedInt32((int)value);
                else
                    throw new NotSupportedException($"Unsupported VDCIntegerPrecision {precision}!");
            }

        }

        private void WriteFixedPoint32(double value)
        {
            var val = value.GetWholePart();
            var fraction = value.GetFractionalPart();            
            if (value < 0 && fraction != 0)
                val -= 1;

            WriteSignedInt16(val);
            WriteUInt16((int)(fraction * Two_Ex_16));
        }

        private void WriteFixedPoint64(double value)
        {
            var val = value.GetWholePart();
            var fraction = value.GetFractionalPart();
            if (value < 0 && fraction != 0)
                val -= 1;

            WriteSignedInt32(val);
            WriteUInt32((int)(fraction * Two_Ex_32));
        }

        public void WriteFloatingPoint32(double value)
        {
            var bytes = BitConverter.GetBytes((Single)value);
            _bucket.AddRange(bytes.Reverse());
        }

        private void WriteFloatingPoint64(double value)
        {
            var lng = BitConverter.DoubleToInt64Bits(value);
            var bytes = BitConverter.GetBytes(lng);
            _bucket.AddRange(bytes.Reverse());
        }
        #endregion
    }
}
