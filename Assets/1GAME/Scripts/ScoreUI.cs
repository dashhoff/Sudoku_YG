using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        EventBus.OnScoreChanged += UpdateText;
    }

    private void OnDisable()
    {
        EventBus.OnScoreChanged -= UpdateText;
    }

    private void UpdateText(int score)
    {
        _text.text = score.ToString();
    }
}