using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Botones : MonoBehaviour {
    public AudioSource audio;

    public void VolumeChange()
    {
        audio.volume = GameObject.Find("Volumen").GetComponent<Slider>().value;
    }
    public void btn_Inipartida (string LevelName)
    {
        SceneManager.LoadScene("Batalla");
    }

    public void btn_Opciones()
    {

    }
    public void btn_Enviarsuge()
    {

    }

    public void btn_Cerrarsesion()
    {

    }

    public void salir()
    {
        Application.Quit();
    }


	}
	
	
