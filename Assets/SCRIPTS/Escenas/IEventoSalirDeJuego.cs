using UnityEngine;

public interface IEventoSalirDeJuego : IEvento
{
}

/// <summary>
/// Event that is called to exit the game
/// </summary>
public class ExitGameEvent : IEventoSalirDeJuego
{
    private GameObject _gameObject;
    public GameObject ObjetoPadre => _gameObject;

    public ExitGameEvent(GameObject gameObject)
    {
        _gameObject = gameObject;
    }
}