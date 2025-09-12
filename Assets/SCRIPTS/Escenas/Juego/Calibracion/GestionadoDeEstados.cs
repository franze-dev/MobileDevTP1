using UnityEngine;
using System.Collections.Generic;

public class GestionadoDeEstados : MonoBehaviour
{
    public enum Estados { Calibrando, Jugando, Finalizado }
    public Estados EstAct = Estados.Calibrando;
}
