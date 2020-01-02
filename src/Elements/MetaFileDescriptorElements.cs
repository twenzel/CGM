using codessentials.CGM.Commands;

namespace codessentials.CGM.Elements
{
    public static class MetaFileDescriptorElements
    {
        public static Command CreateCommand(int elementId, int elementClass, CgmFile container)
        {
            return ((MetaFileDescriptorElement)elementId) switch
            {
                MetaFileDescriptorElement.METAFILE_VERSION => new MetafileVersion(container),
                MetaFileDescriptorElement.METAFILE_DESCRIPTION => new MetafileDescription(container),
                MetaFileDescriptorElement.VDC_TYPE => new VdcType(container),
                MetaFileDescriptorElement.INTEGER_PRECISION => new IntegerPrecision(container),
                MetaFileDescriptorElement.REAL_PRECISION => new RealPrecision(container),
                MetaFileDescriptorElement.INDEX_PRECISION => new IndexPrecision(container),
                MetaFileDescriptorElement.COLOUR_PRECISION => new ColourPrecision(container),
                MetaFileDescriptorElement.COLOUR_INDEX_PRECISION => new ColourIndexPrecision(container),
                MetaFileDescriptorElement.MAXIMUM_COLOUR_INDEX => new MaximumColourIndex(container),
                MetaFileDescriptorElement.COLOUR_VALUE_EXTENT => new ColourValueExtent(container),
                MetaFileDescriptorElement.METAFILE_ELEMENT_LIST => new MetafileElementList(container),
                MetaFileDescriptorElement.METAFILE_DEFAULTS_REPLACEMENT => new MetafileDefaultsReplacement(container),
                MetaFileDescriptorElement.FONT_LIST => new FontList(container),
                MetaFileDescriptorElement.CHARACTER_SET_LIST => new CharacterSetList(container),
                MetaFileDescriptorElement.CHARACTER_CODING_ANNOUNCER => new CharacterCodingAnnouncer(container),
                MetaFileDescriptorElement.NAME_PRECISION => new NamePrecision(container),
                MetaFileDescriptorElement.MAXIMUM_VDC_EXTENT => new MaximumVdcExtent(container),
                MetaFileDescriptorElement.SEGMENT_PRIORITY_EXTENT => new SegmentPriorityExtend(container),
                MetaFileDescriptorElement.COLOUR_MODEL => new ColourModel(container),
                MetaFileDescriptorElement.COLOUR_CALIBRATION => new ColourCalibration(container),
                MetaFileDescriptorElement.FONT_PROPERTIES => new FontProperties(container),
                MetaFileDescriptorElement.GLYPH_MAPPING => new GlyphMapping(container),
                MetaFileDescriptorElement.SYMBOL_LIBRARY_LIST => new SymbolLibraryList(container),
                MetaFileDescriptorElement.PICTURE_DIRECTORY => new PictureDirectory(container),
                _ => new UnknownCommand(elementId, elementClass, container),
            };
        }
    }
}

