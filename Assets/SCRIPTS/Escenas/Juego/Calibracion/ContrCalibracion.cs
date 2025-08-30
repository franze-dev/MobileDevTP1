using UnityEngine;
using System.Collections;

public class ContrCalibracion : MonoBehaviour
{
	public Player Pj;
	public float TiempEspCalib = 3;
	float Tempo2 = 0;
	
	public enum Estados{Calibrando, Tutorial, Finalizado}
	public Estados EstAct = Estados.Calibrando;
	
	public ManejoPallets Partida;
	public ManejoPallets Llegada;
	public Pallet P;
    public ManejoPallets palletsMover;
	
	[SerializeField] private GameManager GM;
	
	void Start () 
	{
        palletsMover.enabled = false;
        Pj.ContrCalib = this;
		
		P.CintaReceptora = Llegada.gameObject;
		Partida.Recibir(P);
		
		SetActivComp(false);
    }
	
	void Update ()
	{
		if(EstAct == ContrCalibracion.Estados.Tutorial)
		{
			if(Tempo2 < TiempEspCalib)
			{
				Tempo2 += Time.deltaTime;
				if(Tempo2 > TiempEspCalib)
				{
					 SetActivComp(true);
				}
			}
		}
	}
	
	public void IniciarTesteo()
	{
		EstAct = ContrCalibracion.Estados.Tutorial;
        palletsMover.enabled = true;
    }
	
	public void FinTutorial()
	{
		EstAct = ContrCalibracion.Estados.Finalizado;
        palletsMover.enabled = false;
        GM.FinCalibracion(Pj.IdPlayer);
	}
	
	void SetActivComp(bool estado)
	{
		if(Partida.Renderer != null)
			Partida.Renderer.enabled = estado;
		Partida.GetComponent<Collider>().enabled = estado;
		if(Llegada.Renderer != null)
			Llegada.Renderer.enabled = estado;
		Llegada.GetComponent<Collider>().enabled = estado;
		P.Renderer.enabled = estado;
	}
}
