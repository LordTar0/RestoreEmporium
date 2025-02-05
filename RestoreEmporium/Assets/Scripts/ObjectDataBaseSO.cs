using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Restore Emporium / DataBases / New Object Data Base"))]
public class ObjectDataBaseSO : ScriptableObject
{
    public List<ObjectData> objectData;


}

[Serializable]
public class ObjectData
{
    [field: SerializeField] public int ID { get; private set; } = -1;
    [field: SerializeField] public string Name { get; private set; } = "newObject";
    [field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField] public GameObject Prefab { get; private set; }
}
