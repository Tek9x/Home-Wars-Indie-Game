using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200002C RID: 44
public class ATT_LightAntiTankInfantry : MonoBehaviour
{
	// Token: 0x06000206 RID: 518 RVA: 0x00058514 File Offset: 0x00056714
	private void Start()
	{
		this.CanvasFPS = GameObject.FindGameObjectWithTag("CanvasFPS");
		this.mirinoFisso = this.CanvasFPS.transform.GetChild(2).transform.GetChild(0).gameObject;
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.InfoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.suonoArma = base.GetComponent<AudioSource>();
		this.suonoRicarica = base.transform.GetChild(1).GetComponent<AudioSource>();
		this.alleatoNav = base.GetComponent<NavMeshAgent>();
		this.velocitàAlleatoNav = base.GetComponent<NavMeshAgent>().speed;
		this.layerColpo = 165120;
		this.layerVisuale = 256;
		this.campoCameraIniziale = this.terzaCamera.GetComponent<Camera>().fieldOfView;
		this.timerRinculo = 0.3f;
		this.tempoFraSparoERicarica = 1f;
		this.particelleCulo1 = this.culo1.GetComponent<ParticleSystem>();
		this.particelleCulo1Fumo = this.culo1.transform.GetChild(0).GetComponent<ParticleSystem>();
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.velocitàIniziale = this.alleatoNav.speed;
	}

	// Token: 0x06000207 RID: 519 RVA: 0x00058698 File Offset: 0x00056898
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
					this.mirinoFisso.GetComponent<Image>().sprite = this.mirinoImmagineFPS;
					base.gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
					this.bocca1.transform.parent.GetComponent<MeshRenderer>().enabled = false;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					this.mirinoFisso.GetComponent<Image>().sprite = this.mirinoImmagineTPS;
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

	// Token: 0x06000208 RID: 520 RVA: 0x000589AC File Offset: 0x00056BAC
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

	// Token: 0x06000209 RID: 521 RVA: 0x00058A30 File Offset: 0x00056C30
	private void CondizioniArma()
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
				this.suonoRicarica.Play();
				this.suonoRicaricaAttivo = true;
			}
		}
		else
		{
			base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] = false;
			this.suonoRicaricaAttivo = false;
		}
	}

	// Token: 0x0600020A RID: 522 RVA: 0x00058BDC File Offset: 0x00056DDC
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
			}
			this.CameraFPS();
			this.timerPosizionamentoTPS = 0f;
		}
	}

	// Token: 0x0600020B RID: 523 RVA: 0x00058CD8 File Offset: 0x00056ED8
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

	// Token: 0x0600020C RID: 524 RVA: 0x00058D78 File Offset: 0x00056F78
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.ossoArma1.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 24f;
		}
	}

	// Token: 0x0600020D RID: 525 RVA: 0x00058DF8 File Offset: 0x00056FF8
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
						if (num3 > this.munizione.GetComponent<DatiGeneraliMunizione>().portataMinima && num3 < this.munizione.GetComponent<DatiGeneraliMunizione>().portataMassima)
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
					if (num5 > this.munizione.GetComponent<DatiGeneraliMunizione>().portataMinima && num5 < this.munizione.GetComponent<DatiGeneraliMunizione>().portataMassima)
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

	// Token: 0x0600020E RID: 526 RVA: 0x0005988C File Offset: 0x00057A8C
	private void AttaccoIndipendente()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[0])
		{
			if (this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaAllPiùViciniPerTipo.Contains(base.gameObject) || this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] > 0.05f)
			{
				this.suonoArma.Play();
				this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] = 0f;
			}
			this.timerFrequenzaArma1 = 0f;
			this.particelleCulo1.Play();
			this.particelleCulo1Fumo.Play();
			List<float> listaValoriArma;
			List<float> expr_14F = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int index;
			int expr_152 = index = 5;
			float num = listaValoriArma[index];
			expr_14F[expr_152] = num - 1f;
			List<float> listaValoriArma2;
			List<float> expr_173 = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int expr_176 = index = 6;
			num = listaValoriArma2[index];
			expr_173[expr_176] = num - 1f;
			this.SparoIndipendente1();
			this.timerDopoSparo = 0f;
		}
	}

	// Token: 0x0600020F RID: 527 RVA: 0x00059A38 File Offset: 0x00057C38
	private void SparoIndipendente1()
	{
		this.razzo = (UnityEngine.Object.Instantiate(this.razzoPrefab, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.razzo.GetComponent<DatiOrdignoInterno>().bersaglio = this.unitàBersaglio;
		this.razzo.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x06000210 RID: 528 RVA: 0x00059AAC File Offset: 0x00057CAC
	private void AttaccoPrimaPersona()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo))
		{
			if (this.targetSparo.collider.gameObject.tag == "Nemico" || this.targetSparo.collider.gameObject.tag == "Nemico Testa" || this.targetSparo.collider.gameObject.tag == "Nemico Coll Suppl")
			{
				if (Vector3.Distance(base.transform.position, this.targetSparo.point) > this.munizione.GetComponent<DatiGeneraliMunizione>().portataMinima && Vector3.Distance(base.transform.position, this.targetSparo.point) <= this.munizione.GetComponent<DatiGeneraliMunizione>().portataMassima)
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
		if (Input.GetMouseButtonDown(0) && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] && base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[1])
		{
			this.timerFrequenzaArma1 = 0f;
			this.SparoArma();
			this.suonoArma.Play();
			List<float> listaValoriArma;
			List<float> expr_1E1 = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int index;
			int expr_1E4 = index = 5;
			float num = listaValoriArma[index];
			expr_1E1[expr_1E4] = num - 1f;
			List<float> listaValoriArma2;
			List<float> expr_205 = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int expr_209 = index = 6;
			num = listaValoriArma2[index];
			expr_205[expr_209] = num - 1f;
			this.avviaRinculo = true;
			this.timerRinculo = 0.3f;
			this.timerDopoSparo = 0f;
			if (!this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.particelleCulo1.Play();
				this.particelleCulo1Fumo.Play();
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

	// Token: 0x06000211 RID: 529 RVA: 0x00059D60 File Offset: 0x00057F60
	private void SparoArma()
	{
		if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
		{
			this.razzo = (UnityEngine.Object.Instantiate(this.razzoPrefab, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
			this.razzo.GetComponent<DatiOrdignoInterno>().lanciatoInFPS = true;
			this.razzo.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
		}
		else
		{
			this.razzo = (UnityEngine.Object.Instantiate(this.razzoPrefab, this.strutturaFPS1.transform.GetChild(1).transform.position, this.strutturaFPS1.transform.GetChild(1).transform.rotation) as GameObject);
			this.razzo.GetComponent<DatiOrdignoInterno>().lanciatoInFPS = true;
			this.razzo.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
		}
	}

	// Token: 0x040008E7 RID: 2279
	public float angVertMax;

	// Token: 0x040008E8 RID: 2280
	public float angVertMin;

	// Token: 0x040008E9 RID: 2281
	public GameObject bocca1;

	// Token: 0x040008EA RID: 2282
	public GameObject culo1;

	// Token: 0x040008EB RID: 2283
	public GameObject ossoArma1;

	// Token: 0x040008EC RID: 2284
	private float timerFrequenzaArma1;

	// Token: 0x040008ED RID: 2285
	private float timerRicarica1;

	// Token: 0x040008EE RID: 2286
	private bool ricaricaInCorso1;

	// Token: 0x040008EF RID: 2287
	private float timerDopoSparo;

	// Token: 0x040008F0 RID: 2288
	private float tempoFraSparoERicarica;

	// Token: 0x040008F1 RID: 2289
	public GameObject razzoPrefab;

	// Token: 0x040008F2 RID: 2290
	private GameObject razzo;

	// Token: 0x040008F3 RID: 2291
	private GameObject CanvasFPS;

	// Token: 0x040008F4 RID: 2292
	private GameObject mirinoFisso;

	// Token: 0x040008F5 RID: 2293
	public Sprite mirinoImmagineFPS;

	// Token: 0x040008F6 RID: 2294
	public Sprite mirinoImmagineTPS;

	// Token: 0x040008F7 RID: 2295
	public GameObject strutturaPrefabFPS1;

	// Token: 0x040008F8 RID: 2296
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x040008F9 RID: 2297
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x040008FA RID: 2298
	public Vector3 aggiustamentoTraslVisualeFPS;

	// Token: 0x040008FB RID: 2299
	public Vector3 aggiustamentoRotVisualeFPS;

	// Token: 0x040008FC RID: 2300
	private GameObject strutturaFPS1;

	// Token: 0x040008FD RID: 2301
	private float timerPosizionamentoTPS;

	// Token: 0x040008FE RID: 2302
	private float timerPosizionamentoFPS;

	// Token: 0x040008FF RID: 2303
	private float campoCameraIniziale;

	// Token: 0x04000900 RID: 2304
	private GameObject infoNeutreTattica;

	// Token: 0x04000901 RID: 2305
	private GameObject terzaCamera;

	// Token: 0x04000902 RID: 2306
	private GameObject primaCamera;

	// Token: 0x04000903 RID: 2307
	private AudioSource suonoArma;

	// Token: 0x04000904 RID: 2308
	private AudioSource suonoRicarica;

	// Token: 0x04000905 RID: 2309
	private NavMeshAgent alleatoNav;

	// Token: 0x04000906 RID: 2310
	private float velocitàAlleatoNav;

	// Token: 0x04000907 RID: 2311
	private GameObject IANemico;

	// Token: 0x04000908 RID: 2312
	private GameObject InfoAlleati;

	// Token: 0x04000909 RID: 2313
	private RaycastHit targetSparo;

	// Token: 0x0400090A RID: 2314
	private int layerColpo;

	// Token: 0x0400090B RID: 2315
	private int layerVisuale;

	// Token: 0x0400090C RID: 2316
	public GameObject unitàBersaglio;

	// Token: 0x0400090D RID: 2317
	private Vector3 centroUnitàBersaglio;

	// Token: 0x0400090E RID: 2318
	public bool avviaRinculo;

	// Token: 0x0400090F RID: 2319
	public float timerRinculo;

	// Token: 0x04000910 RID: 2320
	private GameObject munizione;

	// Token: 0x04000911 RID: 2321
	private bool primoFrameAvvenuto;

	// Token: 0x04000912 RID: 2322
	private ParticleSystem particelleCulo1;

	// Token: 0x04000913 RID: 2323
	private ParticleSystem particelleCulo1Fumo;

	// Token: 0x04000914 RID: 2324
	private float distFineOrdineMovimento;

	// Token: 0x04000915 RID: 2325
	private bool calcoloDistJump;

	// Token: 0x04000916 RID: 2326
	private bool calcoloJumpEffettuato;

	// Token: 0x04000917 RID: 2327
	private float velocitàIniziale;

	// Token: 0x04000918 RID: 2328
	private float timerAggRicerca;

	// Token: 0x04000919 RID: 2329
	private bool suonoRicaricaAttivo;
}
