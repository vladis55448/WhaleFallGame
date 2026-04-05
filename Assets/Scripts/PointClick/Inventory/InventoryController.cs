using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private GameObject _root;
    [SerializeField]
    private GameObject _inspectionRoot;
    [SerializeField]
    private Image _inspectionImage;
    [SerializeField]
    private InventoryButton _inventoryButtonPrefab;
    [SerializeField]
    private Transform _listRoot;
    [SerializeField]
    private Transform _sellector;
    [SerializeField]
    private Button _inspectButton;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private DialogController _dialogController;


    private List<InventoryButton> _buttons = new List<InventoryButton>();
    private InventoryButton _currentButton;

    private const string ITEMS_PATH = "InventoryItems/";

    public event Action Closed;

    private void Start()
    {
        Disable();
        _closeButton.onClick.AddListener(() => Disable());
        _inspectButton.onClick.AddListener(() => Inspect());
    }

    public void Activate()
    {
        Cursor.lockState = CursorLockMode.None;
        _root.SetActive(true);
    }

    public void Disable()
    {
        _inspectionRoot.SetActive(false);
        _root.SetActive(false);
        Closed?.Invoke();
        _currentButton = null;
        _sellector.gameObject.SetActive(false);
    }

    public void AddItem(string itemId)
    {
        var item = Resources.Load<InventoryItem>(ITEMS_PATH + itemId);
        if (item == null)
        {
            throw new System.Exception("Null Inventory Item");
        }
        var button = Instantiate(_inventoryButtonPrefab, _listRoot);
        _buttons.Add(button);
        button.Init(item);
        button.Click += OnItemClick;
    }

    public void RemoveItem(string itemId)
    {

    }

    public bool HasItem(string id)
    {
        foreach(var button in _buttons)
        {
            if (button.ItemId.Equals(id))
            {
                return true;
            }
        }
        return false;
    }

    private void OnItemClick(InventoryButton button)
    {
        _currentButton = button;
        _inspectionImage.sprite = button.Sprite;
        _sellector.position = _currentButton.transform.position;
        _sellector.gameObject.SetActive(true);
    }

    private void Inspect()
    {
        if(_currentButton == null)
            return;
        _dialogController.DialogCompleted += OnInspectionDialogCompleted;
        _dialogController.StartMockDialog(_currentButton.DialogId);
        _root.SetActive(false);
        _inspectionRoot.SetActive(true);
    }

    private void OnInspectionDialogCompleted()
    {
        _root.SetActive(true);
        _inspectionRoot.SetActive(false);
    }
}
