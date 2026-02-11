using TMPro;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        EventBus.OnLivesChanged += UpdateText;
        EventBus.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        EventBus.OnLivesChanged -= UpdateText;
        EventBus.OnGameOver -= OnGameOver;
    }

    private void UpdateText(int lives)
    {
        _text.text =  lives.ToString();
    }

    private void OnGameOver()
    {
        _text.text = "0";
    }
}