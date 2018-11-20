using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {
    #region Propiedades
    public Personaje pj;
    public Enemigo EN1;
    private Mapa zona;
    private Turno turno;
    public AccionesPJ accionespj;
    private Habilidad HabUtilizada;
    private Objeto ObjUtilizado;
    int finalizar;
    public enum Turno
    {
        Personaje,
        Enemigo
    }
    public enum AccionesPJ
    {
        Atacar,
        Habilidad,
        Objeto,
        Escapar,
        Decision
    }
    #endregion
    #region Elementos
    private Text texto;
    private Button boton;
    private RectTransform vidaPJ;
    private RectTransform manaPJ;
    private RectTransform vidaEn1;
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

    public Habilidad HabUtilizada1
    {
        get
        {
            return HabUtilizada;
        }

        set
        {
            HabUtilizada = value;
        }
    }

    public Objeto ObjUtilizado1
    {
        get
        {
            return ObjUtilizado;
        }

        set
        {
            ObjUtilizado = value;
        }
    }
    #endregion
    #region Start y Update
    void Start() {
        pj = GameManager.PJ;
        Personaje pjBattle = GameObject.Find("Personaje").AddComponent<Personaje>();
        pjBattle.puedeMoverse = false;
        pjBattle.clase = pj.clase;
        Texto = GameObject.Find("Estado").GetComponent<Text>();
        vidaPJ = (RectTransform)GameObject.Find("BarraPJHP").gameObject.transform.Find("Centro");
        manaPJ = (RectTransform)GameObject.Find("BarraPJMP").gameObject.transform.Find("Centro");
        vidaEn1 = (RectTransform)GameObject.Find("BarraENM1").gameObject.transform.Find("Centro");
        GameObject.Find("BarraPJHP").transform.Find("Nombre").GetComponent<Text>().text = pj.clase.nombre;
        esperas = false;
        finalizar = 0;
        accionespj = AccionesPJ.Decision;
        GameObject.Find("Atacar").GetComponent<Button>().Select();
    }
    void Update()
    {
        //Actualizar la barra de vida automáticamente.
        if(EN1.Hp <= 0 || pj.HP1 <= 0)
        {

            StartCoroutine("Victoria");
        }
        else
        {
            StartCoroutine("BarraAliado");
            StartCoroutine("BarraEnemigos");
        }
        if (InputManager.KeyDown("Cancelar") && accionespj == AccionesPJ.Decision)
        {
            GetComponent<Inventario>().Cerrar();
            GetComponent<SkillManager>().Cerrar();
            BotonesON();
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
    public void DecidirTurnos()
    {
        if (accionespj != AccionesPJ.Decision)
        {
            if (Turno1 == Turno.Enemigo)
            {
                Turno1 = Turno.Personaje;
            }
            else
            {
                Turno1 = Turno.Enemigo;
            }
        }
        else
        {
            finalizar = 0;
            if (EN1.Spe > pj.SPE1)
            {
                turno = Turno.Enemigo;
            }
            else
            {
                turno = Turno.Personaje;
            }
        }
        if (finalizar == 2)
        {
            accionespj = AccionesPJ.Decision;
            DecidirTurnos();
        }
        StartCoroutine(Acciones());
    }
    void BotonesON()
    {
        Button[] listado = GameObject.Find("Panel").GetComponentsInChildren<Button>();
        foreach (Button x in listado)
        {
            x.interactable = true;
            GameObject.FindObjectOfType<BattleManager>().Texto.text = "¿Qué vas a hacer?";
        }
    }
    public void AccionesdelPJ(string accion)
    {
        foreach(AccionesPJ x in (AccionesPJ[])System.Enum.GetValues(typeof(AccionesPJ)))
        {
            if(x.ToString() == accion)
            {
                accionespj = x;
                StartCoroutine(Acciones());
            }
        }
    }
    #endregion
    #region Rutinas
    IEnumerator Acciones()
    {
        if(accionespj != AccionesPJ.Decision)
        {
            if (Turno1 == Turno.Enemigo && EN1.Hp > 0)
            {
                BotonesOFF();
                //Script básico de probabilidades.
                float prob = Random.Range(0, 100);
                if (prob <= 5 && EN1.Hp < (EN1.Maxhp / 3))
                {
                    acc = AccionesEnemigo.Escapar;
                }
                else if (prob > 5 && prob <= 90)
                {
                    acc = AccionesEnemigo.Atacar;
                }
                else
                {
                    if ((EN1.Hp / EN1.Maxhp) < 0.2)
                    {
                        acc = AccionesEnemigo.Curarse;
                    }
                    else
                    {
                        acc = AccionesEnemigo.Atacar;
                    }
                }
                //End
                Texto.text = "El enemigo se está preparando.";
                yield return new WaitForSeconds(3f);
                Texto.text = "¡El enemigo va a " + acc + "!";
                yield return new WaitForSeconds(3f);
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
                        int curacion = Mathf.RoundToInt(Random.Range(EN1.Maxhp*.05f, EN1.Maxhp * .1f));
                        EN1.Hp += curacion;
                        Texto.text = "El enemigo ha recuperado "+curacion+" de HP.";
                        yield return new WaitForSeconds(5f);
                        DecidirTurnos();
                        break;
                }
                finalizar += 1;
            }
            else if(Turno1 == Turno.Personaje)
            {
                switch (accionespj)
                {
                    case AccionesPJ.Atacar:
                        StartCoroutine(Ataque());
                        break;
                    case AccionesPJ.Habilidad:
                        HabUtilizada.Utilizar();
                        break;
                    case AccionesPJ.Objeto:
                        ObjUtilizado1.Utilizar();
                        break;
                    case AccionesPJ.Escapar:
                        print("uwu");
                        break;
                }
                finalizar += 1;
            }
        }
        else
        {
            Button[] listado = GameObject.Find("Panel").GetComponentsInChildren<Button>();
            BotonesOFF();
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
        GameObject.Find("BarraPJHP").transform.Find("Centro").transform.Find("HP").GetComponent<Text>().text = "HP " + pj.HP1.ToString();
        float alto = vidaPJ.rect.height;
        float ancho = 120 * (pj.HP1 / (float)pj.MaxHP1);
        float movimiento = (vidaPJ.rect.width - ancho) / 2f;
        if (ancho != 0)
        {
            vidaPJ.sizeDelta = new Vector2(ancho, alto);
            vidaPJ.anchoredPosition = new Vector2(vidaPJ.anchoredPosition.x - movimiento, vidaPJ.anchoredPosition.y);
        }
        //Barra de MP
        GameObject.Find("BarraPJMP").transform.Find("Centro").transform.Find("MP").GetComponent<Text>().text = "MP " + pj.MP1.ToString();
        float altoMP = manaPJ.rect.height;
        float anchoMP = 120 * (pj.MP1 / (float)pj.MaxMP1);
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
        GameObject.Find("BarraENM1").transform.Find("Centro").transform.Find("HP").GetComponent<Text>().text = "HP " + EN1.Hp.ToString();
        float alto = vidaEn1.rect.height;
        float ancho = 120 * (EN1.Hp / (float)EN1.Maxhp);
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
        Animator dmg = GameObject.Find("Daño").GetComponent<Animator>();
        int daño;
        float media;
        float critico = Random.Range(0, 100);
        switch (Turno1)
        {
            case Turno.Enemigo:
                anim = GameObject.Find("Enemigo").GetComponent<Animator>();
                media = EN1.Atk - (pj.DEF1 * 0.25f);
                if(media < 0)
                {
                    media = 1;
                }
                daño = Mathf.RoundToInt(Random.Range(media- (media*.1f), media + (media * .1f)));
                if(critico < 10)
                {
                    daño += daño / 2;
                    Texto.text = "¡Has recibido " + daño + " de daño CRÍTICO!";
                }
                else
                {
                    Texto.text = "¡Has recibido " + daño + " de daño!";
                }
                anim.SetTrigger("Ataque");
                GameObject.Find("Daño").GetComponent<Text>().text = "-" + daño;
                dmg.SetTrigger("Personaje");
                AudioClip oof = Resources.Load<AudioClip>("SFX/Oof");
                yield return new WaitForSeconds(0.5f);
                GameObject.Find("SFX").GetComponent<AudioSource>().PlayOneShot(oof);
                yield return new WaitForSeconds(2.5f);
                anim.ResetTrigger("Ataque");
                pj.HP1 -= daño;
                break;
            case Turno.Personaje:
                anim = GameObject.Find("Personaje").GetComponent<Animator>();
                media = pj.ATK1 - (EN1.Def * 0.25f);
                if (media < 0)
                {
                    media = 1;
                }
                daño = Mathf.RoundToInt(Random.Range(media - (media * .1f), media + (media * .1f)));
                if (critico < 10)
                {
                    daño += daño / 2;
                    Texto.text = "¡Has realizado " + daño + " de daño CRÍTICO!";
                }
                else
                {
                    Texto.text = "¡Has realizado " + daño + " de daño!";
                }
                anim.SetTrigger("Ataque");
                Animator en = GameObject.Find("Enemigo").GetComponent<Animator>();
                yield return new WaitForSeconds(0.5f);
                GameObject.Find("Daño").GetComponent<Text>().text = "-" + daño;
                dmg.SetTrigger("Enemigo");
                en.SetTrigger("Sufrimiento");
                yield return new WaitForSeconds(0.5f);
                GameObject.Find("Enemigo").GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0)[0].clip.length - 1f);
                en.ResetTrigger("Sufrimiento");
                anim.ResetTrigger("Ataque");
                EN1.Hp -= daño;
                break;
        }
        GameObject.Find("Daño").GetComponent<Text>().text = "";
        dmg.ResetTrigger("Enemigo");
        dmg.ResetTrigger("Personaje");
        DecidirTurnos();
    }
    public IEnumerator Animacion(int daño, AnimationClip animHab)
    {
        Animator hab = GameObject.Find("AtaqueAnim").GetComponent<Animator>();
        Animator dmg = GameObject.Find("Daño").GetComponent<Animator>();
        Animator anim = GameObject.Find("Personaje").GetComponent<Animator>();
        anim.SetTrigger("Ataque");
        Animator en = GameObject.Find("Enemigo").GetComponent<Animator>();
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("Daño").GetComponent<Text>().text = "-" + daño;
        dmg.SetTrigger("Enemigo");
        en.SetTrigger("Sufrimiento");
        print(animHab.name);
        hab.Play(animHab.name);
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("Enemigo").GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0)[0].clip.length-1f);
        en.ResetTrigger("Sufrimiento");
        anim.ResetTrigger("Ataque");
        if(Turno1 == Turno.Personaje)
        {
            EN1.Hp -= daño;
        }
        DecidirTurnos();
    } 
    public IEnumerator Esperar(Objeto item, string[] mensaje, int daño, AnimationClip habAnim)
    {
        esperas = true;
        BotonesOFF();
        foreach(string x in mensaje)
        {
            Texto.text = x;
            if(x.Contains("de daño"))
            {
                if (daño != 0)
                {
                    StartCoroutine(Animacion(daño, habAnim));
                }
            }
            yield return new WaitForSeconds(3f);
        }
        esperas = false;
        if (item != null)
        {
            print("Se resta el item");
            item.Cantidad -= 1;
            DecidirTurnos();
        }
    }
    IEnumerator Victoria()
    {
        if (EN1.Hp <= 0)
        {
            StopCoroutine(BarraEnemigos());
            EN1.Hp = 1;
            GameObject.Find("BarraENM1").transform.Find("Centro").transform.Find("HP").GetComponent<Text>().text = "HP 0";
            Destroy(GameObject.Find("Enemigo"));
            StopCoroutine(Acciones());
            BotonesOFF();
            yield return new WaitForSeconds(2f);
            AudioClip vic = Resources.Load<AudioClip>("Música/FF7 - Victory Fanfare");
            GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>().Stop();
            GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>().PlayOneShot(vic);
            Texto.text = "¡Has eliminado al enemigo!";
            yield return new WaitForSeconds(3f);
            Texto.text = "¡Ganaste!";
            yield return new WaitForSeconds(3f);
            Texto.text = "Has recibido " + EN1.Exp + " puntos de experiencia.";
            yield return new WaitForSeconds(5f);
            GameManager.Experiencia += Mathf.RoundToInt(EN1.Exp);
            GameManager.PJ = pj;
            SceneManager.LoadScene("Mapa");
        }
        else if (pj.HP1 <= 0)
        {
            StopCoroutine(Acciones());
            BotonesOFF();
            yield return new WaitForSeconds(3f);
            Texto.text = "Perdiste!";
            yield return new WaitForSeconds(3f);
        }
        yield return true;
    }
    #endregion
}