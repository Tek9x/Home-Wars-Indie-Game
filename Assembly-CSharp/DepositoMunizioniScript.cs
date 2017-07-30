using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class DepositoMunizioniScript : MonoBehaviour
{
	// Token: 0x06000011 RID: 17 RVA: 0x00003004 File Offset: 0x00001204
	private void Start()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.frequenzaDiRifornimento = 3f;
		this.suonoRifor = base.transform.GetComponent<AudioSource>();
		this.quadRaggioRifor = base.transform.GetChild(0).gameObject;
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00003054 File Offset: 0x00001254
	private void Update()
	{
		this.RifornimentoTruppe();
		if (this.infoAlleati.GetComponent<GestioneComandanteInUI>().evidenziaAlleatiENemici)
		{
			this.quadRaggioRifor.GetComponent<MeshRenderer>().enabled = true;
		}
		else
		{
			this.quadRaggioRifor.GetComponent<MeshRenderer>().enabled = false;
		}
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000030A4 File Offset: 0x000012A4
	private void RifornimentoTruppe()
	{
		this.timerRifornimento += Time.deltaTime;
		bool flag = false;
		if (this.timerRifornimento > this.frequenzaDiRifornimento)
		{
			foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati)
			{
				if (!current.GetComponent<PresenzaAlleato>().èPerRifornimento)
				{
					float num = Vector3.Distance(base.transform.position, current.transform.position);
					if (num < this.raggioRifor)
					{
						for (int i = 0; i < current.GetComponent<PresenzaAlleato>().numeroArmi; i++)
						{
							if (current.GetComponent<PresenzaAlleato>().ListaArmi[i][6] < current.GetComponent<PresenzaAlleato>().ListaArmi[i][4])
							{
								current.GetComponent<PresenzaAlleato>().rifornimentoAttivo = true;
								flag = true;
							}
						}
					}
				}
				else
				{
					float num2 = Vector3.Distance(base.transform.position, current.transform.position);
					if (num2 < this.raggioRifor)
					{
						if (current.GetComponent<PresenzaAlleato>().volante)
						{
							if (Physics.Raycast(current.transform.position, -Vector3.up, 3.5f, 256))
							{
								if (current.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp < current.GetComponent<PresenzaAlleato>().puntiRifornimentoMax - this.puntiRiforRestaurati)
								{
									current.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp += this.puntiRiforRestaurati;
									flag = true;
								}
								else if (current.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp < current.GetComponent<PresenzaAlleato>().puntiRifornimentoMax)
								{
									current.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp = current.GetComponent<PresenzaAlleato>().puntiRifornimentoMax;
									flag = true;
								}
							}
						}
						else if (current.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp < current.GetComponent<PresenzaAlleato>().puntiRifornimentoMax - this.puntiRiforRestaurati)
						{
							current.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp += this.puntiRiforRestaurati;
							flag = true;
						}
						else if (current.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp < current.GetComponent<PresenzaAlleato>().puntiRifornimentoMax)
						{
							current.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp = current.GetComponent<PresenzaAlleato>().puntiRifornimentoMax;
							flag = true;
						}
					}
				}
				float num3 = Vector3.Distance(base.transform.position, current.transform.position);
				if (num3 < this.raggioRifor && this.infoAlleati.GetComponent<InfoMunizionamento>().ListaTipiMunizioniBaseTattica[11].GetComponent<QuantitàMunizione>().quantità > 0f && current.GetComponent<PresenzaAlleato>().vita < current.GetComponent<PresenzaAlleato>().vitaIniziale)
				{
					current.GetComponent<PresenzaAlleato>().riparazioneAttiva = true;
					this.infoAlleati.GetComponent<InfoMunizionamento>().ListaTipiMunizioniBaseTattica[11].GetComponent<QuantitàMunizione>().quantità -= 1f;
				}
			}
			this.timerRifornimento = 0f;
		}
		if (flag)
		{
			this.suonoRifor.Play();
		}
	}

	// Token: 0x04000028 RID: 40
	private GameObject quadRaggioRifor;

	// Token: 0x04000029 RID: 41
	private float frequenzaDiRifornimento;

	// Token: 0x0400002A RID: 42
	private float timerRifornimento;

	// Token: 0x0400002B RID: 43
	private GameObject infoAlleati;

	// Token: 0x0400002C RID: 44
	private AudioSource suonoRifor;

	// Token: 0x0400002D RID: 45
	public float raggioRifor;

	// Token: 0x0400002E RID: 46
	public int puntiRiforRestaurati;
}
