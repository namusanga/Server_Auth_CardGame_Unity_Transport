using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerData playerData = new PlayerData();
    [HideInInspector] public PlayerData opponentData = new PlayerData();

    public int guid { 
        get
        {
            return playerData.guid; 
        }
        set
        {
            playerData.guid = value;
        }
    }

    public PlayerType playerType = PlayerType.MAIN;
    public bool activePlayer;

    /// <summary>
    /// called when out local client is told to connect
    /// this tell the inidual players to also start a connection
    /// </summary>
    public virtual void OnClientConnected()
    {

    }

    public virtual void OnOpponentCardMoved(OnCardMovedMessage onCardMovedMessage)
    {
        GameFunctions.MoveCard(opponentData.GetZone(onCardMovedMessage.from_Zone), opponentData.GetZone(onCardMovedMessage.to_Zone), onCardMovedMessage.cardGuid);
    }

    internal virtual void OnStartTurnMessage(StartTurnMessage _startTurnMessage)
    {
        LoadPlayerState(ref playerData, _startTurnMessage.playerData);
        LoadPlayerState(ref opponentData, _startTurnMessage.opponentData);
        activePlayer = true;
    }

    internal virtual void OnEndTurnMessage(EndTurnMessage _endTurnMessage)
    {
        LoadPlayerState(ref playerData, _endTurnMessage.playerData);
        LoadPlayerState(ref opponentData, _endTurnMessage.opponentData);
        activePlayer = false;
    }


    /// <summary>
    /// tell the server that we want to be connected
    /// </summary>
    public virtual void ConnectedPlayerToServer(StartGameMessage msg)
    {

    }


    public virtual void OnStartGame(StartGameMessage msg)
    {

        if (Logger.LogPlayerStartGameMessages)
        {
            Debug.Log($"CLIENT:: Got start game message");
        }

        //LOAD THE PLAYER STATS
        //payer
        LoadPlayerState(ref playerData, msg.playerData);
        //opponent
        LoadPlayerState(ref opponentData, msg.opponentData);
    }


    internal virtual void RegisterWithServer()
    {
    }

    public virtual void LoadPlayerState(ref PlayerData _playerData, NetPlayerData _netData)
    {
        //update the name
        _playerData.nickName = _netData.playerNickName;
        //update the playerInstanceId
        _playerData.guid = _netData.playerGuid;
        //save the localplayer id
        _playerData.localPlayerId = _netData.localPlayerId;

        //load net zones
        LoadNetZone(_playerData.boardZone, _netData.boardZone, _playerData);
        LoadNetZone(_playerData.handZone, _netData.handZone, _playerData);
    }

    public void LoadNetZone(RuntimeZone _localZone, NetZone _netZone, PlayerData _ownerPlayer)
    {
        //remove any illegal cards
        for (int i = 0; i < _localZone.cards.Count; i++)
        {
            RuntimeCard _card = _localZone.cards[i];
            if (Array.Exists(_netZone.cards, x => x.unique_cardId == _card.guid) == false)
            {
                //remove the card
                StartCoroutine( GameScene.Active.ShowPlayerMessage("Developer Warning:: The Server Reverted A Move"));
                _localZone.RemoveCard(_card.guid);
            }
        }

        //add any new cards
        for (int i = 0; i < _netZone.cards.Length; i++)
        {
            NetCard _card = _netZone.cards[i];
            RuntimeCard _localCard = _localZone.cards.Find(x => x.guid == _card.unique_cardId);
            if (_localCard == null)
            {
                //add the card
                _localZone.AddCard(NetworkUtilities.GetRuntimeCard(_card, _ownerPlayer));
            }
        }
    }

    public static Player GetPlayer(int guid)
    {
        List<Player> allPlayers = new List<Player>(FindObjectsOfType<Player>());

        return allPlayers.Find(x => x.playerData.guid == guid);
    }

    public void PlayerCard(RuntimeCard _card)
    {
        //RUN LOCAL SIMULATION
        GameFunctions.MoveCard(playerData.handZone, playerData.boardZone, _card.guid);
        //SEND MESSAGE TO SERVER
        MoveCardMessage moveCardMessgae = new MoveCardMessage()
        {
            playerGuid = playerData.guid,
            cardGuid = _card.guid,
            from_ZoneId = 0,
            to_ZoneId = 1
        };

        GameClient.Active.SendMessageToServer(moveCardMessgae);
    }


    public void RequestEndTurn()
    {
        GameClient.Active.SendMessageToServer(new RequestEndTurnMessage());
    }

    public enum PlayerType
    {
        MAIN,
        REMOTE_OPPONENT,
        AI_OPPONENT
    }
}
