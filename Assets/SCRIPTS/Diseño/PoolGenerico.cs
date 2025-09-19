using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolGenerico", menuName = "ScriptableObjects/PoolGenerico")]
public class PoolGenerico : ScriptableObject
{
    [SerializeField] List<GameObject> prefabs;
    [SerializeField] int cantidadInicial = 10;

    Dictionary<string, List<GameObject>> _pool = new Dictionary<string, List<GameObject>>();

    private void Awake()
    {
        foreach (var prefab in prefabs)
            CrearPool(prefab);
    }

    private void OnDestroy()
    {
        foreach (var lista in _pool.Values)
            foreach (var obj in lista)
                if (obj != null)
                    Destroy(obj);
    }

    public void CrearPool(GameObject prefab)
    {
        CrearPool(prefab, cantidadInicial);
    }

    public void CrearPool(GameObject prefab, int cantidad)
    {
        if (_pool.ContainsKey(prefab.name))
            Debug.LogWarning(prefab.name + " ya tiene una pool");

        List<GameObject> lista = new List<GameObject>();

        for (int i = 0; i < cantidad; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            lista.Add(obj);
        }

        _pool.Add(prefab.name, lista);
    }

    public GameObject Obtener(string llave)
    {
        var lista = _pool[llave];

        if (lista == null)
            Debug.LogWarning($"No existe la llave {llave} en el pool");

        foreach (var obj in lista)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        return null;
    }

    public void Devolver(GameObject obj)
    {
        obj.SetActive(false);
    }
}
