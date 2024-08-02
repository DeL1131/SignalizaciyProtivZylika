using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(House))]

public class Signalization : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _volume;

    private AudioSource _audioSource;
    private House _house;

    private float _volumeChangeRate = 0.1f;
    private float _volumeChangeInterval = 1f;
    private bool _isIncreasingVolume = false;
    private float _targetVolume;

    private void Awake()
    {
        _house = GetComponent<House>();
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = _audioClip;
        _audioSource.volume = _volume;
    }

    private void OnEnable()
    {
        _house.HouseEntryDetected += IncreaseVolume;
        _house.HouseExitDetected += DecreaseVolume;
    }

    private void OnDisable()
    {
        _house.HouseEntryDetected -= IncreaseVolume;
        _house.HouseExitDetected -= DecreaseVolume;
    }

    private void Update()
    {
        if (_audioSource.volume == 0)
        {
            _audioSource.Stop();
            _isIncreasingVolume = false;
            StopCoroutine(ChangeVolume());
        }
    }

    private void IncreaseVolume()
    {
        if (_isIncreasingVolume != true)
        {
            _targetVolume = 1f;
            StartCoroutine(ChangeVolume());
            _audioSource.Play();
        }
        else
        {
            _targetVolume = 1f;
        }
    }

    private void DecreaseVolume()
    {
        _targetVolume = 0f;
    }

    private IEnumerator ChangeVolume()
    {
        _isIncreasingVolume = true;

        WaitForSeconds waitForSeconds = new WaitForSeconds(_volumeChangeInterval);

        while (_isIncreasingVolume)
        {
            _volume = Mathf.MoveTowards(_volume, _targetVolume, _volumeChangeRate);
            _audioSource.volume = _volume;
            yield return waitForSeconds;
        }
    }
}