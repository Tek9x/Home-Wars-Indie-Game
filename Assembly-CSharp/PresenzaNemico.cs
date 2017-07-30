using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F3 RID: 243
public class PresenzaNemico : MonoBehaviour
{
	// Token: 0x060007CF RID: 1999 RVA: 0x00115648 File Offset: 0x00113848
	private void Start()
	{
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.secondaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[1];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.varieMappaLocale = GameObject.FindGameObjectWithTag("VarieMappaLocale");
		this.cerchioSel = base.transform.GetChild(0).gameObject;
		if (this.insettoVolante)
		{
			this.cerchioSel2 = base.transform.GetChild(1).gameObject;
			this.testoVita = base.transform.GetChild(2).gameObject;
		}
		else
		{
			this.testoVita = base.transform.GetChild(1).gameObject;
		}
		this.tipoBattaglia = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().tipoBattaglia;
		if (this.tipoBattaglia == 0)
		{
			this.obbiettivo = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato;
			this.posObbiettivo = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.transform.position;
		}
		else if (this.tipoBattaglia == 4)
		{
			this.obbiettivo = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply;
			this.posObbiettivo = this.obbiettivo.transform.position;
		}
		else if (this.tipoBattaglia == 5)
		{
			this.obbiettivo = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().satellite;
		}
		this.dimInizCerchioSel = this.cerchioSel.transform.localScale.x;
		this.materialeSelezione = this.infoAlleati.GetComponent<GestioneComandanteInUI>().coloreSelInsetti;
		this.materialeEvidenziazione = this.infoAlleati.GetComponent<GestioneComandanteInUI>().coloreEvidInsetti;
		this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici.Add(base.gameObject);
		this.confermaInsettoVolante = this.insettoVolante;
		this.layerSelezione = 4384000;
		this.vita = this.vita * (1f + this.IANemico.GetComponent<InfoGenericheNemici>().bonusSurvivalSalute) * ((1f + GestoreNeutroStrategia.valVitaStagionaleNemici / 100f) * (this.IANemico.GetComponent<InfoGenericheNemici>().fattoreVitaNemici / 100f));
		this.vitaIniziale = this.vita;
		this.fattoreAttacco = (1f + this.IANemico.GetComponent<InfoGenericheNemici>().bonusSurvivalAttacco) * ((1f + GestoreNeutroStrategia.valAttaccoStagionaleNemici / 100f) * (this.IANemico.GetComponent<InfoGenericheNemici>().fattoreAttaccoNemici / 100f));
		this.danno1 *= this.fattoreAttacco;
		this.danno2 *= this.fattoreAttacco;
		this.dannoVeleno *= this.fattoreAttacco;
		this.muoviti = true;
		foreach (GameObject current in this.ListaPartiVisibili)
		{
			current.GetComponent<SkinnedMeshRenderer>().enabled = false;
		}
		this.audioInsetto = base.GetComponent<AudioSource>();
		this.tempoSuono = 1f;
		this.maxDistanzaAudio = this.audioInsetto.maxDistance;
		this.ListaSuoniInsCopiata = new List<AudioClip>();
		for (int i = 0; i < this.IANemico.GetComponent<InfoGenericheNemici>().ListaSuoniInsetti.Count; i++)
		{
			this.ListaSuoniInsCopiata.Add(this.IANemico.GetComponent<InfoGenericheNemici>().ListaSuoniInsetti[i]);
		}
		if (base.GetComponent<NavMeshAgent>())
		{
			this.insettoNav = base.GetComponent<NavMeshAgent>();
		}
		if (this.tipoBattaglia == 7)
		{
			this.tipoObbiettivo = 0;
			this.raggioRicercaLocale = this.IANemico.GetComponent<IANemicoTattica>().raggioPerRicercaDistDaLuogo;
		}
		else if (this.tipoBattaglia == 0 || this.tipoBattaglia == 3 || this.tipoBattaglia == 4 || this.tipoBattaglia == 5 || this.tipoBattaglia == 6)
		{
			this.tipoObbiettivo = 1;
		}
		else if (this.tipoBattaglia == 2)
		{
			this.tipoObbiettivo = 2;
		}
		this.layerNemico = 132096;
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x00115AF0 File Offset: 0x00113CF0
	private void Update()
	{
		if (this.confermaInsettoVolante)
		{
			this.insettoVolante = true;
		}
		else
		{
			this.insettoVolante = false;
		}
		this.cameraAttiva = this.primaCamera.GetComponent<PrimaCamera>().oggettoCameraAttiva;
		this.Vita();
		this.Visibilità();
		this.Evidenziazione();
		this.PuntiChiaveInsetto();
		this.ControlloAudio();
		this.FunzioneCmportamento();
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x00115B58 File Offset: 0x00113D58
	private void Vita()
	{
		if (this.vita <= 0f || this.morto)
		{
			this.morto = true;
			if (!this.giàMorto)
			{
				this.giàMorto = true;
				GestoreNeutroTattica.numNemiciMorti++;
				this.IANemico.GetComponent<InfoGenericheNemici>().totalePerExpBatt += this.numPerSommaExp;
			}
			this.timerMorte += Time.deltaTime;
		}
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00115BD4 File Offset: 0x00113DD4
	private void Visibilità()
	{
		this.timerVisibilità += Time.deltaTime;
		float num = 3f + UnityEngine.Random.Range(-0.5f, 0.5f);
		if (this.timerVisibilità > num)
		{
			foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati)
			{
				if (current != null)
				{
					float num2 = Vector3.Distance(base.transform.position, current.transform.position);
					this.elementiVisibili = 0;
					if (num2 < current.GetComponent<PresenzaAlleato>().raggioVisivo)
					{
						this.elementiVisibili++;
						this.èStatoVisto = true;
						break;
					}
				}
			}
			if (this.elementiVisibili == 0)
			{
				this.èStatoVisto = false;
			}
			if (this.èStatoVisto)
			{
				foreach (GameObject current2 in this.ListaPartiVisibili)
				{
					current2.GetComponent<SkinnedMeshRenderer>().enabled = true;
				}
			}
			else
			{
				foreach (GameObject current3 in this.ListaPartiVisibili)
				{
					current3.GetComponent<SkinnedMeshRenderer>().enabled = false;
				}
			}
			this.timerVisibilità = 0f;
		}
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x00115DB4 File Offset: 0x00113FB4
	private void Evidenziazione()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva != 3)
			{
				if (this.IANemico.GetComponent<GestioneUIPerNemici>().nemicoPerPannelloInfo && this.IANemico.GetComponent<GestioneUIPerNemici>().nemicoPerPannelloInfo == base.gameObject)
				{
					this.nemicoEvidenziato = true;
				}
				else
				{
					this.nemicoEvidenziato = false;
				}
			}
			else
			{
				this.nemicoEvidenziato = false;
			}
		}
		if (this.nemicoEvidenziato || (this.infoAlleati.GetComponent<GestioneComandanteInUI>().evidenziaAlleatiENemici && this.èStatoVisto))
		{
			this.cerchioSel.GetComponent<MeshRenderer>().enabled = true;
			this.testoVita.GetComponent<MeshRenderer>().enabled = true;
			if (this.insettoVolante)
			{
				this.cerchioSel2.GetComponent<MeshRenderer>().enabled = true;
			}
			if (this.nemicoEvidenziato)
			{
				this.cerchioSel.GetComponent<MeshRenderer>().material = this.materialeSelezione;
				if (this.insettoVolante)
				{
					this.cerchioSel2.GetComponent<MeshRenderer>().material = this.materialeSelezione;
				}
			}
			else if (this.infoAlleati.GetComponent<GestioneComandanteInUI>().evidenziaAlleatiENemici)
			{
				this.cerchioSel.GetComponent<MeshRenderer>().material = this.materialeEvidenziazione;
				if (this.insettoVolante)
				{
					this.cerchioSel2.GetComponent<MeshRenderer>().material = this.materialeEvidenziazione;
				}
			}
			float num;
			if (this.cameraAttiva == this.primaCamera || this.cameraAttiva == this.terzaCamera)
			{
				num = Vector3.Distance(base.transform.position, this.cameraAttiva.transform.position);
			}
			else
			{
				num = this.secondaCamera.GetComponent<Camera>().orthographicSize * 1.8f;
			}
			float num2 = num / 200f * (this.dimInizCerchioSel * 1f + 1f);
			if (num2 < this.dimInizCerchioSel)
			{
				this.cerchioSel.transform.localScale = new Vector3(this.dimInizCerchioSel, this.dimInizCerchioSel, 0f);
				if (this.insettoVolante)
				{
					this.cerchioSel2.transform.localScale = new Vector3(this.dimInizCerchioSel, this.dimInizCerchioSel, 0f);
				}
			}
			else
			{
				this.cerchioSel.transform.localScale = new Vector3(num2, num2, 0f);
				if (this.insettoVolante)
				{
					this.cerchioSel.transform.localScale = new Vector3(num2, num2, 0f);
				}
			}
			if (this.vita > 0f)
			{
				this.testoVita.GetComponent<TextMesh>().text = (this.vita * 100f / this.vitaIniziale).ToString("F1") + "%";
			}
			else
			{
				this.testoVita.GetComponent<TextMesh>().text = "0%";
			}
			Vector3 normalized = (base.transform.position - this.cameraAttiva.transform.position).normalized;
			this.testoVita.transform.forward = normalized;
			float num3 = num / 1000f * 1.5f;
			float num4 = 0.2f;
			if (num3 > num4)
			{
				this.testoVita.transform.localScale = new Vector3(num3, num3, num3);
			}
			else
			{
				this.testoVita.transform.localScale = new Vector3(num4, num4, num4);
			}
		}
		else
		{
			this.cerchioSel.GetComponent<MeshRenderer>().enabled = false;
			this.testoVita.GetComponent<MeshRenderer>().enabled = false;
			if (this.insettoVolante)
			{
				this.cerchioSel2.GetComponent<MeshRenderer>().enabled = false;
			}
		}
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x001161A4 File Offset: 0x001143A4
	private void PuntiChiaveInsetto()
	{
		this.centroInsetto = base.transform.TransformPoint(base.GetComponent<CapsuleCollider>().center);
		this.centroBaseInsetto = base.transform.GetChild(0).transform.position;
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x001161EC File Offset: 0x001143EC
	private void ControlloAudio()
	{
		this.timerSuono += Time.deltaTime;
		if (this.timerSuono > this.tempoSuono)
		{
			float num = Vector3.Distance(this.primaCamera.GetComponent<PrimaCamera>().oggettoCameraAttiva.transform.position, base.transform.position);
			if (num < this.maxDistanzaAudio)
			{
				this.audioInsetto.enabled = true;
				float f = UnityEngine.Random.Range(0f, (float)this.ListaSuoniInsCopiata.Count - 0.01f);
				GestoreNeutroStrategia.valoreRandomSeed++;
				UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
				this.audioInsetto.Stop();
				this.audioInsetto.clip = this.ListaSuoniInsCopiata[Mathf.FloorToInt(f)];
				this.audioInsetto.Play();
				this.tempoSuono = UnityEngine.Random.Range(10f, 16f);
				this.timerSuono = 0f;
			}
			else
			{
				this.audioInsetto.enabled = false;
			}
		}
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x001162F8 File Offset: 0x001144F8
	private void FunzioneCmportamento()
	{
		this.timerAggRicercaObb += Time.deltaTime;
		if (this.tipoComportamento == 0 || this.comportInSospeso)
		{
			if (this.tipoBattaglia == 0)
			{
				this.bersaglio = this.obbiettivo;
			}
			else if (this.tipoBattaglia == 4)
			{
				this.bersaglio = this.obbiettivo;
			}
			else if (this.tipoBattaglia == 5)
			{
				this.posObbiettivo = this.obbiettivo.transform.position;
				this.bersaglio = this.obbiettivo;
			}
			else if (this.tipoBattaglia == 6)
			{
				if (this.obbiettivo == null || (this.obbiettivo != null && this.obbiettivo.GetComponent<ObbiettivoTatticoScript>().vita <= 0f))
				{
					for (int i = 0; i < this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamionPerConvoglio.Count; i++)
					{
						if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamionPerConvoglio[i] != null && this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamionPerConvoglio[i].GetComponent<ObbiettivoTatticoScript>().vita > 0f)
						{
							this.obbiettivo = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamionPerConvoglio[i];
							this.posObbiettivo = this.obbiettivo.transform.position;
							break;
						}
					}
				}
				else
				{
					this.posObbiettivo = this.obbiettivo.transform.position;
				}
			}
			else if (this.tipoBattaglia == 7)
			{
				if (this.obbiettivo == null || (this.obbiettivo != null && this.obbiettivo.GetComponent<ObbiettivoTatticoScript>().vita <= 0f))
				{
					for (int j = 0; j < this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaPanePerConvoglio.Count; j++)
					{
						if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaPanePerConvoglio[j] != null && this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaPanePerConvoglio[j].GetComponent<ObbiettivoTatticoScript>().vita > 0f)
						{
							this.obbiettivo = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaPanePerConvoglio[j];
							this.posObbiettivo = this.obbiettivo.transform.position;
							break;
						}
					}
				}
				else
				{
					this.posObbiettivo = this.obbiettivo.transform.position;
				}
			}
		}
		if (this.tipoComportamento == 0)
		{
			if (this.tipoObbiettivo == 0)
			{
				this.FunzioneDifendiObbiettivo();
			}
			else if (this.tipoObbiettivo == 1)
			{
				this.FunzioneAttaccaObbiettivo();
			}
			else if (this.tipoObbiettivo == 2)
			{
				this.FunzioneConquistaPunti();
			}
		}
		else if (this.tipoComportamento == 1)
		{
			this.timerAggRicercaAttCasuale += Time.deltaTime;
			if (this.comportInSospeso)
			{
				if (this.tipoObbiettivo == 0)
				{
					this.FunzioneDifendiObbiettivo();
				}
				else if (this.tipoObbiettivo == 1)
				{
					this.FunzioneAttaccaObbiettivo();
				}
				else if (this.tipoObbiettivo == 2)
				{
					this.FunzioneConquistaPunti();
				}
				if (this.timerAggRicercaAttCasuale > 5f)
				{
					this.FunzioneAttaccoCasualeLimitato();
				}
			}
			else if (this.timerAggRicercaAttCasuale > 5f)
			{
				this.FunzioneAttaccoCasualeLimitato();
			}
		}
		else if (this.tipoComportamento == 2)
		{
			this.FunzioneAttaccoCasualeTotale();
		}
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x001166B4 File Offset: 0x001148B4
	private void FunzioneDifendiObbiettivo()
	{
		float num = Vector3.Distance(base.transform.position, this.posObbiettivo);
		if (num > this.raggioRicercaLocale)
		{
			this.inLocale = false;
		}
		else
		{
			this.inLocale = true;
		}
		if (!this.inLocale)
		{
			if (this.bersaglio == null)
			{
				float num2 = Vector3.Distance(base.transform.position, this.destinazione);
				if (num2 < 10f)
				{
					this.destDecisa = false;
				}
				if (this.insettoVolante)
				{
					float num3 = UnityEngine.Random.Range(4f, 20f);
					this.destinazione = new Vector3(this.posObbiettivo.x, this.posObbiettivo.y + num3, this.posObbiettivo.z);
				}
				else if (!this.destDecisa && this.insettoNav.isOnNavMesh)
				{
					for (int i = 0; i < 10; i++)
					{
						float num4 = UnityEngine.Random.Range(-this.raggioRicercaLocale, this.raggioRicercaLocale);
						if (num4 < 80f)
						{
							num4 += 70f;
						}
						float value = UnityEngine.Random.value;
						if (value < 0.5f)
						{
							num4 *= -1f;
						}
						GestoreNeutroStrategia.valoreRandomSeed++;
						UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
						float num5 = UnityEngine.Random.Range(-this.raggioRicercaLocale, this.raggioRicercaLocale);
						if (num5 < 80f)
						{
							num5 += 70f;
						}
						value = UnityEngine.Random.value;
						if (value < 0.5f)
						{
							num5 *= -1f;
						}
						this.destinazione = new Vector3(this.posObbiettivo.x + num4, this.posObbiettivo.y, this.posObbiettivo.z + num5);
						this.insettoNav.SetDestination(this.destinazione);
						NavMeshPath navMeshPath = new NavMeshPath();
						this.insettoNav.CalculatePath(this.destinazione, navMeshPath);
						if (navMeshPath.status != NavMeshPathStatus.PathInvalid)
						{
							this.destDecisa = true;
							break;
						}
					}
				}
			}
		}
		else
		{
			if (this.bersaglio == null)
			{
				float num6 = Vector3.Distance(base.transform.position, this.destinazione);
				if (num6 < 10f)
				{
					this.destDecisa = false;
				}
				if (!this.destDecisa)
				{
					this.destDecisa = true;
					if (this.insettoVolante)
					{
						float num7 = UnityEngine.Random.Range(-this.raggioRicercaLocale, this.raggioRicercaLocale);
						if (num7 < 80f)
						{
							num7 += 70f;
						}
						float value2 = UnityEngine.Random.value;
						if (value2 < 0.5f)
						{
							num7 *= -1f;
						}
						GestoreNeutroStrategia.valoreRandomSeed++;
						UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
						float num8 = UnityEngine.Random.Range(-this.raggioRicercaLocale, this.raggioRicercaLocale);
						if (num8 < 80f)
						{
							num8 += 70f;
						}
						value2 = UnityEngine.Random.value;
						if (value2 < 0.5f)
						{
							num8 *= -1f;
						}
						float num9 = UnityEngine.Random.Range(4f, 20f);
						this.destinazione = new Vector3(this.posObbiettivo.x + num7, this.posObbiettivo.y + num9, this.posObbiettivo.z + num8);
					}
					else if (this.insettoNav.isOnNavMesh)
					{
						for (int j = 0; j < 10; j++)
						{
							float num10 = UnityEngine.Random.Range(-this.raggioRicercaLocale, this.raggioRicercaLocale);
							if (num10 < 80f)
							{
								num10 += 70f;
							}
							float value3 = UnityEngine.Random.value;
							if (value3 < 0.5f)
							{
								num10 *= -1f;
							}
							GestoreNeutroStrategia.valoreRandomSeed++;
							UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
							float num11 = UnityEngine.Random.Range(-this.raggioRicercaLocale, this.raggioRicercaLocale);
							if (num11 < 80f)
							{
								num11 += 70f;
							}
							value3 = UnityEngine.Random.value;
							if (value3 < 0.5f)
							{
								num11 *= -1f;
							}
							this.destinazione = new Vector3(this.posObbiettivo.x + num10, this.posObbiettivo.y, this.posObbiettivo.z + num11);
							this.insettoNav.SetDestination(this.destinazione);
							NavMeshPath navMeshPath2 = new NavMeshPath();
							this.insettoNav.CalculatePath(this.destinazione, navMeshPath2);
							if (navMeshPath2.status != NavMeshPathStatus.PathInvalid)
							{
								break;
							}
						}
					}
				}
			}
			int count = this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb.Count;
			if (count > 0)
			{
				this.destDecisa = false;
				if (this.timerAggRicercaObb > 3f && this.bersaglio == null)
				{
					this.timerAggRicercaObb = 0f;
					GestoreNeutroStrategia.valoreRandomSeed++;
					UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
					GameObject gameObject = null;
					if (this.insettoVolante)
					{
						if (this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb.Count > 0)
						{
							for (int k = 0; k < this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb.Count; k++)
							{
								if (this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb[k] && this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb[k].GetComponent<PresenzaAlleato>().volante)
								{
									gameObject = this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb[k];
								}
							}
						}
					}
					else if (this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb.Count > 0)
					{
						for (int l = 0; l < this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb.Count; l++)
						{
							if (this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb[l])
							{
								gameObject = this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb[l];
							}
						}
					}
					if (gameObject)
					{
						bool flag = false;
						for (int m = 0; m < gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco.Count; m++)
						{
							if (gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco[m] == null)
							{
								this.bersaglio = gameObject;
								gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco[m] = base.gameObject;
								break;
							}
							if (gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco[gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco.Count - 1] != null)
							{
								flag = true;
							}
						}
						if (flag)
						{
							bool flag2 = false;
							GestoreNeutroStrategia.valoreRandomSeed++;
							UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
							int num12 = UnityEngine.Random.Range(0, count - 1);
							for (int n = num12; n < count; n++)
							{
								bool flag3 = false;
								if (this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb[n])
								{
									if (this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb[n].GetComponent<PresenzaAlleato>().volante)
									{
										if (this.insettoVolante)
										{
											flag3 = true;
										}
									}
									else
									{
										flag3 = true;
									}
								}
								if (flag3)
								{
									for (int num13 = 0; num13 < this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb[n].GetComponent<PresenzaAlleato>().ListaNemInAttacco.Count; num13++)
									{
										if (this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb[n].GetComponent<PresenzaAlleato>().ListaNemInAttacco[num13] == null)
										{
											this.bersaglio = this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb[n];
											this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb[n].GetComponent<PresenzaAlleato>().ListaNemInAttacco[num13] = base.gameObject;
											break;
										}
									}
									if (n == count - 1)
									{
										flag2 = true;
									}
								}
							}
							if (flag2)
							{
								for (int num14 = 0; num14 < num12; num14++)
								{
									bool flag4 = false;
									if (this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb[num14])
									{
										if (this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb[num14].GetComponent<PresenzaAlleato>().volante)
										{
											if (this.insettoVolante)
											{
												flag4 = true;
											}
										}
										else
										{
											flag4 = true;
										}
									}
									if (flag4)
									{
										for (int num15 = 0; num15 < gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco.Count; num15++)
										{
											if (this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb[num14].GetComponent<PresenzaAlleato>().ListaNemInAttacco[num15] == null)
											{
												this.bersaglio = this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb[num14];
												this.IANemico.GetComponent<IANemicoTattica>().ListaAlleatiSuObb[num14].GetComponent<PresenzaAlleato>().ListaNemInAttacco[num15] = base.gameObject;
												break;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x00117050 File Offset: 0x00115250
	private void FunzioneAttaccaObbiettivo()
	{
		if (this.tipoBattaglia == 5)
		{
			if (!this.allontanamentoPerAtt)
			{
				this.destinazione = this.posObbiettivo;
			}
		}
		else if (this.tipoBattaglia == 6)
		{
			if (!this.allontanamentoPerAtt)
			{
				this.destinazione = this.posObbiettivo;
			}
		}
		else
		{
			if (this.insettoVolante && !this.allontanamentoPerAtt && this.primaMetaRaggiunta)
			{
				this.destinazione = this.posObbiettivo;
			}
			else
			{
				if (this.sparpagliato && !this.primaMetaRaggiunta)
				{
					if (!this.destDecisa)
					{
						this.destDecisa = true;
						if (this.insettoVolante)
						{
							GestoreNeutroStrategia.valoreRandomSeed++;
							UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
							float num = (float)UnityEngine.Random.Range(-100, 100);
							GestoreNeutroStrategia.valoreRandomSeed++;
							UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
							float num2 = (float)UnityEngine.Random.Range(-100, 100);
							int index = UnityEngine.Random.Range(0, 4);
							Vector3 position = this.varieMappaLocale.transform.GetChild(0).GetChild(index).transform.position;
							float num3 = UnityEngine.Random.Range(50f, 100f);
							this.destinazione = new Vector3(position.x + num, position.y + num3, num2 + num2);
						}
						else if (this.insettoNav.isOnNavMesh)
						{
							for (int i = 0; i < 5; i++)
							{
								GestoreNeutroStrategia.valoreRandomSeed++;
								UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
								float num4 = (float)UnityEngine.Random.Range(-80, 80);
								GestoreNeutroStrategia.valoreRandomSeed++;
								UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
								float num5 = (float)UnityEngine.Random.Range(-80, 80);
								int index2 = UnityEngine.Random.Range(0, 4);
								Vector3 position2 = this.varieMappaLocale.transform.GetChild(0).GetChild(index2).transform.position;
								this.destinazione = new Vector3(position2.x + num4, position2.y, position2.z + num5);
								this.insettoNav.SetDestination(this.destinazione);
								NavMeshPath navMeshPath = new NavMeshPath();
								this.insettoNav.CalculatePath(this.destinazione, navMeshPath);
								if (navMeshPath.status != NavMeshPathStatus.PathInvalid)
								{
									break;
								}
								if (i == 4)
								{
									this.destinazione = this.posObbiettivo;
									this.primaMetaRaggiunta = true;
								}
							}
						}
					}
					if (Vector3.Distance(this.centroBaseInsetto, this.destinazione) < 10f)
					{
						this.primaMetaRaggiunta = true;
						this.destDecisa = false;
					}
				}
				if ((!this.sparpagliato || (this.sparpagliato && this.primaMetaRaggiunta)) && !this.secondaMetaRaggiunta)
				{
					if (!this.destDecisa)
					{
						this.destDecisa = true;
						if (this.insettoVolante)
						{
							GestoreNeutroStrategia.valoreRandomSeed++;
							UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
							float x = (float)UnityEngine.Random.Range(-150, 150);
							GestoreNeutroStrategia.valoreRandomSeed++;
							UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
							float z = (float)UnityEngine.Random.Range(-150, 150);
							float y = UnityEngine.Random.Range(50f, 100f);
							this.destinazione = new Vector3(x, y, z);
						}
						else if (this.insettoNav.isOnNavMesh)
						{
							for (int j = 0; j < 5; j++)
							{
								GestoreNeutroStrategia.valoreRandomSeed++;
								UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
								float num6 = (float)UnityEngine.Random.Range(-80, 80);
								GestoreNeutroStrategia.valoreRandomSeed++;
								UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
								float num7 = (float)UnityEngine.Random.Range(-80, 80);
								this.destinazione = new Vector3(this.posObbiettivo.x + num6, base.transform.position.y, this.posObbiettivo.z + num7);
								this.insettoNav.SetDestination(this.destinazione);
								NavMeshPath navMeshPath2 = new NavMeshPath();
								this.insettoNav.CalculatePath(this.destinazione, navMeshPath2);
								if (navMeshPath2.status != NavMeshPathStatus.PathInvalid)
								{
									break;
								}
								if (j == 4)
								{
									this.destinazione = this.posObbiettivo;
									this.secondaMetaRaggiunta = true;
								}
							}
						}
					}
					if (Vector3.Distance(this.centroBaseInsetto, this.destinazione) < 10f)
					{
						this.destinazione = this.posObbiettivo;
						this.secondaMetaRaggiunta = true;
					}
				}
			}
			if (this.insettoVolante && !this.allontanamentoPerAtt && this.comportInSospeso && this.secondaMetaRaggiunta)
			{
				this.destinazione = this.posObbiettivo;
			}
		}
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x0011751C File Offset: 0x0011571C
	private void FunzioneAttaccoCasualeLimitato()
	{
		if (this.bersaglio == null)
		{
			GestoreNeutroStrategia.valoreRandomSeed++;
			UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
			GameObject gameObject = null;
			if (this.insettoVolante)
			{
				if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèVolante.Count > 0)
				{
					int index = UnityEngine.Random.Range(0, this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèVolante.Count - 1);
					gameObject = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèVolante[index];
				}
				else if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante.Count > 0)
				{
					int index2 = UnityEngine.Random.Range(0, this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante.Count - 1);
					gameObject = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante[index2];
				}
			}
			else if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante.Count > 0)
			{
				int index3 = UnityEngine.Random.Range(0, this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante.Count - 1);
				gameObject = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante[index3];
			}
			if (gameObject)
			{
				bool flag = false;
				for (int i = 0; i < gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco.Count; i++)
				{
					if (gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco[i] == null)
					{
						this.bersaglio = gameObject;
						gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco[i] = base.gameObject;
						this.comportInSospeso = false;
						break;
					}
					if (gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco[gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco.Count - 1] != null)
					{
						flag = true;
					}
				}
				if (flag)
				{
					bool flag2 = false;
					GestoreNeutroStrategia.valoreRandomSeed++;
					UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
					int num = UnityEngine.Random.Range(0, this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati.Count - 1);
					for (int j = num; j < this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati.Count; j++)
					{
						bool flag3 = false;
						if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[j])
						{
							if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[j].GetComponent<PresenzaAlleato>().volante)
							{
								if (this.insettoVolante)
								{
									flag3 = true;
								}
							}
							else
							{
								flag3 = true;
							}
						}
						if (flag3)
						{
							for (int k = 0; k < gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco.Count; k++)
							{
								if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[j].GetComponent<PresenzaAlleato>().ListaNemInAttacco[k] == null)
								{
									this.bersaglio = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[j];
									this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[j].GetComponent<PresenzaAlleato>().ListaNemInAttacco[k] = base.gameObject;
									this.comportInSospeso = false;
									break;
								}
							}
							if (j == this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati.Count - 1)
							{
								flag2 = true;
							}
						}
					}
					if (flag2)
					{
						for (int l = 0; l < num; l++)
						{
							for (int m = 0; m < gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco.Count; m++)
							{
								bool flag4 = false;
								if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[l])
								{
									if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[l].GetComponent<PresenzaAlleato>().volante)
									{
										if (this.insettoVolante)
										{
											flag4 = true;
										}
									}
									else
									{
										flag4 = true;
									}
								}
								if (flag4 && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[l].GetComponent<PresenzaAlleato>().ListaNemInAttacco[m] == null)
								{
									this.bersaglio = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[l];
									this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[l].GetComponent<PresenzaAlleato>().ListaNemInAttacco[m] = base.gameObject;
									this.comportInSospeso = false;
									break;
								}
							}
						}
					}
				}
			}
			if (this.bersaglio == null)
			{
				this.comportInSospeso = true;
			}
		}
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x001179F0 File Offset: 0x00115BF0
	private void FunzioneAttaccoCasualeTotale()
	{
		if (this.bersaglio == null)
		{
			GestoreNeutroStrategia.valoreRandomSeed++;
			UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
			GameObject gameObject = null;
			if (this.insettoVolante)
			{
				if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèVolante.Count > 0)
				{
					int index = UnityEngine.Random.Range(0, this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèVolante.Count - 1);
					gameObject = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèVolante[index];
				}
				else if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante.Count > 0)
				{
					int index2 = UnityEngine.Random.Range(0, this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante.Count - 1);
					gameObject = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante[index2];
				}
			}
			else if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante.Count > 0)
			{
				int index3 = UnityEngine.Random.Range(0, this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante.Count - 1);
				gameObject = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante[index3];
			}
			if (gameObject)
			{
				bool flag = false;
				for (int i = 0; i < gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco.Count; i++)
				{
					if (gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco[i] == null)
					{
						this.bersaglio = gameObject;
						gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco[i] = base.gameObject;
						this.comportInSospeso = false;
						break;
					}
					if (gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco[gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco.Count - 1] != null)
					{
						flag = true;
					}
				}
				if (flag)
				{
					bool flag2 = false;
					GestoreNeutroStrategia.valoreRandomSeed++;
					UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
					int num = UnityEngine.Random.Range(0, this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati.Count - 1);
					for (int j = num; j < this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati.Count; j++)
					{
						bool flag3 = false;
						if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[j])
						{
							if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[j].GetComponent<PresenzaAlleato>().volante)
							{
								if (this.insettoVolante)
								{
									flag3 = true;
								}
							}
							else
							{
								flag3 = true;
							}
						}
						if (flag3)
						{
							for (int k = 0; k < gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco.Count; k++)
							{
								if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[j].GetComponent<PresenzaAlleato>().ListaNemInAttacco[k] == null)
								{
									this.bersaglio = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[j];
									this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[j].GetComponent<PresenzaAlleato>().ListaNemInAttacco[k] = base.gameObject;
									this.comportInSospeso = false;
									break;
								}
							}
							if (j == this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati.Count - 1)
							{
								flag2 = true;
							}
						}
					}
					if (flag2)
					{
						for (int l = 0; l < num; l++)
						{
							for (int m = 0; m < gameObject.GetComponent<PresenzaAlleato>().ListaNemInAttacco.Count; m++)
							{
								bool flag4 = false;
								if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[l])
								{
									if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[l].GetComponent<PresenzaAlleato>().volante)
									{
										if (this.insettoVolante)
										{
											flag4 = true;
										}
									}
									else
									{
										flag4 = true;
									}
								}
								if (flag4 && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[l].GetComponent<PresenzaAlleato>().ListaNemInAttacco[m] == null)
								{
									this.bersaglio = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[l];
									this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[l].GetComponent<PresenzaAlleato>().ListaNemInAttacco[m] = base.gameObject;
									this.comportInSospeso = false;
									break;
								}
							}
						}
					}
				}
			}
			if (this.bersaglio == null)
			{
				GestoreNeutroStrategia.valoreRandomSeed++;
				UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
				if (this.insettoVolante)
				{
					if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèVolante.Count > 0)
					{
						int index4 = UnityEngine.Random.Range(0, this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèVolante.Count - 1);
						gameObject = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèVolante[index4];
						this.bersaglio = gameObject;
					}
					else if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante.Count > 0)
					{
						int index5 = UnityEngine.Random.Range(0, this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante.Count - 1);
						gameObject = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante[index5];
						this.bersaglio = gameObject;
					}
				}
				else if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante.Count > 0)
				{
					int index6 = UnityEngine.Random.Range(0, this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante.Count - 1);
					gameObject = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante[index6];
					this.bersaglio = gameObject;
				}
			}
		}
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x00117FF4 File Offset: 0x001161F4
	private void FunzioneConquistaPunti()
	{
		if (!this.destDecisa)
		{
			float num = 50f;
			this.destDecisa = true;
			GestoreNeutroStrategia.valoreRandomSeed++;
			UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
			this.posProssPuntoConq = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaBandiereConq[UnityEngine.Random.Range(0, 5)].transform.position;
			if (this.insettoVolante)
			{
				GestoreNeutroStrategia.valoreRandomSeed++;
				UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
				float num2 = UnityEngine.Random.Range(-num, num);
				GestoreNeutroStrategia.valoreRandomSeed++;
				UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
				float num3 = UnityEngine.Random.Range(-num, num);
				float y = UnityEngine.Random.Range(20f, 60f);
				this.destinazione = new Vector3(this.posProssPuntoConq.x + num2, y, this.posProssPuntoConq.z + num3);
			}
			else if (this.insettoNav.isOnNavMesh)
			{
				for (int i = 0; i < 10; i++)
				{
					GestoreNeutroStrategia.valoreRandomSeed++;
					UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
					float num4 = UnityEngine.Random.Range(-num, num);
					GestoreNeutroStrategia.valoreRandomSeed++;
					UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
					float num5 = UnityEngine.Random.Range(-num, num);
					this.destinazione = new Vector3(this.posProssPuntoConq.x + num4, this.posProssPuntoConq.y, this.posProssPuntoConq.z + num5);
					this.insettoNav.SetDestination(this.destinazione);
					NavMeshPath navMeshPath = new NavMeshPath();
					this.insettoNav.CalculatePath(this.destinazione, navMeshPath);
					if (navMeshPath.status != NavMeshPathStatus.PathInvalid)
					{
						break;
					}
				}
			}
		}
		if (Vector3.Distance(this.centroBaseInsetto, this.destinazione) < 10f)
		{
			this.destDecisa = false;
		}
	}

	// Token: 0x04001D6E RID: 7534
	public int tipoInsetto;

	// Token: 0x04001D6F RID: 7535
	public string nomeInsetto;

	// Token: 0x04001D70 RID: 7536
	public Sprite immagineInsetto;

	// Token: 0x04001D71 RID: 7537
	public float danno1;

	// Token: 0x04001D72 RID: 7538
	public float danno2;

	// Token: 0x04001D73 RID: 7539
	public float dannoVeleno;

	// Token: 0x04001D74 RID: 7540
	public float frequenzaAttacco;

	// Token: 0x04001D75 RID: 7541
	public GameObject oggettoDescrizione;

	// Token: 0x04001D76 RID: 7542
	public float vita;

	// Token: 0x04001D77 RID: 7543
	public float armatura;

	// Token: 0x04001D78 RID: 7544
	public bool insettoVolante;

	// Token: 0x04001D79 RID: 7545
	public bool èSaltatore;

	// Token: 0x04001D7A RID: 7546
	public bool èCamminatore;

	// Token: 0x04001D7B RID: 7547
	public string velocitàInsetto;

	// Token: 0x04001D7C RID: 7548
	public int numMembriGruppo;

	// Token: 0x04001D7D RID: 7549
	public float numPerSommaExp;

	// Token: 0x04001D7E RID: 7550
	public float tempoSpawnGruppo;

	// Token: 0x04001D7F RID: 7551
	private bool confermaInsettoVolante;

	// Token: 0x04001D80 RID: 7552
	public List<GameObject> ListaPartiVisibili;

	// Token: 0x04001D81 RID: 7553
	public bool morto;

	// Token: 0x04001D82 RID: 7554
	public float timerMorte;

	// Token: 0x04001D83 RID: 7555
	private GameObject IANemico;

	// Token: 0x04001D84 RID: 7556
	private GameObject infoAlleati;

	// Token: 0x04001D85 RID: 7557
	private GameObject primaCamera;

	// Token: 0x04001D86 RID: 7558
	private GameObject secondaCamera;

	// Token: 0x04001D87 RID: 7559
	private GameObject terzaCamera;

	// Token: 0x04001D88 RID: 7560
	private GameObject infoNeutreTattica;

	// Token: 0x04001D89 RID: 7561
	private GameObject varieMappaLocale;

	// Token: 0x04001D8A RID: 7562
	public bool èStatoVisto;

	// Token: 0x04001D8B RID: 7563
	private int elementiVisibili;

	// Token: 0x04001D8C RID: 7564
	private float timerVisibilità;

	// Token: 0x04001D8D RID: 7565
	private bool nemicoEvidenziato;

	// Token: 0x04001D8E RID: 7566
	private RaycastHit hit;

	// Token: 0x04001D8F RID: 7567
	public Vector3 centroInsetto;

	// Token: 0x04001D90 RID: 7568
	public Vector3 centroBaseInsetto;

	// Token: 0x04001D91 RID: 7569
	private int layerSelezione;

	// Token: 0x04001D92 RID: 7570
	public float vitaIniziale;

	// Token: 0x04001D93 RID: 7571
	private GameObject testoVita;

	// Token: 0x04001D94 RID: 7572
	private GameObject cameraAttiva;

	// Token: 0x04001D95 RID: 7573
	private GameObject cerchioSel;

	// Token: 0x04001D96 RID: 7574
	private GameObject cerchioSel2;

	// Token: 0x04001D97 RID: 7575
	public GameObject bersaglio;

	// Token: 0x04001D98 RID: 7576
	public Vector3 destinazione;

	// Token: 0x04001D99 RID: 7577
	public bool obbiettivoInizDato;

	// Token: 0x04001D9A RID: 7578
	public bool ricercaAssente;

	// Token: 0x04001D9B RID: 7579
	public bool ricercaInseguimentoObbiettivo;

	// Token: 0x04001D9C RID: 7580
	public bool ricercaBersPerDistDaIns;

	// Token: 0x04001D9D RID: 7581
	public bool ricercaBersPerTipo;

	// Token: 0x04001D9E RID: 7582
	public bool ricercaBersPerDistDaLuogo;

	// Token: 0x04001D9F RID: 7583
	public bool ricercaCasuale;

	// Token: 0x04001DA0 RID: 7584
	public float raggioRicercaLocale;

	// Token: 0x04001DA1 RID: 7585
	public bool inLocale;

	// Token: 0x04001DA2 RID: 7586
	public int indiceBandieraConq;

	// Token: 0x04001DA3 RID: 7587
	public bool destDecisa;

	// Token: 0x04001DA4 RID: 7588
	public int tipolDaRicercare;

	// Token: 0x04001DA5 RID: 7589
	public bool muoviti;

	// Token: 0x04001DA6 RID: 7590
	public bool allontanamentoPerAtt;

	// Token: 0x04001DA7 RID: 7591
	private Material materialeSelezione;

	// Token: 0x04001DA8 RID: 7592
	private Material materialeEvidenziazione;

	// Token: 0x04001DA9 RID: 7593
	private float dimInizCerchioSel;

	// Token: 0x04001DAA RID: 7594
	private AudioSource audioInsetto;

	// Token: 0x04001DAB RID: 7595
	private float maxDistanzaAudio;

	// Token: 0x04001DAC RID: 7596
	private float timerDaCreazioneIns;

	// Token: 0x04001DAD RID: 7597
	private int tipoBattaglia;

	// Token: 0x04001DAE RID: 7598
	private Vector3 posObbiettivo;

	// Token: 0x04001DAF RID: 7599
	private GameObject obbiettivo;

	// Token: 0x04001DB0 RID: 7600
	private NavMeshAgent insettoNav;

	// Token: 0x04001DB1 RID: 7601
	private List<AudioClip> ListaSuoniInsCopiata;

	// Token: 0x04001DB2 RID: 7602
	private float timerSuono;

	// Token: 0x04001DB3 RID: 7603
	private float tempoSuono;

	// Token: 0x04001DB4 RID: 7604
	public float fattoreAttacco;

	// Token: 0x04001DB5 RID: 7605
	public int tipoComportamento;

	// Token: 0x04001DB6 RID: 7606
	public bool comportInSospeso;

	// Token: 0x04001DB7 RID: 7607
	public int tipoObbiettivo;

	// Token: 0x04001DB8 RID: 7608
	private float timerAggRicercaAttCasuale;

	// Token: 0x04001DB9 RID: 7609
	private float timerAggRicercaObb;

	// Token: 0x04001DBA RID: 7610
	private bool metaFittRaggiunta;

	// Token: 0x04001DBB RID: 7611
	private Vector3 posProssPuntoConq;

	// Token: 0x04001DBC RID: 7612
	public bool primaMetaRaggiunta;

	// Token: 0x04001DBD RID: 7613
	public bool secondaMetaRaggiunta;

	// Token: 0x04001DBE RID: 7614
	private int layerNemico;

	// Token: 0x04001DBF RID: 7615
	private RaycastHit hitNemico;

	// Token: 0x04001DC0 RID: 7616
	public bool sparpagliato;

	// Token: 0x04001DC1 RID: 7617
	private bool giàMorto;
}
