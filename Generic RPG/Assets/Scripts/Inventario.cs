using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour {
    public List<PlanObjeto> objetos;
    public GameObject obj;
    private int espacios;
    private Component[] listado;
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
        float equis = -379f;
        float igriega = 177.3f;
        for (int i = 1; i - 1 < objetos.Count; i++)
        {
            int contador = 1;
            Debug.Log("Objeto: " + objetos[i - 1].nombre);
            Debug.Log("Objeto: " + objetos[i - 1].name);
            obj.GetComponent<Objeto>().item = objetos[i - 1];
            obj.GetComponent<Objeto>().Calculo(objetos);
            GameObject objeto;
            bool existe = false;
            listado = GameObject.Find("Canvas").GetComponentsInChildren<Objeto>();
            foreach (Objeto ob in listado)
            {
                if(ob.item == objetos[i - 1])
                {
                    print("Existe");
                    existe = true;
                }
                else
                {
                    print("No existe.");
                }
            }
            if (obj.GetComponent<Objeto>().Cantidad > 1 && objetos[i-1].name == obj.GetComponent<Objeto>().item.name && existe == true)
            {
                print("Se salta");
            }
            else
            {
                objeto = Instantiate(obj, new Vector2(equis, igriega), Quaternion.identity) as GameObject;
                objeto.name = "Obj_" + contador;
                objeto.transform.SetParent(GameObject.Find("Grilla").transform, false);
                objeto.GetComponent<Objeto>().Calculo(objetos);
                print(objeto.GetComponent<Objeto>().Cantidad);
                contador += 1;
            }
            equis += 120;
            if (i % 5 == 0)
            {
                igriega -= 120;
                equis = -379f;
            }
            print(igriega);
        }
    }
    public void Agregar(PlanObjeto obj)
    {
        int al = Random.Range(1, 4);
        objetos.Add(obj);
    }
}
