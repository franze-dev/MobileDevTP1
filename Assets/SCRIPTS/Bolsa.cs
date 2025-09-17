using UnityEngine;
using System.Collections;

public class Bolsa : MonoBehaviour
{
	public BolsaLogica.Valores Monto;
	//public int IdPlayer = 0;
	public string TagPlayer = "";
	public Texture2D ImagenInventario;
	Jugador Pj = null;
	
	bool Desapareciendo;
	public GameObject Particulas;
	public float TiempParts = 2.5f;

	private Renderer Renderer;
	private Collider Collider;
	private ParticleSystem ParticleSystem;

    // Use this for initialization
    void Start () 
	{
		Monto = BolsaLogica.Valores.Valor2;
		
		
		if(Particulas != null)
			Particulas.SetActive(false);
			
		Renderer = GetComponent<Renderer>();
		Collider = GetComponent<Collider>();
		ParticleSystem = Particulas?.GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update ()
	{
		
		if(Desapareciendo)
		{
			TiempParts -= Time.deltaTime;
			if(TiempParts <= 0)
			{
				Renderer.enabled = true;
				Collider.enabled = true;
				
				ParticleSystem.Stop();
				gameObject.SetActive(false);
			}
		}
		
	}
	
	void OnTriggerEnter(Collider coll)
	{
		if(coll.tag == TagPlayer)
		{
			Pj = coll.GetComponent<Jugador>();
				if(Pj.AgregarBolsa(this))
					Desaparecer();
		}
	}
	
	public void Desaparecer()
	{
		ParticleSystem.Play();
		Desapareciendo = true;
		
		Renderer.enabled = false;
		Collider.enabled = false;
		
		if(Particulas != null)
		{
            ParticleSystem.Play();
		}
	
	}
}
