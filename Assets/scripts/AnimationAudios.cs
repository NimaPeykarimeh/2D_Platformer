using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAudios : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip footStepAudio;
    [SerializeField] float minPitch = 0.9f;
    [SerializeField] float maxPitch = 1.2f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void FootStepSound()
    {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(footStepAudio);
    }
}
