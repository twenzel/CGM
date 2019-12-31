using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Commands
{ 
    public abstract class GenericIndexCommand : Command
    {
        public int Index { get; set; }
        private string _name;

        public GenericIndexCommand(CommandConstructorArguments arguments, string name)
            : base(arguments)
        {           
            _name = name;
        }

        protected void SetValue(int index)
        {
            Index = index;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Index = reader.ReadIndex();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(Index);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" {_name} {WriteIndex(Index)};");
        }

        public override string ToString()
        {
            return $"{_name} {Index};";
        }
    }
}
