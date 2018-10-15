using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlanMision : ScriptableObject {
    #region Propiedades
    public int ID;
    public string mision;
    public string descripcion;
    public PlanObjeto[] recompensas;
    public int experiencia;
    #endregion
}
