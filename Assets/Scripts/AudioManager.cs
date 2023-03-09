using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _backgroundAudioSource;
    [SerializeField] private AudioSource _soundAudioSource;

    [SerializeField] private AudioClip _audioClip;

    [SerializeField] private AudioClip _buttonSound;
    [SerializeField] private AudioClip _endSound;

    private void Awake()
    {
        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic()
    {
        _backgroundAudioSource.clip = _audioClip;
        _backgroundAudioSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        _soundAudioSource.PlayOneShot(clip);
    }

    public void PlayButtonSound()
    {
        _soundAudioSource.PlayOneShot(_buttonSound);
    }

    public void PlayEndSound()
    {
        _backgroundAudioSource.Stop();
        PlaySound(_endSound);
    }
}