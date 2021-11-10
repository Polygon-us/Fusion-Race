using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Tools.ServicesManager;
using UnityEngine;

public class GameEvents : NetworkBehaviour
{
    #region Information

    [Networked(OnChanged = nameof(OnChange))]
    [HideInInspector] public string _Event { get; set; }

    public Dictionary<string, Action<string>> events;

    #endregion

    public override void Spawned()
    {
        events = new Dictionary<string, Action<string>>()
        {
            {
                "SetName", (string receiveData) =>
                {
                    string[] data = receiveData.Split(',');

                    Transform player = ServicesManager.instance.Get<GameManager>()._Players.GetChild(int.Parse(data[0]));

                    player.GetComponentInChildren<TextMesh>().text = data[1];
                }
            },
            {
                "SetReady", (string receiveData) =>
                {
                    GameManager gameManger = ServicesManager.instance.Get<GameManager>();

                    if(gameManger._GameState == GameManager.GameState.NotReady)
                    {
                        Transform player = gameManger._Players.GetChild(int.Parse(receiveData));

                        Ready ready = player.GetComponent<Ready>();

                        if(!ready._Ready)
                            ready._Ready = true;

                        ready.readyTextGlobe= Instantiate(ready.readyTextGlobePrefab);

                        ready.readyTextGlobe.transform.SetParent(player, true);

                        ready.readyTextGlobe.GetComponent<Follow>().SetTarget(player.GetChild(0), new Vector3(0.5f, 0.5f, 0f));

                        if(Runner.IsServer)
                        {
                            GameManager gameManager = ServicesManager.instance.Get<GameManager>();

                            for (int i = 0; i < gameManager._Players.childCount; i++)
                            {
                                if(!gameManager._Players.GetChild(i).gameObject.GetComponent<Ready>()._Ready)
                                    return;
                            }

                            ServicesManager.instance.Get<Player>().Runner.Spawn(gameManager._LevelOneActivator, null, null);
                        }
                    }
                }
            },
            {
                "SetWiner", (string receiveData) =>
                {
                    ServicesManager.instance.Get<GameManager>().GameOver(receiveData);

                    StartCoroutine(RestarGameCoroutine());
                }
            },
        };

        ServicesManager.instance.Register(this, true);
    }

    public static void OnChange(Changed<GameEvents> changed)
    {
        string[] message = changed.Behaviour._Event.Split(">");

        if (changed.Behaviour.events.ContainsKey(message[0]))
            changed.Behaviour.events[message[0]](message[1]);
    }

    IEnumerator RestarGameCoroutine()
    {
        yield return new WaitForSeconds(5);

        ServicesManager.instance.Get<GameManager>().RestartGame();
    }
}
