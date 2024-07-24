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
    private bool _isDecreasingVolume = false;

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
        }
    }

    private void IncreaseVolume()
    {
        if (_isIncreasingVolume != true)
        {
            StopCoroutine(GradualDecreaseVolume());
            _isDecreasingVolume = false;

            StartCoroutine(GradualIncreaseVolume());
            _audioSource.Play();
        }
    }

    private void DecreaseVolume()
    {
        if (_isDecreasingVolume != true)
        {
            StopCoroutine(GradualIncreaseVolume());
            _isIncreasingVolume = false;

            StartCoroutine(GradualDecreaseVolume());
        }
    }

    private IEnumerator GradualIncreaseVolume()
    {
        float maxVolume = 1;
        _isIncreasingVolume = true;

        WaitForSeconds waitForSeconds = new WaitForSeconds(_volumeChangeInterval);

        while (_isIncreasingVolume)
        {
            _volume = Mathf.MoveTowards(_volume, maxVolume, _volumeChangeRate);
            _audioSource.volume = _volume;
            yield return waitForSeconds;
        }
    }

    private IEnumerator GradualDecreaseVolume()
    {
        float minVolume = 0;
        _isDecreasingVolume = true;

        WaitForSeconds waitForSeconds = new WaitForSeconds(_volumeChangeInterval);

        while (_isDecreasingVolume)
        {
            _volume = Mathf.MoveTowards(_volume, minVolume, _volumeChangeRate);
            _audioSource.volume = _volume;
            yield return waitForSeconds;
        }
    }
}