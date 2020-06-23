using System.Collections.Generic;
using UnityEngine;

public class RuntimeZone
{
    public List<RuntimeCard> cards = new List<RuntimeCard>();
    public string zoneName;

    public System.Action<RuntimeCard, int> OnCardAdded;
    public System.Action<RuntimeCard, int> OnCardRemoved;

    public void AddCard(int _id, string _name, PlayerData _ownerPlayer)
    {

        cards.Add(new RuntimeCard());
        cards[cards.Count - 1].cardName = _name;
        cards[cards.Count - 1].guid = _id;
        cards[cards.Count - 1].ownerPlayer = _ownerPlayer;

        //call delegates
        OnCardAdded?.Invoke(cards[cards.Count - 1], cards.Count);

    }

    public void AddCard(RuntimeCard _card)
    {
        cards.Add(_card);
        OnCardAdded?.Invoke(cards[cards.Count - 1], cards.Count);
    }

    public void RemoveCard(int unique_instanceId)
    {

        RuntimeCard _card = cards.Find(x => x.guid == unique_instanceId);
        if (_card != null)
        {
            cards.Remove(_card);
        }
        else
        {
            Debug.LogError("No such card has been found in collection");
        }


        OnCardRemoved?.Invoke(_card, cards.Count);
    }
}
