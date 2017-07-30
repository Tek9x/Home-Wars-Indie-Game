using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000029 RID: 41
public class ATT_GrenadeLauncher : MonoBehaviour
{
	// Token: 0x060001E2 RID: 482 RVA: 0x00052FF8 File Offset: 0x000511F8
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
		this.tempoFraSparoERicarica = 1f;
		this.suonoArma = base.GetComponent<AudioSource>();
		this.suonoRicarica = base.transform.GetChild(1).GetComponent<AudioSource>();
		this.particelleBocca1 = this.bocca1.GetComponent<ParticleSystem>();
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.velocitàIniziale = this.alleatoNav.speed;
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x00053154 File Offset: 0x00051354
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
					this.mirinoFisso.GetComponent<CanvasGroup>().alpha = 0f;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					base.gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
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
			this.terzaCamera.GetComponent<Camera>().fieldOfView = this.campoCameraIniziale;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x00053414 File Offset: 0x00051614
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

	// Token: 0x060001E5 RID: 485 RVA: 0x00053498 File Offset: 0x00051698
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

	// Token: 0x060001E6 RID: 486 RVA: 0x000536F8 File Offset: 0x000518F8
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

	// Token: 0x060001E7 RID: 487 RVA: 0x00053810 File Offset: 0x00051A10
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

	// Token: 0x060001E8 RID: 488 RVA: 0x000538B0 File Offset: 0x00051AB0
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

	// Token: 0x060001E9 RID: 489 RVA: 0x00053930 File Offset: 0x00051B30
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
				else
				{
					if (num5 > 3f)
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

	// Token: 0x060001EA RID: 490 RVA: 0x00054394 File Offset: 0x00052594
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

	// Token: 0x060001EB RID: 491 RVA: 0x000545AC File Offset: 0x000527AC
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
		if (Input.GetMouseButton(0) && base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[1])
		{
			this.timerFrequenzaArma1 = 0f;
			this.Sparo();
			this.suonoArma.Play();
			List<float> listaValoriArma;
			List<float> expr_191 = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int index;
			int expr_194 = index = 5;
			float num = listaValoriArma[index];
			expr_191[expr_194] = num - 1f;
			List<float> listaValoriArma2;
			List<float> expr_1B5 = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int expr_1B9 = index = 6;
			num = listaValoriArma2[index];
			expr_1B5[expr_1B9] = num - 1f;
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
	}

	// Token: 0x060001EC RID: 492 RVA: 0x000547D4 File Offset: 0x000529D4
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

	// Token: 0x04000858 RID: 2136
	public float angVertMax;

	// Token: 0x04000859 RID: 2137
	public float angVertMin;

	// Token: 0x0400085A RID: 2138
	public GameObject bocca1;

	// Token: 0x0400085B RID: 2139
	public GameObject ossoArma1;

	// Token: 0x0400085C RID: 2140
	private float timerFrequenzaArma1;

	// Token: 0x0400085D RID: 2141
	private float timerRicarica1;

	// Token: 0x0400085E RID: 2142
	private float timerRicaricaParziale1;

	// Token: 0x0400085F RID: 2143
	private float timerDopoSparo;

	// Token: 0x04000860 RID: 2144
	private float tempoFraSparoERicarica;

	// Token: 0x04000861 RID: 2145
	private GameObject CanvasFPS;

	// Token: 0x04000862 RID: 2146
	private GameObject mirinoFisso;

	// Token: 0x04000863 RID: 2147
	public GameObject strutturaPrefabFPS1;

	// Token: 0x04000864 RID: 2148
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000865 RID: 2149
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000866 RID: 2150
	public Vector3 aggiustamentoTraslVisualeFPS;

	// Token: 0x04000867 RID: 2151
	public Vector3 aggiustamentoRotVisualeFPS;

	// Token: 0x04000868 RID: 2152
	private GameObject strutturaFPS1;

	// Token: 0x04000869 RID: 2153
	private float timerPosizionamentoTPS;

	// Token: 0x0400086A RID: 2154
	private float timerPosizionamentoFPS;

	// Token: 0x0400086B RID: 2155
	private float campoCameraIniziale;

	// Token: 0x0400086C RID: 2156
	private GameObject infoNeutreTattica;

	// Token: 0x0400086D RID: 2157
	private GameObject terzaCamera;

	// Token: 0x0400086E RID: 2158
	private GameObject primaCamera;

	// Token: 0x0400086F RID: 2159
	private NavMeshAgent alleatoNav;

	// Token: 0x04000870 RID: 2160
	private float velocitàAlleatoNav;

	// Token: 0x04000871 RID: 2161
	private GameObject IANemico;

	// Token: 0x04000872 RID: 2162
	private GameObject InfoAlleati;

	// Token: 0x04000873 RID: 2163
	private RaycastHit targetSparo;

	// Token: 0x04000874 RID: 2164
	private int layerColpo;

	// Token: 0x04000875 RID: 2165
	private int layerVisuale;

	// Token: 0x04000876 RID: 2166
	private GameObject unitàBersaglio;

	// Token: 0x04000877 RID: 2167
	private Vector3 centroUnitàBersaglio;

	// Token: 0x04000878 RID: 2168
	private GameObject munizione;

	// Token: 0x04000879 RID: 2169
	private bool primoFrameAvvenuto;

	// Token: 0x0400087A RID: 2170
	private AudioSource suonoArma;

	// Token: 0x0400087B RID: 2171
	private AudioSource suonoRicarica;

	// Token: 0x0400087C RID: 2172
	private ParticleSystem particelleBocca1;

	// Token: 0x0400087D RID: 2173
	private GameObject granata;

	// Token: 0x0400087E RID: 2174
	private GameObject munizioneArma1;

	// Token: 0x0400087F RID: 2175
	private float distFineOrdineMovimento;

	// Token: 0x04000880 RID: 2176
	private bool calcoloDistJump;

	// Token: 0x04000881 RID: 2177
	private bool calcoloJumpEffettuato;

	// Token: 0x04000882 RID: 2178
	private float velocitàIniziale;

	// Token: 0x04000883 RID: 2179
	private float timerAggRicerca;
}
