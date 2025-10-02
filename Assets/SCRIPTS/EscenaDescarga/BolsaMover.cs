using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolsaMover : ManejoBolsas
{
    [SerializeField] private DetectorInput InputDetector;

    public MoveType miInput;
    public enum MoveType
    {
        WASD,
        Arrows,
        Gestos
    }

    public ManejoBolsas Desde, Hasta;
    private int ContadorPasos = 0;
    [SerializeField] Jugador PJ;
    [SerializeField] private GameObject BotonTransladar;
    [SerializeField] private GameObject BotonTransladarSingle;
    private GestionDeModoDeJuego gestion;

    private bool PrimerCompleto => ContadorPasos == 1;
    private bool SegundoCompleto => ContadorPasos == 2;
    private bool TercerCompleto => ContadorPasos == 0;

    private void Start()
    {
        if (InputDetector == null)
            Debug.LogError("Falta detector de input en " + gameObject.name);

        if (InputDetector.InputAct == DetectorInput.TipoInput.Touch)
        {
            miInput = MoveType.Gestos;

            ProveedorServicios.IntentarObtenerServicio(out gestion);

            if (BotonTransladar != null && BotonTransladarSingle != null)
            {
                if (gestion.IsMultiplayer)
                {
                    Destroy(BotonTransladarSingle);
                    BotonTransladar.SetActive(false);
                }
                else
                {
                    Destroy(BotonTransladar);
                    BotonTransladarSingle.SetActive(false);
                }
            }
        }
        else
        {
            if (BotonTransladar != null && BotonTransladarSingle != null)
                if (BotonTransladar)
                {
                    Destroy(BotonTransladar);
                    Destroy(BotonTransladarSingle);
                }
        }
    }

    public void Trasladar()
    {
        if (TercerCompleto && !Tenencia() && Desde.Tenencia())
            PrimerPaso();

        else if (PrimerCompleto && Tenencia())
            SegundoPaso();

        else if (SegundoCompleto && Tenencia())
            TercerPaso();
    }

    private void Update()
    {
        InputDetector.DetectorGestos.Actualizar();

        if (PJ.EstAct == Jugador.Estados.EnDescarga)
            switch (miInput)
            {
                case MoveType.WASD:
                    if (!Tenencia() && Desde.Tenencia() && Input.GetKeyDown(KeyCode.A))
                    {
                        PrimerPaso();
                    }
                    if (Tenencia() && Input.GetKeyDown(KeyCode.S))
                    {
                        SegundoPaso();
                    }
                    if (SegundoCompleto && Tenencia() && Input.GetKeyDown(KeyCode.D))
                    {
                        TercerPaso();
                    }
                    break;
                case MoveType.Arrows:
                    if (!Tenencia() && Desde.Tenencia() && Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        PrimerPaso();
                    }
                    if (Tenencia() && Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        SegundoPaso();
                    }
                    if (SegundoCompleto && Tenencia() && Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        TercerPaso();
                    }
                    break;
                default:
                    if (BotonTransladar != null || BotonTransladarSingle != null)
                    {
                        if (gestion.IsMultiplayer)
                            BotonTransladar.SetActive(true);
                        else
                            BotonTransladarSingle.SetActive(true);
                    }
                    break;
            }
    }

    void PrimerPaso()
    {
        Desde.Dar(this);
        ContadorPasos = 1;
    }
    void SegundoPaso()
    {
        base.Bolsas[0].transform.position = transform.position;
        ContadorPasos = 2;
    }
    void TercerPaso()
    {
        Dar(Hasta);
        ContadorPasos = 0;
    }

    public override void Dar(ManejoBolsas receptor)
    {
        if (Tenencia())
        {
            if (receptor.Recibir(Bolsas[0]))
            {
                Bolsas.RemoveAt(0);
            }
        }
    }
    public override bool Recibir(BolsaLogica bolsa)
    {
        if (!Tenencia())
        {
            bolsa.Portador = this.gameObject;
            base.Recibir(bolsa);
            return true;
        }
        else
            return false;
    }
}
