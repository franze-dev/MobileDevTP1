using UnityEngine;

public class AnimadorDePuerta : MonoBehaviour 
{
    enum AnimationActual { Abriendo, Cerrando, Quieta }

    [SerializeField] private string _nombreDeEntrada = "Entrada";
    [SerializeField] private string _nombreDeSalida = "Salida";
    [SerializeField] private GameObject _puerta;
    [SerializeField] public ControladorDeDescarga controlador;
    private AnimationActual _animacionActual = AnimationActual.Quieta;
    private Animation _animacionPuerta;

    private void Awake()
    {
        _animacionPuerta = _puerta.GetComponent<Animation>();
    }

    void Update()
    {
        switch (_animacionActual)
        {
            case AnimationActual.Cerrando:

                if (_animacionPuerta.IsPlaying(_nombreDeSalida))
                {
                    _animacionActual = AnimationActual.Quieta;
                    controlador.FinAnimSalida();
                }

                break;
        }
    }

    public void AnimarEntrada()
    {
        _animacionActual = AnimationActual.Abriendo;
        _animacionPuerta.Play(_nombreDeEntrada);
        AnimarPuerta();
    }

    public void AnimarSalida()
    {
        _animacionActual = AnimationActual.Cerrando;
        _animacionPuerta.Play(_nombreDeSalida);
        AnimarPuerta();
    }

    public void AnimarPuerta()
    {
        if (_puerta != null)
        {
            _animacionPuerta["AnimPuerta"].time = 0;
            _animacionPuerta["AnimPuerta"].speed = 1;
            _animacionPuerta.Play("AnimPuerta");
        }
    }
}
