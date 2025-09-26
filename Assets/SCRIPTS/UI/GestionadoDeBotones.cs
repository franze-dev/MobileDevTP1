using UnityEngine;

public class GestionadoDeBotones : MonoBehaviour
{
    private GestionadoDeMenues gestion;

    private void Start()
    {
        ProveedorServicios.IntentarObtenerServicio(out gestion);
    }

    public void ACreditos()
    {
       gestion.TransicionA(new EstadoCreditos());
    }

    public void ASalir()
    {
        gestion.TransicionA(new EstadoSalir());
    }

    public void AMenuPrincipal()
    {
        gestion.TransicionA(new EstadoMenuPrincipal());
    }

    public void AMenuAnterior()
    {
        IEstadoMenu Anterior = gestion.EstadoAnterior;
        gestion.TransicionA(Anterior);
    }

    public void Despausar()
    {
        DespachadorEventos.Despachar<IEventoPausa>(new EventoPausa(gameObject));
    }

    public void AJuego()
    {
        DespachadorEventos.Despachar<IEventoActivarEscena>(new EventoActivarJuego(gameObject, false));
    }

    public void AModos()
    {
        gestion.TransicionA(new EstadoModos());
    }
    public void SalirDeJuego()
    {
        DespachadorEventos.Despachar<IEventoSalirDeJuego>(new EventoSalirDeJuego(gameObject));
    }
}