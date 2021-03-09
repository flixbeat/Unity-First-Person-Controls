using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource1;
    [SerializeField] AudioSource audioSource2;
    [SerializeField] AudioClip[] audioClips;
    public void PlayFootStepSound(float crouchValue)
    {
        if (crouchValue == 0) audioSource1.volume = 1f;
        else audioSource1.volume = 0.7f;
        audioSource1.pitch = Random.Range(0.9f, 1.1f);
        audioSource1.clip = audioClips[0];
        audioSource1.Play();
    }

    public void PlayFootStepCrouchSound()
    {
        audioSource1.pitch = 0.8f;
        audioSource1.clip = audioClips[0];
        audioSource1.volume = .5f;
        audioSource1.Play();
    }

    public void PlayLandSound()
    {
        audioSource2.pitch = 1.5f;
        audioSource2.clip = audioClips[1];
        audioSource2.Play();
    }
}
