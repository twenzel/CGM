using codessentials.CGM.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=28
    /// </summary>
    public class BitonalTile : TileElement
    {
        private CGMColor _backgroundColor;
        private CGMColor _foregroundColor;

        public BitonalTile(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 28, container))
        {
           
        }

        public BitonalTile(CGMFile container, CompressionType compressionType, int rowPaddingIndicator, CGMColor backgroundcolor, CGMColor foregroundcolor, StructuredDataRecord sdr, MemoryStream image)
            :this(container)
        {
            _sdr = sdr;
            _compressionType = compressionType;
            _rowPaddingIndicator = rowPaddingIndicator;
            _backgroundColor = backgroundcolor;
            _foregroundColor = foregroundcolor;
            _image = image;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _compressionType = (CompressionType)reader.ReadIndex();
            _rowPaddingIndicator = reader.ReadInt();

            _backgroundColor = reader.ReadColor();
            _foregroundColor = reader.ReadColor();

            readSdrAndBitStream(reader);
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex((int)_compressionType);
            writer.WriteInt(_rowPaddingIndicator);
            writer.WriteColor(_backgroundColor);
            writer.WriteColor(_foregroundColor);
            WriteSdrAndBitStream(writer);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" BITONALTILE");
            writer.Write($" {WriteInt((int)_compressionType)}");
            writer.Write($" {WriteInt(_rowPaddingIndicator)}");
            
            writer.Write($" {WriteColor(_backgroundColor)}");
            writer.Write($" {WriteColor(_foregroundColor)}");           

            WriteSDR(writer, _sdr);
            if (_image != null)
                writer.Write($" {WriteBitStream(_image.ToArray())}");

            writer.WriteLine(";");
        }

        protected override void readBitmap(IBinaryReader reader)
        {
            reader.Unsupported("BITMAP for BitonalTile");
        }

        protected override void writeBitmap(IBinaryWriter writer)
        {
            writer.Unsupported("BITMAP for BitonalTile");
        }

        public CGMColor Backgroundcolor => _backgroundColor;
        public CGMColor Foregroundcolor => _foregroundColor;

    }
}