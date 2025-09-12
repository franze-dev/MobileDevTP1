using UnityEngine;
using System.Collections;

public class Estanteria : ManejoBolsas
{
    public Cinta CintaReceptora;//cinta que debe recibir la bolsa
    public BolsaLogica.Valores Valor;
    PilaBolsaMng Contenido;
    public bool Anim = false;


    //animacion de parpadeo
    public float Intervalo = 0.7f;
    public float Permanencia = 0.2f;
    float AnimTempo = 0;
    public GameObject ModelSuelo;
    public Color32 ColorParpadeo;
    Color32 ColorOrigModel;

    //--------------------------------//	

    void Start()
    {
        Contenido = GetComponent<PilaBolsaMng>();
        ColorOrigModel = ModelSuelo.GetComponent<Renderer>().material.color;
    }

    void Update()
    {
        //animacion de parpadeo
        if (Anim)
        {
            AnimTempo += Time.deltaTime;
            if (AnimTempo > Permanencia)
            {
                if (ModelSuelo.GetComponent<Renderer>().material.color == ColorParpadeo)
                {
                    AnimTempo = 0;
                    ModelSuelo.GetComponent<Renderer>().material.color = ColorOrigModel;
                }
            }
            if (AnimTempo > Intervalo)
            {
                if (ModelSuelo.GetComponent<Renderer>().material.color == ColorOrigModel)
                {
                    AnimTempo = 0;
                    ModelSuelo.GetComponent<Renderer>().material.color = ColorParpadeo;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        ManejoBolsas recept = other.GetComponent<ManejoBolsas>();
        if (recept != null)
        {
            Dar(recept);
        }
    }

    //------------------------------------------------------------//

    public override void Dar(ManejoBolsas receptor)
    {
        if (Tenencia())
        {
            if (Controlador.GetBolsaEnMov() == null)
            {
                if (receptor.Recibir(Bolsas[0]))
                {
                    //enciende la cinta y el indicador
                    //cambia la textura de cuantos bolsa le queda
                    CintaReceptora.Encender();
                    Controlador.SalidaBolsa(Bolsas[0]);
                    Bolsas[0].GetComponent<Renderer>().enabled = true;
                    Bolsas.RemoveAt(0);
                    Contenido.Sacar();
                    ApagarAnim();
                    //Debug.Log("bolsa entregado a Mano de Estanteria");
                }
            }
        }
    }

    public override bool Recibir(BolsaLogica bolsa)
    {
        bolsa.CintaReceptora = CintaReceptora.gameObject;
        bolsa.Portador = this.gameObject;
        Contenido.Agregar();
        bolsa.GetComponent<Renderer>().enabled = false;
        return base.Recibir(bolsa);
    }

    public void ApagarAnim()
    {
        Anim = false;
        ModelSuelo.GetComponent<Renderer>().material.color = ColorOrigModel;
    }
    public void EncenderAnim()
    {
        Anim = true;
        ModelSuelo.GetComponent<Renderer>().material.color = ColorOrigModel;
    }
}
