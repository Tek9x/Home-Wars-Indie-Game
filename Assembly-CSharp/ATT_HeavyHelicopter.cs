using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000023 RID: 35
public class ATT_HeavyHelicopter : MonoBehaviour
{
	// Token: 0x0600018C RID: 396 RVA: 0x0004347C File Offset: 0x0004167C
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
		this.cannone = this.bocca1.transform.parent.gameObject;
		this.ListaSparoConDestra = new List<bool>();
		this.ListaSparoConDestra.Add(this.sparoConDestra01);
		this.ListaSparoConDestra.Add(this.sparoConDestra23);
		this.ListaGruppiOrdigniAttivi = new List<bool>();
		this.coloreBaseMirini = this.mirinoFissoVelivoli.GetComponent<Image>().color;
		this.suonoBocca1 = this.bocca1.GetComponent<AudioSource>();
		this.suonoBeep = base.transform.GetChild(0).GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.suonoInterno = base.transform.GetChild(2).GetComponent<AudioSource>();
		this.volumeBaseEsterno = this.suonoMotore.volume;
		this.particelleBocca1 = this.bocca1.GetComponent<ParticleSystem>();
		this.testoPotenzaMotore = this.mirinoInfoVelivoli.transform.GetChild(0).GetComponent<Text>();
		this.testoAltitudine = this.mirinoInfoVelivoli.transform.GetChild(1).GetComponent<Text>();
	}

	// Token: 0x0600018D RID: 397 RVA: 0x000437C4 File Offset: 0x000419C4
	private void Update()
	{
		this.CreazioneOrdigniConRifornimento();
		this.CondizioniArma1();
		this.CondizioniArma2();
		this.CondizioniArma3();
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
		this.munizioneArma1 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0];
		this.timerFrequenzaArma1 += Time.deltaTime;
		if (Physics.Raycast(base.transform.position, -Vector3.up, 5f, 256))
		{
			this.vicinoATerra = true;
		}
		else
		{
			this.vicinoATerra = false;
		}
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.PreparazioneAttacco();
			if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Tab))
			{
				this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera = null;
				this.timerPosizionamentoTPS = 0f;
				this.timerPosizionamentoFPS = 0f;
			}
		}
		else
		{
			this.GestioneVisuali();
			this.SelezioneArma();
			this.GestioneOrdigniPrimaPersona();
			this.Mirini();
			this.StrumentazioneMirini();
			if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 0)
			{
				this.AttaccoPrimaPersonaArma1();
				this.ordignoDaLanciare = null;
			}
			if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva == 3)
			{
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
					base.gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
					base.gameObject.transform.GetChild(2).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
					base.gameObject.transform.GetChild(2).transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
					base.gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					base.gameObject.transform.GetChild(3).transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
					this.suonoInterno.Play();
					this.suonoMotore.volume = this.volumeBaseEsterno / 2.3f;
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
					this.zoomAttivo = false;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					base.gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
					base.gameObject.transform.GetChild(2).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					base.gameObject.transform.GetChild(2).transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
					base.gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
					base.gameObject.transform.GetChild(3).transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
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
			base.gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
			base.gameObject.transform.GetChild(2).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
			base.gameObject.transform.GetChild(2).transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
			base.gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			base.gameObject.transform.GetChild(3).transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
			this.suonoInterno.Stop();
			this.suonoMotore.volume = this.volumeBaseEsterno;
			this.audioBeepLungoAttivo = false;
			this.suonoBeep.Stop();
			this.timerDiAggancio = 0f;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
			this.zoomAttivo = false;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x0600018E RID: 398 RVA: 0x00043D64 File Offset: 0x00041F64
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
		this.ListaOrdigniLocali = new List<GameObject>();
		this.ListaOrdigniLocali.Add(this.ordigno0);
		this.ListaOrdigniLocali.Add(this.ordigno1);
		this.ListaOrdigniLocali.Add(this.ordigno2);
		this.ListaOrdigniLocali.Add(this.ordigno3);
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

	// Token: 0x0600018F RID: 399 RVA: 0x000444E0 File Offset: 0x000426E0
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

	// Token: 0x06000190 RID: 400 RVA: 0x000447D4 File Offset: 0x000429D4
	private void CondizioniArma1()
	{
		base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[0][6];
	}

	// Token: 0x06000191 RID: 401 RVA: 0x00044810 File Offset: 0x00042A10
	private void CondizioniArma2()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[1][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[1][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[1][6];
		}
	}

	// Token: 0x06000192 RID: 402 RVA: 0x00044880 File Offset: 0x00042A80
	private void CondizioniArma3()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[2][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[2][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[2][6];
		}
	}

	// Token: 0x06000193 RID: 403 RVA: 0x000448F0 File Offset: 0x00042AF0
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

	// Token: 0x06000194 RID: 404 RVA: 0x0004499C File Offset: 0x00042B9C
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

	// Token: 0x06000195 RID: 405 RVA: 0x00044A24 File Offset: 0x00042C24
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = base.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = base.transform.rotation;
		}
		base.transform.GetChild(3).transform.GetChild(0).transform.Rotate(-Vector3.forward * 1000f * Time.deltaTime);
	}

	// Token: 0x06000196 RID: 406 RVA: 0x00044AE8 File Offset: 0x00042CE8
	private void Mirini()
	{
		this.mirinoFissoVelivoli.GetComponent<CanvasGroup>().alpha = 1f;
		this.mirinoInfoVelivoli.GetComponent<CanvasGroup>().alpha = 1f;
		this.livelloSuolo.GetComponent<CanvasGroup>().alpha = 1f;
		this.mirinoBombe.GetComponent<CanvasGroup>().alpha = 0f;
		if (this.mitragliatoreAttivo)
		{
			this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 0f;
		}
		if (this.ordignoDaLanciare && this.ordignoDaLanciare.transform.parent)
		{
			if (this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 9 || this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 10)
			{
				this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 0f;
				this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 0f;
				this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 0f;
				this.SistemaDiLancioInPrimaPersona();
			}
			else if (this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 23)
			{
				this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 0f;
				this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 0f;
				this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 1f;
				this.SistemaDiLancioInPrimaPersona();
			}
			else if (this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 24)
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

	// Token: 0x06000197 RID: 407 RVA: 0x00044D84 File Offset: 0x00042F84
	private void StrumentazioneMirini()
	{
		float num = 30f + base.GetComponent<MOV_HeavyHelicopter>().velocitàMax / 1.5f * 2f + base.GetComponent<MOV_HeavyHelicopter>().velocitàMax;
		float num2 = (30f + base.GetComponent<MOV_HeavyHelicopter>().velocitàTraslSalitaEffettiva + Mathf.Abs(base.GetComponent<MOV_HeavyHelicopter>().velocitàTraslDavDietroEffettiva) + Mathf.Abs(base.GetComponent<MOV_HeavyHelicopter>().velocitàTraslLatEffettiva)) / num * 100f;
		this.testoPotenzaMotore.text = num2.ToString("F0") + "%";
		this.testoAltitudine.text = base.transform.position.y.ToString("F0");
		this.livelloSuolo.transform.eulerAngles = new Vector3(0f, 0f, -base.transform.eulerAngles.z);
	}

	// Token: 0x06000198 RID: 408 RVA: 0x00044E74 File Offset: 0x00043074
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
			base.GetComponent<MOV_AUTOM_HeavyHelicopter>().muoviti = false;
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
								base.GetComponent<MOV_AUTOM_HeavyHelicopter>().muoviti = true;
							}
							else
							{
								base.GetComponent<MOV_AUTOM_HeavyHelicopter>().muoviti = false;
								this.unitàBersaglio = null;
							}
						}
						for (int j = 0; j < base.GetComponent<PresenzaAlleato>().numeroArmi; j++)
						{
							if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[j] && num4 < this.ListaMunizioniAttiveUnità[j].GetComponent<DatiGeneraliMunizione>().portataMassima)
							{
								base.transform.LookAt(this.centroUnitàBersaglio);
								if (j == 0)
								{
									this.AttaccoIndipendente1();
								}
								else if (j >= 1 && !this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
								{
									if (!this.ListaSparoConDestra[j - 1])
									{
										if (this.ListaOrdigniLocali[(j - 1) * 2].transform.childCount > 1 && this.ListaOrdigniLocali[(j - 1) * 2].transform.GetChild(1) != null)
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
										this.ListaSparoConDestra[j - 1] = false;
										this.AttaccoIndipendenteOrdigni();
										break;
									}
									if (this.ListaOrdigniLocali[(j - 1) * 2].transform.childCount > 1)
									{
										this.ordignoDaLanciare = this.ListaOrdigniLocali[(j - 1) * 2].transform.GetChild(1).gameObject;
										this.numArmaOrdignoDaLanciare = j;
										this.AttaccoIndipendenteOrdigni();
									}
									else if (this.ListaOrdigniLocali[(j - 1) * 2 + 1].transform.childCount > 1)
									{
										this.ordignoDaLanciare = this.ListaOrdigniLocali[(j - 1) * 2 + 1].transform.GetChild(1).gameObject;
										this.numArmaOrdignoDaLanciare = j;
										this.AttaccoIndipendenteOrdigni();
									}
								}
							}
						}
						if (num4 < num2)
						{
							if (!flag)
							{
								base.transform.LookAt(this.centroUnitàBersaglio);
								base.GetComponent<MOV_AUTOM_HeavyHelicopter>().muoviti = false;
							}
							else if (!base.GetComponent<PresenzaAlleato>().comportamentoDifensivo && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino)
							{
								base.GetComponent<MOV_AUTOM_HeavyHelicopter>().muoviti = true;
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
							base.GetComponent<MOV_AUTOM_HeavyHelicopter>().muoviti = true;
						}
						else
						{
							base.GetComponent<MOV_AUTOM_HeavyHelicopter>().muoviti = false;
							this.unitàBersaglio = null;
						}
					}
					for (int k = 0; k < base.GetComponent<PresenzaAlleato>().numeroArmi; k++)
					{
						if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[k] && num6 < this.ListaMunizioniAttiveUnità[k].GetComponent<DatiGeneraliMunizione>().portataMassima)
						{
							base.transform.LookAt(this.centroUnitàBersaglio);
							if (k == 0)
							{
								this.AttaccoIndipendente1();
							}
							else if (k >= 1 && !this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
							{
								if (!this.ListaSparoConDestra[k - 1])
								{
									if (this.ListaOrdigniLocali[(k - 1) * 2].transform.childCount > 1 && this.ListaOrdigniLocali[(k - 1) * 2].transform.GetChild(1) != null)
									{
										this.ordignoDaLanciare = this.ListaOrdigniLocali[(k - 1) * 2].transform.GetChild(1).gameObject;
										this.numArmaOrdignoDaLanciare = k;
										this.ListaSparoConDestra[k - 1] = true;
										this.AttaccoIndipendenteOrdigni();
										break;
									}
								}
								else if (this.ListaOrdigniLocali[(k - 1) * 2 + 1].transform.childCount > 1 && this.ListaOrdigniLocali[(k - 1) * 2 + 1].transform.GetChild(1) != null)
								{
									this.ordignoDaLanciare = this.ListaOrdigniLocali[(k - 1) * 2 + 1].transform.GetChild(1).gameObject;
									this.numArmaOrdignoDaLanciare = k;
									this.ListaSparoConDestra[k - 1] = false;
									this.AttaccoIndipendenteOrdigni();
									break;
								}
								if (this.ListaOrdigniLocali[(k - 1) * 2].transform.childCount > 1)
								{
									this.ordignoDaLanciare = this.ListaOrdigniLocali[(k - 1) * 2].transform.GetChild(1).gameObject;
									this.numArmaOrdignoDaLanciare = k;
									this.AttaccoIndipendenteOrdigni();
								}
								else if (this.ListaOrdigniLocali[(k - 1) * 2 + 1].transform.childCount > 1)
								{
									this.ordignoDaLanciare = this.ListaOrdigniLocali[(k - 1) * 2 + 1].transform.GetChild(1).gameObject;
									this.numArmaOrdignoDaLanciare = k;
									this.AttaccoIndipendenteOrdigni();
								}
							}
						}
					}
					if (num6 < num2)
					{
						if (!flag)
						{
							base.transform.LookAt(this.centroUnitàBersaglio);
							base.GetComponent<MOV_AUTOM_HeavyHelicopter>().muoviti = false;
						}
						else if (!base.GetComponent<PresenzaAlleato>().comportamentoDifensivo && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino)
						{
							base.GetComponent<MOV_AUTOM_HeavyHelicopter>().muoviti = true;
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
				base.GetComponent<MOV_AUTOM_HeavyHelicopter>().muoviti = true;
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
								if (num7 < this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMassima && !Physics.Linecast(this.bocca1.transform.position, current.GetComponent<PresenzaNemico>().centroInsetto, this.layerVisuale))
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
				base.GetComponent<MOV_AUTOM_HeavyHelicopter>().muoviti = true;
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
								if (num9 < this.munizioneArma1.GetComponent<DatiGeneraliMunizione>().portataMassima && !Physics.Linecast(this.bocca1.transform.position, current2.GetComponent<PresenzaNemico>().centroInsetto, this.layerVisuale))
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
				base.GetComponent<MOV_AUTOM_HeavyHelicopter>().muoviti = true;
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
			base.GetComponent<MOV_AUTOM_HeavyHelicopter>().muoviti = true;
		}
	}

	// Token: 0x06000199 RID: 409 RVA: 0x00045D38 File Offset: 0x00043F38
	private void AttaccoIndipendente1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && !this.vicinoATerra && this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f && !Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][0])
		{
			if (this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaAllPiùViciniPerTipo.Contains(base.gameObject) || this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] > 0.05f)
			{
				this.suonoBocca1.Play();
				this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] = 0f;
			}
			this.timerFrequenzaArma1 = 0f;
			this.mm30Proiettile = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
			this.mm30Proiettile.GetComponent<mm30Proiettile>().target = this.unitàBersaglio;
			this.mm30Proiettile.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
			List<float> list;
			List<float> expr_1BD = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int index;
			int expr_1C0 = index = 5;
			float num = list[index];
			expr_1BD[expr_1C0] = num - 1f;
			List<float> list2;
			List<float> expr_1E7 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int expr_1EA = index = 6;
			num = list2[index];
			expr_1E7[expr_1EA] = num - 1f;
			if (!this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
			{
				this.particelleBocca1.Play();
			}
		}
	}

	// Token: 0x0600019A RID: 410 RVA: 0x00045F68 File Offset: 0x00044168
	private void AttaccoIndipendenteOrdigni()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][5] > 0f && this.timerDiLancio > base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][0] && !this.vicinoATerra)
		{
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = false;
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().bersaglio = this.unitàBersaglio;
			List<float> list;
			List<float> expr_A9 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
			int index;
			int expr_AC = index = 5;
			float num = list[index];
			expr_A9[expr_AC] = num - 1f;
			List<float> list2;
			List<float> expr_D8 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
			int expr_DC = index = 6;
			num = list2[index];
			expr_D8[expr_DC] = num - 1f;
			this.timerDiLancio = 0f;
			for (int i = 0; i < this.ListaOrdigniLocali.Count; i++)
			{
				this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ordignoAttivo = false;
			}
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().ordignoAttivo = true;
			this.ordignoDaLanciare = null;
		}
	}

	// Token: 0x0600019B RID: 411 RVA: 0x000460C8 File Offset: 0x000442C8
	private void SelezioneArma()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 0;
			this.audioBeepLungoAttivo = false;
			this.suonoBeep.Stop();
			this.timerDiAggancio = 0f;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 1;
			this.audioBeepLungoAttivo = false;
			this.suonoBeep.Stop();
			this.timerDiAggancio = 0f;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 2;
			this.audioBeepLungoAttivo = false;
			this.suonoBeep.Stop();
			this.timerDiAggancio = 0f;
		}
	}

	// Token: 0x0600019C RID: 412 RVA: 0x00046174 File Offset: 0x00044374
	private void GestioneOrdigniPrimaPersona()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.mitragliatoreAttivo = true;
			this.gruppo01Attivo = false;
			this.gruppo23Attivo = false;
			this.timerDiAggancio = 0f;
			this.ordignoDaLanciare = null;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.mitragliatoreAttivo = false;
			this.gruppo01Attivo = true;
			this.gruppo23Attivo = false;
			this.timerDiAggancio = 0f;
			this.ordignoDaLanciare = null;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			this.mitragliatoreAttivo = false;
			this.gruppo01Attivo = false;
			this.gruppo23Attivo = true;
			this.timerDiAggancio = 0f;
			this.ordignoDaLanciare = null;
		}
		this.ListaGruppiOrdigniAttivi.Clear();
		this.ListaGruppiOrdigniAttivi.Add(this.gruppo01Attivo);
		this.ListaGruppiOrdigniAttivi.Add(this.gruppo23Attivo);
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

	// Token: 0x0600019D RID: 413 RVA: 0x00046468 File Offset: 0x00044668
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
		if (Input.GetMouseButton(0) && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][1])
		{
			this.timerFrequenzaArma1 = 0f;
			this.SparoArma1();
			this.suonoBocca1.Play();
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
		}
	}

	// Token: 0x0600019E RID: 414 RVA: 0x000466A4 File Offset: 0x000448A4
	private void SparoArma1()
	{
		this.mm30Proiettile = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.mm30Proiettile.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.mm30Proiettile.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x0600019F RID: 415 RVA: 0x00046714 File Offset: 0x00044914
	private void SistemaDiLancioInPrimaPersona()
	{
		this.ListaBersPPPossibili.Clear();
		float num = (float)(Screen.width / 13);
		if ((this.ordignoDaLanciare && this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 9) || this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 10)
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
				List<float> list;
				List<float> expr_222 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
				int index;
				int expr_226 = index = 5;
				float num2 = list[index];
				expr_222[expr_226] = num2 - 1f;
				List<float> list2;
				List<float> expr_257 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
				int expr_25B = index = 6;
				num2 = list2[index];
				expr_257[expr_25B] = num2 - 1f;
				for (int i = 0; i < this.ListaOrdigniLocali.Count; i++)
				{
					this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ordignoAttivo = false;
				}
				this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().ordignoAttivo = true;
				this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = true;
				this.ordignoDaLanciare = null;
			}
		}
		if (this.ordignoDaLanciare && this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 23 && Input.GetMouseButtonDown(0) && base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][5] > 0f && this.timerDiLancio > base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][1])
		{
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = true;
			List<float> list3;
			List<float> expr_3A0 = list3 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
			int index;
			int expr_3A4 = index = 5;
			float num2 = list3[index];
			expr_3A0[expr_3A4] = num2 - 1f;
			List<float> list4;
			List<float> expr_3D5 = list4 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
			int expr_3D9 = index = 6;
			num2 = list4[index];
			expr_3D5[expr_3D9] = num2 - 1f;
			this.timerDiLancio = 0f;
			for (int j = 0; j < this.ListaOrdigniLocali.Count; j++)
			{
				this.ListaOrdigniLocali[j].GetComponent<DatiOrdignoEsterno>().ordignoAttivo = false;
			}
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().ordignoAttivo = true;
			this.ordignoDaLanciare = null;
		}
		if (this.ordignoDaLanciare && this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 24)
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
				float num9 = Vector2.Distance(this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(this.sensoreRaggioMirinoMobile.transform.position), this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(this.mirinoMissiliFisso.transform.position)) / 1000f;
				float num10 = Vector2.Distance(this.terzaCamera.GetComponent<Camera>().WorldToScreenPoint(this.bersaglioInPP.GetComponent<PresenzaNemico>().centroInsetto), this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f)));
				if (num10 < num9 && this.bersaglioèAvantiInPP)
				{
					this.mirinoMissiliMobile.GetComponent<Image>().color = Color.red;
					this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1f;
					if (!this.audioBeepLungoAttivo)
					{
						this.suonoBeep.Play();
						this.audioBeepLungoAttivo = true;
					}
					if (Input.GetMouseButtonDown(0) && base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][5] > 0f && this.timerDiLancio > base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][1] && this.ordignoDaLanciare != null)
					{
						this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().bersaglio = this.bersaglioInPP;
						this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = true;
						List<float> list5;
						List<float> expr_B05 = list5 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
						int index;
						int expr_B09 = index = 5;
						float num2 = list5[index];
						expr_B05[expr_B09] = num2 - 1f;
						List<float> list6;
						List<float> expr_B3A = list6 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
						int expr_B3E = index = 6;
						num2 = list6[index];
						expr_B3A[expr_B3E] = num2 - 1f;
						this.timerDiLancio = 0f;
						for (int k = 0; k < this.ListaOrdigniLocali.Count; k++)
						{
							this.ListaOrdigniLocali[k].GetComponent<DatiOrdignoEsterno>().ordignoAttivo = false;
						}
						this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().ordignoAttivo = true;
						this.ordignoDaLanciare.transform.parent.GetComponent<AudioSource>().Play();
						this.ordignoDaLanciare = null;
						this.audioBeepLungoAttivo = false;
						this.suonoBeep.Stop();
					}
				}
				if (num10 > num9 || !this.bersaglioèAvantiInPP)
				{
					this.mirinoMissiliMobile.GetComponent<Image>().color = this.coloreBaseMirini;
					this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
					this.audioBeepLungoAttivo = false;
					this.suonoBeep.Stop();
				}
				float num11 = Vector2.Distance(this.terzaCamera.GetComponent<Camera>().WorldToScreenPoint(this.bersaglioInPP.GetComponent<PresenzaNemico>().centroInsetto), this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f)));
				if (num11 > num || !this.bersaglioèAvantiInPP)
				{
					this.bersaglioInPP = null;
				}
				if (base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][5] <= 0f)
				{
					this.audioBeepLungoAttivo = false;
					this.suonoBeep.Stop();
				}
			}
		}
	}

	// Token: 0x040006FC RID: 1788
	private GameObject infoNeutreTattica;

	// Token: 0x040006FD RID: 1789
	private GameObject terzaCamera;

	// Token: 0x040006FE RID: 1790
	private GameObject primaCamera;

	// Token: 0x040006FF RID: 1791
	public GameObject bocca1;

	// Token: 0x04000700 RID: 1792
	private GameObject IANemico;

	// Token: 0x04000701 RID: 1793
	private GameObject InfoAlleati;

	// Token: 0x04000702 RID: 1794
	public float angAttaccoMin;

	// Token: 0x04000703 RID: 1795
	public float angAttaccoMax;

	// Token: 0x04000704 RID: 1796
	private GameObject mm30Proiettile;

	// Token: 0x04000705 RID: 1797
	private float timerFrequenzaArma1;

	// Token: 0x04000706 RID: 1798
	private float timerRicarica1;

	// Token: 0x04000707 RID: 1799
	private bool ricaricaInCorso1;

	// Token: 0x04000708 RID: 1800
	private float timerDiLancio;

	// Token: 0x04000709 RID: 1801
	private int layerColpo;

	// Token: 0x0400070A RID: 1802
	private int layerVisuale;

	// Token: 0x0400070B RID: 1803
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x0400070C RID: 1804
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x0400070D RID: 1805
	private float timerPosizionamentoTPS;

	// Token: 0x0400070E RID: 1806
	private float timerPosizionamentoFPS;

	// Token: 0x0400070F RID: 1807
	private GameObject CanvasFPS;

	// Token: 0x04000710 RID: 1808
	private GameObject sensoreRaggioMirino;

	// Token: 0x04000711 RID: 1809
	private GameObject sensoreRaggioMirinoMobile;

	// Token: 0x04000712 RID: 1810
	private GameObject mirinoFissoVelivoli;

	// Token: 0x04000713 RID: 1811
	private GameObject mirinoMissiliFisso;

	// Token: 0x04000714 RID: 1812
	private GameObject mirinoMissiliMobile;

	// Token: 0x04000715 RID: 1813
	private Color coloreBaseMirini;

	// Token: 0x04000716 RID: 1814
	private GameObject mirinoMissiliFiloguidati;

	// Token: 0x04000717 RID: 1815
	private GameObject mirinoBombe;

	// Token: 0x04000718 RID: 1816
	private GameObject mirinoInfoVelivoli;

	// Token: 0x04000719 RID: 1817
	private GameObject livelloSuolo;

	// Token: 0x0400071A RID: 1818
	private RaycastHit targetSparo;

	// Token: 0x0400071B RID: 1819
	private NavMeshAgent alleatoNav;

	// Token: 0x0400071C RID: 1820
	private float velocitàAlleatoNav;

	// Token: 0x0400071D RID: 1821
	private GameObject cannone;

	// Token: 0x0400071E RID: 1822
	public GameObject unitàBersaglio;

	// Token: 0x0400071F RID: 1823
	private Vector3 centroUnitàBersaglio;

	// Token: 0x04000720 RID: 1824
	private GameObject munizioneArma1;

	// Token: 0x04000721 RID: 1825
	private GameObject munizioneArma2;

	// Token: 0x04000722 RID: 1826
	private GameObject munizioneArma3;

	// Token: 0x04000723 RID: 1827
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x04000724 RID: 1828
	private List<GameObject> ListaOrdigniLocali;

	// Token: 0x04000725 RID: 1829
	private GameObject ordignoDaLanciare;

	// Token: 0x04000726 RID: 1830
	private int numArmaOrdignoDaLanciare;

	// Token: 0x04000727 RID: 1831
	private GameObject ordigno0;

	// Token: 0x04000728 RID: 1832
	private GameObject ordigno1;

	// Token: 0x04000729 RID: 1833
	private GameObject ordigno2;

	// Token: 0x0400072A RID: 1834
	private GameObject ordigno3;

	// Token: 0x0400072B RID: 1835
	public Vector3 posizioneOrdigni01;

	// Token: 0x0400072C RID: 1836
	public Vector3 posizioneOrdigni23;

	// Token: 0x0400072D RID: 1837
	private bool mitragliatoreAttivo;

	// Token: 0x0400072E RID: 1838
	private bool gruppo01Attivo;

	// Token: 0x0400072F RID: 1839
	private bool gruppo23Attivo;

	// Token: 0x04000730 RID: 1840
	private float timerDiAggancio;

	// Token: 0x04000731 RID: 1841
	public List<GameObject> ListaOrdigniAttiviLocale;

	// Token: 0x04000732 RID: 1842
	private List<bool> ListaGruppiOrdigniAttivi;

	// Token: 0x04000733 RID: 1843
	private bool sparoConDestra01;

	// Token: 0x04000734 RID: 1844
	private bool sparoConDestra23;

	// Token: 0x04000735 RID: 1845
	private List<bool> ListaSparoConDestra;

	// Token: 0x04000736 RID: 1846
	private bool primoFrameAvvenuto;

	// Token: 0x04000737 RID: 1847
	public List<GameObject> ListaBersPPPossibili;

	// Token: 0x04000738 RID: 1848
	public GameObject bersaglioInPP;

	// Token: 0x04000739 RID: 1849
	private bool bersaglioèAvantiInPP;

	// Token: 0x0400073A RID: 1850
	private bool bersDavantiEAPortata;

	// Token: 0x0400073B RID: 1851
	private float volumeBaseEsterno;

	// Token: 0x0400073C RID: 1852
	private GameObject ordignoFittizio;

	// Token: 0x0400073D RID: 1853
	private bool audioBeepLungoAttivo;

	// Token: 0x0400073E RID: 1854
	private bool bersèNelMirino;

	// Token: 0x0400073F RID: 1855
	private AudioSource suonoMotore;

	// Token: 0x04000740 RID: 1856
	private AudioSource suonoInterno;

	// Token: 0x04000741 RID: 1857
	private AudioSource suonoBocca1;

	// Token: 0x04000742 RID: 1858
	private AudioSource suonoBeep;

	// Token: 0x04000743 RID: 1859
	private ParticleSystem particelleBocca1;

	// Token: 0x04000744 RID: 1860
	private bool zoomAttivo;

	// Token: 0x04000745 RID: 1861
	private Text testoPotenzaMotore;

	// Token: 0x04000746 RID: 1862
	private Text testoAltitudine;

	// Token: 0x04000747 RID: 1863
	private float timerAggRicerca;

	// Token: 0x04000748 RID: 1864
	private bool vicinoATerra;
}
