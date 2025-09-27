using UnityEngine;

public class GestionDeJugadores : MonoBehaviour
{
    [SerializeField] Jugador Jugador1;
    [SerializeField] Jugador Jugador2;

    private void Start()
    {
        ProveedorServicios.IntentarObtenerServicio<GestionDeModoDeJuego>(out var gestion);

        if (gestion == null)
            return;

        if (!gestion.IsMultiplayer)
        {
            Jugador2.enabled = false;
            Jugador2.gameObject.SetActive(false);
        }
    }
}
