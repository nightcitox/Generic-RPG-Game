using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mapa : MonoBehaviour {
    #region Propiedades
    public List<PlanEnemigo> enemigos;
    private string nombre;
    static public PlanEnemigo en;
    public int lv;
    public static int NivelZona;
    #endregion
    void Start () {
        NivelZona = lv;
    }
    public void CalcularProbabilidad()
    {
        float ProbEncuentro = Random.Range(0, 101);
        if(ProbEncuentro > 100)
        {
            Debug.Log("No hay encuentro.");
            return;
        }
        else
        {
            float SumaEncuentros = 0;
            for(int i = 0; i < enemigos.Count; i++)
            {
                SumaEncuentros += enemigos[i].ProbabilidadAparecer;
            }
            float Encuentro = Random.Range(0, SumaEncuentros);
            for(int i = 0; i < enemigos.Count; i++)
            {
                if(Encuentro <= enemigos[i].ProbabilidadAparecer)
                {
                    Debug.Log(enemigos[i].nombre);
                    en = enemigos[i];
                    SceneManager.LoadScene("Batalla");
                    return;
                }
                Encuentro -= enemigos[i].ProbabilidadAparecer;
            }
        }
    }
}
