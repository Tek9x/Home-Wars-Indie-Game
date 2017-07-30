using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020000C3 RID: 195
public class CaricaDati : MonoBehaviour
{
	// Token: 0x060006CE RID: 1742 RVA: 0x000F0CC4 File Offset: 0x000EEEC4
	private void Start()
	{
		this.inizioLivello = GameObject.FindGameObjectWithTag("InizioLivello");
		this.tipoBattaglia = GestoreNeutroStrategia.tipoBattaglia;
		if (!base.GetComponent<OltreScene>().scenaDiMenu)
		{
			if (base.GetComponent<OltreScene>().èInStrategia)
			{
				this.cameraCasa = GameObject.FindGameObjectWithTag("MainCamera");
				this.headquarters = GameObject.FindGameObjectWithTag("Headquarters");
				this.nest = GameObject.FindGameObjectWithTag("Nest");
				this.finestraCarica = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Carica In Strategia").gameObject;
				this.listaPulsantiSlot = this.finestraCarica.transform.GetChild(0).FindChild("Lista Pulsanti Slot").gameObject;
				this.elencoPostiEdifici = this.headquarters.transform.FindChild("lista posti").gameObject;
				this.varieMappaStrategica = GameObject.FindGameObjectWithTag("VarieMappaStrategica");
				Time.timeScale = 1f;
				if (CaricaDati.InizializConCaricamStrategia)
				{
					this.InizializzaConIlCaricamento();
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().aggiornaScrittaDataETurno = true;
					this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().aggiornaLavoroEdifici = true;
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().aggMissioneExtra = true;
					CaricaDati.InizializConCaricamStrategia = false;
				}
				else
				{
					this.senzaInizializ = true;
				}
				if (GestoreNeutroStrategia.vincitore != 0)
				{
					this.CaricamentoDaTatticaAStrategia();
				}
				this.ListaSlotCaricabili = new List<int>();
				for (int i = 0; i < 10; i++)
				{
					this.ListaSlotCaricabili.Add(PlayerPrefs.GetInt(i + " slot è caricabile"));
				}
				this.ListaSlotCaricabili[9] = 1;
			}
			else
			{
				this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
				this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
				this.varieMappaLocale = GameObject.FindGameObjectWithTag("VarieMappaLocale");
				this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
				if (GestoreNeutroTattica.èBattagliaVeloce)
				{
					this.CaricaDatiBattagliaVeloce();
				}
				else
				{
					this.CaricamentoDaStrategiaATattica();
				}
			}
		}
		else if (this.inizioLivello.GetComponent<OltreScene>().èMenuIniziale)
		{
			this.CanvasMenuIniziale = GameObject.FindGameObjectWithTag("CanvasMenuIniz");
			this.listaPulsantiSlot = this.CanvasMenuIniziale.transform.FindChild("sfondo caricamento").GetChild(0).GetChild(0).FindChild("Lista Pulsanti Slot").gameObject;
		}
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x000F0F34 File Offset: 0x000EF134
	private void Update()
	{
		this.tipoBattaglia = GestoreNeutroStrategia.tipoBattaglia;
		if (base.GetComponent<OltreScene>().èInStrategia)
		{
			if (this.finestraCarica.GetComponent<CanvasGroup>().alpha == 1f)
			{
				for (int i = 0; i < 9; i++)
				{
					if (PlayerPrefs.GetInt(i + " slot è caricabile") == 0)
					{
						this.listaPulsantiSlot.transform.GetChild(i).GetComponent<Button>().interactable = false;
						this.listaPulsantiSlot.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = string.Empty;
						this.listaPulsantiSlot.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = string.Empty;
						this.listaPulsantiSlot.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = string.Empty;
					}
					else
					{
						this.listaPulsantiSlot.transform.GetChild(i).GetComponent<Button>().interactable = true;
						this.listaPulsantiSlot.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString(i + " nome salvataggio");
						this.listaPulsantiSlot.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "turn: " + PlayerPrefs.GetInt(i + " numero turno").ToString();
						this.listaPulsantiSlot.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = PlayerPrefs.GetString(i + " data salvataggio");
					}
					if (i == CaricaDati.slotSuCuiCaricare)
					{
						this.listaPulsantiSlot.transform.GetChild(i).GetComponent<Image>().color = base.GetComponent<SalvaDati>().coloreSelez;
					}
					else
					{
						this.listaPulsantiSlot.transform.GetChild(i).GetComponent<Image>().color = base.GetComponent<SalvaDati>().coloreNonSelez;
					}
				}
			}
			if (CaricaDati.caricamentoAttivo)
			{
				if (PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " slot è caricabile") == 1)
				{
					this.CaricamentoScript();
				}
				CaricaDati.caricamentoAttivo = false;
				CaricaDati.InizializConCaricamStrategia = true;
				GestoreNeutroStrategia.aggElencoBattaglia = false;
				GestoreNeutroStrategia.aggElencoMissione = false;
				GestoreNeutroStrategia.mostraResocontoBattaglia = false;
				GestoreNeutroStrategia.mostraResocontoMissione = false;
				GestoreNeutroStrategia.mostraElencoResoconto = false;
			}
			if (this.cancellaSalvataggioSelez)
			{
				this.CancellaSalvataggioScript();
				this.cancellaSalvataggioSelez = false;
			}
		}
		else if (base.GetComponent<OltreScene>().scenaDiMenu && this.CanvasMenuIniziale != null)
		{
			for (int j = 0; j < 9; j++)
			{
				if (PlayerPrefs.GetInt(j + " slot è caricabile") == 0)
				{
					this.listaPulsantiSlot.transform.GetChild(j).GetComponent<Button>().interactable = false;
					this.listaPulsantiSlot.transform.GetChild(j).GetChild(0).GetComponent<Text>().text = string.Empty;
					this.listaPulsantiSlot.transform.GetChild(j).GetChild(1).GetComponent<Text>().text = string.Empty;
					this.listaPulsantiSlot.transform.GetChild(j).GetChild(2).GetComponent<Text>().text = string.Empty;
				}
				else
				{
					this.listaPulsantiSlot.transform.GetChild(j).GetComponent<Button>().interactable = true;
					this.listaPulsantiSlot.transform.GetChild(j).GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString(j + " nome salvataggio");
					this.listaPulsantiSlot.transform.GetChild(j).GetChild(1).GetComponent<Text>().text = "turn: " + PlayerPrefs.GetInt(j + " numero turno").ToString();
					this.listaPulsantiSlot.transform.GetChild(j).GetChild(2).GetComponent<Text>().text = PlayerPrefs.GetString(j + " data salvataggio");
				}
				if (j == CaricaDati.slotSuCuiCaricare)
				{
					this.listaPulsantiSlot.transform.GetChild(j).GetComponent<Image>().color = base.GetComponent<SalvaDati>().coloreSelez;
				}
				else
				{
					this.listaPulsantiSlot.transform.GetChild(j).GetComponent<Image>().color = base.GetComponent<SalvaDati>().coloreNonSelez;
				}
			}
			if (CaricaDati.caricamentoAttivo)
			{
				if (PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " slot è caricabile") == 1)
				{
					this.CaricamentoScript();
				}
				CaricaDati.caricamentoAttivo = false;
				CaricaDati.InizializConCaricamStrategia = true;
				GestoreNeutroStrategia.aggElencoBattaglia = false;
				GestoreNeutroStrategia.aggElencoMissione = false;
				GestoreNeutroStrategia.mostraResocontoBattaglia = false;
				GestoreNeutroStrategia.mostraResocontoMissione = false;
				GestoreNeutroStrategia.mostraElencoResoconto = false;
			}
			if (this.cancellaSalvataggioSelez)
			{
				this.CancellaSalvataggioScript();
				this.cancellaSalvataggioSelez = false;
			}
		}
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x000F1458 File Offset: 0x000EF658
	private void CaricamentoScript()
	{
		CaricaScene.nomeScenaDaCaricare = PlayerPrefs.GetString(CaricaDati.slotSuCuiCaricare + " nome livello");
		SceneManager.LoadScene("Scena Di Caricamento");
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x000F1490 File Offset: 0x000EF690
	private void InizializzaConIlCaricamento()
	{
		for (int i = 0; i < this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.Count; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[i].GetComponent<CentroStanza>().ListaSettori[j] = PlayerPrefs.GetInt(string.Concat(new object[]
				{
					CaricaDati.slotSuCuiCaricare,
					" stanza ",
					i,
					" settore ",
					j
				}));
			}
			this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[i].GetComponent<CentroStanza>().quiCèStataBattaglia = PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" stanza ",
				i,
				"c'è stata battaglia"
			}));
		}
		this.cameraCasa.GetComponent<GestoreNeutroStrategia>().numeroTurno = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " numero turno");
		this.cameraCasa.GetComponent<GestoreNeutroStrategia>().giornoData = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " giorno");
		this.cameraCasa.GetComponent<GestoreNeutroStrategia>().meseData = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " mese");
		this.cameraCasa.GetComponent<GestoreNeutroStrategia>().annoData = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " anno");
		this.cameraCasa.GetComponent<GestoreNeutroStrategia>().saltoGiorniPerTurno = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " salto dei giorni");
		GestoreNeutroStrategia.stagione = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " stagione di questa campagna");
		GestoreNeutroStrategia.valVitaStagionaleNemici = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " valore vita stagione nemici");
		GestoreNeutroStrategia.valAttaccoStagionaleNemici = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " valore attacco stagione nemici");
		this.cameraCasa.GetComponent<GestoreNeutroStrategia>().numAlleatiMortiinTotale = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " numero alleati morti in totale");
		this.cameraCasa.GetComponent<GestoreNeutroStrategia>().numNemiciMortiinTotale = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " numero nemici morti in totale");
		this.nest.GetComponent<IANemicoStrategia>().livelloNest = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " livelloNest");
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti = new List<GameObject>();
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti.Add(this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().plasticaGrezzaPres);
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti.Add(this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().plasticaRaffinataPres);
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti.Add(this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().metalloGrezzoPres);
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti.Add(this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().metalloRaffinatoPres);
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti.Add(this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().energiaGrezzaPres);
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti.Add(this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().energiaRaffinataPres);
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti.Add(this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().incendiarioGrezzoPres);
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti.Add(this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().incendiarioRaffinatoPres);
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti.Add(this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().tossicoGrezzoPres);
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti.Add(this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().tossicoRaffinatoPres);
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti.Add(this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().esperienzaPres);
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[0].GetComponent<PresenzaRisorsa>().quantitàRisorsa = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " plastica grezza");
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[1].GetComponent<PresenzaRisorsa>().quantitàRisorsa = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " plastica raffinata");
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[2].GetComponent<PresenzaRisorsa>().quantitàRisorsa = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " metallo grezzo");
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[3].GetComponent<PresenzaRisorsa>().quantitàRisorsa = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " metallo raffinato");
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[4].GetComponent<PresenzaRisorsa>().quantitàRisorsa = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " energia grezza");
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[5].GetComponent<PresenzaRisorsa>().quantitàRisorsa = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " energia raffinata");
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[6].GetComponent<PresenzaRisorsa>().quantitàRisorsa = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " incendiario grezzo");
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[7].GetComponent<PresenzaRisorsa>().quantitàRisorsa = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " incendiario raffinato");
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[8].GetComponent<PresenzaRisorsa>().quantitàRisorsa = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " tossico grezzo");
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[9].GetComponent<PresenzaRisorsa>().quantitàRisorsa = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " tossico raffinato");
		this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[10].GetComponent<PresenzaRisorsa>().quantitàRisorsa = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " esperienza");
		this.nest.GetComponent<IANemicoStrategia>().freshFoodPresente = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " fresh food presente");
		this.nest.GetComponent<IANemicoStrategia>().freshFoodEsterno = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " fresh food esterno");
		this.nest.GetComponent<IANemicoStrategia>().freshFoodBattOMiss = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " fresh food batt o miss");
		this.nest.GetComponent<IANemicoStrategia>().rottenFoodPresenteFormicheSwarm = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " rotten food presente formiche swarm");
		this.nest.GetComponent<IANemicoStrategia>().rottenFoodPresenteCavalletteSwarm = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " rotten food presente cavallette swarm");
		this.nest.GetComponent<IANemicoStrategia>().rottenFoodPresenteVespaSwarm = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " rotten food presente vespe swarm");
		this.nest.GetComponent<IANemicoStrategia>().rottenFoodEsterno = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " rotten food esterno");
		this.nest.GetComponent<IANemicoStrategia>().freshFoodBattOMiss = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " rotten food batt o miss");
		this.nest.GetComponent<IANemicoStrategia>().highProteinFoodPresente = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " high protein food presente");
		this.nest.GetComponent<IANemicoStrategia>().highProteinFoodEsterno = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " high protein food esterno");
		this.nest.GetComponent<IANemicoStrategia>().highProteinFoodBattOMiss = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " high protein food batt o miss");
		this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi = new List<GameObject>();
		for (int k = 0; k < PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " numero eserciti alleati attivi"); k++)
		{
			Vector3 position = new Vector3(PlayerPrefs.GetFloat(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito alleato numero ",
				k,
				" posizioneX"
			})), PlayerPrefs.GetFloat(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito alleato numero ",
				k,
				" posizioneY"
			})), PlayerPrefs.GetFloat(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito alleato numero ",
				k,
				" posizioneZ"
			})));
			GameObject item = UnityEngine.Object.Instantiate(this.headquarters.GetComponent<GestioneEsercitiAlleati>().esercitoPrefab, position, Quaternion.identity) as GameObject;
			this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi.Add(item);
		}
		List<int> list = new List<int>();
		if (GestoreNeutroStrategia.vincitore != 0)
		{
			for (int l = 0; l < this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaUnitàAlleate.Count; l++)
			{
				list.Add(PlayerPrefs.GetInt(string.Concat(new object[]
				{
					CaricaDati.slotSuCuiCaricare,
					" lista alleati rimanenti posizione ",
					l,
					" quantità"
				})));
			}
		}
		for (int m = 0; m < this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi.Count; m++)
		{
			this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[m].GetComponent<PresenzaAlleataStrategica>().nomeEsercito = PlayerPrefs.GetString(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito alleato ",
				m,
				" nome"
			}));
			this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[m].name = PlayerPrefs.GetString(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito alleato ",
				m,
				" nome"
			}));
			this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[m].GetComponent<PresenzaAlleataStrategica>().numIdentitàAlleato = PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito alleato ",
				m,
				" identità"
			}));
			this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[m].GetComponent<PresenzaAlleataStrategica>().primoTurnoPerCanc = PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito alleato ",
				m,
				" primoTurnoPerCanc"
			}));
			if (PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito alleato ",
				m,
				" può ancora muoversi"
			})) == 0)
			{
				this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[m].GetComponent<PresenzaAlleataStrategica>().puòAncoraMuoversi = false;
			}
			else
			{
				this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[m].GetComponent<PresenzaAlleataStrategica>().puòAncoraMuoversi = true;
			}
			if (PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito alleato ",
				m,
				" partecipa a battaglia"
			})) == 0 || this.tipoBattaglia == 3 || this.tipoBattaglia == 5)
			{
				for (int n = 0; n < 30; n++)
				{
					this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[m].GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[n] = PlayerPrefs.GetInt(string.Concat(new object[]
					{
						CaricaDati.slotSuCuiCaricare,
						" esercito alleato ",
						m,
						" elemento ",
						n
					}));
				}
			}
			else
			{
				for (int num = 0; num < 30; num++)
				{
					bool flag = false;
					int num2 = 0;
					while (num2 < list.Count && !flag)
					{
						if (PlayerPrefs.GetInt(string.Concat(new object[]
						{
							CaricaDati.slotSuCuiCaricare,
							" esercito alleato ",
							m,
							" elemento ",
							num
						})) == num2 && list[num2] > 0)
						{
							this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[m].GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[num] = PlayerPrefs.GetInt(string.Concat(new object[]
							{
								CaricaDati.slotSuCuiCaricare,
								" esercito alleato ",
								m,
								" elemento ",
								num
							}));
							List<int> list2;
							List<int> expr_DA9 = list2 = list;
							int num3;
							int expr_DAE = num3 = num2;
							num3 = list2[num3];
							expr_DA9[expr_DAE] = num3 - 1;
							flag = true;
						}
						num2++;
					}
					if (!flag)
					{
						this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[m].GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[num] = 100;
					}
				}
			}
		}
		this.headquarters.GetComponent<GestioneEsercitiAlleati>().sequenzaNumNomeEser = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " sequenza numero nome eserciti alleati");
		this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici = new List<GameObject>();
		for (int num4 = 0; num4 < PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " numero eserciti nemici"); num4++)
		{
			Vector3 position2 = new Vector3(PlayerPrefs.GetFloat(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito nemico numero ",
				num4,
				" posizioneX"
			})), PlayerPrefs.GetFloat(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito nemico numero ",
				num4,
				" posizioneY"
			})), PlayerPrefs.GetFloat(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito nemico numero ",
				num4,
				" posizioneZ"
			})));
			GameObject gameObject = null;
			if (PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito nemico ",
				num4,
				" tipo"
			})) == 0)
			{
				gameObject = (UnityEngine.Object.Instantiate(this.nest.GetComponent<IANemicoStrategia>().esercitoTipo0, position2, Quaternion.identity) as GameObject);
			}
			else if (PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito nemico ",
				num4,
				" tipo"
			})) == 1)
			{
				gameObject = (UnityEngine.Object.Instantiate(this.nest.GetComponent<IANemicoStrategia>().esercitoTipo1, position2, Quaternion.identity) as GameObject);
			}
			else if (PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito nemico ",
				num4,
				" tipo"
			})) == 2)
			{
				gameObject = (UnityEngine.Object.Instantiate(this.nest.GetComponent<IANemicoStrategia>().esercitoTipo2, position2, Quaternion.identity) as GameObject);
			}
			else if (PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito nemico ",
				num4,
				" tipo"
			})) == 3)
			{
				gameObject = (UnityEngine.Object.Instantiate(this.nest.GetComponent<IANemicoStrategia>().esercitoTipo3, position2, Quaternion.identity) as GameObject);
			}
			this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici.Add(gameObject);
			gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser = new List<List<int>>();
			for (int num5 = 0; num5 < this.nest.GetComponent<IANemicoStrategia>().ListaTipiInsetti.Count; num5++)
			{
				List<int> list3 = new List<int>();
				list3.Add(100);
				list3.Add(0);
				gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser.Add(list3);
			}
		}
		List<int> list4 = new List<int>();
		if (GestoreNeutroStrategia.vincitore != 0)
		{
			for (int num6 = 0; num6 < this.nest.GetComponent<IANemicoStrategia>().ListaTipiInsetti.Count; num6++)
			{
				list4.Add(PlayerPrefs.GetInt(string.Concat(new object[]
				{
					CaricaDati.slotSuCuiCaricare,
					" lista nemici rimanenti posizione ",
					num6,
					" quantità"
				})));
			}
		}
		List<GameObject> list5 = new List<GameObject>();
		for (int num7 = 0; num7 < this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici.Count; num7++)
		{
			if (PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito nemico numero ",
				num7,
				" numero stanza"
			})) < 0 || PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito nemico numero ",
				num7,
				" numero stanza"
			})) > this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.Count - 1)
			{
				this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().posizioneAttuale = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[0];
				this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().vecchiaPosizioneAttuale = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[0];
				this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().destinazione = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[0];
			}
			else
			{
				this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().posizioneAttuale = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[PlayerPrefs.GetInt(string.Concat(new object[]
				{
					CaricaDati.slotSuCuiCaricare,
					" esercito nemico numero ",
					num7,
					" numero stanza"
				}))];
				this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().vecchiaPosizioneAttuale = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[PlayerPrefs.GetInt(string.Concat(new object[]
				{
					CaricaDati.slotSuCuiCaricare,
					" esercito nemico numero ",
					num7,
					" numero stanza"
				}))];
				this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().destinazione = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[PlayerPrefs.GetInt(string.Concat(new object[]
				{
					CaricaDati.slotSuCuiCaricare,
					" esercito nemico numero ",
					num7,
					" numero stanza"
				}))];
			}
			this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().nomeEserInsetti = PlayerPrefs.GetString(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito nemico ",
				num7,
				" nome"
			}));
			this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].name = PlayerPrefs.GetString(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito nemico ",
				num7,
				" nome"
			}));
			this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().numIdentitàNemico = PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito nemico ",
				num7,
				" identità"
			}));
			this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().tipoComportamentoGruppo = PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito nemico ",
				num7,
				" comportamento"
			}));
			this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().tipoDiSwarm = PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito nemico ",
				num7,
				" tipo"
			}));
			this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().swarmSpecialeHaAttaccato = PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito nemico speciale ",
				num7,
				" ha attaccato"
			}));
			this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().tipoDiInsettoOrda = PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito nemico ",
				num7,
				" tipo di orda"
			}));
			for (int num8 = 0; num8 < this.nest.GetComponent<IANemicoStrategia>().ListaTipiInsetti.Count; num8++)
			{
				this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num8][0] = PlayerPrefs.GetInt(string.Concat(new object[]
				{
					CaricaDati.slotSuCuiCaricare,
					" esercito nemico ",
					num7,
					" elemento ",
					num8,
					" tipo"
				}));
				this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num8][1] = PlayerPrefs.GetInt(string.Concat(new object[]
				{
					CaricaDati.slotSuCuiCaricare,
					" esercito nemico ",
					num7,
					" elemento ",
					num8,
					" quantità"
				}));
			}
			if (PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" esercito nemico ",
				num7,
				" partecipa a battaglia"
			})) == 0)
			{
				for (int num9 = 0; num9 < this.nest.GetComponent<IANemicoStrategia>().ListaTipiInsetti.Count; num9++)
				{
					this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num9][0] = PlayerPrefs.GetInt(string.Concat(new object[]
					{
						CaricaDati.slotSuCuiCaricare,
						" esercito nemico ",
						num7,
						" elemento ",
						num9,
						" tipo"
					}));
					this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num9][1] = PlayerPrefs.GetInt(string.Concat(new object[]
					{
						CaricaDati.slotSuCuiCaricare,
						" esercito nemico ",
						num7,
						" elemento ",
						num9,
						" quantità"
					}));
				}
			}
			else if (this.tipoBattaglia == 0 || this.tipoBattaglia == 1 || this.tipoBattaglia == 2)
			{
				for (int num10 = 0; num10 < this.nest.GetComponent<IANemicoStrategia>().ListaTipiInsetti.Count; num10++)
				{
					this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num10][0] = 100;
					this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num10][1] = 0;
				}
				list5.Add(this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7]);
			}
			else
			{
				for (int num11 = 0; num11 < this.nest.GetComponent<IANemicoStrategia>().ListaTipiInsetti.Count; num11++)
				{
					this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num11][0] = PlayerPrefs.GetInt(string.Concat(new object[]
					{
						CaricaDati.slotSuCuiCaricare,
						" esercito nemico ",
						num7,
						" elemento ",
						num11,
						" tipo"
					}));
					this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num7].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num11][1] = PlayerPrefs.GetInt(string.Concat(new object[]
					{
						CaricaDati.slotSuCuiCaricare,
						" esercito nemico ",
						num7,
						" elemento ",
						num11,
						" quantità"
					}));
				}
			}
		}
		this.nest.GetComponent<IANemicoStrategia>().numerazioneEser = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " sequenza numero nome eserciti nemici");
		List<GameObject> list6 = new List<GameObject>();
		for (int num12 = 0; num12 < list5.Count; num12++)
		{
			if (list5[num12].GetComponent<PresenzaNemicaStrategica>().tipoDiSwarm != 0)
			{
				int tipoDiInsettoOrda = list5[num12].GetComponent<PresenzaNemicaStrategica>().tipoDiInsettoOrda;
				list5[num12].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][0] = tipoDiInsettoOrda;
				list5[num12].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = PlayerPrefs.GetInt(string.Concat(new object[]
				{
					CaricaDati.slotSuCuiCaricare,
					" lista nemici rimanenti posizione ",
					tipoDiInsettoOrda,
					" quantità"
				}));
			}
			else
			{
				list6.Add(list5[num12]);
			}
		}
		int num13 = 0;
		int num14 = 80;
		int count = list6.Count;
		int num15 = 0;
		for (int num16 = 0; num16 < this.nest.GetComponent<IANemicoStrategia>().ListaTipiInsetti.Count; num16++)
		{
			num13 += PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" lista nemici rimanenti posizione ",
				num16,
				" quantità"
			}));
		}
		if (num13 > 0)
		{
			num15 = Mathf.CeilToInt(1f * (float)num13 / (float)num14);
			if (count < num15)
			{
				num15 = count;
			}
		}
		if (num15 > 0)
		{
			for (int num17 = 0; num17 < this.nest.GetComponent<IANemicoStrategia>().ListaTipiInsetti.Count; num17++)
			{
				int num18 = PlayerPrefs.GetInt(string.Concat(new object[]
				{
					CaricaDati.slotSuCuiCaricare,
					" lista nemici rimanenti posizione ",
					num17,
					" quantità"
				}));
				if (num18 > 0)
				{
					int value = Mathf.FloorToInt((float)(num18 / num15));
					for (int num19 = 0; num19 < num15; num19++)
					{
						if (num19 == num15 - 1)
						{
							bool flag2 = false;
							int num20 = 0;
							while (num20 < list6[num19].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser.Count && !flag2)
							{
								if (list6[num19].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num20][0] == num17)
								{
									list6[num19].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num20][1] = num18;
									flag2 = true;
								}
								else if (list6[num19].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num20][0] == 100)
								{
									list6[num19].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num20][0] = num17;
									list6[num19].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num20][1] = num18;
									flag2 = true;
								}
								num20++;
							}
						}
						else
						{
							bool flag3 = false;
							int num21 = 0;
							while (num21 < list6[num19].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser.Count && !flag3)
							{
								if (list6[num19].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num21][0] == num17)
								{
									list6[num19].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num21][1] = value;
									num18 -= list6[num19].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num21][1];
									flag3 = true;
								}
								else if (list6[num19].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num21][0] == 100)
								{
									list6[num19].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num21][0] = num17;
									list6[num19].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num21][1] = value;
									num18 -= list6[num19].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num21][1];
									flag3 = true;
								}
								num21++;
							}
						}
					}
				}
			}
		}
		for (int num22 = 0; num22 < this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici.Count; num22++)
		{
			if (this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num22] != null)
			{
				bool flag4 = true;
				int num23 = 0;
				while (num23 < this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num22].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num23].Count && flag4)
				{
					if (this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num22].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num23][1] > 0)
					{
						flag4 = false;
					}
					num23++;
				}
				if (flag4)
				{
					UnityEngine.Object.Destroy(this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num22]);
					this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num22] = null;
				}
			}
		}
		List<GameObject> list7 = new List<GameObject>();
		for (int num24 = 0; num24 < this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici.Count; num24++)
		{
			list7.Add(this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num24]);
		}
		this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici = new List<GameObject>();
		for (int num25 = 0; num25 < list7.Count; num25++)
		{
			if (list7[num25] != null)
			{
				this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici.Add(list7[num25]);
			}
		}
		for (int num26 = 0; num26 < 16; num26++)
		{
			int @int = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " edificio in posto " + num26);
			this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaPostiInHeadquarters[num26] = @int;
			if (@int != 100)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaEdificiPossibili[this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaPostiInHeadquarters[num26]], Vector3.zero, this.headquarters.transform.rotation) as GameObject;
				gameObject2.transform.parent = this.elencoPostiEdifici.transform.GetChild(num26);
				gameObject2.transform.localPosition = new Vector3(0f, 0f, 1f);
				gameObject2.GetComponent<PresenzaEdificio>().aggiornaFumo = true;
			}
			this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaAccesoOSpento[num26] = PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" edificio in posto ",
				num26,
				" è acceso"
			}));
			if (@int != 100 && this.elencoPostiEdifici.transform.GetChild(num26).childCount > 0)
			{
				this.elencoPostiEdifici.transform.GetChild(num26).GetChild(0).GetComponent<PresenzaEdificio>().èAcceso = this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaAccesoOSpento[num26];
			}
		}
		for (int num27 = 0; num27 < this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.Count; num27++)
		{
			this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaSatelliti[num27] = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " satellite in camera " + num27);
			if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaSatelliti[num27] == 1)
			{
				GameObject gameObject3 = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[num27];
				GameObject gameObject4 = UnityEngine.Object.Instantiate(this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().satellitePrefab, gameObject3.transform.position, Quaternion.identity) as GameObject;
				gameObject4.transform.parent = gameObject3.transform;
				gameObject4.transform.localPosition = new Vector3(0f, 6.5f, 0f);
			}
		}
		for (int num28 = 0; num28 < this.headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia.Count; num28++)
		{
			this.headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[num28].GetComponent<QuantitàMunizione>().quantità = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " quantità munizione " + num28);
		}
		for (int num29 = 0; num29 < this.headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaQuantitàSupporto.Count; num29++)
		{
			this.headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaQuantitàSupporto[num29] = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " quantità supporto " + num29);
		}
		for (int num30 = 0; num30 < this.headquarters.GetComponent<GestioneSblocchi>().ListaSblocchi.Count; num30++)
		{
			this.headquarters.GetComponent<GestioneSblocchi>().ListaSblocchi[num30].GetComponent<PresenzaSblocco>().èSbloccato = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " sblocco " + num30);
		}
		this.cameraCasa.GetComponent<GestoreNeutroStrategia>().missioneDaDecidere = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " missione decisa");
		this.cameraCasa.GetComponent<GestoreNeutroStrategia>().missionePresente = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " c'è missione");
		this.cameraCasa.GetComponent<GestoreNeutroStrategia>().tipoMissione = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " tipo missione");
		this.cameraCasa.GetComponent<GestoreNeutroStrategia>().stanzaDiMissione = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " stanza missione");
	}

	// Token: 0x060006D2 RID: 1746 RVA: 0x000F3AA8 File Offset: 0x000F1CA8
	private void CancellaSalvataggioScript()
	{
		PlayerPrefs.SetInt(CaricaDati.slotSuCuiCaricare + " slot è caricabile", 0);
		PlayerPrefs.SetString(CaricaDati.slotSuCuiCaricare + " nome salvataggio", string.Empty);
	}

	// Token: 0x060006D3 RID: 1747 RVA: 0x000F3AF0 File Offset: 0x000F1CF0
	private void CaricamentoDaStrategiaATattica()
	{
		this.IANemico.GetComponent<InfoGenericheNemici>().livelloNest = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " livelloNest");
		this.IANemico.GetComponent<InfoGenericheNemici>().bonusSurvivalSalute = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " valore vita survival nemici");
		this.IANemico.GetComponent<InfoGenericheNemici>().bonusSurvivalAttacco = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " valore attacco survival nemici");
		this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().giorniFittizi = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " giorni fittizi");
		if (GestoreNeutroStrategia.tipoBattaglia < 3)
		{
			this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili = new List<List<int>>();
			for (int i = 0; i < 48; i++)
			{
				List<int> list = new List<int>();
				list.Add(100);
				list.Add(0);
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili.Add(list);
			}
			for (int j = 0; j < 48; j++)
			{
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[j][0] = PlayerPrefs.GetInt(string.Concat(new object[]
				{
					CaricaDati.slotSuCuiCaricare,
					" lista alleati per tattica posizione ",
					j,
					" tipo"
				}));
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[j][1] = PlayerPrefs.GetInt(string.Concat(new object[]
				{
					CaricaDati.slotSuCuiCaricare,
					" lista alleati per tattica posizione ",
					j,
					" quantità"
				}));
			}
			this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciCaricati = new List<List<int>>();
			for (int k = 0; k < 48; k++)
			{
				List<int> list2 = new List<int>();
				list2.Add(100);
				list2.Add(0);
				this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciCaricati.Add(list2);
			}
			for (int l = 0; l < 48; l++)
			{
				this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciCaricati[l][0] = PlayerPrefs.GetInt(string.Concat(new object[]
				{
					CaricaDati.slotSuCuiCaricare,
					" lista nemici per tattica posizione ",
					l,
					" tipo"
				}));
				this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciCaricati[l][1] = PlayerPrefs.GetInt(string.Concat(new object[]
				{
					CaricaDati.slotSuCuiCaricare,
					" lista nemici per tattica posizione ",
					l,
					" quantità"
				}));
			}
			this.IANemico.GetComponent<IANemicoTattica>().tipoDiOrda = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " tipo di swarm");
			int @int = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " indice stanza battaglia");
			for (int m = 0; m < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento.Count; m++)
			{
				if (m == this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento.Count - 1)
				{
					if (PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " appartenenza stanza battaglia") == 1)
					{
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[m].transform.GetChild(0).tag = "AreaSchieramentoAlleato";
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[m].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierAlleato;
					}
					else if (PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " appartenenza stanza battaglia") == 2)
					{
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[m].transform.GetChild(0).tag = "AreaSchieramentoNemico";
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[m].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierNemico;
					}
					else
					{
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[m].transform.GetChild(0).tag = "AreaSchieramentoNeutro";
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[m].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierNeutro;
					}
				}
				else
				{
					bool flag = false;
					int num = 0;
					while (num < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento.Count && !flag)
					{
						if (int.Parse(this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[m].transform.GetChild(0).name) == PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " numero identità stanza " + num))
						{
							if (PlayerPrefs.GetInt(string.Concat(new object[]
							{
								CaricaDati.slotSuCuiCaricare,
								" stanza vicina ",
								num,
								" appartenenza"
							})) == 1)
							{
								this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[m].transform.GetChild(0).tag = "AreaSchieramentoAlleato";
								this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[m].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierAlleato;
							}
							else if (PlayerPrefs.GetInt(string.Concat(new object[]
							{
								CaricaDati.slotSuCuiCaricare,
								" stanza vicina ",
								num,
								" appartenenza"
							})) == 2)
							{
								this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[m].transform.GetChild(0).tag = "AreaSchieramentoNemico";
								this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[m].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierNemico;
							}
							else
							{
								this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[m].transform.GetChild(0).tag = "AreaSchieramentoNeutro";
								this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[m].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierNeutro;
							}
						}
						num++;
					}
				}
			}
		}
		else
		{
			this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili = new List<List<int>>();
			for (int n = 0; n < 48; n++)
			{
				List<int> list3 = new List<int>();
				list3.Add(100);
				list3.Add(0);
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili.Add(list3);
			}
			for (int num2 = 0; num2 < 48; num2++)
			{
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num2][0] = PlayerPrefs.GetInt(string.Concat(new object[]
				{
					CaricaDati.slotSuCuiCaricare,
					" lista alleati per tattica posizione ",
					num2,
					" tipo"
				}));
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[num2][1] = PlayerPrefs.GetInt(string.Concat(new object[]
				{
					CaricaDati.slotSuCuiCaricare,
					" lista alleati per tattica posizione ",
					num2,
					" quantità"
				}));
			}
			if (this.tipoBattaglia == 3 || this.tipoBattaglia == 4 || this.tipoBattaglia == 5)
			{
				for (int num3 = 0; num3 < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento.Count; num3++)
				{
					if (num3 == 0)
					{
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num3].transform.GetChild(0).tag = "AreaSchieramentoAlleato";
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num3].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierAlleato;
					}
					else
					{
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num3].transform.GetChild(0).tag = "AreaSchieramentoNemico";
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num3].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierNemico;
					}
				}
			}
			else if (this.tipoBattaglia == 6)
			{
				for (int num4 = 0; num4 < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento.Count; num4++)
				{
					if (num4 == 0 || num4 == 1)
					{
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num4].transform.GetChild(0).tag = "AreaSchieramentoAlleato";
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num4].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierAlleato;
					}
					else
					{
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num4].transform.GetChild(0).tag = "AreaSchieramentoNemico";
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num4].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierNemico;
					}
				}
			}
			else if (this.tipoBattaglia == 7)
			{
				for (int num5 = 0; num5 < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento.Count; num5++)
				{
					if (num5 == 0 || num5 == 1)
					{
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num5].transform.GetChild(0).tag = "AreaSchieramentoNemico";
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num5].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierNemico;
					}
					else
					{
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num5].transform.GetChild(0).tag = "AreaSchieramentoAlleato";
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num5].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierAlleato;
					}
				}
			}
		}
		int num6 = 0;
		int num7 = 0;
		for (int num8 = 0; num8 < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento.Count; num8++)
		{
			if (this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num8].transform.GetChild(0).tag == "AreaSchieramentoAlleato")
			{
				num6++;
			}
			if (this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num8].transform.GetChild(0).tag == "AreaSchieramentoNemico")
			{
				num7++;
			}
		}
		if (num6 == 0)
		{
			this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[0].transform.GetChild(0).tag = "AreaSchieramentoAlleato";
			this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[0].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierAlleato;
		}
		else if (num7 == 0)
		{
			this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[0].transform.GetChild(0).tag = "AreaSchieramentoNemico";
			this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[0].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierNemico;
		}
		for (int num9 = 0; num9 < this.infoAlleati.GetComponent<InfoMunizionamento>().ListaTipiMunizioniBaseTattica.Count; num9++)
		{
			this.infoAlleati.GetComponent<InfoMunizionamento>().ListaTipiMunizioniBaseTattica[num9].GetComponent<QuantitàMunizione>().quantità = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " quantità munizione " + num9);
		}
		for (int num10 = 0; num10 < this.infoAlleati.GetComponent<InfoMunizionamento>().ListaQuantitàSupportoTattica.Count; num10++)
		{
			this.infoAlleati.GetComponent<InfoMunizionamento>().ListaQuantitàSupportoTattica[num10] = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " quantità supporto " + num10);
		}
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x000F4860 File Offset: 0x000F2A60
	private void CaricamentoDaTatticaAStrategia()
	{
		this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[GestoreNeutroStrategia.indiceStanzaDiBattaglia];
		GameObject bandieraSelezionata = this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata;
		bandieraSelezionata.GetComponent<CentroStanza>().ListaAllSopravv = new List<int>();
		bandieraSelezionata.GetComponent<CentroStanza>().ListaAllPresentiInBatt = new List<int>();
		bandieraSelezionata.GetComponent<CentroStanza>().ListaDanniAlleati = new List<float>();
		for (int i = 0; i < this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaUnitàAlleate.Count; i++)
		{
			bandieraSelezionata.GetComponent<CentroStanza>().ListaAllSopravv.Add(0);
			bandieraSelezionata.GetComponent<CentroStanza>().ListaAllSopravv[i] = PlayerPrefs.GetInt(string.Concat(new object[]
			{
				CaricaDati.slotSuCuiCaricare,
				" lista alleati rimanenti posizione ",
				i,
				" quantità"
			}));
			bandieraSelezionata.GetComponent<CentroStanza>().ListaAllPresentiInBatt.Add(0);
			bandieraSelezionata.GetComponent<CentroStanza>().ListaAllPresentiInBatt[i] = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " alleato presente in battaglia " + i);
			bandieraSelezionata.GetComponent<CentroStanza>().ListaDanniAlleati.Add(0f);
			bandieraSelezionata.GetComponent<CentroStanza>().ListaDanniAlleati[i] = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " danno alleato " + i);
		}
		if (this.tipoBattaglia == 0 || this.tipoBattaglia == 1 || this.tipoBattaglia == 2)
		{
			bandieraSelezionata.GetComponent<CentroStanza>().ListaNemSopravv = new List<int>();
			bandieraSelezionata.GetComponent<CentroStanza>().ListaNemPresentiInBatt = new List<int>();
			bandieraSelezionata.GetComponent<CentroStanza>().ListaDanniNemici = new List<float>();
			for (int j = 0; j < this.nest.GetComponent<IANemicoStrategia>().ListaTipiInsetti.Count; j++)
			{
				bandieraSelezionata.GetComponent<CentroStanza>().ListaNemSopravv.Add(0);
				bandieraSelezionata.GetComponent<CentroStanza>().ListaNemSopravv[j] = PlayerPrefs.GetInt(string.Concat(new object[]
				{
					CaricaDati.slotSuCuiCaricare,
					" lista nemici rimanenti posizione ",
					j,
					" quantità"
				}));
				bandieraSelezionata.GetComponent<CentroStanza>().ListaNemPresentiInBatt.Add(0);
				bandieraSelezionata.GetComponent<CentroStanza>().ListaNemPresentiInBatt[j] = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " nemico presente in battaglia " + j);
				bandieraSelezionata.GetComponent<CentroStanza>().ListaDanniNemici.Add(0f);
				bandieraSelezionata.GetComponent<CentroStanza>().ListaDanniNemici[j] = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + " danno nemico " + j);
			}
			bandieraSelezionata.GetComponent<CentroStanza>().numIdentSwarmSpecialeInAtt = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " num identità swarm speciale in battaglia");
		}
		else if (this.tipoBattaglia == 3)
		{
			GestoreNeutroStrategia.soldatiSalvatiInBatt3 = PlayerPrefs.GetInt(CaricaDati.slotSuCuiCaricare + " soldati salvati in battaglia 3");
		}
		this.cameraCasa.GetComponent<GestoreNeutroStrategia>().totaleExpDaBatt = PlayerPrefs.GetFloat(CaricaDati.slotSuCuiCaricare + "totale per Exp");
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x000F4BB0 File Offset: 0x000F2DB0
	private void CaricaDatiBattagliaVeloce()
	{
		this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili = new List<List<int>>();
		for (int i = 0; i < 48; i++)
		{
			List<int> list = new List<int>();
			list.Add(100);
			list.Add(0);
			this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili.Add(list);
		}
		for (int j = 0; j < 48; j++)
		{
			this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[j][0] = PlayerPrefs.GetInt("lista alleati per batt vel posizione " + j + " tipo");
			this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiDisponibili[j][1] = PlayerPrefs.GetInt("lista alleati per batt vel posizione " + j + " quantità");
		}
		this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciCaricati = new List<List<int>>();
		for (int k = 0; k < 48; k++)
		{
			List<int> list2 = new List<int>();
			list2.Add(100);
			list2.Add(0);
			this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciCaricati.Add(list2);
		}
		for (int l = 0; l < 48; l++)
		{
			this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciCaricati[l][0] = PlayerPrefs.GetInt("lista nemici per batt vel posizione " + l + " tipo");
			this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciCaricati[l][1] = PlayerPrefs.GetInt("lista nemici per batt vel posizione " + l + " quantità");
		}
		if (GestoreNeutroStrategia.tipoBattaglia < 6)
		{
			for (int m = 0; m < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento.Count; m++)
			{
				bool flag = false;
				int num = 0;
				while (num < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaSchierAlleatiBattVel.Count && !flag)
				{
					if (m == this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaSchierAlleatiBattVel[num])
					{
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[m].transform.GetChild(0).tag = "AreaSchieramentoAlleato";
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[m].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierAlleato;
						flag = true;
					}
					else
					{
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[m].transform.GetChild(0).tag = "AreaSchieramentoNemico";
						this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[m].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierNemico;
					}
					num++;
				}
			}
		}
		else if (GestoreNeutroStrategia.tipoBattaglia == 6)
		{
			for (int n = 0; n < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento.Count; n++)
			{
				if (n == 0 || n == 1)
				{
					this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[n].transform.GetChild(0).tag = "AreaSchieramentoAlleato";
					this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[n].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierAlleato;
				}
				else
				{
					this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[n].transform.GetChild(0).tag = "AreaSchieramentoNemico";
					this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[n].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierNemico;
				}
			}
		}
		else if (GestoreNeutroStrategia.tipoBattaglia == 7)
		{
			for (int num2 = 0; num2 < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento.Count; num2++)
			{
				if (num2 == 0 || num2 == 1)
				{
					this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num2].transform.GetChild(0).tag = "AreaSchieramentoNemico";
					this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num2].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierNemico;
				}
				else
				{
					this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num2].transform.GetChild(0).tag = "AreaSchieramentoAlleato";
					this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num2].GetComponent<MeshRenderer>().material = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().coloreSchierAlleato;
				}
			}
		}
		this.infoAlleati.GetComponent<InfoGenericheAlleati>().numMaxAlleati = PlayerPrefs.GetInt("batt vel imp numero alleati");
		this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().numeroMaxAlleati = PlayerPrefs.GetInt("batt vel imp numero alleati");
		this.IANemico.GetComponent<InfoGenericheNemici>().numMaxNemici = PlayerPrefs.GetInt("batt vel imp numero nemici");
		this.IANemico.GetComponent<InfoGenericheNemici>().fattoreVitaNemici = PlayerPrefs.GetFloat("batt vel imp vita nemici");
		this.IANemico.GetComponent<InfoGenericheNemici>().fattoreAttaccoNemici = PlayerPrefs.GetFloat("batt vel imp attacco nemici");
	}

	// Token: 0x0400195E RID: 6494
	public static bool caricamentoAttivo;

	// Token: 0x0400195F RID: 6495
	public static bool caricamentoNuovaCampagna;

	// Token: 0x04001960 RID: 6496
	public static int slotSuCuiCaricare;

	// Token: 0x04001961 RID: 6497
	public bool cancellaSalvataggioSelez;

	// Token: 0x04001962 RID: 6498
	private List<int> ListaSlotCaricabili;

	// Token: 0x04001963 RID: 6499
	public static bool InizializConCaricamStrategia;

	// Token: 0x04001964 RID: 6500
	public static bool InizializConCaricamTattica;

	// Token: 0x04001965 RID: 6501
	public bool senzaInizializ;

	// Token: 0x04001966 RID: 6502
	private GameObject cameraCasa;

	// Token: 0x04001967 RID: 6503
	private GameObject headquarters;

	// Token: 0x04001968 RID: 6504
	private GameObject nest;

	// Token: 0x04001969 RID: 6505
	private GameObject finestraCarica;

	// Token: 0x0400196A RID: 6506
	private GameObject listaPulsantiSlot;

	// Token: 0x0400196B RID: 6507
	private GameObject elencoPostiEdifici;

	// Token: 0x0400196C RID: 6508
	private GameObject varieMappaStrategica;

	// Token: 0x0400196D RID: 6509
	private GameObject infoAlleati;

	// Token: 0x0400196E RID: 6510
	private GameObject infoNeutreTattica;

	// Token: 0x0400196F RID: 6511
	private GameObject varieMappaLocale;

	// Token: 0x04001970 RID: 6512
	private GameObject IANemico;

	// Token: 0x04001971 RID: 6513
	private GameObject CanvasMenuIniziale;

	// Token: 0x04001972 RID: 6514
	private GameObject inizioLivello;

	// Token: 0x04001973 RID: 6515
	private int tipoBattaglia;
}
