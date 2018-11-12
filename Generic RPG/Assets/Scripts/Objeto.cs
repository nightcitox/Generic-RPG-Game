using System.Collections.Generic;
using UnityEngine;
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

    public PlanObjeto.Tipo Tipo { get => tipo; set => tipo = value; }
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
        if(SceneManagement.Scene.name == "Batalla")
        {
            switch (Tipo)
            {
                case PlanObjeto.Tipo.Equipo:
                    //Crear clase de equipamiento.
                case PlanObjeto.Tipo.Consumible:
                    if(PlanObjeto.Efecto.Curacion == efecto)
                    {
                        switch (objetivoEfecto)
                        {
                            case PlanObjeto.objetivoEfecto.HP:
                                GameObject.Find("Personaje").GetComponent<Personaje>().HP += cantidadEfecto;
                            case PlanObjeto.objetivoEfecto.MP:
                                GameObject.Find("Personaje").GetComponent<Personaje>().MP += cantidadEfecto;
                        }
                    }else if(PlanObjeto.Efecto.Buff == efecto)
                    {
                        foreach(PlanObjeto.objetivoEfecto ef in PlanObjeto.objetivoEfecto)
                        {
                            Personaje.Buff(objetivoEfecto.toString(), cantidadEfecto);
                        }
                    }
                case PlanObjeto.Tipo.Mision:
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
