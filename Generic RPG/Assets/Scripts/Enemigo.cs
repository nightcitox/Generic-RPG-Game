using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour {
    #region propiedades
    public PlanEnemigo enemigo;
    private string nombre;
    private int baseHP;
    private float exp;
    private int baseATK;
    private int baseDEF;
    private int baseSPE;
    private Sprite sprite;
    private Animator anim;
    #endregion
    // Use this for initialization
    void Start () {
        nombre = enemigo.nombre;
        baseHP = enemigo.baseHP;
        exp = enemigo.exp;
        baseATK = enemigo.baseATK;
        baseDEF = enemigo.baseDEF;
        baseSPE = enemigo.baseSPE;
    }
}
