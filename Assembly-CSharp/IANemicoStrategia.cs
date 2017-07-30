using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000EC RID: 236
public class IANemicoStrategia : MonoBehaviour
{
	// Token: 0x0600079C RID: 1948 RVA: 0x0010F414 File Offset: 0x0010D614
	private void Start()
	{
		this.CanvasStrategia = GameObject.FindGameObjectWithTag("CanvasStrategia");
		this.Schede = GameObject.FindGameObjectWithTag("Schede");
		this.schedaIntelligence = this.Schede.transform.FindChild("scheda 2").gameObject;
		this.nest = GameObject.FindGameObjectWithTag("Nest");
		this.headquarters = GameObject.FindGameObjectWithTag("Headquarters");
		this.cameraCasa = GameObject.FindGameObjectWithTag("MainCamera");
		this.pulsanteFitPerStrategia = GameObject.FindGameObjectWithTag("PulsFittStrategia");
		this.elendoEserNemico = this.CanvasStrategia.transform.FindChild("Visualizza Esercito Insetti").transform.FindChild("elenco esercito nemico").gameObject;
		this.elendoEserNemicoBarraVert = this.elendoEserNemico.transform.FindChild("Scroll View elenco esercito nemico").GetChild(2).gameObject;
		this.contenutoVisualElencoEserNemico = this.elendoEserNemico.transform.FindChild("Scroll View elenco esercito nemico").GetChild(0).GetChild(0).gameObject;
		this.dettagliEserNemico = this.CanvasStrategia.transform.FindChild("Dettagli Veloci Insetto").gameObject;
		this.scrittaNomeEserIns = this.elendoEserNemico.transform.GetChild(0).gameObject;
		this.sfondoRinominaEserIns = this.elendoEserNemico.transform.FindChild("sfondo rinomina").gameObject;
		this.barraEvoluzione = this.schedaIntelligence.transform.FindChild("barra evoluzione").GetChild(0).GetChild(0).GetChild(0).gameObject;
		this.barraAltaLivNest = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Barra Alta").FindChild("barra nemici").FindChild("livello nest").gameObject;
		this.barraAltaBilancioFreshFood = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Barra Alta").FindChild("barra nemici").FindChild("bilancio fresh food").gameObject;
		this.barraAltaBilancioRottenFood = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Barra Alta").FindChild("barra nemici").FindChild("bilancio rotten food").gameObject;
		this.barraAltaBilancioHighPRoteinFood = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Barra Alta").FindChild("barra nemici").FindChild("bilancio high protein food").gameObject;
		this.bilancioFreshFoodPresenteNumero = this.schedaIntelligence.transform.FindChild("bilancio Fresh Food").FindChild("bilancio Food presente numero").gameObject;
		this.bilancioFreshFoodFuturoNumeri = this.schedaIntelligence.transform.FindChild("bilancio Fresh Food").FindChild("bilancio Food futuro numeri").gameObject;
		this.bilancioRottenFoodPresenteNumero = this.schedaIntelligence.transform.FindChild("bilancio Rotten Food").FindChild("bilancio Food presente numero").gameObject;
		this.bilancioRottenFoodFuturoNumeri = this.schedaIntelligence.transform.FindChild("bilancio Rotten Food").FindChild("bilancio Food futuro numeri").gameObject;
		this.bilancioHighProteinFoodPresenteNumero = this.schedaIntelligence.transform.FindChild("bilancio High Protein Food").FindChild("bilancio Food presente numero").gameObject;
		this.bilancioHighProteinFoodFuturoNumeri = this.schedaIntelligence.transform.FindChild("bilancio High Protein Food").FindChild("bilancio Food futuro numeri").gameObject;
		this.avanzamentoStandardSwarm = this.schedaIntelligence.transform.FindChild("barra Standard Swarm").GetChild(2).gameObject;
		this.barraStandardSwarm = this.schedaIntelligence.transform.FindChild("barra Standard Swarm").GetChild(1).GetChild(0).gameObject;
		this.avanzamentoFormicheSwarm = this.schedaIntelligence.transform.FindChild("barra Formiche Swarm").GetChild(2).gameObject;
		this.barraFormicheSwarm = this.schedaIntelligence.transform.FindChild("barra Formiche Swarm").GetChild(1).GetChild(0).gameObject;
		this.avanzamentoCavalletteSwarm = this.schedaIntelligence.transform.FindChild("barra Cavallette Swarm").GetChild(2).gameObject;
		this.barraCavalletteSwarm = this.schedaIntelligence.transform.FindChild("barra Cavallette Swarm").GetChild(1).GetChild(0).gameObject;
		this.avanzamentoVespaSwarm = this.schedaIntelligence.transform.FindChild("barra Vespe Swarm").GetChild(2).gameObject;
		this.barraVespaSwarm = this.schedaIntelligence.transform.FindChild("barra Vespe Swarm").GetChild(1).GetChild(0).gameObject;
		this.bonusAttivi = this.schedaIntelligence.transform.FindChild("bonus attivi").gameObject;
		this.inizioLivello = GameObject.FindGameObjectWithTag("InizioLivello");
		this.ListaStanzeCopia = new List<GameObject>();
		for (int i = 0; i < this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.Count; i++)
		{
			this.ListaStanzeCopia.Add(this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[i]);
		}
		this.barraAltaLivNest.GetComponent<Text>().text = "NEST lv. " + this.livelloNest;
		this.ListaDeiTipi = new List<List<int>>();
		this.ListaDeiTipi.Add(this.ListaTipo1);
		this.ListaDeiTipi.Add(this.ListaTipo2);
		this.ListaDeiTipi.Add(this.ListaTipo3);
		this.ListaDeiTipi.Add(this.ListaTipo4);
		this.ListaDeiTipi.Add(this.ListaTipo5);
		this.ListaDeiTipi.Add(this.ListaTipo6);
		this.ListaDeiTipi.Add(this.ListaTipo7);
		this.ListaDeiTipi.Add(this.ListaTipo8);
		this.ListaDeiTipi.Add(this.ListaTipo9);
		this.ListaDeiTipi.Add(this.ListaTipo10);
		this.highProteinFoodLimiteMax = 5000f;
		this.quantitàPerScatto = this.highProteinFoodLimiteMax / 10f;
		this.rottenFoodPerNuovoFormicheSwarm = 500f;
		this.rottenFoodPerNuovoCavalletteSwarm = 750f;
		this.rottenFoodPerNuovoVespaSwarm = 1000f;
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x0010FA70 File Offset: 0x0010DC70
	private void Update()
	{
		this.scattoturnoVersoAlleati = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().scattoTurnoVersoAlleati;
		this.scattoturnoVersoNemici = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().scattoTurnoVersoNemici;
		if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().turnoNemicoAttivo)
		{
			this.FunzioniInTurnoNemico();
		}
		else
		{
			this.FunzioniInTurnoAlleato();
		}
		this.CalcoloFood();
		this.BarraAlta();
		if (this.pulsanteFitPerStrategia.GetComponent<PulsFitPerStrategia>().schedaAperta == 1 && this.Schede.GetComponent<CanvasGroup>().alpha == 1f)
		{
			this.VisualIntelligenceUI();
		}
		this.Varie();
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x0010FB18 File Offset: 0x0010DD18
	private void FunzioniInTurnoNemico()
	{
		if (this.creaNuovoStandardSwarm)
		{
			this.creaNuovoStandardSwarm = false;
			this.CreaStandardSwarm();
		}
		if (this.creaNuovoFormicheSwarm)
		{
			this.creaNuovoFormicheSwarm = false;
			this.CreaFormicaSwarm();
		}
		if (this.creaNuovoCavalletteSwarm)
		{
			this.creaNuovoCavalletteSwarm = false;
			this.CreaCavallettaSwarm();
		}
		if (this.creaNuovoVespaSwarm)
		{
			this.creaNuovoVespaSwarm = false;
			this.CreaVespaSwarm();
		}
		if (!this.VittoriaNemica)
		{
			this.MovimentoEsercitiNemici();
		}
		this.VerificaFineTurnoNemico();
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x0010FB9C File Offset: 0x0010DD9C
	private void FunzioniInTurnoAlleato()
	{
		if (this.visualizzaEser)
		{
			this.VisualizEsercitoInsetti();
		}
		if (this.visualizzaDettagli)
		{
			this.VisualizzaDettagliInsetto();
		}
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x0010FBCC File Offset: 0x0010DDCC
	private void CreaStandardSwarm()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(this.esercitoTipo0, this.nest.transform.position + this.nest.transform.forward * 3f, this.nest.transform.rotation) as GameObject;
		gameObject.name = "Bugs Swarm " + this.numerazioneEser;
		gameObject.GetComponent<PresenzaNemicaStrategica>().nomeEserInsetti = "Bugs Swarm " + this.numerazioneEser;
		gameObject.GetComponent<PresenzaNemicaStrategica>().numIdentitàNemico = this.numerazioneEser;
		this.numerazioneEser++;
		gameObject.GetComponent<PresenzaNemicaStrategica>().tipoDiSwarm = 0;
		gameObject.GetComponent<PresenzaNemicaStrategica>().tipoDiInsettoOrda = 100;
		gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser = new List<List<int>>();
		for (int i = 0; i < base.GetComponent<IANemicoStrategia>().ListaTipiInsetti.Count; i++)
		{
			List<int> list = new List<int>();
			list.Add(100);
			list.Add(0);
			gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser.Add(list);
		}
		int j = 15;
		int num = this.livelloNest;
		int num2 = 0;
		while (j > 0)
		{
			for (int k = 0; k < 2; k++)
			{
				bool flag = false;
				int num3 = this.ListaDeiTipi[num - 1][UnityEngine.Random.Range(0, this.ListaDeiTipi[num - 1].Count)];
				int num4 = 0;
				while (num4 < gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser.Count && !flag)
				{
					if (gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num4][0] == num3)
					{
						List<int> list2;
						List<int> expr_1A1 = list2 = gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num4];
						int num5;
						int expr_1A5 = num5 = 1;
						num5 = list2[num5];
						expr_1A1[expr_1A5] = num5 + this.numSpawnGruppiPerEser;
						flag = true;
					}
					else if (gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num4][0] == 100)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num4][0] = num3;
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num4][1] = this.numSpawnGruppiPerEser;
						flag = true;
					}
					num4++;
				}
				j--;
			}
			num--;
			if (num <= 0)
			{
				num = this.livelloNest;
			}
			num2++;
		}
		this.ListaEsercitiNemici.Add(gameObject);
		gameObject.GetComponent<PresenzaNemicaStrategica>().destinazione = this.ListaStanzeCopia[0];
		gameObject.GetComponent<PresenzaNemicaStrategica>().muoviti = true;
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x0010FE84 File Offset: 0x0010E084
	private void CreaFormicaSwarm()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(this.esercitoTipo1, this.nest.transform.position + this.nest.transform.forward * 3f, this.nest.transform.rotation) as GameObject;
		gameObject.name = "Ants Swarm " + this.numerazioneEser;
		gameObject.GetComponent<PresenzaNemicaStrategica>().nomeEserInsetti = "Ants Swarm " + this.numerazioneEser;
		gameObject.GetComponent<PresenzaNemicaStrategica>().numIdentitàNemico = this.numerazioneEser;
		this.numerazioneEser++;
		gameObject.GetComponent<PresenzaNemicaStrategica>().tipoDiSwarm = 1;
		int @int = PlayerPrefs.GetInt("fattore diff spawn gruppi");
		gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser = new List<List<int>>();
		for (int i = 0; i < base.GetComponent<IANemicoStrategia>().ListaTipiInsetti.Count; i++)
		{
			List<int> list = new List<int>();
			list.Add(100);
			list.Add(0);
			gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser.Add(list);
			if (i == 0)
			{
				if (this.livelloNest == 1 || this.livelloNest == 2)
				{
					gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][0] = 0;
					if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().numeroTurno < 15)
					{
						if (@int == 0)
						{
							gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 30;
						}
						else if (@int == 1)
						{
							gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 60;
						}
						else if (@int == 2)
						{
							gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 100;
						}
						else if (@int == 3)
						{
							gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 150;
						}
					}
					else if (@int == 0)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 100;
					}
					else if (@int == 1)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 200;
					}
					else if (@int == 2)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 300;
					}
					else if (@int == 3)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 400;
					}
					gameObject.GetComponent<PresenzaNemicaStrategica>().tipoDiInsettoOrda = 0;
				}
				else if (this.livelloNest == 3 || this.livelloNest == 4)
				{
					gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][0] = 5;
					gameObject.GetComponent<PresenzaNemicaStrategica>().tipoDiInsettoOrda = 5;
					if (@int == 0)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 100;
					}
					else if (@int == 1)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 200;
					}
					else if (@int == 2)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 300;
					}
					else if (@int == 3)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 400;
					}
				}
				else if (this.livelloNest == 5)
				{
					gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][0] = 13;
					gameObject.GetComponent<PresenzaNemicaStrategica>().tipoDiInsettoOrda = 13;
					if (@int == 0)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 100;
					}
					else if (@int == 1)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 200;
					}
					else if (@int == 2)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 300;
					}
					else if (@int == 3)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 400;
					}
				}
				else if (this.livelloNest == 6 || this.livelloNest == 7 || this.livelloNest == 8)
				{
					gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][0] = 16;
					gameObject.GetComponent<PresenzaNemicaStrategica>().tipoDiInsettoOrda = 16;
					if (@int == 0)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 100;
					}
					else if (@int == 1)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 200;
					}
					else if (@int == 2)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 300;
					}
					else if (@int == 3)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 400;
					}
				}
				else if (this.livelloNest == 9 || this.livelloNest == 10)
				{
					gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][0] = 27;
					gameObject.GetComponent<PresenzaNemicaStrategica>().tipoDiInsettoOrda = 27;
					if (@int == 0)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 100;
					}
					else if (@int == 1)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 200;
					}
					else if (@int == 2)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 300;
					}
					else if (@int == 3)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 400;
					}
				}
			}
		}
		this.ListaEsercitiNemici.Add(gameObject);
		gameObject.GetComponent<PresenzaNemicaStrategica>().destinazione = this.ListaStanzeCopia[0];
		gameObject.GetComponent<PresenzaNemicaStrategica>().muoviti = true;
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x001104C8 File Offset: 0x0010E6C8
	private void CreaCavallettaSwarm()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(this.esercitoTipo2, this.nest.transform.position + this.nest.transform.forward * 3f, this.nest.transform.rotation) as GameObject;
		gameObject.name = "Grasshoppers Swarm " + this.numerazioneEser;
		gameObject.GetComponent<PresenzaNemicaStrategica>().nomeEserInsetti = "Grasshoppers Swarm " + this.numerazioneEser;
		gameObject.GetComponent<PresenzaNemicaStrategica>().numIdentitàNemico = this.numerazioneEser;
		this.numerazioneEser++;
		gameObject.GetComponent<PresenzaNemicaStrategica>().tipoDiSwarm = 2;
		int @int = PlayerPrefs.GetInt("fattore diff spawn gruppi");
		gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser = new List<List<int>>();
		for (int i = 0; i < base.GetComponent<IANemicoStrategia>().ListaTipiInsetti.Count; i++)
		{
			List<int> list = new List<int>();
			list.Add(100);
			list.Add(0);
			gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser.Add(list);
			if (i == 0)
			{
				if (this.livelloNest == 4 || this.livelloNest == 5)
				{
					gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][0] = 10;
					gameObject.GetComponent<PresenzaNemicaStrategica>().tipoDiInsettoOrda = 10;
					if (@int == 0)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 30;
					}
					else if (@int == 1)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 60;
					}
					else if (@int == 2)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 100;
					}
					else if (@int == 3)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 150;
					}
				}
				else if (this.livelloNest == 6 || this.livelloNest == 7)
				{
					gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][0] = 15;
					gameObject.GetComponent<PresenzaNemicaStrategica>().tipoDiInsettoOrda = 15;
					if (@int == 0)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 30;
					}
					else if (@int == 1)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 60;
					}
					else if (@int == 2)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 100;
					}
					else if (@int == 3)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 150;
					}
				}
				else if (this.livelloNest == 8 || this.livelloNest == 9 || this.livelloNest == 10)
				{
					gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][0] = 24;
					gameObject.GetComponent<PresenzaNemicaStrategica>().tipoDiInsettoOrda = 24;
					if (@int == 0)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 30;
					}
					else if (@int == 1)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 60;
					}
					else if (@int == 2)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 100;
					}
					else if (@int == 3)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 150;
					}
				}
			}
		}
		this.ListaEsercitiNemici.Add(gameObject);
		gameObject.GetComponent<PresenzaNemicaStrategica>().destinazione = this.ListaStanzeCopia[0];
		gameObject.GetComponent<PresenzaNemicaStrategica>().muoviti = true;
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x001108A8 File Offset: 0x0010EAA8
	private void CreaVespaSwarm()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(this.esercitoTipo3, this.nest.transform.position + this.nest.transform.forward * 3f, this.nest.transform.rotation) as GameObject;
		gameObject.name = "Wasps Swarm " + this.numerazioneEser;
		gameObject.GetComponent<PresenzaNemicaStrategica>().nomeEserInsetti = "Wasps Swarm " + this.numerazioneEser;
		gameObject.GetComponent<PresenzaNemicaStrategica>().numIdentitàNemico = this.numerazioneEser;
		this.numerazioneEser++;
		gameObject.GetComponent<PresenzaNemicaStrategica>().tipoDiSwarm = 3;
		int @int = PlayerPrefs.GetInt("fattore diff spawn gruppi");
		gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser = new List<List<int>>();
		for (int i = 0; i < base.GetComponent<IANemicoStrategia>().ListaTipiInsetti.Count; i++)
		{
			List<int> list = new List<int>();
			list.Add(100);
			list.Add(0);
			gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser.Add(list);
			if (i == 0)
			{
				if (this.livelloNest == 5 || this.livelloNest == 6)
				{
					gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][0] = 12;
					gameObject.GetComponent<PresenzaNemicaStrategica>().tipoDiInsettoOrda = 12;
					if (@int == 0)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 30;
					}
					else if (@int == 1)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 60;
					}
					else if (@int == 2)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 100;
					}
					else if (@int == 3)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 150;
					}
				}
				else if (this.livelloNest == 7 || this.livelloNest == 8)
				{
					gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][0] = 19;
					gameObject.GetComponent<PresenzaNemicaStrategica>().tipoDiInsettoOrda = 19;
					if (@int == 0)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 30;
					}
					else if (@int == 1)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 60;
					}
					else if (@int == 2)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 100;
					}
					else if (@int == 3)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 150;
					}
				}
				else if (this.livelloNest == 9)
				{
					gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][0] = 29;
					gameObject.GetComponent<PresenzaNemicaStrategica>().tipoDiInsettoOrda = 29;
					if (@int == 0)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 30;
					}
					else if (@int == 1)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 60;
					}
					else if (@int == 2)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 100;
					}
					else if (@int == 3)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 150;
					}
				}
				else if (this.livelloNest == 10)
				{
					gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][0] = 30;
					gameObject.GetComponent<PresenzaNemicaStrategica>().tipoDiInsettoOrda = 30;
					if (@int == 0)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 30;
					}
					else if (@int == 1)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 60;
					}
					else if (@int == 2)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 100;
					}
					else if (@int == 3)
					{
						gameObject.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1] = 150;
					}
				}
			}
		}
		this.ListaEsercitiNemici.Add(gameObject);
		gameObject.GetComponent<PresenzaNemicaStrategica>().destinazione = this.ListaStanzeCopia[0];
		gameObject.GetComponent<PresenzaNemicaStrategica>().muoviti = true;
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x00110D38 File Offset: 0x0010EF38
	private void MovimentoEsercitiNemici()
	{
		this.ListaStanzeRichiedonoAzione = new List<GameObject>();
		for (int i = 0; i < this.ListaStanzeCopia.Count; i++)
		{
			if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[i].GetComponent<CentroStanza>().settoriNemici < 3)
			{
				this.ListaStanzeRichiedonoAzione.Add(this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[i]);
			}
		}
		int num = 0;
		List<int> list = new List<int>();
		this.numLimitePerAmmasso = 2f;
		this.ListaQuantitàNemiciNecessari = new List<float>();
		int num2 = 0;
		for (int j = 0; j < this.ListaStanzeRichiedonoAzione.Count; j++)
		{
			this.ListaQuantitàNemiciNecessari.Add((float)this.ListaStanzeRichiedonoAzione[j].GetComponent<CentroStanza>().numTruppeAlleatePres * this.numLimitePerAmmasso);
		}
		for (int k = 0; k < this.ListaEsercitiNemici.Count; k++)
		{
			int num3 = 0;
			for (int l = 0; l < this.ListaEsercitiNemici[k].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser.Count; l++)
			{
				num3 += this.ListaEsercitiNemici[k].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[l][1];
			}
			list.Add(num3);
		}
		if (this.ListaQuantitàNemiciNecessari.Count > 0)
		{
			for (int m = 0; m < this.ListaEsercitiNemici.Count; m++)
			{
				if ((float)(num + list[m]) > this.ListaQuantitàNemiciNecessari[num2])
				{
					this.ListaEsercitiNemici[m].GetComponent<PresenzaNemicaStrategica>().destinazione = this.ListaStanzeRichiedonoAzione[num2];
					this.ListaEsercitiNemici[m].GetComponent<PresenzaNemicaStrategica>().muoviti = true;
					num = 0;
					num2++;
					if (num2 >= this.ListaStanzeRichiedonoAzione.Count)
					{
						num2 = 0;
					}
				}
				else
				{
					num += list[m];
				}
			}
		}
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x00110F54 File Offset: 0x0010F154
	private void VerificaFineTurnoNemico()
	{
		int num = 0;
		for (int i = 0; i < this.ListaEsercitiNemici.Count; i++)
		{
			if (this.ListaEsercitiNemici[i] != null && this.ListaEsercitiNemici[i].GetComponent<PresenzaNemicaStrategica>().prontoPerFineTurnoNemico)
			{
				num++;
			}
		}
		if (num == this.ListaEsercitiNemici.Count)
		{
			this.turnoNemicoPuòFinirePerIAN = true;
		}
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x00110FD0 File Offset: 0x0010F1D0
	private void VisualizEsercitoInsetti()
	{
		if (this.aggiornaEser)
		{
			this.elendoEserNemicoBarraVert.GetComponent<Scrollbar>().value = 1f;
			GameObject esercitoSelezionato = this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato;
			if (this.headquarters.GetComponent<GestioneEsercitiAlleati>().rinominaEser && this.headquarters.GetComponent<GestioneEsercitiAlleati>().tipoEserDaRinominare == 2)
			{
				this.headquarters.GetComponent<GestioneEsercitiAlleati>().rinominaEser = false;
				esercitoSelezionato.GetComponent<PresenzaNemicaStrategica>().nomeEserInsetti = this.sfondoRinominaEserIns.transform.GetChild(0).GetChild(2).GetComponent<Text>().text;
			}
			this.scrittaNomeEserIns.GetComponent<Text>().text = esercitoSelezionato.GetComponent<PresenzaNemicaStrategica>().nomeEserInsetti;
			for (int i = 0; i < base.GetComponent<IANemicoStrategia>().ListaTipiInsetti.Count; i++)
			{
				if (esercitoSelezionato.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[i][0] != 100)
				{
					this.contenutoVisualElencoEserNemico.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 1f;
					this.contenutoVisualElencoEserNemico.transform.GetChild(i).GetComponent<CanvasGroup>().interactable = true;
					this.contenutoVisualElencoEserNemico.transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = true;
					this.contenutoVisualElencoEserNemico.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = this.ListaTipiInsetti[esercitoSelezionato.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[i][0]].GetComponent<PresenzaNemico>().immagineInsetto;
					this.contenutoVisualElencoEserNemico.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = this.ListaTipiInsetti[esercitoSelezionato.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[i][0]].GetComponent<PresenzaNemico>().nomeInsetto;
					this.contenutoVisualElencoEserNemico.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = esercitoSelezionato.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[i][1].ToString() + " G";
				}
				else
				{
					this.contenutoVisualElencoEserNemico.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 0f;
					this.contenutoVisualElencoEserNemico.transform.GetChild(i).GetComponent<CanvasGroup>().interactable = false;
					this.contenutoVisualElencoEserNemico.transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
			}
			this.aggiornaEser = false;
		}
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x00111278 File Offset: 0x0010F478
	private void VisualizzaDettagliInsetto()
	{
		if (this.aggiornaDettagliEser)
		{
			GameObject gameObject = this.ListaTipiInsetti[0];
			if (this.origineDeiDettagli == 0)
			{
				GameObject esercitoSelezionato = this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato;
				gameObject = this.ListaTipiInsetti[esercitoSelezionato.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[this.numElencoPerVisualizInsetto][0]];
			}
			else if (this.origineDeiDettagli == 1)
			{
				int num = 0;
				for (int i = 0; i < this.ListaTipiInsetti.Count; i++)
				{
					if (this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().ListaNemiciInSchermBatt[i][1] != 0)
					{
						if (num == this.numElencoPerVisualizInsetto)
						{
							gameObject = this.ListaTipiInsetti[this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().ListaNemiciInSchermBatt[i][0]];
							break;
						}
						num++;
					}
				}
			}
			else if (this.origineDeiDettagli == 2)
			{
				gameObject = this.ListaTipiInsetti[this.numElencoPerVisualizInsetto];
			}
			this.dettagliEserNemico.transform.GetChild(1).GetComponent<Text>().text = gameObject.GetComponent<PresenzaNemico>().nomeInsetto;
			this.dettagliEserNemico.transform.GetChild(2).GetComponent<Image>().sprite = gameObject.GetComponent<PresenzaNemico>().immagineInsetto;
			float num2 = (1f + this.bonusSurvivalSalute) * ((1f + GestoreNeutroStrategia.valVitaStagionaleNemici / 100f) * (PlayerPrefs.GetFloat("vita nemici") / 100f));
			float num3 = (1f + this.bonusSurvivalAttacco) * ((1f + GestoreNeutroStrategia.valAttaccoStagionaleNemici / 100f) * (PlayerPrefs.GetFloat("attacco nemici") / 100f));
			this.dettagliEserNemico.transform.GetChild(3).GetComponent<Text>().text = string.Concat(new string[]
			{
				"Health:  ",
				(gameObject.GetComponent<PresenzaNemico>().vita * num2).ToString("F0"),
				"\nArmor:  ",
				(gameObject.GetComponent<PresenzaNemico>().armatura * 100f).ToString(),
				"%\nDamage 1:  ",
				(gameObject.GetComponent<PresenzaNemico>().danno1 * num3).ToString("F0"),
				"\nDamage 2:  ",
				(gameObject.GetComponent<PresenzaNemico>().danno2 * num3).ToString("F0"),
				"\nVenom Damage:  ",
				(gameObject.GetComponent<PresenzaNemico>().dannoVeleno * num3).ToString("F0"),
				"\nAttack Rate:  ",
				gameObject.GetComponent<PresenzaNemico>().frequenzaAttacco.ToString(),
				"\nSpeed:  ",
				gameObject.GetComponent<PresenzaNemico>().velocitàInsetto,
				"\nFlying:  ",
				gameObject.GetComponent<PresenzaNemico>().insettoVolante.ToString(),
				"\nJumping:  ",
				gameObject.GetComponent<PresenzaNemico>().èSaltatore.ToString(),
				"\nMembers for group:  ",
				gameObject.GetComponent<PresenzaNemico>().numMembriGruppo.ToString()
			});
			this.dettagliEserNemico.transform.GetChild(4).GetComponent<Text>().text = gameObject.GetComponent<PresenzaNemico>().oggettoDescrizione.GetComponent<Text>().text;
			this.aggiornaDettagliEser = false;
		}
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x00111600 File Offset: 0x0010F800
	private void CalcoloFood()
	{
		int @int = PlayerPrefs.GetInt("fattore diff spawn gruppi");
		if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().numeroTurno < 4)
		{
			this.freshFoodPerNuovoStandardSwarm = 2f;
			if (@int == 0)
			{
				this.numSpawnGruppiPerEser = 1;
			}
			else if (@int == 1)
			{
				this.numSpawnGruppiPerEser = 2;
			}
			else if (@int == 2)
			{
				this.numSpawnGruppiPerEser = 3;
			}
			else if (@int == 3)
			{
				this.numSpawnGruppiPerEser = 4;
			}
		}
		else if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().numeroTurno < 7)
		{
			this.freshFoodPerNuovoStandardSwarm = 30f;
			if (@int == 0)
			{
				this.numSpawnGruppiPerEser = 1;
			}
			else if (@int == 1)
			{
				this.numSpawnGruppiPerEser = 2;
			}
			else if (@int == 2)
			{
				this.numSpawnGruppiPerEser = 3;
			}
			else if (@int == 3)
			{
				this.numSpawnGruppiPerEser = 4;
			}
		}
		else if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().numeroTurno < 12)
		{
			this.freshFoodPerNuovoStandardSwarm = 100f;
			if (@int == 0)
			{
				this.numSpawnGruppiPerEser = 2;
			}
			else if (@int == 1)
			{
				this.numSpawnGruppiPerEser = 3;
			}
			else if (@int == 2)
			{
				this.numSpawnGruppiPerEser = 4;
			}
			else if (@int == 3)
			{
				this.numSpawnGruppiPerEser = 5;
			}
		}
		else if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().numeroTurno < 20)
		{
			this.freshFoodPerNuovoStandardSwarm = 130f;
			if (@int == 0)
			{
				this.numSpawnGruppiPerEser = 3;
			}
			else if (@int == 1)
			{
				this.numSpawnGruppiPerEser = 4;
			}
			else if (@int == 2)
			{
				this.numSpawnGruppiPerEser = 5;
			}
			else if (@int == 3)
			{
				this.numSpawnGruppiPerEser = 6;
			}
		}
		else
		{
			this.freshFoodPerNuovoStandardSwarm = 150f;
			if (@int == 0)
			{
				this.numSpawnGruppiPerEser = 4;
			}
			else if (@int == 1)
			{
				this.numSpawnGruppiPerEser = 5;
			}
			else if (@int == 2)
			{
				this.numSpawnGruppiPerEser = 6;
			}
			else if (@int == 3)
			{
				this.numSpawnGruppiPerEser = 7;
			}
		}
		this.freshFoodInterno = 0f;
		this.rottenFoodInterno = 0f;
		this.highProteinFoodInterno = 0f;
		for (int i = 0; i < this.ListaStanzeCopia.Count; i++)
		{
			this.freshFoodInterno += this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[i].GetComponent<CentroStanza>().guadagnoRealeFreshFood * PlayerPrefs.GetFloat("fattore diff fresh food");
			this.rottenFoodInterno += this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[i].GetComponent<CentroStanza>().guadagnoRealeRottenFood * PlayerPrefs.GetFloat("fattore diff rotten food");
			this.highProteinFoodInterno += this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[i].GetComponent<CentroStanza>().guadagnoRealeHighProteinFood * PlayerPrefs.GetFloat("fattore diff high protein food");
		}
		if (this.scattoturnoVersoAlleati)
		{
			this.freshFoodEsterno = UnityEngine.Random.Range(25f, 35f) * PlayerPrefs.GetFloat("fattore diff fresh food");
			this.rottenFoodEsterno = UnityEngine.Random.Range(10f, 22f) * PlayerPrefs.GetFloat("fattore diff rotten food");
			if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().saltoGiorniPerTurno == 1)
			{
				this.highProteinFoodEsterno = UnityEngine.Random.Range(15f, 25f) * PlayerPrefs.GetFloat("fattore diff hogh protein food");
			}
			else if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().saltoGiorniPerTurno == 2)
			{
				this.highProteinFoodEsterno = UnityEngine.Random.Range(55f, 70f) * PlayerPrefs.GetFloat("fattore diff high protein food");
			}
			else if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().saltoGiorniPerTurno == 3)
			{
				this.highProteinFoodEsterno = UnityEngine.Random.Range(85f, 100f) * PlayerPrefs.GetFloat("fattore diff high protein food");
			}
			this.freshFoodBattOMiss = 0f;
			this.rottenFoodBattOMiss = 0f;
			this.highProteinFoodBattOMiss = 0f;
		}
		else if (this.scattoturnoVersoNemici)
		{
			if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().missionePresente == 1)
			{
				this.freshFoodBattOMiss += 20f;
				this.rottenFoodBattOMiss += 10f;
				this.highProteinFoodBattOMiss += 10f;
			}
			this.freshFoodPresente += this.freshFoodFuturo;
			this.rottenFoodPresenteFormicheSwarm += this.rottenFoodFuturo;
			if (this.livelloNest >= 4)
			{
				this.rottenFoodPresenteCavalletteSwarm += this.rottenFoodFuturo;
			}
			if (this.livelloNest >= 5)
			{
				this.rottenFoodPresenteVespaSwarm += this.rottenFoodFuturo;
			}
			this.highProteinFoodPresente += this.highProteinFoodFuturo;
			if (this.freshFoodPresente >= this.freshFoodPerNuovoStandardSwarm)
			{
				this.freshFoodPresente -= this.freshFoodPerNuovoStandardSwarm;
				this.creaNuovoStandardSwarm = true;
			}
			if (this.rottenFoodPresenteFormicheSwarm >= this.rottenFoodPerNuovoFormicheSwarm)
			{
				this.rottenFoodPresenteFormicheSwarm -= this.rottenFoodPerNuovoFormicheSwarm;
				this.creaNuovoFormicheSwarm = true;
			}
			if (this.rottenFoodPresenteCavalletteSwarm >= this.rottenFoodPerNuovoCavalletteSwarm)
			{
				this.rottenFoodPresenteCavalletteSwarm -= this.rottenFoodPerNuovoCavalletteSwarm;
				this.creaNuovoCavalletteSwarm = true;
			}
			if (this.rottenFoodPresenteVespaSwarm >= this.rottenFoodPerNuovoVespaSwarm)
			{
				this.rottenFoodPresenteVespaSwarm -= this.rottenFoodPerNuovoVespaSwarm;
				this.creaNuovoVespaSwarm = true;
			}
			if (this.highProteinFoodPresente > this.quantitàPerScatto * 9f)
			{
				this.livelloNest = 10;
			}
			else
			{
				float num = this.highProteinFoodPresente / this.quantitàPerScatto;
				for (int j = 0; j < 10; j++)
				{
					if (num > (float)j && num < (float)(j + 1))
					{
						this.livelloNest = j + 1;
						break;
					}
				}
			}
		}
		this.freshFoodFuturo = this.freshFoodEsterno + this.freshFoodInterno + this.freshFoodBattOMiss;
		this.rottenFoodFuturo = this.rottenFoodEsterno + this.rottenFoodInterno + this.rottenFoodBattOMiss;
		this.highProteinFoodFuturo = this.highProteinFoodEsterno + this.highProteinFoodInterno + this.highProteinFoodBattOMiss;
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x00111C4C File Offset: 0x0010FE4C
	private void VisualIntelligenceUI()
	{
		if (GestoreNeutroStrategia.valVitaStagionaleNemici > 0f || GestoreNeutroStrategia.valAttaccoStagionaleNemici > 0f || this.bonusSurvivalSalute > 0f || this.bonusSurvivalAttacco > 0f)
		{
			this.bonusAttivi.GetComponent<Image>().color = this.coloreBonusAttivi;
		}
		else if (GestoreNeutroStrategia.valVitaStagionaleNemici == 0f && GestoreNeutroStrategia.valAttaccoStagionaleNemici == 0f && this.bonusSurvivalSalute == 0f && this.bonusSurvivalAttacco == 0f)
		{
			this.bonusAttivi.GetComponent<Image>().color = this.coloreBonusDisattivi;
		}
		this.bonusAttivi.transform.GetChild(1).GetComponent<Text>().text = "Season Enemies Health:   +" + GestoreNeutroStrategia.valVitaStagionaleNemici + "%";
		this.bonusAttivi.transform.GetChild(2).GetComponent<Text>().text = "Season Enemies Attacks:   +" + GestoreNeutroStrategia.valAttaccoStagionaleNemici + "%";
		this.bonusAttivi.transform.GetChild(3).GetComponent<Text>().text = "Survival Enemies Health:   +" + (this.bonusSurvivalSalute * 100f).ToString("F0") + "%";
		this.bonusAttivi.transform.GetChild(4).GetComponent<Text>().text = "Survival Enemies Attacks:   +" + (this.bonusSurvivalAttacco * 100f).ToString("F0") + "%";
		this.bilancioFreshFoodPresenteNumero.GetComponent<Text>().text = this.freshFoodPresente.ToString("F1");
		this.bilancioFreshFoodFuturoNumeri.GetComponent<Text>().text = string.Concat(new string[]
		{
			this.freshFoodInterno.ToString("F1"),
			"\n\n",
			this.freshFoodEsterno.ToString("F1"),
			"\n\n",
			this.freshFoodBattOMiss.ToString("F1"),
			"\n\n",
			(this.freshFoodInterno + this.freshFoodEsterno + this.freshFoodBattOMiss).ToString("F1")
		});
		this.bilancioRottenFoodPresenteNumero.GetComponent<Text>().text = "-";
		this.bilancioRottenFoodFuturoNumeri.GetComponent<Text>().text = string.Concat(new string[]
		{
			this.rottenFoodInterno.ToString("F1"),
			"\n\n",
			this.rottenFoodEsterno.ToString("F1"),
			"\n\n",
			this.rottenFoodBattOMiss.ToString("F1"),
			"\n\n",
			(this.rottenFoodInterno + this.rottenFoodEsterno + this.rottenFoodBattOMiss).ToString("F1")
		});
		this.bilancioHighProteinFoodPresenteNumero.GetComponent<Text>().text = this.highProteinFoodPresente.ToString("F1");
		this.bilancioHighProteinFoodFuturoNumeri.GetComponent<Text>().text = string.Concat(new string[]
		{
			this.highProteinFoodInterno.ToString("F1"),
			"\n\n",
			this.highProteinFoodEsterno.ToString("F1"),
			"\n\n",
			this.highProteinFoodBattOMiss.ToString("F1"),
			"\n\n",
			(this.highProteinFoodInterno + this.highProteinFoodEsterno + this.highProteinFoodBattOMiss).ToString("F1")
		});
		this.barraEvoluzione.GetComponent<Image>().fillAmount = this.highProteinFoodPresente / this.highProteinFoodLimiteMax;
		this.barraStandardSwarm.GetComponent<Image>().fillAmount = this.freshFoodPresente / this.freshFoodPerNuovoStandardSwarm;
		this.avanzamentoStandardSwarm.GetComponent<Text>().text = this.freshFoodPresente.ToString("F1") + " / " + this.freshFoodPerNuovoStandardSwarm.ToString("F1");
		this.barraFormicheSwarm.GetComponent<Image>().fillAmount = this.rottenFoodPresenteFormicheSwarm / this.rottenFoodPerNuovoFormicheSwarm;
		this.avanzamentoFormicheSwarm.GetComponent<Text>().text = this.rottenFoodPresenteFormicheSwarm.ToString("F1") + " / " + this.rottenFoodPerNuovoFormicheSwarm.ToString("F1");
		this.barraCavalletteSwarm.GetComponent<Image>().fillAmount = this.rottenFoodPresenteCavalletteSwarm / this.rottenFoodPerNuovoCavalletteSwarm;
		this.avanzamentoCavalletteSwarm.GetComponent<Text>().text = this.rottenFoodPresenteCavalletteSwarm.ToString("F1") + " / " + this.rottenFoodPerNuovoCavalletteSwarm.ToString("F1");
		this.barraVespaSwarm.GetComponent<Image>().fillAmount = this.rottenFoodPresenteVespaSwarm / this.rottenFoodPerNuovoVespaSwarm;
		this.avanzamentoVespaSwarm.GetComponent<Text>().text = this.rottenFoodPresenteVespaSwarm.ToString("F1") + " / " + this.rottenFoodPerNuovoVespaSwarm.ToString("F1");
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x00112174 File Offset: 0x00110374
	private void BarraAlta()
	{
		this.barraAltaLivNest.GetComponent<Text>().text = "NEST lv. " + this.livelloNest;
		this.barraAltaBilancioFreshFood.GetComponent<Text>().text = string.Concat(new string[]
		{
			"FF: ",
			this.freshFoodPresente.ToString("F0"),
			"(+",
			this.freshFoodFuturo.ToString("F0"),
			") / ",
			this.freshFoodPerNuovoStandardSwarm.ToString()
		});
		this.barraAltaBilancioRottenFood.GetComponent<Text>().text = "RF: (+" + this.rottenFoodFuturo.ToString("F0") + ")";
		this.barraAltaBilancioHighPRoteinFood.GetComponent<Text>().text = string.Concat(new string[]
		{
			"HPF: ",
			this.highProteinFoodPresente.ToString("F0"),
			"(+",
			this.highProteinFoodFuturo.ToString("F0"),
			") / ",
			this.highProteinFoodLimiteMax.ToString()
		});
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x001122A4 File Offset: 0x001104A4
	private void Varie()
	{
		int num = 0;
		for (int i = 0; i < this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.Count; i++)
		{
			if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[i].GetComponent<CentroStanza>().appartenenzaBandiera == 2)
			{
				num++;
			}
		}
		if (num == 3)
		{
			this.bonusSurvivalSalute = 0.3f;
			this.bonusSurvivalAttacco = 0.1f;
		}
		else if (num == 2)
		{
			this.bonusSurvivalSalute = 0.6f;
			this.bonusSurvivalAttacco = 0.2f;
		}
		else if (num == 1 || num == 0)
		{
			this.bonusSurvivalSalute = 1f;
			this.bonusSurvivalAttacco = 0.3f;
		}
		else
		{
			this.bonusSurvivalSalute = 0f;
			this.bonusSurvivalAttacco = 0f;
		}
		if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[0].GetComponent<CentroStanza>().numTruppeAlleatePres > 0)
		{
			this.bonusSurvivalSalute = 1f;
			this.bonusSurvivalAttacco = 0.3f;
		}
	}

	// Token: 0x04001C50 RID: 7248
	public List<GameObject> ListaTipiInsetti;

	// Token: 0x04001C51 RID: 7249
	private GameObject CanvasStrategia;

	// Token: 0x04001C52 RID: 7250
	private GameObject Schede;

	// Token: 0x04001C53 RID: 7251
	private GameObject schedaIntelligence;

	// Token: 0x04001C54 RID: 7252
	private GameObject nest;

	// Token: 0x04001C55 RID: 7253
	private GameObject headquarters;

	// Token: 0x04001C56 RID: 7254
	private GameObject cameraCasa;

	// Token: 0x04001C57 RID: 7255
	private GameObject pulsanteFitPerStrategia;

	// Token: 0x04001C58 RID: 7256
	private GameObject elencoNemiciInEser;

	// Token: 0x04001C59 RID: 7257
	private GameObject elendoEserNemico;

	// Token: 0x04001C5A RID: 7258
	private GameObject elendoEserNemicoBarraVert;

	// Token: 0x04001C5B RID: 7259
	private GameObject contenutoVisualElencoEserNemico;

	// Token: 0x04001C5C RID: 7260
	private GameObject dettagliEserNemico;

	// Token: 0x04001C5D RID: 7261
	private GameObject scrittaNomeEserIns;

	// Token: 0x04001C5E RID: 7262
	private GameObject sfondoRinominaEserIns;

	// Token: 0x04001C5F RID: 7263
	private GameObject bilancioFreshFoodPresenteNumero;

	// Token: 0x04001C60 RID: 7264
	private GameObject bilancioFreshFoodFuturoNumeri;

	// Token: 0x04001C61 RID: 7265
	private GameObject bilancioRottenFoodPresenteNumero;

	// Token: 0x04001C62 RID: 7266
	private GameObject bilancioRottenFoodFuturoNumeri;

	// Token: 0x04001C63 RID: 7267
	private GameObject bilancioHighProteinFoodPresenteNumero;

	// Token: 0x04001C64 RID: 7268
	private GameObject bilancioHighProteinFoodFuturoNumeri;

	// Token: 0x04001C65 RID: 7269
	private GameObject avanzamentoStandardSwarm;

	// Token: 0x04001C66 RID: 7270
	private GameObject barraStandardSwarm;

	// Token: 0x04001C67 RID: 7271
	private GameObject avanzamentoFormicheSwarm;

	// Token: 0x04001C68 RID: 7272
	private GameObject barraFormicheSwarm;

	// Token: 0x04001C69 RID: 7273
	private GameObject avanzamentoCavalletteSwarm;

	// Token: 0x04001C6A RID: 7274
	private GameObject barraCavalletteSwarm;

	// Token: 0x04001C6B RID: 7275
	private GameObject avanzamentoVespaSwarm;

	// Token: 0x04001C6C RID: 7276
	private GameObject barraVespaSwarm;

	// Token: 0x04001C6D RID: 7277
	private GameObject barraEvoluzione;

	// Token: 0x04001C6E RID: 7278
	private GameObject barraAltaLivNest;

	// Token: 0x04001C6F RID: 7279
	private GameObject barraAltaBilancioFreshFood;

	// Token: 0x04001C70 RID: 7280
	private GameObject barraAltaBilancioRottenFood;

	// Token: 0x04001C71 RID: 7281
	private GameObject barraAltaBilancioHighPRoteinFood;

	// Token: 0x04001C72 RID: 7282
	private GameObject bonusAttivi;

	// Token: 0x04001C73 RID: 7283
	private GameObject inizioLivello;

	// Token: 0x04001C74 RID: 7284
	public List<int> ListaTipo1;

	// Token: 0x04001C75 RID: 7285
	public List<int> ListaTipo2;

	// Token: 0x04001C76 RID: 7286
	public List<int> ListaTipo3;

	// Token: 0x04001C77 RID: 7287
	public List<int> ListaTipo4;

	// Token: 0x04001C78 RID: 7288
	public List<int> ListaTipo5;

	// Token: 0x04001C79 RID: 7289
	public List<int> ListaTipo6;

	// Token: 0x04001C7A RID: 7290
	public List<int> ListaTipo7;

	// Token: 0x04001C7B RID: 7291
	public List<int> ListaTipo8;

	// Token: 0x04001C7C RID: 7292
	public List<int> ListaTipo9;

	// Token: 0x04001C7D RID: 7293
	public List<int> ListaTipo10;

	// Token: 0x04001C7E RID: 7294
	private List<List<int>> ListaDeiTipi;

	// Token: 0x04001C7F RID: 7295
	public float freshFoodEsterno;

	// Token: 0x04001C80 RID: 7296
	public float freshFoodInterno;

	// Token: 0x04001C81 RID: 7297
	public float freshFoodBattOMiss;

	// Token: 0x04001C82 RID: 7298
	public float freshFoodPresente;

	// Token: 0x04001C83 RID: 7299
	public float freshFoodFuturo;

	// Token: 0x04001C84 RID: 7300
	public float rottenFoodEsterno;

	// Token: 0x04001C85 RID: 7301
	public float rottenFoodInterno;

	// Token: 0x04001C86 RID: 7302
	public float rottenFoodBattOMiss;

	// Token: 0x04001C87 RID: 7303
	public float rottenFoodPresenteFormicheSwarm;

	// Token: 0x04001C88 RID: 7304
	public float rottenFoodPresenteCavalletteSwarm;

	// Token: 0x04001C89 RID: 7305
	public float rottenFoodPresenteVespaSwarm;

	// Token: 0x04001C8A RID: 7306
	public float rottenFoodFuturo;

	// Token: 0x04001C8B RID: 7307
	public float highProteinFoodEsterno;

	// Token: 0x04001C8C RID: 7308
	public float highProteinFoodInterno;

	// Token: 0x04001C8D RID: 7309
	public float highProteinFoodBattOMiss;

	// Token: 0x04001C8E RID: 7310
	public float highProteinFoodPresente;

	// Token: 0x04001C8F RID: 7311
	public float highProteinFoodFuturo;

	// Token: 0x04001C90 RID: 7312
	private float highProteinFoodLimiteMax;

	// Token: 0x04001C91 RID: 7313
	private float freshFoodPerNuovoStandardSwarm;

	// Token: 0x04001C92 RID: 7314
	private float rottenFoodPerNuovoFormicheSwarm;

	// Token: 0x04001C93 RID: 7315
	private float rottenFoodPerNuovoCavalletteSwarm;

	// Token: 0x04001C94 RID: 7316
	private float rottenFoodPerNuovoVespaSwarm;

	// Token: 0x04001C95 RID: 7317
	private bool creaNuovoStandardSwarm;

	// Token: 0x04001C96 RID: 7318
	private int numStandardSwarmDaCreare;

	// Token: 0x04001C97 RID: 7319
	private int numSpawnGruppiPerEser;

	// Token: 0x04001C98 RID: 7320
	public int numerazioneEser;

	// Token: 0x04001C99 RID: 7321
	private bool creaNuovoFormicheSwarm;

	// Token: 0x04001C9A RID: 7322
	private bool creaNuovoCavalletteSwarm;

	// Token: 0x04001C9B RID: 7323
	private bool creaNuovoVespaSwarm;

	// Token: 0x04001C9C RID: 7324
	public int livelloNest;

	// Token: 0x04001C9D RID: 7325
	public GameObject esercitoTipo0;

	// Token: 0x04001C9E RID: 7326
	public GameObject esercitoTipo1;

	// Token: 0x04001C9F RID: 7327
	public GameObject esercitoTipo2;

	// Token: 0x04001CA0 RID: 7328
	public GameObject esercitoTipo3;

	// Token: 0x04001CA1 RID: 7329
	public List<GameObject> ListaEsercitiNemici;

	// Token: 0x04001CA2 RID: 7330
	private List<GameObject> ListaStanzeCopia;

	// Token: 0x04001CA3 RID: 7331
	public List<GameObject> ListaStanzeNemSicure;

	// Token: 0x04001CA4 RID: 7332
	public List<GameObject> ListaStanzeNemBorderInterne;

	// Token: 0x04001CA5 RID: 7333
	public List<GameObject> ListaStanzeNemBorderEsterne;

	// Token: 0x04001CA6 RID: 7334
	public List<GameObject> ListaStanzeNemBorIntPiùVuote;

	// Token: 0x04001CA7 RID: 7335
	public bool calcoloObbiettiEff;

	// Token: 0x04001CA8 RID: 7336
	public bool movimentiOrdinati;

	// Token: 0x04001CA9 RID: 7337
	private List<int> ListaSettoriPerStanza;

	// Token: 0x04001CAA RID: 7338
	public bool turnoNemicoPuòFinirePerIAN;

	// Token: 0x04001CAB RID: 7339
	public bool VittoriaNemica;

	// Token: 0x04001CAC RID: 7340
	private bool esplorazioneInizTerminata;

	// Token: 0x04001CAD RID: 7341
	public bool visualizzaEser;

	// Token: 0x04001CAE RID: 7342
	public bool visualizzaDettagli;

	// Token: 0x04001CAF RID: 7343
	public bool aggiornaEser;

	// Token: 0x04001CB0 RID: 7344
	public bool aggiornaDettagliEser;

	// Token: 0x04001CB1 RID: 7345
	public int numElencoPerVisualizInsetto;

	// Token: 0x04001CB2 RID: 7346
	public int origineDeiDettagli;

	// Token: 0x04001CB3 RID: 7347
	private List<List<GameObject>> ListaComportamentiInsetti;

	// Token: 0x04001CB4 RID: 7348
	private List<GameObject> ListaComportamentoGruppo0;

	// Token: 0x04001CB5 RID: 7349
	private List<GameObject> ListaComportamentoGruppo1;

	// Token: 0x04001CB6 RID: 7350
	private List<GameObject> ListaComportamentoGruppo2;

	// Token: 0x04001CB7 RID: 7351
	private List<GameObject> ListaComportamentoGruppo3;

	// Token: 0x04001CB8 RID: 7352
	private List<int> ListaTipiPresentiInVisualEserNemico;

	// Token: 0x04001CB9 RID: 7353
	private float numLimitePerAmmasso;

	// Token: 0x04001CBA RID: 7354
	private bool scattoturnoVersoAlleati;

	// Token: 0x04001CBB RID: 7355
	private bool scattoturnoVersoNemici;

	// Token: 0x04001CBC RID: 7356
	private float quantitàPerScatto;

	// Token: 0x04001CBD RID: 7357
	public List<GameObject> ListaStanzeRichiedonoAzione;

	// Token: 0x04001CBE RID: 7358
	public List<float> ListaQuantitàNemiciNecessari;

	// Token: 0x04001CBF RID: 7359
	public float bonusSurvivalSalute;

	// Token: 0x04001CC0 RID: 7360
	public float bonusSurvivalAttacco;

	// Token: 0x04001CC1 RID: 7361
	public Color coloreBonusDisattivi;

	// Token: 0x04001CC2 RID: 7362
	public Color coloreBonusAttivi;
}
