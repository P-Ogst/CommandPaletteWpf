using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CommandPaletteLibrary
{
    public interface IPaletteCommand
    {
        Func<IEnumerable<object>, object> CreateCommandParameter { get; }
        ICommand Command { get; }
        string Name { get; }
        string Explanation { get; }
        public IEnumerable<IPaletteParameter> Parameters { get; }
    }
}
