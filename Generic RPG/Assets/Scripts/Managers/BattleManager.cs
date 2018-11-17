﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {
    #region propiedades
    public Personaje pj;
    public Enemigo EN1;
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
        pj = GameManager.PJ;
        Texto = GameObject.Find("Estado").GetComponent<Text>();
        vidaPJ = (RectTransform)GameObject.Find("BarraPJHP").gameObject.transform.Find("Centro");
        manaPJ = (RectTransform)GameObject.Find("BarraPJMP").gameObject.transform.Find("Centro");
        vidaEn1 = (RectTransform)GameObject.Find("BarraENM1").gameObject.transform.Find("Centro");
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
        StartCoroutine(Acciones());
    }
    #endregion
    #region rutinas
    IEnumerator Acciones()
    {
        if(EN1.Hp > 0)
        {
            if (Turno1 == Turno.Enemigo)
            {
                BotonesOFF();
                //Script básico de probabilidades.
                float prob = Random.Range(0, 100);
                if (prob <= 10 && EN1.Hp < (EN1.Maxhp / 3))
                {
                    acc = AccionesEnemigo.Escapar;
                }
                else if (prob > 10 && prob <= 80)
                {
                    acc = AccionesEnemigo.Atacar;
                }
                else
                {
                    if ((EN1.Hp / EN1.Maxhp) < 0.2)
                    {
                        acc = AccionesEnemigo.Curarse;
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
                        EN1.Hp += Mathf.RoundToInt(Random.Range(EN1.Maxhp*.05f, EN1.Maxhp * .1f));
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
                pj.HP1 = pj.HP1 - daño;
                anim.SetTrigger("Ataque");
                GameObject.Find("Daño").GetComponent<Text>().text = "-" + daño;
                dmg.SetTrigger("Personaje");
                yield return new WaitForSeconds(3f);
                anim.ResetTrigger("Ataque");
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
                EN1.Hp = EN1.Hp - daño;
                anim.SetTrigger("Ataque");
                Animator en = GameObject.Find("Enemigo").GetComponent<Animator>();
                GameObject.Find("Daño").GetComponent<Text>().text = "-"+daño;
                dmg.SetTrigger("Enemigo");
                en.SetTrigger("Sufrimiento");
                GameObject.Find("Enemigo").GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(3f);
                en.ResetTrigger("Sufrimiento");
                anim.ResetTrigger("Ataque");
                break;
        }
        GameObject.Find("Daño").GetComponent<Text>().text = "";
        dmg.ResetTrigger("Enemigo");
        dmg.ResetTrigger("Personaje");
        DecidirTurnos();
    }
    public IEnumerator Animacion(int daño)
    {
        Animator dmg = GameObject.Find("Daño").GetComponent<Animator>();
        Animator anim = GameObject.Find("Personaje").GetComponent<Animator>();
        anim.SetTrigger("Ataque");
        Animator en = GameObject.Find("Enemigo").GetComponent<Animator>();
        GameObject.Find("Daño").GetComponent<Text>().text = "-" + daño;
        dmg.SetTrigger("Enemigo");
        en.SetTrigger("Sufrimiento");
        GameObject.Find("Enemigo").GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(3f);
        en.ResetTrigger("Sufrimiento");
        anim.ResetTrigger("Ataque");
        DecidirTurnos();
        GameObject.Find("Daño").GetComponent<Text>().text = "";
    } 
    public IEnumerator Esperar(Objeto item, string[] mensaje, int daño)
    {
        esperas = true;
        BotonesOFF();
        foreach(string x in mensaje)
        {
            Texto.text = x;
            yield return new WaitForSeconds(3f);
        }
        esperas = false;
        if (item != null)
        {
            item.Cantidad -= 1;
            DecidirTurnos();
        }
        else if(daño != 0)
        {
            StartCoroutine(Animacion(daño));
        }
        else
        {
            DecidirTurnos();
        }
    }
    IEnumerator Victoria()
    {
        if (EN1.Hp <= 0)
        {
            StopCoroutine(Acciones());
            BotonesOFF();
            yield return new WaitForSeconds(3f);
            Texto.text = "Ganaste!";
            yield return new WaitForSeconds(3f);
            Texto.text = "Has recibido " + EN1.Exp + " puntos de experiencia.";
            yield return new WaitForSeconds(5f);
            GameManager.Experiencia += Mathf.RoundToInt(EN1.Exp);
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
