using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CommandPaletteLibrary
{
    public interface IPaletteSearchParameter : IPaletteParameter
    {
        public ObservableCollection<IPaletteSearchItem> CandidateParameters { get; }
    }
}
