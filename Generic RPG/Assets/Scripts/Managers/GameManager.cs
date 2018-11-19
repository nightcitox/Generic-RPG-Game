using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
    //Almacena los datos de la misión actual, diálogo actual para mostrarlo y el Personaje actual con su nivel, experiencia y estadísticas.
    #region Propiedades
    public static string UsuarioConectado;
    public static bool sesion;
    public static PlanMision mision;
    public static GameObject DialogueHolder;
    public static Inventario inventario;
    static public Personaje PJ;
    static public int Experiencia;
    private List<int> niveles;
    public PlanClase clasesita;
    static public Vector2 PosMapa;
    public GameObject Prefab;
    public static bool menusActivos;
    public static InfoPartida info = new InfoPartida();
    #endregion
    #region Métodos
    void Start () {
        if (SceneManager.GetActiveScene().name == "MainMenu")
            VerificarGuardado(false);
        if(FindObjectOfType<Personaje>() != null)
            PJ = GameObject.Find("Personaje").GetComponent<Personaje>();
        if(PJ != null)
        {
            menusActivos = false;
            DialogueHolder = Prefab;
            if (PosMapa == new Vector2(0, 0))
            {
                PosMapa = new Vector2(0f, 0f);
            }
            if (clasesita != null)
            {
                PJ.clase = clasesita;
            }
            niveles = new List<int>();
            inventario = GetComponent<Inventario>();
            int formula = 0;
            for (int i = 0; i < 30; i++)
            {
                if (i >= 20)
                {
                    formula += ((i - 1) * 500) + 1500;
                }
                else if (i >= 10)
                {
                    formula += ((i - 1) * 250) + 1500;
                }
                else if (i == 0)
                {
                    formula = 0;
                }
                else
                {
                    formula += ((i - 1) * 250) + 1000;
                }
                niveles.Add(formula);
            }
            int j = 0;
            foreach (int x in niveles)
            {
                j += 1;
            }
            SubirNivel();
            if (SceneManager.GetActiveScene().name != "Batalla")
                GameObject.Find("Personaje").transform.position = PosMapa;
        }
    }
	void Update () {
        //Sistema de Guardado.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            info.posActualMapa = new float[2]
            {
                GameObject.Find("Personaje").GetComponent<Transform>().localPosition.x,
                GameObject.Find("Personaje").GetComponent<Transform>().localPosition.y
            };
            info.Experiencia = Experiencia;
            info.nombre = PJ.Nombre;
            info.clase = clasesita.nombre;
            info.hp = PJ.HP1;
            info.mp = PJ.MP1;
            info.mapaActual = SceneManager.GetActiveScene().name;
            GuardarPartida();
        }
        //Sistema de Cargado.
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            info = CargarPartida();
            GameObject.Find("Personaje").GetComponent<Transform>().localPosition = new Vector2(info.posActualMapa[0], info.posActualMapa[1]);
        }
        if (InputManager.KeyDown("Menus") && menusActivos)
        {
            switch (inventario.Abierto)
            {
                case true:
                    inventario.Cerrar();
                    break;
                case false:
                    Menus();
                    break;
            }
        }
        //Event System para la Interfaz con el Custom Input Manager.
        #region Movimiento en UI
        if(InputManager.GUIActivo)
        {
            AudioSource sfx = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioSource>();
            PJ.puedeMoverse = false;
            AxisEventData ad = new AxisEventData(EventSystem.current);
            AudioClip cursor = Resources.Load<AudioClip>("SFX/GUI/Ogg/Cursor_tones/cursor_style_2");
            if (ad.selectedObject == null)
                GameObject.Find("Atacar").GetComponent<Button>().Select();
            if (InputManager.KeyDown("Arriba"))
            {
                sfx.PlayOneShot(cursor);
                ad.moveDir = MoveDirection.Up;
            }
            else if (InputManager.KeyDown("Abajo"))
            {
                sfx.PlayOneShot(cursor);
                ad.moveDir = MoveDirection.Down;
            }
            else if (InputManager.KeyDown("Izquierda"))
            {
                sfx.PlayOneShot(cursor);
                ad.moveDir = MoveDirection.Left;
            }
            else if (InputManager.KeyDown("Derecha"))
            {
                sfx.PlayOneShot(cursor);
                ad.moveDir = MoveDirection.Right;
            }
            else if (InputManager.KeyDown("Aceptar"))
            {
                cursor = Resources.Load<AudioClip>("SFX/GUI/Ogg/Confirm_tones/style2/confirm_style_2_002");
                sfx.PlayOneShot(cursor);
                ad.selectedObject.gameObject.GetComponent<Button>().OnSubmit(ad);
            }
            else
                return;
            ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, ad, ExecuteEvents.moveHandler);
        }
        else if(PJ != null)
        {
            PJ.puedeMoverse = true;
        }
        #endregion
    }
    void OnGUI()
    {
        if(SceneManager.GetActiveScene().name == "Batalla")
            InputManager.GUIActivo = true;
    }
    void Menus()
    {
        inventario.Abrir();
    }
    public void SubirNivel()
    {
        int nivelanterior = PJ.Nivel;
        int j = 0;
        foreach (int x in niveles)
        {
            if(Experiencia >= x)
            {
                PJ.Nivel = j+1;
            }
            j += 1;
        }
        if(nivelanterior != PJ.Nivel)
        {
            PJ.Estadisticas();
        }
    }
    public void EscogerClase()
    {

    }
    #endregion
    #region Guardar y Cargar
    public static void GuardarPartida()
    {
        const string carpeta = "Guardados";
        const string ext = ".dat";
        string path = Application.persistentDataPath +"/"+ carpeta;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string dataPath = Path.Combine(path, UsuarioConectado.ToLower() + ext);
        BinaryFormatter formateador = new BinaryFormatter();
        using (FileStream fs = File.Open(dataPath, FileMode.OpenOrCreate))
        {
            formateador.Serialize(fs, info);
        }
    }
    public static InfoPartida CargarPartida()
    {
        string[] filePaths = GetFilePaths();
        if (filePaths.Length > 0)
        {
            BinaryFormatter formateador = new BinaryFormatter();
            using (FileStream fs = File.Open(filePaths[0], FileMode.Open))
            {
                return (InfoPartida)formateador.Deserialize(fs);
            }
        }
        else
        {
            print("No se encontraron datos guardados.");
            return null;
        }
    }
    static string[] GetFilePaths()
    {
        const string carpeta = "Guardados";
        string ext = UsuarioConectado.ToLower() + ".dat";
        string path = Application.persistentDataPath + "/" + carpeta;
        return Directory.GetFiles(path, ext);
    }
    #endregion
    #region Cambiar Escenas
    public void CambiarEscena(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    #endregion
    #region Control de Menus
    static bool opciones;
    public void VerificarGuardado(bool carga)
    {
        bool partida = false;
        if (GetFilePaths().Length == 0)
        {
            GameObject.Find("btn_Partida").GetComponentInChildren<Text>().text = "Nueva Partida";
            partida = false;
        }
        else
        {
            GameObject.Find("btn_Partida").GetComponentInChildren<Text>().text = "Continuar";
            partida = true;
        }
        if (carga)
        {
            switch (partida)
            {
                case true:
                    info = CargarPartida();
                    SceneManager.LoadScene(info.mapaActual);
                    PosMapa.x = info.posActualMapa[0];
                    PosMapa.y = info.posActualMapa[1];
                    break;
                case false:
                    SceneManager.LoadScene("Mapa");
                    break;
            }
        }
        
    }
    public void Volumen()
    {
        if (opciones || SceneManager.GetActiveScene().name == "MainMenu")
        {
            AudioSource[] audios = FindObjectsOfType<AudioSource>();
            foreach(AudioSource x in audios)
            {
                x.volume = FindObjectOfType<Slider>().value;
            }
        }
    }
    public void Resolucion()
    {
        if(opciones || SceneManager.GetActiveScene().name == "MainMenu")
        {
            Dropdown res = GameObject.Find("Resolucion").GetComponent<Dropdown>();
            string[] valores = res.options[res.value].text.Split('x');
            int[] resFinal = new int[2]
            {
                int.Parse(valores[0]),
                int.Parse(valores[1])
            };
            Screen.SetResolution(resFinal[0], resFinal[1], FullScreenMode.Windowed);
        }
    }
    public void Graficos()
    {
        if (opciones || SceneManager.GetActiveScene().name == "MainMenu")
        {
            Dropdown res = GameObject.Find("Graficos").GetComponent<Dropdown>();
            QualitySettings.SetQualityLevel(res.value);
        }
    }
    public void CerrarJuego()
    {
        Application.Quit();
    }
    #endregion
}
