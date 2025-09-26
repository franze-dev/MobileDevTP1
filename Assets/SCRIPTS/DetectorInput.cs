using UnityEngine;

public class DetectorInput : MonoBehaviour
{
    public enum TipoInput
    {
        Teclado,
        Touch
    }

    public TipoInput InputAct;
    public DetectorGestos DetectorGestos;

    private void Awake()
    {
        if (!DetectorGestos)
            DetectorGestos = GetComponent<DetectorGestos>();
    }
}