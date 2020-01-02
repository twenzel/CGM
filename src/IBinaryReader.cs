using System.Drawing;
using codessentials.CGM.Classes;
using codessentials.CGM.Commands;

namespace codessentials.CGM
{
    /// <summary>
    /// Interface to read binary CGM files
    /// </summary>
    public interface IBinaryReader
    {
        int CurrentArg { get; }
        int ArgumentsCount { get; }
        byte[] Arguments { get; }

        int ReadEnum();
        string ReadString();
        int ReadIndex();
        int ReadInt();
        string ReadFixedString();
        StructuredDataRecord ReadSDR();
        CGMColor ReadColor();
        CGMColor ReadColor(int localColorPrecision);
        double ReadVdc();
        int ReadName();
        Color ReadDirectColor();
        ViewportPoint ReadViewportPoint();
        Command ReadEmbeddedCommand();
        double ReadReal();
        string ReadFixedStringWithFallback(int length);
        CGMPoint ReadPoint();
        byte ReadByte();
        void AlignOnWord();
        int ReadColorIndex();
        int ReadColorIndex(int localColorPrecision);
        int SizeOfEnum();
        int SizeOfPoint();
        int ReadUInt(int precision);
        bool ReadBool();
        double ReadSizeSpecification(SpecificationMode edgeWidthSpecificationMode);
        double ReadFloatingPoint();
        double ReadFloatingPoint32();
        int SizeOfInt();
        void Unsupported(string message);
        int SizeOfDirectColor();
    }
}
