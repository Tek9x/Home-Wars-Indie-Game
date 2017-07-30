using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000106 RID: 262
public class GestoreNeutroStrategia : MonoBehaviour
{
	// Token: 0x06000840 RID: 2112 RVA: 0x00121320 File Offset: 0x0011F520
	private void Start()
	{
		GestoreNeutroStrategia.valoreRandomSeed = DateTime.Now.Second;
		this.CanvasStrategia = GameObject.FindGameObjectWithTag("CanvasStrategia");
		this.pulsanteFineTurno = this.CanvasStrategia.transform.GetChild(0).GetChild(1).gameObject;
		this.Nest = GameObject.FindGameObjectWithTag("Nest");
		this.Headquarters = GameObject.FindGameObjectWithTag("Headquarters");
		this.pulsanteFineTurno = this.CanvasStrategia.transform.FindChild("Barra Alta").FindChild("fine turno").gameObject;
		this.pulsanteSalva = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Menu Strategico").GetChild(0).FindChild("Salva").gameObject;
		this.scrittaData = this.CanvasStrategia.transform.FindChild("Barra Alta").FindChild("data e turno").FindChild("data").gameObject;
		this.scrittaTurno = this.CanvasStrategia.transform.FindChild("Barra Alta").FindChild("data e turno").FindChild("turno").gameObject;
		this.schermFineCampagna = this.CanvasStrategia.transform.FindChild("Fine Campagna").gameObject;
		this.oggettoMusica = GameObject.FindGameObjectWithTag("Musica");
		this.bloccoColorePuls = this.pulsanteFineTurno.GetComponent<Button>().colors;
		this.ListaDiListeDiVicinanze = new List<List<int>>();
		this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon0);
		this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon1);
		this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon2);
		this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon3);
		this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon4);
		this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon5);
		this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon6);
		this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon7);
		this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon8);
		if (this.ListaCentroStanze.Count > 9)
		{
			this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon9);
		}
		if (this.ListaCentroStanze.Count > 10)
		{
			this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon10);
		}
		if (this.ListaCentroStanze.Count > 11)
		{
			this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon11);
		}
		if (this.ListaCentroStanze.Count > 12)
		{
			this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon12);
		}
		if (this.ListaCentroStanze.Count > 13)
		{
			this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon13);
		}
		if (this.ListaCentroStanze.Count > 14)
		{
			this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon14);
		}
		if (this.ListaCentroStanze.Count > 15)
		{
			this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon15);
		}
		if (this.ListaCentroStanze.Count > 16)
		{
			this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon16);
		}
		if (this.ListaCentroStanze.Count > 17)
		{
			this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon17);
		}
		if (this.ListaCentroStanze.Count > 18)
		{
			this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon18);
		}
		if (this.ListaCentroStanze.Count > 19)
		{
			this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon19);
		}
		if (this.ListaCentroStanze.Count > 20)
		{
			this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon20);
		}
		if (this.ListaCentroStanze.Count > 21)
		{
			this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon21);
		}
		if (this.ListaCentroStanze.Count > 22)
		{
			this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon22);
		}
		if (this.ListaCentroStanze.Count > 23)
		{
			this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon23);
		}
		if (this.ListaCentroStanze.Count > 24)
		{
			this.ListaDiListeDiVicinanze.Add(this.ListaVicinanzaCon24);
		}
		this.aggiornaScrittaDataETurno = true;
		this.ListaMesi = new List<string>();
		this.ListaMesi.Add("March");
		this.ListaMesi.Add("April");
		this.ListaMesi.Add("May");
		this.ListaMesi.Add("June");
		this.ListaMesi.Add("July");
		this.ListaMesi.Add("August");
		this.ListaMesi.Add("September");
		this.ListaMesi.Add("October");
		if (this.numeroTurno == 0)
		{
			this.giornoData = 1;
			this.meseData = 0;
			GameObject gameObject = this.CanvasStrategia.transform.FindChild("Schermate Inizio Campagna").FindChild("schermata consigli").gameObject;
			gameObject.GetComponent<CanvasGroup>().alpha = 1f;
			gameObject.GetComponent<CanvasGroup>().interactable = true;
			gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
			this.Nest.GetComponent<IANemicoStrategia>().livelloNest = 1;
			this.giorniDaInizioFittizi = 0f;
		}
		this.ListaRicompenseRisorse = new List<List<int>>();
		for (int i = 0; i < 4; i++)
		{
			List<int> list = new List<int>();
			list.Add(0);
			list.Add(0);
			this.ListaRicompenseRisorse.Add(list);
		}
		this.ListaRicompenseArmi = new List<List<int>>();
		for (int j = 0; j < 3; j++)
		{
			List<int> list2 = new List<int>();
			list2.Add(0);
			list2.Add(0);
			this.ListaRicompenseArmi.Add(list2);
		}
		if (GestoreNeutroStrategia.campagnaAppenaIniziata)
		{
			GestoreNeutroStrategia.campagnaAppenaIniziata = false;
			if (GestoreNeutroStrategia.stagione == 0)
			{
				GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermate Inizio Campagna").FindChild("prima schermata").gameObject;
				gameObject2.GetComponent<CanvasGroup>().alpha = 1f;
				gameObject2.GetComponent<CanvasGroup>().interactable = true;
				gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = true;
			}
			this.saltoGiorniPerTurno = PlayerPrefs.GetInt("campagna frequenza turni");
			this.annoData = PlayerPrefs.GetInt("campagna anno partenza");
			GestoreNeutroStrategia.valVitaStagionaleNemici = (float)(GestoreNeutroStrategia.stagione * 15);
			GestoreNeutroStrategia.valAttaccoStagionaleNemici = (float)(GestoreNeutroStrategia.stagione * 5);
			GestoreNeutroStrategia.mostraResocontoBattaglia = false;
			GestoreNeutroStrategia.mostraElencoResoconto = false;
		}
		if (this.oggettoMusica)
		{
			if (!this.oggettoMusica.GetComponent<MusicaScript>().musica.isPlaying)
			{
				this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica = 4;
				this.oggettoMusica.GetComponent<MusicaScript>().attivitàInMusica = true;
				this.oggettoMusica.GetComponent<MusicaScript>().musica.volume = PlayerPrefs.GetFloat("volume musica strategia");
			}
			else if (this.oggettoMusica.GetComponent<MusicaScript>().musica.isPlaying && this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica != 4)
			{
				this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica = 4;
				this.oggettoMusica.GetComponent<MusicaScript>().attivitàInMusica = true;
				this.oggettoMusica.GetComponent<MusicaScript>().musica.volume = PlayerPrefs.GetFloat("volume musica strategia");
			}
		}
		for (int k = 0; k <= this.meseData; k++)
		{
			if (k < this.meseData)
			{
				this.giorniDaInizioFittizi += 30f;
			}
			else
			{
				for (int l = 0; l < 30; l++)
				{
					if (l >= this.giornoData)
					{
						break;
					}
					this.giorniDaInizioFittizi += 1f;
				}
			}
		}
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x00121B1C File Offset: 0x0011FD1C
	private void Update()
	{
		this.timerDaInizioScena += Time.deltaTime;
		if (GestoreNeutroStrategia.vincitore != 0 && !this.ricompenseDecise && !this.assegnaRicompense)
		{
			this.premiAssegnati++;
			this.ricompenseDecise = true;
			this.PostBattaglia();
		}
		if (this.assegnaRicompense)
		{
			this.assegnaRicompense = false;
			this.AssegnaPremi();
			GestoreNeutroStrategia.vincitore = 0;
			this.numAlleatiMortiinTotale += GestoreNeutroTattica.numAlleatiMorti;
			this.numNemiciMortiinTotale += GestoreNeutroTattica.numNemiciMorti;
		}
		this.ScattoTurni();
		this.DateETurni();
		this.VerificheTurniVarie();
		if (this.missioneDaDecidere == 1)
		{
			this.missioneDaDecidere = 0;
			this.GeneratoreMissioniSecEdExtra();
		}
		this.Varie();
		this.FineCampagna();
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x00121BF0 File Offset: 0x0011FDF0
	private void PostBattaglia()
	{
		if (GestoreNeutroStrategia.tipoBattaglia == 0 || GestoreNeutroStrategia.tipoBattaglia == 1 || GestoreNeutroStrategia.tipoBattaglia == 2)
		{
			GameObject gameObject = this.ListaCentroStanze[GestoreNeutroStrategia.indiceStanzaDiBattaglia];
			if (GestoreNeutroStrategia.vincitore == 1 && GestoreNeutroStrategia.attaccante == 0)
			{
				bool flag = false;
				int num = 0;
				while (num < 3 && !flag)
				{
					if (gameObject.GetComponent<CentroStanza>().ListaSettori[num] == 2)
					{
						gameObject.GetComponent<CentroStanza>().ListaSettori[num] = 1;
						flag = true;
					}
					num++;
				}
			}
			else if (GestoreNeutroStrategia.vincitore == 2 && GestoreNeutroStrategia.attaccante == 1)
			{
				bool flag2 = false;
				int num2 = 0;
				while (num2 < 3 && !flag2)
				{
					if (gameObject.GetComponent<CentroStanza>().ListaSettori[num2] == 1)
					{
						gameObject.GetComponent<CentroStanza>().ListaSettori[num2] = 2;
						flag2 = true;
					}
					num2++;
				}
			}
		}
		this.CreazionePremi();
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x00121CF4 File Offset: 0x0011FEF4
	private void ScattoTurni()
	{
		if (!this.turnoNemicoAttivo)
		{
			if (this.timerTurnoAlleati == 0f && this.timerTurnoNemici > 0f)
			{
				this.scattoTurnoVersoAlleati = true;
				this.aggMissioneExtra = true;
				for (int i = 0; i < this.Headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi.Count; i++)
				{
					this.Headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[i].GetComponent<PresenzaAlleataStrategica>().puòAncoraMuoversi = true;
				}
				this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().aggiornaLavoroEdifici = true;
				if (this.numeroTurno > 0)
				{
					this.missioneDaDecidere = 1;
				}
				this.Nest.GetComponent<IANemicoStrategia>().calcoloObbiettiEff = false;
				this.Nest.GetComponent<IANemicoStrategia>().movimentiOrdinati = false;
				this.Nest.GetComponent<IANemicoStrategia>().turnoNemicoPuòFinirePerIAN = false;
				if (this.Nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici.Count > 0)
				{
					for (int j = 0; j < this.Nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici.Count; j++)
					{
						if (this.Nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[j] != null)
						{
							this.Nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[j].GetComponent<PresenzaNemicaStrategica>().prontoPerFineTurnoNemico = false;
							this.Nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[j].GetComponent<PresenzaNemicaStrategica>().swarmSpecialeHaAttaccato = 0;
							this.Nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[j].GetComponent<PresenzaNemicaStrategica>().vecchiaPosizioneAttuale = this.Nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[j].GetComponent<PresenzaNemicaStrategica>().posizioneAttuale;
						}
					}
				}
				for (int k = 0; k < this.ListaCentroStanze.Count; k++)
				{
					this.ListaCentroStanze[k].GetComponent<CentroStanza>().quiCèStataBattaglia = 0;
				}
			}
			else
			{
				this.scattoTurnoVersoAlleati = false;
				if (this.timerTurnoAlleati < 1f)
				{
					this.aggMissioneExtra = true;
				}
				else
				{
					this.aggMissioneExtra = false;
				}
			}
			this.timerTurnoAlleati += Time.deltaTime;
			this.timerTurnoNemici = 0f;
		}
		else
		{
			if (this.timerTurnoAlleati > 0f && this.timerTurnoNemici == 0f)
			{
				this.scattoTurnoVersoNemici = true;
			}
			else
			{
				this.scattoTurnoVersoNemici = false;
			}
			this.timerTurnoAlleati = 0f;
			this.timerTurnoNemici += Time.deltaTime;
		}
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x00121F90 File Offset: 0x00120190
	private void DateETurni()
	{
		if (this.cambioTurnoEffettuato)
		{
			this.numeroTurno++;
			this.aggiornaScrittaDataETurno = true;
			bool flag = false;
			for (int i = 0; i < this.saltoGiorniPerTurno; i++)
			{
				this.giornoData++;
				if (this.meseData == 1 || this.meseData == 3 || this.meseData == 6)
				{
					if (this.giornoData > 30)
					{
						this.giornoData = 1;
						flag = true;
					}
				}
				else if (this.giornoData > 31)
				{
					this.giornoData = 1;
					flag = true;
				}
			}
			if (flag)
			{
				this.meseData++;
			}
			for (int j = 0; j <= this.meseData; j++)
			{
				if (j < this.meseData)
				{
					this.giorniDaInizioFittizi += 30f;
				}
				else
				{
					for (int k = 0; k < 30; k++)
					{
						if (k >= this.giornoData)
						{
							break;
						}
						this.giorniDaInizioFittizi += 1f;
					}
				}
			}
			this.cambioTurnoEffettuato = false;
		}
		if (this.aggiornaScrittaDataETurno)
		{
			this.scrittaTurno.GetComponent<Text>().text = "Turn: " + this.numeroTurno;
			this.scrittaData.GetComponent<Text>().text = string.Concat(new string[]
			{
				this.giornoData.ToString(),
				" ",
				this.ListaMesi[this.meseData],
				" ",
				this.annoData.ToString()
			});
			this.aggiornaScrittaDataETurno = false;
		}
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x0012215C File Offset: 0x0012035C
	private void VerificheTurniVarie()
	{
		int num = 0;
		for (int i = 0; i < this.ListaCentroStanze.Count; i++)
		{
			if (this.ListaCentroStanze[i].GetComponent<CentroStanza>().quiNemicoStaAttaccando)
			{
				num++;
			}
		}
		if (num != 0)
		{
			this.ciSonoAttacchiIrrisolti = true;
		}
		else
		{
			this.ciSonoAttacchiIrrisolti = false;
		}
		int num2 = 0;
		for (int j = 0; j < this.Headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi.Count; j++)
		{
			if (!this.Headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[j].GetComponent<PresenzaAlleataStrategica>().èFermo)
			{
				num2++;
			}
		}
		if (num2 != 0)
		{
			this.esercitiInMovimento = true;
		}
		else
		{
			this.esercitiInMovimento = false;
		}
		if (this.turnoNemicoAttivo && this.Nest.GetComponent<IANemicoStrategia>().turnoNemicoPuòFinirePerIAN && this.timerTurnoNemici > 1f)
		{
			this.turnoNemicoAttivo = false;
			this.cambioTurnoEffettuato = true;
		}
		if (this.turnoNemicoAttivo && this.timerTurnoNemici > 30f)
		{
			this.turnoNemicoAttivo = false;
			this.cambioTurnoEffettuato = true;
		}
		if (this.turnoNemicoAttivo || this.esercitiInMovimento)
		{
			this.pulsanteSalva.GetComponent<Button>().interactable = false;
		}
		if (!this.turnoNemicoAttivo && !this.esercitiInMovimento)
		{
			this.pulsanteSalva.GetComponent<Button>().interactable = true;
		}
		if (this.turnoNemicoAttivo)
		{
			this.pulsanteFineTurno.GetComponent<Button>().interactable = false;
			this.bloccoColorePuls.disabledColor = Color.grey;
			this.pulsanteFineTurno.GetComponent<Button>().colors = this.bloccoColorePuls;
		}
		else
		{
			if (this.ciSonoAttacchiIrrisolti || this.esercitiInMovimento)
			{
				this.pulsanteFineTurno.GetComponent<Button>().interactable = false;
				this.bloccoColorePuls.disabledColor = Color.red;
				this.pulsanteFineTurno.GetComponent<Button>().colors = this.bloccoColorePuls;
			}
			if (!this.ciSonoAttacchiIrrisolti && !this.esercitiInMovimento)
			{
				this.pulsanteFineTurno.GetComponent<Button>().interactable = true;
			}
		}
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x0012239C File Offset: 0x0012059C
	private void GeneratoreMissioniSecEdExtra()
	{
		this.missionePresente = 0;
		if (this.numeroTurno > 10)
		{
			float value = UnityEngine.Random.value;
			GestoreNeutroStrategia.valoreRandomSeed++;
			UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
			if (value <= 0.6f)
			{
				this.missionePresente = 1;
			}
			else
			{
				this.missionePresente = 0;
			}
		}
		else
		{
			this.missionePresente = 0;
		}
		if (this.missionePresente == 1)
		{
			GestoreNeutroStrategia.valoreRandomSeed++;
			UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
			this.tipoMissione = UnityEngine.Random.Range(3, 8);
			if (this.tipoMissione == 5)
			{
				this.ListaStanzePerMissioneSatellite = new List<int>();
				for (int i = 0; i < this.ListaSatelliti.Count; i++)
				{
					if (this.ListaSatelliti[i] == 1)
					{
						this.ListaStanzePerMissioneSatellite.Add(i);
					}
				}
				if (this.ListaStanzePerMissioneSatellite.Count > 0)
				{
					int num = UnityEngine.Random.Range(0, this.ListaStanzePerMissioneSatellite.Count);
					GestoreNeutroStrategia.valoreRandomSeed++;
					UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
					for (int j = 0; j < this.ListaCentroStanze.Count; j++)
					{
						if (this.ListaStanzePerMissioneSatellite[num] == j)
						{
							this.stanzaDiMissione = j;
						}
					}
				}
				else
				{
					this.missionePresente = 0;
				}
			}
			else if (this.tipoMissione == 6 || this.tipoMissione == 7)
			{
				int num = UnityEngine.Random.Range(0, this.ListaStanzePerMissioneConvoglio.Count);
				GestoreNeutroStrategia.valoreRandomSeed++;
				UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
				for (int k = 0; k < this.ListaCentroStanze.Count; k++)
				{
					if (this.ListaStanzePerMissioneConvoglio[num] == k)
					{
						this.stanzaDiMissione = k;
					}
				}
			}
			else
			{
				int num = UnityEngine.Random.Range(0, this.ListaCentroStanze.Count);
				GestoreNeutroStrategia.valoreRandomSeed++;
				UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
				for (int l = 0; l < this.ListaCentroStanze.Count; l++)
				{
					if (num == l)
					{
						this.stanzaDiMissione = l;
					}
				}
			}
		}
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x001225E4 File Offset: 0x001207E4
	private void Varie()
	{
		if (this.partenzaTimerAggMissioneExtra)
		{
			this.timerAggMissioneExtra += Time.deltaTime;
			this.aggMissioneExtra = true;
			if (this.timerAggMissioneExtra > 6f)
			{
				this.timerAggMissioneExtra = 0f;
				this.partenzaTimerAggMissioneExtra = false;
			}
		}
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x00122638 File Offset: 0x00120838
	private void CreazionePremi()
	{
		if (GestoreNeutroStrategia.vincitore == 1)
		{
			if (GestoreNeutroStrategia.tipoBattaglia == 0 || GestoreNeutroStrategia.tipoBattaglia == 1 || GestoreNeutroStrategia.tipoBattaglia == 2 || GestoreNeutroStrategia.tipoBattaglia == 6 || GestoreNeutroStrategia.tipoBattaglia == 7)
			{
				List<float> list = new List<float>();
				for (int i = 0; i < this.Headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaRapportiRisorse.Count; i++)
				{
					list.Add(this.Headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaRapportiRisorse[i]);
				}
				this.ListaRicompenseRisorse[0][0] = 10;
				this.ListaRicompenseRisorse[0][1] = Mathf.FloorToInt(this.totaleExpDaBatt / 110f);
				for (int j = 1; j < this.ListaRicompenseRisorse.Count; j++)
				{
					int num = UnityEngine.Random.Range(0, this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti.Count - 1);
					this.ListaRicompenseRisorse[j][0] = num;
					this.ListaRicompenseRisorse[j][1] = Mathf.FloorToInt(UnityEngine.Random.Range(500f / list[num], 1000f / list[num])) + 1;
				}
			}
			else if (GestoreNeutroStrategia.tipoBattaglia == 3 || GestoreNeutroStrategia.tipoBattaglia == 4 || GestoreNeutroStrategia.tipoBattaglia == 5)
			{
				List<float> list2 = new List<float>();
				for (int k = 0; k < this.Headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaRapportiValoreArmi.Count; k++)
				{
					list2.Add(this.Headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaRapportiValoreArmi[k]);
				}
				this.ListaRicompenseRisorse[0][0] = 10;
				this.ListaRicompenseRisorse[0][1] = Mathf.FloorToInt(this.totaleExpDaBatt / 110f);
				for (int l = 0; l < this.ListaRicompenseArmi.Count; l++)
				{
					int num2 = UnityEngine.Random.Range(0, this.Headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia.Count);
					this.ListaRicompenseArmi[l][0] = num2;
					if (GestoreNeutroStrategia.tipoBattaglia == 3)
					{
						this.ListaRicompenseArmi[l][1] = Mathf.FloorToInt(UnityEngine.Random.Range(1000f / list2[num2], 2000f / list2[num2]) * 0.5f) + 1;
					}
					else if (GestoreNeutroStrategia.tipoBattaglia == 4)
					{
						this.ListaRicompenseArmi[l][1] = Mathf.FloorToInt(UnityEngine.Random.Range(1000f / list2[num2], 2000f / list2[num2]) * 2f) + 1;
					}
					else
					{
						this.ListaRicompenseArmi[l][1] = Mathf.FloorToInt(UnityEngine.Random.Range(1000f / list2[num2], 2000f / list2[num2])) + 1;
					}
				}
			}
		}
		else if (GestoreNeutroStrategia.vincitore == 2)
		{
			if (GestoreNeutroStrategia.tipoBattaglia == 0 || GestoreNeutroStrategia.tipoBattaglia == 1 || GestoreNeutroStrategia.tipoBattaglia == 2)
			{
				this.ricompensaFreshFoodNemico = Mathf.Floor((float)UnityEngine.Random.Range(15, 25) * PlayerPrefs.GetFloat("fattore diff fresh food"));
				this.ricompensaRottenFoodNemico = Mathf.Floor((float)UnityEngine.Random.Range(10, 20) * PlayerPrefs.GetFloat("fattore diff rotten food"));
				this.ricompensaHighProteinFoodNemico = Mathf.Floor((float)UnityEngine.Random.Range(5, 10) * PlayerPrefs.GetFloat("fattore diff high protein food"));
			}
			else if (GestoreNeutroStrategia.tipoBattaglia == 3 || GestoreNeutroStrategia.tipoBattaglia == 4 || GestoreNeutroStrategia.tipoBattaglia == 5)
			{
				this.ricompensaFreshFoodNemico = Mathf.Floor((float)UnityEngine.Random.Range(10, 15) * PlayerPrefs.GetFloat("fattore diff fresh food"));
				this.ricompensaRottenFoodNemico = Mathf.Floor((float)UnityEngine.Random.Range(5, 10) * PlayerPrefs.GetFloat("fattore diff rotten food"));
				this.ricompensaHighProteinFoodNemico = Mathf.Floor((float)UnityEngine.Random.Range(3, 6)) * PlayerPrefs.GetFloat("fattore diff high protein food");
			}
			else if (GestoreNeutroStrategia.tipoBattaglia == 6 || GestoreNeutroStrategia.tipoBattaglia == 7)
			{
				this.ricompensaFreshFoodNemico = Mathf.Floor((float)UnityEngine.Random.Range(15, 20) * PlayerPrefs.GetFloat("fattore diff fresh food"));
				this.ricompensaRottenFoodNemico = Mathf.Floor((float)UnityEngine.Random.Range(10, 15) * PlayerPrefs.GetFloat("fattore diff rotten food"));
				this.ricompensaHighProteinFoodNemico = Mathf.Floor((float)UnityEngine.Random.Range(5, 10) * PlayerPrefs.GetFloat("fattore diff high protein food"));
			}
		}
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x00122AF8 File Offset: 0x00120CF8
	private void AssegnaPremi()
	{
		if (GestoreNeutroStrategia.vincitore == 1)
		{
			if (!this.battagliaATavolino)
			{
				if (GestoreNeutroStrategia.tipoBattaglia == 0 || GestoreNeutroStrategia.tipoBattaglia == 1 || GestoreNeutroStrategia.tipoBattaglia == 2 || GestoreNeutroStrategia.tipoBattaglia == 6 || GestoreNeutroStrategia.tipoBattaglia == 7)
				{
					for (int i = 0; i < this.ListaRicompenseRisorse.Count; i++)
					{
						bool flag = false;
						int num = 0;
						while (num < this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti.Count && !flag)
						{
							if (this.ListaRicompenseRisorse[i][0] == num)
							{
								if (GestoreNeutroStrategia.tipoBattaglia == 6)
								{
									if (i == 0)
									{
										this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[num].GetComponent<PresenzaRisorsa>().quantitàRisorsa += (float)this.ListaRicompenseRisorse[i][1];
									}
									else
									{
										this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[num].GetComponent<PresenzaRisorsa>().quantitàRisorsa += (float)(this.ListaRicompenseRisorse[i][1] * GestoreNeutroStrategia.convogliArrivati);
									}
								}
								else
								{
									this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[num].GetComponent<PresenzaRisorsa>().quantitàRisorsa += (float)this.ListaRicompenseRisorse[i][1];
								}
								flag = true;
							}
							num++;
						}
					}
				}
				else if (GestoreNeutroStrategia.tipoBattaglia == 3 || GestoreNeutroStrategia.tipoBattaglia == 4 || GestoreNeutroStrategia.tipoBattaglia == 5)
				{
					this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[10].GetComponent<PresenzaRisorsa>().quantitàRisorsa += (float)this.ListaRicompenseRisorse[0][1];
					for (int j = 0; j < this.ListaRicompenseArmi.Count; j++)
					{
						bool flag2 = false;
						int num2 = 0;
						while (num2 < this.Headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia.Count && !flag2)
						{
							if (this.ListaRicompenseArmi[j][0] == num2)
							{
								if (GestoreNeutroStrategia.tipoBattaglia == 3)
								{
									this.Headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[num2].GetComponent<QuantitàMunizione>().quantità += (float)(this.ListaRicompenseArmi[j][1] * GestoreNeutroStrategia.soldatiSalvatiInBatt3);
								}
								else
								{
									this.Headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[num2].GetComponent<QuantitàMunizione>().quantità += (float)this.ListaRicompenseArmi[j][1];
								}
								flag2 = true;
							}
							num2++;
						}
					}
					this.Headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().aggiornaMunizioniPres = true;
				}
			}
		}
		else if (GestoreNeutroStrategia.vincitore == 2)
		{
			if (GestoreNeutroStrategia.tipoBattaglia == 7)
			{
				this.Nest.GetComponent<IANemicoStrategia>().freshFoodBattOMiss += this.ricompensaFreshFoodNemico * (float)GestoreNeutroStrategia.convogliArrivati;
				this.Nest.GetComponent<IANemicoStrategia>().rottenFoodBattOMiss += this.ricompensaRottenFoodNemico * (float)GestoreNeutroStrategia.convogliArrivati;
				this.Nest.GetComponent<IANemicoStrategia>().highProteinFoodBattOMiss += this.ricompensaHighProteinFoodNemico * (float)GestoreNeutroStrategia.convogliArrivati;
			}
			else
			{
				this.Nest.GetComponent<IANemicoStrategia>().freshFoodBattOMiss += this.ricompensaFreshFoodNemico;
				this.Nest.GetComponent<IANemicoStrategia>().rottenFoodBattOMiss += this.ricompensaRottenFoodNemico;
				this.Nest.GetComponent<IANemicoStrategia>().highProteinFoodBattOMiss += this.ricompensaHighProteinFoodNemico;
			}
		}
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x00122ED4 File Offset: 0x001210D4
	private void FineCampagna()
	{
		if (this.scattoTurnoVersoAlleati && this.meseData == 7 && this.giornoData == 1)
		{
			this.campagnaèFinita = true;
			if (this.ListaCentroStanze[0].GetComponent<CentroStanza>().settoriAlleati == 3)
			{
				this.vincitoreCampagna = 1;
			}
			else
			{
				this.vincitoreCampagna = 2;
			}
		}
		if (this.ListaCentroStanze[0].GetComponent<CentroStanza>().settoriAlleati == 3)
		{
			this.campagnaèFinita = true;
			this.vincitoreCampagna = 1;
		}
		else if (this.ListaCentroStanze[this.ListaCentroStanze.Count - 1].GetComponent<CentroStanza>().settoriNemici == 3)
		{
			this.campagnaèFinita = true;
			this.vincitoreCampagna = 2;
		}
		if (this.campagnaèFinita)
		{
			this.schermFineCampagna.GetComponent<CanvasGroup>().alpha = 1f;
			this.schermFineCampagna.GetComponent<CanvasGroup>().interactable = true;
			this.schermFineCampagna.GetComponent<CanvasGroup>().blocksRaycasts = true;
			this.schermFineCampagna.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<Text>().text = "Total Dead Allies:  " + this.numAlleatiMortiinTotale;
			this.schermFineCampagna.transform.GetChild(0).GetChild(2).GetChild(2).GetComponent<Text>().text = "Total Dead Enemies:  " + this.numNemiciMortiinTotale;
			if (this.vincitoreCampagna == 1)
			{
				this.schermFineCampagna.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = this.immEsitoCampVittoria;
				this.schermFineCampagna.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "Congratulations General!\nAfter months of fighting, we have finally crushed the invaders!\nWe are sure they will try to attack us next year, but now it’s time to celebrate.\nWe have at least 5 months to rest, train new troops and create new tactics.\nHonor is ours, General!";
				this.schermFineCampagna.transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<Text>().text = "General, your service has been fundamental to the success of this War.\nHowever, we understand that after such a venture you may want to take a break, which is why we accept your resignation, hoping that one day you’ll want to lead us to victory again.\nWe heard that other our nearby armies, after many defeats, are looking for a valiant General. If you want to meet them, we can send messengers to alert them about your arrival. They would be happy to see you!";
			}
			else
			{
				this.schermFineCampagna.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = this.immEsitoCampSconfitta;
				this.schermFineCampagna.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "It's over!\nAfter months of fighting, we didn’t manage to block the invaders and our house is now in their hands.\nThe only thing left to do is dismantle our Headquarters and hide while waiting for the colder months. However, the defeat is so heavy we could no longer be able to organize an Army.";
				this.schermFineCampagna.transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<Text>().text = "Unfortunately General, because of the loss of our Army, we are forced to dismiss the officers, you included.\nWe hope one day we’ll have our revenge against these invaders, but until then we are forced to live in the shadows.";
				this.schermFineCampagna.transform.GetChild(0).GetChild(4).GetComponent<CanvasGroup>().interactable = false;
				this.schermFineCampagna.transform.GetChild(0).GetChild(4).GetComponent<Image>().color = Color.grey;
			}
			if (this.oggettoMusica && !this.fineCampMusicaPartita)
			{
				this.fineCampMusicaPartita = true;
				if (this.vincitoreCampagna == 1)
				{
					this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica = 2;
				}
				else if (this.vincitoreCampagna == 2)
				{
					this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica = 3;
				}
				this.oggettoMusica.GetComponent<MusicaScript>().attivitàInMusica = true;
			}
			if (this.continuaStagioneCampagna)
			{
				this.continuaStagioneCampagna = false;
				GestoreNeutroStrategia.campagnaAppenaIniziata = true;
				PlayerPrefs.SetInt("campagna frequenza turni", this.saltoGiorniPerTurno);
				PlayerPrefs.SetInt("campagna anno partenza", this.annoData + 1);
				GestoreNeutroStrategia.stagione++;
				CaricaScene.nomeScenaDaCaricare = SceneManager.GetActiveScene().name;
				SceneManager.LoadScene("Scena Di Caricamento");
			}
		}
	}

	// Token: 0x04001EBE RID: 7870
	public static int valoreRandomSeed;

	// Token: 0x04001EBF RID: 7871
	public List<GameObject> ListaDescrizMissioni;

	// Token: 0x04001EC0 RID: 7872
	public List<GameObject> ListaDescrizTruppePerMissioni;

	// Token: 0x04001EC1 RID: 7873
	public List<GameObject> ListaDescrPremioMissAlleato;

	// Token: 0x04001EC2 RID: 7874
	public List<GameObject> ListaDescrPremioMissNemico;

	// Token: 0x04001EC3 RID: 7875
	public bool turnoNemicoAttivo;

	// Token: 0x04001EC4 RID: 7876
	public bool scattoTurnoVersoAlleati;

	// Token: 0x04001EC5 RID: 7877
	public bool scattoTurnoVersoNemici;

	// Token: 0x04001EC6 RID: 7878
	private float timerTurnoAlleati;

	// Token: 0x04001EC7 RID: 7879
	public float timerTurnoNemici;

	// Token: 0x04001EC8 RID: 7880
	public int numeroTurno;

	// Token: 0x04001EC9 RID: 7881
	private List<string> ListaMesi;

	// Token: 0x04001ECA RID: 7882
	public int saltoGiorniPerTurno;

	// Token: 0x04001ECB RID: 7883
	public bool aggiornaScrittaDataETurno;

	// Token: 0x04001ECC RID: 7884
	public int giornoData;

	// Token: 0x04001ECD RID: 7885
	public int meseData;

	// Token: 0x04001ECE RID: 7886
	public int annoData;

	// Token: 0x04001ECF RID: 7887
	public float timerDaInizioScena;

	// Token: 0x04001ED0 RID: 7888
	public float giorniDaInizioFittizi;

	// Token: 0x04001ED1 RID: 7889
	private GameObject CanvasStrategia;

	// Token: 0x04001ED2 RID: 7890
	private GameObject Nest;

	// Token: 0x04001ED3 RID: 7891
	private GameObject Headquarters;

	// Token: 0x04001ED4 RID: 7892
	private GameObject pulsanteFineTurno;

	// Token: 0x04001ED5 RID: 7893
	private GameObject pulsanteSalva;

	// Token: 0x04001ED6 RID: 7894
	private GameObject scrittaData;

	// Token: 0x04001ED7 RID: 7895
	private GameObject scrittaTurno;

	// Token: 0x04001ED8 RID: 7896
	private GameObject schermFineCampagna;

	// Token: 0x04001ED9 RID: 7897
	public List<GameObject> ListaCentroStanze;

	// Token: 0x04001EDA RID: 7898
	public List<List<int>> ListaDiListeDiVicinanze;

	// Token: 0x04001EDB RID: 7899
	public List<int> ListaVicinanzaCon0;

	// Token: 0x04001EDC RID: 7900
	public List<int> ListaVicinanzaCon1;

	// Token: 0x04001EDD RID: 7901
	public List<int> ListaVicinanzaCon2;

	// Token: 0x04001EDE RID: 7902
	public List<int> ListaVicinanzaCon3;

	// Token: 0x04001EDF RID: 7903
	public List<int> ListaVicinanzaCon4;

	// Token: 0x04001EE0 RID: 7904
	public List<int> ListaVicinanzaCon5;

	// Token: 0x04001EE1 RID: 7905
	public List<int> ListaVicinanzaCon6;

	// Token: 0x04001EE2 RID: 7906
	public List<int> ListaVicinanzaCon7;

	// Token: 0x04001EE3 RID: 7907
	public List<int> ListaVicinanzaCon8;

	// Token: 0x04001EE4 RID: 7908
	public List<int> ListaVicinanzaCon9;

	// Token: 0x04001EE5 RID: 7909
	public List<int> ListaVicinanzaCon10;

	// Token: 0x04001EE6 RID: 7910
	public List<int> ListaVicinanzaCon11;

	// Token: 0x04001EE7 RID: 7911
	public List<int> ListaVicinanzaCon12;

	// Token: 0x04001EE8 RID: 7912
	public List<int> ListaVicinanzaCon13;

	// Token: 0x04001EE9 RID: 7913
	public List<int> ListaVicinanzaCon14;

	// Token: 0x04001EEA RID: 7914
	public List<int> ListaVicinanzaCon15;

	// Token: 0x04001EEB RID: 7915
	public List<int> ListaVicinanzaCon16;

	// Token: 0x04001EEC RID: 7916
	public List<int> ListaVicinanzaCon17;

	// Token: 0x04001EED RID: 7917
	public List<int> ListaVicinanzaCon18;

	// Token: 0x04001EEE RID: 7918
	public List<int> ListaVicinanzaCon19;

	// Token: 0x04001EEF RID: 7919
	public List<int> ListaVicinanzaCon20;

	// Token: 0x04001EF0 RID: 7920
	public List<int> ListaVicinanzaCon21;

	// Token: 0x04001EF1 RID: 7921
	public List<int> ListaVicinanzaCon22;

	// Token: 0x04001EF2 RID: 7922
	public List<int> ListaVicinanzaCon23;

	// Token: 0x04001EF3 RID: 7923
	public List<int> ListaVicinanzaCon24;

	// Token: 0x04001EF4 RID: 7924
	public List<int> ListaStanzePerMissioneConvoglio;

	// Token: 0x04001EF5 RID: 7925
	private List<int> ListaStanzePerMissioneSatellite;

	// Token: 0x04001EF6 RID: 7926
	public bool cambioTurnoEffettuato;

	// Token: 0x04001EF7 RID: 7927
	public static int tipoBattaglia;

	// Token: 0x04001EF8 RID: 7928
	public static int attaccante;

	// Token: 0x04001EF9 RID: 7929
	public static bool inTattica;

	// Token: 0x04001EFA RID: 7930
	public static int vincitore;

	// Token: 0x04001EFB RID: 7931
	public static int convogliArrivati;

	// Token: 0x04001EFC RID: 7932
	public static int soldatiSalvatiInBatt3;

	// Token: 0x04001EFD RID: 7933
	public static int indiceStanzaDiBattaglia;

	// Token: 0x04001EFE RID: 7934
	public static bool mostraResocontoBattaglia;

	// Token: 0x04001EFF RID: 7935
	public static bool mostraResocontoMissione;

	// Token: 0x04001F00 RID: 7936
	public static bool mostraElencoResoconto;

	// Token: 0x04001F01 RID: 7937
	public bool battagliaATavolino;

	// Token: 0x04001F02 RID: 7938
	public bool ricompenseScelte;

	// Token: 0x04001F03 RID: 7939
	public bool ricompenseDecise;

	// Token: 0x04001F04 RID: 7940
	public int premiAssegnati;

	// Token: 0x04001F05 RID: 7941
	public bool assegnaRicompense;

	// Token: 0x04001F06 RID: 7942
	private bool ciSonoAttacchiIrrisolti;

	// Token: 0x04001F07 RID: 7943
	private bool esercitiInMovimento;

	// Token: 0x04001F08 RID: 7944
	private ColorBlock bloccoColorePuls;

	// Token: 0x04001F09 RID: 7945
	public List<int> ListaSatelliti;

	// Token: 0x04001F0A RID: 7946
	public Sprite strisciaVittoria;

	// Token: 0x04001F0B RID: 7947
	public Sprite strisciaSconfitta;

	// Token: 0x04001F0C RID: 7948
	public int missioneDaDecidere;

	// Token: 0x04001F0D RID: 7949
	public int missionePresente;

	// Token: 0x04001F0E RID: 7950
	public int stanzaDiMissione;

	// Token: 0x04001F0F RID: 7951
	public int tipoMissione;

	// Token: 0x04001F10 RID: 7952
	public bool aggMissioneExtra;

	// Token: 0x04001F11 RID: 7953
	public float timerAggMissioneExtra;

	// Token: 0x04001F12 RID: 7954
	public bool partenzaTimerAggMissioneExtra;

	// Token: 0x04001F13 RID: 7955
	public List<List<int>> ListaRicompenseRisorse;

	// Token: 0x04001F14 RID: 7956
	public List<List<int>> ListaRicompenseArmi;

	// Token: 0x04001F15 RID: 7957
	public float ricompensaFreshFoodNemico;

	// Token: 0x04001F16 RID: 7958
	public float ricompensaRottenFoodNemico;

	// Token: 0x04001F17 RID: 7959
	public float ricompensaHighProteinFoodNemico;

	// Token: 0x04001F18 RID: 7960
	public static bool campagnaAppenaIniziata;

	// Token: 0x04001F19 RID: 7961
	public static bool controlloSatellite;

	// Token: 0x04001F1A RID: 7962
	public Color coloreCampVinta;

	// Token: 0x04001F1B RID: 7963
	public Color coloreCampPersa;

	// Token: 0x04001F1C RID: 7964
	private bool campagnaèFinita;

	// Token: 0x04001F1D RID: 7965
	private int vincitoreCampagna;

	// Token: 0x04001F1E RID: 7966
	public Sprite immEsitoCampVittoria;

	// Token: 0x04001F1F RID: 7967
	public Sprite immEsitoCampSconfitta;

	// Token: 0x04001F20 RID: 7968
	public static int stagione;

	// Token: 0x04001F21 RID: 7969
	public bool continuaStagioneCampagna;

	// Token: 0x04001F22 RID: 7970
	public static float valVitaStagionaleNemici;

	// Token: 0x04001F23 RID: 7971
	public static float valAttaccoStagionaleNemici;

	// Token: 0x04001F24 RID: 7972
	public int numAlleatiMortiinTotale;

	// Token: 0x04001F25 RID: 7973
	public int numNemiciMortiinTotale;

	// Token: 0x04001F26 RID: 7974
	private GameObject oggettoMusica;

	// Token: 0x04001F27 RID: 7975
	public static bool aggElencoBattaglia;

	// Token: 0x04001F28 RID: 7976
	public static bool aggElencoMissione;

	// Token: 0x04001F29 RID: 7977
	public static bool ripristinaBarraVert;

	// Token: 0x04001F2A RID: 7978
	private bool fineCampMusicaPartita;

	// Token: 0x04001F2B RID: 7979
	public float totaleExpDaBatt;
}
