using UnityEngine;
using System.Collections;

public class HandCard : CardVisual
{
    private void OnMouseDown()
    {
        Player myPlayer = Player.GetPlayer(card.ownerPlayer.guid);

        myPlayer.PlayerCard(card);
    }
}
