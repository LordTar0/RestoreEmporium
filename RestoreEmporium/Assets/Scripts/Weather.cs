using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Restore Emporium/New Weather")]
public class Weather : ScriptableObject
{
    public int ID = -1;

    public Sprite Icon;
    public string Name;
    [TextArea(0,10)] public string Description;
    [Space(10)]

    [Range(0.01f,2)] public float NPCMultiplier;
    [Range(0.01f,2)] public float ShippingMultipler;
    public weatherType weatherType;
}

public enum weatherType
{
    Normal,
    Sunny,
    Wet,
    Cold
}