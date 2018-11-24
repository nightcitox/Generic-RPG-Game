using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSFX : MonoBehaviour
{
    public AudioClip sfx;
    public void UsarEfecto()
    {
        GetComponent<AudioSource>().PlayOneShot(sfx);
    }
}