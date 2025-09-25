using System;

public static class DespachadorEventos
{
    public static void Despachar<T>(T evento) where T : IEvento
    {
        if (ProveedorEventos.Eventos.TryGetValue(typeof(T), out var accion))
            (accion as Action<T>)?.Invoke(evento);
    }
}
