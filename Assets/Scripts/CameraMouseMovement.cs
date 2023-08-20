using UnityEngine;

public class CameraMouseMovement : MonoBehaviour
{
    [SerializeField]
    private float sensitivity = 2.0f; // Sensitivity control

    [SerializeField]
    private GameObject cameraObject; // Reference to the camera GameObject

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Adjust the camera's position based on mouse movement
        cameraObject.transform.Translate(new Vector3(mouseX, mouseY, 0));
    }
}
