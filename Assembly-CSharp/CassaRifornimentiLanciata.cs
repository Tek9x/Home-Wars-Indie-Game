using System;
using UnityEngine;

// Token: 0x020000B1 RID: 177
public class CassaRifornimentiLanciata : MonoBehaviour
{
	// Token: 0x06000667 RID: 1639 RVA: 0x000E4520 File Offset: 0x000E2720
	private void Start()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.frequenzaDiRifornimento = 3f;
		this.suonoRifor = base.GetComponent<AudioSource>();
		this.scrittaRifor = base.transform.GetChild(0).gameObject;
		this.puntiRifornimentoIniziali = this.puntiRifornimentoDisp;
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x000E45A4 File Offset: 0x000E27A4
	private void Update()
	{
		this.cameraAttiva = this.primaCamera.GetComponent<PrimaCamera>().oggettoCameraAttiva;
		if (!this.aTerra)
		{
			base.transform.position += -Vector3.up * 10f * Time.deltaTime;
			if (Physics.Raycast(base.transform.position, -Vector3.up, out this.hitTerreno, this.sensoreAltezza, 256))
			{
				this.aTerra = true;
				base.transform.position = this.hitTerreno.point;
			}
		}
		else if (!this.paracaduteTolto)
		{
			this.paracaduteTolto = true;
			base.GetComponent<NavMeshObstacle>().enabled = true;
			UnityEngine.Object.Destroy(base.transform.GetChild(2).gameObject);
			this.suonoRifor.Play();
		}
		else
		{
			this.RifornimentoTruppe();
			if (this.puntiRifornimentoDisp <= 0)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x000E46BC File Offset: 0x000E28BC
	private void RifornimentoTruppe()
	{
		this.timerRifornimento += Time.deltaTime;
		bool flag = false;
		if (this.timerRifornimento > this.frequenzaDiRifornimento && this.puntiRifornimentoDisp > 0)
		{
			foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati)
			{
				if (!current.GetComponent<PresenzaAlleato>().èPerRifornimento)
				{
					float num = Vector3.Distance(base.transform.position, current.transform.position);
					if (num < this.raggioDiRifornimento)
					{
						for (int i = 0; i < current.GetComponent<PresenzaAlleato>().numeroArmi; i++)
						{
							if (current.GetComponent<PresenzaAlleato>().ListaArmi[i][6] < current.GetComponent<PresenzaAlleato>().ListaArmi[i][4] && this.infoAlleati.GetComponent<InfoMunizionamento>().ListaTipiMunizioniBaseTattica[current.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[i].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().posInListaMunizioniBase].GetComponent<QuantitàMunizione>().quantità > 0f)
							{
								current.GetComponent<PresenzaAlleato>().rifornimentoAttivo = true;
								this.puntiRifornimentoDisp--;
								flag = true;
							}
						}
					}
				}
				float num2 = Vector3.Distance(base.transform.position, current.transform.position);
				if (num2 < this.raggioDiRifornimento && this.infoAlleati.GetComponent<InfoMunizionamento>().ListaTipiMunizioniBaseTattica[11].GetComponent<QuantitàMunizione>().quantità > 0f && current.GetComponent<PresenzaAlleato>().vita < current.GetComponent<PresenzaAlleato>().vitaIniziale)
				{
					current.GetComponent<PresenzaAlleato>().riparazioneAttiva = true;
					this.puntiRifornimentoDisp--;
					this.infoAlleati.GetComponent<InfoMunizionamento>().ListaTipiMunizioniBaseTattica[11].GetComponent<QuantitàMunizione>().quantità -= 1f;
				}
			}
			this.timerRifornimento = 0f;
		}
		if (flag)
		{
			this.suonoRifor.clip = this.rumoreRifornimento;
			this.suonoRifor.volume = 0.28f;
			this.suonoRifor.Play();
		}
		this.scrittaRifor.GetComponent<TextMesh>().text = this.puntiRifornimentoDisp.ToString() + "/" + this.puntiRifornimentoIniziali.ToString();
		Vector3 normalized = (base.transform.position - this.cameraAttiva.transform.position).normalized;
		this.scrittaRifor.transform.forward = normalized;
		float num3 = Vector3.Distance(base.transform.position, this.cameraAttiva.transform.position);
		float num4 = num3 / 400f;
		float num5 = 0.2f;
		if (num4 > num5)
		{
			this.scrittaRifor.transform.localScale = new Vector3(num4, num4, num4);
		}
		else
		{
			this.scrittaRifor.transform.localScale = new Vector3(num5, num5, num5);
		}
	}

	// Token: 0x040017ED RID: 6125
	private GameObject infoAlleati;

	// Token: 0x040017EE RID: 6126
	private GameObject primaCamera;

	// Token: 0x040017EF RID: 6127
	private GameObject cameraAttiva;

	// Token: 0x040017F0 RID: 6128
	private GameObject infoNeutreTattica;

	// Token: 0x040017F1 RID: 6129
	public float raggioDiRifornimento;

	// Token: 0x040017F2 RID: 6130
	public int puntiRifornimentoDisp;

	// Token: 0x040017F3 RID: 6131
	private int puntiRifornimentoIniziali;

	// Token: 0x040017F4 RID: 6132
	public float sensoreAltezza;

	// Token: 0x040017F5 RID: 6133
	public bool aTerra;

	// Token: 0x040017F6 RID: 6134
	private bool paracaduteTolto;

	// Token: 0x040017F7 RID: 6135
	private RaycastHit hitTerreno;

	// Token: 0x040017F8 RID: 6136
	private float timerRifornimento;

	// Token: 0x040017F9 RID: 6137
	private float frequenzaDiRifornimento;

	// Token: 0x040017FA RID: 6138
	private AudioSource suonoRifor;

	// Token: 0x040017FB RID: 6139
	public AudioClip rumoreRifornimento;

	// Token: 0x040017FC RID: 6140
	private GameObject scrittaRifor;
}
