using System.Text;

namespace codessentials.CGM
{
    public class Message
    {
        public string Text { get; }
        public string CommandDescription { get; }

        public ClassCode ElementClass { get; private set; }

        public int ElementId { get; private set; }

        public Severity Severity { get; private set; }

        /// <summary>
        /// Initialize a new message
        /// </summary>
        /// <param name="severity">The severity of the message</param>
        /// <param name="elementClass">The corresponding element class</param>
        /// <param name="elementId">The corresponding element ID</param>
        /// <param name="message">A message</param>
        /// <param name="commandDescription">The command description (optional), typically the output of the <code>ToString()</code> method for the command</param>
        public Message(Severity severity, ClassCode elementClass, int elementId, string message, string commandDescription)
        {
            Severity = severity;
            ElementClass = elementClass;
            ElementId = elementId;
            Text = message;
            CommandDescription = commandDescription;
        }

        public override string ToString()
        {
            if (Severity == Severity.Info)
                return $"{Severity}: {Text}";
            else
            {
                var sb = new StringBuilder();
                sb.Append($"{Severity}:( {ElementClass} {ElementId}) {Text}");

                if (CommandDescription != null)
                    sb.Append($" {{{CommandDescription}}}");

                return sb.ToString();
            }
        }
    }
}
