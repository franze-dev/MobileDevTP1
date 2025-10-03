using UnityEngine;

public class ControlDireccion : MonoBehaviour
{
    public enum TipoInput { WASD, Flechas, Gestos }
    public TipoInput InputAct = ControlDireccion.TipoInput.WASD;

    public Transform ManoDer;
    public Transform ManoIzq;

    public float MaxAng = 90;
    public float DesSensibilidad = 90;

    public bool Habilitado = true;
    private CarController CarController;
    [SerializeField] int CamionId = 0;
    private GameManager GameManager => GameManager.Instancia;
    private Rect ZonaCorrespondiente;

    [SerializeField] private DetectorInput InputDetector;
    private GestionDeTeclas Teclas;
    private GestionDeGestos Gestos;
    private GiroComando InfoComandoIzq;
    private GiroComando InfoComandoDer;

    void Start()
    {
        CarController = gameObject.GetComponent<CarController>();

        if (InputDetector == null)
            Debug.LogError("Falta el Input Detector en " + gameObject.name);

        if (InputDetector.DetectorGestos == null)
            Debug.LogError("Falta el Detector de Gestos en " + gameObject.name);

        Teclas = new();
        Gestos = new(InputDetector.DetectorGestos);

        if (InputDetector.InputAct == DetectorInput.TipoInput.Touch)
        {
            InputAct = TipoInput.Gestos;

            ZonaCorrespondiente = GameManager.ZonaCorrespondeA(CamionId);

            Gestos.Conectar(DetectorGestos.Direccion.Izq, ZonaCorrespondiente, new GiroComando(-1, CarController));
            Gestos.Conectar(DetectorGestos.Direccion.Der, ZonaCorrespondiente, new GiroComando(1, CarController));
        }
        else
        {
            if (InputAct == TipoInput.WASD)
            {
                Teclas.Conectar(KeyCode.A, new GiroComando(-1, CarController));
                Teclas.Conectar(KeyCode.D, new GiroComando(1, CarController));
            }
            else
            {
                Teclas.Conectar(KeyCode.LeftArrow, new GiroComando(-1, CarController));
                Teclas.Conectar(KeyCode.RightArrow, new GiroComando(1, CarController));
            }
        }
    }

    void Update()
    {
        if (Habilitado)
        {
            if (InputDetector.InputAct == DetectorInput.TipoInput.Touch)
                Gestos.EjecutarInputs();
            else
                Teclas.EjecutarInputs();
        }
    }
}

internal class GiroComando : IInputComando
{
    private int Dir;
    private CarController Auto;

    public GiroComando(int dir, CarController auto)
    {
        Dir = dir;
        Auto = auto;
    }

    public void Execute()
    {
        Auto.SetGiro(Dir);
    }
}