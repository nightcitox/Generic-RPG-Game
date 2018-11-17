using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlanDialogo : ScriptableObject {
    public int id;
    public string[] dialogos;
    public TipoDialogo tipo;
    public enum TipoDialogo
    {
        Dialogo,
        ConOpciones,
        Cinematica
    }
}
