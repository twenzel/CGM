using codessentials.CGM.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Commands
{
    public abstract class TileElement : Command
    {
        protected CompressionType _compressionType;
        protected int _rowPaddingIndicator;
        protected StructuredDataRecord _sdr;
        protected MemoryStream _image;

        public TileElement(CommandConstructorArguments arguments)
           : base(arguments)
        {
        }              

        protected void readSdrAndBitStream(IBinaryReader reader)
        {
            // the kind of information contained in the structured data record will
            // depend on the compression type
            _sdr = reader.ReadSDR();

            if (_compressionType == CompressionType.BASELINE_JPEG || _compressionType == CompressionType.PNG)
            {
                _image = new MemoryStream(reader.ArgumentsCount - reader.CurrentArg);

                while (reader.CurrentArg < reader.ArgumentsCount)
                    _image.WriteByte(reader.ReadByte());
            }
            else
            {
                switch (_compressionType)
                {
                    case CompressionType.BITMAP:
                        readBitmap(reader);
                        break;
                    default:
                        reader.Unsupported("unsupported compression type " + _compressionType);
                        break;
                }
            }
        }

        protected void WriteSdrAndBitStream(IBinaryWriter writer)
        {
            writer.WriteSDR(DataRecord);
            if (_compressionType == CompressionType.BASELINE_JPEG || _compressionType == CompressionType.PNG)
            {
                foreach (var b in _image.ToArray())
                    writer.WriteByte(b);                
            }
            else
            {
                switch (_compressionType)
                {
                    case CompressionType.BITMAP:
                        writeBitmap(writer);
                        break;
                    default:
                        writer.Unsupported("unsupported compression type " + _compressionType);
                        break;
                }
            }
        }

        abstract protected void readBitmap(IBinaryReader reader);
        abstract protected void writeBitmap(IBinaryWriter writer);

        public StructuredDataRecord DataRecord => _sdr;

        public CompressionType CompressionType => _compressionType;
        public int RowPaddingIndicator => _rowPaddingIndicator;
        public MemoryStream Image => _image;
    }
}
