using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=17
    /// </remarks>
    public class BeginCompoundTextPath : Command
    {     

        public BeginCompoundTextPath(CGMFile container)
            :base(new CommandConstructorArguments(ClassCode.DelimiterElement, 17, container))
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
            writer.WriteLine($" BEGCOMPOTEXTPATH;");
        }
    }
}