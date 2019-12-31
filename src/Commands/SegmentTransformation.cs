using System;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=8, ElementId=4
    /// </summary>
    public class SegmentTransformation : Command
    {
        public int Identifier { get; set; }
        public double ScaleX { get; set; }
        public double RotationX { get; set; }
        public double RotationY { get; set; }
        public double ScaleY { get; set; }
        public double TranslationX { get; set; }
        public double TranslationY { get; set; }

        public SegmentTransformation(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.SegmentControlandSegmentAttributeElements, 4, container))
        {
           
        }

        public SegmentTransformation(CGMFile container, int id, double scaleX, double rotationX, double rotationY, double scaleY, double translationX, double translationY)
            :this(container)
        {
            Identifier = id;
            ScaleX = scaleX;            
            RotationX = rotationX;
            RotationY = rotationY;
            ScaleY = scaleY;
            TranslationX = translationX;
            TranslationY = translationY;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Identifier = reader.ReadName();
            ScaleX = reader.ReadReal();
            RotationX = reader.ReadReal();
            RotationY = reader.ReadReal();
            ScaleY = reader.ReadReal();
            TranslationX = reader.ReadVdc();
            TranslationY = reader.ReadVdc();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteName(Identifier);
            writer.WriteReal(ScaleX);
            writer.WriteReal(RotationX);
            writer.WriteReal(RotationY);
            writer.WriteReal(ScaleY);
            writer.WriteVdc(TranslationX);
            writer.WriteVdc(TranslationY);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" SEGTRAN {WriteName(Identifier)}");
            writer.Write($" {WriteReal(ScaleX)} {WriteReal(RotationX)} {WriteReal(RotationY)} {WriteReal(ScaleY)} {WriteVDC(TranslationX)} {WriteVDC(TranslationY)}");
            writer.WriteLine(";");
        }
    }
}