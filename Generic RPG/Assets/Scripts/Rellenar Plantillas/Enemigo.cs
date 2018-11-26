using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

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
    private int[] bufos = new int[3]; //0-ATK, 1-DEF, 2-SPE
    private float ProbabilidadAparecer;
    private int nivel;
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

    public float ProbabilidadAparecer1
    {
        get
        {
            return ProbabilidadAparecer;
        }

        set
        {
            ProbabilidadAparecer = value;
        }
    }

    public int Nivel
    {
        get
        {
            return nivel;
        }

        set
        {
            nivel = value;
        }
    }
    #endregion
    // Use this for initialization
    void Start() {
        enemigo = Mapa.en;
        Nivel = Random.Range(Mapa.NivelZona[0], Mapa.NivelZona[1]);
        for (int i = 0; i < 2; i++)
        {
            bufos[i] = 0;
        }
        nombre = enemigo.nombre;
        sprite = enemigo.sprite;
        GetComponent<SpriteRenderer>().sprite = sprite;
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = enemigo.anim as RuntimeAnimatorController;
        GameObject.Find("BarraENM1").transform.Find("Text").GetComponent<Text>().text = Nombre;
        GameObject.Find("BarraENM1").transform.Find("NivelText").GetComponent<Text>().text = "Nivel: " + Nivel;
        CalcularStatsPorNivel();
        GameObject.Find("BattleManager").GetComponent<BattleManager>().DecidirTurnos();
    }
    void Update()
    {
        if(hp <= 0)
        {
            hp = 0;
        }
        if (hp > maxhp)
        {
            hp = maxhp;
        }
    }
    void CalcularStatsPorNivel()
    {
        maxhp = Mathf.RoundToInt(enemigo.baseHP + (((float)Nivel / 10) * enemigo.baseHP) + Random.Range(Nivel - (float)Nivel / 2, Nivel + (float)Nivel / 2));
        hp = maxhp;
        exp = Mathf.RoundToInt(enemigo.exp + (((float)Nivel / 10) * enemigo.exp) + Random.Range(Nivel - (float)Nivel / 2, Nivel + (float)Nivel / 2));
        atk = Mathf.RoundToInt(enemigo.baseATK + (((float)Nivel / 20) * enemigo.baseATK) + Random.Range(Nivel - (float)Nivel / 2, Nivel + (float)Nivel / 2));
        def = Mathf.RoundToInt(enemigo.baseDEF + (((float)Nivel / 20) * enemigo.baseDEF) + Random.Range(Nivel - (float)Nivel / 2, Nivel + (float)Nivel / 2));
        spe = Mathf.RoundToInt(enemigo.baseSPE + (((float)Nivel / 20) * enemigo.baseSPE) + Random.Range(Nivel - (float)Nivel / 2, Nivel + (float)Nivel / 2));
        Personaje pj = GameObject.Find("Personaje").GetComponent<Personaje>();
        BattleManager bm = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        if (Spe > pj.SPE1)
        {
            bm.Turno1 = BattleManager.Turno.Enemigo;
            bm.Texto.text = "¡El primer turno corresponde al enemigo!";
        }
        else
        {
            bm.Turno1 = BattleManager.Turno.Personaje;
        }
        bm.StartCoroutine("Acciones");
    }
    string AplicarStats(string stat, int cant)
    {
        string msj = "";
        if(atk+bufos[0] <= 0)
        {
            atk = 1;
            msj = "El ataque de "+nombre+" no puede bajar más.";
        }
        else
        {
            atk += bufos[0];
        }
        if (def + bufos[1] <= 0)
        {
            def = 1;
            msj = "La defensa de " + nombre + " no puede bajar más.";
        }
        else
        {
            def += bufos[1];
        }
        if (spe + bufos[2] <= 0)
        {
            spe = 1;
            msj = "La velocidad de " + nombre + " no puede bajar más.";
        }
        else
        {
            spe += bufos[2];
        }
        print("MaxHP: " + maxhp + " EXP: " + exp + " ATK: " + atk + " DEF: " + def + " SPE: " + spe);
        if(msj == "")
        {
            return "Has bajado la " + stat + " del enemigo en " + cant + " puntos.";
        }
        else
        {
            return msj;
        }
    }
    public void Buff(string stat, int cant)
    {
        switch (stat)
        {
            case "ATK":
                bufos[0] += cant;
                break;
            case "DEF":
                bufos[1] += cant;
                break;
            case "SPE":
                bufos[2] += cant;
                break;
        }
        AplicarStats(null, 0);
        //Falta agregar el tiempo que demora. Por ahora voy a hacerlo pa que al salir de la batalla elimine los bufos.
    }
    public string DeBuff(string stat, int cant)
    {
        switch (stat)
        {
            case "ATK":
                bufos[0] -= cant;
                break;
            case "DEF":
                bufos[1] -= cant;
                break;
            case "SPE":
                bufos[2] -= cant;
                break;
        }
        return AplicarStats(stat, cant);
        //Falta agregar el tiempo que demora. Por ahora voy a hacerlo pa que al salir de la batalla elimine los bufos.
    }
    public void TirarDaño()
    {
        Animator dmg = GameObject.Find("Daño").GetComponent<Animator>();
        dmg.ResetTrigger("Enemigo");
        dmg.ResetTrigger("Personaje");
        dmg.SetTrigger("Personaje");
        AudioClip oof = Resources.Load<AudioClip>("SFX/Oof");
        GameObject.Find("SFX").GetComponent<AudioSource>().PlayOneShot(oof);
    }
}
