using Unity.Cinemachine;
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
    [SerializeField]
    private CinemachineImpulseSource _impulseSource;
    [SerializeField]
    private float _stepDelay;
    [SerializeField]
    private float _stepFrequency;
    [SerializeField]
    private float _stepHeight;


    private float _stepTime = 0;
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
    }

    private void Update()
    {
        if (_isFixed)
            return;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        float currentSpeedX = _walkSpeed * Input.GetAxis("Vertical");
        float currentSpeedY = _walkSpeed * Input.GetAxis("Horizontal");

        _moveDirection = (forward * currentSpeedX) + (right * currentSpeedY);
        if (_moveDirection.magnitude > 0)
        {
            _stepTime += Time.deltaTime * _stepFrequency;
            if (_stepTime - _stepDelay > 0.9)
            {
                _impulseSource.GenerateImpulse(Vector3.up * _stepHeight);
                _stepTime = 0;
            }
        }
        else
        {
            _stepTime = _stepDelay;
        }
        _moveDirection += Vector3.down * 9.8f;

        _characterController.Move(_moveDirection * Time.deltaTime);

        _rotationX += -Input.GetAxis("Mouse Y") * _sensitivityY;
        _rotationX = Mathf.Clamp(_rotationX, -_lookXLimit, _lookXLimit);
        _cameraRoot.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _sensitivityX, 0);
        CheckInteraction();
    }

    private void CheckInteraction()
    {
        RaycastHit hit;
        if (Physics.Raycast(_cameraRoot.position, _cameraRoot.forward, out hit, 2f))
        {
            var interactive = hit.transform.GetComponent<FirstPersonInteractive>();
            if (interactive != null)
            {
                _interactHint.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    _interactHint.SetActive(false);
                    interactive.Activate();
                }
            }
            else
            {
                _interactHint.SetActive(false);
            }
        }
        else
        {
            _interactHint.SetActive(false);
        }
    }
}
