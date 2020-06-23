using UnityEditor;
using UnityEngine;

public class GameStateConsole : EditorWindow
{
    public GameServer gameServer;
    private Player aiPlayer;
    private Player localPlayer;

    [MenuItem("Tools/Game State Console")]
    static void DoIt()
    {
        EditorWindow.GetWindow<GameStateConsole>();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Refresh Variables"))
            RefreshReferences();

        if (gameServer)
        {
            GUILayout.Label("ACCORDING TO SERVER", EditorStyles.boldLabel);
            //SHOW PLAYERS ON THE SERVER
            for (int i = 0; i < gameServer.players.Count; i++)
            {
                DrawPlayer(gameServer.players[i]);
            }

            //HAS THE GAME BEEN STARTED
            GUILayout.Label($"Game Started:: {gameServer.gameStarted}");
        }

        GUILayout.Space(5);

        if (aiPlayer)
        {
            GUILayout.Label("ACCORDING TO REMOTE _AI", EditorStyles.boldLabel);
            DrawPlayer(aiPlayer.playerData);
            GUILayout.Label("opponent INFO");
            DrawPlayer(aiPlayer.opponentData);
        }

        if (localPlayer)
        {
            GUILayout.Label("ACCORDING TO LOCAL PLAYER", EditorStyles.boldLabel);
            DrawPlayer(localPlayer.playerData);
        }
    }

    /// <summary>
    /// will refresh all references in the editor
    /// </summary>
    public void RefreshReferences()
    {
        gameServer = FindObjectOfType<GameServer>();
        aiPlayer = FindObjectOfType<RemotePlayer_AI>();
        localPlayer = FindObjectOfType<LocalPlayer>();
    }

    //FUNCTION TO DRAW PLAYER DATA
    public void DrawPlayer(PlayerData _playerData)
    {
        GUILayout.Space(2);
        EditorGUILayout.LabelField(string.Format("PLAYER:: GUID -- {0},  Name -- {1},  Active -- {2}", _playerData.guid, _playerData.nickName, Player.GetPlayer(_playerData.guid).activePlayer.ToString()));

        //the net id
        EditorGUILayout.LabelField(string.Format("      ConnectionID:: {0}", (_playerData.m_connenction != null? _playerData.m_connenction.InternalId.ToString(): "Unassigned")));

        //the number of cards
        EditorGUILayout.LabelField(string.Format("      Hand Cards:: {0}", _playerData.handZone.cards.Count));

    }
}