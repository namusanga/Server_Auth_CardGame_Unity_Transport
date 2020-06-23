using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    public void EndTurn()
    {
        LocalPlayer localPlayer = FindObjectOfType<LocalPlayer>();
        if (localPlayer.activePlayer)
        {
            localPlayer.RequestEndTurn();
        }
    }
}
