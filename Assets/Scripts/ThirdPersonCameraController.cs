using Unity.VisualScripting;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public Transform target; // player
    public PlayerMovement playerMovement;

    public float xRotationMax = 45f;
    public float xRotationMin = -15f;
    public float xSensitivity = 2f;
    public float ySensitivity = 2f;

    private float xRotation; // pitch

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        float mouseX = playerMovement.LookInput.x * xSensitivity;
        float mouseY = playerMovement.LookInput.y * ySensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, xRotationMin, xRotationMax);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
