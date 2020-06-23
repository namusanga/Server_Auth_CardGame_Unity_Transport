using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.Networking.Transport;

public class GameClient : Client
{

    public List<Player> localPlayers = new List<Player>();
    public GameObject mainPlayerPrfab;
    public GameObject opponentPlayerPrefab;

    /// <summary>
    /// the currently active game client
    /// </summary>
    public static GameClient Active
    {
        get
        {
            if (internal_active == null)
                internal_active = FindObjectOfType<GameClient>();
            return internal_active;
        }
    }
    public static GameClient internal_active;

    public override void RegisterMessageHandlerClasses()
    {
        base.RegisterMessageHandlerClasses();

        new Client_StartGameMessage(this);

        new Client_CardMessageHandler(this);

        AddMessageHandler(NetworkProtocal.SerevrNotification, NotificationHandler);

    }

    private void NotificationHandler(MessageBase _msg, NetworkConnection _connection)
    {
       StartCoroutine( GameScene.Active.ShowPlayerMessage("Server Notification"+ (_msg as ServerNotificationMessage).notification));
    }

    public override void UnregisterMessageHandlerClasses()
    {
        base.UnregisterMessageHandlerClasses();

        RemoveMessageHandler(NetworkProtocal.StartGameMessaege);
    }

    public override void OnClientConnected()
    {
        base.OnClientConnected();

        //START THE MAIN PLAYER
        Player _mainPlayer = Instantiate(mainPlayerPrfab).GetComponent<Player>();
        _mainPlayer.gameObject.SetActive(true);
        //SET THE PLAYER UNIQUE NET ID
        _mainPlayer.playerData.guid = localPlayers.Count;
        //set the connection to the server
        _mainPlayer.playerData.m_connenction = m_Connection;
        _mainPlayer.opponentData.m_connenction = m_Connection;
        //save the player
        localPlayers.Add(_mainPlayer);
        //tell the player to register
        _mainPlayer.RegisterWithServer();
        //make this the active player to start with
        _mainPlayer.activePlayer = true;

        //START THE OPPONENT PLAYER
        Player _opponentPlayer = Instantiate(opponentPlayerPrefab).GetComponent<Player>();
        _opponentPlayer.gameObject.SetActive(true);
        //set the player uniqueNetID
        _opponentPlayer.playerData.guid = localPlayers.Count;
        //set the connection to the server
        _opponentPlayer.playerData.m_connenction = m_Connection;
        _opponentPlayer.opponentData.m_connenction = m_Connection;
        //save the player
        localPlayers.Add(_opponentPlayer);
        //tell the player to register
        _opponentPlayer.RegisterWithServer();
    }
}
