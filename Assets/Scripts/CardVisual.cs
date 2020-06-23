using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CardVisual : MonoBehaviour
{

    public TextMeshPro text;
    public SpriteRenderer spriteRend;
    public List<Sprite> allSprites = new List<Sprite>();

    [HideInInspector] public RuntimeCard card;

    public virtual void Init(RuntimeCard _card)
    {
        card = _card;
        text.text = _card.cardName;

        if (spriteRend)
        {
            int rand = UnityEngine.Random.Range(0, allSprites.Count);
            spriteRend.sprite = allSprites[rand];
        }
    }

    /// <summary>
    /// Get a card visual using the id
    /// </summary>
    /// <param name="unique_CardInstanceID">the id to use</param>
    /// <returns></returns>
    public static CardVisual FindCardVisual(int unique_CardInstanceID)
    {
        List<CardVisual> allCards = new List<CardVisual>(Object.FindObjectsOfType<CardVisual>());

        return allCards.Find(x => x.card.guid == unique_CardInstanceID);
    }
}
