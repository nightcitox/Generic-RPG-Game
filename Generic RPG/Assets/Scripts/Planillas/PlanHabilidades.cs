using UnityEngine;

[CreateAssetMenu]
public class PlanHabilidades : ScriptableObject {
    public string nombre;
    public int baseDMG;
    public int lrnLVL;
    public PlanClase[] clases;
    public Tipo tipo;
    public Sprite icono;
    public string Descripcion;
    public enum Tipo
    {
        Bufo,
        Debufo,
        Ataque,
        Curacion
    }
    public int MPUse;
}
