using codessentials.CGM.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=19
    /// </remarks>
    public class BeginTileArray : Command
    {
        private CGMPoint _position;
        private int _cellPathDirection;
        private int _lineProgressionDirection;
        private int _nTilesInPathDirection;
        private int _nTilesInLineDirection;
        private int _nCellsPerTileInPathDirection;
        private int _nCellsPerTileInLineDirection;
        private double _cellSizeInPathDirection;
        private double _cellSizeInLineDirection;
        private int _imageOffsetInPathDirection;
        private int _imageOffsetInLineDirection;
        private int _nCellsInPathDirection;
        private int _nCellsInLineDirection;

        public BeginTileArray(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 19, container))
        {
            
        }

        public BeginTileArray(CGMFile container, CGMPoint position, int cellPathDirection, int lineProgressionDirection, int numberTilesInPathDirection,
            int numberTilesInLineDirection, int numberCellsPerTileInPathDirection, int numberCellsPerTileInLineDirection, double cellSizeInPathDirection,
            double cellSizeInLineDirection, int imageOffsetinPathDirection, int imageOffsetInLineDirection, int numberCellsInPathDirection, int numberCellsInLineDirection)
            : this(container)
        {
            _position = position;
            _cellPathDirection = cellPathDirection;
            _lineProgressionDirection = lineProgressionDirection;
            _nTilesInPathDirection = numberTilesInPathDirection;
            _nTilesInLineDirection = numberTilesInLineDirection;
            _nCellsPerTileInPathDirection = numberCellsPerTileInPathDirection;
            _nCellsPerTileInLineDirection = numberCellsPerTileInLineDirection;
            _cellSizeInPathDirection = cellSizeInPathDirection;
            _cellSizeInLineDirection = cellSizeInLineDirection;
            _imageOffsetInPathDirection = imageOffsetinPathDirection;
            _imageOffsetInLineDirection = imageOffsetInLineDirection;
            _nCellsInPathDirection = numberCellsInPathDirection;
            _nCellsInLineDirection = numberCellsInLineDirection;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _position = reader.ReadPoint();
            _cellPathDirection = reader.ReadEnum();
            _lineProgressionDirection = reader.ReadEnum();
            _nTilesInPathDirection = reader.ReadInt();
            _nTilesInLineDirection = reader.ReadInt();
            _nCellsPerTileInPathDirection = reader.ReadInt();
            _nCellsPerTileInLineDirection = reader.ReadInt();
            _cellSizeInPathDirection = reader.ReadReal();
            _cellSizeInLineDirection = reader.ReadReal();
            _imageOffsetInPathDirection = reader.ReadInt();
            _imageOffsetInLineDirection = reader.ReadInt();
            _nCellsInPathDirection = reader.ReadInt();
            _nCellsInLineDirection = reader.ReadInt();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WritePoint(_position);
            writer.WriteEnum(_cellPathDirection);
            writer.WriteEnum(_lineProgressionDirection);
            writer.WriteInt(_nTilesInPathDirection);
            writer.WriteInt(_nTilesInLineDirection);
            writer.WriteInt(_nCellsPerTileInPathDirection);
            writer.WriteInt(_nCellsPerTileInLineDirection);
            writer.WriteReal(_cellSizeInPathDirection);
            writer.WriteReal(_cellSizeInLineDirection);
            writer.WriteInt(_imageOffsetInPathDirection);
            writer.WriteInt(_imageOffsetInLineDirection);
            writer.WriteInt(_nCellsInPathDirection);
            writer.WriteInt(_nCellsInLineDirection);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" BEGTILEARRAY {WritePoint(_position)} {GetDirection(_cellPathDirection)}");
            writer.Write($" {GetLineDirection(_lineProgressionDirection)}");
            writer.Write($" {WriteInt(_nTilesInPathDirection)}");
            writer.Write($" {WriteInt(_nTilesInLineDirection)}");
            writer.Write($" {WriteInt(_nCellsPerTileInPathDirection)}");
            writer.Write($" {WriteInt(_nCellsPerTileInLineDirection)}");
            writer.Write($" {WriteReal(_cellSizeInPathDirection)}");
            writer.Write($" {WriteReal(_cellSizeInLineDirection)}");
            writer.Write($" {WriteInt(_imageOffsetInPathDirection)}");
            writer.Write($" {WriteInt(_imageOffsetInLineDirection)}");
            writer.Write($" {WriteInt(_nCellsInPathDirection)}");
            writer.Write($" {WriteInt(_nCellsInLineDirection)}");
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
            StringBuilder builder = new StringBuilder();
            builder.Append("BeginTileArray [position=");
            builder.Append(_position);
            builder.Append(", cellPathDirection=");
            builder.Append(_cellPathDirection);
            builder.Append(", lineProgressionDirection=");
            builder.Append(_lineProgressionDirection);
            builder.Append(", nTilesInPathDirection=");
            builder.Append(_nTilesInPathDirection);
            builder.Append(", nTilesInLineDirection=");
            builder.Append(_nTilesInLineDirection);
            builder.Append(", nCellsPerTileInPathDirection=");
            builder.Append(_nCellsPerTileInPathDirection);
            builder.Append(", nCellsPerTileInLineDirection=");
            builder.Append(_nCellsPerTileInLineDirection);
            builder.Append(", cellSizeInPathDirection=");
            builder.Append(_cellSizeInPathDirection);
            builder.Append(", cellSizeInLineDirection=");
            builder.Append(_cellSizeInLineDirection);
            builder.Append(", imageOffsetInPathDirection=");
            builder.Append(_imageOffsetInPathDirection);
            builder.Append(", imageOffsetInLineDirection=");
            builder.Append(_imageOffsetInLineDirection);
            builder.Append(", nCellsInPathDirection=");
            builder.Append(_nCellsInPathDirection);
            builder.Append(", nCellsInLineDirection=");
            builder.Append(_nCellsInLineDirection);
            builder.Append("]");
            return builder.ToString();
        }

        public CGMPoint Position => _position;
        public int CellPathDirection => _cellPathDirection;
        public int LineProgressionDirection => _lineProgressionDirection;
        public int NumberTilesInPathDirection => _nTilesInPathDirection;
        public int NumberTilesInLineDirection => _nTilesInLineDirection;
        public int NumberCellsPerTileInPathDirection => _nCellsPerTileInPathDirection;
        public int NumberCellsPerTileInLineDirection => _nCellsPerTileInLineDirection;
        public double CellSizeInPathDirection => _cellSizeInPathDirection;
        public double CellSizeInLineDirection => _cellSizeInLineDirection;
        public int ImageOffsetInPathDirection => _imageOffsetInPathDirection;
        public int ImageOffsetInLineDirection => _imageOffsetInLineDirection;
        public int NumberCellsInPathDirection => _nCellsInPathDirection;
        public int NumberCellsInLineDirection => _nCellsInLineDirection;
    }
}