using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000005 RID: 5
public class GestioneComandanteInUI : MonoBehaviour
{
	// Token: 0x06000015 RID: 21 RVA: 0x000033F0 File Offset: 0x000015F0
	private void Start()
	{
		this.CanvasComandante = GameObject.FindGameObjectWithTag("CanvasComandante");
		this.barraUnitàSelez = this.CanvasComandante.transform.GetChild(0).gameObject;
		this.rettPerArmi = this.barraUnitàSelez.transform.FindChild("Rettangolo Centrale").FindChild("rettangolo per armi").gameObject;
		this.rettPerTrappole = this.barraUnitàSelez.transform.FindChild("Rettangolo Centrale").FindChild("rettangolo per trappole").gameObject;
		this.modalitàUnitàSingola = this.barraUnitàSelez.transform.GetChild(0).transform.GetChild(0).gameObject;
		this.modalitàUnitàMultipla = this.barraUnitàSelez.transform.GetChild(0).transform.GetChild(1).gameObject;
		this.datiGenerali = this.barraUnitàSelez.transform.GetChild(0).transform.GetChild(2).gameObject;
		this.arma1 = this.rettPerArmi.transform.GetChild(0).gameObject;
		this.arma2 = this.rettPerArmi.transform.GetChild(1).gameObject;
		this.arma3 = this.rettPerArmi.transform.GetChild(2).gameObject;
		this.arma4 = this.rettPerArmi.transform.GetChild(3).gameObject;
		this.quadratoDestro = this.barraUnitàSelez.transform.GetChild(2).gameObject;
		this.quadroSelezMunizioni = this.quadratoDestro.transform.FindChild("per munizioni").gameObject;
		this.tipoMunizione1 = this.quadroSelezMunizioni.transform.GetChild(1).gameObject;
		this.tipoMunizione2 = this.quadroSelezMunizioni.transform.GetChild(2).gameObject;
		this.tipoMunizione3 = this.quadroSelezMunizioni.transform.GetChild(3).gameObject;
		this.tipoMunizione4 = this.quadroSelezMunizioni.transform.GetChild(4).gameObject;
		this.tipoMunizione5 = this.quadroSelezMunizioni.transform.GetChild(5).gameObject;
		this.sopraRettangolo = this.barraUnitàSelez.transform.GetChild(3).gameObject;
		this.quadratoInformazioniUnità = this.CanvasComandante.transform.FindChild("Informazioni Unità").gameObject;
		this.barraTipolRinforzi = this.CanvasComandante.transform.FindChild("Barra Superiore").FindChild("Barra Rinforzi").FindChild("barra tipologie").gameObject;
		this.elencoUnitàRinforzi = this.CanvasComandante.transform.FindChild("Barra Superiore").FindChild("Barra Rinforzi").FindChild("elenco unità").gameObject;
		this.sfondoQuadratoAerei = this.CanvasComandante.transform.FindChild("Quadro Aerei").FindChild("sfondo quadrato").gameObject;
		this.scritteAvvSchieramento = this.CanvasComandante.transform.FindChild("Scritte Varie").FindChild("avvertenze schieramento").gameObject;
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.barraSuperiore = this.CanvasComandante.transform.FindChild("Barra Superiore").gameObject;
		this.quadroAerei = this.CanvasComandante.transform.FindChild("Quadro Aerei").gameObject;
		this.quadroInfoTrappola = this.quadratoDestro.transform.FindChild("per trappole").gameObject;
		this.inizioBattaglia = this.CanvasComandante.transform.FindChild("Varie Battaglia").FindChild("Sfondo scritta inizio").gameObject;
		this.modalitàTrappola = this.barraUnitàSelez.transform.FindChild("Quadrato Sinistro").transform.FindChild("trappola").gameObject;
		this.scrittaQuotaTrappola = this.modalitàTrappola.transform.FindChild("per quota").FindChild("sfondo testo").GetChild(0).gameObject;
		this.pulsanteAtterra = this.barraUnitàSelez.transform.FindChild("Rettangolo Centrale").FindChild("pulsante atterraggio").gameObject;
		this.schermataParà = this.CanvasComandante.transform.FindChild("Pannello Paracadutisti").gameObject;
		this.elencoPossibiliParà = this.schermataParà.transform.FindChild("elenco unità possibili").gameObject;
		this.elencoParà = this.schermataParà.transform.FindChild("elenco parà").gameObject;
		this.pulsanteProntoAlLancio = this.schermataParà.transform.FindChild("Pronto").gameObject;
		this.pannelloMunizioni = this.barraSuperiore.transform.FindChild("Munizioni").FindChild("sfondo").gameObject;
		this.elencoMunizioniInPannello = this.pannelloMunizioni.transform.FindChild("elenco munizioni").gameObject;
		this.pulsanteFuocoSalvaRockArt = this.barraUnitàSelez.transform.FindChild("Rettangolo Centrale").FindChild("salva Rocket Artillery").gameObject;
		this.spuntaCompDifensivo = this.datiGenerali.transform.GetChild(0).transform.GetChild(2).gameObject;
		this.spuntaCercaAutomBers = this.datiGenerali.transform.GetChild(1).transform.GetChild(2).gameObject;
		this.spuntaCercaBersDif = this.datiGenerali.transform.GetChild(2).transform.GetChild(2).gameObject;
		this.spuntaCercaBersDifVicino = this.datiGenerali.transform.GetChild(3).transform.GetChild(2).gameObject;
		this.sfondoStatistiche = this.CanvasComandante.transform.FindChild("Barra Superiore").FindChild("Statistiche").GetChild(1).gameObject;
		this.contoAllaRovescia = this.CanvasComandante.transform.FindChild("Varie Battaglia").FindChild("Countdown").gameObject;
		this.scrittaAvvertSapper = this.CanvasComandante.transform.FindChild("Scritte Varie").FindChild("avvertimento sapper").gameObject;
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.attacchiSpecialiAlleati = GameObject.FindGameObjectWithTag("Attacchi Speciali Alleati");
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.inizioBattaglia.GetComponent<CanvasGroup>().alpha = 1f;
		this.inizioBattaglia.GetComponent<CanvasGroup>().interactable = true;
		this.inizioBattaglia.GetComponent<CanvasGroup>().blocksRaycasts = true;
		this.contoAllaRovescia.GetComponent<CanvasGroup>().alpha = 1f;
		this.contoAllaRovescia.GetComponent<CanvasGroup>().interactable = true;
		this.contoAllaRovescia.GetComponent<CanvasGroup>().blocksRaycasts = true;
		this.numeroArmaSelezionata = 0;
		this.ListaPosizione0 = new List<GameObject>();
		this.ListaPosizione1 = new List<GameObject>();
		this.ListaPosizione2 = new List<GameObject>();
		this.ListaPosizione3 = new List<GameObject>();
		this.ListaPosizione4 = new List<GameObject>();
		this.ListaPosizione5 = new List<GameObject>();
		this.ListaPosizione6 = new List<GameObject>();
		this.ListaPosizione7 = new List<GameObject>();
		this.ListaPosizione8 = new List<GameObject>();
		this.ListaPosizione9 = new List<GameObject>();
		this.ListaPosizioni = new List<List<GameObject>>();
		this.ListaPosizioni.Add(this.ListaPosizione0);
		this.ListaPosizioni.Add(this.ListaPosizione1);
		this.ListaPosizioni.Add(this.ListaPosizione2);
		this.ListaPosizioni.Add(this.ListaPosizione3);
		this.ListaPosizioni.Add(this.ListaPosizione4);
		this.ListaPosizioni.Add(this.ListaPosizione5);
		this.ListaPosizioni.Add(this.ListaPosizione6);
		this.ListaPosizioni.Add(this.ListaPosizione7);
		this.ListaPosizioni.Add(this.ListaPosizione8);
		this.ListaPosizioni.Add(this.ListaPosizione9);
		this.ListaTipiMunizioni = new List<GameObject>();
		this.ListaTipiMunizioni.Add(this.tipoMunizione1);
		this.ListaTipiMunizioni.Add(this.tipoMunizione2);
		this.ListaTipiMunizioni.Add(this.tipoMunizione3);
		this.ListaTipiMunizioni.Add(this.tipoMunizione4);
		this.ListaTipiMunizioni.Add(this.tipoMunizione5);
		this.ListaArmiUI = new List<GameObject>();
		this.ListaArmiUI.Add(this.arma1);
		this.ListaArmiUI.Add(this.arma2);
		this.ListaArmiUI.Add(this.arma3);
		this.ListaArmiUI.Add(this.arma4);
		this.numMaxAlleati = base.GetComponent<InfoGenericheAlleati>().numMaxAlleati;
		if (!GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.numMaxNemici = PlayerPrefs.GetInt("max nemici");
		}
		else
		{
			this.numMaxNemici = PlayerPrefs.GetInt("batt vel imp numero nemici");
			this.numMaxAlleati = PlayerPrefs.GetInt("batt vel imp numero alleati");
		}
		if (this.tipoBattaglia == 0)
		{
			this.tempoCountdown = 90f;
		}
		else if (this.tipoBattaglia == 1)
		{
			this.tempoCountdown = 20f;
		}
		else if (this.tipoBattaglia == 2)
		{
			this.tempoCountdown = 20f;
		}
		else if (this.tipoBattaglia == 3)
		{
			this.tempoCountdown = 60f;
		}
		else if (this.tipoBattaglia == 4)
		{
			this.tempoCountdown = 60f;
		}
		else if (this.tipoBattaglia == 5)
		{
			this.tempoCountdown = 10f;
		}
		else if (this.tipoBattaglia == 6)
		{
			this.tempoCountdown = 10f;
		}
		else if (this.tipoBattaglia == 7)
		{
			this.tempoCountdown = 10f;
		}
		this.numeroGruppoSchier = 10;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00003E3C File Offset: 0x0000203C
	private void Update()
	{
		if (this.aggMorte)
		{
			this.aggMorte = false;
			this.primaCamera.GetComponent<Selezionamento>().azzeramentoSelezione = true;
		}
		this.ControlloElementiSelezionati();
		this.numeroTipoDiverseSel = this.ListaTipiTruppeSel.Count;
		if (base.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0 && this.numeroTipoDiverseSel > 0)
		{
			this.unitàSelezionata = base.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0];
		}
		else
		{
			this.unitàSelezionata = null;
		}
		if (this.primaCamera.GetComponent<Selezionamento>().trappolaSelez != null)
		{
			this.trappolaSelez = this.primaCamera.GetComponent<Selezionamento>().trappolaSelez;
		}
		else
		{
			this.trappolaSelez = null;
		}
		if (this.primaCamera.GetComponent<Selezionamento>().azzeramentoSelezione)
		{
			this.PuliziaGenerale();
		}
		if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva != 3)
		{
			this.barraUnitàSelez.GetComponent<CanvasGroup>().alpha = 1f;
			this.barraUnitàSelez.GetComponent<CanvasGroup>().interactable = true;
			this.barraUnitàSelez.GetComponent<CanvasGroup>().blocksRaycasts = true;
			this.barraSuperiore.GetComponent<CanvasGroup>().alpha = 1f;
			this.barraSuperiore.GetComponent<CanvasGroup>().interactable = true;
			this.barraSuperiore.GetComponent<CanvasGroup>().blocksRaycasts = true;
			this.quadroAerei.GetComponent<CanvasGroup>().alpha = 1f;
			this.quadroAerei.GetComponent<CanvasGroup>().interactable = true;
			this.quadroAerei.GetComponent<CanvasGroup>().blocksRaycasts = true;
			this.QuadratoSinistro();
			this.RettangoloCentrale();
			this.QuadratoDestro();
			this.SopraRettangolo();
			this.VarieCanvasComandante();
		}
		else
		{
			this.barraUnitàSelez.GetComponent<CanvasGroup>().alpha = 0f;
			this.barraUnitàSelez.GetComponent<CanvasGroup>().interactable = false;
			this.barraUnitàSelez.GetComponent<CanvasGroup>().blocksRaycasts = false;
			this.barraSuperiore.GetComponent<CanvasGroup>().alpha = 0f;
			this.barraSuperiore.GetComponent<CanvasGroup>().interactable = false;
			this.barraSuperiore.GetComponent<CanvasGroup>().blocksRaycasts = false;
			this.quadroAerei.GetComponent<CanvasGroup>().alpha = 0f;
			this.quadroAerei.GetComponent<CanvasGroup>().interactable = false;
			this.quadroAerei.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		this.EvidenziazioneAlleatiENemici();
		if (Input.GetKeyDown(KeyCode.F) && !this.preparLancioParàAttivo)
		{
			this.primaCamera.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
			this.primaCamera.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
			this.primaCamera.GetComponent<AudioSource>().clip = this.primaCamera.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
			if (this.barraTipolRinforzi.GetComponent<CanvasGroup>().alpha == 0f)
			{
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().schieramentoAttivo = true;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().scattoInSchierAttivo = true;
				this.barraTipolRinforzi.GetComponent<CanvasGroup>().alpha = 1f;
				this.barraTipolRinforzi.GetComponent<CanvasGroup>().interactable = true;
				this.barraTipolRinforzi.GetComponent<CanvasGroup>().blocksRaycasts = true;
			}
			else
			{
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().schieramentoAttivo = false;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().scattoInSchierNonAttivo = true;
				this.barraTipolRinforzi.GetComponent<CanvasGroup>().alpha = 0f;
				this.barraTipolRinforzi.GetComponent<CanvasGroup>().interactable = false;
				this.barraTipolRinforzi.GetComponent<CanvasGroup>().blocksRaycasts = false;
				for (int i = 0; i < this.elencoUnitàRinforzi.transform.childCount; i++)
				{
					this.elencoUnitàRinforzi.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 0f;
					this.elencoUnitàRinforzi.transform.GetChild(i).GetComponent<CanvasGroup>().interactable = false;
					this.elencoUnitàRinforzi.transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
				this.chiudiRinforzi = true;
				this.scritteAvvSchieramento.GetComponent<CanvasGroup>().alpha = 0f;
				this.partenzaTimerScrittaAvvSchier = false;
			}
		}
		if (this.barraTipolRinforzi.GetComponent<CanvasGroup>().alpha == 1f)
		{
			this.GestioneRinforzi();
		}
		else if (this.chiudiRinforzi)
		{
			this.chiudiRinforzi = false;
			UnityEngine.Object.Destroy(this.alleatoèDaSchier);
			this.alleatoèDaSchier = null;
			this.alleatoRealeèDaSchier = null;
			this.alleatoDaPosizionare = false;
			this.aggiornaElencoRinforzi = true;
		}
		if (Input.GetKeyDown(KeyCode.G) || this.pulsApriChiudiQAPremuto)
		{
			this.pulsApriChiudiQAPremuto = false;
			this.quadroAereoAperto = !this.quadroAereoAperto;
			this.primaCamera.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
			this.primaCamera.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
			this.primaCamera.GetComponent<AudioSource>().clip = this.primaCamera.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		}
		if (this.quadroAereoAperto)
		{
			this.sfondoQuadratoAerei.GetComponent<CanvasGroup>().alpha = 1f;
			this.sfondoQuadratoAerei.GetComponent<CanvasGroup>().interactable = true;
			this.sfondoQuadratoAerei.GetComponent<CanvasGroup>().blocksRaycasts = true;
			this.QuadratoAereiUI();
		}
		else
		{
			this.sfondoQuadratoAerei.GetComponent<CanvasGroup>().alpha = 0f;
			this.sfondoQuadratoAerei.GetComponent<CanvasGroup>().interactable = false;
			this.sfondoQuadratoAerei.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		if (this.quadratoInformazioniUnità.GetComponent<CanvasGroup>().alpha == 1f)
		{
			this.InfoAlleatoUI();
		}
		if (this.preparLancioParàAttivo || this.annullaLancio)
		{
			this.SchermataLancio();
			this.annullaLancio = false;
		}
		if (this.pannelloMunizioni.GetComponent<CanvasGroup>().alpha == 1f)
		{
			this.timerPannMuniz += Time.deltaTime;
			this.VisualizzaMunizioni();
		}
		if (this.statisticheAperte)
		{
			this.FunzioneStatistiche();
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			this.visualePanoramica = !this.visualePanoramica;
			if (this.visualePanoramica)
			{
				this.CanvasComandante.GetComponent<CanvasGroup>().alpha = 0f;
				this.CanvasComandante.GetComponent<CanvasGroup>().interactable = false;
				this.CanvasComandante.GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
			else
			{
				this.CanvasComandante.GetComponent<CanvasGroup>().alpha = 1f;
				this.CanvasComandante.GetComponent<CanvasGroup>().interactable = true;
				this.CanvasComandante.GetComponent<CanvasGroup>().blocksRaycasts = true;
			}
			this.primaCamera.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
			this.primaCamera.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
			this.primaCamera.GetComponent<AudioSource>().clip = this.primaCamera.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		}
		if (!this.fineCountdown)
		{
			this.FunzioneContoAllaRovescia();
		}
		if (this.partenzaTimerScrittaAvvSchier)
		{
			this.timerScrittaAvvSchier += Time.unscaledDeltaTime;
			this.scritteAvvSchieramento.GetComponent<CanvasGroup>().alpha = 1f;
			if (this.timerScrittaAvvSchier > 1f)
			{
				this.scritteAvvSchieramento.GetComponent<CanvasGroup>().alpha = 0f;
				this.timerScrittaAvvSchier = 0f;
				this.partenzaTimerScrittaAvvSchier = false;
			}
		}
		if (this.partenzaTimerScrAvvSapper)
		{
			this.timerScrittaAvvSapper += Time.unscaledDeltaTime;
			this.scrittaAvvertSapper.GetComponent<CanvasGroup>().alpha = 1f;
			if (this.timerScrittaAvvSapper > 1f)
			{
				this.scrittaAvvertSapper.GetComponent<CanvasGroup>().alpha = 0f;
				this.timerScrittaAvvSapper = 0f;
				this.partenzaTimerScrAvvSapper = false;
			}
		}
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00004604 File Offset: 0x00002804
	private void FunzioneContoAllaRovescia()
	{
		if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().faseSchierInizTerminata)
		{
			this.timerCountdown += Time.deltaTime;
			this.contoAllaRovescia.transform.GetChild(0).GetComponent<Text>().text = (this.tempoCountdown - this.timerCountdown).ToString("F1") + "s";
			if (this.timerCountdown > this.tempoCountdown)
			{
				this.fineCountdown = true;
				this.contoAllaRovescia.GetComponent<CanvasGroup>().alpha = 0f;
				this.contoAllaRovescia.GetComponent<CanvasGroup>().interactable = false;
				this.contoAllaRovescia.GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
			else
			{
				this.contoAllaRovescia.GetComponent<CanvasGroup>().interactable = true;
			}
		}
		else
		{
			this.contoAllaRovescia.transform.GetChild(0).GetComponent<Text>().text = this.tempoCountdown.ToString("F1") + "s";
			this.contoAllaRovescia.GetComponent<CanvasGroup>().interactable = false;
		}
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00004728 File Offset: 0x00002928
	private void ControlloElementiSelezionati()
	{
		if (this.primaCamera.GetComponent<Selezionamento>().azzeramentoSelezione)
		{
			this.ListaTipiTruppeSel.Clear();
			for (int i = 0; i < 10; i++)
			{
				this.ListaPosizioni[i].Clear();
			}
			this.numeroTipoDiverseSel = 0;
			this.modalitàUnitàSingola.GetComponent<CanvasGroup>().alpha = 0f;
			this.modalitàUnitàSingola.GetComponent<CanvasGroup>().interactable = false;
			this.modalitàUnitàSingola.GetComponent<CanvasGroup>().blocksRaycasts = false;
			this.modalitàUnitàMultipla.GetComponent<CanvasGroup>().alpha = 0f;
			this.modalitàUnitàMultipla.GetComponent<CanvasGroup>().interactable = false;
			this.modalitàUnitàMultipla.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		if (base.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0)
		{
			if (this.ListaTipiTruppeSel.Count == 0)
			{
				this.ListaTipiTruppeSel.Add(null);
				this.ListaTipiTruppeSel[0] = base.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0];
				this.numeroTipoDiverseSel = 1;
			}
			if (!this.ListaPosizione0.Contains(base.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0]))
			{
				this.ListaPosizione0.Add(base.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0]);
			}
		}
		for (int j = 1; j < base.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count; j++)
		{
			bool flag = false;
			if (base.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[j].GetComponent<PresenzaAlleato>().tipoTruppa == this.ListaTipiTruppeSel[0].GetComponent<PresenzaAlleato>().tipoTruppa)
			{
				if (!this.ListaPosizione0.Contains(base.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[j]))
				{
					this.ListaPosizione0.Add(base.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[j]);
				}
				flag = true;
			}
			for (int k = 1; k < 10; k++)
			{
				if (!flag && this.ListaTipiTruppeSel.Count < k + 1)
				{
					this.ListaTipiTruppeSel.Add(base.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[j]);
					flag = true;
				}
				if (!flag && this.ListaTipiTruppeSel[k] == null)
				{
					this.ListaPosizioni[k].Add(base.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[j]);
					flag = true;
				}
				else if (!flag && this.ListaTipiTruppeSel[k] != null && base.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[j].GetComponent<PresenzaAlleato>().tipoTruppa == this.ListaTipiTruppeSel[k].GetComponent<PresenzaAlleato>().tipoTruppa)
				{
					if (!this.ListaPosizioni[k].Contains(base.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[j]))
					{
						this.ListaPosizioni[k].Add(base.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[j]);
					}
					flag = true;
				}
			}
		}
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00004A48 File Offset: 0x00002C48
	private void PuliziaGenerale()
	{
		this.numeroArmaSelezionata = 0;
		for (int i = 0; i < 4; i++)
		{
			this.ListaArmiUI[i].GetComponent<CanvasGroup>().alpha = 0f;
			this.ListaArmiUI[i].GetComponent<CanvasGroup>().interactable = false;
			this.ListaArmiUI[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		for (int j = 0; j < 5; j++)
		{
			this.ListaTipiMunizioni[j].GetComponent<CanvasGroup>().alpha = 0f;
			this.ListaTipiMunizioni[j].GetComponent<CanvasGroup>().interactable = false;
			this.ListaTipiMunizioni[j].GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00004B14 File Offset: 0x00002D14
	private void QuadratoSinistro()
	{
		if (this.ListaTipiTruppeSel.Count > 0 && this.ListaTipiTruppeSel[0] != null)
		{
			this.modalitàTrappola.GetComponent<CanvasGroup>().alpha = 0f;
			this.modalitàTrappola.GetComponent<CanvasGroup>().interactable = false;
			this.modalitàTrappola.GetComponent<CanvasGroup>().blocksRaycasts = false;
			if (this.unitàSelezionata)
			{
				foreach (Transform transform in this.datiGenerali.transform)
				{
					transform.GetComponent<CanvasGroup>().alpha = 1f;
					transform.GetComponent<CanvasGroup>().interactable = true;
					transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
				}
				if (this.ListaPosizione0[0].GetComponent<PresenzaAlleato>().comportamentoDifensivo)
				{
					this.spuntaCompDifensivo.GetComponent<CanvasGroup>().alpha = 1f;
				}
				else
				{
					this.spuntaCompDifensivo.GetComponent<CanvasGroup>().alpha = 0f;
				}
				if (this.ListaPosizione0[0].GetComponent<PresenzaAlleato>().ricercaAutomaticaBersaglio)
				{
					this.spuntaCercaAutomBers.GetComponent<CanvasGroup>().alpha = 1f;
				}
				else
				{
					this.spuntaCercaAutomBers.GetComponent<CanvasGroup>().alpha = 0f;
				}
				if (this.ListaPosizione0[0].GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBers)
				{
					this.spuntaCercaBersDif.GetComponent<CanvasGroup>().alpha = 1f;
				}
				else
				{
					this.spuntaCercaBersDif.GetComponent<CanvasGroup>().alpha = 0f;
				}
				if (this.ListaPosizione0[0].GetComponent<PresenzaAlleato>().ricercaAutomDifensivaBersVicino)
				{
					this.spuntaCercaBersDifVicino.GetComponent<CanvasGroup>().alpha = 1f;
				}
				else
				{
					this.spuntaCercaBersDifVicino.GetComponent<CanvasGroup>().alpha = 0f;
				}
			}
			if (this.ListaTipiTruppeSel.Count == 1)
			{
				this.modalitàUnitàMultipla.GetComponent<CanvasGroup>().alpha = 0f;
				this.modalitàUnitàMultipla.GetComponent<CanvasGroup>().interactable = false;
				this.modalitàUnitàMultipla.GetComponent<CanvasGroup>().blocksRaycasts = false;
				this.modalitàUnitàSingola.GetComponent<CanvasGroup>().alpha = 1f;
				this.modalitàUnitàSingola.GetComponent<CanvasGroup>().interactable = true;
				this.modalitàUnitàSingola.GetComponent<CanvasGroup>().blocksRaycasts = true;
				this.modalitàUnitàSingola.transform.GetChild(0).GetComponent<Text>().text = this.ListaPosizione0[0].GetComponent<PresenzaAlleato>().nomeUnità;
				this.modalitàUnitàSingola.transform.GetChild(1).GetComponent<Image>().sprite = this.ListaPosizione0[0].GetComponent<PresenzaAlleato>().immagineUnità;
				if (this.ListaPosizione0.Count == 1)
				{
					this.modalitàUnitàSingola.transform.GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					this.modalitàUnitàSingola.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 1f;
					this.modalitàUnitàSingola.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = this.ListaPosizione0[0].GetComponent<PresenzaAlleato>().vita.ToString("F0") + "/" + this.ListaPosizione0[0].GetComponent<PresenzaAlleato>().vitaIniziale.ToString("F0");
				}
				else if (this.ListaPosizione0.Count > 1)
				{
					this.modalitàUnitàSingola.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0f;
					this.modalitàUnitàSingola.transform.GetChild(3).GetComponent<CanvasGroup>().alpha = 1f;
					this.modalitàUnitàSingola.transform.GetChild(3).GetComponent<Text>().text = base.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count.ToString();
				}
			}
			else if (this.ListaTipiTruppeSel.Count > 1)
			{
				this.modalitàUnitàSingola.GetComponent<CanvasGroup>().alpha = 0f;
				this.modalitàUnitàSingola.GetComponent<CanvasGroup>().interactable = false;
				this.modalitàUnitàSingola.GetComponent<CanvasGroup>().blocksRaycasts = false;
				this.modalitàUnitàSingola.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0f;
				this.modalitàUnitàSingola.transform.GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
				this.modalitàUnitàMultipla.GetComponent<CanvasGroup>().alpha = 1f;
				this.modalitàUnitàMultipla.GetComponent<CanvasGroup>().interactable = true;
				this.modalitàUnitàMultipla.GetComponent<CanvasGroup>().blocksRaycasts = true;
				if (this.ListaTipiTruppeSel.Count <= 9)
				{
					for (int i = 0; i < 9; i++)
					{
						if (i < this.numeroTipoDiverseSel)
						{
							if (this.ListaTipiTruppeSel[i] != null)
							{
								this.modalitàUnitàMultipla.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 1f;
								this.modalitàUnitàMultipla.transform.GetChild(i).GetComponent<CanvasGroup>().interactable = true;
								this.modalitàUnitàMultipla.transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = true;
								this.modalitàUnitàMultipla.transform.GetChild(i).GetComponent<Image>().sprite = this.ListaTipiTruppeSel[i].GetComponent<PresenzaAlleato>().immagineUnità;
								this.modalitàUnitàMultipla.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = this.ListaPosizioni[i].Count.ToString();
							}
						}
						else
						{
							this.modalitàUnitàMultipla.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 0f;
							this.modalitàUnitàMultipla.transform.GetChild(i).GetComponent<CanvasGroup>().interactable = false;
							this.modalitàUnitàMultipla.transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = false;
						}
					}
				}
				else
				{
					for (int j = 0; j < 8; j++)
					{
						this.modalitàUnitàMultipla.transform.GetChild(j).GetComponent<CanvasGroup>().alpha = 1f;
						this.modalitàUnitàMultipla.transform.GetChild(j).GetComponent<CanvasGroup>().interactable = true;
						this.modalitàUnitàMultipla.transform.GetChild(j).GetComponent<CanvasGroup>().blocksRaycasts = true;
						this.modalitàUnitàMultipla.transform.GetChild(j).GetComponent<Image>().sprite = this.ListaTipiTruppeSel[j].GetComponent<PresenzaAlleato>().immagineUnità;
						this.modalitàUnitàMultipla.transform.GetChild(j).transform.GetChild(0).GetComponent<Text>().text = this.ListaPosizioni[j].Count.ToString();
					}
					this.modalitàUnitàMultipla.transform.GetChild(8).GetComponent<CanvasGroup>().alpha = 1f;
					this.modalitàUnitàMultipla.transform.GetChild(8).GetComponent<CanvasGroup>().interactable = true;
					this.modalitàUnitàMultipla.transform.GetChild(8).GetComponent<CanvasGroup>().blocksRaycasts = true;
					this.modalitàUnitàMultipla.transform.GetChild(8).GetComponent<Image>().sprite = this.puntiniDiSospensione;
				}
			}
			else
			{
				foreach (Transform transform2 in base.transform)
				{
					transform2.GetComponent<CanvasGroup>().alpha = 0f;
					transform2.GetComponent<CanvasGroup>().interactable = false;
					transform2.GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
			}
		}
		else if (this.trappolaSelez != null)
		{
			this.modalitàTrappola.GetComponent<CanvasGroup>().alpha = 1f;
			this.modalitàTrappola.GetComponent<CanvasGroup>().interactable = true;
			this.modalitàTrappola.GetComponent<CanvasGroup>().blocksRaycasts = true;
			this.modalitàTrappola.transform.GetChild(0).GetComponent<Text>().text = this.trappolaSelez.GetComponent<PresenzaTrappola>().nomeTrappola;
			this.modalitàTrappola.transform.GetChild(1).GetComponent<Image>().sprite = this.trappolaSelez.GetComponent<PresenzaTrappola>().immagineTrappola;
			this.modalitàTrappola.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 1f;
			this.modalitàTrappola.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = this.trappolaSelez.GetComponent<PresenzaTrappola>().vita.ToString("F0") + "/" + this.trappolaSelez.GetComponent<PresenzaTrappola>().vitaIniziale.ToString("F0");
			this.modalitàTrappola.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>().text = "COST: " + (this.trappolaSelez.GetComponent<PresenzaTrappola>().costoPuntiBattaglia / 2f).ToString("F0");
			if (this.trappolaSelez.GetComponent<PresenzaTrappola>().tipoTrappola == 5)
			{
				this.modalitàTrappola.transform.GetChild(5).GetComponent<CanvasGroup>().alpha = 1f;
				this.modalitàTrappola.transform.GetChild(5).GetComponent<CanvasGroup>().interactable = true;
				this.modalitàTrappola.transform.GetChild(5).GetComponent<CanvasGroup>().blocksRaycasts = true;
				this.scrittaQuotaTrappola.GetComponent<Text>().text = this.trappolaSelez.GetComponent<MinaVolante>().quotaDecisa.ToString("F0");
			}
			else
			{
				this.modalitàTrappola.transform.GetChild(5).GetComponent<CanvasGroup>().alpha = 0f;
				this.modalitàTrappola.transform.GetChild(5).GetComponent<CanvasGroup>().interactable = false;
				this.modalitàTrappola.transform.GetChild(5).GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
			foreach (Transform transform3 in this.datiGenerali.transform)
			{
				transform3.GetComponent<CanvasGroup>().alpha = 0f;
				transform3.GetComponent<CanvasGroup>().interactable = false;
				transform3.GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
			this.datiGenerali.transform.GetChild(0).transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0f;
			this.datiGenerali.transform.GetChild(1).transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0f;
			this.datiGenerali.transform.GetChild(2).transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0f;
			this.modalitàUnitàSingola.GetComponent<CanvasGroup>().alpha = 0f;
			this.modalitàUnitàSingola.GetComponent<CanvasGroup>().interactable = false;
			this.modalitàUnitàSingola.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		else
		{
			foreach (Transform transform4 in this.datiGenerali.transform)
			{
				transform4.GetComponent<CanvasGroup>().alpha = 0f;
				transform4.GetComponent<CanvasGroup>().interactable = false;
				transform4.GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
			this.datiGenerali.transform.GetChild(0).transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0f;
			this.datiGenerali.transform.GetChild(1).transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0f;
			this.datiGenerali.transform.GetChild(2).transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0f;
			this.modalitàUnitàSingola.GetComponent<CanvasGroup>().alpha = 0f;
			this.modalitàUnitàSingola.GetComponent<CanvasGroup>().interactable = false;
			this.modalitàUnitàSingola.GetComponent<CanvasGroup>().blocksRaycasts = false;
			this.modalitàTrappola.GetComponent<CanvasGroup>().alpha = 0f;
			this.modalitàTrappola.GetComponent<CanvasGroup>().interactable = false;
			this.modalitàTrappola.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		if (this.ListaTipiTruppeSel.Count <= 1)
		{
			for (int k = 0; k < 3; k++)
			{
				this.modalitàUnitàMultipla.transform.GetChild(k).GetComponent<CanvasGroup>().alpha = 0f;
				this.modalitàUnitàMultipla.transform.GetChild(k).GetComponent<CanvasGroup>().interactable = false;
				this.modalitàUnitàMultipla.transform.GetChild(k).GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
		}
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00005948 File Offset: 0x00003B48
	private void RettangoloCentrale()
	{
		if (this.ListaTipiTruppeSel.Count == 1 && this.ListaPosizione0.Count > 0)
		{
			if (this.unitàSelezionata != null && !this.unitàSelezionata.GetComponent<PresenzaAlleato>().èPerRifornimento)
			{
				GameObject gameObject = this.ListaPosizione0[0];
				if (!this.unitàSelezionata.GetComponent<PresenzaAlleato>().èGeniere)
				{
					this.rettPerArmi.GetComponent<CanvasGroup>().alpha = 1f;
					this.rettPerArmi.GetComponent<CanvasGroup>().interactable = true;
					this.rettPerArmi.GetComponent<CanvasGroup>().blocksRaycasts = true;
					this.rettPerTrappole.GetComponent<CanvasGroup>().alpha = 0f;
					this.rettPerTrappole.GetComponent<CanvasGroup>().interactable = false;
					this.rettPerTrappole.GetComponent<CanvasGroup>().blocksRaycasts = false;
					for (int i = 0; i < 4; i++)
					{
						if (i < this.unitàSelezionata.GetComponent<PresenzaAlleato>().numeroArmi)
						{
							this.ListaArmiUI[i].GetComponent<CanvasGroup>().alpha = 1f;
							this.ListaArmiUI[i].GetComponent<CanvasGroup>().interactable = true;
							this.ListaArmiUI[i].GetComponent<CanvasGroup>().blocksRaycasts = true;
							this.ListaArmiUI[i].transform.GetChild(0).GetComponent<Text>().text = gameObject.GetComponent<PresenzaAlleato>().ListaNomiArmi[i];
							this.ListaArmiUI[i].transform.GetChild(1).GetComponent<Text>().text = "Ammo: " + gameObject.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[i].GetComponent<DatiGeneraliMunizione>().nome;
							this.ListaArmiUI[i].transform.GetChild(2).GetComponent<Text>().text = gameObject.GetComponent<PresenzaAlleato>().ListaArmi[i][5].ToString() + " / " + gameObject.GetComponent<PresenzaAlleato>().ListaArmi[i][6].ToString();
							this.ListaArmiUI[i].transform.GetChild(3).GetComponent<Text>().text = "Damage: " + gameObject.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[i].GetComponent<DatiGeneraliMunizione>().danno.ToString();
							this.ListaArmiUI[i].transform.GetChild(4).GetComponent<Text>().text = "Piercing Value: " + gameObject.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[i].GetComponent<DatiGeneraliMunizione>().penetrazione.ToString();
							this.ListaArmiUI[i].transform.GetChild(5).GetComponent<Text>().text = "Range: " + gameObject.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[i].GetComponent<DatiGeneraliMunizione>().portataMinima.ToString() + "-" + gameObject.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[i].GetComponent<DatiGeneraliMunizione>().portataMassima.ToString();
							this.ListaArmiUI[i].transform.GetChild(6).GetComponent<Text>().text = "Blast Radius: " + gameObject.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[i].GetComponent<DatiGeneraliMunizione>().raggioEffetto.ToString();
							this.ListaArmiUI[i].transform.GetChild(7).GetComponent<Text>().text = string.Concat(new string[]
							{
								"Rate: ",
								gameObject.GetComponent<PresenzaAlleato>().ListaArmi[i][0].ToString(),
								"  (",
								gameObject.GetComponent<PresenzaAlleato>().ListaArmi[i][1].ToString(),
								")"
							});
							this.ListaArmiUI[i].transform.GetChild(8).GetComponent<Text>().text = "Reload: " + gameObject.GetComponent<PresenzaAlleato>().ListaArmi[i][2].ToString() + "s";
						}
						else
						{
							this.ListaArmiUI[i].GetComponent<CanvasGroup>().alpha = 0f;
							this.ListaArmiUI[i].GetComponent<CanvasGroup>().interactable = false;
							this.ListaArmiUI[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
						}
					}
					for (int j = 0; j < 4; j++)
					{
						if (gameObject.GetComponent<PresenzaAlleato>().ListaArmiAttivate[j])
						{
							this.ListaArmiUI[j].GetComponent<Image>().color = this.coloreArmaAttiva;
						}
						else
						{
							this.ListaArmiUI[j].GetComponent<Image>().color = this.coloreArmaDisattiva;
						}
					}
					if (this.unitàSelezionata.GetComponent<PresenzaAlleato>().èElicottero)
					{
						this.pulsanteAtterra.GetComponent<CanvasGroup>().alpha = 1f;
						this.pulsanteAtterra.GetComponent<CanvasGroup>().interactable = true;
						this.pulsanteAtterra.GetComponent<CanvasGroup>().blocksRaycasts = true;
					}
					else
					{
						this.pulsanteAtterra.GetComponent<CanvasGroup>().alpha = 0f;
						this.pulsanteAtterra.GetComponent<CanvasGroup>().interactable = false;
						this.pulsanteAtterra.GetComponent<CanvasGroup>().blocksRaycasts = false;
					}
					if (this.ListaPosizione0[0].GetComponent<PresenzaAlleato>().tipoTruppaTerrConOrdigni == 3)
					{
						this.pulsanteFuocoSalvaRockArt.GetComponent<CanvasGroup>().alpha = 1f;
						this.pulsanteFuocoSalvaRockArt.GetComponent<CanvasGroup>().interactable = true;
						this.pulsanteFuocoSalvaRockArt.GetComponent<CanvasGroup>().blocksRaycasts = true;
						if (this.ListaPosizione0[0].GetComponent<ATT_RocketArtillery>().rafficaAttiva)
						{
							this.pulsanteFuocoSalvaRockArt.transform.GetChild(0).GetComponent<Button>().interactable = true;
							this.pulsanteFuocoSalvaRockArt.transform.GetChild(1).GetComponent<Button>().interactable = false;
						}
						else
						{
							this.pulsanteFuocoSalvaRockArt.transform.GetChild(0).GetComponent<Button>().interactable = false;
							this.pulsanteFuocoSalvaRockArt.transform.GetChild(1).GetComponent<Button>().interactable = true;
						}
					}
					else
					{
						this.pulsanteFuocoSalvaRockArt.GetComponent<CanvasGroup>().alpha = 0f;
						this.pulsanteFuocoSalvaRockArt.GetComponent<CanvasGroup>().interactable = false;
						this.pulsanteFuocoSalvaRockArt.GetComponent<CanvasGroup>().blocksRaycasts = false;
					}
				}
				else
				{
					this.rettPerArmi.GetComponent<CanvasGroup>().alpha = 0f;
					this.rettPerArmi.GetComponent<CanvasGroup>().interactable = false;
					this.rettPerArmi.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.rettPerTrappole.GetComponent<CanvasGroup>().alpha = 1f;
					this.rettPerTrappole.GetComponent<CanvasGroup>().interactable = true;
					this.rettPerTrappole.GetComponent<CanvasGroup>().blocksRaycasts = true;
					this.pulsanteAtterra.GetComponent<CanvasGroup>().alpha = 0f;
					this.pulsanteAtterra.GetComponent<CanvasGroup>().interactable = false;
					this.pulsanteAtterra.GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.pulsanteFuocoSalvaRockArt.GetComponent<CanvasGroup>().alpha = 0f;
					this.pulsanteFuocoSalvaRockArt.GetComponent<CanvasGroup>().interactable = false;
					this.pulsanteFuocoSalvaRockArt.GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
			}
			else if (this.unitàSelezionata)
			{
				this.rettPerArmi.GetComponent<CanvasGroup>().alpha = 1f;
				this.rettPerArmi.GetComponent<CanvasGroup>().interactable = true;
				this.rettPerArmi.GetComponent<CanvasGroup>().blocksRaycasts = true;
				this.rettPerTrappole.GetComponent<CanvasGroup>().alpha = 0f;
				this.rettPerTrappole.GetComponent<CanvasGroup>().interactable = false;
				this.rettPerTrappole.GetComponent<CanvasGroup>().blocksRaycasts = false;
				this.arma1.GetComponent<CanvasGroup>().alpha = 1f;
				this.arma1.GetComponent<CanvasGroup>().interactable = true;
				this.arma1.GetComponent<CanvasGroup>().blocksRaycasts = true;
				this.arma1.transform.GetChild(0).GetComponent<Text>().text = "AMMO SUPPLIES";
				if (this.unitàSelezionata.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp > 0)
				{
					this.arma1.transform.GetChild(1).GetComponent<Text>().text = "Supplies Remaining: " + this.unitàSelezionata.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp.ToString() + "/" + this.unitàSelezionata.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp.ToString();
				}
				else
				{
					this.arma1.transform.GetChild(1).GetComponent<Text>().text = "Supplies Remaining: 0";
				}
				this.arma1.transform.GetChild(2).GetComponent<Text>().text = string.Empty;
				this.arma1.transform.GetChild(3).GetComponent<Text>().text = string.Empty;
				this.arma1.transform.GetChild(4).GetComponent<Text>().text = string.Empty;
				this.arma1.transform.GetChild(5).GetComponent<Text>().text = string.Empty;
				this.arma1.transform.GetChild(6).GetComponent<Text>().text = string.Empty;
				this.arma1.transform.GetChild(7).GetComponent<Text>().text = string.Empty;
				this.arma1.transform.GetChild(8).GetComponent<Text>().text = string.Empty;
				this.arma2.GetComponent<CanvasGroup>().alpha = 1f;
				this.arma2.GetComponent<CanvasGroup>().interactable = true;
				this.arma2.GetComponent<CanvasGroup>().blocksRaycasts = true;
				this.arma2.transform.GetChild(0).GetComponent<Text>().text = "REPAIR SUPPLIES";
				if (this.unitàSelezionata.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp > 0)
				{
					this.arma2.transform.GetChild(1).GetComponent<Text>().text = "Supplies Remaining: " + this.unitàSelezionata.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp.ToString() + "/" + this.unitàSelezionata.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp.ToString();
				}
				else
				{
					this.arma2.transform.GetChild(1).GetComponent<Text>().text = "Supplies Remaining: 0";
				}
				this.arma2.transform.GetChild(2).GetComponent<Text>().text = string.Empty;
				this.arma2.transform.GetChild(3).GetComponent<Text>().text = string.Empty;
				this.arma2.transform.GetChild(4).GetComponent<Text>().text = string.Empty;
				this.arma2.transform.GetChild(5).GetComponent<Text>().text = string.Empty;
				this.arma2.transform.GetChild(6).GetComponent<Text>().text = string.Empty;
				this.arma2.transform.GetChild(7).GetComponent<Text>().text = string.Empty;
				this.arma2.transform.GetChild(8).GetComponent<Text>().text = string.Empty;
				if (this.unitàSelezionata.GetComponent<PresenzaAlleato>().èElicottero)
				{
					this.pulsanteAtterra.GetComponent<CanvasGroup>().alpha = 1f;
					this.pulsanteAtterra.GetComponent<CanvasGroup>().interactable = true;
					this.pulsanteAtterra.GetComponent<CanvasGroup>().blocksRaycasts = true;
				}
				else
				{
					this.pulsanteAtterra.GetComponent<CanvasGroup>().alpha = 0f;
					this.pulsanteAtterra.GetComponent<CanvasGroup>().interactable = false;
					this.pulsanteAtterra.GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
				this.arma3.GetComponent<CanvasGroup>().alpha = 0f;
				this.arma3.GetComponent<CanvasGroup>().interactable = false;
				this.arma3.GetComponent<CanvasGroup>().blocksRaycasts = false;
				this.arma4.GetComponent<CanvasGroup>().alpha = 0f;
				this.arma4.GetComponent<CanvasGroup>().interactable = false;
				this.arma4.GetComponent<CanvasGroup>().blocksRaycasts = false;
				for (int k = 0; k < 4; k++)
				{
					if (this.ListaPosizione0[0].GetComponent<PresenzaAlleato>().ListaArmiAttivate[k])
					{
						this.ListaArmiUI[k].GetComponent<Image>().color = this.coloreArmaAttiva;
					}
					else
					{
						this.ListaArmiUI[k].GetComponent<Image>().color = this.coloreArmaDisattiva;
					}
				}
				this.pulsanteFuocoSalvaRockArt.GetComponent<CanvasGroup>().alpha = 0f;
				this.pulsanteFuocoSalvaRockArt.GetComponent<CanvasGroup>().interactable = false;
				this.pulsanteFuocoSalvaRockArt.GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
		}
		else
		{
			for (int l = 0; l < 4; l++)
			{
				this.ListaArmiUI[l].GetComponent<CanvasGroup>().alpha = 0f;
				this.ListaArmiUI[l].GetComponent<CanvasGroup>().interactable = false;
				this.ListaArmiUI[l].GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
			this.rettPerArmi.GetComponent<CanvasGroup>().alpha = 0f;
			this.rettPerArmi.GetComponent<CanvasGroup>().interactable = false;
			this.rettPerArmi.GetComponent<CanvasGroup>().blocksRaycasts = false;
			this.rettPerTrappole.GetComponent<CanvasGroup>().alpha = 0f;
			this.rettPerTrappole.GetComponent<CanvasGroup>().interactable = false;
			this.rettPerTrappole.GetComponent<CanvasGroup>().blocksRaycasts = false;
			this.pulsanteAtterra.GetComponent<CanvasGroup>().alpha = 0f;
			this.pulsanteAtterra.GetComponent<CanvasGroup>().interactable = false;
			this.pulsanteAtterra.GetComponent<CanvasGroup>().blocksRaycasts = false;
			this.pulsanteFuocoSalvaRockArt.GetComponent<CanvasGroup>().alpha = 0f;
			this.pulsanteFuocoSalvaRockArt.GetComponent<CanvasGroup>().interactable = false;
			this.pulsanteFuocoSalvaRockArt.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	// Token: 0x0600001C RID: 28 RVA: 0x000067D8 File Offset: 0x000049D8
	private void QuadratoDestro()
	{
		if (this.unitàSelezionata && this.ListaTipiTruppeSel.Count == 1 && this.ListaPosizione0.Count > 0 && !this.unitàSelezionata.GetComponent<PresenzaAlleato>().èPerRifornimento)
		{
			if (!this.unitàSelezionata.GetComponent<PresenzaAlleato>().èGeniere)
			{
				this.quadroSelezMunizioni.GetComponent<CanvasGroup>().alpha = 1f;
				this.quadroSelezMunizioni.GetComponent<CanvasGroup>().interactable = true;
				this.quadroSelezMunizioni.GetComponent<CanvasGroup>().blocksRaycasts = true;
				this.quadroInfoTrappola.GetComponent<CanvasGroup>().alpha = 0f;
				this.quadroInfoTrappola.GetComponent<CanvasGroup>().interactable = false;
				this.quadroInfoTrappola.GetComponent<CanvasGroup>().blocksRaycasts = false;
				for (int i = 0; i < 4; i++)
				{
					if (i == this.numeroArmaSelezionata)
					{
						for (int j = 0; j < this.unitàSelezionata.GetComponent<PresenzaAlleato>().ListaNumeroTipiMunizioni[i]; j++)
						{
							this.ListaTipiMunizioni[j].GetComponent<CanvasGroup>().alpha = 1f;
							this.ListaTipiMunizioni[j].GetComponent<CanvasGroup>().interactable = true;
							this.ListaTipiMunizioni[j].GetComponent<CanvasGroup>().blocksRaycasts = true;
							this.ListaTipiMunizioni[j].transform.GetChild(0).GetComponent<Text>().text = this.ListaPosizione0[0].GetComponent<PresenzaAlleato>().ListaMunizioneArmi[this.numeroArmaSelezionata][j].GetComponent<DatiGeneraliMunizione>().nome;
							this.ListaTipiMunizioni[j].transform.GetChild(1).GetComponent<Text>().text = "Quantity: " + this.ListaPosizione0[0].GetComponent<PresenzaAlleato>().ListaMunizioneArmi[this.numeroArmaSelezionata][j].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().quantità;
							this.ListaTipiMunizioni[j].transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 1f;
							if (this.ListaPosizione0[0].GetComponent<PresenzaAlleato>().ListaMunizioniAttive[this.numeroArmaSelezionata] == this.ListaPosizione0[0].GetComponent<PresenzaAlleato>().ListaMunizioneArmi[this.numeroArmaSelezionata][j])
							{
								this.ListaTipiMunizioni[j].transform.GetChild(3).GetComponent<CanvasGroup>().alpha = 1f;
							}
							else
							{
								this.ListaTipiMunizioni[j].transform.GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
							}
						}
					}
				}
			}
			else
			{
				this.quadroSelezMunizioni.GetComponent<CanvasGroup>().alpha = 0f;
				this.quadroSelezMunizioni.GetComponent<CanvasGroup>().interactable = false;
				this.quadroSelezMunizioni.GetComponent<CanvasGroup>().blocksRaycasts = false;
				this.quadroInfoTrappola.GetComponent<CanvasGroup>().alpha = 1f;
				this.quadroInfoTrappola.GetComponent<CanvasGroup>().interactable = true;
				this.quadroInfoTrappola.GetComponent<CanvasGroup>().blocksRaycasts = true;
				GameObject gameObject = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaTrappolePossibili[this.numInfoTrappola];
				this.quadroInfoTrappola.transform.GetChild(0).GetComponent<Text>().text = gameObject.GetComponent<PresenzaTrappola>().nomeTrappola;
				this.quadroInfoTrappola.transform.GetChild(1).GetComponent<Text>().text = string.Concat(new object[]
				{
					"Battle Point Cost: ",
					gameObject.GetComponent<PresenzaTrappola>().costoPuntiBattaglia,
					"\nHealth: ",
					gameObject.GetComponent<PresenzaTrappola>().vita,
					"\nDamage: ",
					gameObject.GetComponent<PresenzaTrappola>().danno,
					"\nPiercing: ",
					gameObject.GetComponent<PresenzaTrappola>().penetrazione,
					"\nBlast Range: ",
					gameObject.GetComponent<PresenzaTrappola>().portata,
					"\nRate: ",
					gameObject.GetComponent<PresenzaTrappola>().frequenzaAttacco,
					"\nEnemy's Speed: ",
					(gameObject.GetComponent<PresenzaTrappola>().percDiRallentamento * 100f).ToString(),
					"%"
				});
			}
		}
		else
		{
			this.quadroSelezMunizioni.GetComponent<CanvasGroup>().alpha = 0f;
			this.quadroSelezMunizioni.GetComponent<CanvasGroup>().interactable = false;
			this.quadroSelezMunizioni.GetComponent<CanvasGroup>().blocksRaycasts = false;
			this.quadroInfoTrappola.GetComponent<CanvasGroup>().alpha = 0f;
			this.quadroInfoTrappola.GetComponent<CanvasGroup>().interactable = false;
			this.quadroInfoTrappola.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00006CE8 File Offset: 0x00004EE8
	private void SopraRettangolo()
	{
		if (this.unitàSelezionata && this.attacchiSpecialiAlleati.GetComponent<AttacchiAlleatiSpecialiScript>().artiglieriaInLista)
		{
			this.sopraRettangolo.transform.GetChild(0).GetComponent<Button>().interactable = true;
		}
		else
		{
			this.sopraRettangolo.transform.GetChild(0).GetComponent<Button>().interactable = false;
		}
		if (this.unitàSelezionata && this.attacchiSpecialiAlleati.GetComponent<AttacchiAlleatiSpecialiScript>().bombardiereInLista)
		{
			this.sopraRettangolo.transform.GetChild(1).GetComponent<Button>().interactable = true;
		}
		else
		{
			this.sopraRettangolo.transform.GetChild(1).GetComponent<Button>().interactable = false;
		}
		if (this.unitàSelezionata && this.ListaPosizione0[0].GetComponent<PresenzaAlleato>().èGeniere)
		{
			if (this.unitàSelezionata.GetComponent<ATT_Sapper>().dinamitePossibile)
			{
				this.sopraRettangolo.transform.GetChild(3).GetComponent<Button>().interactable = true;
				if (!this.unitàSelezionata.GetComponent<ATT_Sapper>().piazzDinamiteAttivo)
				{
					this.sopraRettangolo.transform.GetChild(3).transform.GetChild(0).GetComponent<Image>().fillAmount = 0f;
				}
			}
			else
			{
				this.sopraRettangolo.transform.GetChild(3).GetComponent<Button>().interactable = false;
				this.sopraRettangolo.transform.GetChild(3).transform.GetChild(0).GetComponent<Image>().fillAmount = 0f;
			}
		}
		else
		{
			this.sopraRettangolo.transform.GetChild(3).GetComponent<Button>().interactable = false;
			this.sopraRettangolo.transform.GetChild(3).transform.GetChild(0).GetComponent<Image>().fillAmount = 0f;
		}
		if (this.barraTipolRinforzi.GetComponent<CanvasGroup>().alpha == 0f && this.aereoParàDisponibile && this.attacchiSpecialiAlleati.GetComponent<AttacchiAlleatiSpecialiScript>().timerProssLancioParà > 0f)
		{
			this.sopraRettangolo.transform.GetChild(4).GetComponent<Button>().interactable = true;
		}
		else
		{
			this.sopraRettangolo.transform.GetChild(4).GetComponent<Button>().interactable = false;
		}
		if (this.unitàSelezionata)
		{
			this.sopraRettangolo.transform.GetChild(2).GetComponent<Button>().interactable = true;
		}
		else
		{
			this.sopraRettangolo.transform.GetChild(2).GetComponent<Button>().interactable = false;
		}
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00006FB8 File Offset: 0x000051B8
	private void VarieCanvasComandante()
	{
		if (this.quadratoInformazioniUnità.GetComponent<CanvasGroup>().alpha == 1f && Input.GetMouseButtonDown(0))
		{
			this.quadratoInformazioniUnità.GetComponent<CanvasGroup>().alpha = 0f;
		}
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00007000 File Offset: 0x00005200
	private void EvidenziazioneAlleatiENemici()
	{
		if (Input.GetKeyDown(KeyCode.X) && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati.Count > 0)
		{
			this.evidenziaAlleatiENemici = !this.evidenziaAlleatiENemici;
			this.primaCamera.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
			this.primaCamera.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
			this.primaCamera.GetComponent<AudioSource>().clip = this.primaCamera.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		}
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00007088 File Offset: 0x00005288
	private void GestioneRinforzi()
	{
		if (this.aggiornaElencoRinforzi)
		{
			this.ListaAlleatiInTipolSchier = new List<GameObject>();
			for (int i = 0; i < this.elencoUnitàRinforzi.transform.childCount; i++)
			{
				this.elencoUnitàRinforzi.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 0f;
				this.elencoUnitàRinforzi.transform.GetChild(i).GetComponent<CanvasGroup>().interactable = false;
				this.elencoUnitàRinforzi.transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
			bool flag = false;
			int num = 0;
			if (this.tipoElencoRinforzi == 0)
			{
				int num2 = 0;
				while (num2 < 48 && !flag)
				{
					if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num2][1] != 0)
					{
						GameObject gameObject = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiPossibili[this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num2][0]];
						if (gameObject.GetComponent<PresenzaAlleato>().èFanteria)
						{
							this.elencoUnitàRinforzi.transform.GetChild(num).GetComponent<CanvasGroup>().alpha = 1f;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetComponent<CanvasGroup>().interactable = true;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetComponent<CanvasGroup>().blocksRaycasts = true;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetChild(0).GetComponent<Image>().sprite = gameObject.GetComponent<PresenzaAlleato>().immagineUnità;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetChild(1).GetComponent<Text>().text = gameObject.GetComponent<PresenzaAlleato>().nomeUnità;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetChild(2).GetComponent<Text>().text = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num2][1].ToString();
							this.elencoUnitàRinforzi.transform.GetChild(num).GetChild(3).GetChild(0).GetComponent<Text>().text = (gameObject.GetComponent<PresenzaAlleato>().costoInPlastica / 10f).ToString() + " BP";
							this.ListaAlleatiInTipolSchier.Add(gameObject);
							num++;
						}
					}
					num2++;
				}
			}
			else if (this.tipoElencoRinforzi == 1)
			{
				int num3 = 0;
				while (num3 < 48 && !flag)
				{
					if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num3][1] != 0)
					{
						GameObject gameObject2 = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiPossibili[this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num3][0]];
						if (gameObject2.GetComponent<PresenzaAlleato>().èMezzo)
						{
							this.elencoUnitàRinforzi.transform.GetChild(num).GetComponent<CanvasGroup>().alpha = 1f;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetComponent<CanvasGroup>().interactable = true;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetComponent<CanvasGroup>().blocksRaycasts = true;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetChild(0).GetComponent<Image>().sprite = gameObject2.GetComponent<PresenzaAlleato>().immagineUnità;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetChild(1).GetComponent<Text>().text = gameObject2.GetComponent<PresenzaAlleato>().nomeUnità;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetChild(2).GetComponent<Text>().text = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num3][1].ToString();
							this.elencoUnitàRinforzi.transform.GetChild(num).GetChild(3).GetChild(0).GetComponent<Text>().text = (gameObject2.GetComponent<PresenzaAlleato>().costoInPlastica / 10f).ToString() + " BP";
							this.ListaAlleatiInTipolSchier.Add(gameObject2);
							num++;
						}
					}
					num3++;
				}
			}
			else if (this.tipoElencoRinforzi == 2)
			{
				int num4 = 0;
				while (num4 < 48 && !flag)
				{
					if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num4][1] != 0)
					{
						GameObject gameObject3 = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiPossibili[this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num4][0]];
						if (gameObject3.GetComponent<PresenzaAlleato>().èArtiglieria && !gameObject3.GetComponent<PresenzaAlleato>().èFanteria)
						{
							this.elencoUnitàRinforzi.transform.GetChild(num).GetComponent<CanvasGroup>().alpha = 1f;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetComponent<CanvasGroup>().interactable = true;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetComponent<CanvasGroup>().blocksRaycasts = true;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetChild(0).GetComponent<Image>().sprite = gameObject3.GetComponent<PresenzaAlleato>().immagineUnità;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetChild(1).GetComponent<Text>().text = gameObject3.GetComponent<PresenzaAlleato>().nomeUnità;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetChild(2).GetComponent<Text>().text = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num4][1].ToString();
							this.elencoUnitàRinforzi.transform.GetChild(num).GetChild(3).GetChild(0).GetComponent<Text>().text = (gameObject3.GetComponent<PresenzaAlleato>().costoInPlastica / 10f).ToString() + " BP";
							this.ListaAlleatiInTipolSchier.Add(gameObject3);
							num++;
						}
					}
					num4++;
				}
			}
			else if (this.tipoElencoRinforzi == 3)
			{
				int num5 = 0;
				while (num5 < 48 && !flag)
				{
					if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num5][1] != 0)
					{
						GameObject gameObject4 = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiPossibili[this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num5][0]];
						if (gameObject4.GetComponent<PresenzaAlleato>().volante)
						{
							this.elencoUnitàRinforzi.transform.GetChild(num).GetComponent<CanvasGroup>().alpha = 1f;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetComponent<CanvasGroup>().interactable = true;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetComponent<CanvasGroup>().blocksRaycasts = true;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetChild(0).GetComponent<Image>().sprite = gameObject4.GetComponent<PresenzaAlleato>().immagineUnità;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetChild(1).GetComponent<Text>().text = gameObject4.GetComponent<PresenzaAlleato>().nomeUnità;
							this.elencoUnitàRinforzi.transform.GetChild(num).GetChild(2).GetComponent<Text>().text = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num5][1].ToString();
							this.elencoUnitàRinforzi.transform.GetChild(num).GetChild(3).GetChild(0).GetComponent<Text>().text = (gameObject4.GetComponent<PresenzaAlleato>().costoInPlastica / 10f).ToString() + " BP";
							this.ListaAlleatiInTipolSchier.Add(gameObject4);
							num++;
						}
					}
					num5++;
				}
			}
			this.aggiornaElencoRinforzi = false;
		}
		if (this.creaAlleatoPerSchieramento)
		{
			if (this.alleatoèDaSchier)
			{
				this.tipoTruppaDaCancellare = this.alleatoRealeèDaSchier.GetComponent<PresenzaAlleato>().tipoTruppa;
				this.alleatoDaPosizionare = false;
				UnityEngine.Object.Destroy(this.alleatoèDaSchier);
			}
			bool flag2 = false;
			bool flag3 = false;
			int num6 = 0;
			while (num6 < 48 && !flag3)
			{
				if (this.ListaAlleatiInTipolSchier[this.slotRinforzoSelez].GetComponent<PresenzaAlleato>().tipoTruppa == this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num6][0])
				{
					flag2 = (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num6][1] > 0);
					this.indiceTruppa = num6;
					flag3 = true;
				}
				num6++;
			}
			if (flag2)
			{
				if (this.ListaAlleatiInTipolSchier[this.slotRinforzoSelez].GetComponent<PresenzaAlleato>().èAereo)
				{
					if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().puntiBattaglia >= this.ListaAlleatiInTipolSchier[this.slotRinforzoSelez].GetComponent<PresenzaAlleato>().costoInPlastica / 10f)
					{
						bool flag4 = false;
						int num7 = 0;
						while (num7 < 9 && !flag4)
						{
							if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAereiInQuadrato[num7] == null)
							{
								this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAereiInQuadrato[num7] = this.ListaAlleatiInTipolSchier[this.slotRinforzoSelez];
								List<int> list;
								List<int> expr_9F4 = list = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[this.indiceTruppa];
								int num8;
								int expr_9F8 = num8 = 1;
								num8 = list[num8];
								expr_9F4[expr_9F8] = num8 - 1;
								this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaDisponibilitàAerei[num7] = true;
								this.aggiornaElencoRinforzi = true;
								flag4 = true;
								this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().puntiBattaglia -= this.ListaAlleatiInTipolSchier[this.slotRinforzoSelez].GetComponent<PresenzaAlleato>().costoInPlastica / 10f;
							}
							num7++;
						}
						this.quadroAereoAperto = true;
					}
				}
				else
				{
					this.alleatoRealeèDaSchier = this.ListaAlleatiInTipolSchier[this.slotRinforzoSelez];
					this.alleatoèDaSchier = (UnityEngine.Object.Instantiate(this.alleatoRealeèDaSchier.GetComponent<PresenzaAlleato>().copiaPerSchieramento, this.posSchierUnità, Quaternion.identity) as GameObject);
					this.ultimoTipoSchierUsato = this.alleatoRealeèDaSchier.GetComponent<PresenzaAlleato>().tipoTruppa;
					this.alleatoDaPosizionare = true;
				}
			}
			this.creaAlleatoPerSchieramento = false;
		}
		if (this.alleatoDaPosizionare && this.alleatoèDaSchier != null)
		{
			if (this.primaCamera.GetComponent<Selezionamento>().ultimaAereaSchierToccata != null)
			{
				this.alleatoèDaSchier.transform.forward = this.primaCamera.GetComponent<Selezionamento>().ultimaAereaSchierToccata.transform.forward;
			}
			if (this.alleatoRealeèDaSchier.GetComponent<PresenzaAlleato>().èElicottero)
			{
				if (this.alleatoRealeèDaSchier.GetComponent<PresenzaAlleato>().tipoTruppa == 37)
				{
					this.alleatoèDaSchier.transform.position = this.posSchierUnità + Vector3.up * 150f;
				}
				else
				{
					this.alleatoèDaSchier.transform.position = this.posSchierUnità + Vector3.up * 70f;
				}
			}
			else
			{
				this.alleatoèDaSchier.transform.position = new Vector3(this.posSchierUnità.x, this.posSchierUnità.y + this.alleatoRealeèDaSchier.GetComponent<PresenzaAlleato>().altezzaInSchier, this.posSchierUnità.z);
			}
			if (Input.GetMouseButtonUp(0) && this.alleatoèDaSchier && this.slotRinforzoSelez < this.ListaAlleatiInTipolSchier.Count && this.ultimoTipoSchierUsato != this.ListaAlleatiInTipolSchier[this.slotRinforzoSelez].GetComponent<PresenzaAlleato>().tipoTruppa)
			{
				this.tipoTruppaDaCancellare = this.alleatoRealeèDaSchier.GetComponent<PresenzaAlleato>().tipoTruppa;
				this.alleatoDaPosizionare = false;
				UnityEngine.Object.Destroy(this.alleatoèDaSchier);
			}
			if (Input.GetMouseButtonDown(0))
			{
				int index = 0;
				bool flag5 = false;
				bool flag6 = false;
				int num9 = 0;
				while (num9 < 48 && !flag6)
				{
					if (this.slotRinforzoSelez < this.ListaAlleatiInTipolSchier.Count && this.ListaAlleatiInTipolSchier[this.slotRinforzoSelez].GetComponent<PresenzaAlleato>().tipoTruppa == this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num9][0])
					{
						index = num9;
						if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num9][1] > 0)
						{
							flag5 = true;
						}
						else
						{
							flag5 = false;
							UnityEngine.Object.Destroy(this.alleatoèDaSchier);
							this.alleatoèDaSchier = null;
							this.alleatoRealeèDaSchier = null;
							this.alleatoDaPosizionare = false;
						}
						flag6 = true;
					}
					else
					{
						flag5 = false;
					}
					num9++;
				}
				if (!EventSystem.current.IsPointerOverGameObject())
				{
					if (flag5)
					{
						if (base.GetComponent<InfoGenericheAlleati>().ListaAlleati.Count < this.numMaxAlleati)
						{
							if (this.posPerSchierValida)
							{
								int num10 = 1;
								int num11 = base.GetComponent<InfoGenericheAlleati>().ListaAlleati.Count;
								if (Input.GetKey(KeyCode.LeftControl))
								{
									num10 = this.numeroGruppoSchier;
								}
								for (int j = 0; j < num10; j++)
								{
									if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[index][1] > 0 && num11 < this.numMaxAlleati && this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().puntiBattaglia >= this.alleatoRealeèDaSchier.GetComponent<PresenzaAlleato>().costoInPlastica / 10f)
									{
										GameObject gameObject5 = UnityEngine.Object.Instantiate(this.alleatoRealeèDaSchier, this.alleatoèDaSchier.transform.position, this.alleatoèDaSchier.transform.rotation) as GameObject;
										this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().puntiBattaglia -= this.alleatoRealeèDaSchier.GetComponent<PresenzaAlleato>().costoInPlastica / 10f;
										gameObject5.GetComponent<PresenzaAlleato>().giàSchierato = true;
										if (gameObject5.GetComponent<NavMeshAgent>())
										{
											gameObject5.GetComponent<NavMeshAgent>().enabled = true;
											if (gameObject5.GetComponent<NavMeshAgent>().isOnNavMesh)
											{
												List<int> list2;
												List<int> expr_F1A = list2 = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[this.ListaAlleatiInTipolSchier[this.slotRinforzoSelez].GetComponent<PresenzaAlleato>().tipoTruppa];
												int num8;
												int expr_F1E = num8 = 1;
												num8 = list2[num8];
												expr_F1A[expr_F1E] = num8 - 1;
												this.primaCamera.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
												this.primaCamera.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
												this.primaCamera.GetComponent<GestioneSuoniCamera>().suonoVoci.clip = this.primaCamera.GetComponent<GestioneSuoniCamera>().suonoSchierAlleato;
												this.ultimoTipoSchierUsato = this.ListaAlleatiInTipolSchier[this.slotRinforzoSelez].GetComponent<PresenzaAlleato>().tipoTruppa;
												num11++;
											}
											else
											{
												UnityEngine.Object.Destroy(this.alleatoèDaSchier);
												UnityEngine.Object.Destroy(gameObject5);
												this.aggiornaElencoRinforzi = true;
											}
										}
										else
										{
											List<int> list3;
											List<int> expr_FF6 = list3 = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[this.ListaAlleatiInTipolSchier[this.slotRinforzoSelez].GetComponent<PresenzaAlleato>().tipoTruppa];
											int num8;
											int expr_FFA = num8 = 1;
											num8 = list3[num8];
											expr_FF6[expr_FFA] = num8 - 1;
											this.primaCamera.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
											this.primaCamera.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
											this.primaCamera.GetComponent<GestioneSuoniCamera>().suonoVoci.clip = this.primaCamera.GetComponent<GestioneSuoniCamera>().suonoSchierAlleato;
											this.ultimoTipoSchierUsato = this.ListaAlleatiInTipolSchier[this.slotRinforzoSelez].GetComponent<PresenzaAlleato>().tipoTruppa;
											num11++;
										}
										this.aggiornaElencoRinforzi = true;
									}
								}
							}
							else
							{
								this.partenzaTimerScrittaAvvSchier = true;
								this.scritteAvvSchieramento.GetComponent<Text>().text = "Invalid position";
							}
						}
						else
						{
							this.partenzaTimerScrittaAvvSchier = true;
							this.scritteAvvSchieramento.GetComponent<Text>().text = "Too many troops on the battlefield";
						}
					}
					else
					{
						this.partenzaTimerScrittaAvvSchier = true;
						this.scritteAvvSchieramento.GetComponent<Text>().text = "No troops available";
					}
				}
			}
		}
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00008190 File Offset: 0x00006390
	private void QuadratoAereiUI()
	{
		this.aereoParàDisponibile = false;
		for (int i = 0; i < 9; i++)
		{
			GameObject gameObject = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAereiInQuadrato[i];
			if (gameObject == null)
			{
				this.sfondoQuadratoAerei.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 0f;
				this.sfondoQuadratoAerei.transform.GetChild(i).GetComponent<CanvasGroup>().interactable = false;
				this.sfondoQuadratoAerei.transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = false;
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAereiInVolo[i] = false;
			}
			else
			{
				this.sfondoQuadratoAerei.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 1f;
				this.sfondoQuadratoAerei.transform.GetChild(i).GetComponent<CanvasGroup>().interactable = true;
				this.sfondoQuadratoAerei.transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = true;
				this.sfondoQuadratoAerei.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = gameObject.GetComponent<PresenzaAlleato>().immagineUnità;
				if (!base.GetComponent<InfoGenericheAlleati>().ListaDisponibilitàAerei[i])
				{
					if (base.GetComponent<InfoGenericheAlleati>().ListaAereiInVolo[i])
					{
						this.sfondoQuadratoAerei.transform.GetChild(i).GetChild(1).GetComponent<Text>().color = Color.green;
						this.sfondoQuadratoAerei.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "IN ACTION";
					}
					else
					{
						this.sfondoQuadratoAerei.transform.GetChild(i).GetChild(1).GetComponent<Text>().color = Color.red;
						this.sfondoQuadratoAerei.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "REFUELLING";
					}
				}
				else
				{
					this.sfondoQuadratoAerei.transform.GetChild(i).GetChild(1).GetComponent<Text>().color = Color.cyan;
					this.sfondoQuadratoAerei.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "READY";
				}
				if (gameObject.GetComponent<PresenzaAlleato>().tipoTruppa == 44 && base.GetComponent<InfoGenericheAlleati>().ListaDisponibilitàAerei[i])
				{
					this.aereoParàDisponibile = true;
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().numAereoParàDispInQuadro = i;
				}
			}
		}
	}

	// Token: 0x06000022 RID: 34 RVA: 0x0000842C File Offset: 0x0000662C
	private void InfoAlleatoUI()
	{
		if (this.aggiornaAlleatoPerInfo)
		{
			if (this.visualInfoTrappola)
			{
				if (this.origineInfoTrappola == 0)
				{
					this.alleatoPerInfo = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaTrappolePossibili[this.numInfoTrappola];
				}
				else
				{
					this.alleatoPerInfo = this.primaCamera.GetComponent<Selezionamento>().trappolaSelez;
				}
			}
			else if (this.infoDaRinforzi)
			{
				bool flag = false;
				int num = 0;
				while (num < 48 && !flag)
				{
					if (this.ListaAlleatiInTipolSchier[this.slotPerInfoRinforzi].GetComponent<PresenzaAlleato>().tipoTruppa == this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num][0])
					{
						this.alleatoPerInfo = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiPossibili[this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num][0]];
					}
					num++;
				}
			}
			else
			{
				this.alleatoPerInfo = this.infoAlleati.GetComponent<GestioneComandanteInUI>().ListaPosizioni[0][0];
			}
			this.aggiornaAlleatoPerInfo = false;
		}
		if (!this.visualInfoTrappola)
		{
			this.quadratoInformazioniUnità.transform.GetChild(0).GetComponent<Text>().text = this.alleatoPerInfo.GetComponent<PresenzaAlleato>().nomeUnità;
			if (!this.alleatoPerInfo.GetComponent<PresenzaAlleato>().èPerRifornimento)
			{
				if (this.alleatoPerInfo.GetComponent<PresenzaAlleato>().èAereo)
				{
					this.quadratoInformazioniUnità.transform.GetChild(1).GetComponent<Text>().text = string.Concat(new object[]
					{
						"Health:  ",
						this.alleatoPerInfo.GetComponent<PresenzaAlleato>().vita.ToString(),
						"\nCost in Battle Point: ",
						(this.alleatoPerInfo.GetComponent<PresenzaAlleato>().costoInPlastica / 10f).ToString("F0"),
						"\nFuel:  ",
						this.alleatoPerInfo.GetComponent<PresenzaAlleato>().carburante.ToString("F0"),
						"\nSpeed:  ",
						this.alleatoPerInfo.GetComponent<PresenzaAlleato>().velocitàIndicativa,
						"\nVisual Range:  ",
						this.alleatoPerInfo.GetComponent<PresenzaAlleato>().raggioVisivo.ToString(),
						"\nClimbing:  ",
						this.alleatoPerInfo.GetComponent<PresenzaAlleato>().scalatrice.ToString(),
						"\nRepair Step:  ",
						this.alleatoPerInfo.GetComponent<PresenzaAlleato>().velocitàRiparazione,
						"\nCost in Refined Plastic: ",
						this.alleatoPerInfo.GetComponent<PresenzaAlleato>().costoInPlastica
					});
				}
				else
				{
					this.quadratoInformazioniUnità.transform.GetChild(1).GetComponent<Text>().text = string.Concat(new object[]
					{
						"Health:  ",
						this.alleatoPerInfo.GetComponent<PresenzaAlleato>().vita.ToString(),
						"\nCost in Battle Point: ",
						(this.alleatoPerInfo.GetComponent<PresenzaAlleato>().costoInPlastica / 10f).ToString("F0"),
						"\nFuel:  N.D.\nSpeed:  ",
						this.alleatoPerInfo.GetComponent<PresenzaAlleato>().velocitàIndicativa,
						"\nVisual Range:  ",
						this.alleatoPerInfo.GetComponent<PresenzaAlleato>().raggioVisivo.ToString(),
						"\nClimbing:  ",
						this.alleatoPerInfo.GetComponent<PresenzaAlleato>().scalatrice.ToString(),
						"\nRepair Step:  ",
						this.alleatoPerInfo.GetComponent<PresenzaAlleato>().velocitàRiparazione,
						"\nCost in Refined Plastic: ",
						this.alleatoPerInfo.GetComponent<PresenzaAlleato>().costoInPlastica
					});
				}
			}
			else
			{
				this.quadratoInformazioniUnità.transform.GetChild(1).GetComponent<Text>().text = string.Concat(new object[]
				{
					"Health:  ",
					this.alleatoPerInfo.GetComponent<PresenzaAlleato>().vita.ToString(),
					"\nCost in Battle Point: ",
					(this.alleatoPerInfo.GetComponent<PresenzaAlleato>().costoInPlastica / 10f).ToString("F0"),
					"\nSpeed:  ",
					this.alleatoPerInfo.GetComponent<PresenzaAlleato>().velocitàIndicativa,
					"\nSupply Capacity:  ",
					this.alleatoPerInfo.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp.ToString(),
					"\nSupply Range:  ",
					this.alleatoPerInfo.GetComponent<PresenzaAlleato>().raggioDiRifornimento.ToString(),
					"\nClimbing:  ",
					this.alleatoPerInfo.GetComponent<PresenzaAlleato>().scalatrice.ToString(),
					"\nRepair Step:  ",
					this.alleatoPerInfo.GetComponent<PresenzaAlleato>().velocitàRiparazione,
					"\nCost in Refined Plastic: ",
					this.alleatoPerInfo.GetComponent<PresenzaAlleato>().costoInPlastica
				});
			}
			this.quadratoInformazioniUnità.transform.GetChild(2).GetComponent<Text>().text = this.alleatoPerInfo.GetComponent<PresenzaAlleato>().oggettoDescrizione.GetComponent<Text>().text;
		}
		else
		{
			this.quadratoInformazioniUnità.transform.GetChild(0).GetComponent<Text>().text = this.alleatoPerInfo.GetComponent<PresenzaTrappola>().nomeTrappola;
			this.quadratoInformazioniUnità.transform.GetChild(1).GetComponent<Text>().text = string.Concat(new object[]
			{
				"Battle Point Cost: ",
				this.alleatoPerInfo.GetComponent<PresenzaTrappola>().costoPuntiBattaglia,
				"\nHealth: ",
				this.alleatoPerInfo.GetComponent<PresenzaTrappola>().vita,
				"\nDamage: ",
				this.alleatoPerInfo.GetComponent<PresenzaTrappola>().danno,
				"\nPiercing: ",
				this.alleatoPerInfo.GetComponent<PresenzaTrappola>().penetrazione,
				"\nBlast Range: ",
				this.alleatoPerInfo.GetComponent<PresenzaTrappola>().portata,
				"\nRate: ",
				this.alleatoPerInfo.GetComponent<PresenzaTrappola>().frequenzaAttacco,
				"\nEnemy's Speed: ",
				(this.alleatoPerInfo.GetComponent<PresenzaTrappola>().percDiRallentamento * 100f).ToString(),
				"%"
			});
			this.quadratoInformazioniUnità.transform.GetChild(2).GetComponent<Text>().text = this.alleatoPerInfo.GetComponent<PresenzaTrappola>().oggettoDescrizione.GetComponent<Text>().text;
		}
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00008B14 File Offset: 0x00006D14
	private void SchermataLancio()
	{
		if (this.aggiungiParà)
		{
			int num = 0;
			if (this.ListaParàPerLancio.Count > 0)
			{
				for (int i = 0; i < this.ListaParàPerLancio.Count; i++)
				{
					if (this.ListaParàPerLancio[i] != null)
					{
						num++;
					}
				}
			}
			this.aggiungiParà = false;
			if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().puntiBattaglia >= this.ListaPossibiliParà[this.numInListaPossParà].GetComponent<PresenzaAlleato>().costoInPlastica / 10f && base.GetComponent<InfoGenericheAlleati>().ListaAlleati.Count + num < base.GetComponent<InfoGenericheAlleati>().numMaxAlleati)
			{
				int index = 0;
				for (int j = 0; j < 48; j++)
				{
					if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[j][0] == this.ListaPossibiliParà[this.numInListaPossParà].GetComponent<PresenzaAlleato>().tipoTruppa)
					{
						index = j;
						break;
					}
				}
				if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[index][1] > 0)
				{
					if (this.ListaParàPerLancio.Count == 0)
					{
						this.ListaParàPerLancio.Add(this.ListaPossibiliParà[this.numInListaPossParà]);
						List<int> list;
						List<int> expr_170 = list = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[index];
						int num2;
						int expr_174 = num2 = 1;
						num2 = list[num2];
						expr_170[expr_174] = num2 - 1;
						this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().puntiBattaglia -= this.ListaPossibiliParà[this.numInListaPossParà].GetComponent<PresenzaAlleato>().costoInPlastica / 10f;
					}
					else
					{
						bool flag = false;
						for (int k = 0; k < this.ListaParàPerLancio.Count; k++)
						{
						}
						if (!flag && this.ListaParàPerLancio.Count <= 6)
						{
							this.ListaParàPerLancio.Add(this.ListaPossibiliParà[this.numInListaPossParà]);
							List<int> list2;
							List<int> expr_235 = list2 = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[index];
							int num2;
							int expr_239 = num2 = 1;
							num2 = list2[num2];
							expr_235[expr_239] = num2 - 1;
							this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().puntiBattaglia -= this.ListaPossibiliParà[this.numInListaPossParà].GetComponent<PresenzaAlleato>().costoInPlastica / 10f;
						}
					}
				}
			}
		}
		if (this.togliParà)
		{
			this.togliParà = false;
			if (this.ListaParàPerLancio.Count > 0 && this.ListaParàPerLancio.Count >= this.numInListaParà + 1)
			{
				for (int l = 0; l < 48; l++)
				{
					if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[l][0] == this.ListaParàPerLancio[this.numInListaParà].GetComponent<PresenzaAlleato>().tipoTruppa)
					{
						List<int> list3;
						List<int> expr_31F = list3 = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[l];
						int num2;
						int expr_323 = num2 = 1;
						num2 = list3[num2];
						expr_31F[expr_323] = num2 + 1;
						this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().puntiBattaglia += this.ListaPossibiliParà[this.numInListaPossParà].GetComponent<PresenzaAlleato>().costoInPlastica / 10f;
						break;
					}
				}
				this.ListaParàPerLancio.RemoveAt(this.numInListaParà);
			}
		}
		if (this.aggListaPossParà)
		{
			this.aggListaPossParà = false;
			bool flag2 = false;
			int num3 = 0;
			int num4 = 0;
			while (num4 < 48 && !flag2)
			{
				if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num4][0] != 100)
				{
					GameObject gameObject = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiPossibili[this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num4][0]];
					if (gameObject.GetComponent<PresenzaAlleato>().èFanteria && gameObject.GetComponent<PresenzaAlleato>().tipoTruppa != 11 && gameObject.GetComponent<PresenzaAlleato>().tipoTruppa != 15)
					{
						this.elencoPossibiliParà.transform.GetChild(num3).GetComponent<CanvasGroup>().alpha = 1f;
						this.elencoPossibiliParà.transform.GetChild(num3).GetComponent<CanvasGroup>().interactable = true;
						this.elencoPossibiliParà.transform.GetChild(num3).GetComponent<CanvasGroup>().blocksRaycasts = true;
						this.elencoPossibiliParà.transform.GetChild(num3).GetChild(0).GetComponent<Image>().sprite = gameObject.GetComponent<PresenzaAlleato>().immagineUnità;
						this.elencoPossibiliParà.transform.GetChild(num3).GetChild(1).GetComponent<Text>().text = gameObject.GetComponent<PresenzaAlleato>().nomeUnità;
						this.elencoPossibiliParà.transform.GetChild(num3).GetChild(2).GetComponent<Text>().text = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num4][1].ToString();
						this.elencoPossibiliParà.transform.GetChild(num3).GetChild(3).GetChild(0).GetComponent<Text>().text = (gameObject.GetComponent<PresenzaAlleato>().costoInPlastica / 10f).ToString() + " BP";
						this.ListaPossibiliParà.Add(gameObject);
						if (num3 < 14)
						{
							num3++;
						}
					}
				}
				else
				{
					flag2 = true;
				}
				num4++;
			}
		}
		if (this.aggListaParà)
		{
			this.aggListaParà = false;
			for (int m = 0; m < 7; m++)
			{
				if (this.ListaParàPerLancio.Count >= m + 1 && this.ListaParàPerLancio[m] != null)
				{
					this.elencoParà.transform.GetChild(m).transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1f;
					this.elencoParà.transform.GetChild(m).transform.GetChild(0).GetComponent<CanvasGroup>().interactable = true;
					this.elencoParà.transform.GetChild(m).transform.GetChild(0).GetComponent<CanvasGroup>().blocksRaycasts = true;
					this.elencoParà.transform.GetChild(m).transform.GetChild(0).GetComponent<Image>().sprite = this.ListaParàPerLancio[m].GetComponent<PresenzaAlleato>().immagineUnità;
				}
				else
				{
					this.elencoParà.transform.GetChild(m).transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
					this.elencoParà.transform.GetChild(m).transform.GetChild(0).GetComponent<CanvasGroup>().interactable = false;
					this.elencoParà.transform.GetChild(m).transform.GetChild(0).GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
			}
		}
		if (this.ListaParàPerLancio.Count == 0)
		{
			this.pulsanteProntoAlLancio.GetComponent<Button>().interactable = false;
		}
		else
		{
			this.pulsanteProntoAlLancio.GetComponent<Button>().interactable = true;
		}
		if (this.annullaLancio)
		{
			for (int n = 0; n < this.ListaParàPerLancio.Count; n++)
			{
				if (!(this.ListaParàPerLancio[n] != null))
				{
					break;
				}
				for (int num5 = 0; num5 < 48; num5++)
				{
					if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num5][0] == this.ListaParàPerLancio[n].GetComponent<PresenzaAlleato>().tipoTruppa)
					{
						List<int> list4;
						List<int> expr_815 = list4 = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num5];
						int num6;
						int expr_819 = num6 = 1;
						num6 = list4[num6];
						expr_815[expr_819] = num6 + 1;
						this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().puntiBattaglia += this.ListaParàPerLancio[n].GetComponent<PresenzaAlleato>().costoInPlastica / 10f;
						break;
					}
				}
			}
		}
	}

	// Token: 0x06000024 RID: 36 RVA: 0x000093BC File Offset: 0x000075BC
	private void VisualizzaMunizioni()
	{
		if (this.timerPannMuniz > 1.5f)
		{
			this.aggPannMuniz = true;
		}
		if (this.aggPannMuniz)
		{
			this.aggPannMuniz = false;
			this.timerPannMuniz = 0f;
			for (int i = 0; i < this.infoAlleati.GetComponent<InfoMunizionamento>().ListaTipiMunizioniBaseTattica.Count; i++)
			{
				GameObject gameObject = this.infoAlleati.GetComponent<InfoMunizionamento>().ListaTipiMunizioniBaseTattica[i];
				this.elencoMunizioniInPannello.transform.GetChild(i).GetComponent<Text>().text = gameObject.GetComponent<QuantitàMunizione>().name + ": " + gameObject.GetComponent<QuantitàMunizione>().quantità.ToString("F0");
			}
		}
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00009480 File Offset: 0x00007680
	private void FunzioneStatistiche()
	{
		this.sfondoStatistiche.transform.GetChild(0).GetComponent<Text>().text = "Allies on battlefield:  " + base.GetComponent<InfoGenericheAlleati>().ListaAlleati.Count.ToString() + " / " + this.numMaxAlleati.ToString();
		this.sfondoStatistiche.transform.GetChild(1).GetComponent<Text>().text = "Dead Allies:  " + GestoreNeutroTattica.numAlleatiMorti.ToString();
		this.sfondoStatistiche.transform.GetChild(2).GetComponent<Text>().text = "Allies on standby:  " + base.GetComponent<InfoGenericheAlleati>().alleatiDiRiserva.ToString();
		this.sfondoStatistiche.transform.GetChild(3).GetComponent<Text>().text = "Enemies on battlefield:  " + this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici.Count.ToString() + " / " + this.numMaxNemici.ToString();
		this.sfondoStatistiche.transform.GetChild(4).GetComponent<Text>().text = "Dead Enemies:  " + GestoreNeutroTattica.numNemiciMorti.ToString();
		this.sfondoStatistiche.transform.GetChild(5).GetComponent<Text>().text = "Enemy groups on standby:  " + this.IANemico.GetComponent<InfoGenericheNemici>().numRinforziNemici.ToString() + " G";
	}

	// Token: 0x0400002F RID: 47
	private GameObject CanvasComandante;

	// Token: 0x04000030 RID: 48
	private GameObject barraUnitàSelez;

	// Token: 0x04000031 RID: 49
	private GameObject modalitàUnitàSingola;

	// Token: 0x04000032 RID: 50
	private GameObject modalitàUnitàMultipla;

	// Token: 0x04000033 RID: 51
	private GameObject datiGenerali;

	// Token: 0x04000034 RID: 52
	private List<GameObject> ListaArmiUI;

	// Token: 0x04000035 RID: 53
	private GameObject arma1;

	// Token: 0x04000036 RID: 54
	private GameObject arma2;

	// Token: 0x04000037 RID: 55
	private GameObject arma3;

	// Token: 0x04000038 RID: 56
	private GameObject arma4;

	// Token: 0x04000039 RID: 57
	private GameObject quadratoDestro;

	// Token: 0x0400003A RID: 58
	public List<GameObject> ListaTipiMunizioni;

	// Token: 0x0400003B RID: 59
	private GameObject tipoMunizione1;

	// Token: 0x0400003C RID: 60
	private GameObject tipoMunizione2;

	// Token: 0x0400003D RID: 61
	private GameObject tipoMunizione3;

	// Token: 0x0400003E RID: 62
	private GameObject tipoMunizione4;

	// Token: 0x0400003F RID: 63
	private GameObject tipoMunizione5;

	// Token: 0x04000040 RID: 64
	public GameObject quadratoInformazioniUnità;

	// Token: 0x04000041 RID: 65
	private GameObject sopraRettangolo;

	// Token: 0x04000042 RID: 66
	private GameObject barraTipolRinforzi;

	// Token: 0x04000043 RID: 67
	private GameObject elencoUnitàRinforzi;

	// Token: 0x04000044 RID: 68
	private GameObject sfondoQuadratoAerei;

	// Token: 0x04000045 RID: 69
	private GameObject scritteAvvSchieramento;

	// Token: 0x04000046 RID: 70
	private GameObject infoNeutreTattica;

	// Token: 0x04000047 RID: 71
	private GameObject barraSuperiore;

	// Token: 0x04000048 RID: 72
	private GameObject quadroAerei;

	// Token: 0x04000049 RID: 73
	private GameObject quadroInfoTrappola;

	// Token: 0x0400004A RID: 74
	private GameObject quadroSelezMunizioni;

	// Token: 0x0400004B RID: 75
	private GameObject rettPerArmi;

	// Token: 0x0400004C RID: 76
	private GameObject rettPerTrappole;

	// Token: 0x0400004D RID: 77
	private GameObject inizioBattaglia;

	// Token: 0x0400004E RID: 78
	private GameObject modalitàTrappola;

	// Token: 0x0400004F RID: 79
	private GameObject scrittaQuotaTrappola;

	// Token: 0x04000050 RID: 80
	private GameObject pulsanteAtterra;

	// Token: 0x04000051 RID: 81
	private GameObject schermataParà;

	// Token: 0x04000052 RID: 82
	private GameObject elencoPossibiliParà;

	// Token: 0x04000053 RID: 83
	private GameObject elencoParà;

	// Token: 0x04000054 RID: 84
	private GameObject pulsanteProntoAlLancio;

	// Token: 0x04000055 RID: 85
	private GameObject pannelloMunizioni;

	// Token: 0x04000056 RID: 86
	private GameObject elencoMunizioniInPannello;

	// Token: 0x04000057 RID: 87
	private GameObject pulsanteFuocoSalvaRockArt;

	// Token: 0x04000058 RID: 88
	private GameObject spuntaCompDifensivo;

	// Token: 0x04000059 RID: 89
	private GameObject spuntaCercaAutomBers;

	// Token: 0x0400005A RID: 90
	private GameObject spuntaCercaBersDif;

	// Token: 0x0400005B RID: 91
	private GameObject spuntaCercaBersDifVicino;

	// Token: 0x0400005C RID: 92
	private GameObject sfondoStatistiche;

	// Token: 0x0400005D RID: 93
	private GameObject contoAllaRovescia;

	// Token: 0x0400005E RID: 94
	private GameObject scrittaAvvertSapper;

	// Token: 0x0400005F RID: 95
	private GameObject terzaCamera;

	// Token: 0x04000060 RID: 96
	private GameObject primaCamera;

	// Token: 0x04000061 RID: 97
	private GameObject infoAlleati;

	// Token: 0x04000062 RID: 98
	private GameObject attacchiSpecialiAlleati;

	// Token: 0x04000063 RID: 99
	private GameObject IANemico;

	// Token: 0x04000064 RID: 100
	public GameObject unitàSelezionata;

	// Token: 0x04000065 RID: 101
	private bool selezioneMonoTipo;

	// Token: 0x04000066 RID: 102
	private bool selezioneMultiTipo;

	// Token: 0x04000067 RID: 103
	public int numeroTipoDiverseSel;

	// Token: 0x04000068 RID: 104
	public List<GameObject> ListaTipiTruppeSel;

	// Token: 0x04000069 RID: 105
	public List<List<GameObject>> ListaPosizioni;

	// Token: 0x0400006A RID: 106
	public List<GameObject> ListaPosizione0;

	// Token: 0x0400006B RID: 107
	private List<GameObject> ListaPosizione1;

	// Token: 0x0400006C RID: 108
	private List<GameObject> ListaPosizione2;

	// Token: 0x0400006D RID: 109
	private List<GameObject> ListaPosizione3;

	// Token: 0x0400006E RID: 110
	private List<GameObject> ListaPosizione4;

	// Token: 0x0400006F RID: 111
	private List<GameObject> ListaPosizione5;

	// Token: 0x04000070 RID: 112
	private List<GameObject> ListaPosizione6;

	// Token: 0x04000071 RID: 113
	private List<GameObject> ListaPosizione7;

	// Token: 0x04000072 RID: 114
	private List<GameObject> ListaPosizione8;

	// Token: 0x04000073 RID: 115
	private List<GameObject> ListaPosizione9;

	// Token: 0x04000074 RID: 116
	public Sprite puntiniDiSospensione;

	// Token: 0x04000075 RID: 117
	public Color coloreArmaAttiva;

	// Token: 0x04000076 RID: 118
	public Color coloreArmaDisattiva;

	// Token: 0x04000077 RID: 119
	public int numeroArmaSelezionata;

	// Token: 0x04000078 RID: 120
	public bool evidenziaAlleatiENemici;

	// Token: 0x04000079 RID: 121
	public Material coloreSelAlleati;

	// Token: 0x0400007A RID: 122
	public Material coloreEvidAlleati;

	// Token: 0x0400007B RID: 123
	public Material coloreSelInsetti;

	// Token: 0x0400007C RID: 124
	public Material coloreEvidInsetti;

	// Token: 0x0400007D RID: 125
	public int tipoElencoRinforzi;

	// Token: 0x0400007E RID: 126
	public bool aggiornaElencoRinforzi;

	// Token: 0x0400007F RID: 127
	public int slotRinforzoSelez;

	// Token: 0x04000080 RID: 128
	private int ultimoTipoSchierUsato;

	// Token: 0x04000081 RID: 129
	public bool aggiornaListaRinforzi;

	// Token: 0x04000082 RID: 130
	public bool creaAlleatoPerSchieramento;

	// Token: 0x04000083 RID: 131
	public List<GameObject> ListaAlleatiInTipolSchier;

	// Token: 0x04000084 RID: 132
	public Vector3 posSchierUnità;

	// Token: 0x04000085 RID: 133
	public bool alleatoDaPosizionare;

	// Token: 0x04000086 RID: 134
	public GameObject alleatoèDaSchier;

	// Token: 0x04000087 RID: 135
	public GameObject alleatoRealeèDaSchier;

	// Token: 0x04000088 RID: 136
	private int indiceTruppa;

	// Token: 0x04000089 RID: 137
	public bool cancRinforzoSchierato;

	// Token: 0x0400008A RID: 138
	public int tipoTruppaDaCancellare;

	// Token: 0x0400008B RID: 139
	public bool posPerSchierValida;

	// Token: 0x0400008C RID: 140
	public bool chiudiRinforzi;

	// Token: 0x0400008D RID: 141
	public bool infoDaRinforzi;

	// Token: 0x0400008E RID: 142
	public int slotPerInfoRinforzi;

	// Token: 0x0400008F RID: 143
	public bool aggiornaAlleatoPerInfo;

	// Token: 0x04000090 RID: 144
	private GameObject alleatoPerInfo;

	// Token: 0x04000091 RID: 145
	private float timerScrittaAvvSchier;

	// Token: 0x04000092 RID: 146
	private bool partenzaTimerScrittaAvvSchier;

	// Token: 0x04000093 RID: 147
	public bool quadroAereoAperto;

	// Token: 0x04000094 RID: 148
	public bool apriQuadroAereo;

	// Token: 0x04000095 RID: 149
	public bool pulsApriChiudiQAPremuto;

	// Token: 0x04000096 RID: 150
	public int numInfoTrappola;

	// Token: 0x04000097 RID: 151
	public bool visualInfoTrappola;

	// Token: 0x04000098 RID: 152
	private GameObject trappolaSelez;

	// Token: 0x04000099 RID: 153
	public int origineInfoTrappola;

	// Token: 0x0400009A RID: 154
	public bool preparLancioParàAttivo;

	// Token: 0x0400009B RID: 155
	private bool aereoParàDisponibile;

	// Token: 0x0400009C RID: 156
	public List<GameObject> ListaPossibiliParà;

	// Token: 0x0400009D RID: 157
	public List<GameObject> ListaParàPerLancio;

	// Token: 0x0400009E RID: 158
	public int numInListaPossParà;

	// Token: 0x0400009F RID: 159
	public int numInListaParà;

	// Token: 0x040000A0 RID: 160
	public bool aggiungiParà;

	// Token: 0x040000A1 RID: 161
	public bool togliParà;

	// Token: 0x040000A2 RID: 162
	public bool aggListaPossParà;

	// Token: 0x040000A3 RID: 163
	public bool aggListaParà;

	// Token: 0x040000A4 RID: 164
	public bool mirinoParàAttivo;

	// Token: 0x040000A5 RID: 165
	public bool partenzaAereoParà;

	// Token: 0x040000A6 RID: 166
	public bool annullaLancio;

	// Token: 0x040000A7 RID: 167
	private float timerPannMuniz;

	// Token: 0x040000A8 RID: 168
	public bool aggPannMuniz;

	// Token: 0x040000A9 RID: 169
	public bool aggMorte;

	// Token: 0x040000AA RID: 170
	public bool statisticheAperte;

	// Token: 0x040000AB RID: 171
	private int numMaxAlleati;

	// Token: 0x040000AC RID: 172
	private int numMaxNemici;

	// Token: 0x040000AD RID: 173
	private bool visualePanoramica;

	// Token: 0x040000AE RID: 174
	public float timerCountdown;

	// Token: 0x040000AF RID: 175
	private float tempoCountdown;

	// Token: 0x040000B0 RID: 176
	public bool fineCountdown;

	// Token: 0x040000B1 RID: 177
	public int tipoBattaglia;

	// Token: 0x040000B2 RID: 178
	public bool partenzaTimerScrAvvSapper;

	// Token: 0x040000B3 RID: 179
	private float timerScrittaAvvSapper;

	// Token: 0x040000B4 RID: 180
	private int numeroGruppoSchier;
}
