using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractorComponent : MonoBehaviour
{
    // Should be false when used from a non-player to avoid checking for GetKeyDown
    public bool LocalPlayerInteractor = true;
    LayerMask InteractionMask;
    RaycastHit Hit;

    void Start()
    {
        InteractionMask = LayerMask.GetMask("Interaction");
    }

    void Update()
    {
        if (!LocalPlayerInteractor)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out Hit, 2, InteractionMask, QueryTriggerInteraction.Ignore))
            {
                ExecuteEvents.Execute<IInteractable>(Hit.transform.root.gameObject, null, (x, y) => x.OnInteraction(this));
            }
        }
    }
}
