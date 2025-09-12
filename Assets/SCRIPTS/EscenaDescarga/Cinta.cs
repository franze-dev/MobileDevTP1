using UnityEngine;
using System.Collections;

public class Cinta : ManejoBolsas 
{
	public bool Encendida;//lo que hace la animacion
	public float Velocidad = 1;
	public GameObject Mano;
	public float Tiempo = 0.5f;
	private Transform ObjAct = null;
	
	//animacion de parpadeo
	public float Intervalo = 0.7f;
	public float Permanencia = 0.2f;
	private float AnimTempo = 0;
	public GameObject ModelCinta;
	public Color32 ColorParpadeo;
	private Color32 _colorInicial;
	private Renderer _renderizadorDeCinta;
	
	void Start () 
	{
		_renderizadorDeCinta = ModelCinta.GetComponent<Renderer>();
		_colorInicial = _renderizadorDeCinta.material.color;
    }
	
	void Update () 
	{
		//animacion de parpadeo
		if(Encendida)
		{
			AnimTempo += Time.deltaTime;
			if(AnimTempo > Permanencia)
			{
				if(_renderizadorDeCinta.material.color == ColorParpadeo)
				{
					AnimTempo = 0;
					_renderizadorDeCinta.material.color = _colorInicial;
				}
			}
			if(AnimTempo > Intervalo)
			{
				if(_renderizadorDeCinta.material.color == _colorInicial)
				{
					AnimTempo = 0;
					_renderizadorDeCinta.material.color = ColorParpadeo;
				}
			}
		}
		
		//movimiento del bolsa
		for(int i = 0; i < Bolsas.Count; i++)
		{
			if(Bolsas[i].Renderer.enabled)
			{
				if(!Bolsas[i].EnSmoot)
				{
					Bolsas[i].enabled = false;
					Bolsas[i].TempoEnCinta += Time.deltaTime;
					
					Bolsas[i].transform.position += transform.right * Velocidad * Time.deltaTime;
					Vector3 vAux = Bolsas[i].transform.localPosition;
					vAux.y = 3.61f;//altura especifica
					Bolsas[i].transform.localPosition = vAux;					
					
					if(Bolsas[i].TempoEnCinta >= Bolsas[i].TiempEnCinta)
					{
						Bolsas[i].TempoEnCinta = 0;
						ObjAct.gameObject.SetActive(false);
					}
				}
			}
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		ManejoBolsas recept = other.GetComponent<ManejoBolsas>();

		if(recept)
			Dar(recept);
	}
	
	public override bool Recibir(BolsaLogica p)
	{
        Controlador.LlegadaBolsa(p);
        p.Portador = gameObject;
        ObjAct = p.transform;
        base.Recibir(p);
        Apagar();

        return true;
    }
	
	public void Encender()
	{
		Encendida = true;
		_renderizadorDeCinta.material.color = _colorInicial;
	}
	public void Apagar()
	{
		Encendida = false;
		_renderizadorDeCinta.material.color = _colorInicial;
	}
}
