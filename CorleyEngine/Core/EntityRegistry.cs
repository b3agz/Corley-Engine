using System.Linq;
using System.Collections.Generic;

namespace CorleyEngine.Core;

/// <summary>
/// Responsible for providing unique IDs for <see cref="Entity"/> classes. IDs can be
/// returned to the registry for re-use if the Entity is destroyed.
/// </summary>
public static class EntityRegistry {

    private static int _nextAvailableId = 0;

    // Holds IDs that have been returned to the registry.
    private static Queue<int> _recycledIds = new();

    /// <summary>
    /// Returns a unique ID. If any IDs have been returned for recycling, it will
    /// prioritise those over new IDs.
    /// </summary>
    public static int GenerateId() {

        // If have IDs for recycling, get the next one and return it.
        if (_recycledIds.Count > 0) {
            return _recycledIds.Dequeue();
        }

        int newId = _nextAvailableId;
        _nextAvailableId++;
        return newId;

    }

    /// <summary>
    /// Returns an ID to the registry for re-use after the associated <see cref="Entity" />
    /// is destroyed.
    /// </summary>
    public static void ReleaseId(int id) {

        if (!_recycledIds.Contains(id))
            _recycledIds.Enqueue(id);

    }

    /// <summary>
    /// Registers an existing ID (such as from a saved file or when loading a new scene) so the
    /// registry doesn't attempt to reissue it.
    /// </summary>
    public static void RegisterLoadedId(int savedId) {

        // Since the base ID logic is just incrementing the ID number, if the ID is equal to or greater
        // than the next ID, we can just add one and return that. This guarantees it will be higher than
        // anything currently in the registry.
        if (savedId >= _nextAvailableId)
            _nextAvailableId = savedId + 1;

        // If the ID is in the recycling queue, it needs to be removed so it doesn't get handed out again.
        if (_recycledIds.Contains(savedId)) {
            IEnumerable<int> filteredQueue = _recycledIds.Where(id => id != savedId);
            _recycledIds = new Queue<int>(filteredQueue);
        }
    }


    /// <summary>
    /// Clears the registry entirely, both the nextID tracker and the recycled ID queue.
    /// </summary>
    public static void Reset() {

        _nextAvailableId = 0;
        _recycledIds.Clear();

    }
}
