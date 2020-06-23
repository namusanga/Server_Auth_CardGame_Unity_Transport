using UnityEngine;
using System.Collections;

[System.Serializable]
public class NetPlayerData 
{
    public int playerGuid;
    public string playerNickName;
    public int localPlayerId;

    public NetZone handZone;
    public NetZone boardZone;
}
