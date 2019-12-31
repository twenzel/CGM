using System;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=7
    /// </remarks>
    public class EndSegment : Command
    {
        public EndSegment(CGMFile container)
            :base(new CommandConstructorArguments(ClassCode.DelimiterElement, 7, container))
        {

        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {

        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" ENDSEG;");
        }
    }
}