using UnityEngine;
using System.Collections;

public class PantallaCalibTuto : MonoBehaviour
{
    [Header("Imagenes Teclado")]
    public Texture2D[] ImagenesDelTuto;
    public Texture2D[] ImagenesDeCalib;
    public Texture2D ImaReady;
    [Header("Textos Gestos")]
    public GameObject TextDeslizaArriba;
    public GameObject TextTocaElBoton;
    public GameObject TextListos;

    public float Intervalo = 1.6f;//tiempo de cada cuanto cambia de imagen
    float TempoIntTuto = 0;
    int EnCursoTuto = 0;
    int EnCursoCalib = 0;
    float TempoIntCalib = 0;


    [SerializeField] private ContrCalibracion ContrCalib;
    [SerializeField] private DetectorInput InputDetector;
    private Renderer Renderer;

    private void Awake()
    {
        if (ContrCalib == null)
            Debug.LogError("Falta el GestionadoDeEstados en " + gameObject.name);

        Renderer = GetComponent<Renderer>();

        TextDeslizaArriba?.SetActive(false);
        TextTocaElBoton?.SetActive(false);
        TextListos?.SetActive(false);
    }

    void Update()
    {
        switch (ContrCalib.EstAct)
        {
            case ContrCalibracion.EstadoCalibracion.Calibrando:

                if (InputDetector.InputAct == DetectorInput.TipoInput.Teclado)
                    ActualizarCalibracion();
                else
                {
                    TextListos.SetActive(false);
                    TextTocaElBoton.SetActive(false);
                    TextDeslizaArriba.SetActive(true);
                }

                    break;
            case ContrCalibracion.EstadoCalibracion.Tutorial:
                if (InputDetector.InputAct == DetectorInput.TipoInput.Teclado)
                    ActualizarTutorialTec();
                else
                {
                    TextDeslizaArriba.SetActive(false);
                    TextListos.SetActive(false);
                    TextTocaElBoton.SetActive(true);
                }

                    break;
            case ContrCalibracion.EstadoCalibracion.Finalizado:
                if (InputDetector.InputAct == DetectorInput.TipoInput.Teclado)
                    Renderer.material.mainTexture = ImaReady;
                else
                {
                    TextDeslizaArriba.SetActive(false);
                    TextTocaElBoton.SetActive(false);
                    TextListos.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    private void ActualizarTutorialTec()
    {
        TempoIntTuto += Time.deltaTime;
        if (TempoIntTuto >= Intervalo)
        {
            TempoIntTuto = 0;
            if (EnCursoTuto + 1 < ImagenesDelTuto.Length)
                EnCursoTuto++;
            else
                EnCursoTuto = 0;
        }
        Renderer.material.mainTexture = ImagenesDelTuto[EnCursoTuto];
    }

    private void ActualizarCalibracion()
    {
        TempoIntCalib += Time.deltaTime;
        if (TempoIntCalib >= Intervalo)
        {
            TempoIntCalib = 0;
            if (EnCursoCalib + 1 < ImagenesDeCalib.Length)
                EnCursoCalib++;
            else
                EnCursoCalib = 0;
        }
        Renderer.material.mainTexture = ImagenesDeCalib[EnCursoCalib];
    }
}
