using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    //Almacena los datos de la misión actual, diálogo actual para mostrarlo y el Personaje actual con su nivel, experiencia y estadísticas.
    #region Propiedades
    public static string UsuarioConectado;
    public Sprite imagenBoton;
    public static bool sesion;
    public static PlanMision mision;
    public static GameObject DialogueHolder;
    public static Inventario inventario;
    static public Personaje PJ;
    static public int Experiencia;
    public static List<int> niveles;
    public static PlanClase clasesita;
    static public Vector2 PosMapa;
    public GameObject Prefab;
    public static bool menusActivos;
    public static bool partidaCargada;
    public static InfoPartida info = new InfoPartida();
    public static float volumen = 1f;
    public bool PuedeAbrirMenu;
    public static string Escena;
    public static GameManager instance = null;
    #endregion
    #region Métodos
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            PJ = FindObjectOfType<Personaje>();
        }
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        FindObjectOfType<Personaje>().clase = PJ.clase;
        AudioSource bgm = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        bgm.volume = volumen;
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            InputManager.GUIActivo = true;
            VerificarGuardado(false);
        }
        if (PJ != null)
        {
            DialogueHolder = Prefab;
            if (PosMapa == new Vector2(0, 0))
            {
                PosMapa = new Vector2(0f, 0f);
            }
            if (PJ.clase == null)
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
            if (SceneManager.GetActiveScene().name != "Batalla" && partidaCargada)
            {
                GameObject.Find("Personaje").transform.position = PosMapa;
                partidaCargada = false;
            }
            if (SceneManager.GetActiveScene().name != "Batalla")
            {
                SubirNivel();
            }
        }
    }
    void Update()
    {
        Scene actual = SceneManager.GetActiveScene();
        if (actual.name != "Login" && actual.name != "Registro" && actual.name != "MainMenu" && actual.name != "Batalla")
        {
            GameObject menu = FindObjectOfType<Canvas>().transform.Find("MainMenu").transform.Find("General").gameObject;
            if (InputManager.KeyDown("Menus") && menusActivos && PuedeAbrirMenu)
            {
                AbrirMenu();
            }
            else if ((InputManager.KeyDown("Menus") || (InputManager.KeyDown("Cancelar"))) && !menusActivos && menu.activeSelf)
            {
                CerrarMenu();
            }
        }
        //Event System para la Interfaz con el Custom Input Manager.
        #region Movimiento en UI
        if (InputManager.GUIActivo)
        {
            AudioSource sfx = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioSource>();
            sfx.volume = volumen;
            if (PJ != null)
                Personaje.puedeMoverse = false;
            AxisEventData ad = new AxisEventData(EventSystem.current);
            AudioClip cursor = Resources.Load<AudioClip>("SFX/GUI/Ogg/Cursor_tones/cursor_style_2");
            if (ad.selectedObject == null && SceneManager.GetActiveScene().name == "Batalla")
                GameObject.Find("Atacar").GetComponent<Button>().Select();
            if (ad.selectedObject != null)
            {
                if (InputManager.KeyDown("Arriba"))
                {
                    ad.moveDir = MoveDirection.Up;
                }
                else if (InputManager.KeyDown("Abajo"))
                {
                    ad.moveDir = MoveDirection.Down;
                }
                else if (InputManager.KeyDown("Izquierda"))
                {
                    ad.moveDir = MoveDirection.Left;
                }
                else if (InputManager.KeyDown("Derecha"))
                {
                    ad.moveDir = MoveDirection.Right;
                }
                else if (InputManager.KeyDown("Aceptar"))
                {
                    cursor = Resources.Load<AudioClip>("SFX/GUI/Ogg/Confirm_tones/style2/confirm_style_2_002");

                    ad.selectedObject.gameObject.GetComponent<Button>().OnSubmit(ad);
                }
                else
                    return;
                sfx.PlayOneShot(cursor);
            }
            ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, ad, ExecuteEvents.moveHandler);
            if (SceneManager.GetActiveScene().name != "Batalla")
            {
                GameObject menu = FindObjectOfType<Canvas>().transform.Find("MainMenu").gameObject;
                if (menu.activeSelf)
                {
                    MoverBotones(ad);
                }
            }
        }
        #endregion
    }
    void OnGUI()
    {
        if (SceneManager.GetActiveScene().name != "Login" && SceneManager.GetActiveScene().name != "Registro" && SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (SceneManager.GetActiveScene().name == "Batalla" || FindObjectOfType<Canvas>().transform.Find("MainMenu").gameObject.activeSelf)
            {
                InputManager.GUIActivo = true;
            }
            else
            {
                InputManager.GUIActivo = false;
            }
        }
    }
    public void SubirNivel()
    {
        int nivelanterior = PJ.Nivel;
        int j = 0;
        foreach (int x in niveles)
        {
            if (Experiencia >= x)
            {
                PJ.Nivel = j + 1;
            }
            j += 1;
        }
        if (nivelanterior != PJ.Nivel)
        {
            PJ.Estadisticas();
        }
    }
    public void EscogerClase(string clase)
    {
        string[] nombres = UnityEditor.AssetDatabase.FindAssets(clase);
        for (int i = 0; i < nombres.Length; i++)
        {
            string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(nombres[i]);
            PlanClase clasex = UnityEditor.AssetDatabase.LoadAssetAtPath<PlanClase>(assetPath);
            if (clasex != null)
            {
                clasesita = clasex;
                PJ.clase = clasex;
                PJ.Estadisticas();
                SubirNivel();
            }
        }
    }
    #endregion
    #region Guardar y Cargar
    public void GuardarPartida()
    {
        //Propiedades que guarda.
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
        //fin
        const string carpeta = "Guardados";
        const string ext = ".dat";
        string path = Application.persistentDataPath + "/" + carpeta;
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
        CerrarMenu();
    }
    public InfoPartida CargarPartida()
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
    #region Control de Menus
    public static bool opciones;
    bool partida;
    Button[] botonMenu;
    void FuncionBotones()
    {
        botonMenu = new Button[4];
        botonMenu[0] = GameObject.Find("MainMenu").transform.Find("General").transform.Find("InventarioBTN").gameObject.GetComponent<Button>();
        botonMenu[0].onClick.AddListener(BotonInventario);

        botonMenu[1] = GameObject.Find("MainMenu").transform.Find("General").transform.Find("HabilidadesBTN").gameObject.GetComponent<Button>();
        botonMenu[1].onClick.AddListener(BotonHabilidad);

        botonMenu[2] = GameObject.Find("MainMenu").transform.Find("General").transform.Find("ReanudarBTN").gameObject.GetComponent<Button>();
        botonMenu[2].onClick.AddListener(CerrarMenu);

        botonMenu[3] = GameObject.Find("MainMenu").transform.Find("Guardar").transform.Find("AceptarBTN").gameObject.GetComponent<Button>();
        botonMenu[3].onClick.AddListener(GuardarPartida);

        // On Value Change listener del cosito.
        Dropdown dd = GameObject.Find("MainMenu").transform.Find("Panel_Graficos").transform.Find("Panel").transform.Find("GraficosCH").gameObject.GetComponent<Dropdown>();
        dd.onValueChanged.AddListener(delegate { Graficos(dd); });

        Slider sl = GameObject.Find("MainMenu").transform.Find("Panel_Graficos").transform.Find("Panel").transform.Find("VolumenCH").gameObject.GetComponent<Slider>();
        sl.onValueChanged.AddListener(delegate { Volumen(sl); });

        dd = GameObject.Find("MainMenu").transform.Find("Panel_Graficos").transform.Find("Panel").transform.Find("ResolucionCH").gameObject.GetComponent<Dropdown>();
        dd.onValueChanged.AddListener(delegate { Resolucion(dd); });
    }
    void BotonInventario()
    {
        inventario.Abrir();
        FindObjectOfType<SkillManager>().Cerrar();
    }
    void BotonHabilidad()
    {
        FindObjectOfType<SkillManager>().Desplegar();
        inventario.Cerrar();
    }
    public void VerificarGuardado(bool carga)
    {
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
        print(partida);
        if (carga)
        {
            switch (partida)
            {
                case true:
                    info = CargarPartida();
                    InputManager.GUIActivo = false;
                    partidaCargada = true;
                    SceneManager.LoadScene(info.mapaActual);
                    PosMapa.x = info.posActualMapa[0];
                    PosMapa.y = info.posActualMapa[1];
                    break;
                case false:
                    InputManager.GUIActivo = false;
                    SceneManager.LoadScene("Escoger Clase");
                    break;
            }
        }
    }
    public void Volumen(Slider vol)
    {
        if (opciones || SceneManager.GetActiveScene().name == "MainMenu")
        {
            AudioSource[] audios = FindObjectsOfType<AudioSource>();
            foreach (AudioSource x in audios)
            {
                x.volume = vol.value;
            }
            volumen = vol.value;
        }
        else
        {
            print("Opciones desactivado");
        }
    }
    public void Resolucion(Dropdown res)
    {
        if (opciones || SceneManager.GetActiveScene().name == "MainMenu")
        {
            string[] valores = res.options[res.value].text.Split('x');
            int[] resFinal = new int[2]
            {
                int.Parse(valores[0]),
                int.Parse(valores[1])
            };
            Screen.SetResolution(resFinal[0], resFinal[1], FullScreenMode.Windowed);
        }
    }
    public void Graficos(Dropdown res)
    {
        if (opciones || SceneManager.GetActiveScene().name == "MainMenu")
        {
            QualitySettings.SetQualityLevel(res.value);
        }
    }
    public void CerrarJuego()
    {
        Application.Quit();
    }
    private Button[] botones;
    public void MoverBotones(AxisEventData ad)
    {
        botones = FindObjectsOfType<Button>();
        if (ad.selectedObject == null)
        {
            botones[0].Select();
        }
        foreach (Button x in botones)
        {
            if (x.gameObject != ad.selectedObject)
            {
                x.GetComponent<Image>().sprite = null;
                x.GetComponent<Image>().color = new Color(255, 255, 255, 0);
            }
            else
            {
                x.GetComponent<Image>().sprite = imagenBoton;
                x.GetComponent<Image>().color = new Color(255, 255, 255, 255);
            }
        }
    }
    void AbrirMenu()
    {
        opciones = true;
        menusActivos = false;
        FindObjectOfType<Canvas>().transform.Find("MainMenu").gameObject.SetActive(true);
        FuncionBotones();
        AudioClip cursor = Resources.Load<AudioClip>("SFX/GUI/Ogg/Confirm_tones/style2/confirm_style_2_002");
        AudioSource sfx = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioSource>();
        sfx.volume = volumen;
        sfx.PlayOneShot(cursor);
    }
    public void CerrarMenu()
    {
        FindObjectOfType<Canvas>().transform.Find("MainMenu").gameObject.SetActive(false);
        AudioClip cursor = Resources.Load<AudioClip>("SFX/GUI/Ogg/Confirm_tones/style2/confirm_style_2_002");
        AudioSource sfx = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioSource>();
        sfx.volume = volumen;
        sfx.PlayOneShot(cursor);
        Personaje.puedeMoverse = true;
        menusActivos = true;
        opciones = false;
    }
    public void Estadísticas()
    {
        if (opciones)
        {
            GameObject.Find("NombreNivel").GetComponent<Text>().text = PJ.Nombre + " Nivel " + PJ.Nivel;
            GameObject.Find("Atk").GetComponent<Text>().text = "ATK " + (PJ.ATK1 - PJ.Bufos1[2]) + " + " + PJ.Bufos1[2];
            GameObject.Find("Spe").GetComponent<Text>().text = "SPE " + (PJ.SPE1 - PJ.Bufos1[4]) + " + " + PJ.Bufos1[4];
            GameObject.Find("Def").GetComponent<Text>().text = "DEF " + (PJ.DEF1 - PJ.Bufos1[3]) + " + " + PJ.Bufos1[3];
            GameObject.Find("PJIdle").GetComponent<Animator>().SetBool(PJ.clase.nombre, true);
            //300 el largo de la barrita
            //% experiencia
            float porcentaje = (float)Experiencia / (float)niveles[PJ.Nivel + 1];
            porcentaje = porcentaje * 300;
            GameObject.Find("Barrita").GetComponent<RectTransform>().sizeDelta = new Vector2(porcentaje, 22);
            GameObject.Find("EXP").GetComponent<Text>().text = "EXP " + Experiencia + "/" + niveles[PJ.Nivel + 1];
            //HP Count barrita numeritos cosa.
            porcentaje = PJ.HP1 / PJ.MaxHP1;
            porcentaje = porcentaje * 140;
            GameObject.Find("BarritaHP").GetComponent<RectTransform>().sizeDelta = new Vector2(porcentaje, 22);
            GameObject.Find("HPCount").GetComponent<Text>().text = PJ.HP1 + "/" + PJ.MaxHP1;
            //MP Count barrita numeritos cosa.
            porcentaje = PJ.MP1 / PJ.MaxMP1;
            porcentaje = porcentaje * 140;
            GameObject.Find("BarritaMP").GetComponent<RectTransform>().sizeDelta = new Vector2(porcentaje, 22);
            GameObject.Find("MPCount").GetComponent<Text>().text = PJ.MP1 + "/" + PJ.MaxMP1;

        }
    }
    #endregion
}
