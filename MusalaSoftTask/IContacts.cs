using System;

namespace MusalaSoftTask
{
    interface IContacts <TKey, TValue>
        where TKey : IComparable<TKey>
    {
        void AddNewPair(
            TKey name,
            TValue phoneNumber
        );

        void DeletePairByName(
            TKey name
        );

        string GetPhoneNumberByName(
            TKey name
        );

        string ListWholePhoneBook();
    }
}
