using UnityEngine;

/// <summary>
/// Handles trigger detection, door destruction, and ball respawning.
/// The ballPrefab field must point to the Ball prefab (Assets/Prefabs/Ball.prefab) —
/// the complete ball with Rigidbody, SphereCollider, BallRespawn, and BallSkinApplier.
/// The skin is re-applied automatically by BallSkinApplier.Start() using the saved PlayerPrefs index.
/// </summary>
public class BallRespawn : MonoBehaviour
{
    [Header("Ball Prefab")]
    [Tooltip("Assign Assets/Prefabs/Ball.prefab — the full ball with all components, NOT a skin prefab.")]
    public GameObject ballPrefab;

    [Header("Spawn Settings")]
    public Transform spawnPoint;

    // Instance fields — NOT static. Static fields persist across play sessions and
    // cause the second ball to spawn with stale door-destroyed state on re-runs.
    [HideInInspector] public bool doorDestroyed = false;
    [HideInInspector] public bool doorDestroyed2 = false;

    private Rigidbody rb;
    private bool hasTriggered = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("[BallRespawn] No Rigidbody on Ball!", gameObject);
            return;
        }

        // Unconditionally enable physics on every spawn — never rely on prefab defaults.
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("KnopfZone"))
        {
            hasTriggered = true;

            if (!doorDestroyed)
            {
                GameObject door = GameObject.FindWithTag("Door1");
                if (door != null) Destroy(door);
                Destroy(other.gameObject);
                doorDestroyed = true;
                DoorCounter.Instance?.RegisterDoorOpened();
            }

            SpawnNewBall();
        }
        else if (other.CompareTag("Knopf2"))
        {
            hasTriggered = true;

            if (!doorDestroyed2)
            {
                GameObject door2 = GameObject.FindWithTag("Door2");
                if (door2 != null) Destroy(door2);
                Destroy(other.gameObject);
                doorDestroyed2 = true;
                DoorCounter.Instance?.RegisterDoorOpened();
            }

            SpawnNewBall();
        }
    }

    private void SpawnNewBall()
    {
        if (ballPrefab == null)
        {
            Debug.LogError("[BallRespawn] ballPrefab is null — assign Assets/Prefabs/Ball.prefab in the Inspector!", gameObject);
            return;
        }
        if (spawnPoint == null)
        {
            Debug.LogError("[BallRespawn] spawnPoint is null!", gameObject);
            return;
        }

        Vector3 spawnPos = spawnPoint.position + Vector3.up * 1f;
        GameObject newBall = Instantiate(ballPrefab, spawnPos, Quaternion.identity);

        // Enforce physics before the new ball's Start() runs so no single frame is wrong.
        Rigidbody newRb = newBall.GetComponent<Rigidbody>();
        if (newRb != null)
        {
            newRb.isKinematic = false;
            newRb.useGravity = true;
            newRb.velocity = Vector3.zero;
            newRb.angularVelocity = Vector3.zero;
            newRb.interpolation = RigidbodyInterpolation.Interpolate;
            newRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        // Forward door state and config to the new ball instance.
        BallRespawn next = newBall.GetComponent<BallRespawn>();
        if (next != null)
        {
            next.ballPrefab = ballPrefab;
            next.spawnPoint = spawnPoint;
            next.doorDestroyed = doorDestroyed;
            next.doorDestroyed2 = doorDestroyed2;
        }

        // Point the camera at the new ball.
        CameraController cam = Camera.main != null ? Camera.main.GetComponent<CameraController>() : null;
        if (cam != null)
            cam.ball = newBall;

        Destroy(gameObject);
    }
}