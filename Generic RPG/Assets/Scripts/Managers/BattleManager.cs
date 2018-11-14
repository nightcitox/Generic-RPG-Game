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
    private Turno turno;
    public enum Turno
    {
        Personaje,
        Enemigo
    }
    #endregion
    #region elementos
    private Text texto;
    private Button boton;
    private RectTransform vidaPJ;
    private RectTransform manaPJ;
    private RectTransform vidaEn1;
    private RectTransform vidaEn2;
    private RectTransform vidaEn3;
    private AccionesEnemigo acc;
    bool esperas;
    private enum AccionesEnemigo
    {
        Atacar = 0,
        Curarse = 1,
        Escapar = 2
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

    public Turno Turno1
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
    #endregion
    #region Start y Update
    void Start() {
        if (EN1.Spe > pj.SPE1)
        {
            Turno1 = Turno.Enemigo;
        }
        else
        {
            Turno1 = Turno.Personaje;
        }
        Texto = GameObject.Find("Daño").GetComponent<Text>();
        vidaPJ = (RectTransform)GameObject.Find("BarraPJHP").gameObject.transform.Find("Centro");
        manaPJ = (RectTransform)GameObject.Find("BarraPJMP").gameObject.transform.Find("Centro");
        vidaEn1 = (RectTransform)GameObject.Find("BarraENM1").gameObject.transform.Find("Centro");
        GameObject.Find("BarraENM1").transform.Find("Text").GetComponent<Text>().text = EN1.Nombre;
        GameObject.Find("BarraPJHP").transform.Find("Nombre").GetComponent<Text>().text = pj.clase.nombre;
        esperas = false;
    }
    void Update()
    {
        //Actualizar la barra de vida automáticamente.
        StartCoroutine("BarraAliado");
        StartCoroutine("BarraEnemigos");
        StartCoroutine("Victoria");
        if (Input.GetAxis("Cancel") > 0 && Turno1 == Turno.Personaje)
        {
            GetComponent<Inventario>().Cerrar();
            GetComponent<SkillManager>().Cerrar();
        }
    }
    #endregion
    #region Métodos
    public void BotonesOFF()
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
    public void Rutinas(string rutina)
    {
        StartCoroutine(rutina);
    }
    void DecidirTurnos()
    {
        if(Turno1 == Turno.Enemigo)
        {
            Turno1 = Turno.Personaje;
        }
        else
        {
            Turno1 = Turno.Enemigo;
        }
    }
    #endregion
    #region rutinas
    IEnumerator Acciones()
    {
        if (Turno1 == Turno.Enemigo)
        {
            BotonesOFF();
            Texto.text = "El enemigo se está preparando.";
            yield return new WaitForSeconds(3f);
            Texto.text = "¡El enemigo va a " + acc + "!";
            yield return new WaitForSeconds(3f);

            //Script básico de probabilidades.
            float prob = Random.Range(0, 100);
            if (prob <= 10 && EN1.Hp < (EN1.Maxhp/3) )
            {
                acc = AccionesEnemigo.Escapar;
            }else if (prob > 10 && prob <= 90)
            {
                acc = AccionesEnemigo.Atacar;
            }
            else
            {
                acc = AccionesEnemigo.Curarse;
            }
            //End

            switch (acc)
            {
                case AccionesEnemigo.Atacar:
                    StartCoroutine(Ataque());
                    break;
                case AccionesEnemigo.Escapar:
                    Texto.text = "El enemigo ha escapado.";
                    yield return new WaitForSeconds(5f);
                    SceneManager.LoadScene("Mapa");
                    break;
                case AccionesEnemigo.Curarse:
                    EN1.Hp += 5;
                    DecidirTurnos();
                    break;
            }
        }
        else
        {
            Button[] listado = GameObject.Find("Panel").GetComponentsInChildren<Button>();
            foreach (Button x in listado)
            {
                x.interactable = true;
                texto.text = "¿Qué vas a hacer?";
            }
        }
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
    IEnumerator Ataque()
    {
        Animator anim;
        int daño;
        switch (Turno1)
        {
            case Turno.Enemigo:
                anim = GameObject.Find("Enemigo 1").GetComponent<Animator>();
                daño = (EN1.Atk - (pj.DEF1 / 10));
                Texto.text = "¡Has recibido " + daño + " de daño!";
                pj.HP1 = pj.HP1 - daño;
                anim.SetTrigger("Ataque");
                yield return new WaitForSeconds(3f);
                anim.ResetTrigger("Ataque");
                break;
            case Turno.Personaje:
                anim = GameObject.Find("Personaje").GetComponent<Animator>();
                daño = (pj.ATK1 - EN1.Def / 5);
                Texto.text = "¡Has hecho " + daño + " de daño!";
                EN1.Hp = EN1.Hp - daño;
                anim.SetTrigger("Ataque");
                yield return new WaitForSeconds(3f);
                anim.ResetTrigger("Ataque");
                break;
        }
        DecidirTurnos();
        StartCoroutine(Acciones());
    }
    public IEnumerator Esperar(Objeto item)
    {
        esperas = true;
        BotonesOFF();
        print("esperando...");
        yield return new WaitForSeconds(3f);
        esperas = false;
        print("esperado");
        if (item != null)
        {
            item.Cantidad -= 1;
        }
        StartCoroutine(Acciones());
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
    #endregion
}
