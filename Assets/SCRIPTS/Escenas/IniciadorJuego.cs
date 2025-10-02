using System;
using UnityEngine;

public class IniciadorJuego : MonoBehaviour
{
    private void Awake()
    {
        ProveedorEventos.Suscribir<IEventoEmpezarJuego>(EmpezarJuego);
    }

    private void EmpezarJuego(IEventoEmpezarJuego juego)
    {
        DespachadorEventos.Despachar<IEventoActivarEscena>(new EventoActivarMenu(new EstadoMenuPrincipal(), gameObject, false));

        Destroy(gameObject);
    }
}

internal class EventoEmpezarJuego : IEventoEmpezarJuego
{
    private GameObject gameObject;
    public GameObject ObjetoPadre => gameObject;

    public EventoEmpezarJuego(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }
}

internal interface IEventoEmpezarJuego : IEvento
{
}