using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    [SerializeField]
    private string _id;

    [SerializeField]
    private Sprite _sprite;

    [SerializeField]
    private string _dialogId;

    public string Id => _id;
    public Sprite Sprite => _sprite;
    public string DialogId => _dialogId;
}
