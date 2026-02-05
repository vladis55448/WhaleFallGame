using UnityEngine;

public class Compass : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private Transform _compasCenter;
    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private float _compasRadius;
    [SerializeField]
    private int _angleStep;
    [SerializeField]
    private GameObject _segmentPrefab;

    private void Start()
    {
        for (int i = 0; i < 360; i += _angleStep)
        {
            var segment = Instantiate(_segmentPrefab, _compasCenter);
            var z = Mathf.Cos(Mathf.Deg2Rad * i);
            var x = Mathf.Sin(Mathf.Deg2Rad * i);
            segment.transform.localPosition = new Vector3(x, 0, z) * _compasRadius;
            segment.transform.eulerAngles = new Vector3(0, i, 0);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        var euler = new Vector3();
        euler.y = -_target.eulerAngles.y;
        euler.y = ((int)euler.y / _angleStep) * _angleStep;
        var rot = Quaternion.Lerp(_compasCenter.rotation, Quaternion.Euler(euler), Time.deltaTime * _rotationSpeed);
        _compasCenter.rotation = rot;
    }
}
