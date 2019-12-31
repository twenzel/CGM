using codessentials.CGM.Classes;
using System.Collections.Generic;
using System.IO;
using System;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=17
    /// </summary>
    public class EllipseElement : Command
    {
        public CGMPoint Center { get; set; }
        public CGMPoint FirstConjugateDiameterEndPoint { get; set; }
        public CGMPoint SecondConjugateDiameterEndPoint { get; set; }

        public EllipseElement(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 17, container))
        {
           
        }

        public EllipseElement(CommandConstructorArguments args)
           : base(args)
        {

        }

        public EllipseElement(CGMFile container, CGMPoint center, CGMPoint first, CGMPoint second)
            :this(container)
        {
            SetValues(center, first, second);
        }

        protected void SetValues(CGMPoint center, CGMPoint first, CGMPoint second)
        {
            Center = center;
            FirstConjugateDiameterEndPoint = first;
            SecondConjugateDiameterEndPoint = second;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Center = reader.ReadPoint();
            FirstConjugateDiameterEndPoint = reader.ReadPoint();
            SecondConjugateDiameterEndPoint = reader.ReadPoint();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WritePoint(Center);
            writer.WritePoint(FirstConjugateDiameterEndPoint);
            writer.WritePoint(SecondConjugateDiameterEndPoint);
        }

        protected virtual void WriteValues(IClearTextWriter writer)
        {
            writer.Write($" {WritePoint(Center.X, Center.Y)}");
            writer.Write($" {WritePoint(FirstConjugateDiameterEndPoint)}");
            writer.Write($" {WritePoint(SecondConjugateDiameterEndPoint)}");
            //writer.WriteLine("");
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write("  ELLIPSE");
            WriteValues(writer);
            writer.WriteLine(";");
        }
    }
}