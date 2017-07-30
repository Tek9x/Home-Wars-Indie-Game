using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000030 RID: 48
public class ATT_Mech : MonoBehaviour
{
	// Token: 0x06000237 RID: 567 RVA: 0x00060264 File Offset: 0x0005E464
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
		this.ListaMunizioniAttiveUnità = new List<GameObject>();
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma1);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma2);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma3);
		this.suonoTorretta = base.transform.GetChild(1).transform.GetChild(0).GetComponent<AudioSource>();
		this.suonoInterno = base.transform.GetChild(1).GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.suonoMotore.clip = this.motoreFermo;
		this.suonoMotore.Play();
		this.volumeMotoreIniziale = this.suonoMotore.volume;
		this.tempoFraSparoERicarica2 = 1f;
		this.tempoFraSparoERicarica3 = 1f;
		this.suonoArma2 = this.bocca2.GetComponent<AudioSource>();
		if (GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.moltiplicatoreAttaccoInFPS = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().moltiplicatoreFPSBattVeloce;
		}
		else
		{
			this.moltiplicatoreAttaccoInFPS = PlayerPrefs.GetFloat("moltiplicatore danni PP");
		}
		this.suonoArma1 = this.bocca1.GetComponent<AudioSource>();
		this.suonoArma2 = this.bocca2.GetComponent<AudioSource>();
		this.suonoArma31 = this.bocca31.GetComponent<AudioSource>();
		this.suonoArma32 = this.bocca32.GetComponent<AudioSource>();
		this.suonoRicarica1 = this.corpoArma1.GetComponent<AudioSource>();
		this.suonoRicarica2 = this.corpoArma2.GetComponent<AudioSource>();
		this.suonoRicarica3 = this.bocca31.transform.parent.GetComponent<AudioSource>();
		this.ListaSuoniArma3 = new List<AudioSource>();
		this.ListaSuoniArma3.Add(this.suonoArma31);
		this.ListaSuoniArma3.Add(this.suonoArma32);
		this.particelleArma1 = this.bocca1.GetComponent<ParticleSystem>();
		this.particelleArma2 = this.bocca2.GetComponent<ParticleSystem>();
		this.particelleArma31 = this.bocca31.GetComponent<ParticleSystem>();
		this.particelleArma32 = this.bocca32.GetComponent<ParticleSystem>();
		this.particelleArma1Bis = this.bocca1.transform.GetChild(0).GetComponent<ParticleSystem>();
		this.ListaParticelleArma3 = new List<ParticleSystem>();
		this.ListaParticelleArma3.Add(this.particelleArma31);
		this.ListaParticelleArma3.Add(this.particelleArma32);
		this.ListaBoccheArma3 = new List<GameObject>();
		this.ListaBoccheArma3.Add(this.bocca31);
		this.ListaBoccheArma3.Add(this.bocca32);
		this.torretta = base.transform.GetChild(1).transform.GetChild(0).gameObject;
		this.sparoArma1 = this.bocca1.transform.GetChild(0).gameObject;
		this.mechAnim = base.GetComponent<Animator>();
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.velocitàIniziale = this.alleatoNav.speed;
	}

	// Token: 0x06000238 RID: 568 RVA: 0x0006062C File Offset: 0x0005E82C
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
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.rotArma1Attiva = false;
			this.rotArma2Attiva = false;
			this.GestioneSuoniIndipendenti();
			if (!this.alleatoNav.isOnOffMeshLink)
			{
				this.PreparazioneAttacco();
				this.calcoloJumpEffettuato = false;
			}
			else
			{
				this.InJump();
			}
			if (!this.unitàBersaglio)
			{
				this.rotArma1Attiva = false;
				this.rotArma2Attiva = false;
				this.suonoArma1.Stop();
				this.suonoArma1Partito = false;
				this.particelleArma1.Stop();
				this.particelleArma1Bis.Stop();
			}
		}
		else
		{
			this.armaAttivaInFPS = base.GetComponent<PresenzaAlleato>().armaAttivaInFPS;
			this.GestioneVisuali();
			this.SelezioneArma();
			if (this.armaAttivaInFPS == 0)
			{
				this.AttaccoPrimaPersonaArma1();
			}
			if (this.armaAttivaInFPS == 1)
			{
				this.AttaccoPrimaPersonaArma2();
			}
			if (this.armaAttivaInFPS == 2)
			{
				this.AttaccoPrimaPersonaArma3();
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
					this.cockpit.GetComponent<MeshRenderer>().enabled = true;
					this.zoomAttivo = false;
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					this.mirinoElettr1.GetComponent<Image>().sprite = this.mirinoTPS;
					this.mirinoElettr1.GetComponent<CanvasGroup>().alpha = 1f;
					this.suonoInterno.Stop();
					this.cockpit.GetComponent<MeshRenderer>().enabled = false;
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
			this.torretta.transform.rotation = base.transform.rotation;
			this.baseArma1e2.transform.rotation = base.transform.rotation;
			this.baseArma3.transform.rotation = base.transform.rotation;
			this.suonoTorretta.Stop();
			base.GetComponent<MOV_Mech>().suonoTorrettaPartito = false;
			this.rotArma1Attiva = false;
			this.rotArma2Attiva = false;
			this.cockpit.GetComponent<MeshRenderer>().enabled = false;
			this.zoomAttivo = false;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x06000239 RID: 569 RVA: 0x00060B34 File Offset: 0x0005ED34
	private void InJump()
	{
		if (!this.calcoloJumpEffettuato)
		{
			this.calcoloDistJump = true;
		}
		if (this.calcoloDistJump)
		{
			this.calcoloDistJump = false;
			this.calcoloJumpEffettuato = true;
			float num = Mathf.Abs(this.alleatoNav.destination.y - base.transform.position.y);
			this.alleatoNav.speed = this.velocitàIniziale / (num / 80f) / 10f;
		}
	}

	// Token: 0x0600023A RID: 570 RVA: 0x00060BB8 File Offset: 0x0005EDB8
	private void GestioneSuoniIndipendenti()
	{
		this.suonoMotore.volume = this.volumeMotoreIniziale;
		this.suonoInterno.Stop();
		if (this.alleatoNav.velocity.magnitude > 0f && !this.inMovimento)
		{
			this.suonoMotore.clip = this.motoreViaggio;
			this.suonoMotore.Play();
			this.inMovimento = true;
			this.fermo = false;
			this.mechAnim.SetBool(this.camminataHash, true);
		}
		if (this.alleatoNav.velocity.magnitude == 0f && !this.fermo)
		{
			this.suonoMotore.clip = this.motoreFermo;
			this.suonoMotore.Play();
			this.inMovimento = false;
			this.fermo = true;
			this.mechAnim.SetBool(this.camminataHash, false);
		}
	}

	// Token: 0x0600023B RID: 571 RVA: 0x00060CAC File Offset: 0x0005EEAC
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
			this.rotArma1Attiva = false;
			this.suonoArma1.Stop();
			this.suonoArma1Partito = false;
			this.particelleArma1.Stop();
			this.particelleArma1Bis.Stop();
		}
		if (this.rotArma1Attiva)
		{
			this.corpoArma1.transform.Rotate(Vector3.up * 12f);
		}
	}

	// Token: 0x0600023C RID: 572 RVA: 0x00060F64 File Offset: 0x0005F164
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
			this.rotArma2Attiva = false;
		}
		if (this.rotArma2Attiva)
		{
			this.corpoArma2.transform.Rotate(Vector3.up * 7f);
		}
	}

	// Token: 0x0600023D RID: 573 RVA: 0x000611F4 File Offset: 0x0005F3F4
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
		if (this.cannone3Attivo >= 2)
		{
			this.cannone3Attivo = 0;
		}
	}

	// Token: 0x0600023E RID: 574 RVA: 0x00061464 File Offset: 0x0005F664
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

	// Token: 0x0600023F RID: 575 RVA: 0x00061510 File Offset: 0x0005F710
	private void CameraTPS()
	{
		this.timerPosizionamentoTPS += Time.deltaTime;
		if (this.timerPosizionamentoTPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.baseArma1e2.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraTPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localEulerAngles = new Vector3(0f, 0f, this.baseArma1e2.transform.eulerAngles.z);
			this.rotArma1Attiva = false;
			this.rotArma2Attiva = false;
			this.suonoArma1.Stop();
			this.suonoArma1Partito = false;
			this.particelleArma1.Stop();
			this.particelleArma1Bis.Stop();
			this.suonoMotore.Stop();
			this.inMovimento = false;
			this.fermo = false;
		}
	}

	// Token: 0x06000240 RID: 576 RVA: 0x00061608 File Offset: 0x0005F808
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.torretta.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = this.baseArma1e2.transform.rotation;
		}
	}

	// Token: 0x06000241 RID: 577 RVA: 0x00061698 File Offset: 0x0005F898
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
		for (int i = 0; i < 3; i++)
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
				if (this.unitàBersaglio)
				{
					float num3 = Vector3.Dot((this.centroUnitàBersaglio - base.transform.position).normalized, base.transform.up);
					if (num3 < this.angVertMax && num3 > this.angVertMin)
					{
						float num4 = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
						if (num4 > this.ListaMunizioniAttiveUnità[0].GetComponent<DatiGeneraliMunizione>().portataMassima)
						{
							this.rotArma1Attiva = false;
							this.suonoArma1.Stop();
							this.suonoArma1Partito = false;
							this.particelleArma1.Stop();
							this.particelleArma1Bis.Stop();
						}
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
								this.baseArma1e2.transform.LookAt(this.centroUnitàBersaglio);
								this.baseArma3.transform.LookAt(this.centroUnitàBersaglio);
								if (j == 0)
								{
									this.AttaccoIndipendente1();
									this.rotArma1Attiva = true;
								}
								if (j == 1)
								{
									this.AttaccoIndipendente2();
									this.rotArma2Attiva = true;
								}
								if (j == 2)
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
								this.baseArma1e2.transform.LookAt(this.centroUnitàBersaglio);
								this.baseArma3.transform.LookAt(this.centroUnitàBersaglio);
								this.alleatoNav.speed = 0f;
							}
							else
							{
								this.rotArma1Attiva = false;
								this.rotArma2Attiva = false;
								this.suonoArma1.Stop();
								this.suonoArma1Partito = false;
								this.particelleArma1.Stop();
								this.particelleArma1Bis.Stop();
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
							this.rotArma1Attiva = false;
							this.rotArma2Attiva = false;
							this.suonoArma1.Stop();
							this.suonoArma1Partito = false;
							this.particelleArma1.Stop();
							this.particelleArma1Bis.Stop();
						}
					}
					this.rotArma1Attiva = false;
					this.rotArma2Attiva = false;
					this.suonoArma1.Stop();
					this.suonoArma1Partito = false;
					this.particelleArma1.Stop();
					this.particelleArma1Bis.Stop();
				}
			}
			else if (this.unitàBersaglio && this.alleatoNav.enabled)
			{
				float num5 = Vector3.Dot((this.centroUnitàBersaglio - base.transform.position).normalized, base.transform.up);
				float num6 = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
				if (num5 < this.angVertMax && num5 > this.angVertMin)
				{
					if (num6 > this.ListaMunizioniAttiveUnità[0].GetComponent<DatiGeneraliMunizione>().portataMassima)
					{
						this.rotArma1Attiva = false;
						this.suonoArma1.Stop();
						this.suonoArma1Partito = false;
						this.particelleArma1.Stop();
						this.particelleArma1Bis.Stop();
					}
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
							this.baseArma1e2.transform.LookAt(this.centroUnitàBersaglio);
							this.baseArma3.transform.LookAt(this.centroUnitàBersaglio);
							if (k == 0)
							{
								this.AttaccoIndipendente1();
								this.rotArma1Attiva = true;
							}
							else if (k == 1)
							{
								this.AttaccoIndipendente2();
								this.rotArma2Attiva = true;
							}
							if (k == 2)
							{
								this.AttaccoIndipendente3();
							}
						}
					}
					if (!base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0])
					{
						this.rotArma1Attiva = false;
						this.suonoArma1.Stop();
						this.suonoArma1Partito = false;
						this.particelleArma1.Stop();
						this.particelleArma1Bis.Stop();
					}
					if (num6 < num2)
					{
						if (!flag)
						{
							base.transform.LookAt(new Vector3(this.centroUnitàBersaglio.x, base.transform.position.y, this.centroUnitàBersaglio.z));
							this.baseArma1e2.transform.LookAt(this.centroUnitàBersaglio);
							this.baseArma3.transform.LookAt(this.centroUnitàBersaglio);
							this.alleatoNav.speed = 0f;
						}
						else
						{
							this.rotArma1Attiva = false;
							this.rotArma2Attiva = false;
							this.suonoArma1.Stop();
							this.suonoArma1Partito = false;
							this.particelleArma1.Stop();
							this.particelleArma1Bis.Stop();
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
					this.rotArma1Attiva = false;
					this.rotArma2Attiva = false;
					this.suonoArma1.Stop();
					this.suonoArma1Partito = false;
					this.particelleArma1.Stop();
					this.particelleArma1Bis.Stop();
					if (num6 > 3f)
					{
						this.alleatoNav.speed = this.velocitàAlleatoNav;
						this.unitàBersaglio = null;
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
			this.rotArma1Attiva = false;
			this.rotArma2Attiva = false;
			this.suonoArma1.Stop();
			this.suonoArma1Partito = false;
			this.particelleArma1.Stop();
			this.particelleArma1Bis.Stop();
		}
	}

	// Token: 0x06000242 RID: 578 RVA: 0x00062560 File Offset: 0x00060760
	private void AttaccoIndipendente1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f)
		{
			if (this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][0])
			{
				this.timerFrequenzaArma1 = 0f;
				List<float> list;
				List<float> expr_C5 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
				int index;
				int expr_C8 = index = 5;
				float num = list[index];
				expr_C5[expr_C8] = num - 1f;
				List<float> list2;
				List<float> expr_EF = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
				int expr_F3 = index = 6;
				num = list2[index];
				expr_EF[expr_F3] = num - 1f;
				this.SparoIndipendente1();
				if (!this.suonoArma1Partito)
				{
					this.suonoArma1.Play();
					this.suonoArma1Partito = true;
					this.particelleArma1.Play();
				}
			}
			Vector3 normalized = (this.centroUnitàBersaglio - this.sparoArma1.transform.position).normalized;
			this.sparoArma1.transform.forward = normalized;
		}
	}

	// Token: 0x06000243 RID: 579 RVA: 0x000626DC File Offset: 0x000608DC
	private void SparoIndipendente1()
	{
		float num = 0f;
		if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().penetrazione)
		{
			num = this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().penetrazione;
		}
		else if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f)
		{
			num = this.unitàBersaglio.GetComponent<PresenzaNemico>().vita;
		}
		this.unitàBersaglio.GetComponent<PresenzaNemico>().vita -= this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().penetrazione;
		if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.unitàBersaglio.GetComponent<PresenzaNemico>().armatura))
		{
			num += this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.unitàBersaglio.GetComponent<PresenzaNemico>().armatura);
		}
		else if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f)
		{
			num += this.unitàBersaglio.GetComponent<PresenzaNemico>().vita;
		}
		this.unitàBersaglio.GetComponent<PresenzaNemico>().vita -= this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.unitàBersaglio.GetComponent<PresenzaNemico>().armatura);
		List<float> listaDanniAlleati;
		List<float> expr_179 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
		int tipoTruppa;
		int expr_186 = tipoTruppa = base.GetComponent<PresenzaAlleato>().tipoTruppa;
		float num2 = listaDanniAlleati[tipoTruppa];
		expr_179[expr_186] = num2 + num;
	}

	// Token: 0x06000244 RID: 580 RVA: 0x00062884 File Offset: 0x00060A84
	private void AttaccoIndipendente2()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[1] && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca2.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] > 0f && this.timerFrequenzaArma2 > base.GetComponent<PresenzaAlleato>().ListaArmi[1][0])
		{
			this.timerFrequenzaArma2 = 0f;
			this.particelleArma2.Play();
			this.suonoArma2.Play();
			List<float> list;
			List<float> expr_DB = list = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
			int index;
			int expr_DE = index = 5;
			float num = list[index];
			expr_DB[expr_DE] = num - 1f;
			List<float> list2;
			List<float> expr_105 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[1];
			int expr_108 = index = 6;
			num = list2[index];
			expr_105[expr_108] = num - 1f;
			this.SparoIndipendente2();
		}
	}

	// Token: 0x06000245 RID: 581 RVA: 0x000629B8 File Offset: 0x00060BB8
	private void SparoIndipendente2()
	{
		this.proiettileArma2 = (UnityEngine.Object.Instantiate(this.munizioneArma2, this.bocca2.transform.position, this.bocca2.transform.rotation) as GameObject);
		this.proiettileArma2.GetComponent<DatiProiettile>().locazioneTarget = this.centroUnitàBersaglio;
		this.proiettileArma2.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x06000246 RID: 582 RVA: 0x00062A2C File Offset: 0x00060C2C
	private void AttaccoIndipendente3()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[2] && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca32.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] > 0f && this.timerFrequenzaArma3 > base.GetComponent<PresenzaAlleato>().ListaArmi[2][0])
		{
			this.timerFrequenzaArma3 = 0f;
			this.ListaSuoniArma3[this.cannone3Attivo].Play();
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
			this.timerDopoSparo3 = 0f;
			this.cannone3Attivo++;
		}
	}

	// Token: 0x06000247 RID: 583 RVA: 0x00062B78 File Offset: 0x00060D78
	private void SparoIndipendente3()
	{
		this.proiettileArma3 = (UnityEngine.Object.Instantiate(this.munizioneArma3, this.ListaBoccheArma3[this.cannone3Attivo].transform.position, this.ListaBoccheArma3[this.cannone3Attivo].transform.rotation) as GameObject);
		this.proiettileArma3.GetComponent<DatiProiettile>().locazioneTarget = this.centroUnitàBersaglio;
		this.proiettileArma3.GetComponent<DatiProiettile>().target = this.unitàBersaglio;
		this.proiettileArma3.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x06000248 RID: 584 RVA: 0x00062C18 File Offset: 0x00060E18
	private void SelezioneArma()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 0;
			this.timerPosizionamentoFPS = 0f;
			this.rotArma1Attiva = false;
			this.rotArma2Attiva = false;
			this.suonoArma1.Stop();
			this.suonoArma1Partito = false;
			this.particelleArma1.Stop();
			this.particelleArma1Bis.Stop();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 1;
			this.timerPosizionamentoFPS = 0f;
			this.rotArma1Attiva = false;
			this.rotArma2Attiva = false;
			this.suonoArma1.Stop();
			this.suonoArma1Partito = false;
			this.particelleArma1.Stop();
			this.particelleArma1Bis.Stop();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 2;
			this.timerPosizionamentoFPS = 0f;
			this.rotArma1Attiva = false;
			this.rotArma2Attiva = false;
			this.suonoArma1.Stop();
			this.suonoArma1Partito = false;
			this.particelleArma1.Stop();
			this.particelleArma1Bis.Stop();
		}
	}

	// Token: 0x06000249 RID: 585 RVA: 0x00062D30 File Offset: 0x00060F30
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
				this.timerFrequenzaArma1 = 0f;
				this.SparoArma1();
				List<float> list;
				List<float> expr_1D4 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
				int index;
				int expr_1D7 = index = 5;
				float num = list[index];
				expr_1D4[expr_1D7] = num - 1f;
				List<float> list2;
				List<float> expr_200 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
				int expr_204 = index = 6;
				num = list2[index];
				expr_200[expr_204] = num - 1f;
				if (!this.suonoArma1Partito)
				{
					this.suonoArma1.Play();
					this.suonoArma1Partito = true;
					this.particelleArma1.Play();
				}
			}
			Vector3 normalized = (this.targetSparo.point - this.sparoArma1.transform.position).normalized;
			this.sparoArma1.transform.forward = normalized;
			this.rotArma1Attiva = true;
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.rotArma1Attiva = false;
			this.suonoArma1.Stop();
			this.suonoArma1Partito = false;
			this.particelleArma1.Stop();
			this.particelleArma1Bis.Stop();
		}
	}

	// Token: 0x0600024A RID: 586 RVA: 0x00063000 File Offset: 0x00061200
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

	// Token: 0x0600024B RID: 587 RVA: 0x00063658 File Offset: 0x00061858
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

	// Token: 0x0600024C RID: 588 RVA: 0x0006389C File Offset: 0x00061A9C
	private void SparoArma2()
	{
		this.proiettileArma2 = (UnityEngine.Object.Instantiate(this.munizioneArma2, this.bocca2.transform.position, this.bocca2.transform.rotation) as GameObject);
		this.proiettileArma2.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.proiettileArma2.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x0600024D RID: 589 RVA: 0x0006390C File Offset: 0x00061B0C
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
			this.ListaSuoniArma3[this.cannone3Attivo].Play();
			List<float> list;
			List<float> expr_1EA = list = base.GetComponent<PresenzaAlleato>().ListaArmi[2];
			int index;
			int expr_1ED = index = 5;
			float num = list[index];
			expr_1EA[expr_1ED] = num - 1f;
			List<float> list2;
			List<float> expr_214 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[2];
			int expr_218 = index = 6;
			num = list2[index];
			expr_214[expr_218] = num - 1f;
			this.timerDopoSparo3 = 0f;
			this.cannone3Attivo++;
		}
	}

	// Token: 0x0600024E RID: 590 RVA: 0x00063B64 File Offset: 0x00061D64
	private void SparoArma3()
	{
		this.proiettileArma3 = (UnityEngine.Object.Instantiate(this.munizioneArma3, this.ListaBoccheArma3[this.cannone3Attivo].transform.position, this.ListaBoccheArma3[this.cannone3Attivo].transform.rotation) as GameObject);
		this.proiettileArma3.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.proiettileArma3.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x040009A6 RID: 2470
	public float angVertMax;

	// Token: 0x040009A7 RID: 2471
	public float angVertMin;

	// Token: 0x040009A8 RID: 2472
	private GameObject infoNeutreTattica;

	// Token: 0x040009A9 RID: 2473
	private GameObject terzaCamera;

	// Token: 0x040009AA RID: 2474
	private GameObject primaCamera;

	// Token: 0x040009AB RID: 2475
	public GameObject bocca1;

	// Token: 0x040009AC RID: 2476
	public GameObject bocca2;

	// Token: 0x040009AD RID: 2477
	public GameObject bocca31;

	// Token: 0x040009AE RID: 2478
	public GameObject bocca32;

	// Token: 0x040009AF RID: 2479
	private GameObject IANemico;

	// Token: 0x040009B0 RID: 2480
	private GameObject InfoAlleati;

	// Token: 0x040009B1 RID: 2481
	private float timerFrequenzaArma1;

	// Token: 0x040009B2 RID: 2482
	private float timerRicarica1;

	// Token: 0x040009B3 RID: 2483
	private bool ricaricaInCorso1;

	// Token: 0x040009B4 RID: 2484
	private float timerDopoSparo1;

	// Token: 0x040009B5 RID: 2485
	private float tempoFraSparoERicarica1;

	// Token: 0x040009B6 RID: 2486
	private float timerFrequenzaArma2;

	// Token: 0x040009B7 RID: 2487
	private float timerRicarica2;

	// Token: 0x040009B8 RID: 2488
	private bool ricaricaInCorso2;

	// Token: 0x040009B9 RID: 2489
	private float timerDopoSparo2;

	// Token: 0x040009BA RID: 2490
	private float tempoFraSparoERicarica2;

	// Token: 0x040009BB RID: 2491
	private float timerFrequenzaArma3;

	// Token: 0x040009BC RID: 2492
	private float timerRicarica3;

	// Token: 0x040009BD RID: 2493
	private bool ricaricaInCorso3;

	// Token: 0x040009BE RID: 2494
	private float timerDopoSparo3;

	// Token: 0x040009BF RID: 2495
	private float tempoFraSparoERicarica3;

	// Token: 0x040009C0 RID: 2496
	private int layerColpo;

	// Token: 0x040009C1 RID: 2497
	private int layerVisuale;

	// Token: 0x040009C2 RID: 2498
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x040009C3 RID: 2499
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x040009C4 RID: 2500
	private float timerPosizionamentoTPS;

	// Token: 0x040009C5 RID: 2501
	private float timerPosizionamentoFPS;

	// Token: 0x040009C6 RID: 2502
	private GameObject CanvasFPS;

	// Token: 0x040009C7 RID: 2503
	private GameObject mirinoElettr1;

	// Token: 0x040009C8 RID: 2504
	public Sprite mirinoTPS;

	// Token: 0x040009C9 RID: 2505
	public Sprite mirinoFPS;

	// Token: 0x040009CA RID: 2506
	private RaycastHit targetSparo;

	// Token: 0x040009CB RID: 2507
	private GameObject proiettileCarro;

	// Token: 0x040009CC RID: 2508
	private NavMeshAgent alleatoNav;

	// Token: 0x040009CD RID: 2509
	private float velocitàAlleatoNav;

	// Token: 0x040009CE RID: 2510
	public GameObject baseArma1e2;

	// Token: 0x040009CF RID: 2511
	public GameObject baseArma3;

	// Token: 0x040009D0 RID: 2512
	private GameObject torretta;

	// Token: 0x040009D1 RID: 2513
	private GameObject unitàBersaglio;

	// Token: 0x040009D2 RID: 2514
	private Vector3 centroUnitàBersaglio;

	// Token: 0x040009D3 RID: 2515
	private GameObject munizioneArma1;

	// Token: 0x040009D4 RID: 2516
	private GameObject munizioneArma2;

	// Token: 0x040009D5 RID: 2517
	private GameObject munizioneArma3;

	// Token: 0x040009D6 RID: 2518
	private AudioSource suonoTorretta;

	// Token: 0x040009D7 RID: 2519
	private AudioSource suonoInterno;

	// Token: 0x040009D8 RID: 2520
	private AudioSource suonoMotore;

	// Token: 0x040009D9 RID: 2521
	public AudioClip motoreFermo;

	// Token: 0x040009DA RID: 2522
	public AudioClip motoreViaggio;

	// Token: 0x040009DB RID: 2523
	private bool inMovimento;

	// Token: 0x040009DC RID: 2524
	private bool fermo;

	// Token: 0x040009DD RID: 2525
	public float volumeMotoreIniziale;

	// Token: 0x040009DE RID: 2526
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x040009DF RID: 2527
	private bool suonoRicaricaAttivo;

	// Token: 0x040009E0 RID: 2528
	private AudioSource suonoArma1;

	// Token: 0x040009E1 RID: 2529
	private AudioSource suonoRicarica1;

	// Token: 0x040009E2 RID: 2530
	private AudioSource suonoArma2;

	// Token: 0x040009E3 RID: 2531
	private AudioSource suonoRicarica2;

	// Token: 0x040009E4 RID: 2532
	private AudioSource suonoArma31;

	// Token: 0x040009E5 RID: 2533
	private AudioSource suonoArma32;

	// Token: 0x040009E6 RID: 2534
	private AudioSource suonoRicarica3;

	// Token: 0x040009E7 RID: 2535
	private List<AudioSource> ListaSuoniArma3;

	// Token: 0x040009E8 RID: 2536
	private bool suonoArma1Partito;

	// Token: 0x040009E9 RID: 2537
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x040009EA RID: 2538
	private int armaAttivaInFPS;

	// Token: 0x040009EB RID: 2539
	private ParticleSystem particelleArma1;

	// Token: 0x040009EC RID: 2540
	private ParticleSystem particelleArma2;

	// Token: 0x040009ED RID: 2541
	private ParticleSystem particelleArma3;

	// Token: 0x040009EE RID: 2542
	private List<ParticleSystem> ListaParticelleArma3;

	// Token: 0x040009EF RID: 2543
	private ParticleSystem particelleArma31;

	// Token: 0x040009F0 RID: 2544
	private ParticleSystem particelleArma32;

	// Token: 0x040009F1 RID: 2545
	private ParticleSystem particelleArma1Bis;

	// Token: 0x040009F2 RID: 2546
	private GameObject proiettileArma2;

	// Token: 0x040009F3 RID: 2547
	private GameObject proiettileArma3;

	// Token: 0x040009F4 RID: 2548
	private bool rotArma1Attiva;

	// Token: 0x040009F5 RID: 2549
	private bool rotArma2Attiva;

	// Token: 0x040009F6 RID: 2550
	private int cannone3Attivo;

	// Token: 0x040009F7 RID: 2551
	private List<GameObject> ListaBoccheArma3;

	// Token: 0x040009F8 RID: 2552
	public GameObject cockpit;

	// Token: 0x040009F9 RID: 2553
	private Animator mechAnim;

	// Token: 0x040009FA RID: 2554
	private int camminataHash = Animator.StringToHash("camminata-attivata");

	// Token: 0x040009FB RID: 2555
	public GameObject corpoArma1;

	// Token: 0x040009FC RID: 2556
	public GameObject corpoArma2;

	// Token: 0x040009FD RID: 2557
	private GameObject sparoArma1;

	// Token: 0x040009FE RID: 2558
	private bool zoomAttivo;

	// Token: 0x040009FF RID: 2559
	private float distFineOrdineMovimento;

	// Token: 0x04000A00 RID: 2560
	private bool calcoloDistJump;

	// Token: 0x04000A01 RID: 2561
	private bool calcoloJumpEffettuato;

	// Token: 0x04000A02 RID: 2562
	private float velocitàIniziale;

	// Token: 0x04000A03 RID: 2563
	private float timerAggRicerca;
}
