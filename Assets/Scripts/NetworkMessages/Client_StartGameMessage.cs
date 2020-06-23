using UnityEngine;
using System.Collections;
using System;
using Unity.Networking.Transport;

public class Client_StartGameMessage : MessagesHandlerClass
{

    public GameClient client => our_networkBehaviour as GameClient;

    public Client_StartGameMessage(NetworkBehaviour _client) : base(_client)
    {
        _client.AddMessageHandler(NetworkProtocal.StartGameMessaege, OnStartGameMessage);
        _client.AddMessageHandler(NetworkProtocal.EndTurnMessage, OnEndTurnMessage);
        _client.AddMessageHandler(NetworkProtocal.StartTurnMessage, OnStartTurnMessage);
    }

    private void OnStartTurnMessage(MessageBase _msg, NetworkConnection _connection)
    {
        StartTurnMessage _startTurnMessage = _msg as StartTurnMessage;
        foreach(Player p in client.localPlayers)
        {
            if (p.guid == _startTurnMessage.recepientGUID)
                p.OnStartTurnMessage(_startTurnMessage);
        }
    }

    private void OnEndTurnMessage(MessageBase _msg, NetworkConnection _connection)
    {
        EndTurnMessage _endTurnMessage = _msg as EndTurnMessage;
        foreach(Player p in client.localPlayers)
        {
            if (p.guid == _endTurnMessage.recepientGUID)
                p.OnEndTurnMessage(_endTurnMessage);
        }
    }

    private void OnStartGameMessage(MessageBase _msg, NetworkConnection _connection)
    {
        StartGameMessage startGameMessage = _msg as StartGameMessage;

        Player[] localPlayers = GameObject.FindObjectsOfType<Player>();
        for (int i = 0; i < localPlayers.Length; i++)
        {
            if(localPlayers[i].playerData.guid == startGameMessage.playerData.playerGuid)
            {
                localPlayers[i].OnStartGame(startGameMessage);
            }
        }
    }
}
