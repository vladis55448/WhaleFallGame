using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField]
    private float _walkSpeed = 5f;
    [SerializeField]
    private float _sensitivityX = 2f;
    [SerializeField]
    private float _sensitivityY = 2f;
    [SerializeField]
    private Transform _cameraRoot;
    [SerializeField]
    private float _lookXLimit = 45f;
    [SerializeField]
    private GameObject _interactHint;

    private Vector3 _moveDirection = Vector3.zero;
    private float _rotationX = 0;
    private CharacterController _characterController;
    private bool _isFixed = false;

    public void Fixate()
    {
        _isFixed = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Unlock()
    {
        _isFixed = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (_isFixed)
            return;
        // Handle Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        float currentSpeedX = _walkSpeed * Input.GetAxis("Vertical");
        float currentSpeedY = _walkSpeed * Input.GetAxis("Horizontal");
        float movementDirectionY = _moveDirection.y;
        _moveDirection = (forward * currentSpeedX) + (right * currentSpeedY);

        // Move the Character
        _characterController.Move(_moveDirection * Time.deltaTime);

        // Handle Rotation and Camera Movement (Look Around)
        _rotationX += -Input.GetAxis("Mouse Y") * _sensitivityY;
        _rotationX = Mathf.Clamp(_rotationX, -_lookXLimit, _lookXLimit);
        _cameraRoot.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _sensitivityX, 0);
        CheckInteraction();


    }

    private void CheckInteraction()
    {
        RaycastHit hit;
        if (Physics.Raycast(_cameraRoot.position, _cameraRoot.forward, out hit, 4f))
        {
            var interactive = hit.transform.GetComponent<FirstPersonInteractive>();
            if (interactive != null)
            {
                _interactHint.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _interactHint.SetActive(false);
                    interactive.Activate();
                }
            }
        }
        else
        {
            _interactHint.SetActive(false);
        }
    }
}
