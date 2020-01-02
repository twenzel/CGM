namespace codessentials.CGM.Commands
{
    public class CharacterExpansionFactor : Command
    {
        public double Factor { get; private set; }

        public CharacterExpansionFactor(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 12, container))
        {

        }

        public CharacterExpansionFactor(CgmFile container, double factor)
            : this(container)
        {
            Factor = factor;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Factor = reader.ReadReal();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteReal(Factor);
        }

        public override string ToString()
        {
            return $"CharacterExpansionFactor {Factor}";
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" CHAREXPAN {WriteReal(Factor)};");
        }
    }
}