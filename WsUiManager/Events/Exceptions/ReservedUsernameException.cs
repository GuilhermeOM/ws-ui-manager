namespace WsUiManager.Events.Exceptions;

public class ReservedUsernameException(string message = "Nome solicitado se trata de um nome reservado.") : Exception(message) { }
