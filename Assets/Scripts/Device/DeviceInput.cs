using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceInput : DeviceInputOutput
{
    public float RequiredPower = 0.0f;
    public float InputPower = 0.0f;

    protected override string GetText()
    {
        return "IN: " + RequiredPower.ToString();
    }
}
