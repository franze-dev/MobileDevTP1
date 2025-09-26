using System;
using UnityEngine;
using UnityEngine.UIElements;

public interface IEventoActivarEscena : IEvento
{
    int Indice { get; }
    bool DescargaAnterior { get; set; }
}

public class EventoActivarJuego : IEventoActivarEscena
{
    private GameObject Objeto;
    public GameObject ObjetoPadre { get => Objeto; }
    public int Indice => DatosEscenaJuego.Index;
    public bool DescargaAnterior { get; set; }

    public EventoActivarJuego(GameObject ObjetoActivador, bool DescargarAnterior = true)
    {
        Objeto = ObjetoActivador;
        DescargaAnterior = DescargarAnterior;
        if (!DescargaAnterior)
        {
            ProveedorServicios.IntentarObtenerServicio<GestionadoDeMenues>(out var gestion);
            gestion.EsconderObjetos();
        }
    }
}

public class EventoActivarMenu : IEventoActivarEscena
{
    private GameObject Objeto;
    public GameObject ObjetoPadre { get => Objeto; }
    public int Indice => DatosEscenaMenu.Index;
    public bool DescargaAnterior { get; set; }
    public IEstadoMenu SiguienteEstado { get; set; }

    public EventoActivarMenu(IEstadoMenu Siguiente, GameObject ObjetoActivador, bool DescargarAnterior = true)
    {
        this.DescargaAnterior = DescargarAnterior;
        Objeto = ObjetoActivador;
        SiguienteEstado = Siguiente;

        ProveedorServicios.IntentarObtenerServicio<GestionadoDeMenues>(out var menu);

        menu?.TransicionA(Siguiente);
    }
}   

public class EventoActivarFinal : IEventoActivarEscena
{
    private GameObject Objeto;
    public int Indice => ProveedorServicios.IntentarObtenerServicio<ControladorFlujoEscenas>(out var controlador) ?
                         controlador.Contenedor.EscenaFinalIndice : -1;
    public GameObject ObjetoPadre { get => Objeto; }
    public bool DescargaAnterior { get; set; }

    public EventoActivarFinal(GameObject ObjetoActivador, bool DescargarAnterior = true)
    {
        Objeto = ObjetoActivador;
        DescargaAnterior = DescargarAnterior;
    }
}

