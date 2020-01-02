using codessentials.CGM.Classes;

namespace codessentials.CGM
{
    /// <summary>
    /// Writer interface to write binary values
    /// </summary>
    public interface IBinaryWriter
    {
        void WriteString(string data);

        void WriteFixedString(string data);

        void WriteInt(int data);

        void WriteUInt(int data, int precision);

        void WriteEnum(int data);

        void WriteBool(bool data);

        void WriteIndex(int data);

        void WriteName(int data);

        void WriteColor(CGMColor color, int localColorPrecision = -1);

        void WriteDirectColor(System.Drawing.Color color);

        void WriteColorIndex(int index);

        void WriteColorIndex(int index, int localColorPrecision);
        void WriteVdc(double data);

        void WritePoint(CGMPoint point);

        void WriteReal(double data);

        void WriteSDR(StructuredDataRecord data);

        void WriteFloatingPoint32(double data);

        void WriteFloatingPoint(double data);

        void FillToWord();

        void WriteViewportPoint(ViewportPoint data);

        void WriteSizeSpecification(double data, SpecificationMode specificationMode);

        void WriteEmbeddedCommand(Commands.Command command);
        void Unsupported(string message);
        void WriteByte(byte data);
    }
}
