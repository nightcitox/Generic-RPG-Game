using UnityEngine;
using UnityEngine.UI;

public class Objeto : MonoBehaviour {
    #region Propiedades
    public PlanObjeto item;
    private string nombre;
    private string descripcion;
    private Sprite icono;
    private int nivelRestriccion;
    private PlanObjeto.Efecto efecto;
    private PlanObjeto.Tipo tipo;
    private bool reutilizable;
    #endregion
    // Use this for initialization
    void Start () {
        Inicializar();
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
        this.GetComponent<Image>().sprite = icono;
    }
    public void Utilizar()
    {
        if(reutilizable == false)
        {
            //Elimina el objeto.
        }
        else
        {
            //Se utiliza
        }
    }
}
