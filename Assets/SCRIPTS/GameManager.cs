using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

    bool ConteoRedresivo = true;
    public Rect ConteoPosEsc;
    public float ConteoParaInicion = 3;
    public GUISkin GS_ConteoInicio;

    public Rect TiempoGUI = new Rect();
    public GUISkin GS_TiempoGUI;

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

    void Awake()
    {
        Instancia = this;

        if (GestionEstados == null)
            Debug.LogError("Falta el GestionadoDeEstados en " + gameObject.name);

        if (CanvasJuego == null)
            Debug.LogError("Falta el Canvas de Juego en " + gameObject.name);

        CanvasJuego?.SetActive(false);
    }

    void Start()
    {
        IniciarCalibracion();
    }

    void Update()
    {
        //REINICIAR
        if (Input.GetKey(KeyCode.Mouse1) &&
           Input.GetKey(KeyCode.Keypad0))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        //CIERRA LA APLICACION
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        switch (GestionEstados.EstAct)
        {
            case GestionadoDeEstados.Estados.Calibrando:

                CanvasJuego.SetActive(false);

                //SKIP EL TUTORIAL
                if (Input.GetKey(KeyCode.Mouse0) &&
                       Input.GetKey(KeyCode.Keypad0))
                {
                    if (PlayerInfo1 != null && PlayerInfo2 != null)
                    {
                        FinCalibracion(0);
                        FinCalibracion(1);
                    }
                }

                if (PlayerInfo1.PJ == null && Input.GetKeyDown(KeyCode.W))
                {
                    PlayerInfo1 = new InfoJugador(0, Player1);
                    PlayerInfo1.LadoAct = Visualizacion.Lado.Izq;
                    SetPosicion(PlayerInfo1);
                }

                if (PlayerInfo2.PJ == null && Input.GetKeyDown(KeyCode.UpArrow))
                {
                    PlayerInfo2 = new InfoJugador(1, Player2);
                    PlayerInfo2.LadoAct = Visualizacion.Lado.Der;
                    SetPosicion(PlayerInfo2);
                }

                //cuando los 2 pj terminaron los tutoriales empiesa la carrera
                if (PlayerInfo1.PJ != null && PlayerInfo2.PJ != null)
                {
                    if (PlayerInfo1.FinTuto2 && PlayerInfo2.FinTuto2)
                    {
                        EmpezarCarrera();
                    }
                }

                break;
            case GestionadoDeEstados.Estados.Jugando:

                CanvasJuego.SetActive(true);
                Player1.ChequearDescarga();
                Player2.ChequearDescarga();

                //SKIP LA CARRERA
                if (Input.GetKey(KeyCode.Mouse1) &&
                   Input.GetKey(KeyCode.Keypad0))
                {
                    TiempoDeJuego = 0;
                }

                if (TiempoDeJuego <= 0)
                {
                    FinalizarCarrera();
                }

                if (ConteoRedresivo)
                {
                    ConteoParaInicion -= Time.deltaTime;
                    if (ConteoParaInicion < 0)
                    {
                        EmpezarCarrera();
                        ConteoRedresivo = false;
                    }
                }
                else
                {
                    //baja el tiempo del juego
                    TiempoDeJuego -= Time.deltaTime;
                }
                break;
            case GestionadoDeEstados.Estados.Finalizado:
                CanvasJuego.SetActive(false);
                TiempEspMuestraPts -= Time.deltaTime;
                if (TiempEspMuestraPts <= 0)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

                break;
            default:
                break;
        }
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


        Player1.CambiarACalibracion();
        Player2.CambiarACalibracion();
    }
    void EmpezarCarrera()
    {
        Player1.Frenado.RestaurarVel();
        Player1.Direccion.Habilitado = true;

        Player2.Frenado.RestaurarVel();
        Player2.Direccion.Habilitado = true;
    }

    void FinalizarCarrera()
    {
        GestionEstados.EstAct = GestionadoDeEstados.Estados.Finalizado;

        TiempoDeJuego = 0;

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

        Player1?.Frenado.Frenar();
        Player2?.Frenado.Frenar();

        Player1?.ContrDesc?.FinDelJuego();
        Player2?.ContrDesc?.FinDelJuego();
    }

    //se encarga de posicionar la camara derecha para el jugador que esta a la derecha y viseversa
    void SetPosicion(InfoJugador pjInf)
    {
        pjInf.PJ.MiVisualizacion.SetLado(pjInf.LadoAct);
        //en este momento, solo la primera vez, deberia setear la otra camara asi no se superponen
        pjInf.PJ.ContrCalib.IniciarTesteo();


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

    void CambiarACarrera()
    {
        for (int i = 0; i < ObjsCarrera.Length; i++)
            ObjsCarrera[i].SetActive(true);

        //desactivacion de la calibracion
        PlayerInfo1.FinCalibrado = true;

        //posiciona los camiones dependiendo de que lado de la pantalla esten
        if (PlayerInfo1.LadoAct == Visualizacion.Lado.Izq)
        {
            Player1.gameObject.transform.position = PosCamionesCarrera[0];
            Player2.gameObject.transform.position = PosCamionesCarrera[1];
        }
        else
        {
            Player1.gameObject.transform.position = PosCamionesCarrera[1];
            Player2.gameObject.transform.position = PosCamionesCarrera[0];
        }

        Player1.transform.forward = Vector3.forward;
        Player1.Frenado.Frenar();
        Player1.CambiarAConduccion();

        Player2.transform.forward = Vector3.forward;
        Player2.Frenado.Frenar();
        Player2.CambiarAConduccion();

        //los deja andando
        Player1.Frenado.RestaurarVel();
        Player2.Frenado.RestaurarVel();
        //cancela la direccion
        Player1.Direccion.Habilitado = false;
        Player2.Direccion.Habilitado = false;
        //les de direccion
        Player1.transform.forward = Vector3.forward;
        Player2.transform.forward = Vector3.forward;

        GestionEstados.EstAct = GestionadoDeEstados.Estados.Jugando;
    }

    public void FinCalibracion(int playerID)
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
