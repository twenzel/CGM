using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=3
    /// </remarks>
    public class BeginPicture : Command
    {
        private string _pictureName;

        public BeginPicture(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 3, container))
        {
            
        }

        public BeginPicture(CGMFile container, string name)
            :this(container)
        {
            _pictureName = name;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _pictureName = reader.ArgumentsCount > 0 ? reader.ReadString() : "";            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteString(_pictureName);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"\n BEGPIC '{_pictureName}';");
        }

        public override string ToString()
        {
            return "BeginPicture " + _pictureName;
        }

        public string PictureName => _pictureName;
    }
}