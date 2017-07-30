using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020000C8 RID: 200
public class SalvaDati : MonoBehaviour
{
	// Token: 0x060006E8 RID: 1768 RVA: 0x000F8468 File Offset: 0x000F6668
	private void Start()
	{
		if (!base.GetComponent<OltreScene>().scenaDiMenu)
		{
			if (base.GetComponent<OltreScene>().èInStrategia)
			{
				this.cameraCasa = GameObject.FindGameObjectWithTag("MainCamera");
				this.headquarters = GameObject.FindGameObjectWithTag("Headquarters");
				this.nest = GameObject.FindGameObjectWithTag("Nest");
				this.finestraSalva = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Salva").gameObject;
				this.listaPulsantiSlot = this.finestraSalva.transform.GetChild(0).FindChild("Lista Pulsanti Slot").gameObject;
				this.scrittaNomeSalvataggio = GameObject.FindGameObjectWithTag("scrittaSalvaConNome").gameObject;
				this.pulsanteSalva = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Salva").GetChild(0).FindChild("Salva").gameObject;
				this.scrittaSalvataggioAvvenuto = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Scritte Varie").FindChild("sfondo scritta salvataggio avvenuto").GetChild(0).gameObject;
			}
			else
			{
				this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
				this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
				this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
			}
		}
		else if (GameObject.FindGameObjectWithTag("CanvasBattVeloce"))
		{
			this.CanvasBattVel = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		}
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x000F85EC File Offset: 0x000F67EC
	private void Update()
	{
		if (!base.GetComponent<OltreScene>().scenaDiMenu)
		{
			this.tipoBattaglia = GestoreNeutroStrategia.tipoBattaglia;
			if (base.GetComponent<OltreScene>().èInStrategia)
			{
				if (this.finestraSalva.GetComponent<CanvasGroup>().alpha == 1f)
				{
					if (PlayerPrefs.GetInt(this.slotSuCuiSalvare + " slot è caricabile") == 0)
					{
						this.pulsanteSalva.GetComponent<Button>().interactable = false;
					}
					else
					{
						this.pulsanteSalva.GetComponent<Button>().interactable = true;
					}
					for (int i = 0; i < 9; i++)
					{
						if (PlayerPrefs.GetInt(i + " slot è caricabile") == 0)
						{
							this.listaPulsantiSlot.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = string.Empty;
							this.listaPulsantiSlot.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = string.Empty;
							this.listaPulsantiSlot.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = string.Empty;
						}
						else
						{
							this.listaPulsantiSlot.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString(i + " nome salvataggio");
							this.listaPulsantiSlot.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "turn: " + PlayerPrefs.GetInt(i + " numero turno").ToString();
							this.listaPulsantiSlot.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = PlayerPrefs.GetString(i + " data salvataggio");
						}
						if (i == this.slotSuCuiSalvare)
						{
							this.listaPulsantiSlot.transform.GetChild(i).GetComponent<Image>().color = this.coloreSelez;
						}
						else
						{
							this.listaPulsantiSlot.transform.GetChild(i).GetComponent<Image>().color = this.coloreNonSelez;
						}
					}
				}
				if (this.salvataggioAttivo)
				{
					this.SalvataggioInternoAStrategia();
					PlayerPrefs.Save();
					this.salvataggioAttivo = false;
					this.èSalvaConNome = false;
					this.timerPostSalvPartito = true;
					if (GestoreNeutroStrategia.inTattica)
					{
						this.SalvataggioDaStrategiaATattica();
						SceneManager.LoadScene("Scena Di Caricamento");
					}
				}
				if (this.timerPostSalvPartito && !this.salvataggioNascosto)
				{
					this.ScrittaSalvataggioAvvenuto();
				}
			}
			else if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().salvaDatiBattaglia)
			{
				this.slotSuCuiSalvare = 9;
				this.SalvataggioDaTatticaAStrategia();
				CaricaScene.nomeScenaDaCaricare = CaricaScene.nomeScenaCasaPerRitornoAStrategia;
				SceneManager.LoadScene("Scena Di Caricamento");
			}
		}
		else if (this.salvaPerBattVel)
		{
			this.salvaPerBattVel = false;
			this.SalvaDatiBattagliaVeloce();
			SceneManager.LoadScene("Scena Di Caricamento");
		}
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x000F88F4 File Offset: 0x000F6AF4
	private void ScrittaSalvataggioAvvenuto()
	{
		this.timerPostSalvataggio += Time.deltaTime;
		if (this.timerPostSalvataggio > 0f && this.timerPostSalvataggio < 0.3f)
		{
			this.scrittaSalvataggioAvvenuto.transform.parent.GetComponent<CanvasGroup>().alpha += Time.deltaTime * 3f;
		}
		else if (this.timerPostSalvataggio > 0.3f && this.timerPostSalvataggio < 3.3f)
		{
			this.scrittaSalvataggioAvvenuto.transform.parent.GetComponent<CanvasGroup>().alpha = 1f;
		}
		if (this.timerPostSalvataggio > 3.3f && this.timerPostSalvataggio < 3.6f)
		{
			this.scrittaSalvataggioAvvenuto.transform.parent.GetComponent<CanvasGroup>().alpha -= Time.deltaTime * 3f;
		}
		if (this.timerPostSalvataggio > 0.3f && this.timerPostSalvataggio < 1.3f)
		{
			this.scrittaSalvataggioAvvenuto.GetComponent<CanvasGroup>().alpha += Time.deltaTime;
		}
		else if (this.timerPostSalvataggio > 1.3f && this.timerPostSalvataggio < 2.3f)
		{
			this.scrittaSalvataggioAvvenuto.GetComponent<CanvasGroup>().alpha = 1f;
		}
		if (this.timerPostSalvataggio > 2.3f && this.timerPostSalvataggio < 3.3f)
		{
			this.scrittaSalvataggioAvvenuto.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
		}
		if (this.timerPostSalvataggio > 3.6f)
		{
			this.timerPostSalvataggio = 0f;
			this.timerPostSalvPartito = false;
			this.scrittaSalvataggioAvvenuto.transform.parent.GetComponent<CanvasGroup>().alpha = 0f;
			this.scrittaSalvataggioAvvenuto.GetComponent<CanvasGroup>().alpha = 0f;
			this.salvataggioNascosto = false;
		}
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x000F8B00 File Offset: 0x000F6D00
	private void SalvataggioInternoAStrategia()
	{
		if (this.èSalvaConNome)
		{
			this.nomeSalvataggio = this.scrittaNomeSalvataggio.GetComponent<Text>().text;
			this.èSalvaConNome = false;
		}
		else
		{
			this.nomeSalvataggio = PlayerPrefs.GetString(this.slotSuCuiSalvare + " nome salvataggio");
		}
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " slot è caricabile", 1);
		PlayerPrefs.SetString(this.slotSuCuiSalvare + " nome salvataggio", this.nomeSalvataggio);
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " numero turno", this.cameraCasa.GetComponent<GestoreNeutroStrategia>().numeroTurno);
		PlayerPrefs.SetString(this.slotSuCuiSalvare + " data salvataggio", DateTime.Now.ToString());
		Scene activeScene = SceneManager.GetActiveScene();
		PlayerPrefs.SetString(this.slotSuCuiSalvare + " nome livello", activeScene.name);
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " giorno", this.cameraCasa.GetComponent<GestoreNeutroStrategia>().giornoData);
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " mese", this.cameraCasa.GetComponent<GestoreNeutroStrategia>().meseData);
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " anno", this.cameraCasa.GetComponent<GestoreNeutroStrategia>().annoData);
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " salto dei giorni", this.cameraCasa.GetComponent<GestoreNeutroStrategia>().saltoGiorniPerTurno);
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " stagione di questa campagna", GestoreNeutroStrategia.stagione);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " valore vita stagione nemici", (float)(GestoreNeutroStrategia.stagione * 20));
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " valore attacco stagione nemici", (float)(GestoreNeutroStrategia.stagione * 10));
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " numero alleati morti in totale", this.cameraCasa.GetComponent<GestoreNeutroStrategia>().numAlleatiMortiinTotale);
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " numero nemici morti in totale", this.cameraCasa.GetComponent<GestoreNeutroStrategia>().numNemiciMortiinTotale);
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " livelloNest", this.nest.GetComponent<IANemicoStrategia>().livelloNest);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " giorni fittizi", this.cameraCasa.GetComponent<GestoreNeutroStrategia>().giorniDaInizioFittizi);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " plastica grezza", this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[0].GetComponent<PresenzaRisorsa>().quantitàRisorsa);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " metallo grezzo", this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[2].GetComponent<PresenzaRisorsa>().quantitàRisorsa);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " energia grezza", this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[4].GetComponent<PresenzaRisorsa>().quantitàRisorsa);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " incendiario grezzo", this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[6].GetComponent<PresenzaRisorsa>().quantitàRisorsa);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " tossico grezzo", this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[8].GetComponent<PresenzaRisorsa>().quantitàRisorsa);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " plastica raffinata", this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[1].GetComponent<PresenzaRisorsa>().quantitàRisorsa);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " metallo raffinato", this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[3].GetComponent<PresenzaRisorsa>().quantitàRisorsa);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " energia raffinata", this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[5].GetComponent<PresenzaRisorsa>().quantitàRisorsa);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " incendiario raffinato", this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[7].GetComponent<PresenzaRisorsa>().quantitàRisorsa);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " tossico raffinato", this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[9].GetComponent<PresenzaRisorsa>().quantitàRisorsa);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " esperienza", this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[10].GetComponent<PresenzaRisorsa>().quantitàRisorsa);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " fresh food presente", this.nest.GetComponent<IANemicoStrategia>().freshFoodPresente);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " fresh food esterno", this.nest.GetComponent<IANemicoStrategia>().freshFoodEsterno);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " fresh food batt o miss", this.nest.GetComponent<IANemicoStrategia>().freshFoodBattOMiss);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " rotten food presente formiche swarm", this.nest.GetComponent<IANemicoStrategia>().rottenFoodPresenteFormicheSwarm);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " rotten food presente cavallette swarm", this.nest.GetComponent<IANemicoStrategia>().rottenFoodPresenteCavalletteSwarm);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " rotten food presente vespe swarm", this.nest.GetComponent<IANemicoStrategia>().rottenFoodPresenteVespaSwarm);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " rotten food esterno", this.nest.GetComponent<IANemicoStrategia>().rottenFoodEsterno);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " rotten food batt o miss", this.nest.GetComponent<IANemicoStrategia>().freshFoodBattOMiss);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " high protein food presente", this.nest.GetComponent<IANemicoStrategia>().highProteinFoodPresente);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " high protein food esterno", this.nest.GetComponent<IANemicoStrategia>().highProteinFoodEsterno);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " high protein food batt o miss", this.nest.GetComponent<IANemicoStrategia>().highProteinFoodBattOMiss);
		for (int i = 0; i < this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.Count; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				PlayerPrefs.SetInt(string.Concat(new object[]
				{
					this.slotSuCuiSalvare,
					" stanza ",
					i,
					" settore ",
					j
				}), this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[i].GetComponent<CentroStanza>().ListaSettori[j]);
			}
			PlayerPrefs.SetInt(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" stanza ",
				i,
				"c'è stata battaglia"
			}), this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[i].GetComponent<CentroStanza>().quiCèStataBattaglia);
		}
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " numero eserciti alleati attivi", this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi.Count);
		for (int k = 0; k < this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi.Count; k++)
		{
			PlayerPrefs.SetFloat(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" esercito alleato numero ",
				k,
				" posizioneX"
			}), this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[k].transform.position.x);
			PlayerPrefs.SetFloat(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" esercito alleato numero ",
				k,
				" posizioneY"
			}), this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[k].transform.position.y);
			PlayerPrefs.SetFloat(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" esercito alleato numero ",
				k,
				" posizioneZ"
			}), this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[k].transform.position.z);
			for (int l = 0; l < 30; l++)
			{
				PlayerPrefs.SetInt(string.Concat(new object[]
				{
					this.slotSuCuiSalvare,
					" esercito alleato ",
					k,
					" elemento ",
					l
				}), this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[k].GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[l]);
			}
			PlayerPrefs.SetString(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" esercito alleato ",
				k,
				" nome"
			}), this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[k].GetComponent<PresenzaAlleataStrategica>().nomeEsercito);
			PlayerPrefs.SetInt(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" esercito alleato ",
				k,
				" identità"
			}), this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[k].GetComponent<PresenzaAlleataStrategica>().numIdentitàAlleato);
			PlayerPrefs.SetInt(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" esercito alleato ",
				k,
				" primoTurnoPerCanc"
			}), this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[k].GetComponent<PresenzaAlleataStrategica>().primoTurnoPerCanc);
			if (!this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[k].GetComponent<PresenzaAlleataStrategica>().puòAncoraMuoversi)
			{
				PlayerPrefs.SetInt(string.Concat(new object[]
				{
					this.slotSuCuiSalvare,
					" esercito alleato ",
					k,
					" può ancora muoversi"
				}), 0);
			}
			else
			{
				PlayerPrefs.SetInt(string.Concat(new object[]
				{
					this.slotSuCuiSalvare,
					" esercito alleato ",
					k,
					" può ancora muoversi"
				}), 1);
			}
		}
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " sequenza numero nome eserciti alleati", this.headquarters.GetComponent<GestioneEsercitiAlleati>().sequenzaNumNomeEser);
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " numero eserciti nemici", this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici.Count);
		for (int m = 0; m < this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici.Count; m++)
		{
			PlayerPrefs.SetFloat(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" esercito nemico numero ",
				m,
				" posizioneX"
			}), this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[m].transform.position.x);
			PlayerPrefs.SetFloat(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" esercito nemico numero ",
				m,
				" posizioneY"
			}), this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[m].transform.position.y);
			PlayerPrefs.SetFloat(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" esercito nemico numero ",
				m,
				" posizioneZ"
			}), this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[m].transform.position.z);
			PlayerPrefs.SetInt(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" esercito nemico numero ",
				m,
				" numero stanza"
			}), this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.IndexOf(this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[m].GetComponent<PresenzaNemicaStrategica>().posizioneAttuale));
			for (int n = 0; n < this.nest.GetComponent<IANemicoStrategia>().ListaTipiInsetti.Count; n++)
			{
				PlayerPrefs.SetInt(string.Concat(new object[]
				{
					this.slotSuCuiSalvare,
					" esercito nemico ",
					m,
					" elemento ",
					n,
					" tipo"
				}), this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[m].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[n][0]);
				PlayerPrefs.SetInt(string.Concat(new object[]
				{
					this.slotSuCuiSalvare,
					" esercito nemico ",
					m,
					" elemento ",
					n,
					" quantità"
				}), this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[m].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[n][1]);
			}
			PlayerPrefs.SetString(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" esercito nemico ",
				m,
				" nome"
			}), this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[m].GetComponent<PresenzaNemicaStrategica>().nomeEserInsetti);
			PlayerPrefs.SetInt(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" esercito nemico ",
				m,
				" identità"
			}), this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[m].GetComponent<PresenzaNemicaStrategica>().numIdentitàNemico);
			PlayerPrefs.SetInt(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" esercito nemico ",
				m,
				" comportamento"
			}), this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[m].GetComponent<PresenzaNemicaStrategica>().tipoComportamentoGruppo);
			PlayerPrefs.SetInt(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" esercito nemico ",
				m,
				" tipo"
			}), this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[m].GetComponent<PresenzaNemicaStrategica>().tipoDiSwarm);
			PlayerPrefs.SetInt(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" esercito nemico speciale ",
				m,
				" ha attaccato"
			}), this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[m].GetComponent<PresenzaNemicaStrategica>().swarmSpecialeHaAttaccato);
			PlayerPrefs.SetInt(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" esercito nemico ",
				m,
				" tipo di orda"
			}), this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[m].GetComponent<PresenzaNemicaStrategica>().tipoDiInsettoOrda);
		}
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " sequenza numero nome eserciti nemici", this.nest.GetComponent<IANemicoStrategia>().numerazioneEser);
		for (int num = 0; num < 16; num++)
		{
			PlayerPrefs.SetInt(this.slotSuCuiSalvare + " edificio in posto " + num, this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaPostiInHeadquarters[num]);
			PlayerPrefs.SetInt(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" edificio in posto ",
				num,
				" è acceso"
			}), this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaAccesoOSpento[num]);
		}
		for (int num2 = 0; num2 < this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.Count; num2++)
		{
			PlayerPrefs.SetInt(this.slotSuCuiSalvare + " satellite in camera " + num2, this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaSatelliti[num2]);
		}
		for (int num3 = 0; num3 < this.headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia.Count; num3++)
		{
			PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " quantità munizione " + num3, this.headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[num3].GetComponent<QuantitàMunizione>().quantità);
		}
		for (int num4 = 0; num4 < this.headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaQuantitàSupporto.Count; num4++)
		{
			PlayerPrefs.SetInt(this.slotSuCuiSalvare + " quantità supporto " + num4, this.headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaQuantitàSupporto[num4]);
		}
		for (int num5 = 0; num5 < this.headquarters.GetComponent<GestioneSblocchi>().ListaSblocchi.Count; num5++)
		{
			PlayerPrefs.SetInt(this.slotSuCuiSalvare + " sblocco " + num5, this.headquarters.GetComponent<GestioneSblocchi>().ListaSblocchi[num5].GetComponent<PresenzaSblocco>().èSbloccato);
		}
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " missione decisa", this.cameraCasa.GetComponent<GestoreNeutroStrategia>().missioneDaDecidere);
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " c'è missione", this.cameraCasa.GetComponent<GestoreNeutroStrategia>().missionePresente);
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " tipo missione", this.cameraCasa.GetComponent<GestoreNeutroStrategia>().tipoMissione);
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " stanza missione", this.cameraCasa.GetComponent<GestoreNeutroStrategia>().stanzaDiMissione);
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x000F9EEC File Offset: 0x000F80EC
	private void SalvataggioDaStrategiaATattica()
	{
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " valore vita survival nemici", this.nest.GetComponent<IANemicoStrategia>().bonusSurvivalSalute);
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " valore attacco survival nemici", this.nest.GetComponent<IANemicoStrategia>().bonusSurvivalAttacco);
		if (this.tipoBattaglia < 3)
		{
			for (int i = 0; i < 48; i++)
			{
				PlayerPrefs.SetInt(string.Concat(new object[]
				{
					this.slotSuCuiSalvare,
					" lista alleati per tattica posizione ",
					i,
					" tipo"
				}), this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().ListaAlleatiInSchermBatt[i][0]);
				PlayerPrefs.SetInt(string.Concat(new object[]
				{
					this.slotSuCuiSalvare,
					" lista alleati per tattica posizione ",
					i,
					" quantità"
				}), this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().ListaAlleatiInSchermBatt[i][1]);
			}
			for (int j = 0; j < 48; j++)
			{
				PlayerPrefs.SetInt(string.Concat(new object[]
				{
					this.slotSuCuiSalvare,
					" lista nemici per tattica posizione ",
					j,
					" tipo"
				}), this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().ListaNemiciInSchermBatt[j][0]);
				PlayerPrefs.SetInt(string.Concat(new object[]
				{
					this.slotSuCuiSalvare,
					" lista nemici per tattica posizione ",
					j,
					" quantità"
				}), this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().ListaNemiciInSchermBatt[j][1]);
			}
			if (this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().numIdentSwarmSpecialeInAtt != 0)
			{
				for (int k = 0; k < this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici.Count; k++)
				{
					if (this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[k].GetComponent<PresenzaNemicaStrategica>().numIdentitàNemico == this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().numIdentSwarmSpecialeInAtt)
					{
						PlayerPrefs.SetInt(this.slotSuCuiSalvare + " tipo di swarm", this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[k].GetComponent<PresenzaNemicaStrategica>().tipoDiInsettoOrda);
						break;
					}
				}
			}
			int num = 0;
			bool flag = false;
			int num2 = 0;
			while (num2 < this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.Count && !flag)
			{
				if (this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata == this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[num2])
				{
					num = num2;
					flag = true;
				}
				num2++;
			}
			PlayerPrefs.SetInt(this.slotSuCuiSalvare + " indice stanza battaglia", num);
			PlayerPrefs.SetInt(this.slotSuCuiSalvare + " appartenenza stanza battaglia", this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[num].GetComponent<CentroStanza>().appartenenzaBandiera);
			for (int l = 0; l < this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaDiListeDiVicinanze[num].Count; l++)
			{
				PlayerPrefs.SetInt(this.slotSuCuiSalvare + " numero identità stanza " + l, this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaDiListeDiVicinanze[num][l]);
				PlayerPrefs.SetInt(string.Concat(new object[]
				{
					this.slotSuCuiSalvare,
					" stanza vicina ",
					l,
					" appartenenza"
				}), this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaDiListeDiVicinanze[num][l]].GetComponent<CentroStanza>().appartenenzaBandiera);
			}
			for (int m = 0; m < this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi.Count; m++)
			{
				if (this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[m].GetComponent<PresenzaAlleataStrategica>().posizioneAttuale == this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata)
				{
					PlayerPrefs.SetInt(string.Concat(new object[]
					{
						this.slotSuCuiSalvare,
						" esercito alleato ",
						m,
						" partecipa a battaglia"
					}), 1);
				}
				else
				{
					PlayerPrefs.SetInt(string.Concat(new object[]
					{
						this.slotSuCuiSalvare,
						" esercito alleato ",
						m,
						" partecipa a battaglia"
					}), 0);
				}
			}
			for (int n = 0; n < this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici.Count; n++)
			{
				if (this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[n].GetComponent<PresenzaNemicaStrategica>().posizioneAttuale == this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata)
				{
					if (this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().numIdentSwarmSpecialeInAtt == 0)
					{
						if (this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[n].GetComponent<PresenzaNemicaStrategica>().tipoDiSwarm == 0)
						{
							PlayerPrefs.SetInt(string.Concat(new object[]
							{
								this.slotSuCuiSalvare,
								" esercito nemico ",
								n,
								" partecipa a battaglia"
							}), 1);
						}
						else
						{
							PlayerPrefs.SetInt(string.Concat(new object[]
							{
								this.slotSuCuiSalvare,
								" esercito nemico ",
								n,
								" partecipa a battaglia"
							}), 0);
						}
					}
					else if (this.nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[n].GetComponent<PresenzaNemicaStrategica>().numIdentitàNemico == this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().numIdentSwarmSpecialeInAtt)
					{
						PlayerPrefs.SetInt(string.Concat(new object[]
						{
							this.slotSuCuiSalvare,
							" esercito nemico ",
							n,
							" partecipa a battaglia"
						}), 1);
					}
					else
					{
						PlayerPrefs.SetInt(string.Concat(new object[]
						{
							this.slotSuCuiSalvare,
							" esercito nemico ",
							n,
							" partecipa a battaglia"
						}), 0);
					}
				}
				else
				{
					PlayerPrefs.SetInt(string.Concat(new object[]
					{
						this.slotSuCuiSalvare,
						" esercito nemico ",
						n,
						" partecipa a battaglia"
					}), 0);
				}
			}
			PlayerPrefs.SetInt(this.slotSuCuiSalvare + " num identità swarm speciale in battaglia", this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().numIdentSwarmSpecialeInAtt);
		}
		else
		{
			for (int num3 = 0; num3 < 48; num3++)
			{
				PlayerPrefs.SetInt(string.Concat(new object[]
				{
					this.slotSuCuiSalvare,
					" lista alleati per tattica posizione ",
					num3,
					" tipo"
				}), this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().ListaAlleatiInSchermBatt[num3][0]);
				PlayerPrefs.SetInt(string.Concat(new object[]
				{
					this.slotSuCuiSalvare,
					" lista alleati per tattica posizione ",
					num3,
					" quantità"
				}), this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().ListaAlleatiInSchermBatt[num3][1]);
			}
			if (this.tipoBattaglia == 4 || this.tipoBattaglia == 6 || this.tipoBattaglia == 7)
			{
				for (int num4 = 0; num4 < this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi.Count; num4++)
				{
					if (this.headquarters.GetComponent<GestioneEsercitiAlleati>().ListaEserAlleatiAttivi[num4].GetComponent<PresenzaAlleataStrategica>().posizioneAttuale == this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata)
					{
						PlayerPrefs.SetInt(string.Concat(new object[]
						{
							this.slotSuCuiSalvare,
							" esercito alleato ",
							num4,
							" partecipa a battaglia"
						}), 1);
					}
					else
					{
						PlayerPrefs.SetInt(string.Concat(new object[]
						{
							this.slotSuCuiSalvare,
							" esercito alleato ",
							num4,
							" partecipa a battaglia"
						}), 0);
					}
				}
			}
		}
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x000FA860 File Offset: 0x000F8A60
	private void SalvataggioDaTatticaAStrategia()
	{
		PlayerPrefs.SetInt(this.slotSuCuiSalvare + " numero superstiti alleati", this.infoAlleati.GetComponent<InfoGenericheAlleati>().numeroTotAlleatiRimanenti);
		for (int i = 0; i < this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiPostBattaglia.Count; i++)
		{
			PlayerPrefs.SetInt(string.Concat(new object[]
			{
				this.slotSuCuiSalvare,
				" lista alleati rimanenti posizione ",
				i,
				" quantità"
			}), this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleatiPostBattaglia[i]);
		}
		for (int j = 0; j < this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAllPresInBatt.Count; j++)
		{
			PlayerPrefs.SetInt(this.slotSuCuiSalvare + " alleato presente in battaglia " + j, this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAllPresInBatt[j]);
			PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " danno alleato " + j, this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati[j]);
		}
		for (int k = 0; k < this.infoAlleati.GetComponent<InfoMunizionamento>().ListaTipiMunizioniBaseTattica.Count; k++)
		{
			PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " quantità munizione " + k, this.infoAlleati.GetComponent<InfoMunizionamento>().ListaTipiMunizioniBaseTattica[k].GetComponent<QuantitàMunizione>().quantità);
		}
		for (int l = 0; l < this.infoAlleati.GetComponent<InfoMunizionamento>().ListaQuantitàSupportoTattica.Count; l++)
		{
			PlayerPrefs.SetInt(this.slotSuCuiSalvare + " quantità supporto " + l, this.infoAlleati.GetComponent<InfoMunizionamento>().ListaQuantitàSupportoTattica[l]);
		}
		if (this.tipoBattaglia == 0 || this.tipoBattaglia == 1 || this.tipoBattaglia == 2)
		{
			PlayerPrefs.SetInt(this.slotSuCuiSalvare + " numero superstiti nemici", this.IANemico.GetComponent<InfoGenericheNemici>().numeroTotNemiciRimanenti);
			for (int m = 0; m < this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciPostBattaglia.Count; m++)
			{
				PlayerPrefs.SetInt(string.Concat(new object[]
				{
					this.slotSuCuiSalvare,
					" lista nemici rimanenti posizione ",
					m,
					" quantità"
				}), this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemiciPostBattaglia[m]);
			}
			for (int n = 0; n < this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemPresInBatt.Count; n++)
			{
				PlayerPrefs.SetInt(this.slotSuCuiSalvare + " nemico presente in battaglia " + n, this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemPresInBatt[n]);
				PlayerPrefs.SetFloat(this.slotSuCuiSalvare + " danno nemico " + n, this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici[n]);
			}
		}
		else if (this.tipoBattaglia == 3)
		{
			PlayerPrefs.SetInt(this.slotSuCuiSalvare + " soldati salvati in battaglia 3", this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().soldatiSalvatiInBatt3);
		}
		PlayerPrefs.SetFloat(this.slotSuCuiSalvare + "totale per Exp", this.IANemico.GetComponent<InfoGenericheNemici>().totalePerExpBatt);
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x000FAC24 File Offset: 0x000F8E24
	private void SalvaDatiBattagliaVeloce()
	{
		for (int i = 0; i < 48; i++)
		{
			PlayerPrefs.SetInt("lista alleati per batt vel posizione " + i + " tipo", i);
			PlayerPrefs.SetInt("lista alleati per batt vel posizione " + i + " quantità", this.CanvasBattVel.GetComponent<BattagliaVeloceScript>().ListaAlleatiQuantitàBattVel[i]);
		}
		for (int j = 0; j < 48; j++)
		{
			if (j < this.CanvasBattVel.GetComponent<BattagliaVeloceScript>().ListaNemici.Count)
			{
				PlayerPrefs.SetInt("lista nemici per batt vel posizione " + j + " tipo", j);
				PlayerPrefs.SetInt("lista nemici per batt vel posizione " + j + " quantità", this.CanvasBattVel.GetComponent<BattagliaVeloceScript>().ListaNemiciQuantitàBattVel[j]);
			}
			else
			{
				PlayerPrefs.SetInt("lista nemici per batt vel posizione " + j + " tipo", 100);
				PlayerPrefs.SetInt("lista nemici per batt vel posizione " + j + " quantità", 0);
			}
		}
		PlayerPrefs.SetInt("batt vel imp numero alleati", this.CanvasBattVel.GetComponent<BattagliaVeloceScript>().valoreRealeMaxAlleati);
		PlayerPrefs.SetInt("batt vel imp numero nemici", this.CanvasBattVel.GetComponent<BattagliaVeloceScript>().valoreRealeMaxNemici);
		PlayerPrefs.SetFloat("batt vel imp vita nemici", this.CanvasBattVel.GetComponent<BattagliaVeloceScript>().impVitaNemici.GetComponent<Slider>().value);
		PlayerPrefs.SetFloat("batt vel imp attacco nemici", this.CanvasBattVel.GetComponent<BattagliaVeloceScript>().impAttaccoNemici.GetComponent<Slider>().value);
	}

	// Token: 0x040019DD RID: 6621
	public bool salvataggioAttivo;

	// Token: 0x040019DE RID: 6622
	public bool salvaPerBattVel;

	// Token: 0x040019DF RID: 6623
	public int slotSuCuiSalvare;

	// Token: 0x040019E0 RID: 6624
	public string nomeSalvataggio;

	// Token: 0x040019E1 RID: 6625
	public bool èSalvaConNome;

	// Token: 0x040019E2 RID: 6626
	private float timerPostSalvataggio;

	// Token: 0x040019E3 RID: 6627
	private bool timerPostSalvPartito;

	// Token: 0x040019E4 RID: 6628
	private bool rimbalzoVisibilitàScritta;

	// Token: 0x040019E5 RID: 6629
	private GameObject cameraCasa;

	// Token: 0x040019E6 RID: 6630
	private GameObject headquarters;

	// Token: 0x040019E7 RID: 6631
	private GameObject nest;

	// Token: 0x040019E8 RID: 6632
	private GameObject finestraSalva;

	// Token: 0x040019E9 RID: 6633
	private GameObject listaPulsantiSlot;

	// Token: 0x040019EA RID: 6634
	private GameObject scrittaNomeSalvataggio;

	// Token: 0x040019EB RID: 6635
	private GameObject pulsanteSalva;

	// Token: 0x040019EC RID: 6636
	private GameObject scrittaSalvataggioAvvenuto;

	// Token: 0x040019ED RID: 6637
	private GameObject inizioLivello;

	// Token: 0x040019EE RID: 6638
	private GameObject infoNeutreTattica;

	// Token: 0x040019EF RID: 6639
	private GameObject infoAlleati;

	// Token: 0x040019F0 RID: 6640
	private GameObject IANemico;

	// Token: 0x040019F1 RID: 6641
	private GameObject CanvasBattVel;

	// Token: 0x040019F2 RID: 6642
	public Color coloreSelez;

	// Token: 0x040019F3 RID: 6643
	public Color coloreNonSelez;

	// Token: 0x040019F4 RID: 6644
	public bool salvataggioNascosto;

	// Token: 0x040019F5 RID: 6645
	private int tipoBattaglia;
}
