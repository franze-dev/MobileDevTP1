using UnityEngine;
using System.Collections;

public class LoopTextura : MonoBehaviour 
{
	public float Intervalo = 1;
	float Tempo = 0;
	
	public Texture2D[] Imagenes;
	int Contador = 0;
	private Renderer Renderer;

	// Use this for initialization
	void Start () 
	{
		Renderer = GetComponent<Renderer>();

        if (Imagenes.Length > 0)
            Renderer.material.mainTexture = Imagenes[0];
	}
	
	// Update is called once per frame
	void Update () 
	{
		Tempo += Time.deltaTime;
		
		if(Tempo >= Intervalo)
		{
			Tempo = 0;
			Contador++;
			if(Contador >= Imagenes.Length)
			{
				Contador = 0;
			}
			Renderer.material.mainTexture = Imagenes[Contador];
		}
	}
}
