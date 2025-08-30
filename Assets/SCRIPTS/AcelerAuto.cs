using UnityEngine;
using System.Collections;

public class AcelerAuto : MonoBehaviour 
{
	public float AcelPorSeg = 0;
	float Velocidad = 0;
	public float VelMax = 0;
	ReductorVelColl Obstaculo = null;
	
	bool Avil = true;
	public float TiempRecColl = 0;
	float Tempo = 0;

	// Update is called once per frame
	void Update ()
	{
		if(Avil)
		{
			Tempo += Time.deltaTime;
			if(Tempo > TiempRecColl)
			{
				Tempo = 0;
				Avil = false;
			}
		}
	}
	
	void FixedUpdate () 
	{
		if(Velocidad < VelMax)
			Velocidad += AcelPorSeg * Time.fixedDeltaTime;
		
		GetComponent<Rigidbody>().AddForce(this.transform.forward * Velocidad);
	}
	
	 void OnCollisionEnter(Collision collision)
	{
		if(!Avil)
		{
			Obstaculo = collision.transform.GetComponent<ReductorVelColl>();
			if(Obstaculo != null)
				GetComponent<Rigidbody>().linearVelocity /= 2;

			Obstaculo = null;
		}
	}
	
	public void Chocar(ReductorVelColl obst)
	{
		GetComponent<Rigidbody>().linearVelocity /= 2;
	}
	
}
