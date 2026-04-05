using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Image _image;

    private InventoryItem _item;

    public string ItemId => _item.Id;
    public string DialogId => _item.DialogId;
    public Sprite Sprite => _item.Sprite;
    public event Action<InventoryButton> Click;

    public void Init(InventoryItem item)
    {
        _item = item;
        _image.sprite = item.Sprite;
        Debug.Log(_image.sprite.name);
        _button.onClick.AddListener(() => { Click?.Invoke(this); });
    }
}
