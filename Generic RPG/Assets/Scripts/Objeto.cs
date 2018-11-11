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
        tipo = item.tipo;
        reutilizable = item.reutilizable;
        icono = item.icono;
        transform.Find("Item").GetComponent<Image>().sprite = icono;
    }
    void Utilizar()
    {
        GameObject.Find("Descripcion").GetComponent<Text>().text = descripcion;
        if(reutilizable == false)
        {
            cantidad -= 1;
        }
        else
        {
            //Se utiliza
        }
        GetComponentInChildren<Text>().text = "" + cantidad;
        Debug.Log(cantidad);
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
        GetComponentInChildren<Text>().text = "" + cantidad;
    }
}
