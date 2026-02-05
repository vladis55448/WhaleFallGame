using UnityEngine;
using UnityEngine.Events;

public class FirstPersonInteractive : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _events;

    public void Activate()
    {
        _events?.Invoke();
    }
}
