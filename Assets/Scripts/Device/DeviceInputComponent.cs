using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceInputComponent : DeviceInputOutputComponent
{
    public float RequiredPower = 0.0f;
    public float InputPower = 0.0f;

    protected override string GetText()
    {
        return "IN: " + RequiredPower.ToString();
    }
}
