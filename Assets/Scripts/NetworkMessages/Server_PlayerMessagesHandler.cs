using System;
using Unity.Networking.Transport;
using UnityEngine;

public class Server_PlayerMessagesHandler : MessagesHandlerClass
{
    public GameServer server => our_networkBehaviour as GameServer;

    public Server_PlayerMessagesHandler(NetworkBehaviour _server) : base(_server)
    {
        _server.AddMessageHandler(NetworkProtocal.RegisterPlayerMessage, OnPlayerRegister);
        _server.AddMessageHandler(NetworkProtocal.RequestEndTurnMessage, OnPlayerRequestEndTurn);
    }

    private void OnPlayerRequestEndTurn(MessageBase _msg, NetworkConnection _connection)
    {
        server.EndCurrentTurn();
    }

    private void OnPlayerRegister(MessageBase _msg, NetworkConnection _connection)
    {
        RegisterPlayerMessage registerPlayerMessage = _msg as RegisterPlayerMessage;


        //add him to the players
        PlayerData _player = new PlayerData()
        {
            guid = server.players.Count,
            nickName = registerPlayerMessage._playerName,
            boardZone = new RuntimeZone(),
            handZone = new RuntimeZone(),
            m_connenction = _connection,
            localPlayerId = registerPlayerMessage.localPlayeriD
        };

        server.AddPlayerToList(_player);

        //give him cards
        for (int i = 0; i < 2; i++)
        {
            _player.handZone.AddCard(server.nextCardId++, server.cardNames[UnityEngine.Random.Range(0, 4)], _player);
        }

        if (server.players.Count == 2)
            server.StartGame();


        #region Logging
        if (Logger.LogServerMessages)
        {
            Debug.Log($"SERVER:: Player registered {_player.nickName}");
        }
        #endregion
    }


}
