using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003F RID: 63
public class ATT_LightAntiAircraftVehicle : MonoBehaviour
{
	// Token: 0x06000328 RID: 808 RVA: 0x0008205C File Offset: 0x0008025C
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
		this.cannone = base.GetComponent<MOV_LightAntiAircraftVehicle>().cannoni;
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
		this.particelleBocca1 = this.bocca1.GetComponent<ParticleSystem>();
		this.particelleBocca2 = this.bocca2.GetComponent<ParticleSystem>();
		this.particelleBocca1bis = this.bocca1.transform.GetChild(1).GetComponent<ParticleSystem>();
		this.particelleBocca2bis = this.bocca2.transform.GetChild(1).GetComponent<ParticleSystem>();
		this.sparo1 = this.bocca1.transform.GetChild(1).gameObject;
		this.sparo2 = this.bocca2.transform.GetChild(1).gameObject;
		this.suonoArma1 = this.bocca1.GetComponent<AudioSource>();
		this.suonoArma2 = this.bocca2.GetComponent<AudioSource>();
		this.suonoRicarica = this.bocca1.transform.parent.GetComponent<AudioSource>();
		if (GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.moltiplicatoreAttaccoInFPS = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().moltiplicatoreFPSBattVeloce;
		}
		else
		{
			this.moltiplicatoreAttaccoInFPS = PlayerPrefs.GetFloat("moltiplicatore danni PP");
		}
		this.arma1 = this.bocca1.transform.GetChild(0).gameObject;
		this.arma2 = this.bocca2.transform.GetChild(0).gameObject;
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
	}

	// Token: 0x06000329 RID: 809 RVA: 0x00082458 File Offset: 0x00080658
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
		this.timerFrequenzaArma1 += Time.deltaTime;
		this.timerDiLancio += Time.deltaTime;
		this.timerDopoSparo += Time.deltaTime;
		this.unitàBersaglio = base.GetComponent<PresenzaAlleato>().unitàBersaglio;
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.GestioneSuoniIndipendenti();
			this.PreparazioneAttacco();
			if (!this.unitàBersaglio)
			{
				this.particelleBocca1.Stop();
				this.particelleBocca2.Stop();
				this.particelleBocca1bis.Stop();
				this.particelleBocca2bis.Stop();
				this.suonoArma1.Stop();
				this.suonoArma2.Stop();
				this.arma1StaSparando = false;
			}
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
			}
			base.GetComponent<NavMeshAgent>().enabled = false;
			if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
			{
				this.mirinoElettr1.GetComponent<CanvasGroup>().alpha = 1f;
				this.mirinoElettr1.GetComponent<Image>().sprite = this.mirinoTPS;
				this.timerInTPS += Time.deltaTime;
			}
			if (this.timerInTPS > 0f && this.timerInTPS < 0.1f)
			{
				this.particelleBocca1.Stop();
				this.particelleBocca2.Stop();
				this.particelleBocca1bis.Stop();
				this.particelleBocca2bis.Stop();
				this.suonoArma1.Stop();
				this.suonoArma2.Stop();
				this.arma1StaSparando = false;
				this.ordignoDaLanciare = null;
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
			base.GetComponent<MOV_LightAntiAircraftVehicle>().torretta.transform.rotation = base.transform.rotation;
			base.GetComponent<MOV_LightAntiAircraftVehicle>().cannoni.transform.rotation = base.transform.rotation;
			this.suonoTorretta.Stop();
			base.GetComponent<MOV_LightAntiAircraftVehicle>().suonoTorrettaPartito = false;
			this.audioBeepCortoAttivo = false;
			this.audioBeepLungoAttivo = false;
			this.suonoBeep.Stop();
			this.timerDiAggancio = 0f;
			this.mirinoMissiliMobile.GetComponent<Image>().color = this.coloreBaseMirini;
			this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
			this.particelleBocca1.Stop();
			this.particelleBocca2.Stop();
			this.particelleBocca1bis.Stop();
			this.particelleBocca2bis.Stop();
			this.suonoArma1.Stop();
			this.suonoArma2.Stop();
			this.arma1StaSparando = false;
			this.rotArma11Attiva = false;
			this.rotArma12Attiva = false;
			this.timerInTPS = 0f;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
			this.zoomAttivo = false;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x0600032A RID: 810 RVA: 0x000829F0 File Offset: 0x00080BF0
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

	// Token: 0x0600032B RID: 811 RVA: 0x00082BB0 File Offset: 0x00080DB0
	private void CreazioneInizialeOrdigni()
	{
		for (int i = 0; i < base.GetComponent<PresenzaAlleato>().numeroCoppieOrdigni; i++)
		{
			this.ListaOrdigniAttiviLocale[i] = base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[i];
		}
		this.ordigno0 = (UnityEngine.Object.Instantiate(base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[0], base.transform.position, base.transform.rotation) as GameObject);
		this.ordigno0.transform.parent = base.GetComponent<MOV_LightAntiAircraftVehicle>().cannoni.transform;
		this.ordigno0.transform.localPosition = this.posizioneOrdigni0;
		this.ordigno1 = (UnityEngine.Object.Instantiate(base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[1], base.transform.position, base.transform.rotation) as GameObject);
		this.ordigno1.transform.parent = base.GetComponent<MOV_LightAntiAircraftVehicle>().cannoni.transform;
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

	// Token: 0x0600032C RID: 812 RVA: 0x00082E10 File Offset: 0x00081010
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

	// Token: 0x0600032D RID: 813 RVA: 0x00082FD8 File Offset: 0x000811D8
	private void CondizioniArma1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] <= 0f && this.timerDopoSparo > 0.15f && this.timerDopoSparo < 0.25f)
		{
			this.particelleBocca1.Stop();
			this.particelleBocca2.Stop();
			this.particelleBocca1bis.Stop();
			this.particelleBocca2bis.Stop();
			this.suonoArma1.Stop();
			this.suonoArma2.Stop();
		}
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] <= 0f && this.timerDopoSparo > this.tempoFraSparoERicarica)
		{
			base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] = true;
			this.arma1StaSparando = false;
		}
		if (Input.GetKeyDown(KeyCode.R) && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] && this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera == base.gameObject && base.GetComponent<PresenzaAlleato>().ListaArmi[0][6] > 0f && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] < base.GetComponent<PresenzaAlleato>().ListaArmi[0][3])
		{
			base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] = true;
			this.arma1StaSparando = false;
			this.particelleBocca1.Stop();
			this.particelleBocca2.Stop();
			this.particelleBocca1bis.Stop();
			this.particelleBocca2bis.Stop();
			this.suonoArma1.Stop();
			this.suonoArma2.Stop();
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
				this.particelleBocca1.Stop();
				this.particelleBocca2.Stop();
				this.particelleBocca1bis.Stop();
				this.particelleBocca2bis.Stop();
				this.suonoArma1.Stop();
				this.suonoArma2.Stop();
				this.arma1StaSparando = false;
				this.rotArma11Attiva = false;
				this.rotArma12Attiva = false;
			}
		}
		if (this.rotArma11Attiva)
		{
			this.arma1.transform.Rotate(-Vector3.up * 9f);
		}
		if (this.rotArma12Attiva)
		{
			this.arma2.transform.Rotate(-Vector3.up * 9f);
		}
	}

	// Token: 0x0600032E RID: 814 RVA: 0x000833C0 File Offset: 0x000815C0
	private void CondizioniArma2()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[1][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[1][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[1][6];
		}
	}

	// Token: 0x0600032F RID: 815 RVA: 0x00083430 File Offset: 0x00081630
	private void CondizioniArma3()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[2][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[2][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[2][6];
		}
	}

	// Token: 0x06000330 RID: 816 RVA: 0x000834A0 File Offset: 0x000816A0
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

	// Token: 0x06000331 RID: 817 RVA: 0x0008354C File Offset: 0x0008174C
	private void CameraTPS()
	{
		this.timerPosizionamentoTPS += Time.deltaTime;
		if (this.timerPosizionamentoTPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.cannone.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraTPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localEulerAngles = new Vector3(this.rotazioneCameraTPS.x, 0f, this.cannone.transform.eulerAngles.z);
			if (this.timerPosizionamentoTPS < 0.15f)
			{
				this.particelleBocca1.Stop();
				this.particelleBocca2.Stop();
				this.particelleBocca1bis.Stop();
				this.particelleBocca2bis.Stop();
				this.suonoArma1.Stop();
				this.suonoArma2.Stop();
				this.arma1StaSparando = false;
				this.rotArma11Attiva = false;
				this.rotArma12Attiva = false;
			}
		}
	}

	// Token: 0x06000332 RID: 818 RVA: 0x00083660 File Offset: 0x00081860
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

	// Token: 0x06000333 RID: 819 RVA: 0x000836F0 File Offset: 0x000818F0
	private void Mirini()
	{
		if (this.cannoneAttivo)
		{
			this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 0f;
		}
		if (this.ordignoDaLanciare && this.ordignoDaLanciare.transform.parent && this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 29)
		{
			this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 1f;
			this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 1f;
			this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 0f;
			this.SistemaDiLancioInPrimaPersona();
		}
		if (!this.ordignoDaLanciare || !this.ordignoDaLanciare.transform.parent)
		{
			this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 0f;
		}
	}

	// Token: 0x06000334 RID: 820 RVA: 0x00083840 File Offset: 0x00081A40
	private void PreparazioneAttacco()
	{
		bool flag = false;
		if (this.unitàBersaglio)
		{
			flag = Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale);
			this.centroUnitàBersaglio = this.unitàBersaglio.GetComponent<PresenzaNemico>().centroInsetto;
		}
		else
		{
			this.particelleBocca1.Stop();
			this.particelleBocca2.Stop();
			this.particelleBocca1bis.Stop();
			this.particelleBocca2bis.Stop();
			this.suonoArma1.Stop();
			this.suonoArma2.Stop();
			this.arma1StaSparando = false;
			this.rotArma11Attiva = false;
			this.rotArma12Attiva = false;
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
		if (!base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0])
		{
			this.particelleBocca1.Stop();
			this.particelleBocca2.Stop();
			this.particelleBocca1bis.Stop();
			this.particelleBocca2bis.Stop();
			this.suonoArma1.Stop();
			this.suonoArma2.Stop();
			this.arma1StaSparando = false;
			this.rotArma11Attiva = false;
			this.rotArma12Attiva = false;
		}
		if (!base.GetComponent<PresenzaAlleato>().destinazioneOrdinata)
		{
			this.alleatoNav.speed = 0f;
			if (base.GetComponent<PresenzaAlleato>().attaccoOrdinato)
			{
				base.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
				this.unitàBersaglio = this.primaCamera.GetComponent<Selezionamento>().oggettoBersaglio;
				if (this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
				{
					float num3 = Vector3.Dot((this.centroUnitàBersaglio - base.transform.position).normalized, base.transform.up);
					if (num3 < this.angVertMax && num3 > this.angVertMin)
					{
						float num4 = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
						if (num4 >= num2)
						{
							this.particelleBocca1.Stop();
							this.particelleBocca2.Stop();
							this.particelleBocca1bis.Stop();
							this.particelleBocca2bis.Stop();
							this.suonoArma1.Stop();
							this.suonoArma2.Stop();
							this.arma1StaSparando = false;
							this.rotArma11Attiva = false;
							this.rotArma12Attiva = false;
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
								else if (j >= 1 && this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante && this.ListaOrdigniLocali[j - 1].transform.childCount > 1 && this.ListaOrdigniLocali[j - 1].transform.GetChild(1) != null)
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
							this.particelleBocca1.Stop();
							this.particelleBocca2.Stop();
							this.particelleBocca1bis.Stop();
							this.particelleBocca2bis.Stop();
							this.suonoArma1.Stop();
							this.suonoArma2.Stop();
							this.arma1StaSparando = false;
						}
					}
				}
			}
			else if (this.unitàBersaglio && this.alleatoNav.enabled && this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
			{
				if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita <= 0f)
				{
					base.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
					this.particelleBocca1.Stop();
					this.particelleBocca2.Stop();
					this.particelleBocca1bis.Stop();
					this.particelleBocca2bis.Stop();
					this.suonoArma1.Stop();
					this.suonoArma2.Stop();
					this.arma1StaSparando = false;
				}
				float num5 = Vector3.Dot((this.centroUnitàBersaglio - base.transform.position).normalized, base.transform.up);
				float num6 = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
				if (num5 < this.angVertMax && num5 > this.angVertMin)
				{
					if (num6 >= num2)
					{
						this.particelleBocca1.Stop();
						this.particelleBocca2.Stop();
						this.particelleBocca1bis.Stop();
						this.particelleBocca2bis.Stop();
						this.suonoArma1.Stop();
						this.suonoArma2.Stop();
						this.arma1StaSparando = false;
						this.rotArma11Attiva = false;
						this.rotArma12Attiva = false;
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
							else if (k >= 1 && this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante && this.ListaOrdigniLocali[k - 1].transform.childCount > 1 && this.ListaOrdigniLocali[k - 1].transform.GetChild(1) != null)
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
						else
						{
							this.particelleBocca1.Stop();
							this.particelleBocca2.Stop();
							this.particelleBocca1bis.Stop();
							this.particelleBocca2bis.Stop();
							this.suonoArma1.Stop();
							this.suonoArma2.Stop();
							this.arma1StaSparando = false;
							this.rotArma11Attiva = false;
							this.rotArma12Attiva = false;
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
				else
				{
					if (num6 > 3f)
					{
						this.alleatoNav.speed = this.velocitàAlleatoNav;
						this.unitàBersaglio = null;
					}
					this.particelleBocca1.Stop();
					this.particelleBocca2.Stop();
					this.particelleBocca1bis.Stop();
					this.particelleBocca2bis.Stop();
					this.suonoArma1.Stop();
					this.suonoArma2.Stop();
					this.arma1StaSparando = false;
					this.rotArma11Attiva = false;
					this.rotArma12Attiva = false;
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
			this.particelleBocca1.Stop();
			this.particelleBocca2.Stop();
			this.particelleBocca1bis.Stop();
			this.particelleBocca2bis.Stop();
			this.suonoArma1.Stop();
			this.suonoArma2.Stop();
			this.arma1StaSparando = false;
			this.rotArma11Attiva = false;
			this.rotArma12Attiva = false;
		}
	}

	// Token: 0x06000335 RID: 821 RVA: 0x000848BC File Offset: 0x00082ABC
	private void AttaccoIndipendente1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(base.transform.position, this.centroUnitàBersaglio, this.layerVisuale))
		{
			if (base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][0])
			{
				this.timerFrequenzaArma1 = 0f;
				List<float> list;
				List<float> expr_C0 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
				int index;
				int expr_C3 = index = 5;
				float num = list[index];
				expr_C0[expr_C3] = num - 1f;
				List<float> list2;
				List<float> expr_EE = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
				int expr_F2 = index = 6;
				num = list2[index];
				expr_EE[expr_F2] = num - 1f;
				this.timerDopoSparo = 0f;
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
				List<float> expr_291 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int expr_29F = index = base.GetComponent<PresenzaAlleato>().tipoTruppa;
				num = listaDanniAlleati[index];
				expr_291[expr_29F] = num + num2;
				if (!this.arma1StaSparando)
				{
					this.suonoArma1.Play();
					this.suonoArma2.Play();
					this.particelleBocca1bis.Play();
					this.particelleBocca2bis.Play();
					this.particelleBocca1.Play();
					this.particelleBocca2.Play();
					this.arma1StaSparando = true;
				}
				Vector3 normalized = (this.centroUnitàBersaglio - this.sparo1.transform.position).normalized;
				this.sparo1.transform.forward = normalized;
				Vector3 normalized2 = (this.centroUnitàBersaglio - this.sparo2.transform.position).normalized;
				this.sparo2.transform.forward = normalized2;
			}
			this.rotArma11Attiva = true;
			this.rotArma12Attiva = true;
		}
	}

	// Token: 0x06000336 RID: 822 RVA: 0x00084C50 File Offset: 0x00082E50
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

	// Token: 0x06000337 RID: 823 RVA: 0x00084DA8 File Offset: 0x00082FA8
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

	// Token: 0x06000338 RID: 824 RVA: 0x00084E48 File Offset: 0x00083048
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

	// Token: 0x06000339 RID: 825 RVA: 0x00084FEC File Offset: 0x000831EC
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
		if (Input.GetMouseButton(0))
		{
			if (!base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0] && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][1])
			{
				if (!this.arma1StaSparando)
				{
					this.suonoArma1.Play();
					this.suonoArma2.Play();
					this.particelleBocca1bis.Play();
					this.particelleBocca2bis.Play();
					this.particelleBocca1.Play();
					this.particelleBocca2.Play();
					this.arma1StaSparando = true;
				}
				this.timerFrequenzaArma1 = 0f;
				this.SparoArma1();
				List<float> list;
				List<float> expr_228 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
				int index;
				int expr_22B = index = 5;
				float num = list[index];
				expr_228[expr_22B] = num - 1f;
				List<float> list2;
				List<float> expr_256 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
				int expr_25A = index = 6;
				num = list2[index];
				expr_256[expr_25A] = num - 1f;
				this.timerDopoSparo = 0f;
			}
			Vector3 normalized = (this.targetSparo.point - this.terzaCamera.transform.position).normalized;
			this.sparo1.transform.forward = normalized;
			Vector3 normalized2 = (this.targetSparo.point - this.terzaCamera.transform.position).normalized;
			this.sparo2.transform.forward = normalized2;
			this.rotArma11Attiva = true;
			this.rotArma12Attiva = true;
		}
		if (Input.GetMouseButtonUp(0) && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[0])
		{
			this.suonoArma1.Stop();
			this.suonoArma2.Stop();
			this.particelleBocca1.Stop();
			this.particelleBocca2.Stop();
			this.particelleBocca1bis.Stop();
			this.particelleBocca2bis.Stop();
			this.arma1StaSparando = false;
			this.rotArma11Attiva = false;
			this.rotArma12Attiva = false;
		}
	}

	// Token: 0x0600033A RID: 826 RVA: 0x00085378 File Offset: 0x00083578
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

	// Token: 0x0600033B RID: 827 RVA: 0x000859D0 File Offset: 0x00083BD0
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
		if (this.ordignoDaLanciare && this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 29)
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
					if (current && current.GetComponent<PresenzaNemico>().insettoVolante)
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
				this.mirinoMissiliMobile.GetComponent<Image>().color = this.coloreBaseMirini;
				this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
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
						List<float> list3;
						List<float> expr_A40 = list3 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
						int index;
						int expr_A44 = index = 5;
						float num2 = list3[index];
						expr_A40[expr_A44] = num2 - 1f;
						List<float> list4;
						List<float> expr_A75 = list4 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
						int expr_A79 = index = 6;
						num2 = list4[index];
						expr_A75[expr_A79] = num2 - 1f;
						this.timerDiLancio = 0f;
						this.timerDiAggancio = 0f;
						for (int j = 0; j < this.ListaOrdigniLocali.Count; j++)
						{
							this.ListaOrdigniLocali[j].GetComponent<DatiOrdignoEsterno>().ordignoAttivo = false;
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

	// Token: 0x04000D49 RID: 3401
	public float angVertMax;

	// Token: 0x04000D4A RID: 3402
	public float angVertMin;

	// Token: 0x04000D4B RID: 3403
	private GameObject infoNeutreTattica;

	// Token: 0x04000D4C RID: 3404
	private GameObject terzaCamera;

	// Token: 0x04000D4D RID: 3405
	private GameObject primaCamera;

	// Token: 0x04000D4E RID: 3406
	public GameObject bocca1;

	// Token: 0x04000D4F RID: 3407
	public GameObject bocca2;

	// Token: 0x04000D50 RID: 3408
	private GameObject IANemico;

	// Token: 0x04000D51 RID: 3409
	private GameObject InfoAlleati;

	// Token: 0x04000D52 RID: 3410
	private float timerFrequenzaArma1;

	// Token: 0x04000D53 RID: 3411
	private float timerRicarica1;

	// Token: 0x04000D54 RID: 3412
	private bool ricaricaInCorso1;

	// Token: 0x04000D55 RID: 3413
	private float timerDopoSparo;

	// Token: 0x04000D56 RID: 3414
	private float tempoFraSparoERicarica;

	// Token: 0x04000D57 RID: 3415
	private bool arma1StaSparando;

	// Token: 0x04000D58 RID: 3416
	private int layerColpo;

	// Token: 0x04000D59 RID: 3417
	private int layerVisuale;

	// Token: 0x04000D5A RID: 3418
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000D5B RID: 3419
	public Vector3 rotazioneCameraTPS;

	// Token: 0x04000D5C RID: 3420
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000D5D RID: 3421
	private float timerPosizionamentoTPS;

	// Token: 0x04000D5E RID: 3422
	private float timerPosizionamentoFPS;

	// Token: 0x04000D5F RID: 3423
	private GameObject CanvasFPS;

	// Token: 0x04000D60 RID: 3424
	private GameObject mirinoElettr1;

	// Token: 0x04000D61 RID: 3425
	public Sprite mirinoTPS;

	// Token: 0x04000D62 RID: 3426
	public Sprite mirinoFPS;

	// Token: 0x04000D63 RID: 3427
	private GameObject sensoreRaggioMirino;

	// Token: 0x04000D64 RID: 3428
	private GameObject sensoreRaggioMirinoMobile;

	// Token: 0x04000D65 RID: 3429
	private GameObject mirinoMissiliFisso;

	// Token: 0x04000D66 RID: 3430
	private GameObject mirinoMissiliMobile;

	// Token: 0x04000D67 RID: 3431
	private Color coloreBaseMirini;

	// Token: 0x04000D68 RID: 3432
	private GameObject mirinoMissiliFiloguidati;

	// Token: 0x04000D69 RID: 3433
	private RaycastHit targetSparo;

	// Token: 0x04000D6A RID: 3434
	private GameObject mm30Proiettile;

	// Token: 0x04000D6B RID: 3435
	private NavMeshAgent alleatoNav;

	// Token: 0x04000D6C RID: 3436
	private float velocitàAlleatoNav;

	// Token: 0x04000D6D RID: 3437
	private GameObject cannone;

	// Token: 0x04000D6E RID: 3438
	private GameObject unitàBersaglio;

	// Token: 0x04000D6F RID: 3439
	private Vector3 centroUnitàBersaglio;

	// Token: 0x04000D70 RID: 3440
	private GameObject munizioneArma1;

	// Token: 0x04000D71 RID: 3441
	private GameObject munizioneArma2;

	// Token: 0x04000D72 RID: 3442
	private GameObject munizioneArma3;

	// Token: 0x04000D73 RID: 3443
	private AudioSource suonoTorretta;

	// Token: 0x04000D74 RID: 3444
	private AudioSource suonoInterno;

	// Token: 0x04000D75 RID: 3445
	private AudioSource suonoMotore;

	// Token: 0x04000D76 RID: 3446
	public AudioClip motoreFermo;

	// Token: 0x04000D77 RID: 3447
	public AudioClip motorePartenza;

	// Token: 0x04000D78 RID: 3448
	public AudioClip motoreViaggio;

	// Token: 0x04000D79 RID: 3449
	public AudioClip motoreStop;

	// Token: 0x04000D7A RID: 3450
	private float timerPartenza;

	// Token: 0x04000D7B RID: 3451
	private float timerStop;

	// Token: 0x04000D7C RID: 3452
	private bool primaPartenza;

	// Token: 0x04000D7D RID: 3453
	public float volumeMotoreIniziale;

	// Token: 0x04000D7E RID: 3454
	private bool inPartenza;

	// Token: 0x04000D7F RID: 3455
	private bool partenzaFinita;

	// Token: 0x04000D80 RID: 3456
	private bool inStop;

	// Token: 0x04000D81 RID: 3457
	public bool stopFinito;

	// Token: 0x04000D82 RID: 3458
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x04000D83 RID: 3459
	private List<GameObject> ListaOrdigniLocali;

	// Token: 0x04000D84 RID: 3460
	private GameObject ordignoDaLanciare;

	// Token: 0x04000D85 RID: 3461
	private int numArmaOrdignoDaLanciare;

	// Token: 0x04000D86 RID: 3462
	private GameObject ordigno0;

	// Token: 0x04000D87 RID: 3463
	private GameObject ordigno1;

	// Token: 0x04000D88 RID: 3464
	public Vector3 posizioneOrdigni0;

	// Token: 0x04000D89 RID: 3465
	public Vector3 posizioneOrdigni1;

	// Token: 0x04000D8A RID: 3466
	private bool cannoneAttivo;

	// Token: 0x04000D8B RID: 3467
	private bool gruppo0Attivo;

	// Token: 0x04000D8C RID: 3468
	private bool gruppo1Attivo;

	// Token: 0x04000D8D RID: 3469
	private float timerDiAggancio;

	// Token: 0x04000D8E RID: 3470
	public List<GameObject> ListaOrdigniAttiviLocale;

	// Token: 0x04000D8F RID: 3471
	private List<bool> ListaGruppiOrdigniAttivi;

	// Token: 0x04000D90 RID: 3472
	private bool primoFrameAvvenuto;

	// Token: 0x04000D91 RID: 3473
	public List<GameObject> ListaBersPPPossibili;

	// Token: 0x04000D92 RID: 3474
	public GameObject bersaglioInPP;

	// Token: 0x04000D93 RID: 3475
	private bool bersaglioèAvantiInPP;

	// Token: 0x04000D94 RID: 3476
	private bool bersDavantiEAPortata;

	// Token: 0x04000D95 RID: 3477
	private float timerDiLancio;

	// Token: 0x04000D96 RID: 3478
	private AudioSource suonoBeep;

	// Token: 0x04000D97 RID: 3479
	public AudioClip beepLungo;

	// Token: 0x04000D98 RID: 3480
	public AudioClip beepCorto;

	// Token: 0x04000D99 RID: 3481
	private bool audioBeepLungoAttivo;

	// Token: 0x04000D9A RID: 3482
	private bool audioBeepCortoAttivo;

	// Token: 0x04000D9B RID: 3483
	private ParticleSystem particelleBocca1;

	// Token: 0x04000D9C RID: 3484
	private ParticleSystem particelleBocca2;

	// Token: 0x04000D9D RID: 3485
	private ParticleSystem particelleBocca1bis;

	// Token: 0x04000D9E RID: 3486
	private ParticleSystem particelleBocca2bis;

	// Token: 0x04000D9F RID: 3487
	private GameObject sparo1;

	// Token: 0x04000DA0 RID: 3488
	private GameObject sparo2;

	// Token: 0x04000DA1 RID: 3489
	private AudioSource suonoArma1;

	// Token: 0x04000DA2 RID: 3490
	private AudioSource suonoArma2;

	// Token: 0x04000DA3 RID: 3491
	private AudioSource suonoRicarica;

	// Token: 0x04000DA4 RID: 3492
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x04000DA5 RID: 3493
	private GameObject arma1;

	// Token: 0x04000DA6 RID: 3494
	private GameObject arma2;

	// Token: 0x04000DA7 RID: 3495
	private float timerInTPS;

	// Token: 0x04000DA8 RID: 3496
	private bool rotArma11Attiva;

	// Token: 0x04000DA9 RID: 3497
	private bool rotArma12Attiva;

	// Token: 0x04000DAA RID: 3498
	private bool zoomAttivo;

	// Token: 0x04000DAB RID: 3499
	private float distFineOrdineMovimento;

	// Token: 0x04000DAC RID: 3500
	private float timerAggRicerca;
}
