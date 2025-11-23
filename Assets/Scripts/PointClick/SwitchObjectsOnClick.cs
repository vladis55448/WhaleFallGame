using UnityEngine;

public class SwitchObjectsOnClick : MonoBehaviour, IClickComponent
{
    [SerializeField]
    private GameObject _disableObject;
    [SerializeField]
    private GameObject _enableObject;

    public void Click()
    {
        _disableObject.SetActive(false);
        _enableObject.SetActive(true);
    }
}
