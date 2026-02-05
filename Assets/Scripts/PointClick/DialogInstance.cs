using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogInstance : MonoBehaviour, IClickComponent
{
    [SerializeField]
    private string _dialogId;
    [SerializeField]
    private UnityEvent _onStart = null;

    [SerializeField]
    private UnityEvent _onComplete = null;
    [SerializeField]
    private List<DialogTriggeredEvent> _events;
    [SerializeField]
    private bool _disableOnComplete;

    public string Id => _dialogId;

    public void Click()
    {
        _onStart?.Invoke();
        DialogController.Instance.StartDialog(this);
    }

    public void ActivateEvent(string eventId)
    {
        foreach (var ev in _events)
        {
            if (ev.Id.Equals(eventId))
            {
                ev.UnityEvent?.Invoke();
            }
        }
    }

    public void Complete()
    {
        _onComplete?.Invoke();
        if (_disableOnComplete)
            gameObject.SetActive(false);
    }
}

[Serializable]
public class DialogTriggeredEvent
{
    public string Id;
    public UnityEvent UnityEvent;
}