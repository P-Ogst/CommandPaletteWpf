using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CommandPaletteLibrary.Infrastructure
{
    public class DelegateCommand : ICommand
    {
        private Action<object> _execute;
        private Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            return _canExecute.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _execute.Invoke(parameter);
            }

        }
    }
}
