using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000107 RID: 263
public class GestoreNeutroTattica : MonoBehaviour
{
	// Token: 0x0600084C RID: 2124 RVA: 0x00123280 File Offset: 0x00121480
	private void Awake()
	{
		this.ListaCamere = new List<GameObject>();
		this.ListaCamere.Add(GameObject.FindGameObjectWithTag("MainCamera"));
		this.ListaCamere.Add(GameObject.FindGameObjectWithTag("NoMainCamera"));
		this.ListaCamere.Add(GameObject.FindGameObjectWithTag("Terza Camera"));
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x001232D8 File Offset: 0x001214D8
	private void Start()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.varieMappaLocale = GameObject.FindGameObjectWithTag("VarieMappaLocale");
		this.primaCamera = this.ListaCamere[0];
		this.secondaCamera = this.ListaCamere[1];
		this.scrittaTimerBattagliaUI = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Timer").FindChild("tempo").gameObject;
		this.scrittaDescrTipoBatt = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Obbiettivi").FindChild("sfondo").GetChild(0).gameObject;
		this.battagliaTipo0UI = GameObject.FindWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Condizioni Vittoria").FindChild("Battaglia Tipo 0").gameObject;
		this.battagliaTipo1UI = GameObject.FindWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Condizioni Vittoria").FindChild("Battaglia Tipo 1").gameObject;
		this.battagliaTipo2UI = GameObject.FindWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Condizioni Vittoria").FindChild("Battaglia Tipo 2").gameObject;
		this.battagliaTipo3UI = GameObject.FindWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Condizioni Vittoria").FindChild("Battaglia Tipo 3").gameObject;
		this.battagliaTipo4UI = GameObject.FindWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Condizioni Vittoria").FindChild("Battaglia Tipo 4").gameObject;
		this.battagliaTipo5UI = GameObject.FindWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Condizioni Vittoria").FindChild("Battaglia Tipo 5").gameObject;
		this.battagliaTipo6UI = GameObject.FindWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Condizioni Vittoria").FindChild("Battaglia Tipo 6").gameObject;
		this.battagliaTipo7UI = GameObject.FindWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Condizioni Vittoria").FindChild("Battaglia Tipo 7").gameObject;
		this.esitoBattagliaUI = GameObject.FindWithTag("CanvasComandante").transform.FindChild("Esito Battaglia").gameObject;
		this.esitoBattaVeloceUI = GameObject.FindWithTag("CanvasComandante").transform.FindChild("Esito Battaglia Veloce").gameObject;
		this.esBatVelListaAlleati = this.esitoBattaVeloceUI.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
		this.esBatVelListaNemici = this.esitoBattaVeloceUI.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject;
		this.oggettoMusica = GameObject.FindGameObjectWithTag("Musica");
		this.inizioLivello = GameObject.FindGameObjectWithTag("InizioLivello");
		this.pulsVinciBattAnticipo = GameObject.FindWithTag("CanvasComandante").transform.FindChild("Varie Battaglia").FindChild("Vinci battaglia in anticipo").gameObject;
		this.schieramentoAttivo = true;
		this.livelloNest = this.IANemico.GetComponent<InfoGenericheNemici>().livelloNest;
		if (GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.livelloNest = 10;
		}
		else
		{
			this.livelloNest = PlayerPrefs.GetInt("livelloNest");
		}
		if (!GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.numeroMaxAlleati = PlayerPrefs.GetInt("max alleati");
		}
		if (GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.puntiBattaglia = 100000f;
		}
		else
		{
			this.valoreBasePerBattPont = 4f;
			this.puntiBattaglia = this.valoreBasePerBattPont * (float)this.numeroMaxAlleati * (this.giorniFittizi / 18f + 1f);
			if (this.puntiBattaglia <= 0f)
			{
				this.puntiBattaglia = 1500f;
			}
		}
		GestoreNeutroTattica.numAlleatiMorti = 0;
		GestoreNeutroTattica.numNemiciMorti = 0;
		this.moltiplicatoreFPSBattVeloce = 2f;
		this.impostDurataBatt = PlayerPrefs.GetInt("durata battaglia");
		if (this.tipoBattaglia != 3 && this.tipoBattaglia != 5)
		{
			Vector3 a = Vector3.zero;
			Vector3 position = Vector3.zero;
			if (GestoreNeutroTattica.èBattagliaVeloce && this.tipoBattaglia == 0)
			{
				a = base.transform.TransformDirection(this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[this.varieMappaLocale.GetComponent<VarieMappaLocale>().schierPerAvampAlleato].transform.forward);
				position = this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[this.varieMappaLocale.GetComponent<VarieMappaLocale>().schierPerAvampAlleato].transform.position + a * 60f;
				position = new Vector3(position.x, 0f, position.z);
			}
			else
			{
				for (int i = 0; i < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento.Count; i++)
				{
					if (this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[i].transform.GetChild(0).tag == "AreaSchieramentoAlleato")
					{
						a = base.transform.TransformDirection(this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[i].transform.forward);
						position = this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[i].transform.position + a * 60f;
						position = new Vector3(position.x, 0f, position.z);
						break;
					}
				}
			}
			this.depositoMunizioni = (UnityEngine.Object.Instantiate(this.depositoMunizioniPrefab, position, Quaternion.identity) as GameObject);
		}
		if (this.tipoBattaglia == 0)
		{
			Vector3 a2 = Vector3.zero;
			Vector3 position2 = Vector3.zero;
			if (GestoreNeutroTattica.èBattagliaVeloce)
			{
				a2 = base.transform.TransformDirection(this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[this.varieMappaLocale.GetComponent<VarieMappaLocale>().schierPerAvampAlleato].transform.forward);
				position2 = this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[this.varieMappaLocale.GetComponent<VarieMappaLocale>().schierPerAvampAlleato].transform.position + a2 * 120f;
				position2 = new Vector3(position2.x, 1f, position2.z);
			}
			else
			{
				for (int j = 0; j < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento.Count; j++)
				{
					if (this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[j].transform.GetChild(0).tag == "AreaSchieramentoAlleato")
					{
						a2 = base.transform.TransformDirection(this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[j].transform.forward);
						position2 = this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[j].transform.position + a2 * 120f;
						position2 = new Vector3(position2.x, 1f, position2.z);
						break;
					}
				}
			}
			this.avampostoAlleato = (UnityEngine.Object.Instantiate(this.avampostoAlleatoPrefab, position2, Quaternion.identity) as GameObject);
			this.tempoMaxPartita = (float)(900 * (this.impostDurataBatt + 1));
			this.scrittaDescrTipoBatt.GetComponent<Text>().text = this.oggDescrObbBattTipo0.GetComponent<Text>().text;
			this.battagliaTipo0UI.GetComponent<CanvasGroup>().alpha = 1f;
			this.tempoPuntiBatt = 5f;
			this.incrementoPuntiBatt = 10f;
		}
		else if (this.tipoBattaglia == 1)
		{
			Vector3 a3 = Vector3.zero;
			Vector3 position3 = Vector3.zero;
			if (GestoreNeutroTattica.èBattagliaVeloce)
			{
				a3 = base.transform.TransformDirection(this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[this.varieMappaLocale.GetComponent<VarieMappaLocale>().schierPerAvampNemico].transform.forward);
				position3 = this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[this.varieMappaLocale.GetComponent<VarieMappaLocale>().schierPerAvampNemico].transform.position + a3 * 70f;
				position3 = new Vector3(position3.x, 1f, position3.z);
			}
			else
			{
				for (int k = 0; k < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento.Count; k++)
				{
					if (this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[k].transform.GetChild(0).tag == "AreaSchieramentoNemico")
					{
						a3 = base.transform.TransformDirection(this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[k].transform.forward);
						position3 = this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[k].transform.position + a3 * 70f;
					}
				}
			}
			this.avampostoNemico = (UnityEngine.Object.Instantiate(this.avampostoNemicoPrefab, position3, Quaternion.identity) as GameObject);
			this.ListaBersagliDinamite = new List<GameObject>();
			this.ListaBersagliDinamite.Add(this.avampostoNemico);
			this.tempoMaxPartita = (float)(900 * (this.impostDurataBatt + 1));
			this.scrittaDescrTipoBatt.GetComponent<Text>().text = this.oggDescrObbBattTipo1.GetComponent<Text>().text;
			this.battagliaTipo1UI.GetComponent<CanvasGroup>().alpha = 1f;
			this.tempoPuntiBatt = 5f;
			this.incrementoPuntiBatt = 10f;
			this.avampostoNemico.GetComponent<ObbiettivoTatticoScript>().vita += this.avampostoNemico.GetComponent<ObbiettivoTatticoScript>().vita / 2f * (float)this.livelloNest;
		}
		else if (this.tipoBattaglia == 2)
		{
			this.ListaPossessoBandiere = new List<int>();
			for (int l = 0; l < 5; l++)
			{
				this.ListaPossessoBandiere.Add(0);
			}
			this.ListaBandiereConq = new List<GameObject>();
			for (int m = 0; m < 5; m++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(this.bandieraConquistaPrefab, this.varieMappaLocale.transform.GetChild(0).GetChild(m).transform.position, Quaternion.identity) as GameObject;
				gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = this.matBandieraNeutra;
				this.ListaBandiereConq.Add(gameObject);
			}
			this.tempoMaxPartita = (float)(600 * (this.impostDurataBatt + 1));
			this.puntConqMassimi = 350f;
			this.puntiConqAVolta = 1f;
			this.puntiConqTempo = 5f;
			this.scrittaDescrTipoBatt.GetComponent<Text>().text = this.oggDescrObbBattTipo2.GetComponent<Text>().text;
			this.battagliaTipo2UI.GetComponent<CanvasGroup>().alpha = 1f;
			this.tempoPuntiBatt = 5f;
			this.incrementoPuntiBatt = 10f;
		}
		else if (this.tipoBattaglia == 3)
		{
			this.tempoMaxPartita = (float)(300 * (this.impostDurataBatt + 1));
			this.scrittaDescrTipoBatt.GetComponent<Text>().text = this.oggDescrObbBattTipo3.GetComponent<Text>().text;
			this.battagliaTipo3UI.GetComponent<CanvasGroup>().alpha = 1f;
			this.tempoPuntiBatt = 999999f;
			this.incrementoPuntiBatt = 0f;
		}
		else if (this.tipoBattaglia == 4)
		{
			this.cassaSupply = (UnityEngine.Object.Instantiate(this.cassaSupplyPrefab, this.varieMappaLocale.GetComponent<VarieMappaLocale>().centroPerEstrazione, Quaternion.identity) as GameObject);
			this.tempoMaxPartita = (float)(300 * (this.impostDurataBatt + 1));
			this.scrittaDescrTipoBatt.GetComponent<Text>().text = this.oggDescrObbBattTipo4.GetComponent<Text>().text;
			this.battagliaTipo4UI.GetComponent<CanvasGroup>().alpha = 1f;
			this.tempoPuntiBatt = 5f;
			this.incrementoPuntiBatt = 2f;
		}
		else if (this.tipoBattaglia == 5)
		{
			Vector3 position4 = new Vector3(this.varieMappaLocale.GetComponent<VarieMappaLocale>().centroPerGunship.x + this.varieMappaLocale.GetComponent<VarieMappaLocale>().raggioInserimentoPerGunship, 250f, this.varieMappaLocale.GetComponent<VarieMappaLocale>().centroPerGunship.z);
			this.satellite = (UnityEngine.Object.Instantiate(this.satellitePrefab, position4, Quaternion.identity) as GameObject);
			this.tempoMaxPartita = (float)(300 * (this.impostDurataBatt + 1));
			this.scrittaDescrTipoBatt.GetComponent<Text>().text = this.oggDescrObbBattTipo5.GetComponent<Text>().text;
			this.battagliaTipo5UI.GetComponent<CanvasGroup>().alpha = 1f;
			this.tempoPuntiBatt = 999999f;
			this.incrementoPuntiBatt = 0f;
			this.puntoPerSatellite = new Vector3(this.varieMappaLocale.GetComponent<VarieMappaLocale>().centroPerGunship.x, 250f, this.varieMappaLocale.GetComponent<VarieMappaLocale>().centroPerGunship.z);
		}
		else if (this.tipoBattaglia == 6)
		{
			this.ListaPosConvAlleato = new List<Vector3>();
			for (int n = 0; n < this.varieMappaLocale.transform.GetChild(1).childCount; n++)
			{
				this.ListaPosConvAlleato.Add(this.varieMappaLocale.transform.GetChild(1).GetChild(n).transform.position);
			}
			this.ListaCamionPerConvoglio = new List<GameObject>();
			for (int num = 0; num < 4; num++)
			{
				this.ListaCamionPerConvoglio.Add(null);
				this.ListaCamionPerConvoglio[num] = (UnityEngine.Object.Instantiate(this.camionPerConvoglioPrefab, this.ListaPosConvAlleato[0], Quaternion.identity) as GameObject);
				this.ListaCamionPerConvoglio[num].transform.forward = this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[1].transform.forward;
				this.ListaCamionPerConvoglio[num].transform.position -= this.ListaCamionPerConvoglio[num].transform.forward * (float)num * 16f;
				this.ListaCamionPerConvoglio[num].transform.position += this.ListaCamionPerConvoglio[num].transform.forward * 40f;
				this.ListaCamionPerConvoglio[num].transform.position += -this.ListaCamionPerConvoglio[num].transform.right * 40f;
				this.ListaCamionPerConvoglio[num].GetComponent<CamionPerConvoglio>().numConvoglio = num;
			}
			this.tempoMaxPartita = (float)(1200 * (this.impostDurataBatt + 1));
			this.scrittaDescrTipoBatt.GetComponent<Text>().text = this.oggDescrObbBattTipo6.GetComponent<Text>().text;
			this.battagliaTipo6UI.GetComponent<CanvasGroup>().alpha = 1f;
			this.tempoPuntiBatt = 10f;
			this.incrementoPuntiBatt = 5f;
		}
		else if (this.tipoBattaglia == 7)
		{
			this.ListaPosConvNemico = new List<Vector3>();
			for (int num2 = 0; num2 < this.varieMappaLocale.transform.GetChild(2).childCount; num2++)
			{
				this.ListaPosConvNemico.Add(this.varieMappaLocale.transform.GetChild(2).GetChild(num2).transform.position);
			}
			this.ListaPanePerConvoglio = new List<GameObject>();
			for (int num3 = 0; num3 < 5; num3++)
			{
				this.ListaPanePerConvoglio.Add(null);
				this.ListaPanePerConvoglio[num3] = (UnityEngine.Object.Instantiate(this.panePerConvoglioPrefab, this.ListaPosConvNemico[0], Quaternion.identity) as GameObject);
				this.ListaPanePerConvoglio[num3].transform.forward = this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[0].transform.forward;
				this.ListaPanePerConvoglio[num3].transform.position -= this.ListaPanePerConvoglio[num3].transform.forward * (float)num3 * 14f;
				this.ListaPanePerConvoglio[num3].transform.position += this.ListaPanePerConvoglio[num3].transform.forward * 40f;
				this.ListaPanePerConvoglio[num3].GetComponent<PanePerConvoglio>().numPane = num3;
			}
			this.tempoMaxPartita = (float)(1200 * (this.impostDurataBatt + 1));
			this.scrittaDescrTipoBatt.GetComponent<Text>().text = this.oggDescrObbBattTipo7.GetComponent<Text>().text;
			this.battagliaTipo7UI.GetComponent<CanvasGroup>().alpha = 1f;
			this.tempoPuntiBatt = 10f;
			this.incrementoPuntiBatt = 5f;
		}
		this.ListaDanniAlleati = new List<float>();
		this.ListaDanniNemici = new List<float>();
		for (int num4 = 0; num4 < 48; num4++)
		{
			this.ListaDanniAlleati.Add(0f);
			this.ListaDanniNemici.Add(0f);
		}
		if (this.oggettoMusica)
		{
			if (!this.oggettoMusica.GetComponent<MusicaScript>().musica.isPlaying)
			{
				if (this.tipoBattaglia == 0 || this.tipoBattaglia == 3 || this.tipoBattaglia == 5)
				{
					this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica = 5;
				}
				else if (this.tipoBattaglia == 1 || this.tipoBattaglia == 6 || this.tipoBattaglia == 7)
				{
					this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica = 6;
				}
				else if (this.tipoBattaglia == 2 || this.tipoBattaglia == 4)
				{
					this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica = 7;
				}
				this.oggettoMusica.GetComponent<MusicaScript>().attivitàInMusica = true;
				this.oggettoMusica.GetComponent<MusicaScript>().musica.volume = PlayerPrefs.GetFloat("volume musica tattica");
			}
			else if (this.oggettoMusica.GetComponent<MusicaScript>().musica.isPlaying && (this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica != 5 || this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica != 6 || this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica != 7))
			{
				if (this.tipoBattaglia == 0 || this.tipoBattaglia == 3 || this.tipoBattaglia == 5)
				{
					this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica = 5;
				}
				else if (this.tipoBattaglia == 1 || this.tipoBattaglia == 6 || this.tipoBattaglia == 7)
				{
					this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica = 6;
				}
				else if (this.tipoBattaglia == 2 || this.tipoBattaglia == 4)
				{
					this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica = 7;
				}
				this.oggettoMusica.GetComponent<MusicaScript>().attivitàInMusica = true;
				this.oggettoMusica.GetComponent<MusicaScript>().musica.volume = PlayerPrefs.GetFloat("volume musica tattica");
			}
		}
		NavMesh.pathfindingIterationsPerFrame = 500;
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x001247C0 File Offset: 0x001229C0
	private void Update()
	{
		if (!this.primoFramePassato)
		{
			this.primoFramePassato = true;
			this.PrimoFrame();
		}
		if (!this.areeSchierDecise)
		{
			this.areeSchierDecise = true;
			this.InizializBattaglia();
		}
		this.GestioneTempo();
		this.GestioneAreeSchieramento();
		this.GestioneBattagliePerTipo();
		if (this.timerGiocoEffet > 10f && !this.battagliaTerminata)
		{
			if (this.tipoBattaglia == 1 && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati.Count <= 0 && this.infoAlleati.GetComponent<InfoGenericheAlleati>().alleatiDiRiserva <= 0)
			{
				this.battagliaTerminata = true;
				GestoreNeutroStrategia.vincitore = 2;
			}
			if (this.tipoBattaglia == 0)
			{
				if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici.Count <= 40 && this.IANemico.GetComponent<InfoGenericheNemici>().numRinforziNemici <= 0)
				{
					this.pulsVinciBattAnticipo.GetComponent<CanvasGroup>().alpha = 1f;
					this.pulsVinciBattAnticipo.GetComponent<CanvasGroup>().interactable = true;
					this.pulsVinciBattAnticipo.GetComponent<CanvasGroup>().blocksRaycasts = true;
				}
				if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici.Count <= 0 && this.IANemico.GetComponent<InfoGenericheNemici>().numRinforziNemici <= 0)
				{
					this.battagliaTerminata = true;
					GestoreNeutroStrategia.vincitore = 1;
				}
			}
		}
		if (this.battagliaTerminata)
		{
			this.timerFine += Time.unscaledDeltaTime;
			this.pulsVinciBattAnticipo.GetComponent<CanvasGroup>().alpha = 0f;
			this.pulsVinciBattAnticipo.GetComponent<CanvasGroup>().interactable = false;
			this.pulsVinciBattAnticipo.GetComponent<CanvasGroup>().blocksRaycasts = false;
			this.BattagliaTerminata();
		}
		if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.M))
		{
			this.timerGiocoEffet = 9999f;
		}
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x001249C8 File Offset: 0x00122BC8
	private void InizializBattaglia()
	{
		for (int i = 0; i < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento.Count; i++)
		{
			if (this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[i].transform.GetChild(0).tag == "AreaSchieramentoAlleato")
			{
				this.primaCamera.transform.position = this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[i].transform.position + Vector3.up * 100f;
				this.primaCamera.transform.forward = this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[i].transform.forward;
				this.secondaCamera.transform.position = new Vector3(0f, 370f, 0f);
				this.primaCamera.GetComponent<PrimaCamera>().posInizialePerRiposiz = this.primaCamera.transform.position;
				this.primaCamera.GetComponent<PrimaCamera>().rotInizialePerRiposiz = this.primaCamera.transform.eulerAngles;
				break;
			}
		}
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x00124B10 File Offset: 0x00122D10
	private void PrimoFrame()
	{
		if (this.tipoBattaglia == 3)
		{
			if (GestoreNeutroTattica.èBattagliaVeloce)
			{
				List<GameObject> list = new List<GameObject>();
				foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiPossibili)
				{
					if (current.GetComponent<PresenzaAlleato>().èFanteria && current.GetComponent<PresenzaAlleato>().tipoTruppa != 15)
					{
						list.Add(current);
					}
				}
				float num = 6f;
				Vector3 centroPerEstrazione = this.varieMappaLocale.GetComponent<VarieMappaLocale>().centroPerEstrazione;
				for (int i = 0; i < 8; i++)
				{
					int num2 = UnityEngine.Random.Range(0, 14);
					GestoreNeutroStrategia.valoreRandomSeed++;
					UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
					for (int j = 0; j < list.Count; j++)
					{
						if (num2 == j)
						{
							Vector3 position = Vector3.zero;
							if (i == 0)
							{
								position = new Vector3(0f, list[j].GetComponent<PresenzaAlleato>().altezzaCentroUnità + 0.25f, num) + centroPerEstrazione;
							}
							else if (i == 1)
							{
								position = new Vector3(4.2f, list[j].GetComponent<PresenzaAlleato>().altezzaCentroUnità + 0.25f, 4.2f) + centroPerEstrazione;
							}
							else if (i == 2)
							{
								position = new Vector3(num, list[j].GetComponent<PresenzaAlleato>().altezzaCentroUnità + 0.25f, 0f) + centroPerEstrazione;
							}
							else if (i == 3)
							{
								position = new Vector3(4.2f, list[j].GetComponent<PresenzaAlleato>().altezzaCentroUnità + 0.25f, -4.2f) + centroPerEstrazione;
							}
							else if (i == 4)
							{
								position = new Vector3(0f, list[j].GetComponent<PresenzaAlleato>().altezzaCentroUnità + 0.25f, -num) + centroPerEstrazione;
							}
							else if (i == 5)
							{
								position = new Vector3(-4.2f, list[j].GetComponent<PresenzaAlleato>().altezzaCentroUnità + 0.25f, -4.2f) + centroPerEstrazione;
							}
							else if (i == 6)
							{
								position = new Vector3(-num, list[j].GetComponent<PresenzaAlleato>().altezzaCentroUnità + 0.25f, 0f) + centroPerEstrazione;
							}
							else if (i == 7)
							{
								position = new Vector3(-4.2f, list[j].GetComponent<PresenzaAlleato>().altezzaCentroUnità + 0.25f, 4.2f) + centroPerEstrazione;
							}
							GameObject gameObject = UnityEngine.Object.Instantiate(list[j], position, Quaternion.identity) as GameObject;
						}
					}
				}
			}
			else
			{
				Vector3 centroPerEstrazione2 = this.varieMappaLocale.GetComponent<VarieMappaLocale>().centroPerEstrazione;
				float num3 = 6f;
				for (int k = 0; k < 8; k++)
				{
					GameObject gameObject2 = null;
					bool flag = false;
					int num4 = 0;
					while (num4 < 8 && !flag)
					{
						if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num4][0] < 15 && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num4][1] > 0)
						{
							flag = true;
							gameObject2 = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiPossibili[this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num4][0]];
							List<int> list2;
							List<int> expr_3BD = list2 = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num4];
							int num5;
							int expr_3C1 = num5 = 1;
							num5 = list2[num5];
							expr_3BD[expr_3C1] = num5 - 1;
						}
						num4++;
					}
					Vector3 position2 = Vector3.zero;
					if (k == 0)
					{
						position2 = new Vector3(0f, gameObject2.GetComponent<PresenzaAlleato>().altezzaCentroUnità + 0.25f, num3) + centroPerEstrazione2;
					}
					else if (k == 1)
					{
						position2 = new Vector3(4.2f, gameObject2.GetComponent<PresenzaAlleato>().altezzaCentroUnità + 0.25f, 4.2f) + centroPerEstrazione2;
					}
					else if (k == 2)
					{
						position2 = new Vector3(num3, gameObject2.GetComponent<PresenzaAlleato>().altezzaCentroUnità + 0.25f, 0f) + centroPerEstrazione2;
					}
					else if (k == 3)
					{
						position2 = new Vector3(4.2f, gameObject2.GetComponent<PresenzaAlleato>().altezzaCentroUnità + 0.25f, -4.2f) + centroPerEstrazione2;
					}
					else if (k == 4)
					{
						position2 = new Vector3(0f, gameObject2.GetComponent<PresenzaAlleato>().altezzaCentroUnità + 0.25f, -num3) + centroPerEstrazione2;
					}
					else if (k == 5)
					{
						position2 = new Vector3(-4.2f, gameObject2.GetComponent<PresenzaAlleato>().altezzaCentroUnità + 0.25f, -4.2f) + centroPerEstrazione2;
					}
					else if (k == 6)
					{
						position2 = new Vector3(-num3, gameObject2.GetComponent<PresenzaAlleato>().altezzaCentroUnità + 0.25f, 0f) + centroPerEstrazione2;
					}
					else if (k == 7)
					{
						position2 = new Vector3(-4.2f, gameObject2.GetComponent<PresenzaAlleato>().altezzaCentroUnità + 0.25f, 4.2f) + centroPerEstrazione2;
					}
					GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject2, position2, Quaternion.identity) as GameObject;
				}
			}
		}
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x001250F8 File Offset: 0x001232F8
	private void GestioneTempo()
	{
		if (this.infoAlleati.GetComponent<GestioneComandanteInUI>().fineCountdown)
		{
			this.timerGiocoEffet += Time.deltaTime;
			this.timerPuntiBatt += Time.deltaTime;
		}
		if (this.timerGiocoEffet < this.tempoMaxPartita)
		{
			float num = this.tempoMaxPartita - this.timerGiocoEffet;
			int num2 = Mathf.FloorToInt(num / 60f);
			int num3 = Mathf.RoundToInt(num - (float)(num2 * 60));
			this.scrittaTimerBattagliaUI.GetComponent<Text>().text = num2.ToString("00") + " : " + num3.ToString("00");
			if (this.timerPuntiBatt > this.tempoPuntiBatt)
			{
				this.timerPuntiBatt = 0f;
				this.puntiBattaglia += this.incrementoPuntiBatt;
			}
		}
		else
		{
			this.scrittaTimerBattagliaUI.GetComponent<Text>().text = "00 : 00";
		}
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x001251F8 File Offset: 0x001233F8
	private void GestioneAreeSchieramento()
	{
		if (this.scattoInSchierAttivo)
		{
			foreach (GameObject current in this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento)
			{
				current.GetComponent<MeshRenderer>().enabled = true;
			}
			this.schieramentoAttivo = true;
			this.scattoInSchierAttivo = false;
		}
		else if (this.scattoInSchierNonAttivo)
		{
			foreach (GameObject current2 in this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento)
			{
				current2.GetComponent<MeshRenderer>().enabled = false;
			}
			this.schieramentoAttivo = false;
			this.scattoInSchierNonAttivo = false;
		}
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x00125308 File Offset: 0x00123508
	private void GestioneBattagliePerTipo()
	{
		if (this.tipoBattaglia == 0)
		{
			this.BattagliaTipo0();
		}
		else if (this.tipoBattaglia == 1)
		{
			this.BattagliaTipo1();
		}
		else if (this.tipoBattaglia == 2)
		{
			this.BattagliaTipo2();
		}
		else if (this.tipoBattaglia == 3)
		{
			this.BattagliaTipo3();
		}
		else if (this.tipoBattaglia == 4)
		{
			this.BattagliaTipo4();
		}
		else if (this.tipoBattaglia == 5)
		{
			this.BattagliaTipo5();
		}
		else if (this.tipoBattaglia == 6)
		{
			this.BattagliaTipo6();
		}
		else if (this.tipoBattaglia == 7)
		{
			this.BattagliaTipo7();
		}
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x001253C8 File Offset: 0x001235C8
	private void BattagliaTipo0()
	{
		float num = this.avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita / this.avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vitaIniziale;
		this.battagliaTipo0UI.transform.GetChild(2).GetComponent<Image>().fillAmount = num;
		this.battagliaTipo0UI.transform.GetChild(3).GetComponent<Text>().text = (num * 100f).ToString("F0") + "%";
		if (!this.termineInAnticipo)
		{
			if (this.avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita <= 0f)
			{
				this.battagliaTerminata = true;
				GestoreNeutroStrategia.vincitore = 2;
				this.avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita = 0f;
			}
			else if (this.timerGiocoEffet > this.tempoMaxPartita && this.avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
			{
				this.battagliaTerminata = true;
				GestoreNeutroStrategia.vincitore = 1;
			}
		}
		else
		{
			this.battagliaTerminata = true;
			GestoreNeutroStrategia.vincitore = 2;
		}
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x001254E4 File Offset: 0x001236E4
	private void BattagliaTipo1()
	{
		float num = this.avampostoNemico.GetComponent<ObbiettivoTatticoScript>().vita / this.avampostoNemico.GetComponent<ObbiettivoTatticoScript>().vitaIniziale;
		this.battagliaTipo1UI.transform.GetChild(2).GetComponent<Image>().fillAmount = num;
		this.battagliaTipo1UI.transform.GetChild(3).GetComponent<Text>().text = (num * 100f).ToString("F0") + "%";
		if (!this.termineInAnticipo)
		{
			if (this.avampostoNemico.GetComponent<ObbiettivoTatticoScript>().vita <= 0f)
			{
				this.battagliaTerminata = true;
				GestoreNeutroStrategia.vincitore = 1;
				this.avampostoNemico.GetComponent<ObbiettivoTatticoScript>().vita = 0f;
			}
			else if (this.timerGiocoEffet > this.tempoMaxPartita && this.avampostoNemico.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
			{
				this.battagliaTerminata = true;
				GestoreNeutroStrategia.vincitore = 2;
			}
		}
		else
		{
			this.battagliaTerminata = true;
			GestoreNeutroStrategia.vincitore = 2;
		}
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x00125600 File Offset: 0x00123800
	private void BattagliaTipo2()
	{
		for (int i = 0; i < 5; i++)
		{
			int num = 0;
			int num2 = 0;
			foreach (GameObject current in this.ListaBandiereConq[i].GetComponent<ColliderObbiettivo>().ListaUnitàInObbiettivo)
			{
				if (current != null && current.tag == "Alleato" && current.GetComponent<PresenzaAlleato>().giàSchierato)
				{
					num++;
				}
				else if (current != null && current.tag == "Nemico")
				{
					num2++;
				}
			}
			if (num2 > num)
			{
				this.ListaPossessoBandiere[i] = 2;
			}
			else if (num2 < num)
			{
				this.ListaPossessoBandiere[i] = 1;
			}
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			if (this.infoAlleati.GetComponent<GestioneComandanteInUI>().evidenziaAlleatiENemici)
			{
				for (int j = 0; j < 5; j++)
				{
					this.ListaBandiereConq[j].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
				}
			}
			else
			{
				for (int k = 0; k < 5; k++)
				{
					this.ListaBandiereConq[k].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
				}
			}
		}
		this.battagliaTipo2UI.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = this.puntiConqAlleati.ToString("F0") + " / " + this.puntConqMassimi.ToString("F0");
		this.battagliaTipo2UI.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = this.puntiConqNemici.ToString("F0") + " / " + this.puntConqMassimi.ToString("F0");
		this.puntiConqTimer += Time.deltaTime;
		if (this.puntiConqTimer > this.puntiConqTempo && this.infoAlleati.GetComponent<GestioneComandanteInUI>().fineCountdown)
		{
			this.puntiConqTimer = 0f;
			for (int l = 0; l < 5; l++)
			{
				if (this.ListaPossessoBandiere[l] == 1)
				{
					this.puntiConqAlleati += this.puntiConqAVolta;
				}
				else if (this.ListaPossessoBandiere[l] == 2)
				{
					this.puntiConqNemici += this.puntiConqAVolta;
				}
			}
		}
		for (int m = 0; m < 5; m++)
		{
			if (this.ListaPossessoBandiere[m] == 0)
			{
				this.ListaBandiereConq[m].transform.GetChild(0).GetComponent<MeshRenderer>().material = this.matBandieraNeutra;
			}
			else if (this.ListaPossessoBandiere[m] == 1)
			{
				this.ListaBandiereConq[m].transform.GetChild(0).GetComponent<MeshRenderer>().material = this.matBandieraAlleata;
			}
			else if (this.ListaPossessoBandiere[m] == 2)
			{
				this.ListaBandiereConq[m].transform.GetChild(0).GetComponent<MeshRenderer>().material = this.matBandieraNemica;
			}
		}
		if (!this.termineInAnticipo)
		{
			if (this.puntiConqAlleati >= this.puntConqMassimi)
			{
				this.battagliaTerminata = true;
				GestoreNeutroStrategia.vincitore = 1;
			}
			else if (this.puntiConqNemici >= this.puntConqMassimi)
			{
				this.battagliaTerminata = true;
				GestoreNeutroStrategia.vincitore = 2;
			}
			else if (this.timerGiocoEffet > this.tempoMaxPartita)
			{
				if (this.puntiConqAlleati >= this.puntiConqNemici)
				{
					this.battagliaTerminata = true;
					GestoreNeutroStrategia.vincitore = 1;
				}
				else
				{
					this.battagliaTerminata = true;
					GestoreNeutroStrategia.vincitore = 2;
				}
			}
		}
		else
		{
			this.battagliaTerminata = true;
			GestoreNeutroStrategia.vincitore = 2;
		}
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x00125A60 File Offset: 0x00123C60
	private void BattagliaTipo3()
	{
		this.battagliaTipo3UI.transform.GetChild(1).GetComponent<Text>().text = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèFanteria.Count.ToString();
		if (this.timerGiocoEffet > this.tempoMaxPartita && !this.heliPerEstrazCreato)
		{
			this.heliPerEstraz = (UnityEngine.Object.Instantiate(this.heliPerEstrazPrefab, this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[0].transform.position + Vector3.up * 70f, Quaternion.identity) as GameObject);
			this.heliPerEstraz.transform.forward = this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[0].transform.forward;
			this.heliPerEstrazCreato = true;
		}
		if (!this.termineInAnticipo)
		{
			if (this.heliPerEstrazCreato && this.heliPerEstraz.GetComponent<HeliPerEstrazione>().heliInSalvo)
			{
				this.battagliaTerminata = true;
				GestoreNeutroStrategia.vincitore = 1;
			}
			else if ((!this.heliPerEstrazCreato || !this.heliPerEstraz.GetComponent<HeliPerEstrazione>().caricoPreso) && this.faseSchierInizTerminata && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèFanteria.Count <= 0)
			{
				this.battagliaTerminata = true;
				GestoreNeutroStrategia.vincitore = 2;
			}
		}
		else
		{
			this.battagliaTerminata = true;
			GestoreNeutroStrategia.vincitore = 2;
		}
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x00125BEC File Offset: 0x00123DEC
	private void BattagliaTipo4()
	{
		float num = this.cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita / this.cassaSupply.GetComponent<ObbiettivoTatticoScript>().vitaIniziale;
		this.battagliaTipo4UI.transform.GetChild(2).GetComponent<Image>().fillAmount = num;
		this.battagliaTipo4UI.transform.GetChild(3).GetComponent<Text>().text = (num * 100f).ToString("F0") + "%";
		if (this.timerGiocoEffet >= this.tempoMaxPartita && !this.heliPerEstrazCreato)
		{
			this.heliPerEstraz = (UnityEngine.Object.Instantiate(this.heliPerEstrazPrefab, this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[0].transform.position + Vector3.up * 70f, Quaternion.identity) as GameObject);
			this.heliPerEstraz.transform.forward = this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[0].transform.forward;
			this.heliPerEstrazCreato = true;
		}
		if (!this.termineInAnticipo)
		{
			if (this.heliPerEstrazCreato && this.heliPerEstraz.GetComponent<HeliPerEstrazione>().heliInSalvo)
			{
				this.battagliaTerminata = true;
				GestoreNeutroStrategia.vincitore = 1;
			}
			else if ((!this.heliPerEstrazCreato || !this.heliPerEstraz.GetComponent<HeliPerEstrazione>().caricoPreso) && this.cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita <= 0f)
			{
				this.battagliaTerminata = true;
				GestoreNeutroStrategia.vincitore = 2;
			}
		}
		else
		{
			this.battagliaTerminata = true;
			GestoreNeutroStrategia.vincitore = 2;
		}
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x00125DA8 File Offset: 0x00123FA8
	private void BattagliaTipo5()
	{
		float num = this.satellite.GetComponent<ObbiettivoTatticoScript>().vita / this.satellite.GetComponent<ObbiettivoTatticoScript>().vitaIniziale;
		this.battagliaTipo5UI.transform.GetChild(2).GetComponent<Image>().fillAmount = num;
		this.battagliaTipo5UI.transform.GetChild(3).GetComponent<Text>().text = (num * 100f).ToString("F0") + "%";
		this.satellite.transform.RotateAround(this.puntoPerSatellite, base.transform.up, 5f * Time.deltaTime);
		this.satellite.transform.Rotate(Vector3.up * 7f * Time.deltaTime);
		if (!this.termineInAnticipo)
		{
			if (this.satellite.GetComponent<ObbiettivoTatticoScript>().vita <= 0f)
			{
				this.battagliaTerminata = true;
				GestoreNeutroStrategia.vincitore = 2;
				this.satellite.GetComponent<ObbiettivoTatticoScript>().vita = 0f;
			}
			else if (this.timerGiocoEffet > this.tempoMaxPartita && this.satellite.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
			{
				this.battagliaTerminata = true;
				GestoreNeutroStrategia.vincitore = 1;
			}
		}
		else
		{
			this.battagliaTerminata = true;
			GestoreNeutroStrategia.vincitore = 2;
		}
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x00125F18 File Offset: 0x00124118
	private void BattagliaTipo6()
	{
		this.camionArrivati = 0;
		this.camionDistrutti = 0;
		for (int i = 0; i < 4; i++)
		{
			if (i < this.ListaCamionPerConvoglio.Count)
			{
				if (this.ListaCamionPerConvoglio[i] != null)
				{
					if (this.ListaCamionPerConvoglio[i].GetComponent<ObbiettivoTatticoScript>().vita > 0f)
					{
						this.battagliaTipo6UI.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = (this.ListaCamionPerConvoglio[i].GetComponent<ObbiettivoTatticoScript>().vita / this.ListaCamionPerConvoglio[i].GetComponent<ObbiettivoTatticoScript>().vitaIniziale * 100f).ToString("F0") + "%";
					}
					else
					{
						this.battagliaTipo6UI.transform.GetChild(i).GetComponent<Image>().color = Color.red;
						this.battagliaTipo6UI.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "0%";
						this.camionDistrutti++;
					}
				}
				else
				{
					this.battagliaTipo6UI.transform.GetChild(i).GetComponent<Image>().color = Color.cyan;
					this.camionArrivati++;
				}
			}
		}
		for (int j = 0; j < this.ListaCamionPerConvoglio.Count; j++)
		{
			if (this.ListaCamionPerConvoglio[j] != null)
			{
				Vector3 normalized = (this.ListaCamionPerConvoglio[j].transform.position - this.ListaCamere[0].GetComponent<PrimaCamera>().oggettoCameraAttiva.transform.position).normalized;
				this.ListaCamionPerConvoglio[j].transform.GetChild(2).forward = normalized;
				if (this.ListaCamionPerConvoglio[j].GetComponent<ObbiettivoTatticoScript>().vita > 0f)
				{
					this.ListaCamionPerConvoglio[j].transform.GetChild(2).GetComponent<TextMesh>().text = this.ListaCamionPerConvoglio[j].GetComponent<ObbiettivoTatticoScript>().vita.ToString("F0") + " / " + this.ListaCamionPerConvoglio[j].GetComponent<ObbiettivoTatticoScript>().vitaIniziale.ToString("F0");
				}
				else
				{
					this.ListaCamionPerConvoglio[j].transform.GetChild(2).GetComponent<TextMesh>().text = "0 / " + this.ListaCamionPerConvoglio[j].GetComponent<ObbiettivoTatticoScript>().vitaIniziale.ToString("F0");
				}
			}
		}
		if (!this.termineInAnticipo)
		{
			if (this.timerGiocoEffet >= this.tempoMaxPartita)
			{
				if (this.camionArrivati > 0)
				{
					this.battagliaTerminata = true;
					GestoreNeutroStrategia.vincitore = 1;
					GestoreNeutroStrategia.convogliArrivati = this.camionArrivati;
				}
				else
				{
					this.battagliaTerminata = true;
					GestoreNeutroStrategia.vincitore = 2;
				}
			}
			else if (this.camionArrivati == 4)
			{
				this.battagliaTerminata = true;
				GestoreNeutroStrategia.vincitore = 1;
				GestoreNeutroStrategia.convogliArrivati = this.camionArrivati;
			}
			else if (this.camionDistrutti == 4)
			{
				this.battagliaTerminata = true;
				GestoreNeutroStrategia.vincitore = 2;
			}
			else if (this.camionArrivati + this.camionDistrutti == this.ListaCamionPerConvoglio.Count && this.camionArrivati > 0)
			{
				this.battagliaTerminata = true;
				GestoreNeutroStrategia.vincitore = 1;
				GestoreNeutroStrategia.convogliArrivati = this.camionArrivati;
			}
		}
		else
		{
			this.battagliaTerminata = true;
			GestoreNeutroStrategia.vincitore = 2;
		}
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x001262E8 File Offset: 0x001244E8
	private void BattagliaTipo7()
	{
		this.paniArrivati = 0;
		this.paniDistrutti = 0;
		for (int i = 0; i < 5; i++)
		{
			if (i < this.ListaPanePerConvoglio.Count)
			{
				if (this.ListaPanePerConvoglio[i] != null)
				{
					if (this.ListaPanePerConvoglio[i].GetComponent<ObbiettivoTatticoScript>().vita > 0f)
					{
						this.battagliaTipo7UI.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = (this.ListaPanePerConvoglio[i].GetComponent<ObbiettivoTatticoScript>().vita / this.ListaPanePerConvoglio[i].GetComponent<ObbiettivoTatticoScript>().vitaIniziale * 100f).ToString("F0") + "%";
					}
					else
					{
						this.battagliaTipo7UI.transform.GetChild(i).GetComponent<Image>().color = Color.black;
						this.battagliaTipo7UI.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "0%";
						this.paniDistrutti++;
					}
				}
				else
				{
					this.battagliaTipo7UI.transform.GetChild(i).GetComponent<Image>().color = Color.yellow;
					this.paniArrivati++;
				}
			}
		}
		for (int j = 0; j < this.ListaPanePerConvoglio.Count; j++)
		{
			if (this.ListaPanePerConvoglio[j] != null)
			{
				Vector3 normalized = (this.ListaPanePerConvoglio[j].transform.position - this.ListaCamere[0].GetComponent<PrimaCamera>().oggettoCameraAttiva.transform.position).normalized;
				this.ListaPanePerConvoglio[j].transform.GetChild(2).forward = normalized;
				if (this.ListaPanePerConvoglio[j].GetComponent<ObbiettivoTatticoScript>().vita > 0f)
				{
					this.ListaPanePerConvoglio[j].transform.GetChild(2).GetComponent<TextMesh>().text = this.ListaPanePerConvoglio[j].GetComponent<ObbiettivoTatticoScript>().vita.ToString("F0") + " / " + this.ListaPanePerConvoglio[j].GetComponent<ObbiettivoTatticoScript>().vitaIniziale.ToString("F0");
				}
				else
				{
					this.ListaPanePerConvoglio[j].transform.GetChild(2).GetComponent<TextMesh>().text = "0 / " + this.ListaPanePerConvoglio[j].GetComponent<ObbiettivoTatticoScript>().vitaIniziale.ToString("F0");
				}
			}
		}
		if (!this.termineInAnticipo)
		{
			if (this.timerGiocoEffet >= this.tempoMaxPartita)
			{
				if (this.paniArrivati == 0)
				{
					this.battagliaTerminata = true;
					GestoreNeutroStrategia.vincitore = 1;
				}
				else
				{
					this.battagliaTerminata = true;
					GestoreNeutroStrategia.vincitore = 2;
					GestoreNeutroStrategia.convogliArrivati = this.paniArrivati;
				}
			}
			else if (this.paniArrivati == this.ListaPanePerConvoglio.Count)
			{
				this.battagliaTerminata = true;
				GestoreNeutroStrategia.vincitore = 2;
				GestoreNeutroStrategia.convogliArrivati = this.paniArrivati;
			}
			else if (this.paniDistrutti == 5)
			{
				this.battagliaTerminata = true;
				GestoreNeutroStrategia.vincitore = 1;
			}
		}
		else
		{
			this.battagliaTerminata = true;
			GestoreNeutroStrategia.vincitore = 2;
		}
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x00126678 File Offset: 0x00124878
	private void BattagliaTerminata()
	{
		if (this.oggettoMusica)
		{
			if (!this.oggettoMusica.GetComponent<MusicaScript>().musica.isPlaying)
			{
				if (GestoreNeutroStrategia.vincitore == 1)
				{
					this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica = 2;
				}
				else if (GestoreNeutroStrategia.vincitore == 2)
				{
					this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica = 3;
				}
				this.oggettoMusica.GetComponent<MusicaScript>().attivitàInMusica = true;
				this.oggettoMusica.GetComponent<MusicaScript>().musica.volume = PlayerPrefs.GetFloat("volume musica tattica");
			}
			else if (this.oggettoMusica.GetComponent<MusicaScript>().musica.isPlaying && this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica != 2 && this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica != 3)
			{
				if (GestoreNeutroStrategia.vincitore == 1)
				{
					this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica = 2;
				}
				else if (GestoreNeutroStrategia.vincitore == 2)
				{
					this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica = 3;
				}
				this.oggettoMusica.GetComponent<MusicaScript>().attivitàInMusica = true;
				this.oggettoMusica.GetComponent<MusicaScript>().musica.volume = PlayerPrefs.GetFloat("volume musica tattica");
			}
		}
		if (GestoreNeutroStrategia.vincitore == 1)
		{
			this.primaCamera.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
			this.primaCamera.GetComponent<GestioneSuoniCamera>().numListaVoceSelez = 14;
		}
		else if (GestoreNeutroStrategia.vincitore == 2)
		{
			this.primaCamera.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
			this.primaCamera.GetComponent<GestioneSuoniCamera>().numListaVoceSelez = 15;
		}
		if (GestoreNeutroTattica.èBattagliaVeloce)
		{
			if (!this.risultatiMostrati && this.timerFine > 0.5f)
			{
				this.risultatiMostrati = true;
				this.esitoBattaVeloceUI.GetComponent<CanvasGroup>().alpha = 1f;
				this.esitoBattaVeloceUI.GetComponent<CanvasGroup>().interactable = true;
				this.esitoBattaVeloceUI.GetComponent<CanvasGroup>().blocksRaycasts = true;
				this.esitoBattaVeloceUI.transform.FindChild("perdite").GetChild(0).GetChild(0).GetComponent<Text>().text = "DEAD ALLIES:\n" + GestoreNeutroTattica.numAlleatiMorti.ToString();
				this.esitoBattaVeloceUI.transform.FindChild("perdite").GetChild(0).GetChild(1).GetComponent<Text>().text = "DEAD ENEMIES:\n" + GestoreNeutroTattica.numNemiciMorti.ToString();
				if (GestoreNeutroStrategia.vincitore == 1)
				{
					this.esitoBattaVeloceUI.transform.FindChild("sfondo immagine esito").GetComponent<Image>().color = Color.blue;
					this.esitoBattaVeloceUI.transform.FindChild("sfondo immagine esito").GetChild(0).GetComponent<Image>().sprite = this.scrittaVittoria;
				}
				else if (GestoreNeutroStrategia.vincitore == 2)
				{
					this.esitoBattaVeloceUI.transform.FindChild("sfondo immagine esito").GetComponent<Image>().color = Color.red;
					this.esitoBattaVeloceUI.transform.FindChild("sfondo immagine esito").GetChild(0).GetComponent<Image>().sprite = this.scrittaSconfitta;
				}
				for (int i = 0; i < 48; i++)
				{
					bool flag = false;
					int num = 0;
					while (num < 48 && !flag)
					{
						if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAllPresInBatt[num] == 1)
						{
							flag = true;
							this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAllPresInBatt[num] = 0;
							this.esBatVelListaAlleati.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 1f;
							this.esBatVelListaAlleati.transform.GetChild(i).GetComponent<CanvasGroup>().interactable = true;
							this.esBatVelListaAlleati.transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = true;
							this.esBatVelListaAlleati.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiPossibili[num].GetComponent<PresenzaAlleato>().immagineUnità;
							this.esBatVelListaAlleati.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiPossibili[num].GetComponent<PresenzaAlleato>().nomeUnità;
							this.esBatVelListaAlleati.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = "REMAINING: " + this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiPostBattaglia[num].ToString();
							this.esBatVelListaAlleati.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = "DMG: " + this.ListaDanniAlleati[num].ToString("F0") + " HP";
						}
						num++;
					}
				}
				for (int j = 0; j < 33; j++)
				{
					bool flag2 = false;
					int num2 = 0;
					while (num2 < 33 && !flag2)
					{
						if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemPresInBatt[num2] == 1)
						{
							flag2 = true;
							this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemPresInBatt[num2] = 0;
							this.esBatVelListaNemici.transform.GetChild(j).GetComponent<CanvasGroup>().alpha = 1f;
							this.esBatVelListaNemici.transform.GetChild(j).GetComponent<CanvasGroup>().interactable = true;
							this.esBatVelListaNemici.transform.GetChild(j).GetComponent<CanvasGroup>().blocksRaycasts = true;
							this.esBatVelListaNemici.transform.GetChild(j).GetChild(0).GetComponent<Image>().sprite = this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciPossibili[num2].GetComponent<PresenzaNemico>().immagineInsetto;
							this.esBatVelListaNemici.transform.GetChild(j).GetChild(1).GetComponent<Text>().text = this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciPossibili[num2].GetComponent<PresenzaNemico>().nomeInsetto;
							this.esBatVelListaNemici.transform.GetChild(j).GetChild(2).GetComponent<Text>().text = "REMAINING " + this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciPostBattaglia[num2].ToString();
							this.esBatVelListaNemici.transform.GetChild(j).GetChild(3).GetComponent<Text>().text = "DMG: " + this.ListaDanniNemici[num2].ToString("F0") + " HP";
						}
						num2++;
					}
				}
			}
		}
		else
		{
			if (this.tipoBattaglia == 5)
			{
				GestoreNeutroStrategia.controlloSatellite = true;
			}
			if (GestoreNeutroStrategia.vincitore == 1)
			{
				this.esitoBattagliaUI.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1f;
				this.esitoBattagliaUI.transform.GetChild(0).GetComponent<CanvasGroup>().interactable = true;
				this.esitoBattagliaUI.transform.GetChild(0).GetComponent<CanvasGroup>().blocksRaycasts = true;
			}
			else if (GestoreNeutroStrategia.vincitore == 2)
			{
				this.esitoBattagliaUI.transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 1f;
				this.esitoBattagliaUI.transform.GetChild(1).GetComponent<CanvasGroup>().interactable = true;
				this.esitoBattagliaUI.transform.GetChild(1).GetComponent<CanvasGroup>().blocksRaycasts = true;
			}
			GestoreNeutroStrategia.inTattica = false;
			CaricaDati.InizializConCaricamStrategia = true;
			CaricaScene.nomeScenaDaCaricare = CaricaScene.nomeScenaCasaPerRitornoAStrategia;
		}
	}

	// Token: 0x04001F2C RID: 7980
	private GameObject infoAlleati;

	// Token: 0x04001F2D RID: 7981
	private GameObject IANemico;

	// Token: 0x04001F2E RID: 7982
	private GameObject varieMappaLocale;

	// Token: 0x04001F2F RID: 7983
	private GameObject primaCamera;

	// Token: 0x04001F30 RID: 7984
	private GameObject secondaCamera;

	// Token: 0x04001F31 RID: 7985
	private GameObject scrittaTimerBattagliaUI;

	// Token: 0x04001F32 RID: 7986
	private GameObject scrittaDescrTipoBatt;

	// Token: 0x04001F33 RID: 7987
	private GameObject battagliaTipo0UI;

	// Token: 0x04001F34 RID: 7988
	private GameObject battagliaTipo1UI;

	// Token: 0x04001F35 RID: 7989
	private GameObject battagliaTipo2UI;

	// Token: 0x04001F36 RID: 7990
	private GameObject battagliaTipo3UI;

	// Token: 0x04001F37 RID: 7991
	private GameObject battagliaTipo4UI;

	// Token: 0x04001F38 RID: 7992
	private GameObject battagliaTipo5UI;

	// Token: 0x04001F39 RID: 7993
	private GameObject battagliaTipo6UI;

	// Token: 0x04001F3A RID: 7994
	private GameObject battagliaTipo7UI;

	// Token: 0x04001F3B RID: 7995
	private GameObject esitoBattagliaUI;

	// Token: 0x04001F3C RID: 7996
	private GameObject esitoBattaVeloceUI;

	// Token: 0x04001F3D RID: 7997
	private GameObject esBatVelListaAlleati;

	// Token: 0x04001F3E RID: 7998
	private GameObject esBatVelListaNemici;

	// Token: 0x04001F3F RID: 7999
	private GameObject inizioLivello;

	// Token: 0x04001F40 RID: 8000
	private GameObject pulsVinciBattAnticipo;

	// Token: 0x04001F41 RID: 8001
	public float puntiBattaglia;

	// Token: 0x04001F42 RID: 8002
	private float tempoPuntiBatt;

	// Token: 0x04001F43 RID: 8003
	private float timerPuntiBatt;

	// Token: 0x04001F44 RID: 8004
	private float incrementoPuntiBatt;

	// Token: 0x04001F45 RID: 8005
	public static bool èBattagliaVeloce;

	// Token: 0x04001F46 RID: 8006
	public List<GameObject> ListaCamere;

	// Token: 0x04001F47 RID: 8007
	public bool faseSchierInizTerminata;

	// Token: 0x04001F48 RID: 8008
	public bool schieramentoAttivo;

	// Token: 0x04001F49 RID: 8009
	public bool scattoInSchierAttivo;

	// Token: 0x04001F4A RID: 8010
	public bool scattoInSchierNonAttivo;

	// Token: 0x04001F4B RID: 8011
	public Material coloreSchierAlleato;

	// Token: 0x04001F4C RID: 8012
	public Material coloreSchierNemico;

	// Token: 0x04001F4D RID: 8013
	public Material coloreSchierNeutro;

	// Token: 0x04001F4E RID: 8014
	public int tipoBattaglia;

	// Token: 0x04001F4F RID: 8015
	public GameObject depositoMunizioniPrefab;

	// Token: 0x04001F50 RID: 8016
	private GameObject depositoMunizioni;

	// Token: 0x04001F51 RID: 8017
	public GameObject avampostoAlleatoPrefab;

	// Token: 0x04001F52 RID: 8018
	public GameObject avampostoAlleato;

	// Token: 0x04001F53 RID: 8019
	public GameObject avampostoNemicoPrefab;

	// Token: 0x04001F54 RID: 8020
	public GameObject avampostoNemico;

	// Token: 0x04001F55 RID: 8021
	public GameObject cassaSupplyPrefab;

	// Token: 0x04001F56 RID: 8022
	public GameObject cassaSupply;

	// Token: 0x04001F57 RID: 8023
	public GameObject heliPerEstrazPrefab;

	// Token: 0x04001F58 RID: 8024
	public GameObject heliPerEstraz;

	// Token: 0x04001F59 RID: 8025
	public GameObject satellitePrefab;

	// Token: 0x04001F5A RID: 8026
	public GameObject satellite;

	// Token: 0x04001F5B RID: 8027
	private List<Vector3> ListaPosConvAlleato;

	// Token: 0x04001F5C RID: 8028
	public GameObject camionPerConvoglioPrefab;

	// Token: 0x04001F5D RID: 8029
	public List<GameObject> ListaCamionPerConvoglio;

	// Token: 0x04001F5E RID: 8030
	private List<Vector3> ListaPosConvNemico;

	// Token: 0x04001F5F RID: 8031
	public GameObject panePerConvoglioPrefab;

	// Token: 0x04001F60 RID: 8032
	public List<GameObject> ListaPanePerConvoglio;

	// Token: 0x04001F61 RID: 8033
	public float timerGiocoEffet;

	// Token: 0x04001F62 RID: 8034
	private float tempoMaxPartita;

	// Token: 0x04001F63 RID: 8035
	public GameObject oggDescrObbBattTipo0;

	// Token: 0x04001F64 RID: 8036
	public GameObject oggDescrObbBattTipo1;

	// Token: 0x04001F65 RID: 8037
	public GameObject oggDescrObbBattTipo2;

	// Token: 0x04001F66 RID: 8038
	public GameObject oggDescrObbBattTipo3;

	// Token: 0x04001F67 RID: 8039
	public GameObject oggDescrObbBattTipo4;

	// Token: 0x04001F68 RID: 8040
	public GameObject oggDescrObbBattTipo5;

	// Token: 0x04001F69 RID: 8041
	public GameObject oggDescrObbBattTipo6;

	// Token: 0x04001F6A RID: 8042
	public GameObject oggDescrObbBattTipo7;

	// Token: 0x04001F6B RID: 8043
	public bool battagliaTerminata;

	// Token: 0x04001F6C RID: 8044
	public bool salvaDatiBattaglia;

	// Token: 0x04001F6D RID: 8045
	public List<GameObject> ListaBersagliDinamite;

	// Token: 0x04001F6E RID: 8046
	public Material matBandieraNeutra;

	// Token: 0x04001F6F RID: 8047
	public Material matBandieraNemica;

	// Token: 0x04001F70 RID: 8048
	public Material matBandieraAlleata;

	// Token: 0x04001F71 RID: 8049
	public GameObject bandieraConquistaPrefab;

	// Token: 0x04001F72 RID: 8050
	public List<GameObject> ListaBandiereConq;

	// Token: 0x04001F73 RID: 8051
	public List<int> ListaPossessoBandiere;

	// Token: 0x04001F74 RID: 8052
	private float puntiConqAlleati;

	// Token: 0x04001F75 RID: 8053
	private float puntiConqNemici;

	// Token: 0x04001F76 RID: 8054
	private float puntConqMassimi;

	// Token: 0x04001F77 RID: 8055
	private float puntiConqAVolta;

	// Token: 0x04001F78 RID: 8056
	private float puntiConqTempo;

	// Token: 0x04001F79 RID: 8057
	private float puntiConqTimer;

	// Token: 0x04001F7A RID: 8058
	private List<GameObject> ListaReconSquad;

	// Token: 0x04001F7B RID: 8059
	private bool heliPerEstrazCreato;

	// Token: 0x04001F7C RID: 8060
	private int camionArrivati;

	// Token: 0x04001F7D RID: 8061
	private int camionDistrutti;

	// Token: 0x04001F7E RID: 8062
	private int paniArrivati;

	// Token: 0x04001F7F RID: 8063
	private int paniDistrutti;

	// Token: 0x04001F80 RID: 8064
	private bool areeSchierDecise;

	// Token: 0x04001F81 RID: 8065
	private Vector3 puntoPerSatellite;

	// Token: 0x04001F82 RID: 8066
	private bool primoFramePassato;

	// Token: 0x04001F83 RID: 8067
	public int fattorePuntiBattaglia;

	// Token: 0x04001F84 RID: 8068
	public static int numAlleatiMorti;

	// Token: 0x04001F85 RID: 8069
	public static int numNemiciMorti;

	// Token: 0x04001F86 RID: 8070
	public bool termineInAnticipo;

	// Token: 0x04001F87 RID: 8071
	public List<float> ListaDanniAlleati;

	// Token: 0x04001F88 RID: 8072
	public List<float> ListaDanniNemici;

	// Token: 0x04001F89 RID: 8073
	public float danniDelGiocatore;

	// Token: 0x04001F8A RID: 8074
	private bool risultatiMostrati;

	// Token: 0x04001F8B RID: 8075
	public Sprite scrittaVittoria;

	// Token: 0x04001F8C RID: 8076
	public Sprite scrittaSconfitta;

	// Token: 0x04001F8D RID: 8077
	private float timerFine;

	// Token: 0x04001F8E RID: 8078
	public int soldatiSalvatiInBatt3;

	// Token: 0x04001F8F RID: 8079
	private int livelloNest;

	// Token: 0x04001F90 RID: 8080
	private GameObject oggettoMusica;

	// Token: 0x04001F91 RID: 8081
	public float moltiplicatoreFPSBattVeloce;

	// Token: 0x04001F92 RID: 8082
	private int impostDurataBatt;

	// Token: 0x04001F93 RID: 8083
	public int numeroMaxAlleati;

	// Token: 0x04001F94 RID: 8084
	private float valoreBasePerBattPont;

	// Token: 0x04001F95 RID: 8085
	public float giorniFittizi;
}
