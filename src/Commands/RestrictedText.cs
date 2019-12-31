using codessentials.CGM.Classes;
using System.Collections.Generic;
using System.IO;
using System;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=5
    /// </summary>
    public class RestrictedText : TextCommand
    {
        public double DeltaWidth { get; set; }
        public double DeltaHeight { get; set; }
        public bool Final { get; set; }

        public RestrictedText(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 5, container))
        {

           
        }

        public RestrictedText(CGMFile container, string data, CGMPoint position, double deltaWidth, double deltaHeight, bool final)
            :this(container)
        {
            DeltaWidth = deltaWidth;
            DeltaHeight = deltaHeight;
            Final = final;
            SetValues(data, position);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            DeltaWidth = reader.ReadVdc();
            DeltaHeight = reader.ReadVdc();
            _position = reader.ReadPoint();

            Final = reader.ReadBool();

            _string = reader.ReadString();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteVdc(DeltaWidth);
            writer.WriteVdc(DeltaHeight);
            writer.WritePoint(Position);
            writer.WriteBool(Final);
            writer.WriteString(Text);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($"  RESTRTEXT");
            writer.Write(" " + WriteDouble(DeltaWidth));
            writer.Write(" " + WriteDouble(DeltaHeight));
            writer.Write("  " + WritePoint(_position));

            if (Final)
                writer.Write($" final");
            else
                writer.Write($" notfinal");

            writer.Write($" {WriteString(_string)}");

            writer.WriteLine(";");
        }

        public override string ToString()
        {
            return $"RestrictedText {_string} deltaWidth={DeltaWidth} deltaHeight={DeltaHeight} textPosition.x={_position.X} textPosition.y={_position.Y}";
        }
    }
}