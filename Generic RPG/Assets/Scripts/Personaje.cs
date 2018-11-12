using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Personaje : MonoBehaviour {
    #region propiedades
    private string nombre;
    private int nivel;
    private float exp;
    public PlanClase clase;
    public float mov_spe;
    private Vector3 pos;
    private Vector3 endPos;
    public bool puedeMoverse;
    #endregion
    #region estadisticas
    private int MaxHP;
    private int MaxMP;
    private int HP;
    private int MP;
    private int ATK;
    private int DEF;
    private int SPE;
    #endregion
    #region GetSetStats
    public int MaxHP1
    {
        get
        {
            return MaxHP;
        }

        set
        {
            MaxHP = value;
        }
    }

    public int MaxMP1
    {
        get
        {
            return MaxMP;
        }

        set
        {
            MaxMP = value;
        }
    }

    public int HP1
    {
        get
        {
            return HP;
        }

        set
        {
            HP = value;
        }
    }

    public int MP1
    {
        get
        {
            return MP;
        }

        set
        {
            MP = value;
        }
    }

    public int ATK1
    {
        get
        {
            return ATK;
        }

        set
        {
            ATK = value;
        }
    }

    public int DEF1
    {
        get
        {
            return DEF;
        }

        set
        {
            DEF = value;
        }
    }

    public int SPE1
    {
        get
        {
            return SPE;
        }

        set
        {
            SPE = value;
        }
    }

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
    #endregion
    // Use this for initialization
    void Start () {
        Nivel = 1;
        nombre = clase.nombre;
        Estadisticas();
    }
	
	// Update is called once per frame
	void Update () {
        Movimiento();
    }
    void Estadisticas()
    {
        float formula;
        formula = (float)clase.baseHP + ((Nivel - 1) * (clase.baseHP / 10));
        MaxHP1 = Mathf.RoundToInt(formula);
        HP1 = Mathf.RoundToInt(formula);
        formula = (float)clase.baseMP + (Nivel - 1 * clase.baseMP / 10);
        MaxMP1 = Mathf.RoundToInt(formula);
        MP1 = Mathf.RoundToInt(formula);
        formula = (float)clase.baseATK + (Nivel - 1 * clase.baseATK / 25);
        ATK1 = Mathf.RoundToInt(formula);
        formula = (float)clase.baseDEF + (Nivel - 1 * clase.baseDEF / 25);
        DEF1 = Mathf.RoundToInt(formula);
        formula = (float)clase.baseSPE + (Nivel - 1 * clase.baseSPE / 25);
        SPE1 = Mathf.RoundToInt(formula);
    }

    void Movimiento()
    {
        if(puedeMoverse == true) {
        float VelX = (Input.GetAxis("Horizontal") * mov_spe) * Time.deltaTime;
        float VelY = (Input.GetAxis("Vertical") * mov_spe) * Time.deltaTime;
        transform.Translate(new Vector3(VelX, VelY));
        }
    }
}
