namespace codessentials.CGM.Commands
{
    public class CharacterSpacing : Command
    {
        public double Space { get; private set; }

        public CharacterSpacing(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 13, container))
        {

        }

        public CharacterSpacing(CGMFile container, double space)
            : this(container)
        {
            Space = space;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Space = reader.ReadReal();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteReal(Space);
        }

        public override string ToString()
        {
            return $"CharacterSpacing {Space}";
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" CHARSPACE {WriteReal(Space)};");
        }
    }
}