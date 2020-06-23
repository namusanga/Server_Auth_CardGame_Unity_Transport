using System.Collections.Generic;
using Unity.Networking.Transport;

public class GameServer : Server
{
    public List<PlayerData> players = new List<PlayerData>();

    public int currentPlayer;
    public int inactivePlayer
    {
        get
        {
            if (currentPlayer == 0)
                return 1;
            return 0;
        }
    }

    public string[] cardNames = new string[] { "Card1", "Card2", "Card3", "Card4" };

    public int nextCardId = 0;

    public bool gameStarted = false;

    /// <summary>
    /// only one move is allowed per turn
    /// </summary>
    public int madeMoves = 0;

    public override void RegisterMessageHandlerClasses()
    {
        base.RegisterMessageHandlerClasses();

        //for registering players
        new Server_PlayerMessagesHandler(this);

        //handling played cards
        new Server_CardMessagesHandler(this);

        new Server_KeepAliveHandler(this);
    }

    internal void EndCurrentTurn()
    {
        //send end turn message to the current player
        SendNetMessage(ref m_Driver, players[currentPlayer].m_connenction, new EndTurnMessage()
        {
            recepientGUID = players[currentPlayer].guid,

            playerData = NetworkUtilities.GetNetPlayerData(players[currentPlayer]),

            opponentData = NetworkUtilities.GetNetPlayerData(players[inactivePlayer])

        }) ;
        //send start turn to the other player
        SendNetMessage(ref m_Driver, players[inactivePlayer].m_connenction, new StartTurnMessage()
        {
            recepientGUID = players[inactivePlayer].guid,

            playerData = NetworkUtilities.GetNetPlayerData(players[inactivePlayer]),

            opponentData = NetworkUtilities.GetNetPlayerData(players[currentPlayer])
        });

        madeMoves = 0;
        currentPlayer = inactivePlayer;
    }

    public override void UnregisterMessageHandlerClasses()
    {
        base.UnregisterMessageHandlerClasses();

        //for registering player
        RemoveMessageHandler(NetworkProtocal.RegisterPlayerMessage);

        //cards
        RemoveMessageHandler(NetworkProtocal.MoveCardMessage);
    }

    public void AddPlayerToList(PlayerData _player)
    {
        players.Add(_player);
    }


    public void StartGame()
    {
        //first player
        StartGameMessage startGameMessage = new StartGameMessage()
        {
            playerData = NetworkUtilities.GetNetPlayerData(players[0]),
            opponentData = NetworkUtilities.GetNetPlayerData(players[1])
        };

        SendNetMessage(ref m_Driver, players[0].m_connenction, startGameMessage);


        //second player
        startGameMessage = new StartGameMessage()
        {
            playerData = NetworkUtilities.GetNetPlayerData(players[1]),
            opponentData = NetworkUtilities.GetNetPlayerData(players[0])
        };

        SendNetMessage(ref m_Driver, players[1].m_connenction, startGameMessage);

        currentPlayer = players.IndexOf(players.Find(x => x.guid == 0));

        gameStarted = true;
    }

    public virtual void SendToClient(NetworkConnection _connection, MessageBase msg)
    {
        SendNetMessage(ref m_Driver, _connection, msg);
    }
}
