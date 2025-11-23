using UnityEngine;

public class SpiderMovementController : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraRoot;
    [SerializeField]
    private Transform _followTarget;
    [SerializeField]
    private Vector3 _followOffset;
    [SerializeField]
    private ProcedualMovementController _procedualMovementController;
    [SerializeField]
    public float _sensitivity = 10f;
    [SerializeField]
    public float _maxYAngle = 80f;
    [SerializeField]
    public float _minYAngle = 80f;

    private Vector2 currentRotation;
    private Camera _camera;
    private void Start()
    {
        _camera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        currentRotation.x += Input.GetAxis("Mouse X") * _sensitivity;
        currentRotation.y -= Input.GetAxis("Mouse Y") * _sensitivity;
        currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
        currentRotation.y = Mathf.Clamp(currentRotation.y, _minYAngle, _maxYAngle);
        _cameraRoot.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
        _cameraRoot.position = _followTarget.position + _followOffset;
        var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        var forward = _camera.transform.right * input.x;
        var right = _camera.transform.forward * input.z;
        var cameraRelativeMovement = forward + right;
        var cameraDirection = _camera.transform.forward;
        _procedualMovementController.MoveInDirection(cameraRelativeMovement, cameraDirection);
    }
}
