using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(House))]

public class Signalization : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _volume;

    private AudioSource _audioSource;
    private Coroutine _currentCoroutine;
    private House _house;

    private float _volumeChangeRate = 0.1f;
    private float _volumeChangeInterval = 1f;
    private float _increaseTargetVolume = 1f;
    private float _decreaseTargetVolume = 0f;

    private void Awake()
    {
        _house = GetComponent<House>();
        _audioSource = gameObject.GetComponent<AudioSource>();
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

    private void IncreaseVolume()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        _audioSource.Play();
        _currentCoroutine = StartCoroutine(ChangeVolume(_increaseTargetVolume));
    }

    private void DecreaseVolume()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        _currentCoroutine = StartCoroutine(ChangeVolume(_decreaseTargetVolume));
    }

    private IEnumerator ChangeVolume(float targetVolume)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_volumeChangeInterval);

        while (_volume != targetVolume)
        {
            _volume = Mathf.MoveTowards(_volume, targetVolume, _volumeChangeRate);
            _audioSource.volume = _volume;
            yield return waitForSeconds;
        }
    }
}