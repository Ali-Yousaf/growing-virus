using UnityEngine;

public class CellIdleShake : MonoBehaviour
{
    [SerializeField] private float moveRadius = 0.05f;
    [SerializeField]private float moveSpeed = 2f;

    private Vector3 startPos;
    private Vector3 targetOffset;

    void Start()
    {
        startPos = transform.position;
        PickNewTarget();
    }

    void Update()
    {
        Vector3 targetPosition = startPos + targetOffset;
        moveSpeed = Random.Range(1f, 4f);
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * moveSpeed
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        targetOffset = new Vector3(
            Random.Range(-moveRadius, moveRadius),
            Random.Range(-moveRadius, moveRadius),
            0f
        );
    }
}