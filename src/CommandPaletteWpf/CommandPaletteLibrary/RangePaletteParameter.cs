using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPaletteLibrary
{
    public class RangePaletteParameter : IInputPaletteParameter
    {
        public object Min { get; }

        public object Max { get; }

        public Type ParameterType { get; }

        public string Name { get; }

        public string Explanation { get; }

        public RangePaletteParameter(object min, object max, Type type, string name, string explanation)
        {
            Min = min;
            Max = max;
            ParameterType = type;
            Name = name;
            Explanation = explanation;
        }
    }
}
