using System.IO;
using codessentials.CGM.Classes;

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
            : this(container)
        {
            CompressionType = compressionType;
            RowPaddingIndicator = rowPaddingIndicator;
            DataRecord = sdr;
            Image = image;
            CellColorPrecision = cellColorPrecision;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            CompressionType = (CompressionType)reader.ReadIndex();
            RowPaddingIndicator = reader.ReadInt();

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
            writer.Write($" {WriteInt((int)CompressionType)}");
            writer.Write($" {WriteInt(RowPaddingIndicator)}");

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


            WriteSDR(writer, DataRecord);
            if (Image != null)
                writer.Write($" {WriteBitStream(Image.ToArray())}");

            writer.WriteLine(";");
        }

        protected override void ReadBitmap(IBinaryReader reader)
        {
            reader.Unsupported("BITMAP for Tile");
        }

        protected override void WriteBitmap(IBinaryWriter writer)
        {
            writer.Unsupported("BITMAP for Tile");
        }

        public override string ToString()
        {
            return $"Tile [compressionType={CompressionType}, rowPaddingIndicator={RowPaddingIndicator}]";
        }
    }
}
