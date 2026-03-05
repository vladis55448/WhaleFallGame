using UnityEngine;
using UnityEngine.Events;

public class EventOnStart : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onStart = null;

    void Start()
    {
        _onStart?.Invoke();
    }
}
