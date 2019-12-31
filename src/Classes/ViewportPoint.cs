using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Classes
{
    /// <summary>
    /// The abstract parameter type VC is a single value; a viewport point, VP, is an ordered pair of VC
    /// </summary>
    public class ViewportPoint
    {
        public VC FirstPoint { get; set; }
        public VC SecondPoint { get; set; }        
    }    
}
