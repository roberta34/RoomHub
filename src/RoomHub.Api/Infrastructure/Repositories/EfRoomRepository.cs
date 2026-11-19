using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Infrastructure.Persistence;

namespace RoomHub.Api.Infrastructure.Repositories;

public sealed class EfRoomRepository(RoomHubDbContext db) : IRoomRepository
{
    public void Add(Room room) => db.Rooms.Add(room);

    public Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        db.Rooms.FirstOrDefaultAsync(room => room.Id == id, cancellationToken);
}
