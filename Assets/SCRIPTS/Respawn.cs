using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour 
{
	CheakPoint CPAct;
	CheakPoint CPAnt;
	
	public float AngMax = 90;//angulo maximo antes del cual se reinicia el camion
	int VerifPorCuadro = 20;
	int Contador = 0;
	
	public float RangMinDer = 0;
	public float RangMaxDer = 0;
	
	bool IgnorandoColision = false;
	public float TiempDeNoColision = 2;
	float Tempo = 0;

	private Rigidbody Rigidbody;
	private CarController CarController;
	private Visualizacion Visualizacion;

	void Start () 
	{
		//restaura las colisiones
		Physics.IgnoreLayerCollision(8,9,false);

		Rigidbody = GetComponent<Rigidbody>();
		CarController = GetComponent<CarController>();
        Visualizacion = GetComponent<Visualizacion>();
    }
	
	void Update ()
	{
		if(CPAct != null)
		{
			Contador++;
			if(Contador == VerifPorCuadro)
			{
				Contador = 0;
				if(AngMax < Quaternion.Angle(transform.rotation, CPAct.transform.rotation))
				{
					Respawnear();
				}
			}
		}
		
		if(IgnorandoColision)
		{
			Tempo += Time.deltaTime;
			if(Tempo > TiempDeNoColision)
				IgnorarColision(false);
		}
		
	}

	public void Respawnear()
	{
        Rigidbody.linearVelocity = Vector3.zero;

        CarController.SetGiro(0f);
		
		if(CPAct.Habilitado())
		{
			if(Visualizacion.LadoAct == Visualizacion.Lado.Der)
				transform.position = CPAct.transform.position + CPAct.transform.right * Random.Range(RangMinDer, RangMaxDer);
			else 
				transform.position = CPAct.transform.position + CPAct.transform.right * Random.Range(RangMinDer * (-1), RangMaxDer * (-1));
			transform.forward = CPAct.transform.forward;
		}
		else if(CPAnt != null)
		{
			if(Visualizacion.LadoAct == Visualizacion.Lado.Der)
				transform.position = CPAnt.transform.position + CPAnt.transform.right * Random.Range(RangMinDer, RangMaxDer);
			else
				transform.position = CPAnt.transform.position + CPAnt.transform.right * Random.Range(RangMinDer * (-1), RangMaxDer * (-1));
			transform.forward = CPAnt.transform.forward;
		}
		
		IgnorarColision(true);
	}
	
	public void Respawnear(Vector3 pos)
	{
        Rigidbody.linearVelocity = Vector3.zero;
		
		CarController.SetGiro(0f);
		
		transform.position = pos;
		
		IgnorarColision(true);
	}
	
	public void Respawnear(Vector3 pos, Vector3 dir)
	{
		Rigidbody.linearVelocity = Vector3.zero;
		
		CarController.SetGiro(0f);
		
		transform.position = pos;
		transform.forward = dir;
		
		IgnorarColision(true);
	}
	
	public void AgregarCheakPoint(CheakPoint cp)
	{
		if(cp != CPAct)
		{
			CPAnt = CPAct;
			CPAct = cp;
		}
	}
	
	void IgnorarColision(bool b)
	{
		Physics.IgnoreLayerCollision(8,9,b);
		IgnorandoColision = b;	
		Tempo = 0;
	}
}
