using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Objeto : MonoBehaviour{
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
    void Start () {
        BotonUtilizar = GetComponentInChildren<Button>();
        Inicializar();
        BotonUtilizar.onClick.AddListener(Utilizar);
    }
    void Update()
    {
        if (cantidad == 0 && item.tipo != PlanObjeto.Tipo.Equipo)
        {
            print("Se destruye");
            foreach (PlanObjeto ob in Inventario.objetos)
            {
                Inventario.objetos.Remove(item);
            }
            Destroy(gameObject);
        }
        GameObject seleccionado = EventSystem.current.currentSelectedGameObject;
        print(seleccionado.GetComponentInParent<Objeto>());
        print(BotonUtilizar.name);
        if(seleccionado.GetComponentInParent<Objeto>() == this)
        {
            Seleccionar();
        }
    }
    void Seleccionar()
    {
        print("Seleccionado " + gameObject.name);
        GameObject.Find("Inventario").transform.Find("Descripcion").transform.Find("Text").gameObject.GetComponent<Text>().text = descripcion;
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
    public void Utilizar()
    {
        string[] mensaje = new string[1];
        Scene actual = SceneManager.GetActiveScene();
        switch (Tipo)
        {
            case PlanObjeto.Tipo.Equipo:
                print("uwu");
                //Crear clase de equipamiento.
                break;
            case PlanObjeto.Tipo.Consumible:
                if (PlanObjeto.Efecto.Curacion == efecto)
                {
                    if (actual.name == "Batalla")
                    {
                        BattleManager bm = GameObject.Find("GameManager").GetComponent<BattleManager>();
                        if (bm.accionespj == BattleManager.AccionesPJ.Decision)
                        {
                            bm.ObjUtilizado1 = this;
                            print(bm.ObjUtilizado1.nombre);
                            bm.AccionesdelPJ("Objeto");
                        }
                        else if(bm.accionespj == BattleManager.AccionesPJ.Objeto)
                        {
                            mensaje = new string[2];
                            mensaje[0] = GameObject.Find("Personaje").GetComponent<Personaje>().Nombre + " ha utilizado " + nombre + ".";
                            mensaje[1] = GameObject.Find("Personaje").GetComponent<Personaje>().Nombre + " ha recuperado " + cantidadEfecto + " de "+ objetivoEfecto.ToString() +".";
                            bm.StartCoroutine(bm.Esperar(this, mensaje, 0, null));
                            print("inicia");
                            switch (objetivoEfecto)
                            {
                                case PlanObjeto.Objetivo.HP:
                                    bm.pj.HP1 += cantidadEfecto;
                                    print("Se cura");
                                    break;
                                case PlanObjeto.Objetivo.MP:
                                    bm.pj.MP1 += cantidadEfecto;
                                    break;
                            }
                            print("Acaba");
                        }
                    }
                    else
                    {
                        print("inicia");
                        switch (objetivoEfecto)
                        {
                            case PlanObjeto.Objetivo.HP:
                                GameManager.PJ.HP1 += cantidadEfecto;
                                print("Se cura");
                                break;
                            case PlanObjeto.Objetivo.MP:
                                GameManager.PJ.MP1 += cantidadEfecto;
                                break;
                        }
                        print("Acaba");
                    }
                }
                else if (PlanObjeto.Efecto.Buff == efecto)
                {
                    foreach (PlanObjeto.Objetivo ef in (PlanObjeto.Objetivo[])Enum.GetValues(typeof(PlanObjeto.Objetivo)))
                    {
                        GameObject.Find("Personaje").GetComponent<Personaje>().Buff(objetivoEfecto.ToString(), cantidadEfecto);
                    }
                }
                break;
            case PlanObjeto.Tipo.Mision:
                break;
        }
        if(actual.name != "Batalla")
        {
            GetComponentInChildren<Text>().text = cantidad.ToString();
        }
    }
    public void Calculo(List<PlanObjeto> objs)
    {
        if(item.tipo == PlanObjeto.Tipo.Equipo)
        {
            cantidad = 1;
            GetComponentInChildren<Text>().text = "Equipo";
        }
        else
        {
            int contador = 0;
            foreach (PlanObjeto ob in objs)
            {
                if (ob == item)
                {
                    contador += 1;
                }
            }
            cantidad = contador;
            GetComponentInChildren<Text>().text = cantidad.ToString();
        }
    }
}
