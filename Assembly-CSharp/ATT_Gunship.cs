using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000016 RID: 22
public class ATT_Gunship : MonoBehaviour
{
	// Token: 0x060000C1 RID: 193 RVA: 0x00023FF8 File Offset: 0x000221F8
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
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma2);
		this.tempoFraSparoERicarica1 = 1f;
		this.tempoFraSparoERicarica2 = 1f;
		this.suonoArma1 = this.bocca1.GetComponent<AudioSource>();
		this.suonoArma2 = this.bocca2.GetComponent<AudioSource>();
		this.suonoArma3 = this.bocca3.GetComponent<AudioSource>();
		this.suonoRicarica1 = this.bocca1.transform.parent.GetChild(0).GetComponent<AudioSource>();
		this.suonoRicarica2 = this.bocca2.transform.parent.GetComponent<AudioSource>();
		this.suonoRicarica3 = this.bocca3.transform.parent.GetComponent<AudioSource>();
		this.particelleArma1 = this.bocca1.GetComponent<ParticleSystem>();
		this.particelleArma2 = this.bocca2.GetComponent<ParticleSystem>();
		this.particelleArma3 = this.bocca3.GetComponent<ParticleSystem>();
		this.corpoAereo = base.transform.GetChild(2).gameObject;
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.suonoInterno = base.transform.GetChild(2).GetComponent<AudioSource>();
		this.volumeMotoreIniziale = base.GetComponent<AudioSource>().volume;
		this.corpoArma1 = this.baseArma1.transform.GetChild(0).GetChild(0).gameObject;
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00024238 File Offset: 0x00022438
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
		this.CondizioniArma1();
		this.CondizioniArma2();
		this.CondizioniArma3();
		this.unitàBersaglio = base.GetComponent<PresenzaAlleato>().unitàBersaglio;
		this.baseArma1.transform.localEulerAngles = new Vector3(this.baseArma1.transform.localEulerAngles.x, this.baseArma1.transform.localEulerAngles.y, 0f);
		this.baseArma2.transform.localEulerAngles = new Vector3(this.baseArma2.transform.localEulerAngles.x, this.baseArma2.transform.localEulerAngles.y, 0f);
		this.baseArma3.transform.localEulerAngles = new Vector3(this.baseArma3.transform.localEulerAngles.x, this.baseArma3.transform.localEulerAngles.y, 0f);
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.PreparazioneAttacco();
			this.ControlloArmiPrimarie();
		}
		else
		{
			this.GestioneVisuali();
			this.SelezioneArma();
			if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
			{
				this.mirinoElettr1.GetComponent<CanvasGroup>().alpha = 1f;
				this.mirinoElettr1.GetComponent<Image>().sprite = this.mirinoTPS;
			}
			else if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 0)
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
			if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva == 3)
			{
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
					this.mirinoElettr1.GetComponent<Image>().sprite = this.mirinoFPS;
					this.mirinoElettr1.GetComponent<CanvasGroup>().alpha = 1f;
					this.suonoMotore.volume = this.volumeMotoreIniziale / 5f;
					this.suonoInterno.Play();
					this.suonoArma1.spatialBlend = 0f;
					this.suonoArma2.spatialBlend = 0f;
					this.suonoArma3.spatialBlend = 0f;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					this.mirinoElettr1.GetComponent<Image>().sprite = this.mirinoTPS;
					this.mirinoElettr1.GetComponent<CanvasGroup>().alpha = 1f;
					this.suonoInterno.Stop();
					this.suonoMotore.volume = this.volumeMotoreIniziale;
					this.suonoArma1.spatialBlend = 1f;
					this.suonoArma2.spatialBlend = 1f;
					this.suonoArma3.spatialBlend = 1f;
					this.suonoArma1.Stop();
					this.suonoArma1Partito = false;
					this.particelleArma1.Stop();
					this.rotArma1Attiva = false;
				}
			}
		}
		if (this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona && this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0 && this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0] == base.gameObject)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona = false;
			this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera = null;
			this.timerPosizionamentoTPS = 0f;
			this.timerPosizionamentoFPS = 0f;
			this.baseArma1.transform.localEulerAngles = new Vector3(0f, 270f, 0f);
			this.baseArma2.transform.localEulerAngles = new Vector3(0f, 270f, 0f);
			this.baseArma3.transform.localEulerAngles = new Vector3(0f, 270f, 0f);
			this.suonoInterno.Stop();
			this.suonoMotore.volume = this.volumeMotoreIniziale;
			this.suonoArma1.spatialBlend = 1f;
			this.suonoArma2.spatialBlend = 1f;
			this.suonoArma3.spatialBlend = 1f;
			this.suonoArma1.Stop();
			this.suonoArma1Partito = false;
			this.particelleArma1.Stop();
			this.rotArma1Attiva = false;
		}
		if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().tipoBattaglia == 3)
		{
			base.GetComponent<PresenzaAlleato>().carburante = base.GetComponent<PresenzaAlleato>().carburanteIniziale;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00024844 File Offset: 0x00022A44
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
			this.suonoArma1.Stop();
			this.suonoArma1Partito = false;
			this.particelleArma1.Stop();
			this.rotArma1Attiva = false;
		}
		if (this.rotArma1Attiva)
		{
			this.corpoArma1.transform.Rotate(Vector3.right * 12f);
		}
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00024AF0 File Offset: 0x00022CF0
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

	// Token: 0x060000C5 RID: 197 RVA: 0x00024D50 File Offset: 0x00022F50
	private void CondizioniArma3()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] <= 0f && base.GetComponent<PresenzaAlleato>().ListaArmi[2][6] > 0f)
		{
			this.timerRicarica3 += Time.deltaTime;
			if (this.timerRicarica3 > base.GetComponent<PresenzaAlleato>().ListaArmi[2][2])
			{
				if (base.GetComponent<PresenzaAlleato>().ListaArmi[2][6] < base.GetComponent<PresenzaAlleato>().ListaArmi[2][3])
				{
					base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[2][6];
					this.timerRicarica3 = 0f;
				}
				else
				{
					base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[2][3];
					this.timerRicarica3 = 0f;
				}
			}
		}
		if (this.timerFrequenzaArma3 > 1f && this.timerFrequenzaArma3 < base.GetComponent<PresenzaAlleato>().ListaArmi[2][1])
		{
			base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[2] = true;
			if (!this.suonoRicaricaAttivo)
			{
				this.suonoRicarica3.Play();
				this.suonoRicaricaAttivo = true;
			}
		}
		else
		{
			base.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[2] = false;
			this.suonoRicaricaAttivo = false;
		}
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00024EFC File Offset: 0x000230FC
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
				this.terzaCamera.GetComponent<Camera>().fieldOfView = 20f;
				this.zoomAttivo = true;
			}
		}
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00024FA8 File Offset: 0x000231A8
	private void CameraTPS()
	{
		this.timerPosizionamentoTPS += Time.deltaTime;
		if (this.timerPosizionamentoTPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = base.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraTPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localEulerAngles = new Vector3(30f, 0f, 0f);
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
			this.suonoArma1.Stop();
			this.suonoArma1Partito = false;
			this.particelleArma1.Stop();
			this.rotArma1Attiva = false;
		}
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00025070 File Offset: 0x00023270
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.corpoAereo.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localEulerAngles = this.baseArma2.transform.localEulerAngles;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
		}
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00025114 File Offset: 0x00023314
	private void PreparazioneAttacco()
	{
		float num = 0f;
		if (this.unitàBersaglio)
		{
			this.centroUnitàBersaglio = this.unitàBersaglio.GetComponent<PresenzaNemico>().centroInsetto;
			bool flag = Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale);
			float num2 = Vector3.Angle(-this.corpoAereo.transform.right, (this.centroUnitàBersaglio - this.corpoAereo.transform.position).normalized);
			float num3 = Vector3.Dot((this.centroUnitàBersaglio - this.corpoAereo.transform.position).normalized, -this.corpoAereo.transform.right);
			if (num2 < this.angDiTiro && num3 > 0f && !flag)
			{
				this.bersèNelMirino = true;
			}
			else
			{
				this.bersèNelMirino = false;
				this.rotArma1Attiva = false;
				this.suonoArma1.Stop();
				this.suonoArma1Partito = false;
				this.particelleArma1.Stop();
			}
			num = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
		}
		else
		{
			this.rotArma1Attiva = false;
			this.suonoArma1.Stop();
			this.suonoArma1Partito = false;
			this.particelleArma1.Stop();
		}
		if (!base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0])
		{
			this.rotArma1Attiva = false;
			this.suonoArma1.Stop();
			this.suonoArma1Partito = false;
			this.particelleArma1.Stop();
		}
		if (!base.GetComponent<PresenzaAlleato>().destinazioneOrdinata)
		{
			if (base.GetComponent<PresenzaAlleato>().attaccoOrdinato)
			{
				base.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
				this.unitàBersaglio = this.primaCamera.GetComponent<Selezionamento>().oggettoBersaglio;
				if (this.unitàBersaglio && !this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante && this.bersèNelMirino)
				{
					this.baseArma1.transform.LookAt(this.centroUnitàBersaglio);
					this.baseArma2.transform.LookAt(this.centroUnitàBersaglio);
					this.baseArma3.transform.LookAt(this.centroUnitàBersaglio);
					for (int i = 0; i < base.GetComponent<PresenzaAlleato>().numeroArmi; i++)
					{
						if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[i] && num < this.ListaMunizioniAttiveUnità[i].GetComponent<DatiGeneraliMunizione>().portataMassima)
						{
							if (i == 0)
							{
								this.AttaccoIndipendente1();
							}
							if (i == 1)
							{
								this.AttaccoIndipendente2();
							}
							if (i == 2)
							{
								this.AttaccoIndipendente3();
							}
						}
					}
					if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita <= 0f)
					{
						base.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
					}
				}
			}
			else
			{
				if (this.unitàBersaglio && !this.bersèNelMirino)
				{
					this.unitàBersaglio = null;
				}
				if (this.unitàBersaglio && !this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante && this.bersèNelMirino)
				{
					this.baseArma1.transform.LookAt(this.centroUnitàBersaglio);
					this.baseArma2.transform.LookAt(this.centroUnitàBersaglio);
					this.baseArma3.transform.LookAt(this.centroUnitàBersaglio);
					if (num > this.ListaMunizioniAttiveUnità[0].GetComponent<DatiGeneraliMunizione>().portataMassima)
					{
						this.rotArma1Attiva = false;
						this.suonoArma1.Stop();
						this.suonoArma1Partito = false;
						this.particelleArma1.Stop();
						this.unitàBersaglio = null;
					}
					for (int j = 0; j < base.GetComponent<PresenzaAlleato>().numeroArmi; j++)
					{
						if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[j] && num < this.ListaMunizioniAttiveUnità[j].GetComponent<DatiGeneraliMunizione>().portataMassima)
						{
							if (j == 0)
							{
								this.AttaccoIndipendente1();
							}
							if (j == 1)
							{
								this.AttaccoIndipendente2();
							}
							if (j == 2)
							{
								this.AttaccoIndipendente3();
							}
						}
					}
					if (this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita <= 0f)
					{
						this.unitàBersaglio = null;
						base.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
					}
					if (this.unitàBersaglio && !this.unitàBersaglio.GetComponent<PresenzaNemico>().èStatoVisto)
					{
						this.unitàBersaglio = null;
						base.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
					}
				}
				else if (this.unitàBersaglio == null)
				{
					if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti.Count > 0)
					{
						GestoreNeutroStrategia.valoreRandomSeed++;
						UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
						float f = UnityEngine.Random.Range(0f, (float)this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti.Count - 0.01f);
						bool flag2 = false;
						for (int k = Mathf.FloorToInt(f); k < this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti.Count; k++)
						{
							GameObject gameObject = this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti[k];
							if (gameObject != null)
							{
								Vector3 centroInsetto = gameObject.GetComponent<PresenzaNemico>().centroInsetto;
								if (!Physics.Linecast(this.bocca1.transform.position, centroInsetto, this.layerVisuale) && Vector3.Distance(base.transform.position, centroInsetto) < this.ListaMunizioniAttiveUnità[0].GetComponent<DatiGeneraliMunizione>().portataMassima)
								{
									Transform transform = this.corpoAereo.transform;
									float num4 = Vector3.Angle(-transform.right, (centroInsetto - transform.position).normalized);
									float num5 = Vector3.Dot((centroInsetto - transform.position).normalized, -transform.right);
									if (num4 < this.angDiTiro && num5 > 0f)
									{
										this.unitàBersaglio = gameObject;
										flag2 = true;
										break;
									}
								}
							}
						}
						if (!flag2)
						{
							for (int l = 0; l < Mathf.FloorToInt(f) - 1; l++)
							{
								GameObject gameObject2 = this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti[l];
								if (gameObject2 != null)
								{
									Vector3 centroInsetto2 = gameObject2.GetComponent<PresenzaNemico>().centroInsetto;
									if (!Physics.Linecast(this.bocca1.transform.position, centroInsetto2, this.layerVisuale) && Vector3.Distance(base.transform.position, centroInsetto2) < this.ListaMunizioniAttiveUnità[0].GetComponent<DatiGeneraliMunizione>().portataMassima)
									{
										Transform transform2 = this.corpoAereo.transform;
										float num6 = Vector3.Angle(-transform2.right, (centroInsetto2 - transform2.position).normalized);
										float num7 = Vector3.Dot((centroInsetto2 - transform2.position).normalized, -transform2.right);
										if (num6 < this.angDiTiro && num7 > 0f)
										{
											this.unitàBersaglio = gameObject2;
											break;
										}
									}
								}
							}
						}
					}
				}
				else if (base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers)
				{
					if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti.Count > 0)
					{
						GestoreNeutroStrategia.valoreRandomSeed++;
						UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
						float f2 = UnityEngine.Random.Range(0f, (float)this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti.Count - 0.01f);
						bool flag3 = false;
						for (int m = Mathf.FloorToInt(f2); m < this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti.Count; m++)
						{
							GameObject gameObject3 = this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti[m];
							if (gameObject3 != null && gameObject3.GetComponent<PresenzaNemico>().èStatoVisto)
							{
								Vector3 centroInsetto3 = gameObject3.GetComponent<PresenzaNemico>().centroInsetto;
								if (!Physics.Linecast(this.bocca1.transform.position, centroInsetto3, this.layerVisuale) && Vector3.Distance(base.transform.position, centroInsetto3) < this.ListaMunizioniAttiveUnità[0].GetComponent<DatiGeneraliMunizione>().portataMassima)
								{
									Transform transform3 = this.corpoAereo.transform;
									float num8 = Vector3.Angle(-transform3.right, (centroInsetto3 - transform3.position).normalized);
									float num9 = Vector3.Dot((centroInsetto3 - transform3.position).normalized, -transform3.right);
									if (num8 < this.angDiTiro && num9 > 0f)
									{
										this.unitàBersaglio = gameObject3;
										flag3 = true;
										break;
									}
								}
							}
						}
						if (!flag3)
						{
							for (int n = 0; n < Mathf.FloorToInt(f2) - 1; n++)
							{
								GameObject gameObject4 = this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti[n];
								if (gameObject4 != null && gameObject4.GetComponent<PresenzaNemico>().èStatoVisto)
								{
									Vector3 centroInsetto4 = gameObject4.GetComponent<PresenzaNemico>().centroInsetto;
									if (!Physics.Linecast(this.bocca1.transform.position, centroInsetto4, this.layerVisuale) && Vector3.Distance(base.transform.position, centroInsetto4) < this.ListaMunizioniAttiveUnità[0].GetComponent<DatiGeneraliMunizione>().portataMassima)
									{
										Transform transform4 = this.corpoAereo.transform;
										float num10 = Vector3.Angle(-transform4.right, (centroInsetto4 - transform4.position).normalized);
										float num11 = Vector3.Dot((centroInsetto4 - transform4.position).normalized, -transform4.right);
										if (num10 < this.angDiTiro && num11 > 0f)
										{
											this.unitàBersaglio = gameObject4;
											break;
										}
									}
								}
							}
						}
					}
				}
				else if (base.GetComponent<PresenzaAlleato>().ricercaAutomaticaBersaglio && this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti.Count > 0)
				{
					GestoreNeutroStrategia.valoreRandomSeed++;
					UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
					float f3 = UnityEngine.Random.Range(0f, (float)this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti.Count - 0.01f);
					bool flag4 = false;
					for (int num12 = Mathf.FloorToInt(f3); num12 < this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti.Count; num12++)
					{
						GameObject gameObject5 = this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti[num12];
						if (gameObject5 != null && gameObject5.GetComponent<PresenzaNemico>().èStatoVisto)
						{
							Vector3 centroInsetto5 = gameObject5.GetComponent<PresenzaNemico>().centroInsetto;
							if (!Physics.Linecast(this.bocca1.transform.position, centroInsetto5, this.layerVisuale) && Vector3.Distance(base.transform.position, centroInsetto5) < this.ListaMunizioniAttiveUnità[0].GetComponent<DatiGeneraliMunizione>().portataMassima)
							{
								Transform transform5 = this.corpoAereo.transform;
								float num13 = Vector3.Angle(-transform5.right, (centroInsetto5 - transform5.position).normalized);
								float num14 = Vector3.Dot((centroInsetto5 - transform5.position).normalized, -transform5.right);
								if (num13 < this.angDiTiro && num14 > 0f)
								{
									this.unitàBersaglio = gameObject5;
									flag4 = true;
									break;
								}
							}
						}
					}
					if (!flag4)
					{
						for (int num15 = 0; num15 < Mathf.FloorToInt(f3) - 1; num15++)
						{
							GameObject gameObject6 = this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti[num15];
							if (gameObject6 != null && gameObject6.GetComponent<PresenzaNemico>().èStatoVisto)
							{
								Vector3 centroInsetto6 = gameObject6.GetComponent<PresenzaNemico>().centroInsetto;
								if (!Physics.Linecast(this.bocca1.transform.position, centroInsetto6, this.layerVisuale) && Vector3.Distance(base.transform.position, centroInsetto6) < this.ListaMunizioniAttiveUnità[0].GetComponent<DatiGeneraliMunizione>().portataMassima)
								{
									Transform transform6 = this.corpoAereo.transform;
									float num16 = Vector3.Angle(-transform6.right, (centroInsetto6 - transform6.position).normalized);
									float num17 = Vector3.Dot((centroInsetto6 - transform6.position).normalized, -transform6.right);
									if (num16 < this.angDiTiro && num17 > 0f)
									{
										this.unitàBersaglio = gameObject6;
										break;
									}
								}
							}
						}
					}
				}
			}
		}
		else
		{
			this.unitàBersaglio = null;
			this.suonoArma1.Stop();
			this.suonoArma1Partito = false;
			this.particelleArma1.Stop();
			this.rotArma1Attiva = false;
		}
		if ((this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita < 0f) || base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] < 0f || !base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0])
		{
			this.suonoArma1.Stop();
			this.suonoArma1Partito = false;
			this.particelleArma1.Stop();
			this.rotArma1Attiva = false;
		}
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00025F90 File Offset: 0x00024190
	private void AttaccoIndipendente1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][0])
		{
			this.timerFrequenzaArma1 = 0f;
			this.particelleArma1.Play();
			List<float> list;
			List<float> expr_AA = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int index;
			int expr_AD = index = 5;
			float num = list[index];
			expr_AA[expr_AD] = num - 1f;
			List<float> list2;
			List<float> expr_D4 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int expr_D7 = index = 6;
			num = list2[index];
			expr_D4[expr_D7] = num - 1f;
			this.SparoIndipendente1();
			this.rotArma1Attiva = true;
			if (!this.suonoArma1Partito)
			{
				this.suonoArma1.Play();
				this.suonoArma1Partito = true;
			}
		}
	}

	// Token: 0x060000CB RID: 203 RVA: 0x000260B4 File Offset: 0x000242B4
	private void SparoIndipendente1()
	{
		this.proiettileArma1 = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.proiettileArma1.GetComponent<DatiProiettile>().target = this.unitàBersaglio;
		this.proiettileArma1.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00026128 File Offset: 0x00024328
	private void AttaccoIndipendente2()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[1] && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] > 0f && this.timerFrequenzaArma2 > base.GetComponent<PresenzaAlleato>().ListaArmi[1][0])
		{
			this.timerFrequenzaArma2 = 0f;
			this.particelleArma2.Play();
			this.suonoArma2.Play();
			List<float> list;
			List<float> expr_B5 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
			int index;
			int expr_B8 = index = 5;
			float num = list[index];
			expr_B5[expr_B8] = num - 1f;
			List<float> list2;
			List<float> expr_DF = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
			int expr_E2 = index = 6;
			num = list2[index];
			expr_DF[expr_E2] = num - 1f;
			this.SparoIndipendente2();
		}
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00026234 File Offset: 0x00024434
	private void SparoIndipendente2()
	{
		this.proiettileArma2 = (UnityEngine.Object.Instantiate(this.munizioneArma2, this.bocca2.transform.position, this.bocca2.transform.rotation) as GameObject);
		this.proiettileArma2.GetComponent<DatiProiettile>().locazioneTarget = this.centroUnitàBersaglio;
		this.proiettileArma2.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060000CE RID: 206 RVA: 0x000262A8 File Offset: 0x000244A8
	private void AttaccoIndipendente3()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[2] && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] > 0f && this.timerFrequenzaArma3 > base.GetComponent<PresenzaAlleato>().ListaArmi[2][0])
		{
			this.timerFrequenzaArma3 = 0f;
			this.particelleArma3.Play();
			this.suonoArma3.Play();
			List<float> list;
			List<float> expr_B5 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[2];
			int index;
			int expr_B8 = index = 5;
			float num = list[index];
			expr_B5[expr_B8] = num - 1f;
			List<float> list2;
			List<float> expr_DF = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[2];
			int expr_E2 = index = 6;
			num = list2[index];
			expr_DF[expr_E2] = num - 1f;
			this.SparoIndipendente3();
		}
	}

	// Token: 0x060000CF RID: 207 RVA: 0x000263B4 File Offset: 0x000245B4
	private void SparoIndipendente3()
	{
		this.proiettileArma3 = (UnityEngine.Object.Instantiate(this.munizioneArma3, this.bocca3.transform.position, this.bocca3.transform.rotation) as GameObject);
		this.proiettileArma3.GetComponent<DatiProiettile>().locazioneTarget = this.centroUnitàBersaglio;
		this.proiettileArma3.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00026428 File Offset: 0x00024628
	private void SelezioneArma()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 0;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 1;
			this.suonoArma1.Stop();
			this.suonoArma1Partito = false;
			this.particelleArma1.Stop();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 2;
			this.suonoArma1.Stop();
			this.suonoArma1Partito = false;
			this.particelleArma1.Stop();
		}
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x000264B8 File Offset: 0x000246B8
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
			List<float> list;
			List<float> expr_198 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int index;
			int expr_19B = index = 5;
			float num = list[index];
			expr_198[expr_19B] = num - 1f;
			List<float> list2;
			List<float> expr_1C2 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int expr_1C6 = index = 6;
			num = list2[index];
			expr_1C2[expr_1C6] = num - 1f;
			this.particelleArma1.Play();
			this.rotArma1Attiva = true;
			if (!this.suonoArma1Partito)
			{
				this.suonoArma1.Play();
				this.suonoArma1Partito = true;
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.suonoArma1.Stop();
			this.suonoArma1Partito = false;
			this.particelleArma1.Stop();
			this.rotArma1Attiva = false;
		}
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00026700 File Offset: 0x00024900
	private void SparoArma1()
	{
		this.proiettileArma1 = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.proiettileArma1.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.proiettileArma1.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00026770 File Offset: 0x00024970
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
			List<float> expr_1CD = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
			int expr_1D1 = index = 6;
			num = list2[index];
			expr_1CD[expr_1D1] = num - 1f;
			this.particelleArma2.Play();
		}
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x00026970 File Offset: 0x00024B70
	private void SparoArma2()
	{
		this.proiettileArma2 = (UnityEngine.Object.Instantiate(this.munizioneArma2, this.bocca2.transform.position, this.bocca2.transform.rotation) as GameObject);
		this.proiettileArma2.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.proiettileArma2.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x000269E0 File Offset: 0x00024BE0
	private void AttaccoPrimaPersonaArma3()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.targetSparo))
		{
			if (this.targetSparo.collider.gameObject.tag == "Nemico" || this.targetSparo.collider.gameObject.tag == "Nemico Testa" || this.targetSparo.collider.gameObject.tag == "Nemico Coll Suppl")
			{
				if (Vector3.Distance(base.transform.position, this.targetSparo.point) <= this.munizioneArma3.GetComponent<DatiGeneraliMunizione>().portataMassima)
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
			List<float> expr_1A3 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[2];
			int index;
			int expr_1A6 = index = 5;
			float num = list[index];
			expr_1A3[expr_1A6] = num - 1f;
			List<float> list2;
			List<float> expr_1CD = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[2];
			int expr_1D1 = index = 6;
			num = list2[index];
			expr_1CD[expr_1D1] = num - 1f;
			this.particelleArma3.Play();
		}
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00026BE0 File Offset: 0x00024DE0
	private void SparoArma3()
	{
		this.proiettileArma3 = (UnityEngine.Object.Instantiate(this.munizioneArma3, this.bocca3.transform.position, this.bocca3.transform.rotation) as GameObject);
		this.proiettileArma3.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.proiettileArma3.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00026C50 File Offset: 0x00024E50
	private void ControlloArmiPrimarie()
	{
		bool flag = true;
		for (int i = 0; i < 3; i++)
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

	// Token: 0x04000401 RID: 1025
	public float angDiTiro;

	// Token: 0x04000402 RID: 1026
	private GameObject infoNeutreTattica;

	// Token: 0x04000403 RID: 1027
	private GameObject primaCamera;

	// Token: 0x04000404 RID: 1028
	public GameObject bocca1;

	// Token: 0x04000405 RID: 1029
	public GameObject bocca2;

	// Token: 0x04000406 RID: 1030
	public GameObject bocca3;

	// Token: 0x04000407 RID: 1031
	private GameObject terzaCamera;

	// Token: 0x04000408 RID: 1032
	private GameObject IANemico;

	// Token: 0x04000409 RID: 1033
	private GameObject InfoAlleati;

	// Token: 0x0400040A RID: 1034
	private float timerFrequenzaArma1;

	// Token: 0x0400040B RID: 1035
	private float timerRicarica1;

	// Token: 0x0400040C RID: 1036
	private bool ricaricaInCorso1;

	// Token: 0x0400040D RID: 1037
	private float timerDopoSparo1;

	// Token: 0x0400040E RID: 1038
	private float tempoFraSparoERicarica1;

	// Token: 0x0400040F RID: 1039
	private float timerFrequenzaArma2;

	// Token: 0x04000410 RID: 1040
	private float timerRicarica2;

	// Token: 0x04000411 RID: 1041
	private bool ricaricaInCorso2;

	// Token: 0x04000412 RID: 1042
	private float timerDopoSparo2;

	// Token: 0x04000413 RID: 1043
	private float tempoFraSparoERicarica2;

	// Token: 0x04000414 RID: 1044
	private float timerFrequenzaArma3;

	// Token: 0x04000415 RID: 1045
	private float timerRicarica3;

	// Token: 0x04000416 RID: 1046
	private bool ricaricaInCorso3;

	// Token: 0x04000417 RID: 1047
	private int layerColpo;

	// Token: 0x04000418 RID: 1048
	private int layerVisuale;

	// Token: 0x04000419 RID: 1049
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x0400041A RID: 1050
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x0400041B RID: 1051
	private float timerPosizionamentoTPS;

	// Token: 0x0400041C RID: 1052
	private float timerPosizionamentoFPS;

	// Token: 0x0400041D RID: 1053
	private GameObject CanvasFPS;

	// Token: 0x0400041E RID: 1054
	private GameObject mirinoElettr1;

	// Token: 0x0400041F RID: 1055
	public Sprite mirinoTPS;

	// Token: 0x04000420 RID: 1056
	public Sprite mirinoFPS;

	// Token: 0x04000421 RID: 1057
	private RaycastHit targetSparo;

	// Token: 0x04000422 RID: 1058
	private GameObject proiettileArma1;

	// Token: 0x04000423 RID: 1059
	private GameObject proiettileArma2;

	// Token: 0x04000424 RID: 1060
	private GameObject proiettileArma3;

	// Token: 0x04000425 RID: 1061
	public GameObject baseArma1;

	// Token: 0x04000426 RID: 1062
	public GameObject baseArma2;

	// Token: 0x04000427 RID: 1063
	public GameObject baseArma3;

	// Token: 0x04000428 RID: 1064
	private GameObject unitàBersaglio;

	// Token: 0x04000429 RID: 1065
	private Vector3 centroUnitàBersaglio;

	// Token: 0x0400042A RID: 1066
	private GameObject munizioneArma1;

	// Token: 0x0400042B RID: 1067
	private GameObject munizioneArma2;

	// Token: 0x0400042C RID: 1068
	private GameObject munizioneArma3;

	// Token: 0x0400042D RID: 1069
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x0400042E RID: 1070
	private AudioSource suonoArma1;

	// Token: 0x0400042F RID: 1071
	private AudioSource suonoArma2;

	// Token: 0x04000430 RID: 1072
	private AudioSource suonoArma3;

	// Token: 0x04000431 RID: 1073
	private AudioSource suonoRicarica1;

	// Token: 0x04000432 RID: 1074
	private AudioSource suonoRicarica2;

	// Token: 0x04000433 RID: 1075
	private AudioSource suonoRicarica3;

	// Token: 0x04000434 RID: 1076
	private bool suonoRicaricaAttivo;

	// Token: 0x04000435 RID: 1077
	private bool suonoArma1Partito;

	// Token: 0x04000436 RID: 1078
	private AudioSource suonoMotore;

	// Token: 0x04000437 RID: 1079
	private AudioSource suonoInterno;

	// Token: 0x04000438 RID: 1080
	private float volumeMotoreIniziale;

	// Token: 0x04000439 RID: 1081
	private ParticleSystem particelleArma1;

	// Token: 0x0400043A RID: 1082
	private ParticleSystem particelleArma2;

	// Token: 0x0400043B RID: 1083
	private ParticleSystem particelleArma3;

	// Token: 0x0400043C RID: 1084
	private bool bersèNelMirino;

	// Token: 0x0400043D RID: 1085
	private GameObject corpoAereo;

	// Token: 0x0400043E RID: 1086
	private bool zoomAttivo;

	// Token: 0x0400043F RID: 1087
	private bool rotArma1Attiva;

	// Token: 0x04000440 RID: 1088
	private GameObject corpoArma1;

	// Token: 0x04000441 RID: 1089
	private float timerSenzaArmiPrimarie;
}
