using UnityEngine;

public class Pallet : MonoBehaviour 
{
	public Valores Valor;
	public float Tiempo;
	public GameObject CintaReceptora = null;
	public GameObject Portador = null;
	public float TiempEnCinta = 1.5f;
	public float TempoEnCinta = 0;
	
	public enum Valores {Valor1 = 100000, 
						 Valor2 = 250000, 
						 Valor3 = 500000}
	
	
	public float TiempSmoot = 0.3f;
	public bool EnSmoot = false;
	private float _tempoSmoot = 0;
	public Renderer Renderer;
	

	void Start()
	{
		Renderer = GetComponent<Renderer>();

        Pasaje();
	}
	
	void LateUpdate () 
	{
		if(Portador != null)
		{
			if(EnSmoot)
			{
				_tempoSmoot += Time.deltaTime;
				if(_tempoSmoot >= TiempSmoot)
				{
					EnSmoot = false;
					_tempoSmoot = 0;
				}
				else
				{
					print("smoot");
					
					if(Portador.GetComponent<ManoRecept>() != null)
						transform.position = Portador.transform.position - Vector3.up * 1.2f;
					else
						transform.position = Vector3.Lerp(transform.position, Portador.transform.position, Time.deltaTime * 10);
				}
				
			}
			else
			{
				print("crudo");
				
				if(Portador.GetComponent<ManoRecept>() != null)
					transform.position = Portador.transform.position - Vector3.up * 1.2f;
				else
					transform.position = Portador.transform.position;
					
			}
		}
			
	}
	
	public void Pasaje()
	{
		EnSmoot = true;
		_tempoSmoot = 0;
	}
}
