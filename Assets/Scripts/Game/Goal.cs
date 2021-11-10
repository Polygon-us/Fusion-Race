using Tools.ServicesManager;
using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.transform.parent.GetComponent<Player>();

        if (player == ServicesManager.instance.Get<Player>())
        {
            if (player.Runner.IsServer)
                ServicesManager.instance.Get<GameEvents>()._Event = $"SetWiner>{player._IdText}";
            else
                player.SendReliableMessageFromPlayerToServer($"SetWiner>{player._IdText}");
        }

        gameObject.SetActive(false);
    }
}
