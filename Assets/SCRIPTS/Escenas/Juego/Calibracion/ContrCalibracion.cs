using UnityEngine;

public class ContrCalibracion : MonoBehaviour
{
    public Jugador Pj;
    public float TiempEspCalib = 3;
    float Tempo2 = 0;

    [SerializeField] private GestionadoDeEstados GestionEstados;

    public ManejoBolsas Partida;
    public ManejoBolsas Llegada;
    public BolsaLogica P;
    public ManejoBolsas bolsasMover;

    public enum EstadoCalibracion { Calibrando, Tutorial, Finalizado }
    public EstadoCalibracion EstAct = EstadoCalibracion.Calibrando;

    [SerializeField] private GameManager GM;

    void Start()
    {
        if (GestionEstados == null)
            Debug.LogError("Falta el GestionadoDeEstados en " + gameObject.name);

        if (bolsasMover == null)
            Debug.LogError("Falta el BolsasMover en " + gameObject.name);

        bolsasMover.enabled = false;
        Pj.ContrCalib = this;

        P.CintaReceptora = Llegada.gameObject;
        Partida.Recibir(P);

        SetActivComp(false);
    }

    void Update()
    {
        if (EstAct == EstadoCalibracion.Tutorial)
        {
            if (Tempo2 < TiempEspCalib)
            {
                Tempo2 += Time.deltaTime;
                if (Tempo2 > TiempEspCalib)
                {
                    SetActivComp(true);
                }
            }
        }
    }

    

    public void IniciarTesteo()
    {
        EstAct = EstadoCalibracion.Tutorial;
        bolsasMover.enabled = true;
    }

    public void FinTutorial()
    {
        EstAct = EstadoCalibracion.Finalizado;
        bolsasMover.enabled = false;
        GM.FinCalibracion(Pj.IdPlayer);
    }

    void SetActivComp(bool estado)
    {
        if (Partida.Renderer != null)
            Partida.Renderer.enabled = estado;
        Partida.Collider.enabled = estado;
        if (Llegada.Renderer != null)
            Llegada.Renderer.enabled = estado;
        Llegada.Collider.enabled = estado;
        P.Renderer.enabled = estado;
    }
}
