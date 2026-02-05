using UnityEngine;

public class Positioner : MonoBehaviour
{
    [SerializeField]
    private Transform _transform;
    [SerializeField]
    private Transform _target;

    public void Reposition()
    {
        _transform.position = _target.position;
        _transform.rotation = _target.rotation;
    }
}
