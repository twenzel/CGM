using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Commands
{
    public abstract class CapCommand: Command
    {
        public LineCapIndicator LineIndicator { get; set; }
        public DashCapIndicator DashIndicator { get; set; }

        public CapCommand(CommandConstructorArguments arguments) 
            : base(arguments)
        {
            
        }    
      

        public override void ReadFromBinary(IBinaryReader reader)
        {
            int lineIndic = reader.ReadIndex();
            switch (lineIndic)
            {
                case 1:
                    LineIndicator = LineCapIndicator.UNSPECIFIED;
                    break;
                case 2:
                    LineIndicator = LineCapIndicator.BUTT;
                    break;
                case 3:
                    LineIndicator = LineCapIndicator.ROUND;
                    break;
                case 4:
                    LineIndicator = LineCapIndicator.PROJECTING_SQUARE;
                    break;
                case 5:
                    LineIndicator = LineCapIndicator.TRIANGLE;
                    break;
                default:
                    reader.Unsupported("unsupported line cap indicator " + lineIndic);
                    LineIndicator = LineCapIndicator.UNSPECIFIED;
                    break;
            }

            int dashIndic = reader.ReadIndex();
            switch (dashIndic)
            {
                case 1:
                    DashIndicator = DashCapIndicator.UNSPECIFIED;
                    break;
                case 2:
                    DashIndicator = DashCapIndicator.BUTT;
                    break;
                case 3:
                    DashIndicator = DashCapIndicator.MATCH;
                    break;
                default:
                    reader.Unsupported("unsupported dash cap indicator " + dashIndic);
                    DashIndicator = DashCapIndicator.UNSPECIFIED;
                    break;
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex((int)LineIndicator);
            writer.WriteIndex((int)DashIndicator);
        }

        protected void SetValues(LineCapIndicator lineIndicator, DashCapIndicator dashIndicator)
        {
            LineIndicator = lineIndicator;
            DashIndicator = dashIndicator;
        }
    }
}
