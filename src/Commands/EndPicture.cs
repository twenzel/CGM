using System;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=5
    /// </remarks>
    public class EndPicture : Command
    {
        public EndPicture(CGMFile container)
            :base(new CommandConstructorArguments(ClassCode.DelimiterElement, 5, container))
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
            writer.WriteLine($" ENDPIC;");
        }

        public override string ToString()
        {
            return "EndPicture";
        }
    }
}