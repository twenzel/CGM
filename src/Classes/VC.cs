namespace codessentials.CGM.Classes
{
    /// <summary>
    /// Represents the abstract VC parameter type
    /// </summary>
    public class VC
    {
        // The abstract parameter type VC, a single VC value, is either a real or an integer, depending on the declaration of the picture descriptor 
        // element DEVICE VIEWPORT SPECIFICATION MODE. When DEVICE VIEWPORT SPECIFICATION MODE is 'fraction of display surface', the value is real.
        // When DEVICE VIEWPORT SPECIFICATION MODE is 'millimetres with scale factor' or 'physical device coordinates', the value is integer.
        // Subsequent tables use a single set of values, VC, BVC and VCR, recognizing that they are computed differently depending on DEVICE VIEWPORT SPECIFICATION MODE

        public int ValueInt { get; set; }
        public double ValueReal { get; set; }
    }
}
