using System;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// ClassId=8, ElementId=1
    /// </remarks>
    public class CopySegment : Command
    {
        public int Id { get; set; }

        public double XScale { get; set; }

        public double XRotation { get; set; }

        public double YRotation { get; set; }

        public double YScale { get; set; }

        public double XTranslation { get; set; }
        public double YTranslation { get; set; }

        public bool Flag { get; set; }

        public CopySegment(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.SegmentControlandSegmentAttributeElements, 1, container))
        {
            
        }

        public CopySegment(CGMFile container, int id, double xScale, double xRotation, double yRotation, double yScale, double xTranslation, double yTranslation, bool flag)
            :this(container)
        {
            Id = id;
            XScale = xScale;
            XRotation = xRotation;
            YRotation = yRotation;
            YScale = yScale;
            XTranslation = xTranslation;
            YTranslation = yTranslation;
            Flag = flag;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Id = reader.ReadName();

            XScale = reader.ReadReal();
            XRotation = reader.ReadReal();
            YRotation = reader.ReadReal();
            YScale = reader.ReadReal();
            XTranslation = reader.ReadVdc();
            YTranslation = reader.ReadVdc();

            Flag = reader.ReadBool();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteName(Id);
            writer.WriteReal(XScale);
            writer.WriteReal(XRotation);
            writer.WriteReal(YRotation);
            writer.WriteReal(YScale);
            writer.WriteVdc(XTranslation);
            writer.WriteVdc(YTranslation);
            writer.WriteBool(Flag);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" COPYSEG {WriteName(Id)}");
            writer.Write($" {WriteReal(XScale)} {WriteReal(XRotation)} {WriteReal(YRotation)} {WriteReal(YScale)} {WriteVDC(XTranslation)} {WriteVDC(YTranslation)}");
            writer.WriteLine($" {WriteBoolYesNo(Flag)};");
        }
    }
}