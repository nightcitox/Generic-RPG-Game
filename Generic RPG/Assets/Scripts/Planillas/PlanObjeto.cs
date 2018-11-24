using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlanObjeto : ScriptableObject {
    #region Propiedades
    public string nombre;
    public string descripcion;
    public int nivelRestriccion;
    public Efecto efecto;
    public Tipo tipo;
    public Sprite icono;
    public bool reutilizable;
    public int cantidadEfecto;
    public Objetivo objetivoEfecto;
    public PlanClase[] clasesPermitidas;
    #endregion
    public enum Objetivo
    {
        HP,
        MP,
        ATK,
        DEF,
        SPE
    }
    public enum Efecto
    {
        Curacion,
        Buff,
        Debuff,
        Equipo,
        Ninguno
    }
    public enum Tipo
    {
        Equipo,
        Consumible,
        Mision
    }
    public enum TipoEquipo
    {
        Casco,
        Armadura,
        Pantalones,
        Zapatos,
        Arma,
        Ninguno
    }
    public TipoEquipo tipoEquipo;
}
