using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=17
    /// </summary>
    public class EllipseElement : Command
    {
        public CgmPoint Center { get; set; }
        public CgmPoint FirstConjugateDiameterEndPoint { get; set; }
        public CgmPoint SecondConjugateDiameterEndPoint { get; set; }

        public EllipseElement(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 17, container))
        {

        }

        public EllipseElement(CommandConstructorArguments args)
           : base(args)
        {

        }

        public EllipseElement(CgmFile container, CgmPoint center, CgmPoint first, CgmPoint second)
            : this(container)
        {
            SetValues(center, first, second);
        }

        protected void SetValues(CgmPoint center, CgmPoint first, CgmPoint second)
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
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write("  ELLIPSE");
            WriteValues(writer);
            writer.WriteLine(";");
        }
    }
}