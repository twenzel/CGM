
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=1
    /// </remarks>
    public class BeginMetafile : Command
    {
        private string _fileName;

        public string FileName { get { return _fileName; } }

        public BeginMetafile(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 1, container))
        {
            
        }

        public BeginMetafile(CGMFile container, string fileName)
            :this(container)
        {
            _fileName = fileName;            
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _fileName = reader.ArgumentsCount > 0 ? reader.ReadString() : "";            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {        
            writer.WriteString(_fileName);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"BEGMF {WriteString(_fileName)};");
        }

        public override string ToString()
        {
            return "BeginMetafile " + _fileName;
        }        
    }
}