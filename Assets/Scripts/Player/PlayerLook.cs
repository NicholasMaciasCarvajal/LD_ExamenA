using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Camera playerCamera;

    [Header("Sensibilidad")]
    [SerializeField] private float rotationSensibility = 100f;

    private float cameraVerticalAngle;
    private Vector2 lookInput;

    private PlayerInputAction inputActions;

    private void Awake()
    {
        playerCamera = Camera.main;
        inputActions = new PlayerInputAction();

        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        Look();
    }

    private void Look()
    {
        Vector2 mouseDelta = lookInput * rotationSensibility * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseDelta.x);

        cameraVerticalAngle += mouseDelta.y;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -60f, 60f);

        playerCamera.transform.localRotation = Quaternion.Euler(-cameraVerticalAngle, 0f, 0f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
