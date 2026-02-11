using TMPro;
using UnityEngine;

public class HintsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    // пример минимального обработчика в HintUI (если у тебя нет)
    private void OnEnable() => EventBus.OnHintsChanged += UpdateHints;
    private void OnDisable() => EventBus.OnHintsChanged -= UpdateHints;
    private void UpdateHints(int hints) => _text.text = "Hints: " + hints;
}