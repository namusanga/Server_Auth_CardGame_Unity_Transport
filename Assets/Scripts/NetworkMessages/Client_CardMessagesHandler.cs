using UnityEngine;
using System.Collections;
using System;
using Unity.Networking.Transport;

public class Client_CardMessageHandler : MessagesHandlerClass
{

    public GameClient client => our_networkBehaviour as GameClient;

    public Client_CardMessageHandler (NetworkBehaviour _client) : base(_client)
    {
        _client.AddMessageHandler(NetworkProtocal.OnCardMovedMessage, OnCardMoved);
    }

    private void OnCardMoved(MessageBase _msg, NetworkConnection _connection)
    {
        OnCardMovedMessage onCardMovedMessage = _msg as OnCardMovedMessage;
        foreach(Player p in client.localPlayers)
        {
            if(p.guid != onCardMovedMessage.playerGuid)
            {
                p.OnOpponentCardMoved(onCardMovedMessage);
            }
        }       
    }
}
