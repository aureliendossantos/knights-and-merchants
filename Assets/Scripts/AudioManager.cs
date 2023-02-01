using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip placeMarker;
    [SerializeField] AudioClip build;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaceMarker()
    {
        audioSource.PlayOneShot(placeMarker);
    }

    public void Build()
    {
        audioSource.PlayOneShot(build);
    }
}
