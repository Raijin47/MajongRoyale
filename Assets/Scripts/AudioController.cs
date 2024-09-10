using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audioClips;

    private void Awake()
    {
        Instance = this;
    }

    public void Play(int id)
    {
        _audioSource.clip = _audioClips[id];
        _audioSource.Play();
    }
}
