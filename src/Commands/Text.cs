using codessentials.CGM.Classes;
using System.Collections.Generic;
using System.IO;
using System;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=4
    /// </summary>
    public class Text : TextCommand
    {
        public bool Final { get; set; }

        public Text(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 4, container))
        {

        }

        public Text(CGMFile container, string data, CGMPoint position, bool final)
            : this(container)
        {
            Final = final;
            SetValues(data, position);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _position = reader.ReadPoint();
            Final = reader.ReadEnum() != 0;
            _string = reader.ReadString();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WritePoint(Position);
            writer.WriteBool(Final);
            writer.WriteString(Text);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" TEXT {WritePoint(_position)}");

            if (Final)
                writer.Write($" final");
            else
                writer.Write($" notfinal");

            writer.Write($" {WriteString(_string)}");

            writer.WriteLine(";");
        }
    }
}