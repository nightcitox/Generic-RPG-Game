using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private Animator anim;
    private Animator atak;
    private SpriteRenderer esprai;
    public Sprite hoja;
    private Text texto;
    private Vector3 pos;
    private Button boton;
    private RectTransform vidaPJ;
    private RectTransform vidaEn1;
    private RectTransform vidaEn2;
    private RectTransform vidaEn3;
    #endregion
    void Start() {
        turno = 1;
        texto = GameObject.Find("Daño").GetComponent<Text>();
        anim = GameObject.Find("PJ").GetComponent<Animator>();
        atak = GameObject.Find("Ataque").GetComponent<Animator>();
        vidaPJ = (RectTransform)GameObject.Find("BarraPJ").gameObject.transform.Find("Centro");
        vidaEn1 = (RectTransform)GameObject.Find("BarraENM1").gameObject.transform.Find("Centro");
        GameObject.Find("BarraENM1").transform.Find("Text").GetComponent<Text>().text = EN1.Nombre;
        GameObject.Find("BarraPJ").transform.Find("Text").GetComponent<Text>().text = pj.clase.nombre;
    }
	void Update () {
		if(EN1.Hp <= 0)
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
        boton = GameObject.Find("Atacar").GetComponent<Button>();
        boton.interactable = false;
        if (turno%2 == 1)
        {
            int daño = (pj.ATK1 - EN1.Def);
            texto.text = "Personaje: "+ daño;
            EN1.Hp = EN1.Hp - (pj.ATK1 - EN1.Def);
            StartCoroutine("AtaqueAnim");
            float alto = vidaEn1.rect.height;
            float ancho = 300*(EN1.Hp / (float)EN1.Maxhp);
            float movimiento = (vidaEn1.rect.width - ancho) / 2f;
            if (ancho != 0)
            {
                vidaEn1.sizeDelta = new Vector2(ancho, alto);
                vidaEn1.anchoredPosition = new Vector2(vidaEn1.anchoredPosition.x - movimiento, vidaEn1.anchoredPosition.y);
            }
            turno += 1;
        }
        else
        {
            int daño = (EN1.Atk - (pj.DEF1 / 10));
            pj.HP1 = pj.HP1 - daño;
            texto.text = "Enemigo: " + daño;
            float alto = vidaPJ.rect.height;
            float ancho = 300*(pj.HP1 / (float)pj.MaxHP1);
            float movimiento = (vidaPJ.rect.width - ancho)/2f;
            if(ancho != 0)
            {
                vidaPJ.sizeDelta = new Vector2(ancho, alto);
                vidaPJ.anchoredPosition = new Vector2(vidaPJ.anchoredPosition.x - movimiento, vidaPJ.anchoredPosition.y);
            }
            boton.interactable = true;
            boton.Select();
            turno += 1;
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
        print("comienza");
        yield return new WaitForSeconds(1.5f);
        print("termina");
        boton.interactable = true;
        boton.Select();
        anim.ResetTrigger("Ataque");
        atak.SetBool("Ataque", false);
        esprai.sprite = null;
        atak.transform.position = new Vector3(-5.5f, 0.5f, 0f);
    }
    #endregion
}
