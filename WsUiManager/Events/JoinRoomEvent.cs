namespace WsUiManager.Events;

using System.Threading.Tasks;
using Fleck;
using Serilog;
using WsUiManager.Entities;
using WsUiManager.Entities.Enums;
using WsUiManager.Events.Base;
using WsUiManager.Events.Exceptions;

public class JoinRoomEvent : BaseEvent
{
    private string roomName = "";

    public required string RoomName
    {
        get => this.roomName;
        set => this.roomName = Enum.GetNames(typeof(Room))
            .Where(room => room.Equals(value, StringComparison.OrdinalIgnoreCase))
            .FirstOrDefault("");
    }
}

public class JoinRoom : BaseHandler<JoinRoomEvent>
{
    public override async Task Handle(JoinRoomEvent eventType, IWebSocketConnection socket)
    {
        if (string.IsNullOrEmpty(eventType.RoomName))
        {
            throw new RoomNotExistsException();
        }

        var roomAsEnum = Enum.Parse<Room>(eventType.RoomName);
        var success = StateService.AddToRoom(socket, (int)roomAsEnum);

        if (!success)
        {
            throw new EventFailedException();
        }

        Log.Information("{@Id} - Cliente juntou-se a sala {@Room}.",
            socket.ConnectionInfo.Id, Enum.GetName(typeof(Room), (int)roomAsEnum));

        await socket.Send(new Message<JoinRoomMessage>()
        {
            ConnectionId = socket.ConnectionInfo.Id,
            Name = "JOINROOM_FEEDBACK",
            Data = new JoinRoomMessage()
            {
                Feedback = "Adicionado a sala com sucesso!",
                RoomName = Enum.GetName(typeof(Room), roomAsEnum) ?? ""
            },
        }.AsJson());
    }
}

