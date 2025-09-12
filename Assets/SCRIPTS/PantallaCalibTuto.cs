using UnityEngine;
using System.Collections;

public class PantallaCalibTuto : MonoBehaviour
{
    public Texture2D[] ImagenesDelTuto;
    public float Intervalo = 1.2f;//tiempo de cada cuanto cambia de imagen
    float TempoIntTuto = 0;
    int EnCursoTuto = 0;

    public Texture2D[] ImagenesDeCalib;
    int EnCursoCalib = 0;
    float TempoIntCalib = 0;

    public Texture2D ImaReady;

    [SerializeField] private ContrCalibracion ContrCalib;

    private void Awake()
    {
        if (ContrCalib == null)
            Debug.LogError("Falta el GestionadoDeEstados en " + gameObject.name);
    }

    void Update()
    {
        switch (ContrCalib.EstAct)
        {
            case ContrCalibracion.EstadoCalibracion.Calibrando:
                //pongase en posicion para iniciar
                TempoIntCalib += Time.deltaTime;
                if (TempoIntCalib >= Intervalo)
                {
                    TempoIntCalib = 0;
                    if (EnCursoCalib + 1 < ImagenesDeCalib.Length)
                        EnCursoCalib++;
                    else
                        EnCursoCalib = 0;
                }
                GetComponent<Renderer>().material.mainTexture = ImagenesDeCalib[EnCursoCalib];

                break;
            case ContrCalibracion.EstadoCalibracion.Tutorial:
                //tome la bolsa y depositela en el estante
                TempoIntTuto += Time.deltaTime;
                if (TempoIntTuto >= Intervalo)
                {
                    TempoIntTuto = 0;
                    if (EnCursoTuto + 1 < ImagenesDelTuto.Length)
                        EnCursoTuto++;
                    else
                        EnCursoTuto = 0;
                }
                GetComponent<Renderer>().material.mainTexture = ImagenesDelTuto[EnCursoTuto];

                break;
            case ContrCalibracion.EstadoCalibracion.Finalizado:
                //esperando al otro jugador		
                GetComponent<Renderer>().material.mainTexture = ImaReady;
                break;
            default:
                break;
        }
    }
}
