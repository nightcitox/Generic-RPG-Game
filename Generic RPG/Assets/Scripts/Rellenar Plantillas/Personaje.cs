using UnityEngine;
using UnityEngine.SceneManagement;

public class Personaje : MonoBehaviour {
    #region propiedades
    private string nombre;
    private int nivel;
    private PlanObjeto[] equipo = new PlanObjeto[5];
    public PlanClase clase;
    public float mov_spe;
    private Vector3 pos;
    private Vector3 endPos;
    public static bool puedeMoverse;
    private int[] bufos = new int[5]; //0-HP, 1-MP, 2-ATK, 3-DEF, 4-SPE
    bool moviendose;
    bool dentro;
    float timer;
    float espera = 0.5f;
    float UltimaPos;
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

    public PlanObjeto[] Equipo
    {
        get
        {
            return equipo;
        }

        set
        {
            equipo = value;
        }
    }

    public int[] Bufos1
    {
        get
        {
            return bufos;
        }

        set
        {
            bufos = value;
        }
    }
    #endregion
    #region Start y Update
    void Start() {
        if(GameManager.clasesita != null)
        {
            clase = GameManager.clasesita;
        }
        GameManager.menusActivos = true;
        for (int i = 0; i < 4; i++)
        {
            Bufos1[i] = 0;
        }
        nombre = GameManager.UsuarioConectado;
        if (SceneManager.GetActiveScene().name == "Batalla")
        {
            GetComponent<SpriteRenderer>().sprite = clase.battler;
            Animator anim = GetComponent<Animator>();
            anim.runtimeAnimatorController = clase.battlerController as RuntimeAnimatorController;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = clase.mapSprite;
            Animator anim = GetComponent<Animator>();
            anim.runtimeAnimatorController = clase.mapController as RuntimeAnimatorController;
        }
    }
    void Update()
    {
        if (HP > MaxHP)
        {
            HP = MaxHP;
        }
        if (MP > MaxMP)
        {
            MP = MaxMP;
        }
        if (SceneManager.GetActiveScene().name != "Batalla")
        {
            Movimiento();
            if (dentro == false || moviendose == false) { return; }
            if (timer > espera)
            {
                timer = 0f;
                GameObject.Find("Zona Encuentros").GetComponent<Mapa>().CalcularProbabilidad();
            }
            timer += Time.deltaTime;
        }
    }
    #endregion
    #region Métodos Unity
    void OnCollisionStay2D(Collision2D col)
    {
        col.otherRigidbody.AddForce(-0.1F * col.otherRigidbody.velocity);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Encuentros") == true)
        {
            dentro = true;
            GameObject.Find("Zona Encuentros").GetComponent<Mapa>().CalcularProbabilidad();
        }
        else if (col.gameObject.CompareTag("TP") == true)
        {
            Fader.nombreMapa = col.gameObject.name;
            puedeMoverse = false;
            Fader.cambiarLvl = true;
        }
        else if (col.gameObject.CompareTag("Evento") == true)
        {
            col.gameObject.GetComponent<Evento>().Accionar();
            puedeMoverse = false;
            print("1");
        }
        else
        {
            return;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Encuentros") == false) { return; }
        dentro = false;
        timer = 0f;
    }
    #endregion
    #region Métodos
    public void Buff(string stat, int cant)
    {
        print("Sube" + stat + " " + cant);
        switch (stat)
        {
            case "HP":
                Bufos1[0] += cant;
                HP += cant;
                break;
            case "MP":
                Bufos1[1] += cant;
                MP += cant;
                break;
            case "ATK":
                Bufos1[2] += cant;
                break;
            case "DEF":
                Bufos1[3] += cant;
                break;
            case "SPE":
                Bufos1[4] += cant;
                break;
        }
        Bufos();
        //Falta agregar el tiempo que demora. Por ahora voy a hacerlo pa que al salir de la batalla elimine los bufos.
    }
    public void DeBuff(string stat, int cant)
    {
        print("Baja: "+stat + " " + cant);
        switch (stat)
        {
            case "HP":
                Bufos1[0] -= cant;
                HP += cant;
                break;
            case "MP":
                Bufos1[1] -= cant;
                MP += cant;
                break;
            case "ATK":
                Bufos1[2] -= cant;
                break;
            case "DEF":
                Bufos1[3] -= cant;
                break;
            case "SPE":
                Bufos1[4] -= cant;
                break;
        }
        Bufos();
        //Falta agregar el tiempo que demora. Por ahora voy a hacerlo pa que al salir de la batalla elimine los bufos.
    }
    void Bufos()
    {
        //Acá meteré los bufos del equipamiento.
        Estadisticas();
        HP = GameManager.PJ.HP;
        MP = GameManager.PJ.MP;
        MaxHP += Bufos1[0];
        MaxMP += Bufos1[1];
        ATK += Bufos1[2];
        DEF += Bufos1[3];
        SPE += Bufos1[4];
    }
    public void Estadisticas()
    {
        MaxHP = Mathf.RoundToInt(((nivel - 1) * .1f) * clase.baseHP + clase.baseHP);
        HP1 = MaxHP;
        MaxMP = Mathf.RoundToInt(((nivel - 1) * .1f) * clase.baseMP + clase.baseMP);
        MP1 = MaxMP;
        ATK1 = Mathf.RoundToInt(((nivel - 1) * .25f) * clase.baseATK + clase.baseATK);
        DEF1 = Mathf.RoundToInt(((nivel - 1) * .25f) * clase.baseDEF + clase.baseDEF);
        SPE1 = Mathf.RoundToInt(((nivel - 1) * .25f) * clase.baseSPE + clase.baseSPE);
    }
    void Movimiento()
    {
        Animator mov = GetComponent<Animator>();
        if(puedeMoverse == true) {
            moviendose = true;
            float VelX = 0;
            float VelY = 0;
            if(InputManager.Key("Derecha"))
            {
                GameObject.FindGameObjectWithTag("Jugador").transform.localScale = new Vector2(5f, 5f);
                VelX = (mov_spe) * Time.deltaTime;
                VelY = 0;
                UltimaPos = 0.5f;
            }else if (InputManager.Key("Arriba"))
            {
                VelY = (mov_spe) * Time.deltaTime;
                VelX = 0;
                UltimaPos = 0.75f;
            }
            else if (InputManager.Key("Izquierda"))
            {
                GameObject.FindGameObjectWithTag("Jugador").transform.localScale = new Vector2(-5f, 5f);
                VelX = (mov_spe*-1) * Time.deltaTime;
                VelY = 0;
                UltimaPos = 0.25f;
            }
            else if (InputManager.Key("Abajo"))
            {
                VelY = (mov_spe*-1) * Time.deltaTime;
                VelX = 0;
                UltimaPos = 0f;
            }
            transform.Translate(new Vector2(VelX, VelY));
            mov.SetBool("Moviendose", true);
            mov.SetFloat("Mov X", VelX);
            mov.SetFloat("Mov Y", VelY);
            //moviendose = false;
        }
        if(!InputManager.Key("Derecha") && !InputManager.Key("Izquierda") && !InputManager.Key("Arriba") && !InputManager.Key("Abajo"))
        {
            moviendose = false;
            mov.SetBool("Moviendose", false);
            mov.Play("Nada", 0, UltimaPos);
        }
    }
    #endregion
}