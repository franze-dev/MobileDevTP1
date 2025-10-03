using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GestionDeCamaras : MonoBehaviour
{
    [SerializeField] private List<Camera> CamerasP1;
    [SerializeField] private List<Camera> CamerasP2;
    private GestionDeModoDeJuego Gestion;

    private void Start()
    {
        ProveedorServicios.IntentarObtenerServicio(out Gestion);

        if (Gestion != null)
            if (Gestion.JugadoresOpcionActual == Gestion.UnJugador)
            {
                foreach (var cam in CamerasP2)
                    cam.gameObject.SetActive(false);

                foreach (var cam in CamerasP1)
                {
                    cam.gameObject.SetActive(true);
                    var res = cam.rect;
                    res.x = 0;
                    res.width = 1;
                    cam.rect = res;
                }

            }
    }
}
