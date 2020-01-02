using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=17
    /// </remarks>
    public class MaximumVDCExtent : Command
    {
        public CGMPoint FirstCorner { get; set; }
        public CGMPoint SecondCorner { get; set; }

        public MaximumVDCExtent(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 17, container))
        {

        }

        public MaximumVDCExtent(CGMFile container, CGMPoint first, CGMPoint second)
            : this(container)
        {
            FirstCorner = first;
            SecondCorner = second;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            FirstCorner = reader.ReadPoint();
            SecondCorner = reader.ReadPoint();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WritePoint(FirstCorner);
            writer.WritePoint(SecondCorner);
        }

        public override string ToString()
        {
            return $"MaximumVDCExtent [{FirstCorner.X},{FirstCorner.Y}] [{SecondCorner.X},{SecondCorner.Y}]";
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" MAXVDCEXT {WritePoint(FirstCorner)} {WritePoint(SecondCorner)};");
        }
    }
}
