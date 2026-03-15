using UnityEngine;

public class RedCellSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject redCellPrefab;

    [Header("Spawn Control")]
    public bool spawning = true;
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private int maxCells = 50;

    [Header("Spawn Area")]
    [SerializeField] private float spawnRadius = 12f;
    [SerializeField] private float minSpawnDistance = 3f;

    [Header("Spawn Validation")]
    [SerializeField] private LayerMask cellLayer;
    [SerializeField] private float overlapCheckRadius = 0.5f;

    private float spawnTimer;

    void Update()
    {
        if (!spawning || player == null || redCellPrefab == null)
            return;

        // follow player
        transform.position = player.position;

        // limit total cells
        if (GameObject.FindGameObjectsWithTag("RedCell").Length >= maxCells)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnRate)
        {
            spawnTimer = 0f;
            TrySpawnCell();
        }
    }

    void TrySpawnCell()
    {
        for (int i = 0; i < 5; i++) 
        {
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;

            if (randomOffset.magnitude < minSpawnDistance)
                continue;

            Vector3 spawnPos = player.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

            Collider2D hit = Physics2D.OverlapCircle(spawnPos, overlapCheckRadius, cellLayer);

            if (hit == null)
            {
                Instantiate(redCellPrefab, spawnPos, Quaternion.identity);
                return;
            }
        }
    }
}