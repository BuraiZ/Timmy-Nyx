using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCenter : MonoBehaviour {
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public void PlaySFX(AudioClip clip) {
        sfxSource.PlayOneShot(clip);
    }
}
