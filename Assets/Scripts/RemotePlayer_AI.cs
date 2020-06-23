using UnityEngine;
using System.Collections;

public class RemotePlayer_AI : Player
{
    internal override void RegisterWithServer()
    {
        base.RegisterWithServer();
        //MAKE REGISTER MESSAGE
        RegisterPlayerMessage registerPlayerMessage = new RegisterPlayerMessage()
        {
            _playerName = "AI Player",
            localPlayeriD = GameClient.Active.localPlayers.Count - 1
        };
        //SEND THE MESSAGE
        GameClient.Active.SendMessageToServer(registerPlayerMessage);
    }
}
