using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateLookedAtObjects : MonoBehaviour
{
    [SerializeField]
    private float maxActivateDistance = 4.0f;

    [SerializeField]
    private Text hoverText;

    private IActivatable objectLookedAt;

	// Update is called once per frame
	void Update ()
    {
        UpdateObjectLookedAt();
        UpdateHoverText();
        HandleInput();
    }

    private void UpdateObjectLookedAt()
    {
        //Debug.DrawRay(transform.position, transform.forward * maxActivateDistance, Color.blue);
        RaycastHit raycastHit;

        if (Physics.Raycast(transform.position, transform.forward,
            out raycastHit, maxActivateDistance))
        {
            //Debug.Log("Racast hit " + raycastHit.transform.name);

            objectLookedAt = raycastHit.transform.GetComponent<IActivatable>();
        }
        else
            objectLookedAt = null;
    }

    private void UpdateHoverText()
    {
        if (objectLookedAt == null)
            hoverText.text = string.Empty;
        else
            hoverText.text = objectLookedAt.NameText;
    }

    private void HandleInput()
    {
        if (objectLookedAt != null && Input.GetButtonDown("Activate"))
        {
            objectLookedAt.DoActivate();
        }
    }
}
