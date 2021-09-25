using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Assertions;

[RequireComponent(typeof(OutlineComponent))]
[RequireComponent(typeof(CombinedBoundsComponent))]
public class DeviceComponent : MonoBehaviour
{
    protected CombinedBoundsComponent CombinedBounds = null;
    protected List<DeviceInputComponent> DeviceInputs = new List<DeviceInputComponent>();
    protected List<DeviceOutputComponent> DeviceOutputs = new List<DeviceOutputComponent>();
    public List<Transform> Inputs = new List<Transform>();
    public List<Transform> Outputs = new List<Transform>();
    public DevicePassThroughComponent PassThrough;
    public float PowerOutput = 0.0f;
    public float RequiredPower = 0.0f;
    public bool ShowDebugInfo = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        CombinedBounds = GetComponent<CombinedBoundsComponent>();

        Assert.IsNotNull(CombinedBounds);

        // This is a bit inconsistent with the passthrough pins as we don't create the component in runtime
        Inputs.ForEach(x =>
        {
            if (x && x.gameObject)
            {
                var Component = x.gameObject.AddComponent<DeviceInputComponent>();
                Component.RequiredPower = RequiredPower;
                DeviceInputs.Add(Component);
            }
        });

        Outputs.ForEach(x =>
        {
            if (x && x.gameObject)
            {
                var Component = x.gameObject.AddComponent<DeviceOutputComponent>();
                Component.OutputPower = PowerOutput;
                DeviceOutputs.Add(Component);
            }
        });
    }

    protected virtual void Update()
    {
        if (DeviceInputs.Count > 0)
        {
            float InputPower = DeviceInputs.Sum(x => x.InputPower);

            float PowerDelta = InputPower - RequiredPower;

            if (PowerDelta >= 0)
            {
                OnPowered();

                if (ShouldPassThrough())
                {
                    PassThrough.OutputPower = InputPower - RequiredPower;
                }
                else if (PassThrough)
                {
                    PassThrough.OutputPower = 0;
                }

                return;
            }
        }

        OnOutOfPower();

        if (PassThrough)
        {
            PassThrough.OutputPower = 0.0f;
        }
    }

    protected virtual bool ShouldPassThrough()
    {
        return PassThrough != null;
    }

    protected virtual void OnPowered()
    {

    }

    protected virtual void OnOutOfPower()
    {

    }

    protected virtual void OnGUI()
    {
        Transform CameraTransform = Camera.main.transform;
        GUIStyle TextStyle = new GUIStyle
        {
            fontSize = 32,
            normal = new GUIStyleState
            {
                textColor = Color.white,
            }
        };

        if (Vector3.Dot((transform.position - CameraTransform.position).normalized, CameraTransform.forward) > 0.0f)
        {
            {
                Vector3 WorldPosition = CombinedBounds.CombinedBounds.center;
                Vector3 ScreenPosition = Camera.main.WorldToScreenPoint(WorldPosition);
                GUI.Label(new Rect(ScreenPosition.x, Screen.height - ScreenPosition.y, 128, 16), gameObject.name, TextStyle);
            }

            if (ShowDebugInfo)
            {

                Vector3 ScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
                float InputPower = DeviceInputs.Sum(x => x.InputPower);

                GUI.Label(new Rect(ScreenPosition.x, Screen.height - ScreenPosition.y, 128, 16), "Input power: " + InputPower.ToString(), TextStyle);
                if (ShouldPassThrough())
                {
                    float PassthroughPower = InputPower - RequiredPower;
                    GUI.Label(new Rect(ScreenPosition.x, Screen.height - ScreenPosition.y + 32, 128, 16), "Passthrough power: " + PassthroughPower.ToString(), TextStyle);
                }
            }
        }
    }
}
