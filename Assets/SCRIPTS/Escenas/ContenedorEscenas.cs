using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ContenerorEscenas", menuName = "ScriptableObjects/ContenedorEscenas")]
public class ContenedorEscenas : ScriptableObject
{
    [SerializeField] private int MenuIndice;
    [SerializeField] private int JuegoIndice;
    [SerializeField] private int CargandoIndice;
    [SerializeField] private int FinalIndice;

    public int EscenaMenuIndice { get => MenuIndice; }
    public int EscenaJuegoIndice { get => JuegoIndice; }
    public int EscenaCargandoIndice { get => CargandoIndice; }
    public int EscenaFinalIndice { get => FinalIndice; set => FinalIndice = value; }

#if UNITY_EDITOR
    [SerializeField] private SceneAsset EscenaMenu;
    [SerializeField] private SceneAsset EscenaJuego;
    [SerializeField] private SceneAsset EscenaFinal;

    /// <summary>
    /// Saves the indexes of the provided scenes
    /// </summary>
    private void OnValidate()
    {
        MenuIndice = CargadorEscenas.ObtenerIndice(EscenaMenu);
        JuegoIndice = CargadorEscenas.ObtenerIndice(EscenaJuego);
        FinalIndice = CargadorEscenas.ObtenerIndice(EscenaFinal);
    }
#endif
}
