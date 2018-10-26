using UnityEngine;

[CreateAssetMenu]
public class PlanHabilidades : ScriptableObject {
    public string nombre;
    public int baseDMG;
    public int lrnLVL;
    public PlanClase[] clases;
}
