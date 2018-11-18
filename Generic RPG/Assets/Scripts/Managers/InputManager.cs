using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager{
    static Dictionary<string, KeyCode> DicTeclas;
    static KeyCode[] Tecla = new KeyCode[7]
    {
        KeyCode.Z,
        KeyCode.X,
        KeyCode.Tab,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow
    };
    static string[] Accion = new string[7]
    {
        "Aceptar",
        "Cancelar",
        "Menus",
        "Arriba",
        "Abajo",
        "Izquierda",
        "Derecha"
    };
    public static bool GUIActivo;
    static InputManager()
    {
        Inicializar();
    }
    static void Inicializar()
    {
        DicTeclas = new Dictionary<string, KeyCode>();
        for(int i = 0; i<Tecla.Length; i++)
        {
            DicTeclas.Add(Accion[i], Tecla[i]);
        }
    }
    public static void CambiarTeclas(string tecla, KeyCode key)
    {
        if (DicTeclas.ContainsKey(tecla))
        {
            Debug.Log("Ya existe esa tecla.");
        }
        else
        {
            DicTeclas[tecla] = key;
        }
    }
    public static bool KeyDown(string tecla)
    {
        return Input.GetKeyDown(DicTeclas[tecla]);
    }
    public static bool KeyUp(string tecla)
    {
        return Input.GetKeyUp(DicTeclas[tecla]);
    }
    public static bool Key(string tecla)
    {
        return Input.GetKey(DicTeclas[tecla]);
    }
}
