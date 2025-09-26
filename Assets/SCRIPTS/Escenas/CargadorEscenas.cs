using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CargadorEscenas : MonoBehaviour
{
    private List<Scene> listaEscenas;

    private void Awake()
    {
        ProveedorEventos.Suscribir<IEventoSalirDeJuego>(SalirDeJuego);

        listaEscenas = new()
        {
            SceneManager.GetActiveScene()
        };
    }

    private void OnDestroy()
    {
        DescargarTodas();
        ProveedorEventos.Desuscribir<IEventoSalirDeJuego>(SalirDeJuego);
    }

    public static void SalirDeJuego(IEventoSalirDeJuego evento)
    {
        Debug.Log("Salir juego");
        Application.Quit();
    }

    public void CargarEscena(int nuevaEscena, int transicion, bool unloadPrevious = true)
    {
        StartCoroutine(CorrutinaCargarEscena(nuevaEscena, transicion, unloadPrevious));
    }

    public int EscenaActiva()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        return (int)activeScene.buildIndex;
    }

    public void DescargarTodas()
    {
        foreach (var scene in listaEscenas)
            if (scene.IsValid() && scene.isLoaded)
                SceneManager.UnloadSceneAsync(scene);

        listaEscenas.Clear();
    }

    public IEnumerator CorrutinaCargarEscena(int nuevaEscena, int transicion, bool unloadPrevious = true)
    {
        if (EstaCargada(nuevaEscena))
            yield break;

        var anterior = EscenaActiva();

        if (unloadPrevious)
        {
            yield return SceneManager.UnloadSceneAsync(anterior);
            yield return null;
        }

        var cargando = SceneManager.LoadSceneAsync(nuevaEscena, LoadSceneMode.Additive);

        if (!EstaCargada(transicion))
        {
            yield return SceneManager.LoadSceneAsync(transicion, LoadSceneMode.Additive);
            listaEscenas.Add(ObtenerEscena(transicion));
        }

        ActivarEscena(transicion);

        while (!cargando.isDone)
        {
            DespachadorEventos.Despachar<IEventoCarga>(new EventoCarga(gameObject, cargando.progress));
            yield return null;
        }

        yield return cargando;
        listaEscenas.Add(ObtenerEscena(nuevaEscena));
        yield return null;

        ActivarEscena(nuevaEscena);
        DespachadorEventos.Despachar<IEventoReiniciarCarga>(new EventoReiniciarCarga(gameObject));
    }

    private Scene ObtenerEscena(int indice)
    {
        return SceneManager.GetSceneByBuildIndex(indice);
    }

    public Scene BuscarEnLista(int nuevaEscena)
    {
        Scene escena = new();

        foreach (var miEscena in listaEscenas)
        {
            escena = miEscena;
            bool esLaMisma = miEscena.buildIndex == (int)nuevaEscena;
            if (esLaMisma)
                return miEscena;
        }

        Debug.LogWarning($"{nuevaEscena} is not loaded yet");
        return escena;
    }


    public bool ActivarEscena(int nuevaEscena)
    {
        Scene escena = BuscarEnLista(nuevaEscena);

        if (escena.buildIndex != (int)nuevaEscena)
            return false;

        SceneManager.SetActiveScene(escena);
        return true;
    }

    private void DescargarEscena(Scene escena)
    {
        listaEscenas.Remove(escena);

        if (!escena.isLoaded)
        {
            Debug.LogWarning($"Tried to unload a scene that is already unloaded. Did not do it. Scene state: {escena}");
            return;
        }

        SceneManager.UnloadSceneAsync(escena);
    }

    public void DescargarEscena(int nuevaEscena)
    {
        Scene escena = BuscarEnLista(nuevaEscena);

        if (escena.buildIndex != (int)nuevaEscena)
            return;

        DescargarEscena(escena);
    }


    public bool EstaCargada(int indice)
    {
        foreach (var escena in listaEscenas)
        {
            bool esLaMisma = escena.buildIndex == (int)indice;
            if (esLaMisma)
                return true;
        }
        return false;
    }

    public bool EsJuego(int indice)
    {
        return indice == DatosEscenaJuego.Index;
    }

#if UNITY_EDITOR    
    public static int ObtenerIndice(SceneAsset asset)
    {
        if (!asset)
            return 0;

        return SceneUtility.GetBuildIndexByScenePath(AssetDatabase.GetAssetPath(asset));
    }
#endif
}
