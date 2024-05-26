using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;
using UnityEngine.Video;
using Unity.XR.CoreUtils;
using NodeCanvas.Framework;

public class PickupTrigger : MonoBehaviour
{
    public GameObject player; // The player object
    public GameObject interactionUI; // The UI element to show when player is close enough
    public bool hasPickedUp = false; // The bool value to change after picking up
    public  Blackboard bb;
    private void Start()
    {
        interactionUI.SetActive(false); // Ensure the UI is not visible at start
    
        bb.SetVariableValue("Isbookfinded",false);
    }

    private void Update()
    {
        // Check if the player is close to the object and has not picked it up yet
        if (Vector3.Distance(player.transform.position, transform.position) < 3f && !hasPickedUp)
        {
            Debug.Log("可以拾取物体了"); // Log message for debugging
            interactionUI.SetActive(true); // Show the interaction UI
        }
        else
        {
            interactionUI.SetActive(false); // Hide the interaction UI
        }
    }

    public void PickupItem()
    {
        // This method should be called by the UI button's onClick event
        hasPickedUp = true; // Change the bool value to true
        interactionUI.SetActive(false); // Hide the interaction UI after picking up
         bb.SetVariableValue("Isbookfinded",true);
        // Additional logic for picking up the item can be added here
        Debug.Log("物体已拾取"); // Log message for debugging
    }
}