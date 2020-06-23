using UnityEngine;
using System.Collections;
using Unity.Networking.Transport;

public class Test_MessageHandlerClass : MessagesHandlerClass
{
 

    public Test_MessageHandlerClass(NetworkBehaviour _behaviour):base(_behaviour)
    {
        _behaviour.AddMessageHandler(NetworkProtocal.EmptyMessage, OnEmptyMessage);

    }

    public void OnEmptyMessage(MessageBase _msg, NetworkConnection _connection)
    {
        Debug.Log(our_networkBehaviour.LogPrefix + "Got empty message");
    }

}

