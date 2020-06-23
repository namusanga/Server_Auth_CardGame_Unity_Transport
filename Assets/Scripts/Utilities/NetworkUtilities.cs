using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class NetworkUtilities
{
    public static NetCard GetNetCard(RuntimeCard _card)
    {
        NetCard _madeCard = new NetCard()
        {
            cardName = _card.cardName,
            unique_cardId = _card.guid
        };

        return _madeCard;
    }

    public static RuntimeCard GetRuntimeCard(NetCard _netCard, PlayerData _ownerPlayer)
    {
        RuntimeCard _liveCard = new RuntimeCard()
        {
            cardName = _netCard.cardName,
            guid = _netCard.unique_cardId,
            ownerPlayer = _ownerPlayer
        };

        return _liveCard;
    }

    public static NetZone GetNetZone(RuntimeZone _zone)
    {

        NetCard[] _cards;
        List<NetCard> _tempCards = new List<NetCard>();

        for (int i = 0; i < _zone.cards.Count; i++)
        {
            _tempCards.Add(GetNetCard(_zone.cards[i])); 
        }

        _cards = _tempCards.ToArray();


        return new NetZone()
        {
            _zoneName = _zone.zoneName,
            cards = _cards
        };
    }

    public static NetPlayerData GetNetPlayerData(PlayerData _data)
    {
        return new NetPlayerData()
        {
            playerGuid = _data.guid,
            playerNickName = _data.nickName,
            localPlayerId = _data.localPlayerId,
            handZone = GetNetZone(_data.handZone),
            boardZone = GetNetZone(_data.boardZone)
        };
    }

}
