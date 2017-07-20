using System;

namespace MusalaSoftTask
{
    interface IOutgoingCalls <Tkey>
        where Tkey : IComparable<Tkey>
    {
        void AddOutgoingCall(
            Tkey name
        );

        string ListTopCalledCalls();
    }
}
