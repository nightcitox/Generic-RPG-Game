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
    private int[] bufos = new int[5]; //0-HP, 1-MP, 2-ATK, 3-DEF, 4-SPE
    bool moviendose;
    bool dentro;
    float timer;
    float waitTime = 1f;
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
        Nivel = 5;
        for(int i = 0; i < 4; i++)
        {
            bufos[i] = 0;
        }
        nombre = clase.nombre;
        Estadisticas();
    }

    private bool isInside = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Encuentros") == false ) { return; }
        dentro = true;
        GameObject.Find("Zona Encuentros").GetComponent<Mapa>().CalcularProbabilidad();
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Encuentros") == false) { return; }
        dentro = false;
        timer = 0f;
    }
    // Update is called once per frame
    public void Buff(string stat, int cant)
    {
        switch (stat)
        {
            case "HP":
                bufos[0] += cant;
                HP += cant;
                break;
            case "MP":
                bufos[1] += cant;
                MP += cant;
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
        Bufos();
        //Falta agregar el tiempo que demora. Por ahora voy a hacerlo pa que al salir de la batalla elimine los bufos.
    }
    public void DeBuff(string stat, int cant)
    {
        switch (stat)
        {
            case "HP":
                bufos[0] -= cant;
                HP += cant;
                break;
            case "MP":
                bufos[1] -= cant;
                MP += cant;
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
        Bufos();
        //Falta agregar el tiempo que demora. Por ahora voy a hacerlo pa que al salir de la batalla elimine los bufos.
    }
    void Update () {
        if(HP > MaxHP)
        {
            HP = MaxHP;
        }
        if (MP > MaxMP)
        {
            MP = MaxMP;
        }
        if(SceneManager.GetActiveScene().name != "Batalla")
        {
            Movimiento();
            print(dentro + " " + moviendose);
            if (dentro == false || moviendose == false) { return; }
            if (timer > waitTime)
            {
                timer = 0f;
                GameObject.Find("Zona Encuentros").GetComponent<Mapa>().CalcularProbabilidad();
            }
            timer += Time.deltaTime;
        }
    }
    void Bufos()
    {
        //Acá meteré los bufos del equipamiento.
        MaxHP += bufos[0];
        MaxMP += bufos[1];
        ATK += bufos[2];
        DEF += bufos[3];
        SPE += bufos[4];
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
            moviendose = true;
        float VelX = (Input.GetAxis("Horizontal") * mov_spe) * Time.deltaTime;
        float VelY = (Input.GetAxis("Vertical") * mov_spe) * Time.deltaTime;
        transform.Translate(new Vector3(VelX, VelY));
        }
        if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            moviendose = false;
        }
    }
}
