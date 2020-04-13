using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CommandPaletteLibrary
{
    public interface IPaletteCommand
    {
        ICommand Command { get; }
        string Name { get; }
        string Explanation { get; }
    }
}
