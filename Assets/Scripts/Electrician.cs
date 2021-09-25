using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Electrician : MonoBehaviour
{
    public Texture BoxTexture;

    public int CrosshairSize = 32;
    GUIStyle BoxStyle = new GUIStyle();
    public Vector3 HitLoc = Vector3.zero;
    public LineRenderer RopeRenderer;
    Outline ActiveOutline = null;

    int IgnoreMask = 0;
    RaycastHit HitInfo;
    Wire CurrentWire = null;
    // Start is called before the first frame update
    void Start()
    {
        RopeRenderer = GetComponent<LineRenderer>();
        RopeRenderer.positionCount = 0;
        RopeRenderer.startColor = Color.black;
        RopeRenderer.endColor = Color.black;

        BoxStyle.normal.background = null;

        IgnoreMask = ~LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            AttachWire();
        }
    }

    void AttachWire()
    {
        Transform CameraTransform = Camera.main.transform;
        if (Physics.Raycast(CameraTransform.position, CameraTransform.forward, out HitInfo, 1000.0f, IgnoreMask, QueryTriggerInteraction.Ignore))
        {
            DeviceInputOutput DevicePlug = HitInfo.collider.GetComponentInChildren<DeviceInputOutput>();

            if (CurrentWire)
            {
                if (DevicePlug)
                {
                    if (CurrentWire.AttachTo(DevicePlug))
                    {
                        CurrentWire = null;
                    }
                }
                else
                {
                    CurrentWire.AddPoint(HitInfo.point);
                }
            }
            else if (DevicePlug)
            {
                GameObject WireObject = new GameObject();
                CurrentWire = WireObject.AddComponent<Wire>();
                if (CurrentWire.AttachTo(DevicePlug))
                {
                    Debug.Log("Wire Created");
                }
                else
                {
                    Debug.LogWarning("There was an error while creating the wire.");
                    Object.Destroy(WireObject);
                    CurrentWire = null;
                }
            }
        }
    }

    void OnGUI()
    {
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));

        {
            Rect Position =
                new Rect(Screen.width / 2 - CrosshairSize / 2,
                    Screen.height / 2 - CrosshairSize / 2,
                    CrosshairSize,
                    CrosshairSize);
            GUI.Box(Position, BoxTexture, BoxStyle);
        }

        if (HitLoc != Vector3.zero)
        {
            if (Vector3.Dot(Camera.main.transform.forward, (HitLoc - Camera.main.transform.position).normalized) > 0.0f)
            {
                var ProjectedPos = Camera.main.WorldToScreenPoint(HitLoc);
                Rect Position = new Rect(ProjectedPos.x, Screen.height - ProjectedPos.y, CrosshairSize, CrosshairSize);
                GUI.Box(Position, BoxTexture, BoxStyle);
            }
        }

        GUI.EndGroup();
    }
}
