using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    [Header("Clicks")]
    [SerializeField] private AudioClip _click1;
    [SerializeField] private AudioClip _click2;

    [Header("Game")]
    [SerializeField] private AudioClip _correct;
    [SerializeField] private AudioClip _wrong;

    private void OnEnable()
    {
        EventBus.OnSound += Play;
    }

    private void OnDisable()
    {
        EventBus.OnSound -= Play;
    }

    private void Play(SoundType type)
    {
        if (_audioSource == null) return;

        switch (type)
        {
            case SoundType.Click1:
                _audioSource.PlayOneShot(_click1);
                break;

            case SoundType.Click2:
                _audioSource.PlayOneShot(_click2);
                break;

            case SoundType.Correct:
                _audioSource.PlayOneShot(_correct);
                break;

            case SoundType.Wrong:
                _audioSource.PlayOneShot(_wrong);
                break;
        }
    }
}