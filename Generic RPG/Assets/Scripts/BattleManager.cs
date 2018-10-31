using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {
    #region propiedades
    public Personaje pj;
    public PlanEnemigo enemigo1;
    private PlanEnemigo enemigo2;
    private PlanEnemigo enemigo3;
    private Mapa zona;
    private int turno;
    #endregion
    #region anim
    private Animator anim;
    private Animator atak;
    private SpriteRenderer esprai;
    public Sprite hoja;
    private Text texto;
    private Vector3 pos;
    #endregion
    // Use this for initialization
    void Start() {
        turno = 1;
        texto = GameObject.Find("Daño").GetComponent<Text>();
        anim = GameObject.Find("PJ").GetComponent<Animator>();
        atak = GameObject.Find("Ataque").GetComponent<Animator>();
	}
	// Update is called once per frame
	void Update () {
		if(enemigo1.baseHP <= 0)
        {
            texto.text = "Ganaste!";
        }
        else if (pj.HP1 <= 0)
        {
            texto.text = "Perdiste!";
        }
	}
    public void Rutinas(string rutina)
    {
        StartCoroutine(rutina);
    }
    #region rutinas
    IEnumerator Daño()
    {
        if(turno%2 == 1)
        {
            int daño = (pj.ATK1 - enemigo1.baseDEF);
            texto.text = "Personaje: "+daño;
            enemigo1.baseHP = enemigo1.baseHP - (pj.ATK1 - enemigo1.baseDEF);
            StartCoroutine("AtaqueAnim");
            turno += 1;
        }
        else
        {
            int daño = (enemigo1.baseATK - (pj.DEF1 / 10));
            pj.HP1 = pj.HP1 - daño;
            turno += 1;
            texto.text = "Enemigo: " + daño;
        }
        yield return true;
    }

    IEnumerator AtaqueAnim()
    {
        anim.SetTrigger("Ataque");
        esprai = atak.GetComponent<SpriteRenderer>();
        esprai.sprite = hoja;
        atak.SetBool("Ataque", true);
        pos = atak.transform.position;
        atak.transform.Translate(new Vector3(5, 0, 0));
        yield return new WaitForSeconds(1.5f);
        anim.ResetTrigger("Ataque");
        atak.SetBool("Ataque", false);
        esprai.sprite = null;
        atak.transform.position = new Vector3(-5.5f, 0.5f, 0f);
    }
    #endregion
}
