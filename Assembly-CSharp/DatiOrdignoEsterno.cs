using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000089 RID: 137
public class DatiOrdignoEsterno : MonoBehaviour
{
	// Token: 0x06000576 RID: 1398 RVA: 0x000B2BF0 File Offset: 0x000B0DF0
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x000B2C2C File Offset: 0x000B0E2C
	private void Update()
	{
		if (this.ordignoAttivo)
		{
			for (int i = 1; i < base.transform.childCount; i++)
			{
				if (base.transform.childCount > 1)
				{
					base.transform.GetChild(1).GetComponent<DatiGeneraliMunizione>().ordignoLocaleAttivo = true;
					this.ordignoAttivo = false;
					break;
				}
			}
		}
	}

	// Token: 0x0400149E RID: 5278
	public int tipologiaOrdigno;

	// Token: 0x0400149F RID: 5279
	public List<float> ListaValoriOrdigno;

	// Token: 0x040014A0 RID: 5280
	public GameObject munizioneUsata;

	// Token: 0x040014A1 RID: 5281
	public List<Vector3> ListaPosizioniMunizioni;

	// Token: 0x040014A2 RID: 5282
	public List<GameObject> ListaMunizioniFisiche;

	// Token: 0x040014A3 RID: 5283
	public Sprite ordignoSprite;

	// Token: 0x040014A4 RID: 5284
	public bool ordignoAttivo;

	// Token: 0x040014A5 RID: 5285
	public GameObject bersaglio;

	// Token: 0x040014A6 RID: 5286
	public bool lanciatoInFPS;

	// Token: 0x040014A7 RID: 5287
	public GameObject terzaCamera;

	// Token: 0x040014A8 RID: 5288
	public GameObject ordignoPassivoInVolo;

	// Token: 0x040014A9 RID: 5289
	public float velocitàDelVelivAlLancio;

	// Token: 0x040014AA RID: 5290
	private GameObject infoNeutreTattica;

	// Token: 0x040014AB RID: 5291
	public Vector3 zonaTarget;

	// Token: 0x040014AC RID: 5292
	public float velocitàAereo;
}
