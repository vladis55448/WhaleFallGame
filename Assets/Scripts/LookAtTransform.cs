using UnityEngine;

public class LookAtTransform : MonoBehaviour
{
    [SerializeField]
    private Transform _startTarget;

    private Transform _currentTarget = null;

    private void Start()
    {
        if (_startTarget != null)
        {
            _currentTarget = _startTarget;
        }
    }

    private void Update()
    {
        if (_currentTarget == null)
            return;
        transform.LookAt(_currentTarget);
    }
}