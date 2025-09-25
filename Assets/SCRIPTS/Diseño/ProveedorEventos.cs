using System;
using System.Collections.Generic;

public static class ProveedorEventos
{
    private static Dictionary<Type, Delegate> AccionesEvento = new();
    public static Dictionary<Type, Delegate> Eventos { get { return AccionesEvento; } }

    public static void Suscribir<T>(Action<T> accion) where T : IEvento
    {
        if (AccionesEvento.TryGetValue(typeof(T), out var accionExistente))
            AccionesEvento[typeof(T)] = Delegate.Combine(accionExistente, accion);
        else
            AccionesEvento[typeof(T)] = accion;
    }

    public static void Desuscribir<T>(Action<T> accion) where T : IEvento
    {
        if (AccionesEvento.TryGetValue(typeof(T), out var accionExistente))
        {
            var nuevaAccion = Delegate.Remove(accionExistente, accion);

            if (nuevaAccion == null)
                AccionesEvento.Remove(typeof(T));
            else
                AccionesEvento[typeof(T)] = nuevaAccion;
        }
    }

}
