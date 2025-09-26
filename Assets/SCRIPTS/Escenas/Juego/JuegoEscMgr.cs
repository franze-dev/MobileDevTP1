using UnityEngine;
using System.Collections;

public class JuegoEscMgr : MonoBehaviour 
{
	bool JuegoFinalizado = false;
	public float TiempoEsperaFin = 25;//tiempo que espera la aplicacion para volver al video introductorio desp de terminada la partida
	float Tempo = 0;
	
	bool JuegoIniciado = false;
	public float TiempoEsperaInicio = 120;//tiempo que espera la aplicacion para volver al video introductorio desp de terminada la partida
	float Tempo2 = 0;
	
	void Update () 
	{
		if(JuegoFinalizado)
		{
			Tempo += Time.deltaTime;
			if(Tempo > TiempoEsperaFin)
			{
				Tempo = 0;
				DespachadorEventos.Despachar<IEventoActivarEscena>(new EventoActivarJuego(gameObject));
			}
		}
		
		if(!JuegoIniciado)
		{
			Tempo2 += Time.deltaTime;
			if(Tempo > TiempoEsperaInicio)
			{
				Tempo2 = 0;
                DespachadorEventos.Despachar<IEventoActivarEscena>(new EventoActivarJuego(gameObject));
			}
		}		
		
	}
	
	public void JuegoFinalizar()
	{
		JuegoFinalizado = true;
	}
	
	public void JuegoIniciar()
	{
		JuegoIniciado = true;
	}
}
