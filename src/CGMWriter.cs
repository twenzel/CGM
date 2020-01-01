using codessentials.CGM.Commands;
using System;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM
{
    /// <summary>
    /// Write to simplify the creation of CGM files.
    /// </summary>
    public class CGMWriter
    {
        private CGMFile _cgm;

        /// <summary>
        /// Initializes the writer and creates a new binary or clear text file
        /// </summary>
        /// <param name="format">Binary or clear text</param>
        /// <param name="graphicName">Optional. The name of the graphic. Will be written to meta data.</param>
        /// <param name="version">Version of the graphic. Will be written to meta data.</param>
        public CGMWriter(FileFormat format, string graphicName = default, int version = 1)
        {
            if (format == FileFormat.Binary)
                _cgm = new BinaryCGMFile();
            else
                _cgm = new ClearTextCGMFile();

            if (!string.IsNullOrEmpty(graphicName))
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

        public void SetVDCType(VDCType.Type type)
        {
            AddCommand(new VDCType(_cgm, type));
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
            if (_cgm is BinaryCGMFile binary)
                return binary.GetContent();
            else if (_cgm is ClearTextCGMFile clearText)
            {
                using (var stream = new MemoryStream())
                {
                    clearText.WriteFile(stream);

                    return stream.ToArray();
                }
            }
            else
                throw new InvalidOperationException("Unknown CGM format!");
        }
    }
}
