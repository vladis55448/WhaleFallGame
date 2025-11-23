using UnityEngine;

public class ProcedualFollower : MonoBehaviour
{
    [SerializeField]
    private ProcedualMovementController _movementController;
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private Transform _root;
    [SerializeField]
    private float _distanceFollow;

    private void Update()
    {
        if (Vector3.Distance(_root.position, _target.position) < _distanceFollow)
            return;
        var direction = (_target.position - _root.position).normalized;
        _movementController.MoveInDirection(direction, direction);
    }
}
