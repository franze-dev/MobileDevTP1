using UnityEngine;

public class Jugador : MonoBehaviour
{
    public int Dinero = 0;
    public int IdPlayer = 0;

    public Bolsa[] Bolsas;
    public int CantBolsAct = 0;
    public string TagBolsas = "";

    public enum Estados { EnDescarga, EnConduccion, EnCalibracion, EnTutorial }
    public Estados EstAct = Estados.EnConduccion;

    public GameObject CanvasPlayer;

    [HideInInspector] public ControladorDeDescarga ContrDesc;
    [HideInInspector] public ContrCalibracion ContrCalib;
    [HideInInspector] public ContrTutorial ContrTuto;
    [HideInInspector] public Frenado Frenado;
    [HideInInspector] public Respawn Respawn;
    [HideInInspector] public Rigidbody Rigidbody;
    [HideInInspector] public Collider[] Colliders;

    [SerializeField] private GameObject CanvasDescarga;

    public Visualizacion MiVisualizacion;
    public ControlDireccion Direccion;

    void Start()
    {
        if (CanvasPlayer == null)
            Debug.LogWarning("No se han asignado los elementos de UI del jugador " + IdPlayer);

        for (int i = 0; i < Bolsas.Length; i++)
            Bolsas[i] = null;

        MiVisualizacion = GetComponent<Visualizacion>();
        Frenado = GetComponent<Frenado>();
        Rigidbody = GetComponent<Rigidbody>();
        Respawn = GetComponent<Respawn>();
        Colliders = GetComponentsInChildren<Collider>();

        CanvasDescarga?.SetActive(false);
    }

    public bool AgregarBolsa(Bolsa b)
    {
        if (CantBolsAct + 1 <= Bolsas.Length)
        {
            Bolsas[CantBolsAct] = b;
            CantBolsAct++;
            Dinero += (int)b.Monto;
            b.Desaparecer();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void VaciarInv()
    {
        for (int i = 0; i < Bolsas.Length; i++)
            Bolsas[i] = null;

        CantBolsAct = 0;
    }

    public bool ConBolasas()
    {
        for (int i = 0; i < Bolsas.Length; i++)
        {
            if (Bolsas[i] != null)
            {
                return true;
            }
        }
        return false;
    }

    public void SetContrDesc(ControladorDeDescarga contr)
    {
        ContrDesc = contr;
    }

    public ControladorDeDescarga GetContr()
    {
        return ContrDesc;
    }

    public void CambiarACalibracion()
    {
        CanvasDescarga?.SetActive(false);
        MiVisualizacion.CambiarACalibracion();
        EstAct = Jugador.Estados.EnCalibracion;
    }

    public void CambiarATutorial()
    {
        CanvasDescarga?.SetActive(false);
        MiVisualizacion.CambiarATutorial();
        EstAct = Jugador.Estados.EnTutorial;
        ContrTuto.Iniciar();
    }

    public void CambiarAConduccion()
    {
        CanvasDescarga?.SetActive(false);
        MiVisualizacion.CambiarAConduccion();
        EstAct = Jugador.Estados.EnConduccion;
    }

    public void CambiarADescarga()
    {
        CanvasDescarga?.SetActive(true);
        MiVisualizacion.CambiarADescarga();
        EstAct = Jugador.Estados.EnDescarga;
    }

    public void SacarBolasa()
    {
        for (int i = 0; i < Bolsas.Length; i++)
        {
            if (Bolsas[i] != null)
            {
                Bolsas[i] = null;
                return;
            }
        }
    }

    public void ChequearDescarga()
    {
        if (EstAct == Estados.EnDescarga)
        {
            if (CanvasPlayer.activeSelf)
                CanvasPlayer.SetActive(false);
        }

        if (EstAct != Estados.EnDescarga)
        {
            if (CanvasPlayer.activeSelf == false)
                CanvasPlayer.SetActive(true);
        }

    }
}
