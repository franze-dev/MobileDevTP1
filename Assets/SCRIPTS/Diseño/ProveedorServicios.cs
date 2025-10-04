using System;
using System.Collections.Generic;

public static class ProveedorServicios
{
    private static readonly Dictionary<Type, object> Servicios = new();

    public static void DefinirServicio<T>(T servicio, bool sobreescribir = false)
    {
        if (!Servicios.TryAdd(typeof(T), servicio) && sobreescribir)
            Servicios[typeof(T)] = servicio;
    }
    
    public static bool IntentarObtenerServicio<T>(out T servicio) where T : class
    {
        if (Servicios.TryGetValue(typeof(T), out var miServicio)
            && miServicio is T tServicio)
        {
            servicio = tServicio;
            return true;
        }

        servicio = null;
        return false;
    }
}
