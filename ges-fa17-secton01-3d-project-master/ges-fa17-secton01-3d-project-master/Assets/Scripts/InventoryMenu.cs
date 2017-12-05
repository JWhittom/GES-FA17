using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryMenuPanel;

    [SerializeField]
    private FirstPersonController firstPersonController;

    private bool IsMenuShowing
    {
        get { return inventoryMenuPanel.activeSelf; }
    }

    public List<InventoryObject> PlayerInventory { get; private set; }

    private void Awake()
    {
        PlayerInventory = new List<InventoryObject>();
    }

    // Use this for initialization
    void Start ()
    {
        HideMenu();
	}

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if (IsMenuShowing)
                HideMenu();
            else
                ShowMenu();
        }
    }

    private void HideMenu()
    {
        inventoryMenuPanel.SetActive(false);
        UpdateCursor();
        firstPersonController.enabled = true;
    }

    private void ShowMenu()
    {
        inventoryMenuPanel.SetActive(true);
        UpdateCursor();
        firstPersonController.enabled = false;
    }

    private void UpdateCursor()
    {
        if(IsMenuShowing)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
