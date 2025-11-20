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
            // Gyro liefert Orientierung in Gerätkoordinaten → muss angepasst werden
            Quaternion deviceRotation = GyroToUnity(gyro.attitude);
            Vector3 tilt = deviceRotation * Vector3.forward;

            // Wir lesen die Neigung (x/z) aus
            inputX = Mathf.Clamp(tilt.x * 2f, -1f, 1f);
            inputZ = Mathf.Clamp(tilt.z * 2f, -1f, 1f);
        }
        else
        {
            // Fallback für Editor/PC
            inputX = Input.GetAxis("Vertical");
            inputZ = Input.GetAxis("Horizontal");
        }

        float targetX = Mathf.Clamp(inputX * maxRotation, -maxRotation, maxRotation);
        float targetZ = Mathf.Clamp(inputZ * maxRotation, -maxRotation, maxRotation);

        Quaternion targetRotation = initialRotation * Quaternion.Euler(targetX, 0f, targetZ);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        // Optional: Kamera-Interaktion beibehalten
        if (Camera.main != null && Camera.main.GetComponent<CameraController>() != null)
        {
            Camera.main.GetComponent<CameraController>().zRotation += inputZ;
        }
    }

    /// <summary>
    /// Wandelt Gyro-Koordinaten in Unitys Weltkoordinatensystem um.
    /// </summary>
    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
