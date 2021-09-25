using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonComponent : DeviceComponent, IInteractable
{
    public float ActiveTime = 2.0f;
    public Vector3 PressedOffset = Vector3.zero;
    bool IsPressed = false;

    protected override bool ShouldPassThrough()
    {
        return base.ShouldPassThrough() && IsPressed;
    }

    public void OnInteraction(InteractorComponent Interactor)
    {
        if (IsPressed)
        {
            return;
        }

        StartCoroutine(OnButtonPressed());
    }

    IEnumerator OnButtonPressed()
    {
        Pressed();
        yield return new WaitForSeconds(ActiveTime);
        Released();
    }

    void Pressed()
    {
        IsPressed = true;
        Transform Mesh = transform.Find("ButtonMesh");
        if (Mesh)
        {
            Mesh.localPosition -= PressedOffset;
        }
    }

    void Released()
    {
        IsPressed = false;
        Transform Mesh = transform.Find("ButtonMesh");
        if (Mesh)
        {
            Mesh.localPosition += PressedOffset;
        }
    }
}
