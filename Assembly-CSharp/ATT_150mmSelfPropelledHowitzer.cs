using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000037 RID: 55
public class ATT_150mmSelfPropelledHowitzer : MonoBehaviour
{
	// Token: 0x0600029B RID: 667 RVA: 0x0006DD6C File Offset: 0x0006BF6C
	private void Start()
	{
		this.CanvasFPS = GameObject.FindGameObjectWithTag("CanvasFPS");
		this.angolazioneMortairTPS = this.CanvasFPS.transform.GetChild(2).transform.GetChild(7).gameObject;
		this.angolazioneMortairTPSLancetta = this.CanvasFPS.transform.GetChild(2).transform.GetChild(7).gameObject.transform.GetChild(0).gameObject;
		this.angolazioneTesto = this.CanvasFPS.transform.GetChild(2).transform.GetChild(7).gameObject.transform.GetChild(1).gameObject;
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.InfoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.attacchiSpecialiAlleati = GameObject.FindGameObjectWithTag("Attacchi Speciali Alleati");
		this.alleatoNav = base.GetComponent<NavMeshAgent>();
		this.velocitàAlleatoNav = base.GetComponent<NavMeshAgent>().speed;
		this.layerColpo = 165120;
		this.layerVisuale = 256;
		this.campoCameraIniziale = this.terzaCamera.GetComponent<Camera>().fieldOfView;
		this.ListaMunizioniAttiveUnità = new List<GameObject>();
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma1);
		this.suonoTorretta = this.baseTorretta.GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.volumeMotoreIniziale = this.suonoMotore.volume;
		this.suonoMotore.clip = this.motoreFermo;
		this.suonoMotore.Play();
		this.valoreAngolo = -15f;
		this.AngMinPP = base.GetComponent<MOV_150mmSelfPropelledHowitzer>().angCannoniVertMin;
		this.AngMaxPP = base.GetComponent<MOV_150mmSelfPropelledHowitzer>().angCannoniVertMax;
		this.cannone = this.baseCannone.transform.GetChild(0).transform.GetChild(0).gameObject;
		this.tempoFraSparoERicarica1 = 1f;
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.ritardoFrequenzaArma = UnityEngine.Random.Range(-this.intervalloDiRitardoFreqArma, this.intervalloDiRitardoFreqArma);
	}

	// Token: 0x0600029C RID: 668 RVA: 0x0006DFD4 File Offset: 0x0006C1D4
	private void Update()
	{
		this.ListaMunizioniAttiveUnità[0] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[0][0];
		this.munizioneArma1 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0];
		this.timerFrequenzaArma1 += Time.deltaTime;
		this.timerDopoSparo1 += Time.deltaTime;
		this.CondizioniArma1();
		this.RinculoCannone();
		this.unitàBersaglio = base.GetComponent<PresenzaAlleato>().unitàBersaglio;
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.GestioneSuoniIndipendenti();
			this.PreparazioneAttacco();
		}
		else
		{
			this.GestioneVisuali();
			this.AlzoCannonePrimaPersona();
			this.AttaccoPrimaPersonaArma1();
			base.GetComponent<NavMeshAgent>().enabled = false;
			if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
			{
				this.angolazioneMortairTPS.GetComponent<CanvasGroup>().alpha = 1f;
			}
			if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva == 3)
			{
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
					this.angolazioneMortairTPS.GetComponent<CanvasGroup>().alpha = 1f;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
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
			this.angolazioneMortairTPS.GetComponent<CanvasGroup>().alpha = 0f;
			base.GetComponent<MOV_150mmSelfPropelledHowitzer>().torretta.transform.rotation = base.transform.rotation;
			this.suonoTorretta.Stop();
			base.GetComponent<MOV_150mmSelfPropelledHowitzer>().suonoTorrettaPartito = false;
			this.baseCannone.transform.localEulerAngles = new Vector3(-40f, 0f, 0f);
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x0600029D RID: 669 RVA: 0x0006E268 File Offset: 0x0006C468
	private void GestioneSuoniIndipendenti()
	{
		this.suonoMotore.volume = this.volumeMotoreIniziale;
		if (this.alleatoNav.velocity.magnitude > 0f)
		{
			this.timerPartenza += Time.deltaTime;
			this.timerStop = 0f;
			this.inStop = false;
			this.stopFinito = false;
		}
		if (!this.inPartenza && this.timerPartenza > 0f)
		{
			this.suonoMotore.clip = this.motorePartenza;
			this.suonoMotore.Play();
			this.inPartenza = true;
		}
		if (!this.partenzaFinita && this.timerPartenza > this.motorePartenza.length)
		{
			this.suonoMotore.clip = this.motoreViaggio;
			this.suonoMotore.Play();
			this.partenzaFinita = true;
		}
		if (this.alleatoNav.velocity.magnitude == 0f)
		{
			this.timerStop += Time.deltaTime;
			this.timerPartenza = 0f;
			this.inPartenza = false;
			this.partenzaFinita = false;
		}
		if (!this.inStop && this.timerStop > 0f)
		{
			this.suonoMotore.clip = this.motoreStop;
			this.suonoMotore.Play();
			this.inStop = true;
		}
		if (!this.stopFinito && this.timerStop > this.motoreStop.length)
		{
			this.suonoMotore.clip = this.motoreFermo;
			this.suonoMotore.Play();
			this.stopFinito = true;
		}
	}

	// Token: 0x0600029E RID: 670 RVA: 0x0006E41C File Offset: 0x0006C61C
	private void CondizioniArma1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] <= 0f && this.timerDopoSparo1 > this.tempoFraSparoERicarica1)
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
					this.bocca1.transform.parent.GetComponent<AudioSource>().Play();
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

	// Token: 0x0600029F RID: 671 RVA: 0x0006E688 File Offset: 0x0006C888
	private void RinculoCannone()
	{
		if (this.cannoneSparato)
		{
			if (this.cannone.transform.localPosition.y < -2.1f && !this.cannoneFinoInFondo)
			{
				this.cannone.transform.localPosition += base.transform.up * 15f * Time.deltaTime;
			}
			if (this.cannone.transform.localPosition.y >= -2.1f && !this.cannoneFinoInFondo)
			{
				this.cannoneFinoInFondo = true;
			}
			if (this.cannone.transform.localPosition.y > -4.58f && this.cannoneFinoInFondo)
			{
				this.cannone.transform.localPosition += -base.transform.up * 2f * Time.deltaTime;
			}
			if (this.cannone.transform.localPosition.y <= -4.58f && this.cannoneFinoInFondo)
			{
				this.cannoneFinoInFondo = false;
				this.cannoneSparato = false;
			}
		}
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x0006E7E4 File Offset: 0x0006C9E4
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
		if (this.valoreAngolo < this.AngMaxPP)
		{
			this.valoreAngolo += Mathf.Abs(Input.GetAxis("Mouse ScrollWheel") * 3f);
			if (Input.GetKey(KeyCode.PageDown))
			{
				this.valoreAngolo -= 0.03f;
			}
		}
		else if (this.valoreAngolo > this.AngMinPP)
		{
			this.valoreAngolo -= Mathf.Abs(Input.GetAxis("Mouse ScrollWheel") * 3f);
			if (Input.GetKey(KeyCode.PageUp))
			{
				this.valoreAngolo += 0.03f;
			}
		}
		else
		{
			this.valoreAngolo += Input.GetAxis("Mouse ScrollWheel") * 3f;
			if (Input.GetKey(KeyCode.PageUp))
			{
				this.valoreAngolo -= 0.03f;
			}
			else if (Input.GetKey(KeyCode.PageDown))
			{
				this.valoreAngolo += 0.03f;
			}
		}
		this.angolazioneMortairTPSLancetta.transform.eulerAngles = new Vector3(0f, 0f, this.valoreAngolo);
		this.angolazioneTesto.GetComponent<Text>().text = Mathf.Abs(this.valoreAngolo).ToString("F1") + "°";
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x0006E9AC File Offset: 0x0006CBAC
	private void CameraTPS()
	{
		this.timerPosizionamentoTPS += Time.deltaTime;
		if (this.timerPosizionamentoTPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.baseTorretta.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraTPS;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = this.campoCameraIniziale;
		}
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x0006EA2C File Offset: 0x0006CC2C
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.baseTorretta.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
		}
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x0006EA98 File Offset: 0x0006CC98
	private void PreparazioneAttacco()
	{
		if (this.unitàBersaglio)
		{
			this.centroUnitàBersaglio = this.unitàBersaglio.GetComponent<PresenzaNemico>().centroInsetto;
		}
		float num = Vector3.Distance(base.transform.position, this.alleatoNav.destination);
		if (num <= this.distFineOrdineMovimento)
		{
			base.GetComponent<PresenzaAlleato>().destinazioneOrdinata = false;
		}
		float portataMinima = this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMinima;
		float portataMassima = this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMassima;
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
						if (num3 > portataMassima)
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
						if (num3 > portataMinima && num3 < portataMassima)
						{
							base.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
							this.alleatoNav.speed = 0f;
							this.AttaccoIndipendente1();
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
					if (num5 > portataMassima)
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
					else if (num5 > portataMinima && num5 < portataMassima)
					{
						base.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
						this.alleatoNav.speed = 0f;
						this.AttaccoIndipendente1();
					}
					else if (num5 < portataMinima)
					{
						this.alleatoNav.speed = this.velocitàAlleatoNav;
						this.unitàBersaglio = null;
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
								if (num6 < this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMassima && num6 > this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMinima)
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
								if (num8 < this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMassima && num8 > this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMinima)
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
		if (base.GetComponent<PresenzaAlleato>().attaccoZonaOrdinato && this.alleatoNav.enabled)
		{
			Vector3 luogoAttZonaArt = base.GetComponent<PresenzaAlleato>().luogoAttZonaArt;
			float num12 = Vector3.Distance(luogoAttZonaArt, base.transform.position);
			float num13 = Vector3.Dot((base.GetComponent<PresenzaAlleato>().luogoAttZonaArt - base.transform.position).normalized, base.transform.up);
			if (num13 < this.angVertMax && num13 > this.angVertMin)
			{
				if (num12 < portataMinima || num12 > portataMassima)
				{
					if (!base.GetComponent<PresenzaAlleato>().comportamentoDifensivo && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino)
					{
						this.alleatoNav.SetDestination(luogoAttZonaArt);
						this.alleatoNav.speed = this.velocitàAlleatoNav;
					}
					else
					{
						base.transform.LookAt(luogoAttZonaArt);
						this.alleatoNav.speed = 0f;
					}
				}
				if (num12 > portataMinima && num12 < portataMassima)
				{
					base.transform.LookAt(luogoAttZonaArt);
					this.alleatoNav.speed = 0f;
					if (base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[0] + this.ritardoFrequenzaArma)
					{
						if (this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaAllPiùViciniPerTipo.Contains(base.gameObject) || this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] > 0.05f)
						{
							this.bocca1.GetComponent<AudioSource>().Play();
							this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] = 0f;
						}
						this.timerFrequenzaArma1 = 0f;
						this.ritardoFrequenzaArma = UnityEngine.Random.Range(-this.intervalloDiRitardoFreqArma, this.intervalloDiRitardoFreqArma);
						this.SparoIndipendenteZona();
						this.bocca1.GetComponent<ParticleSystem>().Play();
						List<float> listaValoriArma;
						List<float> expr_B3E = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
						int index;
						int expr_B42 = index = 5;
						float num14 = listaValoriArma[index];
						expr_B3E[expr_B42] = num14 - 1f;
						List<float> listaValoriArma2;
						List<float> expr_B68 = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
						int expr_B6C = index = 6;
						num14 = listaValoriArma2[index];
						expr_B68[expr_B6C] = num14 - 1f;
					}
				}
			}
			else if (num12 > 3f)
			{
				this.alleatoNav.speed = this.velocitàAlleatoNav;
				this.unitàBersaglio = null;
			}
		}
		if (base.GetComponent<PresenzaAlleato>().destinazioneOrdinata)
		{
			this.alleatoNav.speed = this.velocitàAlleatoNav;
			this.unitàBersaglio = null;
		}
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x0006F6B4 File Offset: 0x0006D8B4
	private void AttaccoIndipendente1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[0] + this.ritardoFrequenzaArma)
		{
			if (this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaAllPiùViciniPerTipo.Contains(base.gameObject) || this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] > 0.05f)
			{
				this.bocca1.GetComponent<AudioSource>().Play();
				this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] = 0f;
			}
			this.timerFrequenzaArma1 = 0f;
			this.ritardoFrequenzaArma = UnityEngine.Random.Range(-this.intervalloDiRitardoFreqArma, this.intervalloDiRitardoFreqArma);
			this.bocca1.GetComponent<ParticleSystem>().Play();
			List<float> listaValoriArma;
			List<float> expr_147 = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int index;
			int expr_14A = index = 5;
			float num = listaValoriArma[index];
			expr_147[expr_14A] = num - 1f;
			List<float> listaValoriArma2;
			List<float> expr_16B = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int expr_16E = index = 6;
			num = listaValoriArma2[index];
			expr_16B[expr_16E] = num - 1f;
			this.SparoIndipendente1();
		}
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x0006F84C File Offset: 0x0006DA4C
	private void SparoIndipendente1()
	{
		this.proiettileArtiglieria = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		float num = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
		this.ampiezzaCerchioPrecisione = base.GetComponent<PresenzaAlleato>().valoreInizialePrecisione + num / base.GetComponent<PresenzaAlleato>().valorePerditaPrecisione;
		GestoreNeutroStrategia.valoreRandomSeed++;
		UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
		float num2 = (float)UnityEngine.Random.Range(-1, 1) * this.ampiezzaCerchioPrecisione;
		float num3 = (float)UnityEngine.Random.Range(-1, 1) * this.ampiezzaCerchioPrecisione;
		Vector3 locazioneTarget = new Vector3(this.centroUnitàBersaglio.x + num2, this.centroUnitàBersaglio.y, this.centroUnitàBersaglio.z + num3);
		this.proiettileArtiglieria.GetComponent<DatiProiettile>().locazioneTarget = locazioneTarget;
		this.cannoneSparato = true;
		this.proiettileArtiglieria.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
		this.timerDopoSparo1 = 0f;
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x0006F968 File Offset: 0x0006DB68
	private void SparoIndipendenteZona()
	{
		this.proiettileArtiglieria = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		Vector3 luogoAttZonaArt = base.GetComponent<PresenzaAlleato>().luogoAttZonaArt;
		float num = Vector3.Distance(base.transform.position, luogoAttZonaArt);
		this.ampiezzaCerchioPrecisione = base.GetComponent<PresenzaAlleato>().valoreInizialePrecisione + num / base.GetComponent<PresenzaAlleato>().valorePerditaPrecisione;
		GestoreNeutroStrategia.valoreRandomSeed++;
		UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
		float num2 = UnityEngine.Random.Range(-1f, 1f) * this.ampiezzaCerchioPrecisione;
		float num3 = UnityEngine.Random.Range(-1f, 1f) * this.ampiezzaCerchioPrecisione;
		Vector3 locazioneTarget = new Vector3(luogoAttZonaArt.x + num2, luogoAttZonaArt.y, luogoAttZonaArt.z + num3);
		this.proiettileArtiglieria.GetComponent<DatiProiettile>().locazioneTarget = locazioneTarget;
		this.cannoneSparato = true;
		this.proiettileArtiglieria.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
		this.timerDopoSparo1 = 0f;
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x0006FA8C File Offset: 0x0006DC8C
	private void AlzoCannonePrimaPersona()
	{
		this.baseCannone.transform.localEulerAngles = new Vector3(this.valoreAngolo, 0f, 0f);
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x0006FAB4 File Offset: 0x0006DCB4
	private void AttaccoPrimaPersonaArma1()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo))
		{
			if (this.targetSparo.collider.gameObject.tag == "Nemico" || this.targetSparo.collider.gameObject.tag == "Nemico Testa" || this.targetSparo.collider.gameObject.tag == "Nemico Coll Suppl")
			{
				if (Vector3.Distance(base.transform.position, this.targetSparo.point) > this.ListaMunizioniAttiveUnità[0].GetComponent<DatiGeneraliMunizione>().portataMinima && Vector3.Distance(base.transform.position, this.targetSparo.point) <= this.ListaMunizioniAttiveUnità[0].GetComponent<DatiGeneraliMunizione>().portataMassima)
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
		if (Input.GetMouseButton(0) && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][1])
		{
			this.timerFrequenzaArma1 = 0f;
			this.SparoArma1();
			this.bocca1.GetComponent<AudioSource>().Play();
			this.bocca1.GetComponent<ParticleSystem>().Play();
			List<float> list;
			List<float> expr_1DE = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int index;
			int expr_1E1 = index = 5;
			float num = list[index];
			expr_1DE[expr_1E1] = num - 1f;
			List<float> list2;
			List<float> expr_208 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int expr_20C = index = 6;
			num = list2[index];
			expr_208[expr_20C] = num - 1f;
			this.timerDopoSparo1 = 0f;
			if (!this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.bocca1.GetComponent<ParticleSystem>().Play();
			}
		}
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x0006FD14 File Offset: 0x0006DF14
	private void SparoArma1()
	{
		this.proiettileArtiglieria = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.proiettileArtiglieria.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.cannoneSparato = true;
		this.proiettileArtiglieria.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x04000B19 RID: 2841
	public float angVertMax;

	// Token: 0x04000B1A RID: 2842
	public float angVertMin;

	// Token: 0x04000B1B RID: 2843
	private GameObject infoNeutreTattica;

	// Token: 0x04000B1C RID: 2844
	private GameObject terzaCamera;

	// Token: 0x04000B1D RID: 2845
	private GameObject primaCamera;

	// Token: 0x04000B1E RID: 2846
	public GameObject bocca1;

	// Token: 0x04000B1F RID: 2847
	private GameObject IANemico;

	// Token: 0x04000B20 RID: 2848
	private GameObject InfoAlleati;

	// Token: 0x04000B21 RID: 2849
	private GameObject attacchiSpecialiAlleati;

	// Token: 0x04000B22 RID: 2850
	private float timerFrequenzaArma1;

	// Token: 0x04000B23 RID: 2851
	private float timerRicarica1;

	// Token: 0x04000B24 RID: 2852
	private bool ricaricaInCorso1;

	// Token: 0x04000B25 RID: 2853
	private float timerDopoSparo1;

	// Token: 0x04000B26 RID: 2854
	private float tempoFraSparoERicarica1;

	// Token: 0x04000B27 RID: 2855
	private float ritardoFrequenzaArma;

	// Token: 0x04000B28 RID: 2856
	public float intervalloDiRitardoFreqArma;

	// Token: 0x04000B29 RID: 2857
	private int layerColpo;

	// Token: 0x04000B2A RID: 2858
	private int layerVisuale;

	// Token: 0x04000B2B RID: 2859
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000B2C RID: 2860
	public Vector3 rotazioneCameraTPS;

	// Token: 0x04000B2D RID: 2861
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000B2E RID: 2862
	private float timerPosizionamentoTPS;

	// Token: 0x04000B2F RID: 2863
	private float timerPosizionamentoFPS;

	// Token: 0x04000B30 RID: 2864
	private float campoCameraIniziale;

	// Token: 0x04000B31 RID: 2865
	private GameObject CanvasFPS;

	// Token: 0x04000B32 RID: 2866
	private GameObject angolazioneMortairTPS;

	// Token: 0x04000B33 RID: 2867
	private GameObject angolazioneMortairTPSLancetta;

	// Token: 0x04000B34 RID: 2868
	private GameObject angolazioneTesto;

	// Token: 0x04000B35 RID: 2869
	private float valoreAngolo;

	// Token: 0x04000B36 RID: 2870
	private RaycastHit targetSparo;

	// Token: 0x04000B37 RID: 2871
	private GameObject proiettileArtiglieria;

	// Token: 0x04000B38 RID: 2872
	private NavMeshAgent alleatoNav;

	// Token: 0x04000B39 RID: 2873
	private float velocitàAlleatoNav;

	// Token: 0x04000B3A RID: 2874
	public GameObject baseCannone;

	// Token: 0x04000B3B RID: 2875
	public GameObject baseTorretta;

	// Token: 0x04000B3C RID: 2876
	private GameObject cannone;

	// Token: 0x04000B3D RID: 2877
	private GameObject unitàBersaglio;

	// Token: 0x04000B3E RID: 2878
	private Vector3 centroUnitàBersaglio;

	// Token: 0x04000B3F RID: 2879
	private GameObject munizioneArma1;

	// Token: 0x04000B40 RID: 2880
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x04000B41 RID: 2881
	private float AngMinPP;

	// Token: 0x04000B42 RID: 2882
	private float AngMaxPP;

	// Token: 0x04000B43 RID: 2883
	public GameObject provaDistanza;

	// Token: 0x04000B44 RID: 2884
	private bool suonoRicaricaAttivo;

	// Token: 0x04000B45 RID: 2885
	public float ampiezzaCerchioPrecisione;

	// Token: 0x04000B46 RID: 2886
	private AudioSource suonoTorretta;

	// Token: 0x04000B47 RID: 2887
	private AudioSource suonoMotore;

	// Token: 0x04000B48 RID: 2888
	public AudioClip motoreFermo;

	// Token: 0x04000B49 RID: 2889
	public AudioClip motorePartenza;

	// Token: 0x04000B4A RID: 2890
	public AudioClip motoreViaggio;

	// Token: 0x04000B4B RID: 2891
	public AudioClip motoreStop;

	// Token: 0x04000B4C RID: 2892
	private float timerPartenza;

	// Token: 0x04000B4D RID: 2893
	private float timerStop;

	// Token: 0x04000B4E RID: 2894
	private bool primaPartenza;

	// Token: 0x04000B4F RID: 2895
	public float volumeMotoreIniziale;

	// Token: 0x04000B50 RID: 2896
	private bool inPartenza;

	// Token: 0x04000B51 RID: 2897
	private bool partenzaFinita;

	// Token: 0x04000B52 RID: 2898
	private bool inStop;

	// Token: 0x04000B53 RID: 2899
	public bool stopFinito;

	// Token: 0x04000B54 RID: 2900
	private bool cannoneSparato;

	// Token: 0x04000B55 RID: 2901
	private bool cannoneFinoInFondo;

	// Token: 0x04000B56 RID: 2902
	private float distFineOrdineMovimento;

	// Token: 0x04000B57 RID: 2903
	private float timerAggRicerca;
}
