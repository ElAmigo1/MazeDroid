using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject ball;
    public Vector3 offset;
    public float zRotation;
    public float followSpeed = 5f;  // Smooth Follow Geschwindigkeit

    void Start()
    {
        // Startposition setzen
        transform.position = ball.transform.position + offset;
        transform.rotation = Quaternion.Euler(90f, 180f, -180f); // statische Rotation
    }

    void LateUpdate()
    {
        // Zielposition nur für X, Y, Z (Offset vom Ball)
        Vector3 targetPosition = ball.transform.position + offset;

        // Smooth Follow (ohne Rotation um den Ball)
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Kamera bleibt statisch orientiert
        // transform.rotation wird hier nicht verändert
    }
}
