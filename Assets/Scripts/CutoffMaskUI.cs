using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutoffMaskUI : Image
{
    public override Material materialForRendering
    {
        get
        {
            Material material = new Material(base.materialForRendering);
            material.SetInt("_StencilComp", 6);
            return material;
        }
    }

    protected override void Start()
    {
        base.Start();
        this.Fix();
    }

    public void Fix()
    {
        base.StartCoroutine(this.CoFix());
    }

    private IEnumerator CoFix()
    {
        yield return null;
        base.maskable = false;
        base.maskable = true;
    }

    public void SetRadius(float radius)
    {
        if (this.material != null)
        {
            this.material.SetFloat("_Radius", radius);
        }
    }
}
