using UnityEngine;
using System.Collections;

/// <summary>
/// static class the handle common functions
/// </summary>
public class GameFunctions
{
    public static void MoveCard(RuntimeZone _fromZone, RuntimeZone _toZone, int _unique_CardId)
    {
        RuntimeCard _card = _fromZone.cards.Find(x => x.guid == _unique_CardId);

        _fromZone.RemoveCard(_unique_CardId);
        _toZone.AddCard(_card);
    }
}
