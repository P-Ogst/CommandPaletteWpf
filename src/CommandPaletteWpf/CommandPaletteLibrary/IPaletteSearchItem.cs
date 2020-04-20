using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPaletteLibrary
{
    public interface IPaletteSearchItem
    {
        object Value { get; }
        string Name { get; }
        string Explanation { get; }
    }
}
