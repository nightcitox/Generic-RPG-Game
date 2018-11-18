using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class menú : MonoBehaviour {

	public Sprite botonuwu;

	public Button[] botones;
	AxisEventData unu;
	void Start(){
		unu = new AxisEventData(EventSystem.current); 

	}

	void Update(){
		if (unu.selectedObject == null) {
			botones [0].Select ();

		} else {
			unu.selectedObject.GetComponent<Image>().sprite = botonuwu; 
			unu.selectedObject.GetComponent<Image> ().color = new Color (255, 255, 255, 255);
			moverseuwu();
		}
		
	}

	public void moverseuwu() {
		foreach(Button x in botones){
			if(x.gameObject != unu.selectedObject){
				x.GetComponent<Image>().sprite = null;
				x.GetComponent<Image>().color = new Color (255, 255, 255, 0);
			}
		}
	}
}
