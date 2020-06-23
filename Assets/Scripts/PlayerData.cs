using UnityEngine;
using System;
using Unity.Networking.Transport;
public class PlayerData {

    //unique identifier for this player on all networks
    //in the server. players can have the same NetConnection but never the same playerNetInstaneId
    public int guid;
    public string nickName = "";
    public int localPlayerId;
    /// <summary>
    /// ON SERVER:: the connection for this player, used to send this player messages
    /// ON CLIENT:: the connection to the server
    /// </summary>
    public NetworkConnection m_connenction;

    public RuntimeZone boardZone = new RuntimeZone();
    public RuntimeZone handZone = new RuntimeZone();

    /// <summary>
    /// get a zone using the id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public RuntimeZone GetZone(int id)
    {
        if (id == 0)
            return handZone;
        else return boardZone;
    }
    
    
}
