using UnityEngine;
using System.Collections;

public class ControlDireccion : MonoBehaviour
{
    public enum TipoInput { Mouse, Kinect, AWSD, Arrows }
    public TipoInput InputAct = ControlDireccion.TipoInput.Mouse;

    public Transform ManoDer;
    public Transform ManoIzq;

    public float MaxAng = 90;
    public float DesSencibilidad = 90;

    float Giro = 0;

    public enum Sentido { Der, Izq }
    Sentido DirAct;

    public bool Habilitado = true;
    CarController CarController;

    void Start()
    {
        CarController = gameObject.GetComponent<CarController>();
    }

    void Update()
    {
        switch (InputAct)
        {
            case TipoInput.Mouse:
                if (Habilitado)
                    CarController.SetGiro(MousePos.Relation(MousePos.AxisRelation.Horizontal));

                break;

            case TipoInput.Kinect:

                if (ManoIzq.position.y > ManoDer.position.y)
                {
                    DirAct = Sentido.Der;
                }
                else
                {
                    DirAct = Sentido.Izq;
                }

                switch (DirAct)
                {
                    case Sentido.Der:
                        if (Angulo() <= MaxAng)
                            Giro = Angulo() / (MaxAng + DesSencibilidad);
                        else
                            Giro = 1;

                        if (Habilitado)
                            CarController.SetGiro(Giro);

                        break;

                    case Sentido.Izq:
                        if (Angulo() <= MaxAng)
                            Giro = (Angulo() / (MaxAng + DesSencibilidad)) * (-1);
                        else
                            Giro = (-1);

                        if (Habilitado)
                            CarController.SetGiro(Giro);

                        break;
                }
                break;
            case TipoInput.AWSD:
                if (Habilitado)
                {
                    if (Input.GetKey(KeyCode.A))
                    {
                        CarController.SetGiro(-1);
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        CarController.SetGiro(1);
                    }
                }
                break;
            case TipoInput.Arrows:
                if (Habilitado)
                {
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        CarController.SetGiro(-1);
                    }
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        CarController.SetGiro(1);
                    }
                }
                break;
        }
    }

    public float GetGiro()
    {
        return Giro;
    }

    float Angulo()
    {
        Vector2 diferencia = new Vector2(ManoDer.localPosition.x, ManoDer.localPosition.y)
                           - new Vector2(ManoIzq.localPosition.x, ManoIzq.localPosition.y);

        return Vector2.Angle(diferencia, new Vector2(1, 0));
    }

}
