using UnityEngine;

public class BallRespawn : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject ballPrefab;
    public Transform spawnPoint;
    public Transform mapParent;
    public float surfaceOffset = 0.5f;
    public float followSpeed = 10f;

    private Rigidbody rb;
    private static bool doorDestroyed = false;
    private static bool doorDestroyed2 = false;
    private bool hasTriggered = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void FixedUpdate()
    {
        if (hasTriggered && mapParent != null)
        {
            Vector3 targetPos = transform.position;
            targetPos.y = mapParent.position.y + surfaceOffset;

            rb.MovePosition(Vector3.Lerp(transform.position, targetPos, followSpeed * Time.fixedDeltaTime));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        // 🔵 KnopfZone → Door1
        if (other.CompareTag("KnopfZone"))
        {
            hasTriggered = true;

            StopBall();

            if (!doorDestroyed)
            {
                GameObject door = GameObject.FindWithTag("Door1");
                if (door != null)
                {
                    Destroy(door);
                    Destroy(other.gameObject);
                }
                doorDestroyed = true;
            }

            SpawnNewBall();
        }

        // 🔴 Knopf2 → Door2
        else if (other.CompareTag("Knopf2"))
        {
            hasTriggered = true;

            StopBall();

            if (!doorDestroyed2)
            {
                GameObject door2 = GameObject.FindWithTag("Door2");
                if (door2 != null)
                {
                    Destroy(door2);
                    Destroy(other.gameObject);
                }
                doorDestroyed2 = true;
            }

            SpawnNewBall();
        }
    }

    void StopBall()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        // 🔒 WICHTIG: nur in Scene erlauben
        if (mapParent != null && gameObject.scene.isLoaded)
        {
            transform.SetParent(mapParent);

            Vector3 newPosition = transform.position;
            newPosition.y = mapParent.position.y + surfaceOffset;
            transform.position = newPosition;
        }
    }

    // 🧱 Neuen Ball spawnen
    void SpawnNewBall()
    {
        if (ballPrefab != null && spawnPoint != null)
        {
            GameObject go = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);

                // 👉 Referenzen nachträglich setzen
                BallRespawn br = go.GetComponent<BallRespawn>();
                br.mapParent = mapParent;
                br.spawnPoint = spawnPoint;

                // Camera update
                Camera.main.GetComponent<CameraController>().ball = go;
                Camera.main.GetComponent<CameraController>().zRotation = 0.0f;
        }
        else
        {
            Debug.LogWarning("BallRespawn: BallPrefab oder SpawnPoint fehlt!");
        }
    }
}