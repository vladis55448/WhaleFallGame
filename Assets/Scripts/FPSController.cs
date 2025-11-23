using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField]
    private int _fps;

    private void Start()
    {
        Application.targetFrameRate = _fps;
    }
}
