using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolsaMover : ManejoBolsas {

    public MoveType miInput;
    public enum MoveType {
        WASD,
        Arrows
    }

    public ManejoBolsas Desde, Hasta;
    bool segundoCompleto = false;

    private void Update() {
        switch (miInput) {
            case MoveType.WASD:
                if (!Tenencia() && Desde.Tenencia() && Input.GetKeyDown(KeyCode.A)) {
                    PrimerPaso();
                }
                if (Tenencia() && Input.GetKeyDown(KeyCode.S)) {
                    SegundoPaso();
                }
                if (segundoCompleto && Tenencia() && Input.GetKeyDown(KeyCode.D)) {
                    TercerPaso();
                }
                break;
            case MoveType.Arrows:
                if (!Tenencia() && Desde.Tenencia() && Input.GetKeyDown(KeyCode.LeftArrow)) {
                    PrimerPaso();
                }
                if (Tenencia() && Input.GetKeyDown(KeyCode.DownArrow)) {
                    SegundoPaso();
                }
                if (segundoCompleto && Tenencia() && Input.GetKeyDown(KeyCode.RightArrow)) {
                    TercerPaso();
                }
                break;
            default:
                break;
        }
    }

    void PrimerPaso() {
        Desde.Dar(this);
        segundoCompleto = false;
    }
    void SegundoPaso() {
        base.Bolsas[0].transform.position = transform.position;
        segundoCompleto = true;
    }
    void TercerPaso() {
        Dar(Hasta);
        segundoCompleto = false;
    }

    public override void Dar(ManejoBolsas receptor) {
        if (Tenencia()) {
            if (receptor.Recibir(Bolsas[0])) {
                Bolsas.RemoveAt(0);
            }
        }
    }
    public override bool Recibir(BolsaLogica bolsa) {
        if (!Tenencia()) {
            bolsa.Portador = this.gameObject;
            base.Recibir(bolsa);
            return true;
        }
        else
            return false;
    }
}
