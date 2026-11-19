using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Rooms;

public sealed class Room
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Capacity { get; private set; }

    private Room(Guid id, string name, int capacity)
    {
        Id = id;
        Name = name;
        Capacity = capacity;
    }

    public static Result<Room> Create(string? name, int capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return new Error("InvalidName", "Room name is required.", ErrorType.Validation);
        }

        if (capacity <= 0)
        {
            return new Error("InvalidCapacity", "Capacity must be positive.", ErrorType.Validation);
        }

        return new Room(Guid.NewGuid(), name.Trim(), capacity);
    }
}
