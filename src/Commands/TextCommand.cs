using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    public abstract class TextCommand : Command
    {
        /// <summary>
        /// The string to display
        /// </summary>
        public string Text { get; protected set; }

        /// <summary>
        /// The position at which the string should be displayed
        /// </summary>
        public CGMPoint Position { get; protected set; }

        protected TextCommand(CommandConstructorArguments args)
            : base(args)
        {
        }

        protected void SetValues(string data, CGMPoint position)
        {
            Text = data;
            Position = position;
        }

        public override string ToString()
        {
            return $"Text position={Position} string={Text}";
        }
    }
}
