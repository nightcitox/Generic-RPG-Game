using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogManager : MonoBehaviour {
    public GameObject[] Inputs;
    public Button Submit;

    public void Rutinas(string rutina)
    {
        StartCoroutine(rutina);
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
        switch (link.text)
        {
            case "0":
                print("Logueado exitosamente.");
                SceneManager.LoadScene("MainMenu");
                break;
            case "2":
                print("El usuario es incorrecto.");
                break;
            case "3":
                print("La contraseña es incorrecta.");
                break;
        }
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
        if(link.text == "0")
        {
            print("Se ha creado exitosamente el usuario.");
            SceneManager.LoadScene("Login");
        }
        else
        {
            print("Hubo un error. Error #"+link.text);
        }
    }
    public void Verificar()
    {
        int verificado = 0;
        foreach(GameObject inp in Inputs)
        {
            if(inp.name == "NicknameInput" && inp.transform.Find("Text").GetComponent<Text>().text.Length >= 8)
            {
                verificado += 1;
            }else if (inp.name == "PasswordInput" && inp.transform.Find("Text").GetComponent<Text>().text.Length >= 8)
            {
                verificado += 1;
            }
        }
        if(verificado == 2)
        {
            Submit.interactable = true;
        }
        else
        {
            Submit.interactable = false;
        }
    }
}
