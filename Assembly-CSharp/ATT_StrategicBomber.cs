using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200001B RID: 27
public class ATT_StrategicBomber : MonoBehaviour
{
	// Token: 0x06000129 RID: 297 RVA: 0x00034FC0 File Offset: 0x000331C0
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
		this.layerColpo = 34048;
		this.layerVisuale = 256;
		this.ListaMunizioniAttiveUnità = new List<GameObject>();
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma1);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma2);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma3);
		this.ListaMunizioniAttiveUnità.Add(this.munizioneArma4);
		this.ListaSparoConDestra = new List<bool>();
		this.ListaSparoConDestra.Add(this.sparoConDestra01);
		this.ListaSparoConDestra.Add(this.sparoConDestra23);
		this.ListaSparoConDestra.Add(this.sparoConDestra45);
		this.ListaSparoConDestra.Add(this.sparoConDestra67);
		this.ListaGruppiOrdigniAttivi = new List<bool>();
		this.coloreBaseMirini = this.mirinoFissoVelivoli.GetComponent<Image>().color;
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.volumeBaseEsterno = this.suonoMotore.volume;
		this.suonoMotore.Play();
		this.testoPotenzaMotore = this.mirinoInfoVelivoli.transform.GetChild(0).GetComponent<Text>();
		this.testoAltitudine = this.mirinoInfoVelivoli.transform.GetChild(1).GetComponent<Text>();
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00035304 File Offset: 0x00033504
	private void Update()
	{
		this.CreazioneOrdigniConRifornimento();
		this.CondizioniArma1();
		this.CondizioniArma2();
		this.CondizioniArma3();
		this.CondizioniArma4();
		this.timerDiLancio += Time.deltaTime;
		if (!this.primoFrameAvvenuto)
		{
			this.CreazioneInizialeOrdigni();
			this.gruppo01Attivo = true;
			this.primoFrameAvvenuto = true;
		}
		this.unitàBersaglio = base.GetComponent<PresenzaAlleato>().unitàBersaglio;
		this.ListaMunizioniAttiveUnità[0] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[0][0];
		this.ListaMunizioniAttiveUnità[1] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[1][0];
		this.ListaMunizioniAttiveUnità[2] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[2][0];
		this.ListaMunizioniAttiveUnità[3] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[3][0];
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.PreparazioneAttacco();
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
				base.GetComponent<MOV_StrategicBomber>().velocitàFrontaleEffettiva = 20f;
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.terzaCamera.transform.localPosition = this.posCameraPsteriore;
				this.terzaCamera.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
				base.gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
				base.gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
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
						this.suonoMotore.volume = this.volumeBaseEsterno / 3f;
					}
					if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
					{
						this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
						base.gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
						base.gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
						base.gameObject.transform.GetChild(2).GetComponent<AudioSource>().Stop();
						this.suonoMotore.volume = this.volumeBaseEsterno;
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
			this.bersaglioInPP = null;
			this.angCameraBomb.GetComponent<CanvasGroup>().alpha = 0f;
			this.visualeBombAttiva = false;
			this.timerPresenzaInAereo = 0f;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00035A40 File Offset: 0x00033C40
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
		this.ordigno6 = (UnityEngine.Object.Instantiate(base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[3], base.transform.position, base.transform.rotation) as GameObject);
		this.ordigno6.transform.parent = base.transform;
		this.ordigno6.transform.localPosition = this.posizioneOrdigni67;
		this.ordigno7 = (UnityEngine.Object.Instantiate(base.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[3], base.transform.position, base.transform.rotation) as GameObject);
		this.ordigno7.transform.parent = base.transform;
		this.ordigno7.transform.localPosition = new Vector3(-this.posizioneOrdigni67.x, this.posizioneOrdigni67.y, this.posizioneOrdigni67.z);
		this.ListaOrdigniLocali = new List<GameObject>();
		this.ListaOrdigniLocali.Add(this.ordigno0);
		this.ListaOrdigniLocali.Add(this.ordigno1);
		this.ListaOrdigniLocali.Add(this.ordigno2);
		this.ListaOrdigniLocali.Add(this.ordigno3);
		this.ListaOrdigniLocali.Add(this.ordigno4);
		this.ListaOrdigniLocali.Add(this.ordigno5);
		this.ListaOrdigniLocali.Add(this.ordigno6);
		this.ListaOrdigniLocali.Add(this.ordigno7);
		for (int j = 0; j < base.GetComponent<PresenzaAlleato>().numeroCoppieOrdigni; j++)
		{
			if (base.GetComponent<PresenzaAlleato>().ListaArmi[j][5] % 2f == 0f)
			{
				if (j * 2 == 0 || j * 2 == 2 || j * 2 == 4 || j * 2 == 6)
				{
					int num = 0;
					while ((float)num < base.GetComponent<PresenzaAlleato>().ListaArmi[j][5] / 2f)
					{
						this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[num] = (UnityEngine.Object.Instantiate(this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().munizioneUsata, base.transform.position, base.transform.rotation) as GameObject);
						this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[num].transform.parent = this.ListaOrdigniLocali[j * 2].transform;
						this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[num].transform.localPosition = this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().ListaPosizioniMunizioni[num];
						num++;
					}
				}
				if (j * 2 + 1 == 1 || j * 2 + 1 == 3 || j * 2 + 1 == 5 || j * 2 + 1 == 7)
				{
					int num2 = 0;
					while ((float)num2 < base.GetComponent<PresenzaAlleato>().ListaArmi[j][5] / 2f)
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
				int num3 = Mathf.RoundToInt(base.GetComponent<PresenzaAlleato>().ListaArmi[j][5] / 2f);
				if (j * 2 == 0 || j * 2 == 2 || j * 2 == 4 || j * 2 == 4)
				{
					for (int k = 0; k < num3 + 1; k++)
					{
						this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k] = (UnityEngine.Object.Instantiate(this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().munizioneUsata, base.transform.position, base.transform.rotation) as GameObject);
						this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k].transform.parent = this.ListaOrdigniLocali[j * 2].transform;
						this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[k].transform.localPosition = this.ListaOrdigniLocali[j * 2].GetComponent<DatiOrdignoEsterno>().ListaPosizioniMunizioni[k];
					}
				}
				if (j * 2 + 1 == 1 || j * 2 + 1 == 3 || j * 2 + 1 == 5 || j * 2 + 1 == 7)
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

	// Token: 0x0600012C RID: 300 RVA: 0x000363EC File Offset: 0x000345EC
	private void CreazioneOrdigniConRifornimento()
	{
		if (base.GetComponent<PresenzaAlleato>().reintegrazioneNecessaria)
		{
			for (int i = 0; i < base.GetComponent<PresenzaAlleato>().numeroCoppieOrdigni; i++)
			{
				for (int j = 0; j < base.GetComponent<PresenzaAlleato>().ListaNumReintegrazioniOrdigni[i]; j++)
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

	// Token: 0x0600012D RID: 301 RVA: 0x000366DC File Offset: 0x000348DC
	private void CondizioniArma1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[0][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[0][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[0][6];
		}
	}

	// Token: 0x0600012E RID: 302 RVA: 0x0003674C File Offset: 0x0003494C
	private void CondizioniArma2()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[1][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[1][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[1][6];
		}
	}

	// Token: 0x0600012F RID: 303 RVA: 0x000367BC File Offset: 0x000349BC
	private void CondizioniArma3()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[2][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[2][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[2][6];
		}
	}

	// Token: 0x06000130 RID: 304 RVA: 0x0003682C File Offset: 0x00034A2C
	private void CondizioniArma4()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[3][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[3][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[3][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[3][6];
		}
	}

	// Token: 0x06000131 RID: 305 RVA: 0x0003689C File Offset: 0x00034A9C
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

	// Token: 0x06000132 RID: 306 RVA: 0x00036B04 File Offset: 0x00034D04
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

	// Token: 0x06000133 RID: 307 RVA: 0x00036B8C File Offset: 0x00034D8C
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

	// Token: 0x06000134 RID: 308 RVA: 0x00036C14 File Offset: 0x00034E14
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

	// Token: 0x06000135 RID: 309 RVA: 0x00036C9C File Offset: 0x00034E9C
	private void Mirini()
	{
		this.mirinoFissoVelivoli.GetComponent<CanvasGroup>().alpha = 1f;
		this.mirinoInfoVelivoli.GetComponent<CanvasGroup>().alpha = 1f;
		this.livelloSuolo.GetComponent<CanvasGroup>().alpha = 1f;
		this.mirinoBombe.GetComponent<CanvasGroup>().alpha = 1f;
		this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 0f;
		this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 0f;
		this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 0f;
		if (this.ordignoDaLanciare && this.ordignoDaLanciare.transform.parent && (!Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyUp(KeyCode.Space)))
		{
			this.SistemaDiLancioInPrimaPersona();
		}
	}

	// Token: 0x06000136 RID: 310 RVA: 0x00036D90 File Offset: 0x00034F90
	private void StrumentazioneMirini()
	{
		float frontaleVelocitàMax = base.GetComponent<MOV_StrategicBomber>().frontaleVelocitàMax;
		float num = base.GetComponent<MOV_StrategicBomber>().velocitàFrontaleEffettiva / base.GetComponent<MOV_StrategicBomber>().frontaleVelocitàMax * 100f;
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

	// Token: 0x06000137 RID: 311 RVA: 0x00036ED0 File Offset: 0x000350D0
	private void PreparazioneAttacco()
	{
		bool flag = false;
		if (this.unitàBersaglio)
		{
			flag = Physics.Linecast(base.transform.position, this.centroUnitàBersaglio, this.layerVisuale);
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
				base.GetComponent<MOV_AUTOM_StrategicBomber>().ripetitoreDiAttaccoOrdinato = true;
				base.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
				this.unitàBersaglio = this.primaCamera.GetComponent<Selezionamento>().oggettoBersaglio;
				if (this.unitàBersaglio && !flag && !this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
				{
					for (int i = 0; i < base.GetComponent<PresenzaAlleato>().numeroArmi; i++)
					{
						if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[i] && num2 < this.distanzaDiSgancio && i >= 0 && !this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
						{
							if (this.ListaOrdigniLocali[i * 2].transform.childCount > 1)
							{
								if (this.ListaOrdigniLocali[i * 2].transform.GetChild(1) != null)
								{
									this.ordignoDaLanciare = this.ListaOrdigniLocali[i * 2].transform.GetChild(1).gameObject;
									this.numArmaOrdignoDaLanciare = i;
									this.ListaSparoConDestra[i] = true;
									this.AttaccoIndipendenteOrdigni();
									break;
								}
							}
							else if (this.ListaOrdigniLocali[i * 2 + 1].transform.childCount > 1 && this.ListaOrdigniLocali[i * 2 + 1].transform.GetChild(1) != null)
							{
								this.ordignoDaLanciare = this.ListaOrdigniLocali[i * 2 + 1].transform.GetChild(1).gameObject;
								this.numArmaOrdignoDaLanciare = i;
								this.ListaSparoConDestra[i] = true;
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
				for (int j = 0; j < base.GetComponent<PresenzaAlleato>().numeroArmi; j++)
				{
					if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[j] && num2 < this.distanzaDiSgancio && j >= 0 && !this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
					{
						if (this.ListaOrdigniLocali[j * 2].transform.childCount > 1)
						{
							if (this.ListaOrdigniLocali[j * 2].transform.GetChild(1) != null)
							{
								this.ordignoDaLanciare = this.ListaOrdigniLocali[j * 2].transform.GetChild(1).gameObject;
								this.numArmaOrdignoDaLanciare = j;
								this.ListaSparoConDestra[j] = true;
								this.AttaccoIndipendenteOrdigni();
								break;
							}
						}
						else if (this.ListaOrdigniLocali[j * 2 + 1].transform.childCount > 1 && this.ListaOrdigniLocali[j * 2 + 1].transform.GetChild(1) != null)
						{
							this.ordignoDaLanciare = this.ListaOrdigniLocali[j * 2 + 1].transform.GetChild(1).gameObject;
							this.numArmaOrdignoDaLanciare = j;
							this.ListaSparoConDestra[j] = true;
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
								float num3 = Vector3.Distance(base.transform.position, current.GetComponent<PresenzaNemico>().centroInsetto);
								if (num3 < 500f && !Physics.Linecast(base.transform.position, current.GetComponent<PresenzaNemico>().centroInsetto, this.layerVisuale) && current.transform.position.y < base.transform.position.y - 20f)
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
								float num4 = Vector3.Distance(base.transform.position, current2.GetComponent<PresenzaNemico>().centroInsetto);
								if (num4 < 500f && !Physics.Linecast(base.transform.position, current2.GetComponent<PresenzaNemico>().centroInsetto, this.layerVisuale))
								{
									float num5 = Vector3.Dot((current2.GetComponent<PresenzaNemico>().centroInsetto - base.transform.position).normalized, base.transform.up);
									if (current2.transform.position.y < base.transform.position.y - 20f)
									{
										list2.Add(current2);
									}
								}
							}
						}
						if (list2.Count > 0)
						{
							float num6 = 9999f;
							for (int k = 0; k < list2.Count; k++)
							{
								float num7 = Vector3.Distance(base.transform.position, list2[k].GetComponent<PresenzaNemico>().centroInsetto);
								if (num7 < num6)
								{
									num6 = num7;
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
			float num8 = Vector3.Distance(base.GetComponent<PresenzaAlleato>().luogoAttZonaBomb, b2);
			for (int l = 0; l < base.GetComponent<PresenzaAlleato>().numeroArmi; l++)
			{
				if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[l] && num8 < this.distanzaDiSgancio && l >= 0)
				{
					if (this.ListaOrdigniLocali[l * 2].transform.childCount > 1)
					{
						if (this.ListaOrdigniLocali[l * 2].transform.GetChild(1) != null)
						{
							this.ordignoDaLanciare = this.ListaOrdigniLocali[l * 2].transform.GetChild(1).gameObject;
							this.numArmaOrdignoDaLanciare = l;
							this.ListaSparoConDestra[l] = true;
							this.AttaccoIndipendenteOrdigni();
							break;
						}
					}
					else if (this.ListaOrdigniLocali[l * 2 + 1].transform.childCount > 1 && this.ListaOrdigniLocali[l * 2 + 1].transform.GetChild(1) != null)
					{
						this.ordignoDaLanciare = this.ListaOrdigniLocali[l * 2 + 1].transform.GetChild(1).gameObject;
						this.numArmaOrdignoDaLanciare = l;
						this.ListaSparoConDestra[l] = true;
						this.AttaccoIndipendenteOrdigni();
						break;
					}
				}
			}
		}
	}

	// Token: 0x06000138 RID: 312 RVA: 0x00037A44 File Offset: 0x00035C44
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

	// Token: 0x06000139 RID: 313 RVA: 0x00037B9C File Offset: 0x00035D9C
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

	// Token: 0x0600013A RID: 314 RVA: 0x00037C0C File Offset: 0x00035E0C
	private void GestioneOrdigniPrimaPersona()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.gruppo01Attivo = true;
			this.gruppo23Attivo = false;
			this.gruppo45Attivo = false;
			this.gruppo67Attivo = false;
			this.ordignoDaLanciare = null;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.gruppo01Attivo = false;
			this.gruppo23Attivo = true;
			this.gruppo45Attivo = false;
			this.gruppo67Attivo = false;
			this.ordignoDaLanciare = null;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			this.gruppo01Attivo = false;
			this.gruppo23Attivo = false;
			this.gruppo45Attivo = true;
			this.gruppo67Attivo = false;
			this.ordignoDaLanciare = null;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			this.gruppo01Attivo = false;
			this.gruppo23Attivo = false;
			this.gruppo45Attivo = false;
			this.gruppo67Attivo = true;
			this.ordignoDaLanciare = null;
		}
		this.ListaGruppiOrdigniAttivi.Clear();
		this.ListaGruppiOrdigniAttivi.Add(this.gruppo01Attivo);
		this.ListaGruppiOrdigniAttivi.Add(this.gruppo23Attivo);
		this.ListaGruppiOrdigniAttivi.Add(this.gruppo45Attivo);
		this.ListaGruppiOrdigniAttivi.Add(this.gruppo67Attivo);
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
							this.numArmaOrdignoDaLanciare = i;
							this.ListaSparoConDestra[i] = true;
							break;
						}
					}
					else if (this.ListaOrdigniLocali[i * 2 + 1].transform.childCount > 1 && this.ListaOrdigniLocali[i * 2 + 1].transform.GetChild(1) != null)
					{
						this.ordignoDaLanciare = this.ListaOrdigniLocali[i * 2 + 1].transform.GetChild(1).gameObject;
						this.numArmaOrdignoDaLanciare = i;
						this.ListaSparoConDestra[i] = false;
						break;
					}
					if (this.ListaOrdigniLocali[i * 2].transform.childCount > 1)
					{
						this.ordignoDaLanciare = this.ListaOrdigniLocali[i * 2].transform.GetChild(1).gameObject;
						this.numArmaOrdignoDaLanciare = i;
					}
					else if (this.ListaOrdigniLocali[i * 2 + 1].transform.childCount > 1)
					{
						this.ordignoDaLanciare = this.ListaOrdigniLocali[i * 2 + 1].transform.GetChild(1).gameObject;
						this.numArmaOrdignoDaLanciare = i;
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

	// Token: 0x0600013B RID: 315 RVA: 0x00037F3C File Offset: 0x0003613C
	private void SistemaDiLancioInPrimaPersona()
	{
		this.ListaBersPPPossibili.Clear();
		float num = (float)(Screen.width / 13);
		float num2 = Vector3.Dot(base.transform.up, Vector3.up);
		if (this.ordignoDaLanciare && Input.GetMouseButton(0) && num2 > 0.2f && base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][5] > 0f && this.timerDiLancio > base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][1])
		{
			this.timerDiLancio = 0f;
			List<float> list;
			List<float> expr_BF = list = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
			int index;
			int expr_C2 = index = 5;
			float num3 = list[index];
			expr_BF[expr_C2] = num3 - 1f;
			List<float> list2;
			List<float> expr_F2 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
			int expr_F6 = index = 6;
			num3 = list2[index];
			expr_F2[expr_F6] = num3 - 1f;
			for (int i = 0; i < this.ListaOrdigniLocali.Count; i++)
			{
				this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ordignoAttivo = false;
			}
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().ordignoAttivo = true;
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = true;
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().velocitàDelVelivAlLancio = base.GetComponent<MOV_StrategicBomber>().velocitàFrontaleEffettiva;
			this.ordignoDaLanciare = null;
		}
	}

	// Token: 0x0600013C RID: 316 RVA: 0x000380F0 File Offset: 0x000362F0
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

	// Token: 0x040005AD RID: 1453
	public float distanzaDiSgancio;

	// Token: 0x040005AE RID: 1454
	private GameObject infoNeutreTattica;

	// Token: 0x040005AF RID: 1455
	private GameObject primaCamera;

	// Token: 0x040005B0 RID: 1456
	private GameObject terzaCamera;

	// Token: 0x040005B1 RID: 1457
	private GameObject IANemico;

	// Token: 0x040005B2 RID: 1458
	private GameObject InfoAlleati;

	// Token: 0x040005B3 RID: 1459
	private float timerFrequenzaArma1;

	// Token: 0x040005B4 RID: 1460
	private float timerRicarica1;

	// Token: 0x040005B5 RID: 1461
	private bool ricaricaInCorso1;

	// Token: 0x040005B6 RID: 1462
	private float timerDiLancio;

	// Token: 0x040005B7 RID: 1463
	private int layerColpo;

	// Token: 0x040005B8 RID: 1464
	private int layerVisuale;

	// Token: 0x040005B9 RID: 1465
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x040005BA RID: 1466
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x040005BB RID: 1467
	public Vector3 posizionamentoCameraBombardamento;

	// Token: 0x040005BC RID: 1468
	private float timerPosizionamentoTPS;

	// Token: 0x040005BD RID: 1469
	private float timerPosizionamentoFPS;

	// Token: 0x040005BE RID: 1470
	private float timerPosizionamentoVisuBomb;

	// Token: 0x040005BF RID: 1471
	public bool visualeBombAttiva;

	// Token: 0x040005C0 RID: 1472
	public Vector3 posCameraPsteriore;

	// Token: 0x040005C1 RID: 1473
	private GameObject CanvasFPS;

	// Token: 0x040005C2 RID: 1474
	private GameObject sensoreRaggioMirino;

	// Token: 0x040005C3 RID: 1475
	private GameObject sensoreRaggioMirinoMobile;

	// Token: 0x040005C4 RID: 1476
	private GameObject mirinoFissoVelivoli;

	// Token: 0x040005C5 RID: 1477
	private GameObject mirinoMissiliFisso;

	// Token: 0x040005C6 RID: 1478
	private GameObject mirinoMissiliMobile;

	// Token: 0x040005C7 RID: 1479
	private Color coloreBaseMirini;

	// Token: 0x040005C8 RID: 1480
	private GameObject mirinoMissiliFiloguidati;

	// Token: 0x040005C9 RID: 1481
	private GameObject mirinoBombe;

	// Token: 0x040005CA RID: 1482
	private GameObject mirinoInfoVelivoli;

	// Token: 0x040005CB RID: 1483
	private GameObject livelloSuolo;

	// Token: 0x040005CC RID: 1484
	private GameObject angCameraBomb;

	// Token: 0x040005CD RID: 1485
	private RaycastHit targetSparo;

	// Token: 0x040005CE RID: 1486
	private NavMeshAgent alleatoNav;

	// Token: 0x040005CF RID: 1487
	private float velocitàAlleatoNav;

	// Token: 0x040005D0 RID: 1488
	public GameObject unitàBersaglio;

	// Token: 0x040005D1 RID: 1489
	private Vector3 centroUnitàBersaglio;

	// Token: 0x040005D2 RID: 1490
	private GameObject munizioneArma1;

	// Token: 0x040005D3 RID: 1491
	private GameObject munizioneArma2;

	// Token: 0x040005D4 RID: 1492
	private GameObject munizioneArma3;

	// Token: 0x040005D5 RID: 1493
	private GameObject munizioneArma4;

	// Token: 0x040005D6 RID: 1494
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x040005D7 RID: 1495
	private List<GameObject> ListaOrdigniLocali;

	// Token: 0x040005D8 RID: 1496
	public GameObject ordignoDaLanciare;

	// Token: 0x040005D9 RID: 1497
	private int numArmaOrdignoDaLanciare;

	// Token: 0x040005DA RID: 1498
	private GameObject ordigno0;

	// Token: 0x040005DB RID: 1499
	private GameObject ordigno1;

	// Token: 0x040005DC RID: 1500
	private GameObject ordigno2;

	// Token: 0x040005DD RID: 1501
	private GameObject ordigno3;

	// Token: 0x040005DE RID: 1502
	private GameObject ordigno4;

	// Token: 0x040005DF RID: 1503
	private GameObject ordigno5;

	// Token: 0x040005E0 RID: 1504
	private GameObject ordigno6;

	// Token: 0x040005E1 RID: 1505
	private GameObject ordigno7;

	// Token: 0x040005E2 RID: 1506
	public Vector3 posizioneOrdigni01;

	// Token: 0x040005E3 RID: 1507
	public Vector3 posizioneOrdigni23;

	// Token: 0x040005E4 RID: 1508
	public Vector3 posizioneOrdigni45;

	// Token: 0x040005E5 RID: 1509
	public Vector3 posizioneOrdigni67;

	// Token: 0x040005E6 RID: 1510
	private bool gruppo01Attivo;

	// Token: 0x040005E7 RID: 1511
	private bool gruppo23Attivo;

	// Token: 0x040005E8 RID: 1512
	private bool gruppo45Attivo;

	// Token: 0x040005E9 RID: 1513
	private bool gruppo67Attivo;

	// Token: 0x040005EA RID: 1514
	public List<GameObject> ListaOrdigniAttiviLocale;

	// Token: 0x040005EB RID: 1515
	private List<bool> ListaGruppiOrdigniAttivi;

	// Token: 0x040005EC RID: 1516
	private bool sparoConDestra01;

	// Token: 0x040005ED RID: 1517
	private bool sparoConDestra23;

	// Token: 0x040005EE RID: 1518
	private bool sparoConDestra45;

	// Token: 0x040005EF RID: 1519
	private bool sparoConDestra67;

	// Token: 0x040005F0 RID: 1520
	private List<bool> ListaSparoConDestra;

	// Token: 0x040005F1 RID: 1521
	private bool primoFrameAvvenuto;

	// Token: 0x040005F2 RID: 1522
	public List<GameObject> ListaBersPPPossibili;

	// Token: 0x040005F3 RID: 1523
	public GameObject bersaglioInPP;

	// Token: 0x040005F4 RID: 1524
	private bool bersaglioèAvantiInPP;

	// Token: 0x040005F5 RID: 1525
	private bool bersDavantiEAPortata;

	// Token: 0x040005F6 RID: 1526
	private float volumeBaseEsterno;

	// Token: 0x040005F7 RID: 1527
	private GameObject ordignoFittizio;

	// Token: 0x040005F8 RID: 1528
	private bool bersèDavanti;

	// Token: 0x040005F9 RID: 1529
	private bool bersèNelMirino;

	// Token: 0x040005FA RID: 1530
	private GameObject supportoMissilePassivoInVolo;

	// Token: 0x040005FB RID: 1531
	private AudioSource suonoMotore;

	// Token: 0x040005FC RID: 1532
	private Text testoPotenzaMotore;

	// Token: 0x040005FD RID: 1533
	private Text testoAltitudine;

	// Token: 0x040005FE RID: 1534
	private float timerPresenzaInAereo;

	// Token: 0x040005FF RID: 1535
	private float timerAggRicerca;

	// Token: 0x04000600 RID: 1536
	private float timerSenzaArmiPrimarie;
}
