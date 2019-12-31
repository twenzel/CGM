using codessentials.CGM.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Commands
{
    public abstract class TextCommand : Command
    {
        /** The string to display */
        protected string _string;
	
	    /** The position at which the string should be displayed */
	    protected CGMPoint _position;

        public TextCommand(CommandConstructorArguments args)
            : base(args)
        {
        } 

        protected void SetValues(string data, CGMPoint position)
        {
            _string = data;
            _position = position;
        }

        public override string ToString()
        {
            return $"Text position={_position} string={_string}";
        }

        public string Text
        {
            get { return _string; }
        }

        public CGMPoint Position
        {
            get { return _position; }
        }
    }
}
