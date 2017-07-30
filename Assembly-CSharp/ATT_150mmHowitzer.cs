using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000036 RID: 54
public class ATT_150mmHowitzer : MonoBehaviour
{
	// Token: 0x0600028A RID: 650 RVA: 0x0006BF44 File Offset: 0x0006A144
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
		this.suonoMovimento = base.GetComponent<AudioSource>();
		this.valoreAngolo = -25f;
		this.cannone = this.baseCannone.transform.GetChild(0).transform.GetChild(0).gameObject;
		this.AngMinPP = base.GetComponent<MOV_150mmHowitzer>().angVertMin;
		this.AngMaxPP = base.GetComponent<MOV_150mmHowitzer>().angVertMax;
		this.ruote1 = base.transform.GetChild(1).transform.GetChild(1).gameObject;
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.ritardoFrequenzaArma = UnityEngine.Random.Range(-this.intervalloDiRitardoFreqArma, this.intervalloDiRitardoFreqArma);
	}

	// Token: 0x0600028B RID: 651 RVA: 0x0006C184 File Offset: 0x0006A384
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
			this.GestioneRuote();
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
			this.baseCannone.transform.localEulerAngles = new Vector3(-28f, 0f, 0f);
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x0600028C RID: 652 RVA: 0x0006C3D4 File Offset: 0x0006A5D4
	private void GestioneSuoniIndipendenti()
	{
		if (this.alleatoNav.velocity.magnitude > 0f && !this.inPartenza)
		{
			this.suonoMovimento.Play();
			this.inPartenza = true;
		}
		if (this.alleatoNav.velocity.magnitude == 0f)
		{
			this.suonoMovimento.Stop();
			this.inPartenza = false;
		}
	}

	// Token: 0x0600028D RID: 653 RVA: 0x0006C44C File Offset: 0x0006A64C
	private void GestioneRuote()
	{
		if (this.inPartenza)
		{
			this.ruote1.transform.Rotate(Vector3.right * 2.5f);
		}
	}

	// Token: 0x0600028E RID: 654 RVA: 0x0006C484 File Offset: 0x0006A684
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

	// Token: 0x0600028F RID: 655 RVA: 0x0006C640 File Offset: 0x0006A840
	private void RinculoCannone()
	{
		if (this.cannoneSparato)
		{
			if (this.cannone.transform.localPosition.y < -0.1f && !this.cannoneFinoInFondo)
			{
				this.cannone.transform.localPosition += base.transform.up * 15f * Time.deltaTime;
			}
			if (this.cannone.transform.localPosition.y >= -0.1f && !this.cannoneFinoInFondo)
			{
				this.cannoneFinoInFondo = true;
			}
			if (this.cannone.transform.localPosition.y > -1.44f && this.cannoneFinoInFondo)
			{
				this.cannone.transform.localPosition += -base.transform.up * 0.5f * Time.deltaTime;
			}
			if (this.cannone.transform.localPosition.y <= -1.44f && this.cannoneFinoInFondo)
			{
				this.cannoneFinoInFondo = false;
				this.cannoneSparato = false;
			}
		}
	}

	// Token: 0x06000290 RID: 656 RVA: 0x0006C79C File Offset: 0x0006A99C
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
			this.valoreAngolo += Mathf.Abs(Input.GetAxis("Mouse ScrollWheel") * 5f);
			if (Input.GetKey(KeyCode.PageDown))
			{
				this.valoreAngolo -= 0.05f;
			}
		}
		else if (this.valoreAngolo > this.AngMinPP)
		{
			this.valoreAngolo -= Mathf.Abs(Input.GetAxis("Mouse ScrollWheel") * 5f);
			if (Input.GetKey(KeyCode.PageUp))
			{
				this.valoreAngolo += 0.05f;
			}
		}
		else
		{
			this.valoreAngolo += Input.GetAxis("Mouse ScrollWheel") * 5f;
			if (Input.GetKey(KeyCode.PageUp))
			{
				this.valoreAngolo -= 0.05f;
			}
			else if (Input.GetKey(KeyCode.PageDown))
			{
				this.valoreAngolo += 0.05f;
			}
		}
		this.angolazioneMortairTPSLancetta.transform.eulerAngles = new Vector3(0f, 0f, this.valoreAngolo);
		this.angolazioneTesto.GetComponent<Text>().text = Mathf.Abs(this.valoreAngolo).ToString("F1") + "°";
	}

	// Token: 0x06000291 RID: 657 RVA: 0x0006C964 File Offset: 0x0006AB64
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

	// Token: 0x06000292 RID: 658 RVA: 0x0006C9E0 File Offset: 0x0006ABE0
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = base.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
		}
	}

	// Token: 0x06000293 RID: 659 RVA: 0x0006CA48 File Offset: 0x0006AC48
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

	// Token: 0x06000294 RID: 660 RVA: 0x0006D6AC File Offset: 0x0006B8AC
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

	// Token: 0x06000295 RID: 661 RVA: 0x0006D844 File Offset: 0x0006BA44
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
	}

	// Token: 0x06000296 RID: 662 RVA: 0x0006D954 File Offset: 0x0006BB54
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
	}

	// Token: 0x06000297 RID: 663 RVA: 0x0006DA6C File Offset: 0x0006BC6C
	private void AlzoCannonePrimaPersona()
	{
		this.baseCannone.transform.localEulerAngles = new Vector3(this.valoreAngolo, 0f, 0f);
	}

	// Token: 0x06000298 RID: 664 RVA: 0x0006DA94 File Offset: 0x0006BC94
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

	// Token: 0x06000299 RID: 665 RVA: 0x0006DCEC File Offset: 0x0006BEEC
	private void SparoArma1()
	{
		this.proiettileArtiglieria = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.proiettileArtiglieria.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.cannoneSparato = true;
		this.proiettileArtiglieria.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x04000AE8 RID: 2792
	public float angVertMax;

	// Token: 0x04000AE9 RID: 2793
	public float angVertMin;

	// Token: 0x04000AEA RID: 2794
	private GameObject infoNeutreTattica;

	// Token: 0x04000AEB RID: 2795
	private GameObject terzaCamera;

	// Token: 0x04000AEC RID: 2796
	private GameObject primaCamera;

	// Token: 0x04000AED RID: 2797
	public GameObject bocca1;

	// Token: 0x04000AEE RID: 2798
	private GameObject IANemico;

	// Token: 0x04000AEF RID: 2799
	private GameObject InfoAlleati;

	// Token: 0x04000AF0 RID: 2800
	private GameObject attacchiSpecialiAlleati;

	// Token: 0x04000AF1 RID: 2801
	private float timerFrequenzaArma1;

	// Token: 0x04000AF2 RID: 2802
	private float timerRicarica1;

	// Token: 0x04000AF3 RID: 2803
	private bool ricaricaInCorso1;

	// Token: 0x04000AF4 RID: 2804
	private float ritardoFrequenzaArma;

	// Token: 0x04000AF5 RID: 2805
	public float intervalloDiRitardoFreqArma;

	// Token: 0x04000AF6 RID: 2806
	private int layerColpo;

	// Token: 0x04000AF7 RID: 2807
	private int layerVisuale;

	// Token: 0x04000AF8 RID: 2808
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000AF9 RID: 2809
	public Vector3 rotazioneCameraTPS;

	// Token: 0x04000AFA RID: 2810
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000AFB RID: 2811
	private float timerPosizionamentoTPS;

	// Token: 0x04000AFC RID: 2812
	private float timerPosizionamentoFPS;

	// Token: 0x04000AFD RID: 2813
	private float campoCameraIniziale;

	// Token: 0x04000AFE RID: 2814
	private GameObject CanvasFPS;

	// Token: 0x04000AFF RID: 2815
	private GameObject angolazioneMortairTPS;

	// Token: 0x04000B00 RID: 2816
	private GameObject angolazioneMortairTPSLancetta;

	// Token: 0x04000B01 RID: 2817
	private GameObject angolazioneTesto;

	// Token: 0x04000B02 RID: 2818
	private float valoreAngolo;

	// Token: 0x04000B03 RID: 2819
	private RaycastHit targetSparo;

	// Token: 0x04000B04 RID: 2820
	private GameObject proiettileArtiglieria;

	// Token: 0x04000B05 RID: 2821
	private NavMeshAgent alleatoNav;

	// Token: 0x04000B06 RID: 2822
	private float velocitàAlleatoNav;

	// Token: 0x04000B07 RID: 2823
	public GameObject baseCannone;

	// Token: 0x04000B08 RID: 2824
	private GameObject cannone;

	// Token: 0x04000B09 RID: 2825
	private GameObject unitàBersaglio;

	// Token: 0x04000B0A RID: 2826
	private Vector3 centroUnitàBersaglio;

	// Token: 0x04000B0B RID: 2827
	private GameObject munizioneArma1;

	// Token: 0x04000B0C RID: 2828
	private AudioSource suonoMovimento;

	// Token: 0x04000B0D RID: 2829
	private bool inPartenza;

	// Token: 0x04000B0E RID: 2830
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x04000B0F RID: 2831
	private float AngMinPP;

	// Token: 0x04000B10 RID: 2832
	private float AngMaxPP;

	// Token: 0x04000B11 RID: 2833
	public GameObject provaDistanza;

	// Token: 0x04000B12 RID: 2834
	private bool suonoRicaricaAttivo;

	// Token: 0x04000B13 RID: 2835
	private GameObject ruote1;

	// Token: 0x04000B14 RID: 2836
	private bool cannoneSparato;

	// Token: 0x04000B15 RID: 2837
	private bool cannoneFinoInFondo;

	// Token: 0x04000B16 RID: 2838
	public float ampiezzaCerchioPrecisione;

	// Token: 0x04000B17 RID: 2839
	private float distFineOrdineMovimento;

	// Token: 0x04000B18 RID: 2840
	private float timerAggRicerca;
}
