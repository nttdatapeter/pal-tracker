using System.Collections.Generic;

namespace PalTracker
{
    public interface IOperationCounter<T>
    {
        void Increment(TrackedOperation operation);

        IDictionary<TrackedOperation, int> GetCounts { get; }

        string Name { get; }
    }
}