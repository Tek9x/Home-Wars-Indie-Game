using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003D RID: 61
public class ATT_HeavyFightingVehicle : MonoBehaviour
{
	// Token: 0x060002FF RID: 767 RVA: 0x0007BB58 File Offset: 0x00079D58
	private void Start()
	{
		this.CanvasFPS = GameObject.FindGameObjectWithTag("CanvasFPS");
		this.mirinoElettr1 = this.CanvasFPS.transform.GetChild(2).transform.GetChild(5).gameObject;
		this.sensoreRaggioMirino = this.CanvasFPS.transform.GetChild(2).transform.GetChild(2).gameObject;
		this.sensoreRaggioMirinoMobile = this.CanvasFPS.transform.GetChild(2).transform.GetChild(4).gameObject;
		this.mirinoMissiliFisso = this.CanvasFPS.transform.GetChild(2).transform.GetChild(1).gameObject;
		this.mirinoMissiliMobile = this.CanvasFPS.transform.GetChild(2).transform.GetChild(3).gameObject;
		this.mirinoMissiliFiloguidati = this.CanvasFPS.transform.GetChild(2).transform.GetChild(8).gameObject;
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.InfoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.alleatoNav = base.GetComponent<NavMeshAgent>();
		this.velocitàAlleatoNav = base.GetComponent<NavMeshAgent>().speed;
		this.layerColpo = 165120;
		this.layerVisuale = 256;
		this.cannone = base.GetComponent<MOV_HeavyFightingVehicle>().cannoni;
		this.ListaMunizioniAttiveUnità = new List<GameObject>();
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma1);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma2);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma3);
		this.suonoTorretta = base.transform.GetChild(1).transform.GetChild(1).GetComponent<AudioSource>();
		this.suonoInterno = base.transform.GetChild(1).GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.volumeMotoreIniziale = this.suonoMotore.volume;
		this.suonoMotore.clip = this.motoreFermo;
		this.suonoMotore.Play();
		this.suonoBeep = base.transform.GetChild(0).GetComponent<AudioSource>();
		this.tempoFraSparoERicarica = 0.4f;
		this.ListaGruppiOrdigniAttivi = new List<bool>();
		this.coloreBaseMirini = this.mirinoElettr1.GetComponent<Image>().color;
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
	}

	// Token: 0x06000300 RID: 768 RVA: 0x0007BE18 File Offset: 0x0007A018
	private void Update()
	{
		this.ListaMunizioniAttiveUnità[0] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[0][0];
		this.ListaMunizioniAttiveUnità[1] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[1][0];
		this.ListaMunizioniAttiveUnità[2] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[2][0];
		this.munizioneArma1 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0];
		this.munizioneArma2 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[1];
		this.munizioneArma3 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[2];
		this.CreazioneOrdigniConRifornimento();
		this.CondizioniArma1();
		this.CondizioniArma2();
		this.CondizioniArma3();
		if (!this.primoFrameAvvenuto)
		{
			this.CreazioneInizialeOrdigni();
			this.primoFrameAvvenuto = true;
		}
		else
		{
			if (!this.ordigniFisiciAssegnati)
			{
				this.ordignoFisico0 = base.transform.GetChild(1).transform.GetChild(1).transform.GetChild(2).gameObject;
				this.ordignoFisico1 = base.transform.GetChild(1).transform.GetChild(1).transform.GetChild(3).gameObject;
				this.ordigniFisiciAssegnati = true;
			}
			this.AllineamentoMiraOrdigni();
		}
		this.timerFrequenzaArma1 += Time.deltaTime;
		this.timerDiLancio += Time.deltaTime;
		this.timerDopoSparo += Time.deltaTime;
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
			this.GestioneOrdigniPrimaPersona();
			this.Mirini();
			if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 0)
			{
				this.AttaccoPrimaPersonaArma1();
				this.ordignoDaLanciare = null;
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
					this.zoomAttivo = false;
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					this.mirinoElettr1.GetComponent<Image>().sprite = this.mirinoTPS;
					this.mirinoElettr1.GetComponent<CanvasGroup>().alpha = 1f;
					this.suonoInterno.Stop();
					this.zoomAttivo = false;
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
			base.GetComponent<MOV_HeavyFightingVehicle>().torretta.transform.rotation = base.transform.rotation;
			base.GetComponent<MOV_HeavyFightingVehicle>().cannoni.transform.rotation = base.transform.rotation;
			this.suonoTorretta.Stop();
			base.GetComponent<MOV_HeavyFightingVehicle>().suonoTorrettaPartito = false;
			this.audioBeepCortoAttivo = false;
			this.audioBeepLungoAttivo = false;
			this.suonoBeep.Stop();
			this.timerDiAggancio = 0f;
			this.mirinoMissiliMobile.GetComponent<Image>().color = this.coloreBaseMirini;
			this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
			this.zoomAttivo = false;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x06000301 RID: 769 RVA: 0x0007C2F4 File Offset: 0x0007A4F4
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

	// Token: 0x06000302 RID: 770 RVA: 0x0007C4B4 File Offset: 0x0007A6B4
	private void CreazioneInizialeOrdigni()
	{
		for (int i = 0; i < base.GetComponent<PresenzaAlleato>().numeroCoppieOrdigni; i++)
		{
			this.ListaOrdigniAttiviLocale[i] = base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[i];
		}
		this.ordigno0 = (UnityEngine.Object.Instantiate(base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[0], base.transform.position, base.transform.rotation) as GameObject);
		this.ordigno0.transform.parent = base.transform.GetChild(1).transform.GetChild(1).transform;
		this.ordigno0.transform.localPosition = this.posizioneOrdigni0;
		this.ordigno1 = (UnityEngine.Object.Instantiate(base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[1], base.transform.position, base.transform.rotation) as GameObject);
		this.ordigno1.transform.parent = base.transform.GetChild(1).transform.GetChild(1).transform;
		this.ordigno1.transform.localPosition = this.posizioneOrdigni1;
		this.ListaOrdigniLocali = new List<GameObject>();
		this.ListaOrdigniLocali.Add(this.ordigno0);
		this.ListaOrdigniLocali.Add(this.ordigno1);
		for (int j = 0; j < base.GetComponent<PresenzaAlleato>().numeroCoppieOrdigni; j++)
		{
			int num = 0;
			while ((float)num < base.GetComponent<PresenzaAlleato>().ListaArmi[j + 1][5])
			{
				this.ListaOrdigniLocali[j].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[num] = (UnityEngine.Object.Instantiate(this.ListaOrdigniLocali[j].GetComponent<DatiOrdignoEsterno>().munizioneUsata, base.transform.position, base.transform.rotation) as GameObject);
				this.ListaOrdigniLocali[j].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[num].transform.parent = this.ListaOrdigniLocali[j].transform;
				this.ListaOrdigniLocali[j].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[num].transform.localPosition = this.ListaOrdigniLocali[j].GetComponent<DatiOrdignoEsterno>().ListaPosizioniMunizioni[num];
				num++;
			}
		}
	}

	// Token: 0x06000303 RID: 771 RVA: 0x0007C72C File Offset: 0x0007A92C
	private void CreazioneOrdigniConRifornimento()
	{
		if (base.GetComponent<PresenzaAlleato>().reintegrazioneNecessaria)
		{
			for (int i = 0; i < base.GetComponent<PresenzaAlleato>().numeroCoppieOrdigni; i++)
			{
				for (int j = 0; j < base.GetComponent<PresenzaAlleato>().ListaNumReintegrazioniOrdigni[i + 1]; j++)
				{
					for (int k = 0; k < this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche.Count; k++)
					{
						if (this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k] == null)
						{
							this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k] = (UnityEngine.Object.Instantiate(this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().munizioneUsata, base.transform.position, base.transform.rotation) as GameObject);
							this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k].transform.parent = this.ListaOrdigniLocali[i].transform;
							this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k].transform.localPosition = this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ListaPosizioniMunizioni[k];
							this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k].transform.eulerAngles = this.ListaOrdigniLocali[i].transform.eulerAngles;
							break;
						}
					}
				}
			}
			base.GetComponent<PresenzaAlleato>().reintegrazioneNecessaria = false;
		}
	}

	// Token: 0x06000304 RID: 772 RVA: 0x0007C8F4 File Offset: 0x0007AAF4
	private void CondizioniArma1()
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
					this.cannone.transform.GetChild(0).GetComponent<AudioSource>().Play();
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

	// Token: 0x06000305 RID: 773 RVA: 0x0007CB64 File Offset: 0x0007AD64
	private void CondizioniArma2()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[1][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[1][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[1][6];
		}
	}

	// Token: 0x06000306 RID: 774 RVA: 0x0007CBD4 File Offset: 0x0007ADD4
	private void CondizioniArma3()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[2][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[2][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[2][6];
		}
	}

	// Token: 0x06000307 RID: 775 RVA: 0x0007CC44 File Offset: 0x0007AE44
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

	// Token: 0x06000308 RID: 776 RVA: 0x0007CCF0 File Offset: 0x0007AEF0
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

	// Token: 0x06000309 RID: 777 RVA: 0x0007CD9C File Offset: 0x0007AF9C
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

	// Token: 0x0600030A RID: 778 RVA: 0x0007CE2C File Offset: 0x0007B02C
	private void Mirini()
	{
		if (this.cannoneAttivo)
		{
			this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 0f;
		}
		if (this.ordignoDaLanciare && this.ordignoDaLanciare.transform.parent)
		{
			if (this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 25 || this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 26)
			{
				this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 0f;
				this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 0f;
				this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 0f;
				this.SistemaDiLancioInPrimaPersona();
			}
			else if (this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 27)
			{
				this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 0f;
				this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 0f;
				this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 1f;
				this.SistemaDiLancioInPrimaPersona();
			}
			else if (this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 28)
			{
				this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 1f;
				this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 1f;
				this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 0f;
				this.SistemaDiLancioInPrimaPersona();
			}
		}
		if (!this.ordignoDaLanciare || !this.ordignoDaLanciare.transform.parent)
		{
			this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 0f;
		}
	}

	// Token: 0x0600030B RID: 779 RVA: 0x0007D074 File Offset: 0x0007B274
	private void AllineamentoMiraOrdigni()
	{
		this.ordignoFisico0.transform.eulerAngles = this.cannone.transform.eulerAngles;
		this.ordignoFisico1.transform.eulerAngles = this.cannone.transform.eulerAngles;
	}

	// Token: 0x0600030C RID: 780 RVA: 0x0007D0C4 File Offset: 0x0007B2C4
	private void PreparazioneAttacco()
	{
		bool flag = false;
		if (this.unitàBersaglio)
		{
			flag = Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale);
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
								else if (j >= 1 && !this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante && this.ListaOrdigniLocali[j - 1].transform.childCount > 1 && this.ListaOrdigniLocali[j - 1].transform.GetChild(1) != null)
								{
									this.ordignoDaLanciare = this.ListaOrdigniLocali[j - 1].transform.GetChild(1).gameObject;
									this.numArmaOrdignoDaLanciare = j;
									this.AttaccoIndipendenteOrdigni();
									break;
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
							else if (k >= 1 && !this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante && this.ListaOrdigniLocali[k - 1].transform.childCount > 1 && this.ListaOrdigniLocali[k - 1].transform.GetChild(1) != null)
							{
								this.ordignoDaLanciare = this.ListaOrdigniLocali[k - 1].transform.GetChild(1).gameObject;
								this.numArmaOrdignoDaLanciare = k;
								this.AttaccoIndipendenteOrdigni();
								break;
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

	// Token: 0x0600030D RID: 781 RVA: 0x0007DE0C File Offset: 0x0007C00C
	private void AttaccoIndipendente1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[0])
		{
			if (this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaAllPiùViciniPerTipo.Contains(base.gameObject) || this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] > 0.05f)
			{
				this.bocca1.GetComponent<AudioSource>().Play();
				this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] = 0f;
			}
			this.timerFrequenzaArma1 = 0f;
			this.bocca1.GetComponent<ParticleSystem>().Play();
			List<float> listaValoriArma;
			List<float> expr_14E = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int index;
			int expr_151 = index = 5;
			float num = listaValoriArma[index];
			expr_14E[expr_151] = num - 1f;
			List<float> listaValoriArma2;
			List<float> expr_172 = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int expr_175 = index = 6;
			num = listaValoriArma2[index];
			expr_172[expr_175] = num - 1f;
			this.SparoIndipendente1();
			this.timerDopoSparo = 0f;
		}
	}

	// Token: 0x0600030E RID: 782 RVA: 0x0007DFB8 File Offset: 0x0007C1B8
	private void SparoIndipendente1()
	{
		this.mm30Proiettile = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.mm30Proiettile.GetComponent<mm30Proiettile>().target = this.unitàBersaglio;
		this.mm30Proiettile.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x0600030F RID: 783 RVA: 0x0007E02C File Offset: 0x0007C22C
	private void AttaccoIndipendenteOrdigni()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][5] > 0f && this.timerDiLancio > base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][0])
		{
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = false;
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().bersaglio = this.unitàBersaglio;
			List<float> list;
			List<float> expr_9E = list = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
			int index;
			int expr_A1 = index = 5;
			float num = list[index];
			expr_9E[expr_A1] = num - 1f;
			List<float> list2;
			List<float> expr_CD = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
			int expr_D1 = index = 6;
			num = list2[index];
			expr_CD[expr_D1] = num - 1f;
			this.timerDiLancio = 0f;
			for (int i = 0; i < this.ListaOrdigniLocali.Count; i++)
			{
				this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ordignoAttivo = false;
			}
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().ordignoAttivo = true;
			this.ordignoDaLanciare = null;
		}
	}

	// Token: 0x06000310 RID: 784 RVA: 0x0007E184 File Offset: 0x0007C384
	private void SelezioneArma()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 0;
			this.audioBeepCortoAttivo = false;
			this.audioBeepLungoAttivo = false;
			this.suonoBeep.Stop();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 1;
			this.audioBeepCortoAttivo = false;
			this.audioBeepLungoAttivo = false;
			this.suonoBeep.Stop();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 2;
			this.audioBeepCortoAttivo = false;
			this.audioBeepLungoAttivo = false;
			this.suonoBeep.Stop();
		}
	}

	// Token: 0x06000311 RID: 785 RVA: 0x0007E224 File Offset: 0x0007C424
	private void GestioneOrdigniPrimaPersona()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.cannoneAttivo = true;
			this.gruppo0Attivo = false;
			this.gruppo1Attivo = false;
			this.timerDiAggancio = 0f;
			this.ordignoDaLanciare = null;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.cannoneAttivo = false;
			this.gruppo0Attivo = true;
			this.gruppo1Attivo = false;
			this.timerDiAggancio = 0f;
			this.ordignoDaLanciare = null;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			this.cannoneAttivo = false;
			this.gruppo0Attivo = false;
			this.gruppo1Attivo = true;
			this.timerDiAggancio = 0f;
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
						this.numArmaOrdignoDaLanciare = i + 1;
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

	// Token: 0x06000312 RID: 786 RVA: 0x0007E3C8 File Offset: 0x0007C5C8
	private void AttaccoPrimaPersonaArma1()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo))
		{
			if (this.targetSparo.collider.gameObject.tag == "Nemico" || this.targetSparo.collider.gameObject.tag == "Nemico Testa")
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
			this.bocca1.GetComponent<AudioSource>().Play();
			this.bocca1.GetComponent<ParticleSystem>().Play();
			List<float> list;
			List<float> expr_1D0 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int index;
			int expr_1D3 = index = 5;
			float num = list[index];
			expr_1D0[expr_1D3] = num - 1f;
			List<float> list2;
			List<float> expr_1FA = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int expr_1FE = index = 6;
			num = list2[index];
			expr_1FA[expr_1FE] = num - 1f;
			this.timerDopoSparo = 0f;
		}
	}

	// Token: 0x06000313 RID: 787 RVA: 0x0007E5F8 File Offset: 0x0007C7F8
	private void SparoArma1()
	{
		this.mm30Proiettile = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.mm30Proiettile.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.mm30Proiettile.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x06000314 RID: 788 RVA: 0x0007E668 File Offset: 0x0007C868
	private void SistemaDiLancioInPrimaPersona()
	{
		this.ListaBersPPPossibili.Clear();
		float num = (float)(Screen.width / 13);
		if ((this.ordignoDaLanciare && this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 25) || this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 26)
		{
			Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			if (Physics.Raycast(ray, out this.targetSparo))
			{
				if (this.targetSparo.collider.gameObject.tag == "Nemico" || this.targetSparo.collider.gameObject.tag == "Nemico Testa" || this.targetSparo.collider.gameObject.tag == "Nemico Coll Suppl")
				{
					if (Vector3.Distance(base.transform.position, this.targetSparo.point) > this.ordignoDaLanciare.GetComponent<DatiGeneraliMunizione>().portataMinima && Vector3.Distance(base.transform.position, this.targetSparo.point) <= this.ordignoDaLanciare.GetComponent<DatiGeneraliMunizione>().portataMassima)
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
			if (Input.GetMouseButton(0) && base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][5] > 0f && this.timerDiLancio > base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][1])
			{
				this.timerDiLancio = 0f;
				this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = true;
				List<float> list;
				List<float> expr_23D = list = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
				int index;
				int expr_241 = index = 5;
				float num2 = list[index];
				expr_23D[expr_241] = num2 - 1f;
				List<float> list2;
				List<float> expr_272 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
				int expr_276 = index = 6;
				num2 = list2[index];
				expr_272[expr_276] = num2 - 1f;
				for (int i = 0; i < this.ListaOrdigniLocali.Count; i++)
				{
					this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ordignoAttivo = false;
				}
				this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().ordignoAttivo = true;
				this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = true;
				this.ordignoDaLanciare = null;
			}
		}
		if (this.ordignoDaLanciare && this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 27 && Input.GetMouseButtonDown(0) && base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][5] > 0f && this.timerDiLancio > base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][1])
		{
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = true;
			List<float> list3;
			List<float> expr_3BB = list3 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
			int index;
			int expr_3BF = index = 5;
			float num2 = list3[index];
			expr_3BB[expr_3BF] = num2 - 1f;
			List<float> list4;
			List<float> expr_3F0 = list4 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
			int expr_3F4 = index = 6;
			num2 = list4[index];
			expr_3F0[expr_3F4] = num2 - 1f;
			this.timerDiLancio = 0f;
			for (int j = 0; j < this.ListaOrdigniLocali.Count; j++)
			{
				this.ListaOrdigniLocali[j].GetComponent<DatiOrdignoEsterno>().ordignoAttivo = false;
			}
			this.ordignoDaLanciare.transform.parent.GetComponent<AudioSource>().Play();
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().ordignoAttivo = true;
			this.ordignoDaLanciare = null;
		}
		if (this.ordignoDaLanciare && this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 28)
		{
			Ray ray2 = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			if (Physics.Raycast(ray2, out this.targetSparo))
			{
				if (this.targetSparo.collider.gameObject.tag == "Nemico" || this.targetSparo.collider.gameObject.tag == "Nemico Testa" || this.targetSparo.collider.gameObject.tag == "Nemico Coll Suppl")
				{
					if (Vector3.Distance(base.transform.position, this.targetSparo.point) > this.ordignoDaLanciare.GetComponent<DatiGeneraliMunizione>().portataMinima && Vector3.Distance(base.transform.position, this.targetSparo.point) <= this.ordignoDaLanciare.GetComponent<DatiGeneraliMunizione>().portataMassima)
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
			if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici.Count > 0)
			{
				foreach (GameObject current in this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici)
				{
					if (current && !current.GetComponent<PresenzaNemico>().insettoVolante)
					{
						Vector3 centroInsetto = current.GetComponent<PresenzaNemico>().centroInsetto;
						Vector3 vector = centroInsetto - base.transform.position;
						float num3 = Vector3.Dot(base.transform.forward, vector.normalized);
						float num4 = Vector3.Distance(base.transform.position, centroInsetto);
						if (num4 > this.ordignoDaLanciare.GetComponent<DatiGeneraliMunizione>().portataMinima && num4 < this.ordignoDaLanciare.GetComponent<DatiGeneraliMunizione>().portataMassima && num3 > 0f && !Physics.Linecast(base.transform.position, centroInsetto, this.layerVisuale))
						{
							float num5 = Vector2.Distance(this.terzaCamera.GetComponent<Camera>().WorldToScreenPoint(centroInsetto), this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f)));
							if (num5 < num)
							{
								this.ListaBersPPPossibili.Add(current);
							}
							this.bersDavantiEAPortata = true;
						}
						else
						{
							this.bersDavantiEAPortata = false;
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
				if (this.ListaBersPPPossibili.Count > 0)
				{
					this.bersaglioInPP = this.ListaBersPPPossibili[0];
				}
			}
			if (this.bersaglioInPP)
			{
				Vector3 vector2 = this.bersaglioInPP.GetComponent<PresenzaNemico>().centroInsetto - base.transform.position;
				float num6 = Vector3.Dot(base.transform.forward, vector2.normalized);
				if (num6 > 0f)
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
					float num7 = 999f;
					foreach (GameObject current2 in this.ListaBersPPPossibili)
					{
						float num8 = Vector2.Distance(this.terzaCamera.GetComponent<Camera>().WorldToScreenPoint(current2.GetComponent<CapsuleCollider>().center), this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f)));
						if (num8 < num7)
						{
							num7 = num8;
							this.bersaglioInPP = current2;
						}
					}
				}
				this.mirinoMissiliMobile.transform.position = this.terzaCamera.GetComponent<Camera>().WorldToScreenPoint(this.bersaglioInPP.GetComponent<PresenzaNemico>().centroInsetto);
				float num9 = Vector2.Distance(this.terzaCamera.GetComponent<Camera>().WorldToScreenPoint(this.bersaglioInPP.GetComponent<PresenzaNemico>().centroInsetto), this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f)));
				if (this.bersaglioInPP && this.bersaglioèAvantiInPP)
				{
					this.timerDiAggancio += Time.deltaTime;
					this.mirinoMissiliMobile.GetComponent<Image>().color = Color.red;
					if (!this.audioBeepCortoAttivo && this.timerDiAggancio < 2.5f)
					{
						this.suonoBeep.clip = this.beepCorto;
						this.suonoBeep.Play();
						this.audioBeepLungoAttivo = false;
						this.audioBeepCortoAttivo = true;
					}
					if (!this.audioBeepLungoAttivo && this.timerDiAggancio > 2.5f)
					{
						this.suonoBeep.clip = this.beepLungo;
						this.suonoBeep.Play();
						this.audioBeepLungoAttivo = true;
						this.audioBeepCortoAttivo = false;
					}
					if (this.audioBeepLungoAttivo && this.timerDiAggancio > 2.5f)
					{
						this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1f;
					}
					if (Input.GetMouseButtonDown(0) && base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][5] > 0f && this.timerDiLancio > base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][1] && this.ordignoDaLanciare != null && this.timerDiAggancio > 2.5f)
					{
						this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().bersaglio = this.bersaglioInPP;
						this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = true;
						List<float> list5;
						List<float> expr_B94 = list5 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
						int index;
						int expr_B98 = index = 5;
						float num2 = list5[index];
						expr_B94[expr_B98] = num2 - 1f;
						List<float> list6;
						List<float> expr_BC9 = list6 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
						int expr_BCD = index = 6;
						num2 = list6[index];
						expr_BC9[expr_BCD] = num2 - 1f;
						this.timerDiLancio = 0f;
						this.timerDiAggancio = 0f;
						for (int k = 0; k < this.ListaOrdigniLocali.Count; k++)
						{
							this.ListaOrdigniLocali[k].GetComponent<DatiOrdignoEsterno>().ordignoAttivo = false;
						}
						this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().ordignoAttivo = true;
						this.ordignoDaLanciare.transform.parent.GetComponent<AudioSource>().Play();
						this.ordignoDaLanciare = null;
						this.audioBeepLungoAttivo = false;
						this.suonoBeep.Stop();
						this.mirinoMissiliMobile.GetComponent<Image>().color = this.coloreBaseMirini;
						this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
					}
				}
				if (num9 > num || !this.bersaglioèAvantiInPP)
				{
					this.mirinoMissiliMobile.GetComponent<Image>().color = this.coloreBaseMirini;
					this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
					this.audioBeepLungoAttivo = false;
					this.audioBeepCortoAttivo = false;
					this.suonoBeep.Stop();
					this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().bersaglio = null;
					this.timerDiAggancio = 0f;
				}
				float num10 = Vector2.Distance(this.terzaCamera.GetComponent<Camera>().WorldToScreenPoint(this.bersaglioInPP.GetComponent<PresenzaNemico>().centroInsetto), this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f)));
				if (num10 > num || !this.bersaglioèAvantiInPP)
				{
					this.bersaglioInPP = null;
				}
			}
			if (base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][5] <= 0f)
			{
				this.audioBeepLungoAttivo = false;
				this.audioBeepCortoAttivo = false;
				this.suonoBeep.Stop();
				this.mirinoMissiliMobile.GetComponent<Image>().color = this.coloreBaseMirini;
				this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
				this.mirinoMissiliMobile.transform.position = this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f));
			}
		}
	}

	// Token: 0x04000CB1 RID: 3249
	public float angVertMax;

	// Token: 0x04000CB2 RID: 3250
	public float angVertMin;

	// Token: 0x04000CB3 RID: 3251
	private GameObject infoNeutreTattica;

	// Token: 0x04000CB4 RID: 3252
	private GameObject terzaCamera;

	// Token: 0x04000CB5 RID: 3253
	private GameObject primaCamera;

	// Token: 0x04000CB6 RID: 3254
	public GameObject bocca1;

	// Token: 0x04000CB7 RID: 3255
	private GameObject IANemico;

	// Token: 0x04000CB8 RID: 3256
	private GameObject InfoAlleati;

	// Token: 0x04000CB9 RID: 3257
	private float timerFrequenzaArma1;

	// Token: 0x04000CBA RID: 3258
	private float timerRicarica1;

	// Token: 0x04000CBB RID: 3259
	private bool ricaricaInCorso1;

	// Token: 0x04000CBC RID: 3260
	private float timerDopoSparo;

	// Token: 0x04000CBD RID: 3261
	private float tempoFraSparoERicarica;

	// Token: 0x04000CBE RID: 3262
	private int layerColpo;

	// Token: 0x04000CBF RID: 3263
	private int layerVisuale;

	// Token: 0x04000CC0 RID: 3264
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000CC1 RID: 3265
	public Vector3 rotazioneCameraTPS;

	// Token: 0x04000CC2 RID: 3266
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000CC3 RID: 3267
	private float timerPosizionamentoTPS;

	// Token: 0x04000CC4 RID: 3268
	private float timerPosizionamentoFPS;

	// Token: 0x04000CC5 RID: 3269
	private GameObject CanvasFPS;

	// Token: 0x04000CC6 RID: 3270
	private GameObject mirinoElettr1;

	// Token: 0x04000CC7 RID: 3271
	public Sprite mirinoTPS;

	// Token: 0x04000CC8 RID: 3272
	public Sprite mirinoFPS;

	// Token: 0x04000CC9 RID: 3273
	private GameObject sensoreRaggioMirino;

	// Token: 0x04000CCA RID: 3274
	private GameObject sensoreRaggioMirinoMobile;

	// Token: 0x04000CCB RID: 3275
	private GameObject mirinoMissiliFisso;

	// Token: 0x04000CCC RID: 3276
	private GameObject mirinoMissiliMobile;

	// Token: 0x04000CCD RID: 3277
	private Color coloreBaseMirini;

	// Token: 0x04000CCE RID: 3278
	private GameObject mirinoMissiliFiloguidati;

	// Token: 0x04000CCF RID: 3279
	private RaycastHit targetSparo;

	// Token: 0x04000CD0 RID: 3280
	private GameObject mm30Proiettile;

	// Token: 0x04000CD1 RID: 3281
	private NavMeshAgent alleatoNav;

	// Token: 0x04000CD2 RID: 3282
	private float velocitàAlleatoNav;

	// Token: 0x04000CD3 RID: 3283
	private GameObject cannone;

	// Token: 0x04000CD4 RID: 3284
	private GameObject unitàBersaglio;

	// Token: 0x04000CD5 RID: 3285
	private Vector3 centroUnitàBersaglio;

	// Token: 0x04000CD6 RID: 3286
	private GameObject munizioneArma1;

	// Token: 0x04000CD7 RID: 3287
	private GameObject munizioneArma2;

	// Token: 0x04000CD8 RID: 3288
	private GameObject munizioneArma3;

	// Token: 0x04000CD9 RID: 3289
	private AudioSource suonoTorretta;

	// Token: 0x04000CDA RID: 3290
	private AudioSource suonoInterno;

	// Token: 0x04000CDB RID: 3291
	private AudioSource suonoMotore;

	// Token: 0x04000CDC RID: 3292
	public AudioClip motoreFermo;

	// Token: 0x04000CDD RID: 3293
	public AudioClip motorePartenza;

	// Token: 0x04000CDE RID: 3294
	public AudioClip motoreViaggio;

	// Token: 0x04000CDF RID: 3295
	public AudioClip motoreStop;

	// Token: 0x04000CE0 RID: 3296
	private float timerPartenza;

	// Token: 0x04000CE1 RID: 3297
	private float timerStop;

	// Token: 0x04000CE2 RID: 3298
	private bool primaPartenza;

	// Token: 0x04000CE3 RID: 3299
	public float volumeMotoreIniziale;

	// Token: 0x04000CE4 RID: 3300
	private bool inPartenza;

	// Token: 0x04000CE5 RID: 3301
	private bool partenzaFinita;

	// Token: 0x04000CE6 RID: 3302
	private bool inStop;

	// Token: 0x04000CE7 RID: 3303
	public bool stopFinito;

	// Token: 0x04000CE8 RID: 3304
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x04000CE9 RID: 3305
	private List<GameObject> ListaOrdigniLocali;

	// Token: 0x04000CEA RID: 3306
	private GameObject ordignoDaLanciare;

	// Token: 0x04000CEB RID: 3307
	private int numArmaOrdignoDaLanciare;

	// Token: 0x04000CEC RID: 3308
	private GameObject ordigno0;

	// Token: 0x04000CED RID: 3309
	private GameObject ordigno1;

	// Token: 0x04000CEE RID: 3310
	public Vector3 posizioneOrdigni0;

	// Token: 0x04000CEF RID: 3311
	public Vector3 posizioneOrdigni1;

	// Token: 0x04000CF0 RID: 3312
	private bool cannoneAttivo;

	// Token: 0x04000CF1 RID: 3313
	private bool gruppo0Attivo;

	// Token: 0x04000CF2 RID: 3314
	private bool gruppo1Attivo;

	// Token: 0x04000CF3 RID: 3315
	private float timerDiAggancio;

	// Token: 0x04000CF4 RID: 3316
	public List<GameObject> ListaOrdigniAttiviLocale;

	// Token: 0x04000CF5 RID: 3317
	private List<bool> ListaGruppiOrdigniAttivi;

	// Token: 0x04000CF6 RID: 3318
	private bool primoFrameAvvenuto;

	// Token: 0x04000CF7 RID: 3319
	public List<GameObject> ListaBersPPPossibili;

	// Token: 0x04000CF8 RID: 3320
	public GameObject bersaglioInPP;

	// Token: 0x04000CF9 RID: 3321
	private bool bersaglioèAvantiInPP;

	// Token: 0x04000CFA RID: 3322
	private bool bersDavantiEAPortata;

	// Token: 0x04000CFB RID: 3323
	private float timerDiLancio;

	// Token: 0x04000CFC RID: 3324
	private AudioSource suonoBeep;

	// Token: 0x04000CFD RID: 3325
	public AudioClip beepLungo;

	// Token: 0x04000CFE RID: 3326
	public AudioClip beepCorto;

	// Token: 0x04000CFF RID: 3327
	private bool audioBeepLungoAttivo;

	// Token: 0x04000D00 RID: 3328
	private bool audioBeepCortoAttivo;

	// Token: 0x04000D01 RID: 3329
	private GameObject ordignoFisico0;

	// Token: 0x04000D02 RID: 3330
	private GameObject ordignoFisico1;

	// Token: 0x04000D03 RID: 3331
	private bool ordigniFisiciAssegnati;

	// Token: 0x04000D04 RID: 3332
	private bool zoomAttivo;

	// Token: 0x04000D05 RID: 3333
	private float distFineOrdineMovimento;

	// Token: 0x04000D06 RID: 3334
	private float timerAggRicerca;
}
