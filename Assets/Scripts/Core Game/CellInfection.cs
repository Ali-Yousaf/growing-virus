using UnityEngine;
using System.Collections;

public class CellInfection : MonoBehaviour
{
    [SerializeField] private GameObject infectTextGameObj;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Color infectedColor = Color.magenta;

    [SerializeField] private GameObject virusPrefab;
    [SerializeField] private ParticleSystem bloodParticles;
    [SerializeField] private float infectionTime = 2f;

    private bool playerNearby = false;
    private bool infected = false;

    void Awake()
    {
        
    }

    void Start()
    {
        infectTextGameObj.SetActive(false);
    }

    void Update()
    {
        if (playerNearby && !infected && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(InfectCell());
        }
    }

    IEnumerator InfectCell()
    {
        infected = true;

        Color startColor = sprite.color;
        float timer = 0f;

        while (timer < infectionTime)
        {
            timer += Time.deltaTime;
            sprite.color = Color.Lerp(startColor, infectedColor, timer / infectionTime);
            yield return null;
        }

        // Play particles
        if (bloodParticles != null)
            bloodParticles.Play();

        yield return new WaitForSeconds(0.3f);

        // Spawn new virus
        Instantiate(virusPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;

        infectTextGameObj.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;

        infectTextGameObj.SetActive(false);
    }
}