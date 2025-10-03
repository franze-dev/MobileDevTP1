
using System.Collections.Generic;
using UnityEngine;

public interface IComando
{
    void Execute();
}

public interface IInputComando : IComando
{

}

public class GestionDeTeclas
{
    private Dictionary<KeyCode, IInputComando> ConectorTeclas = new Dictionary<KeyCode, IInputComando>();

    public T Conectar<T>(KeyCode tecla, T inputComando) where T : IInputComando
    {
        ConectorTeclas[tecla] = inputComando;
        return inputComando;
    }

    public void EjecutarInputsUnaVez()
    {
        foreach (var comando in ConectorTeclas)
            if (Input.GetKeyDown(comando.Key))
                comando.Value.Execute();
    }

    public void EjecutarInputs()
    {
        foreach (var comando in ConectorTeclas)
            if (Input.GetKey(comando.Key))
                comando.Value.Execute();
    }
}

public class GestionDeGestos
{
    struct Gesto
    {
        public DetectorGestos.Direccion Dir;
        public Rect Zona;
        public IInputComando Comando;

        public Gesto(DetectorGestos.Direccion dir, Rect zona, IInputComando comando)
        {
            this.Dir = dir;
            this.Zona = zona;
            this.Comando = comando;
        }
    }

    private List<Gesto> ConectorGestos = new List<Gesto>();
    private DetectorGestos Detector;

    public GestionDeGestos(DetectorGestos detector)
    {
        Detector = detector;
    }

    public T Conectar<T>(DetectorGestos.Direccion dir, Rect zona, T inputComando) where T : IInputComando 
    {
        ConectorGestos.Add(new(dir, zona, inputComando));
        return inputComando;
    }

    public void EjecutarInputs()
    {
        foreach (var gesto in ConectorGestos)
            if (Detector.DeslizarDesde(gesto.Dir, gesto.Zona))
                gesto.Comando.Execute();
    }
}