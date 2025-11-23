using Unity.VisualScripting;
using UnityEngine;

public class ProcedualLeg : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private Transform _bodyTarget;
    [SerializeField]
    private Transform _raycastRoot;
    [SerializeField]
    private float _repositionDistance;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _stepHeight;
    [SerializeField]
    private ParticleSystem _stepParticles;
    [SerializeField]
    private LayerMask _mask;

    private Vector3 _previousPosition;
    private Vector3 _nextPosition;
    private float _lerp = 1;

    [HideInInspector]
    public bool CanMove = true;
    public Transform BodyTarget => _bodyTarget;
    public Transform RaycastTarget => _raycastRoot;
    public bool IsMoving => _lerp < 1;

    // Update is called once per frame
    private void Update()
    {
        var hit = new RaycastHit();
        if (Physics.Raycast(_raycastRoot.position, -_raycastRoot.up, out hit, 100, _mask))
        {
            Debug.DrawLine(_raycastRoot.position, hit.point);
            if (Vector3.Distance(hit.point, _target.position) > _repositionDistance)
            {
                if (_lerp >= 1 && CanMove)
                {
                    _raycastRoot.forward = Vector3.ProjectOnPlane(_raycastRoot.parent.forward, hit.normal).normalized;
                    _previousPosition = _target.position;
                    _nextPosition = hit.point;
                    _lerp = 0;
                }
            }
        }
        if (_lerp < 1)
        {
            var position = Vector3.Lerp(_previousPosition, _nextPosition, _lerp);
            position.y += Mathf.Sin(_lerp * Mathf.PI) * _stepHeight;
            _target.position = position;
            _lerp += Time.deltaTime * _speed;
            if (_lerp >= 1)
            {
                _stepParticles.transform.position = _nextPosition;
                _stepParticles.Play();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (_target != null)
            Gizmos.DrawSphere(_target.position, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_nextPosition, 0.2f);
        Gizmos.color = Color.blue;
        if (_raycastRoot != null)
        {
            Gizmos.DrawSphere(_raycastRoot.position, 0.2f);
            Gizmos.DrawLine(_raycastRoot.position, _raycastRoot.position - _raycastRoot.parent.up * 10);
        }
    }
}
