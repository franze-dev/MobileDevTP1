using UnityEngine;

public class GestionadoDePausa : MonoBehaviour
{
    private KeyCode TeclaPausa = KeyCode.Escape;

    public static bool Pausado { get; set; }

    private void Start()
    {
        Time.timeScale = 1f;

        ProveedorEventos.Suscribir<IEventoPausa>(Pausar);
    }

    private void Update()
    {
        if (Input.GetKeyUp(TeclaPausa))
            DespachadorEventos.Despachar<IEventoPausa>(new EventoPausa(gameObject));
    }
    private void OnDestroy()
    {
        ProveedorEventos.Desuscribir<IEventoPausa>(Pausar);
    }

    private void Pausar(IEventoPausa pauseEvent)
    {
        ProveedorServicios.IntentarObtenerServicio<GestionadoDeMenues>(out var gestion);

        if (gestion.EstadoActual != null && gestion.EstadoActual is not EstadoPausa)
            return;

        Pausado = !Pausado;

            Time.timeScale = Pausado ? 0f : 1f;

        if (Pausado)
            DespachadorEventos.Despachar<IEventoActivarEscena>(new EventoActivarMenu(new EstadoPausa(), gameObject, false));
        else
            DespachadorEventos.Despachar<IEventoActivarEscena>(new EventoActivarJuego(gameObject, false));
    }
}

public class EventoPausa : IEventoPausa
{
    private GameObject gameObject;
    public GameObject ObjetoPadre => gameObject;

    public EventoPausa(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }
}

public interface IEventoPausa : IEvento
{

}