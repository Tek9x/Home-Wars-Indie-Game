using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003B RID: 59
public class ATT_BattleTank : MonoBehaviour
{
	// Token: 0x060002DD RID: 733 RVA: 0x00076740 File Offset: 0x00074940
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
		this.cannoni1e2 = base.GetComponent<MOV_BattleTank>().cannoni1e2;
		this.cannoni3e4 = base.GetComponent<MOV_BattleTank>().cannoni3e4;
		this.ListaMunizioniAttiveUnità = new List<GameObject>();
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma1);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma2);
		this.suonoTorretta = base.transform.GetChild(1).transform.GetChild(1).GetComponent<AudioSource>();
		this.suonoInterno = base.transform.GetChild(1).GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.volumeMotoreIniziale = this.suonoMotore.volume;
		this.suonoMotore.clip = this.motoreFermo;
		this.suonoMotore.Play();
		this.tempoFraSparoERicarica1 = 1f;
		this.tempoFraSparoERicarica2 = 1f;
		this.particelleBoccaTPS1 = this.bocca3.GetComponent<ParticleSystem>();
		this.particelleBoccaTPS2 = this.bocca4.GetComponent<ParticleSystem>();
		this.suonoFlamethrower1 = this.bocca3.GetComponent<AudioSource>();
		this.suonoFlamethrower2 = this.bocca4.GetComponent<AudioSource>();
		this.suonoCannone1 = this.bocca1.GetComponent<AudioSource>();
		this.suonoCannone2 = this.bocca2.GetComponent<AudioSource>();
		this.ListaBocche = new List<GameObject>();
		this.ListaBocche.Add(this.bocca1);
		this.ListaBocche.Add(this.bocca2);
		this.ListaCannoni = new List<GameObject>();
		this.ListaCannoni.Add(this.bocca1.transform.parent.gameObject);
		this.ListaCannoni.Add(this.bocca2.transform.parent.gameObject);
		this.ListaBoolCannoniSparati = new List<bool>();
		this.ListaBoolCannoniSparati.Add(this.cannone1Sparato);
		this.ListaBoolCannoniSparati.Add(this.cannone2Sparato);
		this.ListaBoolCannoniInFondo = new List<bool>();
		this.ListaBoolCannoniInFondo.Add(this.cannone1InFondo);
		this.ListaBoolCannoniInFondo.Add(this.cannone2InFondo);
		this.ListaSuoniCannoni = new List<AudioSource>();
		this.ListaSuoniCannoni.Add(this.suonoCannone1);
		this.ListaSuoniCannoni.Add(this.suonoCannone2);
		this.ListaParticelleCannoni = new List<ParticleSystem>();
		this.ListaParticelleCannoni.Add(this.bocca1.GetComponent<ParticleSystem>());
		this.ListaParticelleCannoni.Add(this.bocca2.GetComponent<ParticleSystem>());
		if (GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.moltiplicatoreAttaccoInFPS = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().moltiplicatoreFPSBattVeloce;
		}
		else
		{
			this.moltiplicatoreAttaccoInFPS = PlayerPrefs.GetFloat("moltiplicatore danni PP");
		}
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.ritardoFrequenzaArma = UnityEngine.Random.Range(-this.intervalloDiRitardoFreqArma, this.intervalloDiRitardoFreqArma);
	}

	// Token: 0x060002DE RID: 734 RVA: 0x00076AF8 File Offset: 0x00074CF8
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
		this.GestioneCannoni();
		this.unitàBersaglio = base.GetComponent<PresenzaAlleato>().unitàBersaglio;
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.GestioneSuoniIndipendenti();
			this.PreparazioneAttacco();
			if (Input.GetKeyDown(KeyCode.Q) && this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0 && base.gameObject == this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0])
			{
				this.particelleBoccaTPS1.Stop();
				this.particelleBoccaTPS2.Stop();
				this.fiammaAttiva = false;
				this.suonoFlamethrower1.Stop();
				this.suonoFlamethrower2.Stop();
				this.suonoDuranteAvviato = false;
				this.suonoInizioAvviato = false;
			}
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
			base.GetComponent<MOV_BattleTank>().torretta.transform.rotation = base.transform.rotation;
			base.GetComponent<MOV_BattleTank>().cannoni1e2.transform.rotation = base.transform.rotation;
			base.GetComponent<MOV_BattleTank>().cannoni3e4.transform.rotation = base.transform.rotation;
			this.suonoTorretta.Stop();
			base.GetComponent<MOV_BattleTank>().suonoTorrettaPartito = false;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
			this.zoomAttivo = false;
		}
		if (this.unitàBersaglio == null && this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva != 3)
		{
			this.particelleBoccaTPS1.Stop();
			this.particelleBoccaTPS2.Stop();
			this.fiammaAttiva = false;
			this.suonoFlamethrower1.Stop();
			this.suonoFlamethrower2.Stop();
			this.suonoDuranteAvviato = false;
			this.suonoInizioAvviato = false;
		}
		if (this.suonoFlamethrower1.clip == this.suonoFiammaInizio && !this.suonoDuranteAvviato && this.suonoFlamethrower1.time > 0.47f)
		{
			this.suonoFlamethrower1.clip = this.suonoFiammaDurante;
			this.suonoFlamethrower2.clip = this.suonoFiammaDurante;
			this.suonoFlamethrower1.Play();
			this.suonoFlamethrower2.Play();
			this.suonoDuranteAvviato = true;
			this.suonoInizioAvviato = false;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x060002DF RID: 735 RVA: 0x00077058 File Offset: 0x00075258
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

	// Token: 0x060002E0 RID: 736 RVA: 0x00077218 File Offset: 0x00075418
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
					this.bocca1.transform.parent.transform.parent.GetComponent<AudioSource>().Play();
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

	// Token: 0x060002E1 RID: 737 RVA: 0x00077490 File Offset: 0x00075690
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
					this.suonoDuranteAvviato = false;
					this.suonoInizioAvviato = false;
					this.suonoFlamethrower1.clip = this.suonoRicarica;
					this.suonoFlamethrower2.clip = this.suonoRicarica;
					this.suonoFlamethrower1.loop = false;
					this.suonoFlamethrower2.loop = false;
					this.suonoFlamethrower1.Play();
					this.suonoFlamethrower2.Play();
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

	// Token: 0x060002E2 RID: 738 RVA: 0x00077740 File Offset: 0x00075940
	private void GestioneCannoni()
	{
		if (this.cannoneAttivo >= 2)
		{
			this.cannoneAttivo = 0;
		}
		for (int i = 0; i <= 1; i++)
		{
			if (this.ListaBoolCannoniSparati[i])
			{
				if (this.ListaCannoni[i].transform.localPosition.z > 1.1f && !this.ListaBoolCannoniInFondo[i])
				{
					this.ListaCannoni[i].transform.localPosition += -Vector3.forward * 30f * Time.deltaTime;
				}
				if (this.ListaCannoni[i].transform.localPosition.z <= 1.1f && !this.ListaBoolCannoniInFondo[i])
				{
					this.ListaBoolCannoniInFondo[i] = true;
				}
				if (this.ListaCannoni[i].transform.localPosition.z < 4.6f && this.ListaBoolCannoniInFondo[i])
				{
					this.ListaCannoni[i].transform.localPosition += Vector3.forward * 3f * Time.deltaTime;
				}
				if (this.ListaCannoni[i].transform.localPosition.z >= 4.6f && this.ListaBoolCannoniInFondo[i])
				{
					this.ListaBoolCannoniInFondo[i] = false;
					this.ListaBoolCannoniSparati[i] = false;
				}
			}
		}
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x00077908 File Offset: 0x00075B08
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

	// Token: 0x060002E4 RID: 740 RVA: 0x000779B4 File Offset: 0x00075BB4
	private void CameraTPS()
	{
		this.timerPosizionamentoTPS += Time.deltaTime;
		if (this.timerPosizionamentoTPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.cannoni1e2.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraTPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localEulerAngles = new Vector3(this.rotazioneCameraTPS.x, 0f, this.cannoni1e2.transform.eulerAngles.z);
		}
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x00077A60 File Offset: 0x00075C60
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.cannoni1e2.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = this.cannoni1e2.transform.rotation;
		}
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x00077AF0 File Offset: 0x00075CF0
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
								this.suonoFlamethrower1.Stop();
								this.suonoFlamethrower2.Stop();
								this.suonoDuranteAvviato = false;
								this.suonoInizioAvviato = false;
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
								this.cannoni1e2.transform.LookAt(this.centroUnitàBersaglio);
								this.cannoni3e4.transform.LookAt(this.centroUnitàBersaglio);
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
								this.cannoni1e2.transform.LookAt(this.centroUnitàBersaglio);
								this.cannoni3e4.transform.LookAt(this.centroUnitàBersaglio);
								this.alleatoNav.speed = 0f;
							}
							else
							{
								this.suonoFlamethrower1.Stop();
								this.suonoFlamethrower2.Stop();
								this.suonoDuranteAvviato = false;
								this.suonoInizioAvviato = false;
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
							this.suonoFlamethrower1.Stop();
							this.suonoFlamethrower2.Stop();
							this.suonoDuranteAvviato = false;
							this.suonoInizioAvviato = false;
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
							this.cannoni1e2.transform.LookAt(this.centroUnitàBersaglio);
							this.cannoni3e4.transform.LookAt(this.centroUnitàBersaglio);
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
							this.cannoni1e2.transform.LookAt(this.centroUnitàBersaglio);
							this.cannoni3e4.transform.LookAt(this.centroUnitàBersaglio);
							this.alleatoNav.speed = 0f;
						}
						else
						{
							this.suonoFlamethrower1.Stop();
							this.suonoFlamethrower2.Stop();
							this.suonoDuranteAvviato = false;
							this.suonoInizioAvviato = false;
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
			this.particelleBoccaTPS1.Stop();
			this.particelleBoccaTPS2.Stop();
			this.fiammaAttiva = false;
			this.suonoFlamethrower1.Stop();
			this.suonoFlamethrower2.Stop();
			this.suonoDuranteAvviato = false;
			this.suonoInizioAvviato = false;
		}
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x00078854 File Offset: 0x00076A54
	private void AttaccoIndipendente1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[0] + this.ritardoFrequenzaArma)
		{
			if (this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaAllPiùViciniPerTipo.Contains(base.gameObject) || this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] > 0.05f)
			{
				this.ListaSuoniCannoni[this.cannoneAttivo].Play();
				this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] = 0f;
			}
			this.timerFrequenzaArma1 = 0f;
			this.ritardoFrequenzaArma = UnityEngine.Random.Range(-this.intervalloDiRitardoFreqArma, this.intervalloDiRitardoFreqArma);
			this.ListaParticelleCannoni[this.cannoneAttivo].Play();
			List<float> listaValoriArma;
			List<float> expr_179 = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int index;
			int expr_17C = index = 5;
			float num = listaValoriArma[index];
			expr_179[expr_17C] = num - 1f;
			List<float> listaValoriArma2;
			List<float> expr_19D = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int expr_1A0 = index = 6;
			num = listaValoriArma2[index];
			expr_19D[expr_1A0] = num - 1f;
			this.SparoIndipendente1();
			this.timerDopoSparo1 = 0f;
			this.cannoneAttivo++;
		}
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x00078A38 File Offset: 0x00076C38
	private void SparoIndipendente1()
	{
		this.proiettileCarro = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.proiettileCarro.GetComponent<DatiProiettile>().locazioneTarget = this.centroUnitàBersaglio;
		this.ListaBoolCannoniSparati[this.cannoneAttivo] = true;
		this.proiettileCarro.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x00078AC0 File Offset: 0x00076CC0
	private void AttaccoIndipendente2()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[1])
		{
			if (this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f)
			{
				if (!Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale))
				{
					if (base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] > 0f)
					{
						if (!base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[1] && this.timerFrequenzaArma2 > base.GetComponent<PresenzaAlleato>().ListaArmi[1][0])
						{
							this.timerFrequenzaArma2 = 0f;
							List<float> list;
							List<float> expr_DB = list = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
							int index;
							int expr_DF = index = 5;
							float num = list[index];
							expr_DB[expr_DF] = num - 1f;
							List<float> list2;
							List<float> expr_10B = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
							int expr_10F = index = 6;
							num = list2[index];
							expr_10B[expr_10F] = num - 1f;
							this.timerDopoSparo2 = 0f;
							if (!this.fiammaAttiva)
							{
								this.particelleBoccaTPS1.Play();
								this.particelleBoccaTPS2.Play();
								this.fiammaAttiva = true;
								if (!this.suonoInizioAvviato)
								{
									this.suonoFlamethrower1.clip = this.suonoFiammaInizio;
									this.suonoFlamethrower2.clip = this.suonoFiammaInizio;
									this.suonoFlamethrower1.loop = true;
									this.suonoFlamethrower2.loop = true;
									this.suonoFlamethrower1.Play();
									this.suonoFlamethrower2.Play();
									this.suonoDuranteAvviato = false;
									this.suonoInizioAvviato = true;
								}
							}
							for (int i = 0; i < this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma.Count; i++)
							{
								if (this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[i] == null && i < this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma.Count - 1)
								{
									this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[i] = this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[i + 1];
									this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[i + 1] = null;
								}
							}
							for (int j = 0; j < this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma.Count; j++)
							{
								if (this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[j] == null)
								{
									this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma.RemoveAt(j);
								}
							}
							foreach (GameObject current in this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma)
							{
								if (current && current.tag == "Nemico")
								{
									float num2 = 0f;
									if (current.GetComponent<PresenzaNemico>().vita > this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().penetrazione)
									{
										num2 = this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().penetrazione;
									}
									else if (current.GetComponent<PresenzaNemico>().vita > 0f)
									{
										num2 = current.GetComponent<PresenzaNemico>().vita;
									}
									current.GetComponent<PresenzaNemico>().vita -= this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().penetrazione;
									if (current.GetComponent<PresenzaNemico>().vita > this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura))
									{
										num2 += this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura);
									}
									else if (current.GetComponent<PresenzaNemico>().vita > 0f)
									{
										num2 += current.GetComponent<PresenzaNemico>().vita;
									}
									current.GetComponent<PresenzaNemico>().vita -= this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura);
									List<float> listaDanniAlleati;
									List<float> expr_45A = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
									int expr_468 = index = base.GetComponent<PresenzaAlleato>().tipoTruppa;
									num = listaDanniAlleati[index];
									expr_45A[expr_468] = num + num2;
								}
							}
						}
					}
					else
					{
						this.particelleBoccaTPS1.Stop();
						this.particelleBoccaTPS2.Stop();
						this.fiammaAttiva = false;
						this.suonoDuranteAvviato = false;
						this.suonoInizioAvviato = false;
					}
				}
			}
			else
			{
				this.particelleBoccaTPS1.Stop();
				this.particelleBoccaTPS2.Stop();
				this.fiammaAttiva = false;
				this.suonoFlamethrower1.Stop();
				this.suonoFlamethrower2.Stop();
				this.suonoDuranteAvviato = false;
				this.suonoInizioAvviato = false;
			}
		}
		else
		{
			this.particelleBoccaTPS1.Stop();
			this.particelleBoccaTPS2.Stop();
			this.fiammaAttiva = false;
			this.suonoFlamethrower1.Stop();
			this.suonoFlamethrower2.Stop();
			this.suonoDuranteAvviato = false;
			this.suonoInizioAvviato = false;
		}
	}

	// Token: 0x060002EA RID: 746 RVA: 0x00079044 File Offset: 0x00077244
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

	// Token: 0x060002EB RID: 747 RVA: 0x00079084 File Offset: 0x00077284
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
		if (Input.GetMouseButton(0) && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][1])
		{
			this.timerFrequenzaArma1 = 0f;
			this.SparoArma1();
			this.ListaSuoniCannoni[this.cannoneAttivo].Play();
			this.ListaParticelleCannoni[this.cannoneAttivo].Play();
			List<float> list;
			List<float> expr_200 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int index;
			int expr_203 = index = 5;
			float num = list[index];
			expr_200[expr_203] = num - 1f;
			List<float> list2;
			List<float> expr_22A = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int expr_22E = index = 6;
			num = list2[index];
			expr_22A[expr_22E] = num - 1f;
			this.timerDopoSparo1 = 0f;
			this.cannoneAttivo++;
		}
	}

	// Token: 0x060002EC RID: 748 RVA: 0x000792F0 File Offset: 0x000774F0
	private void SparoArma1()
	{
		this.proiettileCarro = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.ListaBocche[this.cannoneAttivo].transform.position, this.ListaBocche[this.cannoneAttivo].transform.rotation) as GameObject);
		this.proiettileCarro.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.ListaBoolCannoniSparati[this.cannoneAttivo] = true;
		this.proiettileCarro.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060002ED RID: 749 RVA: 0x00079388 File Offset: 0x00077588
	private void AttaccoPrimaPersonaArma2()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo, 9999f, this.layerColpo))
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
		if (Input.GetMouseButton(0) && this.pulsanteFuocoPremuto)
		{
			if (base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] > 0f)
			{
				if (!base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[1] && this.timerFrequenzaArma2 > base.GetComponent<PresenzaAlleato>().ListaArmi[1][1])
				{
					this.timerFrequenzaArma2 = 0f;
					this.SparoArma2();
					List<float> list;
					List<float> expr_1AE = list = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
					int index;
					int expr_1B1 = index = 5;
					float num = list[index];
					expr_1AE[expr_1B1] = num - 1f;
					List<float> list2;
					List<float> expr_1D8 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
					int expr_1DC = index = 6;
					num = list2[index];
					expr_1D8[expr_1DC] = num - 1f;
					this.timerDopoSparo2 = 0f;
				}
			}
			else
			{
				this.particelleBoccaTPS1.Stop();
				this.particelleBoccaTPS2.Stop();
				this.fiammaAttiva = false;
				this.suonoDuranteAvviato = false;
				this.suonoInizioAvviato = false;
			}
		}
		if (Input.GetMouseButtonDown(0) && !this.fiammaAttiva && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[1])
		{
			this.pulsanteFuocoPremuto = true;
			this.particelleBoccaTPS1.Play();
			this.particelleBoccaTPS2.Play();
			this.fiammaAttiva = true;
			if (!this.suonoInizioAvviato)
			{
				this.suonoFlamethrower1.clip = this.suonoFiammaInizio;
				this.suonoFlamethrower2.clip = this.suonoFiammaInizio;
				this.suonoFlamethrower1.loop = true;
				this.suonoFlamethrower2.loop = true;
				this.suonoFlamethrower1.Play();
				this.suonoFlamethrower2.Play();
				this.suonoDuranteAvviato = false;
				this.suonoInizioAvviato = true;
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.particelleBoccaTPS1.Stop();
			this.particelleBoccaTPS2.Stop();
			this.fiammaAttiva = false;
			if (!base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[1])
			{
				this.suonoFlamethrower1.Stop();
				this.suonoFlamethrower2.Stop();
				this.suonoDuranteAvviato = false;
				this.suonoInizioAvviato = false;
			}
		}
		if (base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[1])
		{
			this.particelleBoccaTPS1.Stop();
			this.particelleBoccaTPS2.Stop();
		}
	}

	// Token: 0x060002EE RID: 750 RVA: 0x0007970C File Offset: 0x0007790C
	private void SparoArma2()
	{
		for (int i = 0; i < this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma.Count; i++)
		{
			if (this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[i] == null && i < this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma.Count - 1)
			{
				this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[i] = this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[i + 1];
				this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[i + 1] = null;
			}
		}
		for (int j = 0; j < this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma.Count; j++)
		{
			if (this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma[j] == null)
			{
				this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma.RemoveAt(j);
			}
		}
		foreach (GameObject current in this.cannoni3e4.GetComponent<ATT_FlamethrowerArma>().ListaEffettoFiamma)
		{
			if (current && current.tag == "Nemico")
			{
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
				float num = 0f;
				if (current.GetComponent<PresenzaNemico>().vita > this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
				{
					num = this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				}
				else if (current.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num = current.GetComponent<PresenzaNemico>().vita;
				}
				current.GetComponent<PresenzaNemico>().vita -= this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				if (current.GetComponent<PresenzaNemico>().vita > this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS)
				{
					num += this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
				}
				else if (current.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num += current.GetComponent<PresenzaNemico>().vita;
				}
				current.GetComponent<PresenzaNemico>().vita -= this.munizioneArma2.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
				List<float> listaDanniAlleati;
				List<float> expr_2D3 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int tipoTruppa;
				int expr_2E1 = tipoTruppa = base.GetComponent<PresenzaAlleato>().tipoTruppa;
				float num2 = listaDanniAlleati[tipoTruppa];
				expr_2D3[expr_2E1] = num2 + num;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num;
			}
		}
	}

	// Token: 0x04000C0A RID: 3082
	public float angVertMax;

	// Token: 0x04000C0B RID: 3083
	public float angVertMin;

	// Token: 0x04000C0C RID: 3084
	private GameObject infoNeutreTattica;

	// Token: 0x04000C0D RID: 3085
	private GameObject terzaCamera;

	// Token: 0x04000C0E RID: 3086
	private GameObject primaCamera;

	// Token: 0x04000C0F RID: 3087
	public GameObject bocca1;

	// Token: 0x04000C10 RID: 3088
	public GameObject bocca2;

	// Token: 0x04000C11 RID: 3089
	public GameObject bocca3;

	// Token: 0x04000C12 RID: 3090
	public GameObject bocca4;

	// Token: 0x04000C13 RID: 3091
	private GameObject IANemico;

	// Token: 0x04000C14 RID: 3092
	private GameObject InfoAlleati;

	// Token: 0x04000C15 RID: 3093
	private float timerFrequenzaArma1;

	// Token: 0x04000C16 RID: 3094
	private float timerRicarica1;

	// Token: 0x04000C17 RID: 3095
	private bool ricaricaInCorso1;

	// Token: 0x04000C18 RID: 3096
	private float tempoFraSparoERicarica1;

	// Token: 0x04000C19 RID: 3097
	private float timerDopoSparo1;

	// Token: 0x04000C1A RID: 3098
	private float ritardoFrequenzaArma;

	// Token: 0x04000C1B RID: 3099
	public float intervalloDiRitardoFreqArma;

	// Token: 0x04000C1C RID: 3100
	private float timerFrequenzaArma2;

	// Token: 0x04000C1D RID: 3101
	private float timerRicarica2;

	// Token: 0x04000C1E RID: 3102
	private bool ricaricaInCorso2;

	// Token: 0x04000C1F RID: 3103
	private float timerDopoSparo2;

	// Token: 0x04000C20 RID: 3104
	private float tempoFraSparoERicarica2;

	// Token: 0x04000C21 RID: 3105
	private int layerColpo;

	// Token: 0x04000C22 RID: 3106
	private int layerVisuale;

	// Token: 0x04000C23 RID: 3107
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000C24 RID: 3108
	public Vector3 rotazioneCameraTPS;

	// Token: 0x04000C25 RID: 3109
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000C26 RID: 3110
	private float timerPosizionamentoTPS;

	// Token: 0x04000C27 RID: 3111
	private float timerPosizionamentoFPS;

	// Token: 0x04000C28 RID: 3112
	private GameObject CanvasFPS;

	// Token: 0x04000C29 RID: 3113
	private GameObject mirinoElettr1;

	// Token: 0x04000C2A RID: 3114
	public Sprite mirinoTPS;

	// Token: 0x04000C2B RID: 3115
	public Sprite mirinoFPS;

	// Token: 0x04000C2C RID: 3116
	private RaycastHit targetSparo;

	// Token: 0x04000C2D RID: 3117
	private GameObject proiettileCarro;

	// Token: 0x04000C2E RID: 3118
	private NavMeshAgent alleatoNav;

	// Token: 0x04000C2F RID: 3119
	private float velocitàAlleatoNav;

	// Token: 0x04000C30 RID: 3120
	private GameObject cannoni1e2;

	// Token: 0x04000C31 RID: 3121
	private GameObject cannoni3e4;

	// Token: 0x04000C32 RID: 3122
	private GameObject unitàBersaglio;

	// Token: 0x04000C33 RID: 3123
	private Vector3 centroUnitàBersaglio;

	// Token: 0x04000C34 RID: 3124
	private GameObject munizioneArma1;

	// Token: 0x04000C35 RID: 3125
	private GameObject munizioneArma2;

	// Token: 0x04000C36 RID: 3126
	private AudioSource suonoTorretta;

	// Token: 0x04000C37 RID: 3127
	private AudioSource suonoInterno;

	// Token: 0x04000C38 RID: 3128
	private AudioSource suonoMotore;

	// Token: 0x04000C39 RID: 3129
	public AudioClip motoreFermo;

	// Token: 0x04000C3A RID: 3130
	public AudioClip motorePartenza;

	// Token: 0x04000C3B RID: 3131
	public AudioClip motoreViaggio;

	// Token: 0x04000C3C RID: 3132
	public AudioClip motoreStop;

	// Token: 0x04000C3D RID: 3133
	private float timerPartenza;

	// Token: 0x04000C3E RID: 3134
	private float timerStop;

	// Token: 0x04000C3F RID: 3135
	private bool primaPartenza;

	// Token: 0x04000C40 RID: 3136
	public float volumeMotoreIniziale;

	// Token: 0x04000C41 RID: 3137
	private bool inPartenza;

	// Token: 0x04000C42 RID: 3138
	private bool partenzaFinita;

	// Token: 0x04000C43 RID: 3139
	private bool inStop;

	// Token: 0x04000C44 RID: 3140
	public bool stopFinito;

	// Token: 0x04000C45 RID: 3141
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x04000C46 RID: 3142
	private AudioSource suonoFlamethrower1;

	// Token: 0x04000C47 RID: 3143
	private AudioSource suonoFlamethrower2;

	// Token: 0x04000C48 RID: 3144
	public AudioClip suonoFiammaInizio;

	// Token: 0x04000C49 RID: 3145
	public AudioClip suonoFiammaDurante;

	// Token: 0x04000C4A RID: 3146
	public AudioClip suonoRicarica;

	// Token: 0x04000C4B RID: 3147
	private bool suonoInizioAvviato;

	// Token: 0x04000C4C RID: 3148
	private bool suonoDuranteAvviato;

	// Token: 0x04000C4D RID: 3149
	private ParticleSystem particelleBoccaTPS1;

	// Token: 0x04000C4E RID: 3150
	private ParticleSystem particelleBoccaTPS2;

	// Token: 0x04000C4F RID: 3151
	private List<AudioSource> ListaSuoniCannoni;

	// Token: 0x04000C50 RID: 3152
	private AudioSource suonoCannone1;

	// Token: 0x04000C51 RID: 3153
	private AudioSource suonoCannone2;

	// Token: 0x04000C52 RID: 3154
	private List<GameObject> ListaBocche;

	// Token: 0x04000C53 RID: 3155
	private int cannoneAttivo;

	// Token: 0x04000C54 RID: 3156
	private List<GameObject> ListaCannoni;

	// Token: 0x04000C55 RID: 3157
	private List<bool> ListaBoolCannoniSparati;

	// Token: 0x04000C56 RID: 3158
	private bool cannone1Sparato;

	// Token: 0x04000C57 RID: 3159
	private bool cannone2Sparato;

	// Token: 0x04000C58 RID: 3160
	private List<bool> ListaBoolCannoniInFondo;

	// Token: 0x04000C59 RID: 3161
	private bool cannone1InFondo;

	// Token: 0x04000C5A RID: 3162
	private bool cannone2InFondo;

	// Token: 0x04000C5B RID: 3163
	private List<ParticleSystem> ListaParticelleCannoni;

	// Token: 0x04000C5C RID: 3164
	private ParticleSystem particelleCannone1;

	// Token: 0x04000C5D RID: 3165
	private ParticleSystem particelleCannone2;

	// Token: 0x04000C5E RID: 3166
	private bool fiammaAttiva;

	// Token: 0x04000C5F RID: 3167
	private bool pulsanteFuocoPremuto;

	// Token: 0x04000C60 RID: 3168
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x04000C61 RID: 3169
	private bool zoomAttivo;

	// Token: 0x04000C62 RID: 3170
	private float distFineOrdineMovimento;

	// Token: 0x04000C63 RID: 3171
	private float timerAggRicerca;
}
