using UnityEngine;
using System.Collections;

public class Frenado : MonoBehaviour 
{
	public float VelEntrada = 0;
	public string TagDeposito = "Deposito";
	
	int Contador = 0;
	int CantMensajes = 10;
	float TiempFrenado = 0.5f;
	float Tempo = 0f;
	
	Vector3 Destino;
	
	public bool Frenando = false;
	
	void Start () 
	{
		Frenar();
	}
	
	void FixedUpdate ()
	{
		if(Frenando)
		{
			Tempo += Time.fixedDeltaTime;
			if(Tempo >= (TiempFrenado / CantMensajes) * Contador)
				Contador++;
		}
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if(other.tag == TagDeposito)
		{
			Deposito dep = other.GetComponent<Deposito>();

			if (dep == null)
				Debug.LogError("El objeto con tag " + TagDeposito + " no tiene el script Deposito. (" + other.gameObject.name + ")");

            if (dep.Vacio)
			{	
				if(this.GetComponent<Player>().ConBolasas())
				{
					dep.Entrar(this.GetComponent<Player>());
					Destino = other.transform.position;
					transform.forward = Destino - transform.position;
					Frenar();
				}				
			}
		}
	}
	
	public void Frenar()
	{
		GetComponent<ControlDireccion>().enabled = false;
		gameObject.GetComponent<CarController>().SetAcel(0);

        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
		
		Frenando = true;
		Tempo = 0;
		Contador = 0;
	}
	
	public void RestaurarVel()
	{
		//Debug.Log(gameObject.name + "restaura la velociad");
		GetComponent<ControlDireccion>().enabled = true;
        gameObject.GetComponent<CarController>().SetAcel(1);
        Frenando = false;
		Tempo = 0;
		Contador = 0;
		//gameObject.SendMessage("SetDragZ", 1f);
	}
}
