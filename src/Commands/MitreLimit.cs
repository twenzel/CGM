namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=3, ElementId=19
    /// </summary>
    public class MitreLimit : Command
    {
        public double Limit { get; set; }

        public MitreLimit(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 19, container))
        {
        }

        public MitreLimit(CGMFile container, double limit)
            : this(container)
        {
            Limit = limit;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Limit = reader.ReadReal();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteReal(Limit);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" MITRELIMIT {WriteReal(Limit)};");
        }
    }
}
