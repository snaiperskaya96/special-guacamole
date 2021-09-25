using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombinedBoundsComponent : MonoBehaviour
{
    public Bounds CombinedBounds { get; protected set; }

    void Start()
    {
        List<Renderer> Renderers = GetComponentsInChildren<Renderer>().ToList();

        if (Renderers.Count > 0)
        {
            CombinedBounds = Renderers[0].bounds;

            Renderers.Skip(1).ToList().ForEach(x =>
            {
                CombinedBounds.Encapsulate(x.bounds);
            });
        }
    }
}
