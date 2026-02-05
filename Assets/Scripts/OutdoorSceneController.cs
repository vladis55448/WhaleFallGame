using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OutdoorSceneController : MonoBehaviour
{
    [SerializeField]
    private Transform _playerSpider;
    [SerializeField]
    private List<Contex> _sceneContexts = new List<Contex>();
    public event Action<IndoorData> RequestLoadIndoor;

    public void Init(OutdoorData contextId)
    {
        foreach (var context in _sceneContexts)
        {
            if (contextId == context.Id)
            {
                _playerSpider.position = context.StartTransform.position;
                _playerSpider.rotation = context.StartTransform.rotation;
                _playerSpider.gameObject.SetActive(true);
                context.StartEvents?.Invoke();
                break;
            }
        }
    }

    [Serializable]
    private struct Contex
    {
        public OutdoorData Id;
        public Transform StartTransform;
        public UnityEvent StartEvents;
    }
}
