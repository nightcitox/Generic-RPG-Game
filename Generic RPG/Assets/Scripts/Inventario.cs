using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour {
    public List<PlanObjeto> objetos;
    public GameObject obj;
    private int espacios;
	// Use this for initialization
	void Start () {
	}
    void Utilizar()
    {

    }
    void Equipar()
    {

    }
    void Botar()
    {

    }
    public void Abrir()
    {
        float equis = -400.5f;
        float igriega = 192.5f;
        for (int i=1; i-1 < objetos.Count; i++)
        {
            Debug.Log("Objeto: " + objetos[i-1].nombre);
            Debug.Log("Objeto: " + objetos[i-1].name);
            obj.GetComponent<Objeto>().item = objetos[i-1];
            GameObject objeto = Instantiate(obj, new Vector2(equis, igriega), Quaternion.identity) as GameObject;
            equis += 100;
            if (i % 5 == 0)
            {
                igriega -= 100;
                equis = -400.5f;
            }
            objeto.name = "Obj_" + i;
            print(igriega);
            objeto.transform.SetParent(GameObject.Find("Grilla").transform, false);
        }
    }
    public void Agregar(PlanObjeto obj)
    {
        int al = Random.Range(1, 4);
        objetos.Add(obj);
    }
}
