using UnityEngine;

public class LookAtTransform : MonoBehaviour
{
    [SerializeField]
    private Transform _startTarget;
    [SerializeField]
    private bool _lockToY = false;

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
        var dir = (_currentTarget.position - transform.position).normalized;
        var lookTrans = Quaternion.LookRotation(dir);
        var euler = lookTrans.eulerAngles;
        if (_lockToY)
        {
            euler.x = 0;
            euler.z = 0;
        }
        transform.eulerAngles = euler;
    }
}