using System;
using DG.Tweening;
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
    private float _stepDuration;
    [SerializeField]
    private float _stepHeight;
    [SerializeField]
    private ParticleSystem _stepParticles;
    [SerializeField]
    private LayerMask _mask;

    private Vector3 _previousPosition;
    private Vector3 _nextPosition;

    [HideInInspector]
    public bool CanMove = true;
    public Transform BodyTarget => _bodyTarget;
    public Transform RaycastTarget => _raycastRoot;

    public event Action<ProcedualLeg> StartedMove;
    public event Action<ProcedualLeg> CompletedMove;

    // Update is called once per frame
    private void Update()
    {
        var hit = new RaycastHit();
        if (Physics.Raycast(_raycastRoot.position, -_raycastRoot.up, out hit, 1000, _mask))
        {
            Debug.DrawLine(_raycastRoot.position, hit.point);
            if (Vector3.Distance(hit.point, _target.position) > _repositionDistance)
            {
                if (CanMove)
                {
                    _raycastRoot.forward = Vector3.ProjectOnPlane(_raycastRoot.parent.forward, hit.normal).normalized;
                    _previousPosition = _target.position;
                    _nextPosition = hit.point;
                    Move();
                }
            }
        }
    }

    private void Move()
    {
        CanMove = false;
        StartedMove?.Invoke(this);
        DOVirtual.Float(0, 1, _stepDuration, (value) =>
        {
            var position = Vector3.Lerp(_previousPosition, _nextPosition, value);
            position.y += Mathf.Sin(value * Mathf.PI) * _stepHeight;
            _target.position = position;
        }).SetEase(Ease.Linear).OnComplete(() =>
        {
            CompletedMove?.Invoke(this);
            _stepParticles.transform.position = _nextPosition;
            _stepParticles.Play();
        });
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
