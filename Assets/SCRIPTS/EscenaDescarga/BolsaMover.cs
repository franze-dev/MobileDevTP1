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
    private GestionDeTeclas Teclas;

    private bool PrimerCompleto => ContadorPasos == 1;
    private bool SegundoCompleto => ContadorPasos == 2;
    private bool TercerCompleto => ContadorPasos == 0;

    private void Start()
    {
        if (InputDetector == null)
            Debug.LogError("Falta detector de input en " + gameObject.name);

        Teclas = new();

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
            Destroy(BotonTransladar);
            Destroy(BotonTransladarSingle);

            if (miInput == MoveType.WASD)
            {
                Teclas.Conectar(KeyCode.A, new MoverPaso(0, this));
                Teclas.Conectar(KeyCode.S, new MoverPaso(1, this));
                Teclas.Conectar(KeyCode.D, new MoverPaso(2, this));
            }
            else
            {
                Teclas.Conectar(KeyCode.LeftArrow, new MoverPaso(0, this));
                Teclas.Conectar(KeyCode.DownArrow, new MoverPaso(1, this));
                Teclas.Conectar(KeyCode.RightArrow, new MoverPaso(2, this));
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

    public void HacerPaso(int pasoid)
    {
        if (pasoid == 0)
        {
            if (TercerCompleto && !Tenencia() && Desde.Tenencia())
                PrimerPaso();
        }
        else if (pasoid == 1)
        {
            if (PrimerCompleto && Tenencia())
                SegundoPaso();
        }
        else
        {
            if (SegundoCompleto && Tenencia())
                TercerPaso();
        }
    }

    private void Update()
    {
        if (PJ.EstAct == Jugador.Estados.EnDescarga || PJ.EstAct == Jugador.Estados.EnCalibracion)
        {
            if (InputDetector.InputAct == DetectorInput.TipoInput.Teclado)
                Teclas.EjecutarInputsUnaVez();
            else
            {
                if (BotonTransladar != null || BotonTransladarSingle != null)
                {
                    if (gestion.IsMultiplayer)
                        BotonTransladar.SetActive(true);
                    else
                        BotonTransladarSingle.SetActive(true);
                }
            }
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

internal class MoverPaso : IInputComando
{
    private int PasoID;
    private BolsaMover Mover;

    public MoverPaso(int pasoID, BolsaMover mover)
    {
        PasoID = pasoID;
        Mover = mover;
    }

    public void Execute()
    {
        Mover.HacerPaso(PasoID);
    }
}