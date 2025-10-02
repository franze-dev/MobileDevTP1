using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject MoverSingle;
    [SerializeField] GameObject Mover1;
    [SerializeField] GameObject Mover2;
    private GestionDeModoDeJuego gestion;

    private void Start()
    {
        ProveedorServicios.IntentarObtenerServicio(out gestion);

        Mover1.SetActive(false);
        Mover2.SetActive(false);
        MoverSingle.SetActive(false);
    }

    public void ActivarMover(int id)
    {
        if (gestion.IsMultiplayer)
        {
            if (id == 0)
                Mover1.SetActive(true);
            else
                Mover2.SetActive(true);
        }
        else
            MoverSingle.SetActive(true);
    }

    public void DesactivarMover(int id)
    {
        if (gestion.IsMultiplayer)
        {
            if (id == 0)
                Mover1.SetActive(false);
            else
                Mover2.SetActive(false);
        }
        else
            MoverSingle.SetActive(false);
    }
}
