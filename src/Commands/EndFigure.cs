using System;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=9
    /// </remarks>
    public class EndFigure : Command
    { 

        public EndFigure(CGMFile container)
            :base(new CommandConstructorArguments(ClassCode.DelimiterElement, 9, container))
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
            writer.WriteLine($" ENDFIGURE;");
        }
    }
}