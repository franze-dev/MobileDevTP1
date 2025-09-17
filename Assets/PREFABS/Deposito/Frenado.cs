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
	private Jugador Jugador;
	private ControlDireccion ControlDireccion;
	private CarController CarController;
	private Rigidbody Rigidbody;

    void Start () 
	{
		Jugador = GetComponent<Jugador>();
		ControlDireccion = GetComponent<ControlDireccion>();
		CarController = gameObject.GetComponent<CarController>();
		Rigidbody = GetComponent<Rigidbody>();
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
				if(Jugador.ConBolasas())
				{
					dep.Entrar(Jugador);
					Destino = other.transform.position;
					transform.forward = Destino - transform.position;
					Frenar();
				}				
			}
		}
	}
	
	public void Frenar()
	{
		ControlDireccion.enabled = false;
		CarController.SetAcel(0);

        Rigidbody.linearVelocity = Vector3.zero;
		
		Frenando = true;
		Tempo = 0;
		Contador = 0;
	}
	
	public void RestaurarVel()
	{
		ControlDireccion.enabled = true;
        CarController.SetAcel(1);
        Frenando = false;
		Tempo = 0;
		Contador = 0;
	}
}
