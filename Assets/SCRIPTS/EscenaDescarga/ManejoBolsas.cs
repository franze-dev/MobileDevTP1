using UnityEngine;
using System.Collections;

public class ManejoBolsas : MonoBehaviour 
{
	protected System.Collections.Generic.List<BolsaLogica> Bolsas = new System.Collections.Generic.List<BolsaLogica>();
	public ControladorDeDescarga Controlador;
	protected int Contador = 0;
	public Renderer Renderer;
    public Collider Collider;

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
		Collider = GetComponent<Collider>();
    }

    public virtual bool Recibir(BolsaLogica bolsa)
	{
		Debug.Log(gameObject.name+" / Recibir()");
		Bolsas.Add(bolsa);
        bolsa.Pasaje();
		return true;
	}
	
	public bool Tenencia()
	{
		
		if(Bolsas.Count != 0)
			return true;
		else
			return false;
	}
	
	public virtual void Dar(ManejoBolsas receptor)
	{
		//es el encargado de decidir si le da o no la bolsa
	}
}
