using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SwitchComponent : DeviceComponent, IInteractable
{
    Transform Lever = null;
    Quaternion OffRotation;
    bool SwitchOn = false;

    protected override bool ShouldPassThrough()
    {
        return base.ShouldPassThrough() && SwitchOn;
    }

    public void OnInteraction(InteractorComponent Interactor)
    {
        if (SwitchOn)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }

    private void TurnOn()
    {
        SwitchOn = true;
        Lever.localRotation = Quaternion.Euler(180, 0, 0);
    }

    private void TurnOff()
    {
        SwitchOn = false;
        Lever.localRotation = OffRotation;
    }

    protected override void Start()
    {
        base.Start();

        foreach (Transform ChildTransform in GetComponentsInChildren<Transform>())
        {
            Lever = ChildTransform.Find("Lever");
            if (Lever)
            {
                break;
            }
        }
        Assert.IsNotNull(Lever);
        OffRotation = Lever.localRotation;

        Assert.IsTrue(DeviceOutputs.Count > 0);
    }

    protected override void Update()
    {
        base.Update();

        DeviceOutputs[0].OutputPower = SwitchOn ? Mathf.Max(TotalInputPower - RequiredPower, 0) : 0;
    }
}
