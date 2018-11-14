using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventario : MonoBehaviour {
    public List<PlanObjeto> objetos;
    public GameObject obj;
    private int espacios;
    private Component[] listado;
    private int contador = 1;
    private bool abierto;

    public bool Abierto
    {
        get
        {
            return abierto;
        }

        set
        {
            abierto = value;
        }
    }

    // Use this for initialization
    void Start () {
        Abierto = false;
	}
    public void Cerrar()
    {
        if(Abierto == true)
        {
            listado = GameObject.Find("Contenido").GetComponentsInChildren<Objeto>();
            foreach (Objeto ob in listado)
            {
                Destroy(ob.gameObject);
            }
            contador = 1;
            Abierto = false;
            GameObject.Find("Inventario").gameObject.SetActive(false);
        }
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
        if(Abierto == false)
        {
            print("abriendo");
            GameObject.Find("Panel").transform.Find("Inventario").gameObject.SetActive(true);
            contador = 1;
            for (int i = 0; i < objetos.Count - 1; i++)
            {
                obj.GetComponent<Objeto>().item = objetos[i];
                obj.GetComponent<Objeto>().Calculo(objetos);
                GameObject objeto;
                bool existe = false;
                listado = GameObject.Find("Contenido").GetComponentsInChildren<Objeto>();
                foreach (Objeto ob in listado)
                {
                    print(ob.item.nombre);
                    if (ob.item == objetos[i])
                    {
                        print("Existe");
                        existe = true;
                    }
                    else
                    {
                        print("No existe.");
                    }
                }
                if (existe == true)
                {
                    if(obj.GetComponent<Objeto>().item.tipo == PlanObjeto.Tipo.Equipo)
                    {
                        print("Equipo");
                        objeto = Instantiate(obj, new Vector2(0, 0), Quaternion.identity) as GameObject;
                        objeto.name = "Obj_" + contador;
                        objeto.transform.SetParent(GameObject.Find("Contenido").transform, false);
                        objeto.GetComponent<Objeto>().Calculo(objetos);
                        contador += 1;
                    }
                }
                else
                {
                    Scene actual = SceneManager.GetActiveScene();
                    if (obj.GetComponent<Objeto>().item.tipo == PlanObjeto.Tipo.Consumible && actual.name == "Batalla")
                    {
                        print("Está en batalla");
                        objeto = Instantiate(obj, new Vector2(0, 0), Quaternion.identity) as GameObject;
                        objeto.name = "Obj_" + contador;
                        objeto.transform.SetParent(GameObject.Find("Contenido").transform, false);
                        objeto.GetComponent<Objeto>().Calculo(objetos);
                        contador += 1;
                    }
                    else if (actual.name != "Batalla")
                    {
                        print("No está en batalla");
                        objeto = Instantiate(obj, new Vector2(0, 0), Quaternion.identity) as GameObject;
                        objeto.name = "Obj_" + contador;
                        objeto.transform.SetParent(GameObject.Find("Contenido").transform, false);
                        objeto.GetComponent<Objeto>().Calculo(objetos);
                        contador += 1;
                    }
                }
            }
            Abierto = true;
        }
    }
    public void Agregar(PlanObjeto obj)
    {
        int al = Random.Range(1, 4);
        objetos.Add(obj);
    }
}
