using System;
using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=2, Element=13
    /// </remarks>
    public class TextRepresentation : Command
    {
        public int BundleIndex { get; set; }
        public int FontIndex { get; set; }
        public TextPrecisionType Precision { get; set; }
        public double Spacing { get; set; }
        public double Expansion { get; set; }
        public CGMColor Color { get; set; }

        public TextRepresentation(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 13, container))
        {
            
        }

        public TextRepresentation(CGMFile container, int bundleIndex, int fontIndex, TextPrecisionType precision, double spacing, double expansion, CGMColor color)
            :this(container)
        {
            BundleIndex = bundleIndex;
            FontIndex = fontIndex;
            Precision = precision;
            Spacing = spacing;
            Expansion = expansion;
            Color = color;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            BundleIndex = reader.ReadIndex();
            FontIndex = reader.ReadIndex();
            Precision = (TextPrecisionType)reader.ReadEnum();
            Spacing = reader.ReadReal();
            Expansion = reader.ReadReal();
            Color = reader.ReadColor();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(BundleIndex);
            writer.WriteIndex(FontIndex);
            writer.WriteEnum((int)Precision);
            writer.WriteReal(Spacing);
            writer.WriteReal(Expansion);
            writer.WriteColor(Color);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" TEXTREP {WriteIndex(BundleIndex)} {WriteIndex(FontIndex)} {WriteEnum(Precision)} {WriteReal(Spacing)} {WriteReal(Expansion)} {WriteColor(Color)};");
        }
    }
}