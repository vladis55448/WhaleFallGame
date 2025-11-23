using UnityEngine;
using UnityEngine.AI;

public class ProcedualAIWalker : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent;
    [SerializeField]
    private Transform _target;

    private NavMeshPath _navMeshPath;

    private void Update()
    {
        _agent.destination = _target.position;
    }

    private NavMeshPath CalculatePath(Transform target)
    {
        var path = new NavMeshPath();
        _agent.CalculatePath(target.position, path);
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.Log(path.corners[i]);
        }
        return path;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_navMeshPath == null)
            return;
        for (int i = 0; i < _navMeshPath.corners.Length - 1; i++)
        {
            Gizmos.DrawLine(_navMeshPath.corners[i], _navMeshPath.corners[i + 1]);
        }
    }
}
