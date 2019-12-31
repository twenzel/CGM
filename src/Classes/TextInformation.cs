using codessentials.CGM.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Classes
{
    /// <summary>
    /// Information bundle of text elements
    /// </summary>
    [DebuggerDisplay("{TextCommand.Text} - {ColorCommand.Color} - {HeightCommand.Height}")]
    public class TextInformation
    {
        /// <summary>
        /// Gets or sets the text command.
        /// </summary>
        /// <value>
        /// The text command.
        /// </value>
        public TextCommand TextCommand { get; set; }

        /// <summary>
        /// Gets or sets the color command.
        /// </summary>
        /// <value>
        /// The color command.
        /// </value>
        public TextColour ColorCommand { get; set; }

        /// <summary>
        /// Gets or sets the height command.
        /// </summary>
        /// <value>
        /// The height command.
        /// </value>
        public CharacterHeight HeightCommand { get; set; }
    }
}
