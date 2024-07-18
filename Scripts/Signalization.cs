using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Signalization : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _volume;

    private AudioSource _audioSource;

   private float _volumeChangeRate = 0.1f;

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
            StopAllCoroutines();
            StartCoroutine(GradualIncreaseVolume());
            _audioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StopAllCoroutines();
        StartCoroutine(GradualDecreaseVolume());
    }

    private IEnumerator GradualIncreaseVolume()
    {
        float maxVolume = 1;

        WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

        while (true)
        {
            _volume = Mathf.MoveTowards(_volume, maxVolume, _volumeChangeRate);            
            _audioSource.volume = _volume;            
            yield return waitForSeconds;
        }
    }

    private IEnumerator GradualDecreaseVolume()
    {
        float minVolume = 0;

        WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

        while (true)
        {
            _volume = Mathf.MoveTowards(_volume, minVolume, _volumeChangeRate);
            _audioSource.volume = _volume;
            yield return waitForSeconds;
        }
    }
}
