using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B6 RID: 182
public class GestioneSuoniCamera : MonoBehaviour
{
	// Token: 0x06000683 RID: 1667 RVA: 0x000E75B8 File Offset: 0x000E57B8
	private void Start()
	{
		this.suonoVoci = base.GetComponent<AudioSource>();
		this.ListaDiListeVoci = new List<List<AudioClip>>();
		this.ListaDiListeVoci.Add(this.ListaMovimento);
		this.ListaDiListeVoci.Add(this.ListaAttaccoDiTerra);
		this.ListaDiListeVoci.Add(this.ListaSchieraAereo);
		this.ListaDiListeVoci.Add(this.ListaRitornoAereo);
		this.ListaDiListeVoci.Add(this.ListaAttaccoAircraft);
		this.ListaDiListeVoci.Add(this.ListaAttaccoBombardieri);
		this.ListaDiListeVoci.Add(this.ListaAttaccoArtiglieria);
		this.ListaDiListeVoci.Add(this.ListaVocePerGenerale);
		this.ListaDiListeVoci.Add(this.ListaFermarsi);
		this.ListaDiListeVoci.Add(this.ListaComportFermo);
		this.ListaDiListeVoci.Add(this.ListaComportAggressivo);
		this.ListaDiListeVoci.Add(this.ListaComportDifesa);
		this.ListaDiListeVoci.Add(this.ListaComportColpisciPrimo);
		this.ListaDiListeVoci.Add(this.ListaAlleatoMorto);
		this.ListaDiListeVoci.Add(this.ListaVittoria);
		this.ListaDiListeVoci.Add(this.ListaSconfitta);
		this.ListaDiListeVoci.Add(this.ListaSenzaMunizioni);
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x000E7700 File Offset: 0x000E5900
	private void Update()
	{
		this.FunzioneVoci();
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x000E7708 File Offset: 0x000E5908
	private void FunzioneVoci()
	{
		if (this.attivaVoce)
		{
			this.attivaVoce = false;
			if (this.nonèVoce)
			{
				this.suonoVoci.Play();
			}
			else if (!this.suonoVoci.isPlaying && base.GetComponent<Selezionamento>().clickDestroCorto && !this.audioFineBatt)
			{
				float f = UnityEngine.Random.Range(0f, (float)this.ListaDiListeVoci[this.numListaVoceSelez].Count - 0.01f);
				GestoreNeutroStrategia.valoreRandomSeed++;
				UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
				this.suonoVoci.Stop();
				this.suonoVoci.clip = this.ListaDiListeVoci[this.numListaVoceSelez][Mathf.FloorToInt(f)];
				this.suonoVoci.Play();
				if (this.numListaVoceSelez == 14 || this.numListaVoceSelez == 15)
				{
					this.audioFineBatt = true;
				}
			}
			this.nonèVoce = false;
		}
	}

	// Token: 0x04001837 RID: 6199
	public List<AudioClip> ListaMovimento;

	// Token: 0x04001838 RID: 6200
	public List<AudioClip> ListaAttaccoDiTerra;

	// Token: 0x04001839 RID: 6201
	public List<AudioClip> ListaSchieraAereo;

	// Token: 0x0400183A RID: 6202
	public List<AudioClip> ListaRitornoAereo;

	// Token: 0x0400183B RID: 6203
	public List<AudioClip> ListaAttaccoAircraft;

	// Token: 0x0400183C RID: 6204
	public List<AudioClip> ListaAttaccoBombardieri;

	// Token: 0x0400183D RID: 6205
	public List<AudioClip> ListaAttaccoArtiglieria;

	// Token: 0x0400183E RID: 6206
	public List<AudioClip> ListaVocePerGenerale;

	// Token: 0x0400183F RID: 6207
	public List<AudioClip> ListaFermarsi;

	// Token: 0x04001840 RID: 6208
	public List<AudioClip> ListaComportFermo;

	// Token: 0x04001841 RID: 6209
	public List<AudioClip> ListaComportAggressivo;

	// Token: 0x04001842 RID: 6210
	public List<AudioClip> ListaComportDifesa;

	// Token: 0x04001843 RID: 6211
	public List<AudioClip> ListaComportColpisciPrimo;

	// Token: 0x04001844 RID: 6212
	public List<AudioClip> ListaAlleatoMorto;

	// Token: 0x04001845 RID: 6213
	public List<AudioClip> ListaVittoria;

	// Token: 0x04001846 RID: 6214
	public List<AudioClip> ListaSconfitta;

	// Token: 0x04001847 RID: 6215
	public List<AudioClip> ListaSenzaMunizioni;

	// Token: 0x04001848 RID: 6216
	private List<List<AudioClip>> ListaDiListeVoci;

	// Token: 0x04001849 RID: 6217
	public AudioClip suonoSchierAlleato;

	// Token: 0x0400184A RID: 6218
	public AudioClip suonoPassaggioAereoParà;

	// Token: 0x0400184B RID: 6219
	public AudioClip suonoSelezRinforzo;

	// Token: 0x0400184C RID: 6220
	public AudioClip suonoClickGenerico1;

	// Token: 0x0400184D RID: 6221
	public AudioSource suonoVoci;

	// Token: 0x0400184E RID: 6222
	public bool attivaVoce;

	// Token: 0x0400184F RID: 6223
	public bool nonèVoce;

	// Token: 0x04001850 RID: 6224
	public int numListaVoceSelez;

	// Token: 0x04001851 RID: 6225
	private bool audioFineBatt;
}
