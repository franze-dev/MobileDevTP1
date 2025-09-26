using UnityEngine;

public class IniciadorJuego : MonoBehaviour
{
    private void Start()
    {
        DespachadorEventos.Despachar<IEventoActivarEscena>(new EventoActivarMenu(new EstadoMenuPrincipal(), gameObject, false));

        Destroy(gameObject);
    }
}
