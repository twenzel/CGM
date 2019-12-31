using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Commands
{
    public abstract class DashTypeCommand : Command
    {
        public DashType Type { get; set; } = DashType.SOLID;

        public DashTypeCommand(CommandConstructorArguments args) 
            : base(args)
        {
           
        }
 

        protected void SetValue(DashType type)
        {
            Type = type;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Type = (DashType)reader.ReadInt();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteInt((int)Type);
        }

        protected string WriteDashType()
        {
            return ((int)Type).ToString();
        }
    }
}
