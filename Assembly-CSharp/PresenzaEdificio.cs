using System;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class PresenzaEdificio : MonoBehaviour
{
	// Token: 0x06000075 RID: 117 RVA: 0x00017F18 File Offset: 0x00016118
	private void Update()
	{
		if (this.aggiornaFumo)
		{
			this.aggiornaFumo = false;
			if (base.transform.childCount > 1 && base.transform.GetChild(1).gameObject.name == "fumo")
			{
				for (int i = 0; i < base.transform.GetChild(1).childCount; i++)
				{
					if (this.èAcceso == 1)
					{
						base.transform.GetChild(1).GetChild(i).GetComponent<ParticleSystem>().Stop();
					}
					else
					{
						base.transform.GetChild(1).GetChild(i).GetComponent<ParticleSystem>().Play();
					}
				}
			}
		}
	}

	// Token: 0x040002CF RID: 719
	public string nomeEdificio;

	// Token: 0x040002D0 RID: 720
	public Sprite immagineEdificio;

	// Token: 0x040002D1 RID: 721
	public int tipoEdificio;

	// Token: 0x040002D2 RID: 722
	public GameObject oggettoDescrizione;

	// Token: 0x040002D3 RID: 723
	public float costoCostruzioneInPlastica;

	// Token: 0x040002D4 RID: 724
	public float costoCostruzioneInMetallo;

	// Token: 0x040002D5 RID: 725
	public int èAcceso;

	// Token: 0x040002D6 RID: 726
	public float consumoPlasticaGrezza;

	// Token: 0x040002D7 RID: 727
	public float consumoPlasticaRaffinata;

	// Token: 0x040002D8 RID: 728
	public float consumoMetalloGrezzo;

	// Token: 0x040002D9 RID: 729
	public float consumoMetalloRaffinato;

	// Token: 0x040002DA RID: 730
	public float consumoEnergiaGrezza;

	// Token: 0x040002DB RID: 731
	public float consumoEnergiaRaffinata;

	// Token: 0x040002DC RID: 732
	public float consumoIncendiarioGrezzo;

	// Token: 0x040002DD RID: 733
	public float consumoIncendiarioRaffinato;

	// Token: 0x040002DE RID: 734
	public float consumoTossicoGrezzo;

	// Token: 0x040002DF RID: 735
	public float consumoTossicoRaffinato;

	// Token: 0x040002E0 RID: 736
	public float produzionePlasticaGrezza;

	// Token: 0x040002E1 RID: 737
	public float produzionePlasticaRaffinata;

	// Token: 0x040002E2 RID: 738
	public float produzioneMetalloGrezzo;

	// Token: 0x040002E3 RID: 739
	public float produzioneMetalloRaffinato;

	// Token: 0x040002E4 RID: 740
	public float produzioneEnergiaGrezza;

	// Token: 0x040002E5 RID: 741
	public float produzioneEnergiaRaffinata;

	// Token: 0x040002E6 RID: 742
	public float produzioneIncendiarioGrezzo;

	// Token: 0x040002E7 RID: 743
	public float produzioneIncendiarioRaffinato;

	// Token: 0x040002E8 RID: 744
	public float produzioneTossicoGrezzo;

	// Token: 0x040002E9 RID: 745
	public float produzioneTossicoRaffinato;

	// Token: 0x040002EA RID: 746
	public float produzioneEsperienza;

	// Token: 0x040002EB RID: 747
	public float prodPercentualeDaRifiuti;

	// Token: 0x040002EC RID: 748
	public bool aggiornaFumo;
}
