using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class ATT_Spitlead : MonoBehaviour
{
	// Token: 0x0600010C RID: 268 RVA: 0x0002F194 File Offset: 0x0002D394
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
		this.ListaBocceCampione.Add(this.bocca41);
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
		this.ListaBasiTorretteFPS = new List<GameObject>();
		this.ListaBasiTorretteFPS.Add(this.baseTorretta1FPS);
		this.ListaBasiTorretteFPS.Add(this.baseTorretta2FPS);
		this.ListaBasiTorretteFPS.Add(this.baseTorretta3FPS);
		this.ListaBasiTorretteFPS.Add(this.baseTorretta4FPS);
		this.ListaMunizioniAttiveUnità = new List<GameObject>();
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma1);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma2);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma2);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma3);
		this.bocca11Particelle = this.bocca11.GetComponent<ParticleSystem>();
		this.bocca12Particelle = this.bocca12.GetComponent<ParticleSystem>();
		this.bocca21Particelle = this.bocca21.GetComponent<ParticleSystem>();
		this.bocca22Particelle = this.bocca22.GetComponent<ParticleSystem>();
		this.bocca31Particelle = this.bocca31.GetComponent<ParticleSystem>();
		this.bocca32Particelle = this.bocca32.GetComponent<ParticleSystem>();
		this.bocca41Particelle = this.bocca41.GetComponent<ParticleSystem>();
		this.bocca42Particelle = this.bocca42.GetComponent<ParticleSystem>();
		this.bocca11FPSParticelle = this.bocca11FPS.GetComponent<ParticleSystem>();
		this.bocca12FPSParticelle = this.bocca12FPS.GetComponent<ParticleSystem>();
		this.bocca21FPSParticelle = this.bocca21FPS.GetComponent<ParticleSystem>();
		this.bocca22FPSParticelle = this.bocca22FPS.GetComponent<ParticleSystem>();
		this.bocca31FPSParticelle = this.bocca31FPS.GetComponent<ParticleSystem>();
		this.bocca32FPSParticelle = this.bocca32FPS.GetComponent<ParticleSystem>();
		this.bocca41FPSParticelle = this.bocca41FPS.GetComponent<ParticleSystem>();
		this.bocca42FPSParticelle = this.bocca42FPS.GetComponent<ParticleSystem>();
		this.colpiBocca11 = this.bocca11.transform.GetChild(0).gameObject;
		this.particelleColpiBocca11 = this.colpiBocca11.GetComponent<ParticleSystem>();
		this.colpiBocca12 = this.bocca12.transform.GetChild(0).gameObject;
		this.particelleColpiBocca12 = this.colpiBocca12.GetComponent<ParticleSystem>();
		this.colpiBocca41 = this.bocca41.transform.GetChild(0).gameObject;
		this.particelleColpiBocca41 = this.colpiBocca41.GetComponent<ParticleSystem>();
		this.colpiBocca42 = this.bocca42.transform.GetChild(0).gameObject;
		this.particelleColpiBocca42 = this.colpiBocca42.GetComponent<ParticleSystem>();
		this.colpiBocca11FPS = this.bocca11FPS.transform.GetChild(0).gameObject;
		this.particelleColpiBocca11FPS = this.colpiBocca11FPS.GetComponent<ParticleSystem>();
		this.colpiBocca12FPS = this.bocca12FPS.transform.GetChild(0).gameObject;
		this.particelleColpiBocca12FPS = this.colpiBocca12FPS.GetComponent<ParticleSystem>();
		this.colpiBocca41FPS = this.bocca41FPS.transform.GetChild(0).gameObject;
		this.particelleColpiBocca41FPS = this.colpiBocca41FPS.GetComponent<ParticleSystem>();
		this.colpiBocca42FPS = this.bocca42FPS.transform.GetChild(0).gameObject;
		this.particelleColpiBocca42FPS = this.colpiBocca42FPS.GetComponent<ParticleSystem>();
		this.suonoArma11 = this.bocca11.GetComponent<AudioSource>();
		this.suonoArma12 = this.bocca12.GetComponent<AudioSource>();
		this.suonoArma21 = this.bocca21.GetComponent<AudioSource>();
		this.suonoArma22 = this.bocca22.GetComponent<AudioSource>();
		this.suonoArma31 = this.bocca31.GetComponent<AudioSource>();
		this.suonoArma32 = this.bocca32.GetComponent<AudioSource>();
		this.suonoArma41 = this.bocca41.GetComponent<AudioSource>();
		this.suonoArma42 = this.bocca42.GetComponent<AudioSource>();
		this.suonoArma11FPS = this.bocca11FPS.GetComponent<AudioSource>();
		this.suonoArma12FPS = this.bocca12FPS.GetComponent<AudioSource>();
		this.suonoArma21FPS = this.bocca21FPS.GetComponent<AudioSource>();
		this.suonoArma22FPS = this.bocca22FPS.GetComponent<AudioSource>();
		this.suonoArma31FPS = this.bocca31FPS.GetComponent<AudioSource>();
		this.suonoArma32FPS = this.bocca32FPS.GetComponent<AudioSource>();
		this.suonoArma41FPS = this.bocca41FPS.GetComponent<AudioSource>();
		this.suonoArma42FPS = this.bocca42FPS.GetComponent<AudioSource>();
		this.ListaBocche2 = new List<GameObject>();
		this.ListaBocche2.Add(this.bocca21);
		this.ListaBocche2.Add(this.bocca22);
		this.ListaCannoni2 = new List<GameObject>();
		this.ListaCannoni2.Add(this.bocca21.transform.parent.gameObject);
		this.ListaCannoni2.Add(this.bocca22.transform.parent.gameObject);
		this.ListaBoolCannoni2Sparati = new List<bool>();
		this.ListaBoolCannoni2Sparati.Add(false);
		this.ListaBoolCannoni2Sparati.Add(false);
		this.ListaBoolCannoni2InFondo = new List<bool>();
		this.ListaBoolCannoni2InFondo.Add(false);
		this.ListaBoolCannoni2InFondo.Add(false);
		this.ListaSuoniCannoni2 = new List<AudioSource>();
		this.ListaSuoniCannoni2.Add(this.suonoArma21);
		this.ListaSuoniCannoni2.Add(this.suonoArma22);
		this.ListaParticelleCannoni2 = new List<ParticleSystem>();
		this.ListaParticelleCannoni2.Add(this.bocca21Particelle);
		this.ListaParticelleCannoni2.Add(this.bocca22Particelle);
		this.ListaBocche3 = new List<GameObject>();
		this.ListaBocche3.Add(this.bocca31);
		this.ListaBocche3.Add(this.bocca32);
		this.ListaCannoni3 = new List<GameObject>();
		this.ListaCannoni3.Add(this.bocca31.transform.parent.gameObject);
		this.ListaCannoni3.Add(this.bocca32.transform.parent.gameObject);
		this.ListaBoolCannoni3Sparati = new List<bool>();
		this.ListaBoolCannoni3Sparati.Add(false);
		this.ListaBoolCannoni3Sparati.Add(false);
		this.ListaBoolCannoni3InFondo = new List<bool>();
		this.ListaBoolCannoni3InFondo.Add(false);
		this.ListaBoolCannoni3InFondo.Add(false);
		this.ListaSuoniCannoni3 = new List<AudioSource>();
		this.ListaSuoniCannoni3.Add(this.suonoArma31);
		this.ListaSuoniCannoni3.Add(this.suonoArma32);
		this.ListaParticelleCannoni3 = new List<ParticleSystem>();
		this.ListaParticelleCannoni3.Add(this.bocca31Particelle);
		this.ListaParticelleCannoni3.Add(this.bocca32Particelle);
		this.ListaBocche2FPS = new List<GameObject>();
		this.ListaBocche2FPS.Add(this.bocca21FPS);
		this.ListaBocche2FPS.Add(this.bocca22FPS);
		this.ListaCannoni2FPS = new List<GameObject>();
		this.ListaCannoni2FPS.Add(this.bocca21FPS.transform.parent.gameObject);
		this.ListaCannoni2FPS.Add(this.bocca22FPS.transform.parent.gameObject);
		this.ListaBoolCannoni2SparatiFPS = new List<bool>();
		this.ListaBoolCannoni2SparatiFPS.Add(false);
		this.ListaBoolCannoni2SparatiFPS.Add(false);
		this.ListaBoolCannoni2InFondoFPS = new List<bool>();
		this.ListaBoolCannoni2InFondoFPS.Add(false);
		this.ListaBoolCannoni2InFondoFPS.Add(false);
		this.ListaSuoniCannoni2FPS = new List<AudioSource>();
		this.ListaSuoniCannoni2FPS.Add(this.suonoArma21FPS);
		this.ListaSuoniCannoni2FPS.Add(this.suonoArma22FPS);
		this.ListaParticelleCannoni2FPS = new List<ParticleSystem>();
		this.ListaParticelleCannoni2FPS.Add(this.bocca21FPSParticelle);
		this.ListaParticelleCannoni2FPS.Add(this.bocca22FPSParticelle);
		this.ListaBocche3FPS = new List<GameObject>();
		this.ListaBocche3FPS.Add(this.bocca31FPS);
		this.ListaBocche3FPS.Add(this.bocca32FPS);
		this.ListaCannoni3FPS = new List<GameObject>();
		this.ListaCannoni3FPS.Add(this.bocca31FPS.transform.parent.gameObject);
		this.ListaCannoni3FPS.Add(this.bocca32FPS.transform.parent.gameObject);
		this.ListaBoolCannoni3SparatiFPS = new List<bool>();
		this.ListaBoolCannoni3SparatiFPS.Add(false);
		this.ListaBoolCannoni3SparatiFPS.Add(false);
		this.ListaBoolCannoni3InFondoFPS = new List<bool>();
		this.ListaBoolCannoni3InFondoFPS.Add(false);
		this.ListaBoolCannoni3InFondoFPS.Add(false);
		this.ListaSuoniCannoni3FPS = new List<AudioSource>();
		this.ListaSuoniCannoni3FPS.Add(this.suonoArma31FPS);
		this.ListaSuoniCannoni3FPS.Add(this.suonoArma32FPS);
		this.ListaParticelleCannoni3FPS = new List<ParticleSystem>();
		this.ListaParticelleCannoni3FPS.Add(this.bocca31FPSParticelle);
		this.ListaParticelleCannoni3FPS.Add(this.bocca32FPSParticelle);
		this.suonoRicarica1 = this.baseTorretta1.GetComponent<AudioSource>();
		this.suonoRicarica2 = this.baseTorretta2.GetComponent<AudioSource>();
		this.suonoRicarica3 = this.baseTorretta3.GetComponent<AudioSource>();
		this.suonoRicarica4 = this.baseTorretta4.GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.suonoInterno = base.transform.GetChild(2).GetComponent<AudioSource>();
		this.volumeMotoreIniziale = base.GetComponent<AudioSource>().volume;
		if (GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.moltiplicatoreAttaccoInFPS = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().moltiplicatoreFPSBattVeloce;
		}
		else
		{
			this.moltiplicatoreAttaccoInFPS = PlayerPrefs.GetFloat("moltiplicatore danni PP");
		}
		this.ListaTimerAggRicerca = new List<float>();
		this.ListaTimerAggRicerca.Add(this.timerAggRicerca1);
		this.ListaTimerAggRicerca.Add(this.timerAggRicerca2);
		this.ListaTimerAggRicerca.Add(this.timerAggRicerca3);
		this.ListaTimerAggRicerca.Add(this.timerAggRicerca4);
		if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().tipoBattaglia == 5)
		{
			this.èBattaglia5 = true;
			base.GetComponent<PresenzaAlleato>().raggioVisivo = 9999f;
			base.GetComponent<PresenzaAlleato>().vita = 10000f;
			base.GetComponent<PresenzaAlleato>().vitaIniziale = base.GetComponent<PresenzaAlleato>().vita;
			base.GetComponent<PresenzaAlleato>().carburante = 10000f;
			base.GetComponent<PresenzaAlleato>().carburanteIniziale = base.GetComponent<PresenzaAlleato>().carburante;
			this.suonoRifor = base.transform.GetChild(0).GetComponent<AudioSource>();
		}
	}

	// Token: 0x0600010D RID: 269 RVA: 0x0002FE18 File Offset: 0x0002E018
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
		this.GestioneCannoni();
		this.PreparazioneAttacco();
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
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
			if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva == 3)
			{
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
					if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 0)
					{
						this.ListaBasiTorrette[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
						this.ListaBasiTorrette[0].transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = false;
						this.ListaBasiTorrette[0].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().enabled = false;
						this.ListaBasiTorretteFPS[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
						this.ListaBasiTorretteFPS[0].transform.forward = base.transform.forward;
					}
					if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 1)
					{
						this.ListaBasiTorrette[1].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
						this.ListaBasiTorrette[1].transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = false;
						this.ListaBasiTorrette[1].transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = false;
						this.ListaBasiTorrette[1].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().enabled = false;
						this.ListaBasiTorrette[1].transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = false;
						this.ListaBasiTorretteFPS[1].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
						this.ListaBasiTorretteFPS[1].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
						this.ListaBasiTorretteFPS[1].transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
						this.ListaBasiTorretteFPS[1].transform.forward = base.transform.forward;
					}
					if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 2)
					{
						this.ListaBasiTorrette[2].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
						this.ListaBasiTorrette[2].transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = false;
						this.ListaBasiTorrette[2].transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = false;
						this.ListaBasiTorrette[2].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().enabled = false;
						this.ListaBasiTorrette[2].transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = false;
						this.ListaBasiTorretteFPS[2].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
						this.ListaBasiTorretteFPS[2].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
						this.ListaBasiTorretteFPS[2].transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
						this.ListaBasiTorretteFPS[2].transform.forward = base.transform.forward;
					}
					else if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 3)
					{
						this.ListaBasiTorrette[3].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
						this.ListaBasiTorrette[3].transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = false;
						this.ListaBasiTorrette[3].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().enabled = false;
						this.ListaBasiTorretteFPS[3].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
						this.ListaBasiTorretteFPS[3].transform.forward = -base.transform.forward;
					}
					this.suonoInterno.GetComponent<AudioSource>().Play();
					this.suonoMotore.volume = this.volumeMotoreIniziale / 2.1f;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					this.ListaBasiTorrette[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					this.ListaBasiTorrette[0].transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					this.ListaBasiTorrette[0].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().enabled = true;
					this.ListaBasiTorretteFPS[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
					this.ListaBasiTorrette[1].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					this.ListaBasiTorrette[1].transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					this.ListaBasiTorrette[1].transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					this.ListaBasiTorrette[1].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().enabled = true;
					this.ListaBasiTorrette[1].transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					this.ListaBasiTorretteFPS[1].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
					this.ListaBasiTorretteFPS[1].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
					this.ListaBasiTorretteFPS[1].transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
					this.ListaBasiTorrette[2].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					this.ListaBasiTorrette[2].transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					this.ListaBasiTorrette[2].transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					this.ListaBasiTorrette[2].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().enabled = true;
					this.ListaBasiTorrette[2].transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					this.ListaBasiTorretteFPS[2].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
					this.ListaBasiTorretteFPS[2].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
					this.ListaBasiTorretteFPS[2].transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
					this.ListaBasiTorrette[3].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					this.ListaBasiTorrette[3].transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					this.ListaBasiTorrette[3].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().enabled = true;
					this.ListaBasiTorretteFPS[3].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
					this.suonoInterno.GetComponent<AudioSource>().Stop();
					this.suonoMotore.volume = this.volumeMotoreIniziale;
				}
			}
		}
		else
		{
			this.ControlloArmiPrimarie();
		}
		if (this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0 && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0] == base.gameObject)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona = false;
			this.ListaBasiTorrette[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
			this.ListaBasiTorrette[0].transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
			this.ListaBasiTorrette[0].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().enabled = true;
			this.ListaBasiTorretteFPS[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			this.ListaBasiTorrette[1].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
			this.ListaBasiTorrette[1].transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
			this.ListaBasiTorrette[1].transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
			this.ListaBasiTorrette[1].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().enabled = true;
			this.ListaBasiTorrette[1].transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
			this.ListaBasiTorretteFPS[1].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			this.ListaBasiTorretteFPS[1].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
			this.ListaBasiTorretteFPS[1].transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
			this.ListaBasiTorrette[2].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
			this.ListaBasiTorrette[2].transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
			this.ListaBasiTorrette[2].transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
			this.ListaBasiTorrette[2].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().enabled = true;
			this.ListaBasiTorrette[2].transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
			this.ListaBasiTorretteFPS[2].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			this.ListaBasiTorretteFPS[2].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
			this.ListaBasiTorretteFPS[2].transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
			this.ListaBasiTorrette[3].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
			this.ListaBasiTorrette[3].transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
			this.ListaBasiTorrette[3].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().enabled = true;
			this.ListaBasiTorretteFPS[3].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			this.suonoInterno.GetComponent<AudioSource>().Stop();
			this.suonoMotore.volume = this.volumeMotoreIniziale;
			this.timerPosizionamentoTPS = 0f;
			this.timerPosizionamentoFPS = 0f;
		}
		if (this.èBattaglia5)
		{
			this.RifornimentoBattaglia5();
		}
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00030D54 File Offset: 0x0002EF54
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

	// Token: 0x0600010F RID: 271 RVA: 0x00030FB4 File Offset: 0x0002F1B4
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

	// Token: 0x06000110 RID: 272 RVA: 0x00031214 File Offset: 0x0002F414
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

	// Token: 0x06000111 RID: 273 RVA: 0x00031474 File Offset: 0x0002F674
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

	// Token: 0x06000112 RID: 274 RVA: 0x000316D4 File Offset: 0x0002F8D4
	private void GestioneCannoni()
	{
		if (this.cannone2Attivo >= 2)
		{
			this.cannone2Attivo = 0;
		}
		for (int i = 0; i <= 1; i++)
		{
			if (this.ListaBoolCannoni2Sparati[i])
			{
				if (this.ListaCannoni2[i].transform.localPosition.y < -0.7f && !this.ListaBoolCannoni2InFondo[i])
				{
					this.ListaCannoni2[i].transform.localPosition += Vector3.up * 3f * Time.deltaTime;
				}
				if (this.ListaCannoni2[i].transform.localPosition.y >= -0.7f && !this.ListaBoolCannoni2InFondo[i])
				{
					this.ListaBoolCannoni2InFondo[i] = true;
				}
				if (this.ListaCannoni2[i].transform.localPosition.y > -1.4f && this.ListaBoolCannoni2InFondo[i])
				{
					this.ListaCannoni2[i].transform.localPosition += -Vector3.up * 1f * Time.deltaTime;
				}
				if (this.ListaCannoni2[i].transform.localPosition.y <= -1.4f && this.ListaBoolCannoni2InFondo[i])
				{
					this.ListaBoolCannoni2InFondo[i] = false;
					this.ListaBoolCannoni2Sparati[i] = false;
				}
			}
		}
		if (this.cannone3Attivo >= 2)
		{
			this.cannone3Attivo = 0;
		}
		for (int j = 0; j <= 1; j++)
		{
			if (this.ListaBoolCannoni3Sparati[j])
			{
				if (this.ListaCannoni3[j].transform.localPosition.y < -0.7f && !this.ListaBoolCannoni3InFondo[j])
				{
					this.ListaCannoni3[j].transform.localPosition += Vector3.up * 3f * Time.deltaTime;
				}
				if (this.ListaCannoni3[j].transform.localPosition.y >= -0.7f && !this.ListaBoolCannoni3InFondo[j])
				{
					this.ListaBoolCannoni3InFondo[j] = true;
				}
				if (this.ListaCannoni3[j].transform.localPosition.y > -1.4f && this.ListaBoolCannoni3InFondo[j])
				{
					this.ListaCannoni3[j].transform.localPosition += -Vector3.up * 1f * Time.deltaTime;
				}
				if (this.ListaCannoni3[j].transform.localPosition.y <= -1.4f && this.ListaBoolCannoni3InFondo[j])
				{
					this.ListaBoolCannoni3InFondo[j] = false;
					this.ListaBoolCannoni3Sparati[j] = false;
				}
			}
		}
		if (this.cannone2AttivoFPS >= 2)
		{
			this.cannone2AttivoFPS = 0;
		}
		for (int k = 0; k <= 1; k++)
		{
			if (this.ListaBoolCannoni2SparatiFPS[k])
			{
				if (this.ListaCannoni2FPS[k].transform.localPosition.z > 1.4f && !this.ListaBoolCannoni2InFondoFPS[k])
				{
					this.ListaCannoni2FPS[k].transform.localPosition += -Vector3.forward * 5f * Time.deltaTime;
				}
				if (this.ListaCannoni2FPS[k].transform.localPosition.z <= 1.4f && !this.ListaBoolCannoni2InFondoFPS[k])
				{
					this.ListaBoolCannoni2InFondoFPS[k] = true;
				}
				if (this.ListaCannoni2FPS[k].transform.localPosition.z < 2.41f && this.ListaBoolCannoni2InFondoFPS[k])
				{
					this.ListaCannoni2FPS[k].transform.localPosition += Vector3.forward * 0.8f * Time.deltaTime;
				}
				if (this.ListaCannoni2FPS[k].transform.localPosition.z >= 2.41f && this.ListaBoolCannoni2InFondoFPS[k])
				{
					this.ListaBoolCannoni2InFondoFPS[k] = false;
					this.ListaBoolCannoni2SparatiFPS[k] = false;
				}
			}
		}
		if (this.cannone3AttivoFPS >= 2)
		{
			this.cannone3AttivoFPS = 0;
		}
		for (int l = 0; l <= 1; l++)
		{
			if (this.ListaBoolCannoni3SparatiFPS[l])
			{
				if (this.ListaCannoni3FPS[l].transform.localPosition.z > 1.4f && !this.ListaBoolCannoni3InFondoFPS[l])
				{
					this.ListaCannoni3FPS[l].transform.localPosition += -Vector3.forward * 5f * Time.deltaTime;
				}
				if (this.ListaCannoni3FPS[l].transform.localPosition.z <= 1.4f && !this.ListaBoolCannoni3InFondoFPS[l])
				{
					this.ListaBoolCannoni3InFondoFPS[l] = true;
				}
				if (this.ListaCannoni3FPS[l].transform.localPosition.z < 2.41f && this.ListaBoolCannoni3InFondoFPS[l])
				{
					this.ListaCannoni3FPS[l].transform.localPosition += Vector3.forward * 0.8f * Time.deltaTime;
				}
				if (this.ListaCannoni3FPS[l].transform.localPosition.z >= 2.41f && this.ListaBoolCannoni3InFondoFPS[l])
				{
					this.ListaBoolCannoni3InFondoFPS[l] = false;
					this.ListaBoolCannoni3SparatiFPS[l] = false;
				}
			}
		}
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00031DDC File Offset: 0x0002FFDC
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
				this.terzaCamera.GetComponent<Camera>().fieldOfView = 50f;
				this.zoomAttivo = false;
			}
			else
			{
				this.terzaCamera.GetComponent<Camera>().fieldOfView = 20f;
				this.zoomAttivo = true;
			}
		}
	}

	// Token: 0x06000114 RID: 276 RVA: 0x00031E88 File Offset: 0x00030088
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

	// Token: 0x06000115 RID: 277 RVA: 0x00031F2C File Offset: 0x0003012C
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 0)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.baseTorretta1FPS.transform;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS1;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = this.baseTorretta1FPS.transform.rotation;
				this.terzaCamera.GetComponent<Camera>().fieldOfView = 50f;
			}
			else if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 1)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.baseTorretta2FPS.transform;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS2;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = this.baseTorretta2FPS.transform.rotation;
				this.terzaCamera.GetComponent<Camera>().fieldOfView = 50f;
			}
			else if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 2)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.baseTorretta3FPS.transform;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS3;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = this.baseTorretta3FPS.transform.rotation;
				this.terzaCamera.GetComponent<Camera>().fieldOfView = 50f;
			}
			else if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 3)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.baseTorretta4FPS.transform;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS4;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = this.baseTorretta4FPS.transform.rotation;
				this.terzaCamera.GetComponent<Camera>().fieldOfView = 50f;
			}
		}
	}

	// Token: 0x06000116 RID: 278 RVA: 0x00032184 File Offset: 0x00030384
	private void PreparazioneAttacco()
	{
		for (int i = 0; i < 4; i++)
		{
			bool flag = false;
			if (i != base.GetComponent<PresenzaAlleato>().armaAttivaInFPS)
			{
				flag = true;
			}
			else if (i == base.GetComponent<PresenzaAlleato>().armaAttivaInFPS && !this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				flag = true;
			}
			if (flag)
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
					float num = 0f;
					if (i == 0)
					{
						num = Vector3.Dot(this.ListaUnitàBersagli[0].transform.position - this.ListaBasiTorrette[0].transform.position, base.transform.forward);
					}
					else if (i == 1)
					{
						num = Vector3.Dot(this.ListaUnitàBersagli[1].transform.position - this.ListaBasiTorrette[1].transform.position, base.transform.up);
					}
					else if (i == 2)
					{
						num = Vector3.Dot(this.ListaUnitàBersagli[2].transform.position - this.ListaBasiTorrette[2].transform.position, -base.transform.up);
					}
					else if (i == 3)
					{
						num = Vector3.Dot(this.ListaUnitàBersagli[3].transform.position - this.ListaBasiTorrette[3].transform.position, -base.transform.forward);
					}
					if (!this.ListaVisualiOscurate[i] && num > 0f && Vector3.Distance(this.ListaBasiTorrette[i].transform.position, this.ListaUnitàBersagli[i].transform.position) < this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima)
					{
						this.ListaBersNeiMirini[i] = true;
					}
					else
					{
						this.ListaBersNeiMirini[i] = false;
						this.ListaUnitàBersagli[i] = null;
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
				else if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciVolanti.Count > 0)
				{
					List<float> listaTimerAggRicerca;
					List<float> expr_462 = listaTimerAggRicerca = this.ListaTimerAggRicerca;
					int index;
					int expr_466 = index = i;
					float num2 = listaTimerAggRicerca[index];
					expr_462[expr_466] = num2 + Time.deltaTime;
					if (this.ListaTimerAggRicerca[i] > 1f)
					{
						this.ListaTimerAggRicerca[i] = 0f;
						List<GameObject> list = new List<GameObject>();
						foreach (GameObject current in this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciVolanti)
						{
							if (current != null && current.GetComponent<PresenzaNemico>().èStatoVisto)
							{
								float num3 = Vector3.Distance(this.ListaBasiTorrette[i].transform.position, current.GetComponent<PresenzaNemico>().centroInsetto);
								if (num3 < this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima && !Physics.Linecast(this.ListaBocceCampione[i].transform.position, current.transform.position, this.layerVisuale))
								{
									float num4 = Vector3.Dot((current.GetComponent<PresenzaNemico>().centroInsetto - this.ListaBasiTorrette[i].transform.position).normalized, base.transform.up);
									float num5 = 0f;
									if (i == 0)
									{
										num5 = Vector3.Dot(current.transform.position - this.ListaBasiTorrette[0].transform.position, base.transform.forward);
									}
									else if (i == 1)
									{
										num5 = Vector3.Dot(current.transform.position - this.ListaBasiTorrette[1].transform.position, base.transform.up);
									}
									else if (i == 2)
									{
										num5 = Vector3.Dot(current.transform.position - this.ListaBasiTorrette[2].transform.position, -base.transform.up);
									}
									else if (i == 3)
									{
										num5 = Vector3.Dot(current.transform.position - this.ListaBasiTorrette[3].transform.position, -base.transform.forward);
									}
									if (num5 > 0f)
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
							this.ListaUnitàBersagli[i] = list[Mathf.FloorToInt(f)];
						}
					}
				}
			}
		}
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00032908 File Offset: 0x00030B08
	private void AttaccoIndipendente1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && this.ListaUnitàBersagli[0] && this.ListaUnitàBersagli[0].GetComponent<PresenzaNemico>().vita > 0f && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][0])
		{
			this.colpiBocca11.transform.LookAt(this.ListaUnitàBersagli[0].transform.position);
			this.colpiBocca12.transform.LookAt(this.ListaUnitàBersagli[0].transform.position);
			this.timerFrequenzaArma1 = 0f;
			List<float> list;
			List<float> expr_F7 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int index;
			int expr_FA = index = 5;
			float num = list[index];
			expr_F7[expr_FA] = num - 1f;
			List<float> list2;
			List<float> expr_121 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int expr_125 = index = 6;
			num = list2[index];
			expr_121[expr_125] = num - 1f;
			if (this.sparoConDestra1)
			{
				this.bocca12Particelle.Play();
				this.suonoArma12.Play();
				this.particelleColpiBocca12.Play();
			}
			else
			{
				this.bocca11Particelle.Play();
				this.suonoArma11.Play();
				this.particelleColpiBocca11.Play();
			}
			this.sparoConDestra1 = !this.sparoConDestra1;
			float num2 = 0f;
			if (this.ListaUnitàBersagli[0].GetComponent<PresenzaNemico>().vita > this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().penetrazione)
			{
				num2 = this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().penetrazione;
			}
			else if (this.ListaUnitàBersagli[0].GetComponent<PresenzaNemico>().vita > 0f)
			{
				num2 = this.ListaUnitàBersagli[0].GetComponent<PresenzaNemico>().vita;
			}
			this.ListaUnitàBersagli[0].GetComponent<PresenzaNemico>().vita -= this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().penetrazione;
			if (this.ListaUnitàBersagli[0].GetComponent<PresenzaNemico>().vita > this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.ListaUnitàBersagli[0].GetComponent<PresenzaNemico>().armatura))
			{
				num2 += this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.ListaUnitàBersagli[0].GetComponent<PresenzaNemico>().armatura);
			}
			else if (this.ListaUnitàBersagli[0].GetComponent<PresenzaNemico>().vita > 0f)
			{
				num2 += this.ListaUnitàBersagli[0].GetComponent<PresenzaNemico>().vita;
			}
			this.ListaUnitàBersagli[0].GetComponent<PresenzaNemico>().vita -= this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.ListaUnitàBersagli[0].GetComponent<PresenzaNemico>().armatura);
			List<float> listaDanniAlleati;
			List<float> expr_358 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
			int expr_366 = index = base.GetComponent<PresenzaAlleato>().tipoTruppa;
			num = listaDanniAlleati[index];
			expr_358[expr_366] = num + num2;
		}
	}

	// Token: 0x06000118 RID: 280 RVA: 0x00032C90 File Offset: 0x00030E90
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

	// Token: 0x06000119 RID: 281 RVA: 0x00032DD8 File Offset: 0x00030FD8
	private void SparoIndipendente2()
	{
		this.proiettileCarro2 = (UnityEngine.Object.Instantiate(this.munizioneArma2, this.ListaBocche2[this.cannone2Attivo].transform.position, this.ListaBocche2[this.cannone2Attivo].transform.rotation) as GameObject);
		this.proiettileCarro2.GetComponent<DatiProiettile>().locazioneTarget = this.ListaUnitàBersagli[1].transform.position;
		this.ListaBoolCannoni2Sparati[this.cannone2Attivo] = true;
		this.proiettileCarro2.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00032E84 File Offset: 0x00031084
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

	// Token: 0x0600011B RID: 283 RVA: 0x00032FCC File Offset: 0x000311CC
	private void SparoIndipendente3()
	{
		this.proiettileCarro3 = (UnityEngine.Object.Instantiate(this.munizioneArma3, this.ListaBocche3[this.cannone3Attivo].transform.position, this.ListaBocche3[this.cannone3Attivo].transform.rotation) as GameObject);
		this.proiettileCarro3.GetComponent<DatiProiettile>().locazioneTarget = this.ListaUnitàBersagli[2].transform.position;
		this.ListaBoolCannoni3Sparati[this.cannone3Attivo] = true;
		this.proiettileCarro3.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00033078 File Offset: 0x00031278
	private void AttaccoIndipendente4()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[3] && this.ListaUnitàBersagli[3] && this.ListaUnitàBersagli[3].GetComponent<PresenzaNemico>().vita > 0f && base.GetComponent<PresenzaAlleato>().ListaArmi[3][5] > 0f && this.timerFrequenzaArma4 > base.GetComponent<PresenzaAlleato>().ListaArmi[3][0])
		{
			this.colpiBocca41.transform.LookAt(this.ListaUnitàBersagli[3].transform.position);
			this.colpiBocca42.transform.LookAt(this.ListaUnitàBersagli[3].transform.position);
			this.timerFrequenzaArma4 = 0f;
			List<float> list;
			List<float> expr_F7 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[3];
			int index;
			int expr_FA = index = 5;
			float num = list[index];
			expr_F7[expr_FA] = num - 1f;
			List<float> list2;
			List<float> expr_121 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[3];
			int expr_125 = index = 6;
			num = list2[index];
			expr_121[expr_125] = num - 1f;
			if (this.sparoConDestra4)
			{
				this.bocca42Particelle.Play();
				this.suonoArma42.Play();
				this.particelleColpiBocca42.Play();
			}
			else
			{
				this.bocca41Particelle.Play();
				this.suonoArma41.Play();
				this.particelleColpiBocca41.Play();
			}
			this.sparoConDestra4 = !this.sparoConDestra4;
			float num2 = 0f;
			if (this.ListaUnitàBersagli[3].GetComponent<PresenzaNemico>().vita > this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().penetrazione)
			{
				num2 = this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().penetrazione;
			}
			else if (this.ListaUnitàBersagli[3].GetComponent<PresenzaNemico>().vita > 0f)
			{
				num2 = this.ListaUnitàBersagli[3].GetComponent<PresenzaNemico>().vita;
			}
			this.ListaUnitàBersagli[3].GetComponent<PresenzaNemico>().vita -= this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().penetrazione;
			if (this.ListaUnitàBersagli[3].GetComponent<PresenzaNemico>().vita > this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.ListaUnitàBersagli[3].GetComponent<PresenzaNemico>().armatura))
			{
				num2 += this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.ListaUnitàBersagli[3].GetComponent<PresenzaNemico>().armatura);
			}
			else if (this.ListaUnitàBersagli[3].GetComponent<PresenzaNemico>().vita > 0f)
			{
				num2 += this.ListaUnitàBersagli[3].GetComponent<PresenzaNemico>().vita;
			}
			this.ListaUnitàBersagli[3].GetComponent<PresenzaNemico>().vita -= this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.ListaUnitàBersagli[3].GetComponent<PresenzaNemico>().armatura);
			List<float> listaDanniAlleati;
			List<float> expr_358 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
			int expr_366 = index = base.GetComponent<PresenzaAlleato>().tipoTruppa;
			num = listaDanniAlleati[index];
			expr_358[expr_366] = num + num2;
		}
	}

	// Token: 0x0600011D RID: 285 RVA: 0x00033400 File Offset: 0x00031600
	private void SelezioneArma()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 0;
			this.timerPosizionamentoFPS = 0f;
			if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = true;
			}
			else if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = true;
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 1;
			this.timerPosizionamentoFPS = 0f;
			if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = true;
			}
			else if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = true;
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 2;
			this.timerPosizionamentoFPS = 0f;
			if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = true;
			}
			else if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = true;
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 3;
			this.timerPosizionamentoFPS = 0f;
			if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = true;
			}
			else if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = true;
			}
		}
	}

	// Token: 0x0600011E RID: 286 RVA: 0x000335E0 File Offset: 0x000317E0
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
			Vector3 normalized = (this.targetSparo.point - this.colpiBocca11.transform.position).normalized;
			this.colpiBocca11.transform.forward = normalized;
			normalized = (this.targetSparo.point - this.colpiBocca12.transform.position).normalized;
			this.colpiBocca12.transform.forward = normalized;
			this.timerFrequenzaArma1 = 0f;
			this.SparoArma1();
			if (this.sparoConDestra1)
			{
				this.bocca12FPSParticelle.Play();
				this.suonoArma12FPS.Play();
				this.particelleColpiBocca12.Play();
			}
			else
			{
				this.bocca11FPSParticelle.Play();
				this.suonoArma11FPS.Play();
				this.particelleColpiBocca11.Play();
			}
			this.sparoConDestra1 = !this.sparoConDestra1;
			List<float> list;
			List<float> expr_26D = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int index;
			int expr_271 = index = 5;
			float num = list[index];
			expr_26D[expr_271] = num - 1f;
			List<float> list2;
			List<float> expr_29D = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int expr_2A1 = index = 6;
			num = list2[index];
			expr_29D[expr_2A1] = num - 1f;
		}
	}

	// Token: 0x0600011F RID: 287 RVA: 0x000338AC File Offset: 0x00031AAC
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

	// Token: 0x06000120 RID: 288 RVA: 0x00033F04 File Offset: 0x00032104
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
			this.ListaSuoniCannoni2FPS[this.cannone2AttivoFPS].Play();
			this.ListaParticelleCannoni2FPS[this.cannone2AttivoFPS].Play();
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
			this.cannone2AttivoFPS++;
		}
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00034170 File Offset: 0x00032370
	private void SparoArma2()
	{
		this.proiettileCarro2 = (UnityEngine.Object.Instantiate(this.munizioneArma2, this.ListaBocche2FPS[this.cannone2AttivoFPS].transform.position, this.ListaBocche2FPS[this.cannone2AttivoFPS].transform.rotation) as GameObject);
		this.proiettileCarro2.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.ListaBoolCannoni2SparatiFPS[this.cannone2AttivoFPS] = true;
		this.proiettileCarro2.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00034208 File Offset: 0x00032408
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
			this.ListaSuoniCannoni3FPS[this.cannone3AttivoFPS].Play();
			this.ListaParticelleCannoni3FPS[this.cannone3AttivoFPS].Play();
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
			this.cannone3AttivoFPS++;
		}
	}

	// Token: 0x06000123 RID: 291 RVA: 0x00034474 File Offset: 0x00032674
	private void SparoArma3()
	{
		this.proiettileCarro3 = (UnityEngine.Object.Instantiate(this.munizioneArma3, this.ListaBocche3FPS[this.cannone3AttivoFPS].transform.position, this.ListaBocche3FPS[this.cannone3AttivoFPS].transform.rotation) as GameObject);
		this.proiettileCarro3.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.ListaBoolCannoni3SparatiFPS[this.cannone3AttivoFPS] = true;
		this.proiettileCarro3.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x06000124 RID: 292 RVA: 0x0003450C File Offset: 0x0003270C
	private void AttaccoPrimaPersonaArma4()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo))
		{
			if (this.targetSparo.collider.gameObject.tag == "Nemico" || this.targetSparo.collider.gameObject.tag == "Nemico Testa" || this.targetSparo.collider.gameObject.tag == "Nemico Coll Suppl")
			{
				if (Vector3.Distance(base.transform.position, this.targetSparo.point) <= this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().portataMassima)
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
			Vector3 normalized = (this.targetSparo.point - this.colpiBocca41.transform.position).normalized;
			this.colpiBocca41.transform.forward = normalized;
			normalized = (this.targetSparo.point - this.colpiBocca42.transform.position).normalized;
			this.colpiBocca42.transform.forward = normalized;
			this.timerFrequenzaArma4 = 0f;
			this.SparoArma4();
			if (this.sparoConDestra4)
			{
				this.bocca42FPSParticelle.Play();
				this.suonoArma42FPS.Play();
				this.particelleColpiBocca42.Play();
			}
			else
			{
				this.bocca41FPSParticelle.Play();
				this.suonoArma41FPS.Play();
				this.particelleColpiBocca41.Play();
			}
			this.sparoConDestra4 = !this.sparoConDestra4;
			List<float> list;
			List<float> expr_26D = list = base.GetComponent<PresenzaAlleato>().ListaArmi[3];
			int index;
			int expr_271 = index = 5;
			float num = list[index];
			expr_26D[expr_271] = num - 1f;
			List<float> list2;
			List<float> expr_29D = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[3];
			int expr_2A1 = index = 6;
			num = list2[index];
			expr_29D[expr_2A1] = num - 1f;
		}
	}

	// Token: 0x06000125 RID: 293 RVA: 0x000347D8 File Offset: 0x000329D8
	private void SparoArma4()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo, this.ListaMunizioniAttiveUnità[3].GetComponent<DatiGeneraliMunizione>().portataMassima, this.layerColpo))
		{
			if (this.targetSparo.collider.gameObject.tag == "Nemico")
			{
				GameObject gameObject = this.targetSparo.collider.gameObject;
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
				float num = 0f;
				if (gameObject.GetComponent<PresenzaNemico>().vita > this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
				{
					num = this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				}
				else if (gameObject.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num = gameObject.GetComponent<PresenzaNemico>().vita;
				}
				gameObject.GetComponent<PresenzaNemico>().vita -= this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				if (gameObject.GetComponent<PresenzaNemico>().vita > this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS)
				{
					num += this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
				}
				else if (gameObject.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num += gameObject.GetComponent<PresenzaNemico>().vita;
				}
				gameObject.GetComponent<PresenzaNemico>().vita -= this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
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
				if (gameObject2.GetComponent<PresenzaNemico>().vita > this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f)
				{
					num3 = this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f;
				}
				else if (gameObject2.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num3 = gameObject2.GetComponent<PresenzaNemico>().vita;
				}
				gameObject2.GetComponent<PresenzaNemico>().vita -= this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f;
				if (gameObject2.GetComponent<PresenzaNemico>().vita > this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject2.GetComponent<PresenzaNemico>().armatura) * 2f)
				{
					num3 += this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject2.GetComponent<PresenzaNemico>().armatura) * 2f;
				}
				else if (gameObject2.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num3 += gameObject2.GetComponent<PresenzaNemico>().vita;
				}
				gameObject2.GetComponent<PresenzaNemico>().vita -= this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject2.GetComponent<PresenzaNemico>().armatura) * 2f;
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
				if (gameObject3.GetComponent<PresenzaNemico>().vita > this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
				{
					num4 = this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				}
				else if (gameObject3.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num4 = gameObject3.GetComponent<PresenzaNemico>().vita;
				}
				gameObject3.GetComponent<PresenzaNemico>().vita -= this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				if (gameObject3.GetComponent<PresenzaNemico>().vita > this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject3.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS)
				{
					num4 += this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject3.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
				}
				else if (gameObject3.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num4 += gameObject3.GetComponent<PresenzaNemico>().vita;
				}
				gameObject3.GetComponent<PresenzaNemico>().vita -= this.munizioneArma4.GetComponent<DatiGeneraliMunizione>().danno * (1f - gameObject3.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
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

	// Token: 0x06000126 RID: 294 RVA: 0x00034E30 File Offset: 0x00033030
	private void RifornimentoBattaglia5()
	{
		this.timerDiRifornimento += Time.deltaTime;
		if (this.timerDiRifornimento > 30f)
		{
			this.suonoRifor.Play();
			foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati)
			{
				if (!current.GetComponent<PresenzaAlleato>().èPerRifornimento)
				{
					for (int i = 0; i < current.GetComponent<PresenzaAlleato>().numeroArmi; i++)
					{
						if (current.GetComponent<PresenzaAlleato>().ListaArmi[i][6] < current.GetComponent<PresenzaAlleato>().ListaArmi[i][4])
						{
							current.GetComponent<PresenzaAlleato>().rifornimentoAttivo = true;
						}
					}
				}
			}
			this.timerDiRifornimento = 0f;
		}
	}

	// Token: 0x06000127 RID: 295 RVA: 0x00034F3C File Offset: 0x0003313C
	private void ControlloArmiPrimarie()
	{
		bool flag = true;
		for (int i = 0; i < 4; i++)
		{
			if (base.GetComponent<PresenzaAlleato>().ListaArmi[i][5] > 0f)
			{
				flag = false;
			}
		}
		if (flag)
		{
			this.timerSenzaArmiPrimarie += Time.deltaTime;
			if (this.timerSenzaArmiPrimarie > 6f)
			{
				base.GetComponent<PresenzaAlleato>().carburante = 0f;
			}
		}
	}

	// Token: 0x04000502 RID: 1282
	private GameObject infoNeutreTattica;

	// Token: 0x04000503 RID: 1283
	private GameObject primaCamera;

	// Token: 0x04000504 RID: 1284
	private GameObject infoAlleati;

	// Token: 0x04000505 RID: 1285
	public GameObject bocca11;

	// Token: 0x04000506 RID: 1286
	public GameObject bocca12;

	// Token: 0x04000507 RID: 1287
	public GameObject bocca21;

	// Token: 0x04000508 RID: 1288
	public GameObject bocca22;

	// Token: 0x04000509 RID: 1289
	public GameObject bocca31;

	// Token: 0x0400050A RID: 1290
	public GameObject bocca32;

	// Token: 0x0400050B RID: 1291
	public GameObject bocca41;

	// Token: 0x0400050C RID: 1292
	public GameObject bocca42;

	// Token: 0x0400050D RID: 1293
	public GameObject bocca11FPS;

	// Token: 0x0400050E RID: 1294
	public GameObject bocca12FPS;

	// Token: 0x0400050F RID: 1295
	public GameObject bocca21FPS;

	// Token: 0x04000510 RID: 1296
	public GameObject bocca22FPS;

	// Token: 0x04000511 RID: 1297
	public GameObject bocca31FPS;

	// Token: 0x04000512 RID: 1298
	public GameObject bocca32FPS;

	// Token: 0x04000513 RID: 1299
	public GameObject bocca41FPS;

	// Token: 0x04000514 RID: 1300
	public GameObject bocca42FPS;

	// Token: 0x04000515 RID: 1301
	private GameObject colpiBocca11;

	// Token: 0x04000516 RID: 1302
	private GameObject colpiBocca12;

	// Token: 0x04000517 RID: 1303
	private GameObject colpiBocca41;

	// Token: 0x04000518 RID: 1304
	private GameObject colpiBocca42;

	// Token: 0x04000519 RID: 1305
	private GameObject colpiBocca11FPS;

	// Token: 0x0400051A RID: 1306
	private GameObject colpiBocca12FPS;

	// Token: 0x0400051B RID: 1307
	private GameObject colpiBocca41FPS;

	// Token: 0x0400051C RID: 1308
	private GameObject colpiBocca42FPS;

	// Token: 0x0400051D RID: 1309
	private GameObject terzaCamera;

	// Token: 0x0400051E RID: 1310
	private GameObject IANemico;

	// Token: 0x0400051F RID: 1311
	public GameObject baseTorretta1;

	// Token: 0x04000520 RID: 1312
	public GameObject baseTorretta2;

	// Token: 0x04000521 RID: 1313
	public GameObject baseTorretta3;

	// Token: 0x04000522 RID: 1314
	public GameObject baseTorretta4;

	// Token: 0x04000523 RID: 1315
	public GameObject baseTorretta1FPS;

	// Token: 0x04000524 RID: 1316
	public GameObject baseTorretta2FPS;

	// Token: 0x04000525 RID: 1317
	public GameObject baseTorretta3FPS;

	// Token: 0x04000526 RID: 1318
	public GameObject baseTorretta4FPS;

	// Token: 0x04000527 RID: 1319
	private float timerFrequenzaArma1;

	// Token: 0x04000528 RID: 1320
	private float timerRicarica1;

	// Token: 0x04000529 RID: 1321
	private bool ricaricaInCorso1;

	// Token: 0x0400052A RID: 1322
	private float timerDopoSparo1;

	// Token: 0x0400052B RID: 1323
	private float tempoFraSparoERicarica1;

	// Token: 0x0400052C RID: 1324
	private float timerFrequenzaArma2;

	// Token: 0x0400052D RID: 1325
	private float timerRicarica2;

	// Token: 0x0400052E RID: 1326
	private bool ricaricaInCorso2;

	// Token: 0x0400052F RID: 1327
	private float timerDopoSparo2;

	// Token: 0x04000530 RID: 1328
	private float tempoFraSparoERicarica2;

	// Token: 0x04000531 RID: 1329
	private float timerFrequenzaArma3;

	// Token: 0x04000532 RID: 1330
	private float timerRicarica3;

	// Token: 0x04000533 RID: 1331
	private bool ricaricaInCorso3;

	// Token: 0x04000534 RID: 1332
	private float timerDopoSparo3;

	// Token: 0x04000535 RID: 1333
	private float tempoFraSparoERicarica3;

	// Token: 0x04000536 RID: 1334
	private float timerFrequenzaArma4;

	// Token: 0x04000537 RID: 1335
	private float timerRicarica4;

	// Token: 0x04000538 RID: 1336
	private bool ricaricaInCorso4;

	// Token: 0x04000539 RID: 1337
	private float timerDopoSparo4;

	// Token: 0x0400053A RID: 1338
	private float tempoFraSparoERicarica4;

	// Token: 0x0400053B RID: 1339
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x0400053C RID: 1340
	public Vector3 posizionamentoCameraFPS1;

	// Token: 0x0400053D RID: 1341
	public Vector3 posizionamentoCameraFPS2;

	// Token: 0x0400053E RID: 1342
	public Vector3 posizionamentoCameraFPS3;

	// Token: 0x0400053F RID: 1343
	public Vector3 posizionamentoCameraFPS4;

	// Token: 0x04000540 RID: 1344
	private float timerPosizionamentoTPS;

	// Token: 0x04000541 RID: 1345
	private float timerPosizionamentoFPS;

	// Token: 0x04000542 RID: 1346
	private bool zoomAttivo;

	// Token: 0x04000543 RID: 1347
	private AudioSource suonoArma11;

	// Token: 0x04000544 RID: 1348
	private AudioSource suonoArma12;

	// Token: 0x04000545 RID: 1349
	private AudioSource suonoArma21;

	// Token: 0x04000546 RID: 1350
	private AudioSource suonoArma22;

	// Token: 0x04000547 RID: 1351
	private AudioSource suonoArma31;

	// Token: 0x04000548 RID: 1352
	private AudioSource suonoArma32;

	// Token: 0x04000549 RID: 1353
	private AudioSource suonoArma41;

	// Token: 0x0400054A RID: 1354
	private AudioSource suonoArma42;

	// Token: 0x0400054B RID: 1355
	private AudioSource suonoRicarica1;

	// Token: 0x0400054C RID: 1356
	private AudioSource suonoRicarica2;

	// Token: 0x0400054D RID: 1357
	private AudioSource suonoRicarica3;

	// Token: 0x0400054E RID: 1358
	private AudioSource suonoRicarica4;

	// Token: 0x0400054F RID: 1359
	private AudioSource suonoArma11FPS;

	// Token: 0x04000550 RID: 1360
	private AudioSource suonoArma12FPS;

	// Token: 0x04000551 RID: 1361
	private AudioSource suonoArma21FPS;

	// Token: 0x04000552 RID: 1362
	private AudioSource suonoArma22FPS;

	// Token: 0x04000553 RID: 1363
	private AudioSource suonoArma31FPS;

	// Token: 0x04000554 RID: 1364
	private AudioSource suonoArma32FPS;

	// Token: 0x04000555 RID: 1365
	private AudioSource suonoArma41FPS;

	// Token: 0x04000556 RID: 1366
	private AudioSource suonoArma42FPS;

	// Token: 0x04000557 RID: 1367
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x04000558 RID: 1368
	private int armaAttivaInFPS;

	// Token: 0x04000559 RID: 1369
	private int layerVisuale;

	// Token: 0x0400055A RID: 1370
	private int layerColpo;

	// Token: 0x0400055B RID: 1371
	private List<GameObject> ListaUnitàBersagli;

	// Token: 0x0400055C RID: 1372
	private List<bool> ListaVisualiOscurate;

	// Token: 0x0400055D RID: 1373
	private List<Vector3> ListaCentroInsetti;

	// Token: 0x0400055E RID: 1374
	private List<bool> ListaBersNeiMirini;

	// Token: 0x0400055F RID: 1375
	private List<GameObject> ListaBocceCampione;

	// Token: 0x04000560 RID: 1376
	private List<float> ListaDistanzeDaBers;

	// Token: 0x04000561 RID: 1377
	private List<GameObject> ListaBasiTorrette;

	// Token: 0x04000562 RID: 1378
	private List<GameObject> ListaBasiTorretteFPS;

	// Token: 0x04000563 RID: 1379
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x04000564 RID: 1380
	private GameObject munizioneArma1;

	// Token: 0x04000565 RID: 1381
	private GameObject munizioneArma2;

	// Token: 0x04000566 RID: 1382
	private GameObject munizioneArma3;

	// Token: 0x04000567 RID: 1383
	private GameObject munizioneArma4;

	// Token: 0x04000568 RID: 1384
	private ParticleSystem bocca11Particelle;

	// Token: 0x04000569 RID: 1385
	private ParticleSystem bocca12Particelle;

	// Token: 0x0400056A RID: 1386
	private ParticleSystem bocca21Particelle;

	// Token: 0x0400056B RID: 1387
	private ParticleSystem bocca22Particelle;

	// Token: 0x0400056C RID: 1388
	private ParticleSystem bocca31Particelle;

	// Token: 0x0400056D RID: 1389
	private ParticleSystem bocca32Particelle;

	// Token: 0x0400056E RID: 1390
	private ParticleSystem bocca41Particelle;

	// Token: 0x0400056F RID: 1391
	private ParticleSystem bocca42Particelle;

	// Token: 0x04000570 RID: 1392
	private ParticleSystem bocca11FPSParticelle;

	// Token: 0x04000571 RID: 1393
	private ParticleSystem bocca12FPSParticelle;

	// Token: 0x04000572 RID: 1394
	private ParticleSystem bocca21FPSParticelle;

	// Token: 0x04000573 RID: 1395
	private ParticleSystem bocca22FPSParticelle;

	// Token: 0x04000574 RID: 1396
	private ParticleSystem bocca31FPSParticelle;

	// Token: 0x04000575 RID: 1397
	private ParticleSystem bocca32FPSParticelle;

	// Token: 0x04000576 RID: 1398
	private ParticleSystem bocca41FPSParticelle;

	// Token: 0x04000577 RID: 1399
	private ParticleSystem bocca42FPSParticelle;

	// Token: 0x04000578 RID: 1400
	private ParticleSystem particelleColpiBocca11;

	// Token: 0x04000579 RID: 1401
	private ParticleSystem particelleColpiBocca12;

	// Token: 0x0400057A RID: 1402
	private ParticleSystem particelleColpiBocca41;

	// Token: 0x0400057B RID: 1403
	private ParticleSystem particelleColpiBocca42;

	// Token: 0x0400057C RID: 1404
	private ParticleSystem particelleColpiBocca11FPS;

	// Token: 0x0400057D RID: 1405
	private ParticleSystem particelleColpiBocca12FPS;

	// Token: 0x0400057E RID: 1406
	private ParticleSystem particelleColpiBocca41FPS;

	// Token: 0x0400057F RID: 1407
	private ParticleSystem particelleColpiBocca42FPS;

	// Token: 0x04000580 RID: 1408
	private List<GameObject> ListaBocche2;

	// Token: 0x04000581 RID: 1409
	private int cannone2Attivo;

	// Token: 0x04000582 RID: 1410
	private List<GameObject> ListaCannoni2;

	// Token: 0x04000583 RID: 1411
	private List<bool> ListaBoolCannoni2Sparati;

	// Token: 0x04000584 RID: 1412
	private List<bool> ListaBoolCannoni2InFondo;

	// Token: 0x04000585 RID: 1413
	private List<AudioSource> ListaSuoniCannoni2;

	// Token: 0x04000586 RID: 1414
	private List<ParticleSystem> ListaParticelleCannoni2;

	// Token: 0x04000587 RID: 1415
	private List<GameObject> ListaBocche3;

	// Token: 0x04000588 RID: 1416
	private int cannone3Attivo;

	// Token: 0x04000589 RID: 1417
	private List<GameObject> ListaCannoni3;

	// Token: 0x0400058A RID: 1418
	private List<bool> ListaBoolCannoni3Sparati;

	// Token: 0x0400058B RID: 1419
	private List<bool> ListaBoolCannoni3InFondo;

	// Token: 0x0400058C RID: 1420
	private List<AudioSource> ListaSuoniCannoni3;

	// Token: 0x0400058D RID: 1421
	private List<ParticleSystem> ListaParticelleCannoni3;

	// Token: 0x0400058E RID: 1422
	private List<GameObject> ListaBocche2FPS;

	// Token: 0x0400058F RID: 1423
	private int cannone2AttivoFPS;

	// Token: 0x04000590 RID: 1424
	private List<GameObject> ListaCannoni2FPS;

	// Token: 0x04000591 RID: 1425
	private List<bool> ListaBoolCannoni2SparatiFPS;

	// Token: 0x04000592 RID: 1426
	private List<bool> ListaBoolCannoni2InFondoFPS;

	// Token: 0x04000593 RID: 1427
	private List<AudioSource> ListaSuoniCannoni2FPS;

	// Token: 0x04000594 RID: 1428
	private List<ParticleSystem> ListaParticelleCannoni2FPS;

	// Token: 0x04000595 RID: 1429
	private List<GameObject> ListaBocche3FPS;

	// Token: 0x04000596 RID: 1430
	private int cannone3AttivoFPS;

	// Token: 0x04000597 RID: 1431
	private List<GameObject> ListaCannoni3FPS;

	// Token: 0x04000598 RID: 1432
	private List<bool> ListaBoolCannoni3SparatiFPS;

	// Token: 0x04000599 RID: 1433
	private List<bool> ListaBoolCannoni3InFondoFPS;

	// Token: 0x0400059A RID: 1434
	private List<AudioSource> ListaSuoniCannoni3FPS;

	// Token: 0x0400059B RID: 1435
	private List<ParticleSystem> ListaParticelleCannoni3FPS;

	// Token: 0x0400059C RID: 1436
	private GameObject proiettileCarro2;

	// Token: 0x0400059D RID: 1437
	private GameObject proiettileCarro3;

	// Token: 0x0400059E RID: 1438
	private bool sparoConDestra1;

	// Token: 0x0400059F RID: 1439
	private bool sparoConDestra4;

	// Token: 0x040005A0 RID: 1440
	private RaycastHit targetSparo;

	// Token: 0x040005A1 RID: 1441
	private AudioSource suonoMotore;

	// Token: 0x040005A2 RID: 1442
	private AudioSource suonoInterno;

	// Token: 0x040005A3 RID: 1443
	private float volumeMotoreIniziale;

	// Token: 0x040005A4 RID: 1444
	private List<float> ListaTimerAggRicerca;

	// Token: 0x040005A5 RID: 1445
	private float timerAggRicerca1;

	// Token: 0x040005A6 RID: 1446
	private float timerAggRicerca2;

	// Token: 0x040005A7 RID: 1447
	private float timerAggRicerca3;

	// Token: 0x040005A8 RID: 1448
	private float timerAggRicerca4;

	// Token: 0x040005A9 RID: 1449
	private bool èBattaglia5;

	// Token: 0x040005AA RID: 1450
	private float timerDiRifornimento;

	// Token: 0x040005AB RID: 1451
	private AudioSource suonoRifor;

	// Token: 0x040005AC RID: 1452
	private float timerSenzaArmiPrimarie;
}
