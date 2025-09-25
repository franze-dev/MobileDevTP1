using UnityEngine;

[RequireComponent(typeof(CargadorEscenas))]
public class ControladorFlujoEscenas : MonoBehaviour
{
    /// <summary>
    /// Saves the scene asset container (the scriptable object)
    /// </summary>
    [SerializeField] public ContenedorEscenas Contenedor;
    private CargadorEscenas Cargador;

    private void Awake()
    {
        ProveedorServicios.DefinirServicio<ControladorFlujoEscenas>(this);

        Cargador = GetComponent<CargadorEscenas>();

        ProveedorEventos.Suscribir<IEventoActivarEscena>(ActivarEscena);
    }

    private void OnDestroy()
    {
        ProveedorEventos.Desuscribir<IEventoActivarEscena>(ActivarEscena);
    }

    /// <summary>
    /// Sets an active scene given a scene event
    /// </summary>
    /// <param name="evento"></param>
    public void ActivarEscena(IEventoActivarEscena evento)
    {
        int indice = evento.Indice;

        var escenaActivaIndice = Cargador.EscenaActiva();

        if (escenaActivaIndice == indice)
            return;

        if (!EstaCargada(indice))
            Cargador.CargarEscena(indice, Contenedor.EscenaCargandoIndice);
        else
            Cargador.ActivarEscena(indice);
    }

    /// <summary>
    /// Checks if a scene is loaded given an index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool EstaCargada(int index)
    {
        return Cargador.EstaCargada(index);
    }

    /// <summary>
    /// Unloads all the gameplay levels
    /// </summary>
    public void UnloadGameplay()
    {
        Cargador.DescargarJuego();
    }

    /// <summary>
    /// If the scene in the index is gameplay, it returns true
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool EsJuego(int index)
    {
        return Cargador.EsJuego(index);
    }

    /// <summary>
    /// Unloads the scene at the provided index
    /// </summary>
    /// <param name="index"></param>
    public void DescargarEscena(int index)
    {
        Cargador.DescargarEscena(index);
    }
}
