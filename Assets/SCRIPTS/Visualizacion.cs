#define USB_SINGLETON
using UnityEngine;

/// <summary>
/// clase encargada de TODA la visualizacion
/// de cada player, todo aquello que corresconda a 
/// cada seccion de la pantalla independientemente
/// </summary>
public class Visualizacion : MonoBehaviour
{
    public enum Lado { Izq, Der }
    public Lado LadoAct;

    Jugador Pj;

    //las distintas camaras
    public Camera CamCalibracion;
    public Camera CamConduccion;
    public Camera CamDescarga;

    //NUMERO DEL JUGADOR
    public GameObject Techo;
    private Renderer TechoRend;

    Rect R;

    void Start()
    {
        Pj = GetComponent<Jugador>();
        TechoRend = Techo.GetComponent<Renderer>();
    }

    public void CambiarACalibracion()
    {
        CamCalibracion.enabled = true;
        CamConduccion.enabled = false;
        CamDescarga.enabled = false;
    }

    public void CambiarATutorial()
    {
        CamCalibracion.enabled = false;
        CamConduccion.enabled = true;
        CamDescarga.enabled = false;
    }

    public void CambiarAConduccion()
    {
        CamCalibracion.enabled = false;
        CamConduccion.enabled = true;
        CamDescarga.enabled = false;
    }

    public void CambiarADescarga()
    {
        CamCalibracion.enabled = false;
        CamConduccion.enabled = false;
        CamDescarga.enabled = true;
    }

    public void SetLado(Lado lado)
    {
        LadoAct = lado;

        Rect r = new Rect();
        r.width = CamConduccion.rect.width;
        r.height = CamConduccion.rect.height;
        r.y = CamConduccion.rect.y;

        switch (lado)
        {
            case Lado.Der:
                r.x = 0.5f;
                break;


            case Lado.Izq:
                r.x = 0;
                break;
        }

        CamCalibracion.rect = r;
        CamConduccion.rect = r;
        CamDescarga.rect = r;

        if (TechoRend == null)
            TechoRend = Techo.GetComponent<Renderer>();
    }

    public string PrepararNumeros(int dinero)
    {
        string strDinero = dinero.ToString();
        string res = "";

        if (dinero < 1)//sin ditero
            res = "";
        else if (strDinero.Length == 6)//cientos de miles
        {
            for (int i = 0; i < strDinero.Length; i++)
            {
                res += strDinero[i];

                if (i == 2)
                    res += ".";
            }
        }
        else if (strDinero.Length == 7)//millones
        {
            for (int i = 0; i < strDinero.Length; i++)
            {
                res += strDinero[i];

                if (i == 0 || i == 3)
                    res += ".";
            }
        }

        return res;
    }



}
