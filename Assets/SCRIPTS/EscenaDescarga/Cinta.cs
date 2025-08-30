using UnityEngine;
using System.Collections;

public class Cinta : ManejoPallets 
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
		
		//movimiento del pallet
		for(int i = 0; i < Pallets.Count; i++)
		{
			if(Pallets[i].Renderer.enabled)
			{
				if(!Pallets[i].EnSmoot)
				{
					Pallets[i].enabled = false;
					Pallets[i].TempoEnCinta += Time.deltaTime;
					
					Pallets[i].transform.position += transform.right * Velocidad * Time.deltaTime;
					Vector3 vAux = Pallets[i].transform.localPosition;
					vAux.y = 3.61f;//altura especifica
					Pallets[i].transform.localPosition = vAux;					
					
					if(Pallets[i].TempoEnCinta >= Pallets[i].TiempEnCinta)
					{
						Pallets[i].TempoEnCinta = 0;
						ObjAct.gameObject.SetActive(false);
					}
				}
			}
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		ManejoPallets recept = other.GetComponent<ManejoPallets>();

		if(recept)
			Dar(recept);
	}
	
	public override bool Recibir(Pallet p)
	{
        Controlador.LlegadaPallet(p);
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
