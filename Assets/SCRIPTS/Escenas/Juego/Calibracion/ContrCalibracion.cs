using UnityEngine;

public class ContrCalibracion : MonoBehaviour
{
    public Player Pj;
    public float TiempEspCalib = 3;
    float Tempo2 = 0;

    [SerializeField] private GestionadoDeEstados GestionEstados;

    public ManejoPallets Partida;
    public ManejoPallets Llegada;
    public Pallet P;
    public ManejoPallets palletsMover;

    public enum EstadoCalibracion { Calibrando, Tutorial, Finalizado }
    public EstadoCalibracion EstAct = EstadoCalibracion.Calibrando;

    [SerializeField] private GameManager GM;

    void Start()
    {
        if (GestionEstados == null)
            Debug.LogError("Falta el GestionadoDeEstados en " + gameObject.name);

        palletsMover.enabled = false;
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
        palletsMover.enabled = true;
    }

    public void FinTutorial()
    {
        EstAct = EstadoCalibracion.Finalizado;
        palletsMover.enabled = false;
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
