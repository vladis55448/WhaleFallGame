using System;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using Newtonsoft.Json.Linq;
using TMPro;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    [SerializeField]
    private FirstPersonController _firstPersonController;
    [SerializeField]
    private PointClickController _pointClickController;
    [SerializeField]
    private TMP_Text _textField;
    [SerializeField]
    private GameObject _dialogTextBox;
    [SerializeField]
    private Image _dialogSprite;
    [SerializeField]
    private float _typingDelay;
    [SerializeField]
    private List<DialogCharacter> _colors = new List<DialogCharacter>();
    [SerializeField]
    private List<DialogOptionUI> _options = new List<DialogOptionUI>();
    [SerializeField]
    private float _typingDelayForOptions;

    private bool _active;
    private bool _typingSentence;
    private bool _waitingForAnswer;
    private DialogGraph _currentGraph;
    private INode _currentNode;
    private DialogInstance _currentDialogInstance;
    private Sequence _typingTween;
    private Sequence _waitingTween;

    private CursorLockMode _prevCursorState;

    private const string DIALOG_PATH = "/Dialogs/";
    private const string DATA_TYPE = ".simpleg";

    private static DialogController _instance;
    public static DialogController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<DialogController>();
            }
            return _instance;
        }
    }

    private void Start()
    {
    }

    private void LateUpdate()
    {
        if (_active && !_waitingForAnswer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!_typingSentence)
                {
                    AdvanceDialog();
                }
            }
        }
    }

    private void SetWaitingAnimation()
    {
        _waitingTween = DOTween.Sequence();
        _waitingTween.AppendCallback(() =>
        {
            _textField.text += " \\";
        });
        _waitingTween.AppendInterval(0.5f);
        _waitingTween.AppendCallback(() =>
        {
            _textField.text = _textField.text.Remove(_textField.text.Length - 2);
        });
        _waitingTween.AppendInterval(0.5f);
        _waitingTween.SetLoops(-1);
    }

    private void AdvanceDialog(int portId = 0)
    {
        if (_waitingTween != null)
            _waitingTween.Kill();
        _currentNode = _currentNode.GetOutputPort(portId).firstConnectedPort.GetNode();
        switch (_currentNode)
        {
            case EndNode:
                {
                    Cursor.lockState = _prevCursorState;
                    _active = false;
                    _pointClickController.Locked = false;
                    _textField.text = "";
                    _dialogSprite.sprite = null;
                    _currentDialogInstance.Complete();
                    _dialogTextBox.SetActive(false);
                    break;
                }
            case DialogNode:
                {
                    SetMainTextFromNode();
                    break;
                }
            case OptionsNode:
                {
                    _waitingForAnswer = true;
                    SetMainTextFromNode();
                    _typingTween.AppendCallback(() =>
                    {
                        Cursor.lockState = CursorLockMode.None;
                        foreach (var option in _options)
                        {
                            SetOptionDialogFromNode(option);
                        }
                    });
                    break;
                }
            case EventNode:
                {
                    var node = _currentNode as EventNode;
                    string eventId = "";
                    node.GetNodeOptionByName("EventId").TryGetValue<string>(out eventId);
                    _currentDialogInstance.ActivateEvent(eventId);
                    AdvanceDialog();
                    break;
                }
        }
    }

    private void SetMainTextFromNode()
    {
        var node = _currentNode as Node;
        string _completeMessage = "";
        node.GetNodeOptionByName("Text").TryGetValue<string>(out _completeMessage);

        var characterPort = node.GetInputPortByName("Character");
        var json = JsonUtility.ToJson(characterPort.firstConnectedPort.GetNode());
        var data = JObject.Parse(json);

        string characterId = data["m_Title"].ToString();
        foreach (var c in _colors)
        {
            if (c.CharacterId.Equals(characterId))
            {
                _dialogSprite.sprite = c.Sprite;
                _textField.color = c.Color;
            }
        }

        _typingSentence = true;
        _typingTween = DOTween.Sequence();

        _textField.text = "" + _completeMessage[0];
        for (int i = 1; i < _completeMessage.Length; i++)
        {
            var ch = _completeMessage[i];
            _typingTween.AppendInterval(_typingDelay);
            _typingTween.AppendCallback(() =>
            {
                _textField.text += ch;
            });
        }
        _typingTween.AppendCallback(() =>
        {
            _typingSentence = false;
            SetWaitingAnimation();
        });
    }

    private void SetOptionDialogFromNode(DialogOptionUI option)
    {
        var node = _currentNode as Node;
        option.Text.text = "";
        string completeMessage = "";
        node.GetNodeOptionByName($"Option{option.Id + 1}").TryGetValue<string>(out completeMessage);
        if (string.IsNullOrEmpty(completeMessage))
        {
            option.Button.gameObject.SetActive(false);
            return;
        }
        option.Button.gameObject.SetActive(true);
        completeMessage = $"{option.Id + 1}. {completeMessage}";
        var typingTween = DOTween.Sequence();
        foreach (var ch in completeMessage)
        {
            typingTween.AppendInterval(_typingDelayForOptions);
            typingTween.AppendCallback(() =>
            {
                option.Text.text += ch;
            });
        }
        typingTween.AppendCallback(() =>
        {
            option.Button.onClick.AddListener(() =>
            {
                OnSellectOption(option.Id);
            });
        });
    }

    private void OnSellectOption(int id)
    {
        Cursor.lockState = CursorLockMode.Locked;
        foreach (var option in _options)
        {
            option.Button.onClick = null;
            option.Button.gameObject.SetActive(false);
        }
        _waitingForAnswer = false;
        AdvanceDialog(id);
    }

    public void StartDialog(DialogInstance dialog)
    {
        if (_active)
            return;
        _prevCursorState = Cursor.lockState;
        Cursor.lockState = CursorLockMode.Locked;
        var path = "Assets/StreamingAssets" + DIALOG_PATH + dialog.Id + DATA_TYPE;
        _currentGraph = GraphDatabase.LoadGraph<DialogGraph>(path);
        var nodes = _currentGraph.GetNodes();
        foreach (var n in nodes)
        {
            if (n is StartNode)
            {
                _currentDialogInstance = dialog;
                _currentNode = n;
                _pointClickController.Locked = true;
                _dialogTextBox.SetActive(true);
                _active = true;
                AdvanceDialog();
                break;
            }
        }
    }
}

[Serializable]
public class DialogCharacter
{
    public string CharacterId;
    public Color Color;
    public Sprite Sprite;
}

[Serializable]
public class DialogOptionUI
{
    public int Id;
    public Button Button;
    public TMP_Text Text;
}
