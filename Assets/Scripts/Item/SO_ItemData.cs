using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/New Item")]
public class SO_ItemData : ScriptableObject
{
    public int ID;
    public string Name;
    public string Description;
    public StatEffect statEffect;
    
}
