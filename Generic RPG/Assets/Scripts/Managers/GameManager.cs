using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    //Almacena los datos de la misión actual, diálogo actual para mostrarlo y el Personaje actual con su nivel, experiencia y estadísticas.
    #region Propiedades
    public PlanMision mision;
    public string dialogo;
    private Inventario inventario;
    static public Personaje PJ;
    static public int Experiencia;
    private List<int> niveles;
    #endregion
    // Use this for initialization
    void Start () {
        niveles = new List<int>();
        inventario = GetComponent<Inventario>();
        PJ = GameObject.Find("Personaje").GetComponent<Personaje>();
        int formula = 0;
        for (int i = 0;i < 30; i++)
        {
            if(i >= 20)
            {
                formula += ((i - 1) * 500) + 1500;
            }
            else if(i >= 10)
            {
                formula += ((i - 1) * 250) + 1500;
            }
            else if(i == 0)
            {
                formula = 0;
            }
            else
            {
                formula += ((i-1) * 250) + 1000;
            }
            niveles.Add(formula);
        }
        int j = 0;
        foreach (int x in niveles)
        {
            print("Experiencia necesaria para nivel "+ (j+1) +": "+x);
            j += 1;
        }
        SubirNivel();
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
    public void SubirNivel()
    {
        int j = 0;
        foreach (int x in niveles)
        {
            if(Experiencia >= x)
            {
                PJ.Nivel = j+1;
            }
            j += 1;
        }
        print(PJ.Nivel);
        PJ.Estadisticas();
    }
}
