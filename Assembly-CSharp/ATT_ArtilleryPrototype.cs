using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000039 RID: 57
public class ATT_ArtilleryPrototype : MonoBehaviour
{
	// Token: 0x060002BB RID: 699 RVA: 0x00071CC8 File Offset: 0x0006FEC8
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
		this.layerColpo = 165120;
		this.layerVisuale = 256;
		this.campoCameraIniziale = this.terzaCamera.GetComponent<Camera>().fieldOfView;
		this.ListaMunizioniAttiveUnità = new List<GameObject>();
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma1);
		this.suonoIngranaggi = base.GetComponent<AudioSource>();
		this.valoreAngolo = -35f;
		this.AngMinPP = base.GetComponent<MOV_ArtilleryPrototype>().angCannoniVertMin;
		this.AngMaxPP = base.GetComponent<MOV_ArtilleryPrototype>().angCannoniVertMax;
		this.baseCorpo = base.transform.GetChild(1).transform.GetChild(1).transform.GetChild(1).gameObject;
		this.rotaia = base.transform.GetChild(1).transform.GetChild(1).gameObject;
		this.particellePolvere = base.transform.GetChild(1).transform.GetChild(0).transform.gameObject.GetComponent<ParticleSystem>();
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.ritardoFrequenzaArma = UnityEngine.Random.Range(-this.intervalloDiRitardoFreqArma, this.intervalloDiRitardoFreqArma);
	}

	// Token: 0x060002BC RID: 700 RVA: 0x00071F2C File Offset: 0x0007012C
	private void Update()
	{
		this.ListaMunizioniAttiveUnità[0] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[0][0];
		this.munizioneArma1 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0];
		this.timerFrequenzaArma1 += Time.deltaTime;
		this.CondizioniArma1();
		this.RinculoCannone();
		this.unitàBersaglio = base.GetComponent<PresenzaAlleato>().unitàBersaglio;
		if (!this.giàPosizionato)
		{
			if (base.GetComponent<PresenzaAlleato>().giàSchierato)
			{
				this.giàPosizionato = true;
				this.alleatoNav.enabled = false;
				base.GetComponent<NavMeshObstacle>().enabled = true;
				this.posizioneFinale = base.transform.position;
			}
		}
		else
		{
			base.transform.position = this.posizioneFinale;
		}
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
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
			this.baseCannone.transform.localEulerAngles = new Vector3(-35f, 0f, 0f);
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x060002BD RID: 701 RVA: 0x000721D4 File Offset: 0x000703D4
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

	// Token: 0x060002BE RID: 702 RVA: 0x00072390 File Offset: 0x00070590
	private void RinculoCannone()
	{
		if (this.cannoneSparato)
		{
			if (this.baseCorpo.transform.localPosition.z > -7.07f && !this.cannoneFinoInFondo)
			{
				this.baseCorpo.transform.localPosition += -Vector3.forward * 50f * Time.deltaTime;
			}
			if (this.baseCorpo.transform.localPosition.z <= -7.07f && !this.cannoneFinoInFondo)
			{
				this.cannoneFinoInFondo = true;
			}
			if (this.baseCorpo.transform.localPosition.z < 6.57f && this.cannoneFinoInFondo)
			{
				this.baseCorpo.transform.localPosition += Vector3.forward * 1.5f * Time.deltaTime;
			}
			if (this.baseCorpo.transform.localPosition.z >= 6.57f && this.cannoneFinoInFondo)
			{
				this.cannoneFinoInFondo = false;
				this.cannoneSparato = false;
			}
		}
	}

	// Token: 0x060002BF RID: 703 RVA: 0x000724E0 File Offset: 0x000706E0
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

	// Token: 0x060002C0 RID: 704 RVA: 0x000726A8 File Offset: 0x000708A8
	private void CameraTPS()
	{
		this.timerPosizionamentoTPS += Time.deltaTime;
		if (this.timerPosizionamentoTPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = base.transform.GetChild(1).transform.GetChild(1).transform.GetChild(1).transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraTPS;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = this.campoCameraIniziale;
		}
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x00072744 File Offset: 0x00070944
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.rotaia.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
		}
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x000727B0 File Offset: 0x000709B0
	private void PreparazioneAttacco()
	{
		if (this.unitàBersaglio)
		{
			this.centroUnitàBersaglio = this.unitàBersaglio.GetComponent<PresenzaNemico>().centroInsetto;
		}
		base.GetComponent<PresenzaAlleato>().destinazioneOrdinata = false;
		this.alleatoNav.speed = 0f;
		float portataMinima = this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMinima;
		float portataMassima = this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMassima;
		if (!base.GetComponent<PresenzaAlleato>().destinazioneOrdinata)
		{
			if (base.GetComponent<PresenzaAlleato>().attaccoOrdinato)
			{
				base.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
				this.unitàBersaglio = this.primaCamera.GetComponent<Selezionamento>().oggettoBersaglio;
				if (this.unitàBersaglio && !this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
				{
					float num = Vector3.Dot((this.centroUnitàBersaglio - base.transform.position).normalized, base.transform.up);
					if (num < this.angVertMax && num > this.angVertMin)
					{
						float num2 = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
						if (num2 > portataMinima && num2 < portataMassima)
						{
							this.rotaia.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
							this.AttaccoIndipendente1();
						}
						if (this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita <= 0f)
						{
							base.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
						}
					}
				}
			}
			else if (this.unitàBersaglio && !this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
			{
				float num3 = Vector3.Dot((this.centroUnitàBersaglio - base.transform.position).normalized, base.transform.up);
				float num4 = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
				if (num3 < this.angVertMax && num3 > this.angVertMin)
				{
					if (num4 > portataMinima && num4 < portataMassima)
					{
						this.rotaia.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
						this.AttaccoIndipendente1();
					}
					else if (num4 < portataMinima)
					{
						this.unitàBersaglio = null;
					}
				}
			}
			else if (base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers)
			{
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
								float num5 = Vector3.Distance(base.transform.position, current.GetComponent<PresenzaNemico>().centroInsetto);
								if (num5 < this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMassima && num5 > this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMinima)
								{
									float num6 = Vector3.Dot((current.GetComponent<PresenzaNemico>().centroInsetto - base.transform.position).normalized, base.transform.up);
									if (num6 < this.angVertMax && num6 > this.angVertMin)
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
								float num7 = Vector3.Distance(base.transform.position, current2.GetComponent<PresenzaNemico>().centroInsetto);
								if (num7 < this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMassima && num7 > this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMinima)
								{
									float num8 = Vector3.Dot((current2.GetComponent<PresenzaNemico>().centroInsetto - base.transform.position).normalized, base.transform.up);
									if (num8 < this.angVertMax && num8 > this.angVertMin)
									{
										list2.Add(current2);
									}
								}
							}
						}
						if (list2.Count > 0)
						{
							float num9 = 9999f;
							for (int i = 0; i < list2.Count; i++)
							{
								float num10 = Vector3.Distance(base.transform.position, list2[i].GetComponent<PresenzaNemico>().centroInsetto);
								if (num10 < num9)
								{
									num9 = num10;
									this.unitàBersaglio = list2[i];
								}
							}
						}
					}
				}
			}
			else if (base.GetComponent<PresenzaAlleato>().ricercaAutomaticaBersaglio && this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti.Count > 0)
			{
				this.timerAggRicerca += Time.deltaTime;
				if (this.timerAggRicerca > 1f)
				{
					this.timerAggRicerca = 0f;
					List<GameObject> list3 = new List<GameObject>();
					foreach (GameObject current3 in this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti)
					{
						if (current3 != null)
						{
							float num11 = Vector3.Distance(base.transform.position, current3.GetComponent<PresenzaNemico>().centroInsetto);
							if (num11 < this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMassima && num11 > this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMinima)
							{
								float num12 = Vector3.Dot((current3.GetComponent<PresenzaNemico>().centroInsetto - base.transform.position).normalized, base.transform.up);
								if (num12 < this.angVertMax && num12 > this.angVertMin)
								{
									list3.Add(current3);
								}
							}
						}
					}
					if (list3.Count > 0)
					{
						GestoreNeutroStrategia.valoreRandomSeed++;
						UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
						float f2 = UnityEngine.Random.Range(0f, (float)list3.Count - 0.01f);
						this.unitàBersaglio = list3[Mathf.FloorToInt(f2)];
					}
				}
			}
		}
		if (this.unitàBersaglio && !this.unitàBersaglio.GetComponent<PresenzaNemico>().èStatoVisto)
		{
			this.unitàBersaglio = null;
		}
		if (base.GetComponent<PresenzaAlleato>().attaccoZonaOrdinato && this.alleatoNav.enabled)
		{
			Vector3 luogoAttZonaArt = base.GetComponent<PresenzaAlleato>().luogoAttZonaArt;
			float num13 = Vector3.Distance(luogoAttZonaArt, base.transform.position);
			float num14 = Vector3.Dot((base.GetComponent<PresenzaAlleato>().luogoAttZonaArt - base.transform.position).normalized, base.transform.up);
			if (num14 < this.angVertMax && num14 > this.angVertMin && num13 > portataMinima && num13 < portataMassima)
			{
				this.rotaia.transform.LookAt(new Vector3(luogoAttZonaArt.x, base.transform.position.y, luogoAttZonaArt.z));
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
					List<float> expr_9FF = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
					int index;
					int expr_A03 = index = 5;
					float num15 = listaValoriArma[index];
					expr_9FF[expr_A03] = num15 - 1f;
					List<float> listaValoriArma2;
					List<float> expr_A29 = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
					int expr_A2D = index = 6;
					num15 = listaValoriArma2[index];
					expr_A29[expr_A2D] = num15 - 1f;
				}
			}
		}
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x00073254 File Offset: 0x00071454
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

	// Token: 0x060002C4 RID: 708 RVA: 0x000733EC File Offset: 0x000715EC
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
		this.particellePolvere.Play();
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x00073508 File Offset: 0x00071708
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
		this.particellePolvere.Play();
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x0007362C File Offset: 0x0007182C
	private void AlzoCannonePrimaPersona()
	{
		this.baseCannone.transform.localEulerAngles = new Vector3(this.valoreAngolo, 0f, 0f);
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x00073654 File Offset: 0x00071854
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

	// Token: 0x060002C8 RID: 712 RVA: 0x000738AC File Offset: 0x00071AAC
	private void SparoArma1()
	{
		this.proiettileArtiglieria = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.proiettileArtiglieria.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.cannoneSparato = true;
		this.particellePolvere.Play();
		this.proiettileArtiglieria.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x04000B93 RID: 2963
	public float angVertMax;

	// Token: 0x04000B94 RID: 2964
	public float angVertMin;

	// Token: 0x04000B95 RID: 2965
	private GameObject infoNeutreTattica;

	// Token: 0x04000B96 RID: 2966
	private GameObject terzaCamera;

	// Token: 0x04000B97 RID: 2967
	private GameObject primaCamera;

	// Token: 0x04000B98 RID: 2968
	public GameObject bocca1;

	// Token: 0x04000B99 RID: 2969
	private GameObject IANemico;

	// Token: 0x04000B9A RID: 2970
	private GameObject InfoAlleati;

	// Token: 0x04000B9B RID: 2971
	private GameObject attacchiSpecialiAlleati;

	// Token: 0x04000B9C RID: 2972
	private float timerFrequenzaArma1;

	// Token: 0x04000B9D RID: 2973
	private float timerRicarica1;

	// Token: 0x04000B9E RID: 2974
	private bool ricaricaInCorso1;

	// Token: 0x04000B9F RID: 2975
	private float ritardoFrequenzaArma;

	// Token: 0x04000BA0 RID: 2976
	public float intervalloDiRitardoFreqArma;

	// Token: 0x04000BA1 RID: 2977
	private int layerColpo;

	// Token: 0x04000BA2 RID: 2978
	private int layerVisuale;

	// Token: 0x04000BA3 RID: 2979
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000BA4 RID: 2980
	public Vector3 rotazioneCameraTPS;

	// Token: 0x04000BA5 RID: 2981
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000BA6 RID: 2982
	private float timerPosizionamentoTPS;

	// Token: 0x04000BA7 RID: 2983
	private float timerPosizionamentoFPS;

	// Token: 0x04000BA8 RID: 2984
	private float campoCameraIniziale;

	// Token: 0x04000BA9 RID: 2985
	private GameObject CanvasFPS;

	// Token: 0x04000BAA RID: 2986
	private GameObject angolazioneMortairTPS;

	// Token: 0x04000BAB RID: 2987
	private GameObject angolazioneMortairTPSLancetta;

	// Token: 0x04000BAC RID: 2988
	private GameObject angolazioneTesto;

	// Token: 0x04000BAD RID: 2989
	private float valoreAngolo;

	// Token: 0x04000BAE RID: 2990
	private RaycastHit targetSparo;

	// Token: 0x04000BAF RID: 2991
	private GameObject proiettileArtiglieria;

	// Token: 0x04000BB0 RID: 2992
	private NavMeshAgent alleatoNav;

	// Token: 0x04000BB1 RID: 2993
	public GameObject baseCannone;

	// Token: 0x04000BB2 RID: 2994
	private GameObject unitàBersaglio;

	// Token: 0x04000BB3 RID: 2995
	private Vector3 centroUnitàBersaglio;

	// Token: 0x04000BB4 RID: 2996
	private GameObject munizioneArma1;

	// Token: 0x04000BB5 RID: 2997
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x04000BB6 RID: 2998
	private float AngMinPP;

	// Token: 0x04000BB7 RID: 2999
	private float AngMaxPP;

	// Token: 0x04000BB8 RID: 3000
	public GameObject provaDistanza;

	// Token: 0x04000BB9 RID: 3001
	private bool suonoRicaricaAttivo;

	// Token: 0x04000BBA RID: 3002
	public float ampiezzaCerchioPrecisione;

	// Token: 0x04000BBB RID: 3003
	private AudioSource suonoIngranaggi;

	// Token: 0x04000BBC RID: 3004
	private bool cannoneSparato;

	// Token: 0x04000BBD RID: 3005
	private bool cannoneFinoInFondo;

	// Token: 0x04000BBE RID: 3006
	private GameObject baseCorpo;

	// Token: 0x04000BBF RID: 3007
	private GameObject rotaia;

	// Token: 0x04000BC0 RID: 3008
	private ParticleSystem particellePolvere;

	// Token: 0x04000BC1 RID: 3009
	private float distFineOrdineMovimento;

	// Token: 0x04000BC2 RID: 3010
	private float timerAggRicerca;

	// Token: 0x04000BC3 RID: 3011
	private Vector3 posizioneFinale;

	// Token: 0x04000BC4 RID: 3012
	private bool giàPosizionato;
}
