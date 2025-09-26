using UnityEngine;

public interface IEventoSalirDeJuego : IEvento
{
}

/// <summary>
/// Event that is called to exit the game
/// </summary>
public class EventoSalirDeJuego : IEventoSalirDeJuego
{
    private GameObject _gameObject;
    public GameObject ObjetoPadre => _gameObject;

    public EventoSalirDeJuego(GameObject gameObject)
    {
        _gameObject = gameObject;
    }
}