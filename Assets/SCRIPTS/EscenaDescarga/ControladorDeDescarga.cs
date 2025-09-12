using UnityEngine;
using System.Collections;

public class ControladorDeDescarga : MonoBehaviour 
{
	int Contador = 0;
	
	private Deposito Dep;
	
	public GameObject[] Escenas;//todos los componentes que debe activar en esta escena
	
	public Player Camion;//jugador
	private MeshCollider CollCamion;
	
	public Pallet PEnMov = null;
	
	//las camaras que enciende y apaga
	public GameObject CamaraConduccion;
	public GameObject CamaraDescarga;
	
	//los prefab de los pallets
	public GameObject Pallet1;
	public GameObject Pallet2;
	public GameObject Pallet3;
	
	
	public Estanteria Est1;
	public Estanteria Est2;
	public Estanteria Est3;
	
	public Cinta Cin2;
	
	public float Bonus = 0;
	float TempoBonus;
	
	
	public AnimadorDePuerta ObjAnimado;

	
	//--------------------------------------------------------------//

	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < Escenas.Length; i++)
		{
			Escenas[i].SetActive(false);
		}
		
		CollCamion = Camion?.GetComponentInChildren<MeshCollider>();
		Camion?.SetContrDesc(this);
		if(ObjAnimado != null)
			ObjAnimado.controlador = this;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//contador de tiempo
		if(PEnMov != null)
		{
			if(TempoBonus > 0)
			{
				Bonus = (TempoBonus * (float)PEnMov.Valor) / PEnMov.Tiempo;
				TempoBonus -= Time.deltaTime;
			}
			else
				Bonus = 0;
		}
		
		
	}
	
	//--------------------------------------------------------------//
			
	public void Activar(Deposito d)
	{
		Dep = d;//recibe el deposito para que sepa cuando dejarlo ir al camion
		CamaraConduccion.SetActive(false);//apaga la camara de conduccion
			
		//activa los componentes
		for (int i = 0; i < Escenas.Length; i++)
		{
			Escenas[i].SetActive(true);
		}
		
			
		CollCamion.enabled = false;
		Camion?.CambiarADescarga();
		
		
		GameObject go;
		//asigna los pallets a las estanterias
		for(int i = 0; i < Camion.Bolasas.Length; i++)
		{
			if(Camion.Bolasas[i] != null)
			{
				Contador++;
				
				switch(Camion.Bolasas[i].Monto)
				{
				case Pallet.Valores.Valor1:
					go = (GameObject) Instantiate(Pallet1);
					Est1.Recibir(go.GetComponent<Pallet>());
					break;
					
				case Pallet.Valores.Valor2:
					go = (GameObject) Instantiate(Pallet2);
					Est2.Recibir(go.GetComponent<Pallet>());
					break;
					
				case Pallet.Valores.Valor3:
					go = (GameObject) Instantiate(Pallet3);
					Est3.Recibir(go.GetComponent<Pallet>());
					break;
				}
			}
		}
		//animacion
		ObjAnimado.AnimarEntrada();
		
	}
	
	//cuando sale de un estante
	public void SalidaPallet(Pallet p)
	{
		PEnMov = p;
		TempoBonus = p.Tiempo;
		Camion.SacarBolasa();
		//inicia el contador de tiempo para el bonus
	}
	
	//cuando llega a la cinta
	public void LlegadaPallet(Pallet p)
	{
		//termina el contador y suma los pts
		
		//termina la descarga
		PEnMov = null;
		Contador--;
		
		Camion.Dinero += (int)Bonus;
		
		if(Contador <= 0)
		{
			Finalizacion();
		}
		else
		{
			Est2.EncenderAnim();
		}
	}
	
	public void FinDelJuego()
	{
		//metodo llamado por el GameManager para avisar que se termino el juego
		
		//desactiva lo que da y recibe las bolsas para que no halla mas flujo de estas
		Est2.enabled = false;
		Cin2.enabled = false;
	}
	
	void Finalizacion()
	{
		ObjAnimado.AnimarSalida();
	}
	
	public Pallet GetPalletEnMov()
	{
		return PEnMov;
	}
	
	public void FinAnimEntrada()
	{
		//avisa cuando termino la animacion para que prosiga el juego
		Est2.EncenderAnim();
	}
	
	public void FinAnimSalida()
	{
		//avisa cuando termino la animacion para que prosiga el juego
		
		for (int i = 0; i < Escenas.Length; i++)
		{
			Escenas[i].SetActive(false);
		}
		
		CamaraConduccion.SetActive(true);
		
		CollCamion.enabled = true;
		
		Camion.CambiarAConduccion();
		
		Dep.Soltar();
		
	}
	
}
