using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CommandPaletteLibrary
{
    public class MinMaxPaletteParameter<T> : IPaletteParameter where T : IComparable
    {
        public object Min { get; }

        public object Max { get; }

        public string Name { get; }

        public string Explanation { get; }

        public Func<object, bool> ValidateInput { get; }

        public Func<object, string> CreateInputExplanation { get; }

        public object Input { get; }

        public Func<object, object> CreateInput { get; }

        public event EventHandler BeginInput;

        public MinMaxPaletteParameter(T min, T max, string name, string explanation)
        {
            Min = min;
            Max = max;
            Name = name;
            Explanation = explanation;
            ValidateInput = (obj) =>
            {
                try
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    var value = (T)converter.ConvertFrom(obj);
                    return value.CompareTo(Min) >= 0 && value.CompareTo(Max) <= 0;
                }
                catch
                {
                    return false;
                }
            };
            CreateInput = (obj) =>
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                return (T)converter.ConvertFrom(obj);
            };
            CreateInputExplanation = (obj) =>
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                var value = (T)converter.ConvertFrom(obj);
                return value.ToString();
            };
        }
    }
}
