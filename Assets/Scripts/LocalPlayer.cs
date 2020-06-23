using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class LocalPlayer : Player
{
    //Hand
    public RuntimeZone opponentHandZone => opponentData.handZone;
    public RuntimeZone playerHandZone => playerData.handZone;

    //Board
    public RuntimeZone opponentBoardZone => opponentData.boardZone;
    public RuntimeZone playerBoardZone => playerData.boardZone;


    public override void OnStartGame(StartGameMessage msg)
    {

        //register for callbacks
        playerData.handZone.OnCardAdded += OnPlayerCardAdded_Hand;
        playerData.boardZone.OnCardAdded += OnPlayerCardAdded_Board;
        playerData.boardZone.OnCardRemoved += OnPlayerCardRemoved_Board;

        opponentData.handZone.OnCardAdded += OnOpponentCardAdded_Hand;
        opponentData.boardZone.OnCardAdded += OnOpponentCardAdded_Board;

        base.OnStartGame(msg);
    }


    #region Zone Callbacks
    private void OnPlayerCardRemoved_Board(RuntimeCard _card, int _count)
    {
        CardVisual _visual = CardVisual.FindCardVisual(_card.guid);

        Destroy(_visual.gameObject);
    }

    public void OnPlayerCardAdded_Hand(RuntimeCard _card, int _count)
    {
        CardVisual _cardV = Instantiate(GameScene.Active.playerHandCardPrefab).GetComponent<CardVisual>();
        _cardV.Init(_card);

        //Get the final position
        Vector3 _position = new Vector3(GameScene.Active.playerHandCardsStart.position.x + (90*(_count - 1)), GameScene.Active.playerHandCardsStart.position.y, GameScene.Active.playerHandCardsStart.position.z);

        _cardV.transform.position = _position;

    }

    public void OnPlayerCardAdded_Board(RuntimeCard _card, int _count)
    {
        //DESTROY THE CARD FROM THE HAND
        GameObject handCard = CardVisual.FindCardVisual(_card.guid).gameObject;
        Destroy(handCard);
        //MAKE NEW CARD
        CardVisual newCardVisual = Instantiate(GameScene.Active.boardCardPrefab).GetComponent<CardVisual>();
        newCardVisual.Init(_card);
        newCardVisual.transform.position = new Vector3();
        //ARRANGE THE CARD
        //get new position
        Vector3 newPosition = new Vector3(GameScene.Active.playerBoardCardsStart.transform.position.x + (_count-1) * 90, GameScene.Active.playerBoardCardsStart.transform.position.y, GameScene.Active.playerBoardCardsStart.transform.position.z);

        newCardVisual.transform.DOMove(newPosition, .5f);
    }

    public void OnOpponentCardAdded_Hand(RuntimeCard _card, int _count)
    {
        GameObject _cardV = Instantiate(GameScene.Active.opponentHandCardPrefab);

        //Get the final position
        Vector3 _position = new Vector3(GameScene.Active.opponentHandCardsStart.position.x + (90 * (_count-1)), GameScene.Active.opponentHandCardsStart.position.y, GameScene.Active.opponentHandCardsStart.position.z);

        _cardV.transform.position = _position;
    }

    public void OnOpponentCardAdded_Board(RuntimeCard _card, int _count)
    {

    }
    #endregion


    internal override void RegisterWithServer()
    {
        base.RegisterWithServer();
        //MAKE REGISTER MESSAGE
        RegisterPlayerMessage registerPlayerMessage = new RegisterPlayerMessage()
        {
            _playerName = "LocalPlayer",
            localPlayeriD = GameClient.Active.localPlayers.Count-1
        };
        //SEND THE MESSAGE
        GameClient.Active.SendMessageToServer(registerPlayerMessage);
    }
}
