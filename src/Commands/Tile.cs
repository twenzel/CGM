using codessentials.CGM.Classes;
using System;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=29
    /// </summary>
    public class Tile : TileElement
    {
        public int CellColorPrecision { get; set; }

        public Tile(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 29, container))
        {
            
        }

        public Tile(CGMFile container, CompressionType compressionType, int rowPaddingIndicator, int cellColorPrecision, StructuredDataRecord sdr, MemoryStream image)
            :this(container)
        {
            _compressionType = compressionType;
            _rowPaddingIndicator = rowPaddingIndicator;
            _sdr = sdr;
            _image = image;
            CellColorPrecision = cellColorPrecision;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _compressionType = (CompressionType)reader.ReadIndex();
            _rowPaddingIndicator = reader.ReadInt();

            CellColorPrecision = reader.ReadInt();
            if (CellColorPrecision == 0)
            {
                //if (_container.ColourSelectionMode == ColourSelectionMode.Type.INDEXED)
                //{
                //    cellColorPrecision = _container.ColourIndexPrecision;
                //}
                //else
                //{
                //    cellColorPrecision = _container.ColourPrecision;
                //}
            }

            readSdrAndBitStream(reader);            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex((int)CompressionType);
            writer.WriteInt(RowPaddingIndicator);
            writer.WriteInt(CellColorPrecision);

            WriteSdrAndBitStream(writer);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" TILE");
            writer.Write($" {WriteInt((int)_compressionType)}");
            writer.Write($" {WriteInt(_rowPaddingIndicator)}");

            if (CellColorPrecision == 0)
            {
                if (_container.ColourSelectionMode == ColourSelectionMode.Type.INDEXED)
                {
                    writer.Write(ColourIndexPrecision.WriteValue(_container.ColourIndexPrecision));
                }
                else
                {
                    writer.Write(ColourPrecision.WritValue(_container.ColourPrecision));
                }
            }
            else
                writer.Write(WriteInt(CellColorPrecision));


            WriteSDR(writer, _sdr);
            if (_image != null)
                writer.Write($" {WriteBitStream(_image.ToArray())}");            

            writer.WriteLine(";");
        }

        protected override void readBitmap(IBinaryReader reader)
        {
            reader.Unsupported("BITMAP for Tile");
        }

        protected override void writeBitmap(IBinaryWriter writer)
        {
            writer.Unsupported("BITMAP for Tile");
        }

        public override string ToString()
        {
            return $"Tile [compressionType={_compressionType}, rowPaddingIndicator={_rowPaddingIndicator}]";
        }
    }
}