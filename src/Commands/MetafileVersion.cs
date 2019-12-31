using System;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=1
    /// </remarks>
    public class MetafileVersion : Command
    {
        private int _version;

        public int Version
        {
            get { return _version; }
        }

        public MetafileVersion(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 1, container))
        {
        }

        public MetafileVersion(CGMFile container, int version)
            : this(container)
        {
            _version = version;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _version = reader.ReadInt();

        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteInt(_version);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" mfversion {_version};");
        }

        public override string ToString()
        {
            return $"MetafileVersion {_version}";
        }
    }
}