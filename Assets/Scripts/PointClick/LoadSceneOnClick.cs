using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClock : MonoBehaviour, IClickComponent
{
    [SerializeField]
    private string _sceneName;

    public void Click()
    {
        SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
    }
}
