using UnityEngine;
using System.Collections;

public class EstantePartida : ManejoBolsas
{
	public GameObject ManoReceptora;
	
	void OnTriggerEnter(Collider other)
	{
		ManejoBolsas recept = other.GetComponent<ManejoBolsas>();
		if(recept != null)
		{
			Dar(recept);
		}
	}

	public override void Dar(ManejoBolsas receptor)
	{
        if (receptor.Recibir(Bolsas[0])) {
            Bolsas.RemoveAt(0);
        }
    }
	
	public override bool Recibir (BolsaLogica bolsa)
	{
		//bolsa.CintaReceptora = CintaReceptora.gameObject;
		bolsa.Portador = gameObject;
		return base.Recibir (bolsa);
	}
}
