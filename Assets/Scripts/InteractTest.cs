using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTest : MonoBehaviour, IInteractable
{
    public string GetDescription()
    {
        return "this is Description";
    }

    public string GetName()
    {
        return gameObject.name;
    }

    
}
