using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OutlineComponent : MonoBehaviour
{
    public Material OutlineMaterial = null;
    public GameObject CloneObject = null;

    public float Size = 1.01f;
    public Color Color = Color.red;

    public void EnableOutline()
    {
        if (CloneObject)
        {
            return;
        }

        CloneObject = Object.Instantiate(gameObject, transform.position, transform.rotation);
        CloneObject.transform.SetParent(transform);
        CloneObject.transform.localScale = transform.localScale;

        foreach (Collider CloneCollider in CloneObject.GetComponentsInChildren<Collider>())
        {
            CloneCollider.enabled = false;
        }

        foreach (MeshRenderer Renderer in CloneObject.GetComponentsInChildren<MeshRenderer>())
        {
            Material[] Materials = Renderer.materials;

            OutlineMaterial.SetFloat("OutlineSize", Size);
            OutlineMaterial.SetColor("OutlineColor", Color);

            for (int Index = 0; Index < Materials.Length; Index++)
            {
                Materials[Index] = OutlineMaterial;
            }

            Renderer.materials = Materials;
            Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        //Debug.LogFormat("Outline created for {0}", gameObject.name);
    }

    public void DisableOutline()
    {
        //Debug.LogFormat("Outline removed for {0}", gameObject.name);
        Object.Destroy(CloneObject);
    }

    void Reset()
    {
        if (OutlineMaterial == null)
        {
            OutlineMaterial = Resources.Load<Material>("Materials/OutlineMaterial");
        }
    }

    // Update is called once per frame
    void OnGUI()
    {
        /*
        if (CloneObject)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUIStyle Style = new GUIStyle();
            Style.normal.background = null;
            Style.alignment = TextAnchor.MiddleCenter;
            Style.normal.textColor = Color.white;
            Style.fontSize = 18;
            Vector3 Position = Camera.main.WorldToScreenPoint(gameObject.GetComponentInChildren<ElectricDevice>().CombinedBounds.center);
            GUI.Label(new Rect(Position.x, Screen.height - Position.y, 1, 1), "Hello", Style);
            GUI.EndGroup();
        }
        */
    }
}
