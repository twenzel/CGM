using codessentials.CGM.Classes;
using System.Text;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=19
    /// </remarks>
    public class BeginTileArray : Command
    {
        public CGMPoint Position { get; private set; }
        public int CellPathDirection { get; private set; }
        public int LineProgressionDirection { get; private set; }
        public int NumberTilesInPathDirection { get; private set; }
        public int NumberTilesInLineDirection { get; private set; }
        public int NumberCellsPerTileInPathDirection { get; private set; }
        public int NumberCellsPerTileInLineDirection { get; private set; }
        public double CellSizeInPathDirection { get; private set; }
        public double CellSizeInLineDirection { get; private set; }
        public int ImageOffsetInPathDirection { get; private set; }
        public int ImageOffsetInLineDirection { get; private set; }
        public int NumberCellsInPathDirection { get; private set; }
        public int NumberCellsInLineDirection { get; private set; }

        public BeginTileArray(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 19, container))
        {

        }

        public BeginTileArray(CGMFile container, CGMPoint position, int cellPathDirection, int lineProgressionDirection, int numberTilesInPathDirection,
            int numberTilesInLineDirection, int numberCellsPerTileInPathDirection, int numberCellsPerTileInLineDirection, double cellSizeInPathDirection,
            double cellSizeInLineDirection, int imageOffsetinPathDirection, int imageOffsetInLineDirection, int numberCellsInPathDirection, int numberCellsInLineDirection)
            : this(container)
        {
            Position = position;
            CellPathDirection = cellPathDirection;
            LineProgressionDirection = lineProgressionDirection;
            NumberTilesInPathDirection = numberTilesInPathDirection;
            NumberTilesInLineDirection = numberTilesInLineDirection;
            NumberCellsPerTileInPathDirection = numberCellsPerTileInPathDirection;
            NumberCellsPerTileInLineDirection = numberCellsPerTileInLineDirection;
            CellSizeInPathDirection = cellSizeInPathDirection;
            CellSizeInLineDirection = cellSizeInLineDirection;
            ImageOffsetInPathDirection = imageOffsetinPathDirection;
            ImageOffsetInLineDirection = imageOffsetInLineDirection;
            NumberCellsInPathDirection = numberCellsInPathDirection;
            NumberCellsInLineDirection = numberCellsInLineDirection;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Position = reader.ReadPoint();
            CellPathDirection = reader.ReadEnum();
            LineProgressionDirection = reader.ReadEnum();
            NumberTilesInPathDirection = reader.ReadInt();
            NumberTilesInLineDirection = reader.ReadInt();
            NumberCellsPerTileInPathDirection = reader.ReadInt();
            NumberCellsPerTileInLineDirection = reader.ReadInt();
            CellSizeInPathDirection = reader.ReadReal();
            CellSizeInLineDirection = reader.ReadReal();
            ImageOffsetInPathDirection = reader.ReadInt();
            ImageOffsetInLineDirection = reader.ReadInt();
            NumberCellsInPathDirection = reader.ReadInt();
            NumberCellsInLineDirection = reader.ReadInt();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WritePoint(Position);
            writer.WriteEnum(CellPathDirection);
            writer.WriteEnum(LineProgressionDirection);
            writer.WriteInt(NumberTilesInPathDirection);
            writer.WriteInt(NumberTilesInLineDirection);
            writer.WriteInt(NumberCellsPerTileInPathDirection);
            writer.WriteInt(NumberCellsPerTileInLineDirection);
            writer.WriteReal(CellSizeInPathDirection);
            writer.WriteReal(CellSizeInLineDirection);
            writer.WriteInt(ImageOffsetInPathDirection);
            writer.WriteInt(ImageOffsetInLineDirection);
            writer.WriteInt(NumberCellsInPathDirection);
            writer.WriteInt(NumberCellsInLineDirection);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" BEGTILEARRAY {WritePoint(Position)} {GetDirection(CellPathDirection)}");
            writer.Write($" {GetLineDirection(LineProgressionDirection)}");
            writer.Write($" {WriteInt(NumberTilesInPathDirection)}");
            writer.Write($" {WriteInt(NumberTilesInLineDirection)}");
            writer.Write($" {WriteInt(NumberCellsPerTileInPathDirection)}");
            writer.Write($" {WriteInt(NumberCellsPerTileInLineDirection)}");
            writer.Write($" {WriteReal(CellSizeInPathDirection)}");
            writer.Write($" {WriteReal(CellSizeInLineDirection)}");
            writer.Write($" {WriteInt(ImageOffsetInPathDirection)}");
            writer.Write($" {WriteInt(ImageOffsetInLineDirection)}");
            writer.Write($" {WriteInt(NumberCellsInPathDirection)}");
            writer.Write($" {WriteInt(NumberCellsInLineDirection)}");
            writer.WriteLine(";");
        }

        private string GetLineDirection(int lineProgressionDirection)
        {
            if (lineProgressionDirection == 1)
                return "270";
            else
                return "90";
        }

        private string GetDirection(int cellPathDirection)
        {
            if (cellPathDirection == 1)
                return "90";
            else if (cellPathDirection == 2)
                return "180";
            else if (cellPathDirection == 3)
                return "270";
            else
                return "0";
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("BeginTileArray [position=");
            builder.Append(Position);
            builder.Append(", cellPathDirection=");
            builder.Append(CellPathDirection);
            builder.Append(", lineProgressionDirection=");
            builder.Append(LineProgressionDirection);
            builder.Append(", nTilesInPathDirection=");
            builder.Append(NumberTilesInPathDirection);
            builder.Append(", nTilesInLineDirection=");
            builder.Append(NumberTilesInLineDirection);
            builder.Append(", nCellsPerTileInPathDirection=");
            builder.Append(NumberCellsPerTileInPathDirection);
            builder.Append(", nCellsPerTileInLineDirection=");
            builder.Append(NumberCellsPerTileInLineDirection);
            builder.Append(", cellSizeInPathDirection=");
            builder.Append(CellSizeInPathDirection);
            builder.Append(", cellSizeInLineDirection=");
            builder.Append(CellSizeInLineDirection);
            builder.Append(", imageOffsetInPathDirection=");
            builder.Append(ImageOffsetInPathDirection);
            builder.Append(", imageOffsetInLineDirection=");
            builder.Append(ImageOffsetInLineDirection);
            builder.Append(", nCellsInPathDirection=");
            builder.Append(NumberCellsInPathDirection);
            builder.Append(", nCellsInLineDirection=");
            builder.Append(NumberCellsInLineDirection);
            builder.Append("]");
            return builder.ToString();
        }
    }
}