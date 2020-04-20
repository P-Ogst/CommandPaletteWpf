using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CommandPaletteLibrary
{
    public class PaletteSearchParameter : IPaletteSearchParameter
    {
        public ObservableCollection<IPaletteSearchItem> CandidateParameters { get; }

        public Func<object, bool> ValidateInput => (obj) =>
        {
            var item = obj as IPaletteSearchItem;
            return CandidateParameters.Contains(item);
        };

        public Func<object, object> CreateInput => (obj) =>
        {
            return (obj as IPaletteSearchItem).Value;
        };

        public Func<object, string> CreateInputExplanation => (obj) =>
        {
            var item = obj as IPaletteSearchItem;
            return item.Name;
        };

        public string Name { get; }

        public string Explanation { get; }

        public event EventHandler BeginInput;

        public PaletteSearchParameter(IEnumerable<IPaletteSearchItem> items, string name, string explanation = null)
        {
            CandidateParameters = new ObservableCollection<IPaletteSearchItem>();
            foreach (var item in items)
            {
                CandidateParameters.Add(item);
            }
            Name = name;
            Explanation = explanation;
        }
    }
}
