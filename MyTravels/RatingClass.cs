using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MyTravels
{
    class RatingClass : ObservableCollection<int>
    {
        public RatingClass()
        {
            Add(1);
            Add(2);
            Add(3);
            Add(4);
            Add(5);
            Add(6);
            Add(7);
            Add(8);
            Add(9);
            Add(10);
        }
    }
}
