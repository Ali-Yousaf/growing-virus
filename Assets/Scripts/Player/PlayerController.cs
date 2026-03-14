using UnityEngine;

public class VirusMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveForce = 8f;
    [SerializeField] private float maxSpeed = 4f;
    [SerializeField] private float driftAmount = 0.5f;
    [SerializeField] private float wobbleSpeed = 3f;
    [SerializeField] private float wobbleAmount = 5f;

    private Rigidbody2D rb;
    private Vector2 input;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        input = input.normalized;
    }

    void FixedUpdate()
    {
        // swimming force
        rb.AddForce(input * moveForce);

        // Limit max speed
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);

        // drifting movement
        Vector2 drift = new Vector2(
            Mathf.PerlinNoise(Time.time, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, Time.time) - 0.5f
        ) * driftAmount;

        rb.AddForce(drift);

        // Slight wobble rotation
        float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount;
        rb.rotation = wobble;
    }
}