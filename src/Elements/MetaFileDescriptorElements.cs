using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using codessentials.CGM.Commands;

namespace codessentials.CGM.Elements
{
    public static class MetaFileDescriptorElements
    {
        public static Command CreateCommand(int elementId, int elementClass, CGMFile container)
        {
            switch ((MetaFileDescriptorElement)elementId)
            {
                case MetaFileDescriptorElement.METAFILE_VERSION: 
                    return new MetafileVersion(container);
                case MetaFileDescriptorElement.METAFILE_DESCRIPTION:
                    return new MetafileDescription(container);
                case MetaFileDescriptorElement.VDC_TYPE: 
                    return new VDCType(container);
                case MetaFileDescriptorElement.INTEGER_PRECISION: 
                    return new IntegerPrecision(container);
                case MetaFileDescriptorElement.REAL_PRECISION:
                    return new RealPrecision(container);
                case MetaFileDescriptorElement.INDEX_PRECISION: 
                    return new IndexPrecision(container);
                case MetaFileDescriptorElement.COLOUR_PRECISION:
                    return new ColourPrecision(container);
                case MetaFileDescriptorElement.COLOUR_INDEX_PRECISION: 
                    return new ColourIndexPrecision(container);
                case MetaFileDescriptorElement.MAXIMUM_COLOUR_INDEX: 
                    return new MaximumColourIndex(container);
                case MetaFileDescriptorElement.COLOUR_VALUE_EXTENT: 
                    return new ColourValueExtent(container);
                case MetaFileDescriptorElement.METAFILE_ELEMENT_LIST:
                    return new MetafileElementList(container);
                case MetaFileDescriptorElement.METAFILE_DEFAULTS_REPLACEMENT: 
                    return new MetafileDefaultsReplacement(container);
                case MetaFileDescriptorElement.FONT_LIST: 
                    return new FontList(container);
                case MetaFileDescriptorElement.CHARACTER_SET_LIST: 
                    return new CharacterSetList(container);
                case MetaFileDescriptorElement.CHARACTER_CODING_ANNOUNCER: 
                    return new CharacterCodingAnnouncer(container);
                case MetaFileDescriptorElement.NAME_PRECISION: 
                    return new NamePrecision(container);
                case MetaFileDescriptorElement.MAXIMUM_VDC_EXTENT:
                    return new MaximumVDCExtent(container);                
                case MetaFileDescriptorElement.SEGMENT_PRIORITY_EXTENT:
                    return new SegmentPriorityExtend(container);
                case MetaFileDescriptorElement.COLOUR_MODEL: 
                    return new ColourModel(container);
                case MetaFileDescriptorElement.COLOUR_CALIBRATION: 
                    return new ColourCalibration(container);
                case MetaFileDescriptorElement.FONT_PROPERTIES: 
                    return new FontProperties(container);
                case MetaFileDescriptorElement.GLYPH_MAPPING:
                    return new GlyphMapping(container);
                case MetaFileDescriptorElement.SYMBOL_LIBRARY_LIST: 
                    return new SymbolLibraryList(container);
                case MetaFileDescriptorElement.PICTURE_DIRECTORY: 
                    return new PictureDirectory(container);
                default:                    
                    return new UnknownCommand(elementId, elementClass, container);
            }
        }
    }
}

