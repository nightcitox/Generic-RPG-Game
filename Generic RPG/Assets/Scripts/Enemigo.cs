using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
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
    private int[] bufos = new int[4]; //0-HP, 1-ATK, 2-DEF, 3-SPE
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
        for (int i = 0; i < 3; i++)
        {
            bufos[i] = 0;
        }
        nombre = enemigo.nombre;
        maxhp = enemigo.baseHP;
        hp = enemigo.baseHP;
        exp = enemigo.exp;
        atk = enemigo.baseATK;
        def = enemigo.baseDEF;
        spe = enemigo.baseSPE;
        sprite = enemigo.sprite;
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = enemigo.anim;
    }
    void AplicarStats()
    {
        maxhp = enemigo.baseHP + bufos[0];
        atk = enemigo.baseATK + bufos[1];
        def = enemigo.baseDEF + bufos[2];
        spe = enemigo.baseSPE + bufos[3];
    }
    public void Buff(string stat, int cant)
    {
        switch (stat)
        {
            case "HP":
                bufos[0] += cant;
                break;
            case "ATK":
                bufos[2] += cant;
                break;
            case "DEF":
                bufos[3] += cant;
                break;
            case "SPE":
                bufos[4] += cant;
                break;
        }
        AplicarStats();
        //Falta agregar el tiempo que demora. Por ahora voy a hacerlo pa que al salir de la batalla elimine los bufos.
    }
    public void DeBuff(string stat, int cant)
    {
        switch (stat)
        {
            case "HP":
                bufos[0] -= cant;
                break;
            case "ATK":
                bufos[2] -= cant;
                break;
            case "DEF":
                bufos[3] -= cant;
                break;
            case "SPE":
                bufos[4] -= cant;
                break;
        }
        AplicarStats();
        //Falta agregar el tiempo que demora. Por ahora voy a hacerlo pa que al salir de la batalla elimine los bufos.
    }
}
