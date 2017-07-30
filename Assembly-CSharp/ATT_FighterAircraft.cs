using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000014 RID: 20
public class ATT_FighterAircraft : MonoBehaviour
{
	// Token: 0x06000093 RID: 147 RVA: 0x0001B778 File Offset: 0x00019978
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
		if (this.InfoAlleati.GetComponent<InfoGenericheAlleati>().tipoBattaglia == 5)
		{
			base.GetComponent<PresenzaAlleato>().raggioVisivo = 9999f;
			base.GetComponent<PresenzaAlleato>().carburante = 500f;
			base.GetComponent<PresenzaAlleato>().carburanteIniziale = base.GetComponent<PresenzaAlleato>().carburante;
		}
	}

	// Token: 0x06000094 RID: 148 RVA: 0x0001BB20 File Offset: 0x00019D20
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
			this.primoFrameAvvenuto = true;
		}
		this.unitàBersaglio = base.GetComponent<PresenzaAlleato>().unitàBersaglio;
		this.ListaMunizioniAttiveUnità[0] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[0][0];
		this.ListaMunizioniAttiveUnità[1] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[1][0];
		this.ListaMunizioniAttiveUnità[2] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[2][0];
		this.ListaMunizioniAttiveUnità[3] = base.GetComponent<PresenzaAlleato>().ListaMunizioneArmi[3][0];
		this.munizioneArma1 = base.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[0];
		this.timerFrequenzaArma1 += Time.deltaTime;
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
				base.GetComponent<MOV_FighterAircraft>().velocitàFrontaleEffettiva = 60f;
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
				this.ordignoDaLanciare = null;
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
					this.cannoneStaSparando = false;
					this.suonoBocca1.Stop();
					this.particelleBocca1.Stop();
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					base.gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
					base.gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
					base.gameObject.transform.GetChild(2).GetComponent<AudioSource>().Stop();
					this.suonoMotore.volume = this.volumeBaseEsterno;
					this.cannoneStaSparando = false;
					this.suonoBocca1.Stop();
					this.particelleBocca1.Stop();
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
			this.audioBeepCortoAttivo = false;
			this.audioBeepLungoAttivo = false;
			this.suonoBeep.Stop();
			this.timerDiAggancio = 0f;
			this.mirinoMissiliMobile.GetComponent<Image>().color = this.coloreBaseMirini;
			this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
			this.timerPresenzaInAereo = 0f;
		}
		base.GetComponent<PresenzaAlleato>().unitàBersaglio = this.unitàBersaglio;
	}

	// Token: 0x06000095 RID: 149 RVA: 0x0001C1E4 File Offset: 0x0001A3E4
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

	// Token: 0x06000096 RID: 150 RVA: 0x0001CA68 File Offset: 0x0001AC68
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

	// Token: 0x06000097 RID: 151 RVA: 0x0001CD5C File Offset: 0x0001AF5C
	private void CondizioniArma1()
	{
		base.GetComponent<PresenzaAlleato>().ListaArmi[0][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[0][6];
	}

	// Token: 0x06000098 RID: 152 RVA: 0x0001CD98 File Offset: 0x0001AF98
	private void CondizioniArma2()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[1][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[1][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[1][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[1][6];
		}
	}

	// Token: 0x06000099 RID: 153 RVA: 0x0001CE08 File Offset: 0x0001B008
	private void CondizioniArma3()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[2][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[2][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[2][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[2][6];
		}
	}

	// Token: 0x0600009A RID: 154 RVA: 0x0001CE78 File Offset: 0x0001B078
	private void CondizioniArma4()
	{
		if (base.GetComponent<PresenzaAlleato>().ListaArmi[3][6] != base.GetComponent<PresenzaAlleato>().ListaArmi[3][5])
		{
			base.GetComponent<PresenzaAlleato>().ListaArmi[3][5] = base.GetComponent<PresenzaAlleato>().ListaArmi[3][6];
		}
	}

	// Token: 0x0600009B RID: 155 RVA: 0x0001CEE8 File Offset: 0x0001B0E8
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
	}

	// Token: 0x0600009C RID: 156 RVA: 0x0001CF44 File Offset: 0x0001B144
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

	// Token: 0x0600009D RID: 157 RVA: 0x0001CFCC File Offset: 0x0001B1CC
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

	// Token: 0x0600009E RID: 158 RVA: 0x0001D054 File Offset: 0x0001B254
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
				if (!Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyUp(KeyCode.Space))
				{
					this.SistemaDiLancioInPrimaPersona();
				}
			}
			else if (this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 5 || this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 6)
			{
				this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 1f;
				this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 1f;
				this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 0f;
				if (!Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyUp(KeyCode.Space))
				{
					this.SistemaDiLancioInPrimaPersona();
				}
			}
		}
		if (!this.ordignoDaLanciare || !this.ordignoDaLanciare.transform.parent)
		{
			this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 0f;
		}
	}

	// Token: 0x0600009F RID: 159 RVA: 0x0001D2EC File Offset: 0x0001B4EC
	private void StrumentazioneMirini()
	{
		float frontaleVelocitàMax = base.GetComponent<MOV_FighterAircraft>().frontaleVelocitàMax;
		float num = base.GetComponent<MOV_FighterAircraft>().velocitàFrontaleEffettiva / base.GetComponent<MOV_FighterAircraft>().frontaleVelocitàMax * 100f;
		this.testoPotenzaMotore.text = num.ToString("F0") + "%";
		this.testoAltitudine.text = base.transform.position.y.ToString("F0");
		this.livelloSuolo.transform.eulerAngles = new Vector3(0f, 0f, -base.transform.eulerAngles.z);
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x0001D3A0 File Offset: 0x0001B5A0
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
			if (base.GetComponent<PresenzaAlleato>().attaccoOrdinato)
			{
				base.GetComponent<MOV_AUTOM_FighterAircraft>().ripetitoreDiAttaccoOrdinato = true;
				base.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
				this.unitàBersaglio = this.primaCamera.GetComponent<Selezionamento>().oggettoBersaglio;
				if (this.unitàBersaglio && !flag && this.bersèNelMirino && this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
				{
					float num3 = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
					for (int j = 0; j < base.GetComponent<PresenzaAlleato>().numeroArmi; j++)
					{
						if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[j] && num3 < this.ListaMunizioniAttiveUnità[j].GetComponent<DatiGeneraliMunizione>().portataMassima)
						{
							if (j == 0)
							{
								this.AttaccoIndipendente1();
							}
							else if (j >= 1 && this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
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
					if (this.unitàBersaglio.GetComponent<PresenzaNemico>().vita <= 0f)
					{
						base.GetComponent<PresenzaAlleato>().attaccoOrdinato = false;
					}
				}
			}
			else if (this.unitàBersaglio && !flag && this.bersèNelMirino && this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
			{
				float num4 = Vector3.Distance(base.transform.position, this.centroUnitàBersaglio);
				for (int k = 0; k < base.GetComponent<PresenzaAlleato>().numeroArmi; k++)
				{
					if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[k] && num4 < this.ListaMunizioniAttiveUnità[k].GetComponent<DatiGeneraliMunizione>().portataMassima)
					{
						if (k == 0)
						{
							this.AttaccoIndipendente1();
						}
						else if (k >= 1 && this.unitàBersaglio.GetComponent<PresenzaNemico>().insettoVolante)
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
								float num5 = Vector3.Distance(base.transform.position, current.GetComponent<PresenzaNemico>().centroInsetto);
								if (num5 < num2)
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
								float num6 = Vector3.Distance(base.transform.position, current2.GetComponent<PresenzaNemico>().centroInsetto);
								if (num6 < num2 && num6 < num2)
								{
									list2.Add(current2);
								}
							}
						}
						if (list2.Count > 0)
						{
							GestoreNeutroStrategia.valoreRandomSeed++;
							UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
							float f2 = UnityEngine.Random.Range(0f, (float)list2.Count - 0.01f);
							this.unitàBersaglio = list2[Mathf.FloorToInt(f2)];
						}
					}
				}
			}
			else if (base.GetComponent<PresenzaAlleato>().ricercaAutomaticaBersaglio && this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciVolanti.Count > 0)
			{
				GestoreNeutroStrategia.valoreRandomSeed++;
				UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
				float f3 = UnityEngine.Random.Range(0f, (float)this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciVolanti.Count - 0.01f);
				this.unitàBersaglio = this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciVolanti[Mathf.FloorToInt(f3)];
			}
		}
		else
		{
			this.unitàBersaglio = null;
		}
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x0001DEA4 File Offset: 0x0001C0A4
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

	// Token: 0x060000A2 RID: 162 RVA: 0x0001E038 File Offset: 0x0001C238
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

	// Token: 0x060000A3 RID: 163 RVA: 0x0001E190 File Offset: 0x0001C390
	private void SelezioneArma()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 0;
			this.audioBeepCortoAttivo = false;
			this.audioBeepLungoAttivo = false;
			this.suonoBeep.Stop();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 1;
			this.audioBeepCortoAttivo = false;
			this.audioBeepLungoAttivo = false;
			this.suonoBeep.Stop();
			this.suonoBocca1.Stop();
			this.particelleBocca1.Stop();
			this.cannoneStaSparando = false;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 2;
			this.audioBeepCortoAttivo = false;
			this.audioBeepLungoAttivo = false;
			this.suonoBeep.Stop();
			this.suonoBocca1.Stop();
			this.particelleBocca1.Stop();
			this.cannoneStaSparando = false;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			base.GetComponent<PresenzaAlleato>().armaAttivaInFPS = 3;
			this.audioBeepCortoAttivo = false;
			this.audioBeepLungoAttivo = false;
			this.suonoBeep.Stop();
			this.suonoBocca1.Stop();
			this.particelleBocca1.Stop();
			this.cannoneStaSparando = false;
		}
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x0001E2B8 File Offset: 0x0001C4B8
	private void GestioneOrdigniPrimaPersona()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.mitragliatoreAttivo = true;
			this.gruppo01Attivo = false;
			this.gruppo23Attivo = false;
			this.gruppo45Attivo = false;
			this.timerDiAggancio = 0f;
			this.ordignoDaLanciare = null;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.mitragliatoreAttivo = false;
			this.gruppo01Attivo = true;
			this.gruppo23Attivo = false;
			this.gruppo45Attivo = false;
			this.timerDiAggancio = 0f;
			this.ordignoDaLanciare = null;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			this.mitragliatoreAttivo = false;
			this.gruppo01Attivo = false;
			this.gruppo23Attivo = true;
			this.gruppo45Attivo = false;
			this.timerDiAggancio = 0f;
			this.ordignoDaLanciare = null;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			this.mitragliatoreAttivo = false;
			this.gruppo01Attivo = false;
			this.gruppo23Attivo = false;
			this.gruppo45Attivo = true;
			this.timerDiAggancio = 0f;
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

	// Token: 0x060000A5 RID: 165 RVA: 0x0001E60C File Offset: 0x0001C80C
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
			List<float> list;
			List<float> expr_1BE = list = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int index;
			int expr_1C1 = index = 5;
			float num = list[index];
			expr_1BE[expr_1C1] = num - 1f;
			List<float> list2;
			List<float> expr_1E8 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[0];
			int expr_1EC = index = 6;
			num = list2[index];
			expr_1E8[expr_1EC] = num - 1f;
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
			this.cannoneStaSparando = false;
		}
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x0001E898 File Offset: 0x0001CA98
	private void SparoArma1()
	{
		this.mm20Proiettile = (UnityEngine.Object.Instantiate(this.munizioneArma1, this.bocca1.transform.position, this.bocca1.transform.rotation) as GameObject);
		this.mm20Proiettile.GetComponent<DatiProiettile>().sparatoInFPS = true;
		this.mm20Proiettile.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine = base.GetComponent<PresenzaAlleato>().tipoTruppa;
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x0001E908 File Offset: 0x0001CB08
	private void SistemaDiLancioInPrimaPersona()
	{
		this.ListaBersPPPossibili.Clear();
		float num = (float)(Screen.width / 13);
		if ((this.ordignoDaLanciare && this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 9) || this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 10)
		{
			Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			if (Physics.Raycast(ray, out this.targetSparo))
			{
				if (this.targetSparo.collider.gameObject.tag == "Nemico" || this.targetSparo.collider.gameObject.tag == "Nemico Testa")
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
				List<float> expr_1FE = list = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
				int index;
				int expr_202 = index = 5;
				float num2 = list[index];
				expr_1FE[expr_202] = num2 - 1f;
				List<float> list2;
				List<float> expr_233 = list2 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
				int expr_237 = index = 6;
				num2 = list2[index];
				expr_233[expr_237] = num2 - 1f;
				for (int i = 0; i < this.ListaOrdigniLocali.Count; i++)
				{
					this.ListaOrdigniLocali[i].GetComponent<DatiOrdignoEsterno>().ordignoAttivo = false;
				}
				this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().ordignoAttivo = true;
				this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = true;
				this.ordignoDaLanciare = null;
			}
		}
		if (this.ordignoDaLanciare && this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 5)
		{
			Ray ray2 = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			if (Physics.Raycast(ray2, out this.targetSparo))
			{
				if (this.targetSparo.collider.gameObject.tag == "Nemico" || this.targetSparo.collider.gameObject.tag == "Nemico Testa")
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
					if (current && current.GetComponent<PresenzaNemico>().insettoVolante)
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
						this.suonoBeep.clip = this.beepLungo;
						this.suonoBeep.Play();
						this.audioBeepLungoAttivo = true;
					}
					if (Input.GetMouseButtonDown(0) && base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][5] > 0f && this.timerDiLancio > base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][1] && this.ordignoDaLanciare != null)
					{
						this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().bersaglio = this.bersaglioInPP;
						this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = true;
						List<float> list3;
						List<float> expr_95D = list3 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
						int index;
						int expr_961 = index = 5;
						float num2 = list3[index];
						expr_95D[expr_961] = num2 - 1f;
						List<float> list4;
						List<float> expr_992 = list4 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
						int expr_996 = index = 6;
						num2 = list4[index];
						expr_992[expr_996] = num2 - 1f;
						this.timerDiLancio = 0f;
						for (int j = 0; j < this.ListaOrdigniLocali.Count; j++)
						{
							this.ListaOrdigniLocali[j].GetComponent<DatiOrdignoEsterno>().ordignoAttivo = false;
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
			}
			if (base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][5] <= 0f)
			{
				this.audioBeepLungoAttivo = false;
				this.audioBeepCortoAttivo = false;
				this.suonoBeep.Stop();
				this.mirinoMissiliMobile.GetComponent<Image>().color = this.coloreBaseMirini;
				this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
				this.mirinoMissiliMobile.transform.position = this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f));
			}
		}
		if (this.ordignoDaLanciare && this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == 6)
		{
			Ray ray3 = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			if (Physics.Raycast(ray3, out this.targetSparo))
			{
				if (this.targetSparo.collider.gameObject.tag == "Nemico" || this.targetSparo.collider.gameObject.tag == "Nemico Testa")
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
				foreach (GameObject current3 in this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici)
				{
					if (current3 && current3.GetComponent<PresenzaNemico>().insettoVolante)
					{
						Vector3 centroInsetto2 = current3.GetComponent<PresenzaNemico>().centroInsetto;
						Vector3 vector3 = centroInsetto2 - base.transform.position;
						float num12 = Vector3.Dot(base.transform.forward, vector3.normalized);
						float num13 = Vector3.Distance(base.transform.position, centroInsetto2);
						if (num13 > this.ordignoDaLanciare.GetComponent<DatiGeneraliMunizione>().portataMinima && num13 < this.ordignoDaLanciare.GetComponent<DatiGeneraliMunizione>().portataMassima && num12 > 0f && !Physics.Linecast(base.transform.position, centroInsetto2, this.layerVisuale))
						{
							float num14 = Vector2.Distance(this.terzaCamera.GetComponent<Camera>().WorldToScreenPoint(centroInsetto2), this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f)));
							if (num14 < num)
							{
								this.ListaBersPPPossibili.Add(current3);
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
				Vector3 vector4 = this.bersaglioInPP.GetComponent<PresenzaNemico>().centroInsetto - base.transform.position;
				float num15 = Vector3.Dot(base.transform.forward, vector4.normalized);
				if (num15 > 0f)
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
					float num16 = 999f;
					foreach (GameObject current4 in this.ListaBersPPPossibili)
					{
						float num17 = Vector2.Distance(this.terzaCamera.GetComponent<Camera>().WorldToScreenPoint(current4.GetComponent<CapsuleCollider>().center), this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f)));
						if (num17 < num16)
						{
							num16 = num17;
							this.bersaglioInPP = current4;
						}
					}
				}
				this.mirinoMissiliMobile.transform.position = this.terzaCamera.GetComponent<Camera>().WorldToScreenPoint(this.bersaglioInPP.GetComponent<PresenzaNemico>().centroInsetto);
				float num18 = Vector2.Distance(this.terzaCamera.GetComponent<Camera>().WorldToScreenPoint(this.bersaglioInPP.GetComponent<PresenzaNemico>().centroInsetto), this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f)));
				if (this.bersaglioInPP && this.bersaglioèAvantiInPP)
				{
					this.timerDiAggancio += Time.deltaTime;
					this.mirinoMissiliMobile.GetComponent<Image>().color = Color.red;
					if (!this.audioBeepCortoAttivo && this.timerDiAggancio < 2.5f)
					{
						this.suonoBeep.clip = this.beepCorto;
						this.suonoBeep.Play();
						this.audioBeepLungoAttivo = false;
						this.audioBeepCortoAttivo = true;
					}
					if (!this.audioBeepLungoAttivo && this.timerDiAggancio > 2.5f)
					{
						this.suonoBeep.clip = this.beepLungo;
						this.suonoBeep.Play();
						this.audioBeepLungoAttivo = true;
						this.audioBeepCortoAttivo = false;
					}
					if (this.audioBeepLungoAttivo && this.timerDiAggancio > 2.5f)
					{
						this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1f;
					}
					if (Input.GetMouseButtonDown(0) && base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][5] > 0f && this.timerDiLancio > base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][1] && this.ordignoDaLanciare != null && this.timerDiAggancio > 2.5f)
					{
						this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().bersaglio = this.bersaglioInPP;
						this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS = true;
						List<float> list5;
						List<float> expr_129C = list5 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
						int index;
						int expr_12A0 = index = 5;
						float num2 = list5[index];
						expr_129C[expr_12A0] = num2 - 1f;
						List<float> list6;
						List<float> expr_12D1 = list6 = base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare];
						int expr_12D5 = index = 6;
						num2 = list6[index];
						expr_12D1[expr_12D5] = num2 - 1f;
						this.timerDiLancio = 0f;
						this.timerDiAggancio = 0f;
						for (int k = 0; k < this.ListaOrdigniLocali.Count; k++)
						{
							this.ListaOrdigniLocali[k].GetComponent<DatiOrdignoEsterno>().ordignoAttivo = false;
						}
						this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().ordignoAttivo = true;
						this.ordignoDaLanciare.transform.parent.GetComponent<AudioSource>().Play();
						this.ordignoDaLanciare = null;
						this.audioBeepLungoAttivo = false;
						this.suonoBeep.Stop();
						this.mirinoMissiliMobile.GetComponent<Image>().color = this.coloreBaseMirini;
						this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
					}
				}
				if (num18 > num || !this.bersaglioèAvantiInPP)
				{
					this.mirinoMissiliMobile.GetComponent<Image>().color = this.coloreBaseMirini;
					this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
					this.audioBeepLungoAttivo = false;
					this.audioBeepCortoAttivo = false;
					this.suonoBeep.Stop();
					this.ordignoDaLanciare.transform.parent.GetComponent<DatiOrdignoEsterno>().bersaglio = null;
					this.timerDiAggancio = 0f;
				}
				float num19 = Vector2.Distance(this.terzaCamera.GetComponent<Camera>().WorldToScreenPoint(this.bersaglioInPP.GetComponent<PresenzaNemico>().centroInsetto), this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f)));
				if (num19 > num || !this.bersaglioèAvantiInPP)
				{
					this.bersaglioInPP = null;
				}
			}
			if (base.GetComponent<PresenzaAlleato>().ListaArmi[this.numArmaOrdignoDaLanciare][5] <= 0f)
			{
				this.audioBeepLungoAttivo = false;
				this.audioBeepCortoAttivo = false;
				this.suonoBeep.Stop();
				this.mirinoMissiliMobile.GetComponent<Image>().color = this.coloreBaseMirini;
				this.mirinoMissiliMobile.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
				this.mirinoMissiliMobile.transform.position = this.terzaCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f));
			}
		}
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x0001FEDC File Offset: 0x0001E0DC
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

	// Token: 0x0400034E RID: 846
	private GameObject infoNeutreTattica;

	// Token: 0x0400034F RID: 847
	private GameObject primaCamera;

	// Token: 0x04000350 RID: 848
	public GameObject bocca1;

	// Token: 0x04000351 RID: 849
	private GameObject terzaCamera;

	// Token: 0x04000352 RID: 850
	private GameObject IANemico;

	// Token: 0x04000353 RID: 851
	private GameObject InfoAlleati;

	// Token: 0x04000354 RID: 852
	private GameObject mm20Proiettile;

	// Token: 0x04000355 RID: 853
	private float timerFrequenzaArma1;

	// Token: 0x04000356 RID: 854
	private float timerRicarica1;

	// Token: 0x04000357 RID: 855
	private bool ricaricaInCorso1;

	// Token: 0x04000358 RID: 856
	private float timerDiLancio;

	// Token: 0x04000359 RID: 857
	private int layerColpo;

	// Token: 0x0400035A RID: 858
	private int layerVisuale;

	// Token: 0x0400035B RID: 859
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x0400035C RID: 860
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x0400035D RID: 861
	public Vector3 posizionamentoCabina;

	// Token: 0x0400035E RID: 862
	public Vector3 posCameraPsteriore;

	// Token: 0x0400035F RID: 863
	private float timerPosizionamentoTPS;

	// Token: 0x04000360 RID: 864
	private float timerPosizionamentoFPS;

	// Token: 0x04000361 RID: 865
	private GameObject CanvasFPS;

	// Token: 0x04000362 RID: 866
	private GameObject sensoreRaggioMirino;

	// Token: 0x04000363 RID: 867
	private GameObject sensoreRaggioMirinoMobile;

	// Token: 0x04000364 RID: 868
	private GameObject mirinoFissoVelivoli;

	// Token: 0x04000365 RID: 869
	private GameObject mirinoMissiliFisso;

	// Token: 0x04000366 RID: 870
	private GameObject mirinoMissiliMobile;

	// Token: 0x04000367 RID: 871
	private Color coloreBaseMirini;

	// Token: 0x04000368 RID: 872
	private GameObject mirinoMissiliFiloguidati;

	// Token: 0x04000369 RID: 873
	private GameObject mirinoBombe;

	// Token: 0x0400036A RID: 874
	private GameObject mirinoInfoVelivoli;

	// Token: 0x0400036B RID: 875
	private GameObject livelloSuolo;

	// Token: 0x0400036C RID: 876
	private RaycastHit targetSparo;

	// Token: 0x0400036D RID: 877
	private NavMeshAgent alleatoNav;

	// Token: 0x0400036E RID: 878
	private float velocitàAlleatoNav;

	// Token: 0x0400036F RID: 879
	private GameObject cannone;

	// Token: 0x04000370 RID: 880
	public GameObject unitàBersaglio;

	// Token: 0x04000371 RID: 881
	private Vector3 centroUnitàBersaglio;

	// Token: 0x04000372 RID: 882
	private GameObject munizioneArma1;

	// Token: 0x04000373 RID: 883
	private GameObject munizioneArma2;

	// Token: 0x04000374 RID: 884
	private GameObject munizioneArma3;

	// Token: 0x04000375 RID: 885
	private GameObject munizioneArma4;

	// Token: 0x04000376 RID: 886
	public AudioClip cannoneDurante;

	// Token: 0x04000377 RID: 887
	public AudioClip cannoneFine;

	// Token: 0x04000378 RID: 888
	private List<GameObject> ListaMunizioniAttiveUnità;

	// Token: 0x04000379 RID: 889
	public List<GameObject> ListaOrdigniLocali;

	// Token: 0x0400037A RID: 890
	public GameObject ordignoDaLanciare;

	// Token: 0x0400037B RID: 891
	private int numArmaOrdignoDaLanciare;

	// Token: 0x0400037C RID: 892
	private GameObject ordigno0;

	// Token: 0x0400037D RID: 893
	private GameObject ordigno1;

	// Token: 0x0400037E RID: 894
	private GameObject ordigno2;

	// Token: 0x0400037F RID: 895
	private GameObject ordigno3;

	// Token: 0x04000380 RID: 896
	private GameObject ordigno4;

	// Token: 0x04000381 RID: 897
	private GameObject ordigno5;

	// Token: 0x04000382 RID: 898
	public Vector3 posizioneOrdigni01;

	// Token: 0x04000383 RID: 899
	public Vector3 posizioneOrdigni23;

	// Token: 0x04000384 RID: 900
	public Vector3 posizioneOrdigni45;

	// Token: 0x04000385 RID: 901
	private bool mitragliatoreAttivo;

	// Token: 0x04000386 RID: 902
	private bool gruppo01Attivo;

	// Token: 0x04000387 RID: 903
	private bool gruppo23Attivo;

	// Token: 0x04000388 RID: 904
	private bool gruppo45Attivo;

	// Token: 0x04000389 RID: 905
	public List<GameObject> ListaOrdigniAttiviLocale;

	// Token: 0x0400038A RID: 906
	private List<bool> ListaGruppiOrdigniAttivi;

	// Token: 0x0400038B RID: 907
	private bool sparoConDestra01;

	// Token: 0x0400038C RID: 908
	private bool sparoConDestra23;

	// Token: 0x0400038D RID: 909
	private bool sparoConDestra45;

	// Token: 0x0400038E RID: 910
	private List<bool> ListaSparoConDestra;

	// Token: 0x0400038F RID: 911
	private bool primoFrameAvvenuto;

	// Token: 0x04000390 RID: 912
	public List<GameObject> ListaBersPPPossibili;

	// Token: 0x04000391 RID: 913
	public GameObject bersaglioInPP;

	// Token: 0x04000392 RID: 914
	private bool bersaglioèAvantiInPP;

	// Token: 0x04000393 RID: 915
	private bool bersDavantiEAPortata;

	// Token: 0x04000394 RID: 916
	private float volumeBaseEsterno;

	// Token: 0x04000395 RID: 917
	private GameObject ordignoFittizio;

	// Token: 0x04000396 RID: 918
	private bool bersèNelMirino;

	// Token: 0x04000397 RID: 919
	private bool cannoneStaSparando;

	// Token: 0x04000398 RID: 920
	private bool audioBeepLungoAttivo;

	// Token: 0x04000399 RID: 921
	private bool audioBeepCortoAttivo;

	// Token: 0x0400039A RID: 922
	public AudioClip beepCorto;

	// Token: 0x0400039B RID: 923
	public AudioClip beepLungo;

	// Token: 0x0400039C RID: 924
	private float timerDiAggancio;

	// Token: 0x0400039D RID: 925
	public GameObject supportoMissilePassivoInVolo;

	// Token: 0x0400039E RID: 926
	private AudioSource suonoMotore;

	// Token: 0x0400039F RID: 927
	private AudioSource suonoBocca1;

	// Token: 0x040003A0 RID: 928
	private AudioSource suonoBeep;

	// Token: 0x040003A1 RID: 929
	private ParticleSystem particelleBocca1;

	// Token: 0x040003A2 RID: 930
	private Text testoPotenzaMotore;

	// Token: 0x040003A3 RID: 931
	private Text testoAltitudine;

	// Token: 0x040003A4 RID: 932
	private float timerPresenzaInAereo;

	// Token: 0x040003A5 RID: 933
	private float timerAggRicerca;

	// Token: 0x040003A6 RID: 934
	private float timerSenzaArmiPrimarie;
}
