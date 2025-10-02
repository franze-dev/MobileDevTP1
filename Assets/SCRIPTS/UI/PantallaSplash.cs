using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PantallaSplash : MonoBehaviour
{
    [SerializeField] private GameObject ObjetoPantallaSplash;
    [SerializeField] private float TiempoPantallaSplash = 7f;
    [SerializeField] private GameObject SplashImagen;
    [SerializeField] private float RapidezEscalado = 0.1f;
    [SerializeField] private float EscalaMaxima = 2.4f;
    [SerializeField] private float EscalaMinima = 0.77f;

    private Vector3 EscalaMax => new(EscalaMaxima, EscalaMaxima);
    private Vector3 EscalaMin => new(EscalaMinima, EscalaMinima);
    private Vector3 RapidezEscala => new(RapidezEscalado, RapidezEscalado);

    private void Start()
    {
        StartCoroutine(CargarPantallaSplash());
    }

    private void Update()
    {
        SplashImagen.transform.localScale += RapidezEscala * Time.deltaTime;

        if (SplashImagen.transform.localScale.x > EscalaMax.x ||
            SplashImagen.transform.localScale.y > EscalaMax.y ||
            SplashImagen.transform.localScale.x < EscalaMin.x ||
            SplashImagen.transform.localScale.y < EscalaMin.y)
            RapidezEscalado *= -1;
    }

    private IEnumerator CargarPantallaSplash()
    {
        yield return new WaitForSeconds(TiempoPantallaSplash);

        ObjetoPantallaSplash.SetActive(false);

        DespachadorEventos.Despachar<IEventoEmpezarJuego>(new EventoEmpezarJuego(gameObject));
    }
}
