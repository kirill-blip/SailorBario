using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _backgroundAudioSource;
    [SerializeField] private AudioSource _soundAudioSource;

    [SerializeField] private List<AudioClip> _audioClips;

    [SerializeField] private Vector2 _timeBetweenMusic = new Vector2(3, 5);

    private void PlayBackgroundMusic()
    {
        _backgroundAudioSource.clip = _audioClips[Random.Range(0, _audioClips.Count)];
        _backgroundAudioSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        _soundAudioSource.PlayOneShot(clip);
    }
}