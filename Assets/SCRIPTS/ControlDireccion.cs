using UnityEngine;

public class ControlDireccion : MonoBehaviour
{
    public enum TipoInput { Mouse, AWSD, Flechas, Gestos }
    public TipoInput InputAct = ControlDireccion.TipoInput.Mouse;

    public Transform ManoDer;
    public Transform ManoIzq;

    public float MaxAng = 90;
    public float DesSensibilidad = 90;

    private float Giro = 0;

    public bool Habilitado = true;
    private CarController CarController;
    [SerializeField] int CamionId = 0;
    private GameManager GameManager => GameManager.Instancia;
    private Rect ZonaCorrespondiente;

    [SerializeField] private DetectorInput InputDetector;

    void Start()
    {
        CarController = gameObject.GetComponent<CarController>();

        if (InputDetector == null)
            Debug.LogError("Falta el Input Detector en " + gameObject.name);

        if (InputDetector.DetectorGestos == null)
            Debug.LogError("Falta el Detector de Gestos en " + gameObject.name);

        if (InputDetector.InputAct == DetectorInput.TipoInput.Touch)
        {
            InputAct = TipoInput.Gestos;

            ZonaCorrespondiente = GameManager.ZonaCorrespondeA(CamionId);
        }
    }

    void Update()
    {
        if (InputDetector.InputAct == DetectorInput.TipoInput.Touch)
            InputDetector.DetectorGestos.Actualizar();

        switch (InputAct)
        {
            case TipoInput.Mouse:
                if (Habilitado)
                    CarController.SetGiro(MousePos.Relation(MousePos.AxisRelation.Horizontal));

                break;
            case TipoInput.AWSD:
                if (Habilitado)
                {
                    if (Input.GetKey(KeyCode.A))
                    {
                        CarController.SetGiro(-1);
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        CarController.SetGiro(1);
                    }
                }
                break;
            case TipoInput.Flechas:
                if (Habilitado)
                {
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        CarController.SetGiro(-1);
                    }
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        CarController.SetGiro(1);
                    }
                }
                break;
            case TipoInput.Gestos:
                if (Habilitado)
                {
                    if (InputDetector.DetectorGestos.DeslizarDesde(DetectorGestos.Direccion.Izq, ZonaCorrespondiente))
                    {
                        CarController.SetGiro(-1);
                    }
                    if (InputDetector.DetectorGestos.DeslizarDesde(DetectorGestos.Direccion.Der, ZonaCorrespondiente))
                    {
                        CarController.SetGiro(1);
                    }
                }
                break;
        }
    }

    public float GetGiro()
    {
        return Giro;
    }
}
