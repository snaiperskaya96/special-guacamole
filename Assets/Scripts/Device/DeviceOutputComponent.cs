using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceOutputComponent : DeviceInputOutputComponent
{
    public float OutputPower = 0.0f;

    protected override string GetText()
    {
        return "OUT: " + OutputPower.ToString();
    }
}
