using System;

namespace KubTest.EventSourcing
{
    public interface IEvent
    {
        DateTime EventOccurred { get; }
        int Version { get; }
    }
}
