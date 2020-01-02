using System;
using System.Collections.Generic;
using System.IO;
using codessentials.CGM.Commands;

namespace codessentials.CGM
{
    /// <summary>
    /// Write to simplify the creation of CGM files.
    /// </summary>
    public class CgmWriter
    {
        private readonly CgmFile _cgm;

        /// <summary>
        /// Initializes the writer and creates a new binary or clear text file
        /// </summary>
        /// <param name="format">Binary or clear text</param>
        /// <param name="graphicName">Optional. The name of the graphic. Will be written to meta data.</param>
        /// <param name="version">Version of the graphic. Will be written to meta data.</param>
        public CgmWriter(FileFormat format, string graphicName = default, int version = 1)
        {
            if (format == FileFormat.Binary)
                _cgm = new BinaryCgmFile();
            else
                _cgm = new ClearTextCgmFile();

            if (graphicName != null)
                AddCommand(new BeginMetafile(_cgm, graphicName));

            AddCommand(new MetafileVersion(_cgm, version));
        }

        public void SetDescription(string description)
        {
            AddCommand(new MetafileDescription(_cgm, description));
        }

        public void SetElementList(string element)
        {
            AddCommand(new MetafileElementList(_cgm, element));
        }

        public void SetFontList(string[] fonts)
        {
            AddCommand(new FontList(_cgm, fonts));
        }

        public void SetCharacterSetList(KeyValuePair<CharacterSetList.Type, string>[] items)
        {
            AddCommand(new CharacterSetList(_cgm, items));
        }

        public void SetVDCType(VdcType.Type type)
        {
            AddCommand(new VdcType(_cgm, type));
        }

        public void Finish()
        {
            AddCommand(new EndMetafile(_cgm));
        }

        public void AddCommand(Command command)
        {
            _cgm.Commands.Add(command);
        }

        public byte[] GetContent()
        {
            if (_cgm is BinaryCgmFile binary)
                return binary.GetContent();
            else if (_cgm is ClearTextCgmFile clearText)
            {
                using var stream = new MemoryStream();
                clearText.WriteFile(stream);

                return stream.ToArray();
            }
            else
            {
                throw new InvalidOperationException("Unknown CGM format!");
            }
        }
    }
}
