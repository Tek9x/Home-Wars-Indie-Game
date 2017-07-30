using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000035 RID: 53
public class ATT_Sniper : MonoBehaviour
{
	// Token: 0x0600027E RID: 638 RVA: 0x00069D38 File Offset: 0x00067F38
	private void Start()
	{
		this.CanvasFPS = GameObject.FindGameObjectWithTag("CanvasFPS");
		this.mirinoFisso = this.CanvasFPS.transform.GetChild(2).transform.GetChild(0).gameObject;
		this.mirinoDiPrecisione = this.CanvasFPS.transform.GetChild(0).transform.GetChild(0).gameObject;
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
		this.tempoFraSparoERicarica = 1f;
		this.suonoArma = base.GetComponent<AudioSource>();
		this.suonoRicarica = base.transform.GetChild(1).GetComponent<AudioSource>();
		this.colpiBocca1 = this.bocca1.transform.GetChild(0).gameObject;
		this.particelleBocca1 = this.bocca1.GetComponent<ParticleSystem>();
		this.particelleColpiBocca1 = this.bocca1.transform.GetChild(0).GetComponent<ParticleSystem>();
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.velocitàIniziale = this.alleatoNav.speed;
	}

	// Token: 0x0600027F RID: 639 RVA: 0x00069F34 File Offset: 0x00068134
	private void Update()
	{
		this.munizione = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0];
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
		}
		else
		{
			this.GestioneVisuali();
			this.AttaccoPrimaPersona();
			base.GetComponent<NavMeshAgent>().enabled = false;
			if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
			{
				this.mirinoFisso.GetComponent<CanvasGroup>().alpha = 1f;
				this.mirinoDiPrecisione.GetComponent<CanvasGroup>().alpha = 0f;
			}
			if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva == 3)
			{
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
					this.mirinoFisso.GetComponent<CanvasGroup>().alpha = 0f;
					this.mirinoDiPrecisione.GetComponent<CanvasGroup>().alpha = 1f;
					this.mirinoDiPrecisione.GetComponent<Image>().sprite = this.spriteMirinoSniper;
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 30f;
					base.gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
					this.bocca1.transform.parent.GetComponent<MeshRenderer>().enabled = false;
					this.cappello.GetComponent<MeshRenderer>().enabled = false;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					base.gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
					this.bocca1.transform.parent.GetComponent<MeshRenderer>().enabled = true;
					this.cappello.GetComponent<MeshRenderer>().enabled = true;
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
			this.cappello.GetComponent<MeshRenderer>().enabled = true;
			this.ossoArma1.transform.eulerAngles = base.transform.eulerAngles;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = this.campoCameraIniziale;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x06000280 RID: 640 RVA: 0x0006A2A4 File Offset: 0x000684A4
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

	// Token: 0x06000281 RID: 641 RVA: 0x0006A328 File Offset: 0x00068528
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
			if (base.GetComponent<PresenzaAlleato>().ListaArmi[0][6] > 0f)
			{
				if (this.timerRicarica1 > 0f && this.timerRicarica1 < 0.1f)
				{
					this.suonoRicarica.Play();
				}
				if (this.timerRicarica1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][2])
				{
					if (base.GetComponent<PresenzaAlleato>().ListaArmi[0][6] < base.GetComponent<PresenzaAlleato>().ListaArmi[0][3])
					{
						base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[0][6];
						this.timerRicarica1 = 0f;
						base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] = false;
					}
					else
					{
						base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[0][3];
						this.timerRicarica1 = 0f;
						base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] = false;
					}
				}
			}
		}
	}

	// Token: 0x06000282 RID: 642 RVA: 0x0006A588 File Offset: 0x00068788
	private void GestioneVisuali()
	{
		if (!this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
		{
			this.CameraTPS();
			this.timerPosizionamentoFPS = 0f;
		}
		if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
		{
			this.CameraFPS();
			this.timerPosizionamentoTPS = 0f;
		}
	}

	// Token: 0x06000283 RID: 643 RVA: 0x0006A5E4 File Offset: 0x000687E4
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

	// Token: 0x06000284 RID: 644 RVA: 0x0006A684 File Offset: 0x00068884
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.ossoArma1.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
		}
		float num = 1f;
		if (this.terzaCamera.GetComponent<Camera>().fieldOfView < 40f && -Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			this.terzaCamera.GetComponent<Camera>().fieldOfView += -Input.GetAxis("Mouse ScrollWheel") * num * 10f;
		}
		else if (this.terzaCamera.GetComponent<Camera>().fieldOfView > 10f && -Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			this.terzaCamera.GetComponent<Camera>().fieldOfView += -Input.GetAxis("Mouse ScrollWheel") * num * 10f;
		}
	}

	// Token: 0x06000285 RID: 645 RVA: 0x0006A7AC File Offset: 0x000689AC
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

	// Token: 0x06000286 RID: 646 RVA: 0x0006B214 File Offset: 0x00069414
	private void AttaccoIndipendente()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[0])
		{
			if (this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaAllPiùViciniPerTipo.Contains(base.gameObject) || this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] > 0.05f)
			{
				this.suonoArma.Play();
				this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] = 0f;
			}
			this.colpiBocca1.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
			this.timerFrequenzaArma1 = 0f;
			this.particelleBocca1.Play();
			this.particelleColpiBocca1.Play();
			List<float> listaValoriArma;
			List<float> expr_1A3 = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int index;
			int expr_1A6 = index = 5;
			float num = listaValoriArma[index];
			expr_1A3[expr_1A6] = num - 1f;
			List<float> listaValoriArma2;
			List<float> expr_1C9 = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int expr_1CD = index = 6;
			num = listaValoriArma2[index];
			expr_1C9[expr_1CD] = num - 1f;
			this.timerDopoSparo = 0f;
			float num2 = 0f;
			if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > this.munizione.GetComponent<DatiGeneraliMunizione>().penetrazione)
			{
				num2 = this.munizione.GetComponent<DatiGeneraliMunizione>().penetrazione;
			}
			else if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f)
			{
				num2 = this.unitàBersaglio.GetComponent<PresenzaNemico>().vita;
			}
			this.unitàBersaglio.GetComponent<PresenzaNemico>().vita -= this.munizione.GetComponent<DatiGeneraliMunizione>().penetrazione;
			if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > this.munizione.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.unitàBersaglio.GetComponent<PresenzaNemico>().armatura))
			{
				num2 += this.munizione.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.unitàBersaglio.GetComponent<PresenzaNemico>().armatura);
			}
			else if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f)
			{
				num2 += this.unitàBersaglio.GetComponent<PresenzaNemico>().vita;
			}
			this.unitàBersaglio.GetComponent<PresenzaNemico>().vita -= this.munizione.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.unitàBersaglio.GetComponent<PresenzaNemico>().armatura);
			List<float> listaDanniAlleati;
			List<float> expr_36A = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
			int expr_378 = index = base.GetComponent<PresenzaAlleato>().tipoTruppa;
			num = listaDanniAlleati[index];
			expr_36A[expr_378] = num + num2;
		}
	}

	// Token: 0x06000287 RID: 647 RVA: 0x0006B5B0 File Offset: 0x000697B0
	private void AttaccoPrimaPersona()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo))
		{
			float num = Vector3.Distance(base.transform.position, this.targetSparo.point);
			if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.CanvasFPS.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Max Range:   " + this.munizione.GetComponent<DatiGeneraliMunizione>().portataMassima.ToString("F2") + "\nDistance:   " + num.ToString("F2");
			}
			if (this.targetSparo.collider.gameObject.tag == "Nemico" || this.targetSparo.collider.gameObject.tag == "Nemico Testa" || this.targetSparo.collider.gameObject.tag == "Nemico Coll Suppl")
			{
				if (num <= this.munizione.GetComponent<DatiGeneraliMunizione>().portataMassima)
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
			List<float> expr_22E = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int index;
			int expr_231 = index = 5;
			float num2 = listaValoriArma[index];
			expr_22E[expr_231] = num2 - 1f;
			List<float> listaValoriArma2;
			List<float> expr_256 = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int expr_25A = index = 6;
			num2 = listaValoriArma2[index];
			expr_256[expr_25A] = num2 - 1f;
			this.avviaRinculo = true;
			this.timerRinculo = 0.3f;
			this.timerDopoSparo = 0f;
			if (!this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				Vector3 normalized = (this.targetSparo.point - this.colpiBocca1.transform.position).normalized;
				this.colpiBocca1.transform.forward = normalized;
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

	// Token: 0x06000288 RID: 648 RVA: 0x0006B8EC File Offset: 0x00069AEC
	private void Sparo()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo, this.munizione.GetComponent<DatiGeneraliMunizione>().portataMassima, this.layerColpo))
		{
			if (this.targetSparo.collider.gameObject.tag == "Nemico")
			{
				GameObject gameObject = this.targetSparo.collider.gameObject;
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
				float num = 0f;
				if (gameObject.GetComponent<PresenzaNemico>().vita > this.munizione.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
				{
					num = this.munizione.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				}
				else if (gameObject.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num = gameObject.GetComponent<PresenzaNemico>().vita;
				}
				gameObject.GetComponent<PresenzaNemico>().vita -= this.munizione.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				if (gameObject.GetComponent<PresenzaNemico>().vita > this.munizione.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS)
				{
					num += this.munizione.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
				}
				else if (gameObject.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num += gameObject.GetComponent<PresenzaNemico>().vita;
				}
				gameObject.GetComponent<PresenzaNemico>().vita -= this.munizione.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
				List<float> listaDanniAlleati;
				List<float> expr_202 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int tipoTruppa;
				int expr_210 = tipoTruppa = base.GetComponent<PresenzaAlleato>().tipoTruppa;
				float num2 = listaDanniAlleati[tipoTruppa];
				expr_202[expr_210] = num2 + num;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num;
			}
			else if (this.targetSparo.collider.gameObject.tag == "Nemico Testa")
			{
				GameObject gameObject2 = this.targetSparo.collider.transform.parent.gameObject;
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
				float num3 = 0f;
				if (gameObject2.GetComponent<PresenzaNemico>().vita > this.munizione.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f)
				{
					num3 = this.munizione.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f;
				}
				else if (gameObject2.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num3 = gameObject2.GetComponent<PresenzaNemico>().vita;
				}
				gameObject2.GetComponent<PresenzaNemico>().vita -= this.munizione.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f;
				if (gameObject2.GetComponent<PresenzaNemico>().vita > this.munizione.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject2.GetComponent<PresenzaNemico>().armatura) * 2f)
				{
					num3 += this.munizione.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject2.GetComponent<PresenzaNemico>().armatura) * 2f;
				}
				else if (gameObject2.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num3 += gameObject2.GetComponent<PresenzaNemico>().vita;
				}
				gameObject2.GetComponent<PresenzaNemico>().vita -= this.munizione.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject2.GetComponent<PresenzaNemico>().armatura) * 2f;
				List<float> listaDanniAlleati2;
				List<float> expr_405 = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int tipoTruppa;
				int expr_413 = tipoTruppa = base.GetComponent<PresenzaAlleato>().tipoTruppa;
				float num2 = listaDanniAlleati2[tipoTruppa];
				expr_405[expr_413] = num2 + num3;
			}
			else if (this.targetSparo.collider.gameObject.tag == "Nemico Coll Suppl")
			{
				GameObject gameObject3 = this.targetSparo.collider.transform.parent.gameObject;
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
				float num4 = 0f;
				if (gameObject3.GetComponent<PresenzaNemico>().vita > this.munizione.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
				{
					num4 = this.munizione.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				}
				else if (gameObject3.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num4 = gameObject3.GetComponent<PresenzaNemico>().vita;
				}
				gameObject3.GetComponent<PresenzaNemico>().vita -= this.munizione.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				if (gameObject3.GetComponent<PresenzaNemico>().vita > this.munizione.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject3.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS)
				{
					num4 += this.munizione.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject3.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
				}
				else if (gameObject3.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num4 += gameObject3.GetComponent<PresenzaNemico>().vita;
				}
				gameObject3.GetComponent<PresenzaNemico>().vita -= this.munizione.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject3.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
				List<float> listaDanniAlleati3;
				List<float> expr_603 = listaDanniAlleati3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int tipoTruppa;
				int expr_611 = tipoTruppa = base.GetComponent<PresenzaAlleato>().tipoTruppa;
				float num2 = listaDanniAlleati3[tipoTruppa];
				expr_603[expr_611] = num2 + num4;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num4;
			}
		}
	}

	// Token: 0x04000AB7 RID: 2743
	public float angVertMax;

	// Token: 0x04000AB8 RID: 2744
	public float angVertMin;

	// Token: 0x04000AB9 RID: 2745
	public GameObject bocca1;

	// Token: 0x04000ABA RID: 2746
	private GameObject colpiBocca1;

	// Token: 0x04000ABB RID: 2747
	public GameObject ossoArma1;

	// Token: 0x04000ABC RID: 2748
	public GameObject cappello;

	// Token: 0x04000ABD RID: 2749
	private float timerFrequenzaArma1;

	// Token: 0x04000ABE RID: 2750
	private float timerRicarica1;

	// Token: 0x04000ABF RID: 2751
	private float timerDopoSparo;

	// Token: 0x04000AC0 RID: 2752
	private float tempoFraSparoERicarica;

	// Token: 0x04000AC1 RID: 2753
	public Material verdeNormale;

	// Token: 0x04000AC2 RID: 2754
	public Material verdePerFantasma;

	// Token: 0x04000AC3 RID: 2755
	private GameObject CanvasFPS;

	// Token: 0x04000AC4 RID: 2756
	private GameObject mirinoFisso;

	// Token: 0x04000AC5 RID: 2757
	private GameObject mirinoDiPrecisione;

	// Token: 0x04000AC6 RID: 2758
	public Sprite spriteMirinoSniper;

	// Token: 0x04000AC7 RID: 2759
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000AC8 RID: 2760
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000AC9 RID: 2761
	public Vector3 aggiustamentoTraslVisualeFPS;

	// Token: 0x04000ACA RID: 2762
	public Vector3 aggiustamentoRotVisualeFPS;

	// Token: 0x04000ACB RID: 2763
	private float timerPosizionamentoTPS;

	// Token: 0x04000ACC RID: 2764
	private float timerPosizionamentoFPS;

	// Token: 0x04000ACD RID: 2765
	private float campoCameraIniziale;

	// Token: 0x04000ACE RID: 2766
	private GameObject infoNeutreTattica;

	// Token: 0x04000ACF RID: 2767
	private GameObject terzaCamera;

	// Token: 0x04000AD0 RID: 2768
	private GameObject primaCamera;

	// Token: 0x04000AD1 RID: 2769
	private NavMeshAgent alleatoNav;

	// Token: 0x04000AD2 RID: 2770
	private float velocitàAlleatoNav;

	// Token: 0x04000AD3 RID: 2771
	private GameObject IANemico;

	// Token: 0x04000AD4 RID: 2772
	private GameObject InfoAlleati;

	// Token: 0x04000AD5 RID: 2773
	private RaycastHit targetSparo;

	// Token: 0x04000AD6 RID: 2774
	private int layerColpo;

	// Token: 0x04000AD7 RID: 2775
	private int layerVisuale;

	// Token: 0x04000AD8 RID: 2776
	private GameObject unitàBersaglio;

	// Token: 0x04000AD9 RID: 2777
	private Vector3 centroUnitàBersaglio;

	// Token: 0x04000ADA RID: 2778
	public bool avviaRinculo;

	// Token: 0x04000ADB RID: 2779
	public float timerRinculo;

	// Token: 0x04000ADC RID: 2780
	private GameObject munizione;

	// Token: 0x04000ADD RID: 2781
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x04000ADE RID: 2782
	private bool primoFrameAvvenuto;

	// Token: 0x04000ADF RID: 2783
	private AudioSource suonoArma;

	// Token: 0x04000AE0 RID: 2784
	private AudioSource suonoRicarica;

	// Token: 0x04000AE1 RID: 2785
	private ParticleSystem particelleBocca1;

	// Token: 0x04000AE2 RID: 2786
	private ParticleSystem particelleColpiBocca1;

	// Token: 0x04000AE3 RID: 2787
	private float distFineOrdineMovimento;

	// Token: 0x04000AE4 RID: 2788
	private bool calcoloDistJump;

	// Token: 0x04000AE5 RID: 2789
	private bool calcoloJumpEffettuato;

	// Token: 0x04000AE6 RID: 2790
	private float velocitàIniziale;

	// Token: 0x04000AE7 RID: 2791
	private float timerAggRicerca;
}
