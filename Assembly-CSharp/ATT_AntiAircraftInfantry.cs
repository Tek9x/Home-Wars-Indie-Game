using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000026 RID: 38
public class ATT_AntiAircraftInfantry : MonoBehaviour
{
	// Token: 0x060001BE RID: 446 RVA: 0x0004CBCC File Offset: 0x0004ADCC
	private void Start()
	{
		this.CanvasFPS = GameObject.FindGameObjectWithTag("CanvasFPS");
		this.sensoreRaggioMirino = this.CanvasFPS.transform.GetChild(2).transform.GetChild(2).gameObject;
		this.sensoreRaggioMirinoMobile = this.CanvasFPS.transform.GetChild(2).transform.GetChild(4).gameObject;
		this.mirinoMissiliFisso = this.CanvasFPS.transform.GetChild(2).transform.GetChild(1).gameObject;
		this.mirinoMissiliMobile = this.CanvasFPS.transform.GetChild(2).transform.GetChild(3).gameObject;
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.InfoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.suonoArma = base.GetComponent<AudioSource>();
		this.suonoAggancio = base.transform.GetChild(0).GetComponent<AudioSource>();
		this.suonoRicarica = base.transform.GetChild(1).GetComponent<AudioSource>();
		this.alleatoNav = base.GetComponent<NavMeshAgent>();
		this.velocitàAlleatoNav = base.GetComponent<NavMeshAgent>().speed;
		this.layerColpo = 165120;
		this.layerVisuale = 256;
		this.campoCameraIniziale = this.terzaCamera.GetComponent<Camera>().fieldOfView;
		this.timerRinculo = 0.4f;
		this.tempoFraSparoERicarica = 1f;
		this.particelleCulo1 = this.culo1.GetComponent<ParticleSystem>();
		this.particelleCulo1Fumo = this.culo1.transform.GetChild(0).GetComponent<ParticleSystem>();
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.velocitàIniziale = this.alleatoNav.speed;
	}

	// Token: 0x060001BF RID: 447 RVA: 0x0004CDDC File Offset: 0x0004AFDC
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
			base.GetComponent<NavMeshAgent>().enabled = false;
			if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.AttaccoPrimaPersona();
			}
			if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva == 3)
			{
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
					base.gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
					this.bocca1.transform.parent.GetComponent<MeshRenderer>().enabled = false;
					this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 1f;
					this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 1f;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					base.gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
					this.bocca1.transform.parent.GetComponent<MeshRenderer>().enabled = true;
					this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 0f;
					this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 0f;
					this.audioBeepLungoAttivo = false;
					this.audioBeepCortoAttivo = false;
					this.suonoAggancio.Stop();
					this.timerDiAggancio = 0f;
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
			this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 0f;
			this.ossoArma1.transform.eulerAngles = base.transform.eulerAngles;
			this.audioBeepLungoAttivo = false;
			this.audioBeepCortoAttivo = false;
			this.suonoAggancio.Stop();
			this.timerDiAggancio = 0f;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x0004D178 File Offset: 0x0004B378
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

	// Token: 0x060001C1 RID: 449 RVA: 0x0004D1FC File Offset: 0x0004B3FC
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

	// Token: 0x060001C2 RID: 450 RVA: 0x0004D3A8 File Offset: 0x0004B5A8
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

	// Token: 0x060001C3 RID: 451 RVA: 0x0004D4A4 File Offset: 0x0004B6A4
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

	// Token: 0x060001C4 RID: 452 RVA: 0x0004D544 File Offset: 0x0004B744
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.ossoArma1.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 32f;
		}
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0004D5C4 File Offset: 0x0004B7C4
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
				if (this.primaCamera.GetComponent<Selezionamento>().oggettoBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
				{
					this.unitàBersaglio = this.primaCamera.GetComponent<Selezionamento>().oggettoBersaglio;
				}
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
								this.alleatoNav.SetDestination(new Vector3(this.unitàBersaglio.transform.position.x, base.transform.position.y, this.unitàBersaglio.transform.position.z));
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
							this.alleatoNav.SetDestination(new Vector3(this.unitàBersaglio.transform.position.x, base.transform.position.y, this.unitàBersaglio.transform.position.z));
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
				if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciVolanti.Count > 0)
				{
					this.timerAggRicerca += Time.deltaTime;
					if (this.timerAggRicerca > 1f)
					{
						this.timerAggRicerca = 0f;
						List<GameObject> list = new List<GameObject>();
						foreach (GameObject current in this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciVolanti)
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
				if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciVolanti.Count > 0)
				{
					this.timerAggRicerca += Time.deltaTime;
					if (this.timerAggRicerca > 1f)
					{
						this.timerAggRicerca = 0f;
						List<GameObject> list2 = new List<GameObject>();
						foreach (GameObject current2 in this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciVolanti)
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
				if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciVolanti.Count > 0)
				{
					GestoreNeutroStrategia.valoreRandomSeed++;
					UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
					float f2 = UnityEngine.Random.Range(0f, (float)this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciVolanti.Count - 0.01f);
					this.unitàBersaglio = this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciVolanti[Mathf.FloorToInt(f2)];
				}
			}
		}
		else
		{
			this.alleatoNav.speed = this.velocitàAlleatoNav;
			this.unitàBersaglio = null;
		}
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x0004E094 File Offset: 0x0004C294
	private void AttaccoIndipendente()
	{
		if (this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[0])
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
			List<float> expr_139 = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int index;
			int expr_13C = index = 5;
			float num = listaValoriArma[index];
			expr_139[expr_13C] = num - 1f;
			List<float> listaValoriArma2;
			List<float> expr_15D = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int expr_160 = index = 6;
			num = listaValoriArma2[index];
			expr_15D[expr_160] = num - 1f;
			this.SparoIndipendente1();
			this.timerDopoSparo = 0f;
		}
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x0004E228 File Offset: 0x0004C428
	private void SparoIndipendente1()
	{
		this.missile = (UnityEngine.Object.Instantiate(this.missilePrefab, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.missile.GetComponent<MissileAAFanteria>().ordignoAttivo = true;
		this.missile.GetComponent<DatiOrdignoInterno>().bersaglio = this.unitàBersaglio;
		this.missile.GetComponent<DatiOrdignoInterno>().locazioneTarget = this.centroUnitàBersaglio;
		this.missile.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x0004E2C4 File Offset: 0x0004C4C4
	private void AttaccoPrimaPersona()
	{
		this.ListaBersPPPossibili.Clear();
		this.timerLancio += Time.deltaTime;
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo))
		{
			if (this.targetSparo.collider.gameObject.tag == "Nemico" || this.targetSparo.collider.gameObject.tag == "Nemico Testa" || this.targetSparo.collider.gameObject.tag == "Nemico Coll Suppl")
			{
				if (Vector3.Distance(base.transform.position, this.targetSparo.point) > this.munizione.GetComponent<DatiGeneraliMunizione>().portataMinima && Vector3.Distance(base.transform.position, this.targetSparo.point) < this.munizione.GetComponent<DatiGeneraliMunizione>().portataMassima)
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
		if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici.Count > 0)
		{
			foreach (GameObject current in this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici)
			{
				if (current && current.GetComponent<PresenzaNemico>().insettoVolante)
				{
					Vector3 vector = current.transform.position - base.transform.position;
					float num = Vector3.Dot(base.transform.forward, vector.normalized);
					float num2 = Vector3.Distance(base.transform.position, current.transform.position);
					if (num2 > this.munizione.GetComponent<DatiGeneraliMunizione>().portataMinima && num2 < this.munizione.GetComponent<DatiGeneraliMunizione>().portataMassima && num > 0f && !Physics.Linecast(base.transform.position, current.transform.position, this.layerVisuale))
					{
						float num3 = Vector2.Distance(this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(this.sensoreRaggioMirino.transform.position), this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f))) / 10000f;
						float num4 = Vector2.Distance(this.terzaCamera.GetComponent<Camera>().WorldToScreenPoint(current.transform.position), this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f)));
						if (num4 < num3 && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0])
						{
							this.ListaBersPPPossibili.Add(current);
						}
					}
				}
			}
		}
		else
		{
			this.bersaglioInPP = null;
		}
		if (this.bersaglioInPP == null)
		{
			this.mirinoMissiliMobile.transform.position = this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f));
			if (base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0])
			{
				this.bersaglioInPP = null;
				this.timerDiAggancio = 0f;
				this.mirinoMissiliMobile.GetComponent<Image>().color = this.coloreBaseMirini;
				this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
				this.audioBeepCortoAttivo = false;
				this.audioBeepLungoAttivo = false;
				this.suonoAggancio.Stop();
				this.ListaBersPPPossibili.Clear();
			}
			else if (this.ListaBersPPPossibili.Count > 0)
			{
				this.bersaglioInPP = this.ListaBersPPPossibili[0];
			}
		}
		if (this.bersaglioInPP)
		{
			Vector3 vector2 = this.bersaglioInPP.transform.position - base.transform.position;
			float num5 = Vector3.Dot(base.transform.forward, vector2.normalized);
			if (num5 > 0f)
			{
				this.bersaglioèAvantiInPP = true;
			}
			else
			{
				this.bersaglioèAvantiInPP = false;
			}
		}
		if (this.bersaglioInPP)
		{
			if (this.ListaBersPPPossibili.Count > 1 && Input.GetMouseButtonDown(1))
			{
				float num6 = 999f;
				foreach (GameObject current2 in this.ListaBersPPPossibili)
				{
					float num7 = Vector2.Distance(this.terzaCamera.GetComponent<Camera>().WorldToScreenPoint(current2.transform.position), this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f)));
					if (num7 < num6)
					{
						num6 = num7;
						this.bersaglioInPP = current2;
						this.timerDiAggancio = 0f;
					}
				}
			}
			if (!this.audioBeepCortoAttivo)
			{
				this.suonoAggancio.clip = this.beepCorto;
				this.suonoAggancio.Play();
				this.audioBeepCortoAttivo = true;
			}
			this.mirinoMissiliMobile.transform.position = this.terzaCamera.GetComponent<Camera>().WorldToScreenPoint(this.bersaglioInPP.transform.position);
			float num8 = Vector2.Distance(this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(this.sensoreRaggioMirino.transform.position), this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f))) / 10000f;
			float num9 = Vector2.Distance(this.terzaCamera.GetComponent<Camera>().WorldToScreenPoint(this.bersaglioInPP.transform.position), this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f)));
			if (num9 > num8 || !this.bersaglioèAvantiInPP)
			{
				this.bersaglioInPP = null;
				this.timerDiAggancio = 0f;
				this.mirinoMissiliMobile.GetComponent<Image>().color = this.coloreBaseMirini;
				this.audioBeepCortoAttivo = false;
				this.audioBeepLungoAttivo = false;
				this.suonoAggancio.Stop();
			}
			if (this.bersaglioInPP)
			{
				this.timerDiAggancio += Time.deltaTime;
				this.mirinoMissiliMobile.GetComponent<Image>().color = Color.red;
				if (this.timerDiAggancio > 2f)
				{
					if (!this.audioBeepLungoAttivo)
					{
						this.suonoAggancio.clip = this.beepLungo;
						this.suonoAggancio.Play();
						this.audioBeepLungoAttivo = true;
					}
					this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1f;
					if (Input.GetMouseButtonDown(0) && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] && this.timerLancio > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[1] && base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f)
					{
						this.missile = (UnityEngine.Object.Instantiate(this.missilePrefab, this.strutturaFPS1.transform.GetChild(1).transform.position, this.strutturaFPS1.transform.GetChild(1).transform.rotation) as GameObject);
						this.missile.GetComponent<MissileAAFanteria>().ordignoAttivo = true;
						this.missile.GetComponent<DatiOrdignoInterno>().bersaglio = this.bersaglioInPP;
						this.missile.GetComponent<DatiOrdignoInterno>().lanciatoInFPS = true;
						this.timerLancio = 0f;
						this.missile.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
						this.timerFrequenzaArma1 = 0f;
						this.suonoArma.Play();
						List<float> listaValoriArma;
						List<float> expr_8DA = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
						int index;
						int expr_8DE = index = 5;
						float num10 = listaValoriArma[index];
						expr_8DA[expr_8DE] = num10 - 1f;
						List<float> listaValoriArma2;
						List<float> expr_904 = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
						int expr_908 = index = 6;
						num10 = listaValoriArma2[index];
						expr_904[expr_908] = num10 - 1f;
						this.avviaRinculo = true;
						this.timerRinculo = 0.4f;
						this.timerDopoSparo = 0f;
						this.audioBeepLungoAttivo = false;
						this.audioBeepCortoAttivo = false;
						this.suonoAggancio.Stop();
						if (!this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
						{
							this.particelleCulo1.Play();
							this.particelleCulo1Fumo.Play();
						}
					}
				}
				if (this.bersaglioInPP.GetComponent<PresenzaNemico>().vita < 0f)
				{
					this.bersaglioInPP = null;
					this.timerDiAggancio = 0f;
					this.mirinoMissiliMobile.GetComponent<Image>().color = this.coloreBaseMirini;
					this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
					this.audioBeepCortoAttivo = false;
					this.audioBeepLungoAttivo = false;
					this.suonoAggancio.Stop();
				}
			}
			if (this.timerDiAggancio < 2f)
			{
				this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
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

	// Token: 0x040007B9 RID: 1977
	public float angVertMax;

	// Token: 0x040007BA RID: 1978
	public float angVertMin;

	// Token: 0x040007BB RID: 1979
	public GameObject bocca1;

	// Token: 0x040007BC RID: 1980
	public GameObject culo1;

	// Token: 0x040007BD RID: 1981
	public GameObject ossoArma1;

	// Token: 0x040007BE RID: 1982
	private float timerFrequenzaArma1;

	// Token: 0x040007BF RID: 1983
	private float timerRicarica1;

	// Token: 0x040007C0 RID: 1984
	private bool ricaricaInCorso1;

	// Token: 0x040007C1 RID: 1985
	private float timerDopoSparo;

	// Token: 0x040007C2 RID: 1986
	private float tempoFraSparoERicarica;

	// Token: 0x040007C3 RID: 1987
	public GameObject missilePrefab;

	// Token: 0x040007C4 RID: 1988
	private GameObject missile;

	// Token: 0x040007C5 RID: 1989
	private GameObject CanvasFPS;

	// Token: 0x040007C6 RID: 1990
	private GameObject sensoreRaggioMirino;

	// Token: 0x040007C7 RID: 1991
	private GameObject sensoreRaggioMirinoMobile;

	// Token: 0x040007C8 RID: 1992
	private GameObject mirinoMissiliFisso;

	// Token: 0x040007C9 RID: 1993
	private GameObject mirinoMissiliMobile;

	// Token: 0x040007CA RID: 1994
	private Color coloreBaseMirini;

	// Token: 0x040007CB RID: 1995
	public GameObject strutturaPrefabFPS1;

	// Token: 0x040007CC RID: 1996
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x040007CD RID: 1997
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x040007CE RID: 1998
	public Vector3 aggiustamentoTraslVisualeFPS;

	// Token: 0x040007CF RID: 1999
	public Vector3 aggiustamentoRotVisualeFPS;

	// Token: 0x040007D0 RID: 2000
	private GameObject strutturaFPS1;

	// Token: 0x040007D1 RID: 2001
	private float timerPosizionamentoTPS;

	// Token: 0x040007D2 RID: 2002
	private float timerPosizionamentoFPS;

	// Token: 0x040007D3 RID: 2003
	private float campoCameraIniziale;

	// Token: 0x040007D4 RID: 2004
	private GameObject infoNeutreTattica;

	// Token: 0x040007D5 RID: 2005
	private GameObject terzaCamera;

	// Token: 0x040007D6 RID: 2006
	private GameObject primaCamera;

	// Token: 0x040007D7 RID: 2007
	private AudioSource suonoArma;

	// Token: 0x040007D8 RID: 2008
	private AudioSource suonoAggancio;

	// Token: 0x040007D9 RID: 2009
	private AudioSource suonoRicarica;

	// Token: 0x040007DA RID: 2010
	private NavMeshAgent alleatoNav;

	// Token: 0x040007DB RID: 2011
	private float velocitàAlleatoNav;

	// Token: 0x040007DC RID: 2012
	private GameObject IANemico;

	// Token: 0x040007DD RID: 2013
	private GameObject InfoAlleati;

	// Token: 0x040007DE RID: 2014
	private RaycastHit targetSparo;

	// Token: 0x040007DF RID: 2015
	private int layerColpo;

	// Token: 0x040007E0 RID: 2016
	private int layerVisuale;

	// Token: 0x040007E1 RID: 2017
	private GameObject unitàBersaglio;

	// Token: 0x040007E2 RID: 2018
	private Vector3 centroUnitàBersaglio;

	// Token: 0x040007E3 RID: 2019
	private GameObject bersaglioInPP;

	// Token: 0x040007E4 RID: 2020
	public List<GameObject> ListaBersPPPossibili;

	// Token: 0x040007E5 RID: 2021
	private float timerLancio;

	// Token: 0x040007E6 RID: 2022
	private bool bersaglioèAvantiInPP;

	// Token: 0x040007E7 RID: 2023
	private float timerDiAggancio;

	// Token: 0x040007E8 RID: 2024
	public bool avviaRinculo;

	// Token: 0x040007E9 RID: 2025
	public float timerRinculo;

	// Token: 0x040007EA RID: 2026
	private GameObject munizione;

	// Token: 0x040007EB RID: 2027
	private bool audioBeepCortoAttivo;

	// Token: 0x040007EC RID: 2028
	private bool audioBeepLungoAttivo;

	// Token: 0x040007ED RID: 2029
	public AudioClip beepCorto;

	// Token: 0x040007EE RID: 2030
	public AudioClip beepLungo;

	// Token: 0x040007EF RID: 2031
	private bool primoFrameAvvenuto;

	// Token: 0x040007F0 RID: 2032
	private ParticleSystem particelleCulo1;

	// Token: 0x040007F1 RID: 2033
	private ParticleSystem particelleCulo1Fumo;

	// Token: 0x040007F2 RID: 2034
	private float distFineOrdineMovimento;

	// Token: 0x040007F3 RID: 2035
	private bool calcoloDistJump;

	// Token: 0x040007F4 RID: 2036
	private bool calcoloJumpEffettuato;

	// Token: 0x040007F5 RID: 2037
	private float velocitàIniziale;

	// Token: 0x040007F6 RID: 2038
	private float timerAggRicerca;

	// Token: 0x040007F7 RID: 2039
	private bool suonoRicaricaAttivo;
}
