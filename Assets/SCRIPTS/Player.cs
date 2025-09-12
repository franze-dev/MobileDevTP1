using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public int Dinero = 0;
	public int IdPlayer = 0;
	
	public Bolsa[] Bolsas;
	int CantBolsAct = 0;
	public string TagBolsas = "";
	
	public enum Estados{EnDescarga, EnConduccion, EnCalibracion, EnTutorial}
	public Estados EstAct = Estados.EnConduccion;
	
	public bool EnConduccion = true;
	public bool EnDescarga = false;
	
	public ControladorDeDescarga ContrDesc;
	public ContrCalibracion ContrCalib;
	public ContrTutorial ContrTuto;
	public Frenado Frenado;
    public Respawn Respawn;
	public Rigidbody Rigidbody;
    public Collider[] Colliders;

	Visualizacion MiVisualizacion;

    void Start () 
	{
		for(int i = 0; i< Bolsas.Length;i++)
			Bolsas[i] = null;
		
		MiVisualizacion = GetComponent<Visualizacion>();
		Frenado = GetComponent<Frenado>();
		Rigidbody = GetComponent<Rigidbody>();
		Respawn = GetComponent<Respawn>();
		Colliders = GetComponentsInChildren<Collider>();
    }
	
	public bool AgregarBolsa(Bolsa b)
	{
		if(CantBolsAct + 1 <= Bolsas.Length)
		{
			Bolsas[CantBolsAct] = b;
			CantBolsAct++;
			Dinero += (int)b.Monto;
			b.Desaparecer();
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public void VaciarInv()
	{
		for(int i = 0; i< Bolsas.Length;i++)
			Bolsas[i] = null;
		
		CantBolsAct = 0;
	}
	
	public bool ConBolasas()
	{
		for(int i = 0; i< Bolsas.Length;i++)
		{
			if(Bolsas[i] != null)
			{
				return true;
			}
		}
		return false;
	}
	
	public void SetContrDesc(ControladorDeDescarga contr)
	{
		ContrDesc = contr;
	}
	
	public ControladorDeDescarga GetContr()
	{
		return ContrDesc;
	}
	
	public void CambiarACalibracion()
	{
		MiVisualizacion.CambiarACalibracion();
		EstAct = Player.Estados.EnCalibracion;
	}
	
	public void CambiarATutorial()
	{
		MiVisualizacion.CambiarATutorial();
		EstAct = Player.Estados.EnTutorial;
		ContrTuto.Iniciar();
	}
	
	public void CambiarAConduccion()
	{
		MiVisualizacion.CambiarAConduccion();
		EstAct = Player.Estados.EnConduccion;
	}
	
	public void CambiarADescarga()
	{
		MiVisualizacion.CambiarADescarga();
		EstAct = Player.Estados.EnDescarga;
	}
	
	public void SacarBolasa()
	{
		for(int i = 0; i < Bolsas.Length; i++)
		{
			if(Bolsas[i] != null)
			{
				Bolsas[i] = null;
				return;
			}				
		}
	}
	
	
}
