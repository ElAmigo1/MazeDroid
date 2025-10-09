using UnityEngine;

public class BoardController : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float maxRotation = 10f;
    void Start()
    {
        transform.localEulerAngles = Vector3.zero;
    }

    void FixedUpdate()
    {
        float rotationX = Input.GetAxis("Vertical") * rotationSpeed * Time.fixedDeltaTime;
        float rotationZ = Input.GetAxis("Horizontal") * rotationSpeed * Time.fixedDeltaTime;

        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation.x = (currentRotation.x > 180) ? currentRotation.x - 360 : currentRotation.x;
        currentRotation.z = (currentRotation.z > 180) ? currentRotation.z - 360 : currentRotation.z;

        float newRotationX = Mathf.Clamp(currentRotation.x + rotationX, -maxRotation, maxRotation);
        float newRotationZ = Mathf.Clamp(currentRotation.z - rotationZ, -maxRotation, maxRotation);

        transform.localEulerAngles = new Vector3(newRotationX, 0f, newRotationZ);
    }

}
