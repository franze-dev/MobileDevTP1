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
    private Animation _enterExitAnimations;

    private void Awake()
    {
        _animacionPuerta = _puerta.GetComponent<Animation>();
        _enterExitAnimations = GetComponent<Animation>();
    }

    void Update()
    {
        switch (_animacionActual)
        {
            case AnimationActual.Cerrando:

                if (!_enterExitAnimations.IsPlaying(_nombreDeSalida))
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
        _enterExitAnimations.Play(_nombreDeEntrada);
        AnimarPuerta();
    }

    public void AnimarSalida()
    {
        _animacionActual = AnimationActual.Cerrando;
        _enterExitAnimations.Play(_nombreDeSalida);
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
