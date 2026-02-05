using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IndoorSceneController : MonoBehaviour
{
    [SerializeField]
    private Transform _scenarioParent;
    [SerializeField]
    private List<Contex> _sceneContexts = new List<Contex>();
    public event Action<OutdoorData> RequestLoadOutdoor;

    private const string SCENARIO_LOAD_PATH = "IndoorScenarios/";

    public void Init(IndoorData data)
    {
        foreach (var context in _sceneContexts)
        {
            if (data == context.Id)
            {
                var scenario = Resources.Load(SCENARIO_LOAD_PATH + context.PrefabPath);
                var instance = Instantiate(scenario, _scenarioParent);
                instance.GetComponent<IndoorScenarioController>().RequestLoadOutdoor += LoadOutdoorScene;
                break;
            }
        }
    }

    private void LoadOutdoorScene(OutdoorData contextId)
    {
        RequestLoadOutdoor?.Invoke(contextId);
    }

    [Serializable]
    private struct Contex
    {
        public IndoorData Id;
        public string PrefabPath;
    }
}
