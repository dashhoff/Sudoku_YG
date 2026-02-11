using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private UIPanel _menuPanel;
    [SerializeField] private UIPanel _shopPanel;
    [SerializeField] private UIPanel _winPanel;
    [SerializeField] private UIPanel _losePanel;
    [SerializeField] private UIPanel _refillLifePanel; // новая панель пополнения
    
    [SerializeField] private Button _watchAdButton;
    [SerializeField] private Button _declineButton;

    private void OnEnable()
    {
        EventBus.OnGameStarted += OnGameStarted;
        EventBus.OnWin += OnWin;
        EventBus.OnGameOver += OnLose;
        EventBus.OnRequestLifeRefill += OnRequestLifeRefill; // подписка
    }

    private void OnDisable()
    {
        EventBus.OnGameStarted -= OnGameStarted;
        EventBus.OnWin -= OnWin;
        EventBus.OnGameOver -= OnLose;
        EventBus.OnRequestLifeRefill -= OnRequestLifeRefill;
    }

    private void Start()
    {
        _menuPanel.Open();
        _shopPanel.Close();
        _winPanel.Close();
        _losePanel.Close();
        if (_refillLifePanel != null) _refillLifePanel.Close();
        
        if (_watchAdButton != null)
        {
            _watchAdButton.onClick.RemoveAllListeners();
            _watchAdButton.onClick.AddListener(OnRefillLifeWatchAd);
        }

        if (_declineButton != null)
        {
            _declineButton.onClick.RemoveAllListeners();
            _declineButton.onClick.AddListener(OnRefillLifeClose);
        }
    }

    private void OnGameStarted()
    {
        _menuPanel.Close();
        _shopPanel.Close();
        _winPanel.Close();
        _losePanel.Close();
        if (_refillLifePanel != null) _refillLifePanel.Close();
    }

    private void OnWin() => _winPanel.Open();
    private void OnLose() => _losePanel.Open();

    private void OnRequestLifeRefill()
    {
        if (_refillLifePanel != null) _refillLifePanel.Open();
    }

    // вызовы с кнопок панели пополнения:
    public void OnRefillLifeWatchAd()
    {
        // показываем рекламу; в callback восстановим жизни
        AdsService.Instance.ShowReward(() =>
        {
            // даём 5 жизней (восстановление до полного)
            var gc = FindObjectOfType<GameController>();
            if (gc != null) gc.RestoreToFullLives();
            if (_refillLifePanel != null) _refillLifePanel.Close();
        });
    }

    public void OnRefillLifeClose()
    {
        if (_refillLifePanel != null) _refillLifePanel.Close();
    }

    // кнопки магазина/меню
    public void OpenShop() => _shopPanel.Open();
    public void CloseShop() => _shopPanel.Close();
    public void BackToMenu()
    {
        _menuPanel.Open();
        _winPanel.Close();
        _losePanel.Close();
    }
}
