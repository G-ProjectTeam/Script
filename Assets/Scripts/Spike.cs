using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class Spike : MonoBehaviour
{
	// Token: 0x06000044 RID: 68 RVA: 0x000036F1 File Offset: 0x000018F1
	private void Awake()
	{
		this._rigid = base.GetComponent<Rigidbody2D>();
	}

	// Token: 0x06000045 RID: 69 RVA: 0x000036FF File Offset: 0x000018FF
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == 3)
		{
			other.gameObject.GetComponent<PlayerMovement>().Kill();
		}
		int layer = other.gameObject.layer;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x0000372E File Offset: 0x0000192E
	public void Fall()
	{
		this._rigid.constraints = (RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation);
		this._rigid.linearVelocity = new Vector2(0f, -0.3f);
	}

	// Token: 0x04000054 RID: 84
	private Rigidbody2D _rigid;

	// Token: 0x04000055 RID: 85
	public AudioSource fallSfx;
}
