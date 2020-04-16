using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPaletteLibrary
{
    public class InputParameter
    {
        public string Name { get; }
        public object Input { get; }
        public string InputExplanation { get; }

        public InputParameter(string name, object input, string inputExplanation)
        {
            Name = name;
            Input = input;
            InputExplanation = inputExplanation;
        }
    }
}
