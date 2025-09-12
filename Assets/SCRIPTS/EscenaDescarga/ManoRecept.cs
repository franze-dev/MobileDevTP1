using UnityEngine;
using System.Collections;

public class ManoRecept : ManejoBolsas 
{
	public bool TengoBolsa = false;
	
	void FixedUpdate () 
	{
		TengoBolsa = Tenencia();
	}
	
	void OnTriggerEnter(Collider other)
	{
		ManejoBolsas recept = other.GetComponent<ManejoBolsas>();
		if(recept != null)
			Dar(recept);
	}
	
	public override bool Recibir(BolsaLogica bolsa)
	{
		if(!Tenencia())
		{
			bolsa.Portador = gameObject;
			base.Recibir(bolsa);
			return true;
		}
		else
			return false;
	}
	
	public override void Dar(ManejoBolsas receptor)
	{
		switch (receptor.tag)
		{
		case "Mano":
			if(Tenencia())
				if(receptor.name == "Right Hand")
					if(receptor.Recibir(Bolsas[0]))
						Bolsas.RemoveAt(0);
			break;
			
		case "Cinta":
			if(Tenencia())
				if(receptor.Recibir(Bolsas[0]))
					Bolsas.RemoveAt(0);
			break;
			
		case "Estante":
			break;
		}
	}
}
