using UnityEngine;
using UnityEngine.Events;

public class EventClickComponent : MonoBehaviour, IClickComponent
{
    [SerializeField]
    private UnityEvent _events;
    public void Click()
    {
        _events.Invoke();
    }
}
