using Unity.Cinemachine;
using UnityEngine;

public class HarpoonController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private Transform _verticalBase;
    [SerializeField]
    private Transform _horizontalBase;
    [SerializeField]
    private Transform _launchPoint;
    [SerializeField]
    private LayerMask _mask;
    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private HarpoonProjectile _projectile;
    [SerializeField]
    private float _maxDistance;
    [SerializeField]
    private ParticleSystem _particles;
    [SerializeField]
    private LineRenderer _line;
    [SerializeField]
    private CinemachineImpulseSource _cinemachineImpulseSource;

    // Update is called once per frame
    private void Update()
    {
        Vector3 direction = new Vector3();
        if (_projectile.CurrentState == HarpoonProjectile.State.Rest)
        {
            RaycastHit hit;
            _line.positionCount = 0;
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 100, _mask))
            {
                direction = (hit.point - transform.position).normalized;
                if (Input.GetMouseButtonDown(0) && hit.collider.GetComponent<IHarpoonTarget>() != null)
                {
                    _cinemachineImpulseSource.GenerateImpulse(1);
                    _particles.Play();
                    _projectile.Launch(_launchPoint.position, _launchPoint.forward);
                }
            }
            else
            {
                var cameraPoint = _camera.transform.position + _camera.transform.forward * 100;
                direction = (cameraPoint - transform.position).normalized;
            }
        }
        else
        {
            direction = (_projectile.transform.position - transform.position).normalized;
            if (Vector3.Distance(_projectile.transform.position, transform.position) > _maxDistance)
            {
                _projectile.Return(false);
                return;
            }
            _line.positionCount = 2;
            _line.SetPositions(new Vector3[]
            {
                _launchPoint.position,
                _projectile.ConnectionPosition
            });
            if (_projectile.CurrentState == HarpoonProjectile.State.Stuck)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _cinemachineImpulseSource.GenerateImpulse(0.2f);
                    _projectile.Pull();
                }

                if (Input.GetMouseButtonDown(1))
                {
                    _projectile.Return(false);
                    return;
                }
            }
        }

        direction = Vector3.Lerp(_horizontalBase.forward, direction, _rotationSpeed * Time.deltaTime);
        var rotation = Quaternion.LookRotation(direction).eulerAngles;
        _verticalBase.eulerAngles = new Vector3(0, rotation.y, 0);
        _horizontalBase.localEulerAngles = new Vector3(rotation.x, 0, 0);
    }
}
