using System;
using UnityEngine;
using UnityEngine.UIElements;

public interface IEventoActivarEscena : IEvento
{
    int Indice { get; }
    bool UnloadPrevious { get; set; }
}

public class EventoActivarJuego : IEventoActivarEscena
{
    private GameObject Objeto;
    public int Indice => DatosEscenaJuego.Index;
    public GameObject ObjetoPadre { get => Objeto; }
    public bool UnloadPrevious { get; set; }

    public EventoActivarJuego(GameObject ObjetoActivador, bool unloadPrevious = true)
    {
        Objeto = ObjetoActivador;
        UnloadPrevious = unloadPrevious;
    }
}

public class EventoActivarFinal : IEventoActivarEscena
{
    private GameObject Objeto;
    public int Indice => ProveedorServicios.IntentarObtenerServicio<ControladorFlujoEscenas>(out var controlador) ?
                         controlador.Contenedor.EscenaFinalIndice : -1;
    public GameObject ObjetoPadre { get => Objeto; }
    public bool UnloadPrevious { get; set; }

    public EventoActivarFinal(GameObject ObjetoActivador, bool unloadPrevious = true)
    {
        Objeto = ObjetoActivador;
        UnloadPrevious = unloadPrevious;
    }
}

