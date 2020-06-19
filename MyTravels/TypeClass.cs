using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MyTravels
{
    class TypeClass : ObservableCollection<string>
    {
        public TypeClass()
        {
            Add("Góry");
            Add("Morze");
            Add("Jezioro");
            Add("Miasto");
            Add("Wieś");
            Add("Inny");
        }
    }
}
