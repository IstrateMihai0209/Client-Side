using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float sensitivity;
    [SerializeField]
    private Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
        //Don't show the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        ClientSend.CameraRotation(transform.localRotation);
    }
}