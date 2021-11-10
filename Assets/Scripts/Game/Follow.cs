using Attributes;
using UnityEngine;

public class Follow : MonoBehaviour
{
    #region Information

    [Foldout("Information")]
    [SerializeField] private Transform targetTransform;
    [Foldout("Information")]
    [SerializeField] private Vector3 offset;

    #region Components
    private new Transform transform;
    #endregion

    #endregion

    private void Awake()
    {
        transform = GetComponent<Transform>();

        if (targetTransform != null)
            offset = transform.position - targetTransform.position;
    }

    private void Update()
    {
        if (targetTransform != null)
        {
            if (Vector3.Distance(transform.position, targetTransform.position + offset) < 0.5f)
                transform.position = Vector3.MoveTowards(transform.position, targetTransform.position + offset, 1f);
            else
                transform.position = targetTransform.position + offset;
        }
    }

    public void SetTarget(Transform targetTransform, Vector3 offset)
    {
        this.offset = offset;

        this.targetTransform = targetTransform;
    }
}
