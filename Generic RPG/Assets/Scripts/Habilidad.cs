using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Habilidad : MonoBehaviour {
    #region Propiedades
    private string nombre;
    private int baseDMG;
    private int lrnLVL;
    private PlanHabilidades habilidad;
    private PlanHabilidades.Tipo tipo;
    private int MPUse;
    private Sprite icono;
    private string Descripcion;
    #endregion
    #region GetSetProps
    public string Nombre
    {
        get
        {
            return nombre;
        }

        set
        {
            nombre = value;
        }
    }

    public int BaseDMG
    {
        get
        {
            return baseDMG;
        }

        set
        {
            baseDMG = value;
        }
    }

    public int LrnLVL
    {
        get
        {
            return lrnLVL;
        }

        set
        {
            lrnLVL = value;
        }
    }

    public PlanHabilidades Hab
    {
        get
        {
            return habilidad;
        }

        set
        {
            habilidad = value;
        }
    }

    public int MPUse1
    {
        get
        {
            return MPUse;
        }

        set
        {
            MPUse = value;
        }
    }

    public PlanHabilidades.Tipo Tipo
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

    public Sprite Icono
    {
        get
        {
            return icono;
        }

        set
        {
            icono = value;
        }
    }

    public string Descripcion1
    {
        get
        {
            return Descripcion;
        }

        set
        {
            Descripcion = value;
        }
    }
    #endregion
    #region Metodos
    public void Start()
    {
        nombre = habilidad.nombre;
        BaseDMG = Hab.baseDMG;
        lrnLVL = Hab.lrnLVL;
        Tipo = Hab.tipo;
        MPUse = Hab.MPUse;
        icono = Hab.icono;
        Descripcion = Hab.Descripcion;
        Inicializar();
    }
    void Inicializar()
    {
        gameObject.transform.Find("Ícono").GetComponent<Image>().sprite = icono;
        gameObject.transform.Find("Daño").GetComponent<Text>().text = "DMG: "+baseDMG;
        gameObject.transform.Find("Descripcion").GetComponent<Text>().text = Descripcion;
        gameObject.transform.Find("Tipo").GetComponent<Text>().text = tipo.ToString();
        gameObject.transform.Find("Titulo").GetComponent<Text>().text = nombre;
    }
    #endregion
}
