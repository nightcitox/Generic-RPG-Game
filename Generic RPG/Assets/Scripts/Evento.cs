using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evento : MonoBehaviour {
    public void Accionar()
    {
        Personaje.puedeMoverse = true;
    }
}
