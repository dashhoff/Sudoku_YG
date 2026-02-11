using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private UIPanel _menuPanel;
    [SerializeField] private UIPanel _shopPanel;
    [SerializeField] private UIPanel _winPanel;

    private void OnEnable()
    {
        EventBus.OnGameStarted += HandleGameStarted;
        EventBus.OnWin += HandleWin;
    }

    private void OnDisable()
    {
        EventBus.OnGameStarted -= HandleGameStarted;
        EventBus.OnWin -= HandleWin;
    }

    private void Start()
    {
        _menuPanel.Open();
        _shopPanel.Close();
        _winPanel.Close();
    }

    private void HandleGameStarted()
    {
        _menuPanel.Close();
        _shopPanel.Close();
        _winPanel.Close();
    }

    private void HandleWin()
    {
        _winPanel.Open();
    }


    // кнопки UI
    public void StartGameButton()
    {
        EventBus.InvokeGameStarted();
    }

    public void OpenShopButton()
    {
        _shopPanel.Open();
    }

    public void CloseShopButton()
    {
        _shopPanel.Close();
    }

    public void BackToMenuButton()
    {
        _menuPanel.Open();
        _winPanel.Close();
    }
}