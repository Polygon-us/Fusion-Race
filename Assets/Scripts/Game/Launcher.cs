using Attributes;
using Fusion;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Launcher : MonoBehaviour
{
    #region Information

    [Foldout("Information")]
    [SerializeField] private NetworkRunner playerRunner;

    #region UI
    [Foldout("Information/UI")]
    [SerializeField] private Button hostBtn;
    [Foldout("Information/UI")]
    [SerializeField] private Button joinBtn;
    [Foldout("Information/UI/Waiter")]
    [SerializeField] private GameObject waiter;
    [Foldout("Information/UI/Waiter")]
    [SerializeField] private float waiterSpeed = 100f;
    Coroutine moveWaiterCoroutine;
    #endregion

    Task starGameTask;

    #endregion

    public virtual void Host()
    {
        NetworkRunner playerRunner = Instantiate(this.playerRunner);

        playerRunner.name = "Player Runner [Generated]";

        starGameTask = playerRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Host,
            SessionName = "Default Room",
            Scene = SceneManager.GetActiveScene().buildIndex
        });

        hostBtn.interactable = false;

        joinBtn.interactable = false;

        moveWaiterCoroutine = StartCoroutine(MoveWaiterCoroutine());
    }

    public virtual void Join()
    {
        NetworkRunner playerRunner = Instantiate(this.playerRunner);

        playerRunner.name = "Player Runner [Generated]";

        starGameTask = playerRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Client,
            SessionName = "Default Room",
            Scene = SceneManager.GetActiveScene().buildIndex,
        });

        hostBtn.interactable = false;

        joinBtn.interactable = false;

        moveWaiterCoroutine = StartCoroutine(MoveWaiterCoroutine());
    }

    IEnumerator MoveWaiterCoroutine()
    {
        waiter.SetActive(true);

        Transform transform = waiter.transform;

        while (true)
        {
            transform.eulerAngles += new Vector3(0f, 0f, waiterSpeed) * Time.deltaTime;

            if (starGameTask.IsCompleted)
                End();

            yield return null;
        }
    }

    public void End()
    {
        StopAllCoroutines();

        gameObject.SetActive(false);
    }
}
