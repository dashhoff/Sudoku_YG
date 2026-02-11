using System;
using UnityEngine;
using YG;

public class AdsService : MonoBehaviour
{
    private static AdsService _instance;
    public static AdsService Instance => _instance;

    private Action _rewardCallback;
    private string _pendingPlacement;

    private void Awake()
    {
        _instance = this;
        YG2.onRewardAdv += _OnRewarded;
    }

    private void OnDestroy()
    {
        YG2.onRewardAdv -= _OnRewarded;
    }

    // Универсальный вызов рекламы. placementId можно указать "reward", "life", "hints" и т.д.
    public void ShowReward(Action onReward, string placementId = "reward")
    {
        _rewardCallback = onReward;
        _pendingPlacement = placementId;

#if UNITY_EDITOR
        // эмуляция в редакторе
        Debug.Log($"[AdsService] Editor: emulating reward for placement '{placementId}'");
        _rewardCallback?.Invoke();
        _rewardCallback = null;
        _pendingPlacement = null;
#else
        YG2.RewardedAdvShow(placementId);
#endif
    }

    // Удобные обёртки
    public void ShowLifeReward(Action onReward) => ShowReward(onReward, "life");
    public void ShowHintsReward(Action onReward) => ShowReward(onReward, "hints");

    // Коллбек от YG2
    private void _OnRewarded(string id)
    {
        Debug.Log($"[AdsService] Rewarded callback id: {id}");
        if (string.IsNullOrEmpty(_pendingPlacement) || _rewardCallback == null)
        {
            Debug.Log("[AdsService] No pending reward or callback");
            return;
        }

        if (id == _pendingPlacement)
        {
            _rewardCallback?.Invoke();
            _rewardCallback = null;
            _pendingPlacement = null;
        }
        else
        {
            Debug.Log($"[AdsService] Received reward id '{id}' does not match pending placement '{_pendingPlacement}'");
        }
    }
}