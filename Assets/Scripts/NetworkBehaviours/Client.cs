using Unity.Networking.Transport;
using UnityEngine;


public class Client : NetworkBehaviour
{

    public NetworkDriver m_Driver;
    //list will store our connections
    public NetworkConnection m_Connection { get; private set; }

    public override string LogPrefix => "CLIENT:: ";


    private void Start()
    {
        //RESETS OUT DATA
        m_Driver = NetworkDriver.Create();
        m_Connection = default;

        //INIT 
        NetworkEndPoint endpoint = NetworkEndPoint.LoopbackIpv4;
        endpoint.Port = 9000;//make the port
        m_Connection = m_Driver.Connect(endpoint);//connect to server on local host

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
        m_Driver.Dispose();

        UnregisterMessageHandlerClasses();

    }

    private void Update()
    {
        //FORCE THE UPDATE TO RUN AT THE SAME RATE AS NORMAL UPDATE
        m_Driver.ScheduleUpdate().Complete();

        if (!m_Connection.IsCreated)
        {
            return;
        }


        //HANDLE MESSAGES
        DataStreamReader stream;
        NetworkEvent.Type cmd;

        //START CHECKING FOR EVENT WHICH MIGHT HAVE HAPPENED
        while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                OnClientConnected();
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                OnDataMessage(stream, m_Connection);

            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("CLIENT:: got a disconnect messsage");
                m_Connection = default;
            }

        }
    }

    /// <summary>
    /// send this message to the server
    /// </summary>
    /// <param name="_msg"></param>
    public void SendMessageToServer(MessageBase _msg)
    {
        SendNetMessage(ref m_Driver, m_Connection, _msg);
    }


    /// <summary>
    /// called when the client successfully connects
    /// </summary>
    public virtual void OnClientConnected()
    {

    }
}
