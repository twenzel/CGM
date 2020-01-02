namespace codessentials.CGM.Commands
{
    public abstract class JoinCommand : Command
    {
        public JoinIndicator Type { get; set; }

        public JoinCommand(CommandConstructorArguments args)
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
            switch (indexValue)
            {
                case 1:
                    Type = JoinIndicator.UNSPECIFIED;
                    break;
                case 2:
                    Type = JoinIndicator.MITRE;
                    break;
                case 3:
                    Type = JoinIndicator.ROUND;
                    break;
                case 4:
                    Type = JoinIndicator.BEVEL;
                    break;
                default:
                    Type = JoinIndicator.UNSPECIFIED;
                    break;
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex((int)Type);
        }
    }
}
