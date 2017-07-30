using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000018 RID: 24
public class ATT_LongRangeBomber : MonoBehaviour
{
	// Token: 0x060000EF RID: 239 RVA: 0x0002B2CC File Offset: 0x000294CC
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

	// Token: 0x060000F0 RID: 240 RVA: 0x0002B610 File Offset: 0x00029810
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
				base.GetComponent<MOV_LongRangeBomber>().velocitàFrontaleEffettiva = 20f;
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

	// Token: 0x060000F1 RID: 241 RVA: 0x0002BD4C File Offset: 0x00029F4C
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
				if (j * 2 == 0 || j * 2 == 2 || j * 2 == 4 || j * 2 == 6)
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

	// Token: 0x060000F2 RID: 242 RVA: 0x0002C6F8 File Offset: 0x0002A8F8
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

	// Token: 0x060000F3 RID: 243 RVA: 0x0002C9E8 File Offset: 0x0002ABE8
	private void CondizioniArma1()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[0][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[0][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[0][6];
		}
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x0002CA58 File Offset: 0x0002AC58
	private void CondizioniArma2()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[1][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[1][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[1][6];
		}
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x0002CAC8 File Offset: 0x0002ACC8
	private void CondizioniArma3()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[2][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[2][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[2][6];
		}
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x0002CB38 File Offset: 0x0002AD38
	private void CondizioniArma4()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[3][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[3][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[3][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[3][6];
		}
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x0002CBA8 File Offset: 0x0002ADA8
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

	// Token: 0x060000F8 RID: 248 RVA: 0x0002CE10 File Offset: 0x0002B010
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

	// Token: 0x060000F9 RID: 249 RVA: 0x0002CE98 File Offset: 0x0002B098
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

	// Token: 0x060000FA RID: 250 RVA: 0x0002CF20 File Offset: 0x0002B120
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

	// Token: 0x060000FB RID: 251 RVA: 0x0002CFA8 File Offset: 0x0002B1A8
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

	// Token: 0x060000FC RID: 252 RVA: 0x0002D09C File Offset: 0x0002B29C
	private void StrumentazioneMirini()
	{
		float frontaleVelocitàMax = base.GetComponent<MOV_LongRangeBomber>().frontaleVelocitàMax;
		float num = base.GetComponent<MOV_LongRangeBomber>().velocitàFrontaleEffettiva / base.GetComponent<MOV_LongRangeBomber>().frontaleVelocitàMax * 100f;
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

	// Token: 0x060000FD RID: 253 RVA: 0x0002D1DC File Offset: 0x0002B3DC
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
				base.GetComponent<MOV_AUTOM_LongRangeBomber>().ripetitoreDiAttaccoOrdinato = true;
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

	// Token: 0x060000FE RID: 254 RVA: 0x0002DD50 File Offset: 0x0002BF50
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

	// Token: 0x060000FF RID: 255 RVA: 0x0002DEA8 File Offset: 0x0002C0A8
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

	// Token: 0x06000100 RID: 256 RVA: 0x0002DF18 File Offset: 0x0002C118
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

	// Token: 0x06000101 RID: 257 RVA: 0x0002E248 File Offset: 0x0002C448
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
			this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().velocitàDelVelivAlLancio = base.GetComponent<MOV_LongRangeBomber>().velocitàFrontaleEffettiva;
			this.ordignoDaLanciare = null;
		}
	}

	// Token: 0x06000102 RID: 258 RVA: 0x0002E3FC File Offset: 0x0002C5FC
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

	// Token: 0x04000494 RID: 1172
	public float distanzaDiSgancio;

	// Token: 0x04000495 RID: 1173
	private GameObject infoNeutreTattica;

	// Token: 0x04000496 RID: 1174
	private GameObject primaCamera;

	// Token: 0x04000497 RID: 1175
	private GameObject terzaCamera;

	// Token: 0x04000498 RID: 1176
	private GameObject IANemico;

	// Token: 0x04000499 RID: 1177
	private GameObject InfoAlleati;

	// Token: 0x0400049A RID: 1178
	private float timerFrequenzaArma1;

	// Token: 0x0400049B RID: 1179
	private float timerRicarica1;

	// Token: 0x0400049C RID: 1180
	private bool ricaricaInCorso1;

	// Token: 0x0400049D RID: 1181
	private float timerDiLancio;

	// Token: 0x0400049E RID: 1182
	private int layerColpo;

	// Token: 0x0400049F RID: 1183
	private int layerVisuale;

	// Token: 0x040004A0 RID: 1184
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x040004A1 RID: 1185
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x040004A2 RID: 1186
	public Vector3 posizionamentoCameraBombardamento;

	// Token: 0x040004A3 RID: 1187
	private float timerPosizionamentoTPS;

	// Token: 0x040004A4 RID: 1188
	private float timerPosizionamentoFPS;

	// Token: 0x040004A5 RID: 1189
	private float timerPosizionamentoVisuBomb;

	// Token: 0x040004A6 RID: 1190
	public bool visualeBombAttiva;

	// Token: 0x040004A7 RID: 1191
	public Vector3 posCameraPsteriore;

	// Token: 0x040004A8 RID: 1192
	private GameObject CanvasFPS;

	// Token: 0x040004A9 RID: 1193
	private GameObject sensoreRaggioMirino;

	// Token: 0x040004AA RID: 1194
	private GameObject sensoreRaggioMirinoMobile;

	// Token: 0x040004AB RID: 1195
	private GameObject mirinoFissoVelivoli;

	// Token: 0x040004AC RID: 1196
	private GameObject mirinoMissiliFisso;

	// Token: 0x040004AD RID: 1197
	private GameObject mirinoMissiliMobile;

	// Token: 0x040004AE RID: 1198
	private Color coloreBaseMirini;

	// Token: 0x040004AF RID: 1199
	private GameObject mirinoMissiliFiloguidati;

	// Token: 0x040004B0 RID: 1200
	private GameObject mirinoBombe;

	// Token: 0x040004B1 RID: 1201
	private GameObject mirinoInfoVelivoli;

	// Token: 0x040004B2 RID: 1202
	private GameObject livelloSuolo;

	// Token: 0x040004B3 RID: 1203
	private GameObject angCameraBomb;

	// Token: 0x040004B4 RID: 1204
	private RaycastHit targetSparo;

	// Token: 0x040004B5 RID: 1205
	private NavMeshAgent alleatoNav;

	// Token: 0x040004B6 RID: 1206
	private float velocitàAlleatoNav;

	// Token: 0x040004B7 RID: 1207
	public GameObject unitàBersaglio;

	// Token: 0x040004B8 RID: 1208
	private Vector3 centroUnitàBersaglio;

	// Token: 0x040004B9 RID: 1209
	private GameObject munizioneArma1;

	// Token: 0x040004BA RID: 1210
	private GameObject munizioneArma2;

	// Token: 0x040004BB RID: 1211
	private GameObject munizioneArma3;

	// Token: 0x040004BC RID: 1212
	private GameObject munizioneArma4;

	// Token: 0x040004BD RID: 1213
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x040004BE RID: 1214
	private List<GameObject> ListaOrdigniLocali;

	// Token: 0x040004BF RID: 1215
	public GameObject ordignoDaLanciare;

	// Token: 0x040004C0 RID: 1216
	private int numArmaOrdignoDaLanciare;

	// Token: 0x040004C1 RID: 1217
	private GameObject ordigno0;

	// Token: 0x040004C2 RID: 1218
	private GameObject ordigno1;

	// Token: 0x040004C3 RID: 1219
	private GameObject ordigno2;

	// Token: 0x040004C4 RID: 1220
	private GameObject ordigno3;

	// Token: 0x040004C5 RID: 1221
	private GameObject ordigno4;

	// Token: 0x040004C6 RID: 1222
	private GameObject ordigno5;

	// Token: 0x040004C7 RID: 1223
	private GameObject ordigno6;

	// Token: 0x040004C8 RID: 1224
	private GameObject ordigno7;

	// Token: 0x040004C9 RID: 1225
	public Vector3 posizioneOrdigni01;

	// Token: 0x040004CA RID: 1226
	public Vector3 posizioneOrdigni23;

	// Token: 0x040004CB RID: 1227
	public Vector3 posizioneOrdigni45;

	// Token: 0x040004CC RID: 1228
	public Vector3 posizioneOrdigni67;

	// Token: 0x040004CD RID: 1229
	private bool gruppo01Attivo;

	// Token: 0x040004CE RID: 1230
	private bool gruppo23Attivo;

	// Token: 0x040004CF RID: 1231
	private bool gruppo45Attivo;

	// Token: 0x040004D0 RID: 1232
	private bool gruppo67Attivo;

	// Token: 0x040004D1 RID: 1233
	public List<GameObject> ListaOrdigniAttiviLocale;

	// Token: 0x040004D2 RID: 1234
	private List<bool> ListaGruppiOrdigniAttivi;

	// Token: 0x040004D3 RID: 1235
	private bool sparoConDestra01;

	// Token: 0x040004D4 RID: 1236
	private bool sparoConDestra23;

	// Token: 0x040004D5 RID: 1237
	private bool sparoConDestra45;

	// Token: 0x040004D6 RID: 1238
	private bool sparoConDestra67;

	// Token: 0x040004D7 RID: 1239
	private List<bool> ListaSparoConDestra;

	// Token: 0x040004D8 RID: 1240
	private bool primoFrameAvvenuto;

	// Token: 0x040004D9 RID: 1241
	public List<GameObject> ListaBersPPPossibili;

	// Token: 0x040004DA RID: 1242
	public GameObject bersaglioInPP;

	// Token: 0x040004DB RID: 1243
	private bool bersaglioèAvantiInPP;

	// Token: 0x040004DC RID: 1244
	private bool bersDavantiEAPortata;

	// Token: 0x040004DD RID: 1245
	private float volumeBaseEsterno;

	// Token: 0x040004DE RID: 1246
	private GameObject ordignoFittizio;

	// Token: 0x040004DF RID: 1247
	private bool bersèDavanti;

	// Token: 0x040004E0 RID: 1248
	private bool bersèNelMirino;

	// Token: 0x040004E1 RID: 1249
	private GameObject supportoMissilePassivoInVolo;

	// Token: 0x040004E2 RID: 1250
	private AudioSource suonoMotore;

	// Token: 0x040004E3 RID: 1251
	private Text testoPotenzaMotore;

	// Token: 0x040004E4 RID: 1252
	private Text testoAltitudine;

	// Token: 0x040004E5 RID: 1253
	private float timerPresenzaInAereo;

	// Token: 0x040004E6 RID: 1254
	private float timerAggRicerca;

	// Token: 0x040004E7 RID: 1255
	private float timerSenzaArmiPrimarie;
}
