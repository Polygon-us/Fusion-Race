using Attributes;
using Fusion;
using Tools.ServicesManager;
using UnityEngine;

public class Ready : NetworkBehaviour
{
    #region Information

    private bool ready;

    [Title("Information")]
    public GameObject readyTextGlobePrefab;
    public GameObject readyTextGlobe;

    #endregion

    #region Properties

    public bool _Ready
    {
        get => ready;
        set => ready = value;
    }

    #endregion

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out GameInput gameInput))
        {
            if (gameInput.IsDown(GameInput.READY))
            {
                if (!ready)
                {
                    ready = true;

                    if(ServicesManager.instance.Get<Player>().gameObject.transform.GetSiblingIndex() == transform.GetSiblingIndex())
                        ServicesManager.instance.Get<GameManager>().CloseLevelZeroUI();

                    if (Runner.IsServer)
                        ServicesManager.instance.Get<GameEvents>()._Event = $"SetReady>{transform.GetSiblingIndex()}";
                }
            }
        }
    }
}
