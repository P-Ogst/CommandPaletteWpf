using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CommandPaletteLibrary
{
    public class ResultTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CommandSelector { get; set; }

        public DataTemplate ValueInputSelector { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ObservableCollection<IPaletteCommand>)
            {
                return CommandSelector;
            }
            else if (item is IPaletteParameter)
            {
                return ValueInputSelector;
            }
            return null;
        }
    }
}
