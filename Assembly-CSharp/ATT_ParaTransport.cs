using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class ATT_ParaTransport : MonoBehaviour
{
	// Token: 0x06000104 RID: 260 RVA: 0x0002E480 File Offset: 0x0002C680
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.scrittaPerSelezParà = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Scritte Varie").FindChild("selezione parà").gameObject;
		this.luceVerde = base.transform.FindChild("base corpo").FindChild("corpo luce").GetChild(0).gameObject;
		this.luceRossa = base.transform.FindChild("base corpo").FindChild("corpo luce").GetChild(1).gameObject;
		this.matLuceVerdeSpenta = this.luceVerde.GetComponent<MeshRenderer>().material;
		this.matLuceRossaSpenta = this.luceRossa.GetComponent<MeshRenderer>().material;
		this.luceRossa.GetComponent<MeshRenderer>().material = this.matLuceRossaAccesa;
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.suonoInterno = base.transform.GetChild(2).GetComponent<AudioSource>();
		this.volumeMotoreIniziale = base.GetComponent<AudioSource>().volume;
		this.suonoGo = base.transform.FindChild("suono Go").GetComponent<AudioSource>();
	}

	// Token: 0x06000105 RID: 261 RVA: 0x0002E5FC File Offset: 0x0002C7FC
	private void Update()
	{
		if (base.GetComponent<MOV_AUTOM_ParaTransport>().lancioAvviato)
		{
			this.AzioneDiLancio();
		}
		else if (base.GetComponent<PresenzaAlleato>().vita <= 0f && base.transform.childCount > 6)
		{
			for (int i = 6; i < base.transform.childCount; i++)
			{
				base.transform.GetChild(i).GetComponent<PresenzaAlleato>().vita = 0f;
			}
		}
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.GestioneVisuali();
			if (!base.GetComponent<MOV_AUTOM_ParaTransport>().lancioAvviato)
			{
				this.SelezioneParà();
			}
			if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva == 3)
			{
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
					this.suonoMotore.volume = this.volumeMotoreIniziale;
					this.scrittaPerSelezParà.GetComponent<CanvasGroup>().alpha = 1f;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					this.suonoMotore.volume = this.volumeMotoreIniziale;
					this.scrittaPerSelezParà.GetComponent<CanvasGroup>().alpha = 0f;
				}
			}
		}
		if (this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0 && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0] == base.gameObject)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona = false;
			this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera = null;
			this.timerPosizionamentoTPS = 0f;
			this.timerPosizionamentoFPS = 0f;
			this.suonoMotore.volume = this.volumeMotoreIniziale;
			this.scrittaPerSelezParà.GetComponent<CanvasGroup>().alpha = 0f;
		}
	}

	// Token: 0x06000106 RID: 262 RVA: 0x0002E820 File Offset: 0x0002CA20
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

	// Token: 0x06000107 RID: 263 RVA: 0x0002E8CC File Offset: 0x0002CACC
	private void CameraTPS()
	{
		this.timerPosizionamentoTPS += Time.deltaTime;
		if (this.timerPosizionamentoTPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = base.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraTPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localEulerAngles = Vector3.zero;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
		}
	}

	// Token: 0x06000108 RID: 264 RVA: 0x0002E960 File Offset: 0x0002CB60
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = base.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localEulerAngles = this.rotCameraFPS;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
		}
	}

	// Token: 0x06000109 RID: 265 RVA: 0x0002E9F8 File Offset: 0x0002CBF8
	private void SelezioneParà()
	{
		if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1) && this.ListaParàPresenti[0] != null && this.ListaParàPresenti[0].GetComponent<PresenzaAlleato>().tipoTruppa != 36)
			{
				this.primaCamera.GetComponent<Selezionamento>().numInListaParàDaSelez = 0;
				this.primaCamera.GetComponent<Selezionamento>().selezDaAereoParà = true;
				this.primaCamera.GetComponent<Selezionamento>().aereoOrigineParà = base.gameObject;
				this.timerPosizionamentoTPS = 0f;
				this.timerPosizionamentoFPS = 0f;
				this.suonoInterno.Stop();
				this.suonoMotore.volume = this.volumeMotoreIniziale;
				this.scrittaPerSelezParà.GetComponent<CanvasGroup>().alpha = 0f;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2) && this.ListaParàPresenti.Count >= 2 && this.ListaParàPresenti[1] != null && this.ListaParàPresenti[1].GetComponent<PresenzaAlleato>().tipoTruppa != 36)
			{
				this.primaCamera.GetComponent<Selezionamento>().numInListaParàDaSelez = 1;
				this.primaCamera.GetComponent<Selezionamento>().selezDaAereoParà = true;
				this.primaCamera.GetComponent<Selezionamento>().aereoOrigineParà = base.gameObject;
				this.timerPosizionamentoTPS = 0f;
				this.timerPosizionamentoFPS = 0f;
				this.suonoInterno.Stop();
				this.suonoMotore.volume = this.volumeMotoreIniziale;
				this.scrittaPerSelezParà.GetComponent<CanvasGroup>().alpha = 0f;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3) && this.ListaParàPresenti.Count >= 3 && this.ListaParàPresenti[2] != null && this.ListaParàPresenti[2].GetComponent<PresenzaAlleato>().tipoTruppa != 36)
			{
				this.primaCamera.GetComponent<Selezionamento>().numInListaParàDaSelez = 2;
				this.primaCamera.GetComponent<Selezionamento>().selezDaAereoParà = true;
				this.primaCamera.GetComponent<Selezionamento>().aereoOrigineParà = base.gameObject;
				this.timerPosizionamentoTPS = 0f;
				this.timerPosizionamentoFPS = 0f;
				this.suonoInterno.Stop();
				this.suonoMotore.volume = this.volumeMotoreIniziale;
				this.scrittaPerSelezParà.GetComponent<CanvasGroup>().alpha = 0f;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha4) && this.ListaParàPresenti.Count >= 4 && this.ListaParàPresenti[3] != null && this.ListaParàPresenti[3].GetComponent<PresenzaAlleato>().tipoTruppa != 36)
			{
				this.primaCamera.GetComponent<Selezionamento>().numInListaParàDaSelez = 3;
				this.primaCamera.GetComponent<Selezionamento>().selezDaAereoParà = true;
				this.primaCamera.GetComponent<Selezionamento>().aereoOrigineParà = base.gameObject;
				this.timerPosizionamentoTPS = 0f;
				this.timerPosizionamentoFPS = 0f;
				this.suonoInterno.Stop();
				this.suonoMotore.volume = this.volumeMotoreIniziale;
				this.scrittaPerSelezParà.GetComponent<CanvasGroup>().alpha = 0f;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha5) && this.ListaParàPresenti.Count >= 5 && this.ListaParàPresenti[4] != null && this.ListaParàPresenti[4].GetComponent<PresenzaAlleato>().tipoTruppa != 36)
			{
				this.primaCamera.GetComponent<Selezionamento>().numInListaParàDaSelez = 4;
				this.primaCamera.GetComponent<Selezionamento>().selezDaAereoParà = true;
				this.primaCamera.GetComponent<Selezionamento>().aereoOrigineParà = base.gameObject;
				this.timerPosizionamentoTPS = 0f;
				this.timerPosizionamentoFPS = 0f;
				this.suonoInterno.Stop();
				this.suonoMotore.volume = this.volumeMotoreIniziale;
				this.scrittaPerSelezParà.GetComponent<CanvasGroup>().alpha = 0f;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha6) && this.ListaParàPresenti.Count >= 6 && this.ListaParàPresenti[5] != null && this.ListaParàPresenti[5].GetComponent<PresenzaAlleato>().tipoTruppa != 36)
			{
				this.primaCamera.GetComponent<Selezionamento>().numInListaParàDaSelez = 5;
				this.primaCamera.GetComponent<Selezionamento>().selezDaAereoParà = true;
				this.primaCamera.GetComponent<Selezionamento>().aereoOrigineParà = base.gameObject;
				this.timerPosizionamentoTPS = 0f;
				this.timerPosizionamentoFPS = 0f;
				this.suonoInterno.Stop();
				this.suonoMotore.volume = this.volumeMotoreIniziale;
				this.scrittaPerSelezParà.GetComponent<CanvasGroup>().alpha = 0f;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha7) && this.ListaParàPresenti.Count >= 7 && this.ListaParàPresenti[6] != null && this.ListaParàPresenti[6].GetComponent<PresenzaAlleato>().tipoTruppa != 36)
			{
				this.primaCamera.GetComponent<Selezionamento>().numInListaParàDaSelez = 6;
				this.primaCamera.GetComponent<Selezionamento>().selezDaAereoParà = true;
				this.primaCamera.GetComponent<Selezionamento>().aereoOrigineParà = base.gameObject;
				this.timerPosizionamentoTPS = 0f;
				this.timerPosizionamentoFPS = 0f;
				this.suonoInterno.Stop();
				this.suonoMotore.volume = this.volumeMotoreIniziale;
				this.scrittaPerSelezParà.GetComponent<CanvasGroup>().alpha = 0f;
			}
		}
	}

	// Token: 0x0600010A RID: 266 RVA: 0x0002EFD8 File Offset: 0x0002D1D8
	private void AzioneDiLancio()
	{
		this.timerDaLancio += Time.deltaTime;
		this.luceVerde.GetComponent<MeshRenderer>().material = this.matLuceVerdeAccesa;
		this.luceRossa.GetComponent<MeshRenderer>().material = this.matLuceRossaSpenta;
		if (this.timerDaLancio > 0.5f)
		{
			if (this.timerDaLancio < 6f)
			{
				foreach (GameObject current in this.ListaParàPresenti)
				{
					if (current)
					{
						if (current.transform.localPosition.z > -14.2f)
						{
							current.transform.position += -base.transform.forward * 5f * Time.deltaTime;
						}
						else
						{
							current.GetComponent<PresenzaAlleato>().èInLancio = true;
							current.transform.parent = null;
						}
					}
				}
			}
			if (this.timerDaLancio > 6f)
			{
				base.GetComponent<MOV_AUTOM_ParaTransport>().rientroAttivo = true;
				this.luceVerde.GetComponent<MeshRenderer>().material = this.matLuceVerdeSpenta;
				this.luceRossa.GetComponent<MeshRenderer>().material = this.matLuceRossaAccesa;
				this.ListaParàPresenti = new List<GameObject>();
			}
		}
		else if (!this.suonoGoPartito)
		{
			this.suonoGoPartito = true;
			this.suonoGo.Play();
		}
	}

	// Token: 0x040004E8 RID: 1256
	private GameObject infoNeutreTattica;

	// Token: 0x040004E9 RID: 1257
	private GameObject primaCamera;

	// Token: 0x040004EA RID: 1258
	private GameObject terzaCamera;

	// Token: 0x040004EB RID: 1259
	private GameObject infoAlleati;

	// Token: 0x040004EC RID: 1260
	private GameObject scrittaPerSelezParà;

	// Token: 0x040004ED RID: 1261
	public Vector3 posPrimoParà;

	// Token: 0x040004EE RID: 1262
	public float distFraParà;

	// Token: 0x040004EF RID: 1263
	public List<GameObject> ListaParàPresenti;

	// Token: 0x040004F0 RID: 1264
	private float timerDaLancio;

	// Token: 0x040004F1 RID: 1265
	private Material matLuceRossaSpenta;

	// Token: 0x040004F2 RID: 1266
	private Material matLuceVerdeSpenta;

	// Token: 0x040004F3 RID: 1267
	public Material matLuceRossaAccesa;

	// Token: 0x040004F4 RID: 1268
	public Material matLuceVerdeAccesa;

	// Token: 0x040004F5 RID: 1269
	public GameObject luceRossa;

	// Token: 0x040004F6 RID: 1270
	public GameObject luceVerde;

	// Token: 0x040004F7 RID: 1271
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x040004F8 RID: 1272
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x040004F9 RID: 1273
	public Vector3 rotCameraFPS;

	// Token: 0x040004FA RID: 1274
	private float timerPosizionamentoTPS;

	// Token: 0x040004FB RID: 1275
	private float timerPosizionamentoFPS;

	// Token: 0x040004FC RID: 1276
	private AudioSource suonoMotore;

	// Token: 0x040004FD RID: 1277
	private AudioSource suonoInterno;

	// Token: 0x040004FE RID: 1278
	private float volumeMotoreIniziale;

	// Token: 0x040004FF RID: 1279
	private bool zoomAttivo;

	// Token: 0x04000500 RID: 1280
	private AudioSource suonoGo;

	// Token: 0x04000501 RID: 1281
	private bool suonoGoPartito;
}
