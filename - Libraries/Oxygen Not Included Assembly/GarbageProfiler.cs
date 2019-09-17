﻿// Decompiled with JetBrains decompiler
// Type: GarbageProfiler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;

public static class GarbageProfiler
{
  private static string ROOT_MEMORY_DUMP_PATH = "./memory/";
  private static MemorySnapshot previousSnapshot;
  private static string filename_suffix;
  private static System.Type DEBUG_STATIC_TYPE;

  private static void UnloadUnusedAssets()
  {
    Resources.UnloadUnusedAssets();
  }

  private static void ClearFileName()
  {
    GarbageProfiler.filename_suffix = (string) null;
  }

  public static string GetFileName(string name)
  {
    string fullPath = System.IO.Path.GetFullPath(GarbageProfiler.ROOT_MEMORY_DUMP_PATH);
    if (GarbageProfiler.filename_suffix == null)
    {
      if (!Directory.Exists(fullPath))
        Directory.CreateDirectory(fullPath);
      System.DateTime now = System.DateTime.Now;
      GarbageProfiler.filename_suffix = "_" + now.Year.ToString() + "-" + now.Month.ToString() + "-" + now.Day.ToString() + "_" + now.Hour.ToString() + "-" + now.Minute.ToString() + "-" + now.Second.ToString() + ".csv";
    }
    return System.IO.Path.Combine(fullPath, name + GarbageProfiler.filename_suffix);
  }

  private static void Dump()
  {
    Debug.Log((object) "Writing snapshot...");
    MemorySnapshot memorySnapshot = new MemorySnapshot();
    GarbageProfiler.ClearFileName();
    MemorySnapshot.TypeData[] array = new MemorySnapshot.TypeData[memorySnapshot.types.Count];
    memorySnapshot.types.Values.CopyTo(array, 0);
    Array.Sort<MemorySnapshot.TypeData>(array, 0, array.Length, (IComparer<MemorySnapshot.TypeData>) new GarbageProfiler.InstanceCountComparer());
    using (StreamWriter streamWriter = new StreamWriter(GarbageProfiler.GetFileName("memory_instances")))
    {
      streamWriter.WriteLine("Delta,Instances,NumArrayEntries,Type Name");
      foreach (MemorySnapshot.TypeData typeData1 in array)
      {
        if (typeData1.instanceCount != 0)
        {
          int num = typeData1.instanceCount;
          if (GarbageProfiler.previousSnapshot != null)
          {
            MemorySnapshot.TypeData typeData2 = MemorySnapshot.GetTypeData(typeData1.type, GarbageProfiler.previousSnapshot.types);
            num = typeData1.instanceCount - typeData2.instanceCount;
          }
          streamWriter.WriteLine(num.ToString() + "," + (object) typeData1.instanceCount + "," + (object) typeData1.numArrayEntries + ",\"" + typeData1.type.ToString() + "\"");
        }
      }
    }
    using (StreamWriter streamWriter = new StreamWriter(GarbageProfiler.GetFileName("memory_hierarchies")))
    {
      streamWriter.WriteLine("Delta,Count,Type Hierarchy");
      foreach (MemorySnapshot.TypeData typeData1 in array)
      {
        if (typeData1.instanceCount != 0)
        {
          foreach (KeyValuePair<MemorySnapshot.HierarchyNode, int> hierarchy in typeData1.hierarchies)
          {
            int num1 = hierarchy.Value;
            if (GarbageProfiler.previousSnapshot != null)
            {
              MemorySnapshot.TypeData typeData2 = MemorySnapshot.GetTypeData(typeData1.type, GarbageProfiler.previousSnapshot.types);
              int num2 = 0;
              if (typeData2.hierarchies.TryGetValue(hierarchy.Key, out num2))
                num1 = hierarchy.Value - num2;
            }
            streamWriter.WriteLine(num1.ToString() + "," + (object) hierarchy.Value + ", \"" + typeData1.type.ToString() + ": " + hierarchy.Key.ToString() + "\"");
          }
        }
      }
    }
    GarbageProfiler.previousSnapshot = memorySnapshot;
    Debug.Log((object) "Done writing snapshot!");
  }

  public static void DebugDumpGarbageStats()
  {
    Debug.Log((object) "Writing reference stats...");
    MemorySnapshot memorySnapshot = new MemorySnapshot();
    GarbageProfiler.ClearFileName();
    MemorySnapshot.TypeData[] array1 = new MemorySnapshot.TypeData[memorySnapshot.types.Count];
    memorySnapshot.types.Values.CopyTo(array1, 0);
    Array.Sort<MemorySnapshot.TypeData>(array1, 0, array1.Length, (IComparer<MemorySnapshot.TypeData>) new GarbageProfiler.InstanceCountComparer());
    using (StreamWriter streamWriter = new StreamWriter(GarbageProfiler.GetFileName("garbage_instances")))
    {
      foreach (MemorySnapshot.TypeData typeData1 in array1)
      {
        if (typeData1.instanceCount != 0)
        {
          int num = typeData1.instanceCount;
          if (GarbageProfiler.previousSnapshot != null)
          {
            MemorySnapshot.TypeData typeData2 = MemorySnapshot.GetTypeData(typeData1.type, GarbageProfiler.previousSnapshot.types);
            num = typeData1.instanceCount - typeData2.instanceCount;
          }
          streamWriter.WriteLine(num.ToString() + ", " + (object) typeData1.instanceCount + ", \"" + typeData1.type.ToString() + "\"");
        }
      }
    }
    Array.Sort<MemorySnapshot.TypeData>(array1, 0, array1.Length, (IComparer<MemorySnapshot.TypeData>) new GarbageProfiler.RefCountComparer());
    using (StreamWriter streamWriter = new StreamWriter(GarbageProfiler.GetFileName("garbage_refs")))
    {
      foreach (MemorySnapshot.TypeData typeData1 in array1)
      {
        if (typeData1.refCount != 0)
        {
          int num = typeData1.refCount;
          if (GarbageProfiler.previousSnapshot != null)
          {
            MemorySnapshot.TypeData typeData2 = MemorySnapshot.GetTypeData(typeData1.type, GarbageProfiler.previousSnapshot.types);
            num = typeData1.refCount - typeData2.refCount;
          }
          streamWriter.WriteLine(num.ToString() + ", " + (object) typeData1.refCount + ", \"" + typeData1.type.ToString() + "\"");
        }
      }
    }
    MemorySnapshot.FieldCount[] array2 = new MemorySnapshot.FieldCount[memorySnapshot.fieldCounts.Count];
    memorySnapshot.fieldCounts.Values.CopyTo(array2, 0);
    Array.Sort<MemorySnapshot.FieldCount>(array2, 0, array2.Length, (IComparer<MemorySnapshot.FieldCount>) new GarbageProfiler.FieldCountComparer());
    using (StreamWriter streamWriter = new StreamWriter(GarbageProfiler.GetFileName("garbage_fields")))
    {
      foreach (MemorySnapshot.FieldCount fieldCount1 in array2)
      {
        int num = fieldCount1.count;
        if (GarbageProfiler.previousSnapshot != null)
        {
          foreach (KeyValuePair<int, MemorySnapshot.FieldCount> fieldCount2 in GarbageProfiler.previousSnapshot.fieldCounts)
          {
            if (fieldCount2.Value.name == fieldCount1.name)
            {
              num = fieldCount1.count - fieldCount2.Value.count;
              break;
            }
          }
        }
        streamWriter.WriteLine(num.ToString() + ", " + (object) fieldCount1.count + ", \"" + fieldCount1.name + "\"");
      }
    }
    memorySnapshot.WriteTypeDetails(GarbageProfiler.previousSnapshot);
    GarbageProfiler.previousSnapshot = memorySnapshot;
    Debug.Log((object) "Done writing reference stats!");
  }

  public static void DebugDumpRootItems()
  {
    Debug.Log((object) "Writing root items...");
    System.Type[] array = new System.Type[11]
    {
      typeof (string),
      typeof (HashedString),
      typeof (KAnimHashedString),
      typeof (Tag),
      typeof (bool),
      typeof (CellOffset),
      typeof (Color),
      typeof (Color32),
      typeof (Vector2),
      typeof (Vector3),
      typeof (Vector2I)
    };
    System.Type[] typeArray = new System.Type[3]
    {
      typeof (List<>),
      typeof (HashSet<>),
      typeof (Dictionary<,>)
    };
    string fileName = GarbageProfiler.GetFileName("statics");
    GarbageProfiler.ClearFileName();
    using (StreamWriter streamWriter = new StreamWriter(fileName))
    {
      streamWriter.WriteLine("FieldName,Type,ListLength");
      Assembly[] assemblyArray = new Assembly[2]
      {
        Assembly.GetAssembly(typeof (Game)),
        Assembly.GetAssembly(typeof (App))
      };
      foreach (Assembly assembly in assemblyArray)
      {
        foreach (System.Type type1 in assembly.GetTypes())
        {
          if (type1 == GarbageProfiler.DEBUG_STATIC_TYPE)
            Debugger.Break();
          if (!type1.IsAbstract && !type1.IsGenericType && !type1.ToString().StartsWith("STRINGS."))
          {
            foreach (FieldInfo field in type1.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
              if (field.IsStatic && !field.IsInitOnly && (!field.IsLiteral && !field.Name.Contains("$cache")))
              {
                System.Type fieldType = field.FieldType;
                if (!fieldType.IsPointer && !Helper.IsPOD(fieldType) && Array.IndexOf<System.Type>(array, fieldType) < 0)
                {
                  if (typeof (Array).IsAssignableFrom(fieldType))
                  {
                    System.Type elementType = fieldType.GetElementType();
                    if (elementType.IsPointer || Helper.IsPOD(elementType) || Array.IndexOf<System.Type>(array, elementType) >= 0)
                      continue;
                  }
                  if (fieldType.IsGenericType)
                  {
                    System.Type genericTypeDefinition = fieldType.GetGenericTypeDefinition();
                    System.Type[] genericArguments = fieldType.GetGenericArguments();
                    bool flag1 = false;
                    foreach (System.Type type2 in typeArray)
                    {
                      if (genericTypeDefinition == type2)
                      {
                        bool flag2 = true;
                        foreach (System.Type type3 in genericArguments)
                        {
                          if (!Helper.IsPOD(type3) && Array.IndexOf<System.Type>(array, type3) < 0)
                          {
                            flag2 = false;
                            break;
                          }
                        }
                        if (flag2)
                        {
                          flag1 = true;
                          break;
                        }
                      }
                    }
                    if (flag1)
                      continue;
                  }
                  object obj = field.GetValue((object) null);
                  if (obj != null)
                  {
                    string str;
                    if (typeof (ICollection).IsAssignableFrom(fieldType))
                    {
                      int count = (obj as ICollection).Count;
                      str = string.Format("\"{0}.{1}\",\"{2}\",{3}", (object) type1, (object) field.Name, (object) fieldType, (object) count);
                    }
                    else
                      str = string.Format("\"{0}.{1}\",\"{2}\"", (object) type1, (object) field.Name, (object) fieldType);
                    streamWriter.WriteLine(str);
                  }
                }
              }
            }
          }
        }
      }
    }
    Debug.Log((object) "Done writing reference stats!");
  }

  private class InstanceCountComparer : IComparer<MemorySnapshot.TypeData>
  {
    public int Compare(MemorySnapshot.TypeData a, MemorySnapshot.TypeData b)
    {
      return b.instanceCount - a.instanceCount;
    }
  }

  private class RefCountComparer : IComparer<MemorySnapshot.TypeData>
  {
    public int Compare(MemorySnapshot.TypeData a, MemorySnapshot.TypeData b)
    {
      return b.refCount - a.refCount;
    }
  }

  private class FieldCountComparer : IComparer<MemorySnapshot.FieldCount>
  {
    public int Compare(MemorySnapshot.FieldCount a, MemorySnapshot.FieldCount b)
    {
      return b.count - a.count;
    }
  }
}