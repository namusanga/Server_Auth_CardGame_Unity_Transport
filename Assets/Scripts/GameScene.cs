using UnityEngine;
using System.Collections;
using TMPro;
using DG.Tweening;


/// <summary>
/// provides a map to the entire game scene to show where certain objects are
/// </summary>
public class GameScene : MonoBehaviour
{
    public static GameScene Active
    {
        get
        {
            if (internal_Active == null)
                internal_Active = FindObjectOfType<GameScene>();
            return internal_Active;
        }

    }
    private static GameScene internal_Active;

    public Transform opponentHandCardsStart;
    public Transform opponentBoardCardsStart;
    public Transform playerHandCardsStart;
    public Transform playerBoardCardsStart;

    public GameObject boardCardPrefab;
    public GameObject playerHandCardPrefab;
    public GameObject opponentHandCardPrefab;


    /// <summary>
    /// show a message and the remove it after some 1
    /// </summary>
    /// <param name="_msg"></param>
    /// <returns></returns>
    public IEnumerator ShowPlayerMessage(string _msg)
    {
        Vector3 _inPosition = GameObject.Find("In_Position").transform.position;
        Vector3 _outPosition = GameObject.Find("Out_Position").transform.position;
        TextMeshProUGUI txt = GameObject.Find("NotificationText").GetComponent<TextMeshProUGUI>();

        txt.text = _msg;

        txt.enabled = true;
        txt.transform.position = _outPosition;
        txt.transform.DOMove(_inPosition, .5f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(2);
        txt.transform.DOMove(_outPosition, .5f).SetEase(Ease.InOutSine).OnComplete(()=> {
            txt.enabled = false;    
        });
    }
}



