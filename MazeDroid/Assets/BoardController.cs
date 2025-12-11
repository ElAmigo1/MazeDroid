 using UnityEngine;

public class BoardController : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;   // Sanfte Rotation
    public float maxRotation = 10f;    // Maximalwinkel

    private Quaternion initialRotation;
    private bool useGyro = false;
    private Gyroscope gyro;

    void Start()
    {
        initialRotation = transform.rotation;

        // Rigidbody sicherstellen
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // Gyro initialisieren
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            useGyro = true;
        }
    }

    void FixedUpdate()
    {
        float inputX, inputZ;

        if (useGyro)
        {
            // Gravity = Richtung der Schwerkraft relativ zum Gerät
            Vector3 g = Input.gyro.gravity;

            // g.x: Kippung nach links/rechts
            // g.y: Hoch/runter
            // g.z: Vor/Zurück kippen

            // Tablets liegen meist flacher: Achsen anpassen!        
            float sensitivity = 3f;

            float gx = -g.x;
            float gy = g.y;

            // Smooth nonlinear response (fühlt sich natürlicher an)
            gx = Mathf.Sign(gx) * Mathf.Pow(Mathf.Abs(gx), 0.7f);
            gy = Mathf.Sign(gy) * Mathf.Pow(Mathf.Abs(gy), 0.7f);

            inputX = Mathf.Clamp(gx * sensitivity, -1f, 1f);
            inputZ = Mathf.Clamp(gy * sensitivity, -1f, 1f);



        }
        else
        {
            inputX = Input.GetAxis("Vertical");
            inputZ = Input.GetAxis("Horizontal");
        }

        float targetX = Mathf.Clamp(inputX * maxRotation, -maxRotation, maxRotation);
        float targetZ = Mathf.Clamp(inputZ * maxRotation, -maxRotation, maxRotation);

        Quaternion targetRotation = initialRotation * Quaternion.Euler(targetX, 0f, targetZ);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );
    }


    /// <summary>
    /// Wandelt Gyro-Koordinaten in Unitys Weltkoordinatensystem um.
    /// </summary>
    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
