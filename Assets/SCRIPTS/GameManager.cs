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

    public PlayerInfo PlayerInfo1 = null;
    public PlayerInfo PlayerInfo2 = null;

    public Player Player1;
    public Player Player2;

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
    public Vector3 PosCamion1Tuto = Vector3.zero;
    public Vector3 PosCamion2Tuto = Vector3.zero;

    //listas de GO que activa y desactiva por sub-escena
    //escena de calibracion
    public GameObject[] ObjsCalibracion1;
    public GameObject[] ObjsCalibracion2;
    //escena de tutorial
    public GameObject[] ObjsTuto1;
    public GameObject[] ObjsTuto2;
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

        if (GestionEstados.EstAct == GestionadoDeEstados.Estados.Jugando)
            CanvasJuego.SetActive(true);
        else
            CanvasJuego.SetActive(false);

        switch (GestionEstados.EstAct)
        {
            case GestionadoDeEstados.Estados.Calibrando:

                //SKIP EL TUTORIAL
                    if (Input.GetKey(KeyCode.Mouse0) &&
                       Input.GetKey(KeyCode.Keypad0))
                    {
                        if (PlayerInfo1 != null && PlayerInfo2 != null)
                        {
                            FinCalibracion(0);
                            FinCalibracion(1);

                            FinTutorial(0);
                            FinTutorial(1);
                        }
                    }

                if (PlayerInfo1.PJ == null && Input.GetKeyDown(KeyCode.W))
                {
                    PlayerInfo1 = new PlayerInfo(0, Player1);
                    PlayerInfo1.LadoAct = Visualizacion.Lado.Izq;
                    SetPosicion(PlayerInfo1);
                }

                if (PlayerInfo2.PJ == null && Input.GetKeyDown(KeyCode.UpArrow))
                {
                    PlayerInfo2 = new PlayerInfo(1, Player2);
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

        for (int i = 0; i < ObjsTuto2.Length; i++)
        {
            ObjsTuto2[i].SetActive(false);
            ObjsTuto1[i].SetActive(false);
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
        Player1.GetComponent<ControlDireccion>().Habilitado = true;

        Player2.GetComponent<Frenado>().RestaurarVel();
        Player2.GetComponent<ControlDireccion>().Habilitado = true;
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

        Player1?.GetComponent<Frenado>().Frenar();
        Player2?.GetComponent<Frenado>().Frenar();

        Player1?.ContrDesc?.FinDelJuego();
        Player2?.ContrDesc?.FinDelJuego();
    }

    //se encarga de posicionar la camara derecha para el jugador que esta a la derecha y viseversa
    void SetPosicion(PlayerInfo pjInf)
    {
        pjInf.PJ.GetComponent<Visualizacion>().SetLado(pjInf.LadoAct);
        //en este momento, solo la primera vez, deberia setear la otra camara asi no se superponen
        pjInf.PJ.ContrCalib.IniciarTesteo();


        if (pjInf.PJ == Player1)
        {
            if (pjInf.LadoAct == Visualizacion.Lado.Izq)
                Player2.GetComponent<Visualizacion>().SetLado(Visualizacion.Lado.Der);
            else
                Player2.GetComponent<Visualizacion>().SetLado(Visualizacion.Lado.Izq);
        }
        else
        {
            if (pjInf.LadoAct == Visualizacion.Lado.Izq)
                Player1.GetComponent<Visualizacion>().SetLado(Visualizacion.Lado.Der);
            else
                Player1.GetComponent<Visualizacion>().SetLado(Visualizacion.Lado.Izq);
        }

    }

    void CambiarACarrera()
    {
        for (int i = 0; i < ObjsCarrera.Length; i++)
            ObjsCarrera[i].SetActive(true);

        //desactivacion de la calibracion
        PlayerInfo1.FinCalibrado = true;

        for (int i = 0; i < ObjsTuto1.Length; i++)
            ObjsTuto1[i].SetActive(true);

        for (int i = 0; i < ObjsCalibracion1.Length; i++)
            ObjsCalibracion1[i].SetActive(false);

        PlayerInfo2.FinCalibrado = true;

        for (int i = 0; i < ObjsCalibracion2.Length; i++)
            ObjsCalibracion2[i].SetActive(false);

        for (int i = 0; i < ObjsTuto2.Length; i++)
            ObjsTuto2[i].SetActive(true);

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
        Player1.GetComponent<Frenado>().Frenar();
        Player1.CambiarAConduccion();

        Player2.transform.forward = Vector3.forward;
        Player2.GetComponent<Frenado>().Frenar();
        Player2.CambiarAConduccion();

        //los deja andando
        Player1.GetComponent<Frenado>().RestaurarVel();
        Player2.GetComponent<Frenado>().RestaurarVel();
        //cancela la direccion
        Player1.GetComponent<ControlDireccion>().Habilitado = false;
        Player2.GetComponent<ControlDireccion>().Habilitado = false;
        //les de direccion
        Player1.transform.forward = Vector3.forward;
        Player2.transform.forward = Vector3.forward;

        GestionEstados.EstAct = GestionadoDeEstados.Estados.Jugando;
    }

    public void FinTutorial(int playerID)
    {
        if (playerID == 0)
        {
            PlayerInfo1.FinTuto2 = true;

        }
        else if (playerID == 1)
        {
            PlayerInfo2.FinTuto2 = true;
        }

        if (PlayerInfo1.FinTuto2 && PlayerInfo2.FinTuto2)
        {
            CambiarACarrera();
        }
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
    public class PlayerInfo
    {
        public PlayerInfo(int tipoDeInput, Player pj)
        {
            TipoDeInput = tipoDeInput;
            PJ = pj;
        }

        public bool FinCalibrado = false;
        public bool FinTuto1 = false;
        public bool FinTuto2 = false;

        public Visualizacion.Lado LadoAct;

        public int TipoDeInput = -1;

        public Player PJ;
    }

}
