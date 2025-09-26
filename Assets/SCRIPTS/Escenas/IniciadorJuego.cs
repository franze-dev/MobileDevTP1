using UnityEngine;

public class IniciadorJuego : MonoBehaviour
{
    [SerializeField] private PantallaCarga PantallaCarga;

    private void Start()
    {
        PantallaCarga = ProveedorServicios.IntentarObtenerServicio<PantallaCarga>( out var pantalla) ? pantalla : null;

        DespachadorEventos.Despachar<IEventoActivarEscena>(new EventoActivarJuego(gameObject, false));

        Destroy(gameObject);
    }
}
