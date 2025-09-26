using UnityEngine;

public class GestionadoDeMenues : MonoBehaviour
{
    public GameObject MenuPrincipalObjeto;
    public GameObject MenuPausaObjeto;
    public GameObject MenuModosObjeto;
    public GameObject MenuCreditosObjeto;
    public GameObject MenuSalirObjeto;
    public GameObject FondoObjeto;

    public IEstadoMenu EstadoActual = null;
    public IEstadoMenu EstadoAnterior = null;

    private void Awake()
    {
        ProveedorServicios.DefinirServicio(this);
    }

    private void Start()
    {
        EsconderObjetos();

        if (EstadoActual == null)
            TransicionA(new EstadoMenuPrincipal());
    }

    public void TransicionA(IEstadoMenu estado)
    {
        if (EstadoActual == estado)
            return;


        EstadoAnterior = EstadoActual;
        EstadoActual = estado;
        EsconderObjetos();
        FondoObjeto.SetActive(true);
        EstadoActual.Entrar(this);
    }

    public void EsconderObjetos()
    {
        MenuPrincipalObjeto.SetActive(false);
        MenuPausaObjeto.SetActive(false);
        MenuModosObjeto.SetActive(false);
        MenuCreditosObjeto.SetActive(false);
        MenuSalirObjeto.SetActive(false);
        FondoObjeto.SetActive(false);
    }

    public void MostrarObjeto(GameObject objeto)
    {
        EsconderObjetos();
        FondoObjeto.SetActive(true);
        objeto.SetActive(true);
    }

    public static void ResetGame()
    {
        GestionadoDePausa.Pausado = false;

        ProveedorServicios.IntentarObtenerServicio<ControladorFlujoEscenas>(out var controlador);
        controlador.DescargarEscena(DatosEscenaJuego.Index);
    }
}

public interface IEstadoMenu
{
    void Entrar(GestionadoDeMenues gestion);
}

public class EstadoMenuPrincipal : IEstadoMenu
{
    public void Entrar(GestionadoDeMenues gestion)
    {
        GestionadoDeMenues.ResetGame();

        gestion.MostrarObjeto(gestion.MenuPrincipalObjeto);
    }
}

public class EstadoPausa : IEstadoMenu
{
    public void Entrar(GestionadoDeMenues gestion)
    {
        GestionadoDePausa.Pausado = true;

        gestion.MostrarObjeto(gestion.MenuPrincipalObjeto);
    }
}

public class EstadoCreditos : IEstadoMenu
{
    public void Entrar(GestionadoDeMenues gestion)
    {
        gestion.MostrarObjeto(gestion.MenuCreditosObjeto);
    }
}

public class EstadoSalir : IEstadoMenu
{
    public void Entrar(GestionadoDeMenues gestion)
    {
        gestion.MostrarObjeto(gestion.MenuSalirObjeto);
    }
}

public class EstadoModos : IEstadoMenu
{
    public void Entrar(GestionadoDeMenues gestion)
    {
        gestion.MostrarObjeto(gestion.MenuModosObjeto);
    }
}