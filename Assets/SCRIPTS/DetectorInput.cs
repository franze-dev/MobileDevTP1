using UnityEngine;

public class DetectorInput : MonoBehaviour
{
    public enum TipoInput
    {
        Teclado,
        Touch
    }

    public TipoInput InputAct = TipoInput.Teclado;
    public DetectorGestos DetectorGestos;

    private void Awake()
    {
        if (!DetectorGestos)
            DetectorGestos = GetComponent<DetectorGestos>();

#if UNITY_ANDROID || UNITY_IPHONE
        InputAct = TipoInput.Touch;
#endif

    }
}