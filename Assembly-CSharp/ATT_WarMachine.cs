using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000046 RID: 70
public class ATT_WarMachine : MonoBehaviour
{
	// Token: 0x06000399 RID: 921 RVA: 0x000930C4 File Offset: 0x000912C4
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.layerColpo = 165120;
		this.layerVisuale = 256;
		this.ListaUnitàBersagli = new List<GameObject>();
		this.ListaUnitàBersagli.Add(null);
		this.ListaUnitàBersagli.Add(null);
		this.ListaUnitàBersagli.Add(null);
		this.ListaUnitàBersagli.Add(null);
		this.ListaVisualiOscurate = new List<bool>();
		this.ListaVisualiOscurate.Add(false);
		this.ListaVisualiOscurate.Add(false);
		this.ListaVisualiOscurate.Add(false);
		this.ListaVisualiOscurate.Add(false);
		this.ListaCentroInsetti = new List<Vector3>();
		this.ListaCentroInsetti.Add(Vector3.zero);
		this.ListaCentroInsetti.Add(Vector3.zero);
		this.ListaCentroInsetti.Add(Vector3.zero);
		this.ListaCentroInsetti.Add(Vector3.zero);
		this.ListaBersNeiMirini = new List<bool>();
		this.ListaBersNeiMirini.Add(false);
		this.ListaBersNeiMirini.Add(false);
		this.ListaBersNeiMirini.Add(false);
		this.ListaBersNeiMirini.Add(false);
		this.ListaBocceCampione = new List<GameObject>();
		this.ListaBocceCampione.Add(this.bocca11);
		this.ListaBocceCampione.Add(this.bocca21);
		this.ListaBocceCampione.Add(this.bocca31);
		this.ListaBocceCampione.Add(this.bocca4);
		this.ListaDistanzeDaBers = new List<float>();
		this.ListaDistanzeDaBers.Add(0f);
		this.ListaDistanzeDaBers.Add(0f);
		this.ListaDistanzeDaBers.Add(0f);
		this.ListaDistanzeDaBers.Add(0f);
		this.ListaBasiTorrette = new List<GameObject>();
		this.ListaBasiTorrette.Add(this.baseTorretta1);
		this.ListaBasiTorrette.Add(this.baseTorretta2);
		this.ListaBasiTorrette.Add(this.baseTorretta3);
		this.ListaBasiTorrette.Add(this.baseTorretta4);
		this.ListaMunizioniAttiveUnità = new List<GameObject>();
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma1);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma2);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma2);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma3);
		this.bocca11Particelle = this.bocca11.GetComponent<ParticleSystem>();
		this.bocca12Particelle = this.bocca12.GetComponent<ParticleSystem>();
		this.bocca21Particelle = this.bocca21.GetComponent<ParticleSystem>();
		this.bocca22Particelle = this.bocca22.GetComponent<ParticleSystem>();
		this.bocca23Particelle = this.bocca23.GetComponent<ParticleSystem>();
		this.bocca31Particelle = this.bocca31.GetComponent<ParticleSystem>();
		this.bocca32Particelle = this.bocca32.GetComponent<ParticleSystem>();
		this.bocca33Particelle = this.bocca33.GetComponent<ParticleSystem>();
		this.bocca4Particelle = this.bocca4.GetComponent<ParticleSystem>();
		this.suonoArma11 = this.bocca11.GetComponent<AudioSource>();
		this.suonoArma12 = this.bocca12.GetComponent<AudioSource>();
		this.suonoArma21 = this.bocca21.GetComponent<AudioSource>();
		this.suonoArma22 = this.bocca22.GetComponent<AudioSource>();
		this.suonoArma23 = this.bocca23.GetComponent<AudioSource>();
		this.suonoArma31 = this.bocca31.GetComponent<AudioSource>();
		this.suonoArma32 = this.bocca32.GetComponent<AudioSource>();
		this.suonoArma33 = this.bocca33.GetComponent<AudioSource>();
		this.suonoArma4 = this.bocca4.GetComponent<AudioSource>();
		this.ListaBocche1 = new List<GameObject>();
		this.ListaBocche1.Add(this.bocca11);
		this.ListaBocche1.Add(this.bocca12);
		this.ListaCannoni1 = new List<GameObject>();
		this.ListaCannoni1.Add(this.bocca11.transform.parent.gameObject);
		this.ListaCannoni1.Add(this.bocca12.transform.parent.gameObject);
		this.ListaBoolCannoni1Sparati = new List<bool>();
		this.ListaBoolCannoni1Sparati.Add(false);
		this.ListaBoolCannoni1Sparati.Add(false);
		this.ListaBoolCannoni1InFondo = new List<bool>();
		this.ListaBoolCannoni1InFondo.Add(false);
		this.ListaBoolCannoni1InFondo.Add(false);
		this.ListaSuoniCannoni1 = new List<AudioSource>();
		this.ListaSuoniCannoni1.Add(this.suonoArma11);
		this.ListaSuoniCannoni1.Add(this.suonoArma12);
		this.ListaParticelleCannoni1 = new List<ParticleSystem>();
		this.ListaParticelleCannoni1.Add(this.bocca11Particelle);
		this.ListaParticelleCannoni1.Add(this.bocca12Particelle);
		this.ListaBocche2 = new List<GameObject>();
		this.ListaBocche2.Add(this.bocca21);
		this.ListaBocche2.Add(this.bocca22);
		this.ListaBocche2.Add(this.bocca23);
		this.ListaCannoni2 = new List<GameObject>();
		this.ListaCannoni2.Add(this.bocca21.transform.parent.gameObject);
		this.ListaCannoni2.Add(this.bocca22.transform.parent.gameObject);
		this.ListaCannoni2.Add(this.bocca23.transform.parent.gameObject);
		this.ListaBoolCannoni2Sparati = new List<bool>();
		this.ListaBoolCannoni2Sparati.Add(false);
		this.ListaBoolCannoni2Sparati.Add(false);
		this.ListaBoolCannoni2Sparati.Add(false);
		this.ListaBoolCannoni2InFondo = new List<bool>();
		this.ListaBoolCannoni2InFondo.Add(false);
		this.ListaBoolCannoni2InFondo.Add(false);
		this.ListaBoolCannoni2InFondo.Add(false);
		this.ListaSuoniCannoni2 = new List<AudioSource>();
		this.ListaSuoniCannoni2.Add(this.suonoArma21);
		this.ListaSuoniCannoni2.Add(this.suonoArma22);
		this.ListaSuoniCannoni2.Add(this.suonoArma23);
		this.ListaParticelleCannoni2 = new List<ParticleSystem>();
		this.ListaParticelleCannoni2.Add(this.bocca21Particelle);
		this.ListaParticelleCannoni2.Add(this.bocca22Particelle);
		this.ListaParticelleCannoni2.Add(this.bocca23Particelle);
		this.ListaBocche3 = new List<GameObject>();
		this.ListaBocche3.Add(this.bocca31);
		this.ListaBocche3.Add(this.bocca32);
		this.ListaBocche3.Add(this.bocca33);
		this.ListaCannoni3 = new List<GameObject>();
		this.ListaCannoni3.Add(this.bocca31.transform.parent.gameObject);
		this.ListaCannoni3.Add(this.bocca32.transform.parent.gameObject);
		this.ListaCannoni3.Add(this.bocca33.transform.parent.gameObject);
		this.ListaBoolCannoni3Sparati = new List<bool>();
		this.ListaBoolCannoni3Sparati.Add(false);
		this.ListaBoolCannoni3Sparati.Add(false);
		this.ListaBoolCannoni3Sparati.Add(false);
		this.ListaBoolCannoni3InFondo = new List<bool>();
		this.ListaBoolCannoni3InFondo.Add(false);
		this.ListaBoolCannoni3InFondo.Add(false);
		this.ListaBoolCannoni3InFondo.Add(false);
		this.ListaSuoniCannoni3 = new List<AudioSource>();
		this.ListaSuoniCannoni3.Add(this.suonoArma31);
		this.ListaSuoniCannoni3.Add(this.suonoArma32);
		this.ListaSuoniCannoni3.Add(this.suonoArma33);
		this.ListaParticelleCannoni3 = new List<ParticleSystem>();
		this.ListaParticelleCannoni3.Add(this.bocca31Particelle);
		this.ListaParticelleCannoni3.Add(this.bocca32Particelle);
		this.ListaParticelleCannoni3.Add(this.bocca33Particelle);
		this.suonoRicarica1 = this.baseTorretta1.transform.GetChild(1).GetComponent<AudioSource>();
		this.suonoRicarica2 = this.baseTorretta2.transform.GetChild(1).GetComponent<AudioSource>();
		this.suonoRicarica3 = this.baseTorretta3.transform.GetChild(1).GetComponent<AudioSource>();
		this.suonoRicarica4 = this.baseTorretta4.transform.GetChild(1).GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.suonoInterno = base.transform.GetChild(1).GetComponent<AudioSource>();
		this.volumeMotoreIniziale = base.GetComponent<AudioSource>().volume;
		this.CanvasFPS = GameObject.FindGameObjectWithTag("CanvasFPS");
		this.mirinoElettr1 = this.CanvasFPS.transform.GetChild(2).transform.GetChild(5).gameObject;
		this.alleatoNav = base.GetComponent<NavMeshAgent>();
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.baseCannoni1 = this.baseTorretta1.transform.GetChild(1).gameObject;
		this.baseCannoni2 = this.baseTorretta2.transform.GetChild(1).gameObject;
		this.baseCannoni3 = this.baseTorretta3.transform.GetChild(1).gameObject;
		this.baseCannoni4 = this.baseTorretta4.transform.GetChild(1).gameObject;
		this.suonoTorretta = this.ListaBasiTorrette[3].GetComponent<AudioSource>();
	}

	// Token: 0x0600039A RID: 922 RVA: 0x00093A58 File Offset: 0x00091C58
	private void Update()
	{
		this.ListaMunizioniAttiveUnità[0] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[0][0];
		this.ListaMunizioniAttiveUnità[1] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[1][0];
		this.ListaMunizioniAttiveUnità[2] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[2][0];
		this.ListaMunizioniAttiveUnità[3] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[3][0];
		this.munizioneArma1 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0];
		this.munizioneArma2 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[1];
		this.munizioneArma3 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[2];
		this.munizioneArma4 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[3];
		this.timerFrequenzaArma1 += Time.deltaTime;
		this.timerDopoSparo1 += Time.deltaTime;
		this.timerFrequenzaArma2 += Time.deltaTime;
		this.timerDopoSparo2 += Time.deltaTime;
		this.timerFrequenzaArma3 += Time.deltaTime;
		this.timerDopoSparo3 += Time.deltaTime;
		this.timerFrequenzaArma4 += Time.deltaTime;
		this.timerDopoSparo4 += Time.deltaTime;
		this.CondizioniArma1();
		this.CondizioniArma2();
		this.CondizioniArma3();
		this.CondizioniArma4();
		this.unitàBersaglio = base.GetComponent<PresenzaAlleato>().unitàBersaglio;
		this.GestioneCannoni();
		this.PreparazioneAttacco();
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.GestioneSuoniIndipendenti();
		}
		else
		{
			this.GestioneVisuali();
			this.SelezioneArma();
			if (!this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
			{
				if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 0)
				{
					this.AttaccoPrimaPersonaArma1();
				}
				else if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 1)
				{
					this.AttaccoPrimaPersonaArma2();
				}
				else if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 2)
				{
					this.AttaccoPrimaPersonaArma3();
				}
				else if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 3)
				{
					this.AttaccoPrimaPersonaArma4();
				}
			}
			base.GetComponent<NavMeshAgent>().enabled = false;
			if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva == 3)
			{
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
					this.mirinoElettr1.GetComponent<Image>().sprite = this.mirinoFPS;
					this.mirinoElettr1.GetComponent<CanvasGroup>().alpha = 1f;
					this.suonoInterno.Play();
					this.suonoMotore.spatialBlend = 0.2f;
					this.zoomAttivo = false;
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					this.mirinoElettr1.GetComponent<CanvasGroup>().alpha = 0f;
					this.suonoInterno.Stop();
					this.suonoMotore.spatialBlend = 1f;
					this.zoomAttivo = false;
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
					base.GetComponent<MOV_WarMachine>().torrettaFPS1.transform.rotation = base.transform.rotation;
					base.GetComponent<MOV_WarMachine>().cannoneFPS1.transform.rotation = base.transform.rotation;
					base.GetComponent<MOV_WarMachine>().torrettaFPS2.transform.rotation = base.transform.rotation;
					base.GetComponent<MOV_WarMachine>().cannoneFPS2.transform.rotation = base.transform.rotation;
					base.GetComponent<MOV_WarMachine>().torrettaFPS3.transform.rotation = base.transform.rotation;
					base.GetComponent<MOV_WarMachine>().cannoneFPS3.transform.rotation = base.transform.rotation;
					base.GetComponent<MOV_WarMachine>().torrettaFPS4.transform.rotation = base.transform.rotation;
					base.GetComponent<MOV_WarMachine>().cannoneFPS4.transform.rotation = base.transform.rotation;
				}
			}
		}
		if (this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0 && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0] == base.gameObject)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona = false;
			this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera = null;
			this.timerPosizionamentoTPS = 0f;
			this.timerPosizionamentoFPS = 0f;
			base.GetComponent<NavMeshAgent>().enabled = true;
			base.GetComponent<MOV_WarMachine>().torrettaFPS1.transform.rotation = base.transform.rotation;
			base.GetComponent<MOV_WarMachine>().cannoneFPS1.transform.rotation = base.transform.rotation;
			base.GetComponent<MOV_WarMachine>().torrettaFPS2.transform.rotation = base.transform.rotation;
			base.GetComponent<MOV_WarMachine>().cannoneFPS2.transform.rotation = base.transform.rotation;
			base.GetComponent<MOV_WarMachine>().torrettaFPS3.transform.rotation = base.transform.rotation;
			base.GetComponent<MOV_WarMachine>().cannoneFPS3.transform.rotation = base.transform.rotation;
			base.GetComponent<MOV_WarMachine>().torrettaFPS4.transform.rotation = base.transform.rotation;
			base.GetComponent<MOV_WarMachine>().cannoneFPS4.transform.rotation = base.transform.rotation;
			this.suonoTorretta.Stop();
			this.suonoMotore.spatialBlend = 1f;
			base.GetComponent<MOV_WarMachine>().suonoTorrettaPartito = false;
			this.zoomAttivo = false;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x0600039B RID: 923 RVA: 0x000940D4 File Offset: 0x000922D4
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

	// Token: 0x0600039C RID: 924 RVA: 0x00094294 File Offset: 0x00092494
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

	// Token: 0x0600039D RID: 925 RVA: 0x000944F4 File Offset: 0x000926F4
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

	// Token: 0x0600039E RID: 926 RVA: 0x00094754 File Offset: 0x00092954
	private void CondizioniArma3()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] <= 0f && this.timerDopoSparo3 > this.tempoFraSparoERicarica3)
		{
			base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[2] = true;
		}
		if (Input.GetKeyDown(KeyCode.R) && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[2] && this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera == base.gameObject && base.GetComponent<PresenzaAlleato>().ListaArmi[2][6] > 0f && base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] < base.GetComponent<PresenzaAlleato>().ListaArmi[2][3])
		{
			base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[2] = true;
		}
		if (base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[2])
		{
			this.timerRicarica3 += Time.deltaTime;
			if (base.GetComponent<PresenzaAlleato>().ListaArmi[2][6] > 0f)
			{
				if (this.timerRicarica3 > 0f && this.timerRicarica3 < 0.1f)
				{
					this.suonoRicarica3.Play();
				}
				if (this.timerRicarica3 > base.GetComponent<PresenzaAlleato>().ListaArmi[2][2])
				{
					if (base.GetComponent<PresenzaAlleato>().ListaArmi[2][6] < base.GetComponent<PresenzaAlleato>().ListaArmi[2][3])
					{
						base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[2][6];
						this.timerRicarica3 = 0f;
						base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[2] = false;
					}
					else
					{
						base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[2][3];
						this.timerRicarica3 = 0f;
						base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[2] = false;
					}
				}
			}
		}
	}

	// Token: 0x0600039F RID: 927 RVA: 0x000949B4 File Offset: 0x00092BB4
	private void CondizioniArma4()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[3][5] <= 0f && this.timerDopoSparo4 > this.tempoFraSparoERicarica4)
		{
			base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[3] = true;
		}
		if (Input.GetKeyDown(KeyCode.R) && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[3] && this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera == base.gameObject && base.GetComponent<PresenzaAlleato>().ListaArmi[3][6] > 0f && base.GetComponent<PresenzaAlleato>().ListaArmi[3][5] < base.GetComponent<PresenzaAlleato>().ListaArmi[3][3])
		{
			base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[3] = true;
		}
		if (base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[3])
		{
			this.timerRicarica4 += Time.deltaTime;
			if (base.GetComponent<PresenzaAlleato>().ListaArmi[3][6] > 0f)
			{
				if (this.timerRicarica4 > 0f && this.timerRicarica4 < 0.1f)
				{
					this.suonoRicarica4.Play();
				}
				if (this.timerRicarica4 > base.GetComponent<PresenzaAlleato>().ListaArmi[3][2])
				{
					if (base.GetComponent<PresenzaAlleato>().ListaArmi[3][6] < base.GetComponent<PresenzaAlleato>().ListaArmi[3][3])
					{
						base.GetComponent<PresenzaAlleato>().ListaArmi[3][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[3][6];
						this.timerRicarica4 = 0f;
						base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[3] = false;
					}
					else
					{
						base.GetComponent<PresenzaAlleato>().ListaArmi[3][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[3][3];
						this.timerRicarica4 = 0f;
						base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[3] = false;
					}
				}
			}
		}
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x00094C14 File Offset: 0x00092E14
	private void GestioneCannoni()
	{
		if (this.cannone1Attivo >= 2)
		{
			this.cannone1Attivo = 0;
		}
		for (int i = 0; i <= 1; i++)
		{
			if (this.ListaBoolCannoni1Sparati[i])
			{
				if (this.ListaCannoni1[i].transform.localPosition.y < -0.7f && !this.ListaBoolCannoni1InFondo[i])
				{
					this.ListaCannoni1[i].transform.localPosition += Vector3.up * 12f * Time.deltaTime;
				}
				if (this.ListaCannoni1[i].transform.localPosition.y >= -0.7f && !this.ListaBoolCannoni1InFondo[i])
				{
					this.ListaBoolCannoni1InFondo[i] = true;
				}
				if (this.ListaCannoni1[i].transform.localPosition.y > -2.4f && this.ListaBoolCannoni1InFondo[i])
				{
					this.ListaCannoni1[i].transform.localPosition += -Vector3.up * 2.5f * Time.deltaTime;
				}
				if (this.ListaCannoni1[i].transform.localPosition.y <= -2.4f && this.ListaBoolCannoni1InFondo[i])
				{
					this.ListaBoolCannoni1InFondo[i] = false;
					this.ListaBoolCannoni1Sparati[i] = false;
				}
			}
		}
		if (this.cannone2Attivo >= 3)
		{
			this.cannone2Attivo = 0;
		}
		for (int j = 0; j <= 2; j++)
		{
			if (this.ListaBoolCannoni2Sparati[j])
			{
				if (this.ListaCannoni2[j].transform.localPosition.x < -0.7f && !this.ListaBoolCannoni2InFondo[j])
				{
					this.ListaCannoni2[j].transform.localPosition += Vector3.right * 6f * Time.deltaTime;
				}
				if (this.ListaCannoni2[j].transform.localPosition.x >= -0.7f && !this.ListaBoolCannoni2InFondo[j])
				{
					this.ListaBoolCannoni2InFondo[j] = true;
				}
				if (this.ListaCannoni2[j].transform.localPosition.x > -1.4f && this.ListaBoolCannoni2InFondo[j])
				{
					this.ListaCannoni2[j].transform.localPosition += -Vector3.right * 1f * Time.deltaTime;
				}
				if (this.ListaCannoni2[j].transform.localPosition.x <= -1.4f && this.ListaBoolCannoni2InFondo[j])
				{
					this.ListaBoolCannoni2InFondo[j] = false;
					this.ListaBoolCannoni2Sparati[j] = false;
				}
			}
		}
		if (this.cannone3Attivo >= 3)
		{
			this.cannone3Attivo = 0;
		}
		for (int k = 0; k <= 2; k++)
		{
			if (this.ListaBoolCannoni3Sparati[k])
			{
				if (this.ListaCannoni3[k].transform.localPosition.x < -0.7f && !this.ListaBoolCannoni3InFondo[k])
				{
					this.ListaCannoni3[k].transform.localPosition += Vector3.right * 6f * Time.deltaTime;
				}
				if (this.ListaCannoni3[k].transform.localPosition.x >= -0.7f && !this.ListaBoolCannoni3InFondo[k])
				{
					this.ListaBoolCannoni3InFondo[k] = true;
				}
				if (this.ListaCannoni3[k].transform.localPosition.x > -1.4f && this.ListaBoolCannoni3InFondo[k])
				{
					this.ListaCannoni3[k].transform.localPosition += -Vector3.right * 1f * Time.deltaTime;
				}
				if (this.ListaCannoni3[k].transform.localPosition.x <= -1.4f && this.ListaBoolCannoni3InFondo[k])
				{
					this.ListaBoolCannoni3InFondo[k] = false;
					this.ListaBoolCannoni3Sparati[k] = false;
				}
			}
		}
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x0009515C File Offset: 0x0009335C
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

	// Token: 0x060003A2 RID: 930 RVA: 0x00095208 File Offset: 0x00093408
	private void CameraTPS()
	{
		this.timerPosizionamentoTPS += Time.deltaTime;
		if (this.timerPosizionamentoTPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = base.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraTPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localEulerAngles = new Vector3(30f, 0f, 0f);
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
		}
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x000952AC File Offset: 0x000934AC
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 0)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.baseCannoni1.transform;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS1;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = this.baseCannoni1.transform.rotation;
				this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
			}
			else if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 1)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.baseCannoni2.transform;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS2;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = this.baseCannoni2.transform.rotation;
				this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
			}
			else if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 2)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.baseCannoni3.transform;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS3;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = this.baseCannoni3.transform.rotation;
				this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
			}
			else if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 3)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.baseCannoni4.transform;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS4;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = this.baseCannoni4.transform.rotation;
				this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
			}
		}
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x00095504 File Offset: 0x00093704
	private void PreparazioneAttacco()
	{
		for (int i = 0; i < 4; i++)
		{
			if (i != base.GetComponent<PresenzaAlleato>().armaAttivaInFPS || (i == base.GetComponent<PresenzaAlleato>().armaAttivaInFPS && !this.terzaCamera.GetComponent<TerzaCamera>().èFPS))
			{
				if (this.ListaUnitàBersagli[i] != null)
				{
					this.ListaCentroInsetti[0] = this.ListaUnitàBersagli[i].GetComponent<PresenzaNemico>().centroInsetto;
					if (Physics.Linecast(this.ListaBocceCampione[i].transform.position, this.ListaCentroInsetti[i], this.layerVisuale))
					{
						this.ListaVisualiOscurate[i] = true;
					}
					else
					{
						this.ListaVisualiOscurate[i] = false;
					}
					Vector3 vector = Vector3.zero;
					Vector3 lhs = Vector3.zero;
					Vector3 lhs2 = Vector3.zero;
					if (i == 0)
					{
						vector = (this.ListaUnitàBersagli[i].transform.position - this.ListaBasiTorrette[i].transform.position).normalized;
						lhs = Vector3.ProjectOnPlane(vector, Vector3.up).normalized;
						float num = Vector3.Dot(lhs, base.transform.forward);
						float num2 = Vector3.Project(vector, Vector3.up).magnitude;
						if (!this.ListaVisualiOscurate[i] && num > 0f && num2 < 0.15f && Vector3.Distance(this.ListaBasiTorrette[i].transform.position, this.ListaUnitàBersagli[i].transform.position) < this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima)
						{
							this.ListaBersNeiMirini[i] = true;
						}
						else
						{
							this.ListaBersNeiMirini[i] = false;
							this.ListaUnitàBersagli[i] = null;
						}
					}
					else if (i == 1)
					{
						vector = (this.ListaUnitàBersagli[i].transform.position - this.ListaBasiTorrette[i].transform.position).normalized;
						lhs = Vector3.ProjectOnPlane(vector, Vector3.up).normalized;
						lhs2 = Vector3.ProjectOnPlane(vector, base.transform.forward).normalized;
						float num = Vector3.Dot(lhs, -base.transform.right);
						float num2 = Vector3.Dot(lhs2, -base.transform.right);
						if (!this.ListaVisualiOscurate[i] && num > -0.1f && num2 > 0.8f && Vector3.Distance(this.ListaBasiTorrette[i].transform.position, this.ListaUnitàBersagli[i].transform.position) < this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima)
						{
							this.ListaBersNeiMirini[i] = true;
						}
						else
						{
							this.ListaBersNeiMirini[i] = false;
							this.ListaUnitàBersagli[i] = null;
						}
					}
					else if (i == 2)
					{
						vector = (this.ListaUnitàBersagli[i].transform.position - this.ListaBasiTorrette[i].transform.position).normalized;
						lhs = Vector3.ProjectOnPlane(vector, Vector3.up).normalized;
						lhs2 = Vector3.ProjectOnPlane(vector, base.transform.forward).normalized;
						float num = Vector3.Dot(lhs, base.transform.right);
						float num2 = Vector3.Dot(lhs2, base.transform.right);
						if (!this.ListaVisualiOscurate[i] && num > -0.1f && num2 > 0.8f && Vector3.Distance(this.ListaBasiTorrette[i].transform.position, this.ListaUnitàBersagli[i].transform.position) < this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima)
						{
							this.ListaBersNeiMirini[i] = true;
						}
						else
						{
							this.ListaBersNeiMirini[i] = false;
							this.ListaUnitàBersagli[i] = null;
						}
					}
					else if (i == 3)
					{
						vector = (this.ListaUnitàBersagli[i].transform.position - this.ListaBasiTorrette[i].transform.position).normalized;
						float num2 = Vector3.Dot(vector, base.transform.up);
						if (!this.ListaVisualiOscurate[i] && num2 > -0.3f && num2 < 0.5f && Vector3.Distance(this.ListaBasiTorrette[i].transform.position, this.ListaUnitàBersagli[i].transform.position) < this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima)
						{
							this.ListaBersNeiMirini[i] = true;
						}
						else
						{
							this.ListaBersNeiMirini[i] = false;
							this.ListaUnitàBersagli[i] = null;
						}
					}
					if (this.ListaUnitàBersagli[i] && this.ListaBersNeiMirini[i] && this.ListaUnitàBersagli[i].GetComponent<PresenzaNemico>() && this.ListaUnitàBersagli[i].GetComponent<PresenzaNemico>().vita > 0f)
					{
						this.ListaBasiTorrette[i].transform.LookAt(new Vector3(this.ListaUnitàBersagli[i].transform.position.x, this.ListaBasiTorrette[i].transform.position.y, this.ListaUnitàBersagli[i].transform.position.z));
						this.ListaBasiTorrette[i].transform.GetChild(1).LookAt(this.ListaUnitàBersagli[i].transform.position);
						if (i == 0)
						{
							this.AttaccoIndipendente1();
						}
						else if (i == 1)
						{
							this.AttaccoIndipendente2();
						}
						else if (i == 2)
						{
							this.AttaccoIndipendente3();
						}
						else if (i == 3)
						{
							this.AttaccoIndipendente4();
						}
					}
					if (this.ListaUnitàBersagli[i] != null && !this.ListaUnitàBersagli[i].GetComponent<PresenzaNemico>().èStatoVisto)
					{
						this.ListaUnitàBersagli[i] = null;
					}
				}
				else if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti.Count > 0)
				{
					GestoreNeutroStrategia.valoreRandomSeed++;
					UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
					float f = UnityEngine.Random.Range(0f, (float)this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti.Count - 0.01f);
					bool flag = false;
					for (int j = Mathf.FloorToInt(f); j < this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti.Count; j++)
					{
						GameObject gameObject = this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti[j];
						if (gameObject != null && gameObject.GetComponent<PresenzaNemico>().èStatoVisto)
						{
							Vector3 centroInsetto = gameObject.GetComponent<PresenzaNemico>().centroInsetto;
							if (!Physics.Linecast(this.ListaBocceCampione[i].transform.position, centroInsetto, this.layerVisuale) && Vector3.Distance(this.ListaBasiTorrette[i].transform.position, centroInsetto) < this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima)
							{
								Vector3 vector2 = Vector3.zero;
								Vector3 lhs3 = Vector3.zero;
								Vector3 lhs4 = Vector3.zero;
								if (i == 0)
								{
									vector2 = (centroInsetto - this.ListaBasiTorrette[i].transform.position).normalized;
									lhs3 = Vector3.ProjectOnPlane(vector2, Vector3.up).normalized;
									float num3 = Vector3.Dot(lhs3, base.transform.forward);
									float num4 = Vector3.Project(vector2, Vector3.up).magnitude;
									if (!Physics.Linecast(this.ListaBocceCampione[i].transform.position, centroInsetto, this.layerVisuale) && num3 > 0f && num4 < 0.15f && Vector3.Distance(this.ListaBasiTorrette[i].transform.position, centroInsetto) < this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima)
									{
										this.ListaUnitàBersagli[i] = gameObject;
										flag = true;
										break;
									}
								}
								else if (i == 1)
								{
									vector2 = (centroInsetto - this.ListaBasiTorrette[i].transform.position).normalized;
									lhs3 = Vector3.ProjectOnPlane(vector2, Vector3.up).normalized;
									lhs4 = Vector3.ProjectOnPlane(vector2, base.transform.forward).normalized;
									float num3 = Vector3.Dot(lhs3, -base.transform.right);
									float num4 = Vector3.Dot(lhs4, -base.transform.right);
									if (!Physics.Linecast(this.ListaBocceCampione[i].transform.position, centroInsetto, this.layerVisuale) && num3 > -0.1f && num4 > 0.8f && Vector3.Distance(this.ListaBasiTorrette[i].transform.position, centroInsetto) < this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima)
									{
										this.ListaUnitàBersagli[i] = gameObject;
										flag = true;
										break;
									}
								}
								else if (i == 2)
								{
									vector2 = (centroInsetto - this.ListaBasiTorrette[i].transform.position).normalized;
									lhs3 = Vector3.ProjectOnPlane(vector2, Vector3.up).normalized;
									lhs4 = Vector3.ProjectOnPlane(vector2, base.transform.forward).normalized;
									float num3 = Vector3.Dot(lhs3, base.transform.right);
									float num4 = Vector3.Dot(lhs4, base.transform.right);
									if (!Physics.Linecast(this.ListaBocceCampione[i].transform.position, centroInsetto, this.layerVisuale) && num3 > -0.1f && num4 > 0.8f && Vector3.Distance(this.ListaBasiTorrette[i].transform.position, centroInsetto) < this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima)
									{
										this.ListaUnitàBersagli[i] = gameObject;
										flag = true;
										break;
									}
								}
								else if (i == 3)
								{
									vector2 = (centroInsetto - this.ListaBasiTorrette[i].transform.position).normalized;
									float num4 = Vector3.Dot(vector2, base.transform.up);
									if (!Physics.Linecast(this.ListaBocceCampione[i].transform.position, centroInsetto, this.layerVisuale) && num4 > -0.3f && num4 < 0.5f && Vector3.Distance(this.ListaBasiTorrette[i].transform.position, centroInsetto) < this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima)
									{
										this.ListaUnitàBersagli[i] = gameObject;
										flag = true;
										break;
									}
								}
							}
						}
					}
					if (!flag)
					{
						for (int k = 0; k < Mathf.FloorToInt(f) - 1; k++)
						{
							GameObject gameObject2 = this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti[k];
							if (gameObject2 != null && gameObject2.GetComponent<PresenzaNemico>().èStatoVisto)
							{
								Vector3 centroInsetto2 = gameObject2.GetComponent<PresenzaNemico>().centroInsetto;
								if (!Physics.Linecast(this.ListaBocceCampione[i].transform.position, centroInsetto2, this.layerVisuale) && Vector3.Distance(this.ListaBasiTorrette[i].transform.position, centroInsetto2) < this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima)
								{
									Vector3 vector3 = Vector3.zero;
									Vector3 lhs5 = Vector3.zero;
									Vector3 lhs6 = Vector3.zero;
									if (i == 0)
									{
										vector3 = (centroInsetto2 - this.ListaBasiTorrette[i].transform.position).normalized;
										lhs5 = Vector3.ProjectOnPlane(vector3, Vector3.up).normalized;
										float num5 = Vector3.Dot(lhs5, base.transform.forward);
										float num6 = Vector3.Project(vector3, Vector3.up).magnitude;
										if (!Physics.Linecast(this.ListaBocceCampione[i].transform.position, centroInsetto2, this.layerVisuale) && num5 > 0f && num6 < 0.15f && Vector3.Distance(this.ListaBasiTorrette[i].transform.position, centroInsetto2) < this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima)
										{
											this.ListaUnitàBersagli[i] = gameObject2;
											break;
										}
									}
									else if (i == 1)
									{
										vector3 = (centroInsetto2 - this.ListaBasiTorrette[i].transform.position).normalized;
										lhs5 = Vector3.ProjectOnPlane(vector3, Vector3.up).normalized;
										lhs6 = Vector3.ProjectOnPlane(vector3, base.transform.forward).normalized;
										float num5 = Vector3.Dot(lhs5, -base.transform.right);
										float num6 = Vector3.Dot(lhs6, -base.transform.right);
										if (!Physics.Linecast(this.ListaBocceCampione[i].transform.position, centroInsetto2, this.layerVisuale) && num5 > -0.1f && num6 > 0.8f && Vector3.Distance(this.ListaBasiTorrette[i].transform.position, centroInsetto2) < this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima)
										{
											this.ListaUnitàBersagli[i] = gameObject2;
											break;
										}
									}
									else if (i == 2)
									{
										vector3 = (centroInsetto2 - this.ListaBasiTorrette[i].transform.position).normalized;
										lhs5 = Vector3.ProjectOnPlane(vector3, Vector3.up).normalized;
										lhs6 = Vector3.ProjectOnPlane(vector3, base.transform.forward).normalized;
										float num5 = Vector3.Dot(lhs5, base.transform.right);
										float num6 = Vector3.Dot(lhs6, base.transform.right);
										if (!Physics.Linecast(this.ListaBocceCampione[i].transform.position, centroInsetto2, this.layerVisuale) && num5 > -0.1f && num6 > 0.8f && Vector3.Distance(this.ListaBasiTorrette[i].transform.position, centroInsetto2) < this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima)
										{
											this.ListaUnitàBersagli[i] = gameObject2;
											break;
										}
									}
									else if (i == 3)
									{
										vector3 = (centroInsetto2 - this.ListaBasiTorrette[i].transform.position).normalized;
										float num6 = Vector3.Dot(vector3, base.transform.up);
										if (!Physics.Linecast(this.ListaBocceCampione[i].transform.position, centroInsetto2, this.layerVisuale) && num6 > -0.3f && num6 < 0.5f && Vector3.Distance(this.ListaBasiTorrette[i].transform.position, centroInsetto2) < this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima)
										{
											this.ListaUnitàBersagli[i] = gameObject2;
											break;
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x000966A8 File Offset: 0x000948A8
	private void AttaccoIndipendente1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && this.ListaUnitàBersagli[0] && this.ListaUnitàBersagli[0].GetComponent<PresenzaNemico>().vita > 0f && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][0])
		{
			this.timerFrequenzaArma1 = 0f;
			this.ListaSuoniCannoni1[this.cannone1Attivo].Play();
			this.ListaParticelleCannoni1[this.cannone1Attivo].Play();
			List<float> list;
			List<float> expr_D7 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int index;
			int expr_DA = index = 5;
			float num = list[index];
			expr_D7[expr_DA] = num - 1f;
			List<float> list2;
			List<float> expr_101 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int expr_104 = index = 6;
			num = list2[index];
			expr_101[expr_104] = num - 1f;
			this.SparoIndipendente1();
			this.timerDopoSparo1 = 0f;
			this.cannone1Attivo++;
		}
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x000967F0 File Offset: 0x000949F0
	private void SparoIndipendente1()
	{
		this.proiettileCarro1 = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.ListaBocche1[this.cannone1Attivo].transform.position, this.ListaBocche1[this.cannone1Attivo].transform.rotation) as GameObject);
		this.proiettileCarro1.GetComponent<DatiProiettile>().locazioneTarget = this.ListaUnitàBersagli[0].transform.position;
		this.ListaBoolCannoni1Sparati[this.cannone1Attivo] = true;
		this.proiettileCarro1.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x0009689C File Offset: 0x00094A9C
	private void AttaccoIndipendente2()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[1] && this.ListaUnitàBersagli[1] && this.ListaUnitàBersagli[1].GetComponent<PresenzaNemico>().vita > 0f && base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] > 0f && this.timerFrequenzaArma2 > base.GetComponent<PresenzaAlleato>().ListaArmi[1][0])
		{
			this.timerFrequenzaArma2 = 0f;
			this.ListaSuoniCannoni2[this.cannone2Attivo].Play();
			this.ListaParticelleCannoni2[this.cannone2Attivo].Play();
			List<float> list;
			List<float> expr_D7 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
			int index;
			int expr_DA = index = 5;
			float num = list[index];
			expr_D7[expr_DA] = num - 1f;
			List<float> list2;
			List<float> expr_101 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
			int expr_104 = index = 6;
			num = list2[index];
			expr_101[expr_104] = num - 1f;
			this.SparoIndipendente2();
			this.timerDopoSparo2 = 0f;
			this.cannone2Attivo++;
		}
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x000969E4 File Offset: 0x00094BE4
	private void SparoIndipendente2()
	{
		this.proiettileCarro2 = (UnityEngine.Object.Instantiate(this.munizioneArma2, this.ListaBocche2[this.cannone2Attivo].transform.position, this.ListaBocche2[this.cannone2Attivo].transform.rotation) as GameObject);
		this.proiettileCarro2.GetComponent<DatiProiettile>().locazioneTarget = this.ListaUnitàBersagli[1].transform.position;
		this.ListaBoolCannoni2Sparati[this.cannone2Attivo] = true;
		this.proiettileCarro2.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x00096A90 File Offset: 0x00094C90
	private void AttaccoIndipendente3()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[2] && this.ListaUnitàBersagli[2] && this.ListaUnitàBersagli[2].GetComponent<PresenzaNemico>().vita > 0f && base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] > 0f && this.timerFrequenzaArma3 > base.GetComponent<PresenzaAlleato>().ListaArmi[2][0])
		{
			this.timerFrequenzaArma3 = 0f;
			this.ListaSuoniCannoni3[this.cannone3Attivo].Play();
			this.ListaParticelleCannoni3[this.cannone3Attivo].Play();
			List<float> list;
			List<float> expr_D7 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[2];
			int index;
			int expr_DA = index = 5;
			float num = list[index];
			expr_D7[expr_DA] = num - 1f;
			List<float> list2;
			List<float> expr_101 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[2];
			int expr_104 = index = 6;
			num = list2[index];
			expr_101[expr_104] = num - 1f;
			this.SparoIndipendente3();
			this.timerDopoSparo3 = 0f;
			this.cannone3Attivo++;
		}
	}

	// Token: 0x060003AA RID: 938 RVA: 0x00096BD8 File Offset: 0x00094DD8
	private void SparoIndipendente3()
	{
		this.proiettileCarro3 = (UnityEngine.Object.Instantiate(this.munizioneArma3, this.ListaBocche3[this.cannone3Attivo].transform.position, this.ListaBocche3[this.cannone3Attivo].transform.rotation) as GameObject);
		this.proiettileCarro3.GetComponent<DatiProiettile>().locazioneTarget = this.ListaUnitàBersagli[2].transform.position;
		this.ListaBoolCannoni3Sparati[this.cannone3Attivo] = true;
		this.proiettileCarro3.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060003AB RID: 939 RVA: 0x00096C84 File Offset: 0x00094E84
	private void AttaccoIndipendente4()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[3] && this.ListaUnitàBersagli[3] && this.ListaUnitàBersagli[3].GetComponent<PresenzaNemico>().vita > 0f && base.GetComponent<PresenzaAlleato>().ListaArmi[3][5] > 0f && this.timerFrequenzaArma4 > base.GetComponent<PresenzaAlleato>().ListaArmi[3][0])
		{
			this.timerFrequenzaArma4 = 0f;
			this.suonoArma4.Play();
			this.bocca4Particelle.Play();
			List<float> list;
			List<float> expr_C1 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[3];
			int index;
			int expr_C4 = index = 5;
			float num = list[index];
			expr_C1[expr_C4] = num - 1f;
			List<float> list2;
			List<float> expr_EB = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[3];
			int expr_EE = index = 6;
			num = list2[index];
			expr_EB[expr_EE] = num - 1f;
			this.SparoIndipendente4();
			this.timerDopoSparo4 = 0f;
		}
	}

	// Token: 0x060003AC RID: 940 RVA: 0x00096DA8 File Offset: 0x00094FA8
	private void SparoIndipendente4()
	{
		this.proiettileCarro4 = (UnityEngine.Object.Instantiate(this.munizioneArma4, this.bocca4.transform.position, this.bocca4.transform.rotation) as GameObject);
		this.proiettileCarro4.GetComponent<DatiProiettile>().locazioneTarget = this.ListaUnitàBersagli[3].transform.position;
		this.proiettileCarro4.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060003AD RID: 941 RVA: 0x00096E2C File Offset: 0x0009502C
	private void SelezioneArma()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 0;
			this.timerPosizionamentoFPS = 0f;
			if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
			}
			else if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 1;
			this.timerPosizionamentoFPS = 0f;
			if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
			}
			else if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 2;
			this.timerPosizionamentoFPS = 0f;
			if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
			}
			else if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 3;
			this.timerPosizionamentoFPS = 0f;
			if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
			}
			else if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
			}
		}
	}

	// Token: 0x060003AE RID: 942 RVA: 0x0009700C File Offset: 0x0009520C
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
			this.ListaSuoniCannoni1[this.cannone1Attivo].Play();
			this.ListaParticelleCannoni1[this.cannone1Attivo].Play();
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
			this.cannone1Attivo++;
		}
	}

	// Token: 0x060003AF RID: 943 RVA: 0x00097278 File Offset: 0x00095478
	private void SparoArma1()
	{
		this.proiettileCarro1 = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.ListaBocche1[this.cannone1Attivo].transform.position, this.ListaBocche1[this.cannone1Attivo].transform.rotation) as GameObject);
		this.proiettileCarro1.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.ListaBoolCannoni1Sparati[this.cannone1Attivo] = true;
		this.proiettileCarro1.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x00097310 File Offset: 0x00095510
	private void AttaccoPrimaPersonaArma2()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo))
		{
			if (this.targetSparo.collider.gameObject.tag == "Nemico" || this.targetSparo.collider.gameObject.tag == "Nemico Testa" || this.targetSparo.collider.gameObject.tag == "Nemico Coll Suppl")
			{
				if (Vector3.Distance(base.transform.position, this.targetSparo.point) > this.ListaMunizioniAttiveUnità[1].GetComponent<DatiGeneraliMunizione>().portataMinima && Vector3.Distance(base.transform.position, this.targetSparo.point) <= this.ListaMunizioniAttiveUnità[1].GetComponent<DatiGeneraliMunizione>().portataMassima)
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
			this.ListaSuoniCannoni2[this.cannone2Attivo].Play();
			this.ListaParticelleCannoni2[this.cannone2Attivo].Play();
			List<float> list;
			List<float> expr_200 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
			int index;
			int expr_203 = index = 5;
			float num = list[index];
			expr_200[expr_203] = num - 1f;
			List<float> list2;
			List<float> expr_22A = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
			int expr_22E = index = 6;
			num = list2[index];
			expr_22A[expr_22E] = num - 1f;
			this.timerDopoSparo2 = 0f;
			this.cannone2Attivo++;
		}
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x0009757C File Offset: 0x0009577C
	private void SparoArma2()
	{
		this.proiettileCarro2 = (UnityEngine.Object.Instantiate(this.munizioneArma2, this.ListaBocche2[this.cannone2Attivo].transform.position, this.ListaBocche2[this.cannone2Attivo].transform.rotation) as GameObject);
		this.proiettileCarro2.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.ListaBoolCannoni2Sparati[this.cannone2Attivo] = true;
		this.proiettileCarro2.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x00097614 File Offset: 0x00095814
	private void AttaccoPrimaPersonaArma3()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo))
		{
			if (this.targetSparo.collider.gameObject.tag == "Nemico" || this.targetSparo.collider.gameObject.tag == "Nemico Testa" || this.targetSparo.collider.gameObject.tag == "Nemico Coll Suppl")
			{
				if (Vector3.Distance(base.transform.position, this.targetSparo.point) > this.ListaMunizioniAttiveUnità[2].GetComponent<DatiGeneraliMunizione>().portataMinima && Vector3.Distance(base.transform.position, this.targetSparo.point) <= this.ListaMunizioniAttiveUnità[2].GetComponent<DatiGeneraliMunizione>().portataMassima)
				{
					base.GetComponent<PresenzaAlleato>().ListaFuoriPortataArmi[2] = false;
				}
				else
				{
					base.GetComponent<PresenzaAlleato>().ListaFuoriPortataArmi[2] = true;
				}
			}
			else
			{
				base.GetComponent<PresenzaAlleato>().ListaFuoriPortataArmi[2] = false;
			}
		}
		if (Input.GetMouseButton(0) && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[2] && base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] > 0f && this.timerFrequenzaArma3 > base.GetComponent<PresenzaAlleato>().ListaArmi[2][1])
		{
			this.timerFrequenzaArma3 = 0f;
			this.SparoArma3();
			this.ListaSuoniCannoni3[this.cannone3Attivo].Play();
			this.ListaParticelleCannoni3[this.cannone3Attivo].Play();
			List<float> list;
			List<float> expr_200 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[2];
			int index;
			int expr_203 = index = 5;
			float num = list[index];
			expr_200[expr_203] = num - 1f;
			List<float> list2;
			List<float> expr_22A = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[2];
			int expr_22E = index = 6;
			num = list2[index];
			expr_22A[expr_22E] = num - 1f;
			this.timerDopoSparo3 = 0f;
			this.cannone3Attivo++;
		}
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x00097880 File Offset: 0x00095A80
	private void SparoArma3()
	{
		this.proiettileCarro3 = (UnityEngine.Object.Instantiate(this.munizioneArma3, this.ListaBocche3[this.cannone3Attivo].transform.position, this.ListaBocche3[this.cannone3Attivo].transform.rotation) as GameObject);
		this.proiettileCarro3.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.ListaBoolCannoni3Sparati[this.cannone3Attivo] = true;
		this.proiettileCarro3.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x00097918 File Offset: 0x00095B18
	private void AttaccoPrimaPersonaArma4()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo))
		{
			if (this.targetSparo.collider.gameObject.tag == "Nemico" || this.targetSparo.collider.gameObject.tag == "Nemico Testa" || this.targetSparo.collider.gameObject.tag == "Nemico Coll Suppl")
			{
				if (Vector3.Distance(base.transform.position, this.targetSparo.point) > this.ListaMunizioniAttiveUnità[3].GetComponent<DatiGeneraliMunizione>().portataMinima && Vector3.Distance(base.transform.position, this.targetSparo.point) <= this.ListaMunizioniAttiveUnità[3].GetComponent<DatiGeneraliMunizione>().portataMassima)
				{
					base.GetComponent<PresenzaAlleato>().ListaFuoriPortataArmi[3] = false;
				}
				else
				{
					base.GetComponent<PresenzaAlleato>().ListaFuoriPortataArmi[3] = true;
				}
			}
			else
			{
				base.GetComponent<PresenzaAlleato>().ListaFuoriPortataArmi[3] = false;
			}
		}
		if (Input.GetMouseButton(0) && !base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[3] && base.GetComponent<PresenzaAlleato>().ListaArmi[3][5] > 0f && this.timerFrequenzaArma4 > base.GetComponent<PresenzaAlleato>().ListaArmi[3][1])
		{
			this.timerFrequenzaArma4 = 0f;
			this.SparoArma4();
			this.suonoArma4.Play();
			this.bocca4Particelle.Play();
			List<float> list;
			List<float> expr_1EA = list = base.GetComponent<PresenzaAlleato>().ListaArmi[3];
			int index;
			int expr_1ED = index = 5;
			float num = list[index];
			expr_1EA[expr_1ED] = num - 1f;
			List<float> list2;
			List<float> expr_214 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[3];
			int expr_218 = index = 6;
			num = list2[index];
			expr_214[expr_218] = num - 1f;
			this.timerDopoSparo4 = 0f;
		}
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x00097B60 File Offset: 0x00095D60
	private void SparoArma4()
	{
		this.proiettileCarro4 = (UnityEngine.Object.Instantiate(this.munizioneArma4, this.bocca4.transform.position, this.bocca4.transform.rotation) as GameObject);
		this.proiettileCarro4.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.proiettileCarro4.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x04000F09 RID: 3849
	private GameObject infoNeutreTattica;

	// Token: 0x04000F0A RID: 3850
	private GameObject primaCamera;

	// Token: 0x04000F0B RID: 3851
	private GameObject infoAlleati;

	// Token: 0x04000F0C RID: 3852
	public GameObject bocca11;

	// Token: 0x04000F0D RID: 3853
	public GameObject bocca12;

	// Token: 0x04000F0E RID: 3854
	public GameObject bocca21;

	// Token: 0x04000F0F RID: 3855
	public GameObject bocca22;

	// Token: 0x04000F10 RID: 3856
	public GameObject bocca23;

	// Token: 0x04000F11 RID: 3857
	public GameObject bocca31;

	// Token: 0x04000F12 RID: 3858
	public GameObject bocca32;

	// Token: 0x04000F13 RID: 3859
	public GameObject bocca33;

	// Token: 0x04000F14 RID: 3860
	public GameObject bocca4;

	// Token: 0x04000F15 RID: 3861
	private GameObject baseCannoni1;

	// Token: 0x04000F16 RID: 3862
	private GameObject baseCannoni2;

	// Token: 0x04000F17 RID: 3863
	private GameObject baseCannoni3;

	// Token: 0x04000F18 RID: 3864
	private GameObject baseCannoni4;

	// Token: 0x04000F19 RID: 3865
	private GameObject terzaCamera;

	// Token: 0x04000F1A RID: 3866
	private GameObject IANemico;

	// Token: 0x04000F1B RID: 3867
	private GameObject CanvasFPS;

	// Token: 0x04000F1C RID: 3868
	private GameObject mirinoElettr1;

	// Token: 0x04000F1D RID: 3869
	public Sprite mirinoTPS;

	// Token: 0x04000F1E RID: 3870
	public Sprite mirinoFPS;

	// Token: 0x04000F1F RID: 3871
	public GameObject baseTorretta1;

	// Token: 0x04000F20 RID: 3872
	public GameObject baseTorretta2;

	// Token: 0x04000F21 RID: 3873
	public GameObject baseTorretta3;

	// Token: 0x04000F22 RID: 3874
	public GameObject baseTorretta4;

	// Token: 0x04000F23 RID: 3875
	private float timerFrequenzaArma1;

	// Token: 0x04000F24 RID: 3876
	private float timerRicarica1;

	// Token: 0x04000F25 RID: 3877
	private bool ricaricaInCorso1;

	// Token: 0x04000F26 RID: 3878
	private float timerDopoSparo1;

	// Token: 0x04000F27 RID: 3879
	private float tempoFraSparoERicarica1;

	// Token: 0x04000F28 RID: 3880
	private float timerFrequenzaArma2;

	// Token: 0x04000F29 RID: 3881
	private float timerRicarica2;

	// Token: 0x04000F2A RID: 3882
	private bool ricaricaInCorso2;

	// Token: 0x04000F2B RID: 3883
	private float timerDopoSparo2;

	// Token: 0x04000F2C RID: 3884
	private float tempoFraSparoERicarica2;

	// Token: 0x04000F2D RID: 3885
	private float timerFrequenzaArma3;

	// Token: 0x04000F2E RID: 3886
	private float timerRicarica3;

	// Token: 0x04000F2F RID: 3887
	private bool ricaricaInCorso3;

	// Token: 0x04000F30 RID: 3888
	private float timerDopoSparo3;

	// Token: 0x04000F31 RID: 3889
	private float tempoFraSparoERicarica3;

	// Token: 0x04000F32 RID: 3890
	private float timerFrequenzaArma4;

	// Token: 0x04000F33 RID: 3891
	private float timerRicarica4;

	// Token: 0x04000F34 RID: 3892
	private bool ricaricaInCorso4;

	// Token: 0x04000F35 RID: 3893
	private float timerDopoSparo4;

	// Token: 0x04000F36 RID: 3894
	private float tempoFraSparoERicarica4;

	// Token: 0x04000F37 RID: 3895
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000F38 RID: 3896
	public Vector3 posizionamentoCameraFPS1;

	// Token: 0x04000F39 RID: 3897
	public Vector3 posizionamentoCameraFPS2;

	// Token: 0x04000F3A RID: 3898
	public Vector3 posizionamentoCameraFPS3;

	// Token: 0x04000F3B RID: 3899
	public Vector3 posizionamentoCameraFPS4;

	// Token: 0x04000F3C RID: 3900
	private float timerPosizionamentoTPS;

	// Token: 0x04000F3D RID: 3901
	private float timerPosizionamentoFPS;

	// Token: 0x04000F3E RID: 3902
	private bool zoomAttivo;

	// Token: 0x04000F3F RID: 3903
	private AudioSource suonoArma11;

	// Token: 0x04000F40 RID: 3904
	private AudioSource suonoArma12;

	// Token: 0x04000F41 RID: 3905
	private AudioSource suonoArma21;

	// Token: 0x04000F42 RID: 3906
	private AudioSource suonoArma22;

	// Token: 0x04000F43 RID: 3907
	private AudioSource suonoArma23;

	// Token: 0x04000F44 RID: 3908
	private AudioSource suonoArma31;

	// Token: 0x04000F45 RID: 3909
	private AudioSource suonoArma32;

	// Token: 0x04000F46 RID: 3910
	private AudioSource suonoArma33;

	// Token: 0x04000F47 RID: 3911
	private AudioSource suonoArma4;

	// Token: 0x04000F48 RID: 3912
	private AudioSource suonoRicarica1;

	// Token: 0x04000F49 RID: 3913
	private AudioSource suonoRicarica2;

	// Token: 0x04000F4A RID: 3914
	private AudioSource suonoRicarica3;

	// Token: 0x04000F4B RID: 3915
	private AudioSource suonoRicarica4;

	// Token: 0x04000F4C RID: 3916
	private int armaAttivaInFPS;

	// Token: 0x04000F4D RID: 3917
	private int layerVisuale;

	// Token: 0x04000F4E RID: 3918
	private int layerColpo;

	// Token: 0x04000F4F RID: 3919
	public List<GameObject> ListaUnitàBersagli;

	// Token: 0x04000F50 RID: 3920
	private List<bool> ListaVisualiOscurate;

	// Token: 0x04000F51 RID: 3921
	private List<Vector3> ListaCentroInsetti;

	// Token: 0x04000F52 RID: 3922
	private List<bool> ListaBersNeiMirini;

	// Token: 0x04000F53 RID: 3923
	private List<GameObject> ListaBocceCampione;

	// Token: 0x04000F54 RID: 3924
	private List<float> ListaDistanzeDaBers;

	// Token: 0x04000F55 RID: 3925
	private List<GameObject> ListaBasiTorrette;

	// Token: 0x04000F56 RID: 3926
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x04000F57 RID: 3927
	private GameObject munizioneArma1;

	// Token: 0x04000F58 RID: 3928
	private GameObject munizioneArma2;

	// Token: 0x04000F59 RID: 3929
	private GameObject munizioneArma3;

	// Token: 0x04000F5A RID: 3930
	private GameObject munizioneArma4;

	// Token: 0x04000F5B RID: 3931
	private ParticleSystem bocca11Particelle;

	// Token: 0x04000F5C RID: 3932
	private ParticleSystem bocca12Particelle;

	// Token: 0x04000F5D RID: 3933
	private ParticleSystem bocca21Particelle;

	// Token: 0x04000F5E RID: 3934
	private ParticleSystem bocca22Particelle;

	// Token: 0x04000F5F RID: 3935
	private ParticleSystem bocca23Particelle;

	// Token: 0x04000F60 RID: 3936
	private ParticleSystem bocca31Particelle;

	// Token: 0x04000F61 RID: 3937
	private ParticleSystem bocca32Particelle;

	// Token: 0x04000F62 RID: 3938
	private ParticleSystem bocca33Particelle;

	// Token: 0x04000F63 RID: 3939
	private ParticleSystem bocca4Particelle;

	// Token: 0x04000F64 RID: 3940
	private List<GameObject> ListaBocche1;

	// Token: 0x04000F65 RID: 3941
	private int cannone1Attivo;

	// Token: 0x04000F66 RID: 3942
	private List<GameObject> ListaCannoni1;

	// Token: 0x04000F67 RID: 3943
	private List<bool> ListaBoolCannoni1Sparati;

	// Token: 0x04000F68 RID: 3944
	private List<bool> ListaBoolCannoni1InFondo;

	// Token: 0x04000F69 RID: 3945
	private List<AudioSource> ListaSuoniCannoni1;

	// Token: 0x04000F6A RID: 3946
	private List<ParticleSystem> ListaParticelleCannoni1;

	// Token: 0x04000F6B RID: 3947
	private List<GameObject> ListaBocche2;

	// Token: 0x04000F6C RID: 3948
	private int cannone2Attivo;

	// Token: 0x04000F6D RID: 3949
	private List<GameObject> ListaCannoni2;

	// Token: 0x04000F6E RID: 3950
	private List<bool> ListaBoolCannoni2Sparati;

	// Token: 0x04000F6F RID: 3951
	private List<bool> ListaBoolCannoni2InFondo;

	// Token: 0x04000F70 RID: 3952
	private List<AudioSource> ListaSuoniCannoni2;

	// Token: 0x04000F71 RID: 3953
	private List<ParticleSystem> ListaParticelleCannoni2;

	// Token: 0x04000F72 RID: 3954
	private List<GameObject> ListaBocche3;

	// Token: 0x04000F73 RID: 3955
	private int cannone3Attivo;

	// Token: 0x04000F74 RID: 3956
	private List<GameObject> ListaCannoni3;

	// Token: 0x04000F75 RID: 3957
	private List<bool> ListaBoolCannoni3Sparati;

	// Token: 0x04000F76 RID: 3958
	private List<bool> ListaBoolCannoni3InFondo;

	// Token: 0x04000F77 RID: 3959
	private List<AudioSource> ListaSuoniCannoni3;

	// Token: 0x04000F78 RID: 3960
	private List<ParticleSystem> ListaParticelleCannoni3;

	// Token: 0x04000F79 RID: 3961
	private GameObject proiettileCarro1;

	// Token: 0x04000F7A RID: 3962
	private GameObject proiettileCarro2;

	// Token: 0x04000F7B RID: 3963
	private GameObject proiettileCarro3;

	// Token: 0x04000F7C RID: 3964
	private GameObject proiettileCarro4;

	// Token: 0x04000F7D RID: 3965
	private RaycastHit targetSparo;

	// Token: 0x04000F7E RID: 3966
	private AudioSource suonoTorretta;

	// Token: 0x04000F7F RID: 3967
	private AudioSource suonoInterno;

	// Token: 0x04000F80 RID: 3968
	private AudioSource suonoMotore;

	// Token: 0x04000F81 RID: 3969
	public AudioClip motoreFermo;

	// Token: 0x04000F82 RID: 3970
	public AudioClip motorePartenza;

	// Token: 0x04000F83 RID: 3971
	public AudioClip motoreViaggio;

	// Token: 0x04000F84 RID: 3972
	public AudioClip motoreStop;

	// Token: 0x04000F85 RID: 3973
	private float timerPartenza;

	// Token: 0x04000F86 RID: 3974
	private float timerStop;

	// Token: 0x04000F87 RID: 3975
	private bool primaPartenza;

	// Token: 0x04000F88 RID: 3976
	public float volumeMotoreIniziale;

	// Token: 0x04000F89 RID: 3977
	private bool inPartenza;

	// Token: 0x04000F8A RID: 3978
	private bool partenzaFinita;

	// Token: 0x04000F8B RID: 3979
	private bool inStop;

	// Token: 0x04000F8C RID: 3980
	public bool stopFinito;

	// Token: 0x04000F8D RID: 3981
	private NavMeshAgent alleatoNav;

	// Token: 0x04000F8E RID: 3982
	private float distFineOrdineMovimento;

	// Token: 0x04000F8F RID: 3983
	private GameObject unitàBersaglio;

	// Token: 0x04000F90 RID: 3984
	private Vector3 centroUnitàBersaglio;
}
