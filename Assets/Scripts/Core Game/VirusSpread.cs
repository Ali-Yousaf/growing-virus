using UnityEngine;

public class VirusAutoSpread : MonoBehaviour
{
    public float moveSpeed = 3f;

    [Header("Detection")]
    public float detectionRadius = 3f;
    public LayerMask cellLayer;

    [Header("Swarm Behavior")]
    public Transform player;
    public float maxDistanceFromPlayer = 6f;

    private Transform targetCell;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        // Stay near player
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer > maxDistanceFromPlayer)
        {
            MoveTowards(player.position);
            return;
        }

        // If no target, search for one
        if (targetCell == null)
            FindTarget();

        // Move to target
        if (targetCell != null)
        {
            MoveTowards(targetCell.position);
        }
    }

    void FindTarget()
    {
        Collider2D[] cells = Physics2D.OverlapCircleAll(
            transform.position,
            detectionRadius,
            cellLayer
        );

        float closestDist = Mathf.Infinity;
        Transform closestCell = null;

        foreach (Collider2D cell in cells)
        {
            float dist = Vector2.Distance(transform.position, cell.transform.position);

            if (dist < closestDist)
            {
                closestDist = dist;
                closestCell = cell.transform;
            }
        }

        targetCell = closestCell;
    }

    void MoveTowards(Vector3 target)
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            target,
            moveSpeed * Time.deltaTime
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("RedCell"))
        {
            // Trigger infection on the cell
            CellInfection cell = other.GetComponent<CellInfection>();
            if (cell != null)
            {
                cell.StartCoroutine(cell.InfectCell());
            }

            targetCell = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}