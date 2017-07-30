using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003A RID: 58
public class ATT_AssaultVehicle : MonoBehaviour
{
	// Token: 0x060002CA RID: 714 RVA: 0x00073938 File Offset: 0x00071B38
	private void Start()
	{
		this.CanvasFPS = GameObject.FindGameObjectWithTag("CanvasFPS");
		this.mirinoElettr1 = this.CanvasFPS.transform.GetChild(2).transform.GetChild(5).gameObject;
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.InfoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.alleatoNav = base.GetComponent<NavMeshAgent>();
		this.velocitàAlleatoNav = base.GetComponent<NavMeshAgent>().speed;
		this.layerColpo = 165120;
		this.layerVisuale = 256;
		this.cannone = base.GetComponent<MOV_AssaultVehicle>().cannoni;
		this.ListaMunizioniAttiveUnità = new List<GameObject>();
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma1);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma2);
		this.suonoTorretta = base.transform.GetChild(1).transform.GetChild(5).GetComponent<AudioSource>();
		this.suonoInterno = base.transform.GetChild(1).GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.volumeMotoreIniziale = this.suonoMotore.volume;
		this.suonoMotore.clip = this.motoreFermo;
		this.suonoMotore.Play();
		this.tempoFraSparoERicarica1 = 1f;
		this.tempoFraSparoERicarica2 = 1f;
		this.suonoArma1 = this.bocca1.GetComponent<AudioSource>();
		this.suonoArma2 = this.bocca2.GetComponent<AudioSource>();
		this.suonoRicarica1 = this.bocca1.transform.parent.parent.GetComponent<AudioSource>();
		this.suonoRicarica2 = this.bocca1.transform.parent.GetComponent<AudioSource>();
		this.ruote1 = base.transform.GetChild(1).transform.GetChild(1).gameObject;
		this.ruote2 = base.transform.GetChild(1).transform.GetChild(2).gameObject;
		this.ruote3 = base.transform.GetChild(1).transform.GetChild(3).gameObject;
		this.ruote4 = base.transform.GetChild(1).transform.GetChild(4).gameObject;
		if (GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.moltiplicatoreAttaccoInFPS = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().moltiplicatoreFPSBattVeloce;
		}
		else
		{
			this.moltiplicatoreAttaccoInFPS = PlayerPrefs.GetFloat("moltiplicatore danni PP");
		}
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.colpiBocca2 = this.bocca2.transform.GetChild(0).gameObject;
		this.particelleColpiBocca2 = this.bocca2.transform.GetChild(0).GetComponent<ParticleSystem>();
	}

	// Token: 0x060002CB RID: 715 RVA: 0x00073C48 File Offset: 0x00071E48
	private void Update()
	{
		this.ListaMunizioniAttiveUnità[0] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[0][0];
		this.ListaMunizioniAttiveUnità[1] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[1][0];
		this.munizioneArma1 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0];
		this.munizioneArma2 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[1];
		this.timerFrequenzaArma1 += Time.deltaTime;
		this.timerFrequenzaArma2 += Time.deltaTime;
		this.timerDopoSparo1 += Time.deltaTime;
		this.timerDopoSparo2 += Time.deltaTime;
		this.CondizioniArma1();
		this.CondizioniArma2();
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
			this.SelezioneArma();
			if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 0)
			{
				this.AttaccoPrimaPersonaArma1();
			}
			if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 1)
			{
				this.AttaccoPrimaPersonaArma2();
			}
			base.GetComponent<NavMeshAgent>().enabled = false;
			if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
			{
				this.mirinoElettr1.GetComponent<CanvasGroup>().alpha = 1f;
				this.mirinoElettr1.GetComponent<Image>().sprite = this.mirinoTPS;
			}
			if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva == 3)
			{
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
					this.mirinoElettr1.GetComponent<Image>().sprite = this.mirinoFPS;
					this.mirinoElettr1.GetComponent<CanvasGroup>().alpha = 1f;
					this.suonoInterno.Play();
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
					this.zoomAttivo = false;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					this.mirinoElettr1.GetComponent<Image>().sprite = this.mirinoTPS;
					this.mirinoElettr1.GetComponent<CanvasGroup>().alpha = 1f;
					this.suonoInterno.Stop();
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
					this.zoomAttivo = false;
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
			base.GetComponent<MOV_AssaultVehicle>().torretta.transform.rotation = base.transform.rotation;
			base.GetComponent<MOV_AssaultVehicle>().cannoni.transform.rotation = base.transform.rotation;
			this.suonoTorretta.Stop();
			base.GetComponent<MOV_AssaultVehicle>().suonoTorrettaPartito = false;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
			this.zoomAttivo = false;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x060002CC RID: 716 RVA: 0x00074010 File Offset: 0x00072210
	private void GestioneSuoniIndipendenti()
	{
		this.suonoMotore.volume = this.volumeMotoreIniziale;
		this.suonoInterno.Stop();
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

	// Token: 0x060002CD RID: 717 RVA: 0x000741D0 File Offset: 0x000723D0
	private void GestioneRuote()
	{
		if (this.suonoMotore.clip == this.motorePartenza || this.suonoMotore.clip == this.motoreViaggio)
		{
			this.ruote1.transform.Rotate(Vector3.right * 4f);
			this.ruote2.transform.Rotate(Vector3.right * 4f);
			this.ruote3.transform.Rotate(Vector3.right * 4f);
			this.ruote4.transform.Rotate(Vector3.right * 4f);
		}
	}

	// Token: 0x060002CE RID: 718 RVA: 0x00074290 File Offset: 0x00072490
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
					this.suonoRicarica1.Play();
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

	// Token: 0x060002CF RID: 719 RVA: 0x000744F0 File Offset: 0x000726F0
	private void CondizioniArma2()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] <= 0f && this.timerDopoSparo2 > this.tempoFraSparoERicarica2)
		{
			base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[1] = true;
		}
		if (Input.GetKeyDown(KeyCode.R) && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[1] && this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera == base.gameObject && base.GetComponent<PresenzaAlleato>().ListaArmi[1][6] > 0f && base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] < base.GetComponent<PresenzaAlleato>().ListaArmi[1][3])
		{
			base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[1] = true;
		}
		if (base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[1])
		{
			this.timerRicarica2 += Time.deltaTime;
			if (base.GetComponent<PresenzaAlleato>().ListaArmi[1][6] > 0f)
			{
				if (this.timerRicarica2 > 0f && this.timerRicarica2 < 0.1f)
				{
					this.suonoRicarica2.Play();
				}
				if (this.timerRicarica2 > base.GetComponent<PresenzaAlleato>().ListaArmi[1][2])
				{
					if (base.GetComponent<PresenzaAlleato>().ListaArmi[1][6] < base.GetComponent<PresenzaAlleato>().ListaArmi[1][3])
					{
						base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[1][6];
						this.timerRicarica2 = 0f;
						base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[1] = false;
					}
					else
					{
						base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[1][3];
						this.timerRicarica2 = 0f;
						base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[1] = false;
					}
				}
			}
		}
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x00074750 File Offset: 0x00072950
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
		if (Input.GetMouseButtonDown(1))
		{
			if (this.zoomAttivo)
			{
				this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
				this.zoomAttivo = false;
			}
			else
			{
				this.terzaCamera.GetComponent<Camera>().fieldOfView = 30f;
				this.zoomAttivo = true;
			}
		}
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x000747FC File Offset: 0x000729FC
	private void CameraTPS()
	{
		this.timerPosizionamentoTPS += Time.deltaTime;
		if (this.timerPosizionamentoTPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.cannone.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraTPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localEulerAngles = new Vector3(this.rotazioneCameraTPS.x, 0f, this.cannone.transform.eulerAngles.z);
		}
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x000748A8 File Offset: 0x00072AA8
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.cannone.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = this.cannone.transform.rotation;
		}
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x00074938 File Offset: 0x00072B38
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
		float num2 = 0f;
		for (int i = 0; i < base.GetComponent<PresenzaAlleato>().numeroArmi; i++)
		{
			if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[i] && this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima > num2)
			{
				num2 = this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima;
			}
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
					float num3 = Vector3.Dot((this.centroUnitàBersaglio - base.transform.position).normalized, base.transform.up);
					if (num3 < this.angVertMax && num3 > this.angVertMin)
					{
						float num4 = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
						if (num4 >= num2)
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
						for (int j = 0; j < base.GetComponent<PresenzaAlleato>().numeroArmi; j++)
						{
							if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[j] && num4 < this.ListaMunizioniAttiveUnità[j].GetComponent<DatiGeneraliMunizione>().portataMassima)
							{
								base.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
								this.cannone.transform.LookAt(this.centroUnitàBersaglio);
								if (j == 0)
								{
									this.AttaccoIndipendente1();
								}
								if (j == 1)
								{
									this.AttaccoIndipendente2();
								}
							}
						}
						if (num4 < num2)
						{
							if (!flag)
							{
								base.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
								this.cannone.transform.LookAt(this.centroUnitàBersaglio);
								this.alleatoNav.speed = 0f;
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
				float num5 = Vector3.Dot((this.centroUnitàBersaglio - base.transform.position).normalized, base.transform.up);
				float num6 = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
				if (num5 < this.angVertMax && num5 > this.angVertMin)
				{
					if (num6 >= num2)
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
					for (int k = 0; k < base.GetComponent<PresenzaAlleato>().numeroArmi; k++)
					{
						if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[k] && num6 < this.ListaMunizioniAttiveUnità[k].GetComponent<DatiGeneraliMunizione>().portataMassima)
						{
							base.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
							this.cannone.transform.LookAt(this.centroUnitàBersaglio);
							if (k == 0)
							{
								this.AttaccoIndipendente1();
							}
							else if (k == 1)
							{
								this.AttaccoIndipendente2();
							}
						}
					}
					if (num6 < num2)
					{
						if (!flag)
						{
							base.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
							this.cannone.transform.LookAt(this.centroUnitàBersaglio);
							this.alleatoNav.speed = 0f;
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
				else if (num6 > 3f)
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
								float num7 = Vector3.Distance(base.transform.position, current.GetComponent<PresenzaNemico>().centroInsetto);
								if (num7 < this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMassima && !Physics.Linecast(this.bocca1.transform.position, current.GetComponent<PresenzaNemico>().centroInsetto, this.layerVisuale))
								{
									float num8 = Vector3.Dot((current.GetComponent<PresenzaNemico>().centroInsetto - base.transform.position).normalized, base.transform.up);
									if (num8 < this.angVertMax && num8 > this.angVertMin)
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
								float num9 = Vector3.Distance(base.transform.position, current2.GetComponent<PresenzaNemico>().centroInsetto);
								if (num9 < this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMassima && !Physics.Linecast(this.bocca1.transform.position, current2.GetComponent<PresenzaNemico>().centroInsetto, this.layerVisuale))
								{
									float num10 = Vector3.Dot((current2.GetComponent<PresenzaNemico>().centroInsetto - base.transform.position).normalized, base.transform.up);
									if (num10 < this.angVertMax && num10 > this.angVertMin)
									{
										list2.Add(current2);
									}
								}
							}
						}
						if (list2.Count > 0)
						{
							float num11 = 9999f;
							for (int l = 0; l < list2.Count; l++)
							{
								float num12 = Vector3.Distance(base.transform.position, list2[l].GetComponent<PresenzaNemico>().centroInsetto);
								if (num12 < num11)
								{
									num11 = num12;
									this.unitàBersaglio = list2[l];
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

	// Token: 0x060002D4 RID: 724 RVA: 0x00075574 File Offset: 0x00073774
	private void AttaccoIndipendente1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca2.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][0])
		{
			if (this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaAllPiùViciniPerTipo.Contains(base.gameObject) || this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] > 0.05f)
			{
				this.suonoArma1.Play();
				this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] = 0f;
			}
			this.timerFrequenzaArma1 = 0f;
			this.bocca1.GetComponent<ParticleSystem>().Play();
			List<float> list;
			List<float> expr_15B = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int index;
			int expr_15E = index = 5;
			float num = list[index];
			expr_15B[expr_15E] = num - 1f;
			List<float> list2;
			List<float> expr_187 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int expr_18B = index = 6;
			num = list2[index];
			expr_187[expr_18B] = num - 1f;
			this.SparoIndipendente1();
			float num2 = this.unitàBersaglio.GetComponent<PresenzaNemico>().armatura - this.ListaMunizioniAttiveUnità[0].GetComponent<DatiGeneraliMunizione>().penetrazione;
			if (num2 <= 0f)
			{
				this.unitàBersaglio.GetComponent<PresenzaNemico>().vita -= this.ListaMunizioniAttiveUnità[0].GetComponent<DatiGeneraliMunizione>().danno;
			}
			else
			{
				float num3 = num2 - this.ListaMunizioniAttiveUnità[0].GetComponent<DatiGeneraliMunizione>().danno;
				if (num3 < 0f)
				{
					this.unitàBersaglio.GetComponent<PresenzaNemico>().vita += num3;
				}
			}
		}
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x000757CC File Offset: 0x000739CC
	private void SparoIndipendente1()
	{
		this.granata40mm = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.granata40mm.GetComponent<DatiProiettile>().locazioneTarget = this.centroUnitàBersaglio;
		this.granata40mm.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x00075840 File Offset: 0x00073A40
	private void AttaccoIndipendente2()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[1] && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca2.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] > 0f && this.timerFrequenzaArma2 > base.GetComponent<PresenzaAlleato>().ListaArmi[1][0])
		{
			if (this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaAllPiùViciniPerTipo.Contains(base.gameObject) || this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][1] > 0.05f)
			{
				this.suonoArma2.Play();
				this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][1] = 0f;
			}
			this.colpiBocca2.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
			this.timerFrequenzaArma2 = 0f;
			this.bocca2.GetComponent<ParticleSystem>().Play();
			this.particelleColpiBocca2.Play();
			List<float> list;
			List<float> expr_1A4 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
			int index;
			int expr_1A7 = index = 5;
			float num = list[index];
			expr_1A4[expr_1A7] = num - 1f;
			List<float> list2;
			List<float> expr_1D0 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
			int expr_1D4 = index = 6;
			num = list2[index];
			expr_1D0[expr_1D4] = num - 1f;
			float num2 = 0f;
			if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().penetrazione)
			{
				num2 = this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().penetrazione;
			}
			else if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f)
			{
				num2 = this.unitàBersaglio.GetComponent<PresenzaNemico>().vita;
			}
			this.unitàBersaglio.GetComponent<PresenzaNemico>().vita -= this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().penetrazione;
			if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.unitàBersaglio.GetComponent<PresenzaNemico>().armatura))
			{
				num2 += this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.unitàBersaglio.GetComponent<PresenzaNemico>().armatura);
			}
			else if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f)
			{
				num2 += this.unitàBersaglio.GetComponent<PresenzaNemico>().vita;
			}
			this.unitàBersaglio.GetComponent<PresenzaNemico>().vita -= this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.unitàBersaglio.GetComponent<PresenzaNemico>().armatura);
			List<float> listaDanniAlleati;
			List<float> expr_366 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
			int expr_374 = index = base.GetComponent<PresenzaAlleato>().tipoTruppa;
			num = listaDanniAlleati[index];
			expr_366[expr_374] = num + num2;
		}
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x00075BD8 File Offset: 0x00073DD8
	private void SelezioneArma()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 0;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 1;
		}
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x00075C18 File Offset: 0x00073E18
	private void AttaccoPrimaPersonaArma1()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo))
		{
			if (this.targetSparo.collider.gameObject.tag == "Nemico" || this.targetSparo.collider.gameObject.tag == "Nemico Testa" || this.targetSparo.collider.gameObject.tag == "Nemico Coll Suppl")
			{
				if (Vector3.Distance(base.transform.position, this.targetSparo.point) <= this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMassima)
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
		if (Input.GetMouseButton(0) && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][1])
		{
			this.timerFrequenzaArma1 = 0f;
			this.SparoArma1();
			this.suonoArma1.Play();
			List<float> list;
			List<float> expr_1A3 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int index;
			int expr_1A6 = index = 5;
			float num = list[index];
			expr_1A3[expr_1A6] = num - 1f;
			List<float> list2;
			List<float> expr_1CD = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int expr_1D1 = index = 6;
			num = list2[index];
			expr_1CD[expr_1D1] = num - 1f;
			this.bocca1.GetComponent<ParticleSystem>().Play();
		}
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x00075E20 File Offset: 0x00074020
	private void SparoArma1()
	{
		this.granata40mm = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.granata40mm.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.granata40mm.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060002DA RID: 730 RVA: 0x00075E90 File Offset: 0x00074090
	private void AttaccoPrimaPersonaArma2()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo))
		{
			if (this.targetSparo.collider.gameObject.tag == "Nemico" || this.targetSparo.collider.gameObject.tag == "Nemico Testa" || this.targetSparo.collider.gameObject.tag == "Nemico Coll Suppl")
			{
				if (Vector3.Distance(base.transform.position, this.targetSparo.point) <= this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().portataMassima)
				{
					base.GetComponent<PresenzaAlleato>().ListaFuoriPortataArmi[1] = false;
				}
				else
				{
					base.GetComponent<PresenzaAlleato>().ListaFuoriPortataArmi[1] = true;
				}
			}
			else
			{
				base.GetComponent<PresenzaAlleato>().ListaFuoriPortataArmi[1] = false;
			}
		}
		if (Input.GetMouseButton(0) && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[1] && base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] > 0f && this.timerFrequenzaArma2 > base.GetComponent<PresenzaAlleato>().ListaArmi[1][1])
		{
			this.timerFrequenzaArma2 = 0f;
			this.SparoArma2();
			this.suonoArma2.Play();
			List<float> list;
			List<float> expr_1A3 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
			int index;
			int expr_1A6 = index = 5;
			float num = list[index];
			expr_1A3[expr_1A6] = num - 1f;
			List<float> list2;
			List<float> expr_1CF = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
			int expr_1D3 = index = 6;
			num = list2[index];
			expr_1CF[expr_1D3] = num - 1f;
			this.bocca2.GetComponent<ParticleSystem>().Play();
			Vector3 normalized = (this.targetSparo.point - this.colpiBocca2.transform.position).normalized;
			this.colpiBocca2.transform.forward = normalized;
			this.particelleColpiBocca2.Play();
		}
	}

	// Token: 0x060002DB RID: 731 RVA: 0x000760E0 File Offset: 0x000742E0
	private void SparoArma2()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo, this.ListaMunizioniAttiveUnità[1].GetComponent<DatiGeneraliMunizione>().portataMassima, this.layerColpo))
		{
			if (this.targetSparo.collider.gameObject.tag == "Nemico")
			{
				GameObject gameObject = this.targetSparo.collider.gameObject;
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
				float num = 0f;
				if (gameObject.GetComponent<PresenzaNemico>().vita > this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
				{
					num = this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				}
				else if (gameObject.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num = gameObject.GetComponent<PresenzaNemico>().vita;
				}
				gameObject.GetComponent<PresenzaNemico>().vita -= this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				if (gameObject.GetComponent<PresenzaNemico>().vita > this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS)
				{
					num += this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
				}
				else if (gameObject.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num += gameObject.GetComponent<PresenzaNemico>().vita;
				}
				gameObject.GetComponent<PresenzaNemico>().vita -= this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
				List<float> listaDanniAlleati;
				List<float> expr_208 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int tipoTruppa;
				int expr_216 = tipoTruppa = base.GetComponent<PresenzaAlleato>().tipoTruppa;
				float num2 = listaDanniAlleati[tipoTruppa];
				expr_208[expr_216] = num2 + num;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num;
			}
			else if (this.targetSparo.collider.gameObject.tag == "Nemico Testa")
			{
				GameObject gameObject2 = this.targetSparo.collider.transform.parent.gameObject;
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
				float num3 = 0f;
				if (gameObject2.GetComponent<PresenzaNemico>().vita > this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f)
				{
					num3 = this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f;
				}
				else if (gameObject2.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num3 = gameObject2.GetComponent<PresenzaNemico>().vita;
				}
				gameObject2.GetComponent<PresenzaNemico>().vita -= this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f;
				if (gameObject2.GetComponent<PresenzaNemico>().vita > this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject2.GetComponent<PresenzaNemico>().armatura) * 2f)
				{
					num3 += this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject2.GetComponent<PresenzaNemico>().armatura) * 2f;
				}
				else if (gameObject2.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num3 += gameObject2.GetComponent<PresenzaNemico>().vita;
				}
				gameObject2.GetComponent<PresenzaNemico>().vita -= this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject2.GetComponent<PresenzaNemico>().armatura) * 2f;
				List<float> listaDanniAlleati2;
				List<float> expr_40B = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int tipoTruppa;
				int expr_419 = tipoTruppa = base.GetComponent<PresenzaAlleato>().tipoTruppa;
				float num2 = listaDanniAlleati2[tipoTruppa];
				expr_40B[expr_419] = num2 + num3;
			}
			else if (this.targetSparo.collider.gameObject.tag == "Nemico Coll Suppl")
			{
				GameObject gameObject3 = this.targetSparo.collider.transform.parent.gameObject;
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
				float num4 = 0f;
				if (gameObject3.GetComponent<PresenzaNemico>().vita > this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
				{
					num4 = this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				}
				else if (gameObject3.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num4 = gameObject3.GetComponent<PresenzaNemico>().vita;
				}
				gameObject3.GetComponent<PresenzaNemico>().vita -= this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				if (gameObject3.GetComponent<PresenzaNemico>().vita > this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject3.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS)
				{
					num4 += this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject3.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
				}
				else if (gameObject3.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num4 += gameObject3.GetComponent<PresenzaNemico>().vita;
				}
				gameObject3.GetComponent<PresenzaNemico>().vita -= this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject3.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
				List<float> listaDanniAlleati3;
				List<float> expr_609 = listaDanniAlleati3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int tipoTruppa;
				int expr_617 = tipoTruppa = base.GetComponent<PresenzaAlleato>().tipoTruppa;
				float num2 = listaDanniAlleati3[tipoTruppa];
				expr_609[expr_617] = num2 + num4;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num4;
			}
		}
	}

	// Token: 0x04000BC5 RID: 3013
	public float angVertMax;

	// Token: 0x04000BC6 RID: 3014
	public float angVertMin;

	// Token: 0x04000BC7 RID: 3015
	private GameObject infoNeutreTattica;

	// Token: 0x04000BC8 RID: 3016
	private GameObject terzaCamera;

	// Token: 0x04000BC9 RID: 3017
	private GameObject primaCamera;

	// Token: 0x04000BCA RID: 3018
	public GameObject bocca1;

	// Token: 0x04000BCB RID: 3019
	public GameObject bocca2;

	// Token: 0x04000BCC RID: 3020
	private GameObject colpiBocca2;

	// Token: 0x04000BCD RID: 3021
	private GameObject IANemico;

	// Token: 0x04000BCE RID: 3022
	private GameObject InfoAlleati;

	// Token: 0x04000BCF RID: 3023
	private float timerFrequenzaArma1;

	// Token: 0x04000BD0 RID: 3024
	private float timerRicarica1;

	// Token: 0x04000BD1 RID: 3025
	private bool ricaricaInCorso1;

	// Token: 0x04000BD2 RID: 3026
	private float timerDopoSparo1;

	// Token: 0x04000BD3 RID: 3027
	private float tempoFraSparoERicarica1;

	// Token: 0x04000BD4 RID: 3028
	private float timerFrequenzaArma2;

	// Token: 0x04000BD5 RID: 3029
	private float timerRicarica2;

	// Token: 0x04000BD6 RID: 3030
	private bool ricaricaInCorso2;

	// Token: 0x04000BD7 RID: 3031
	private float timerDopoSparo2;

	// Token: 0x04000BD8 RID: 3032
	private float tempoFraSparoERicarica2;

	// Token: 0x04000BD9 RID: 3033
	private int layerColpo;

	// Token: 0x04000BDA RID: 3034
	private int layerVisuale;

	// Token: 0x04000BDB RID: 3035
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000BDC RID: 3036
	public Vector3 rotazioneCameraTPS;

	// Token: 0x04000BDD RID: 3037
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000BDE RID: 3038
	private float timerPosizionamentoTPS;

	// Token: 0x04000BDF RID: 3039
	private float timerPosizionamentoFPS;

	// Token: 0x04000BE0 RID: 3040
	private GameObject CanvasFPS;

	// Token: 0x04000BE1 RID: 3041
	private GameObject mirinoElettr1;

	// Token: 0x04000BE2 RID: 3042
	public Sprite mirinoTPS;

	// Token: 0x04000BE3 RID: 3043
	public Sprite mirinoFPS;

	// Token: 0x04000BE4 RID: 3044
	private RaycastHit targetSparo;

	// Token: 0x04000BE5 RID: 3045
	private GameObject granata40mm;

	// Token: 0x04000BE6 RID: 3046
	private NavMeshAgent alleatoNav;

	// Token: 0x04000BE7 RID: 3047
	private float velocitàAlleatoNav;

	// Token: 0x04000BE8 RID: 3048
	private GameObject cannone;

	// Token: 0x04000BE9 RID: 3049
	private GameObject unitàBersaglio;

	// Token: 0x04000BEA RID: 3050
	private Vector3 centroUnitàBersaglio;

	// Token: 0x04000BEB RID: 3051
	private GameObject munizioneArma1;

	// Token: 0x04000BEC RID: 3052
	private GameObject munizioneArma2;

	// Token: 0x04000BED RID: 3053
	private AudioSource suonoTorretta;

	// Token: 0x04000BEE RID: 3054
	private AudioSource suonoInterno;

	// Token: 0x04000BEF RID: 3055
	private AudioSource suonoMotore;

	// Token: 0x04000BF0 RID: 3056
	public AudioClip motoreFermo;

	// Token: 0x04000BF1 RID: 3057
	public AudioClip motorePartenza;

	// Token: 0x04000BF2 RID: 3058
	public AudioClip motoreViaggio;

	// Token: 0x04000BF3 RID: 3059
	public AudioClip motoreStop;

	// Token: 0x04000BF4 RID: 3060
	private float timerPartenza;

	// Token: 0x04000BF5 RID: 3061
	private float timerStop;

	// Token: 0x04000BF6 RID: 3062
	private bool primaPartenza;

	// Token: 0x04000BF7 RID: 3063
	public float volumeMotoreIniziale;

	// Token: 0x04000BF8 RID: 3064
	private bool inPartenza;

	// Token: 0x04000BF9 RID: 3065
	private bool partenzaFinita;

	// Token: 0x04000BFA RID: 3066
	private bool inStop;

	// Token: 0x04000BFB RID: 3067
	public bool stopFinito;

	// Token: 0x04000BFC RID: 3068
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x04000BFD RID: 3069
	private AudioSource suonoArma1;

	// Token: 0x04000BFE RID: 3070
	private AudioSource suonoArma2;

	// Token: 0x04000BFF RID: 3071
	public AudioSource suonoRicarica1;

	// Token: 0x04000C00 RID: 3072
	public AudioSource suonoRicarica2;

	// Token: 0x04000C01 RID: 3073
	private GameObject ruote1;

	// Token: 0x04000C02 RID: 3074
	private GameObject ruote2;

	// Token: 0x04000C03 RID: 3075
	private GameObject ruote3;

	// Token: 0x04000C04 RID: 3076
	private GameObject ruote4;

	// Token: 0x04000C05 RID: 3077
	private bool zoomAttivo;

	// Token: 0x04000C06 RID: 3078
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x04000C07 RID: 3079
	private float distFineOrdineMovimento;

	// Token: 0x04000C08 RID: 3080
	private float timerAggRicerca;

	// Token: 0x04000C09 RID: 3081
	private ParticleSystem particelleColpiBocca2;
}
