using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField]
    private float _moveSpeed = 3;

    [Header("Look Settings")]
    [SerializeField]
    private float _lookSpeed = 180;
    [SerializeField, Range(0.1F, 10)]
    private float _lookSensitivity = 1;
    [SerializeField, Tooltip("The minimum and maximum look pitch.")]
    private Vector2 _pitchContraints = new Vector2(-85, +85);
    [SerializeField]
    private bool _shouldInvertPitch = false;

    [Header("Debug")]
    [SerializeField]
    private string _debugText;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _camera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    private void Update()
    {
        var moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
          );
        var lookInput = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
          );

        var moveDir = (moveInput.x * transform.right + moveInput.y * transform.forward).normalized;
        var moveDelta = _moveSpeed * Time.deltaTime * moveDir;
        _characterController.Move(moveDelta);

        var lookDelta = _lookSpeed * _lookSensitivity * Time.deltaTime; 
        _lookPitch += lookDelta * lookInput.y * (_shouldInvertPitch ? +1 : -1);
        _lookYaw += lookDelta * lookInput.x;
        _lookPitch = Mathf.Clamp(_lookPitch, _pitchContraints.x, _pitchContraints.y);

        transform.localRotation = Quaternion.Euler(0, _lookYaw, 0);
        _camera.transform.localRotation = Quaternion.Euler(_lookPitch, 0, 0);

        SetDebugText();
    }

    private void SetDebugText()
    {
        _debugText = $"Look X: {_lookPitch} Look Y: {_lookYaw}";
    }

    private Camera _camera;
    private CharacterController _characterController;

    private float _lookPitch;
    private float _lookYaw;
}
