namespace codessentials.CGM.Commands
{
    public abstract class JoinCommand : Command
    {
        public JoinIndicator Type { get; set; }

        protected JoinCommand(CommandConstructorArguments args)
            : base(args)
        {


        }

        protected void SetValue(JoinIndicator type)
        {
            Type = type;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var indexValue = reader.ReadIndex();
            Type = indexValue switch
            {
                1 => JoinIndicator.UNSPECIFIED,
                2 => JoinIndicator.MITRE,
                3 => JoinIndicator.ROUND,
                4 => JoinIndicator.BEVEL,
                _ => JoinIndicator.UNSPECIFIED,
            };
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex((int)Type);
        }
    }
}
