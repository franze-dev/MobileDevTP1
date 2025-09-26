using UnityEngine;

[RequireComponent(typeof(CargadorEscenas))]
public class ControladorFlujoEscenas : MonoBehaviour
{
    [SerializeField] public ContenedorEscenas Contenedor;
    private CargadorEscenas Cargador;

    private void Awake()
    {
        ProveedorServicios.DefinirServicio(this);

        Cargador = GetComponent<CargadorEscenas>();

        ProveedorEventos.Suscribir<IEventoActivarEscena>(ActivarEscena);
    }

    private void OnDestroy()
    {
        ProveedorEventos.Desuscribir<IEventoActivarEscena>(ActivarEscena);
    }

    public void ActivarEscena(IEventoActivarEscena evento)
    {
        int indice = evento.Indice;

        var escenaActivaIndice = Cargador.EscenaActiva();

        if (escenaActivaIndice == indice)
        {
            Debug.Log("Se trato de cargar una misma escena dos veces. " + escenaActivaIndice);
            return;
        }

        if (evento.Indice == DatosEscenaJuego.Index)
        {
            ProveedorServicios.IntentarObtenerServicio<GestionadoDeMenues>(out var gestion);
            gestion.EstadoActual = null;
        }

        if (!EstaCargada(indice))
            Cargador.CargarEscena(indice, Contenedor.EscenaCargandoIndice, evento.DescargaAnterior);
        else
            Cargador.ActivarEscena(indice);
    }

    public bool EstaCargada(int index)
    {
        return Cargador.EstaCargada(index);
    }

    public bool EsJuego(int index)
    {
        return Cargador.EsJuego(index);
    }

    public void DescargarEscena(int index)
    {
        Cargador.DescargarEscena(index);
    }
}
