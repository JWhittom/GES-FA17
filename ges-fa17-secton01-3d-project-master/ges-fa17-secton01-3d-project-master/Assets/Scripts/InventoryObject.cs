using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryObject : MonoBehaviour, IActivatable
{
    [SerializeField]
    private string nameText;

    [SerializeField]
    private string descriptionText;

    private MeshRenderer meshRenderer;
    private Collider collider;
    private List<InventoryObject> playerInventory;

    public string NameText
    {
        get
        {
            return nameText;
        }
    }

    public void DoActivate()
    {
        meshRenderer.enabled = false;
        collider.enabled = false;
        playerInventory.Add(this);
    }

    // Use this for initialization
    void Start ()
    {
        playerInventory = FindObjectOfType<InventoryMenu>().PlayerInventory;
        meshRenderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
	}
	
}
