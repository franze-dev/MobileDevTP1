using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManejoPuntosFinales : MonoBehaviour
{
    [Header("Timigs")]
    [SerializeField] private float TiempoFade = 2f;
    [SerializeField] private float TiempoAntesPuntos = 1f;
    [SerializeField] private float TiempoParpadeo = 1f;
    [SerializeField] private float TiempoMostrarPuntos = 5f;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI Dinero1;
    [SerializeField] private TextMeshProUGUI Dinero2;
    [SerializeField] private string DineroPrefijo = "$ ";
    [SerializeField] private List<Image> ImagenesDesvanecer;
    [SerializeField] private List<TextMeshProUGUI> TextosDesvanecer;

    private GameObject ObjetoGanador;


    void Start()
    {
        Dinero1.gameObject.SetActive(false);
        Dinero2.gameObject.SetActive(false);
        DefinirGanador();

        StartCoroutine(SecuenciaFinal());
    }

    private IEnumerator SecuenciaFinal()
    {
        yield return StartCoroutine(Fade(0f,1f,TiempoFade));

        yield return new WaitForSeconds(TiempoAntesPuntos);

        Dinero1.gameObject.SetActive(true);
        Dinero2.gameObject.SetActive(true);

        float TiempoActual = 0f;
        float ParpadeoTimer = 0f;

        while (TiempoActual < TiempoMostrarPuntos)
        {
            TiempoActual += Time.deltaTime;
            ParpadeoTimer += Time.deltaTime;

            if (ParpadeoTimer >= TiempoParpadeo)
            {
                ObjetoGanador.SetActive(!ObjetoGanador.activeSelf);
                ParpadeoTimer = 0;
            }

            yield return null;
        }

        ObjetoGanador.SetActive(true);

        yield return StartCoroutine(Fade(1f, 0f, TiempoFade));

        DespachadorEventos.Despachar<IEventoActivarEscena>(new EventoActivarJuego(gameObject));
    }

    private IEnumerator Fade(float desde, float hasta, float tiempoFade)
    {
        float tiempo = 0;

        Color color;

        while (tiempo < tiempoFade)
        {
            tiempo += Time.deltaTime;
            float a = Mathf.Lerp(desde, hasta, tiempo /  tiempoFade);

            foreach(var img in ImagenesDesvanecer)
            {
                color = img.color;
                color.a = a;
                img.color = color;
            }

            foreach (var text in TextosDesvanecer)
            {
                color = text.color;
                color.a = a;
                text.color = color;
            }

            color = Dinero1.color;
            color.a = a;
            Dinero1.color = color;
            Dinero2.color = color;

            yield return null;
        }
    }

    void DefinirGanador()
    {
        switch (DatosPartida.LadoGanadaor)
        {
            case DatosPartida.Lados.Der:
                Dinero2.text = DineroPrefijo + DatosPartida.PtsGanador;
                Dinero1.text = DineroPrefijo + DatosPartida.PtsPerdedor;
                ObjetoGanador = Dinero2.gameObject;
                break;

            case DatosPartida.Lados.Izq:
            case DatosPartida.Lados.Non:
                Dinero1.text = DineroPrefijo + DatosPartida.PtsGanador;
                Dinero2.text = DineroPrefijo + DatosPartida.PtsPerdedor;
                ObjetoGanador = Dinero1.gameObject;
                break;
        }
    }
}