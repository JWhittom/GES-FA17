using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericInteractiveObject : MonoBehaviour, IActivatable
{
    public string NameText
    {
        get
        {
            throw new System.NotImplementedException();
        }
    }

    public void DoActivate()
    {
        Debug.Log(transform.name + " was activated!");
    }
}
