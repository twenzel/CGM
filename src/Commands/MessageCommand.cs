namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=7, ElementId=1
    /// </summary>
    public class MessageCommand : Command
    {
        public enum ActionType
        {
            NoAction = 0,
            Action = 1
        }

        public ActionType Action { get; set; }
        public string Message { get; set; }

        public MessageCommand(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.ExternalElements, 1, container))
        {

        }

        public MessageCommand(CGMFile container, ActionType action, string message)
            : this(container)
        {
            Action = action;
            Message = message;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Action = (ActionType)reader.ReadEnum();
            Message = reader.ReadString();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Action);
            writer.WriteString(Message);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" MESSAGE {WriteEnum(Action)}, {WriteString(Message)};");
        }
    }
}
