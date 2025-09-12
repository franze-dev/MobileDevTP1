using UnityEngine;
using System.Collections;

public class EstantePartida : ManejoPallets
{
	public GameObject ManoReceptora;
	
	void OnTriggerEnter(Collider other)
	{
		ManejoPallets recept = other.GetComponent<ManejoPallets>();
		if(recept != null)
		{
			Dar(recept);
		}
	}

	public override void Dar(ManejoPallets receptor)
	{
        if (receptor.Recibir(Pallets[0])) {
            Pallets.RemoveAt(0);
        }
    }
	
	public override bool Recibir (BolsaLogica pallet)
	{
		//pallet.CintaReceptora = CintaReceptora.gameObject;
		pallet.Portador = gameObject;
		return base.Recibir (pallet);
	}
}
