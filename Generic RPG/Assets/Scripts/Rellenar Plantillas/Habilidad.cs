using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Habilidad : MonoBehaviour {
    #region Propiedades
    private string nombre;
    private int baseDMG;
    private int lrnLVL;
    private PlanHabilidades habilidad;
    private PlanHabilidades.Tipo tipo;
    private int MPUse;
    private Sprite icono;
    private string Descripcion;
    private PlanHabilidades.Objetivo objetivoEfecto;
    private AudioClip sfx;
    private Button BotonHabilidad;
    #endregion
    #region GetSetProps
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

    public int BaseDMG
    {
        get
        {
            return baseDMG;
        }

        set
        {
            baseDMG = value;
        }
    }

    public int LrnLVL
    {
        get
        {
            return lrnLVL;
        }

        set
        {
            lrnLVL = value;
        }
    }

    public PlanHabilidades Hab
    {
        get
        {
            return habilidad;
        }

        set
        {
            habilidad = value;
        }
    }

    public int MPUse1
    {
        get
        {
            return MPUse;
        }

        set
        {
            MPUse = value;
        }
    }

    public PlanHabilidades.Tipo Tipo
    {
        get
        {
            return tipo;
        }

        set
        {
            tipo = value;
        }
    }

    public Sprite Icono
    {
        get
        {
            return icono;
        }

        set
        {
            icono = value;
        }
    }

    public string Descripcion1
    {
        get
        {
            return Descripcion;
        }

        set
        {
            Descripcion = value;
        }
    }

    public PlanHabilidades.Objetivo ObjetivoEfecto
    {
        get
        {
            return objetivoEfecto;
        }

        set
        {
            objetivoEfecto = value;
        }
    }
    #endregion
    #region Metodos
    public void Start()
    {
        BotonHabilidad = GetComponent<Button>();
        BotonHabilidad.onClick.AddListener(Utilizar);
        nombre = habilidad.nombre;
        BaseDMG = Hab.baseDMG;
        lrnLVL = Hab.lrnLVL;
        Tipo = Hab.tipo;
        MPUse = Hab.MPUse;
        icono = Hab.icono;
        Descripcion = Hab.Descripcion;
        ObjetivoEfecto = Hab.obj;
        sfx = Hab.sfx;
        Inicializar();
    }
    void Inicializar()
    {
        switch (tipo)
        {
            case PlanHabilidades.Tipo.Ataque:
                gameObject.transform.Find("Daño").GetComponent<Text>().text = BaseDMG.ToString()+"%";
                break;
            case PlanHabilidades.Tipo.Curacion:
                gameObject.transform.Find("Daño").GetComponent<Text>().text = (baseDMG*-1).ToString();
                break;
            case PlanHabilidades.Tipo.Bufo:
                gameObject.transform.Find("Daño").GetComponent<Text>().text = objetivoEfecto.ToString() + " - " + baseDMG;
                break;
            case PlanHabilidades.Tipo.Debufo:
                gameObject.transform.Find("Daño").GetComponent<Text>().text = objetivoEfecto.ToString() + " - " + baseDMG;
                break;
        }
        gameObject.transform.Find("Panel").transform.Find("Ícono").GetComponent<Image>().sprite = icono;
        gameObject.transform.Find("Descripcion").GetComponent<Text>().text = Descripcion;
        gameObject.transform.Find("Tipo").GetComponent<Text>().text = tipo.ToString();
        gameObject.transform.Find("Panel").transform.Find("Titulo").GetComponent<Text>().text = nombre;
        //gameObject.transform.Find("ManaIcon").transform.Find("Mana").GetComponent<Text>().text = MPUse.ToString();
    }
    public void Utilizar()
    {
        BattleManager bm = GameObject.Find("GameManager").GetComponent<BattleManager>();
        if (bm.accionespj == BattleManager.AccionesPJ.Decision)
        {
            if (bm.pj.MP1 < MPUse)
            {
                bm.Texto.text = "No tienes suficiente mana.";
                GameObject.Find("GameManager").GetComponent<SkillManager>().Cerrar();
            }
            else
            {
                bm.HabUtilizada1 = this;
                bm.AccionesdelPJ("Habilidad");
            }
        }else if(bm.accionespj == BattleManager.AccionesPJ.Habilidad)
        {
            Enemigo en = GameObject.Find("Enemigo").GetComponent<Enemigo>();
            Personaje pj = bm.pj;
            int daño = 0;
            if (pj.MP1 >= MPUse)
            {
                string[] mensaje = new string[2];
                bm.GetComponentInChildren<AudioSource>().PlayOneShot(sfx);
                mensaje[0] = pj.Nombre + " ha utilizado " + nombre + ".";
                //cositos de la animacion que podría tener.
                pj.MP1 -= MPUse;
                switch (tipo)
                {
                    case PlanHabilidades.Tipo.Ataque:
                        float critico = UnityEngine.Random.Range(0, 100);
                        float media = pj.ATK1 * (baseDMG / 100) - (en.Def * 0.25f);
                        if (media < 0)
                        {
                            media = 1;
                        }
                        daño = Mathf.RoundToInt(UnityEngine.Random.Range(media - (media * .1f), media + (media * .1f)));
                        if (critico < 10)
                        {
                            daño += daño / 2;
                            mensaje[1] = "Haces " + daño + " de daño CRÍTICO.";
                        }
                        else
                        {
                            mensaje[1] = "Haces " + daño + " de daño.";
                        }
                        break;
                    case PlanHabilidades.Tipo.Bufo:
                        foreach (PlanHabilidades.Objetivo x in (PlanHabilidades.Objetivo[])Enum.GetValues(typeof(PlanHabilidades.Objetivo)))
                        {
                            if (x == objetivoEfecto)
                            {
                                mensaje[1] = "Has aumentado en " + baseDMG + " tu " + x.ToString();
                                pj.Buff(objetivoEfecto.ToString(), baseDMG);
                            }
                        }
                        break;
                    case PlanHabilidades.Tipo.Curacion:
                        pj.HP1 += BaseDMG;
                        mensaje[1] = "Te has curado " + BaseDMG + " puntos de vida.";
                        break;
                    case PlanHabilidades.Tipo.Debufo:
                        foreach (PlanHabilidades.Objetivo x in (PlanHabilidades.Objetivo[])Enum.GetValues(typeof(PlanHabilidades.Objetivo)))
                        {
                            if (x == objetivoEfecto)
                            {
                                mensaje[1] = en.DeBuff(objetivoEfecto.ToString(), baseDMG);
                            }
                        }
                        break;
                }
                bm.StartCoroutine(bm.Esperar(null, mensaje, daño, habilidad.animacion));
            }
        }
    }
    #endregion
}
