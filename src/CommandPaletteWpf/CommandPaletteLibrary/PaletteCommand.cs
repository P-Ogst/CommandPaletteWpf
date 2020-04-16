using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace CommandPaletteLibrary
{
    public class PaletteCommand : IPaletteCommand
    {
        public ICommand Command { get; }

        public string Name { get; }

        public string Explanation { get; }

        public IEnumerable<IPaletteParameter> Parameters { get; }

        public Func<IEnumerable<object>, object> CreateCommandParameter { get; }

        public PaletteCommand(ICommand command,
                              string name = null,
                              string explanation = null,
                              Func<IEnumerable<object>, object> createCommandParameter = null,
                              IEnumerable<IPaletteParameter> parameters = null)
        {
            Command = command ?? throw new ArgumentNullException();
            Name = name;
            Explanation = explanation;
            CreateCommandParameter = createCommandParameter;
            Parameters = parameters;
        }
    }
}
