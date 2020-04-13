using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CommandPaletteLibrary
{
    public class PaletteCommand : IPaletteCommand
    {
        public ICommand Command { get; }

        public string Name { get; }

        public string Explanation { get; }

        public IEnumerable<IPaletteParameter> Parameters { get; }

        public PaletteCommand(ICommand command, string name = null, string explanation = null, IEnumerable<IPaletteParameter> parameters = null)
        {
            Command = command ?? throw new ArgumentNullException();
            Name = name;
            Explanation = explanation;
            Parameters = parameters;
        }
    }
}
