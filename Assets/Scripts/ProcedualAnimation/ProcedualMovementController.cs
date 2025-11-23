using UnityEngine;

public class ProcedualMovementController : MonoBehaviour
{
    [SerializeField]
    private Transform _root;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private Transform _yRoot;
    [SerializeField]
    private LayerMask _mask;
    [SerializeField]
    private float _distanceFromGround;

    private Vector3 _targetMovement;
    private Vector3 _targetDirection;

    private void FixedUpdate()
    {
        if (_targetMovement.magnitude < 0.2f)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
        var movement = _targetMovement * _speed;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10, _mask))
        {
            var y = hit.point.y + _distanceFromGround;
            _rigidbody.position = new Vector3(_rigidbody.position.x, y, _rigidbody.position.z);
        }
        _rigidbody.linearVelocity = movement;
        var direction = Vector3.RotateTowards(transform.forward, _targetDirection, Time.fixedDeltaTime * _rotationSpeed, 0);
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        _rigidbody.MoveRotation(rotation);
    }

    public void MoveInDirection(Vector3 movement, Vector3 direction)
    {
        _targetMovement = Vector3.ProjectOnPlane(movement, transform.up).normalized;
        _targetDirection = Vector3.ProjectOnPlane(direction, transform.up).normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.up);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, _root.up);
    }
}
