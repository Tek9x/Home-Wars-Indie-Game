﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002E RID: 46
public class ATT_MachineGun : MonoBehaviour
{
	// Token: 0x0600021F RID: 543 RVA: 0x0005BFE4 File Offset: 0x0005A1E4
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
		this.tempoFraSparoERicarica = 1f;
		this.suonoArma = base.GetComponent<AudioSource>();
		this.suonoRicarica = base.transform.GetChild(1).GetComponent<AudioSource>();
		this.colpiBocca1 = this.bocca1.transform.GetChild(0).gameObject;
		this.particelleBocca1 = this.bocca1.GetComponent<ParticleSystem>();
		this.particelleColpiBocca1 = this.bocca1.transform.GetChild(0).GetComponent<ParticleSystem>();
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.velocitàIniziale = this.alleatoNav.speed;
	}

	// Token: 0x06000220 RID: 544 RVA: 0x0005C1B0 File Offset: 0x0005A3B0
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
			if (this.strutturaFPS1)
			{
				UnityEngine.Object.Destroy(this.strutturaFPS1);
			}
		}
		else
		{
			this.GestioneVisuali();
			this.AttaccoPrimaPersonaArma1();
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
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x06000221 RID: 545 RVA: 0x0005C430 File Offset: 0x0005A630
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

	// Token: 0x06000222 RID: 546 RVA: 0x0005C4B4 File Offset: 0x0005A6B4
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

	// Token: 0x06000223 RID: 547 RVA: 0x0005C714 File Offset: 0x0005A914
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

	// Token: 0x06000224 RID: 548 RVA: 0x0005C82C File Offset: 0x0005AA2C
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

	// Token: 0x06000225 RID: 549 RVA: 0x0005C8CC File Offset: 0x0005AACC
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

	// Token: 0x06000226 RID: 550 RVA: 0x0005C94C File Offset: 0x0005AB4C
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
					if (num5 >= this.munizione.GetComponent<DatiGeneraliMunizione>().portataMassima && !base.GetComponent<PresenzaAlleato>().comportamentoDifensivo && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino)
					{
						this.alleatoNav.SetDestination(this.unitàBersaglio.transform.position);
						this.alleatoNav.speed = this.velocitàAlleatoNav;
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

	// Token: 0x06000227 RID: 551 RVA: 0x0005D37C File Offset: 0x0005B57C
	private void AttaccoIndipendente()
	{
		if (this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[0])
		{
			if (this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaAllPiùViciniPerTipo.Contains(base.gameObject) || this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] > 0.1f)
			{
				this.suonoArma.Play();
				this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] = 0f;
			}
			this.colpiBocca1.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
			this.timerFrequenzaArma1 = 0f;
			this.particelleBocca1.Play();
			this.particelleColpiBocca1.Play();
			List<float> listaValoriArma;
			List<float> expr_177 = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int index;
			int expr_17A = index = 5;
			float num = listaValoriArma[index];
			expr_177[expr_17A] = num - 1f;
			List<float> listaValoriArma2;
			List<float> expr_19D = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int expr_1A1 = index = 6;
			num = listaValoriArma2[index];
			expr_19D[expr_1A1] = num - 1f;
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
			List<float> expr_33E = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
			int expr_34C = index = base.GetComponent<PresenzaAlleato>().tipoTruppa;
			num = listaDanniAlleati[index];
			expr_33E[expr_34C] = num + num2;
		}
	}

	// Token: 0x06000228 RID: 552 RVA: 0x0005D6EC File Offset: 0x0005B8EC
	private void AttaccoPrimaPersonaArma1()
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
			this.SparoArma1();
			this.suonoArma.Play();
			if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				Vector3 normalized = (this.targetSparo.point - this.strutturaFPS1.transform.GetChild(1).GetChild(0).transform.position).normalized;
				this.strutturaFPS1.transform.GetChild(1).GetChild(0).transform.forward = normalized;
				this.strutturaFPS1.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
				this.strutturaFPS1.transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().Play();
			}
			else
			{
				Vector3 normalized2 = (this.targetSparo.point - this.colpiBocca1.transform.position).normalized;
				this.colpiBocca1.transform.forward = normalized2;
				this.particelleBocca1.Play();
			}
			List<float> listaValoriArma;
			List<float> expr_289 = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int index;
			int expr_28D = index = 5;
			float num = listaValoriArma[index];
			expr_289[expr_28D] = num - 1f;
			List<float> listaValoriArma2;
			List<float> expr_2B3 = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int expr_2B7 = index = 6;
			num = listaValoriArma2[index];
			expr_2B3[expr_2B7] = num - 1f;
			this.timerDopoSparo = 0f;
		}
	}

	// Token: 0x06000229 RID: 553 RVA: 0x0005D9D8 File Offset: 0x0005BBD8
	private void SparoArma1()
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

	// Token: 0x04000948 RID: 2376
	public float angVertMax;

	// Token: 0x04000949 RID: 2377
	public float angVertMin;

	// Token: 0x0400094A RID: 2378
	public GameObject bocca1;

	// Token: 0x0400094B RID: 2379
	private GameObject colpiBocca1;

	// Token: 0x0400094C RID: 2380
	public GameObject ossoArma1;

	// Token: 0x0400094D RID: 2381
	private float timerFrequenzaArma1;

	// Token: 0x0400094E RID: 2382
	private float timerRicarica1;

	// Token: 0x0400094F RID: 2383
	private bool ricaricaInCorso1;

	// Token: 0x04000950 RID: 2384
	private float timerDopoSparo;

	// Token: 0x04000951 RID: 2385
	private float tempoFraSparoERicarica;

	// Token: 0x04000952 RID: 2386
	private GameObject CanvasFPS;

	// Token: 0x04000953 RID: 2387
	private GameObject mirinoFisso;

	// Token: 0x04000954 RID: 2388
	public GameObject strutturaPrefabFPS1;

	// Token: 0x04000955 RID: 2389
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000956 RID: 2390
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000957 RID: 2391
	public Vector3 aggiustamentoTraslVisualeFPS;

	// Token: 0x04000958 RID: 2392
	public Vector3 aggiustamentoRotVisualeFPS;

	// Token: 0x04000959 RID: 2393
	private GameObject strutturaFPS1;

	// Token: 0x0400095A RID: 2394
	private float timerPosizionamentoTPS;

	// Token: 0x0400095B RID: 2395
	private float timerPosizionamentoFPS;

	// Token: 0x0400095C RID: 2396
	private float campoCameraIniziale;

	// Token: 0x0400095D RID: 2397
	private GameObject infoNeutreTattica;

	// Token: 0x0400095E RID: 2398
	private GameObject terzaCamera;

	// Token: 0x0400095F RID: 2399
	private GameObject primaCamera;

	// Token: 0x04000960 RID: 2400
	private NavMeshAgent alleatoNav;

	// Token: 0x04000961 RID: 2401
	private float velocitàAlleatoNav;

	// Token: 0x04000962 RID: 2402
	private GameObject IANemico;

	// Token: 0x04000963 RID: 2403
	private GameObject InfoAlleati;

	// Token: 0x04000964 RID: 2404
	private RaycastHit targetSparo;

	// Token: 0x04000965 RID: 2405
	private int layerColpo;

	// Token: 0x04000966 RID: 2406
	private int layerVisuale;

	// Token: 0x04000967 RID: 2407
	private GameObject unitàBersaglio;

	// Token: 0x04000968 RID: 2408
	private Vector3 centroUnitàBersaglio;

	// Token: 0x04000969 RID: 2409
	private GameObject munizione;

	// Token: 0x0400096A RID: 2410
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x0400096B RID: 2411
	private bool primoFrameAvvenuto;

	// Token: 0x0400096C RID: 2412
	private AudioSource suonoArma;

	// Token: 0x0400096D RID: 2413
	private AudioSource suonoRicarica;

	// Token: 0x0400096E RID: 2414
	private ParticleSystem particelleBocca1;

	// Token: 0x0400096F RID: 2415
	private ParticleSystem particelleColpiBocca1;

	// Token: 0x04000970 RID: 2416
	private float distFineOrdineMovimento;

	// Token: 0x04000971 RID: 2417
	private bool calcoloDistJump;

	// Token: 0x04000972 RID: 2418
	private bool calcoloJumpEffettuato;

	// Token: 0x04000973 RID: 2419
	private float velocitàIniziale;

	// Token: 0x04000974 RID: 2420
	private float timerAggRicerca;
}
