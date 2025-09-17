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
    private Renderer RendererSuelo;

    void Start()
    {
        Contenido = GetComponent<PilaBolsaMng>();
        ColorOrigModel = ModelSuelo.GetComponent<Renderer>().material.color;
        RendererSuelo = ModelSuelo.GetComponent<Renderer>();
    }

    void Update()
    {
        //animacion de parpadeo
        if (Anim)
        {
            AnimTempo += Time.deltaTime;
            if (AnimTempo > Permanencia)
            {
                if (RendererSuelo.material.color == ColorParpadeo)
                {
                    AnimTempo = 0;
                    RendererSuelo.material.color = ColorOrigModel;
                }
            }
            if (AnimTempo > Intervalo)
            {
                if (RendererSuelo.material.color == ColorOrigModel)
                {
                    AnimTempo = 0;
                    RendererSuelo.material.color = ColorParpadeo;
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
                    Bolsas[0].Renderer.enabled = true;
                    Bolsas.RemoveAt(0);
                    Contenido.Sacar();
                    ApagarAnim();
                }
            }
        }
    }

    public override bool Recibir(BolsaLogica bolsa)
    {
        bolsa.CintaReceptora = CintaReceptora.gameObject;
        bolsa.Portador = this.gameObject;
        Contenido.Agregar();
        bolsa.Renderer.enabled = false;
        return base.Recibir(bolsa);
    }

    public void ApagarAnim()
    {
        Anim = false;
        RendererSuelo.material.color = ColorOrigModel;
    }
    public void EncenderAnim()
    {
        Anim = true;
        RendererSuelo.material.color = ColorOrigModel;
    }
}
