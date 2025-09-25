public interface IDatosEscena
{
    /// <summary>
    /// Saves the index of the scene
    /// </summary>
    static int Index { get; }
}

public class MenuSceneData : IDatosEscena
{
    public static int Index => ProveedorServicios.IntentarObtenerServicio<ControladorFlujoEscenas>(out var controller) ? controller.Contenedor.EscenaMenuIndice : 0;
}

public class GameplaySceneData : IDatosEscena
{

    public static int Index => ProveedorServicios.IntentarObtenerServicio<ControladorFlujoEscenas>(out var controller) ?
                               controller.Contenedor.EscenaJuegoIndice : 0;
}