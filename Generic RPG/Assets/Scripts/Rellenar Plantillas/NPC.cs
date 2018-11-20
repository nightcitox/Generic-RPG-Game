using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour {
    // Ésta es la clase contenedora de la planilla creada para el NPC, es decir, acá se 
    // ingresa el objeto creado anteriormente.
    #region Propiedades
    private string nombre;
    private PlanDialogo dialogos;
    public PlanNPCs npc;
    private PlanMision mision;
    private Text texto;
    bool interaccion;
    bool chatIniciado;
    int index;
    bool puedeContinuar;
    bool MisionActiva;
    #endregion
    void Start()
    {
        nombre = npc.nombre;
        dialogos = npc.dialogos;
    }
    void Update()
    {
        if (interaccion)
        {
            //Aquí coloco que cuando entra en colisión y presiona el botón denominado como Submit
            //haga todo el proceso de los diálogos pero OJO siempre y cuando esté en colisión y permaneza así.
            if (InputManager.KeyDown("Aceptar") && GameManager.menusActivos)
            {
                FindObjectOfType<GameManager>().PuedeAbrirMenu = false;
                GameObject Holder = Instantiate(GameManager.DialogueHolder, FindObjectOfType<Canvas>().transform);
                Holder.name = "Holder";
                texto = GameObject.Find("Dialogo").GetComponent<Text>();
                GameObject.Find("NPCName").GetComponent<Text>().text = nombre;
                AudioClip GUI = Resources.Load<AudioClip>("SFX/GUI/Ogg/Confirm_tones/style2/confirm_style_2_002");
                FindObjectOfType<AudioSource>().PlayOneShot(GUI);
                index = 0;
                //Primero busco el objeto de Personaje.
                Personaje.puedeMoverse = false;
                //Si hay más de un diálogo presente en el sistema del NPC se filtra para que funcionen todos.
                char[] tex = dialogos.dialogos[index].ToCharArray();
                StartCoroutine(TextoCambiante(tex));
                interaccion = false;
            }
        }else if (chatIniciado)
        {
            if (InputManager.KeyDown("Aceptar") && puedeContinuar)
            {
                AudioClip GUI = Resources.Load<AudioClip>("SFX/GUI/Ogg/Confirm_tones/style2/confirm_style_2_002");
                FindObjectOfType<AudioSource>().PlayOneShot(GUI);
                IniciarChat();
            }
        }
    }
    void IniciarChat()
    {
        index += 1;
        if (dialogos.dialogos.Length >= (index+1))
        {
            char[] tex = dialogos.dialogos[index].ToCharArray();
            StartCoroutine(TextoCambiante(tex));
            //Si presiona de nuevo el botón de aceptar, el diálogo acaba y pasaría al siguiente.
        }
        else
        {
            print("No quedan más diálogos");
            if (dialogos.tipo == PlanDialogo.TipoDialogo.ConOpciones)
            {
                puedeContinuar = false;
                GameObject.Find("Holder").transform.Find("Opciones").gameObject.SetActive(true);
                Button con = GameObject.Find("Confirmar").GetComponent<Button>();
                con.Select();
                con.onClick.AddListener(CrearMision);
                Button negar = GameObject.Find("Negar").GetComponent<Button>();
                negar.onClick.AddListener(Salir);
            }
            else
            {
                texto.text = "";
                Destroy(GameObject.Find("Holder"));
                chatIniciado = false;
                Personaje.puedeMoverse = false;
                interaccion = true;
                puedeContinuar = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Jugador")) { interaccion = true; }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        interaccion = false;
    }
    IEnumerator TextoCambiante(char[] tex)
    {
        puedeContinuar = false;
        texto.text = "";
        for (int j = 0; j < tex.Length; j++)
        {
            texto.text = texto.text + tex[j];
            yield return new WaitForSeconds(0.032f);
        }
        yield return new WaitForSeconds(0.5f);
        puedeContinuar = true;
        chatIniciado = true;
    }
    void Salir()
    {
        chatIniciado = false;
        Personaje.puedeMoverse = false;
        interaccion = true;
        puedeContinuar = false;
        Destroy(GameObject.Find("Holder"));
        GameManager.menusActivos = true;
    }
    public void CrearMision()
    {
        if(gameObject.name == "Escoger_Clase")
        {
            FindObjectOfType<GameManager>().EscogerClase(nombre);
            FindObjectOfType<GameManager>().PuedeAbrirMenu = true;
        }
        //En este, se crea la misión y se redirige al GameManager para que se coloque como misión actual y se pueda
        //cumplir el objetivo de la misma.
        GameManager.mision = mision;
        chatIniciado = false;
        Personaje.puedeMoverse = false;
        interaccion = true;
        puedeContinuar = false;
        Destroy(GameObject.Find("Holder"));
        GameManager.menusActivos = true;
    }
    public void AsignarMision(PlanMision asigMision)
    {
        //Método creado principalmente para asignar una misión al NPC actual, ya que, las misiones pueden ser más
        //de una, además de que pueden ser parte de una secuencia.
        mision = asigMision;
    }
}