using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
	void Update () {
        float x = GameObject.FindGameObjectWithTag("Jugador").transform.position.x;
        float y = GameObject.FindGameObjectWithTag("Jugador").transform.position.y;
        transform.position = new Vector3(x, y, -1);
    }
}
