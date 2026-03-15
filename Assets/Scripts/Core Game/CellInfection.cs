using UnityEngine;
using System.Collections;

public class CellInfection : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject infectTextGameObj;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Color infectedColor = Color.magenta;
    [SerializeField] private ParticleSystem bloodParticles;

    [Header("Virus Spawn")]
    [SerializeField] private GameObject[] virusAllyPrefab;
    [SerializeField] private float rareSpawnChanceIndex1 = 0.1f; 
    [SerializeField] private float rareSpawnChanceIndex2 = 0.1f; 
    
    [Header("Infection Settings")]
    [SerializeField] private float infectionTime = 2f;

    [Header("Shake Settings")]
    [SerializeField] private float shakeIntensity = 0.1f;

    private bool playerNearby;
    private bool infected;
    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.position;
    }

    void Update()
    {
        if (playerNearby && !infected && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(InfectCell());
        }
    }

    public IEnumerator InfectCell()
    {
        infected = true;
        PlayerStats.Instance.COUNT_cellsInfected++;

        Color startColor = sprite.color;
        float timer = 0f;

        while (timer < infectionTime)
        {
            timer += Time.deltaTime;

            float progress = timer / infectionTime;
            sprite.color = Color.Lerp(startColor, infectedColor, progress);

            Vector2 randomOffset = Random.insideUnitCircle * shakeIntensity;
            transform.position = originalPos + (Vector3)randomOffset;

            yield return null;
        }

        transform.position = originalPos;

        if (sprite != null)
            sprite.enabled = false;

        if (bloodParticles != null)
            bloodParticles.Play();

        yield return new WaitForSeconds(0.7f);

        SpawnVirus();

        Destroy(gameObject);
    }

    void SpawnVirus()
    {
        if (virusAllyPrefab.Length == 0) return;

        float roll = Random.value;
        int spawnIndex = 0;

        if (virusAllyPrefab.Length > 1 && roll < rareSpawnChanceIndex1)
        {
            spawnIndex = 1;
        }
        else if (virusAllyPrefab.Length > 2 && roll < rareSpawnChanceIndex1 + rareSpawnChanceIndex2)
        {
            spawnIndex = 2;
        }

        Instantiate(virusAllyPrefab[spawnIndex], transform.position, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerNearby = true;

        if (infectTextGameObj != null)
            infectTextGameObj.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerNearby = false;

        if (infectTextGameObj != null)
            infectTextGameObj.SetActive(false);
    }
}