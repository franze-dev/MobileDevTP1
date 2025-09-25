using UnityEngine;
using System.Collections;

public class VeredaRespawn : MonoBehaviour 
{
	public string PlayerTag = "Player";

	void Start () 
	{
		GetComponent<Renderer>().enabled = false;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == PlayerTag)
		{
			other.GetComponent<Respawn>().Respawnear();
		}	
	}
	
	void OnCollisionEnter(Collision collision) 
	{
		if(collision.gameObject.tag == PlayerTag)
		{
			collision.gameObject.GetComponent<Respawn>().Respawnear();
		}
	}
	
}
