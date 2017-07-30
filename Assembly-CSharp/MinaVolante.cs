using System;
using UnityEngine;

// Token: 0x020000FF RID: 255
public class MinaVolante : MonoBehaviour
{
	// Token: 0x06000818 RID: 2072 RVA: 0x0011AC1C File Offset: 0x00118E1C
	private void Start()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.corpoMina = base.transform.GetChild(2).gameObject;
		this.secondoPuntoEsplosione = base.transform.GetChild(4).gameObject;
		this.quotaDecisa = 60f;
		this.quotaMinima = 40f;
		this.quotaMassima = 320f;
		this.passoDiCambioQuota = 20f;
		this.layerNavigazione = 256;
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x0011ACCC File Offset: 0x00118ECC
	private void Update()
	{
		if (!this.esplosioneAttiva)
		{
			if (base.GetComponent<PresenzaTrappola>().disposta)
			{
				this.Innesco();
				this.Quota();
			}
		}
		else
		{
			this.Esplosione();
		}
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x0011AD0C File Offset: 0x00118F0C
	private void Innesco()
	{
		foreach (GameObject current in base.GetComponent<ColliderAreaTrappole>().ListaAreaTrappola)
		{
			if (current != null)
			{
				float num = Vector3.Distance(current.transform.position, base.transform.position);
				if (num < this.distanzaInnesco)
				{
					this.esplosioneAttiva = true;
					base.GetComponent<AudioSource>().Play();
				}
			}
		}
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x0011ADB8 File Offset: 0x00118FB8
	private void Esplosione()
	{
		this.timerPostEspl += Time.deltaTime;
		if (!this.dannoEffettuato)
		{
			foreach (GameObject current in base.GetComponent<ColliderAreaTrappole>().ListaAreaTrappola)
			{
				if (current != null)
				{
					float num = current.GetComponent<PresenzaNemico>().armatura - base.GetComponent<PresenzaTrappola>().penetrazione;
					if (num <= 0f)
					{
						current.GetComponent<PresenzaNemico>().vita -= base.GetComponent<PresenzaTrappola>().danno;
					}
					else
					{
						float num2 = num - base.GetComponent<PresenzaTrappola>().danno;
						if (num2 < 0f)
						{
							current.GetComponent<PresenzaNemico>().vita += num2;
						}
					}
				}
			}
			this.dannoEffettuato = true;
			base.GetComponent<ParticleSystem>().Play();
			this.secondoPuntoEsplosione.GetComponent<ParticleSystem>().Play();
			this.corpoMina.GetComponent<MeshRenderer>().enabled = false;
		}
		if (this.timerPostEspl > 3f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x0011AF04 File Offset: 0x00119104
	private void Quota()
	{
		if (this.primaCamera.GetComponent<Selezionamento>().trappolaSelez == base.gameObject && this.infoAlleati.GetComponent<InfoGenericheAlleati>().aggiorQuota)
		{
			if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().coeffMoltiplPerQuota > 0f)
			{
				if (this.quotaDecisa < this.quotaMassima)
				{
					this.quotaDecisa += this.passoDiCambioQuota;
				}
			}
			else if (this.quotaDecisa > this.quotaMinima)
			{
				this.quotaDecisa -= this.passoDiCambioQuota;
			}
			this.infoAlleati.GetComponent<InfoGenericheAlleati>().aggiorQuota = false;
			this.aggiornaPosizioneQuota = true;
		}
		if (this.aggiornaPosizioneQuota)
		{
			this.quotaPresente = base.transform.position.y;
			if (this.quotaDecisa > this.quotaPresente && !Physics.Raycast(base.transform.position, Vector3.up, out this.hitContatto, 30f, this.layerNavigazione))
			{
				base.transform.position += Vector3.up * 20f * Time.deltaTime;
			}
			if (this.quotaDecisa < this.quotaPresente && !Physics.Raycast(base.transform.position, -Vector3.up, out this.hitContatto, 10f, this.layerNavigazione))
			{
				base.transform.position += -Vector3.up * 20f * Time.deltaTime;
			}
			if (this.quotaPresente > this.quotaDecisa - 1f && this.quotaPresente < this.quotaDecisa + 1f)
			{
				this.aggiornaPosizioneQuota = false;
			}
		}
	}

	// Token: 0x04001E1F RID: 7711
	private GameObject infoAlleati;

	// Token: 0x04001E20 RID: 7712
	private GameObject infoNeutreTattica;

	// Token: 0x04001E21 RID: 7713
	private GameObject primaCamera;

	// Token: 0x04001E22 RID: 7714
	public float distanzaInnesco;

	// Token: 0x04001E23 RID: 7715
	private GameObject corpoMina;

	// Token: 0x04001E24 RID: 7716
	private GameObject secondoPuntoEsplosione;

	// Token: 0x04001E25 RID: 7717
	private bool esplosioneAttiva;

	// Token: 0x04001E26 RID: 7718
	private bool dannoEffettuato;

	// Token: 0x04001E27 RID: 7719
	private float timerPostEspl;

	// Token: 0x04001E28 RID: 7720
	public float quotaDecisa;

	// Token: 0x04001E29 RID: 7721
	private float quotaPresente;

	// Token: 0x04001E2A RID: 7722
	private float quotaMinima;

	// Token: 0x04001E2B RID: 7723
	private float quotaMassima;

	// Token: 0x04001E2C RID: 7724
	private float passoDiCambioQuota;

	// Token: 0x04001E2D RID: 7725
	private bool aggiornaPosizioneQuota;

	// Token: 0x04001E2E RID: 7726
	private RaycastHit hitContatto;

	// Token: 0x04001E2F RID: 7727
	private int layerNavigazione;
}
