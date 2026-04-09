using UnityEngine;

public class BoardController : MonoBehaviour
{
    public const string SensitivityPrefsKey = "GyroSensitivity";
    public const float DefaultSensitivity = 1.5f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;
    public float maxRotation = 10f;

    private Quaternion initialRotation;
    private bool useGyro = false;
    private Gyroscope gyro;
    private float sensitivity;

    void Start()
    {
        initialRotation = transform.rotation;
        sensitivity = PlayerPrefs.GetFloat(SensitivityPrefsKey, DefaultSensitivity);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

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
            Vector3 g = Input.gyro.gravity;

            float rotX = -g.y;
            float rotZ = g.x;

            rotX = Mathf.Sign(rotX) * Mathf.Pow(Mathf.Abs(rotX), 0.7f);
            rotZ = Mathf.Sign(rotZ) * Mathf.Pow(Mathf.Abs(rotZ), 0.7f);

            inputX = Mathf.Clamp(rotX * sensitivity, -1f, 1f);
            inputZ = Mathf.Clamp(rotZ * sensitivity, -1f, 1f);
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
