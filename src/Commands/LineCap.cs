using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    public class LineCap : CapCommand
    {
        public LineCap(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 37, container))
        {
        }

        public LineCap(CGMFile container, LineCapIndicator lineIndicator, DashCapIndicator dashIndicator)
            :this(container)
        {
            SetValues(lineIndicator, dashIndicator);
        }


        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" LINECAP {WriteInt((int)LineIndicator)} {WriteInt((int)DashIndicator)};");
        }
    }
}