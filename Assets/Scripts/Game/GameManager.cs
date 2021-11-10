using Attributes;
using Tools.ServicesManager;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    #region Information

    [Foldout("Information")]
    [SerializeField] private Transform players;
    [Foldout("Information")]
    [SerializeField] private Transform spawnPoints;

    #region Levels
    #region Levels Zero
    [Foldout("Information/Levels/Level Zero")]
    [SerializeField] private GameObject levelZero;
    [Foldout("Information/Levels/Level Zero")]
    [SerializeField] private GameObject levelZeroUI;
    #endregion
    #region Levels One
    [Foldout("Information/Levels/Level One")]
    [SerializeField] private Fusion.NetworkObject levelOneActivator;
    [Foldout("Information/Levels/Level One")]
    [SerializeField] private GameObject levelOne;
    [Foldout("Information/Levels/Level One")]
    [SerializeField] private Fusion.NetworkObject levelOneSecondObstacle;
    [Foldout("Information/Levels/Level One")]
    [SerializeField] private Transform levelOneSecondObstaclesSpawnPoints;
    #endregion

    [Foldout("Information/Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [Foldout("Information/Game Over")]
    [SerializeField] private TMPro.TMP_Text winnerText;
    #endregion

    #region Components
    [Foldout("Information/Components")]
    [SerializeField] private Fusion.NetworkObject gameEvents;
    #endregion

    #endregion

    #region Properties

    public GameState _GameState { get; set; }

    public Transform _Players
    {
        get => players;
        set
        {
            if (players == null)
                players = value;
        }
    }

    public Transform _SpawnPoints
    {
        get => spawnPoints;
    }

    public Fusion.NetworkObject _GameEvents
    {
        get => gameEvents;
    }

    #region Levels
    public Fusion.NetworkObject _LevelOneActivator
    {
        get => levelOneActivator;
    }
    #endregion

    #endregion

    public void SetName(TMPro.TMP_InputField TMP_inputField)
    {
        if (_GameState < GameState.Ended)
        {
            Player player = ServicesManager.instance.Get<Player>();

            if (player.Runner.IsServer)
                ServicesManager.instance.Get<GameEvents>()._Event = $"SetName>0,{TMP_inputField.text}";
            else
                player.SendReliableMessageFromPlayerToServer($"SetName>{player.gameObject.transform.GetSiblingIndex()},{TMP_inputField.text}");
        }
    }

    public void CloseLevelZeroUI()
    {
        if(levelZero.activeSelf)
            levelZeroUI.SetActive(false);
    }

    public void ActiveLevelOne()
    {
        levelOne.SetActive(true);

        levelZero.SetActive(false);

        Fusion.NetworkRunner runner = ServicesManager.instance.Get<Player>().Runner;

        if (runner != null)
        {
            if (runner.IsServer)
            {
                for (int i = 0; i < levelOneSecondObstaclesSpawnPoints.childCount; i++)
                    runner.Spawn(levelOneSecondObstacle, levelOneSecondObstaclesSpawnPoints.GetChild(i).position, null);
            }
        }
    }

    public void GameOver(string winner)
    {
        winnerText.text = $"Ganador: {winner}";

        gameOverPanel.SetActive(true);

        _GameState = GameState.Ended;
    }

    public void RestartGame()
    {
        winnerText.text = "";

        gameOverPanel.SetActive(false);

        for (int i = 0; i < players.childCount; i++)
            players.GetChild(i).gameObject.GetComponent<Player>().RestartPlayer();

        _GameState = GameState.Running;
    }
}
