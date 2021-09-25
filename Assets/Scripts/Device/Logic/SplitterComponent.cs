using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SplitterComponent : DeviceComponent
{
    protected override void Update()
    {
        base.Update();

        int NumBusyOutputs = DeviceOutputs.Count(x => x.Busy);

        foreach (DeviceOutputComponent Output in DeviceOutputs)
        {
            if (Output.Busy)
            {
                Output.OutputPower = TotalInputPower / Mathf.Max(NumBusyOutputs, 1);
            }
            else
            {
                Output.OutputPower = 0;
            }
        }
    }
}
