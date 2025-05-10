using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Restore Emporium/New NPC")]
public class NPCSO : ScriptableObject
{
    public int ID = -1;
    public NPCData data;
}

[System.Serializable]
public class NPCData
{
    public NameAndDescription NameAndDescription;

    public Sprite Icon;
    public Sprite Base;

    public Sprite BlankFace;
    public Sprite HappyFace;
    public Sprite AngryFace;
    public Sprite SadFace;
}