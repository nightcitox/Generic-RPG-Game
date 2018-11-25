using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSFX : MonoBehaviour
{
    public AudioClip[] sfx = new AudioClip[10];
    public int contador = 0;
    public void UsarEfecto()
    {
        GetComponent<AudioSource>().PlayOneShot(sfx[contador]);
        contador += 1;
        if(contador == sfx.Length)
        {
            contador = 0;
        }
    }
}