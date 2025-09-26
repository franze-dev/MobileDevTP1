using UnityEngine;
using UnityEngine.UI;

public class PantallaCarga : MonoBehaviour
{
    [SerializeField] private Image BarraCarga;

    private void Awake()
    {
        if (BarraCarga == null)
            Debug.LogError("No se recibio la barra de carga");

        BarraCarga.fillAmount = 0;

        ProveedorServicios.DefinirServicio(this);

        ProveedorEventos.Suscribir<IEventoCarga>(Cargar);
        ProveedorEventos.Suscribir<IEventoReiniciarCarga>(Reiniciar);
    }

    private void OnDestroy()
    {
        ProveedorServicios.DefinirServicio<PantallaCarga>(null);
    }

    private void Cargar(IEventoCarga carga)
    {
        gameObject.SetActive(true);

        var progreso = Mathf.Clamp01(carga.Progreso / 0.9f);

        BarraCarga.fillAmount = progreso;
    }

    private void Reiniciar(IEventoReiniciarCarga carga)
    {
        BarraCarga.fillAmount = 0;
        gameObject.SetActive(false);
    }
}

public interface IEventoCarga : IEvento
{
    float Progreso { get; set; }
}

public class EventoCarga : IEventoCarga
{
    public float Progreso { get; set; }
    private GameObject Objeto;

    public GameObject ObjetoPadre => Objeto;

    public EventoCarga(GameObject MiObjeto, float MiProgreso)
    {
        Objeto = MiObjeto;
        Progreso = MiProgreso;
    }

}

public interface IEventoReiniciarCarga : IEvento
{
}

public class EventoReiniciarCarga : IEventoReiniciarCarga
{
    private GameObject Objeto;

    public GameObject ObjetoPadre => Objeto;

    public EventoReiniciarCarga(GameObject MiObjeto)
    {
        Objeto = MiObjeto;
    }
}