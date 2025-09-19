using UnityEngine;

public class DetectorGestos : MonoBehaviour
{
    [SerializeField] public static Rect ZonaDerecha;
    [SerializeField] public static Rect ZonaIzquierda;
    [SerializeField] public static Rect ZonaPantalla;

    public enum Direccion { Arr, Aba, Izq, Der }

    private Vector3 PosInicial;
    private Vector3 PosFinal;

    private void Awake()
    {
        ZonaPantalla = new Rect(
            0, 0, Screen.width, Screen.height
            );

        ZonaIzquierda = new Rect(
            0, 0, Screen.width / 2f, Screen.height
            );

        ZonaDerecha = new Rect(
            Screen.width / 2f, 0, Screen.width / 2, Screen.height
            );
    }

    /// <summary>
    /// Actualiza las posiciones inicial y final del toque.
    /// </summary>
    public void Actualizar()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            PosInicial = Input.GetTouch(0).position;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            PosFinal = Input.GetTouch(0).position;
    }

    /// <summary>
    /// Chequea si se ha deslizado en una dirección dada.   
    /// </summary>
    /// <param name="direccion"></param>
    /// <returns></returns>
    public bool Deslizar(Direccion direccion)
    {
        switch (direccion)
        {
            case Direccion.Arr:
                if (PosFinal.y > PosInicial.y)
                {
                    //Reiniciar();
                    return true;
                }
                break;
            case Direccion.Aba:
                if (PosFinal.y < PosInicial.y)
                {
                    //Reiniciar();
                    return true;
                }
                break;
            case Direccion.Izq:
                if (PosFinal.x < PosInicial.x)
                {
                    //Reiniciar();
                    return true;
                }
                break;
            case Direccion.Der:
                if (PosFinal.x > PosInicial.x)
                {
                    //Reiniciar();
                    return true;
                }
                break;
            default:
                return false;
        }
        return false;
    }

    /// <summary>
    /// Chequea si se ha deslizado en una dirección dada, pero solo si el gesto comenzó dentro de un área específica.
    /// </summary>
    /// <param name="direccion"></param>
    /// <param name="zona"></param>
    /// <returns></returns>
    public bool DeslizarDesde(Direccion direccion, Rect zona)
    {
        if (!Contiene(zona, PosInicial))
            return false;

        bool deslizar = Deslizar(direccion);

        if (deslizar)
            Debug.Log("Deslizar desde " + PosInicial + " en " + zona);

        return deslizar;
    }

    private bool Contiene(Rect zona, Vector2 punto)
    {
        return punto.x >= zona.xMin && punto.x <= zona.xMax &&
               punto.y >= zona.yMin && punto.y <= zona.yMax;
    }

    public void Reiniciar()
    {
        //PosInicial = Vector3.zero;
        PosFinal = Vector3.zero;
    }
}
