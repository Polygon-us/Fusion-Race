using Attributes;
using Cinemachine;
using Fusion;
using System.Text;
using Tools.ServicesManager;
using UnityEngine;

public class Player : NetworkBehaviour
{
    #region Information

    [Title("Information/Components")]
    [SerializeField] private TextMesh idText;

    private Vector3 initialPosition;

    #endregion

    #region Properties

    public string _IdText
    {
        get => idText.text;
    }

    #endregion

    public void SendReliableMessageFromPlayerToServer(string message)
    {
        Runner.SendReliableDataFromClientToServer(Encoding.ASCII.GetBytes(message));
    }

    public override void Spawned() 
    {
        GameManager gameManager = ServicesManager.instance.Get<GameManager>();

        if (gameManager._Players == null)
            gameManager._Players = new GameObject("Players [Generated]").transform;

        transform.SetParent(gameManager._Players, true);

        idText.text = transform.parent.childCount.ToString();

        if (Object.HasInputAuthority)
        {
            ServicesManager.instance.Get<CinemachineVirtualCamera>().Follow = transform.GetChild(0);

            initialPosition = transform.GetChild(1).position;

            ServicesManager.instance.Register(this);
        }
    }

    public void RestartPlayer()
    {
        transform.GetChild(1).position = initialPosition;

        GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
    }
}
