using System;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class BallMover : MonoBehaviour
{
	// Token: 0x06000003 RID: 3 RVA: 0x0000206A File Offset: 0x0000026A
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == 8)
		{
			return;
		}
		this.factor *= -1f;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x0000208D File Offset: 0x0000028D
	private void Update()
	{
		base.transform.position += new Vector3(0f, this.factor * Time.deltaTime, 0f);
	}

	// Token: 0x04000002 RID: 2
	public float factor = 3f;
}
