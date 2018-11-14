using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu]
public class PlanEnemigo : ScriptableObject {
    #region propiedades
    public string nombre;
    public int baseHP;
    public int baseATK;
    public int baseDEF;
    public int baseSPE;
    public float exp;
    public Sprite sprite;
    public AnimatorController anim;
    public float ProbabilidadAparecer;
    #endregion
}
