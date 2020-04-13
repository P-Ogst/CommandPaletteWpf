using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPaletteLibrary
{
    public static class PaletteParameterFactory
    {
        public static IPaletteParameter CreateRangeParameter<T>(T min, T max, string name, string explanation = null) where T : IComparable
        {
            if (min.CompareTo(max) > 0)
            {
                throw new ArgumentException();
            }
            return new RangePaletteParameter(min, max, typeof(T), name, explanation);
        }
    }
}
