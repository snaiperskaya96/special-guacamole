using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevicePassThroughComponent : DeviceOutputComponent
{
    protected override string GetText()
    {
        return "PASSTHROUGH: " + Mathf.Max(0.0f, OutputPower).ToString();
    }
}
