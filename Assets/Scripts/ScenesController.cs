using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{
    [SerializeField]
    private string _indoorSceneName;
    [SerializeField]
    private string _outdoorSceneName;

    private IndoorData _indoorData;
    private OutdoorData _outdoorData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadIndoorScene(IndoorData data)
    {
        _indoorData = data;
        SceneManager.sceneLoaded += OnIndoorLoaded;
        SceneManager.LoadScene(_indoorSceneName, LoadSceneMode.Single);
    }

    private void OnIndoorLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnIndoorLoaded;
        var controller = FindAnyObjectByType<IndoorSceneController>();
        controller.RequestLoadOutdoor += LoadOutdoorScene;
        controller.Init(_indoorData);
    }

    public void LoadOutdoorScene(OutdoorData data)
    {
        _outdoorData = data;
        SceneManager.sceneLoaded += OnOutDoorLoaded;
        SceneManager.LoadScene(_outdoorSceneName, LoadSceneMode.Single);
    }

    private void OnOutDoorLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnOutDoorLoaded;
        var controller = FindAnyObjectByType<OutdoorSceneController>();
        controller.RequestLoadIndoor += LoadIndoorScene;
        controller.Init(_outdoorData);
    }
}
