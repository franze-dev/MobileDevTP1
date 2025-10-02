using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GestionDeItems : MonoBehaviour
{
    private int cantidadBolsas = 0;
    private int cantidadTaxis = 0;
    private int cantidadCajas = 0;
    private int cantidadConos = 0;
    [SerializeField] private List<GameObject> bolsas;
    [SerializeField] private List<GameObject> taxis;
    [SerializeField] private List<GameObject> cajas;
    [SerializeField] private List<GameObject> conos;
    private GestionDeModoDeJuego modoDeJuego;

    private void Start()
    {
        ProveedorServicios.IntentarObtenerServicio<GestionDeModoDeJuego>(out modoDeJuego);

        if (bolsas.Count == 0)
            Debug.LogError("No se encontraron las bolsas del juego");

        if (taxis.Count == 0)
            Debug.LogError("No se encontraron los taxis del juego");

        if (cajas.Count == 0)
            Debug.LogError("No se encontraron las cajas del juego");

        if (conos.Count == 0)
            Debug.LogError("No se encontraron las conos del juego");

        cantidadBolsas = bolsas.Count;
        cantidadTaxis = taxis.Count;
        cantidadCajas = cajas.Count;

        modoDeJuego.GuardarValor(cantidadTaxis / 3, cantidadTaxis, nameof(cantidadTaxis));
        modoDeJuego.GuardarValor(cantidadBolsas, cantidadBolsas / 3, nameof(cantidadBolsas));
        modoDeJuego.GuardarValor(cantidadCajas / 3, cantidadCajas, nameof(cantidadCajas));
        modoDeJuego.GuardarValor(cantidadConos / 3, cantidadConos, nameof(cantidadConos));

        cantidadBolsas = (int)modoDeJuego.ObtenerValor(nameof(cantidadBolsas));
        cantidadCajas = (int)modoDeJuego.ObtenerValor(nameof(cantidadCajas));
        cantidadTaxis = (int)modoDeJuego.ObtenerValor(nameof(cantidadTaxis));
        cantidadConos = (int)modoDeJuego.ObtenerValor(nameof(cantidadConos));

        if (cantidadBolsas < bolsas.Count)
            DesactivarRango(bolsas, bolsas.Count - cantidadBolsas);

        if (cantidadTaxis < taxis.Count)
            DesactivarRango(taxis, taxis.Count - cantidadTaxis);

        if (cantidadCajas < cajas.Count)
            DesactivarRango(cajas, cajas.Count - cantidadCajas);

        if (cantidadConos < conos.Count)
            DesactivarRango(conos, conos.Count - cantidadConos);
    }

    

    void DesactivarRango(List<GameObject> obj, int rango)
    {
        for (int i = 0; i < rango; i++)
        {
            bool deleted = false;
            do
            {
                var j = Random.Range(0, obj.Count);

                if (obj[j] == null)
                    deleted = false;
                else if (obj[j] != null)
                {
                    Destroy(obj[j]);
                    deleted = true;
                }

            } while (!deleted);
        }
    }
}
