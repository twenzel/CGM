using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        byte[] Arguments { get;  }

        int ReadEnum();
        string ReadString();
        int ReadIndex();
        int ReadInt();
        string ReadFixedString();
        StructuredDataRecord ReadSDR();
        CGMColor ReadColor();
        double ReadVdc();
        int ReadColorIndex();
        int ReadName();
        Color ReadDirectColor();
        ViewportPoint ReadViewportPoint();
        Command ReadEmbeddedCommand();
        double ReadReal();
        string ReadFixedStringWithFallback(int length);
        CGMPoint ReadPoint();
        byte ReadByte();
        void AlignOnWord();
        int ReadColorIndex(int localColorPrecision);
        int SizeOfEnum();
        int SizeOfPoint();
        int ReadUInt(int precision);        
        bool ReadBool();
        double ReadSizeSpecification(SpecificationMode edgeWidthSpecificationMode);
        double ReadFloatingPoint();
        double ReadFloatingPoint32();
        CGMColor ReadColor(int localColorPrecision = -1);
        int SizeOfInt();
        void Unsupported(string message);
        int SizeOfDirectColor();
    }
}
