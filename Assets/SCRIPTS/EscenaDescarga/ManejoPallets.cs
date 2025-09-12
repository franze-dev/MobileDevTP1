using UnityEngine;
using System.Collections;

public class ManejoPallets : MonoBehaviour 
{
	protected System.Collections.Generic.List<BolsaLogica> Pallets = new System.Collections.Generic.List<BolsaLogica>();
	public ControladorDeDescarga Controlador;
	protected int Contador = 0;
	public Renderer Renderer;
    public Collider Collider;

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
		Collider = GetComponent<Collider>();
    }

    public virtual bool Recibir(BolsaLogica pallet)
	{
		Debug.Log(gameObject.name+" / Recibir()");
		Pallets.Add(pallet);
		pallet.Pasaje();
		return true;
	}
	
	public bool Tenencia()
	{
		
		if(Pallets.Count != 0)
			return true;
		else
			return false;
	}
	
	public virtual void Dar(ManejoPallets receptor)
	{
		//es el encargado de decidir si le da o no la bolsa
	}
}
