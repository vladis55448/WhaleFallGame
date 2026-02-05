using System;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    [SerializeField]
    private PointClickController _pointClickController;
    [SerializeField]
    private TMP_Text _textField;
    [SerializeField]
    private GameObject _dialogTextBox;
    [SerializeField]
    private Image _dialogSprite;
    [SerializeField]
    private TextAsset _config;
    [SerializeField]
    private float _typingDelay;
    [SerializeField]
    private List<DialogCharacter> _colors = new List<DialogCharacter>();

    private bool _active;
    private bool _typingSentence;
    private List<DialogLine> _lines;
    private DialogInstance _currentDialog;
    private List<DialogMessage> _messages;
    private int _messageCounter;
    private string _completeMessage;
    private Sequence _typingTween;
    private Sequence _waitingTween;

    public static DialogController Instance;

    private void Start()
    {
        Instance = this;
        _lines = JsonConvert.DeserializeObject<List<DialogLine>>(_config.text);
    }

    private void LateUpdate()
    {
        if (_active)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_typingSentence)
                {
                    if (_typingTween != null)
                    {
                        _typingTween.Kill();
                    }
                    _textField.text = _completeMessage;
                    _typingSentence = false;
                    SetWaitingAnimation();
                }
                else
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
            _textField.text += '\\';
        });
        _waitingTween.AppendInterval(0.5f);
        _waitingTween.AppendCallback(() =>
        {
            _textField.text = _textField.text.Remove(_textField.text.Length - 1);
        });
        _waitingTween.AppendInterval(0.5f);
        _waitingTween.SetLoops(-1);
    }

    private void AdvanceDialog()
    {
        if (_waitingTween != null)
            _waitingTween.Kill();
        _messageCounter++;
        if (_messageCounter < 0)
            return;
        if (_messageCounter == _messages.Count)
        {
            _active = false;
            _pointClickController.Locked = false;
            _textField.text = "";
            _dialogSprite.sprite = null;
            _currentDialog.Complete();
            _dialogTextBox.SetActive(false);
        }
        else
        {
            var message = _messages[_messageCounter];
            _textField.text = "";
            _completeMessage = message.Text;
            _typingSentence = true;
            _typingTween = DOTween.Sequence();
            foreach (var ch in message.Text)
            {
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

            foreach (var c in _colors)
            {
                if (c.CharacterId.Equals(message.Character))
                {
                    _dialogSprite.sprite = c.Sprite;
                    _textField.color = c.Color;
                }
            }
            _currentDialog.ActivateEvent(message.EventId);
        }
    }

    public void StartDialog(DialogInstance dialog)
    {
        if (_active)
            return;
        _dialogTextBox.SetActive(true);
        foreach (var line in _lines)
        {
            if (line.Id == dialog.Id)
            {
                _messageCounter = -2;
                _messages = line.Messages;
                _currentDialog = dialog;
                _pointClickController.Locked = true;
                _active = true;
                AdvanceDialog();
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
public class DialogLine
{
    public string Id;
    public List<DialogMessage> Messages;
}

[Serializable]
public class DialogMessage
{
    public string Text;
    public string EventId;
    public string Character;
}
