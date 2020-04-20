using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPaletteLibrary
{
    public static class PaletteParameterFactory
    {
        public static IPaletteParameter CreateMinMaxParameter<T>(T min, T max, string name, string explanation = null) where T : IComparable
        {
            if (min.CompareTo(max) > 0)
            {
                throw new ArgumentException();
            }
            return new MinMaxPaletteParameter<T>(min, max, name, explanation);
        }

        public static IPaletteSearchItem CreateSearchItem(object value, string name, string explanation = null)
        {
            return new PaletteSearchItem(value, name, explanation);
        }

        public static IPaletteParameter CreateSearchParameter(IEnumerable<IPaletteSearchItem> items, string name, string explanation = null)
        {
            return new PaletteSearchParameter(items, name, explanation);
        }
    }
}
