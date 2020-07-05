 using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;


public class Server : NetworkBehaviour
{
    public NetworkDriver m_Driver;
    //list will store our connections
    public NativeList<NetworkConnection> m_Connections;

    public override string LogPrefix => "SERVER:: ";

    public virtual void Start()
    {
        //RESETS OUT DATA
        m_Driver = NetworkDriver.Create();

        //CONFIGURATION
        NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;
        endpoint.Port = 9000;

        //BIND THE NET DRIVER
        if (m_Driver.Bind(endpoint) != 0)
            Debug.Log(LogPrefix + "Faileld to bind port 9000");
        else
            //start listening 
            m_Driver.Listen();

        //init the list to hold out connections
        m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);

        RegisterMessageHandlerClasses();
    }

    public override void RegisterMessageHandlerClasses()
    {
        base.RegisterMessageHandlerClasses();
    }

    public override void UnregisterMessageHandlerClasses()
    {
        base.UnregisterMessageHandlerClasses();
    }

    private void OnDestroy()
    {
        //DELETE BOTH OBJECTS IF THE SERER IS DESTROYED
        m_Driver.Dispose();
        m_Connections.Dispose();

        UnregisterMessageHandlerClasses();

    }

    public virtual void Update()
    {
        //FORCE THE NETDRIVER TO USE THE NORMAL UPDATE LOOP
        m_Driver.ScheduleUpdate().Complete();

        //CLEAN UP CONNECTIONS
        //if not made
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
            {
                m_Connections.RemoveAtSwapBack(i);
                --i;
            }
        }

        //ACCEPT NEW CONNECTIONS
        NetworkConnection c;
        //asks for any new connection
        while ((c = m_Driver.Accept()) != default)
        {
            //if it's not null
            //ADD THE NEW CONECTION
            m_Connections.Add(c);
        }

        //START CHECKING FOR EVENT WHICH MIGHT HAVE HAPPENED
        DataStreamReader stream;
        for (int i = 0; i < m_Connections.Length; i++)
        {
            //if the connection is not created
            if (!m_Connections[i].IsCreated)
                continue; //skip it

            //if we have a connection
            //check for an event
            NetworkEvent.Type cmd;
            while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                //start filtering events
                //DATA
                if (cmd == NetworkEvent.Type.Data)
                {
                    OnDataMessage(stream, m_Connections[i]);
                }
                //DISCONNECT
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log(LogPrefix + "Client asked to disconnect");
                    m_Connections[i] = default;
                }
            }
        }
    }

    public override void OnDataMessage(DataStreamReader _stream, NetworkConnection _connection)
    {
        base.OnDataMessage(_stream, _connection);
    }

}
