using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SudokuCell : MonoBehaviour
{
    [SerializeField] private TMP_Text _valueText;
    [SerializeField] private Image _background;
    [SerializeField] private Color _normal;
    [SerializeField] private Color _highlight;

    private Vector2Int _coord;
    private int _value;
    private int _highlightedNumber;
    private bool _fixed;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Click);
    }

    private void OnEnable()
    {
        EventBus.OnHighlightNumber += SetHighlightNumber;
        EventBus.OnCellValueChanged += UpdateValue;
    }

    private void OnDisable()
    {
        EventBus.OnHighlightNumber -= SetHighlightNumber;
        EventBus.OnCellValueChanged -= UpdateValue;
    }

    public void Init(Vector2Int coord,int value,bool isFixed)
    {
        _coord = coord;
        _value = value;
        _fixed = isFixed;
        Refresh();
    }

    private void Click()
    {
        if (_fixed) return;
        EventBus.InvokeCellSelected(_coord);
    }

    private void UpdateValue(Vector2Int coord,int value)
    {
        if (coord != _coord) return;
        _value = value;
        Refresh();
    }

    private void SetHighlightNumber(int number)
    {
        _highlightedNumber = number;
        ApplyHighlight();
    }

    private void Refresh()
    {
        _valueText.text = _value == 0 ? "" : _value.ToString();
        ApplyHighlight();
    }

    private void ApplyHighlight()
    {
        _background.color =
            (_highlightedNumber != 0 && _value == _highlightedNumber)
                ? _highlight
                : _normal;
    }
}