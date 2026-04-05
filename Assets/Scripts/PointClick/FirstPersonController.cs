using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{

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
    private WalkZoneParams _defaultWalkParams;

    [SerializeField]
    private InventoryController _inventoryController;


    private float _walkSpeed = 5f;
    private float _stepDelay;
    private float _stepFrequency;
    private float _stepHeight;


    private float _stepTime = 0;
    private Vector3 _moveDirection = Vector3.zero;
    private float _rotationX = 0;
    private CharacterController _characterController;
    private bool _isFixed = false;
    private List<WalkZone> _zones = new List<WalkZone>();

    public void Fixate()
    {
        _isFixed = true;
    }

    public void Unlock()
    {
        _isFixed = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        SetWalkParams(_defaultWalkParams);
    }

    private void Update()
    {
        if (_isFixed)
            return;
        if (Input.GetKeyDown(KeyCode.I))
        {
            _isFixed = true;
            _inventoryController.Activate();
            _inventoryController.Closed += OnInventoryClosed;
        }
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

    private void OnInventoryClosed()
    {
        _inventoryController.Closed -= OnInventoryClosed;
        Unlock();
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

    private void OnCollisionEnter(Collision collision)
    {
        var walkZone = collision.gameObject.GetComponent<WalkZone>();
        if (walkZone != null)
        {
            if (_zones.Contains(walkZone))
                return;
            _zones.Add(walkZone);
            SetWalkParams(walkZone.Params);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        var walkZone = collision.gameObject.GetComponent<WalkZone>();
        if (walkZone != null)
        {
            _zones.Remove(walkZone);
            if (_zones.Count == 0)
            {
                SetWalkParams(_defaultWalkParams);
            }
            else
            {
                SetWalkParams(_zones[_zones.Count - 1].Params);
            }
        }
    }

    private void SetWalkParams(WalkZoneParams walkParams)
    {
        _walkSpeed = walkParams.Speed;
        _stepDelay = walkParams.StepDelay;
        _stepFrequency = walkParams.StepFrequency;
        _stepHeight = walkParams.StepHeight;
    }
}
