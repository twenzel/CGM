using System.IO;
using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    public abstract class TileElement : Command
    {
        public StructuredDataRecord DataRecord { get; protected set; }

        public CompressionType CompressionType { get; protected set; }
        public int RowPaddingIndicator { get; protected set; }
        public MemoryStream Image { get; protected set; }

        protected TileElement(CommandConstructorArguments arguments)
           : base(arguments)
        {
        }

        protected void readSdrAndBitStream(IBinaryReader reader)
        {
            // the kind of information contained in the structured data record will
            // depend on the compression type
            DataRecord = reader.ReadSDR();

            if (CompressionType == CompressionType.BASELINE_JPEG || CompressionType == CompressionType.PNG)
            {
                Image = new MemoryStream(reader.ArgumentsCount - reader.CurrentArg);

                while (reader.CurrentArg < reader.ArgumentsCount)
                    Image.WriteByte(reader.ReadByte());
            }
            else
            {
                switch (CompressionType)
                {
                    case CompressionType.BITMAP:
                        ReadBitmap(reader);
                        break;
                    default:
                        reader.Unsupported("unsupported compression type " + CompressionType);
                        reader.ReadArgumentEnd();
                        break;
                }
            }
        }

        protected void WriteSdrAndBitStream(IBinaryWriter writer)
        {
            writer.WriteSDR(DataRecord);
            if (CompressionType == CompressionType.BASELINE_JPEG || CompressionType == CompressionType.PNG)
            {
                foreach (var b in Image.ToArray())
                    writer.WriteByte(b);
            }
            else
            {
                switch (CompressionType)
                {
                    case CompressionType.BITMAP:
                        WriteBitmap(writer);
                        break;
                    default:
                        writer.Unsupported("unsupported compression type " + CompressionType);
                        break;
                }
            }
        }

        abstract protected void ReadBitmap(IBinaryReader reader);
        abstract protected void WriteBitmap(IBinaryWriter writer);
    }
}
