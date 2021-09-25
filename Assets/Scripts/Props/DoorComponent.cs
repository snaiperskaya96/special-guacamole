using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Assertions;

public class DoorComponent : MonoBehaviour, IInteractable
{
    bool IsOpen = false;
    Quaternion ClosedRotation;
    Quaternion TargetRotation;
    public float DoorSpeedSeconds = 1;

    void Start()
    {
        ClosedRotation = transform.localRotation;
    }

    public void OnInteraction(InteractorComponent Interactor)
    {
        if (!Interactor)
        {
            return;
        }

        if (IsOpen)
        {
            Close();
        }
        else
        {
            Open(Interactor);
        }
    }

    void Close()
    {
        TargetRotation = ClosedRotation;
        IsOpen = false;
    }

    void Open(InteractorComponent Interactor)
    {
        IsOpen = true;

        Quaternion PossibleRotation1 = Quaternion.Euler(ClosedRotation.eulerAngles + new Vector3(0, 90, 0));
        Quaternion PossibleRotation2 = Quaternion.Euler(ClosedRotation.eulerAngles + new Vector3(0, -90, 0));

        Vector3 PossibleDirection1 = PossibleRotation1 * Vector3.forward;
        Vector3 PossibleDirection2 = PossibleRotation2 * Vector3.forward;

        TargetRotation = PossibleRotation1;
        if (Vector3.Dot(PossibleDirection2, Interactor.transform.forward) >= 0)
        {
            TargetRotation = PossibleRotation2;
        }
    }

    void Update()
    {
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, TargetRotation, Time.deltaTime * (90.0f / DoorSpeedSeconds));
    }
}
