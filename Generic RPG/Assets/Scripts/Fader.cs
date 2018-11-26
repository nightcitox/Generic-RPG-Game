using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fader : MonoBehaviour {
    public Animator fader;
    public static bool cambiarLvl;
    public static string nombreMapa;
	void Update () {
		if(cambiarLvl == true)
        {
            FadeScene();
        }
	}
    public void FadeScene()
    {
        fader.SetTrigger("Fade-Out");
    }
    public void Transicionar()
    {
        cambiarLvl = false;
        Personaje.puedeMoverse = true;
        SceneManager.LoadScene(nombreMapa);
    }
}
