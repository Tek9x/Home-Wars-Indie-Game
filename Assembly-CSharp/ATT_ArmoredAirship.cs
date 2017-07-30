using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000021 RID: 33
public class ATT_ArmoredAirship : MonoBehaviour
{
	// Token: 0x06000160 RID: 352 RVA: 0x0003C58C File Offset: 0x0003A78C
	private void Start()
	{
		this.CanvasFPS = GameObject.FindGameObjectWithTag("CanvasFPS");
		this.mirinoElettr1 = this.CanvasFPS.transform.GetChild(2).transform.GetChild(5).gameObject;
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.InfoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.layerColpo = 165120;
		this.layerVisuale = 256;
		this.ListaMunizioniAttiveUnità = new List<GameObject>();
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma1);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma2);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma3);
		this.suonoTorretta1 = base.transform.GetChild(2).transform.GetChild(1).GetComponent<AudioSource>();
		this.suonoTorretta2 = base.transform.GetChild(2).transform.GetChild(2).GetComponent<AudioSource>();
		this.suonoTorretta3 = base.transform.GetChild(2).transform.GetChild(3).GetComponent<AudioSource>();
		this.suonoInterno = base.transform.GetChild(2).GetComponent<AudioSource>();
		this.tempoFraSparoERicarica1 = 1f;
		this.tempoFraSparoERicarica2 = 1f;
		this.tempoFraSparoERicarica3 = 1f;
		this.suonoArma11 = this.bocca11.GetComponent<AudioSource>();
		this.suonoArma12 = this.bocca12.GetComponent<AudioSource>();
		this.suonoArma13 = this.bocca13.GetComponent<AudioSource>();
		this.suonoArma2 = this.bocca2.GetComponent<AudioSource>();
		this.suonoArma3 = this.bocca3.GetComponent<AudioSource>();
		this.suonoRicarica1 = this.baseArma1.GetComponent<AudioSource>();
		this.suonoRicarica2 = this.baseArma2.GetComponent<AudioSource>();
		this.suonoRicarica3 = this.baseArma3.GetComponent<AudioSource>();
		this.ListaSuoniArma1 = new List<AudioSource>();
		this.ListaSuoniArma1.Add(this.suonoArma11);
		this.ListaSuoniArma1.Add(this.suonoArma12);
		this.ListaSuoniArma1.Add(this.suonoArma13);
		this.particelleArma11 = this.bocca11.GetComponent<ParticleSystem>();
		this.particelleArma12 = this.bocca12.GetComponent<ParticleSystem>();
		this.particelleArma13 = this.bocca13.GetComponent<ParticleSystem>();
		this.particelleArma2 = this.bocca2.GetComponent<ParticleSystem>();
		this.ListaParticelleArma1 = new List<ParticleSystem>();
		this.ListaParticelleArma1.Add(this.particelleArma11);
		this.ListaParticelleArma1.Add(this.particelleArma12);
		this.ListaParticelleArma1.Add(this.particelleArma13);
		this.corpo2PerRotazione = this.baseArma2.transform.GetChild(0).gameObject;
		this.ListaBoccheArma1 = new List<GameObject>();
		this.ListaBoccheArma1.Add(this.bocca11);
		this.ListaBoccheArma1.Add(this.bocca12);
		this.ListaBoccheArma1.Add(this.bocca13);
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.volumeBaseEsterno = this.suonoMotore.volume;
		this.suonoMotore.Play();
	}

	// Token: 0x06000161 RID: 353 RVA: 0x0003C8F4 File Offset: 0x0003AAF4
	private void Update()
	{
		this.ListaMunizioniAttiveUnità[0] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[0][0];
		this.ListaMunizioniAttiveUnità[1] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[1][0];
		this.ListaMunizioniAttiveUnità[2] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[2][0];
		this.munizioneArma1 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0];
		this.munizioneArma2 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[1];
		this.munizioneArma3 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[2];
		this.timerFrequenzaArma1 += Time.deltaTime;
		this.timerFrequenzaArma2 += Time.deltaTime;
		this.timerFrequenzaArma3 += Time.deltaTime;
		this.timerDopoSparo1 += Time.deltaTime;
		this.timerDopoSparo2 += Time.deltaTime;
		this.timerDopoSparo3 += Time.deltaTime;
		this.CondizioniArma1();
		this.CondizioniArma2();
		this.CondizioniArma3();
		this.unitàBersaglio = base.GetComponent<PresenzaAlleato>().unitàBersaglio;
		if (Physics.Raycast(base.transform.position, -Vector3.up, 30f, 256))
		{
			this.vicinoATerra = true;
		}
		else
		{
			this.vicinoATerra = false;
		}
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.rotArma2Attiva = false;
			this.PreparazioneAttacco();
		}
		else
		{
			this.armaAttivaInFPS = base.GetComponent<PresenzaAlleato>().armaAttivaInFPS;
			this.GestioneVisuali();
			this.SelezioneArma();
			if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
			{
				this.mirinoElettr1.GetComponent<CanvasGroup>().alpha = 1f;
				this.mirinoElettr1.GetComponent<Image>().sprite = this.mirinoTPS;
			}
			else if (this.armaAttivaInFPS == 0)
			{
				this.AttaccoPrimaPersonaArma1();
			}
			else if (this.armaAttivaInFPS == 1)
			{
				this.AttaccoPrimaPersonaArma2();
			}
			else if (this.armaAttivaInFPS == 2)
			{
				this.AttaccoPrimaPersonaArma3();
			}
			if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva == 3)
			{
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
					this.mirinoElettr1.GetComponent<Image>().sprite = this.mirinoFPS;
					this.mirinoElettr1.GetComponent<CanvasGroup>().alpha = 1f;
					this.suonoInterno.Play();
					this.suonoMotore.volume = this.volumeBaseEsterno / 3f;
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
					this.zoomAttivo = false;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					this.mirinoElettr1.GetComponent<Image>().sprite = this.mirinoTPS;
					this.mirinoElettr1.GetComponent<CanvasGroup>().alpha = 1f;
					this.suonoInterno.Stop();
					this.suonoMotore.volume = this.volumeBaseEsterno;
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
			base.GetComponent<MOV_ArmoredAirship>().torrettaArma1.transform.rotation = base.transform.rotation;
			base.GetComponent<MOV_ArmoredAirship>().torrettaArma2.transform.rotation = base.transform.rotation;
			base.GetComponent<MOV_ArmoredAirship>().torrettaArma3.transform.rotation = base.transform.rotation;
			this.baseArma1.transform.rotation = base.transform.rotation;
			this.baseArma2.transform.rotation = base.transform.rotation;
			this.baseArma3.transform.rotation = base.transform.rotation;
			this.suonoTorretta1.Stop();
			this.suonoTorretta2.Stop();
			this.suonoTorretta3.Stop();
			base.GetComponent<MOV_ArmoredAirship>().suonoTorrettaPartito = false;
			this.rotArma2Attiva = false;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
			this.zoomAttivo = false;
			this.suonoInterno.Stop();
			this.suonoMotore.volume = this.volumeBaseEsterno / 3f;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x06000162 RID: 354 RVA: 0x0003CE40 File Offset: 0x0003B040
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
		if (this.cannone1Attivo >= 3)
		{
			this.cannone1Attivo = 0;
		}
	}

	// Token: 0x06000163 RID: 355 RVA: 0x0003D0B0 File Offset: 0x0003B2B0
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
		if (this.rotArma2Attiva)
		{
			this.corpo2PerRotazione.transform.Rotate(Vector3.up * 7f);
		}
	}

	// Token: 0x06000164 RID: 356 RVA: 0x0003D338 File Offset: 0x0003B538
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
						base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[3] = false;
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

	// Token: 0x06000165 RID: 357 RVA: 0x0003D598 File Offset: 0x0003B798
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

	// Token: 0x06000166 RID: 358 RVA: 0x0003D644 File Offset: 0x0003B844
	private void CameraTPS()
	{
		this.timerPosizionamentoTPS += Time.deltaTime;
		if (this.timerPosizionamentoTPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = base.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraTPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localEulerAngles = new Vector3(0f, 0f, this.baseArma1.transform.eulerAngles.z);
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
		}
	}

	// Token: 0x06000167 RID: 359 RVA: 0x0003D6FC File Offset: 0x0003B8FC
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			if (this.armaAttivaInFPS == 0)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.baseArma1.transform;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posCameraArma1;
				this.terzaCamera.transform.forward = this.baseArma1.transform.forward;
				this.terzaCamera.GetComponent<Camera>().fieldOfView = 50f;
			}
			else if (this.armaAttivaInFPS == 1)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.baseArma2.transform;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posCameraArma2;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = this.baseArma2.transform.rotation;
				this.terzaCamera.GetComponent<Camera>().fieldOfView = 50f;
			}
			else if (this.armaAttivaInFPS == 2)
			{
				this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.baseArma3.transform;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posCameraArma3;
				this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = this.baseArma3.transform.rotation;
				this.terzaCamera.GetComponent<Camera>().fieldOfView = 50f;
			}
		}
	}

	// Token: 0x06000168 RID: 360 RVA: 0x0003D8B4 File Offset: 0x0003BAB4
	private void PreparazioneAttacco()
	{
		bool flag = false;
		if (this.unitàBersaglio)
		{
			flag = Physics.Linecast(this.bocca2.transform.position, this.centroUnitàBersaglio, this.layerVisuale);
			this.centroUnitàBersaglio = this.unitàBersaglio.GetComponent<PresenzaNemico>().centroInsetto;
			float num = Vector3.Angle(base.transform.forward, (this.centroUnitàBersaglio - base.transform.position).normalized);
			if (num < 5f)
			{
				this.bersèNelMirino = true;
			}
			else
			{
				this.bersèNelMirino = false;
			}
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
			base.GetComponent<MOV_AUTOM_ArmoredAirship>().muoviti = false;
			if (base.GetComponent<PresenzaAlleato>().attaccoOrdinato)
			{
				base.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
				this.unitàBersaglio = this.primaCamera.GetComponent<Selezionamento>().oggettoBersaglio;
				if (this.unitàBersaglio && !this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
				{
					float num3 = Vector3.Dot((this.centroUnitàBersaglio - base.transform.position).normalized, Vector3.up);
					if (num3 < this.angAttaccoMax && num3 > this.angAttaccoMin)
					{
						float num4 = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
						if (num4 >= num2)
						{
							if (!base.GetComponent<PresenzaAlleato>().comportamentoDifensivo && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino)
							{
								base.GetComponent<MOV_AUTOM_ArmoredAirship>().muoviti = true;
							}
							else
							{
								base.GetComponent<MOV_AUTOM_ArmoredAirship>().muoviti = false;
								this.unitàBersaglio = null;
							}
						}
						for (int j = 0; j < base.GetComponent<PresenzaAlleato>().numeroArmi; j++)
						{
							if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[j] && num4 < this.ListaMunizioniAttiveUnità[j].GetComponent<DatiGeneraliMunizione>().portataMassima)
							{
								base.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
								this.baseArma1.transform.LookAt(this.centroUnitàBersaglio);
								this.baseArma2.transform.LookAt(this.centroUnitàBersaglio);
								this.baseArma3.transform.LookAt(this.centroUnitàBersaglio);
								if (j == 0)
								{
									this.AttaccoIndipendente1();
								}
								else if (j == 1)
								{
									this.AttaccoIndipendente2();
								}
								else if (j == 2)
								{
									this.AttaccoIndipendente3();
								}
							}
						}
						if (num4 < num2)
						{
							if (!flag)
							{
								base.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
								this.baseArma1.transform.LookAt(this.centroUnitàBersaglio);
								this.baseArma2.transform.LookAt(this.centroUnitàBersaglio);
								this.baseArma3.transform.LookAt(this.centroUnitàBersaglio);
								base.GetComponent<MOV_AUTOM_ArmoredAirship>().muoviti = false;
							}
							else if (!base.GetComponent<PresenzaAlleato>().comportamentoDifensivo && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino)
							{
								base.GetComponent<MOV_AUTOM_ArmoredAirship>().muoviti = true;
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
			else if (this.unitàBersaglio)
			{
				float num5 = Vector3.Dot((this.centroUnitàBersaglio - base.transform.position).normalized, Vector3.up);
				float num6 = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
				if (num5 < this.angAttaccoMax && num5 > this.angAttaccoMin)
				{
					if (num6 >= num2)
					{
						if (!base.GetComponent<PresenzaAlleato>().comportamentoDifensivo && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino)
						{
							base.GetComponent<MOV_AUTOM_ArmoredAirship>().muoviti = true;
						}
						else
						{
							base.GetComponent<MOV_AUTOM_ArmoredAirship>().muoviti = false;
							this.unitàBersaglio = null;
						}
					}
					for (int k = 0; k < base.GetComponent<PresenzaAlleato>().numeroArmi; k++)
					{
						if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[k] && num6 < this.ListaMunizioniAttiveUnità[k].GetComponent<DatiGeneraliMunizione>().portataMassima)
						{
							base.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
							this.baseArma1.transform.LookAt(this.centroUnitàBersaglio);
							this.baseArma2.transform.LookAt(this.centroUnitàBersaglio);
							this.baseArma3.transform.LookAt(this.centroUnitàBersaglio);
							if (k == 0)
							{
								this.AttaccoIndipendente1();
							}
							else if (k == 1)
							{
								this.AttaccoIndipendente2();
							}
							else if (k == 2)
							{
								this.AttaccoIndipendente3();
							}
						}
					}
					if (num6 < num2)
					{
						if (!flag)
						{
							base.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
							this.baseArma1.transform.LookAt(this.centroUnitàBersaglio);
							this.baseArma2.transform.LookAt(this.centroUnitàBersaglio);
							this.baseArma3.transform.LookAt(this.centroUnitàBersaglio);
							base.GetComponent<MOV_AUTOM_ArmoredAirship>().muoviti = false;
						}
						else if (!base.GetComponent<PresenzaAlleato>().comportamentoDifensivo && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino)
						{
							base.GetComponent<MOV_AUTOM_ArmoredAirship>().muoviti = true;
						}
						else
						{
							this.unitàBersaglio = null;
						}
					}
				}
				if (this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita <= 0f)
				{
					this.unitàBersaglio = null;
				}
				if (this.unitàBersaglio && !this.unitàBersaglio.GetComponent<PresenzaNemico>().èStatoVisto)
				{
					this.unitàBersaglio = null;
				}
				if (this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
				{
					this.unitàBersaglio = null;
				}
			}
			else if (base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers)
			{
				base.GetComponent<MOV_AUTOM_ArmoredAirship>().muoviti = true;
				if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici.Count > 0)
				{
					this.timerAggRicerca += Time.deltaTime;
					if (this.timerAggRicerca > 1f)
					{
						this.timerAggRicerca = 0f;
						List<GameObject> list = new List<GameObject>();
						foreach (GameObject current in this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici)
						{
							if (current != null && current.GetComponent<PresenzaNemico>().èStatoVisto && !current.GetComponent<PresenzaNemico>().insettoVolante)
							{
								float num7 = Vector3.Distance(base.transform.position, current.GetComponent<PresenzaNemico>().centroInsetto);
								if (num7 < this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMassima && !Physics.Linecast(this.baseArma1.transform.position, current.GetComponent<PresenzaNemico>().centroInsetto, this.layerVisuale))
								{
									float num8 = Vector3.Dot((current.GetComponent<PresenzaNemico>().centroInsetto - base.transform.position).normalized, base.transform.up);
									if (num8 < this.angAttaccoMax && num8 > this.angAttaccoMin)
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
				base.GetComponent<MOV_AUTOM_ArmoredAirship>().muoviti = true;
				if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici.Count > 0)
				{
					this.timerAggRicerca += Time.deltaTime;
					if (this.timerAggRicerca > 1f)
					{
						this.timerAggRicerca = 0f;
						List<GameObject> list2 = new List<GameObject>();
						foreach (GameObject current2 in this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici)
						{
							if (current2 != null && current2.GetComponent<PresenzaNemico>().èStatoVisto && !current2.GetComponent<PresenzaNemico>().insettoVolante)
							{
								float num9 = Vector3.Distance(base.transform.position, current2.GetComponent<PresenzaNemico>().centroInsetto);
								if (num9 < this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMassima && !Physics.Linecast(this.baseArma1.transform.position, current2.GetComponent<PresenzaNemico>().centroInsetto, this.layerVisuale))
								{
									float num10 = Vector3.Dot((current2.GetComponent<PresenzaNemico>().centroInsetto - base.transform.position).normalized, base.transform.up);
									if (num10 < this.angAttaccoMax && num10 > this.angAttaccoMin)
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
				base.GetComponent<MOV_AUTOM_ArmoredAirship>().muoviti = true;
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
			this.unitàBersaglio = null;
			base.GetComponent<MOV_AUTOM_ArmoredAirship>().muoviti = true;
		}
	}

	// Token: 0x06000169 RID: 361 RVA: 0x0003E53C File Offset: 0x0003C73C
	private void AttaccoIndipendente1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && !this.vicinoATerra && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca12.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][0])
		{
			this.timerFrequenzaArma1 = 0f;
			this.ListaSuoniArma1[this.cannone1Attivo].Play();
			this.ListaParticelleArma1[this.cannone1Attivo].Play();
			List<float> list;
			List<float> expr_FC = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int index;
			int expr_FF = index = 5;
			float num = list[index];
			expr_FC[expr_FF] = num - 1f;
			List<float> list2;
			List<float> expr_126 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int expr_129 = index = 6;
			num = list2[index];
			expr_126[expr_129] = num - 1f;
			this.SparoIndipendente1();
			this.timerDopoSparo1 = 0f;
			this.cannone1Attivo++;
		}
	}

	// Token: 0x0600016A RID: 362 RVA: 0x0003E6A8 File Offset: 0x0003C8A8
	private void SparoIndipendente1()
	{
		this.proiettileArma1 = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.ListaBoccheArma1[this.cannone1Attivo].transform.position, this.ListaBoccheArma1[this.cannone1Attivo].transform.rotation) as GameObject);
		this.proiettileArma1.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.proiettileArma1.GetComponent<DatiProiettile>().locazioneTarget = this.centroUnitàBersaglio;
		this.proiettileArma1.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x0600016B RID: 363 RVA: 0x0003E744 File Offset: 0x0003C944
	private void AttaccoIndipendente2()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[1] && !this.vicinoATerra && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca2.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] > 0f)
		{
			if (this.timerFrequenzaArma2 > base.GetComponent<PresenzaAlleato>().ListaArmi[1][0])
			{
				this.timerFrequenzaArma2 = 0f;
				this.particelleArma2.Play();
				this.suonoArma2.Play();
				List<float> list;
				List<float> expr_E6 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
				int index;
				int expr_E9 = index = 5;
				float num = list[index];
				expr_E6[expr_E9] = num - 1f;
				List<float> list2;
				List<float> expr_110 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
				int expr_113 = index = 6;
				num = list2[index];
				expr_110[expr_113] = num - 1f;
				this.SparoIndipendente2();
			}
			this.rotArma2Attiva = true;
		}
	}

	// Token: 0x0600016C RID: 364 RVA: 0x0003E888 File Offset: 0x0003CA88
	private void SparoIndipendente2()
	{
		this.proiettileArma2 = (UnityEngine.Object.Instantiate(this.munizioneArma2, this.bocca2.transform.position, this.bocca2.transform.rotation) as GameObject);
		this.proiettileArma2.GetComponent<DatiProiettile>().locazioneTarget = this.centroUnitàBersaglio;
		this.proiettileArma2.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x0600016D RID: 365 RVA: 0x0003E8FC File Offset: 0x0003CAFC
	private void AttaccoIndipendente3()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[2] && !this.vicinoATerra && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca3.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] > 0f && this.timerFrequenzaArma3 > base.GetComponent<PresenzaAlleato>().ListaArmi[2][0])
		{
			this.timerFrequenzaArma3 = 0f;
			this.suonoArma3.Play();
			List<float> list;
			List<float> expr_DB = list = base.GetComponent<PresenzaAlleato>().ListaArmi[2];
			int index;
			int expr_DE = index = 5;
			float num = list[index];
			expr_DB[expr_DE] = num - 1f;
			List<float> list2;
			List<float> expr_105 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[2];
			int expr_108 = index = 6;
			num = list2[index];
			expr_105[expr_108] = num - 1f;
			this.SparoIndipendente3();
		}
	}

	// Token: 0x0600016E RID: 366 RVA: 0x0003EA30 File Offset: 0x0003CC30
	private void SparoIndipendente3()
	{
		this.proiettileArma3 = (UnityEngine.Object.Instantiate(this.munizioneArma3, this.bocca3.transform.position, this.bocca3.transform.rotation) as GameObject);
		this.proiettileArma3.GetComponent<DatiProiettile>().locazioneTarget = this.centroUnitàBersaglio;
		this.proiettileArma3.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x0600016F RID: 367 RVA: 0x0003EAA4 File Offset: 0x0003CCA4
	private void SelezioneArma()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 0;
			this.timerPosizionamentoFPS = 0f;
			this.rotArma2Attiva = false;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 1;
			this.timerPosizionamentoFPS = 0f;
			this.rotArma2Attiva = false;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 2;
			this.timerPosizionamentoFPS = 0f;
			this.rotArma2Attiva = false;
		}
	}

	// Token: 0x06000170 RID: 368 RVA: 0x0003EB30 File Offset: 0x0003CD30
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
			this.ListaSuoniArma1[this.cannone1Attivo].Play();
			this.ListaParticelleArma1[this.cannone1Attivo].Play();
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

	// Token: 0x06000171 RID: 369 RVA: 0x0003ED9C File Offset: 0x0003CF9C
	private void SparoArma1()
	{
		this.proiettileArma1 = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.ListaBoccheArma1[this.cannone1Attivo].transform.position, this.ListaBoccheArma1[this.cannone1Attivo].transform.rotation) as GameObject);
		this.proiettileArma1.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.proiettileArma1.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x06000172 RID: 370 RVA: 0x0003EE24 File Offset: 0x0003D024
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
		if (Input.GetMouseButton(0))
		{
			if (base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] > 0f && this.timerFrequenzaArma2 > base.GetComponent<PresenzaAlleato>().ListaArmi[1][1])
			{
				this.timerFrequenzaArma2 = 0f;
				this.SparoArma2();
				this.suonoArma2.Play();
				this.bocca2.GetComponent<ParticleSystem>().Play();
				List<float> list;
				List<float> expr_1D9 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
				int index;
				int expr_1DC = index = 5;
				float num = list[index];
				expr_1D9[expr_1DC] = num - 1f;
				List<float> list2;
				List<float> expr_203 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
				int expr_207 = index = 6;
				num = list2[index];
				expr_203[expr_207] = num - 1f;
			}
			this.rotArma2Attiva = true;
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.rotArma2Attiva = false;
		}
	}

	// Token: 0x06000173 RID: 371 RVA: 0x0003F068 File Offset: 0x0003D268
	private void SparoArma2()
	{
		this.proiettileArma2 = (UnityEngine.Object.Instantiate(this.munizioneArma2, this.bocca2.transform.position, this.bocca2.transform.rotation) as GameObject);
		this.proiettileArma2.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.proiettileArma2.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0003F0D8 File Offset: 0x0003D2D8
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
			this.suonoArma3.Play();
			List<float> list;
			List<float> expr_1DF = list = base.GetComponent<PresenzaAlleato>().ListaArmi[2];
			int index;
			int expr_1E2 = index = 5;
			float num = list[index];
			expr_1DF[expr_1E2] = num - 1f;
			List<float> list2;
			List<float> expr_209 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[2];
			int expr_20D = index = 6;
			num = list2[index];
			expr_209[expr_20D] = num - 1f;
			this.timerDopoSparo3 = 0f;
		}
	}

	// Token: 0x06000175 RID: 373 RVA: 0x0003F314 File Offset: 0x0003D514
	private void SparoArma3()
	{
		this.proiettileArma3 = (UnityEngine.Object.Instantiate(this.munizioneArma3, this.bocca3.transform.position, this.bocca3.transform.rotation) as GameObject);
		this.proiettileArma3.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.proiettileArma3.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x04000657 RID: 1623
	public float angAttaccoMin;

	// Token: 0x04000658 RID: 1624
	public float angAttaccoMax;

	// Token: 0x04000659 RID: 1625
	private GameObject infoNeutreTattica;

	// Token: 0x0400065A RID: 1626
	private GameObject terzaCamera;

	// Token: 0x0400065B RID: 1627
	private GameObject primaCamera;

	// Token: 0x0400065C RID: 1628
	public GameObject bocca11;

	// Token: 0x0400065D RID: 1629
	public GameObject bocca12;

	// Token: 0x0400065E RID: 1630
	public GameObject bocca13;

	// Token: 0x0400065F RID: 1631
	public GameObject bocca2;

	// Token: 0x04000660 RID: 1632
	public GameObject bocca3;

	// Token: 0x04000661 RID: 1633
	private GameObject IANemico;

	// Token: 0x04000662 RID: 1634
	private GameObject InfoAlleati;

	// Token: 0x04000663 RID: 1635
	private float timerFrequenzaArma1;

	// Token: 0x04000664 RID: 1636
	private float timerRicarica1;

	// Token: 0x04000665 RID: 1637
	private bool ricaricaInCorso1;

	// Token: 0x04000666 RID: 1638
	private float timerDopoSparo1;

	// Token: 0x04000667 RID: 1639
	private float tempoFraSparoERicarica1;

	// Token: 0x04000668 RID: 1640
	private float timerFrequenzaArma2;

	// Token: 0x04000669 RID: 1641
	private float timerRicarica2;

	// Token: 0x0400066A RID: 1642
	private bool ricaricaInCorso2;

	// Token: 0x0400066B RID: 1643
	private float timerDopoSparo2;

	// Token: 0x0400066C RID: 1644
	private float tempoFraSparoERicarica2;

	// Token: 0x0400066D RID: 1645
	private float timerFrequenzaArma3;

	// Token: 0x0400066E RID: 1646
	private float timerRicarica3;

	// Token: 0x0400066F RID: 1647
	private bool ricaricaInCorso3;

	// Token: 0x04000670 RID: 1648
	private float timerDopoSparo3;

	// Token: 0x04000671 RID: 1649
	private float tempoFraSparoERicarica3;

	// Token: 0x04000672 RID: 1650
	private int layerColpo;

	// Token: 0x04000673 RID: 1651
	private int layerVisuale;

	// Token: 0x04000674 RID: 1652
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000675 RID: 1653
	private float timerPosizionamentoTPS;

	// Token: 0x04000676 RID: 1654
	private float timerPosizionamentoFPS;

	// Token: 0x04000677 RID: 1655
	public Vector3 posCameraArma1;

	// Token: 0x04000678 RID: 1656
	public Vector3 posCameraArma2;

	// Token: 0x04000679 RID: 1657
	public Vector3 posCameraArma3;

	// Token: 0x0400067A RID: 1658
	private GameObject CanvasFPS;

	// Token: 0x0400067B RID: 1659
	private GameObject mirinoElettr1;

	// Token: 0x0400067C RID: 1660
	public Sprite mirinoTPS;

	// Token: 0x0400067D RID: 1661
	public Sprite mirinoFPS;

	// Token: 0x0400067E RID: 1662
	private RaycastHit targetSparo;

	// Token: 0x0400067F RID: 1663
	public GameObject baseArma1;

	// Token: 0x04000680 RID: 1664
	public GameObject baseArma2;

	// Token: 0x04000681 RID: 1665
	public GameObject baseArma3;

	// Token: 0x04000682 RID: 1666
	public GameObject unitàBersaglio;

	// Token: 0x04000683 RID: 1667
	private Vector3 centroUnitàBersaglio;

	// Token: 0x04000684 RID: 1668
	private GameObject munizioneArma1;

	// Token: 0x04000685 RID: 1669
	private GameObject munizioneArma2;

	// Token: 0x04000686 RID: 1670
	private GameObject munizioneArma3;

	// Token: 0x04000687 RID: 1671
	private AudioSource suonoTorretta1;

	// Token: 0x04000688 RID: 1672
	private AudioSource suonoTorretta2;

	// Token: 0x04000689 RID: 1673
	private AudioSource suonoTorretta3;

	// Token: 0x0400068A RID: 1674
	private AudioSource suonoInterno;

	// Token: 0x0400068B RID: 1675
	private float timerPartenza;

	// Token: 0x0400068C RID: 1676
	private float timerStop;

	// Token: 0x0400068D RID: 1677
	private bool primaPartenza;

	// Token: 0x0400068E RID: 1678
	private bool inPartenza;

	// Token: 0x0400068F RID: 1679
	private bool partenzaFinita;

	// Token: 0x04000690 RID: 1680
	private bool inStop;

	// Token: 0x04000691 RID: 1681
	public bool stopFinito;

	// Token: 0x04000692 RID: 1682
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x04000693 RID: 1683
	private bool suonoRicaricaAttivo;

	// Token: 0x04000694 RID: 1684
	private AudioSource suonoArma11;

	// Token: 0x04000695 RID: 1685
	private AudioSource suonoArma12;

	// Token: 0x04000696 RID: 1686
	private AudioSource suonoArma13;

	// Token: 0x04000697 RID: 1687
	private AudioSource suonoRicarica1;

	// Token: 0x04000698 RID: 1688
	private AudioSource suonoArma2;

	// Token: 0x04000699 RID: 1689
	private AudioSource suonoRicarica2;

	// Token: 0x0400069A RID: 1690
	private AudioSource suonoArma3;

	// Token: 0x0400069B RID: 1691
	private AudioSource suonoRicarica3;

	// Token: 0x0400069C RID: 1692
	private List<AudioSource> ListaSuoniArma1;

	// Token: 0x0400069D RID: 1693
	private int armaAttivaInFPS;

	// Token: 0x0400069E RID: 1694
	private ParticleSystem particelleArma11;

	// Token: 0x0400069F RID: 1695
	private ParticleSystem particelleArma12;

	// Token: 0x040006A0 RID: 1696
	private ParticleSystem particelleArma13;

	// Token: 0x040006A1 RID: 1697
	private ParticleSystem particelleArma2;

	// Token: 0x040006A2 RID: 1698
	private List<ParticleSystem> ListaParticelleArma1;

	// Token: 0x040006A3 RID: 1699
	private GameObject proiettileArma1;

	// Token: 0x040006A4 RID: 1700
	private GameObject proiettileArma2;

	// Token: 0x040006A5 RID: 1701
	private GameObject proiettileArma3;

	// Token: 0x040006A6 RID: 1702
	private bool rotArma2Attiva;

	// Token: 0x040006A7 RID: 1703
	private GameObject corpo2PerRotazione;

	// Token: 0x040006A8 RID: 1704
	private int cannone1Attivo;

	// Token: 0x040006A9 RID: 1705
	private List<GameObject> ListaBoccheArma1;

	// Token: 0x040006AA RID: 1706
	private bool zoomAttivo;

	// Token: 0x040006AB RID: 1707
	private bool bersèNelMirino;

	// Token: 0x040006AC RID: 1708
	private AudioSource suonoMotore;

	// Token: 0x040006AD RID: 1709
	private float volumeBaseEsterno;

	// Token: 0x040006AE RID: 1710
	private float timerAggRicerca;

	// Token: 0x040006AF RID: 1711
	private bool vicinoATerra;
}
