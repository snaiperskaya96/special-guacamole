using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(CombinedBoundsComponent))]
public class DeviceInputOutput : MonoBehaviour
{
    Outline DeviceOutline = null;
    ElectricDevice ParentDevice = null;
    CombinedBoundsComponent CombinedBounds = null;
    bool WithinRange = false;
    int IgnoreMask = 0;
    bool BeingLookedAt = false;
    GUIStyle LabelStyle = new GUIStyle();
    public bool Busy = false;

    protected virtual string GetText()
    {
        return "";
    }

    // Start is called before the first frameupdate
    void Start()
    {
        DeviceOutline = GetComponent<Outline>();
        CombinedBounds = GetComponentInChildren<CombinedBoundsComponent>();
        ParentDevice = GetComponentInParent<ElectricDevice>();

        Assert.IsNotNull(DeviceOutline);
        Assert.IsNotNull(ParentDevice);

        IgnoreMask = ~LayerMask.GetMask("Player");

        LabelStyle.fontSize = 32;
        LabelStyle.normal.textColor = Color.white;
        LabelStyle.alignment = TextAnchor.MiddleLeft;
    }

    // Update is called once per frame
    void Update()
    {
        if (!WithinRange)
        {
            return;
        }

        Transform CameraTransform = Camera.main.transform;

        RaycastHit HitInfo;
        if (Physics.Raycast(CameraTransform.position, CameraTransform.forward, out HitInfo, 10.0f, IgnoreMask, QueryTriggerInteraction.Ignore))
        {
            BeingLookedAt = HitInfo.collider.gameObject == gameObject;
        }
        else
        {
            BeingLookedAt = false;
        }
    }

    void OnGUI()
    {
        if (BeingLookedAt)
        {
            Vector3 ProjectedPosition = Camera.main.WorldToScreenPoint(CombinedBounds.CombinedBounds.center);
            string Text = ParentDevice.gameObject.name + "\n" + GetText();
            Vector2 Size = LabelStyle.CalcSize(new GUIContent(Text));
            GUI.Label(new Rect(ProjectedPosition.x, Screen.height - ProjectedPosition.y, Size.x, Size.y), Text, LabelStyle);
        }
    }

    private void OnTriggerEnter(Collider Other)
    {
        DeviceOutline?.EnableOutline();
        WithinRange = true;
        BeingLookedAt = false;
    }

    private void OnTriggerExit(Collider Other)
    {
        DeviceOutline.DisableOutline();
        WithinRange = false;
        BeingLookedAt = false;
    }

}
