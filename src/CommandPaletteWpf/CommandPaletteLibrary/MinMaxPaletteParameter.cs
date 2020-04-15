using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CommandPaletteLibrary
{
    public class MinMaxPaletteParameter<T> : IInputPaletteParameter where T : IComparable
    {
        public object Min { get; }

        public object Max { get; }

        public string Name { get; }

        public string Explanation { get; }

        public Func<object, bool> ValidateInput { get; }

        public event EventHandler BeginInput;

        public MinMaxPaletteParameter(T min, T max, string name, string explanation)
        {
            Min = min;
            Max = max;
            Name = name;
            Explanation = explanation;
            ValidateInput = (obj) =>
            {
                if (!(obj is T))
                {
                    return false;
                }
                var value = (T)obj;

                return value.CompareTo(Min) >= 0 && value.CompareTo(Max) <= 0;
            };
        }
    }
}
