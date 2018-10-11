using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atacar : MonoBehaviour {
    public Animator anim;
    public Animator atak;
    private SpriteRenderer esprai;
    public Sprite hoja;
    private Vector3 pos;
    public void Atacarr()
    {
        StartCoroutine("Ataque");
    }
    IEnumerator Ataque()
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
}
