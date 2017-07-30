using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003C RID: 60
public class ATT_HeavyAntiAircraftVehicle : MonoBehaviour
{
	// Token: 0x060002F0 RID: 752 RVA: 0x00079A6C File Offset: 0x00077C6C
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
		this.cannone = base.GetComponent<MOV_HeavyAntiAircraftVehicle>().cannoni;
		this.ListaMunizioniAttiveUnità = new List<GameObject>();
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma1);
		this.suonoTorretta = base.transform.GetChild(1).transform.GetChild(1).GetComponent<AudioSource>();
		this.suonoInterno = base.transform.GetChild(1).GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.volumeMotoreIniziale = this.suonoMotore.volume;
		this.suonoMotore.clip = this.motoreFermo;
		this.suonoMotore.Play();
		this.tempoFraSparoERicarica1 = 1f;
		this.suonoCannone1 = this.bocca1.GetComponent<AudioSource>();
		this.suonoCannone2 = this.bocca2.GetComponent<AudioSource>();
		this.suonoCannone3 = this.bocca3.GetComponent<AudioSource>();
		this.suonoCannone4 = this.bocca4.GetComponent<AudioSource>();
		this.ListaBocche = new List<GameObject>();
		this.ListaBocche.Add(this.bocca1);
		this.ListaBocche.Add(this.bocca2);
		this.ListaBocche.Add(this.bocca3);
		this.ListaBocche.Add(this.bocca4);
		this.ListaCannoni = new List<GameObject>();
		this.ListaCannoni.Add(this.bocca1.transform.parent.gameObject);
		this.ListaCannoni.Add(this.bocca2.transform.parent.gameObject);
		this.ListaCannoni.Add(this.bocca3.transform.parent.gameObject);
		this.ListaCannoni.Add(this.bocca4.transform.parent.gameObject);
		this.ListaBoolCannoniSparati = new List<bool>();
		this.ListaBoolCannoniSparati.Add(this.cannone1Sparato);
		this.ListaBoolCannoniSparati.Add(this.cannone2Sparato);
		this.ListaBoolCannoniSparati.Add(this.cannone3Sparato);
		this.ListaBoolCannoniSparati.Add(this.cannone4Sparato);
		this.ListaBoolCannoniInFondo = new List<bool>();
		this.ListaBoolCannoniInFondo.Add(this.cannone1InFondo);
		this.ListaBoolCannoniInFondo.Add(this.cannone2InFondo);
		this.ListaBoolCannoniInFondo.Add(this.cannone3InFondo);
		this.ListaBoolCannoniInFondo.Add(this.cannone4InFondo);
		this.ListaSuoniCannoni = new List<AudioSource>();
		this.ListaSuoniCannoni.Add(this.suonoCannone1);
		this.ListaSuoniCannoni.Add(this.suonoCannone2);
		this.ListaSuoniCannoni.Add(this.suonoCannone3);
		this.ListaSuoniCannoni.Add(this.suonoCannone4);
		this.ListaParticelleCannoni = new List<ParticleSystem>();
		this.ListaParticelleCannoni.Add(this.bocca1.GetComponent<ParticleSystem>());
		this.ListaParticelleCannoni.Add(this.bocca2.GetComponent<ParticleSystem>());
		this.ListaParticelleCannoni.Add(this.bocca3.GetComponent<ParticleSystem>());
		this.ListaParticelleCannoni.Add(this.bocca4.GetComponent<ParticleSystem>());
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x00079E7C File Offset: 0x0007807C
	private void Update()
	{
		this.ListaMunizioniAttiveUnità[0] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[0][0];
		this.munizioneArma1 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0];
		this.timerFrequenzaArma1 += Time.deltaTime;
		this.timerDopoSparo1 += Time.deltaTime;
		this.CondizioniArma1();
		this.GestioneCannoni();
		this.unitàBersaglio = base.GetComponent<PresenzaAlleato>().unitàBersaglio;
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.GestioneSuoniIndipendenti();
			this.PreparazioneAttacco();
		}
		else
		{
			this.GestioneVisuali();
			if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 0)
			{
				this.AttaccoPrimaPersonaArma1();
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
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 48f;
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
			base.GetComponent<MOV_HeavyAntiAircraftVehicle>().torretta.transform.rotation = base.transform.rotation;
			base.GetComponent<MOV_HeavyAntiAircraftVehicle>().cannoni.transform.rotation = base.transform.rotation;
			this.suonoTorretta.Stop();
			base.GetComponent<MOV_HeavyAntiAircraftVehicle>().suonoTorrettaPartito = false;
			this.terzaCamera.GetComponent<Camera>().nearClipPlane = 0.3f;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
			this.zoomAttivo = false;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x0007A1D8 File Offset: 0x000783D8
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

	// Token: 0x060002F3 RID: 755 RVA: 0x0007A398 File Offset: 0x00078598
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
					base.GetComponent<MOV_HeavyAntiAircraftVehicle>().cannoni.GetComponent<AudioSource>().Play();
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

	// Token: 0x060002F4 RID: 756 RVA: 0x0007A600 File Offset: 0x00078800
	private void GestioneCannoni()
	{
		if (this.cannoneAttivo >= 4)
		{
			this.cannoneAttivo = 0;
		}
		for (int i = 0; i <= 3; i++)
		{
			if (this.ListaBoolCannoniSparati[i])
			{
				if (this.ListaCannoni[i].transform.localPosition.y < -8f && !this.ListaBoolCannoniInFondo[i])
				{
					this.ListaCannoni[i].transform.localPosition += base.transform.up * 10f * Time.deltaTime;
				}
				if (this.ListaCannoni[i].transform.localPosition.y >= -8f && !this.ListaBoolCannoniInFondo[i])
				{
					this.ListaBoolCannoniInFondo[i] = true;
				}
				if (this.ListaCannoni[i].transform.localPosition.y > -10f && this.ListaBoolCannoniInFondo[i])
				{
					this.ListaCannoni[i].transform.localPosition += -base.transform.up * 1f * Time.deltaTime;
				}
				if (this.ListaCannoni[i].transform.localPosition.y <= -10f && this.ListaBoolCannoniInFondo[i])
				{
					this.ListaBoolCannoniInFondo[i] = false;
					this.ListaBoolCannoniSparati[i] = false;
				}
			}
		}
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x0007A7D4 File Offset: 0x000789D4
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
				if (!this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
				{
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
				}
				else
				{
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 48f;
				}
				this.zoomAttivo = false;
			}
			else
			{
				this.terzaCamera.GetComponent<Camera>().fieldOfView = 30f;
				this.zoomAttivo = true;
			}
		}
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x0007A8B0 File Offset: 0x00078AB0
	private void CameraTPS()
	{
		this.timerPosizionamentoTPS += Time.deltaTime;
		if (this.timerPosizionamentoTPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.cannone.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraTPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localEulerAngles = new Vector3(this.rotazioneCameraTPS.x, 0f, this.cannone.transform.eulerAngles.z);
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
			this.terzaCamera.GetComponent<Camera>().nearClipPlane = 0.3f;
		}
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x0007A988 File Offset: 0x00078B88
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.cannone.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = this.cannone.transform.rotation;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 48f;
			this.terzaCamera.GetComponent<Camera>().nearClipPlane = 3f;
		}
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x0007AA44 File Offset: 0x00078C44
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
				if (this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
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
			else if (this.unitàBersaglio && this.alleatoNav.enabled && this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
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
		}
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x0007B660 File Offset: 0x00079860
	private void AttaccoIndipendente1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaValoriArma1[5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaValoriArma1[0])
		{
			this.timerFrequenzaArma1 = 0f;
			this.ListaSuoniCannoni[this.cannoneAttivo].Play();
			this.ListaParticelleCannoni[this.cannoneAttivo].Play();
			List<float> listaValoriArma;
			List<float> expr_DF = listaValoriArma = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int index;
			int expr_E2 = index = 5;
			float num = listaValoriArma[index];
			expr_DF[expr_E2] = num - 1f;
			List<float> listaValoriArma2;
			List<float> expr_103 = listaValoriArma2 = base.GetComponent<PresenzaAlleato>().ListaValoriArma1;
			int expr_106 = index = 6;
			num = listaValoriArma2[index];
			expr_103[expr_106] = num - 1f;
			this.SparoIndipendente1();
			this.timerDopoSparo1 = 0f;
			this.cannoneAttivo++;
		}
	}

	// Token: 0x060002FA RID: 762 RVA: 0x0007B7A8 File Offset: 0x000799A8
	private void SparoIndipendente1()
	{
		this.proiettileCarro = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.proiettileCarro.GetComponent<DatiProiettile>().locazioneTarget = this.centroUnitàBersaglio;
		this.ListaBoolCannoniSparati[this.cannoneAttivo] = true;
		this.proiettileCarro.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060002FB RID: 763 RVA: 0x0007B830 File Offset: 0x00079A30
	private void SelezioneArma()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 0;
		}
	}

	// Token: 0x060002FC RID: 764 RVA: 0x0007B84C File Offset: 0x00079A4C
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

	// Token: 0x060002FD RID: 765 RVA: 0x0007BAB8 File Offset: 0x00079CB8
	private void SparoArma1()
	{
		this.proiettileCarro = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.ListaBocche[this.cannoneAttivo].transform.position, this.ListaBocche[this.cannoneAttivo].transform.rotation) as GameObject);
		this.proiettileCarro.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.ListaBoolCannoniSparati[this.cannoneAttivo] = true;
		this.proiettileCarro.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x04000C64 RID: 3172
	public float angVertMax;

	// Token: 0x04000C65 RID: 3173
	public float angVertMin;

	// Token: 0x04000C66 RID: 3174
	private GameObject infoNeutreTattica;

	// Token: 0x04000C67 RID: 3175
	private GameObject terzaCamera;

	// Token: 0x04000C68 RID: 3176
	private GameObject primaCamera;

	// Token: 0x04000C69 RID: 3177
	public GameObject bocca1;

	// Token: 0x04000C6A RID: 3178
	public GameObject bocca2;

	// Token: 0x04000C6B RID: 3179
	public GameObject bocca3;

	// Token: 0x04000C6C RID: 3180
	public GameObject bocca4;

	// Token: 0x04000C6D RID: 3181
	private GameObject IANemico;

	// Token: 0x04000C6E RID: 3182
	private GameObject InfoAlleati;

	// Token: 0x04000C6F RID: 3183
	private float timerFrequenzaArma1;

	// Token: 0x04000C70 RID: 3184
	private float timerRicarica1;

	// Token: 0x04000C71 RID: 3185
	private bool ricaricaInCorso1;

	// Token: 0x04000C72 RID: 3186
	private float tempoFraSparoERicarica1;

	// Token: 0x04000C73 RID: 3187
	private float timerDopoSparo1;

	// Token: 0x04000C74 RID: 3188
	private int layerColpo;

	// Token: 0x04000C75 RID: 3189
	private int layerVisuale;

	// Token: 0x04000C76 RID: 3190
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000C77 RID: 3191
	public Vector3 rotazioneCameraTPS;

	// Token: 0x04000C78 RID: 3192
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000C79 RID: 3193
	private float timerPosizionamentoTPS;

	// Token: 0x04000C7A RID: 3194
	private float timerPosizionamentoFPS;

	// Token: 0x04000C7B RID: 3195
	private GameObject CanvasFPS;

	// Token: 0x04000C7C RID: 3196
	private GameObject mirinoElettr1;

	// Token: 0x04000C7D RID: 3197
	public Sprite mirinoTPS;

	// Token: 0x04000C7E RID: 3198
	public Sprite mirinoFPS;

	// Token: 0x04000C7F RID: 3199
	private RaycastHit targetSparo;

	// Token: 0x04000C80 RID: 3200
	private GameObject proiettileCarro;

	// Token: 0x04000C81 RID: 3201
	private NavMeshAgent alleatoNav;

	// Token: 0x04000C82 RID: 3202
	private float velocitàAlleatoNav;

	// Token: 0x04000C83 RID: 3203
	private GameObject cannone;

	// Token: 0x04000C84 RID: 3204
	private GameObject unitàBersaglio;

	// Token: 0x04000C85 RID: 3205
	private Vector3 centroUnitàBersaglio;

	// Token: 0x04000C86 RID: 3206
	private GameObject munizioneArma1;

	// Token: 0x04000C87 RID: 3207
	private AudioSource suonoTorretta;

	// Token: 0x04000C88 RID: 3208
	private AudioSource suonoInterno;

	// Token: 0x04000C89 RID: 3209
	private AudioSource suonoMotore;

	// Token: 0x04000C8A RID: 3210
	public AudioClip motoreFermo;

	// Token: 0x04000C8B RID: 3211
	public AudioClip motorePartenza;

	// Token: 0x04000C8C RID: 3212
	public AudioClip motoreViaggio;

	// Token: 0x04000C8D RID: 3213
	public AudioClip motoreStop;

	// Token: 0x04000C8E RID: 3214
	private float timerPartenza;

	// Token: 0x04000C8F RID: 3215
	private float timerStop;

	// Token: 0x04000C90 RID: 3216
	private bool primaPartenza;

	// Token: 0x04000C91 RID: 3217
	public float volumeMotoreIniziale;

	// Token: 0x04000C92 RID: 3218
	private bool inPartenza;

	// Token: 0x04000C93 RID: 3219
	private bool partenzaFinita;

	// Token: 0x04000C94 RID: 3220
	private bool inStop;

	// Token: 0x04000C95 RID: 3221
	public bool stopFinito;

	// Token: 0x04000C96 RID: 3222
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x04000C97 RID: 3223
	private List<AudioSource> ListaSuoniCannoni;

	// Token: 0x04000C98 RID: 3224
	private AudioSource suonoCannone1;

	// Token: 0x04000C99 RID: 3225
	private AudioSource suonoCannone2;

	// Token: 0x04000C9A RID: 3226
	private AudioSource suonoCannone3;

	// Token: 0x04000C9B RID: 3227
	private AudioSource suonoCannone4;

	// Token: 0x04000C9C RID: 3228
	private List<GameObject> ListaBocche;

	// Token: 0x04000C9D RID: 3229
	private int cannoneAttivo;

	// Token: 0x04000C9E RID: 3230
	private List<GameObject> ListaCannoni;

	// Token: 0x04000C9F RID: 3231
	private List<bool> ListaBoolCannoniSparati;

	// Token: 0x04000CA0 RID: 3232
	private bool cannone1Sparato;

	// Token: 0x04000CA1 RID: 3233
	private bool cannone2Sparato;

	// Token: 0x04000CA2 RID: 3234
	private bool cannone3Sparato;

	// Token: 0x04000CA3 RID: 3235
	private bool cannone4Sparato;

	// Token: 0x04000CA4 RID: 3236
	private List<bool> ListaBoolCannoniInFondo;

	// Token: 0x04000CA5 RID: 3237
	private bool cannone1InFondo;

	// Token: 0x04000CA6 RID: 3238
	private bool cannone2InFondo;

	// Token: 0x04000CA7 RID: 3239
	private bool cannone3InFondo;

	// Token: 0x04000CA8 RID: 3240
	private bool cannone4InFondo;

	// Token: 0x04000CA9 RID: 3241
	private List<ParticleSystem> ListaParticelleCannoni;

	// Token: 0x04000CAA RID: 3242
	private ParticleSystem particelleCannone1;

	// Token: 0x04000CAB RID: 3243
	private ParticleSystem particelleCannone2;

	// Token: 0x04000CAC RID: 3244
	private ParticleSystem particelleCannone3;

	// Token: 0x04000CAD RID: 3245
	private ParticleSystem particelleCannone4;

	// Token: 0x04000CAE RID: 3246
	private bool zoomAttivo;

	// Token: 0x04000CAF RID: 3247
	private float distFineOrdineMovimento;

	// Token: 0x04000CB0 RID: 3248
	private float timerAggRicerca;
}
