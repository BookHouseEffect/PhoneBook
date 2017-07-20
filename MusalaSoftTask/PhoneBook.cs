using System;
using System.Collections.Generic;

namespace MusalaSoftTask
{
    public abstract class PhoneBook<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        protected SortedDictionary<TKey, TValue> ContactsList;
        protected SortedSet<Tuple<int, TKey>> OutgoingCallHistory;


        public PhoneBook()
        {
            ContactsList = new SortedDictionary<TKey, TValue>();
            OutgoingCallHistory = new SortedSet<Tuple<int, TKey>>(
                new ReverseSortedSetComparer());
        }

        internal class ReverseSortedSetComparer : IComparer<Tuple<int, TKey>>
        {
            public int Compare(Tuple<int, TKey> x, Tuple<int, TKey> y)
            {
                int value = y.Item1.CompareTo(x.Item1);

                if (value != 0)
                    return value;

                return x.Item2.CompareTo(y.Item2);
            }
        }
    }

    
}
