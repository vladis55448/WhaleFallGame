using DG.Tweening;
using UnityEngine;

public class HarpoonProjectile : MonoBehaviour
{
    [SerializeField]
    private Transform _pullInPoint;
    [SerializeField]
    private float _startForce;
    [SerializeField]
    private GameObject _visuals;
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private Transform _linePoint;
    [SerializeField]
    private Transform _endPoint;


    [HideInInspector]
    public State CurrentState { get; private set; } = State.Rest;

    public enum State
    {
        Rest,
        Released,
        Stuck,
        Returning
    }

    private IHarpoonTarget _target;


    public Vector3 ConnectionPosition => _linePoint.position;

    private void Update()
    {
        transform.rotation = _pullInPoint.rotation;
    }

    public void Launch(Vector3 position, Vector3 direction)
    {
        _visuals.SetActive(true);
        CurrentState = State.Released;
        transform.position = position;
        transform.forward = direction;
        _rigidbody.isKinematic = false;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.AddForce(transform.forward * _startForce, ForceMode.Impulse);
    }

    public void Return(bool poolTarget)
    {
        CurrentState = State.Returning;
        _rigidbody.isKinematic = true;
        DOVirtual.Float(0, 1, 1f, (value) =>
        {
            transform.position = Vector3.Lerp(transform.position, _pullInPoint.position, value);
        }).SetEase(Ease.InQuint).OnComplete(() =>
        {
            if (poolTarget && _target != null)
            {
                _target.PoolCompleted();
            }
            _target = null;
            CurrentState = State.Rest;
            _visuals.SetActive(false);
        });
    }

    public void Pull()
    {
        if (CurrentState != State.Stuck)
            return;
        if (_target != null)
        {
            if (_target.Pull())
            {
                _target.PoolOut(transform, 1f);
                Return(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CurrentState == State.Stuck)
            return;
        var target = other.transform.GetComponent<IHarpoonTarget>();
        if (target != null)
        {
            _target = target;
            target.Hit(_endPoint.position);
            _rigidbody.isKinematic = true;
            CurrentState = State.Stuck;
        }
        else
        {
            Return(false);
        }
    }
}
