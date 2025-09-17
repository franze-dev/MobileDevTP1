using UnityEngine;
using System.Collections;

public class ContrTutorial : MonoBehaviour 
{
	public Jugador Pj;
	public float TiempTuto = 15;
	public float Tempo = 0;
	
	public bool Finalizado = false;
	
	[SerializeField] private GameManager GM;
	
	//------------------------------------------------------------------//

	// Use this for initialization
	void Start () 
	{
		Pj.ContrTuto = this;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<Jugador>() == Pj)
			Finalizar();
	}
	
	//------------------------------------------------------------------//
	
	public void Iniciar()
	{
		Pj.Frenado.RestaurarVel();
	}
	
	public void Finalizar()
	{
		Finalizado = true;
		Pj.Frenado.Frenar();
		Pj.Rigidbody.linearVelocity = Vector3.zero;
		Pj.VaciarInv();
	}
}
