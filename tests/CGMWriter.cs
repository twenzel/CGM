using codessentials.CGM.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Tests
{
    class CGMWriter
    {
        private BinaryCGMFile _cgm;

        public void NewBinaryCgm(string graphicName)
        {
            _cgm = new BinaryCGMFile();
            AddCommand(new BeginMetafile(_cgm, graphicName));
            AddCommand(new MetafileVersion(_cgm, 1));
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
            return _cgm.GetContent();
        }
    }
}
