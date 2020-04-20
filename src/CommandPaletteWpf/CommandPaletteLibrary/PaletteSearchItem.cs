using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPaletteLibrary
{
    public class PaletteSearchItem : IPaletteSearchItem
    {
        public PaletteSearchItem(object value, string name, string explanation = null)
        {
            Value = value;
            Name = name;
            Explanation = explanation;
        }

        public string Name { get; }

        public string Explanation { get; }

        public object Value { get; }
    }
}
