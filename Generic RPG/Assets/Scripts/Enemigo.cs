using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour {
    #region propiedades
    public PlanEnemigo enemigo;
    private string nombre;
    private int maxhp;
    private int hp;
    private float exp;
    private int atk;
    private int def;
    private int spe;
    private Sprite sprite;
    private Animator anim;
    #endregion
    #region GetSetStats
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

    public int Maxhp
    {
        get
        {
            return maxhp;
        }

        set
        {
            maxhp = value;
        }
    }

    public int Hp
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;
        }
    }

    public float Exp
    {
        get
        {
            return exp;
        }

        set
        {
            exp = value;
        }
    }

    public int Atk
    {
        get
        {
            return atk;
        }

        set
        {
            atk = value;
        }
    }

    public int Def
    {
        get
        {
            return def;
        }

        set
        {
            def = value;
        }
    }

    public int Spe
    {
        get
        {
            return spe;
        }

        set
        {
            spe = value;
        }
    }
    #endregion
    // Use this for initialization
    void Start() {
        nombre = enemigo.nombre;
        maxhp = enemigo.baseHP;
        hp = enemigo.baseHP;
        exp = enemigo.exp;
        atk = enemigo.baseATK;
        def = enemigo.baseDEF;
        spe = enemigo.baseSPE;
    }
}
