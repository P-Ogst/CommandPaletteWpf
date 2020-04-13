using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPaletteLibrary
{
    interface IRangePaletteParameter : IPaletteParameter
    {
        public object Min { get; }
        public object Max { get; }
    }
}
