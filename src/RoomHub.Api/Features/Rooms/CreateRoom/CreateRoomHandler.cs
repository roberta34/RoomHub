using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed class CreateRoomHandler(
    IRoomRepository rooms,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateRoomCommand, Result<Room>>
{
    public async Task<Result<Room>> HandleAsync(
        CreateRoomCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = Room.Create(command.Name, command.Capacity);
        if (result.IsSuccess)
        {
            rooms.Add(result.Value!);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return result;
    }
}
