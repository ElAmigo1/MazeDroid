using UnityEngine;

public class BallRespawn : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject ballPrefab;      // Prefab deines Balls
    public Transform spawnPoint;       // Referenz auf den Spawnpoint

    private Rigidbody rb;
    private bool hasTriggered = false; // verhindert mehrfaches Auslösen

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Wenn der Ball die Zone berührt
        if (!hasTriggered && other.CompareTag("KnopfZone"))
        {
            hasTriggered = true;

            // Ball anhalten (bleibt sichtbar & bleibt liegen)
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true; // friert Physik ein, damit er nicht mehr rollt oder fällt

            // Optional: leicht anheben, damit er schön „auf dem Knopf“ liegt
            transform.position = new Vector3(transform.position.x, other.transform.position.y + 0.5f, transform.position.z);

            // Neuen Ball am Spawnpoint erzeugen
            if (ballPrefab != null && spawnPoint != null)
            {
                Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                Debug.LogWarning("BallRespawn: BallPrefab oder SpawnPoint fehlt!");
            }
        }
    }
}
