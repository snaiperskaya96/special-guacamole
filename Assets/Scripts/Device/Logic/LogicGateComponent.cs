using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LogicGateComponent : DeviceComponent
{
    public enum LogicGateType
    {
        AND,
        OR,
        XOR
    }

    public LogicGateType GateType = LogicGateType.AND;

    protected override void Start()
    {
        base.Start();
        Assert.IsTrue(DeviceInputs.Count > 1);
        Assert.IsTrue(DeviceOutputs.Count == 1);
    }
    protected override void Update()
    {
        base.Update();

        float Output = 0;

        DeviceInputComponent A = DeviceInputs[0];
        DeviceInputComponent B = DeviceInputs[1];

        bool ABool = A.InputPower > 0;
        bool BBool = B.InputPower > 0;

        switch (GateType)
        {
            case LogicGateType.AND:
                Output = ABool && BBool ? Mathf.Max(A.InputPower, B.InputPower) : 0;
            break;
            case LogicGateType.OR:
                Output = ABool || BBool ? Mathf.Max(A.InputPower, B.InputPower) : 0;
            break;
            case LogicGateType.XOR:
                Output = ABool ^ BBool ? Mathf.Max(A.InputPower, B.InputPower) : 0;
            break;
        }

        DeviceOutputs[0].OutputPower = Output;
    }
}
