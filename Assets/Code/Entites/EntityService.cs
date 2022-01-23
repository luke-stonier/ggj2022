using System;
using UnityEngine;

public class InstantiateEntityEventArgs : EventArgs {
    public IEntity entity;
    public Vector3 position;
    public Quaternion rotation;

    public InstantiateEntityEventArgs(IEntity _e) { entity = _e; }
    public InstantiateEntityEventArgs(IEntity _e, Vector3 pos) { entity = _e; position = pos; }
    public InstantiateEntityEventArgs(IEntity _e, Vector3 pos, Quaternion rot) { entity = _e; position = pos; rotation = rot; }
}

public static class EntityService
{
    // Globaly accessible Entity Service to instantiate/register/pool/destroy objects in the world
    public static event EventHandler<InstantiateEntityEventArgs> instantiateEvent;

    public static void Instantiate(IEntity entity)
    {
        if (entity == null) return;
        instantiateEvent.Invoke(typeof(EntityService), new InstantiateEntityEventArgs(entity));
    }

    public static void Instantiate(IEntity entity, Vector3 position)
    {
        if (entity == null) return;
        instantiateEvent.Invoke(typeof(EntityService), new InstantiateEntityEventArgs(entity, position));
    }

    public static void Instantiate(IEntity entity, Vector3 position, Quaternion rotation)
    {
        if (entity == null) return;
        instantiateEvent.Invoke(typeof(EntityService), new InstantiateEntityEventArgs(entity, position, rotation));
    }
}