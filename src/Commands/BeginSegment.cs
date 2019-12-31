using System;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=6
    /// </remarks>
    public class BeginSegment : Command
    {
        private int _id;

        public BeginSegment(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 6, container))
        {
           
        }

        public BeginSegment(CGMFile container, int id)
            :this(container)
        {
            _id = id;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _id = reader.ReadName();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteName(_id);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" BEGSEG {WriteName(_id)};");
        }

        public int Id => _id;
    }
}