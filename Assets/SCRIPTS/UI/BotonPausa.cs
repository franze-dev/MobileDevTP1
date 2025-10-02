using UnityEngine;

public class BotonPausa : MonoBehaviour
{
    private GestionadoDeBotones gestionBotones;

    private void Start()
    {
        ProveedorServicios.IntentarObtenerServicio<GestionadoDeBotones>(out gestionBotones);
    }

    public void Pausar()
    {
        gestionBotones.Pausar();
    }
}
