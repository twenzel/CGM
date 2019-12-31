using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM
{
    public class Message
    {
        private Severity _severity;
	    private string _message;
	    private ClassCode _elementClass;
        private int _elementId;
        private string _commandDescription;

        public ClassCode ElementClass
        {
            get { return _elementClass; }
        }

        public int ElementId
        {
            get { return _elementId; }
        }

        public Severity Severity
        {
            get { return _severity; }
        }

        /// <summary>
        /// Initialize a new message
        /// </summary>
        /// <param name="severity">The severity of the message</param>
        /// <param name="elementClass">The corresponding element class</param>
        /// <param name="elementId">The corresponding element ID</param>
        /// <param name="message">A message</param>
        /// <param name="commandDescription">The command description (optional), typically the output of the <code>ToString()</code> method for the command</param>
        public Message(Severity severity, ClassCode elementClass, int elementId, String message, String commandDescription)
        {
            _severity = severity;
            _elementClass = elementClass;
            _elementId = elementId;
            _message = message;
            _commandDescription = commandDescription;
        }

        public override string ToString()
        {           
            if (_severity == Severity.Info)            
                return $"{_severity}: {_message}";
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{_severity}:( {_elementClass} {_elementId}) {_message}");

                if (_commandDescription != null)
                    sb.Append($" {{{_commandDescription}}}");
                
                return sb.ToString();
            }            
        }
    }
}
