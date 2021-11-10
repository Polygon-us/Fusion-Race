using Attributes;
using Fusion;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    #pragma warning disable CS0109

    #region Information

    [Title("Information")]
    [SerializeField] private float speed = 5000f;
    [Title("Information/Jump")]
    [SerializeField] private float force = 2500f;
    [SerializeField] private bool jump = false;
    RaycastHit hit;

    #region Components
    [Title("Information/Components")]
    [SerializeField] private new Transform transform;
    [SerializeField] private new SphereCollider collider;
    [SerializeField] private new Rigidbody rigidbody;
    #endregion

    #endregion

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out GameInput gameInput))
        {
            float speed = this.speed * Runner.DeltaTime;

            if (gameInput.IsDown(GameInput.FORWARD))
                rigidbody.AddForce(Vector3.forward * speed);
            else if (gameInput.IsDown(GameInput.BACKWARD))
                rigidbody.AddForce(Vector3.back * speed);

            if (gameInput.IsDown(GameInput.LEFT))
                rigidbody.AddForce(Vector3.left * speed);
            else if (gameInput.IsDown(GameInput.RIGHT))
                rigidbody.AddForce(Vector3.right * speed);

            if (gameInput.IsDown(GameInput.JUMP))
            {
                if (!jump)
                {
                    jump = true;

                    rigidbody.AddForce(Vector3.up * force);
                }
            }
        }

        if (jump)
        {
            if (rigidbody.velocity.y <= 0)
            {
                if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y - collider.radius / 2f, transform.position.z), Vector3.down, out hit, 0.1f))
                    jump = false;
            }
        }

        if (transform.position.y <= -5f)
            GetComponentInParent<Player>().RestartPlayer();
    }
}
