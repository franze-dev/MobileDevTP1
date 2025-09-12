using UnityEngine;
using System.Collections;

public class ManoRecept : ManejoPallets 
{
	public bool TengoPallet = false;
	
	void FixedUpdate () 
	{
		TengoPallet = Tenencia();
	}
	
	void OnTriggerEnter(Collider other)
	{
		ManejoPallets recept = other.GetComponent<ManejoPallets>();
		if(recept != null)
			Dar(recept);
	}
	
	public override bool Recibir(BolsaLogica pallet)
	{
		if(!Tenencia())
		{
			pallet.Portador = gameObject;
			base.Recibir(pallet);
			return true;
		}
		else
			return false;
	}
	
	public override void Dar(ManejoPallets receptor)
	{
		switch (receptor.tag)
		{
		case "Mano":
			if(Tenencia())
				if(receptor.name == "Right Hand")
					if(receptor.Recibir(Pallets[0]))
						Pallets.RemoveAt(0);
			break;
			
		case "Cinta":
			if(Tenencia())
				if(receptor.Recibir(Pallets[0]))
					Pallets.RemoveAt(0);
			break;
			
		case "Estante":
			break;
		}
	}
}
