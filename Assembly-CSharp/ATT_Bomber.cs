using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000013 RID: 19
public class ATT_Bomber : MonoBehaviour
{
	// Token: 0x0600007B RID: 123 RVA: 0x00018040 File Offset: 0x00016240
	private void Start()
	{
		this.CanvasFPS = GameObject.FindGameObjectWithTag("CanvasFPS");
		this.sensoreRaggioMirino = this.CanvasFPS.transform.GetChild(1).transform.GetChild(2).gameObject;
		this.sensoreRaggioMirinoMobile = this.CanvasFPS.transform.GetChild(1).transform.GetChild(4).gameObject;
		this.mirinoFissoVelivoli = this.CanvasFPS.transform.GetChild(1).transform.GetChild(0).gameObject;
		this.mirinoMissiliFisso = this.CanvasFPS.transform.GetChild(1).transform.GetChild(1).gameObject;
		this.mirinoMissiliMobile = this.CanvasFPS.transform.GetChild(1).transform.GetChild(3).gameObject;
		this.mirinoMissiliFiloguidati = this.CanvasFPS.transform.GetChild(1).transform.GetChild(5).gameObject;
		this.mirinoBombe = this.CanvasFPS.transform.GetChild(1).transform.GetChild(6).gameObject;
		this.mirinoInfoVelivoli = this.CanvasFPS.transform.GetChild(1).transform.GetChild(7).gameObject;
		this.livelloSuolo = this.CanvasFPS.transform.GetChild(1).transform.GetChild(8).gameObject;
		this.angCameraBomb = this.CanvasFPS.transform.GetChild(1).transform.GetChild(9).gameObject;
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
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma4);
		this.cannone = this.bocca1.transform.parent.gameObject;
		this.ListaSparoConDestra = new List<bool>();
		this.ListaSparoConDestra.Add(this.sparoConDestra01);
		this.ListaSparoConDestra.Add(this.sparoConDestra23);
		this.ListaSparoConDestra.Add(this.sparoConDestra45);
		this.ListaGruppiOrdigniAttivi = new List<bool>();
		this.coloreBaseMirini = this.mirinoFissoVelivoli.GetComponent<Image>().color;
		this.suonoBocca1 = this.bocca1.GetComponent<AudioSource>();
		this.suonoBeep = base.transform.GetChild(0).GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.volumeBaseEsterno = this.suonoMotore.volume;
		this.suonoMotore.Play();
		this.particelleBocca1 = this.bocca1.GetComponent<ParticleSystem>();
		this.testoPotenzaMotore = this.mirinoInfoVelivoli.transform.GetChild(0).GetComponent<Text>();
		this.testoAltitudine = this.mirinoInfoVelivoli.transform.GetChild(1).GetComponent<Text>();
	}

	// Token: 0x0600007C RID: 124 RVA: 0x000183C4 File Offset: 0x000165C4
	private void Update()
	{
		this.CreazioneOrdigniConRifornimento();
		this.CondizioniArma1();
		this.CondizioniArma2();
		this.CondizioniArma3();
		this.CondizioniArma4();
		this.timerFrequenzaArma1 += Time.deltaTime;
		this.timerDiLancio += Time.deltaTime;
		if (!this.primoFrameAvvenuto)
		{
			this.CreazioneInizialeOrdigni();
			this.primoFrameAvvenuto = true;
		}
		this.unitàBersaglio = base.GetComponent<PresenzaAlleato>().unitàBersaglio;
		this.ListaMunizioniAttiveUnità[0] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[0][0];
		this.ListaMunizioniAttiveUnità[1] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[1][0];
		this.ListaMunizioniAttiveUnità[2] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[2][0];
		this.ListaMunizioniAttiveUnità[3] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[3][0];
		this.munizioneArma1 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0];
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.PreparazioneAttacco();
			if (this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][0])
			{
				this.cannoneStaSparando = false;
				this.suonoBocca1.Stop();
				this.particelleBocca1.Stop();
			}
			if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Tab))
			{
				this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera = null;
				this.timerPosizionamentoTPS = 0f;
				this.timerPosizionamentoFPS = 0f;
			}
			this.ControlloArmiPrimarie();
		}
		else
		{
			this.GestioneVisuali();
			this.SelezioneArma();
			this.GestioneOrdigniPrimaPersona();
			this.Mirini();
			this.StrumentazioneMirini();
			this.timerPresenzaInAereo += Time.deltaTime;
			if (this.timerPresenzaInAereo > 0f && this.timerPresenzaInAereo < 0.5f)
			{
				base.GetComponent<MOV_Bomber>().velocitàFrontaleEffettiva = 60f;
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.terzaCamera.transform.localPosition = this.posCameraPsteriore;
				this.terzaCamera.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
				base.gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
				base.gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
				this.cannoneStaSparando = false;
				this.suonoBocca1.Stop();
				this.particelleBocca1.Stop();
			}
			else if (Input.GetKeyUp(KeyCode.Space))
			{
				this.timerPosizionamentoTPS = 0f;
				this.timerPosizionamentoFPS = 0f;
				if (!this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
				{
					base.gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
					base.gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
				}
				else
				{
					base.gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
					base.gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
				}
			}
			if ((!Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyUp(KeyCode.Space)) && base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 0)
			{
				this.AttaccoPrimaPersonaArma1();
			}
			if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva == 3)
			{
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
					base.gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
					base.gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					base.gameObject.transform.GetChild(2).GetComponent<AudioSource>().Play();
					this.suonoMotore.volume = this.volumeBaseEsterno / 3f;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS && !this.visualeBombAttiva)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					base.gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
					base.gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
					base.gameObject.transform.GetChild(2).GetComponent<AudioSource>().Stop();
					this.suonoMotore.volume = this.volumeBaseEsterno;
				}
				if (Input.GetMouseButtonDown(1))
				{
					base.gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
					base.gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
					base.gameObject.transform.GetChild(2).GetComponent<AudioSource>().Stop();
					this.suonoMotore.volume = this.volumeBaseEsterno;
				}
				if (Input.GetMouseButtonUp(1))
				{
					if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
					{
						this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
						base.gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
						base.gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
						base.gameObject.transform.GetChild(2).GetComponent<AudioSource>().Play();
						this.suonoBeep.volume = this.volumeBaseEsterno / 3f;
					}
					if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
					{
						this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
						base.gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
						base.gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
						base.gameObject.transform.GetChild(2).GetComponent<AudioSource>().Stop();
						this.suonoBeep.volume = this.volumeBaseEsterno;
					}
				}
			}
		}
		if (this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona && this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0 && this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0] == base.gameObject)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona = false;
			base.gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
			base.gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			base.gameObject.transform.GetChild(2).GetComponent<AudioSource>().Stop();
			this.suonoMotore.volume = this.volumeBaseEsterno;
			this.suonoBocca1.Stop();
			this.cannoneStaSparando = false;
			this.bersaglioInPP = null;
			this.angCameraBomb.GetComponent<CanvasGroup>().alpha = 0f;
			this.visualeBombAttiva = false;
			this.timerPresenzaInAereo = 0f;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00018BC8 File Offset: 0x00016DC8
	private void CreazioneInizialeOrdigni()
	{
		for (int i = 0; i < base.GetComponent<PresenzaAlleato>().numeroCoppieOrdigni; i++)
		{
			this.ListaOrdigniAttiviLocale[i * 2] = base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[i];
			this.ListaOrdigniAttiviLocale[i * 2 + 1] = base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[i];
		}
		this.ordigno0 = (UnityEngine.Object.Instantiate(base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[0], base.transform.position, base.transform.rotation) as GameObject);
		this.ordigno0.transform.parent = base.transform;
		this.ordigno0.transform.localPosition = this.posizioneOrdigni01;
		this.ordigno1 = (UnityEngine.Object.Instantiate(base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[0], base.transform.position, base.transform.rotation) as GameObject);
		this.ordigno1.transform.parent = base.transform;
		this.ordigno1.transform.localPosition = new Vector3(-this.posizioneOrdigni01.x, this.posizioneOrdigni01.y, this.posizioneOrdigni01.z);
		this.ordigno2 = (UnityEngine.Object.Instantiate(base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[1], base.transform.position, base.transform.rotation) as GameObject);
		this.ordigno2.transform.parent = base.transform;
		this.ordigno2.transform.localPosition = this.posizioneOrdigni23;
		this.ordigno3 = (UnityEngine.Object.Instantiate(base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[1], base.transform.position, base.transform.rotation) as GameObject);
		this.ordigno3.transform.parent = base.transform;
		this.ordigno3.transform.localPosition = new Vector3(-this.posizioneOrdigni23.x, this.posizioneOrdigni23.y, this.posizioneOrdigni23.z);
		this.ordigno4 = (UnityEngine.Object.Instantiate(base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[2], base.transform.position, base.transform.rotation) as GameObject);
		this.ordigno4.transform.parent = base.transform;
		this.ordigno4.transform.localPosition = this.posizioneOrdigni45;
		this.ordigno5 = (UnityEngine.Object.Instantiate(base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[2], base.transform.position, base.transform.rotation) as GameObject);
		this.ordigno5.transform.parent = base.transform;
		this.ordigno5.transform.localPosition = new Vector3(-this.posizioneOrdigni45.x, this.posizioneOrdigni45.y, this.posizioneOrdigni45.z);
		this.ListaOrdigniLocali = new List<GameObject>();
		this.ListaOrdigniLocali.Add(this.ordigno0);
		this.ListaOrdigniLocali.Add(this.ordigno1);
		this.ListaOrdigniLocali.Add(this.ordigno2);
		this.ListaOrdigniLocali.Add(this.ordigno3);
		this.ListaOrdigniLocali.Add(this.ordigno4);
		this.ListaOrdigniLocali.Add(this.ordigno5);
		for (int j = 0; j < base.GetComponent<PresenzaAlleato>().numeroCoppieOrdigni; j++)
		{
			if (base.GetComponent<PresenzaAlleato>().ListaArmi[j + 1][5] % 2f == 0f)
			{
				if (j * 2 == 0 || j * 2 == 2 || j * 2 == 4)
				{
					int num = 0;
					while ((float)num < base.GetComponent<PresenzaAlleato>().ListaArmi[j + 1][5] / 2f)
					{
						this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[num] = (UnityEngine.Object.Instantiate(this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().munizioneUsata, base.transform.position, base.transform.rotation) as GameObject);
						this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[num].transform.parent = this.ListaOrdigniLocali[j * 2].transform;
						this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[num].transform.localPosition = this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().ListaPosizioniMunizioni[num];
						num++;
					}
				}
				if (j * 2 + 1 == 1 || j * 2 + 1 == 3 || j * 2 + 1 == 5)
				{
					int num2 = 0;
					while ((float)num2 < base.GetComponent<PresenzaAlleato>().ListaArmi[j + 1][5] / 2f)
					{
						this.ListaOrdigniLocali[j * 2 + 1].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[num2] = (UnityEngine.Object.Instantiate(this.ListaOrdigniLocali[j * 2 + 1].GetComponent<DatiOrdignoEsterno>().munizioneUsata, base.transform.position, base.transform.rotation) as GameObject);
						this.ListaOrdigniLocali[j * 2 + 1].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[num2].transform.parent = this.ListaOrdigniLocali[j * 2 + 1].transform;
						this.ListaOrdigniLocali[j * 2 + 1].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[num2].transform.localPosition = this.ListaOrdigniLocali[j * 2 + 1].GetComponent<DatiOrdignoEsterno>().ListaPosizioniMunizioni[num2];
						num2++;
					}
				}
			}
			else
			{
				int num3 = Mathf.RoundToInt(base.GetComponent<PresenzaAlleato>().ListaArmi[j + 1][5] / 2f);
				if (j * 2 == 0 || j * 2 == 2 || j * 2 == 4)
				{
					for (int k = 0; k < num3 + 1; k++)
					{
						this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k] = (UnityEngine.Object.Instantiate(this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().munizioneUsata, base.transform.position, base.transform.rotation) as GameObject);
						this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k].transform.parent = this.ListaOrdigniLocali[j * 2].transform;
						this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k].transform.localPosition = this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().ListaPosizioniMunizioni[k];
					}
				}
				if (j * 2 + 1 == 1 || j * 2 + 1 == 3 || j * 2 + 1 == 5)
				{
					for (int l = 0; l < num3; l++)
					{
						this.ListaOrdigniLocali[j * 2 + 1].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[l] = (UnityEngine.Object.Instantiate(this.ListaOrdigniLocali[j * 2 + 1].GetComponent<DatiOrdignoEsterno>().munizioneUsata, base.transform.position, base.transform.rotation) as GameObject);
						this.ListaOrdigniLocali[j * 2 + 1].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[l].transform.parent = this.ListaOrdigniLocali[j * 2 + 1].transform;
						this.ListaOrdigniLocali[j * 2 + 1].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[l].transform.localPosition = this.ListaOrdigniLocali[j * 2 + 1].GetComponent<DatiOrdignoEsterno>().ListaPosizioniMunizioni[l];
					}
				}
			}
		}
	}

	// Token: 0x0600007E RID: 126 RVA: 0x0001944C File Offset: 0x0001764C
	private void CreazioneOrdigniConRifornimento()
	{
		if (base.GetComponent<PresenzaAlleato>().reintegrazioneNecessaria)
		{
			for (int i = 0; i < base.GetComponent<PresenzaAlleato>().numeroCoppieOrdigni; i++)
			{
				for (int j = 0; j < base.GetComponent<PresenzaAlleato>().ListaNumReintegrazioniOrdigni[i + 1]; j++)
				{
					bool flag = false;
					for (int k = 0; k < this.ListaOrdigniLocali[i * 2].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche.Count; k++)
					{
						if (this.ListaOrdigniLocali[i * 2].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k] == null)
						{
							this.ListaOrdigniLocali[i * 2].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k] = (UnityEngine.Object.Instantiate(this.ListaOrdigniLocali[i * 2].GetComponent<DatiOrdignoEsterno>().munizioneUsata, base.transform.position, base.transform.rotation) as GameObject);
							this.ListaOrdigniLocali[i * 2].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k].transform.parent = this.ListaOrdigniLocali[i * 2].transform;
							this.ListaOrdigniLocali[i * 2].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k].transform.localPosition = this.ListaOrdigniLocali[i * 2].GetComponent<DatiOrdignoEsterno>().ListaPosizioniMunizioni[k];
							break;
						}
						flag = true;
					}
					if (flag)
					{
						for (int l = 0; l < this.ListaOrdigniLocali[i * 2 + 1].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche.Count; l++)
						{
							if (this.ListaOrdigniLocali[i * 2 + 1].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[l] == null)
							{
								this.ListaOrdigniLocali[i * 2 + 1].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[l] = (UnityEngine.Object.Instantiate(this.ListaOrdigniLocali[i * 2 + 1].GetComponent<DatiOrdignoEsterno>().munizioneUsata, base.transform.position, base.transform.rotation) as GameObject);
								this.ListaOrdigniLocali[i * 2 + 1].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[l].transform.parent = this.ListaOrdigniLocali[i * 2 + 1].transform;
								this.ListaOrdigniLocali[i * 2 + 1].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[l].transform.localPosition = this.ListaOrdigniLocali[i * 2 + 1].GetComponent<DatiOrdignoEsterno>().ListaPosizioniMunizioni[l];
								break;
							}
						}
					}
				}
			}
			base.GetComponent<PresenzaAlleato>().reintegrazioneNecessaria = false;
		}
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00019740 File Offset: 0x00017940
	private void CondizioniArma1()
	{
		base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[0][6];
	}

	// Token: 0x06000080 RID: 128 RVA: 0x0001977C File Offset: 0x0001797C
	private void CondizioniArma2()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[1][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[1][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[1][6];
		}
	}

	// Token: 0x06000081 RID: 129 RVA: 0x000197EC File Offset: 0x000179EC
	private void CondizioniArma3()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[2][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[2][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[2][6];
		}
	}

	// Token: 0x06000082 RID: 130 RVA: 0x0001985C File Offset: 0x00017A5C
	private void CondizioniArma4()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[3][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[3][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[3][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[3][6];
		}
	}

	// Token: 0x06000083 RID: 131 RVA: 0x000198CC File Offset: 0x00017ACC
	private void GestioneVisuali()
	{
		if (!this.terzaCamera.GetComponent<TerzaCamera>().èFPS && !this.visualeBombAttiva)
		{
			this.CameraTPS();
			this.timerPosizionamentoFPS = 0f;
			this.timerPosizionamentoVisuBomb = 0f;
		}
		if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS && !this.visualeBombAttiva)
		{
			this.CameraFPS();
			this.timerPosizionamentoTPS = 0f;
			this.timerPosizionamentoVisuBomb = 0f;
		}
		if (Input.GetMouseButtonDown(1))
		{
			this.VisualeBombardamento();
			this.visualeBombAttiva = true;
			this.timerPosizionamentoTPS = 0f;
			this.timerPosizionamentoFPS = 0f;
		}
		if (Input.GetMouseButtonUp(1))
		{
			if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.CameraFPS();
				this.timerPosizionamentoTPS = 0f;
				this.timerPosizionamentoVisuBomb = 0f;
				this.visualeBombAttiva = false;
			}
			else if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
			{
				this.CameraTPS();
				this.timerPosizionamentoFPS = 0f;
				this.timerPosizionamentoVisuBomb = 0f;
				this.visualeBombAttiva = false;
			}
		}
		if (this.visualeBombAttiva)
		{
			float num = 0f;
			if (Vector3.Angle(base.transform.up, this.terzaCamera.transform.forward) > 90f && Input.GetAxis("Mouse Y") > 0f)
			{
				num = Input.GetAxis("Mouse Y") * 50f * Time.deltaTime;
			}
			if (Vector3.Dot(base.transform.up, this.terzaCamera.transform.up) > -0.7f && Input.GetAxis("Mouse Y") < 0f)
			{
				num = Input.GetAxis("Mouse Y") * 50f * Time.deltaTime;
			}
			this.terzaCamera.transform.Rotate(-num, 0f, 0f);
			base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, 0f);
			this.angCameraBomb.GetComponent<CanvasGroup>().alpha = 1f;
		}
		else
		{
			this.angCameraBomb.GetComponent<CanvasGroup>().alpha = 0f;
		}
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00019B34 File Offset: 0x00017D34
	private void CameraTPS()
	{
		this.timerPosizionamentoTPS += Time.deltaTime;
		if (this.timerPosizionamentoTPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = base.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraTPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = base.transform.rotation;
		}
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00019BBC File Offset: 0x00017DBC
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = base.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = base.transform.rotation;
		}
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00019C44 File Offset: 0x00017E44
	private void VisualeBombardamento()
	{
		this.timerPosizionamentoVisuBomb += Time.deltaTime;
		if (this.timerPosizionamentoVisuBomb < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = base.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraBombardamento;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = base.transform.rotation;
		}
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00019CCC File Offset: 0x00017ECC
	private void Mirini()
	{
		this.mirinoFissoVelivoli.GetComponent<CanvasGroup>().alpha = 1f;
		this.mirinoInfoVelivoli.GetComponent<CanvasGroup>().alpha = 1f;
		this.livelloSuolo.GetComponent<CanvasGroup>().alpha = 1f;
		this.mirinoBombe.GetComponent<CanvasGroup>().alpha = 1f;
		this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 0f;
		this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 0f;
		this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 0f;
		if (this.mitragliatoreAttivo)
		{
			this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 0f;
		}
		if (this.ordignoDaLanciare && this.ordignoDaLanciare.transform.parent && (!Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyUp(KeyCode.Space)))
		{
			this.SistemaDiLancioInPrimaPersona();
		}
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00019E0C File Offset: 0x0001800C
	private void StrumentazioneMirini()
	{
		float frontaleVelocitàMax = base.GetComponent<MOV_Bomber>().frontaleVelocitàMax;
		float num = base.GetComponent<MOV_Bomber>().velocitàFrontaleEffettiva / base.GetComponent<MOV_Bomber>().frontaleVelocitàMax * 100f;
		this.testoPotenzaMotore.text = num.ToString("F0") + "%";
		this.testoAltitudine.text = base.transform.position.y.ToString("F0");
		this.livelloSuolo.transform.eulerAngles = new Vector3(0f, 0f, -base.transform.eulerAngles.z);
		this.mirinoBombe.transform.eulerAngles = new Vector3(0f, 0f, -base.transform.eulerAngles.z);
		if (this.visualeBombAttiva)
		{
			float num2 = Vector3.Angle(base.transform.forward, this.terzaCamera.transform.forward);
			this.angCameraBomb.GetComponent<Text>().text = "  " + num2.ToString("F0") + "°";
		}
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00019F4C File Offset: 0x0001814C
	private void PreparazioneAttacco()
	{
		bool flag = false;
		if (this.unitàBersaglio)
		{
			flag = Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale);
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
		Vector3 b = new Vector3(base.transform.position.x, this.centroUnitàBersaglio.y, base.transform.position.z);
		float num2 = Vector3.Distance(this.centroUnitàBersaglio, b);
		if (!base.GetComponent<PresenzaAlleato>().destinazioneOrdinata)
		{
			if (base.GetComponent<PresenzaAlleato>().attaccoOrdinato)
			{
				base.GetComponent<MOV_AUTOM_Bomber>().ripetitoreDiAttaccoOrdinato = true;
				base.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
				this.unitàBersaglio = this.primaCamera.GetComponent<Selezionamento>().oggettoBersaglio;
				if (this.unitàBersaglio && !flag && !this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
				{
					float num3 = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
					if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && this.bersèNelMirino && num3 < this.ListaMunizioniAttiveUnità[0].GetComponent<DatiGeneraliMunizione>().portataMassima)
					{
						this.AttaccoIndipendente1();
					}
					for (int i = 0; i < base.GetComponent<PresenzaAlleato>().numeroArmi; i++)
					{
						if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[i] && num2 < this.distanzaDiSgancio && i >= 1 && !this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
						{
							if (this.ListaOrdigniLocali[(i - 1) * 2].transform.childCount > 1)
							{
								if (this.ListaOrdigniLocali[(i - 1) * 2].transform.GetChild(1) != null)
								{
									this.ordignoDaLanciare = this.ListaOrdigniLocali[(i - 1) * 2].transform.GetChild(1).gameObject;
									this.numArmaOrdignoDaLanciare = i;
									this.ListaSparoConDestra[i - 1] = true;
									this.AttaccoIndipendenteOrdigni();
									break;
								}
							}
							else if (this.ListaOrdigniLocali[(i - 1) * 2 + 1].transform.childCount > 1 && this.ListaOrdigniLocali[(i - 1) * 2 + 1].transform.GetChild(1) != null)
							{
								this.ordignoDaLanciare = this.ListaOrdigniLocali[(i - 1) * 2 + 1].transform.GetChild(1).gameObject;
								this.numArmaOrdignoDaLanciare = i;
								this.ListaSparoConDestra[i - 1] = true;
								this.AttaccoIndipendenteOrdigni();
								break;
							}
						}
					}
					if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita <= 0f)
					{
						base.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
					}
				}
			}
			else if (this.unitàBersaglio && !flag && !this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
			{
				float num4 = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
				if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && this.bersèNelMirino && num4 < this.ListaMunizioniAttiveUnità[0].GetComponent<DatiGeneraliMunizione>().portataMassima)
				{
					this.AttaccoIndipendente1();
				}
				for (int j = 0; j < base.GetComponent<PresenzaAlleato>().numeroArmi; j++)
				{
					if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[j] && num2 < this.distanzaDiSgancio && j >= 1 && !this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
					{
						if (this.ListaOrdigniLocali[(j - 1) * 2].transform.childCount > 1)
						{
							if (this.ListaOrdigniLocali[(j - 1) * 2].transform.GetChild(1) != null)
							{
								this.ordignoDaLanciare = this.ListaOrdigniLocali[(j - 1) * 2].transform.GetChild(1).gameObject;
								this.numArmaOrdignoDaLanciare = j;
								this.ListaSparoConDestra[j - 1] = true;
								this.AttaccoIndipendenteOrdigni();
								break;
							}
						}
						else if (this.ListaOrdigniLocali[(j - 1) * 2 + 1].transform.childCount > 1 && this.ListaOrdigniLocali[(j - 1) * 2 + 1].transform.GetChild(1) != null)
						{
							this.ordignoDaLanciare = this.ListaOrdigniLocali[(j - 1) * 2 + 1].transform.GetChild(1).gameObject;
							this.numArmaOrdignoDaLanciare = j;
							this.ListaSparoConDestra[j - 1] = true;
							this.AttaccoIndipendenteOrdigni();
							break;
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
			else if (base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers)
			{
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
								float num5 = Vector3.Distance(base.transform.position, current.GetComponent<PresenzaNemico>().centroInsetto);
								if (num5 < 500f && !Physics.Linecast(this.bocca1.transform.position, current.GetComponent<PresenzaNemico>().centroInsetto, this.layerVisuale) && current.transform.position.y < base.transform.position.y - 20f)
								{
									list.Add(current);
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
								float num6 = Vector3.Distance(base.transform.position, current2.GetComponent<PresenzaNemico>().centroInsetto);
								if (num6 < 500f && !Physics.Linecast(this.bocca1.transform.position, current2.GetComponent<PresenzaNemico>().centroInsetto, this.layerVisuale))
								{
									float num7 = Vector3.Dot((current2.GetComponent<PresenzaNemico>().centroInsetto - base.transform.position).normalized, base.transform.up);
									if (current2.transform.position.y < base.transform.position.y - 20f)
									{
										list2.Add(current2);
									}
								}
							}
						}
						if (list2.Count > 0)
						{
							float num8 = 9999f;
							for (int k = 0; k < list2.Count; k++)
							{
								float num9 = Vector3.Distance(base.transform.position, list2[k].GetComponent<PresenzaNemico>().centroInsetto);
								if (num9 < num8)
								{
									num8 = num9;
									this.unitàBersaglio = list2[k];
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
				float f2 = UnityEngine.Random.Range(0f, (float)this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti.Count - 0.01f);
				this.unitàBersaglio = this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciNonVolanti[Mathf.FloorToInt(f2)];
			}
		}
		else
		{
			this.unitàBersaglio = null;
		}
		if (base.GetComponent<PresenzaAlleato>().attaccoZonaOrdinato)
		{
			Vector3 b2 = new Vector3(base.transform.position.x, base.GetComponent<PresenzaAlleato>().luogoAttZonaBomb.y, base.transform.position.z);
			float num10 = Vector3.Distance(base.GetComponent<PresenzaAlleato>().luogoAttZonaBomb, b2);
			for (int l = 0; l < base.GetComponent<PresenzaAlleato>().numeroArmi; l++)
			{
				if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[l] && num10 < this.distanzaDiSgancio && l >= 1)
				{
					if (this.ListaOrdigniLocali[(l - 1) * 2].transform.childCount > 1)
					{
						if (this.ListaOrdigniLocali[(l - 1) * 2].transform.GetChild(1) != null)
						{
							this.ordignoDaLanciare = this.ListaOrdigniLocali[(l - 1) * 2].transform.GetChild(1).gameObject;
							this.numArmaOrdignoDaLanciare = l;
							this.ListaSparoConDestra[l - 1] = true;
							this.AttaccoIndipendenteOrdigni();
							break;
						}
					}
					else if (this.ListaOrdigniLocali[(l - 1) * 2 + 1].transform.childCount > 1 && this.ListaOrdigniLocali[(l - 1) * 2 + 1].transform.GetChild(1) != null)
					{
						this.ordignoDaLanciare = this.ListaOrdigniLocali[(l - 1) * 2 + 1].transform.GetChild(1).gameObject;
						this.numArmaOrdignoDaLanciare = l;
						this.ListaSparoConDestra[l - 1] = true;
						this.AttaccoIndipendenteOrdigni();
						break;
					}
				}
			}
		}
	}

	// Token: 0x0600008A RID: 138 RVA: 0x0001ABB8 File Offset: 0x00018DB8
	private void AttaccoIndipendente1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][0])
		{
			this.timerFrequenzaArma1 = 0f;
			this.mm20Proiettile = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
			this.mm20Proiettile.GetComponent<mm20Proiettile>().target = this.unitàBersaglio;
			this.mm20Proiettile.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
			List<float> list;
			List<float> expr_106 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int index;
			int expr_109 = index = 5;
			float num = list[index];
			expr_106[expr_109] = num - 1f;
			List<float> list2;
			List<float> expr_130 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int expr_133 = index = 6;
			num = list2[index];
			expr_130[expr_133] = num - 1f;
			if (!this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.particelleBocca1.Play();
			}
			if (!this.cannoneStaSparando)
			{
				this.suonoBocca1.Play();
				this.cannoneStaSparando = true;
			}
		}
	}

	// Token: 0x0600008B RID: 139 RVA: 0x0001AD4C File Offset: 0x00018F4C
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

	// Token: 0x0600008C RID: 140 RVA: 0x0001AEA4 File Offset: 0x000190A4
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
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 2;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 3;
		}
	}

	// Token: 0x0600008D RID: 141 RVA: 0x0001AF14 File Offset: 0x00019114
	private void GestioneOrdigniPrimaPersona()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.mitragliatoreAttivo = true;
			this.gruppo01Attivo = false;
			this.gruppo23Attivo = false;
			this.gruppo45Attivo = false;
			this.ordignoDaLanciare = null;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.mitragliatoreAttivo = false;
			this.gruppo01Attivo = true;
			this.gruppo23Attivo = false;
			this.gruppo45Attivo = false;
			this.ordignoDaLanciare = null;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			this.mitragliatoreAttivo = false;
			this.gruppo01Attivo = false;
			this.gruppo23Attivo = true;
			this.gruppo45Attivo = false;
			this.ordignoDaLanciare = null;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			this.mitragliatoreAttivo = false;
			this.gruppo01Attivo = false;
			this.gruppo23Attivo = false;
			this.gruppo45Attivo = true;
			this.ordignoDaLanciare = null;
		}
		this.ListaGruppiOrdigniAttivi.Clear();
		this.ListaGruppiOrdigniAttivi.Add(this.gruppo01Attivo);
		this.ListaGruppiOrdigniAttivi.Add(this.gruppo23Attivo);
		this.ListaGruppiOrdigniAttivi.Add(this.gruppo45Attivo);
		int num = 0;
		if (this.ordignoDaLanciare == null)
		{
			for (int i = 0; i < this.ListaGruppiOrdigniAttivi.Count; i++)
			{
				if (this.ListaGruppiOrdigniAttivi[i])
				{
					if (!this.ListaSparoConDestra[i])
					{
						if (this.ListaOrdigniLocali[i * 2].transform.childCount > 1 && this.ListaOrdigniLocali[i * 2].transform.GetChild(1) != null)
						{
							this.ordignoDaLanciare = this.ListaOrdigniLocali[i * 2].transform.GetChild(1).gameObject;
							this.numArmaOrdignoDaLanciare = i + 1;
							this.ListaSparoConDestra[i] = true;
							break;
						}
					}
					else if (this.ListaOrdigniLocali[i * 2 + 1].transform.childCount > 1 && this.ListaOrdigniLocali[i * 2 + 1].transform.GetChild(1) != null)
					{
						this.ordignoDaLanciare = this.ListaOrdigniLocali[i * 2 + 1].transform.GetChild(1).gameObject;
						this.numArmaOrdignoDaLanciare = i + 1;
						this.ListaSparoConDestra[i] = false;
						break;
					}
					if (this.ListaOrdigniLocali[i * 2].transform.childCount > 1)
					{
						this.ordignoDaLanciare = this.ListaOrdigniLocali[i * 2].transform.GetChild(1).gameObject;
						this.numArmaOrdignoDaLanciare = i + 1;
					}
					else if (this.ListaOrdigniLocali[i * 2 + 1].transform.childCount > 1)
					{
						this.ordignoDaLanciare = this.ListaOrdigniLocali[i * 2 + 1].transform.GetChild(1).gameObject;
						this.numArmaOrdignoDaLanciare = i + 1;
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

	// Token: 0x0600008E RID: 142 RVA: 0x0001B23C File Offset: 0x0001943C
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
		if (Input.GetMouseButton(0) && !this.visualeBombAttiva && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][1])
		{
			this.timerFrequenzaArma1 = 0f;
			this.SparoArma1();
			List<float> list;
			List<float> expr_1C9 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int index;
			int expr_1CC = index = 5;
			float num = list[index];
			expr_1C9[expr_1CC] = num - 1f;
			List<float> list2;
			List<float> expr_1F3 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int expr_1F7 = index = 6;
			num = list2[index];
			expr_1F3[expr_1F7] = num - 1f;
			if (!this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.particelleBocca1.Play();
			}
			if (!this.cannoneStaSparando)
			{
				this.suonoBocca1.Play();
				this.cannoneStaSparando = true;
			}
		}
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] <= 0f || Input.GetMouseButtonUp(0))
		{
			this.suonoBocca1.Stop();
			this.particelleBocca1.Stop();
			this.cannoneStaSparando = false;
		}
	}

	// Token: 0x0600008F RID: 143 RVA: 0x0001B4E0 File Offset: 0x000196E0
	private void SparoArma1()
	{
		this.mm20Proiettile = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.mm20Proiettile.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.mm20Proiettile.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x06000090 RID: 144 RVA: 0x0001B550 File Offset: 0x00019750
	private void SistemaDiLancioInPrimaPersona()
	{
		this.ListaBersPPPossibili.Clear();
		float num = (float)(Screen.width / 13);
		float num2 = Vector3.Dot(base.transform.up, Vector3.up);
		if (Input.GetMouseButton(0) && num2 > 0.2f && base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][5] > 0f && this.timerDiLancio > base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][1])
		{
			this.timerDiLancio = 0f;
			List<float> list;
			List<float> expr_AF = list = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
			int index;
			int expr_B2 = index = 5;
			float num3 = list[index];
			expr_AF[expr_B2] = num3 - 1f;
			List<float> list2;
			List<float> expr_E2 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
			int expr_E6 = index = 6;
			num3 = list2[index];
			expr_E2[expr_E6] = num3 - 1f;
			for (int i = 0; i < this.ListaOrdigniLocali.Count; i++)
			{
				this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ordignoAttivo = false;
			}
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().ordignoAttivo = true;
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = true;
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().velocitàDelVelivAlLancio = base.GetComponent<MOV_Bomber>().velocitàFrontaleEffettiva;
			this.ordignoDaLanciare = null;
		}
	}

	// Token: 0x06000091 RID: 145 RVA: 0x0001B6F4 File Offset: 0x000198F4
	private void ControlloArmiPrimarie()
	{
		bool flag = true;
		for (int i = 1; i < 4; i++)
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

	// Token: 0x040002F7 RID: 759
	public float distanzaDiSgancio;

	// Token: 0x040002F8 RID: 760
	private GameObject infoNeutreTattica;

	// Token: 0x040002F9 RID: 761
	private GameObject primaCamera;

	// Token: 0x040002FA RID: 762
	public GameObject bocca1;

	// Token: 0x040002FB RID: 763
	private GameObject terzaCamera;

	// Token: 0x040002FC RID: 764
	private GameObject IANemico;

	// Token: 0x040002FD RID: 765
	private GameObject InfoAlleati;

	// Token: 0x040002FE RID: 766
	private GameObject mm20Proiettile;

	// Token: 0x040002FF RID: 767
	private float timerFrequenzaArma1;

	// Token: 0x04000300 RID: 768
	private float timerRicarica1;

	// Token: 0x04000301 RID: 769
	private bool ricaricaInCorso1;

	// Token: 0x04000302 RID: 770
	private float timerDiLancio;

	// Token: 0x04000303 RID: 771
	private int layerColpo;

	// Token: 0x04000304 RID: 772
	private int layerVisuale;

	// Token: 0x04000305 RID: 773
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000306 RID: 774
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000307 RID: 775
	public Vector3 posizionamentoCameraBombardamento;

	// Token: 0x04000308 RID: 776
	private float timerPosizionamentoTPS;

	// Token: 0x04000309 RID: 777
	private float timerPosizionamentoFPS;

	// Token: 0x0400030A RID: 778
	private float timerPosizionamentoVisuBomb;

	// Token: 0x0400030B RID: 779
	public bool visualeBombAttiva;

	// Token: 0x0400030C RID: 780
	public Vector3 posCameraPsteriore;

	// Token: 0x0400030D RID: 781
	private GameObject CanvasFPS;

	// Token: 0x0400030E RID: 782
	private GameObject sensoreRaggioMirino;

	// Token: 0x0400030F RID: 783
	private GameObject sensoreRaggioMirinoMobile;

	// Token: 0x04000310 RID: 784
	private GameObject mirinoFissoVelivoli;

	// Token: 0x04000311 RID: 785
	private GameObject mirinoMissiliFisso;

	// Token: 0x04000312 RID: 786
	private GameObject mirinoMissiliMobile;

	// Token: 0x04000313 RID: 787
	private Color coloreBaseMirini;

	// Token: 0x04000314 RID: 788
	private GameObject mirinoMissiliFiloguidati;

	// Token: 0x04000315 RID: 789
	private GameObject mirinoBombe;

	// Token: 0x04000316 RID: 790
	private GameObject mirinoInfoVelivoli;

	// Token: 0x04000317 RID: 791
	private GameObject livelloSuolo;

	// Token: 0x04000318 RID: 792
	private GameObject angCameraBomb;

	// Token: 0x04000319 RID: 793
	private RaycastHit targetSparo;

	// Token: 0x0400031A RID: 794
	private float velocitàAlleatoNav;

	// Token: 0x0400031B RID: 795
	private GameObject cannone;

	// Token: 0x0400031C RID: 796
	public GameObject unitàBersaglio;

	// Token: 0x0400031D RID: 797
	private Vector3 centroUnitàBersaglio;

	// Token: 0x0400031E RID: 798
	private GameObject munizioneArma1;

	// Token: 0x0400031F RID: 799
	private GameObject munizioneArma2;

	// Token: 0x04000320 RID: 800
	private GameObject munizioneArma3;

	// Token: 0x04000321 RID: 801
	private GameObject munizioneArma4;

	// Token: 0x04000322 RID: 802
	public AudioClip cannoneDurante;

	// Token: 0x04000323 RID: 803
	public AudioClip cannoneFine;

	// Token: 0x04000324 RID: 804
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x04000325 RID: 805
	private List<GameObject> ListaOrdigniLocali;

	// Token: 0x04000326 RID: 806
	public GameObject ordignoDaLanciare;

	// Token: 0x04000327 RID: 807
	private int numArmaOrdignoDaLanciare;

	// Token: 0x04000328 RID: 808
	private GameObject ordigno0;

	// Token: 0x04000329 RID: 809
	private GameObject ordigno1;

	// Token: 0x0400032A RID: 810
	private GameObject ordigno2;

	// Token: 0x0400032B RID: 811
	private GameObject ordigno3;

	// Token: 0x0400032C RID: 812
	private GameObject ordigno4;

	// Token: 0x0400032D RID: 813
	private GameObject ordigno5;

	// Token: 0x0400032E RID: 814
	public Vector3 posizioneOrdigni01;

	// Token: 0x0400032F RID: 815
	public Vector3 posizioneOrdigni23;

	// Token: 0x04000330 RID: 816
	public Vector3 posizioneOrdigni45;

	// Token: 0x04000331 RID: 817
	private bool mitragliatoreAttivo;

	// Token: 0x04000332 RID: 818
	private bool gruppo01Attivo;

	// Token: 0x04000333 RID: 819
	private bool gruppo23Attivo;

	// Token: 0x04000334 RID: 820
	private bool gruppo45Attivo;

	// Token: 0x04000335 RID: 821
	public List<GameObject> ListaOrdigniAttiviLocale;

	// Token: 0x04000336 RID: 822
	private List<bool> ListaGruppiOrdigniAttivi;

	// Token: 0x04000337 RID: 823
	private bool sparoConDestra01;

	// Token: 0x04000338 RID: 824
	private bool sparoConDestra23;

	// Token: 0x04000339 RID: 825
	private bool sparoConDestra45;

	// Token: 0x0400033A RID: 826
	private List<bool> ListaSparoConDestra;

	// Token: 0x0400033B RID: 827
	private bool primoFrameAvvenuto;

	// Token: 0x0400033C RID: 828
	public List<GameObject> ListaBersPPPossibili;

	// Token: 0x0400033D RID: 829
	public GameObject bersaglioInPP;

	// Token: 0x0400033E RID: 830
	private bool bersaglioèAvantiInPP;

	// Token: 0x0400033F RID: 831
	private bool bersDavantiEAPortata;

	// Token: 0x04000340 RID: 832
	private float volumeBaseEsterno;

	// Token: 0x04000341 RID: 833
	private GameObject ordignoFittizio;

	// Token: 0x04000342 RID: 834
	private bool bersèDavanti;

	// Token: 0x04000343 RID: 835
	private bool bersèNelMirino;

	// Token: 0x04000344 RID: 836
	private bool cannoneStaSparando;

	// Token: 0x04000345 RID: 837
	private AudioSource suonoMotore;

	// Token: 0x04000346 RID: 838
	private AudioSource suonoBocca1;

	// Token: 0x04000347 RID: 839
	private AudioSource suonoBeep;

	// Token: 0x04000348 RID: 840
	private ParticleSystem particelleBocca1;

	// Token: 0x04000349 RID: 841
	private Text testoPotenzaMotore;

	// Token: 0x0400034A RID: 842
	private Text testoAltitudine;

	// Token: 0x0400034B RID: 843
	private float timerPresenzaInAereo;

	// Token: 0x0400034C RID: 844
	private float timerAggRicerca;

	// Token: 0x0400034D RID: 845
	private float timerSenzaArmiPrimarie;
}
