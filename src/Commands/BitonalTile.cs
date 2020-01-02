using codessentials.CGM.Classes;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=28
    /// </summary>
    public class BitonalTile : TileElement
    {
        public CGMColor Backgroundcolor { get; private set; }
        public CGMColor Foregroundcolor { get; private set; }

        public BitonalTile(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 28, container))
        {

        }

        public BitonalTile(CGMFile container, CompressionType compressionType, int rowPaddingIndicator, CGMColor backgroundcolor, CGMColor foregroundcolor, StructuredDataRecord sdr, MemoryStream image)
            : this(container)
        {
            DataRecord = sdr;
            CompressionType = compressionType;
            RowPaddingIndicator = rowPaddingIndicator;
            Backgroundcolor = backgroundcolor;
            Foregroundcolor = foregroundcolor;
            Image = image;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            CompressionType = (CompressionType)reader.ReadIndex();
            RowPaddingIndicator = reader.ReadInt();

            Backgroundcolor = reader.ReadColor();
            Foregroundcolor = reader.ReadColor();

            readSdrAndBitStream(reader);
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex((int)CompressionType);
            writer.WriteInt(RowPaddingIndicator);
            writer.WriteColor(Backgroundcolor);
            writer.WriteColor(Foregroundcolor);
            WriteSdrAndBitStream(writer);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" BITONALTILE");
            writer.Write($" {WriteInt((int)CompressionType)}");
            writer.Write($" {WriteInt(RowPaddingIndicator)}");

            writer.Write($" {WriteColor(Backgroundcolor)}");
            writer.Write($" {WriteColor(Foregroundcolor)}");

            WriteSDR(writer, DataRecord);
            if (Image != null)
                writer.Write($" {WriteBitStream(Image.ToArray())}");

            writer.WriteLine(";");
        }

        protected override void ReadBitmap(IBinaryReader reader)
        {
            reader.Unsupported("BITMAP for BitonalTile");
        }

        protected override void WriteBitmap(IBinaryWriter writer)
        {
            writer.Unsupported("BITMAP for BitonalTile");
        }
    }
}