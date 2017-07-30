using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class ATT_FlamethrowerInfantry : MonoBehaviour
{
	// Token: 0x060001D6 RID: 470 RVA: 0x00050CD0 File Offset: 0x0004EED0
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
		this.particelleBoccaTPS = this.bocca1.GetComponent<ParticleSystem>();
		this.suonoFlamethrower = base.GetComponent<AudioSource>();
		this.tempoFraSparoERicarica = 0.2f;
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.velocitàIniziale = this.alleatoNav.speed;
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x00050E4C File Offset: 0x0004F04C
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
			if (Input.GetKeyDown(KeyCode.Q) && this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0 && base.gameObject == this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0])
			{
				this.particelleBoccaTPS.Stop();
				this.fiammaAttiva = false;
				this.suonoFlamethrower.Stop();
				this.suonoDuranteAvviato = false;
				this.suonoInizioAvviato = false;
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
					this.particelleBoccaTPS.Stop();
					this.fiammaAttiva = false;
					this.suonoDuranteAvviato = false;
					this.suonoInizioAvviato = false;
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
			this.terzaCamera.GetComponent<Camera>().fieldOfView = this.campoCameraIniziale;
		}
		if (this.unitàBersaglio == null && this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva != 3)
		{
			this.particelleBoccaTPS.Stop();
			this.fiammaAttiva = false;
			this.suonoFlamethrower.Stop();
			this.suonoDuranteAvviato = false;
			this.suonoInizioAvviato = false;
		}
		if (this.suonoFlamethrower.clip == this.suonoFiammaInizio && !this.suonoDuranteAvviato && this.suonoFlamethrower.time > 0.47f)
		{
			this.suonoFlamethrower.clip = this.suonoFiammaDurante;
			this.suonoFlamethrower.Play();
			this.suonoDuranteAvviato = true;
			this.suonoInizioAvviato = false;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x000512B0 File Offset: 0x0004F4B0
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

	// Token: 0x060001D9 RID: 473 RVA: 0x00051334 File Offset: 0x0004F534
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
					this.suonoDuranteAvviato = false;
					this.suonoInizioAvviato = false;
					this.suonoFlamethrower.clip = this.suonoRicarica;
					this.suonoFlamethrower.loop = false;
					this.suonoFlamethrower.Play();
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

	// Token: 0x060001DA RID: 474 RVA: 0x000515BC File Offset: 0x0004F7BC
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
				this.fiammaAttiva = false;
				this.suonoFlamethrower.Stop();
				this.suonoDuranteAvviato = false;
				this.suonoInizioAvviato = false;
			}
			this.CameraFPS();
			this.timerPosizionamentoTPS = 0f;
		}
	}

	// Token: 0x060001DB RID: 475 RVA: 0x000516F4 File Offset: 0x0004F8F4
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

	// Token: 0x060001DC RID: 476 RVA: 0x00051794 File Offset: 0x0004F994
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.ossoArma1.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 47f;
		}
	}

	// Token: 0x060001DD RID: 477 RVA: 0x00051814 File Offset: 0x0004FA14
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
								this.particelleBoccaTPS.Stop();
								this.fiammaAttiva = false;
								this.suonoFlamethrower.Stop();
								this.suonoDuranteAvviato = false;
								this.suonoInizioAvviato = false;
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
							else
							{
								this.particelleBoccaTPS.Stop();
								this.fiammaAttiva = false;
								this.suonoFlamethrower.Stop();
								this.suonoDuranteAvviato = false;
								this.suonoInizioAvviato = false;
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
							this.particelleBoccaTPS.Stop();
							this.fiammaAttiva = false;
							this.suonoFlamethrower.Stop();
							this.suonoDuranteAvviato = false;
							this.suonoInizioAvviato = false;
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
						else
						{
							this.particelleBoccaTPS.Stop();
							this.fiammaAttiva = false;
							this.suonoFlamethrower.Stop();
							this.suonoDuranteAvviato = false;
							this.suonoInizioAvviato = false;
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
			this.particelleBoccaTPS.Stop();
			this.fiammaAttiva = false;
			this.suonoFlamethrower.Stop();
			this.suonoDuranteAvviato = false;
			this.suonoInizioAvviato = false;
		}
	}

	// Token: 0x060001DE RID: 478 RVA: 0x00052350 File Offset: 0x00050550
	private void AttaccoIndipendente()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0])
		{
			if (this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f)
			{
				if (!Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale))
				{
					if (base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f)
					{
						if (!base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[0])
						{
							this.timerFrequenzaArma1 = 0f;
							List<float> listaValoriArma;
							List<float> expr_C9 = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
							int index;
							int expr_CD = index = 5;
							float num = listaValoriArma[index];
							expr_C9[expr_CD] = num - 1f;
							List<float> listaValoriArma2;
							List<float> expr_F3 = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
							int expr_F7 = index = 6;
							num = listaValoriArma2[index];
							expr_F3[expr_F7] = num - 1f;
							this.timerDopoSparo = 0f;
							if (!this.fiammaAttiva)
							{
								this.particelleBoccaTPS.Play();
								this.fiammaAttiva = true;
								if (!this.suonoInizioAvviato)
								{
									this.suonoFlamethrower.clip = this.suonoFiammaInizio;
									this.suonoFlamethrower.loop = true;
									this.suonoFlamethrower.Play();
									this.suonoDuranteAvviato = false;
									this.suonoInizioAvviato = true;
								}
							}
							for (int i = 0; i < this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma.Count; i++)
							{
								if (this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[i] == null && i < this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma.Count - 1)
								{
									this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[i] = this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[i + 1];
									this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[i + 1] = null;
								}
							}
							for (int j = 0; j < this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma.Count; j++)
							{
								if (this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[j] == null)
								{
									this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma.RemoveAt(j);
								}
							}
							foreach (GameObject current in this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma)
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
									List<float> expr_40F = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
									int expr_41D = index = base.GetComponent<PresenzaAlleato>().tipoTruppa;
									num = listaDanniAlleati[index];
									expr_40F[expr_41D] = num + num2;
								}
							}
						}
					}
					else
					{
						this.particelleBoccaTPS.Stop();
						this.fiammaAttiva = false;
						this.suonoDuranteAvviato = false;
						this.suonoInizioAvviato = false;
					}
				}
			}
			else
			{
				this.particelleBoccaTPS.Stop();
				this.fiammaAttiva = false;
				this.suonoFlamethrower.Stop();
				this.suonoDuranteAvviato = false;
				this.suonoInizioAvviato = false;
			}
		}
		else
		{
			this.particelleBoccaTPS.Stop();
			this.fiammaAttiva = false;
			this.suonoFlamethrower.Stop();
			this.suonoDuranteAvviato = false;
			this.suonoInizioAvviato = false;
		}
	}

	// Token: 0x060001DF RID: 479 RVA: 0x00052850 File Offset: 0x00050A50
	private void AttaccoPrimaPersona()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo, 9999f, this.layerColpo))
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
		if (Input.GetMouseButton(0) && this.pulsanteFuocoPremuto)
		{
			if (base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f)
			{
				if (!base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[1])
				{
					this.timerFrequenzaArma1 = 0f;
					this.Sparo();
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
					this.timerDopoSparo = 0f;
				}
			}
			else
			{
				this.particelleBoccaTPS.Stop();
				this.fiammaAttiva = false;
				this.suonoDuranteAvviato = false;
				this.suonoInizioAvviato = false;
			}
		}
		if (Input.GetMouseButtonDown(0) && !this.fiammaAttiva && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0])
		{
			this.pulsanteFuocoPremuto = true;
			if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.strutturaFPS1.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
				this.fiammaAttiva = true;
				if (!this.suonoInizioAvviato)
				{
					this.suonoFlamethrower.clip = this.suonoFiammaInizio;
					this.suonoFlamethrower.loop = true;
					this.suonoFlamethrower.Play();
					this.suonoDuranteAvviato = false;
					this.suonoInizioAvviato = true;
				}
			}
			else
			{
				this.particelleBoccaTPS.Play();
				this.fiammaAttiva = true;
				if (!this.suonoInizioAvviato)
				{
					this.suonoFlamethrower.clip = this.suonoFiammaInizio;
					this.suonoFlamethrower.loop = true;
					this.suonoFlamethrower.Play();
					this.suonoDuranteAvviato = false;
					this.suonoInizioAvviato = true;
				}
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.strutturaFPS1.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
				this.fiammaAttiva = false;
				if (!base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0])
				{
					this.suonoFlamethrower.Stop();
					this.suonoDuranteAvviato = false;
					this.suonoInizioAvviato = false;
				}
			}
			else
			{
				this.particelleBoccaTPS.Stop();
				this.fiammaAttiva = false;
				if (!base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0])
				{
					this.suonoFlamethrower.Stop();
					this.suonoDuranteAvviato = false;
					this.suonoInizioAvviato = false;
				}
			}
		}
		if (base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0])
		{
			if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.strutturaFPS1.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
			}
			else
			{
				this.particelleBoccaTPS.Stop();
			}
		}
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x00052C98 File Offset: 0x00050E98
	private void Sparo()
	{
		for (int i = 0; i < this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma.Count; i++)
		{
			if (this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[i] == null && i < this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma.Count - 1)
			{
				this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[i] = this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[i + 1];
				this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[i + 1] = null;
			}
		}
		for (int j = 0; j < this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma.Count; j++)
		{
			if (this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[j] == null)
			{
				this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma.RemoveAt(j);
			}
		}
		foreach (GameObject current in this.bocca1.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma)
		{
			if (current && current.tag == "Nemico")
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
				List<float> expr_2D3 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int tipoTruppa;
				int expr_2E1 = tipoTruppa = base.GetComponent<PresenzaAlleato>().tipoTruppa;
				float num2 = listaDanniAlleati[tipoTruppa];
				expr_2D3[expr_2E1] = num2 + num;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num;
			}
		}
	}

	// Token: 0x04000828 RID: 2088
	public float angVertMax;

	// Token: 0x04000829 RID: 2089
	public float angVertMin;

	// Token: 0x0400082A RID: 2090
	public GameObject bocca1;

	// Token: 0x0400082B RID: 2091
	public GameObject ossoArma1;

	// Token: 0x0400082C RID: 2092
	private float timerFrequenzaArma1;

	// Token: 0x0400082D RID: 2093
	private float timerRicarica1;

	// Token: 0x0400082E RID: 2094
	private float timerDopoSparo;

	// Token: 0x0400082F RID: 2095
	private float tempoFraSparoERicarica;

	// Token: 0x04000830 RID: 2096
	private GameObject CanvasFPS;

	// Token: 0x04000831 RID: 2097
	private GameObject mirinoFisso;

	// Token: 0x04000832 RID: 2098
	public GameObject strutturaPrefabFPS1;

	// Token: 0x04000833 RID: 2099
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000834 RID: 2100
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000835 RID: 2101
	public Vector3 aggiustamentoTraslVisualeFPS;

	// Token: 0x04000836 RID: 2102
	public Vector3 aggiustamentoRotVisualeFPS;

	// Token: 0x04000837 RID: 2103
	private GameObject strutturaFPS1;

	// Token: 0x04000838 RID: 2104
	private float timerPosizionamentoTPS;

	// Token: 0x04000839 RID: 2105
	private float timerPosizionamentoFPS;

	// Token: 0x0400083A RID: 2106
	private float campoCameraIniziale;

	// Token: 0x0400083B RID: 2107
	private GameObject infoNeutreTattica;

	// Token: 0x0400083C RID: 2108
	private GameObject terzaCamera;

	// Token: 0x0400083D RID: 2109
	private GameObject primaCamera;

	// Token: 0x0400083E RID: 2110
	private AudioSource suonoFlamethrower;

	// Token: 0x0400083F RID: 2111
	public AudioClip suonoFiammaInizio;

	// Token: 0x04000840 RID: 2112
	public AudioClip suonoFiammaDurante;

	// Token: 0x04000841 RID: 2113
	public AudioClip suonoRicarica;

	// Token: 0x04000842 RID: 2114
	private bool suonoInizioAvviato;

	// Token: 0x04000843 RID: 2115
	private bool suonoDuranteAvviato;

	// Token: 0x04000844 RID: 2116
	private NavMeshAgent alleatoNav;

	// Token: 0x04000845 RID: 2117
	private float velocitàAlleatoNav;

	// Token: 0x04000846 RID: 2118
	private GameObject IANemico;

	// Token: 0x04000847 RID: 2119
	private GameObject InfoAlleati;

	// Token: 0x04000848 RID: 2120
	private RaycastHit targetSparo;

	// Token: 0x04000849 RID: 2121
	private int layerColpo;

	// Token: 0x0400084A RID: 2122
	private int layerVisuale;

	// Token: 0x0400084B RID: 2123
	private GameObject unitàBersaglio;

	// Token: 0x0400084C RID: 2124
	private Vector3 centroUnitàBersaglio;

	// Token: 0x0400084D RID: 2125
	private GameObject munizione;

	// Token: 0x0400084E RID: 2126
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x0400084F RID: 2127
	private bool primoFrameAvvenuto;

	// Token: 0x04000850 RID: 2128
	private bool fiammaAttiva;

	// Token: 0x04000851 RID: 2129
	private ParticleSystem particelleBoccaTPS;

	// Token: 0x04000852 RID: 2130
	private bool pulsanteFuocoPremuto;

	// Token: 0x04000853 RID: 2131
	private float distFineOrdineMovimento;

	// Token: 0x04000854 RID: 2132
	private bool calcoloDistJump;

	// Token: 0x04000855 RID: 2133
	private bool calcoloJumpEffettuato;

	// Token: 0x04000856 RID: 2134
	private float velocitàIniziale;

	// Token: 0x04000857 RID: 2135
	private float timerAggRicerca;
}
