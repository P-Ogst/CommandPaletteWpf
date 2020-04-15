using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CommandPaletteLibrary
{
    public interface ISearchPaletteParameter : IPaletteParameter
    {
        // TODO: ISearchItem を足したほうがよさそう
        public ObservableCollection<object> CandidateParameters { get; }
    }
}
