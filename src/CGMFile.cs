using System;
using System.Collections.Generic;
using System.Linq;
using codessentials.CGM.Classes;
using codessentials.CGM.Commands;

namespace codessentials.CGM
{
    /// <summary>
    /// Base class for CGM files
    /// </summary>
    public abstract class CgmFile
    {
        protected List<Command> _commands = new List<Command>();
        protected List<Message> _messages = new List<Message>();
        private ColourTable _colorTable;
        private readonly Dictionary<string, bool> _foundFigureItems = new Dictionary<string, bool>();
        private readonly Dictionary<string, bool> _foundConsumableNumbers = new Dictionary<string, bool>();
        private List<TextCommand> _figureItems = null;
        private List<CgmRectangle> _rectangles = null;
        private List<TextCommand> _torqueTextCandidates = null;

        protected CgmFile()
        {
            ResetMetaDefinitions();
        }

        /// <summary>
        /// Reads a binary CGM file from disk.
        /// </summary>
        /// <param name="filename">Path to the CGM file.</param>
        /// <returns></returns>
        public static BinaryCgmFile ReadBinary(string filename)
        {
            return new BinaryCgmFile(filename);
        }

        /// <summary>
        /// Read a binary CGM file from stream.
        /// </summary>
        /// <param name="data">The stream containing binary CGM data.</param>
        /// <param name="name">Name of the CGM.</param>
        /// <returns></returns>
        public static BinaryCgmFile ReadBinary(System.IO.Stream data, string name = "stream")
        {
            return new BinaryCgmFile(data, name);
        }

        /// <summary>
        /// Copies the current meta information
        /// </summary>
        /// <param name="file">The cgm file to copy from</param>
        public void ApplyValues(CgmFile file)
        {
            Name = file.Name;

            _commands.AddRange(file.Commands);

            ResetMetaDefinitions();
        }

        /// <summary>
        /// Resets settings like VDCRealPrecision or ColourModel
        /// </summary>
        public void ResetMetaDefinitions()
        {
            ColourIndexPrecision = 8;
            ColourPrecision = 8;
            ColourModel = CGM.Commands.ColourModel.Model.RGB;
            ColourSelectionMode = CGM.Commands.ColourSelectionMode.Type.INDEXED;
            ColourValueExtentMinimumColorValueRGB = new int[] { 0, 0, 0 };
            ColourValueExtentMaximumColorValueRGB = new int[] { 255, 255, 255 };
            DeviceViewportSpecificationMode = CGM.Commands.DeviceViewportSpecificationMode.Mode.FRACTION;
            EdgeWidthSpecificationMode = SpecificationMode.ABS;
            IndexPrecision = 16;
            IntegerPrecision = 16;
            NamePrecision = 16;
            LineWidthSpecificationMode = SpecificationMode.ABS;
            MarkerSizeSpecificationMode = SpecificationMode.ABS;
            InteriorStyleSpecificationMode = SpecificationMode.ABS;
            RealPrecision = Precision.Fixed_32;
            RealPrecisionProcessed = false;
            RestrictedTextType = CGM.Commands.RestrictedTextType.Type.BASIC;
            VDCIntegerPrecision = 16;
            VDCRealPrecision = Precision.Fixed_32;
            VDCType = CGM.Commands.VDCType.Type.Integer;
        }

        /// <summary>
        /// Gets the name of the file
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets the current reading colour index precision
        /// </summary>
        public int ColourIndexPrecision { get; set; }

        /// <summary>
        /// Gets or sets the current reading colour precision
        /// </summary>
        public int ColourPrecision { get; set; }

        /// <summary>
        /// Gets or sets the current reading colour model
        /// </summary>
        public ColourModel.Model ColourModel { get; set; }

        /// <summary>
        /// Gets or sets the current reading colour selecion mode
        /// </summary>
        public ColourSelectionMode.Type ColourSelectionMode { get; set; }

        /// <summary>
        /// Gets or sets the current reading MinimumColorValueRGB
        /// </summary>
        public int[] ColourValueExtentMinimumColorValueRGB { get; set; }

        /// <summary>
        /// Gets or sets the current reading MaximumColorValueRGB
        /// </summary>
        public int[] ColourValueExtentMaximumColorValueRGB { get; set; }

        /// <summary>
        /// Gets or sets the current reading DeviceViewportSpecificationMode
        /// </summary>
        public DeviceViewportSpecificationMode.Mode DeviceViewportSpecificationMode { get; set; }

        /// <summary>
        /// Gets or sets the current reading EdgeWidthSpecificationMode
        /// </summary>
        public SpecificationMode EdgeWidthSpecificationMode { get; set; }

        /// <summary>
        /// Gets or sets the current reading Index Precision
        /// </summary>
        public int IndexPrecision { get; set; }

        /// <summary>
        /// Gets or sets the current reading integer Precision
        /// </summary>
        public int IntegerPrecision { get; set; }

        /// <summary>
        /// Gets or sets the current reading name Precision
        /// </summary>
        public int NamePrecision { get; set; }

        /// <summary>
        /// Gets or sets the current reading vdc integer Precision
        /// </summary>
        public int VDCIntegerPrecision { get; set; }

        /// <summary>
        /// Gets or sets the current reading vdc real Precision
        /// </summary>
        public Precision VDCRealPrecision { get; set; }

        /// <summary>
        /// Gets or sets the current reading real Precision
        /// </summary>
        public Precision RealPrecision { get; set; }

        /// <summary>
        /// Gets or sets the current reading real Precision processed flag
        /// </summary>
        public bool RealPrecisionProcessed { get; set; }

        /// <summary>
        ///  Gets or sets the current reading LineWidthSpecificationMode
        /// </summary>
        public SpecificationMode LineWidthSpecificationMode { get; set; }

        /// <summary>
        /// Gets or sets the current reading MarkerSizeSpecificationMode
        /// </summary>
        public SpecificationMode MarkerSizeSpecificationMode { get; set; }

        /// <summary>
        ///  Gets or sets the current reading interior style SpecificationMode
        /// </summary>
        public SpecificationMode InteriorStyleSpecificationMode { get; set; }

        /// <summary>
        /// Gets or sets the current reading RestrictedTextType
        /// </summary>
        public RestrictedTextType.Type RestrictedTextType { get; set; }

        /// <summary>
        /// Gets or sets the current reading VDC Type
        /// </summary>
        public VDCType.Type VDCType { get; set; }

        /// <summary>
        /// The read CGM commands
        /// </summary>
        public List<Command> Commands
        {
            get { return _commands; }
        }

        /// <summary>
        /// Determines whether any text element equals the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public bool ContainsTextElement(string text)
        {
            return GetAllTextCommands().Any(t => t.Text == text);
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <returns></returns>
        public string GetMetaTitle()
        {
            var titleCommand = Commands.SingleOrDefault(c => c.ElementClass == ClassCode.DelimiterElement && c.ElementId == 1) as BeginMetafile;

            return titleCommand?.FileName;
        }

        /// <summary>
        /// Gets the title of the illustration.
        /// </summary>
        /// <returns></returns>
        public string GetGraphicName()
        {
            var names = GetGraphicNames();

            if (!names.Any())
                throw new ArgumentException($"No graphic name text found in CGM ({Name})!");

            if (names.Count() > 1)
                throw new ArgumentException($"More than one graphic name texts found in CGM ({Name})!");

            return names.First().Text;
        }

        /// <summary>
        /// Gets all texts of the figure items.
        /// </summary>
        /// <returns></returns>
        public List<string> GetFigureItemTexts(bool ignoreColor)
        {
            return GetAllFigureItems(ignoreColor).Select(c => c.Text).Distinct().ToList();
        }

        /// <summary>
        /// Determines whether CGM contains a specific figure item text.
        /// </summary>
        /// <param name="textToCheck">The text to check.</param>
        /// <returns></returns>
        public bool ContainsFigureItemText(string textToCheck)
        {
            if (_foundFigureItems.TryGetValue(textToCheck, out var result))
                return result;

            result = GetAllFigureItems(true).Any(c => c.Text == textToCheck);
            _foundFigureItems[textToCheck] = result;

            return result;
        }

        /// <summary>
        /// Determines whether CGM contains a specific consumable number text
        /// </summary>
        /// <param name="textToCheck">The text to check.</param>
        /// <returns></returns>
        public bool ContainsConsumableNumber(string textToCheck)
        {
            if (_foundConsumableNumbers.TryGetValue(textToCheck, out var result))
                return result;

            result = GetConsumableNumberCandidates().Any(c => c.Text == textToCheck);
            _foundConsumableNumbers[textToCheck] = result;

            return result;
        }

        /// <summary>
        /// Determines whether the torque text ("20 Nm (177 lb-in)" exists nearby the figure item)
        /// </summary>
        /// <param name="torqueText">The torque text.</param>
        /// <param name="figureItemNumber">The figure item number.</param>
        /// <returns>
        ///   <c>true</c> if [contains torque text to fig item] [the specified torque text]; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsTorqueTextToFigItem(string torqueText, string figureItemNumber)
        {
            torqueText = torqueText.ReplaceIgnoreCase("lb-in", "Ibf.in");
            torqueText = torqueText.Replace(" and ", "-");
            torqueText = torqueText.Replace(" to ", "-");

            var figureItems = GetAllFigureItems(true).Where(c => c.Text == figureItemNumber);

            if (figureItems.Any())
            {
                var firstText = torqueText.Substring(0, torqueText.IndexOf('(')).Trim();
                var secondText = torqueText.Substring(torqueText.IndexOf('(')).Trim();
                var torqueTexts = GetTorqueTextCandiates();

                var firstLineCommands = torqueTexts.Where(c => c.Text.EqualsIgnoreCase(firstText));

                foreach (var textCommand in firstLineCommands)
                {
                    var secondLineCommand = torqueTexts.FirstOrDefault(c => c.Text.EqualsIgnoreCase(secondText) &&
                        GeometryRecognitionEngine.IsNearBy(textCommand.Position, c.Position, 15)
                        );

                    if (secondLineCommand != null && figureItems.Any(f => GeometryRecognitionEngine.IsNearBy(textCommand.Position, f.Position, 40)))
                        return true;
                }
            }

            return false;
        }

        private IEnumerable<TextCommand> GetAllFigureItems(bool ignoreColor)
        {
            if (_figureItems == null)
            {
                var textInfos = GetAllTextCommands().Where(c => int.TryParse(c.Text, out _)).Select(t => GetInformation(t));

                if (!ignoreColor)
                    // figure items have to be red
                    textInfos = textInfos.Where(t => GetColor(t.ColorCommand).EqualsColor(System.Drawing.Color.Red));

                // size of 10 pt == 2.5356
                textInfos = textInfos.Where(t => Math.Round(t.HeightCommand.Height, 4) == 2.5356);

                _figureItems = textInfos.Select(t => t.TextCommand).ToList();
            }

            return _figureItems;
        }

        private IEnumerable<TextCommand> GetGraphicNames()
        {
            var textInfos = GetAllTextCommands().Where(c => IsWrittenDownToUp(c)).Select(t => GetInformation(t));

            textInfos = textInfos.Where(t => Math.Round(t.HeightCommand.Height, 4) == 1.5214);

            return textInfos.Select(t => t.TextCommand).ToList();
        }

        private IEnumerable<TextCommand> GetConsumableNumberCandidates()
        {
            var textInfos = GetAllTextCommands().Select(t => GetInformation(t));

            textInfos = textInfos.Where(t => Math.Round(t.HeightCommand.Height, 4) == 2.5356);

            return textInfos.Select(t => t.TextCommand).ToList();
        }

        private IEnumerable<TextCommand> GetTorqueTextCandiates()
        {
            if (_torqueTextCandidates == null)
            {
                var rectangles = GetRectangles();

                // text must be located within an rectangle
                var textInfos = GetAllTextCommands().Where(t => rectangles.Any(r => r.Contains(t.Position, 20)));

                _torqueTextCandidates = textInfos.ToList();
            }

            return _torqueTextCandidates;
        }

        private IEnumerable<TextCommand> GetAllTextCommands()
        {
            return Commands.Where(c => c.ElementClass == ClassCode.GraphicalPrimitiveElements && (c.ElementId == 4 || c.ElementId == 5)).Cast<TextCommand>();
        }

        public TextInformation GetInformation(TextCommand command)
        {
            var result = new TextInformation()
            {
                TextCommand = command
            };

            var indexOfTextCommand = Commands.IndexOf(command);

            for (var i = indexOfTextCommand; i >= 0; i--)
            {
                var currentCommand = Commands[i];

                if (currentCommand is TextColour colorCommand && result.ColorCommand == null)
                {
                    result.ColorCommand = colorCommand;
                    continue;
                }

                if (currentCommand is CharacterHeight heightCommand && result.HeightCommand == null)
                {
                    result.HeightCommand = heightCommand;
                }

                if (result.HeightCommand != null && result.ColorCommand != null)
                    break;
            }

            return result;
        }

        private System.Drawing.Color GetColor(ColourCommand command)
        {
            if (ColourSelectionMode == CGM.Commands.ColourSelectionMode.Type.INDEXED)
            {
                var table = GetColorTable();

                return table.GetColor(command.Color.ColorIndex);
            }
            else
            {
                return command.Color.Color;
            }
        }

        private ColourTable GetColorTable()
        {
            if (_colorTable != null || ColourSelectionMode != CGM.Commands.ColourSelectionMode.Type.INDEXED)
                return _colorTable;

            _colorTable = Commands.SingleOrDefault(c => c.ElementClass == ClassCode.AttributeElements && c.ElementId == 34) as ColourTable;

            return _colorTable;
        }

        /// <summary>
        /// Determines whether text is rotated 90 dec counterwise
        /// </summary>
        /// <param name="command">The command.</param>
        public bool IsWrittenDownToUp(TextCommand command)
        {
            var indexOfTextCommand = Commands.IndexOf(command);

            for (var i = indexOfTextCommand; i >= Math.Max(0, indexOfTextCommand - 10); i--)
            {
                var currentCommand = Commands[i];

                if (currentCommand is CharacterOrientation orientationCommand)
                {
                    return orientationCommand.IsDownToUp();
                }
            }

            return false;
        }

        /// <summary>
        /// Gets all found rectangles.
        /// </summary>
        /// <returns></returns>
        public List<CgmRectangle> GetRectangles()
        {
            if (_rectangles == null)
                _rectangles = GeometryRecognitionEngine.GetRectangles(this);

            return _rectangles;
        }

        /// <summary>
        /// Any messages occured while reading or writing the file
        /// </summary>
        public IEnumerable<Message> Messages
        {
            get { return _messages; }
        }
    }
}
