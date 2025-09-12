using UnityEngine;
using System.Collections;

public class EstanteLlegada : ManejoBolsas
{
	public GameObject Mano;
	public ContrCalibracion ContrCalib;
	
	public override bool Recibir(BolsaLogica p)
	{
        p.Portador = this.gameObject;
        base.Recibir(p);
        ContrCalib.FinTutorial();

        return true;
    }
}
