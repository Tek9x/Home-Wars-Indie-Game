using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000022 RID: 34
public class ATT_AttackHelicopter : MonoBehaviour
{
	// Token: 0x06000177 RID: 375 RVA: 0x0003F38C File Offset: 0x0003D58C
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
		this.corpoArma1 = this.bocca1.transform.parent.gameObject;
		this.testoPotenzaMotore = this.mirinoInfoVelivoli.transform.GetChild(0).GetComponent<Text>();
		this.testoAltitudine = this.mirinoInfoVelivoli.transform.GetChild(1).GetComponent<Text>();
	}

	// Token: 0x06000178 RID: 376 RVA: 0x0003F6F0 File Offset: 0x0003D8F0
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
					base.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
					base.transform.GetChild(2).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
					base.transform.GetChild(2).transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
					base.transform.GetChild(2).transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
					base.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					base.transform.GetChild(3).transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
					this.suonoInterno.Play();
					this.suonoMotore.volume = this.volumeBaseEsterno / 2.3f;
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
					this.zoomAttivo = false;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					base.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
					base.transform.GetChild(2).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					base.transform.GetChild(2).transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
					base.transform.GetChild(2).transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
					base.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
					base.transform.GetChild(3).transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
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
			base.gameObject.transform.GetChild(2).transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
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

	// Token: 0x06000179 RID: 377 RVA: 0x0003FCCC File Offset: 0x0003DECC
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

	// Token: 0x0600017A RID: 378 RVA: 0x00040448 File Offset: 0x0003E648
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

	// Token: 0x0600017B RID: 379 RVA: 0x0004073C File Offset: 0x0003E93C
	private void CondizioniArma1()
	{
		base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[0][6];
		if (this.rotArma1Attiva)
		{
			this.corpoArma1.transform.Rotate(Vector3.up * 9f);
			if (base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] == 0f)
			{
				this.rotArma1Attiva = false;
			}
		}
	}

	// Token: 0x0600017C RID: 380 RVA: 0x000407CC File Offset: 0x0003E9CC
	private void CondizioniArma2()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[1][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[1][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[1][6];
		}
	}

	// Token: 0x0600017D RID: 381 RVA: 0x0004083C File Offset: 0x0003EA3C
	private void CondizioniArma3()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[2][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[2][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[2][6];
		}
	}

	// Token: 0x0600017E RID: 382 RVA: 0x000408AC File Offset: 0x0003EAAC
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

	// Token: 0x0600017F RID: 383 RVA: 0x00040958 File Offset: 0x0003EB58
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

	// Token: 0x06000180 RID: 384 RVA: 0x000409E0 File Offset: 0x0003EBE0
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = base.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = base.transform.rotation;
		}
		base.transform.GetChild(3).transform.GetChild(0).transform.Rotate(Vector3.forward * 2000f * Time.deltaTime);
	}

	// Token: 0x06000181 RID: 385 RVA: 0x00040AA0 File Offset: 0x0003ECA0
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
			if (this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 1 || this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 2)
			{
				this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 0f;
				this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 0f;
				this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 0f;
				this.SistemaDiLancioInPrimaPersona();
			}
			else if (this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 3)
			{
				this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 0f;
				this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 0f;
				this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 1f;
				this.SistemaDiLancioInPrimaPersona();
			}
			else if (this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 4)
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

	// Token: 0x06000182 RID: 386 RVA: 0x00040D38 File Offset: 0x0003EF38
	private void StrumentazioneMirini()
	{
		float num = 30f + base.GetComponent<MOV_AttackHelicopter>().velocitàMax / 1.5f * 2f + base.GetComponent<MOV_AttackHelicopter>().velocitàMax;
		float num2 = (30f + base.GetComponent<MOV_AttackHelicopter>().velocitàTraslSalitaEffettiva + Mathf.Abs(base.GetComponent<MOV_AttackHelicopter>().velocitàTraslDavDietroEffettiva) + Mathf.Abs(base.GetComponent<MOV_AttackHelicopter>().velocitàTraslLatEffettiva)) / num * 100f;
		this.testoPotenzaMotore.text = num2.ToString("F0") + "%";
		this.testoAltitudine.text = base.transform.position.y.ToString("F0");
		this.livelloSuolo.transform.eulerAngles = new Vector3(0f, 0f, -base.transform.eulerAngles.z);
	}

	// Token: 0x06000183 RID: 387 RVA: 0x00040E28 File Offset: 0x0003F028
	private void PreparazioneAttacco()
	{
		bool flag = false;
		if (this.unitàBersaglio)
		{
			if (Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale))
			{
				flag = true;
				this.rotArma1Attiva = false;
			}
			else
			{
				flag = false;
			}
			this.centroUnitàBersaglio = this.unitàBersaglio.GetComponent<PresenzaNemico>().centroInsetto;
			float num = Vector3.Angle(base.transform.forward, (this.centroUnitàBersaglio - base.transform.position).normalized);
			if (num < 5f)
			{
				this.bersèNelMirino = true;
			}
			else
			{
				this.bersèNelMirino = false;
				this.rotArma1Attiva = false;
			}
		}
		else
		{
			this.rotArma1Attiva = false;
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
			this.rotArma1Attiva = false;
		}
		if (!base.GetComponent<PresenzaAlleato>().destinazioneOrdinata)
		{
			base.GetComponent<MOV_AUTOM_AttackHelicopter>().muoviti = false;
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
								base.GetComponent<MOV_AUTOM_AttackHelicopter>().muoviti = true;
							}
							else
							{
								base.GetComponent<MOV_AUTOM_AttackHelicopter>().muoviti = false;
								this.unitàBersaglio = null;
							}
							this.rotArma1Attiva = false;
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
								base.GetComponent<MOV_AUTOM_AttackHelicopter>().muoviti = false;
							}
							else if (!base.GetComponent<PresenzaAlleato>().comportamentoDifensivo && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino)
							{
								base.GetComponent<MOV_AUTOM_AttackHelicopter>().muoviti = true;
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
							base.GetComponent<MOV_AUTOM_AttackHelicopter>().muoviti = true;
						}
						else
						{
							base.GetComponent<MOV_AUTOM_AttackHelicopter>().muoviti = false;
							this.unitàBersaglio = null;
						}
						this.rotArma1Attiva = false;
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
							base.GetComponent<MOV_AUTOM_AttackHelicopter>().muoviti = false;
						}
						else if (!base.GetComponent<PresenzaAlleato>().comportamentoDifensivo && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers && !base.GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino)
						{
							base.GetComponent<MOV_AUTOM_AttackHelicopter>().muoviti = true;
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
				base.GetComponent<MOV_AUTOM_AttackHelicopter>().muoviti = true;
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
				base.GetComponent<MOV_AUTOM_AttackHelicopter>().muoviti = true;
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
				base.GetComponent<MOV_AUTOM_AttackHelicopter>().muoviti = true;
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
			base.GetComponent<MOV_AUTOM_AttackHelicopter>().muoviti = true;
			this.rotArma1Attiva = false;
		}
	}

	// Token: 0x06000184 RID: 388 RVA: 0x00041D38 File Offset: 0x0003FF38
	private void AttaccoIndipendente1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && !this.vicinoATerra)
		{
			if (this.unitàBersaglio && this.unitàBersaglio.GetComponent<PresenzaNemico>().vita > 0f)
			{
				if (!Physics.Linecast(this.bocca1.transform.position, this.centroUnitàBersaglio, this.layerVisuale) && base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] > 0f && this.timerFrequenzaArma1 > base.GetComponent<PresenzaAlleato>().ListaArmi[0][0])
				{
					if (this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaAllPiùViciniPerTipo.Contains(base.gameObject) || this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] > 0.05f)
					{
						this.suonoBocca1.Play();
						this.InfoAlleati.GetComponent<InfoGenericheAlleati>().ListaTimerSuoniArmi[base.GetComponent<PresenzaAlleato>().tipoTruppa][0] = 0f;
					}
					this.timerFrequenzaArma1 = 0f;
					this.mm20Proiettile = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
					this.mm20Proiettile.GetComponent<mm20Proiettile>().target = this.unitàBersaglio;
					this.mm20Proiettile.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
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
					this.rotArma1Attiva = true;
					if (!this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
					{
						this.particelleBocca1.Play();
					}
				}
			}
			else
			{
				this.rotArma1Attiva = false;
			}
		}
		else
		{
			this.rotArma1Attiva = false;
		}
	}

	// Token: 0x06000185 RID: 389 RVA: 0x00041F84 File Offset: 0x00040184
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

	// Token: 0x06000186 RID: 390 RVA: 0x000420E4 File Offset: 0x000402E4
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

	// Token: 0x06000187 RID: 391 RVA: 0x00042190 File Offset: 0x00040390
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

	// Token: 0x06000188 RID: 392 RVA: 0x00042484 File Offset: 0x00040684
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

	// Token: 0x06000189 RID: 393 RVA: 0x000426C0 File Offset: 0x000408C0
	private void SparoArma1()
	{
		this.mm20Proiettile = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.mm20Proiettile.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.mm20Proiettile.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x0600018A RID: 394 RVA: 0x00042730 File Offset: 0x00040930
	private void SistemaDiLancioInPrimaPersona()
	{
		this.ListaBersPPPossibili.Clear();
		float num = (float)(Screen.width / 13);
		if ((this.ordignoDaLanciare && this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 1) || this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 2)
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
				List<float> expr_220 = list = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
				int index;
				int expr_224 = index = 5;
				float num2 = list[index];
				expr_220[expr_224] = num2 - 1f;
				List<float> list2;
				List<float> expr_255 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
				int expr_259 = index = 6;
				num2 = list2[index];
				expr_255[expr_259] = num2 - 1f;
				for (int i = 0; i < this.ListaOrdigniLocali.Count; i++)
				{
					this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ordignoAttivo = false;
				}
				this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().ordignoAttivo = true;
				this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = true;
				this.ordignoDaLanciare = null;
			}
		}
		if (this.ordignoDaLanciare && this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 3 && Input.GetMouseButtonDown(0) && base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][5] > 0f && this.timerDiLancio > base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][1])
		{
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = true;
			List<float> list3;
			List<float> expr_39D = list3 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
			int index;
			int expr_3A1 = index = 5;
			float num2 = list3[index];
			expr_39D[expr_3A1] = num2 - 1f;
			List<float> list4;
			List<float> expr_3D2 = list4 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
			int expr_3D6 = index = 6;
			num2 = list4[index];
			expr_3D2[expr_3D6] = num2 - 1f;
			this.timerDiLancio = 0f;
			for (int j = 0; j < this.ListaOrdigniLocali.Count; j++)
			{
				this.ListaOrdigniLocali[j].GetComponent<DatiOrdignoEsterno>().ordignoAttivo = false;
			}
			this.ordignoDaLanciare.transform.parent.GetComponent<AudioSource>().Play();
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().ordignoAttivo = true;
			this.ordignoDaLanciare = null;
		}
		if (this.ordignoDaLanciare && this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 4)
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
						List<float> expr_B1B = list5 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
						int index;
						int expr_B1F = index = 5;
						float num2 = list5[index];
						expr_B1B[expr_B1F] = num2 - 1f;
						List<float> list6;
						List<float> expr_B50 = list6 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
						int expr_B54 = index = 6;
						num2 = list6[index];
						expr_B50[expr_B54] = num2 - 1f;
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

	// Token: 0x040006B0 RID: 1712
	private GameObject infoNeutreTattica;

	// Token: 0x040006B1 RID: 1713
	private GameObject terzaCamera;

	// Token: 0x040006B2 RID: 1714
	private GameObject primaCamera;

	// Token: 0x040006B3 RID: 1715
	public GameObject bocca1;

	// Token: 0x040006B4 RID: 1716
	private GameObject IANemico;

	// Token: 0x040006B5 RID: 1717
	private GameObject InfoAlleati;

	// Token: 0x040006B6 RID: 1718
	public float angAttaccoMin;

	// Token: 0x040006B7 RID: 1719
	public float angAttaccoMax;

	// Token: 0x040006B8 RID: 1720
	private GameObject mm20Proiettile;

	// Token: 0x040006B9 RID: 1721
	private float timerFrequenzaArma1;

	// Token: 0x040006BA RID: 1722
	private float timerRicarica1;

	// Token: 0x040006BB RID: 1723
	private bool ricaricaInCorso1;

	// Token: 0x040006BC RID: 1724
	private float timerDiLancio;

	// Token: 0x040006BD RID: 1725
	private int layerColpo;

	// Token: 0x040006BE RID: 1726
	private int layerVisuale;

	// Token: 0x040006BF RID: 1727
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x040006C0 RID: 1728
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x040006C1 RID: 1729
	private float timerPosizionamentoTPS;

	// Token: 0x040006C2 RID: 1730
	private float timerPosizionamentoFPS;

	// Token: 0x040006C3 RID: 1731
	private GameObject CanvasFPS;

	// Token: 0x040006C4 RID: 1732
	private GameObject sensoreRaggioMirino;

	// Token: 0x040006C5 RID: 1733
	private GameObject sensoreRaggioMirinoMobile;

	// Token: 0x040006C6 RID: 1734
	private GameObject mirinoFissoVelivoli;

	// Token: 0x040006C7 RID: 1735
	private GameObject mirinoMissiliFisso;

	// Token: 0x040006C8 RID: 1736
	private GameObject mirinoMissiliMobile;

	// Token: 0x040006C9 RID: 1737
	private Color coloreBaseMirini;

	// Token: 0x040006CA RID: 1738
	private GameObject mirinoMissiliFiloguidati;

	// Token: 0x040006CB RID: 1739
	private GameObject mirinoBombe;

	// Token: 0x040006CC RID: 1740
	private GameObject mirinoInfoVelivoli;

	// Token: 0x040006CD RID: 1741
	private GameObject livelloSuolo;

	// Token: 0x040006CE RID: 1742
	private RaycastHit targetSparo;

	// Token: 0x040006CF RID: 1743
	private GameObject cannone;

	// Token: 0x040006D0 RID: 1744
	public GameObject unitàBersaglio;

	// Token: 0x040006D1 RID: 1745
	private Vector3 centroUnitàBersaglio;

	// Token: 0x040006D2 RID: 1746
	private GameObject munizioneArma1;

	// Token: 0x040006D3 RID: 1747
	private GameObject munizioneArma2;

	// Token: 0x040006D4 RID: 1748
	private GameObject munizioneArma3;

	// Token: 0x040006D5 RID: 1749
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x040006D6 RID: 1750
	private List<GameObject> ListaOrdigniLocali;

	// Token: 0x040006D7 RID: 1751
	private GameObject ordignoDaLanciare;

	// Token: 0x040006D8 RID: 1752
	private int numArmaOrdignoDaLanciare;

	// Token: 0x040006D9 RID: 1753
	private GameObject ordigno0;

	// Token: 0x040006DA RID: 1754
	private GameObject ordigno1;

	// Token: 0x040006DB RID: 1755
	private GameObject ordigno2;

	// Token: 0x040006DC RID: 1756
	private GameObject ordigno3;

	// Token: 0x040006DD RID: 1757
	public Vector3 posizioneOrdigni01;

	// Token: 0x040006DE RID: 1758
	public Vector3 posizioneOrdigni23;

	// Token: 0x040006DF RID: 1759
	private bool mitragliatoreAttivo;

	// Token: 0x040006E0 RID: 1760
	private bool gruppo01Attivo;

	// Token: 0x040006E1 RID: 1761
	private bool gruppo23Attivo;

	// Token: 0x040006E2 RID: 1762
	private float timerDiAggancio;

	// Token: 0x040006E3 RID: 1763
	public List<GameObject> ListaOrdigniAttiviLocale;

	// Token: 0x040006E4 RID: 1764
	private List<bool> ListaGruppiOrdigniAttivi;

	// Token: 0x040006E5 RID: 1765
	private bool sparoConDestra01;

	// Token: 0x040006E6 RID: 1766
	private bool sparoConDestra23;

	// Token: 0x040006E7 RID: 1767
	private List<bool> ListaSparoConDestra;

	// Token: 0x040006E8 RID: 1768
	private bool primoFrameAvvenuto;

	// Token: 0x040006E9 RID: 1769
	public List<GameObject> ListaBersPPPossibili;

	// Token: 0x040006EA RID: 1770
	public GameObject bersaglioInPP;

	// Token: 0x040006EB RID: 1771
	private bool bersaglioèAvantiInPP;

	// Token: 0x040006EC RID: 1772
	private bool bersDavantiEAPortata;

	// Token: 0x040006ED RID: 1773
	private float volumeBaseEsterno;

	// Token: 0x040006EE RID: 1774
	private bool audioBeepLungoAttivo;

	// Token: 0x040006EF RID: 1775
	private bool bersèNelMirino;

	// Token: 0x040006F0 RID: 1776
	private AudioSource suonoMotore;

	// Token: 0x040006F1 RID: 1777
	private AudioSource suonoInterno;

	// Token: 0x040006F2 RID: 1778
	private AudioSource suonoBocca1;

	// Token: 0x040006F3 RID: 1779
	private AudioSource suonoBeep;

	// Token: 0x040006F4 RID: 1780
	private ParticleSystem particelleBocca1;

	// Token: 0x040006F5 RID: 1781
	private bool rotArma1Attiva;

	// Token: 0x040006F6 RID: 1782
	private GameObject corpoArma1;

	// Token: 0x040006F7 RID: 1783
	private bool zoomAttivo;

	// Token: 0x040006F8 RID: 1784
	private Text testoPotenzaMotore;

	// Token: 0x040006F9 RID: 1785
	private Text testoAltitudine;

	// Token: 0x040006FA RID: 1786
	private float timerAggRicerca;

	// Token: 0x040006FB RID: 1787
	private bool vicinoATerra;
}
