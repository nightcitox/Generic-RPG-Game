using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSFX : MonoBehaviour
{
    public AudioClip sfx;
    public void UsarEfecto()
    {
        print("Si se ejecuta");
        print(sfx.name);
        GetComponent<AudioSource>().PlayOneShot(sfx);
    }
}