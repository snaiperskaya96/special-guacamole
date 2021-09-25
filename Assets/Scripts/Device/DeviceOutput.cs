using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceOutput : DeviceInputOutput
{
    public float OutputPower = 0.0f;

    protected override string GetText()
    {
        return "OUT: " + OutputPower.ToString();
    }
}
