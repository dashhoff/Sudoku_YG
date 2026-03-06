using UnityEngine;
using DG.Tweening;

public class HintVisual : MonoBehaviour
{
    [SerializeField] private RectTransform _videoIcon;
    [SerializeField] private RectTransform _glow;

    [SerializeField] private float _pulseScale = 1.15f;
    [SerializeField] private float _pulseDuration = 0.6f;

    [SerializeField] private float _rotationSpeed = 120f;

    private void Start()
    {
        _StartPulse();
        _StartGlowRotation();
    }

    private void _StartPulse()
    {
        _videoIcon
            .DOScale(_pulseScale, _pulseDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true);
    }

    private void _StartGlowRotation()
    {
        _glow
            .DORotate(new Vector3(0, 0, -360), 360f / _rotationSpeed, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear)
            .SetUpdate(true);
    }
}