using Unity.Networking.Transport;

public class Server_CardMessagesHandler : MessagesHandlerClass
{
    public GameServer server => our_networkBehaviour as GameServer;

    public Server_CardMessagesHandler(NetworkBehaviour _server) : base(_server)
    {
        _server.AddMessageHandler(NetworkProtocal.MoveCardMessage, OnCardMoved);
    }

    private void OnCardMoved(MessageBase _msg, NetworkConnection _connection)
    {
        if (server.madeMoves < 1)
        {

            //convert message
            MoveCardMessage _moveCardMessgae = _msg as MoveCardMessage;
            //get the server player
            int playerIndex = server.players.IndexOf(server.players.Find(x => x.guid == _moveCardMessgae.playerGuid));
            //move the card on the server
            GameFunctions.MoveCard(server.players[playerIndex].GetZone(_moveCardMessgae.from_ZoneId), server.players[playerIndex].GetZone(_moveCardMessgae.to_ZoneId), _moveCardMessgae.cardGuid);
            //tell other players that the card has moved
            BroadcastMoveCardMessgae(server.players[playerIndex].guid, _moveCardMessgae.cardGuid, _moveCardMessgae.from_ZoneId, _moveCardMessgae.to_ZoneId);

            server.madeMoves++;

            //ADDED PORTION
            server.SendToClient(_connection, new ServerNotificationMessage()
            {
                notification = "You played a card"
            });
        }
        else UnityEngine.Debug.Log(server.LogPrefix + "Illigal move detected");
    }

    public void BroadcastMoveCardMessgae(int _playerGUID, int _cardGUID, int _fromZone, int _toZone)
    {
        foreach (NetworkConnection nc in server.m_Connections)
        {
            server.SendToClient(nc, new OnCardMovedMessage()
            {
                playerGuid = _playerGUID,
                cardGuid = _cardGUID,
                from_Zone = _fromZone,
                to_Zone = _toZone
            });
        }
    }
}
