using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Assertions;

public class Lamp : ElectricDevice
{
    Light LightSource = null;
    Light BulbLight = null;
    protected override void Start()
    {
        base.Start();

        LightSource = GetComponentsInChildren<Light>().First(x => x.type == LightType.Spot);
        BulbLight = GetComponentsInChildren<Light>().First(x => x.type == LightType.Point);

        Assert.IsNotNull(LightSource);
        Assert.IsNotNull(BulbLight);

        LightSource.enabled = false;
        BulbLight.enabled = false;
    }

    protected override void OnPowered()
    {
        LightSource.enabled = BulbLight.enabled = true;
    }

    protected override void OnOutOfPower()
    {
        LightSource.enabled = BulbLight.enabled = false;
    }
}
