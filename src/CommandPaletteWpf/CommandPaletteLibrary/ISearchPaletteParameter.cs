using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CommandPaletteLibrary
{
    public interface ISearchPaletteParameter<T> : IPaletteParameter
    {
        public ObservableCollection<T> CandidateParameters { get; }
    }
}
