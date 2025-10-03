using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia;

    public float TiempoDeJuego = 60;

    [SerializeField] private GestionadoDeEstados GestionEstados;
    [SerializeField] private ContrCalibracion ContrCalib1;
    [SerializeField] private ContrCalibracion ContrCalib2;

    public InfoJugador PlayerInfo1 = null;
    public InfoJugador PlayerInfo2 = null;

    public Jugador Player1;
    public Jugador Player2;

    bool ConteoRegresivo = true;
    public float ConteoParaInicio = 3;
    public float TiempEspMuestraPts = 3;

    //posiciones de los camiones dependientes del lado que les toco en la pantalla
    //la pos 0 es para la izquierda y la 1 para la derecha
    public Vector3[] PosCamionesCarrera = new Vector3[2];
    //posiciones de los camiones para el tutorial

    //listas de GO que activa y desactiva por sub-escena
    //escena de calibracion
    public GameObject[] ObjsCalibracion1;
    public GameObject[] ObjsCalibracion2;
    //la pista de carreras
    public GameObject[] ObjsCarrera;
    [SerializeField] private GameObject CanvasJuego;
    [SerializeField] private List<GameObject> CanvasJugadores;
    [SerializeField] private GameObject CanvasSingle;
    [SerializeField] private DetectorInput InputDetector;

    [SerializeField] private AssetReferenceGameObject DepositosRef;
    [SerializeField] private Transform AssetPos;
    [SerializeField] private GameObject InstanciaDeposito;

    private GestionDeModoDeJuego Modos;
    private GestionDeTeclas Teclas;
    private GestionDeGestos Gestos;
    private ComandoArriba InfoComando1;
    private ComandoArriba InfoComando2;

    void Awake()
    {
        Application.runInBackground = true;

        Instancia = this;

        #region Debug
        if (InputDetector == null)
            Debug.LogError("Falta el Input Detector en " + gameObject.name);

        if (GestionEstados == null)
            Debug.LogError("Falta el GestionadoDeEstados en " + gameObject.name);

        if (CanvasJuego == null)
            Debug.LogError("Falta el Canvas de Juego en " + gameObject.name);

        #endregion

        CanvasJuego?.SetActive(false);
        CanvasSingle?.SetActive(false);
    }

    private void OnDestroy()
    {
        DepositosRef.ReleaseInstance(InstanciaDeposito);
        DespachadorEventos.Despachar<IFinJuegoEvento>(new FinJuegoEvento(gameObject));
    }

    void Start()
    {
        ProveedorServicios.IntentarObtenerServicio(out Modos);

        if (!Modos.IsMultiplayer)
            CanvasSingle.SetActive(true);

        Teclas = new GestionDeTeclas();
        Gestos = new GestionDeGestos(InputDetector.DetectorGestos);

        if (InputDetector.InputAct == DetectorInput.TipoInput.Teclado)
        {
            if (Modos.IsMultiplayer)
            {
                InfoComando2 = Teclas.Conectar(KeyCode.UpArrow,
                              new ComandoArriba(this, PlayerInfo2, Player2, Visualizacion.Lado.Der));
                InfoComando1 = Teclas.Conectar(KeyCode.W,
                              new ComandoArriba(this, PlayerInfo1, Player1, Visualizacion.Lado.Izq));
            }
            else
                InfoComando1 = Teclas.Conectar(KeyCode.W,
                              new ComandoArriba(this, PlayerInfo1, Player1, Visualizacion.Lado.Non));
        }
        else
        {
            if (Modos.IsMultiplayer)
            {
                InfoComando1 = Gestos.Conectar(DetectorGestos.Direccion.Arr, DetectorGestos.ZonaIzquierda, 
                              new ComandoArriba(this, PlayerInfo1, Player1, Visualizacion.Lado.Izq));
                InfoComando2 = Gestos.Conectar(DetectorGestos.Direccion.Arr, DetectorGestos.ZonaDerecha, 
                              new ComandoArriba(this, PlayerInfo2, Player2, Visualizacion.Lado.Der));
            }
            else
                InfoComando1 = Gestos.Conectar(DetectorGestos.Direccion.Arr, DetectorGestos.ZonaPantalla, 
                              new ComandoArriba(this, PlayerInfo1, Player1, Visualizacion.Lado.Non));
        }

        IniciarCalibracion();
    }

    void Update()
    {
        if (InputDetector.InputAct == DetectorInput.TipoInput.Touch)
            InputDetector.DetectorGestos.Actualizar();

        switch (GestionEstados.EstAct)
        {
            case GestionadoDeEstados.Estados.Calibrando:

                CanvasJuego.SetActive(false);
                CanvasSingle.SetActive(false);

                foreach (var canvas in CanvasJugadores)
                    canvas.SetActive(false);

                if (InputDetector.InputAct == DetectorInput.TipoInput.Teclado)
                    Teclas.EjecutarInputsUnaVez();
                else
                    Gestos.EjecutarInputs();

                PlayerInfo1 = InfoComando1.Info;
                PlayerInfo2 = InfoComando2.Info;

                if (Modos.IsMultiplayer)
                {
                    if (PlayerInfo1.PJ != null && PlayerInfo2.PJ != null)
                    {
                        if (PlayerInfo1.FinTuto2 && PlayerInfo2.FinTuto2)
                        {
                            EmpezarCarrera();
                        }
                    }
                }
                else if (PlayerInfo1.PJ != null && PlayerInfo1.FinTuto2)
                    EmpezarCarrera();

                break;
            case GestionadoDeEstados.Estados.Jugando:

                if (!CanvasJuego.activeInHierarchy)
                    CanvasJuego.SetActive(true);

                if (!Modos.IsMultiplayer)
                {
                    foreach (var canvas in CanvasJugadores)
                    {
                        if (!canvas.activeInHierarchy)
                            canvas.SetActive(false);
                    }
                    if (!CanvasSingle.activeInHierarchy)
                        CanvasSingle.SetActive(true);
                }
                else
                {
                    if (!CanvasSingle.activeInHierarchy)
                        CanvasSingle.SetActive(false);
                    foreach (var canvas in CanvasJugadores)
                        if (!canvas.activeInHierarchy)
                            canvas.SetActive(true);
                }

                Player1?.ChequearDescarga();

                if (Modos.IsMultiplayer)
                    Player2?.ChequearDescarga();

                if (TiempoDeJuego <= 0)
                {
                    FinalizarCarrera();
                }

                if (ConteoRegresivo)
                {
                    ConteoParaInicio -= Time.deltaTime;
                    if (ConteoParaInicio < 0)
                    {
                        EmpezarCarrera();
                        ConteoRegresivo = false;
                    }
                }
                else
                {
                    //baja el tiempo del juego
                    TiempoDeJuego -= Time.deltaTime;
                }

                break;
            case GestionadoDeEstados.Estados.Finalizado:

                if (CanvasJuego.activeInHierarchy)
                    CanvasJuego.SetActive(false);

                if (CanvasSingle.activeInHierarchy)
                    CanvasSingle.SetActive(false);

                foreach (var canvas in CanvasJugadores)
                {
                    if (canvas.activeInHierarchy)
                        canvas.SetActive(false);
                }

                TiempEspMuestraPts -= Time.deltaTime;
                if (TiempEspMuestraPts <= 0)
                    DespachadorEventos.Despachar<IEventoActivarEscena>(new EventoActivarFinal(gameObject));

                break;
            default:
                break;
        }
    }

    public void InitInfo(out InfoJugador PlayerInfo, Jugador Jugador, Visualizacion.Lado lado)
    {
        PlayerInfo = new InfoJugador(0, Jugador);
        PlayerInfo.LadoAct = lado;
        SetPosicion(PlayerInfo);
    }

    public void IniciarCalibracion()
    {
        for (int i = 0; i < ObjsCalibracion1.Length; i++)
        {
            ObjsCalibracion1[i].SetActive(true);
            ObjsCalibracion2[i].SetActive(true);
        }

        for (int i = 0; i < ObjsCarrera.Length; i++)
        {
            ObjsCarrera[i].SetActive(false);
        }


        Player1?.CambiarACalibracion();
        if (Modos.IsMultiplayer)
            Player2?.CambiarACalibracion();
    }

    void EmpezarCarrera()
    {
        Player1.Frenado.RestaurarVel();
        Player1.Direccion.Habilitado = true;

        if (Player2 != null && Modos.IsMultiplayer)
        {
            Player2.Frenado.RestaurarVel();
            Player2.Direccion.Habilitado = true;
        }
    }

    void FinalizarCarrera()
    {
        GestionEstados.EstAct = GestionadoDeEstados.Estados.Finalizado;

        TiempoDeJuego = 0;

        if (Modos.IsMultiplayer)
        {
            if (Player1.Dinero > Player2.Dinero)
            {
                //lado que gano
                if (PlayerInfo1.LadoAct == Visualizacion.Lado.Der)
                    DatosPartida.LadoGanadaor = DatosPartida.Lados.Der;
                else
                    DatosPartida.LadoGanadaor = DatosPartida.Lados.Izq;

                //puntajes
                DatosPartida.PtsGanador = Player1.Dinero;
                DatosPartida.PtsPerdedor = Player2.Dinero;
            }
            else
            {
                //lado que gano
                if (PlayerInfo2.LadoAct == Visualizacion.Lado.Der)
                    DatosPartida.LadoGanadaor = DatosPartida.Lados.Der;
                else
                    DatosPartida.LadoGanadaor = DatosPartida.Lados.Izq;

                //puntajes
                DatosPartida.PtsGanador = Player2.Dinero;
                DatosPartida.PtsPerdedor = Player1.Dinero;
            }
        }
        else
        {
            DatosPartida.LadoGanadaor = DatosPartida.Lados.Non;
            DatosPartida.PtsGanador = Player1.Dinero;
            DatosPartida.PtsPerdedor = 0;
        }

        Player1?.Frenado.Frenar();
        Player1?.ContrDesc?.FinDelJuego();

        if (Modos.IsMultiplayer)
        {
            Player2?.Frenado.Frenar();
            Player2?.ContrDesc?.FinDelJuego();
        }
    }

    //se encarga de posicionar la camara derecha para el jugador que esta a la derecha y viseversa
    void SetPosicion(InfoJugador pjInf)
    {

        if (!Modos)
            return;

        pjInf.PJ.MiVisualizacion.SetLado(pjInf.LadoAct);
        pjInf.PJ.ContrCalib.IniciarTesteo();


        if (!Modos.IsMultiplayer)
        {
            Player1.MiVisualizacion.SetLado(Visualizacion.Lado.Non);
            Player2 = null;
        }
        else
        {
            if (pjInf.PJ == Player1)
            {
                if (pjInf.LadoAct == Visualizacion.Lado.Izq)
                    Player2.MiVisualizacion.SetLado(Visualizacion.Lado.Der);
                else
                    Player2.MiVisualizacion.SetLado(Visualizacion.Lado.Izq);
            }
            else
            {
                if (pjInf.LadoAct == Visualizacion.Lado.Izq)
                    Player1.MiVisualizacion.SetLado(Visualizacion.Lado.Der);
                else
                    Player1.MiVisualizacion.SetLado(Visualizacion.Lado.Izq);
            }
        }
    }

    void CambiarACarrera()
    {
        StartCoroutine(InstanciarAsset());

        for (int i = 0; i < ObjsCarrera.Length; i++)
            ObjsCarrera[i].SetActive(true);

        //desactivacion de la calibracion
        PlayerInfo1.FinCalibrado = true;

        //posiciona los camiones dependiendo de que lado de la pantalla esten

        if (Modos.IsMultiplayer)
        {
            if (PlayerInfo1.LadoAct == Visualizacion.Lado.Izq)
            {
                Player1.gameObject.transform.position = PosCamionesCarrera[0];
                Player2.gameObject.transform.position = PosCamionesCarrera[1];
            }
            else if (PlayerInfo1.LadoAct == Visualizacion.Lado.Der)
            {
                Player1.gameObject.transform.position = PosCamionesCarrera[1];
                Player2.gameObject.transform.position = PosCamionesCarrera[0];
            }
        }
        else
            Player1.gameObject.transform.position = PosCamionesCarrera[0];

        if (Modos.IsMultiplayer)
        {
            Player2.transform.forward = Vector3.forward;
            Player2?.Frenado.Frenar();
            Player2?.CambiarAConduccion();
            Player2?.Frenado.RestaurarVel();
            Player2.Direccion.Habilitado = false;
            Player2.transform.forward = Vector3.forward;
        }

        Player1.transform.forward = Vector3.forward;
        Player1?.Frenado.Frenar();
        Player1?.CambiarAConduccion();
        Player1?.Frenado.RestaurarVel();
        Player1.Direccion.Habilitado = false;
        Player1.transform.forward = Vector3.forward;

        GestionEstados.EstAct = GestionadoDeEstados.Estados.Jugando;
    }

    private IEnumerator InstanciarAsset()
    {
        var operacion = DepositosRef.InstantiateAsync(AssetPos);

        ProveedorServicios.IntentarObtenerServicio<ControladorFlujoEscenas>(out var cargador);

        if (!cargador)
            yield return operacion;
        else
            yield return cargador.UsarCarga(operacion);

        InstanciaDeposito = operacion.Result;
    }

    public void FinCalibracion(int playerID)
    {
        if (Modos.IsMultiplayer)
        {
            if (playerID == 0)
            {
                PlayerInfo1.FinTuto1 = true;

            }
            else if (playerID == 1)
            {
                PlayerInfo2.FinTuto1 = true;
            }

            if (PlayerInfo1.PJ != null && PlayerInfo2.PJ != null)
                if (PlayerInfo1.FinTuto1 && PlayerInfo2.FinTuto1)
                    CambiarACarrera();
        }
        else
            if (PlayerInfo1.PJ != null)
            CambiarACarrera();
    }

    public Rect ZonaCorrespondeA(int camionId)
    {
        if (Modos == null)
            ProveedorServicios.IntentarObtenerServicio(out Modos);

        if (Modos.IsMultiplayer)
        {
            if (camionId == 0)
                return DetectorGestos.ZonaIzquierda;

            return DetectorGestos.ZonaDerecha;
        }

        return DetectorGestos.ZonaPantalla;
    }

    [System.Serializable]
    public class InfoJugador
    {
        public InfoJugador(int tipoDeInput, Jugador pj)
        {
            TipoDeInput = tipoDeInput;
            PJ = pj;
        }

        public bool FinCalibrado = false;
        public bool FinTuto1 = false;
        public bool FinTuto2 = false;

        public Visualizacion.Lado LadoAct;

        public int TipoDeInput = -1;

        public Jugador PJ;
    }

}

public class ComandoArriba : IInputComando
{
    public GameManager GM;
    public GameManager.InfoJugador Info;
    public Jugador Jugador;
    public Visualizacion.Lado Lado;

    public ComandoArriba(GameManager gm, GameManager.InfoJugador info, Jugador jugador, Visualizacion.Lado lado)
    {
        GM = gm;
        Info = info;
        Jugador = jugador;
        Lado = lado;
    }

    public void Execute()
    {
        if (Info.PJ == null)
            GM.InitInfo(out Info, Jugador, Lado);
    }
}

public class FinJuegoEvento : IFinJuegoEvento
{
    private GameObject objeto;
    public GameObject ObjetoPadre => objeto;

    public FinJuegoEvento(GameObject objeto)
    {
        this.objeto = objeto;
    }
}

public interface IFinJuegoEvento : IEvento
{
}