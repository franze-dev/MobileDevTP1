using UnityEngine;
using System.Collections;

public class Deposito : MonoBehaviour 
{
	Player PjActual;
	public string PlayerTag = "Player";
	public bool Vacio = true;
	public ControladorDeDescarga Contr1;
	public ControladorDeDescarga Contr2;
	
	Collider[] PjColl;
	
	void Start () 
	{
		Physics.IgnoreLayerCollision(8,9,false);
	}
	
	void Update () 
	{
		if(!Vacio)
		{
			PjActual.transform.position = transform.position;
			PjActual.transform.forward = transform.forward;
		}
	}
	
	public void Soltar()
	{
		GameManager.Instancia.EnDeposito = false;

        PjActual.VaciarInv();
		PjActual.Frenado.RestaurarVel();
		PjActual.Respawn.Respawnear(transform.position, transform.forward);
		
		PjActual.Rigidbody.useGravity = true;
		for(int i = 0; i < PjColl.Length; i++)
			PjColl[i].enabled = true;
		
		Physics.IgnoreLayerCollision(8,9,false);
		
		PjActual = null;
		Vacio = true;
	}
	
	public void Entrar(Player pj)
	{
		if(pj.ConBolasas())
		{
			
			PjActual = pj;

			PjColl = PjActual.Colliders;
			for(int i = 0; i < PjColl.Length; i++)
				PjColl[i].enabled = false;
			PjActual.Rigidbody.useGravity = false;
			
			PjActual.transform.position = transform.position;
			PjActual.transform.forward = transform.forward;
			
			Vacio = false;
			
			Physics.IgnoreLayerCollision(8,9,true);
			
			Entro();
		}
	}
	
	public void Entro()
	{
		GameManager.Instancia.EnDeposito = true;

		if(PjActual.IdPlayer == 0)
			Contr1.Activar(this);
		else
			Contr2.Activar(this);
	}
}
