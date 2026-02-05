using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneLoader : MonoBehaviour
{
    private void LateUpdate()
    {
        if (FindFirstObjectByType<ScenesController>())
        {
            Destroy(gameObject);
        }
        else
        {
            SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
        }
    }
}
