using UnityEngine;
using System.Collections;

public class TaxiComp : MonoBehaviour
{
    public string FinTaxiTag = "FinTaxi";
    public string LimiteTag = "Terreno";

    public float Vel = 0;

    public Vector2 TiempCadaCuantoDobla_MaxMin = Vector2.zero;

    public float DuracionGiro = 0;
    float TempoDurGir = 0;

    public float AlcanceVerif = 0;

    public string TagTerreno = "";

    public bool Girando = false;
    Vector3 RotIni;//para saber como volver a su posicion original
    Vector3 PosIni;//para saber donde reiniciar al taxi

    float TiempEntreGiro = 0;
    float TempoEntreGiro = 0;

    public float AngDeGiro = 30;

    RaycastHit RH;

    bool Respawneando = false;


    enum Lado { Der, Izq }

    void Start()
    {
        TiempEntreGiro = (float)Random.Range(TiempCadaCuantoDobla_MaxMin.x, TiempCadaCuantoDobla_MaxMin.y);
        RotIni = this.transform.localEulerAngles;
        PosIni = transform.position;
    }

    void Update()
    {
        if (Respawneando)
        {
            if (Medicion())
                Respawn();
        }
        else
        {
            if (Girando)
            {
                TempoDurGir += Time.deltaTime;
                if (TempoDurGir > DuracionGiro)
                {
                    TempoDurGir = 0;
                    DejarDoblar();
                }
            }
            else
            {
                TempoEntreGiro += Time.deltaTime;
                if (TempoEntreGiro > TiempEntreGiro)
                {
                    TempoEntreGiro = 0;
                    Doblar();
                }
            }
        }


    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == FinTaxiTag)
        {
            transform.position = PosIni;
            transform.localEulerAngles = RotIni;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.transform.tag == LimiteTag)
        {
            Respawneando = true;
        }
    }

    void FixedUpdate()
    {
        this.transform.position += transform.forward * Time.fixedDeltaTime * Vel;
    }

    //--------------------------------------------------------------------//

    bool VerificarCostado(Lado lado)
    {
        switch (lado)
        {
            case Lado.Der:
                if (Physics.Raycast(transform.position, transform.right, out RH, AlcanceVerif))
                {
                    if (RH.transform.tag == TagTerreno)
                    {
                        return false;
                    }
                }
                break;

            case Lado.Izq:
                if (Physics.Raycast(transform.position, transform.right * (-1), out RH, AlcanceVerif))
                {
                    if (RH.transform.tag == TagTerreno)
                    {
                        return false;
                    }
                }
                break;
        }

        return true;
    }

    void Doblar()
    {
        Girando = true;
        //escoje un lado
        Lado lado;
        if ((int)Random.Range(0, 2) == 0)
        {
            lado = TaxiComp.Lado.Izq;
            //verifica, si no da cambia a derecha
            if (!VerificarCostado(lado))
                lado = TaxiComp.Lado.Der;
        }
        else
        {
            lado = TaxiComp.Lado.Der;
            //verifica, si no da cambia a izq
            if (!VerificarCostado(lado))
                lado = TaxiComp.Lado.Izq;
        }


        if (lado == TaxiComp.Lado.Der)
        {
            Vector3 vaux = transform.localEulerAngles;
            vaux.y += AngDeGiro;
            transform.localEulerAngles = vaux;
        }
        else
        {
            Vector3 vaux = transform.localEulerAngles;
            vaux.y -= AngDeGiro;
            transform.localEulerAngles = vaux;
        }
    }

    void DejarDoblar()
    {
        Girando = false;
        TiempEntreGiro = (float)Random.Range(TiempCadaCuantoDobla_MaxMin.x, TiempCadaCuantoDobla_MaxMin.y);

        transform.localEulerAngles = RotIni;
    }

    void Respawn()
    {
        Respawneando = false;

        transform.position = PosIni;
        transform.localEulerAngles = RotIni;
    }

    bool Medicion()
    {
        ProveedorServicios.IntentarObtenerServicio(out GestionDeModoDeJuego gestion);
        float dist1 = (GameManager.Instancia.Player1.transform.position - PosIni).magnitude;

        if (gestion.IsMultiplayer)
        {
            float dist2 = (GameManager.Instancia.Player2.transform.position - PosIni).magnitude;

            if (dist1 > 4 && dist2 > 4)
                return true;
            else
                return false;
        }
        else
        {
            if (dist1 > 4)
                return true;
            else
                return false ;
        }
    }
}
