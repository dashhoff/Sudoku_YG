using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class NumberButton : MonoBehaviour
{
    [SerializeField] private int _number;
    [SerializeField] private Image _background;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _selectedColor = Color.yellow;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(Click);

        if (_background == null)
            _background = GetComponent<Image>();
    }

    private void OnEnable()
    {
        EventBus.OnHighlightNumber += OnHighlightNumber;
    }

    private void OnDisable()
    {
        EventBus.OnHighlightNumber -= OnHighlightNumber;
    }

    private void Click()
    {
        EventBus.InvokeNumberSelected(_number);
    }

    private void OnHighlightNumber(int number)
    {
        bool isSelected = number == _number && number != 0;
        if (_background != null)
            _background.color = isSelected ? _selectedColor : _normalColor;

        if (_label != null)
            _label.fontStyle = isSelected ? FontStyles.Bold : FontStyles.Normal;
    }
}