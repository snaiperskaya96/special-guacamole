using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class Wire : MonoBehaviour
{
    int WireEndIndex = 0;
    DeviceInputOutput[] WireEnds = new DeviceInputOutput[2];
    List<Vector3> WirePoints = new List<Vector3>();
    LineRenderer WireRenderer = null;

    DeviceInputOutput Input = null;
    DeviceInput Output = null;

    void Awake()
    {
        WireRenderer = GetComponentInChildren<LineRenderer>();

        if (!WireRenderer)
        {
            WireRenderer = gameObject.AddComponent<LineRenderer>();
        }

        WireRenderer.positionCount = 0;
        AnimationCurve WidthCurve = new AnimationCurve(new Keyframe[] {
            new Keyframe(0.0f, 0.01f),
            new Keyframe(1.0f, 0.01f),
        });
        WireRenderer.widthCurve = WidthCurve;
        WireRenderer.sharedMaterial = Resources.Load<Material>("Materials/WireMaterial");
    }

    public void AddPoint(Vector3 Point)
    {
        WireRenderer.positionCount = WireRenderer.positionCount + 1;
        WireRenderer.SetPosition(WireRenderer.positionCount - 1, Point);
    }

    public bool AttachTo(DeviceInputOutput Device)
    {
        if (WireEndIndex >= WireEnds.Length)
        {
            return false;
        }

        if (WireEnds.Contains(Device))
        {
            return false;
        }

        if (Device.Busy)
        {
            return false;
        }

        DeviceOutput DeviceAsOutput = Device as DeviceOutput;
        DeviceInput DeviceAsInput = Device as DeviceInput;

        // Avoid attaching to another output source if we already have one
        if (GetInputSlow() && DeviceAsOutput != null)
        {
            return false;
        }

        // Avoid attaching to another input source if we already have one
        if (GetOutputSlow() && DeviceAsInput != null)
        {
            return false;
        }

        AddPoint(Device.transform.position);
        WireEnds[WireEndIndex++] = Device;
        Device.Busy = true;

        if (WireEndIndex >= WireEnds.Length)
        {
            Activate();
        }

        return true;
    }

    DeviceInput GetOutputSlow()
    {
        return WireEnds.SingleOrDefault(x => x as DeviceInput != null) as DeviceInput;
    }

    DeviceOutput GetInputSlow()
    {
        return WireEnds.SingleOrDefault(x => x as DeviceOutput != null) as DeviceOutput;
    }

    void Activate()
    {
        // Yes they are inverted... An input device is where we output the power we take from another output device
        Output = GetOutputSlow();
        Input = GetInputSlow();
    }

    void Update()
    {
        if (Input && Output)
        {
            // this is fucking with my brain so bad...
            DeviceOutput InputDevice = Input as DeviceOutput;
            if (InputDevice)
            {
                Output.InputPower = InputDevice.OutputPower;
            }
        }
    }
}
