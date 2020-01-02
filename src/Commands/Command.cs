using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Base class for all commands
    /// </summary>
    public abstract class Command
    {
        protected ClassCode _elementClass;
        protected int _elementId;
        protected CgmFile _container;

        private static readonly string ZERO_DOUBLE = WriteDouble(0d);

        public ClassCode ElementClass
        {
            get { return _elementClass; }
        }

        public int ElementId
        {
            get { return _elementId; }
        }

        protected Command(CommandConstructorArguments arguments)
        {
            _elementClass = arguments.ElementClass;
            _elementId = arguments.ElementId;
            _container = arguments.Container;
        }

        /// <summary>
        /// Reads the binary data from the reader
        /// </summary>
        /// <param name="reader"></param>
        public abstract void ReadFromBinary(IBinaryReader reader);

        /// <summary>
        /// Writes/exports the command as clear text mode
        /// </summary>
        /// <param name="writer">The text writer to write the clear text to.</param>
        public abstract void WriteAsClearText(IClearTextWriter writer);

        /// <summary>
        /// Writes/exports the command as binary mode
        /// </summary>
        /// <param name="writer">The writer to write the binary content to.</param>
        public abstract void WriteAsBinary(IBinaryWriter writer);

        public override string ToString()
        {
            return GetType().Name;
        }

        /// <summary>
        /// Returns the double value as string with 4 digits
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <returns></returns>
        protected static string WriteDouble(double value)
        {
            return value.ToString("f4", CultureInfo.InvariantCulture);
        }

        protected string WriteReal(double value)
        {
            return WriteDouble(value);
        }

        protected string WriteVDC(double value)
        {
            if (_container.VDCType == VdcType.Type.Real)
                return WriteDouble(value);
            else
                return WriteInt(Convert.ToInt32(value));
        }

        protected string WriteVC(VC value)
        {
            switch (_container.DeviceViewportSpecificationMode)
            {
                case DeviceViewportSpecificationMode.Mode.MM:
                case DeviceViewportSpecificationMode.Mode.PHYDEVCOORD:
                    return WriteInt(value.ValueInt);
                case DeviceViewportSpecificationMode.Mode.FRACTION:
                default:
                    return WriteReal(value.ValueReal);
            }
        }

        protected string WriteViewportPoint(ViewportPoint value)
        {
            return WriteVC(value.FirstPoint) + " " + WriteVC(value.SecondPoint);
        }

        protected string WritePoint(CgmPoint value)
        {
            return WritePoint(value.X, value.Y);
        }

        protected string WritePoint(double x, double y)
        {
            var signCharY = "";

            if (WriteDouble(y) == ZERO_DOUBLE && x < 0)
                signCharY = "-";

            return $"({WriteDouble(x)},{signCharY}{WriteDouble(y)})";
        }

        protected string WriteBool(bool value)
        {
            return value ? "on" : "off";
        }

        protected string WriteBoolYesNo(bool value)
        {
            return value ? "yes" : "no";
        }

        protected string WriteString(string value)
        {
            // remove non-printable elements
            value = new string(value.Where(c => !char.IsControl(c) || c == 13 || c == 10 || c == 9).ToArray());

            return $"'{value}'";
        }

        protected string WriteEnum(object value)
        {
            return $"{value.ToString().ToLower()}";
        }

        protected string WriteName(int value)
        {
            return $"{value}";
        }

        protected string WriteIndex(int value)
        {
            return $"{value}";
        }

        protected string WriteInt(int value)
        {
            return $"{value}";
        }

        protected string WriteColor(Color color, ColourModel.Model model)
        {
            return model switch
            {
                ColourModel.Model.RGB => $"{color.R} {color.G} {color.B}",
                //case ColourModel.Model.CIELAB:
                //    break;
                //case ColourModel.Model.CIELUV:
                //    break;
                //case ColourModel.Model.CMYK:
                //    break;
                //case ColourModel.Model.RGB_RELATED:
                //    break;
                _ => throw new NotImplementedException($"Writing color for {model} is not implemented"),
            };
        }

        protected string WriteColor(CgmColor value)
        {
            if (_container.ColourSelectionMode == ColourSelectionMode.Type.INDEXED)
                return WriteIndex(value.ColorIndex);
            else
                return WriteColor(value.Color, _container.ColourModel);
        }

        protected string WriteObject(object value, StructuredDataRecord.StructuredDataType type)
        {
            switch (type)
            {
                case StructuredDataRecord.StructuredDataType.N:
                    return WriteName(Convert.ToInt32(value));
                case StructuredDataRecord.StructuredDataType.E:
                    return WriteEnum(value);
                case StructuredDataRecord.StructuredDataType.I:
                    return WriteInt(Convert.ToInt32(value));
                case StructuredDataRecord.StructuredDataType.IF8:
                case StructuredDataRecord.StructuredDataType.IF16:
                case StructuredDataRecord.StructuredDataType.IF32:
                case StructuredDataRecord.StructuredDataType.IX:
                    return WriteIndex(Convert.ToInt32(value));
                case StructuredDataRecord.StructuredDataType.R:
                    return WriteReal(Convert.ToDouble(value));
                case StructuredDataRecord.StructuredDataType.S:
                case StructuredDataRecord.StructuredDataType.SF:
                    return WriteString(value.ToString());
                case StructuredDataRecord.StructuredDataType.VC:
                    return WriteVC((VC)value);
                case StructuredDataRecord.StructuredDataType.VDC:
                    return WriteVDC(Convert.ToDouble(value));
                case StructuredDataRecord.StructuredDataType.UI8:
                case StructuredDataRecord.StructuredDataType.UI16:
                case StructuredDataRecord.StructuredDataType.UI32:
                    return WriteInt(Convert.ToInt32(value));
                case StructuredDataRecord.StructuredDataType.BS:
                    return WriteBitStream((byte[])value);
                case StructuredDataRecord.StructuredDataType.SDR:
                case StructuredDataRecord.StructuredDataType.CI:
                    return WriteIndex(Convert.ToInt32(value));
                case StructuredDataRecord.StructuredDataType.CD:
                    return WriteColor((Color)(value), _container.ColourModel);
                case StructuredDataRecord.StructuredDataType.CCO:
                case StructuredDataRecord.StructuredDataType.CL:
                case StructuredDataRecord.StructuredDataType.RESERVED:
                default:
                    throw new NotSupportedException($"Can't write value ({value}) of type {type}!");
            }

            //switch (Type.GetTypeCode(value.GetType()))
            //{
            //    case TypeCode.Empty:
            //        return "";
            //    case TypeCode.Boolean:
            //        return WriteBool(Convert.ToBoolean(value));
            //    case TypeCode.Char:
            //    case TypeCode.SByte:
            //    case TypeCode.Byte:
            //    case TypeCode.Int16:
            //    case TypeCode.UInt16:
            //    case TypeCode.Int32:
            //    case TypeCode.UInt32:
            //    case TypeCode.Int64:
            //    case TypeCode.UInt64:
            //        return WriteInt(Convert.ToInt32(value));
            //    case TypeCode.Single:
            //    case TypeCode.Double:
            //    case TypeCode.Decimal:
            //        return WriteReal(Convert.ToDouble(value));                   
            //    case TypeCode.String:
            //        return WriteString(value.ToString());
            //    default:
            //        throw new NotSupportedException($"Can't write value ({value}) of type {value.GetType()}!");
            //}
        }

        protected string WriteBitStream(byte[] value)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < value.Length; i++)
            {
                var length = i + 4 > value.Length ? value.Length - i : 4;

                if (sb.Length > 0)
                    sb.Append(" ");

                sb.Append(BitConverter.ToString(value, i, length).Replace("-", ""));
                i += 3;
            }

            return sb.ToString();
        }

        protected void WriteSDR(IClearTextWriter writer, StructuredDataRecord record)
        {
            foreach (var member in record.Members)
            {
                writer.Write($" {WriteInt((int)member.Type)} {WriteInt(member.Count)}");

                foreach (var val in member.Data)
                    writer.Write($" {WriteObject(val, member.Type)}");
            }
        }

        public static void Assert(bool isCorrect, string errorMessage)
        {
            if (!isCorrect)
                throw new InvalidOperationException(errorMessage);
        }
    }
}
