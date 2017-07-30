using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000025 RID: 37
public class ATT_SupplyHelicopter : MonoBehaviour
{
	// Token: 0x060001B5 RID: 437 RVA: 0x0004BF58 File Offset: 0x0004A158
	private void Start()
	{
		this.CanvasFPS = GameObject.FindGameObjectWithTag("CanvasFPS");
		this.mirinoFissoVelivoli = this.CanvasFPS.transform.GetChild(1).transform.GetChild(0).gameObject;
		this.mirinoMissiliFisso = this.CanvasFPS.transform.GetChild(1).transform.GetChild(1).gameObject;
		this.mirinoMissiliMobile = this.CanvasFPS.transform.GetChild(1).transform.GetChild(3).gameObject;
		this.mirinoMissiliFiloguidati = this.CanvasFPS.transform.GetChild(1).transform.GetChild(5).gameObject;
		this.mirinoBombe = this.CanvasFPS.transform.GetChild(1).transform.GetChild(6).gameObject;
		this.mirinoInfoVelivoli = this.CanvasFPS.transform.GetChild(1).transform.GetChild(7).gameObject;
		this.livelloSuolo = this.CanvasFPS.transform.GetChild(1).transform.GetChild(8).gameObject;
		this.testoPotenzaMotore = this.mirinoInfoVelivoli.transform.GetChild(0).GetComponent<Text>();
		this.testoAltitudine = this.mirinoInfoVelivoli.transform.GetChild(1).GetComponent<Text>();
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.coloreBaseMirini = this.mirinoFissoVelivoli.GetComponent<Image>().color;
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.suonoInterno = base.transform.GetChild(2).GetComponent<AudioSource>();
		this.volumeBaseEsterno = this.suonoMotore.volume;
		this.frequenzaDiRifornimento = 3f;
		this.suonoRifor = base.transform.GetChild(1).GetComponent<AudioSource>();
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x0004C194 File Offset: 0x0004A394
	private void Update()
	{
		this.RifornimentoTruppe();
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Tab))
			{
				this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera = null;
				this.timerPosizionamentoTPS = 0f;
				this.timerPosizionamentoFPS = 0f;
			}
			if (!base.GetComponent<PresenzaAlleato>().destinazioneOrdinata)
			{
				base.GetComponent<MOV_AUTOM_SupplyHelicopter>().muoviti = true;
			}
		}
		else
		{
			this.GestioneVisuali();
			this.Mirini();
			this.StrumentazioneMirini();
			if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva == 3)
			{
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
					base.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
					base.transform.GetChild(2).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
					base.transform.GetChild(2).transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
					base.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					base.transform.GetChild(3).transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
					this.suonoInterno.Play();
					this.suonoMotore.volume = this.volumeBaseEsterno / 3f;
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
					this.zoomAttivo = false;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					base.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
					base.transform.GetChild(2).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
					base.transform.GetChild(2).transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
					base.transform.GetChild(3).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
					base.transform.GetChild(3).transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
					this.suonoInterno.Stop();
					this.suonoMotore.volume = this.volumeBaseEsterno;
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
					this.zoomAttivo = false;
				}
			}
		}
		if (this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0 && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0] == base.gameObject)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona = false;
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

	// Token: 0x060001B7 RID: 439 RVA: 0x0004C5A8 File Offset: 0x0004A7A8
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

	// Token: 0x060001B8 RID: 440 RVA: 0x0004C654 File Offset: 0x0004A854
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

	// Token: 0x060001B9 RID: 441 RVA: 0x0004C6DC File Offset: 0x0004A8DC
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = base.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.rotation = base.transform.rotation;
		}
		base.transform.GetChild(3).transform.GetChild(0).transform.Rotate(Vector3.forward * 1000f * Time.deltaTime);
	}

	// Token: 0x060001BA RID: 442 RVA: 0x0004C79C File Offset: 0x0004A99C
	private void Mirini()
	{
		this.mirinoFissoVelivoli.GetComponent<CanvasGroup>().alpha = 1f;
		this.mirinoInfoVelivoli.GetComponent<CanvasGroup>().alpha = 1f;
		this.livelloSuolo.GetComponent<CanvasGroup>().alpha = 1f;
		this.mirinoBombe.GetComponent<CanvasGroup>().alpha = 0f;
	}

	// Token: 0x060001BB RID: 443 RVA: 0x0004C800 File Offset: 0x0004AA00
	private void StrumentazioneMirini()
	{
		float num = 30f + base.GetComponent<MOV_SupplyHelicopter>().velocitàMax / 1.5f * 2f + base.GetComponent<MOV_SupplyHelicopter>().velocitàMax;
		float num2 = (30f + base.GetComponent<MOV_SupplyHelicopter>().velocitàTraslSalitaEffettiva + Mathf.Abs(base.GetComponent<MOV_SupplyHelicopter>().velocitàTraslDavDietroEffettiva) + Mathf.Abs(base.GetComponent<MOV_SupplyHelicopter>().velocitàTraslLatEffettiva)) / num * 100f;
		this.testoPotenzaMotore.text = num2.ToString("F0") + "%";
		this.testoAltitudine.text = base.transform.position.y.ToString("F0");
		this.livelloSuolo.transform.eulerAngles = new Vector3(0f, 0f, -base.transform.eulerAngles.z);
	}

	// Token: 0x060001BC RID: 444 RVA: 0x0004C8F0 File Offset: 0x0004AAF0
	private void RifornimentoTruppe()
	{
		this.timerRifornimento += Time.deltaTime;
		bool flag = false;
		if (this.timerRifornimento > this.frequenzaDiRifornimento && base.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp > 0 && Physics.Raycast(base.transform.position, -Vector3.up, 3.5f, 256))
		{
			foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati)
			{
				if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && !current.GetComponent<PresenzaAlleato>().èPerRifornimento)
				{
					float num = Vector3.Distance(base.transform.position, current.transform.position);
					if (num < base.GetComponent<PresenzaAlleato>().raggioDiRifornimento)
					{
						for (int i = 0; i < current.GetComponent<PresenzaAlleato>().numeroArmi; i++)
						{
							if (current.GetComponent<PresenzaAlleato>().ListaArmi[i][6] < current.GetComponent<PresenzaAlleato>().ListaArmi[i][4] && this.infoAlleati.GetComponent<InfoMunizionamento>().ListaTipiMunizioniBaseTattica[current.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[i].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().posInListaMunizioniBase].GetComponent<QuantitàMunizione>().quantità > 0f)
							{
								current.GetComponent<PresenzaAlleato>().rifornimentoAttivo = true;
								base.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp--;
								flag = true;
							}
						}
					}
				}
				float num2 = Vector3.Distance(base.transform.position, current.transform.position);
				if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[1] && num2 < base.GetComponent<PresenzaAlleato>().raggioDiRifornimento && this.infoAlleati.GetComponent<InfoMunizionamento>().ListaTipiMunizioniBaseTattica[11].GetComponent<QuantitàMunizione>().quantità > 0f && current.GetComponent<PresenzaAlleato>().vita < current.GetComponent<PresenzaAlleato>().vitaIniziale)
				{
					current.GetComponent<PresenzaAlleato>().riparazioneAttiva = true;
					base.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp--;
					this.infoAlleati.GetComponent<InfoMunizionamento>().ListaTipiMunizioniBaseTattica[11].GetComponent<QuantitàMunizione>().quantità -= 1f;
				}
			}
			this.timerRifornimento = 0f;
		}
		if (flag)
		{
			this.suonoRifor.Play();
		}
	}

	// Token: 0x0400079E RID: 1950
	private float frequenzaDiRifornimento;

	// Token: 0x0400079F RID: 1951
	private float timerRifornimento;

	// Token: 0x040007A0 RID: 1952
	private GameObject infoNeutreTattica;

	// Token: 0x040007A1 RID: 1953
	private GameObject terzaCamera;

	// Token: 0x040007A2 RID: 1954
	private GameObject primaCamera;

	// Token: 0x040007A3 RID: 1955
	private GameObject IANemico;

	// Token: 0x040007A4 RID: 1956
	private GameObject infoAlleati;

	// Token: 0x040007A5 RID: 1957
	private Text testoPotenzaMotore;

	// Token: 0x040007A6 RID: 1958
	private Text testoAltitudine;

	// Token: 0x040007A7 RID: 1959
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x040007A8 RID: 1960
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x040007A9 RID: 1961
	private float timerPosizionamentoTPS;

	// Token: 0x040007AA RID: 1962
	private float timerPosizionamentoFPS;

	// Token: 0x040007AB RID: 1963
	private GameObject CanvasFPS;

	// Token: 0x040007AC RID: 1964
	private GameObject mirinoFissoVelivoli;

	// Token: 0x040007AD RID: 1965
	private GameObject mirinoMissiliFisso;

	// Token: 0x040007AE RID: 1966
	private GameObject mirinoMissiliMobile;

	// Token: 0x040007AF RID: 1967
	private Color coloreBaseMirini;

	// Token: 0x040007B0 RID: 1968
	private GameObject mirinoMissiliFiloguidati;

	// Token: 0x040007B1 RID: 1969
	private GameObject mirinoBombe;

	// Token: 0x040007B2 RID: 1970
	private GameObject mirinoInfoVelivoli;

	// Token: 0x040007B3 RID: 1971
	private GameObject livelloSuolo;

	// Token: 0x040007B4 RID: 1972
	private float volumeBaseEsterno;

	// Token: 0x040007B5 RID: 1973
	private AudioSource suonoMotore;

	// Token: 0x040007B6 RID: 1974
	private AudioSource suonoInterno;

	// Token: 0x040007B7 RID: 1975
	private bool zoomAttivo;

	// Token: 0x040007B8 RID: 1976
	private AudioSource suonoRifor;
}
