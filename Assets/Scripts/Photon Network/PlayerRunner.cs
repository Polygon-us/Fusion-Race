using Attributes;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using Tools.ServicesManager;
using UnityEngine;

[RequireComponent(typeof(NetworkRunner))]
public class PlayerRunner : MonoBehaviour, INetworkRunnerCallbacks
{
    #region Information

    [Foldout("Information/Events/SpawnPlayer")]
    [SerializeField] private Player playerPrefab;

    private Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    #endregion

    private void Awake()
    {
        spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

        ServicesManager.instance.Register(this, true);
    }

    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnDisconnectedFromServer(NetworkRunner runner) { }

    public void OnInput(NetworkRunner runner, NetworkInput networkInput) 
    {
        var gameInput = new GameInput();
        
        if (Input.GetKey(KeyCode.R))
            gameInput.buttons |= GameInput.READY;
        
        if (Input.GetKey(KeyCode.W))
            gameInput.buttons |= GameInput.FORWARD;

        if (Input.GetKey(KeyCode.S))
            gameInput.buttons |= GameInput.BACKWARD;

        if (Input.GetKey(KeyCode.A))
            gameInput.buttons |= GameInput.LEFT;

        if (Input.GetKey(KeyCode.D))
            gameInput.buttons |= GameInput.RIGHT;
        if (Input.GetKey(KeyCode.Space))
            gameInput.buttons |= GameInput.JUMP;

        networkInput.Set(gameInput);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef playerRef, NetworkInput input) { }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef playerRef)
    {
        runner.Spawn(ServicesManager.instance.Get<GameManager>()._GameEvents, null, null);

        SpawnPlayer(runner, playerRef);
    }

    public virtual void SpawnPlayer(NetworkRunner runner, PlayerRef playerRef)
    {
        GameManager gameManager = ServicesManager.instance.Get<GameManager>();

        if (gameManager._Players == null)
            spawnedCharacters.Add(playerRef, runner.Spawn(playerPrefab, gameManager._SpawnPoints.GetChild(0).position, null, playerRef).Object);
        else
            spawnedCharacters.Add(playerRef, runner.Spawn(playerPrefab, gameManager._SpawnPoints.GetChild(gameManager._Players.childCount).position, null, playerRef).Object);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef playerRef) 
    {
        runner.Despawn(spawnedCharacters[playerRef]);

        Destroy(spawnedCharacters[playerRef].gameObject);

        spawnedCharacters.Remove(playerRef);
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef playerRef, ArraySegment<byte> data) 
    {
        ServicesManager.instance.Get<GameEvents>()._Event = System.Text.Encoding.Default.GetString(data);
    }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
}
