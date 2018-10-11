using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Act : MonoBehaviour {
    public Animator anim;
    private Button boton;
	// Update is called once per frame
	void Update () {
		if (anim.GetBool("Ataque") == false)
        {
            boton = gameObject.GetComponent<Button>();
            boton.interactable = true;
        }
	}
}
