using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000038 RID: 56
public class ATT_320mmSelfPropelledHowitzer : MonoBehaviour
{
	// Token: 0x060002AB RID: 683 RVA: 0x0006FD94 File Offset: 0x0006DF94
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
		this.volumeMotoreIniziale = this.suonoMotore.volume;
		this.suonoMotore.clip = this.motoreFermo;
		this.suonoMotore.Play();
		this.valoreAngolo = -30f;
		this.AngMinPP = base.GetComponent<MOV_320mmSelfPropelledHowitzer>().angCannoniVertMin;
		this.AngMaxPP = base.GetComponent<MOV_320mmSelfPropelledHowitzer>().angCannoniVertMax;
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.cannone = this.baseCannone.transform.GetChild(0).transform.GetChild(0).gameObject;
		this.ritardoFrequenzaArma = UnityEngine.Random.Range(-this.intervalloDiRitardoFreqArma, this.intervalloDiRitardoFreqArma);
	}

	// Token: 0x060002AC RID: 684 RVA: 0x0006FFE0 File Offset: 0x0006E1E0
	private void Update()
	{
		this.ListaMunizioniAttiveUnità[0] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[0][0];
		this.munizioneArma1 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0];
		this.timerFrequenzaArma1 += Time.deltaTime;
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
			this.baseCannone.transform.localEulerAngles = new Vector3(-30f, 0f, 0f);
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x060002AD RID: 685 RVA: 0x0007022C File Offset: 0x0006E42C
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

	// Token: 0x060002AE RID: 686 RVA: 0x000703E0 File Offset: 0x0006E5E0
	private void CondizioniArma1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] <= 0f && base.GetComponent<PresenzaAlleato>().ListaArmi[0][6] > 0f)
		{
			this.timerRicarica1 += Time.deltaTime;
			if (this.timerRicarica1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][2])
			{
				if (base.GetComponent<PresenzaAlleato>().ListaArmi[0][6] < base.GetComponent<PresenzaAlleato>().ListaArmi[0][3])
				{
					base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[0][6];
					this.timerRicarica1 = 0f;
				}
				else
				{
					base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[0][3];
					this.timerRicarica1 = 0f;
				}
			}
		}
		if (this.timerFrequenzaArma1 > 1f && this.timerFrequenzaArma1 < base.GetComponent<PresenzaAlleato>().ListaArmi[0][1])
		{
			base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] = true;
			if (!this.suonoRicaricaAttivo)
			{
				this.bocca1.transform.parent.GetComponent<AudioSource>().Play();
				this.suonoRicaricaAttivo = true;
			}
		}
		else
		{
			base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] = false;
			this.suonoRicaricaAttivo = false;
		}
	}

	// Token: 0x060002AF RID: 687 RVA: 0x0007059C File Offset: 0x0006E79C
	private void RinculoCannone()
	{
		if (this.cannoneSparato)
		{
			if (this.cannone.transform.localPosition.y < -3f && !this.cannoneFinoInFondo)
			{
				this.cannone.transform.localPosition += base.transform.up * 50f * Time.deltaTime;
			}
			if (this.cannone.transform.localPosition.y >= -3f && !this.cannoneFinoInFondo)
			{
				this.cannoneFinoInFondo = true;
			}
			if (this.cannone.transform.localPosition.y > -8.36f && this.cannoneFinoInFondo)
			{
				this.cannone.transform.localPosition += -base.transform.up * 1f * Time.deltaTime;
			}
			if (this.cannone.transform.localPosition.y <= -8.36f && this.cannoneFinoInFondo)
			{
				this.cannoneFinoInFondo = false;
				this.cannoneSparato = false;
			}
		}
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x000706F8 File Offset: 0x0006E8F8
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

	// Token: 0x060002B1 RID: 689 RVA: 0x000708C0 File Offset: 0x0006EAC0
	private void CameraTPS()
	{
		this.timerPosizionamentoTPS += Time.deltaTime;
		if (this.timerPosizionamentoTPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = base.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraTPS;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = this.campoCameraIniziale;
		}
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x0007093C File Offset: 0x0006EB3C
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = base.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
		}
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x000709A4 File Offset: 0x0006EBA4
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
						List<float> expr_B88 = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
						int index;
						int expr_B8C = index = 5;
						float num14 = listaValoriArma[index];
						expr_B88[expr_B8C] = num14 - 1f;
						List<float> listaValoriArma2;
						List<float> expr_BB2 = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
						int expr_BB6 = index = 6;
						num14 = listaValoriArma2[index];
						expr_BB2[expr_BB6] = num14 - 1f;
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

	// Token: 0x060002B4 RID: 692 RVA: 0x00071608 File Offset: 0x0006F808
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

	// Token: 0x060002B5 RID: 693 RVA: 0x000717A0 File Offset: 0x0006F9A0
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
		this.cannoneSparato = true;
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x000718B0 File Offset: 0x0006FAB0
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
		this.cannoneSparato = true;
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x000719C8 File Offset: 0x0006FBC8
	private void AlzoCannonePrimaPersona()
	{
		this.baseCannone.transform.localEulerAngles = new Vector3(this.valoreAngolo, 0f, 0f);
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x000719F0 File Offset: 0x0006FBF0
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
		if (Input.GetMouseButtonDown(0) && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][1])
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
			if (!this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.bocca1.GetComponent<ParticleSystem>().Play();
			}
		}
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00071C48 File Offset: 0x0006FE48
	private void SparoArma1()
	{
		this.proiettileArtiglieria = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.proiettileArtiglieria.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.cannoneSparato = true;
		this.proiettileArtiglieria.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x04000B58 RID: 2904
	public float angVertMax;

	// Token: 0x04000B59 RID: 2905
	public float angVertMin;

	// Token: 0x04000B5A RID: 2906
	private GameObject infoNeutreTattica;

	// Token: 0x04000B5B RID: 2907
	private GameObject terzaCamera;

	// Token: 0x04000B5C RID: 2908
	private GameObject primaCamera;

	// Token: 0x04000B5D RID: 2909
	public GameObject bocca1;

	// Token: 0x04000B5E RID: 2910
	private GameObject IANemico;

	// Token: 0x04000B5F RID: 2911
	private GameObject InfoAlleati;

	// Token: 0x04000B60 RID: 2912
	private GameObject attacchiSpecialiAlleati;

	// Token: 0x04000B61 RID: 2913
	private float timerFrequenzaArma1;

	// Token: 0x04000B62 RID: 2914
	private float timerRicarica1;

	// Token: 0x04000B63 RID: 2915
	private bool ricaricaInCorso1;

	// Token: 0x04000B64 RID: 2916
	private float ritardoFrequenzaArma;

	// Token: 0x04000B65 RID: 2917
	public float intervalloDiRitardoFreqArma;

	// Token: 0x04000B66 RID: 2918
	private int layerColpo;

	// Token: 0x04000B67 RID: 2919
	private int layerVisuale;

	// Token: 0x04000B68 RID: 2920
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000B69 RID: 2921
	public Vector3 rotazioneCameraTPS;

	// Token: 0x04000B6A RID: 2922
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000B6B RID: 2923
	private float timerPosizionamentoTPS;

	// Token: 0x04000B6C RID: 2924
	private float timerPosizionamentoFPS;

	// Token: 0x04000B6D RID: 2925
	private float campoCameraIniziale;

	// Token: 0x04000B6E RID: 2926
	private GameObject CanvasFPS;

	// Token: 0x04000B6F RID: 2927
	private GameObject angolazioneMortairTPS;

	// Token: 0x04000B70 RID: 2928
	private GameObject angolazioneMortairTPSLancetta;

	// Token: 0x04000B71 RID: 2929
	private GameObject angolazioneTesto;

	// Token: 0x04000B72 RID: 2930
	private float valoreAngolo;

	// Token: 0x04000B73 RID: 2931
	private RaycastHit targetSparo;

	// Token: 0x04000B74 RID: 2932
	private GameObject proiettileArtiglieria;

	// Token: 0x04000B75 RID: 2933
	private NavMeshAgent alleatoNav;

	// Token: 0x04000B76 RID: 2934
	private float velocitàAlleatoNav;

	// Token: 0x04000B77 RID: 2935
	public GameObject baseCannone;

	// Token: 0x04000B78 RID: 2936
	private GameObject unitàBersaglio;

	// Token: 0x04000B79 RID: 2937
	private Vector3 centroUnitàBersaglio;

	// Token: 0x04000B7A RID: 2938
	private GameObject munizioneArma1;

	// Token: 0x04000B7B RID: 2939
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x04000B7C RID: 2940
	private float AngMinPP;

	// Token: 0x04000B7D RID: 2941
	private float AngMaxPP;

	// Token: 0x04000B7E RID: 2942
	private bool suonoRicaricaAttivo;

	// Token: 0x04000B7F RID: 2943
	public float ampiezzaCerchioPrecisione;

	// Token: 0x04000B80 RID: 2944
	private AudioSource suonoTorretta;

	// Token: 0x04000B81 RID: 2945
	private AudioSource suonoMotore;

	// Token: 0x04000B82 RID: 2946
	public AudioClip motoreFermo;

	// Token: 0x04000B83 RID: 2947
	public AudioClip motorePartenza;

	// Token: 0x04000B84 RID: 2948
	public AudioClip motoreViaggio;

	// Token: 0x04000B85 RID: 2949
	public AudioClip motoreStop;

	// Token: 0x04000B86 RID: 2950
	private float timerPartenza;

	// Token: 0x04000B87 RID: 2951
	private float timerStop;

	// Token: 0x04000B88 RID: 2952
	private bool primaPartenza;

	// Token: 0x04000B89 RID: 2953
	public float volumeMotoreIniziale;

	// Token: 0x04000B8A RID: 2954
	private bool inPartenza;

	// Token: 0x04000B8B RID: 2955
	private bool partenzaFinita;

	// Token: 0x04000B8C RID: 2956
	private bool inStop;

	// Token: 0x04000B8D RID: 2957
	public bool stopFinito;

	// Token: 0x04000B8E RID: 2958
	private float distFineOrdineMovimento;

	// Token: 0x04000B8F RID: 2959
	private GameObject cannone;

	// Token: 0x04000B90 RID: 2960
	private bool cannoneSparato;

	// Token: 0x04000B91 RID: 2961
	private bool cannoneFinoInFondo;

	// Token: 0x04000B92 RID: 2962
	private float timerAggRicerca;
}
