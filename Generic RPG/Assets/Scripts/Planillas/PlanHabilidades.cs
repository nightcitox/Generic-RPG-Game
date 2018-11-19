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
    public Objetivo obj;
    public AudioClip sfx;
    public Animation animacion;
    public enum Tipo
    {
        Bufo,
        Debufo,
        Ataque,
        Curacion
    }
    public enum Objetivo
    {
        HP,
        MP,
        ATK,
        DEF,
        SPE
    }
    public int MPUse;
}
