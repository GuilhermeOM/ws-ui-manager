namespace WsUiManager.Events.Exceptions;

public class AlreadyInRoomException(string message = "Cliente já está conectado na sala solicitada.") : Exception(message) { }
