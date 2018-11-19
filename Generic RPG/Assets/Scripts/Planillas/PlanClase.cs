using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
[CreateAssetMenu]
public class PlanClase : ScriptableObject {
    #region propiedades
    public string nombre;
    public int baseHP;
    public int baseMP;
    public int baseATK;
    public int baseDEF;
    public int baseSPE;
    public AnimatorController battlerController;
    public Sprite battler;
    public AnimatorController mapController;
    public Sprite mapSprite;
    #endregion
}
