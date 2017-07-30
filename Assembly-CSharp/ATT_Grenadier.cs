using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200002A RID: 42
public class ATT_Grenadier : MonoBehaviour
{
	// Token: 0x060001EE RID: 494 RVA: 0x000548D4 File Offset: 0x00052AD4
	private void Start()
	{
		this.CanvasFPS = GameObject.FindGameObjectWithTag("CanvasFPS");
		this.mirinoFisso = this.CanvasFPS.transform.GetChild(2).transform.GetChild(0).gameObject;
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.InfoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.alleatoNav = base.GetComponent<NavMeshAgent>();
		this.velocitàAlleatoNav = base.GetComponent<NavMeshAgent>().speed;
		this.layerColpo = 165120;
		this.layerVisuale = 256;
		this.campoCameraIniziale = this.terzaCamera.GetComponent<Camera>().fieldOfView;
		this.timerRinculo = 0.3f;
		this.tempoInserimentoSingolo = 1.5f;
		this.tempoFraSparoERicarica = 1f;
		this.suonoArma = base.GetComponent<AudioSource>();
		this.suonoRicarica = base.transform.GetChild(1).GetComponent<AudioSource>();
		this.particelleBocca1 = this.bocca1.GetComponent<ParticleSystem>();
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.velocitàIniziale = this.alleatoNav.speed;
	}

	// Token: 0x060001EF RID: 495 RVA: 0x00054A48 File Offset: 0x00052C48
	private void Update()
	{
		if (!this.primoFrameAvvenuto)
		{
			this.munizione = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0];
			this.primoFrameAvvenuto = true;
		}
		this.munizioneArma1 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0];
		this.unitàBersaglio = base.GetComponent<PresenzaAlleato>().unitàBersaglio;
		this.timerFrequenzaArma1 += Time.deltaTime;
		this.timerDopoSparo += Time.deltaTime;
		this.CondizioniArma();
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			if (!this.alleatoNav.isOnOffMeshLink)
			{
				this.PreparazioneAttacco();
				this.calcoloJumpEffettuato = false;
			}
			else
			{
				this.InJump();
			}
			if (this.strutturaFPS1)
			{
				UnityEngine.Object.Destroy(this.strutturaFPS1);
			}
		}
		else
		{
			this.GestioneVisuali();
			this.AttaccoPrimaPersona();
			base.GetComponent<NavMeshAgent>().enabled = false;
			if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
			{
				this.mirinoFisso.GetComponent<CanvasGroup>().alpha = 1f;
			}
			if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva == 3)
			{
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
					base.gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
					this.bocca1.transform.parent.GetComponent<MeshRenderer>().enabled = false;
					this.mirinoFisso.GetComponent<Image>().sprite = this.mirinoDot;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					base.gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
					this.bocca1.transform.parent.GetComponent<MeshRenderer>().enabled = true;
					this.mirinoFisso.GetComponent<Image>().sprite = this.mirinoNormale;
				}
			}
		}
		if (this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona && this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0 && this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0] == base.gameObject)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona = false;
			this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera = null;
			this.timerPosizionamentoTPS = 0f;
			this.timerPosizionamentoFPS = 0f;
			base.GetComponent<NavMeshAgent>().enabled = true;
			base.gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
			this.bocca1.transform.parent.GetComponent<MeshRenderer>().enabled = true;
			this.ossoArma1.transform.eulerAngles = base.transform.eulerAngles;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = this.campoCameraIniziale;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x00054D8C File Offset: 0x00052F8C
	private void InJump()
	{
		if (!this.calcoloJumpEffettuato)
		{
			this.calcoloDistJump = true;
		}
		if (this.calcoloDistJump)
		{
			this.calcoloDistJump = false;
			this.calcoloJumpEffettuato = true;
			float num = Mathf.Abs(this.alleatoNav.destination.y - base.transform.position.y);
			this.alleatoNav.speed = this.velocitàIniziale / (num / 80f) / 10f;
		}
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x00054E10 File Offset: 0x00053010
	private void CondizioniArma()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] <= 0f && this.timerDopoSparo > this.tempoFraSparoERicarica)
		{
			base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] = true;
		}
		if (Input.GetKeyDown(KeyCode.R) && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] && this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera == base.gameObject && base.GetComponent<PresenzaAlleato>().ListaArmi[0][6] > 0f && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] < base.GetComponent<PresenzaAlleato>().ListaArmi[0][3])
		{
			base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] = true;
		}
		if (base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0])
		{
			this.timerRicarica1 += Time.deltaTime;
			this.timerRicaricaParziale1 += Time.deltaTime;
			if (base.GetComponent<PresenzaAlleato>().ListaArmi[0][6] > 0f)
			{
				if (this.timerRicarica1 > 0f && this.timerRicarica1 < 0.1f)
				{
					this.suonoRicarica.clip = this.suonoInizioRicarica;
					this.suonoRicarica.Play();
				}
				if (this.timerRicaricaParziale1 > 1.2f && this.timerRicaricaParziale1 < 1.27f && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] < base.GetComponent<PresenzaAlleato>().ListaArmi[0][3])
				{
					this.suonoRicarica.clip = this.suonoInserimentoColpo;
					this.suonoRicarica.Play();
				}
				if (this.timerRicaricaParziale1 > this.tempoInserimentoSingolo && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] < base.GetComponent<PresenzaAlleato>().ListaArmi[0][3])
				{
					List<float> list;
					List<float> expr_247 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
					int index;
					int expr_24A = index = 5;
					float num = list[index];
					expr_247[expr_24A] = num + 1f;
					this.timerRicaricaParziale1 = 0.8f;
				}
			}
			if (base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] == base.GetComponent<PresenzaAlleato>().ListaArmi[0][3])
			{
				if (this.timerRicaricaParziale1 > 1.4f && this.timerRicaricaParziale1 < 1.47f)
				{
					this.suonoRicarica.clip = this.suonoFineRicarica;
					this.suonoRicarica.Play();
				}
				if (this.timerRicaricaParziale1 > 2.2f)
				{
					base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] = false;
					this.timerRicarica1 = 0f;
					this.timerRicaricaParziale1 = 0f;
				}
			}
		}
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x00055130 File Offset: 0x00053330
	private void GestioneVisuali()
	{
		if (!this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
		{
			if (this.strutturaFPS1)
			{
				UnityEngine.Object.Destroy(this.strutturaFPS1);
			}
			this.CameraTPS();
			this.timerPosizionamentoFPS = 0f;
		}
		if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
		{
			if (!this.strutturaFPS1)
			{
				this.strutturaFPS1 = (UnityEngine.Object.Instantiate(this.strutturaPrefabFPS1, base.transform.position, this.terzaCamera.transform.rotation) as GameObject);
				this.strutturaFPS1.transform.parent = this.terzaCamera.transform;
				this.strutturaFPS1.transform.localPosition = this.aggiustamentoTraslVisualeFPS;
				this.strutturaFPS1.transform.localEulerAngles = this.aggiustamentoRotVisualeFPS;
				this.strutturaFPS1.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
			}
			this.CameraFPS();
			this.timerPosizionamentoTPS = 0f;
		}
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x00055248 File Offset: 0x00053448
	private void CameraTPS()
	{
		this.timerPosizionamentoTPS += Time.deltaTime;
		if (this.timerPosizionamentoTPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.ossoArma1.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraTPS;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = this.campoCameraIniziale;
			this.terzaCamera.transform.rotation = this.ossoArma1.transform.rotation;
		}
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x000552E8 File Offset: 0x000534E8
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.ossoArma1.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 45f;
		}
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x00055368 File Offset: 0x00053568
	private void PreparazioneAttacco()
	{
		bool flag = false;
		if (this.unitàBersaglio)
		{
			flag = Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale);
		}
		if (this.unitàBersaglio)
		{
			this.centroUnitàBersaglio = this.unitàBersaglio.GetComponent<PresenzaNemico>().centroInsetto;
		}
		float num = Vector3.Distance(base.transform.position, this.alleatoNav.destination);
		if (num <= this.distFineOrdineMovimento)
		{
			base.GetComponent<PresenzaAlleato>().destinazioneOrdinata = false;
		}
		if (!base.GetComponent<PresenzaAlleato>().destinazioneOrdinata)
		{
			this.alleatoNav.speed = 0f;
			if (base.GetComponent<PresenzaAlleato>().attaccoOrdinato)
			{
				base.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
				this.unitàBersaglio = this.primaCamera.GetComponent<Selezionamento>().oggettoBersaglio;
				if (this.unitàBersaglio && !this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
				{
					float num2 = Vector3.Dot((this.centroUnitàBersaglio - base.transform.position).normalized, base.transform.up);
					if (num2 < this.angVertMax && num2 > this.angVertMin)
					{
						float num3 = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
						if (num3 >= this.munizione.GetComponent<DatiGeneraliMunizione>().portataMassima)
						{
							if (!base.GetComponent<PresenzaAlleato>().comportamentoDifensivo && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino)
							{
								this.alleatoNav.SetDestination(this.unitàBersaglio.transform.position);
								this.alleatoNav.speed = this.velocitàAlleatoNav;
							}
							else
							{
								this.unitàBersaglio = null;
							}
						}
						if (num3 < this.munizione.GetComponent<DatiGeneraliMunizione>().portataMassima)
						{
							if (!flag)
							{
								base.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
								this.ossoArma1.transform.LookAt(this.centroUnitàBersaglio);
								this.alleatoNav.speed = 0f;
								this.AttaccoIndipendente();
							}
							else if (!base.GetComponent<PresenzaAlleato>().comportamentoDifensivo && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino)
							{
								this.alleatoNav.SetDestination(this.unitàBersaglio.transform.position);
								this.alleatoNav.speed = this.velocitàAlleatoNav;
							}
							else
							{
								this.unitàBersaglio = null;
							}
						}
						if (this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita <= 0f)
						{
							base.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
						}
					}
				}
			}
			else if (this.unitàBersaglio && this.alleatoNav.enabled && !this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
			{
				float num4 = Vector3.Dot((this.centroUnitàBersaglio - base.transform.position).normalized, base.transform.up);
				float num5 = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
				if (num4 < this.angVertMax && num4 > this.angVertMin)
				{
					if (num5 >= this.munizione.GetComponent<DatiGeneraliMunizione>().portataMassima)
					{
						if (!base.GetComponent<PresenzaAlleato>().comportamentoDifensivo && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino)
						{
							this.alleatoNav.SetDestination(this.unitàBersaglio.transform.position);
							this.alleatoNav.speed = this.velocitàAlleatoNav;
						}
						else
						{
							this.unitàBersaglio = null;
						}
					}
					if (num5 < this.munizione.GetComponent<DatiGeneraliMunizione>().portataMassima)
					{
						if (!flag)
						{
							base.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
							this.ossoArma1.transform.LookAt(this.centroUnitàBersaglio);
							this.alleatoNav.speed = 0f;
							this.AttaccoIndipendente();
						}
						else if (!base.GetComponent<PresenzaAlleato>().comportamentoDifensivo && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino)
						{
							this.alleatoNav.SetDestination(this.unitàBersaglio.transform.position);
							this.alleatoNav.speed = this.velocitàAlleatoNav;
						}
						else
						{
							this.unitàBersaglio = null;
						}
					}
				}
				else if (num5 > 3f)
				{
					this.alleatoNav.speed = this.velocitàAlleatoNav;
					this.unitàBersaglio = null;
				}
				if (this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita <= 0f)
				{
					this.unitàBersaglio = null;
				}
				if (this.unitàBersaglio && !this.unitàBersaglio.GetComponent<PresenzaNemico>().èStatoVisto)
				{
					this.unitàBersaglio = null;
				}
			}
			else if (base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers)
			{
				this.alleatoNav.speed = this.velocitàAlleatoNav;
				if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti.Count > 0)
				{
					this.timerAggRicerca += Time.deltaTime;
					if (this.timerAggRicerca > 1f)
					{
						this.timerAggRicerca = 0f;
						List<GameObject> list = new List<GameObject>();
						foreach (GameObject current in this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti)
						{
							if (current != null && current.GetComponent<PresenzaNemico>().èStatoVisto)
							{
								float num6 = Vector3.Distance(base.transform.position, current.GetComponent<PresenzaNemico>().centroInsetto);
								if (num6 < this.munizione.GetComponent<DatiGeneraliMunizione>().portataMassima && !Physics.Linecast(this.bocca1.transform.position, current.GetComponent<PresenzaNemico>().centroInsetto, this.layerVisuale))
								{
									float num7 = Vector3.Dot((current.GetComponent<PresenzaNemico>().centroInsetto - base.transform.position).normalized, base.transform.up);
									if (num7 < this.angVertMax && num7 > this.angVertMin)
									{
										list.Add(current);
									}
								}
							}
						}
						if (list.Count > 0)
						{
							GestoreNeutroStrategia.valoreRandomSeed++;
							UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
							float f = UnityEngine.Random.Range(0f, (float)list.Count - 0.01f);
							this.unitàBersaglio = list[Mathf.FloorToInt(f)];
						}
					}
				}
			}
			else if (base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino)
			{
				this.alleatoNav.speed = this.velocitàAlleatoNav;
				if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti.Count > 0)
				{
					this.timerAggRicerca += Time.deltaTime;
					if (this.timerAggRicerca > 1f)
					{
						this.timerAggRicerca = 0f;
						List<GameObject> list2 = new List<GameObject>();
						foreach (GameObject current2 in this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti)
						{
							if (current2 != null && current2.GetComponent<PresenzaNemico>().èStatoVisto)
							{
								float num8 = Vector3.Distance(base.transform.position, current2.GetComponent<PresenzaNemico>().centroInsetto);
								if (num8 < this.munizione.GetComponent<DatiGeneraliMunizione>().portataMassima && !Physics.Linecast(this.bocca1.transform.position, current2.GetComponent<PresenzaNemico>().centroInsetto, this.layerVisuale))
								{
									float num9 = Vector3.Dot((current2.GetComponent<PresenzaNemico>().centroInsetto - base.transform.position).normalized, base.transform.up);
									if (num9 < this.angVertMax && num9 > this.angVertMin)
									{
										list2.Add(current2);
									}
								}
							}
						}
						if (list2.Count > 0)
						{
							float num10 = 9999f;
							for (int i = 0; i < list2.Count; i++)
							{
								float num11 = Vector3.Distance(base.transform.position, list2[i].GetComponent<PresenzaNemico>().centroInsetto);
								if (num11 < num10)
								{
									num10 = num11;
									this.unitàBersaglio = list2[i];
								}
							}
						}
					}
				}
			}
			else if (base.GetComponent<PresenzaAlleato>().ricercaAutomaticaBersaglio)
			{
				this.alleatoNav.speed = this.velocitàAlleatoNav;
				if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti.Count > 0)
				{
					GestoreNeutroStrategia.valoreRandomSeed++;
					UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
					float f2 = UnityEngine.Random.Range(0f, (float)this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti.Count - 0.01f);
					this.unitàBersaglio = this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti[Mathf.FloorToInt(f2)];
				}
			}
		}
		else
		{
			this.alleatoNav.speed = this.velocitàAlleatoNav;
			this.unitàBersaglio = null;
		}
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x00055DF8 File Offset: 0x00053FF8
	private void AttaccoIndipendente()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[0])
		{
			if (this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaAllPiùViciniPerTipo.Contains(base.gameObject) || this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] > 0.05f)
			{
				this.suonoArma.Play();
				this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] = 0f;
			}
			this.timerFrequenzaArma1 = 0f;
			this.granata = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
			this.granata.GetComponent<DatiProiettile>().locazioneTarget = this.centroUnitàBersaglio;
			this.particelleBocca1.Play();
			List<float> listaValoriArma;
			List<float> expr_1A6 = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int index;
			int expr_1A9 = index = 5;
			float num = listaValoriArma[index];
			expr_1A6[expr_1A9] = num - 1f;
			List<float> listaValoriArma2;
			List<float> expr_1CA = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int expr_1CD = index = 6;
			num = listaValoriArma2[index];
			expr_1CA[expr_1CD] = num - 1f;
			this.timerDopoSparo = 0f;
			this.granata.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
		}
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x00056010 File Offset: 0x00054210
	private void AttaccoPrimaPersona()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo))
		{
			if (this.targetSparo.collider.gameObject.tag == "Nemico" || this.targetSparo.collider.gameObject.tag == "Nemico Testa" || this.targetSparo.collider.gameObject.tag == "Nemico Coll Suppl")
			{
				if (Vector3.Distance(base.transform.position, this.targetSparo.point) <= this.munizione.GetComponent<DatiGeneraliMunizione>().portataMassima)
				{
					base.GetComponent<PresenzaAlleato>().ListaFuoriPortataArmi[0] = false;
				}
				else
				{
					base.GetComponent<PresenzaAlleato>().ListaFuoriPortataArmi[0] = true;
				}
			}
			else
			{
				base.GetComponent<PresenzaAlleato>().ListaFuoriPortataArmi[0] = false;
			}
		}
		this.ossoArma1.transform.eulerAngles = this.terzaCamera.transform.eulerAngles;
		if (Input.GetMouseButton(0) && base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[1])
		{
			this.timerFrequenzaArma1 = 0f;
			this.Sparo();
			this.suonoArma.Play();
			List<float> listaValoriArma;
			List<float> expr_1B1 = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int index;
			int expr_1B4 = index = 5;
			float num = listaValoriArma[index];
			expr_1B1[expr_1B4] = num - 1f;
			List<float> listaValoriArma2;
			List<float> expr_1D5 = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int expr_1D9 = index = 6;
			num = listaValoriArma2[index];
			expr_1D5[expr_1D9] = num - 1f;
			this.avviaRinculo = true;
			this.timerRinculo = 0.3f;
			this.timerDopoSparo = 0f;
			if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.strutturaFPS1.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
			}
			else
			{
				this.particelleBocca1.Play();
			}
		}
		if (this.avviaRinculo)
		{
			this.timerRinculo -= Time.deltaTime;
		}
		if (this.avviaRinculo && this.timerRinculo <= 0f)
		{
			this.avviaRinculo = false;
		}
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x000562AC File Offset: 0x000544AC
	private void Sparo()
	{
		if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
		{
			this.granata = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
			this.granata.GetComponent<DatiProiettile>().sparatoInFPS = true;
			this.granata.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
		}
		else
		{
			this.granata = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.strutturaFPS1.transform.GetChild(1).transform.position, this.bocca1.transform.rotation) as GameObject);
			this.granata.GetComponent<DatiProiettile>().sparatoInFPS = true;
			this.granata.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
		}
	}

	// Token: 0x04000884 RID: 2180
	public float angVertMax;

	// Token: 0x04000885 RID: 2181
	public float angVertMin;

	// Token: 0x04000886 RID: 2182
	public GameObject bocca1;

	// Token: 0x04000887 RID: 2183
	public GameObject ossoArma1;

	// Token: 0x04000888 RID: 2184
	private float timerFrequenzaArma1;

	// Token: 0x04000889 RID: 2185
	private float timerRicarica1;

	// Token: 0x0400088A RID: 2186
	private float timerRicaricaParziale1;

	// Token: 0x0400088B RID: 2187
	private float timerDopoSparo;

	// Token: 0x0400088C RID: 2188
	private float tempoFraSparoERicarica;

	// Token: 0x0400088D RID: 2189
	private GameObject CanvasFPS;

	// Token: 0x0400088E RID: 2190
	private GameObject mirinoFisso;

	// Token: 0x0400088F RID: 2191
	public GameObject strutturaPrefabFPS1;

	// Token: 0x04000890 RID: 2192
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000891 RID: 2193
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000892 RID: 2194
	public Vector3 aggiustamentoTraslVisualeFPS;

	// Token: 0x04000893 RID: 2195
	public Vector3 aggiustamentoRotVisualeFPS;

	// Token: 0x04000894 RID: 2196
	private GameObject strutturaFPS1;

	// Token: 0x04000895 RID: 2197
	private float timerPosizionamentoTPS;

	// Token: 0x04000896 RID: 2198
	private float timerPosizionamentoFPS;

	// Token: 0x04000897 RID: 2199
	private float campoCameraIniziale;

	// Token: 0x04000898 RID: 2200
	public Sprite mirinoDot;

	// Token: 0x04000899 RID: 2201
	public Sprite mirinoNormale;

	// Token: 0x0400089A RID: 2202
	private GameObject infoNeutreTattica;

	// Token: 0x0400089B RID: 2203
	private GameObject terzaCamera;

	// Token: 0x0400089C RID: 2204
	private GameObject primaCamera;

	// Token: 0x0400089D RID: 2205
	public AudioClip suonoInserimentoColpo;

	// Token: 0x0400089E RID: 2206
	public AudioClip suonoInizioRicarica;

	// Token: 0x0400089F RID: 2207
	public AudioClip suonoFineRicarica;

	// Token: 0x040008A0 RID: 2208
	private float tempoInserimentoSingolo;

	// Token: 0x040008A1 RID: 2209
	private NavMeshAgent alleatoNav;

	// Token: 0x040008A2 RID: 2210
	private float velocitàAlleatoNav;

	// Token: 0x040008A3 RID: 2211
	private GameObject IANemico;

	// Token: 0x040008A4 RID: 2212
	private GameObject InfoAlleati;

	// Token: 0x040008A5 RID: 2213
	private RaycastHit targetSparo;

	// Token: 0x040008A6 RID: 2214
	private int layerColpo;

	// Token: 0x040008A7 RID: 2215
	private int layerVisuale;

	// Token: 0x040008A8 RID: 2216
	private GameObject unitàBersaglio;

	// Token: 0x040008A9 RID: 2217
	private Vector3 centroUnitàBersaglio;

	// Token: 0x040008AA RID: 2218
	public bool avviaRinculo;

	// Token: 0x040008AB RID: 2219
	public float timerRinculo;

	// Token: 0x040008AC RID: 2220
	private GameObject munizione;

	// Token: 0x040008AD RID: 2221
	private bool primoFrameAvvenuto;

	// Token: 0x040008AE RID: 2222
	private AudioSource suonoArma;

	// Token: 0x040008AF RID: 2223
	private AudioSource suonoRicarica;

	// Token: 0x040008B0 RID: 2224
	private ParticleSystem particelleBocca1;

	// Token: 0x040008B1 RID: 2225
	private GameObject granata;

	// Token: 0x040008B2 RID: 2226
	private GameObject munizioneArma1;

	// Token: 0x040008B3 RID: 2227
	private float distFineOrdineMovimento;

	// Token: 0x040008B4 RID: 2228
	private bool calcoloDistJump;

	// Token: 0x040008B5 RID: 2229
	private bool calcoloJumpEffettuato;

	// Token: 0x040008B6 RID: 2230
	private float velocitàIniziale;

	// Token: 0x040008B7 RID: 2231
	private float timerAggRicerca;
}
