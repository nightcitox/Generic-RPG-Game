﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillManager : MonoBehaviour{
    private List<PlanHabilidades> AllSkills = new List<PlanHabilidades>();
    private Personaje PJ;
    private List<PlanHabilidades> habilidadesPJ = new List<PlanHabilidades>();
    public GameObject HabSlot;

    void Start()
    {
        PJ = GameObject.Find("Personaje").GetComponent<Personaje>();
        ListadoHabilidades();
    }
    public void ListadoHabilidades()
    {
        string[] nombres = AssetDatabase.FindAssets("Hab");
        for (int i = 0; i < nombres.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(nombres[i]);
            PlanHabilidades asset = AssetDatabase.LoadAssetAtPath<PlanHabilidades>(assetPath);
            if (asset != null)
            {
                AllSkills.Add(asset);
            }
        }
        foreach (PlanHabilidades x in AllSkills)
        {
            foreach(PlanClase y in x.clases)
            {
                if (y == PJ.clase && PJ.Nivel >= x.lrnLVL)
                {
                    habilidadesPJ.Add(x);
                }
            }
        }
    }
    public void Desplegar()
    {
        if(GameObject.Find("Panel").transform.Find("PanelHabilidades").gameObject.activeSelf == false)
        {
            GameObject.Find("Panel").transform.Find("PanelHabilidades").gameObject.SetActive(true);
            GameObject habil;
            for (int i = 0; i < habilidadesPJ.Count; i++)
            {
                habil = Instantiate(HabSlot, new Vector2(0, 0), Quaternion.identity) as GameObject;
                habil.name = "Hab_" + (i + 1);
                Transform papi = GameObject.Find("Panel").transform.Find("PanelHabilidades").transform.Find("Grilla");
                habil.transform.SetParent(papi, false);
                habil.GetComponent<Habilidad>().Hab = habilidadesPJ[i];
            }
        }
    }
    public void Cerrar()
    {
        if(GameObject.Find("Panel").transform.Find("PanelHabilidades").gameObject.activeSelf == true)
        {
            GameObject habil;
            for (int i = 0; i < habilidadesPJ.Count; i++)
            {
                habil = GameObject.Find("Hab_" + (i + 1));
                Destroy(habil);
            }
            GameObject.Find("PanelHabilidades").SetActive(false);
        }
    }
}