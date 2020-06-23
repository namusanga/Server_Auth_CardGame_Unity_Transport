using Unity.Networking.Transport;
using System;
/// <summary>
/// holds the different ints for mesage types
/// </summary>
public static class NetworkProtocal
{
    public static byte BaseMessage = 0;

    public static byte EmptyMessage = 1;

    public static byte OnPlayerConnected = 2;

    public static byte StartGameMessaege = 3;

    public static byte RegisterPlayerMessage = 4;

    public static byte MoveCardMessage = 5;

    public static byte OnCardMovedMessage = 6;

    public static byte KeepAlive = 7;

    public static byte RequestEndTurnMessage = 8;

    public static byte EndTurnMessage = 9;
    
    public static byte StartTurnMessage = 10;
}

[System.Serializable]
public abstract class MessageBase
{
    //Number for the type of messaege this is
    public byte Type { set; get; }

    public MessageBase()
    {
        Type = NetworkProtocal.BaseMessage;
    }
}


[System.Serializable]
public class EmptyMessage:MessageBase
{
    public EmptyMessage()
    {
        Type = NetworkProtocal.EmptyMessage;
    }
}

[System.Serializable]
public class StartGameMessage : MessageBase
{
    public StartGameMessage(){ Type = NetworkProtocal.StartGameMessaege; }

    public NetPlayerData opponentData;
    public NetPlayerData playerData;
}

[System.Serializable]
public class RegisterPlayerMessage : MessageBase
{
    public RegisterPlayerMessage() { Type = NetworkProtocal.RegisterPlayerMessage; }

    public string _playerName;
    public int localPlayeriD;
}

[System.Serializable]
public class MoveCardMessage : MessageBase
{
    public MoveCardMessage() { Type = NetworkProtocal.MoveCardMessage; }

    public int playerGuid;
    public int cardGuid;
    public int from_ZoneId;
    public int to_ZoneId;
}

[System.Serializable]
public class OnCardMovedMessage:MessageBase
{
    public OnCardMovedMessage() { Type = NetworkProtocal.OnCardMovedMessage; }

    public int playerGuid;
    public int cardGuid;
    public int from_Zone;
    public int to_Zone;
}

[System.Serializable]
public class KeepAliveMessage : MessageBase
{
    public KeepAliveMessage() { Type = NetworkProtocal.KeepAlive; }
}

[System.Serializable]
public class EndTurnMessage : MessageBase
{
    public EndTurnMessage() { Type = NetworkProtocal.EndTurnMessage; }

    public int recepientGUID;
    public NetPlayerData opponentData;
    public NetPlayerData playerData;
}

[System.Serializable]
public class StartTurnMessage : MessageBase
{
    public StartTurnMessage () { Type = NetworkProtocal.StartTurnMessage; }

    public int recepientGUID;
    public NetPlayerData opponentData;
    public NetPlayerData playerData;
}


[System.Serializable]
public class RequestEndTurnMessage : MessageBase
{
    public RequestEndTurnMessage() { Type = NetworkProtocal.RequestEndTurnMessage; }
}






