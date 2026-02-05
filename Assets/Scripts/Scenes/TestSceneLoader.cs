using UnityEngine;

public class TestSceneLoader : MonoBehaviour
{
    [SerializeField]
    private ScenesController _scenesController;

    [SerializeField]
    private SceneType _sceneType;

    [SerializeField]
    private IndoorData _indoorData;

    [SerializeField]
    private OutdoorData _outDoorData;

    private enum SceneType
    {
        Indoor,
        Outdoor
    }

    private void Start()
    {
        if (_sceneType == SceneType.Indoor)
        {
            _scenesController.LoadIndoorScene(_indoorData);
        }
        else
        {
            _scenesController.LoadOutdoorScene(_outDoorData);
        }
    }
}
