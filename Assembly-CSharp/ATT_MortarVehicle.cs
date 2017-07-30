using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000042 RID: 66
public class ATT_MortarVehicle : MonoBehaviour
{
	// Token: 0x0600035C RID: 860 RVA: 0x0008AEA0 File Offset: 0x000890A0
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
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.suonoMotore.clip = this.motoreFermo;
		this.suonoMotore.Play();
		this.valoreAngolo = -70f;
		this.AngMinPP = base.GetComponent<MOV_MortarVehicle>().angCannoniVertMin;
		this.AngMaxPP = base.GetComponent<MOV_MortarVehicle>().angCannoniVertMax;
		this.tempoFraSparoERicarica1 = 1f;
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.ritardoFrequenzaArma = UnityEngine.Random.Range(-this.intervalloDiRitardoFreqArma, this.intervalloDiRitardoFreqArma);
	}

	// Token: 0x0600035D RID: 861 RVA: 0x0008B0C0 File Offset: 0x000892C0
	private void Update()
	{
		this.ListaMunizioniAttiveUnità[0] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[0][0];
		this.munizioneArma1 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0];
		this.timerFrequenzaArma1 += Time.deltaTime;
		this.timerDopoSparo1 += Time.deltaTime;
		this.CondizioniArma1();
		this.unitàBersaglio = base.GetComponent<PresenzaAlleato>().unitàBersaglio;
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.GestioneSuoniIndipendenti();
			this.PreparazioneAttacco();
		}
		else
		{
			this.GestioneVisuali();
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
			this.baseTorretta.transform.eulerAngles = new Vector3(0f, 180f, 0f);
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x0600035E RID: 862 RVA: 0x0008B310 File Offset: 0x00089510
	private void GestioneSuoniIndipendenti()
	{
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

	// Token: 0x0600035F RID: 863 RVA: 0x0008B4B4 File Offset: 0x000896B4
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

	// Token: 0x06000360 RID: 864 RVA: 0x0008B720 File Offset: 0x00089920
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

	// Token: 0x06000361 RID: 865 RVA: 0x0008B8E8 File Offset: 0x00089AE8
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

	// Token: 0x06000362 RID: 866 RVA: 0x0008B968 File Offset: 0x00089B68
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.baseTorretta.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
		}
	}

	// Token: 0x06000363 RID: 867 RVA: 0x0008B9D4 File Offset: 0x00089BD4
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
						base.transform.LookAt(new Vector3(luogoAttZonaArt.x, base.transform.position.y, luogoAttZonaArt.z));
						this.alleatoNav.speed = 0f;
					}
				}
				if (num12 > portataMinima && num12 < portataMassima)
				{
					base.transform.LookAt(new Vector3(luogoAttZonaArt.x, base.transform.position.y, luogoAttZonaArt.z));
					this.alleatoNav.speed = 0f;
					if (base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[0] + this.ritardoFrequenzaArma)
					{
						this.timerFrequenzaArma1 = 0f;
						this.ritardoFrequenzaArma = UnityEngine.Random.Range(-this.intervalloDiRitardoFreqArma, this.intervalloDiRitardoFreqArma);
						this.SparoIndipendenteZona();
						this.bocca1.GetComponent<ParticleSystem>().Play();
						this.bocca1.GetComponent<AudioSource>().Play();
						List<float> listaValoriArma;
						List<float> expr_B0D = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
						int index;
						int expr_B11 = index = 5;
						float num14 = listaValoriArma[index];
						expr_B0D[expr_B11] = num14 - 1f;
						List<float> listaValoriArma2;
						List<float> expr_B37 = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
						int expr_B3B = index = 6;
						num14 = listaValoriArma2[index];
						expr_B37[expr_B3B] = num14 - 1f;
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

	// Token: 0x06000364 RID: 868 RVA: 0x0008C5BC File Offset: 0x0008A7BC
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

	// Token: 0x06000365 RID: 869 RVA: 0x0008C754 File Offset: 0x0008A954
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
		this.proiettileArtiglieria.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
		this.timerDopoSparo1 = 0f;
	}

	// Token: 0x06000366 RID: 870 RVA: 0x0008C868 File Offset: 0x0008AA68
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
		this.proiettileArtiglieria.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
		this.timerDopoSparo1 = 0f;
	}

	// Token: 0x06000367 RID: 871 RVA: 0x0008C984 File Offset: 0x0008AB84
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

	// Token: 0x06000368 RID: 872 RVA: 0x0008CBE4 File Offset: 0x0008ADE4
	private void SparoArma1()
	{
		this.bocca1.transform.eulerAngles = new Vector3(this.valoreAngolo, this.bocca1.transform.eulerAngles.y, this.bocca1.transform.eulerAngles.z);
		this.proiettileArtiglieria = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.proiettileArtiglieria.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.proiettileArtiglieria.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x04000E23 RID: 3619
	public float angVertMax;

	// Token: 0x04000E24 RID: 3620
	public float angVertMin;

	// Token: 0x04000E25 RID: 3621
	private GameObject infoNeutreTattica;

	// Token: 0x04000E26 RID: 3622
	private GameObject terzaCamera;

	// Token: 0x04000E27 RID: 3623
	private GameObject primaCamera;

	// Token: 0x04000E28 RID: 3624
	public GameObject bocca1;

	// Token: 0x04000E29 RID: 3625
	private GameObject IANemico;

	// Token: 0x04000E2A RID: 3626
	private GameObject InfoAlleati;

	// Token: 0x04000E2B RID: 3627
	private GameObject attacchiSpecialiAlleati;

	// Token: 0x04000E2C RID: 3628
	private float timerFrequenzaArma1;

	// Token: 0x04000E2D RID: 3629
	private float timerRicarica1;

	// Token: 0x04000E2E RID: 3630
	private bool ricaricaInCorso1;

	// Token: 0x04000E2F RID: 3631
	private float timerDopoSparo1;

	// Token: 0x04000E30 RID: 3632
	private float tempoFraSparoERicarica1;

	// Token: 0x04000E31 RID: 3633
	private float ritardoFrequenzaArma;

	// Token: 0x04000E32 RID: 3634
	public float intervalloDiRitardoFreqArma;

	// Token: 0x04000E33 RID: 3635
	private int layerColpo;

	// Token: 0x04000E34 RID: 3636
	private int layerVisuale;

	// Token: 0x04000E35 RID: 3637
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000E36 RID: 3638
	public Vector3 rotazioneCameraTPS;

	// Token: 0x04000E37 RID: 3639
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000E38 RID: 3640
	private float timerPosizionamentoTPS;

	// Token: 0x04000E39 RID: 3641
	private float timerPosizionamentoFPS;

	// Token: 0x04000E3A RID: 3642
	private float campoCameraIniziale;

	// Token: 0x04000E3B RID: 3643
	private GameObject CanvasFPS;

	// Token: 0x04000E3C RID: 3644
	private GameObject angolazioneMortairTPS;

	// Token: 0x04000E3D RID: 3645
	private GameObject angolazioneMortairTPSLancetta;

	// Token: 0x04000E3E RID: 3646
	private GameObject angolazioneTesto;

	// Token: 0x04000E3F RID: 3647
	private float valoreAngolo;

	// Token: 0x04000E40 RID: 3648
	private RaycastHit targetSparo;

	// Token: 0x04000E41 RID: 3649
	private GameObject proiettileArtiglieria;

	// Token: 0x04000E42 RID: 3650
	private NavMeshAgent alleatoNav;

	// Token: 0x04000E43 RID: 3651
	private float velocitàAlleatoNav;

	// Token: 0x04000E44 RID: 3652
	public GameObject baseTorretta;

	// Token: 0x04000E45 RID: 3653
	private GameObject unitàBersaglio;

	// Token: 0x04000E46 RID: 3654
	private Vector3 centroUnitàBersaglio;

	// Token: 0x04000E47 RID: 3655
	private GameObject munizioneArma1;

	// Token: 0x04000E48 RID: 3656
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x04000E49 RID: 3657
	private float AngMinPP;

	// Token: 0x04000E4A RID: 3658
	private float AngMaxPP;

	// Token: 0x04000E4B RID: 3659
	public GameObject provaDistanza;

	// Token: 0x04000E4C RID: 3660
	private bool suonoRicaricaAttivo;

	// Token: 0x04000E4D RID: 3661
	public float ampiezzaCerchioPrecisione;

	// Token: 0x04000E4E RID: 3662
	private AudioSource suonoMotore;

	// Token: 0x04000E4F RID: 3663
	public AudioClip motoreFermo;

	// Token: 0x04000E50 RID: 3664
	public AudioClip motorePartenza;

	// Token: 0x04000E51 RID: 3665
	public AudioClip motoreViaggio;

	// Token: 0x04000E52 RID: 3666
	public AudioClip motoreStop;

	// Token: 0x04000E53 RID: 3667
	private float timerPartenza;

	// Token: 0x04000E54 RID: 3668
	private float timerStop;

	// Token: 0x04000E55 RID: 3669
	private bool primaPartenza;

	// Token: 0x04000E56 RID: 3670
	private bool inPartenza;

	// Token: 0x04000E57 RID: 3671
	private bool partenzaFinita;

	// Token: 0x04000E58 RID: 3672
	private bool inStop;

	// Token: 0x04000E59 RID: 3673
	public bool stopFinito;

	// Token: 0x04000E5A RID: 3674
	private float distFineOrdineMovimento;

	// Token: 0x04000E5B RID: 3675
	private float timerAggRicerca;
}
