using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class InteractionControllerComponent : ElectricDevice
{
    bool IsPowered = false;
    InteractorComponent FakeInteractor = null;
    IInteractable TargetInteractable = null;
    // For debugging purposes
    public GameObject TargetInteractableObject = null;
    public float InteractionRadius = 2;


    protected override void Start()
    {
        base.Start();

        Collider[] Overlaps = Physics.OverlapSphere(transform.position, InteractionRadius, LayerMask.GetMask("Interaction"));
        foreach (Collider PossibleTarget in Overlaps)
        {
            IInteractable Interactable = PossibleTarget.gameObject.GetComponentInParent<IInteractable>();
            if (Interactable != null && PossibleTarget.gameObject != gameObject)
            {
                TargetInteractable = Interactable;
                TargetInteractableObject = PossibleTarget.gameObject;
                break;
            }
        }

        GameObject Interactor = new GameObject();
        FakeInteractor = Interactor.AddComponent<InteractorComponent>();
        FakeInteractor.LocalPlayerInteractor = false;

        if (TargetInteractableObject)
        {
            GameObject DebugLinkObject = new GameObject();
            DebugLinkObject.transform.SetParent(transform);
            LineRenderer DebugLinkRenderer = DebugLinkObject.AddComponent<LineRenderer>();
            DebugLinkRenderer.positionCount = 2;
            DebugLinkRenderer.SetPositions(new Vector3[] {
                transform.position,
                TargetInteractableObject.transform.position,
            });
            DebugLinkRenderer.widthCurve = new AnimationCurve(new Keyframe[] {
                new Keyframe(0.0f, 0.01f),
                new Keyframe(1.0f, 0.01f),
            });
        }
    }

    protected override void OnPowered()
    {
        base.OnPowered();

        if (!IsPowered)
        {
            TriggerInteraction();
        }

        IsPowered = true;
    }

    protected override void OnOutOfPower()
    {
        base.OnOutOfPower();

        if (IsPowered)
        {
            TriggerInteraction();
        }

        IsPowered = false;
    }

    void TriggerInteraction()
    {
        if (TargetInteractable != null)
        {
            TargetInteractable.OnInteraction(FakeInteractor);
        }
    }
}
