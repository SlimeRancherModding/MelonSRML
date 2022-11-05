﻿using System;
using System.Collections.Generic;
using MelonSRML.SR2;
using UnhollowerRuntimeLib;

// ReSharper disable once CheckNamespace
public static class GameObjectExtensions
{
  public static T Initialize<T>(this T obj, Action<T> action) where T : UnityEngine.Object
  {
    action(obj);
    return obj;
  }


  public static GameObject FindChildWithPartialName(
    this GameObject obj,
    string name,
    bool noDive = false)
  {
    GameObject childWithPartialName = (GameObject) null;
    foreach (var o in obj.transform)
    {
      var transform = o.Cast<Transform>();
      if (transform.name.StartsWith(name))
      {
        childWithPartialName = transform.gameObject;
        break;
      }
      if (transform.childCount > 0 && !noDive)
      {
        childWithPartialName = transform.gameObject.FindChildWithPartialName(name);
        if ((UnityEngine.Object) childWithPartialName != (UnityEngine.Object) null)
          break;
      }
    }
    return childWithPartialName;
  }

  public static GameObject FindChild(this GameObject obj, string name, bool dive = false)
  {
    if (!dive)
      return obj.transform.Find(name).gameObject;
    GameObject child = (GameObject) null;
    foreach (var o in obj?.transform)
    {
      var transform = o.Cast<Transform>();
      if (!((UnityEngine.Object) transform == (UnityEngine.Object) null))
      {
        if (transform.name.Equals(name))
        {
          child = transform.gameObject;
          break;
        }
        if (transform.childCount > 0)
        {
          child = transform.gameObject.FindChild(name, dive);
          if ((UnityEngine.Object) child != (UnityEngine.Object) null)
            break;
        }
      }
    }
    return child;
  }

  public static GameObject[] FindChildrenWithPartialName(
    this GameObject obj,
    string name,
    bool noDive = false)
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    foreach (var o in obj.transform)
    {
      var transform = o.Cast<Transform>();
      if (transform.name.StartsWith(name))
        gameObjectList.Add(transform.gameObject);
      if (transform.childCount > 0 && !noDive)
        gameObjectList.AddRange((IEnumerable<GameObject>) transform.gameObject.FindChildrenWithPartialName(name));
    }
    return gameObjectList.ToArray();
  }

  public static GameObject[] FindChildren(this GameObject obj, string name, bool noDive = false)
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    foreach (var o in obj.transform)
    {
      var transform = o.Cast<Transform>();
      if (transform.name.Equals(name))
        gameObjectList.Add(transform.gameObject);
      if (transform.childCount > 0 && !noDive)
        gameObjectList.AddRange((IEnumerable<GameObject>) transform.gameObject.FindChildren(name));
    }
    return gameObjectList.ToArray();
  }

  public static GameObject GetChild(this GameObject obj, int index) => obj.transform.GetChild(index).gameObject;

  public static T FindComponentInParent<T>(this GameObject obj) where T : Component
  {
    T componentInParent;
    if (!((UnityEngine.Object) obj == (UnityEngine.Object) null))
    {
      Transform parent1 = obj.transform.parent;
      T obj1 = parent1 != null ? parent1.GetComponent<T>() : default (T);
      if ((object) obj1 == null)
      {
        Transform parent2 = obj.transform.parent;
        componentInParent = parent2 != null ? parent2.gameObject.FindComponentInParent<T>() : default (T);
      }
      else
        componentInParent = obj1;
    }
    else
      componentInParent = default (T);
    return componentInParent;
  }

  public static GameObject GetChildCopy(this GameObject obj, string name) => SRLookup.CopyPrefab(obj.FindChild(name));

  public static GameObject CreatePrefabCopy(this GameObject obj) => SRLookup.CopyPrefab(obj);

  public static void RemoveComponent<T>(this GameObject go) where T : Component => UnityEngine.Object.Destroy((UnityEngine.Object) go.GetComponent<T>());

  public static void RemoveComponent(this GameObject go, System.Type type) => UnityEngine.Object.Destroy((UnityEngine.Object) go.GetComponent(Il2CppType.From(type)));

  public static void RemoveComponent(this GameObject go, string name) => UnityEngine.Object.Destroy((UnityEngine.Object) go.GetComponent(name));

  public static void RemoveComponentImmediate<T>(this GameObject go) where T : Component => UnityEngine.Object.DestroyImmediate((UnityEngine.Object) go.GetComponent<T>());

  public static void RemoveComponentImmediate(this GameObject go, System.Type type) => UnityEngine.Object.DestroyImmediate((UnityEngine.Object) go.GetComponent(Il2CppType.From(type)));

  public static void RemoveComponentImmediate(this GameObject go, string name) => UnityEngine.Object.DestroyImmediate((UnityEngine.Object) go.GetComponent(name));

  public static T GetOrAddComponent<T>(this GameObject go) where T : Component => !((UnityEngine.Object) go.GetComponent<T>() == (UnityEngine.Object) null) ? go.GetComponent<T>() : go.AddComponent<T>();

  public static Component GetOrAddComponent(this GameObject go, System.Type type) => !((UnityEngine.Object) go.GetComponent(Il2CppType.From(type)) == (UnityEngine.Object) null) ? go.GetComponent(Il2CppType.From(type)) : go.AddComponent(Il2CppType.From(type));

  public static Component GetOrAddComponent(this GameObject go, string name) => !((UnityEngine.Object) go.GetComponent(name) == (UnityEngine.Object) null) ? go.GetComponent(name) : go.AddComponent(Il2CppType.From(System.Type.GetType(name)));

  public static bool HasComponent<T>(this GameObject go) where T : Component => (UnityEngine.Object) go.GetComponent<T>() != (UnityEngine.Object) null;

  public static bool HasComponent(this GameObject go, System.Type type) => (UnityEngine.Object) go.GetComponent(Il2CppType.From(type)) != (UnityEngine.Object) null;

  public static bool HasComponent(this GameObject go, string name) => (UnityEngine.Object) go.GetComponent(name) != (UnityEngine.Object) null;

  public static GameObject InstantiateInactive(this GameObject go, bool keepOriginalName = false) => SRLookup.InstantiateInactive(go, keepOriginalName);
  


  public static void Activate(this GameObject go) => go.SetActive(true);

  public static void Deactivate(this GameObject go) => go.SetActive(false);
}
