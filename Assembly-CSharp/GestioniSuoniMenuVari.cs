using System;
using UnityEngine;

// Token: 0x020000B8 RID: 184
public class GestioniSuoniMenuVari : MonoBehaviour
{
	// Token: 0x0600068A RID: 1674 RVA: 0x000E7864 File Offset: 0x000E5A64
	private void Start()
	{
		this.suonoClick = base.GetComponent<AudioSource>();
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x000E7874 File Offset: 0x000E5A74
	private void Update()
	{
		this.FunzioneSuono();
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x000E787C File Offset: 0x000E5A7C
	private void FunzioneSuono()
	{
		if (this.attivaSuono)
		{
			this.attivaSuono = false;
			this.suonoClick.Play();
		}
	}

	// Token: 0x0400185F RID: 6239
	public AudioSource suonoClick;

	// Token: 0x04001860 RID: 6240
	public bool attivaSuono;
}
