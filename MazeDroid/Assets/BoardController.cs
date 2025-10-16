using UnityEngine;

public class BoardController : MonoBehaviour
{
    public float rotationSpeed = 5f;   // Sanfte Rotation
    public float maxRotation = 10f;    // Maximalwinkel

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;

        // Rigidbody auf Map hinzufügen, falls nicht vorhanden
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // schnelle Bewegungen
    }

    void FixedUpdate()
    {
        float inputX = Input.GetAxis("Vertical");
        float inputZ = Input.GetAxis("Horizontal");

        float targetX = Mathf.Clamp(inputX * maxRotation, -maxRotation, maxRotation);
        float targetZ = Mathf.Clamp(inputZ * maxRotation, -maxRotation, maxRotation);

        Quaternion targetRotation = initialRotation * Quaternion.Euler(targetX, 0f, targetZ);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }
}
