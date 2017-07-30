using System;
using UnityEngine;

// Token: 0x0200009F RID: 159
public class DatiProiettile : MonoBehaviour
{
	// Token: 0x060005F7 RID: 1527 RVA: 0x000CF128 File Offset: 0x000CD328
	private void Start()
	{
		base.GetComponent<DatiGeneraliMunizione>().ordignoLocaleAttivo = true;
	}

	// Token: 0x04001684 RID: 5764
	public float distanzaDiInnesco;

	// Token: 0x04001685 RID: 5765
	public Vector3 locazioneTarget;

	// Token: 0x04001686 RID: 5766
	public bool esplosionePerVicinanza;

	// Token: 0x04001687 RID: 5767
	public bool sparatoInFPS;

	// Token: 0x04001688 RID: 5768
	public GameObject target;

	// Token: 0x04001689 RID: 5769
	private float timerAutodistruzione;
}
