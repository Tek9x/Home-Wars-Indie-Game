using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000043 RID: 67
public class ATT_ReconVehicle : MonoBehaviour
{
	// Token: 0x0600036A RID: 874 RVA: 0x0008CCA8 File Offset: 0x0008AEA8
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
		this.cannone = base.GetComponent<MOV_ReconVehicle>().cannoni;
		this.ListaMunizioniAttiveUnità = new List<GameObject>();
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma1);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma2);
		this.suonoTorretta = base.transform.GetChild(1).transform.GetChild(3).GetComponent<AudioSource>();
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
		if (GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.moltiplicatoreAttaccoInFPS = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().moltiplicatoreFPSBattVeloce;
		}
		else
		{
			this.moltiplicatoreAttaccoInFPS = PlayerPrefs.GetFloat("moltiplicatore danni PP");
		}
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.colpiBocca1 = this.bocca1.transform.GetChild(0).gameObject;
		this.particelleColpiBocca1 = this.bocca1.transform.GetChild(0).GetComponent<ParticleSystem>();
		this.colpiBocca2 = this.bocca2.transform.GetChild(0).gameObject;
		this.particelleColpiBocca2 = this.bocca2.transform.GetChild(0).GetComponent<ParticleSystem>();
	}

	// Token: 0x0600036B RID: 875 RVA: 0x0008CFAC File Offset: 0x0008B1AC
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
			base.GetComponent<MOV_ReconVehicle>().torretta.transform.rotation = base.transform.rotation;
			base.GetComponent<MOV_ReconVehicle>().cannoni.transform.rotation = base.transform.rotation;
			this.suonoTorretta.Stop();
			base.GetComponent<MOV_ReconVehicle>().suonoTorrettaPartito = false;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
			this.zoomAttivo = false;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x0600036C RID: 876 RVA: 0x0008D374 File Offset: 0x0008B574
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

	// Token: 0x0600036D RID: 877 RVA: 0x0008D534 File Offset: 0x0008B734
	private void GestioneRuote()
	{
		if (this.suonoMotore.clip == this.motorePartenza || this.suonoMotore.clip == this.motoreViaggio)
		{
			this.ruote1.transform.Rotate(Vector3.right * 6f);
			this.ruote2.transform.Rotate(Vector3.right * 6f);
		}
	}

	// Token: 0x0600036E RID: 878 RVA: 0x0008D5B8 File Offset: 0x0008B7B8
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

	// Token: 0x0600036F RID: 879 RVA: 0x0008D818 File Offset: 0x0008BA18
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

	// Token: 0x06000370 RID: 880 RVA: 0x0008DA78 File Offset: 0x0008BC78
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

	// Token: 0x06000371 RID: 881 RVA: 0x0008DB24 File Offset: 0x0008BD24
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

	// Token: 0x06000372 RID: 882 RVA: 0x0008DBD0 File Offset: 0x0008BDD0
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

	// Token: 0x06000373 RID: 883 RVA: 0x0008DC60 File Offset: 0x0008BE60
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
			else if (this.unitàBersaglio && this.alleatoNav.enabled)
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

	// Token: 0x06000374 RID: 884 RVA: 0x0008E864 File Offset: 0x0008CA64
	private void AttaccoIndipendente1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca2.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][0])
		{
			if (this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaAllPiùViciniPerTipo.Contains(base.gameObject) || this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] > 0.05f)
			{
				this.suonoArma1.Play();
				this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] = 0f;
			}
			this.colpiBocca1.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
			this.timerFrequenzaArma1 = 0f;
			this.bocca1.GetComponent<ParticleSystem>().Play();
			this.particelleColpiBocca1.Play();
			List<float> list;
			List<float> expr_1A4 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int index;
			int expr_1A7 = index = 5;
			float num = list[index];
			expr_1A4[expr_1A7] = num - 1f;
			List<float> list2;
			List<float> expr_1D0 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int expr_1D4 = index = 6;
			num = list2[index];
			expr_1D0[expr_1D4] = num - 1f;
			float num2 = 0f;
			if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().penetrazione)
			{
				num2 = this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().penetrazione;
			}
			else if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f)
			{
				num2 = this.unitàBersaglio.GetComponent<PresenzaNemico>().vita;
			}
			this.unitàBersaglio.GetComponent<PresenzaNemico>().vita -= this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().penetrazione;
			if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.unitàBersaglio.GetComponent<PresenzaNemico>().armatura))
			{
				num2 += this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.unitàBersaglio.GetComponent<PresenzaNemico>().armatura);
			}
			else if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f)
			{
				num2 += this.unitàBersaglio.GetComponent<PresenzaNemico>().vita;
			}
			this.unitàBersaglio.GetComponent<PresenzaNemico>().vita -= this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.unitàBersaglio.GetComponent<PresenzaNemico>().armatura);
			List<float> listaDanniAlleati;
			List<float> expr_366 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
			int expr_374 = index = base.GetComponent<PresenzaAlleato>().tipoTruppa;
			num = listaDanniAlleati[index];
			expr_366[expr_374] = num + num2;
		}
	}

	// Token: 0x06000375 RID: 885 RVA: 0x0008EBFC File Offset: 0x0008CDFC
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

	// Token: 0x06000376 RID: 886 RVA: 0x0008EF94 File Offset: 0x0008D194
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

	// Token: 0x06000377 RID: 887 RVA: 0x0008EFD4 File Offset: 0x0008D1D4
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
			List<float> expr_1CF = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int expr_1D3 = index = 6;
			num = list2[index];
			expr_1CF[expr_1D3] = num - 1f;
			this.bocca1.GetComponent<ParticleSystem>().Play();
			Vector3 normalized = (this.targetSparo.point - this.colpiBocca1.transform.position).normalized;
			this.colpiBocca1.transform.forward = normalized;
			this.particelleColpiBocca1.Play();
		}
	}

	// Token: 0x06000378 RID: 888 RVA: 0x0008F224 File Offset: 0x0008D424
	private void SparoArma1()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo, this.ListaMunizioniAttiveUnità[0].GetComponent<DatiGeneraliMunizione>().portataMassima, this.layerColpo))
		{
			if (this.targetSparo.collider.gameObject.tag == "Nemico")
			{
				GameObject gameObject = this.targetSparo.collider.gameObject;
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
				float num = 0f;
				if (gameObject.GetComponent<PresenzaNemico>().vita > this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
				{
					num = this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				}
				else if (gameObject.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num = gameObject.GetComponent<PresenzaNemico>().vita;
				}
				gameObject.GetComponent<PresenzaNemico>().vita -= this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				if (gameObject.GetComponent<PresenzaNemico>().vita > this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS)
				{
					num += this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
				}
				else if (gameObject.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num += gameObject.GetComponent<PresenzaNemico>().vita;
				}
				gameObject.GetComponent<PresenzaNemico>().vita -= this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
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
				if (gameObject2.GetComponent<PresenzaNemico>().vita > this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f)
				{
					num3 = this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f;
				}
				else if (gameObject2.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num3 = gameObject2.GetComponent<PresenzaNemico>().vita;
				}
				gameObject2.GetComponent<PresenzaNemico>().vita -= this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f;
				if (gameObject2.GetComponent<PresenzaNemico>().vita > this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject2.GetComponent<PresenzaNemico>().armatura) * 2f)
				{
					num3 += this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject2.GetComponent<PresenzaNemico>().armatura) * 2f;
				}
				else if (gameObject2.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num3 += gameObject2.GetComponent<PresenzaNemico>().vita;
				}
				gameObject2.GetComponent<PresenzaNemico>().vita -= this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject2.GetComponent<PresenzaNemico>().armatura) * 2f;
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
				if (gameObject3.GetComponent<PresenzaNemico>().vita > this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
				{
					num4 = this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				}
				else if (gameObject3.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num4 = gameObject3.GetComponent<PresenzaNemico>().vita;
				}
				gameObject3.GetComponent<PresenzaNemico>().vita -= this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				if (gameObject3.GetComponent<PresenzaNemico>().vita > this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject3.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS)
				{
					num4 += this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject3.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
				}
				else if (gameObject3.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num4 += gameObject3.GetComponent<PresenzaNemico>().vita;
				}
				gameObject3.GetComponent<PresenzaNemico>().vita -= this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject3.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
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

	// Token: 0x06000379 RID: 889 RVA: 0x0008F87C File Offset: 0x0008DA7C
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

	// Token: 0x0600037A RID: 890 RVA: 0x0008FACC File Offset: 0x0008DCCC
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

	// Token: 0x04000E5C RID: 3676
	public float angVertMax;

	// Token: 0x04000E5D RID: 3677
	public float angVertMin;

	// Token: 0x04000E5E RID: 3678
	private GameObject infoNeutreTattica;

	// Token: 0x04000E5F RID: 3679
	private GameObject terzaCamera;

	// Token: 0x04000E60 RID: 3680
	private GameObject primaCamera;

	// Token: 0x04000E61 RID: 3681
	public GameObject bocca1;

	// Token: 0x04000E62 RID: 3682
	public GameObject bocca2;

	// Token: 0x04000E63 RID: 3683
	private GameObject colpiBocca1;

	// Token: 0x04000E64 RID: 3684
	private GameObject colpiBocca2;

	// Token: 0x04000E65 RID: 3685
	private GameObject IANemico;

	// Token: 0x04000E66 RID: 3686
	private GameObject InfoAlleati;

	// Token: 0x04000E67 RID: 3687
	private float timerFrequenzaArma1;

	// Token: 0x04000E68 RID: 3688
	private float timerRicarica1;

	// Token: 0x04000E69 RID: 3689
	private bool ricaricaInCorso1;

	// Token: 0x04000E6A RID: 3690
	private float timerDopoSparo1;

	// Token: 0x04000E6B RID: 3691
	private float tempoFraSparoERicarica1;

	// Token: 0x04000E6C RID: 3692
	private float timerFrequenzaArma2;

	// Token: 0x04000E6D RID: 3693
	private float timerRicarica2;

	// Token: 0x04000E6E RID: 3694
	private bool ricaricaInCorso2;

	// Token: 0x04000E6F RID: 3695
	private float timerDopoSparo2;

	// Token: 0x04000E70 RID: 3696
	private float tempoFraSparoERicarica2;

	// Token: 0x04000E71 RID: 3697
	private int layerColpo;

	// Token: 0x04000E72 RID: 3698
	private int layerVisuale;

	// Token: 0x04000E73 RID: 3699
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000E74 RID: 3700
	public Vector3 rotazioneCameraTPS;

	// Token: 0x04000E75 RID: 3701
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000E76 RID: 3702
	private float timerPosizionamentoTPS;

	// Token: 0x04000E77 RID: 3703
	private float timerPosizionamentoFPS;

	// Token: 0x04000E78 RID: 3704
	private GameObject CanvasFPS;

	// Token: 0x04000E79 RID: 3705
	private GameObject mirinoElettr1;

	// Token: 0x04000E7A RID: 3706
	public Sprite mirinoTPS;

	// Token: 0x04000E7B RID: 3707
	public Sprite mirinoFPS;

	// Token: 0x04000E7C RID: 3708
	private RaycastHit targetSparo;

	// Token: 0x04000E7D RID: 3709
	private GameObject proiettileCarro;

	// Token: 0x04000E7E RID: 3710
	private NavMeshAgent alleatoNav;

	// Token: 0x04000E7F RID: 3711
	private float velocitàAlleatoNav;

	// Token: 0x04000E80 RID: 3712
	private GameObject cannone;

	// Token: 0x04000E81 RID: 3713
	private GameObject unitàBersaglio;

	// Token: 0x04000E82 RID: 3714
	private Vector3 centroUnitàBersaglio;

	// Token: 0x04000E83 RID: 3715
	private GameObject munizioneArma1;

	// Token: 0x04000E84 RID: 3716
	private GameObject munizioneArma2;

	// Token: 0x04000E85 RID: 3717
	private AudioSource suonoTorretta;

	// Token: 0x04000E86 RID: 3718
	private AudioSource suonoInterno;

	// Token: 0x04000E87 RID: 3719
	private AudioSource suonoMotore;

	// Token: 0x04000E88 RID: 3720
	public AudioClip motoreFermo;

	// Token: 0x04000E89 RID: 3721
	public AudioClip motorePartenza;

	// Token: 0x04000E8A RID: 3722
	public AudioClip motoreViaggio;

	// Token: 0x04000E8B RID: 3723
	public AudioClip motoreStop;

	// Token: 0x04000E8C RID: 3724
	private float timerPartenza;

	// Token: 0x04000E8D RID: 3725
	private float timerStop;

	// Token: 0x04000E8E RID: 3726
	private bool primaPartenza;

	// Token: 0x04000E8F RID: 3727
	public float volumeMotoreIniziale;

	// Token: 0x04000E90 RID: 3728
	private bool inPartenza;

	// Token: 0x04000E91 RID: 3729
	private bool partenzaFinita;

	// Token: 0x04000E92 RID: 3730
	private bool inStop;

	// Token: 0x04000E93 RID: 3731
	public bool stopFinito;

	// Token: 0x04000E94 RID: 3732
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x04000E95 RID: 3733
	private AudioSource suonoArma1;

	// Token: 0x04000E96 RID: 3734
	private AudioSource suonoArma2;

	// Token: 0x04000E97 RID: 3735
	public AudioSource suonoRicarica1;

	// Token: 0x04000E98 RID: 3736
	public AudioSource suonoRicarica2;

	// Token: 0x04000E99 RID: 3737
	private GameObject ruote1;

	// Token: 0x04000E9A RID: 3738
	private GameObject ruote2;

	// Token: 0x04000E9B RID: 3739
	private bool zoomAttivo;

	// Token: 0x04000E9C RID: 3740
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x04000E9D RID: 3741
	private float distFineOrdineMovimento;

	// Token: 0x04000E9E RID: 3742
	private float timerAggRicerca;

	// Token: 0x04000E9F RID: 3743
	private ParticleSystem particelleColpiBocca1;

	// Token: 0x04000EA0 RID: 3744
	private ParticleSystem particelleColpiBocca2;
}
