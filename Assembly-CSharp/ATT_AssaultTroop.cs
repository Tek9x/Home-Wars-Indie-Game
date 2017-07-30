using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class ATT_AssaultTroop : MonoBehaviour
{
	// Token: 0x060001CA RID: 458 RVA: 0x0004ED7C File Offset: 0x0004CF7C
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
		if (GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.moltiplicatoreAttaccoInFPS = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().moltiplicatoreFPSBattVeloce;
		}
		else
		{
			this.moltiplicatoreAttaccoInFPS = PlayerPrefs.GetFloat("moltiplicatore danni PP");
		}
		this.timerRinculo = 0.3f;
		this.tempoInserimentoSingolo = 0.55f;
		this.tempoFraSparoERicarica = 1.5f;
		this.suonoArma = base.GetComponent<AudioSource>();
		this.suonoRicarica = base.transform.GetChild(1).GetComponent<AudioSource>();
		this.particelleBocca1 = this.bocca1.GetComponent<ParticleSystem>();
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.velocitàIniziale = this.alleatoNav.speed;
	}

	// Token: 0x060001CB RID: 459 RVA: 0x0004EF24 File Offset: 0x0004D124
	private void Update()
	{
		if (!this.primoFrameAvvenuto)
		{
			this.munizione = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0];
			this.primoFrameAvvenuto = true;
		}
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
					this.mirinoFisso.GetComponent<CanvasGroup>().alpha = 0f;
					base.gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
					this.bocca1.transform.parent.GetComponent<MeshRenderer>().enabled = false;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					base.gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
					this.bocca1.transform.parent.GetComponent<MeshRenderer>().enabled = true;
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
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x060001CC RID: 460 RVA: 0x0004F224 File Offset: 0x0004D424
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

	// Token: 0x060001CD RID: 461 RVA: 0x0004F2A8 File Offset: 0x0004D4A8
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
				if (this.timerRicaricaParziale1 > 0f && this.timerRicaricaParziale1 < 0.1f && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] <= base.GetComponent<PresenzaAlleato>().ListaArmi[0][3])
				{
					this.suonoRicarica.clip = this.suonoInserimentoColpo;
					this.suonoRicarica.Play();
				}
				if (this.timerRicaricaParziale1 > this.tempoInserimentoSingolo && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] < base.GetComponent<PresenzaAlleato>().ListaArmi[0][3])
				{
					List<float> list;
					List<float> expr_20B = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
					int index;
					int expr_20E = index = 5;
					float num = list[index];
					expr_20B[expr_20E] = num + 1f;
					this.timerRicaricaParziale1 = 0f;
					this.timerRicarica1 = 0f;
				}
			}
			if (base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] == base.GetComponent<PresenzaAlleato>().ListaArmi[0][3])
			{
				if (this.timerRicarica1 > 0.7f && this.timerRicarica1 < 0.8f)
				{
					this.suonoRicarica.clip = this.suonoRicaricaPump;
					this.suonoRicarica.Play();
				}
				if (this.timerRicarica1 > 1.8f)
				{
					base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] = false;
				}
			}
		}
	}

	// Token: 0x060001CE RID: 462 RVA: 0x0004F580 File Offset: 0x0004D780
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

	// Token: 0x060001CF RID: 463 RVA: 0x0004F698 File Offset: 0x0004D898
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

	// Token: 0x060001D0 RID: 464 RVA: 0x0004F738 File Offset: 0x0004D938
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.ossoArma1.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 30f;
		}
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x0004F7B8 File Offset: 0x0004D9B8
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
				if (this.unitàBersaglio)
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
			else if (this.unitàBersaglio && this.alleatoNav.enabled)
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
				if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici.Count > 0)
				{
					this.timerAggRicerca += Time.deltaTime;
					if (this.timerAggRicerca > 1f)
					{
						this.timerAggRicerca = 0f;
						List<GameObject> list = new List<GameObject>();
						foreach (GameObject current in this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici)
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
				if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici.Count > 0)
				{
					this.timerAggRicerca += Time.deltaTime;
					if (this.timerAggRicerca > 1f)
					{
						this.timerAggRicerca = 0f;
						List<GameObject> list2 = new List<GameObject>();
						foreach (GameObject current2 in this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici)
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
				if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici.Count > 0)
				{
					GestoreNeutroStrategia.valoreRandomSeed++;
					UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
					float f2 = UnityEngine.Random.Range(0f, (float)this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici.Count - 0.01f);
					this.unitàBersaglio = this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici[Mathf.FloorToInt(f2)];
				}
			}
		}
		else
		{
			this.alleatoNav.speed = this.velocitàAlleatoNav;
			this.unitàBersaglio = null;
		}
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x00050220 File Offset: 0x0004E420
	private void AttaccoIndipendente()
	{
		if (base.GetComponent<PresenzaAlleato>().arma1Attivata && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[0])
		{
			if (this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaAllPiùViciniPerTipo.Contains(base.gameObject) || this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] > 0.05f)
			{
				this.suonoArma.Play();
				this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] = 0f;
			}
			this.timerFrequenzaArma1 = 0f;
			this.particelleBocca1.Play();
			List<float> listaValoriArma;
			List<float> expr_154 = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int index;
			int expr_158 = index = 5;
			float num = listaValoriArma[index];
			expr_154[expr_158] = num - 1f;
			List<float> listaValoriArma2;
			List<float> expr_17E = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int expr_182 = index = 6;
			num = listaValoriArma2[index];
			expr_17E[expr_182] = num - 1f;
			this.timerDopoSparo = 0f;
			for (int i = 0; i < this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun.Count; i++)
			{
				if (this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun[i] == null && i < this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun.Count - 1)
				{
					this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun[i] = this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun[i + 1];
					this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun[i + 1] = null;
				}
			}
			for (int j = 0; j < this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun.Count; j++)
			{
				if (this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun[j] == null)
				{
					this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun.RemoveAt(j);
				}
			}
			foreach (GameObject current in this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun)
			{
				if (current && current.tag == "Nemico")
				{
					float num2 = 0f;
					if (current.GetComponent<PresenzaNemico>().vita > this.munizione.GetComponent<DatiGeneraliMunizione>().penetrazione)
					{
						num2 = this.munizione.GetComponent<DatiGeneraliMunizione>().penetrazione;
					}
					else if (current.GetComponent<PresenzaNemico>().vita > 0f)
					{
						num2 = current.GetComponent<PresenzaNemico>().vita;
					}
					current.GetComponent<PresenzaNemico>().vita -= this.munizione.GetComponent<DatiGeneraliMunizione>().penetrazione;
					if (current.GetComponent<PresenzaNemico>().vita > this.munizione.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura))
					{
						num2 += this.munizione.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura);
					}
					else if (current.GetComponent<PresenzaNemico>().vita > 0f)
					{
						num2 += current.GetComponent<PresenzaNemico>().vita;
					}
					current.GetComponent<PresenzaNemico>().vita -= this.munizione.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura);
					List<float> listaDanniAlleati;
					List<float> expr_43C = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int expr_44A = index = base.GetComponent<PresenzaAlleato>().tipoTruppa;
					num = listaDanniAlleati[index];
					expr_43C[expr_44A] = num + num2;
				}
			}
		}
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x000506C8 File Offset: 0x0004E8C8
	private void AttaccoPrimaPersona()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo, 99999f, this.layerColpo))
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
		if (Input.GetMouseButtonDown(0) && base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[1])
		{
			this.timerFrequenzaArma1 = 0f;
			this.Sparo();
			this.suonoArma.Play();
			List<float> listaValoriArma;
			List<float> expr_1BC = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int index;
			int expr_1BF = index = 5;
			float num = listaValoriArma[index];
			expr_1BC[expr_1BF] = num - 1f;
			List<float> listaValoriArma2;
			List<float> expr_1E0 = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int expr_1E4 = index = 6;
			num = listaValoriArma2[index];
			expr_1E0[expr_1E4] = num - 1f;
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

	// Token: 0x060001D4 RID: 468 RVA: 0x0005096C File Offset: 0x0004EB6C
	private void Sparo()
	{
		for (int i = 0; i < this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun.Count; i++)
		{
			if (this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun[i] == null && i < this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun.Count - 1)
			{
				this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun[i] = this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun[i + 1];
				this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun[i + 1] = null;
			}
		}
		for (int j = 0; j < this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun.Count; j++)
		{
			if (this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun[j] == null)
			{
				this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun.RemoveAt(j);
			}
		}
		foreach (GameObject current in this.bocca1.GetComponent<ATT_AssaultTroopArma>().ListaEffettoShotgun)
		{
			if (current != null && current.tag == "Nemico")
			{
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
				float num = 0f;
				if (current.GetComponent<PresenzaNemico>().vita > this.munizione.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
				{
					num = this.munizione.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				}
				else if (current.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num = current.GetComponent<PresenzaNemico>().vita;
				}
				current.GetComponent<PresenzaNemico>().vita -= this.munizione.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				if (current.GetComponent<PresenzaNemico>().vita > this.munizione.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS)
				{
					num += this.munizione.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
				}
				else if (current.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num += current.GetComponent<PresenzaNemico>().vita;
				}
				current.GetComponent<PresenzaNemico>().vita -= this.munizione.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
				List<float> listaDanniAlleati;
				List<float> expr_2D4 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int tipoTruppa;
				int expr_2E2 = tipoTruppa = base.GetComponent<PresenzaAlleato>().tipoTruppa;
				float num2 = listaDanniAlleati[tipoTruppa];
				expr_2D4[expr_2E2] = num2 + num;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num;
			}
		}
	}

	// Token: 0x040007F8 RID: 2040
	public float angVertMax;

	// Token: 0x040007F9 RID: 2041
	public float angVertMin;

	// Token: 0x040007FA RID: 2042
	public GameObject bocca1;

	// Token: 0x040007FB RID: 2043
	public GameObject ossoArma1;

	// Token: 0x040007FC RID: 2044
	private float timerFrequenzaArma1;

	// Token: 0x040007FD RID: 2045
	private float timerRicarica1;

	// Token: 0x040007FE RID: 2046
	private float timerRicaricaParziale1;

	// Token: 0x040007FF RID: 2047
	private float timerDopoSparo;

	// Token: 0x04000800 RID: 2048
	private float tempoFraSparoERicarica;

	// Token: 0x04000801 RID: 2049
	private GameObject CanvasFPS;

	// Token: 0x04000802 RID: 2050
	private GameObject mirinoFisso;

	// Token: 0x04000803 RID: 2051
	public GameObject strutturaPrefabFPS1;

	// Token: 0x04000804 RID: 2052
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000805 RID: 2053
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000806 RID: 2054
	public Vector3 aggiustamentoTraslVisualeFPS;

	// Token: 0x04000807 RID: 2055
	public Vector3 aggiustamentoRotVisualeFPS;

	// Token: 0x04000808 RID: 2056
	private GameObject strutturaFPS1;

	// Token: 0x04000809 RID: 2057
	private float timerPosizionamentoTPS;

	// Token: 0x0400080A RID: 2058
	private float timerPosizionamentoFPS;

	// Token: 0x0400080B RID: 2059
	private float campoCameraIniziale;

	// Token: 0x0400080C RID: 2060
	private GameObject infoNeutreTattica;

	// Token: 0x0400080D RID: 2061
	private GameObject terzaCamera;

	// Token: 0x0400080E RID: 2062
	private GameObject primaCamera;

	// Token: 0x0400080F RID: 2063
	public AudioClip suonoInserimentoColpo;

	// Token: 0x04000810 RID: 2064
	public AudioClip suonoRicaricaPump;

	// Token: 0x04000811 RID: 2065
	private float tempoInserimentoSingolo;

	// Token: 0x04000812 RID: 2066
	private NavMeshAgent alleatoNav;

	// Token: 0x04000813 RID: 2067
	private float velocitàAlleatoNav;

	// Token: 0x04000814 RID: 2068
	private GameObject IANemico;

	// Token: 0x04000815 RID: 2069
	private GameObject InfoAlleati;

	// Token: 0x04000816 RID: 2070
	private RaycastHit targetSparo;

	// Token: 0x04000817 RID: 2071
	private int layerColpo;

	// Token: 0x04000818 RID: 2072
	private int layerVisuale;

	// Token: 0x04000819 RID: 2073
	private GameObject unitàBersaglio;

	// Token: 0x0400081A RID: 2074
	private Vector3 centroUnitàBersaglio;

	// Token: 0x0400081B RID: 2075
	public bool avviaRinculo;

	// Token: 0x0400081C RID: 2076
	public float timerRinculo;

	// Token: 0x0400081D RID: 2077
	private GameObject munizione;

	// Token: 0x0400081E RID: 2078
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x0400081F RID: 2079
	private bool primoFrameAvvenuto;

	// Token: 0x04000820 RID: 2080
	private AudioSource suonoArma;

	// Token: 0x04000821 RID: 2081
	private AudioSource suonoRicarica;

	// Token: 0x04000822 RID: 2082
	private ParticleSystem particelleBocca1;

	// Token: 0x04000823 RID: 2083
	private float distFineOrdineMovimento;

	// Token: 0x04000824 RID: 2084
	private bool calcoloDistJump;

	// Token: 0x04000825 RID: 2085
	private bool calcoloJumpEffettuato;

	// Token: 0x04000826 RID: 2086
	private float velocitàIniziale;

	// Token: 0x04000827 RID: 2087
	private float timerAggRicerca;
}
