using codessentials.CGM.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Commands
{  
    public abstract class ColourCommand : Command
    {
        public CGMColor Color { get; set; }

        public ColourCommand(CommandConstructorArguments args)
            : base(args)
        {
            
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Color = reader.ReadColor();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteColor(Color);
        }

        protected void SetValue(CGMColor color)
        {
            Color = color;
        }
    }
}
