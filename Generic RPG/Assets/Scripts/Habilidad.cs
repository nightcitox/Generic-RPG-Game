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
        print(BotonHabilidad.name);
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
                gameObject.transform.Find("Daño").GetComponent<Text>().text = "Daño: " + baseDMG;
                break;
            case PlanHabilidades.Tipo.Curacion:
                gameObject.transform.Find("Daño").GetComponent<Text>().text = "Curación: " + baseDMG * -1;
                break;
            case PlanHabilidades.Tipo.Bufo:
                gameObject.transform.Find("Daño").GetComponent<Text>().text = objetivoEfecto.ToString() + " - " + baseDMG;
                break;
            case PlanHabilidades.Tipo.Debufo:
                gameObject.transform.Find("Daño").GetComponent<Text>().text = objetivoEfecto.ToString() + " - " + baseDMG;
                break;
        }
        gameObject.transform.Find("Ícono").GetComponent<Image>().sprite = icono;
        gameObject.transform.Find("Descripcion").GetComponent<Text>().text = Descripcion;
        gameObject.transform.Find("Tipo").GetComponent<Text>().text = tipo.ToString();
        gameObject.transform.Find("Titulo").GetComponent<Text>().text = nombre;
    }
    void Utilizar()
    {
        if(GameObject.Find("Personaje").GetComponent<Personaje>().MP1 > MPUse)
        {
            GameObject.Find("GameManager").GetComponent<AudioSource>().Stop();
            GameObject.Find("GameManager").GetComponentInChildren<AudioSource>().PlayOneShot(sfx);
            GameObject.Find("GameManager").GetComponent<AudioSource>().Play();
            //cositos de la animacion que podría tener.
            GameObject.Find("Personaje").GetComponent<Personaje>().MP1 -= MPUse;
            switch (tipo)
            {
                case PlanHabilidades.Tipo.Ataque:
                    GameObject.Find("Enemigo 1").GetComponent<Enemigo>().Hp -= baseDMG + GameObject.Find("Personaje").GetComponent<Personaje>().ATK1;
                    GameObject.Find("GameManager").GetComponent<BattleManager>().Texto.text = "Haces " + (baseDMG + GameObject.Find("Personaje").GetComponent<Personaje>().ATK1)+ " de daño.";
                    
                    break;
                case PlanHabilidades.Tipo.Bufo:
                    foreach (PlanHabilidades.Objetivo x in (PlanHabilidades.Objetivo[])Enum.GetValues(typeof(PlanHabilidades.Objetivo)))
                    {
                        if (x == objetivoEfecto)
                        {
                            GameObject.Find("GameManager").GetComponent<BattleManager>().Texto.text = "Has aumentado en " + baseDMG + " tu " + x.ToString();
                            GameObject.Find("Personaje").GetComponent<Personaje>().Buff(objetivoEfecto.ToString(), baseDMG);
                        }
                    }
                    break;
                case PlanHabilidades.Tipo.Curacion:
                    GameObject.Find("Personaje").GetComponent<Personaje>().HP1 -= BaseDMG;
                    break;
                case PlanHabilidades.Tipo.Debufo:
                    foreach (PlanHabilidades.Objetivo x in (PlanHabilidades.Objetivo[])Enum.GetValues(typeof(PlanHabilidades.Objetivo)))
                    {
                        if (x == objetivoEfecto)
                        {
                            GameObject.Find("Enemigo 1").GetComponent<Enemigo>().DeBuff(objetivoEfecto.ToString(), baseDMG);
                        }
                    }
                    break;
            }
            StartCoroutine(GameObject.Find("GameManager").GetComponent<BattleManager>().Esperar(null));
            GameObject.Find("GameManager").GetComponent<BattleManager>().Turno += 1;
        }
        else
        {
            GameObject.Find("GameManager").GetComponent<BattleManager>().Texto.text = "No tienes suficiente mana.";
            GameObject.Find("GameManager").GetComponent<SkillManager>().Cerrar();
        }
    }
    #endregion
}
