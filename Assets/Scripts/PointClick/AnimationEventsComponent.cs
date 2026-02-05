using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventsComponent : MonoBehaviour
{
    [SerializeField]
    private List<UnityEvent> _animationEvents = new List<UnityEvent>();

    public void PlayEvent(int id)
    {
        if (id < _animationEvents.Count)
            _animationEvents[id]?.Invoke();
        else
            Debug.LogError($"Animation id out of bounds {id}");
    }
}
