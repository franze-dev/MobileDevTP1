using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ContenerorEscenas", menuName = "ScriptableObjects/ContenedorEscenas")]
public class ContenedorEscenas : ScriptableObject
{
    [SerializeField] private int MenuIndice;
    [SerializeField] private int JuegoIndice;
    [SerializeField] private int CargandoIndice;
    [SerializeField] private int FinalIndice;
    [SerializeField] private int ComienzoIndice;

    public int EscenaMenuIndice => MenuIndice;
    public int EscenaJuegoIndice => JuegoIndice;
    public int EscenaCargandoIndice => CargandoIndice;
    public int EscenaFinalIndice => FinalIndice;

#if UNITY_EDITOR
    [SerializeField] private SceneAsset EscenaMenu;
    [SerializeField] private SceneAsset EscenaJuego;
    [SerializeField] private SceneAsset EscenaFinal;
    [SerializeField] private SceneAsset EscenaCarga;
    [SerializeField] private SceneAsset EscenaComienzo;

    /// <summary>
    /// Saves the indexes of the provided scenes
    /// </summary>
    private void OnValidate()
    {
        MenuIndice = CargadorEscenas.ObtenerIndice(EscenaMenu);
        JuegoIndice = CargadorEscenas.ObtenerIndice(EscenaJuego);
        FinalIndice = CargadorEscenas.ObtenerIndice(EscenaFinal);
        CargandoIndice = CargadorEscenas.ObtenerIndice(EscenaCarga);
        ComienzoIndice = CargadorEscenas.ObtenerIndice(EscenaComienzo);
    }
#endif
}
