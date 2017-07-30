using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000044 RID: 68
public class ATT_RocketArtillery : MonoBehaviour
{
	// Token: 0x0600037C RID: 892 RVA: 0x0009012C File Offset: 0x0008E32C
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
		this.alleatoNav = base.GetComponent<NavMeshAgent>();
		this.velocitàAlleatoNav = base.GetComponent<NavMeshAgent>().speed;
		this.layerColpo = 165120;
		this.layerVisuale = 256;
		this.ListaMunizioniAttiveUnità = new List<GameObject>();
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma1);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma2);
		this.suonoTorretta = base.transform.GetChild(1).transform.GetChild(1).GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.volumeMotoreIniziale = this.suonoMotore.volume;
		this.suonoMotore.clip = this.motoreFermo;
		this.suonoMotore.Play();
		this.ListaGruppiOrdigniAttivi = new List<bool>();
		this.valoreAngolo = -40f;
		this.AngMinPP = base.GetComponent<MOV_RocketArtillery>().angCannoniVertMin;
		this.AngMaxPP = base.GetComponent<MOV_RocketArtillery>().angCannoniVertMax;
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.gruppo0Attivo = true;
	}

	// Token: 0x0600037D RID: 893 RVA: 0x00090358 File Offset: 0x0008E558
	private void Update()
	{
		this.ListaMunizioniAttiveUnità[0] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[0][0];
		this.ListaMunizioniAttiveUnità[1] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[1][0];
		this.munizioneArma1 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0];
		this.munizioneArma2 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[1];
		this.CreazioneOrdigniConRifornimento();
		this.CondizioniArma1();
		this.CondizioniArma2();
		if (!this.primoFrameAvvenuto)
		{
			this.CreazioneInizialeOrdigni();
			this.primoFrameAvvenuto = true;
		}
		else if (!this.ordigniFisiciAssegnati)
		{
			this.ordignoFisico0 = this.baseLanciamissili.transform.GetChild(1).gameObject;
			this.ordignoFisico1 = this.baseLanciamissili.transform.GetChild(2).gameObject;
			this.ordigniFisiciAssegnati = true;
		}
		this.timerDiLancio += Time.deltaTime;
		this.unitàBersaglio = base.GetComponent<PresenzaAlleato>().unitàBersaglio;
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.GestioneSuoniIndipendenti();
			this.PreparazioneAttacco();
		}
		else
		{
			this.GestioneVisuali();
			this.SelezioneArma();
			this.AlzoLanciamissiliPrimaPersona();
			this.GestioneOrdigniPrimaPersona();
			if (this.ordignoDaLanciare != null)
			{
				this.SistemaDiLancioInPrimaPersona();
			}
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
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
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
			base.GetComponent<MOV_RocketArtillery>().torretta.transform.rotation = base.transform.rotation;
			base.GetComponent<MOV_RocketArtillery>().lanciamissili.transform.rotation = base.transform.rotation;
			this.suonoTorretta.Stop();
			base.GetComponent<MOV_RocketArtillery>().suonoTorrettaPartito = false;
			this.angolazioneMortairTPS.GetComponent<CanvasGroup>().alpha = 0f;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
			this.baseLanciamissili.transform.localEulerAngles = new Vector3(-40f, 0f, 0f);
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
		if (Input.GetKeyDown(KeyCode.H))
		{
			this.rafficaAttiva = !this.rafficaAttiva;
		}
	}

	// Token: 0x0600037E RID: 894 RVA: 0x00090718 File Offset: 0x0008E918
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

	// Token: 0x0600037F RID: 895 RVA: 0x000908CC File Offset: 0x0008EACC
	private void CreazioneInizialeOrdigni()
	{
		for (int i = 0; i < base.GetComponent<PresenzaAlleato>().numeroCoppieOrdigni; i++)
		{
			this.ListaOrdigniAttiviLocale[i] = base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[i];
		}
		this.ordigno0 = (UnityEngine.Object.Instantiate(base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[0], base.transform.position, base.transform.rotation) as GameObject);
		this.ordigno0.transform.parent = this.baseLanciamissili.transform;
		this.ordigno0.transform.localPosition = this.posizioneOrdigni0;
		this.ordigno0.transform.localRotation = Quaternion.Euler(Vector3.zero);
		this.ordigno1 = (UnityEngine.Object.Instantiate(base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[1], base.transform.position, base.transform.rotation) as GameObject);
		this.ordigno1.transform.parent = this.baseLanciamissili.transform;
		this.ordigno1.transform.localPosition = this.posizioneOrdigni1;
		this.ordigno1.transform.localRotation = Quaternion.Euler(Vector3.zero);
		this.ListaOrdigniLocali = new List<GameObject>();
		this.ListaOrdigniLocali.Add(this.ordigno0);
		this.ListaOrdigniLocali.Add(this.ordigno1);
		for (int j = 0; j < base.GetComponent<PresenzaAlleato>().numeroCoppieOrdigni; j++)
		{
			int num = 0;
			while ((float)num < base.GetComponent<PresenzaAlleato>().ListaArmi[j][5])
			{
				this.ListaOrdigniLocali[j].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[num] = (UnityEngine.Object.Instantiate(this.ListaOrdigniLocali[j].GetComponent<DatiOrdignoEsterno>().munizioneUsata, base.transform.position, base.transform.rotation) as GameObject);
				this.ListaOrdigniLocali[j].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[num].transform.parent = this.ListaOrdigniLocali[j].transform;
				this.ListaOrdigniLocali[j].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[num].transform.localPosition = this.ListaOrdigniLocali[j].GetComponent<DatiOrdignoEsterno>().ListaPosizioniMunizioni[num];
				this.ListaOrdigniLocali[j].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[num].transform.localRotation = Quaternion.Euler(Vector3.zero);
				num++;
			}
		}
	}

	// Token: 0x06000380 RID: 896 RVA: 0x00090B84 File Offset: 0x0008ED84
	private void CreazioneOrdigniConRifornimento()
	{
		if (base.GetComponent<PresenzaAlleato>().reintegrazioneNecessaria)
		{
			for (int i = 0; i < base.GetComponent<PresenzaAlleato>().numeroCoppieOrdigni; i++)
			{
				for (int j = 0; j < base.GetComponent<PresenzaAlleato>().ListaNumReintegrazioniOrdigni[i]; j++)
				{
					for (int k = 0; k < this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche.Count; k++)
					{
						if (this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k] == null)
						{
							this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k] = (UnityEngine.Object.Instantiate(this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().munizioneUsata, base.transform.position, base.transform.rotation) as GameObject);
							this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k].transform.parent = this.ListaOrdigniLocali[i].transform;
							this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k].transform.localPosition = this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ListaPosizioniMunizioni[k];
							this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k].transform.localRotation = Quaternion.Euler(Vector3.zero);
							break;
						}
					}
				}
			}
			base.GetComponent<PresenzaAlleato>().reintegrazioneNecessaria = false;
		}
	}

	// Token: 0x06000381 RID: 897 RVA: 0x00090D40 File Offset: 0x0008EF40
	private void CondizioniArma1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[0][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[0][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[0][6];
		}
	}

	// Token: 0x06000382 RID: 898 RVA: 0x00090DB0 File Offset: 0x0008EFB0
	private void CondizioniArma2()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[1][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[1][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[1][6];
		}
	}

	// Token: 0x06000383 RID: 899 RVA: 0x00090E20 File Offset: 0x0008F020
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

	// Token: 0x06000384 RID: 900 RVA: 0x00090FE8 File Offset: 0x0008F1E8
	private void CameraTPS()
	{
		this.timerPosizionamentoTPS += Time.deltaTime;
		if (this.timerPosizionamentoTPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.baseTorretta.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraTPS;
		}
	}

	// Token: 0x06000385 RID: 901 RVA: 0x00091054 File Offset: 0x0008F254
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.baseTorretta.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
		}
	}

	// Token: 0x06000386 RID: 902 RVA: 0x000910C0 File Offset: 0x0008F2C0
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
							if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f)
							{
								if (this.ListaOrdigniLocali[0].transform.childCount > 1 && this.ListaOrdigniLocali[0].transform.GetChild(1) != null)
								{
									this.ordignoDaLanciare = this.ListaOrdigniLocali[0].transform.GetChild(1).gameObject;
									this.numArmaOrdignoDaLanciare = 0;
									this.SparoIndipendente();
									this.AttaccoIndipendenteOrdigni();
								}
							}
							else if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[1] && base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] > 0f && this.ListaOrdigniLocali[1].transform.childCount > 1 && this.ListaOrdigniLocali[1].transform.GetChild(1) != null)
							{
								this.ordignoDaLanciare = this.ListaOrdigniLocali[1].transform.GetChild(1).gameObject;
								this.numArmaOrdignoDaLanciare = 1;
								this.SparoIndipendente();
								this.AttaccoIndipendenteOrdigni();
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
					if (num5 > portataMassima)
					{
						if (!base.GetComponent<PresenzaAlleato>().comportamentoDifensivo && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino)
						{
							this.alleatoNav.SetDestination(this.unitàBersaglio.transform.position);
							this.alleatoNav.speed = this.velocitàAlleatoNav;
						}
					}
					else if (num5 > portataMinima && num5 < portataMassima)
					{
						base.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
						this.alleatoNav.speed = 0f;
						if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f)
						{
							if (this.ListaOrdigniLocali[0].transform.childCount > 1 && this.ListaOrdigniLocali[0].transform.GetChild(1) != null)
							{
								this.ordignoDaLanciare = this.ListaOrdigniLocali[0].transform.GetChild(1).gameObject;
								this.numArmaOrdignoDaLanciare = 0;
								this.SparoIndipendente();
								this.AttaccoIndipendenteOrdigni();
							}
						}
						else if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[1] && base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] > 0f && this.ListaOrdigniLocali[1].transform.childCount > 1 && this.ListaOrdigniLocali[1].transform.GetChild(1) != null)
						{
							this.ordignoDaLanciare = this.ListaOrdigniLocali[1].transform.GetChild(1).gameObject;
							this.numArmaOrdignoDaLanciare = 1;
							this.SparoIndipendente();
							this.AttaccoIndipendenteOrdigni();
						}
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
					if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f)
					{
						if (this.ListaOrdigniLocali[0].transform.childCount > 1 && this.ListaOrdigniLocali[0].transform.GetChild(1) != null)
						{
							this.ordignoDaLanciare = this.ListaOrdigniLocali[0].transform.GetChild(1).gameObject;
							this.numArmaOrdignoDaLanciare = 0;
							this.SparoIndipendenteZona();
							this.AttaccoIndipendenteOrdigni();
						}
					}
					else if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[1] && base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] > 0f && this.ListaOrdigniLocali[1].transform.childCount > 1 && this.ListaOrdigniLocali[1].transform.GetChild(1) != null)
					{
						this.ordignoDaLanciare = this.ListaOrdigniLocali[1].transform.GetChild(1).gameObject;
						this.numArmaOrdignoDaLanciare = 1;
						this.SparoIndipendenteZona();
						this.AttaccoIndipendenteOrdigni();
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

	// Token: 0x06000387 RID: 903 RVA: 0x00091FC8 File Offset: 0x000901C8
	private void SparoIndipendente()
	{
		float num = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
		this.ampiezzaCerchioPrecisione = base.GetComponent<PresenzaAlleato>().valoreInizialePrecisione + num / base.GetComponent<PresenzaAlleato>().valorePerditaPrecisione;
		GestoreNeutroStrategia.valoreRandomSeed++;
		UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
		float num2 = (float)UnityEngine.Random.Range(-1, 1) * this.ampiezzaCerchioPrecisione;
		float num3 = (float)UnityEngine.Random.Range(-1, 1) * this.ampiezzaCerchioPrecisione;
		Vector3 zonaTarget = new Vector3(this.centroUnitàBersaglio.x + num2, this.centroUnitàBersaglio.y, this.centroUnitàBersaglio.z + num3);
		this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().zonaTarget = zonaTarget;
	}

	// Token: 0x06000388 RID: 904 RVA: 0x00092088 File Offset: 0x00090288
	private void SparoIndipendenteZona()
	{
		Vector3 luogoAttZonaArt = base.GetComponent<PresenzaAlleato>().luogoAttZonaArt;
		float num = Vector3.Distance(base.transform.position, luogoAttZonaArt);
		this.ampiezzaCerchioPrecisione = base.GetComponent<PresenzaAlleato>().valoreInizialePrecisione + num / base.GetComponent<PresenzaAlleato>().valorePerditaPrecisione;
		GestoreNeutroStrategia.valoreRandomSeed++;
		UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
		float num2 = UnityEngine.Random.Range(-1f, 1f) * this.ampiezzaCerchioPrecisione;
		float num3 = UnityEngine.Random.Range(-1f, 1f) * this.ampiezzaCerchioPrecisione;
		Vector3 zonaTarget = new Vector3(luogoAttZonaArt.x + num2, luogoAttZonaArt.y, luogoAttZonaArt.z + num3);
		this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().zonaTarget = zonaTarget;
	}

	// Token: 0x06000389 RID: 905 RVA: 0x00092154 File Offset: 0x00090354
	private void AttaccoIndipendenteOrdigni()
	{
		if (!this.rafficaAttiva)
		{
			this.frequenzaDiFuoco = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][0];
		}
		else
		{
			this.frequenzaDiFuoco = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][0] / 4f;
		}
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][5] > 0f && this.timerDiLancio > this.frequenzaDiFuoco)
		{
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = false;
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().bersaglio = this.unitàBersaglio;
			List<float> list;
			List<float> expr_E2 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
			int index;
			int expr_E5 = index = 5;
			float num = list[index];
			expr_E2[expr_E5] = num - 1f;
			List<float> list2;
			List<float> expr_111 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
			int expr_115 = index = 6;
			num = list2[index];
			expr_111[expr_115] = num - 1f;
			this.timerDiLancio = 0f;
			for (int i = 0; i < this.ListaOrdigniLocali.Count; i++)
			{
				this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ordignoAttivo = false;
			}
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().ordignoAttivo = true;
			this.ordignoDaLanciare = null;
		}
	}

	// Token: 0x0600038A RID: 906 RVA: 0x000922F0 File Offset: 0x000904F0
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

	// Token: 0x0600038B RID: 907 RVA: 0x00092330 File Offset: 0x00090530
	private void GestioneOrdigniPrimaPersona()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.gruppo0Attivo = true;
			this.gruppo1Attivo = false;
			this.ordignoDaLanciare = null;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.gruppo0Attivo = false;
			this.gruppo1Attivo = true;
			this.ordignoDaLanciare = null;
		}
		this.ListaGruppiOrdigniAttivi.Clear();
		this.ListaGruppiOrdigniAttivi.Add(this.gruppo0Attivo);
		this.ListaGruppiOrdigniAttivi.Add(this.gruppo1Attivo);
		int num = 0;
		if (this.ordignoDaLanciare == null)
		{
			for (int i = 0; i < this.ListaGruppiOrdigniAttivi.Count; i++)
			{
				if (this.ListaGruppiOrdigniAttivi[i])
				{
					if (this.ListaOrdigniLocali[i].transform.childCount > 1 && this.ListaOrdigniLocali[i].transform.GetChild(1) != null)
					{
						this.ordignoDaLanciare = this.ListaOrdigniLocali[i].transform.GetChild(1).gameObject;
						this.numArmaOrdignoDaLanciare = i;
						break;
					}
				}
				else
				{
					num++;
				}
			}
		}
		if (num == this.ListaGruppiOrdigniAttivi.Count)
		{
			this.ordignoDaLanciare = null;
		}
	}

	// Token: 0x0600038C RID: 908 RVA: 0x0009247C File Offset: 0x0009067C
	private void AlzoLanciamissiliPrimaPersona()
	{
		this.baseLanciamissili.transform.localEulerAngles = new Vector3(this.valoreAngolo, 0f, 0f);
	}

	// Token: 0x0600038D RID: 909 RVA: 0x000924A4 File Offset: 0x000906A4
	private void SistemaDiLancioInPrimaPersona()
	{
		if (Input.GetMouseButton(0) && base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][5] > 0f && this.timerDiLancio > base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][1])
		{
			this.timerDiLancio = 0f;
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = true;
			List<float> list;
			List<float> expr_94 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
			int index;
			int expr_97 = index = 5;
			float num = list[index];
			expr_94[expr_97] = num - 1f;
			List<float> list2;
			List<float> expr_C3 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
			int expr_C7 = index = 6;
			num = list2[index];
			expr_C3[expr_C7] = num - 1f;
			for (int i = 0; i < this.ListaOrdigniLocali.Count; i++)
			{
				this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ordignoAttivo = false;
			}
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().ordignoAttivo = true;
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = true;
			this.ordignoDaLanciare = null;
		}
	}

	// Token: 0x04000EA1 RID: 3745
	public float angVertMax;

	// Token: 0x04000EA2 RID: 3746
	public float angVertMin;

	// Token: 0x04000EA3 RID: 3747
	private GameObject infoNeutreTattica;

	// Token: 0x04000EA4 RID: 3748
	private GameObject terzaCamera;

	// Token: 0x04000EA5 RID: 3749
	private GameObject primaCamera;

	// Token: 0x04000EA6 RID: 3750
	private GameObject IANemico;

	// Token: 0x04000EA7 RID: 3751
	private GameObject InfoAlleati;

	// Token: 0x04000EA8 RID: 3752
	public GameObject baseTorretta;

	// Token: 0x04000EA9 RID: 3753
	public GameObject baseLanciamissili;

	// Token: 0x04000EAA RID: 3754
	private int layerColpo;

	// Token: 0x04000EAB RID: 3755
	private int layerVisuale;

	// Token: 0x04000EAC RID: 3756
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000EAD RID: 3757
	public Vector3 rotazioneCameraTPS;

	// Token: 0x04000EAE RID: 3758
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000EAF RID: 3759
	private float timerPosizionamentoTPS;

	// Token: 0x04000EB0 RID: 3760
	private float timerPosizionamentoFPS;

	// Token: 0x04000EB1 RID: 3761
	private GameObject CanvasFPS;

	// Token: 0x04000EB2 RID: 3762
	private GameObject angolazioneMortairTPS;

	// Token: 0x04000EB3 RID: 3763
	private GameObject angolazioneMortairTPSLancetta;

	// Token: 0x04000EB4 RID: 3764
	private GameObject angolazioneTesto;

	// Token: 0x04000EB5 RID: 3765
	private float valoreAngolo;

	// Token: 0x04000EB6 RID: 3766
	private RaycastHit targetSparo;

	// Token: 0x04000EB7 RID: 3767
	private NavMeshAgent alleatoNav;

	// Token: 0x04000EB8 RID: 3768
	private float velocitàAlleatoNav;

	// Token: 0x04000EB9 RID: 3769
	private GameObject unitàBersaglio;

	// Token: 0x04000EBA RID: 3770
	private Vector3 centroUnitàBersaglio;

	// Token: 0x04000EBB RID: 3771
	private GameObject munizioneArma1;

	// Token: 0x04000EBC RID: 3772
	private GameObject munizioneArma2;

	// Token: 0x04000EBD RID: 3773
	private AudioSource suonoTorretta;

	// Token: 0x04000EBE RID: 3774
	private AudioSource suonoMotore;

	// Token: 0x04000EBF RID: 3775
	public AudioClip motoreFermo;

	// Token: 0x04000EC0 RID: 3776
	public AudioClip motorePartenza;

	// Token: 0x04000EC1 RID: 3777
	public AudioClip motoreViaggio;

	// Token: 0x04000EC2 RID: 3778
	public AudioClip motoreStop;

	// Token: 0x04000EC3 RID: 3779
	private float timerPartenza;

	// Token: 0x04000EC4 RID: 3780
	private float timerStop;

	// Token: 0x04000EC5 RID: 3781
	private bool primaPartenza;

	// Token: 0x04000EC6 RID: 3782
	public float volumeMotoreIniziale;

	// Token: 0x04000EC7 RID: 3783
	private bool inPartenza;

	// Token: 0x04000EC8 RID: 3784
	private bool partenzaFinita;

	// Token: 0x04000EC9 RID: 3785
	private bool inStop;

	// Token: 0x04000ECA RID: 3786
	public bool stopFinito;

	// Token: 0x04000ECB RID: 3787
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x04000ECC RID: 3788
	private List<GameObject> ListaOrdigniLocali;

	// Token: 0x04000ECD RID: 3789
	private GameObject ordignoDaLanciare;

	// Token: 0x04000ECE RID: 3790
	private int numArmaOrdignoDaLanciare;

	// Token: 0x04000ECF RID: 3791
	private GameObject ordigno0;

	// Token: 0x04000ED0 RID: 3792
	private GameObject ordigno1;

	// Token: 0x04000ED1 RID: 3793
	public Vector3 posizioneOrdigni0;

	// Token: 0x04000ED2 RID: 3794
	public Vector3 posizioneOrdigni1;

	// Token: 0x04000ED3 RID: 3795
	private bool gruppo0Attivo;

	// Token: 0x04000ED4 RID: 3796
	private bool gruppo1Attivo;

	// Token: 0x04000ED5 RID: 3797
	public List<GameObject> ListaOrdigniAttiviLocale;

	// Token: 0x04000ED6 RID: 3798
	private List<bool> ListaGruppiOrdigniAttivi;

	// Token: 0x04000ED7 RID: 3799
	private bool primoFrameAvvenuto;

	// Token: 0x04000ED8 RID: 3800
	public List<GameObject> ListaBersPPPossibili;

	// Token: 0x04000ED9 RID: 3801
	public GameObject bersaglioInPP;

	// Token: 0x04000EDA RID: 3802
	private bool bersaglioèAvantiInPP;

	// Token: 0x04000EDB RID: 3803
	private bool bersDavantiEAPortata;

	// Token: 0x04000EDC RID: 3804
	private float timerDiLancio;

	// Token: 0x04000EDD RID: 3805
	private GameObject ordignoFisico0;

	// Token: 0x04000EDE RID: 3806
	private GameObject ordignoFisico1;

	// Token: 0x04000EDF RID: 3807
	private bool ordigniFisiciAssegnati;

	// Token: 0x04000EE0 RID: 3808
	private float distFineOrdineMovimento;

	// Token: 0x04000EE1 RID: 3809
	private float AngMinPP;

	// Token: 0x04000EE2 RID: 3810
	private float AngMaxPP;

	// Token: 0x04000EE3 RID: 3811
	public float ampiezzaCerchioPrecisione;

	// Token: 0x04000EE4 RID: 3812
	public bool rafficaAttiva;

	// Token: 0x04000EE5 RID: 3813
	private float frequenzaDiFuoco;

	// Token: 0x04000EE6 RID: 3814
	private float timerAggRicerca;
}
