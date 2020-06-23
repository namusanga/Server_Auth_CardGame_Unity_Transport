using UnityEngine;
using System.Collections;
using System;
using Unity.Networking.Transport;

public class Server_KeepAliveHandler : MessagesHandlerClass
{

    public GameServer server => our_networkBehaviour as GameServer;

    public Server_KeepAliveHandler(NetworkBehaviour _server ) : base(_server)
    {
        _server.AddMessageHandler(NetworkProtocal.KeepAlive, OnKeepAlive);
    }

    private void OnKeepAlive(MessageBase _msg, NetworkConnection _connection)
    {
        if (Logger.LogKeepAliveMessages)
        {
            Debug.Log(server.LogPrefix + "Received keep alive messgae");
        }
        server.SendToClient(_connection, new KeepAliveMessage());
    }
}
