using System;
using UnityEngine;

// Token: 0x020000B7 RID: 183
public class GestioneSuoniCasa : MonoBehaviour
{
	// Token: 0x06000687 RID: 1671 RVA: 0x000E7818 File Offset: 0x000E5A18
	private void Start()
	{
		this.cameraCasa = GameObject.FindGameObjectWithTag("MainCamera");
		this.suoniDiCasa = this.cameraCasa.GetComponent<AudioSource>();
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x000E783C File Offset: 0x000E5A3C
	private void Update()
	{
		if (this.attivaSuono)
		{
			this.attivaSuono = false;
			this.suoniDiCasa.Play();
		}
	}

	// Token: 0x04001852 RID: 6226
	private GameObject cameraCasa;

	// Token: 0x04001853 RID: 6227
	public AudioSource suoniDiCasa;

	// Token: 0x04001854 RID: 6228
	public bool attivaSuono;

	// Token: 0x04001855 RID: 6229
	public AudioClip suonoPartenzaSatellite;

	// Token: 0x04001856 RID: 6230
	public AudioClip suonoClickGenerico1;

	// Token: 0x04001857 RID: 6231
	public AudioClip suonoClickScheda;

	// Token: 0x04001858 RID: 6232
	public AudioClip suonoRitirata;

	// Token: 0x04001859 RID: 6233
	public AudioClip suonoVittoriaATavolino;

	// Token: 0x0400185A RID: 6234
	public AudioClip suonoRifiutoMissione;

	// Token: 0x0400185B RID: 6235
	public AudioClip fineTurno;

	// Token: 0x0400185C RID: 6236
	public AudioClip suonoCostruzEdificio;

	// Token: 0x0400185D RID: 6237
	public AudioClip suonoDistruzEdificio;

	// Token: 0x0400185E RID: 6238
	public AudioClip suonoInterruttoreEdificio;
}
