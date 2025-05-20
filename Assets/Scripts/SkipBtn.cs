using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class SkipBtn : MonoBehaviour
{
	// Token: 0x0600003F RID: 63 RVA: 0x00003648 File Offset: 0x00001848
	public void Click()
	{
		Door.Instance.NextStage();
	}
}
