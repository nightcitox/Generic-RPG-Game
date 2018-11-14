using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {
    // Ésta es la clase contenedora de la planilla creada para el NPC, es decir, acá se 
    // ingresa el objeto creado anteriormente.
    #region Propiedades
    private string nombre;
    private PlanDialogo[] dialogos;
    public PlanNPCs npc;
    private PlanMision mision;
    private GameManager game;
    #endregion
    void Start ()
    {
        nombre = npc.nombre;
        dialogos = npc.dialogos;
        game = GetComponent<GameManager>();
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Aquí coloco que cuando entra en colisión y presiona el botón denominado como Submit
        //haga todo el proceso de los diálogos pero OJO siempre y cuando esté en colisión y permaneza así.
        if (Input.GetButton("Submit"))
        {
            //Primero busco el objeto de Personaje.
            Personaje movimiento = GetComponent<Personaje>();
            //Ahora llamo al personaje y cambio la propiedad de "PuedeMoverse" a false pa que no se mueva durante el diálogo.
            movimiento.puedeMoverse = false;
            //Si hay más de un diálogo presente en el sistema del NPC se filtra para que funcionen todos.
            if (dialogos.Length > 1)
            {
                int i;
                //Loop para que ingrese cada diálogo presente.
                for (i = 0; i > dialogos.Length - 1; i++)
                {
                    //Si presiona de nuevo el botón de aceptar, el diálogo acaba y pasaría al siguiente.
                    if (Input.GetButton("Submit"))
                    {
                        game.dialogo = dialogos[i].dialogo;
                    }
                }
            }
            else
            {
                //Ingresa el único diálogo.
                game.dialogo = dialogos[0].dialogo;
            }
        }
    }
    public void CrearMision()
    {
        //En este, se crea la misión y se redirige al GameManager para que se coloque como misión actual y se pueda
        //cumplir el objetivo de la misma.
        GetComponent<GameManager>().mision = mision;
    }
    public void AsignarMision(PlanMision asigMision)
    {
        //Método creado principalmente para asignar una misión al NPC actual, ya que, las misiones pueden ser más
        //de una, además de que pueden ser parte de una secuencia.
        mision = asigMision;
    }
}