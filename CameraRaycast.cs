using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastforCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        interactRaycast();
    }

    void interactRaycast()
    {
        Vector3 playerPosition = transform.position;
        Vector3 forwardDirection = transform.forward;

        Ray interactionRay = new Ray(playerPosition, forwardDirection);
        RaycastHit interactionRayHit;
        float interactionRayLength = 100.0f;
        Vector3 interactionRayEndpoint = playerPosition + forwardDirection * interactionRayLength;
        Debug.DrawLine(playerPosition, interactionRayEndpoint);

        bool hitFound = Physics.Raycast(interactionRay, out interactionRayHit, interactionRayLength);
        if (hitFound)
        {
            GameObject hitGameObject = interactionRayHit.transform.gameObject;
            string hitFeedback = hitGameObject.name;
            Debug.Log("Hit: " + hitFeedback);
        }
        else
        {
            string nothingHitFeedback = "Nothing hit.";
            Debug.Log(nothingHitFeedback);
        }
    }
}
