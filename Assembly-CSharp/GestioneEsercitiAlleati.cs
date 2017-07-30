using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000006 RID: 6
public class GestioneEsercitiAlleati : MonoBehaviour
{
	// Token: 0x06000027 RID: 39 RVA: 0x00009608 File Offset: 0x00007808
	private void Start()
	{
		this.ListaReclutamento = new List<int>();
		this.CanvasStrategia = GameObject.FindGameObjectWithTag("CanvasStrategia");
		this.cameraCasa = GameObject.FindGameObjectWithTag("MainCamera");
		this.PulsanteFittStrategia = GameObject.FindGameObjectWithTag("PulsFittStrategia");
		this.rettEsercitoInRecl = GameObject.FindGameObjectWithTag("Schede").transform.FindChild("scheda 3").transform.FindChild("rettangolo esercito").gameObject;
		this.pulsanteNuovoRecl = GameObject.FindGameObjectWithTag("Schede").transform.FindChild("scheda 3").transform.FindChild("pulsante reclutamento").gameObject;
		this.pulsanteResettaRecl = GameObject.FindGameObjectWithTag("Schede").transform.FindChild("scheda 3").transform.FindChild("pulsante resetta").gameObject;
		this.pulsanteCreaEser = GameObject.FindGameObjectWithTag("Schede").transform.FindChild("scheda 3").transform.FindChild("pulsante crea").gameObject;
		this.elencoPerReclutare = GameObject.FindGameObjectWithTag("Schede").transform.FindChild("scheda 3").transform.FindChild("elenco per reclutare").gameObject;
		this.dettagliUnitàRecl = GameObject.FindGameObjectWithTag("Schede").transform.FindChild("scheda 3").transform.FindChild("dettagli unità").gameObject;
		this.elencoEser = this.CanvasStrategia.transform.FindChild("Visualizza Esercito").transform.FindChild("elenco esercito").gameObject;
		this.elencoEserBarraVerticale = this.elencoEser.transform.FindChild("Scroll View elenco esercito").GetChild(2).gameObject;
		this.elencoSecondoEser = this.CanvasStrategia.transform.FindChild("Visualizza Esercito").transform.FindChild("elenco secondo esercito").gameObject;
		this.elencoSecondoEserBarraVerticale = this.elencoSecondoEser.transform.FindChild("Scroll View elenco esercito").GetChild(2).gameObject;
		this.contenutoVisualElencoEser = this.elencoEser.transform.FindChild("Scroll View elenco esercito").GetChild(0).GetChild(0).gameObject;
		this.contenutoVisualElencoSecondoEser = this.elencoSecondoEser.transform.FindChild("Scroll View elenco esercito").GetChild(0).GetChild(0).gameObject;
		this.dettagliUnitàInEser = this.CanvasStrategia.transform.FindChild("Dettagli Veloci Unità").gameObject;
		this.pulsanteArmi = this.dettagliUnitàInEser.transform.FindChild("Armi").gameObject;
		this.pulsanteArmiRecl = this.dettagliUnitàRecl.transform.FindChild("pulsante armi").gameObject;
		this.Armi = this.CanvasStrategia.transform.FindChild("Armi").gameObject;
		this.pulsanteCongedo = this.dettagliUnitàInEser.transform.FindChild("Congeda unità").gameObject;
		this.elencoAlleatiInSchermBatt = this.CanvasStrategia.transform.FindChild("Schermata Per Battaglia").FindChild("elenco alleati").GetChild(0).gameObject;
		this.scrittaNonCiSonoRisorse = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Scritte Varie").FindChild("sfondo scritta non hai risorse").gameObject;
		this.scrittaNomeEser = this.elencoEser.transform.GetChild(0).gameObject;
		this.scrittaNomeEserSecondo = this.elencoSecondoEser.transform.GetChild(0).gameObject;
		this.sfondoRinominaEser = this.elencoEser.transform.FindChild("sfondo rinomina").gameObject;
		this.sfondoRinominaEserSecondo = this.elencoSecondoEser.transform.FindChild("sfondo rinomina").gameObject;
		this.riassuntoEsercito = this.CanvasStrategia.transform.FindChild("Varie").FindChild("riassunto esercito").gameObject;
		this.tipoTruppaSelez = 100;
		this.ListaTimerTipoTruppa = new List<float>();
		this.ListaTimerTipoTruppa.Add(this.timerDiTipoTruppaSelez);
		this.ListaTimerTipoTruppa.Add(this.timerDiPosizioneInListaRecl);
		this.ListaTimerTipoTruppa.Add(this.timerDiNumPosInEser);
		this.ListaTimerTipoTruppa.Add(this.timerDiNumPosInSchermBatt);
		this.ListaMatricePulsantiArmi = new List<int>();
		this.ListaMatricePulsantiArmi.Add(0);
		this.ListaMatricePulsantiArmi.Add(0);
		this.ListaMatricePulsantiArmi.Add(0);
		this.ListaMatricePulsantiArmi.Add(0);
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00009AB8 File Offset: 0x00007CB8
	private void Update()
	{
		if (this.PulsanteFittStrategia.GetComponent<PulsFitPerStrategia>().schedaAperta == 2)
		{
			if (this.reclutamentoAttivo)
			{
				this.pulsanteNuovoRecl.GetComponent<CanvasGroup>().interactable = false;
				this.pulsanteResettaRecl.GetComponent<CanvasGroup>().interactable = true;
				this.rettEsercitoInRecl.GetComponent<CanvasGroup>().interactable = true;
				this.elencoPerReclutare.GetComponent<CanvasGroup>().interactable = true;
				if (this.ListaReclutamento.Count > 0 && this.ListaReclutamento[0] != 100)
				{
					this.pulsanteCreaEser.GetComponent<CanvasGroup>().interactable = true;
				}
				else
				{
					this.pulsanteCreaEser.GetComponent<CanvasGroup>().interactable = false;
				}
				if (this.tipoTruppaSelez != 100)
				{
					this.dettagliUnitàRecl.GetComponent<CanvasGroup>().interactable = true;
					this.dettagliUnitàRecl.GetComponent<CanvasGroup>().alpha = 1f;
				}
				this.Reclutamento();
			}
			else
			{
				this.pulsanteNuovoRecl.GetComponent<CanvasGroup>().interactable = true;
				this.pulsanteResettaRecl.GetComponent<CanvasGroup>().interactable = false;
				this.pulsanteCreaEser.GetComponent<CanvasGroup>().interactable = false;
				this.rettEsercitoInRecl.GetComponent<CanvasGroup>().interactable = false;
				this.elencoPerReclutare.GetComponent<CanvasGroup>().interactable = false;
				this.dettagliUnitàRecl.GetComponent<CanvasGroup>().interactable = false;
				this.dettagliUnitàRecl.GetComponent<CanvasGroup>().alpha = 0f;
			}
		}
		if (this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato != null && this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato.tag == "Alleato")
		{
			this.RiassuntoVeloceEsercito();
			this.riassuntoEsercito.GetComponent<CanvasGroup>().alpha = 1f;
		}
		else
		{
			this.riassuntoEsercito.GetComponent<CanvasGroup>().alpha = 0f;
		}
		if (this.visualizzaEser)
		{
			this.VisualizzaEsercito();
		}
		if (this.scambioFraEserciti)
		{
			this.FunzioneScambioFraEserciti();
			this.visualizzaDettagli = false;
		}
		if (this.visualizzaDettagli)
		{
			this.VisualizzaDettagliEsercito();
		}
		if (this.armiAperto)
		{
			this.VisualizzaArmi();
		}
		if (this.controlloEserVuoti)
		{
			this.controlloEserVuoti = false;
			this.EliminazioneEserVuoti();
		}
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00009D08 File Offset: 0x00007F08
	private void Reclutamento()
	{
		if (this.resettaListaRecl)
		{
			this.resettaListaRecl = false;
			for (int i = 0; i < this.numUnitàInUnEser; i++)
			{
				if (this.ListaReclutamento.Count > 0 && this.ListaReclutamento[i] != 100)
				{
					for (int j = 0; j < this.ListaUnitàAlleate.Count; j++)
					{
						if (this.ListaUnitàAlleate[j].GetComponent<PresenzaAlleato>().tipoTruppa == this.ListaReclutamento[i])
						{
							this.unitàConsiderata = this.ListaUnitàAlleate[j];
							base.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[1].GetComponent<PresenzaRisorsa>().quantitàRisorsa += this.unitàConsiderata.GetComponent<PresenzaAlleato>().costoInPlastica;
						}
					}
				}
			}
			this.ListaReclutamento.Clear();
			for (int k = 0; k < this.numUnitàInUnEser; k++)
			{
				this.ListaReclutamento.Add(100);
				this.rettEsercitoInRecl.transform.GetChild(k).GetComponent<CanvasGroup>().alpha = 0f;
				this.rettEsercitoInRecl.transform.GetChild(k).GetComponent<CanvasGroup>().interactable = false;
				this.rettEsercitoInRecl.transform.GetChild(k).GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
		}
		if (this.sblocchiAggiornati)
		{
			this.sblocchiAggiornati = false;
			for (int l = 0; l < 4; l++)
			{
				if (l == this.tipoElenco)
				{
					for (int m = 0; m < this.elencoPerReclutare.transform.GetChild(l).childCount; m++)
					{
						if (base.GetComponent<GestioneSblocchi>().ListaSblocchi[int.Parse(this.elencoPerReclutare.transform.GetChild(l).GetChild(m).name)].GetComponent<PresenzaSblocco>().èSbloccato == 1)
						{
							this.elencoPerReclutare.transform.GetChild(l).GetChild(m).GetComponent<Button>().interactable = true;
						}
						else
						{
							this.elencoPerReclutare.transform.GetChild(l).GetChild(m).GetComponent<Button>().interactable = false;
						}
					}
				}
			}
		}
		if (this.aggiungiOTogliAttivo)
		{
			if (!this.giàReclutato)
			{
				if (this.ListaReclutamento[29] == 100)
				{
					for (int n = 0; n < this.ListaUnitàAlleate.Count; n++)
					{
						if (this.ListaUnitàAlleate[n].GetComponent<PresenzaAlleato>().tipoTruppa == this.tipoTruppaSelez)
						{
							this.unitàConsiderata = this.ListaUnitàAlleate[n];
						}
					}
					if (this.unitàConsiderata != null)
					{
						float num = base.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[1].GetComponent<PresenzaRisorsa>().quantitàRisorsa - this.unitàConsiderata.GetComponent<PresenzaAlleato>().costoInPlastica;
						if (num >= 0f)
						{
							base.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[1].GetComponent<PresenzaRisorsa>().quantitàRisorsa -= this.unitàConsiderata.GetComponent<PresenzaAlleato>().costoInPlastica;
							for (int num2 = 0; num2 < this.ListaReclutamento.Count; num2++)
							{
								if (this.ListaReclutamento[num2] == 100)
								{
									this.ListaReclutamento[num2] = this.tipoTruppaSelez;
									break;
								}
							}
							this.aggiungiOTogliAttivo = false;
						}
						else
						{
							this.scrittaNonCiSonoRisorse.GetComponent<CanvasGroup>().alpha = 1f;
							this.aggiungiOTogliAttivo = false;
						}
					}
				}
				else
				{
					this.aggiungiOTogliAttivo = false;
				}
			}
			else
			{
				if (this.ListaReclutamento[this.posizioneInListaRecl] != 100)
				{
					for (int num3 = 0; num3 < this.ListaUnitàAlleate.Count; num3++)
					{
						if (this.ListaUnitàAlleate[num3].GetComponent<PresenzaAlleato>().tipoTruppa == this.ListaReclutamento[this.posizioneInListaRecl])
						{
							this.unitàConsiderata = this.ListaUnitàAlleate[num3];
						}
					}
					base.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[1].GetComponent<PresenzaRisorsa>().quantitàRisorsa += this.unitàConsiderata.GetComponent<PresenzaAlleato>().costoInPlastica;
				}
				this.ListaReclutamento[this.posizioneInListaRecl] = 100;
				bool flag = false;
				int num4 = 0;
				while (num4 < this.numUnitàInUnEser && !flag)
				{
					if (this.ListaReclutamento[num4] == 100)
					{
						bool flag2 = false;
						int num5 = num4 + 1;
						while (num5 < this.numUnitàInUnEser && !flag2)
						{
							if (num5 == 29 && this.ListaReclutamento[29] == 100)
							{
								flag = true;
							}
							else
							{
								this.ListaReclutamento[num4] = this.ListaReclutamento[num5];
								this.ListaReclutamento[num5] = 100;
								flag2 = true;
							}
							num5++;
						}
					}
					this.aggiornaDettagliRecl = true;
					this.aggiungiOTogliAttivo = false;
					num4++;
				}
			}
			for (int num6 = 0; num6 < this.ListaReclutamento.Count; num6++)
			{
				bool flag3 = false;
				int num7 = 0;
				while (num7 < this.ListaUnitàAlleate.Count && !flag3)
				{
					if (this.ListaReclutamento[num6] == this.ListaUnitàAlleate[num7].GetComponent<PresenzaAlleato>().tipoTruppa)
					{
						this.rettEsercitoInRecl.transform.GetChild(num6).transform.GetChild(0).GetComponent<Image>().sprite = this.ListaUnitàAlleate[num7].GetComponent<PresenzaAlleato>().immagineUnità;
						this.rettEsercitoInRecl.transform.GetChild(num6).GetComponent<CanvasGroup>().alpha = 1f;
						this.rettEsercitoInRecl.transform.GetChild(num6).GetComponent<CanvasGroup>().interactable = true;
						this.rettEsercitoInRecl.transform.GetChild(num6).GetComponent<CanvasGroup>().blocksRaycasts = true;
						flag3 = true;
					}
					else
					{
						this.rettEsercitoInRecl.transform.GetChild(num6).GetComponent<CanvasGroup>().alpha = 0f;
						this.rettEsercitoInRecl.transform.GetChild(num6).GetComponent<CanvasGroup>().interactable = false;
						this.rettEsercitoInRecl.transform.GetChild(num6).GetComponent<CanvasGroup>().blocksRaycasts = false;
					}
					num7++;
				}
			}
		}
		if (this.selezVisibileRecl)
		{
			if (this.aggiornaSelezReclVisibile)
			{
				for (int num8 = 0; num8 < this.numUnitàInUnEser; num8++)
				{
					this.rettEsercitoInRecl.transform.GetChild(num8).GetChild(1).GetComponent<CanvasGroup>().alpha = 0f;
				}
				this.rettEsercitoInRecl.transform.GetChild(this.posizioneInListaRecl).GetChild(1).GetComponent<CanvasGroup>().alpha = 1f;
				this.aggiornaSelezReclVisibile = false;
			}
		}
		else
		{
			this.rettEsercitoInRecl.transform.GetChild(this.posizioneInListaRecl).GetChild(1).GetComponent<CanvasGroup>().alpha = 0f;
		}
		if (this.aggiornaDettagliRecl)
		{
			for (int num9 = 0; num9 < 4; num9++)
			{
				for (int num10 = 0; num10 < 11; num10++)
				{
					this.ListaMatricePulsantiArmi[num9] = 0;
				}
			}
			int num11;
			if (this.selezioneInGiàReclutati)
			{
				num11 = this.ListaReclutamento[this.posizioneInListaRecl];
			}
			else
			{
				num11 = this.tipoTruppaSelez;
			}
			for (int num12 = 0; num12 < this.ListaUnitàAlleate.Count; num12++)
			{
				if (num11 == this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().tipoTruppa)
				{
					this.dettagliUnitàRecl.transform.GetChild(0).GetComponent<Text>().text = this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().nomeUnità;
					this.dettagliUnitàRecl.transform.GetChild(1).GetComponent<Image>().sprite = this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().immagineUnità;
					if (!this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().èPerRifornimento)
					{
						if (this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().èAereo)
						{
							this.dettagliUnitàRecl.transform.GetChild(2).GetComponent<Text>().text = string.Concat(new object[]
							{
								"Health:  ",
								this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().vita.ToString(),
								"\nCost in Battle Point: ",
								(this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().costoInPlastica / 10f).ToString("F0"),
								"\nFuel:  ",
								this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().carburante.ToString("F0"),
								"\nSpeed:  ",
								this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().velocitàIndicativa,
								"\nVisual Range:  ",
								this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().raggioVisivo.ToString(),
								"\nClimbing:  ",
								this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().scalatrice.ToString(),
								"\nRepair Step:  ",
								this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().velocitàRiparazione,
								"\nCost in Refined Plastic: ",
								this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().costoInPlastica
							});
						}
						else
						{
							this.dettagliUnitàRecl.transform.GetChild(2).GetComponent<Text>().text = string.Concat(new object[]
							{
								"Health:  ",
								this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().vita.ToString(),
								"\nCost in Battle Point: ",
								(this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().costoInPlastica / 10f).ToString("F0"),
								"\nFuel:  N.D.\nSpeed:  ",
								this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().velocitàIndicativa,
								"\nVisual Range:  ",
								this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().raggioVisivo.ToString(),
								"\nClimbing:  ",
								this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().scalatrice.ToString(),
								"\nRepair Step:  ",
								this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().velocitàRiparazione,
								"\nCost in Refined Plastic: ",
								this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().costoInPlastica
							});
						}
					}
					else
					{
						this.dettagliUnitàRecl.transform.GetChild(2).GetComponent<Text>().text = string.Concat(new object[]
						{
							"Health:  ",
							this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().vita.ToString(),
							"\nCost in Battle Point: ",
							(this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().costoInPlastica / 10f).ToString("F0"),
							"\nSpeed:  ",
							this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().velocitàIndicativa,
							"\nSupply Capacity:  ",
							this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().puntiRifornimentoDisp.ToString(),
							"\nSupply Range:  ",
							this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().raggioDiRifornimento.ToString(),
							"\nClimbing:  ",
							this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().scalatrice.ToString(),
							"\nRepair Step:  ",
							this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().velocitàRiparazione,
							"\nCost in Refined Plastic: ",
							this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().costoInPlastica
						});
					}
					this.dettagliUnitàRecl.transform.GetChild(3).GetComponent<Text>().text = this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().oggettoDescrizione.GetComponent<Text>().text;
					this.dettagliUnitàRecl.transform.GetChild(6).GetChild(0).GetComponent<Text>().text = this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().costoInPlastica.ToString();
					if (this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().tipoTruppa == 10 || this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().tipoTruppa == 11 || this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().tipoTruppa == 16 || this.ListaUnitàAlleate[num12].GetComponent<PresenzaAlleato>().tipoTruppa == 33)
					{
						this.pulsanteArmiRecl.GetComponent<Button>().interactable = false;
					}
					else
					{
						this.pulsanteArmiRecl.GetComponent<Button>().interactable = true;
					}
					break;
				}
			}
			this.aggiornaDettagliRecl = false;
		}
		if (this.creaEsercitoAlleato && this.ListaReclutamento[0] != 100)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.esercitoPrefab, base.transform.position + base.transform.forward * 3f, base.transform.rotation) as GameObject;
			gameObject.name = "Army " + this.sequenzaNumNomeEser;
			gameObject.GetComponent<PresenzaAlleataStrategica>().nomeEsercito = "Army " + this.sequenzaNumNomeEser;
			gameObject.GetComponent<PresenzaAlleataStrategica>().numIdentitàAlleato = this.sequenzaNumNomeEser;
			this.sequenzaNumNomeEser++;
			this.ListaEserAlleatiAttivi.Add(gameObject);
			gameObject.GetComponent<NavMeshAgent>().SetDestination(this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.Count - 1].transform.position);
			for (int num13 = 0; num13 < this.numUnitàInUnEser; num13++)
			{
				gameObject.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[num13] = this.ListaReclutamento[num13];
			}
			this.ListaReclutamento.Clear();
			for (int num14 = 0; num14 < this.numUnitàInUnEser; num14++)
			{
				this.ListaReclutamento.Add(100);
				this.rettEsercitoInRecl.transform.GetChild(num14).GetComponent<CanvasGroup>().alpha = 0f;
				this.rettEsercitoInRecl.transform.GetChild(num14).GetComponent<CanvasGroup>().interactable = false;
				this.rettEsercitoInRecl.transform.GetChild(num14).GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
			this.creaEsercitoAlleato = false;
			this.reclutamentoAttivo = false;
		}
	}

	// Token: 0x0600002A RID: 42 RVA: 0x0000AD0C File Offset: 0x00008F0C
	private void VisualizzaEsercito()
	{
		if (this.aggiornaEser)
		{
			this.elencoEserBarraVerticale.GetComponent<Scrollbar>().value = 1f;
			if (this.rinominaEser && this.tipoEserDaRinominare == 0)
			{
				this.rinominaEser = false;
				this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().nomeEsercito = this.sfondoRinominaEser.transform.GetChild(0).GetChild(2).GetComponent<Text>().text;
			}
			this.scrittaNomeEser.GetComponent<Text>().text = this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().nomeEsercito;
			this.ListaTipiPresentiInVisualEser = new List<int>();
			for (int i = 0; i < this.numUnitàInUnEser; i++)
			{
				int num = this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[i];
				if (num != 100 && !this.ListaTipiPresentiInVisualEser.Contains(num))
				{
					this.ListaTipiPresentiInVisualEser.Add(num);
				}
			}
			for (int j = 0; j < this.numUnitàInUnEser; j++)
			{
				if (j < this.ListaTipiPresentiInVisualEser.Count)
				{
					this.contenutoVisualElencoEser.transform.GetChild(j).GetComponent<CanvasGroup>().alpha = 1f;
					this.contenutoVisualElencoEser.transform.GetChild(j).GetComponent<CanvasGroup>().interactable = true;
					this.contenutoVisualElencoEser.transform.GetChild(j).GetComponent<CanvasGroup>().blocksRaycasts = true;
					this.contenutoVisualElencoEser.transform.GetChild(j).GetChild(0).GetComponent<Image>().sprite = this.ListaUnitàAlleate[this.ListaTipiPresentiInVisualEser[j]].GetComponent<PresenzaAlleato>().immagineUnità;
					this.contenutoVisualElencoEser.transform.GetChild(j).GetChild(1).GetComponent<Text>().text = this.ListaUnitàAlleate[this.ListaTipiPresentiInVisualEser[j]].GetComponent<PresenzaAlleato>().nomeUnità;
					int num2 = 0;
					for (int k = 0; k < this.numUnitàInUnEser; k++)
					{
						if (this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[k] == this.ListaTipiPresentiInVisualEser[j])
						{
							num2++;
						}
					}
					this.contenutoVisualElencoEser.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Text>().text = num2.ToString();
				}
				else
				{
					this.contenutoVisualElencoEser.transform.GetChild(j).GetComponent<CanvasGroup>().alpha = 0f;
					this.contenutoVisualElencoEser.transform.GetChild(j).GetComponent<CanvasGroup>().interactable = false;
					this.contenutoVisualElencoEser.transform.GetChild(j).GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
			}
			this.aggiornaEser = false;
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x0000B00C File Offset: 0x0000920C
	private void RiassuntoVeloceEsercito()
	{
		int num = 0;
		for (int i = 0; i < this.numUnitàInUnEser; i++)
		{
			if (this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[i] != 100)
			{
				num++;
			}
		}
		this.riassuntoEsercito.transform.GetChild(0).GetComponent<Text>().text = "ARMY NAME:  " + this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().nomeEsercito;
		this.riassuntoEsercito.transform.GetChild(1).GetComponent<Text>().text = "UNITS:  " + num.ToString() + "     MOVEMENT:  " + this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().puòAncoraMuoversi.ToString();
		if (this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().turniPerCancell != 100)
		{
			this.riassuntoEsercito.transform.GetChild(2).GetComponent<Text>().text = "This army have to leave this room, otherwise it will be completely discharged! \nTurns remaining:  " + this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().turniPerCancell.ToString();
		}
		else
		{
			this.riassuntoEsercito.transform.GetChild(2).GetComponent<Text>().text = string.Empty;
		}
	}

	// Token: 0x0600002C RID: 44 RVA: 0x0000B178 File Offset: 0x00009378
	private void VisualizzaDettagliEsercito()
	{
		if (this.congedaUnitàSel)
		{
			if (this.origineDeiDettagli == 0)
			{
				bool flag = false;
				bool flag2 = false;
				if (this.ListaTipiPresentiInVisualEser.Count > 0)
				{
					for (int i = 0; i < this.numUnitàInUnEser; i++)
					{
						if (this.ListaTipiPresentiInVisualEser[this.numPosUnità] == this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[i])
						{
							if (!flag2)
							{
								flag2 = true;
								this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[i] = 100;
							}
							else
							{
								flag = true;
							}
						}
					}
					if (!flag)
					{
						this.dettagliUnitàInEser.GetComponent<CanvasGroup>().alpha = 0f;
						this.dettagliUnitàInEser.GetComponent<CanvasGroup>().interactable = false;
						this.dettagliUnitàInEser.GetComponent<CanvasGroup>().blocksRaycasts = false;
						this.visualizzaDettagli = false;
						this.controlloEserVuoti = true;
					}
				}
			}
			this.congedaUnitàSel = false;
			this.aggiornaEser = true;
			this.aggiornaDettagliEser = true;
		}
		if (this.aggiornaDettagliEser)
		{
			if (this.origineDeiDettagli == 0)
			{
				this.pulsanteCongedo.GetComponent<Button>().interactable = true;
			}
			else if (this.origineDeiDettagli == 1)
			{
				this.pulsanteCongedo.GetComponent<Button>().interactable = false;
			}
			for (int j = 0; j < 4; j++)
			{
				for (int k = 0; k < 11; k++)
				{
					this.ListaMatricePulsantiArmi[j] = 0;
				}
			}
			if (this.origineDeiDettagli == 0)
			{
				this.unitàConsiderata = this.ListaUnitàAlleate[this.ListaTipiPresentiInVisualEser[this.numPosUnità]];
			}
			else if (this.origineDeiDettagli == 1)
			{
				int num = 0;
				for (int l = 0; l < this.ListaUnitàAlleate.Count; l++)
				{
					if (this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().ListaAlleatiInSchermBatt[l][1] != 0)
					{
						if (num == this.numPosUnità)
						{
							this.unitàConsiderata = this.ListaUnitàAlleate[this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().ListaAlleatiInSchermBatt[l][0]];
							break;
						}
						num++;
					}
				}
			}
			this.dettagliUnitàInEser.transform.GetChild(1).GetComponent<Text>().text = this.unitàConsiderata.GetComponent<PresenzaAlleato>().nomeUnità;
			this.dettagliUnitàInEser.transform.GetChild(2).GetComponent<Image>().sprite = this.unitàConsiderata.GetComponent<PresenzaAlleato>().immagineUnità;
			if (!this.unitàConsiderata.GetComponent<PresenzaAlleato>().èPerRifornimento)
			{
				if (this.unitàConsiderata.GetComponent<PresenzaAlleato>().èAereo)
				{
					this.dettagliUnitàInEser.transform.GetChild(3).GetComponent<Text>().text = string.Concat(new object[]
					{
						"Health:  ",
						this.unitàConsiderata.GetComponent<PresenzaAlleato>().vita.ToString(),
						"\nCost in Refined Plastic: ",
						this.unitàConsiderata.GetComponent<PresenzaAlleato>().costoInPlastica,
						"\nCost in Battle Point: ",
						(this.unitàConsiderata.GetComponent<PresenzaAlleato>().costoInPlastica / 10f).ToString("F0"),
						"\nSpeed:  ",
						this.unitàConsiderata.GetComponent<PresenzaAlleato>().velocitàIndicativa,
						"\nVisual Range:  ",
						this.unitàConsiderata.GetComponent<PresenzaAlleato>().raggioVisivo.ToString(),
						"\nClimbing:  ",
						this.unitàConsiderata.GetComponent<PresenzaAlleato>().scalatrice.ToString(),
						"\nRepair Step:  ",
						this.unitàConsiderata.GetComponent<PresenzaAlleato>().velocitàRiparazione,
						"\nFuel:  ",
						this.unitàConsiderata.GetComponent<PresenzaAlleato>().carburante.ToString("F0")
					});
				}
				else
				{
					this.dettagliUnitàInEser.transform.GetChild(3).GetComponent<Text>().text = string.Concat(new object[]
					{
						"Health:  ",
						this.unitàConsiderata.GetComponent<PresenzaAlleato>().vita.ToString(),
						"\nCost in Refined Plastic: ",
						this.unitàConsiderata.GetComponent<PresenzaAlleato>().costoInPlastica,
						"\nCost in Battle Point: ",
						(this.unitàConsiderata.GetComponent<PresenzaAlleato>().costoInPlastica / 10f).ToString("F0"),
						"\nSpeed:  ",
						this.unitàConsiderata.GetComponent<PresenzaAlleato>().velocitàIndicativa,
						"\nVisual Range:  ",
						this.unitàConsiderata.GetComponent<PresenzaAlleato>().raggioVisivo.ToString(),
						"\nClimbing:  ",
						this.unitàConsiderata.GetComponent<PresenzaAlleato>().scalatrice.ToString(),
						"\nRepair Step:  ",
						this.unitàConsiderata.GetComponent<PresenzaAlleato>().velocitàRiparazione,
						"\nFuel:  N.D."
					});
				}
				this.pulsanteArmi.GetComponent<Button>().interactable = true;
			}
			else
			{
				this.dettagliUnitàInEser.transform.GetChild(3).GetComponent<Text>().text = string.Concat(new object[]
				{
					"Health:  ",
					this.unitàConsiderata.GetComponent<PresenzaAlleato>().vita.ToString(),
					"\nCost in Battle Point: ",
					(this.unitàConsiderata.GetComponent<PresenzaAlleato>().costoInPlastica / 10f).ToString("F0"),
					"\nSpeed:  ",
					this.unitàConsiderata.GetComponent<PresenzaAlleato>().velocitàIndicativa,
					"\nSupply Capacity:  ",
					this.unitàConsiderata.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp.ToString(),
					"\nSupply Range:  ",
					this.unitàConsiderata.GetComponent<PresenzaAlleato>().raggioDiRifornimento.ToString(),
					"\nClimbing:  ",
					this.unitàConsiderata.GetComponent<PresenzaAlleato>().scalatrice.ToString(),
					"\nRepair Step:  ",
					this.unitàConsiderata.GetComponent<PresenzaAlleato>().velocitàRiparazione,
					"\nCost in Refined Plastic: ",
					this.unitàConsiderata.GetComponent<PresenzaAlleato>().costoInPlastica
				});
				this.pulsanteArmi.GetComponent<Button>().interactable = false;
			}
			this.dettagliUnitàInEser.transform.GetChild(4).GetComponent<Text>().text = this.unitàConsiderata.GetComponent<PresenzaAlleato>().oggettoDescrizione.GetComponent<Text>().text;
			if (this.unitàConsiderata.GetComponent<PresenzaAlleato>().tipoTruppa == 10 || this.unitàConsiderata.GetComponent<PresenzaAlleato>().tipoTruppa == 11 || this.unitàConsiderata.GetComponent<PresenzaAlleato>().tipoTruppa == 16 || this.unitàConsiderata.GetComponent<PresenzaAlleato>().tipoTruppa == 33)
			{
				this.pulsanteArmi.GetComponent<Button>().interactable = false;
			}
			else
			{
				this.pulsanteArmi.GetComponent<Button>().interactable = true;
			}
			this.aggiornaDettagliEser = false;
		}
	}

	// Token: 0x0600002D RID: 45 RVA: 0x0000B8EC File Offset: 0x00009AEC
	private void VisualizzaArmi()
	{
		float num = 0f;
		int num2 = 0;
		for (int i = 0; i < 4; i++)
		{
			if (this.ListaTimerTipoTruppa[i] > num)
			{
				num = this.ListaTimerTipoTruppa[i];
				num2 = i;
			}
		}
		if (num2 == 0)
		{
			this.numAssolutoTipoTruppa = this.tipoTruppaSelez;
		}
		else if (num2 == 1)
		{
			this.numAssolutoTipoTruppa = this.ListaReclutamento[this.posizioneInListaRecl];
		}
		else if (num2 == 2)
		{
			this.numAssolutoTipoTruppa = this.ListaTipiPresentiInVisualEser[this.numPosUnità];
		}
		else if (num2 == 3)
		{
			int num3 = this.numPosUnità;
			for (int j = 0; j < this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().ListaAlleatiInSchermBatt.Count; j++)
			{
				if (this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().ListaAlleatiInSchermBatt[j][1] != 0)
				{
					if (num3 == 0)
					{
						this.numAssolutoTipoTruppa = this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().ListaAlleatiInSchermBatt[j][0];
						break;
					}
					num3--;
				}
			}
		}
		GameObject gameObject = null;
		for (int k = 0; k < this.ListaUnitàAlleate.Count; k++)
		{
			if (this.ListaUnitàAlleate[k].GetComponent<PresenzaAlleato>().tipoTruppa == this.numAssolutoTipoTruppa)
			{
				gameObject = this.ListaUnitàAlleate[k];
				break;
			}
		}
		this.ListaArmiUnitàConsiderata = new List<List<float>>();
		this.ListaArmiUnitàConsiderata.Add(gameObject.GetComponent<PresenzaAlleato>().ListaValoriArma1);
		this.ListaArmiUnitàConsiderata.Add(gameObject.GetComponent<PresenzaAlleato>().ListaValoriArma2);
		this.ListaArmiUnitàConsiderata.Add(gameObject.GetComponent<PresenzaAlleato>().ListaValoriArma3);
		this.ListaArmiUnitàConsiderata.Add(gameObject.GetComponent<PresenzaAlleato>().ListaValoriArma4);
		this.ListaTipiMunizioniArmiUnitàCons = new List<List<GameObject>>();
		this.ListaTipiMunizioniArmiUnitàCons.Add(gameObject.GetComponent<PresenzaAlleato>().ListaTipiMunizioniArma1);
		this.ListaTipiMunizioniArmiUnitàCons.Add(gameObject.GetComponent<PresenzaAlleato>().ListaTipiMunizioniArma2);
		this.ListaTipiMunizioniArmiUnitàCons.Add(gameObject.GetComponent<PresenzaAlleato>().ListaTipiMunizioniArma3);
		this.ListaTipiMunizioniArmiUnitàCons.Add(gameObject.GetComponent<PresenzaAlleato>().ListaTipiMunizioniArma4);
		if (this.aggArmi)
		{
			for (int l = 0; l < 4; l++)
			{
				this.ListaMatricePulsantiArmi[l] = 0;
			}
			this.pulsanteColonnaArma = 0;
			this.pulsanteRigaArma = 0;
		}
		else
		{
			for (int m = 0; m < 4; m++)
			{
				if (this.pulsanteColonnaArma == m)
				{
					this.ListaMatricePulsantiArmi[m] = this.pulsanteRigaArma;
				}
			}
		}
		this.aggArmi = false;
		this.Armi.transform.GetChild(0).GetComponent<Text>().text = "WEAPONS of:  " + gameObject.GetComponent<PresenzaAlleato>().nomeUnità;
		for (int n = 0; n < 4; n++)
		{
			GameObject gameObject2 = this.Armi.transform.GetChild(n + 1).FindChild("lista pulsanti armi").gameObject;
			if (n >= gameObject.GetComponent<PresenzaAlleato>().numeroArmi)
			{
				this.Armi.transform.GetChild(n + 1).GetComponent<CanvasGroup>().alpha = 0f;
				this.Armi.transform.GetChild(n + 1).GetComponent<CanvasGroup>().interactable = false;
				this.Armi.transform.GetChild(n + 1).GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
			else
			{
				this.Armi.transform.GetChild(n + 1).GetComponent<CanvasGroup>().alpha = 1f;
				this.Armi.transform.GetChild(n + 1).GetComponent<CanvasGroup>().interactable = true;
				this.Armi.transform.GetChild(n + 1).GetComponent<CanvasGroup>().blocksRaycasts = true;
				this.Armi.transform.GetChild(n + 1).GetChild(0).GetComponent<Text>().text = "WEAPON " + (n + 1).ToString();
				if (this.ListaArmiUnitàConsiderata[n][0] != 0f)
				{
					this.Armi.transform.GetChild(n + 1).FindChild("pulsante salva armi").GetComponent<CanvasGroup>().interactable = false;
					for (int num4 = 0; num4 < 12; num4++)
					{
						if (num4 < this.ListaTipiMunizioniArmiUnitàCons[n].Count)
						{
							gameObject2.transform.GetChild(num4).GetChild(0).GetComponent<Text>().text = this.ListaTipiMunizioniArmiUnitàCons[n][num4].GetComponent<DatiGeneraliMunizione>().nome;
							gameObject2.transform.GetChild(num4).GetComponent<CanvasGroup>().alpha = 1f;
							gameObject2.transform.GetChild(num4).GetComponent<CanvasGroup>().interactable = true;
							gameObject2.transform.GetChild(num4).GetComponent<CanvasGroup>().blocksRaycasts = true;
							if (num4 == this.ListaMatricePulsantiArmi[n])
							{
								gameObject2.transform.GetChild(num4).GetComponent<Image>().color = this.coloreSelEser;
							}
							else
							{
								gameObject2.transform.GetChild(num4).GetComponent<Image>().color = this.coloreNonSelEser;
							}
						}
						else
						{
							gameObject2.transform.GetChild(num4).GetComponent<CanvasGroup>().alpha = 0f;
							gameObject2.transform.GetChild(num4).GetComponent<CanvasGroup>().interactable = false;
							gameObject2.transform.GetChild(num4).GetComponent<CanvasGroup>().blocksRaycasts = false;
						}
					}
					this.Armi.transform.GetChild(n + 1).GetChild(1).GetComponent<Text>().text = gameObject.GetComponent<PresenzaAlleato>().ListaNomiArmi[n];
					this.Armi.transform.GetChild(n + 1).GetChild(2).GetComponent<Text>().text = string.Concat(new object[]
					{
						"Damage: ",
						this.ListaTipiMunizioniArmiUnitàCons[n][this.ListaMatricePulsantiArmi[n]].GetComponent<DatiGeneraliMunizione>().danno,
						"\nRange: ",
						this.ListaTipiMunizioniArmiUnitàCons[n][this.ListaMatricePulsantiArmi[n]].GetComponent<DatiGeneraliMunizione>().portataMassima,
						"\nRate: ",
						this.ListaArmiUnitàConsiderata[n][0],
						" (",
						this.ListaArmiUnitàConsiderata[n][1],
						")"
					});
					this.Armi.transform.GetChild(n + 1).GetChild(3).GetComponent<Text>().text = string.Concat(new object[]
					{
						"Piercing Value: ",
						this.ListaTipiMunizioniArmiUnitàCons[n][this.ListaMatricePulsantiArmi[n]].GetComponent<DatiGeneraliMunizione>().penetrazione,
						"\nBlast Radius: ",
						this.ListaTipiMunizioniArmiUnitàCons[n][this.ListaMatricePulsantiArmi[n]].GetComponent<DatiGeneraliMunizione>().raggioEffetto,
						"\nReload: ",
						this.ListaArmiUnitàConsiderata[n][2]
					});
					this.Armi.transform.GetChild(n + 1).GetChild(4).GetComponent<Text>().text = this.ListaTipiMunizioniArmiUnitàCons[n][this.ListaMatricePulsantiArmi[n]].GetComponent<DatiGeneraliMunizione>().descrizioneArma.GetComponent<Text>().text;
				}
				else
				{
					this.Armi.transform.GetChild(n + 1).FindChild("pulsante salva armi").GetComponent<CanvasGroup>().interactable = true;
					if (gameObject.GetComponent<PresenzaAlleato>().tipoTruppaVolante != 0)
					{
						for (int num5 = 0; num5 < 12; num5++)
						{
							if (num5 < gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili.Count)
							{
								gameObject2.transform.GetChild(num5).GetChild(0).GetComponent<Text>().text = gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili[num5].GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().nome + "  x" + gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili[num5].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche.Count * 2;
								gameObject2.transform.GetChild(num5).GetComponent<CanvasGroup>().alpha = 1f;
								gameObject2.transform.GetChild(num5).GetComponent<CanvasGroup>().interactable = true;
								gameObject2.transform.GetChild(num5).GetComponent<CanvasGroup>().blocksRaycasts = true;
								if (this.pulsanteColonnaArma == n && this.pulsanteRigaArma == num5)
								{
									gameObject2.transform.GetChild(num5).GetComponent<Image>().color = this.coloreSelEser;
								}
								else
								{
									gameObject2.transform.GetChild(num5).GetComponent<Image>().color = this.coloreNonSelEser;
								}
							}
							else
							{
								gameObject2.transform.GetChild(num5).GetComponent<CanvasGroup>().alpha = 0f;
								gameObject2.transform.GetChild(num5).GetComponent<CanvasGroup>().interactable = false;
								gameObject2.transform.GetChild(num5).GetComponent<CanvasGroup>().blocksRaycasts = false;
							}
						}
						int tipoTruppaVolante = gameObject.GetComponent<PresenzaAlleato>().tipoTruppaVolante;
						int @int;
						if (gameObject.GetComponent<PresenzaAlleato>().tipoTruppaVolante == 5 || gameObject.GetComponent<PresenzaAlleato>().tipoTruppaVolante == 13)
						{
							@int = PlayerPrefs.GetInt(string.Concat(new object[]
							{
								"tipo truppa volante ",
								tipoTruppaVolante,
								" ",
								n
							}));
						}
						else
						{
							@int = PlayerPrefs.GetInt(string.Concat(new object[]
							{
								"tipo truppa volante ",
								tipoTruppaVolante,
								" ",
								n - 1
							}));
						}
						bool flag = false;
						int num6 = 0;
						while (num6 < this.ListaOrdigniTotali.Count && !flag)
						{
							if (this.ListaOrdigniTotali[num6].GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == @int)
							{
								GameObject x = this.ListaOrdigniTotali[num6];
								flag = true;
								for (int num7 = 0; num7 < gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili.Count; num7++)
								{
									if (x == gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili[num7])
									{
										gameObject2.transform.GetChild(num7).GetComponent<Image>().color = this.coloreArmaSalvata;
									}
								}
							}
							num6++;
						}
						GameObject gameObject3 = gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili[this.ListaMatricePulsantiArmi[n]];
						this.Armi.transform.GetChild(n + 1).GetChild(1).GetComponent<Text>().text = gameObject3.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().nome + "  x" + gameObject3.GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche.Count * 2;
						this.Armi.transform.GetChild(n + 1).GetChild(2).GetComponent<Text>().text = string.Concat(new object[]
						{
							"Damage: ",
							gameObject3.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().danno,
							"\nRange: ",
							gameObject3.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().portataMassima,
							"\nRate: ",
							gameObject3.GetComponent<DatiOrdignoEsterno>().ListaValoriOrdigno[0],
							" (",
							gameObject3.GetComponent<DatiOrdignoEsterno>().ListaValoriOrdigno[1],
							")"
						});
						this.Armi.transform.GetChild(n + 1).GetChild(3).GetComponent<Text>().text = string.Concat(new object[]
						{
							"Piercing Value: ",
							gameObject3.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().penetrazione,
							"\nBlast Radius: ",
							gameObject3.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().raggioEffetto,
							"\nReload: ",
							gameObject3.GetComponent<DatiOrdignoEsterno>().ListaValoriOrdigno[2]
						});
						this.Armi.transform.GetChild(n + 1).GetChild(4).GetComponent<Text>().text = gameObject3.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().descrizioneArma.GetComponent<Text>().text;
					}
					else if (gameObject.GetComponent<PresenzaAlleato>().tipoTruppaTerrConOrdigni != 0)
					{
						for (int num8 = 0; num8 < 12; num8++)
						{
							if (num8 < gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili.Count)
							{
								gameObject2.transform.GetChild(num8).GetChild(0).GetComponent<Text>().text = gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili[num8].GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().nome + "  x" + gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili[num8].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche.Count;
								gameObject2.transform.GetChild(num8).GetComponent<CanvasGroup>().alpha = 1f;
								gameObject2.transform.GetChild(num8).GetComponent<CanvasGroup>().interactable = true;
								gameObject2.transform.GetChild(num8).GetComponent<CanvasGroup>().blocksRaycasts = true;
								if (this.pulsanteColonnaArma == n && this.pulsanteRigaArma == num8)
								{
									gameObject2.transform.GetChild(num8).GetComponent<Image>().color = this.coloreSelEser;
								}
								else
								{
									gameObject2.transform.GetChild(num8).GetComponent<Image>().color = this.coloreNonSelEser;
								}
							}
							else
							{
								gameObject2.transform.GetChild(num8).GetComponent<CanvasGroup>().alpha = 0f;
								gameObject2.transform.GetChild(num8).GetComponent<CanvasGroup>().interactable = false;
								gameObject2.transform.GetChild(num8).GetComponent<CanvasGroup>().blocksRaycasts = false;
							}
						}
						int tipoTruppaTerrConOrdigni = gameObject.GetComponent<PresenzaAlleato>().tipoTruppaTerrConOrdigni;
						int int2;
						if (gameObject.GetComponent<PresenzaAlleato>().tipoTruppaTerrConOrdigni == 3)
						{
							int2 = PlayerPrefs.GetInt(string.Concat(new object[]
							{
								"tipo truppa terr con ordigno ",
								tipoTruppaTerrConOrdigni,
								" ",
								n
							}));
						}
						else
						{
							int2 = PlayerPrefs.GetInt(string.Concat(new object[]
							{
								"tipo truppa terr con ordigno ",
								tipoTruppaTerrConOrdigni,
								" ",
								n - 1
							}));
						}
						bool flag2 = false;
						int num9 = 0;
						while (num9 < this.ListaOrdigniTotali.Count && !flag2)
						{
							if (this.ListaOrdigniTotali[num9].GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == int2)
							{
								GameObject x2 = this.ListaOrdigniTotali[num9];
								flag2 = true;
								for (int num10 = 0; num10 < gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili.Count; num10++)
								{
									if (x2 == gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili[num10])
									{
										gameObject2.transform.GetChild(num10).GetComponent<Image>().color = this.coloreArmaSalvata;
									}
								}
							}
							num9++;
						}
						GameObject gameObject4 = gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili[this.ListaMatricePulsantiArmi[n]];
						this.Armi.transform.GetChild(n + 1).GetChild(1).GetComponent<Text>().text = gameObject4.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().nome + "  x" + gameObject4.GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche.Count;
						this.Armi.transform.GetChild(n + 1).GetChild(2).GetComponent<Text>().text = string.Concat(new object[]
						{
							"Damage: ",
							gameObject4.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().danno,
							"\nRange: ",
							gameObject4.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().portataMassima,
							"\nRate: ",
							gameObject4.GetComponent<DatiOrdignoEsterno>().ListaValoriOrdigno[0],
							" (",
							gameObject4.GetComponent<DatiOrdignoEsterno>().ListaValoriOrdigno[1],
							")"
						});
						this.Armi.transform.GetChild(n + 1).GetChild(3).GetComponent<Text>().text = string.Concat(new object[]
						{
							"Piercing Value: ",
							gameObject4.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().penetrazione,
							"\nBlast Radius: ",
							gameObject4.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().raggioEffetto,
							"\nReload: ",
							gameObject4.GetComponent<DatiOrdignoEsterno>().ListaValoriOrdigno[2]
						});
						this.Armi.transform.GetChild(n + 1).GetChild(4).GetComponent<Text>().text = gameObject4.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().descrizioneArma.GetComponent<Text>().text;
					}
				}
			}
		}
		if (this.salvaArmaSelez)
		{
			int tipologiaOrdigno = gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili[this.ListaMatricePulsantiArmi[this.numColonnaArmaDaSalvare]].GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno;
			if (gameObject.GetComponent<PresenzaAlleato>().tipoTruppaVolante != 0)
			{
				if (gameObject.GetComponent<PresenzaAlleato>().tipoTruppaVolante == 5 || gameObject.GetComponent<PresenzaAlleato>().tipoTruppaVolante == 13)
				{
					PlayerPrefs.SetInt(string.Concat(new object[]
					{
						"tipo truppa volante ",
						gameObject.GetComponent<PresenzaAlleato>().tipoTruppaVolante,
						" ",
						this.numColonnaArmaDaSalvare
					}), tipologiaOrdigno);
				}
				else
				{
					PlayerPrefs.SetInt(string.Concat(new object[]
					{
						"tipo truppa volante ",
						gameObject.GetComponent<PresenzaAlleato>().tipoTruppaVolante,
						" ",
						this.numColonnaArmaDaSalvare - 1
					}), tipologiaOrdigno);
				}
			}
			else if (gameObject.GetComponent<PresenzaAlleato>().tipoTruppaTerrConOrdigni != 0)
			{
				if (gameObject.GetComponent<PresenzaAlleato>().tipoTruppaTerrConOrdigni == 3)
				{
					PlayerPrefs.SetInt(string.Concat(new object[]
					{
						"tipo truppa terr con ordigno ",
						gameObject.GetComponent<PresenzaAlleato>().tipoTruppaTerrConOrdigni,
						" ",
						this.numColonnaArmaDaSalvare
					}), tipologiaOrdigno);
				}
				else
				{
					PlayerPrefs.SetInt(string.Concat(new object[]
					{
						"tipo truppa terr con ordigno ",
						gameObject.GetComponent<PresenzaAlleato>().tipoTruppaTerrConOrdigni,
						" ",
						this.numColonnaArmaDaSalvare - 1
					}), tipologiaOrdigno);
				}
			}
			this.salvaArmaSelez = false;
		}
	}

	// Token: 0x0600002E RID: 46 RVA: 0x0000CDAC File Offset: 0x0000AFAC
	private void FunzioneScambioFraEserciti()
	{
		if (this.effettuaScambio)
		{
			this.effettuaScambio = false;
			this.aggScambioEserciti = true;
			this.FunzioneScambioFraEserciti();
			this.aggScambioEserciti = true;
			GameObject esercitoSelezionato = this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato;
			GameObject secondoEsercitoSelezionato = this.cameraCasa.GetComponent<SelezionamentoInStrategia>().secondoEsercitoSelezionato;
			if (this.eserDellaTruppaSel == 1 && esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[0] != 100)
			{
				if (this.numPosUnità > 30)
				{
					this.numPosUnità = 0;
				}
				if (this.ListaTipiPresentiInVisualEser.Count > this.numPosUnità)
				{
					int num = this.ListaTipiPresentiInVisualEser[this.numPosUnità];
					if (secondoEsercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[this.numUnitàInUnEser - 1] == 100)
					{
						bool flag = false;
						bool flag2 = false;
						for (int i = 0; i < this.numUnitàInUnEser; i++)
						{
							if (!flag && secondoEsercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[i] == 100)
							{
								secondoEsercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[i] = num;
								flag = true;
							}
							if (!flag2 && esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[i] == num)
							{
								esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[i] = 100;
								flag2 = true;
							}
							if (flag && flag2)
							{
								break;
							}
						}
					}
				}
			}
			else if (this.eserDellaTruppaSel == 2 && secondoEsercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[0] != 100)
			{
				if (this.numPosUnità > 30)
				{
					this.numPosUnità = 0;
				}
				if (this.ListaTipiPresentiInVisualSecondoEser.Count > this.numPosUnità)
				{
					int num2 = this.ListaTipiPresentiInVisualSecondoEser[this.numPosUnità];
					if (esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[this.numUnitàInUnEser - 1] == 100)
					{
						bool flag3 = false;
						bool flag4 = false;
						for (int j = 0; j < this.numUnitàInUnEser; j++)
						{
							if (!flag3 && esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[j] == 100)
							{
								esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[j] = num2;
								flag3 = true;
							}
							if (!flag4 && secondoEsercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[j] == num2)
							{
								secondoEsercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[j] = 100;
								flag4 = true;
							}
							if (flag3 && flag4)
							{
								break;
							}
						}
					}
				}
			}
			for (int k = 0; k < this.numUnitàInUnEser; k++)
			{
				if (esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[k] == 100 && k < this.numUnitàInUnEser - 1)
				{
					esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[k] = esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[k + 1];
					esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[k + 1] = 100;
				}
				if (secondoEsercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[k] == 100 && k < this.numUnitàInUnEser - 1)
				{
					secondoEsercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[k] = secondoEsercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[k + 1];
					secondoEsercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[k + 1] = 100;
				}
			}
		}
		this.FunzioneAggiornaEsercitiScambio();
		if (this.numPosUnità != 100)
		{
			if (this.eserDellaTruppaSel == 1)
			{
				for (int l = 0; l < this.numUnitàInUnEser; l++)
				{
					if (l == this.numPosUnità)
					{
						ColorBlock colors = this.contenutoVisualElencoEser.transform.GetChild(l).GetComponent<Button>().colors;
						colors.normalColor = this.coloreSelScambioEser;
						this.contenutoVisualElencoEser.transform.GetChild(l).GetComponent<Button>().colors = colors;
					}
					else
					{
						ColorBlock colors2 = this.contenutoVisualElencoEser.transform.GetChild(l).GetComponent<Button>().colors;
						colors2.normalColor = this.coloreNonSelScambioEser;
						this.contenutoVisualElencoEser.transform.GetChild(l).GetComponent<Button>().colors = colors2;
					}
					ColorBlock colors3 = this.contenutoVisualElencoSecondoEser.transform.GetChild(l).GetComponent<Button>().colors;
					colors3.normalColor = this.coloreNonSelScambioEser;
					this.contenutoVisualElencoSecondoEser.transform.GetChild(l).GetComponent<Button>().colors = colors3;
				}
			}
			else if (this.eserDellaTruppaSel == 2)
			{
				for (int m = 0; m < this.numUnitàInUnEser; m++)
				{
					if (m == this.numPosUnità)
					{
						ColorBlock colors4 = this.contenutoVisualElencoSecondoEser.transform.GetChild(m).GetComponent<Button>().colors;
						colors4.normalColor = this.coloreSelScambioEser;
						this.contenutoVisualElencoSecondoEser.transform.GetChild(m).GetComponent<Button>().colors = colors4;
					}
					else
					{
						ColorBlock colors5 = this.contenutoVisualElencoSecondoEser.transform.GetChild(m).GetComponent<Button>().colors;
						colors5.normalColor = this.coloreNonSelScambioEser;
						this.contenutoVisualElencoSecondoEser.transform.GetChild(m).GetComponent<Button>().colors = colors5;
					}
					ColorBlock colors6 = this.contenutoVisualElencoEser.transform.GetChild(m).GetComponent<Button>().colors;
					colors6.normalColor = this.coloreNonSelScambioEser;
					this.contenutoVisualElencoEser.transform.GetChild(m).GetComponent<Button>().colors = colors6;
				}
			}
		}
		this.aggScambioEserciti = false;
	}

	// Token: 0x0600002F RID: 47 RVA: 0x0000D370 File Offset: 0x0000B570
	private void FunzioneAggiornaEsercitiScambio()
	{
		if (this.aggScambioEserciti)
		{
			this.elencoEserBarraVerticale.GetComponent<Scrollbar>().value = 1f;
			this.elencoSecondoEserBarraVerticale.GetComponent<Scrollbar>().value = 1f;
			if (this.rinominaEser && this.tipoEserDaRinominare == 0)
			{
				this.rinominaEser = false;
				this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().nomeEsercito = this.sfondoRinominaEser.transform.GetChild(0).GetChild(2).GetComponent<Text>().text;
			}
			this.scrittaNomeEser.GetComponent<Text>().text = this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().nomeEsercito;
			this.ListaTipiPresentiInVisualEser = new List<int>();
			for (int i = 0; i < this.numUnitàInUnEser; i++)
			{
				int num = this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[i];
				if (num != 100 && !this.ListaTipiPresentiInVisualEser.Contains(num))
				{
					this.ListaTipiPresentiInVisualEser.Add(num);
				}
			}
			for (int j = 0; j < this.numUnitàInUnEser; j++)
			{
				if (j < this.ListaTipiPresentiInVisualEser.Count)
				{
					this.contenutoVisualElencoEser.transform.GetChild(j).GetComponent<CanvasGroup>().alpha = 1f;
					this.contenutoVisualElencoEser.transform.GetChild(j).GetComponent<CanvasGroup>().interactable = true;
					this.contenutoVisualElencoEser.transform.GetChild(j).GetComponent<CanvasGroup>().blocksRaycasts = true;
					this.contenutoVisualElencoEser.transform.GetChild(j).GetChild(0).GetComponent<Image>().sprite = this.ListaUnitàAlleate[this.ListaTipiPresentiInVisualEser[j]].GetComponent<PresenzaAlleato>().immagineUnità;
					this.contenutoVisualElencoEser.transform.GetChild(j).GetChild(1).GetComponent<Text>().text = this.ListaUnitàAlleate[this.ListaTipiPresentiInVisualEser[j]].GetComponent<PresenzaAlleato>().nomeUnità;
					int num2 = 0;
					for (int k = 0; k < this.numUnitàInUnEser; k++)
					{
						if (this.cameraCasa.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[k] == this.ListaTipiPresentiInVisualEser[j])
						{
							num2++;
						}
					}
					this.contenutoVisualElencoEser.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Text>().text = num2.ToString();
				}
				else
				{
					this.contenutoVisualElencoEser.transform.GetChild(j).GetComponent<CanvasGroup>().alpha = 0f;
					this.contenutoVisualElencoEser.transform.GetChild(j).GetComponent<CanvasGroup>().interactable = false;
					this.contenutoVisualElencoEser.transform.GetChild(j).GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
			}
			if (this.rinominaEser && this.tipoEserDaRinominare == 1)
			{
				this.rinominaEser = false;
				this.cameraCasa.GetComponent<SelezionamentoInStrategia>().secondoEsercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().nomeEsercito = this.sfondoRinominaEserSecondo.transform.GetChild(0).GetChild(2).GetComponent<Text>().text;
			}
			this.scrittaNomeEserSecondo.GetComponent<Text>().text = this.cameraCasa.GetComponent<SelezionamentoInStrategia>().secondoEsercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().nomeEsercito;
			this.ListaTipiPresentiInVisualSecondoEser = new List<int>();
			for (int l = 0; l < this.numUnitàInUnEser; l++)
			{
				int num3 = this.cameraCasa.GetComponent<SelezionamentoInStrategia>().secondoEsercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[l];
				if (num3 != 100 && !this.ListaTipiPresentiInVisualSecondoEser.Contains(num3))
				{
					this.ListaTipiPresentiInVisualSecondoEser.Add(num3);
				}
			}
			for (int m = 0; m < this.numUnitàInUnEser; m++)
			{
				if (m < this.ListaTipiPresentiInVisualSecondoEser.Count)
				{
					this.contenutoVisualElencoSecondoEser.transform.GetChild(m).GetComponent<CanvasGroup>().alpha = 1f;
					this.contenutoVisualElencoSecondoEser.transform.GetChild(m).GetComponent<CanvasGroup>().interactable = true;
					this.contenutoVisualElencoSecondoEser.transform.GetChild(m).GetComponent<CanvasGroup>().blocksRaycasts = true;
					this.contenutoVisualElencoSecondoEser.transform.GetChild(m).GetChild(0).GetComponent<Image>().sprite = this.ListaUnitàAlleate[this.ListaTipiPresentiInVisualSecondoEser[m]].GetComponent<PresenzaAlleato>().immagineUnità;
					this.contenutoVisualElencoSecondoEser.transform.GetChild(m).GetChild(1).GetComponent<Text>().text = this.ListaUnitàAlleate[this.ListaTipiPresentiInVisualSecondoEser[m]].GetComponent<PresenzaAlleato>().nomeUnità;
					int num4 = 0;
					for (int n = 0; n < this.numUnitàInUnEser; n++)
					{
						if (this.cameraCasa.GetComponent<SelezionamentoInStrategia>().secondoEsercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[n] == this.ListaTipiPresentiInVisualSecondoEser[m])
						{
							num4++;
						}
					}
					this.contenutoVisualElencoSecondoEser.transform.GetChild(m).GetChild(0).GetChild(0).GetComponent<Text>().text = num4.ToString();
				}
				else
				{
					this.contenutoVisualElencoSecondoEser.transform.GetChild(m).GetComponent<CanvasGroup>().alpha = 0f;
					this.contenutoVisualElencoSecondoEser.transform.GetChild(m).GetComponent<CanvasGroup>().interactable = false;
					this.contenutoVisualElencoSecondoEser.transform.GetChild(m).GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
			}
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x0000D968 File Offset: 0x0000BB68
	private void EliminazioneEserVuoti()
	{
		for (int i = 0; i < this.ListaEserAlleatiAttivi.Count; i++)
		{
			bool flag = true;
			for (int j = 0; j < this.numUnitàInUnEser; j++)
			{
				if (this.ListaEserAlleatiAttivi[i].GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[j] != 100)
				{
					flag = false;
				}
			}
			if (flag)
			{
				UnityEngine.Object.Destroy(this.ListaEserAlleatiAttivi[i]);
				this.ListaEserAlleatiAttivi[i] = null;
			}
		}
		for (int k = 0; k < this.ListaEserAlleatiAttivi.Count; k++)
		{
			if (this.ListaEserAlleatiAttivi[k] == null)
			{
				for (int l = k; l < this.ListaEserAlleatiAttivi.Count; l++)
				{
					if (l < this.ListaEserAlleatiAttivi.Count - 1)
					{
						this.ListaEserAlleatiAttivi[l] = this.ListaEserAlleatiAttivi[l + 1];
					}
					else
					{
						this.ListaEserAlleatiAttivi[l] = null;
					}
				}
			}
		}
		for (int m = 0; m < this.ListaEserAlleatiAttivi.Count; m++)
		{
			if (this.ListaEserAlleatiAttivi[m] == null)
			{
				this.ListaEserAlleatiAttivi.RemoveAt(m);
				m--;
				this.dettagliUnitàInEser.GetComponent<CanvasGroup>().alpha = 0f;
				this.dettagliUnitàInEser.GetComponent<CanvasGroup>().interactable = false;
				this.dettagliUnitàInEser.GetComponent<CanvasGroup>().blocksRaycasts = false;
				this.visualizzaEser = false;
				this.visualizzaDettagli = false;
				this.elencoEser.transform.parent.GetComponent<CanvasGroup>().alpha = 0f;
				this.elencoEser.transform.parent.GetComponent<CanvasGroup>().interactable = false;
				this.elencoEser.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
		}
	}

	// Token: 0x040000B5 RID: 181
	public int numeroTotaleTipiTruppeVolantiCopiato;

	// Token: 0x040000B6 RID: 182
	public int numeroTotaleTipiTruppeTerrConOrdigniCopiato;

	// Token: 0x040000B7 RID: 183
	public int numeroTotaleTipiOrdigniCopiato;

	// Token: 0x040000B8 RID: 184
	public GameObject esercitoPrefab;

	// Token: 0x040000B9 RID: 185
	public Color coloreSelEser;

	// Token: 0x040000BA RID: 186
	public Color coloreNonSelEser;

	// Token: 0x040000BB RID: 187
	public Color coloreArmaSalvata;

	// Token: 0x040000BC RID: 188
	public bool reclutamentoAttivo;

	// Token: 0x040000BD RID: 189
	public int tipoTruppaSelez;

	// Token: 0x040000BE RID: 190
	public bool giàReclutato;

	// Token: 0x040000BF RID: 191
	public bool aggiungiOTogliAttivo;

	// Token: 0x040000C0 RID: 192
	public int posizioneInListaRecl;

	// Token: 0x040000C1 RID: 193
	public bool aggiornaDettagliRecl;

	// Token: 0x040000C2 RID: 194
	public bool selezioneInGiàReclutati;

	// Token: 0x040000C3 RID: 195
	public bool selezVisibileRecl;

	// Token: 0x040000C4 RID: 196
	public bool aggiornaSelezReclVisibile;

	// Token: 0x040000C5 RID: 197
	private GameObject CanvasStrategia;

	// Token: 0x040000C6 RID: 198
	private GameObject cameraCasa;

	// Token: 0x040000C7 RID: 199
	private GameObject PulsanteFittStrategia;

	// Token: 0x040000C8 RID: 200
	private GameObject pulsanteNuovoRecl;

	// Token: 0x040000C9 RID: 201
	private GameObject pulsanteResettaRecl;

	// Token: 0x040000CA RID: 202
	private GameObject pulsanteCreaEser;

	// Token: 0x040000CB RID: 203
	private GameObject rettEsercitoInRecl;

	// Token: 0x040000CC RID: 204
	private GameObject elencoPerReclutare;

	// Token: 0x040000CD RID: 205
	private GameObject dettagliUnitàRecl;

	// Token: 0x040000CE RID: 206
	private GameObject elencoEser;

	// Token: 0x040000CF RID: 207
	private GameObject elencoEserBarraVerticale;

	// Token: 0x040000D0 RID: 208
	private GameObject elencoSecondoEser;

	// Token: 0x040000D1 RID: 209
	private GameObject elencoSecondoEserBarraVerticale;

	// Token: 0x040000D2 RID: 210
	private GameObject contenutoVisualElencoEser;

	// Token: 0x040000D3 RID: 211
	private GameObject contenutoVisualElencoSecondoEser;

	// Token: 0x040000D4 RID: 212
	private GameObject dettagliUnitàInEser;

	// Token: 0x040000D5 RID: 213
	private GameObject pulsanteArmi;

	// Token: 0x040000D6 RID: 214
	private GameObject pulsanteArmiRecl;

	// Token: 0x040000D7 RID: 215
	private GameObject pulsanteCongedo;

	// Token: 0x040000D8 RID: 216
	private GameObject elencoAlleatiInSchermBatt;

	// Token: 0x040000D9 RID: 217
	private GameObject scrittaNonCiSonoRisorse;

	// Token: 0x040000DA RID: 218
	private GameObject scrittaNomeEser;

	// Token: 0x040000DB RID: 219
	private GameObject scrittaNomeEserSecondo;

	// Token: 0x040000DC RID: 220
	private GameObject sfondoRinominaEser;

	// Token: 0x040000DD RID: 221
	private GameObject sfondoRinominaEserSecondo;

	// Token: 0x040000DE RID: 222
	private GameObject riassuntoEsercito;

	// Token: 0x040000DF RID: 223
	public List<int> ListaReclutamento;

	// Token: 0x040000E0 RID: 224
	public bool resettaListaRecl;

	// Token: 0x040000E1 RID: 225
	public bool creaEsercitoAlleato;

	// Token: 0x040000E2 RID: 226
	public List<GameObject> ListaUnitàAlleate;

	// Token: 0x040000E3 RID: 227
	public int sequenzaNumNomeEser;

	// Token: 0x040000E4 RID: 228
	public int numPosUnità;

	// Token: 0x040000E5 RID: 229
	public bool visualizzaEser;

	// Token: 0x040000E6 RID: 230
	public bool visualizzaDettagli;

	// Token: 0x040000E7 RID: 231
	public bool aggiornaEser;

	// Token: 0x040000E8 RID: 232
	public bool aggiornaDettagliEser;

	// Token: 0x040000E9 RID: 233
	public int origineDeiDettagli;

	// Token: 0x040000EA RID: 234
	public bool congedaUnitàSel;

	// Token: 0x040000EB RID: 235
	public int numAssolutoTipoTruppa;

	// Token: 0x040000EC RID: 236
	public float timerDiTipoTruppaSelez;

	// Token: 0x040000ED RID: 237
	public float timerDiPosizioneInListaRecl;

	// Token: 0x040000EE RID: 238
	public float timerDiNumPosInEser;

	// Token: 0x040000EF RID: 239
	public float timerDiNumPosInSchermBatt;

	// Token: 0x040000F0 RID: 240
	public List<float> ListaTimerTipoTruppa;

	// Token: 0x040000F1 RID: 241
	public bool armiAperto;

	// Token: 0x040000F2 RID: 242
	private GameObject Armi;

	// Token: 0x040000F3 RID: 243
	private List<List<float>> ListaArmiUnitàConsiderata;

	// Token: 0x040000F4 RID: 244
	public List<GameObject> ListaOrdigniTotali;

	// Token: 0x040000F5 RID: 245
	private List<List<GameObject>> ListaTipiMunizioniArmiUnitàCons;

	// Token: 0x040000F6 RID: 246
	public int pulsanteColonnaArma;

	// Token: 0x040000F7 RID: 247
	public int pulsanteRigaArma;

	// Token: 0x040000F8 RID: 248
	private List<int> ListaMatricePulsantiArmi;

	// Token: 0x040000F9 RID: 249
	public bool salvaArmaSelez;

	// Token: 0x040000FA RID: 250
	public int numColonnaArmaDaSalvare;

	// Token: 0x040000FB RID: 251
	public bool aggArmi;

	// Token: 0x040000FC RID: 252
	public List<GameObject> ListaEserAlleatiAttivi;

	// Token: 0x040000FD RID: 253
	private GameObject unitàConsiderata;

	// Token: 0x040000FE RID: 254
	public bool sblocchiAggiornati;

	// Token: 0x040000FF RID: 255
	public int tipoElenco;

	// Token: 0x04000100 RID: 256
	private List<int> ListaTipiPresentiInVisualEser;

	// Token: 0x04000101 RID: 257
	private List<int> ListaTipiPresentiInVisualSecondoEser;

	// Token: 0x04000102 RID: 258
	public bool controlloEserVuoti;

	// Token: 0x04000103 RID: 259
	public int numUnitàInUnEser;

	// Token: 0x04000104 RID: 260
	public bool scambioFraEserciti;

	// Token: 0x04000105 RID: 261
	public bool aggScambioEserciti;

	// Token: 0x04000106 RID: 262
	public bool effettuaScambio;

	// Token: 0x04000107 RID: 263
	public Color coloreNonSelScambioEser;

	// Token: 0x04000108 RID: 264
	public Color coloreSelScambioEser;

	// Token: 0x04000109 RID: 265
	public int eserDellaTruppaSel;

	// Token: 0x0400010A RID: 266
	public bool rinominaEser;

	// Token: 0x0400010B RID: 267
	public int tipoEserDaRinominare;
}
