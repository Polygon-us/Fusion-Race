using Fusion;
using Tools.ServicesManager;

public class LevelOneActivator : MonoBehaviour
{
    public void Awake()
    {
        NetworkObject[] networkObjects = FindObjectsOfType<NetworkObject>();

        for (int i = 0; i < networkObjects.Length; i++)
        {
            Ready ready = networkObjects[i].gameObject.GetComponent<Ready>();

            if (ready != null)
            {
                if(ready.readyTextGlobe != null)
                    ready.readyTextGlobe.SetActive(false);
            }
        }

        GameManager gameManager = ServicesManager.instance.Get<GameManager>();

        gameManager._GameState = GameManager.GameState.Running;

        gameManager.CloseLevelZeroUI();

        gameManager.ActiveLevelOne();
    }
}
