using System;
using UnityEngine;

// Token: 0x02000088 RID: 136
public class DatiGeneraliMunizione : MonoBehaviour
{
	// Token: 0x06000574 RID: 1396 RVA: 0x000B2B4C File Offset: 0x000B0D4C
	private void Update()
	{
		if (this.ordignoLocaleAttivo)
		{
			this.timerAutodistruzione += Time.deltaTime;
			if (!this.èDiArtiglieria)
			{
				if (this.timerAutodistruzione > 6f)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
			else if (this.èDiLungaVita)
			{
				if (this.timerAutodistruzione > 60f)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
			else if (this.timerAutodistruzione > 20f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x04001491 RID: 5265
	public string nome;

	// Token: 0x04001492 RID: 5266
	public float portataMinima;

	// Token: 0x04001493 RID: 5267
	public float portataMassima;

	// Token: 0x04001494 RID: 5268
	public float danno;

	// Token: 0x04001495 RID: 5269
	public float penetrazione;

	// Token: 0x04001496 RID: 5270
	public float raggioEffetto;

	// Token: 0x04001497 RID: 5271
	public GameObject tipoMunizioneBase;

	// Token: 0x04001498 RID: 5272
	public GameObject descrizioneArma;

	// Token: 0x04001499 RID: 5273
	public bool èDiArtiglieria;

	// Token: 0x0400149A RID: 5274
	public bool èDiLungaVita;

	// Token: 0x0400149B RID: 5275
	public bool ordignoLocaleAttivo;

	// Token: 0x0400149C RID: 5276
	private float timerAutodistruzione;

	// Token: 0x0400149D RID: 5277
	public int truppaDiOrigine;
}
