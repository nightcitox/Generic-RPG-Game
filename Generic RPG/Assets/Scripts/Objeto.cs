using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Objeto : MonoBehaviour {
    #region Propiedades
    public PlanObjeto item;
    private string nombre;
    private string descripcion;
    private Sprite icono;
    private int cantidad;
    private int nivelRestriccion;
    private PlanObjeto.Efecto efecto;
    private PlanObjeto.Tipo tipo;
    private bool reutilizable;
    private Button BotonUtilizar;
    private int cantidadEfecto;
    private PlanObjeto.Objetivo objetivoEfecto;
    void Update()
    {
        if(cantidad == 0)
        {
            GameObject.Find("EventSystem").GetComponent<Inventario>().objetos.Remove(item);
            Destroy(gameObject);
        }
    }
    public int Cantidad
    {
        get
        {
            return cantidad;
        }

        set
        {
            cantidad = value;
        }
    }

    public PlanObjeto.Tipo Tipo
    {
        get
        {
            return tipo;
        }

        set
        {
            tipo = value;
        }
    }

    #endregion
    // Use this for initialization
    void Start () {
        BotonUtilizar = GetComponentInChildren<Button>();
        Inicializar();
        BotonUtilizar.onClick.AddListener(Utilizar);
    }
    public void Inicializar()
    {
        nombre = item.nombre;
        descripcion = item.descripcion;
        nivelRestriccion = item.nivelRestriccion;
        efecto = item.efecto;
        Tipo = item.tipo;
        reutilizable = item.reutilizable;
        icono = item.icono;
        transform.Find("Item").GetComponent<Image>().sprite = icono;
        cantidadEfecto = item.cantidadEfecto;
        objetivoEfecto = item.objetivoEfecto;
    }
    void Utilizar()
    {
        GameObject.Find("Descripcion").GetComponent<Text>().text = descripcion;
        if (reutilizable == false)
        {
            cantidad -= 1;
        }
        Scene actual = SceneManager.GetActiveScene();
        if (actual.name == "Batalla")
        {
            switch (Tipo)
            {
                case PlanObjeto.Tipo.Equipo:
                    //Crear clase de equipamiento.
                    break;
                case PlanObjeto.Tipo.Consumible:
                    if(PlanObjeto.Efecto.Curacion == efecto)
                    {
                        switch (objetivoEfecto)
                        {
                            case PlanObjeto.Objetivo.HP:
                                GameObject.Find("Personaje").GetComponent<Personaje>().HP1 += cantidadEfecto;
                                break;
                            case PlanObjeto.Objetivo.MP:
                                GameObject.Find("Personaje").GetComponent<Personaje>().MP1 += cantidadEfecto;
                                break;
                        }
                    }else if(PlanObjeto.Efecto.Buff == efecto)
                    {
                        foreach(PlanObjeto.Objetivo ef in (PlanObjeto.Objetivo[])Enum.GetValues(typeof(PlanObjeto.Objetivo)))
                        {
                            GameObject.Find("Personaje").GetComponent<Personaje>().Buff(objetivoEfecto.ToString(), cantidadEfecto);
                        }
                    }
                    break;
                case PlanObjeto.Tipo.Mision:
                    break;
            }
        }
        GetComponentInChildren<Text>().text = cantidad.ToString();
    }
    public void Calculo(List<PlanObjeto> objs)
    {
        int contador = 0;
        foreach (PlanObjeto ob in objs)
        {
            if(ob == item)
            {
                contador += 1;
            }
        }
        cantidad = contador;
        GetComponentInChildren<Text>().text = cantidad.ToString();
    }
}
