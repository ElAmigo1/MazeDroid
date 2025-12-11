using UnityEngine;

public class BallRespawn : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject ballPrefab;      // Prefab des Balls
    public Transform spawnPoint;       // Spawnposition
    public Transform mapParent;        // Map, auf der der Ball liegen soll
    public float surfaceOffset = 0.5f; // Abstand über Map-Oberfläche
    public float followSpeed = 10f;    // Wie schnell der Ball vertikal angepasst wird

    private Rigidbody rb;
    private static bool doorDestroyed = false;
    private bool hasTriggered = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Optional: Interpolation für glatte Physik
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void FixedUpdate()
    {
        // Wenn der Ball auf der Map liegen soll (nach Knopf-Zone)
        if (hasTriggered && mapParent != null)
        {
            // Sanfte Anpassung der Y-Position über der Map
            Vector3 targetPos = transform.position;
            targetPos.y = mapParent.position.y + surfaceOffset;
            rb.MovePosition(Vector3.Lerp(transform.position, targetPos, followSpeed * Time.fixedDeltaTime));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("KnopfZone"))
        {
            hasTriggered = true;

            // Ball stoppen
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true; // friert Physik ein

            // Ball als Child der Map setzen
            if (mapParent != null)
            {
                transform.SetParent(mapParent);
                Vector3 newPosition = transform.position;
                newPosition.y = mapParent.position.y + surfaceOffset;
                transform.position = newPosition;

            }

            // Tür einmal zerstören
            if (!doorDestroyed)
            {
                GameObject door = GameObject.FindWithTag("Door1");
                GameObject Cube = GameObject.FindWithTag("KnopfZone");

                if (door != null)
                {
                    Destroy(door);
                    Destroy(Cube);
                }
                doorDestroyed = true;

            }

            // Neuen Ball am Spawnpoint erzeugen
            if (ballPrefab != null && spawnPoint != null)
            {
                GameObject go = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
                Camera.main.GetComponent<CameraController>().ball = go;
                Camera.main.GetComponent<CameraController>().zRotation = 0.0f;

            }
            else
            {
                Debug.LogWarning("BallRespawn: BallPrefab oder SpawnPoint fehlt!");
            }
        }
    }
}
