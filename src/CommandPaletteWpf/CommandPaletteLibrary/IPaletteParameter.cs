using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPaletteLibrary
{
    public interface IPaletteParameter
    {
        Type ParameterType { get; }
        string Name { get; }
        string Explanation { get; }
    }
}
