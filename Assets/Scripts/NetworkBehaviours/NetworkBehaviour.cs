using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Base class for all network objects
/// Recives Messages and allocates them to the right handlers
/// Sends Messages
/// </summary>
public class NetworkBehaviour : MonoBehaviour
{
    public const int PORT = 9000;
    public const int BYTE_ARRAY_SIZE = 1024;


    //settings
    public virtual string LogPrefix
    {
        get
        {
            return "NONE:: ";
        }
    }


    /// <summary>
    /// Delegate for all functions that want to handle network messages
    /// </summary>
    /// <param name="_msg">The message we want to handle</param>
    /// <param name="_connection">The connection that has sent the message</param>
    public delegate void MessageHandlerDelegate(MessageBase _msg, NetworkConnection _connection);
    public Dictionary<byte, MessageHandlerDelegate> m_MessageHandlers = new Dictionary<byte, MessageHandlerDelegate>();

    public virtual void RegisterMessageHandlerClasses()
    {
    }

    public virtual void UnregisterMessageHandlerClasses()
    {
    }
    //REGISTERING HANDLERS
    public void AddMessageHandler(byte _type, MessageHandlerDelegate _handler)
    {
        if (m_MessageHandlers.ContainsKey(_type) == false)
            m_MessageHandlers.Add(_type, _handler);
        else
            Debug.LogError(LogPrefix + "handler already registered for this message");

    }

    public void RemoveMessageHandler(byte _type)
    {
        if (m_MessageHandlers.ContainsKey(_type) == true)
            m_MessageHandlers.Remove(_type);
        else
            Debug.LogError(LogPrefix + "not found for this message");

    }

    //READING AND WIRITING MESSAGES
    public MessageBase ReadMessage(DataStreamReader stream)
    {
        byte[] _bytes = new byte[BYTE_ARRAY_SIZE];

        for (int i = 0; i < BYTE_ARRAY_SIZE; i++)
        {
            _bytes[i] = stream.ReadByte();
        }

        //use binary formatter to read the data
        BinaryFormatter _formatter = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(_bytes);
        MessageBase _msg = _formatter.Deserialize(ms) as MessageBase;

        return _msg;
    }


    public void WriteMessage(ref DataStreamWriter _writer, MessageBase msg)
    {
        byte[] _bytes = new byte[BYTE_ARRAY_SIZE];


        //PACK THE DATA INTO BYTES
        BinaryFormatter _formatter = new BinaryFormatter();

        byte[] _tempBuffer;
        using (var _stream = new MemoryStream())
        {
            _formatter.Serialize(_stream, msg);
            _tempBuffer = _stream.ToArray();
        }

        //save the data
        for (int i = 0; i < _tempBuffer.Length; i++)
        {
            _bytes[i] = _tempBuffer[i];
        }

        //WRITE IT TO THE DATA STREAM
        for (int i = 0; i < BYTE_ARRAY_SIZE; i++)
        {
            _writer.WriteByte(_bytes[i]);
        }
    }

    //SENDING AND READING MESSAGES
    public virtual void SendNetMessage(ref NetworkDriver _driver, NetworkConnection _connection, MessageBase _msg)
    {
        DataStreamWriter _writer = _driver.BeginSend(_connection);
        WriteMessage(ref _writer, _msg);
        _driver.EndSend(_writer);
    }

    /// <summary>
    /// called when a data message is recieved
    /// </summary>
    /// <param name="_stream"></param>
    public virtual void OnDataMessage(DataStreamReader _stream, NetworkConnection _connection)
    {
        MessageBase _msg = ReadMessage(_stream);
        foreach (KeyValuePair<byte, MessageHandlerDelegate> keyValuePair in m_MessageHandlers)
        {
            if (keyValuePair.Key == _msg.Type)
            {
                keyValuePair.Value.Invoke(_msg, _connection);
                return;
            }
        }
    }

}
