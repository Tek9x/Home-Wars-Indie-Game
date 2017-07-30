using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000009 RID: 9
public class GestioneRisorseEHeadquartiers : MonoBehaviour
{
	// Token: 0x0600003F RID: 63 RVA: 0x0000FC28 File Offset: 0x0000DE28
	private void Start()
	{
		this.inizioLivello = GameObject.FindGameObjectWithTag("InizioLivello");
		this.cameraGiocatore = GameObject.FindGameObjectWithTag("MainCamera");
		this.Schede = GameObject.FindGameObjectWithTag("Schede");
		this.scheda1 = this.Schede.transform.FindChild("scheda 1").gameObject;
		this.elencoRisorsePresenti = this.scheda1.transform.FindChild("resoconto risorse").FindChild("questo turno").gameObject;
		this.elencoRisorseProsTurno = this.scheda1.transform.FindChild("resoconto risorse").FindChild("prossimo turno").gameObject;
		this.pulsanteFitPerStrategia = GameObject.FindGameObjectWithTag("PulsFittStrategia");
		this.cameraCasa = GameObject.FindGameObjectWithTag("MainCamera");
		this.elencoPostiEdifici = base.transform.FindChild("lista posti").gameObject;
		this.varieMappaStrategica = GameObject.FindGameObjectWithTag("VarieMappaStrategica");
		this.pannelloInfo = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Gestione Headquarters").FindChild("info costruzioni").gameObject;
		this.scrittaNonCiSonoRisorse = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Scritte Varie").FindChild("sfondo scritta non hai risorse").gameObject;
		this.pulsanteAccesoOSpento = this.pannelloInfo.transform.FindChild("pulsante acceso o spento").gameObject;
		this.barraAltaAlleati = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Barra Alta").FindChild("barra alleati").gameObject;
		this.sfondoInfoRisorsa = this.scheda1.transform.FindChild("info risorsa").GetChild(0).gameObject;
		this.centroStanzaUI = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Centro Stanza").gameObject;
		this.pulsanteSatellite = this.centroStanzaUI.transform.FindChild("lancia o abbatti satellite").gameObject;
		this.domandaLanciareOAbbattereSat = this.centroStanzaUI.transform.FindChild("domanda lanciare o abbattere satellite").GetChild(0).gameObject;
		if (this.ListaRisorsePresenti.Count == 0)
		{
			this.ListaRisorsePresenti = new List<GameObject>();
			this.ListaRisorsePresenti.Add(this.plasticaGrezzaPres);
			this.ListaRisorsePresenti.Add(this.plasticaRaffinataPres);
			this.ListaRisorsePresenti.Add(this.metalloGrezzoPres);
			this.ListaRisorsePresenti.Add(this.metalloRaffinatoPres);
			this.ListaRisorsePresenti.Add(this.energiaGrezzaPres);
			this.ListaRisorsePresenti.Add(this.energiaRaffinataPres);
			this.ListaRisorsePresenti.Add(this.incendiarioGrezzoPres);
			this.ListaRisorsePresenti.Add(this.incendiarioRaffinatoPres);
			this.ListaRisorsePresenti.Add(this.tossicoGrezzoPres);
			this.ListaRisorsePresenti.Add(this.tossicoRaffinatoPres);
			this.ListaRisorsePresenti.Add(this.esperienzaPres);
		}
		this.ListaRisorseFuture = new List<float>();
		this.ListaRisorseFuture.Add(this.plasticaGrezzaFut);
		this.ListaRisorseFuture.Add(this.plasticaRaffinataFut);
		this.ListaRisorseFuture.Add(this.metalloGrezzoFut);
		this.ListaRisorseFuture.Add(this.metalloRaffinatoFut);
		this.ListaRisorseFuture.Add(this.energiaGrezzaFut);
		this.ListaRisorseFuture.Add(this.energiaRaffinataFut);
		this.ListaRisorseFuture.Add(this.incendiarioGrezzoFut);
		this.ListaRisorseFuture.Add(this.incendiarioRaffinatoFut);
		this.ListaRisorseFuture.Add(this.tossicoGrezzoFut);
		this.ListaRisorseFuture.Add(this.tossicoRaffinatoFut);
		this.ListaRisorseFuture.Add(this.esperienzaFut);
		if (this.inizioLivello.GetComponent<CaricaDati>().senzaInizializ)
		{
			this.inizioLivello.GetComponent<CaricaDati>().senzaInizializ = false;
			for (int i = 0; i < base.GetComponent<GestioneSblocchi>().ListaSblocchi.Count; i++)
			{
				base.GetComponent<GestioneSblocchi>().ListaSblocchi[i].GetComponent<PresenzaSblocco>().èSbloccato = 0;
			}
			base.GetComponent<GestioneSblocchi>().ListaSblocchi[0].GetComponent<PresenzaSblocco>().èSbloccato = 1;
			if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().numeroTurno == 0)
			{
				this.ListaRisorsePresenti[0].GetComponent<PresenzaRisorsa>().quantitàRisorsa = 200f;
				this.ListaRisorsePresenti[1].GetComponent<PresenzaRisorsa>().quantitàRisorsa = 2000f;
				this.ListaRisorsePresenti[2].GetComponent<PresenzaRisorsa>().quantitàRisorsa = 70f;
				this.ListaRisorsePresenti[3].GetComponent<PresenzaRisorsa>().quantitàRisorsa = 150f;
				this.ListaRisorsePresenti[4].GetComponent<PresenzaRisorsa>().quantitàRisorsa = 70f;
				this.ListaRisorsePresenti[5].GetComponent<PresenzaRisorsa>().quantitàRisorsa = 50f;
				this.ListaRisorsePresenti[6].GetComponent<PresenzaRisorsa>().quantitàRisorsa = 50f;
				this.ListaRisorsePresenti[7].GetComponent<PresenzaRisorsa>().quantitàRisorsa = 10f;
				this.ListaRisorsePresenti[8].GetComponent<PresenzaRisorsa>().quantitàRisorsa = 30f;
				this.ListaRisorsePresenti[9].GetComponent<PresenzaRisorsa>().quantitàRisorsa = 5f;
				this.ListaRisorsePresenti[10].GetComponent<PresenzaRisorsa>().quantitàRisorsa = (float)(200 + GestoreNeutroStrategia.stagione * 100);
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[0].GetComponent<QuantitàMunizione>().quantità = 30000f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[1].GetComponent<QuantitàMunizione>().quantità = 5000f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[2].GetComponent<QuantitàMunizione>().quantità = 2000f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[3].GetComponent<QuantitàMunizione>().quantità = 100f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[4].GetComponent<QuantitàMunizione>().quantità = 1000f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[5].GetComponent<QuantitàMunizione>().quantità = 200f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[6].GetComponent<QuantitàMunizione>().quantità = 150f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[7].GetComponent<QuantitàMunizione>().quantità = 100f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[8].GetComponent<QuantitàMunizione>().quantità = 50f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[9].GetComponent<QuantitàMunizione>().quantità = 200f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[10].GetComponent<QuantitàMunizione>().quantità = 1000f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[11].GetComponent<QuantitàMunizione>().quantità = 1000f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[12].GetComponent<QuantitàMunizione>().quantità = 100f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[13].GetComponent<QuantitàMunizione>().quantità = 50f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[14].GetComponent<QuantitàMunizione>().quantità = 30f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[15].GetComponent<QuantitàMunizione>().quantità = 80f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[16].GetComponent<QuantitàMunizione>().quantità = 40f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[17].GetComponent<QuantitàMunizione>().quantità = 20f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[18].GetComponent<QuantitàMunizione>().quantità = 80f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[19].GetComponent<QuantitàMunizione>().quantità = 40f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[20].GetComponent<QuantitàMunizione>().quantità = 20f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[21].GetComponent<QuantitàMunizione>().quantità = 30f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[22].GetComponent<QuantitàMunizione>().quantità = 15f;
				base.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[23].GetComponent<QuantitàMunizione>().quantità = 5f;
				for (int j = 0; j < this.ListaPostiInHeadquarters.Count; j++)
				{
					this.ListaPostiInHeadquarters[j] = 100;
				}
			}
		}
	}

	// Token: 0x06000040 RID: 64 RVA: 0x000104FC File Offset: 0x0000E6FC
	private void Update()
	{
		this.CalcoloRisorse();
		if (this.avviaCostruzione)
		{
			this.CostruisciStruttura();
			this.avviaCostruzione = false;
		}
		if (this.avviaDemolizione)
		{
			this.DemolisciStruttura();
			this.avviaDemolizione = false;
		}
		if (this.pannelloInfo.GetComponent<CanvasGroup>().alpha == 1f)
		{
			this.DettagliInfoEdifici();
		}
		if (this.aggiornaLavoroEdifici)
		{
			this.CalcolaLavoroEdifici();
			this.aggiornaLavoroEdifici = false;
		}
		if (this.scrittaNonCiSonoRisorse.GetComponent<CanvasGroup>().alpha == 1f)
		{
			this.timerScrittaNonCiSonoRisorse += Time.deltaTime;
			if (this.timerScrittaNonCiSonoRisorse > 2f)
			{
				this.scrittaNonCiSonoRisorse.GetComponent<CanvasGroup>().alpha = 0f;
				this.timerScrittaNonCiSonoRisorse = 0f;
			}
		}
		if (this.pulsanteFitPerStrategia.GetComponent<PulsFitPerStrategia>().schedaAperta == 0)
		{
			this.ResocontoInHeadquartiers();
		}
		if (this.sfondoInfoRisorsa.transform.parent.GetComponent<CanvasGroup>().alpha == 1f)
		{
			this.MostraDettagliRisorsa();
		}
		if (this.centroStanzaUI.GetComponent<CanvasGroup>().alpha == 1f)
		{
			this.GestioneSatellite();
		}
		this.ResocontoInBarraAlto();
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00010640 File Offset: 0x0000E840
	private void CalcoloRisorse()
	{
		this.totFutPlasticaGrezzaDaStanze = 0f;
		this.totFutMetalloGrezzoDaStanze = 0f;
		this.totFutEnergiaGrezzaDaStanze = 0f;
		this.totFutIncendiarioGrezzoDaStanze = 0f;
		this.totFutTossicoGrezzoDaStanze = 0f;
		for (int i = 0; i < 11; i++)
		{
			this.ListaRisorseFuture[i] = 0f;
		}
		for (int j = 0; j < this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.Count; j++)
		{
			this.totFutPlasticaGrezzaDaStanze += this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[j].GetComponent<CentroStanza>().guadagnoRealePlastica;
			this.totFutMetalloGrezzoDaStanze += this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[j].GetComponent<CentroStanza>().guadagnoRealeMetallo;
			this.totFutEnergiaGrezzaDaStanze += this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[j].GetComponent<CentroStanza>().guadagnoRealeEnergia;
			this.totFutIncendiarioGrezzoDaStanze += this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[j].GetComponent<CentroStanza>().guadagnoRealeMatIncendiario;
			this.totFutTossicoGrezzoDaStanze += this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[j].GetComponent<CentroStanza>().guadagnoRealeMatTossico;
		}
		this.ListaRisorseFuture[0] = this.totFutPlasticaGrezzaDaStanze + this.postLavEdificioPlasticaGrezza;
		this.ListaRisorseFuture[1] = this.postLavEdificioPlasticaRaffinata;
		this.ListaRisorseFuture[2] = this.totFutMetalloGrezzoDaStanze + this.postLavEdificioMetalloGrezzo;
		this.ListaRisorseFuture[3] = this.postLavEdificioMetalloRaffinato;
		this.ListaRisorseFuture[4] = this.totFutEnergiaGrezzaDaStanze + this.postLavEdificioEnergiaGrezza;
		this.ListaRisorseFuture[5] = this.postLavEdificioEnergiaRaffinata;
		this.ListaRisorseFuture[6] = this.totFutIncendiarioGrezzoDaStanze + this.postLavEdificioIncendiarioGrezzo;
		this.ListaRisorseFuture[7] = this.postLavEdificioIncendiarioRaffinato;
		this.ListaRisorseFuture[8] = this.totFutTossicoGrezzoDaStanze + this.postLavEdificioTossicoGrezzo;
		this.ListaRisorseFuture[9] = this.postLavEdificioTossicoRaffinato;
		this.ListaRisorseFuture[10] = this.postLavEdificioEsperienza;
		if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().scattoTurnoVersoAlleati)
		{
			for (int k = 0; k < 11; k++)
			{
				this.ListaRisorsePresenti[k].GetComponent<PresenzaRisorsa>().quantitàRisorsa += this.ListaRisorseFuture[k];
			}
		}
	}

	// Token: 0x06000042 RID: 66 RVA: 0x000108EC File Offset: 0x0000EAEC
	private void ResocontoInHeadquartiers()
	{
		for (int i = 0; i < 11; i++)
		{
			this.elencoRisorsePresenti.transform.GetChild(i + 1).GetComponent<Text>().text = this.ListaRisorsePresenti[i].GetComponent<PresenzaRisorsa>().quantitàRisorsa.ToString("F0");
		}
		for (int j = 0; j < 11; j++)
		{
			if (this.ListaRisorseFuture[j] > 0f)
			{
				this.elencoRisorseProsTurno.transform.GetChild(j + 1).GetComponent<Text>().text = "+" + this.ListaRisorseFuture[j].ToString("F0");
			}
			else
			{
				this.elencoRisorseProsTurno.transform.GetChild(j + 1).GetComponent<Text>().text = this.ListaRisorseFuture[j].ToString("F0");
			}
		}
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000109F0 File Offset: 0x0000EBF0
	private void ResocontoInBarraAlto()
	{
		if (this.ListaRisorseFuture[1] > 0f)
		{
			this.barraAltaAlleati.transform.GetChild(1).GetComponent<Text>().text = this.ListaRisorsePresenti[1].GetComponent<PresenzaRisorsa>().quantitàRisorsa.ToString("F0") + "(+" + this.ListaRisorseFuture[1].ToString("F0") + ")";
		}
		else
		{
			this.barraAltaAlleati.transform.GetChild(1).GetComponent<Text>().text = this.ListaRisorsePresenti[1].GetComponent<PresenzaRisorsa>().quantitàRisorsa.ToString("F0") + "(" + this.ListaRisorseFuture[1].ToString("F0") + ")";
		}
		if (this.ListaRisorseFuture[3] > 0f)
		{
			this.barraAltaAlleati.transform.GetChild(3).GetComponent<Text>().text = this.ListaRisorsePresenti[3].GetComponent<PresenzaRisorsa>().quantitàRisorsa.ToString("F0") + "(+" + this.ListaRisorseFuture[3].ToString("F0") + ")";
		}
		else
		{
			this.barraAltaAlleati.transform.GetChild(3).GetComponent<Text>().text = this.ListaRisorsePresenti[3].GetComponent<PresenzaRisorsa>().quantitàRisorsa.ToString("F0") + "(" + this.ListaRisorseFuture[3].ToString("F0") + ")";
		}
		if (this.ListaRisorseFuture[5] > 0f)
		{
			this.barraAltaAlleati.transform.GetChild(5).GetComponent<Text>().text = this.ListaRisorsePresenti[5].GetComponent<PresenzaRisorsa>().quantitàRisorsa.ToString("F0") + "(+" + this.ListaRisorseFuture[5].ToString("F0") + ")";
		}
		else
		{
			this.barraAltaAlleati.transform.GetChild(5).GetComponent<Text>().text = this.ListaRisorsePresenti[5].GetComponent<PresenzaRisorsa>().quantitàRisorsa.ToString("F0") + "(" + this.ListaRisorseFuture[5].ToString("F0") + ")";
		}
		if (this.ListaRisorseFuture[7] > 0f)
		{
			this.barraAltaAlleati.transform.GetChild(7).GetComponent<Text>().text = this.ListaRisorsePresenti[7].GetComponent<PresenzaRisorsa>().quantitàRisorsa.ToString("F0") + "(+" + this.ListaRisorseFuture[7].ToString("F0") + ")";
		}
		else
		{
			this.barraAltaAlleati.transform.GetChild(7).GetComponent<Text>().text = this.ListaRisorsePresenti[7].GetComponent<PresenzaRisorsa>().quantitàRisorsa.ToString("F0") + "(" + this.ListaRisorseFuture[7].ToString("F0") + ")";
		}
		if (this.ListaRisorseFuture[9] > 0f)
		{
			this.barraAltaAlleati.transform.GetChild(9).GetComponent<Text>().text = this.ListaRisorsePresenti[9].GetComponent<PresenzaRisorsa>().quantitàRisorsa.ToString("F0") + "(+" + this.ListaRisorseFuture[9].ToString("F0") + ")";
		}
		else
		{
			this.barraAltaAlleati.transform.GetChild(9).GetComponent<Text>().text = this.ListaRisorsePresenti[9].GetComponent<PresenzaRisorsa>().quantitàRisorsa.ToString("F0") + "(" + this.ListaRisorseFuture[9].ToString("F0") + ")";
		}
		this.barraAltaAlleati.transform.GetChild(11).GetComponent<Text>().text = this.ListaRisorsePresenti[10].GetComponent<PresenzaRisorsa>().quantitàRisorsa.ToString("F0") + "(+" + this.ListaRisorseFuture[10].ToString("F0") + ")";
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00010ED8 File Offset: 0x0000F0D8
	private void CostruisciStruttura()
	{
		float num = this.ListaRisorsePresenti[1].GetComponent<PresenzaRisorsa>().quantitàRisorsa - this.ListaEdificiPossibili[this.tipologiaEdificio].GetComponent<PresenzaEdificio>().costoCostruzioneInPlastica;
		float num2 = this.ListaRisorsePresenti[3].GetComponent<PresenzaRisorsa>().quantitàRisorsa - this.ListaEdificiPossibili[this.tipologiaEdificio].GetComponent<PresenzaEdificio>().costoCostruzioneInMetallo;
		if (num >= 0f && num2 >= 0f)
		{
			this.ListaRisorsePresenti[1].GetComponent<PresenzaRisorsa>().quantitàRisorsa -= this.ListaEdificiPossibili[this.tipologiaEdificio].GetComponent<PresenzaEdificio>().costoCostruzioneInPlastica;
			this.ListaRisorsePresenti[3].GetComponent<PresenzaRisorsa>().quantitàRisorsa -= this.ListaEdificiPossibili[this.tipologiaEdificio].GetComponent<PresenzaEdificio>().costoCostruzioneInMetallo;
			GameObject gameObject = UnityEngine.Object.Instantiate(this.ListaEdificiPossibili[this.tipologiaEdificio], Vector3.zero, base.transform.rotation) as GameObject;
			gameObject.transform.parent = this.elencoPostiEdifici.transform.GetChild(this.numeroPosto);
			gameObject.transform.localPosition = new Vector3(0f, 0f, 1f);
			this.ListaPostiInHeadquarters[this.numeroPosto] = this.tipologiaEdificio;
			gameObject.GetComponent<PresenzaEdificio>().aggiornaFumo = true;
			base.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
			base.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = base.GetComponent<GestioneSuoniCasa>().suonoCostruzEdificio;
		}
		else
		{
			this.scrittaNonCiSonoRisorse.GetComponent<CanvasGroup>().alpha = 1f;
		}
	}

	// Token: 0x06000045 RID: 69 RVA: 0x000110A4 File Offset: 0x0000F2A4
	private void DemolisciStruttura()
	{
		UnityEngine.Object.Destroy(this.elencoPostiEdifici.transform.GetChild(this.numeroPosto).GetChild(0).gameObject);
		this.ListaPostiInHeadquarters[this.numeroPosto] = 100;
		base.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		base.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = base.GetComponent<GestioneSuoniCasa>().suonoDistruzEdificio;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00011114 File Offset: 0x0000F314
	private void DettagliInfoEdifici()
	{
		GameObject gameObject = null;
		if (this.elencoPostiEdifici.transform.GetChild(this.numeroPosto).childCount > 0)
		{
			gameObject = this.elencoPostiEdifici.transform.GetChild(this.numeroPosto).GetChild(0).gameObject;
		}
		if (this.cambiaAccesoSpento)
		{
			if (this.ListaAccesoOSpento[this.numeroPosto] == 0)
			{
				this.ListaAccesoOSpento[this.numeroPosto] = 1;
				gameObject.GetComponent<PresenzaEdificio>().èAcceso = 1;
			}
			else
			{
				this.ListaAccesoOSpento[this.numeroPosto] = 0;
				gameObject.GetComponent<PresenzaEdificio>().èAcceso = 0;
			}
			this.cambiaAccesoSpento = false;
			gameObject.GetComponent<PresenzaEdificio>().aggiornaFumo = true;
		}
		this.pannelloInfo.transform.GetChild(0).GetComponent<Text>().text = this.ListaEdificiPossibili[this.tipologiaEdificio].GetComponent<PresenzaEdificio>().nomeEdificio;
		this.pannelloInfo.transform.GetChild(1).GetComponent<Image>().sprite = this.ListaEdificiPossibili[this.tipologiaEdificio].GetComponent<PresenzaEdificio>().immagineEdificio;
		this.pannelloInfo.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = this.ListaEdificiPossibili[this.tipologiaEdificio].GetComponent<PresenzaEdificio>().costoCostruzioneInPlastica.ToString("F0");
		this.pannelloInfo.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = this.ListaEdificiPossibili[this.tipologiaEdificio].GetComponent<PresenzaEdificio>().costoCostruzioneInMetallo.ToString("F0");
		this.pannelloInfo.transform.GetChild(2).GetChild(2).GetComponent<Text>().text = this.ListaEdificiPossibili[this.tipologiaEdificio].GetComponent<PresenzaEdificio>().consumoEnergiaRaffinata.ToString("F0") + "  per turn";
		this.pannelloInfo.transform.GetChild(3).GetComponent<Text>().text = this.ListaEdificiPossibili[this.tipologiaEdificio].GetComponent<PresenzaEdificio>().oggettoDescrizione.GetComponent<Text>().text;
		if (gameObject != null)
		{
			if (this.ListaAccesoOSpento[this.numeroPosto] == 0)
			{
				this.pulsanteAccesoOSpento.GetComponent<Image>().color = Color.yellow;
				this.pulsanteAccesoOSpento.transform.GetChild(0).GetComponent<Text>().color = Color.black;
				this.pulsanteAccesoOSpento.transform.GetChild(0).GetComponent<Text>().text = "ON";
			}
			else
			{
				this.pulsanteAccesoOSpento.GetComponent<Image>().color = Color.black;
				this.pulsanteAccesoOSpento.transform.GetChild(0).GetComponent<Text>().color = Color.white;
				this.pulsanteAccesoOSpento.transform.GetChild(0).GetComponent<Text>().text = "OFF";
			}
		}
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00011434 File Offset: 0x0000F634
	private void CalcolaLavoroEdifici()
	{
		this.postLavEdificioPlasticaGrezza = 0f;
		this.postLavEdificioMetalloGrezzo = 0f;
		this.postLavEdificioEnergiaGrezza = 0f;
		this.postLavEdificioIncendiarioGrezzo = 0f;
		this.postLavEdificioTossicoGrezzo = 0f;
		this.postLavEdificioPlasticaRaffinata = 0f;
		this.postLavEdificioMetalloRaffinato = 0f;
		this.postLavEdificioEnergiaRaffinata = 0f;
		this.postLavEdificioIncendiarioRaffinato = 0f;
		this.postLavEdificioTossicoRaffinato = 0f;
		if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().saltoGiorniPerTurno == 1)
		{
			this.postLavEdificioEsperienza = 0f;
		}
		else if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().saltoGiorniPerTurno == 2)
		{
			this.postLavEdificioEsperienza = 30f;
		}
		else if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().saltoGiorniPerTurno == 3)
		{
			this.postLavEdificioEsperienza = 60f;
		}
		for (int i = 0; i < 16; i++)
		{
			if (this.ListaPostiInHeadquarters[i] != 100 && this.ListaAccesoOSpento[i] == 0)
			{
				if (this.ListaPostiInHeadquarters[i] == 0)
				{
					if (this.ListaRisorsePresenti[0].GetComponent<PresenzaRisorsa>().quantitàRisorsa + this.postLavEdificioPlasticaGrezza >= this.ListaEdificiPossibili[0].GetComponent<PresenzaEdificio>().consumoPlasticaGrezza && this.ListaRisorsePresenti[5].GetComponent<PresenzaRisorsa>().quantitàRisorsa + this.postLavEdificioEnergiaRaffinata >= this.ListaEdificiPossibili[0].GetComponent<PresenzaEdificio>().consumoEnergiaRaffinata)
					{
						this.postLavEdificioPlasticaGrezza -= this.ListaEdificiPossibili[0].GetComponent<PresenzaEdificio>().consumoPlasticaGrezza;
						this.postLavEdificioEnergiaRaffinata -= this.ListaEdificiPossibili[0].GetComponent<PresenzaEdificio>().consumoEnergiaRaffinata;
						this.postLavEdificioPlasticaRaffinata += this.ListaEdificiPossibili[0].GetComponent<PresenzaEdificio>().produzionePlasticaRaffinata;
					}
				}
				else if (this.ListaPostiInHeadquarters[i] == 1)
				{
					if (this.ListaRisorsePresenti[2].GetComponent<PresenzaRisorsa>().quantitàRisorsa + this.postLavEdificioMetalloGrezzo >= this.ListaEdificiPossibili[1].GetComponent<PresenzaEdificio>().consumoMetalloGrezzo && this.ListaRisorsePresenti[5].GetComponent<PresenzaRisorsa>().quantitàRisorsa + this.postLavEdificioEnergiaRaffinata >= this.ListaEdificiPossibili[1].GetComponent<PresenzaEdificio>().consumoEnergiaRaffinata)
					{
						this.postLavEdificioMetalloGrezzo -= this.ListaEdificiPossibili[1].GetComponent<PresenzaEdificio>().consumoMetalloGrezzo;
						this.postLavEdificioEnergiaRaffinata -= this.ListaEdificiPossibili[1].GetComponent<PresenzaEdificio>().consumoEnergiaRaffinata;
						this.postLavEdificioMetalloRaffinato += this.ListaEdificiPossibili[1].GetComponent<PresenzaEdificio>().produzioneMetalloRaffinato;
					}
				}
				else if (this.ListaPostiInHeadquarters[i] == 2)
				{
					if (this.ListaRisorsePresenti[4].GetComponent<PresenzaRisorsa>().quantitàRisorsa + this.postLavEdificioEnergiaGrezza >= this.ListaEdificiPossibili[2].GetComponent<PresenzaEdificio>().consumoEnergiaGrezza)
					{
						this.postLavEdificioEnergiaGrezza -= this.ListaEdificiPossibili[2].GetComponent<PresenzaEdificio>().consumoEnergiaGrezza;
						this.postLavEdificioEnergiaRaffinata += this.ListaEdificiPossibili[2].GetComponent<PresenzaEdificio>().produzioneEnergiaRaffinata;
					}
				}
				else if (this.ListaPostiInHeadquarters[i] == 3)
				{
					if (this.ListaRisorsePresenti[6].GetComponent<PresenzaRisorsa>().quantitàRisorsa + this.postLavEdificioIncendiarioGrezzo >= this.ListaEdificiPossibili[3].GetComponent<PresenzaEdificio>().consumoIncendiarioGrezzo && this.ListaRisorsePresenti[5].GetComponent<PresenzaRisorsa>().quantitàRisorsa + this.postLavEdificioEnergiaRaffinata >= this.ListaEdificiPossibili[3].GetComponent<PresenzaEdificio>().consumoEnergiaRaffinata)
					{
						this.postLavEdificioIncendiarioGrezzo -= this.ListaEdificiPossibili[3].GetComponent<PresenzaEdificio>().consumoIncendiarioGrezzo;
						this.postLavEdificioEnergiaRaffinata -= this.ListaEdificiPossibili[3].GetComponent<PresenzaEdificio>().consumoEnergiaRaffinata;
						this.postLavEdificioIncendiarioRaffinato += this.ListaEdificiPossibili[3].GetComponent<PresenzaEdificio>().produzioneIncendiarioRaffinato;
					}
				}
				else if (this.ListaPostiInHeadquarters[i] == 4)
				{
					if (this.ListaRisorsePresenti[8].GetComponent<PresenzaRisorsa>().quantitàRisorsa + this.postLavEdificioTossicoGrezzo >= this.ListaEdificiPossibili[4].GetComponent<PresenzaEdificio>().consumoTossicoGrezzo && this.ListaRisorsePresenti[5].GetComponent<PresenzaRisorsa>().quantitàRisorsa + this.postLavEdificioEnergiaRaffinata >= this.ListaEdificiPossibili[4].GetComponent<PresenzaEdificio>().consumoEnergiaRaffinata)
					{
						this.postLavEdificioTossicoGrezzo -= this.ListaEdificiPossibili[4].GetComponent<PresenzaEdificio>().consumoTossicoGrezzo;
						this.postLavEdificioEnergiaRaffinata -= this.ListaEdificiPossibili[4].GetComponent<PresenzaEdificio>().consumoEnergiaRaffinata;
						this.postLavEdificioTossicoRaffinato += this.ListaEdificiPossibili[4].GetComponent<PresenzaEdificio>().produzioneTossicoRaffinato;
					}
				}
				else if (this.ListaPostiInHeadquarters[i] == 5)
				{
					if (this.ListaRisorsePresenti[5].GetComponent<PresenzaRisorsa>().quantitàRisorsa >= this.ListaEdificiPossibili[5].GetComponent<PresenzaEdificio>().consumoEnergiaRaffinata)
					{
						this.postLavEdificioEnergiaRaffinata -= this.ListaEdificiPossibili[5].GetComponent<PresenzaEdificio>().consumoEnergiaRaffinata;
						this.postLavEdificioPlasticaGrezza += this.ListaEdificiPossibili[5].GetComponent<PresenzaEdificio>().prodPercentualeDaRifiuti / 100f * this.totFutPlasticaGrezzaDaStanze;
						this.postLavEdificioMetalloGrezzo += this.ListaEdificiPossibili[5].GetComponent<PresenzaEdificio>().prodPercentualeDaRifiuti / 100f * this.totFutMetalloGrezzoDaStanze;
						this.postLavEdificioEnergiaGrezza += this.ListaEdificiPossibili[5].GetComponent<PresenzaEdificio>().prodPercentualeDaRifiuti / 100f * this.totFutEnergiaGrezzaDaStanze;
						this.postLavEdificioIncendiarioGrezzo += this.ListaEdificiPossibili[5].GetComponent<PresenzaEdificio>().prodPercentualeDaRifiuti / 100f * this.totFutIncendiarioGrezzoDaStanze;
						this.postLavEdificioTossicoGrezzo += this.ListaEdificiPossibili[5].GetComponent<PresenzaEdificio>().prodPercentualeDaRifiuti / 100f * this.totFutTossicoGrezzoDaStanze;
					}
				}
				else if (this.ListaPostiInHeadquarters[i] != 6)
				{
					if (this.ListaPostiInHeadquarters[i] == 7 && this.ListaRisorsePresenti[3].GetComponent<PresenzaRisorsa>().quantitàRisorsa + this.postLavEdificioMetalloRaffinato >= this.ListaEdificiPossibili[7].GetComponent<PresenzaEdificio>().consumoMetalloRaffinato && this.ListaRisorsePresenti[5].GetComponent<PresenzaRisorsa>().quantitàRisorsa + this.postLavEdificioEnergiaRaffinata >= this.ListaEdificiPossibili[7].GetComponent<PresenzaEdificio>().consumoEnergiaRaffinata)
					{
						this.postLavEdificioMetalloRaffinato -= this.ListaEdificiPossibili[7].GetComponent<PresenzaEdificio>().consumoMetalloRaffinato;
						this.postLavEdificioEnergiaRaffinata -= this.ListaEdificiPossibili[7].GetComponent<PresenzaEdificio>().consumoEnergiaRaffinata;
						this.postLavEdificioEsperienza += this.ListaEdificiPossibili[7].GetComponent<PresenzaEdificio>().produzioneEsperienza;
					}
				}
			}
		}
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00011C04 File Offset: 0x0000FE04
	private void MostraDettagliRisorsa()
	{
		this.sfondoInfoRisorsa.transform.GetChild(0).GetComponent<Text>().text = this.ListaRisorsePresenti[this.tipoRisorsa].GetComponent<PresenzaRisorsa>().nomeRisorsa;
		this.sfondoInfoRisorsa.transform.GetChild(1).GetComponent<Text>().text = this.ListaRisorsePresenti[this.tipoRisorsa].GetComponent<Text>().text;
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00011C80 File Offset: 0x0000FE80
	private void GestioneSatellite()
	{
		if (this.aggiornaSatellite)
		{
			bool flag = false;
			int num = 0;
			while (num < this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.Count && !flag)
			{
				if (this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata == this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[num])
				{
					this.numSatelliteStanza = num;
					flag = true;
				}
				num++;
			}
			this.aggiornaSatellite = false;
		}
		if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaSatelliti[this.numSatelliteStanza] == 0)
		{
			ColorBlock colors = this.pulsanteSatellite.GetComponent<Button>().colors;
			colors.normalColor = Color.blue;
			this.pulsanteSatellite.GetComponent<Button>().colors = colors;
			this.pulsanteSatellite.transform.GetChild(0).GetComponent<Text>().text = "Launch Satellite";
			this.pulsanteSatellite.transform.GetChild(0).GetComponent<Text>().color = Color.white;
			if (this.domandaAperta)
			{
				this.domandaLanciareOAbbattereSat.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1f;
				this.domandaLanciareOAbbattereSat.transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 0f;
			}
			if (this.creaOAbbattiSatellite)
			{
				bool flag2 = false;
				for (int i = 0; i < this.ListaPostiInHeadquarters.Count; i++)
				{
					if (this.ListaPostiInHeadquarters[i] == 6)
					{
						flag2 = true;
						break;
					}
				}
				if (flag2 && this.ListaRisorsePresenti[1].GetComponent<PresenzaRisorsa>().quantitàRisorsa >= 1000f && this.ListaRisorsePresenti[3].GetComponent<PresenzaRisorsa>().quantitàRisorsa >= 70f && this.ListaRisorsePresenti[5].GetComponent<PresenzaRisorsa>().quantitàRisorsa >= 100f && this.ListaRisorsePresenti[7].GetComponent<PresenzaRisorsa>().quantitàRisorsa >= 50f)
				{
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaSatelliti[this.numSatelliteStanza] = 1;
					this.ListaRisorsePresenti[1].GetComponent<PresenzaRisorsa>().quantitàRisorsa -= 1000f;
					this.ListaRisorsePresenti[3].GetComponent<PresenzaRisorsa>().quantitàRisorsa -= 70f;
					this.ListaRisorsePresenti[5].GetComponent<PresenzaRisorsa>().quantitàRisorsa -= 100f;
					this.ListaRisorsePresenti[7].GetComponent<PresenzaRisorsa>().quantitàRisorsa -= 50f;
					GameObject gameObject = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[this.numSatelliteStanza];
					GameObject gameObject2 = UnityEngine.Object.Instantiate(this.satellitePrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
					gameObject2.transform.parent = gameObject.transform;
					gameObject2.transform.localPosition = new Vector3(0f, 6.5f, 0f);
					this.cameraGiocatore.GetComponent<AudioSource>().clip = base.GetComponent<GestioneSuoniCasa>().suonoPartenzaSatellite;
					base.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
				}
				else
				{
					this.scrittaNonCiSonoRisorse.GetComponent<CanvasGroup>().alpha = 1f;
				}
				this.creaOAbbattiSatellite = false;
			}
		}
		else
		{
			ColorBlock colors2 = this.pulsanteSatellite.GetComponent<Button>().colors;
			colors2.normalColor = Color.red;
			this.pulsanteSatellite.GetComponent<Button>().colors = colors2;
			this.pulsanteSatellite.transform.GetChild(0).GetComponent<Text>().text = "Shot Down Satellite";
			this.pulsanteSatellite.transform.GetChild(0).GetComponent<Text>().color = Color.black;
			if (this.domandaAperta)
			{
				this.domandaLanciareOAbbattereSat.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
				this.domandaLanciareOAbbattereSat.transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 1f;
			}
			if (this.creaOAbbattiSatellite)
			{
				this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaSatelliti[this.numSatelliteStanza] = 0;
				UnityEngine.Object.Destroy(this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[this.numSatelliteStanza].transform.FindChild("satellite(Clone)").gameObject);
				this.creaOAbbattiSatellite = false;
			}
		}
	}

	// Token: 0x04000157 RID: 343
	private GameObject inizioLivello;

	// Token: 0x04000158 RID: 344
	private GameObject cameraGiocatore;

	// Token: 0x04000159 RID: 345
	private GameObject Schede;

	// Token: 0x0400015A RID: 346
	private GameObject scheda1;

	// Token: 0x0400015B RID: 347
	private GameObject elencoRisorsePresenti;

	// Token: 0x0400015C RID: 348
	private GameObject elencoRisorseProsTurno;

	// Token: 0x0400015D RID: 349
	private GameObject pulsanteFitPerStrategia;

	// Token: 0x0400015E RID: 350
	private GameObject cameraCasa;

	// Token: 0x0400015F RID: 351
	private GameObject elencoPostiEdifici;

	// Token: 0x04000160 RID: 352
	private GameObject varieMappaStrategica;

	// Token: 0x04000161 RID: 353
	private GameObject pannelloInfo;

	// Token: 0x04000162 RID: 354
	private GameObject scrittaNonCiSonoRisorse;

	// Token: 0x04000163 RID: 355
	private GameObject pulsanteAccesoOSpento;

	// Token: 0x04000164 RID: 356
	private GameObject barraAltaAlleati;

	// Token: 0x04000165 RID: 357
	private GameObject sfondoInfoRisorsa;

	// Token: 0x04000166 RID: 358
	private GameObject centroStanzaUI;

	// Token: 0x04000167 RID: 359
	private GameObject pulsanteSatellite;

	// Token: 0x04000168 RID: 360
	private GameObject domandaLanciareOAbbattereSat;

	// Token: 0x04000169 RID: 361
	private float totFutPlasticaGrezzaDaStanze;

	// Token: 0x0400016A RID: 362
	private float totFutMetalloGrezzoDaStanze;

	// Token: 0x0400016B RID: 363
	private float totFutEnergiaGrezzaDaStanze;

	// Token: 0x0400016C RID: 364
	private float totFutIncendiarioGrezzoDaStanze;

	// Token: 0x0400016D RID: 365
	private float totFutTossicoGrezzoDaStanze;

	// Token: 0x0400016E RID: 366
	public List<int> ListaPostiInHeadquarters;

	// Token: 0x0400016F RID: 367
	public bool avviaCostruzione;

	// Token: 0x04000170 RID: 368
	public bool avviaDemolizione;

	// Token: 0x04000171 RID: 369
	public int numeroPosto;

	// Token: 0x04000172 RID: 370
	public int tipologiaEdificio;

	// Token: 0x04000173 RID: 371
	public List<GameObject> ListaEdificiPossibili;

	// Token: 0x04000174 RID: 372
	private float timerScrittaNonCiSonoRisorse;

	// Token: 0x04000175 RID: 373
	public bool aggiornaLavoroEdifici;

	// Token: 0x04000176 RID: 374
	private float postLavEdificioPlasticaGrezza;

	// Token: 0x04000177 RID: 375
	private float postLavEdificioMetalloGrezzo;

	// Token: 0x04000178 RID: 376
	private float postLavEdificioEnergiaGrezza;

	// Token: 0x04000179 RID: 377
	private float postLavEdificioIncendiarioGrezzo;

	// Token: 0x0400017A RID: 378
	private float postLavEdificioTossicoGrezzo;

	// Token: 0x0400017B RID: 379
	private float postLavEdificioPlasticaRaffinata;

	// Token: 0x0400017C RID: 380
	private float postLavEdificioMetalloRaffinato;

	// Token: 0x0400017D RID: 381
	private float postLavEdificioEnergiaRaffinata;

	// Token: 0x0400017E RID: 382
	private float postLavEdificioIncendiarioRaffinato;

	// Token: 0x0400017F RID: 383
	private float postLavEdificioTossicoRaffinato;

	// Token: 0x04000180 RID: 384
	private float postLavEdificioEsperienza;

	// Token: 0x04000181 RID: 385
	public bool cambiaAccesoSpento;

	// Token: 0x04000182 RID: 386
	public List<int> ListaAccesoOSpento;

	// Token: 0x04000183 RID: 387
	public int tipoRisorsa;

	// Token: 0x04000184 RID: 388
	public bool creaOAbbattiSatellite;

	// Token: 0x04000185 RID: 389
	private int numSatelliteStanza;

	// Token: 0x04000186 RID: 390
	public bool aggiornaSatellite;

	// Token: 0x04000187 RID: 391
	public bool domandaAperta;

	// Token: 0x04000188 RID: 392
	public GameObject satellitePrefab;

	// Token: 0x04000189 RID: 393
	public List<GameObject> ListaRisorsePresenti;

	// Token: 0x0400018A RID: 394
	public GameObject plasticaGrezzaPres;

	// Token: 0x0400018B RID: 395
	public GameObject plasticaRaffinataPres;

	// Token: 0x0400018C RID: 396
	public GameObject metalloGrezzoPres;

	// Token: 0x0400018D RID: 397
	public GameObject metalloRaffinatoPres;

	// Token: 0x0400018E RID: 398
	public GameObject energiaGrezzaPres;

	// Token: 0x0400018F RID: 399
	public GameObject energiaRaffinataPres;

	// Token: 0x04000190 RID: 400
	public GameObject incendiarioGrezzoPres;

	// Token: 0x04000191 RID: 401
	public GameObject incendiarioRaffinatoPres;

	// Token: 0x04000192 RID: 402
	public GameObject tossicoGrezzoPres;

	// Token: 0x04000193 RID: 403
	public GameObject tossicoRaffinatoPres;

	// Token: 0x04000194 RID: 404
	public GameObject esperienzaPres;

	// Token: 0x04000195 RID: 405
	public List<float> ListaRisorseFuture;

	// Token: 0x04000196 RID: 406
	public float plasticaGrezzaFut;

	// Token: 0x04000197 RID: 407
	public float plasticaRaffinataFut;

	// Token: 0x04000198 RID: 408
	public float metalloGrezzoFut;

	// Token: 0x04000199 RID: 409
	public float metalloRaffinatoFut;

	// Token: 0x0400019A RID: 410
	public float energiaGrezzaFut;

	// Token: 0x0400019B RID: 411
	public float energiaRaffinataFut;

	// Token: 0x0400019C RID: 412
	public float incendiarioGrezzoFut;

	// Token: 0x0400019D RID: 413
	public float incendiarioRaffinatoFut;

	// Token: 0x0400019E RID: 414
	public float tossicoGrezzoFut;

	// Token: 0x0400019F RID: 415
	public float tossicoRaffinatoFut;

	// Token: 0x040001A0 RID: 416
	public float esperienzaFut;

	// Token: 0x040001A1 RID: 417
	public Material colorePostoEdificioNormale;

	// Token: 0x040001A2 RID: 418
	public Material colorePostoEdificioSelez;
}
