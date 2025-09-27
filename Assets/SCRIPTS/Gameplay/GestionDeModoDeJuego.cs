using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class GestionDeModoDeJuego : MonoBehaviour
{
    [Header("Valores en las opciones")]
    [Header("Jugadores")]
    [SerializeField] private int MultijugadorID = 0;
    [SerializeField] private int UnJugadorID = 1;
    [Header("Dificultades")]
    [SerializeField] private int FacilID = 1;
    [SerializeField] private int NormalID = 0;
    [SerializeField] private int DificilID = 2;

    public int Multijugador => MultijugadorID;
    public int UnJugador => UnJugadorID;
    public int Facil => FacilID;
    public int Normal => NormalID;
    public int Dificil => DificilID;

    [Header("Opciones")]
    [SerializeField] private TMP_Dropdown JugadoresOpciones;
    [SerializeField] private TMP_Dropdown DificultadOpciones;

    public int JugadoresOpcionActual => JugadoresOpciones.value;
    public int DificultadOpcionActual => DificultadOpciones.value;
    public bool IsMultiplayer => JugadoresOpcionActual == Multijugador;

    private Dictionary<string, float[]> valueVersions;

    private void Awake()
    {
        ProveedorServicios.DefinirServicio(this);
        valueVersions = new();
    }

    private void OnDestroy()
    {
        ProveedorServicios.DefinirServicio<GestionDeModoDeJuego>(null);
    }
    public void GuardarValor(float ValorFacil, float ValorNormal, float ValorDificil, 
                             string NombreVariable)
    {
        float[] valores = new float[3];

        valores[Facil] = ValorFacil;
        valores[Normal] = ValorNormal;
        valores[Dificil] = ValorDificil;

        valueVersions.Add(NombreVariable, valores);
    }

    public float ObtenerValor(string NombreVariable)
    {
        return valueVersions[NombreVariable][DificultadOpcionActual];
    }
}
