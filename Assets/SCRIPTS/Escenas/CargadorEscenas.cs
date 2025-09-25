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

    public void CargarEscena(int nuevaEscena, int transicion)
    {
        StartCoroutine(CorrutinaCargarEscena(nuevaEscena, transicion));
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

    public IEnumerator CorrutinaCargarEscena(int nuevaEscena, int transicion)
    {
        if (EstaCargada(nuevaEscena))
            yield break;

        var anterior = EscenaActiva();

        SceneManager.LoadScene(transicion);
        yield return SceneManager.UnloadSceneAsync(anterior);
        yield return null;
        yield return SceneManager.LoadSceneAsync(nuevaEscena);
        yield return null;
        yield return SceneManager.UnloadSceneAsync(transicion);
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
