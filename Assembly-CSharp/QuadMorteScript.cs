using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class QuadMorteScript : MonoBehaviour
{
	// Token: 0x06000079 RID: 121 RVA: 0x00017FF0 File Offset: 0x000161F0
	private void Update()
	{
		if (this.attivo)
		{
			this.timerScomparsa += Time.deltaTime;
			if (this.timerScomparsa > 10f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x040002F5 RID: 757
	public bool attivo;

	// Token: 0x040002F6 RID: 758
	private float timerScomparsa;
}
