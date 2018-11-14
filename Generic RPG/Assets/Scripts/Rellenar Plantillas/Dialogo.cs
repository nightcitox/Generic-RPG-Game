using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogo : MonoBehaviour {
    public PlanDialogo dial;
    private Text texto;
    private string dialogo;
    private int id;
    // Use this for initialization
    void Start() {
        //Agarra el componente de tipo Texto del objeto en el que esté colocado.
        texto = GetComponent<Text>();
        if (dial != null)
        {
            dialogo = dial.dialogo;
            id = dial.id;
            texto.text = dialogo;
        }
        else
        {
            texto.text = "Ingresa un diálogo poh weta.";
        }
	}
}
