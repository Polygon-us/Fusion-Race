using Attributes;
using Fusion;
using UnityEngine;

public class ZigZagMovement : NetworkBehaviour
{
    #region Information

    [Title("Information")]
    [SerializeField] private float timeToMove = 2.5f;

    private float initialPosition;
    private float finalPosition;
    private float time;

    #endregion

    #region Components

    new Transform transform;
    NetworkTransform networkTransform;

    #endregion

    public override void Spawned()
    {
        transform = GetComponent<Transform>();

        networkTransform = GetComponentInParent<NetworkTransform>();

        if (Runner.IsServer)
        {
            initialPosition = transform.position.x;

            finalPosition = -transform.position.x;

            time = Time.time;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Runner.IsServer)
        {
            if (Time.time <= time + timeToMove)
                networkTransform.Teleport(new Vector3(initialPosition + ((finalPosition - initialPosition) * ((Time.time - time) / timeToMove)), transform.position.y, transform.position.z));
            else
            {
                initialPosition = transform.position.x;

                finalPosition = -transform.position.x;

                time = Time.time;
            }
        }
    }
}
