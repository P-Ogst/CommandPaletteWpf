using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPaletteLibrary
{
    public interface IPaletteParameter
    {
        event EventHandler BeginInput;
        Func<object, bool> ValidateInput { get; }
        Func<object, object> CreateInput { get; }
        Func<object, string> CreateInputExplanation { get; }

        string Name { get; }
        string Explanation { get; }
    }
}
