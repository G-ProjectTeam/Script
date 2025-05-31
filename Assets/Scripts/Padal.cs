using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200000D RID: 13
public class Padal : MonoBehaviour
{
	// Token: 0x0600002D RID: 45 RVA: 0x00002EDD File Offset: 0x000010DD
	private void Awake()
	{
		this._beforePos = base.transform.localPosition;
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002EF0 File Offset: 0x000010F0
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == 3)
		{
			this.pressSfx.Play();
			base.transform.localPosition = this._beforePos - new Vector3(0f, this.fallAmount, 0f);
			Debug.Log("pedal pressed!");
			this.events.Invoke();
		}
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002F56 File Offset: 0x00001156
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.layer == 3)
		{
			base.transform.localPosition = this._beforePos;
		}
	}

	// Token: 0x04000035 RID: 53
	public UnityEvent events;

	// Token: 0x04000036 RID: 54
	private Vector3 _beforePos;

	// Token: 0x04000037 RID: 55
	public float fallAmount = 0.2f;

	// Token: 0x04000038 RID: 56
	public AudioSource pressSfx;
}
