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
    [SerializeField] int CamionId = 0;
    private GameManager GameManager => GameManager.Instancia;
    private Rect ZonaCorrespondiente;

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

            ZonaCorrespondiente = GameManager.ZonaCorrespondeA(CamionId);
        }
    }

    private void Update()
    {
        InputDetector.DetectorGestos.Actualizar();

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

            case MoveType.Gestos:

                if (InputDetector.DetectorGestos.DeslizarDesde(DetectorGestos.Direccion.Der, ZonaCorrespondiente))
                {
                    if (TercerCompleto && !Tenencia() && Desde.Tenencia())
                        PrimerPaso();

                    else if (PrimerCompleto && Tenencia())
                        SegundoPaso();

                    else if (SegundoCompleto && Tenencia())
                        TercerPaso();
                }
                break;
            default:
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
