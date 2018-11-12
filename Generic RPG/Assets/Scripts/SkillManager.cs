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
        print(PJ.clase.nombre);
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
        if(GameObject.Find("PanelHabilidades").activeSelf == false)
        {
            GameObject.Find("Panel").transform.Find("PanelHabilidades").gameObject.SetActive(true);
            float equis = -2.1f;
            float igriega = 224.5f;
            GameObject habil;
            for (int i = 0; i < habilidadesPJ.Count; i++)
            {
                habil = Instantiate(HabSlot, new Vector2(equis, igriega), Quaternion.identity) as GameObject;
                habil.name = "Hab_" + (i + 1);
                habil.transform.SetParent(GameObject.Find("Grilla").transform, false);
                habil.GetComponent<Habilidad>().Hab = habilidadesPJ[i];
                habil.GetComponent<Habilidad>().Start();
                igriega -= 75;
            }
        }
    }
    public void Cerrar()
    {
        if(GameObject.Find("PanelHabilidades").activeSelf == true)
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