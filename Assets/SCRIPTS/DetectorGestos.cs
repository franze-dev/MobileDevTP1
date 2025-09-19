using UnityEngine;

public class DetectorGestos : MonoBehaviour
{
    public enum Direccion { Arr, Aba, Izq, Der }

    public Vector3 posInicial;
    public Vector3 posFinal;

    public void Actualizar()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            posInicial = Input.GetTouch(0).position;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            posFinal = Input.GetTouch(0).position;
    }

    public bool Swipe(Direccion direccion)
    {
        switch (direccion)
        {
            case Direccion.Arr:
                if (posFinal.y > posInicial.y)
                    return true;
                break;
            case Direccion.Aba:
                if (posFinal.y < posInicial.y)
                    return true;
                break;
            case Direccion.Izq:
                if (posFinal.x < posInicial.x)
                    return true;
                break;
            case Direccion.Der:
                if (posFinal.x > posInicial.x)
                    return true;
                break;
            default:
                return false;
        }

        return false;
    }
}
