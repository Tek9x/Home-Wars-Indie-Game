using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000BD RID: 189
public class TerzaCamera : MonoBehaviour
{
	// Token: 0x060006B0 RID: 1712 RVA: 0x000ED20C File Offset: 0x000EB40C
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
		this.mirinoFissoTerr = this.CanvasFPS.transform.GetChild(2).transform.GetChild(0).gameObject;
		this.mirinoMissiliFissoTerr = this.CanvasFPS.transform.GetChild(2).transform.GetChild(1).gameObject;
		this.mirinoMissiliMobileTerr = this.CanvasFPS.transform.GetChild(2).transform.GetChild(3).gameObject;
		this.mirinoElettr1 = this.CanvasFPS.transform.GetChild(2).transform.GetChild(5).gameObject;
		this.mirinoMissiliFiloguidatiTerr = this.CanvasFPS.transform.GetChild(2).transform.GetChild(8).gameObject;
		this.mirinoDiPrecisione = this.CanvasFPS.transform.GetChild(0).transform.GetChild(0).gameObject;
		this.mirinoBinocolo = this.CanvasFPS.transform.GetChild(0).transform.GetChild(1).gameObject;
		this.verificaTiro = this.CanvasFPS.transform.GetChild(2).transform.FindChild("verifica tiro").gameObject;
		this.primaCamera = GameObject.Find("Prima Camera");
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x000ED4A8 File Offset: 0x000EB6A8
	private void Update()
	{
		this.timerVerifTro += Time.deltaTime;
		if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva == 3)
		{
			this.ospiteCamera = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0];
			if (!this.terzaCameraPosizionata)
			{
				this.terzaCameraPosizionata = true;
				this.èTPS = true;
				this.èFPS = false;
			}
			if (Input.GetKeyDown(KeyCode.E) && !Input.GetMouseButton(1))
			{
				if (this.èTPS)
				{
					this.èTPS = false;
					this.èFPS = true;
					this.diventaFPS = true;
				}
				else if (this.èFPS)
				{
					this.èTPS = true;
					this.èFPS = false;
					this.diventaTPS = true;
				}
				base.GetComponent<Camera>().fieldOfView = 60f;
			}
			this.FunzioneVerificaTiro();
		}
		else
		{
			this.ospiteCamera = null;
			base.transform.parent = null;
			this.mirinoFissoVelivoli.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliFisso.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliMobile.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliFiloguidati.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoBombe.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoInfoVelivoli.GetComponent<CanvasGroup>().alpha = 0f;
			this.livelloSuolo.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoFissoTerr.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliFissoTerr.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliMobileTerr.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoElettr1.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoMissiliFiloguidatiTerr.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoDiPrecisione.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoBinocolo.GetComponent<CanvasGroup>().alpha = 0f;
			this.mirinoFissoTerr.GetComponent<Image>().sprite = this.mirinoFissoTerrDiBase;
			this.verificaTiro.GetComponent<Image>().enabled = false;
		}
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x000ED6F0 File Offset: 0x000EB8F0
	private void FunzioneVerificaTiro()
	{
		if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().faseSchierInizTerminata && this.timerVerifTro > 0f && this.timerVerifTro < 0.5f)
		{
			this.verificaTiro.GetComponent<Image>().enabled = true;
		}
		else
		{
			this.verificaTiro.GetComponent<Image>().enabled = false;
		}
	}

	// Token: 0x040018DD RID: 6365
	private GameObject CanvasFPS;

	// Token: 0x040018DE RID: 6366
	private GameObject mirinoFissoVelivoli;

	// Token: 0x040018DF RID: 6367
	private GameObject mirinoMissiliFisso;

	// Token: 0x040018E0 RID: 6368
	private GameObject mirinoMissiliMobile;

	// Token: 0x040018E1 RID: 6369
	private GameObject mirinoMissiliFiloguidati;

	// Token: 0x040018E2 RID: 6370
	private GameObject mirinoBombe;

	// Token: 0x040018E3 RID: 6371
	private GameObject mirinoInfoVelivoli;

	// Token: 0x040018E4 RID: 6372
	private GameObject livelloSuolo;

	// Token: 0x040018E5 RID: 6373
	private GameObject mirinoFissoTerr;

	// Token: 0x040018E6 RID: 6374
	private GameObject mirinoMissiliFissoTerr;

	// Token: 0x040018E7 RID: 6375
	private GameObject mirinoMissiliMobileTerr;

	// Token: 0x040018E8 RID: 6376
	private GameObject mirinoElettr1;

	// Token: 0x040018E9 RID: 6377
	private GameObject mirinoMissiliFiloguidatiTerr;

	// Token: 0x040018EA RID: 6378
	private GameObject mirinoDiPrecisione;

	// Token: 0x040018EB RID: 6379
	private GameObject mirinoBinocolo;

	// Token: 0x040018EC RID: 6380
	private GameObject verificaTiro;

	// Token: 0x040018ED RID: 6381
	public Sprite mirinoFissoTerrDiBase;

	// Token: 0x040018EE RID: 6382
	private GameObject primaCamera;

	// Token: 0x040018EF RID: 6383
	public GameObject ospiteCamera;

	// Token: 0x040018F0 RID: 6384
	private GameObject infoAlleati;

	// Token: 0x040018F1 RID: 6385
	private GameObject infoNeutreTattica;

	// Token: 0x040018F2 RID: 6386
	private float timerPosizionamento;

	// Token: 0x040018F3 RID: 6387
	private float rotazioneSG;

	// Token: 0x040018F4 RID: 6388
	public bool èTPS;

	// Token: 0x040018F5 RID: 6389
	public bool èFPS;

	// Token: 0x040018F6 RID: 6390
	public bool diventaTPS;

	// Token: 0x040018F7 RID: 6391
	public bool diventaFPS;

	// Token: 0x040018F8 RID: 6392
	public bool entraInPrimaPersona;

	// Token: 0x040018F9 RID: 6393
	public bool esciDaPrimaPersona;

	// Token: 0x040018FA RID: 6394
	public bool terzaCameraPosizionata;

	// Token: 0x040018FB RID: 6395
	public float timerVerifTro;
}
