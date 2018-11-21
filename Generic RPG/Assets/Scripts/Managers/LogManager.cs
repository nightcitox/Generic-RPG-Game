using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogManager : MonoBehaviour {
    public GameObject[] Inputs;
    public Button Submit;
    private Text mensaje;
    void Start()
    {
        mensaje = GameObject.Find("StatusMSG").GetComponent<Text>();
    }
    public void Rutinas(string rutina)
    {
        StartCoroutine(rutina);
    }
    IEnumerator CerrarSesion()
    {
        GameManager.sesion = false;
        GameManager.UsuarioConectado = "";
        SceneManager.LoadScene("Login");
        yield return true;
    }
    IEnumerator Logearse()
    {
        WWWForm formulario = new WWWForm();
        InputField obj = Inputs[0].GetComponent<InputField>();
        formulario.AddField("usuario", obj.text);
        obj = Inputs[1].GetComponent<InputField>();
        formulario.AddField("contraseña", obj.text);
        WWW link = new WWW("http://localhost/ConexionesSQL/login.php", formulario);
        yield return link;
        string msg = "";
        switch (link.text)
        {
            case "0":
                print("Logueado exitosamente.");
                obj = Inputs[0].GetComponent<InputField>();
                GameManager.UsuarioConectado = obj.text;
                GameManager.sesion = true;
                verifica = false;
                SceneManager.LoadScene("MainMenu");
                break;
            case "2":
                msg = "El usuario es incorrecto.";
                break;
            case "3":
                msg = "La contraseña es incorrecta.";
                break;
            default:
                msg = "No se pudo conectar con el servidor.";
                break;
        }
        Submit.interactable = false;
        GameObject.Find("LoginMSG").GetComponent<Text>().text = msg;
    }
    IEnumerator Registrar()
    {
        WWWForm formulario = new WWWForm();
        InputField obj = Inputs[0].GetComponent<InputField>();
        formulario.AddField("usuario", obj.text);

        obj = Inputs[1].GetComponent<InputField>();
        formulario.AddField("contraseña", obj.text);

        obj = Inputs[2].GetComponent<InputField>();
        formulario.AddField("nombres", obj.text);

        obj = Inputs[3].GetComponent<InputField>();
        formulario.AddField("apellidos", obj.text);

        obj = Inputs[4].GetComponent<InputField>();
        formulario.AddField("año", obj.text);

        obj = Inputs[5].GetComponent<InputField>();
        formulario.AddField("mes", obj.text);

        obj = Inputs[6].GetComponent<InputField>();
        formulario.AddField("dia", obj.text);

        obj = Inputs[7].GetComponent<InputField>();
        formulario.AddField("correo", obj.text);

        WWW link = new WWW("http://localhost/ConexionesSQL/registro.php", formulario);
        yield return link;
        string msg = "";
        switch (link.text)
        {
            case "0":
                msg = "Se ha creado exitosamente el usuario.";
                yield return new WaitForSeconds(2f);
                SceneManager.LoadScene("Login");
                verifica = false;
                break;
            case "3":
                msg = "El usuario ya existe.";
                break;
            case "4":
                msg = "No se pudo crear al usuario.";
                break;
            default:
                msg = "No se pudo conectar con el servidor.";
                break;
        }
        mensaje.text = msg;
        Submit.interactable = false;
    }
    private bool verifica;
    public void Verificar()
    {
        foreach (GameObject inp in Inputs)
        {
            if(inp.name == "NicknameInput" && inp.transform.Find("Text").GetComponent<Text>().text.Length >= 8)
            {
                verifica = true;
            }else if (inp.name == "PasswordInput" && inp.transform.Find("Text").GetComponent<Text>().text.Length >= 8)
            {
                verifica = true;
            }
            else if(inp.name == "NicknameInput" || inp.name == "PasswordInput")
            {
                verifica = false;
            }
        }
    }
    bool verificadisimo;
    public void VerificarDiaMesAño()
    {
        foreach (GameObject inp in Inputs)
        {
            print(inp.name + inp.transform.Find("Text").GetComponent<Text>().text.Length);
            if (inp.name == "DiaInput" && inp.transform.Find("Text").GetComponent<Text>().text.Length > 0)
            {
                int Dia = int.Parse(inp.transform.Find("Text").GetComponent<Text>().text);
                print(Dia);
                if (Dia >= 1 && Dia <= 31)
                {
                    verificadisimo = true;
                }
                else
                {
                    verificadisimo = false;
                    mensaje.text = "Ingrese un rango de días válido (1-31)";
                }
            }
            else if (inp.name == "MesInput" && inp.transform.Find("Text").GetComponent<Text>().text.Length > 0)
            {
                int Mes = int.Parse(inp.transform.Find("Text").GetComponent<Text>().text);
                print(Mes);
                if (Mes >= 1 && Mes <= 12)
                {
                    verificadisimo = true;
                }
                else
                {
                    verificadisimo = false;
                    mensaje.text = "Ingrese un mes válido (1-12)";
                }
            }
            else if (inp.name == "AñoInput" && inp.transform.Find("Text").GetComponent<Text>().text.Length > 0)
            {
                int Año = int.Parse(inp.transform.Find("Text").GetComponent<Text>().text);
                print(Año);
                if (Año >= 1950 && Año <= 2003)
                {
                    verificadisimo = true;
                }
                else
                {
                    verificadisimo = false;
                    mensaje.text = "Ingrese un año válido (1900-2003)";
                }
            }
            else if (inp.name == "DiaInput" || inp.name == "MesInput" || inp.name == "AñoInput")
            {
                verificadisimo = false;
            }
        }
    }
    void Update()
    {
        print("Verifica: " + verifica + " Verificadisimo: " + verificadisimo);
        if (verifica && verificadisimo)
        {
            mensaje.text = "";
            Submit.interactable = true;
        }
        else
        {
            Submit.interactable = false;
        }
    }
}
