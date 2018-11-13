using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {
    #region propiedades
    public Personaje pj;
    public Enemigo EN1;
    private Enemigo EN2;
    private Enemigo EN3;
    private Mapa zona;
    private int turno;
    #endregion
    #region elementos
    private string poseedorTurno = "";
    private Animator anim;
    private Animator atak;
    private SpriteRenderer esprai;
    public Sprite hoja;
    private Text texto;
    private Vector3 pos;
    private Button boton;
    private RectTransform vidaPJ;
    private RectTransform manaPJ;
    private RectTransform vidaEn1;
    private RectTransform vidaEn2;
    private RectTransform vidaEn3;
    private AccionesEnemigo acc;
    bool rutina;
    bool esperas;
    private enum AccionesEnemigo
    {
        Atacar = 0,
        Curarse = 1,
        Escapar = 2
    }
    public int Turno
    {
        get
        {
            return turno;
        }

        set
        {
            turno = value;
        }
    }

    public Text Texto
    {
        get
        {
            return texto;
        }

        set
        {
            texto = value;
        }
    }
    #endregion
    void Start() {
        Turno = 1;
        Texto = GameObject.Find("Daño").GetComponent<Text>();
        anim = GameObject.Find("Personaje").GetComponent<Animator>();
        atak = GameObject.Find("Ataque").GetComponent<Animator>();
        vidaPJ = (RectTransform)GameObject.Find("BarraPJHP").gameObject.transform.Find("Centro");
        manaPJ = (RectTransform)GameObject.Find("BarraPJMP").gameObject.transform.Find("Centro");
        vidaEn1 = (RectTransform)GameObject.Find("BarraENM1").gameObject.transform.Find("Centro");
        GameObject.Find("BarraENM1").transform.Find("Text").GetComponent<Text>().text = EN1.Nombre;
        GameObject.Find("BarraPJHP").transform.Find("Nombre").GetComponent<Text>().text = pj.clase.nombre;
        esperas = false;
    }
    void BotonesOFF()
    {
        Button[] listado = GameObject.Find("Panel").GetComponentsInChildren<Button>();
        foreach (Button x in listado)
        {
            x.interactable = false;
        }
        if(esperas == true)
        {
            GameObject.Find("Panel").transform.Find("PanelHabilidades").gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            GameObject.Find("Panel").transform.Find("Inventario").gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        }
        else
        {
            GameObject.Find("Panel").transform.Find("PanelHabilidades").gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            GameObject.Find("Panel").transform.Find("Inventario").gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            GetComponent<Inventario>().Cerrar();
            GetComponent<SkillManager>().Cerrar();
        }

    }
    public IEnumerator Esperar(Objeto item)
    {
        rutina = true;
        esperas = true;
        StopAllCoroutines();
        BotonesOFF();
        print("esperando...");
        yield return new WaitForSeconds(3f);
        rutina = false;
        esperas = false;
        BotonesOFF();
        print("esperado");
        if (item != null)
        {
            item.Cantidad -= 1;
        }
    }
    IEnumerator Victoria()
    {
        if (EN1.Hp <= 0)
        {
            yield return new WaitForSeconds(3f);
            Texto.text = "Ganaste!";
            yield return new WaitForSeconds(3f);
            Texto.text = "Has recibido " + EN1.Exp + " puntos de experiencia.";
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("Mapa");
        }
        else if (pj.HP1 <= 0)
        {
            yield return new WaitForSeconds(3f);
            Texto.text = "Perdiste!";
            yield return new WaitForSeconds(3f);
        }
        yield return true;
    }
    void Update() {
        //Actualizar la barra de vida automáticamente.
        StartCoroutine("BarraAliado");
        StartCoroutine("BarraEnemigos");
        StartCoroutine("Victoria");
        if (rutina == false)
        {
            StartCoroutine("Turnos");
        }
        else
        {
        }
        if(Input.GetAxis("Cancel") > 0 && poseedorTurno == "Personaje")
        {
            GetComponent<Inventario>().Cerrar();
            GetComponent<SkillManager>().Cerrar();
        }
    }
    public void Rutinas(string rutina)
    {
        StartCoroutine(rutina);
    }
    void AccEn()
    {
        acc = (AccionesEnemigo)Random.Range(0, 1);
        StartCoroutine("Acciones");
    }
    IEnumerator Acciones()
    {
        rutina = true;
        if (poseedorTurno == "Enemigo")
        {
            BotonesOFF();
            Texto.text = "El enemigo se está preparando.";
            yield return new WaitForSeconds(3f);
            Texto.text = "¡El enemigo va a " + acc + "!";
            yield return new WaitForSeconds(3f);
            switch (acc)
            {
                case AccionesEnemigo.Atacar:
                    Daño();
                    rutina = true;
                    break;
                case AccionesEnemigo.Escapar:
                    Texto.text = "El enemigo ha escapado.";
                    yield return new WaitForSeconds(5f);
                    SceneManager.LoadScene("Mapa");
                    rutina = false;
                    break;
                case AccionesEnemigo.Curarse:
                    EN1.Hp += 5;
                    turno += 1;
                    rutina = false;
                    break;
            }
            rutina = false;
            yield return true;
        }
        else
        {
            Button[] listado = GameObject.Find("Panel").GetComponentsInChildren<Button>();
            foreach (Button x in listado)
            {
                x.interactable = true;
                texto.text = "¿Qué vas a hacer?";
            }
            rutina = false;
            yield return true;
        }
        rutina = false;
        yield return StartCoroutine("Turnos");
    }
    IEnumerator Turnos()
    {
        print(poseedorTurno);
        print(turno);
        if (EN1.Spe > pj.SPE1)
        {
            if(turno%2 == 1)
            {
                GetComponent<SkillManager>().Cerrar();
                GetComponent<Inventario>().Cerrar();
                poseedorTurno = "Enemigo";
            }
            else
            {
                poseedorTurno = "Personaje";
            }
        }
        else
        {
            if (turno % 2 == 1)
            {
                poseedorTurno = "Personaje";
            }
            else
            {
                poseedorTurno = "Enemigo";
                GetComponent<SkillManager>().Cerrar();
                GetComponent<Inventario>().Cerrar();
            }
        }
        yield return true;
    }
    IEnumerator BarraAliado()
    {
        //Barra de HP
        GameObject.Find("BarraPJHP").transform.Find("Centro").transform.Find("HP").GetComponent<Text>().text = "HP - " + pj.HP1.ToString();
        float alto = vidaPJ.rect.height;
        float ancho = 300 * (pj.HP1 / (float)pj.MaxHP1);
        float movimiento = (vidaPJ.rect.width - ancho) / 2f;
        if (ancho != 0)
        {
            vidaPJ.sizeDelta = new Vector2(ancho, alto);
            vidaPJ.anchoredPosition = new Vector2(vidaPJ.anchoredPosition.x - movimiento, vidaPJ.anchoredPosition.y);
        }
        //END

        //Barra de MP
        GameObject.Find("BarraPJMP").transform.Find("Centro").transform.Find("MP").GetComponent<Text>().text = "MP - " + pj.MP1.ToString();
        float altoMP = manaPJ.rect.height;
        float anchoMP = 300 * (pj.MP1 / (float)pj.MaxMP1);
        float movimientoMP = (manaPJ.rect.width - anchoMP) / 2f;
        if (anchoMP != 0)
        {
            manaPJ.sizeDelta = new Vector2(anchoMP, altoMP);
            manaPJ.anchoredPosition = new Vector2(manaPJ.anchoredPosition.x - movimientoMP, manaPJ.anchoredPosition.y);
        }
        yield return true;
    }
    IEnumerator BarraEnemigos()
    {
        GameObject.Find("BarraENM1").transform.Find("Centro").transform.Find("HP").GetComponent<Text>().text = "HP - " + EN1.Hp.ToString();
        float alto = vidaEn1.rect.height;
        float ancho = 300 * (EN1.Hp / (float)EN1.Maxhp);
        float movimiento = (vidaEn1.rect.width - ancho) / 2f;
        if (ancho != 0)
        {
            vidaEn1.sizeDelta = new Vector2(ancho, alto);
            vidaEn1.anchoredPosition = new Vector2(vidaEn1.anchoredPosition.x - movimiento, vidaEn1.anchoredPosition.y);
        }
        yield return true;
    }
    #region rutinas
    void Daño()
    {
        AccEn();
        BotonesOFF();
        if (poseedorTurno == "Personaje")
        {
            int daño = (pj.ATK1 - EN1.Def);
            Texto.text = "¡Has hecho "+ daño+ " de daño!";
            EN1.Hp = EN1.Hp - (pj.ATK1 - EN1.Def/10);
        }
        else
        {
            int daño = (EN1.Atk - (pj.DEF1 / 10));
            Texto.text = "¡Has recibido " + daño + " de daño!";
            pj.HP1 = pj.HP1 - daño;
        }
        StartCoroutine("AtaqueAnim");
    }
    IEnumerator AtaqueAnim()
    {
        if(poseedorTurno == "Personaje")
        {
            anim.SetTrigger("Ataque");
            esprai = atak.GetComponent<SpriteRenderer>();
            esprai.sprite = hoja;
            atak.SetBool("Ataque", true);
            pos = atak.transform.position;
            atak.transform.Translate(new Vector3(5, 0, 0));
            print("comienza");
            yield return new WaitForSeconds(1.5f);
            Turno += 1;
            print("termina");
            anim.ResetTrigger("Ataque");
            atak.SetBool("Ataque", false);
            esprai.sprite = null;
            atak.transform.position = new Vector3(-5.5f, 0.5f, 0f);
        }
        else
        {
            Animator uwu = GameObject.Find("Enemigo 1").GetComponent<Animator>();
            uwu.SetTrigger("Ataque");
            Turno += 1;
            uwu.ResetTrigger("Ataque");
        }
        AccEn();
        yield return true;
    }
    #endregion
}
