using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace CommandPaletteLibrary
{
    public class PaletteCommandService
    {
        private ObservableCollection<IPaletteCommand> _commandList = new ObservableCollection<IPaletteCommand>();
        public ObservableCollection<IPaletteCommand> CommandList => _commandList;

        public PaletteCommandService()
        {}

        public void AddCommand(ICommand command, string name = null, string explanation = null, Func<IEnumerable<object>, object> createCommandParameter = null, params IPaletteParameter[] parameters)
        {
            _commandList.Add(new PaletteCommand(command, name, explanation, createCommandParameter, parameters));
        }
    }
}
