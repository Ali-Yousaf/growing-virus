using UnityEngine;
using System.Collections;

public class CellInfection : MonoBehaviour
{
    [SerializeField] private GameObject infectTextGameObj;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Color infectedColor = Color.magenta;

    [SerializeField] private GameObject[] virusAllyPrefab;
    [SerializeField] private ParticleSystem bloodParticles;
    [SerializeField] private float infectionTime = 2f;

    private SpriteRenderer sp;

    [Header("Shake Settings")]
    [SerializeField] private float shakeIntensity = 0.1f;

    private bool playerNearby = false;
    private bool infected = false;

    private Vector3 originalPos;

    void Awake()
    {
        sp = GetComponent<SpriteRenderer>();    
    }

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

            sprite.color = Color.Lerp(startColor, infectedColor, timer / infectionTime);

            //shaking effect
            Vector2 randomOffset = Random.insideUnitCircle * shakeIntensity;
            transform.position = originalPos + (Vector3)randomOffset;

            yield return null;
        }

        transform.position = originalPos;
        sp.enabled = false;
        bloodParticles.Play();

        yield return new WaitForSeconds(0.7f);

        int randomIndex = Random.Range(0, virusAllyPrefab.Length);
        Instantiate(virusAllyPrefab[randomIndex], transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            infectTextGameObj.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            infectTextGameObj.SetActive(false);
        }
    }
}