using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clase : MonoBehaviour {
    #region propiedades
    public PlanClase clase;
    private string nombre;
    private int baseHP;
    private int baseMP;
    private int baseATK;
    private int baseDEF;
    private int baseSPE;
    #endregion
    void Start () {
        nombre = clase.nombre;
        baseHP = clase.baseHP;
        baseMP = clase.baseMP;
        baseATK = clase.baseATK;
        baseDEF = clase.baseDEF;
        baseSPE = clase.baseSPE;
    }
}
