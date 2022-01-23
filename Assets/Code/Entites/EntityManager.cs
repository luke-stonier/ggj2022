using System;
using System.Collections.Generic;
using UnityEngine;

public struct Entity
{
    public UnityEngine.Object o;
    public DateTime t;
}

public class NoResourceException : Exception {
    public NoResourceException() { }
    public NoResourceException(string message): base (message) {}
}

public class EntityManager : MonoBehaviour
{
    // Entity Manager will listen to Entity Service events and complete the unity behaviour
    public List<Entity> entities = new List<Entity>();


    private void Awake()
    {
        RegisterListeners();
    }

    private void RegisterListeners()
    {
        EntityService.instantiateEvent += HandleInstantiateEvent;
    }

    // Helpers

    private T GetResource<T>(string name) where T : UnityEngine.Object
    {
        var loadedResource = Resources.Load<T>(name);
        if (!loadedResource) throw new NoResourceException(name);
        return loadedResource;
    }

    private UnityEngine.Object Create(UnityEngine.Object resource)
    {
        return Create(resource, Vector3.zero, Quaternion.identity);
    }

    private UnityEngine.Object Create(UnityEngine.Object resource, Vector3 pos)
    {
        return Create(resource, pos, Quaternion.identity);
    }

    private UnityEngine.Object Create(UnityEngine.Object resource, Vector3 pos, Quaternion rot) {
        if (resource == null) return null;
        var go = UnityEngine.Object.Instantiate(resource, pos, rot);
        entities.Add(new Entity { o = go, t = DateTime.Now });
        return go;
    }

    private object Instantiate(UnityEngine.Object resource, Vector3? v1, Quaternion? v2)
    {
        throw new NotImplementedException();
    }

    // Handlers

    private void HandleInstantiateEvent(object sender, InstantiateEntityEventArgs e)
    {
        var instantiatedObject = Create(GetResource<UnityEngine.Object>(e.entity.entityResourceName), e.position, e.rotation);
    }
}
