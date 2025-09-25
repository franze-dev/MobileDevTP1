using System;
using UnityEngine;
using UnityEngine.UIElements;

public interface IEventoActivarEscena : IEvento
{
    int Indice { get; }
}

public class EventoActivarJuego : IEventoActivarEscena
{
    private GameObject Objeto;
    public int Indice => DatosEscenaJuego.Index;
    public GameObject ObjetoPadre { get => Objeto; }

    public EventoActivarJuego(GameObject ObjetoActivador)
    {
        Objeto = ObjetoActivador;
    }
}

public class EventoActivarFinal : IEventoActivarEscena
{
    private GameObject Objeto;
    public int Indice => ProveedorServicios.IntentarObtenerServicio<ControladorFlujoEscenas>(out var controlador) ? 
                         controlador.Contenedor.EscenaFinalIndice : -1;
    public GameObject ObjetoPadre { get => Objeto; }

    public EventoActivarFinal(GameObject ObjetoActivador)
    {
        Objeto = ObjetoActivador;
    }
}

