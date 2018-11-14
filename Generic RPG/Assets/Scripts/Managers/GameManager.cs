using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    //Almacena los datos de la misión actual, diálogo actual para mostrarlo y el Personaje actual con su nivel, experiencia y estadísticas.
    #region Propiedades
    public PlanMision mision;
    public string dialogo;
    private Inventario inventario;
    Personaje PJ;
    #endregion
    // Use this for initialization
    void Start () {
        inventario = GetComponent<Inventario>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.H))
        {
            switch (inventario.Abierto)
            {
                case true:
                    inventario.Cerrar();
                    break;
                case false:
                    Menus();
                    break;
            }
        }
    }
    void Menus()
    {
        inventario.Abrir();
    }
}
