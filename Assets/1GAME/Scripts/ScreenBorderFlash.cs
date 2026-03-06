using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenBorderFlash : MonoBehaviour
{
    [SerializeField] private Image _greenBorder;
    [SerializeField] private Image _redBorder;

    [SerializeField] private float _fadeInDuration = 0.15f;
    [SerializeField] private float _fadeOutDuration = 0.25f;

    [SerializeField] private float _maxAlpha = 1f;
    [SerializeField] private float _startAlpha = 0f;

    private void Awake()
    {
        _SetAlpha(_greenBorder, _startAlpha);
        _SetAlpha(_redBorder, _startAlpha);
    }

    public void PlaySuccess()
    {
        _greenBorder.DOKill();
        _SetAlpha(_greenBorder, _startAlpha);

        DOTween.Sequence()
            .Append(_greenBorder.DOFade(_maxAlpha, _fadeInDuration))
            .Append(_greenBorder.DOFade(_startAlpha, _fadeOutDuration))
            .SetUpdate(true);
    }

    public void PlayFail()
    {
        _redBorder.DOKill();
        _SetAlpha(_redBorder, _startAlpha);

        DOTween.Sequence()
            .Append(_redBorder.DOFade(_maxAlpha, _fadeInDuration))
            .Append(_redBorder.DOFade(_startAlpha, _fadeOutDuration))
            .SetUpdate(true);
    }

    private void _SetAlpha(Image image, float value)
    {
        Color c = image.color;
        c.a = value;
        image.color = c;
    }
}