using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Signalization : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _volume;

    private AudioSource _audioSource;

    private float _volumeChangeRate = 0.1f;
    private float _volumeChangeInterval = 1f;
    private bool _isIncreasingVolume;
    private bool _isDecreasingVolume;

    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = _audioClip;
        _audioSource.volume = _volume;
    }

    private void Update()
    {
        if (_audioSource.volume == 0)
        {
            _audioSource.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player _))
        {
            if (_isIncreasingVolume != true)
            {
                StopCoroutine(GradualDecreaseVolume());
                _isDecreasingVolume = false;

                StartCoroutine(GradualIncreaseVolume());
                _audioSource.Play();
            }            
        }
    }

    private void OnTriggerExit(Collider other)
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