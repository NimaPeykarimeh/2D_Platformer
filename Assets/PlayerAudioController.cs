using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    Movement2D movement;
    AudioSource audioSource;
    Movement2D.PlayerStates prevState;
    [SerializeField] AudioClip onGroundSound;
    [SerializeField] AudioClip jumpSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        movement = GetComponent<Movement2D>();
    }

    void CheckGroundState()
    {
        Movement2D.PlayerStates thisState = movement.currentState;
        if (thisState != prevState)
        {
            if (thisState == Movement2D.PlayerStates.Grounded )
            {
                audioSource.PlayOneShot(onGroundSound);
            }
            else if (thisState == Movement2D.PlayerStates.Jumping && movement.isJumped)
            {
                audioSource.pitch = 0.8f;
                audioSource.PlayOneShot(jumpSound);
            }
            prevState = thisState;
        }
    }

    private void Update()
    {
        CheckGroundState();


    }
}
