using UnityEngine;

public class IniciadorJuego : MonoBehaviour
{
    private void Start()
    {
        DespachadorEventos.Despachar<IEventoActivarEscena>(new EventoActivarJuego(gameObject, false));

        Destroy(gameObject);
    }
}
