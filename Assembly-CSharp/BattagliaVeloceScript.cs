using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C2 RID: 194
public class BattagliaVeloceScript : MonoBehaviour
{
	// Token: 0x060006C0 RID: 1728 RVA: 0x000ED9B4 File Offset: 0x000EBBB4
	private void Start()
	{
		this.descrMissione = base.gameObject.transform.GetChild(0).FindChild("sezione missioni").GetChild(1).GetChild(0).gameObject;
		this.oggettoListaMissioni = base.gameObject.transform.GetChild(0).FindChild("sezione missioni").FindChild("Pulsante lista missioni").GetChild(1).gameObject;
		this.oggettoListaDurata = base.gameObject.transform.GetChild(0).FindChild("sezione tempo").FindChild("Pulsante lista tempo").GetChild(1).gameObject;
		this.immagineCasa = base.gameObject.transform.GetChild(0).FindChild("sezione mappa").GetChild(1).gameObject;
		this.oggettoListaCase = base.gameObject.transform.GetChild(0).FindChild("sezione mappa").FindChild("Pulsante lista case").GetChild(1).gameObject;
		this.immagineStanza = base.gameObject.transform.GetChild(0).FindChild("sezione mappa").GetChild(4).gameObject;
		this.oggettoListaStanze = base.gameObject.transform.GetChild(0).FindChild("sezione mappa").FindChild("Pulsante lista stanze").GetChild(1).gameObject;
		this.schedaAlleati = base.gameObject.transform.GetChild(0).FindChild("sezione unità").GetChild(1).gameObject;
		this.schedaNemici = base.gameObject.transform.GetChild(0).FindChild("sezione unità").GetChild(0).gameObject;
		this.contSchedaAlleati = this.schedaAlleati.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
		this.contSchedaNemici = this.schedaNemici.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
		this.dettagliAlleato = base.gameObject.transform.FindChild("Dettagli Veloci Unità").gameObject;
		this.pulsanteArmi = this.dettagliAlleato.transform.FindChild("Armi").gameObject;
		this.dettagliNemico = base.gameObject.transform.FindChild("Dettagli Veloci Insetto").gameObject;
		this.Armi = base.gameObject.transform.FindChild("Armi").gameObject;
		this.scrittaNonSceltaReclAlleati = this.schedaAlleati.transform.GetChild(1).gameObject;
		this.scrittaNonSceltaReclNemici = this.schedaNemici.transform.GetChild(1).gameObject;
		this.scrittaMappaNonHaConvoglio = base.gameObject.transform.GetChild(0).FindChild("sezione mappa").GetChild(6).gameObject;
		this.pulsanteInizia = base.gameObject.transform.FindChild("pulsante Inizia").gameObject;
		this.scrittaMaxNemici = base.gameObject.transform.GetChild(0).FindChild("sezione impostazioni").GetChild(0).GetChild(0).GetChild(0).gameObject;
		this.scrittaMaxAlleati = base.gameObject.transform.GetChild(0).FindChild("sezione impostazioni").GetChild(0).GetChild(1).GetChild(0).gameObject;
		this.impMaxNemici = base.gameObject.transform.GetChild(0).FindChild("sezione impostazioni").GetChild(0).GetChild(0).GetChild(1).gameObject;
		this.impMaxAlleati = base.gameObject.transform.GetChild(0).FindChild("sezione impostazioni").GetChild(0).GetChild(1).GetChild(1).gameObject;
		this.scrittaVitaNemici = base.gameObject.transform.GetChild(0).FindChild("sezione impostazioni").GetChild(1).GetChild(0).gameObject;
		this.impVitaNemici = base.gameObject.transform.GetChild(0).FindChild("sezione impostazioni").GetChild(1).GetChild(1).gameObject;
		this.scrittaAttaccoNemici = base.gameObject.transform.GetChild(0).FindChild("sezione impostazioni").GetChild(2).GetChild(0).gameObject;
		this.impAttaccoNemici = base.gameObject.transform.GetChild(0).FindChild("sezione impostazioni").GetChild(2).GetChild(1).gameObject;
		this.battVelAggMissione = true;
		this.battVelAggDurataBatt = true;
		this.battVelAggMappa = true;
		this.battVelAggAlleati = true;
		this.battVelAggNemici = true;
		this.ListaAlleatiQuantitàBattVel = new List<int>();
		for (int i = 0; i < this.ListaAlleati.Count; i++)
		{
			this.contSchedaAlleati.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite = this.ListaAlleati[i].GetComponent<PresenzaAlleato>().immagineUnità;
			this.contSchedaAlleati.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = this.ListaAlleati[i].GetComponent<PresenzaAlleato>().nomeUnità;
			this.contSchedaAlleati.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = "0";
			this.ListaAlleatiQuantitàBattVel.Add(0);
		}
		this.ListaNemiciQuantitàBattVel = new List<int>();
		for (int j = 0; j < this.ListaNemici.Count; j++)
		{
			this.contSchedaNemici.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Image>().sprite = this.ListaNemici[j].GetComponent<PresenzaNemico>().immagineInsetto;
			this.contSchedaNemici.transform.GetChild(j).GetChild(1).GetComponent<Text>().text = this.ListaNemici[j].GetComponent<PresenzaNemico>().nomeInsetto;
			this.contSchedaNemici.transform.GetChild(j).GetChild(2).GetComponent<Text>().text = "0";
			this.ListaNemiciQuantitàBattVel.Add(0);
		}
		this.ListaMatricePulsantiArmi = new List<int>();
		this.ListaMatricePulsantiArmi.Add(0);
		this.ListaMatricePulsantiArmi.Add(0);
		this.ListaMatricePulsantiArmi.Add(0);
		this.ListaMatricePulsantiArmi.Add(0);
		this.impMaxNemici.GetComponent<Slider>().value = 400f;
		this.impMaxAlleati.GetComponent<Slider>().value = 80f;
		this.impVitaNemici.GetComponent<Slider>().value = 100f;
		this.impAttaccoNemici.GetComponent<Slider>().value = 100f;
		this.numListaDurata = PlayerPrefs.GetInt("durata battaglia");
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x000EE0D4 File Offset: 0x000EC2D4
	private void Update()
	{
		if (this.battVelAggMissione)
		{
			this.battVelAggMissione = false;
			this.AggiornaMissione();
			this.clickSuListaMissioni = false;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			this.oggettoListaMissioni.GetComponent<CanvasGroup>().alpha = 0f;
			this.oggettoListaMissioni.GetComponent<CanvasGroup>().interactable = false;
			this.oggettoListaMissioni.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		if (this.battVelAggDurataBatt)
		{
			this.battVelAggDurataBatt = false;
			this.AggiornaDurataBattaglia();
			this.clickSuListaDurata = false;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			this.oggettoListaDurata.GetComponent<CanvasGroup>().alpha = 0f;
			this.oggettoListaDurata.GetComponent<CanvasGroup>().interactable = false;
			this.oggettoListaDurata.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		if (this.battVelAggMappa)
		{
			this.battVelAggMappa = false;
			this.AggiornaMappa();
			this.cambioCasa = false;
			this.clickSuListaCase = false;
			this.clickSuListaStanze = false;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			this.oggettoListaCase.GetComponent<CanvasGroup>().alpha = 0f;
			this.oggettoListaCase.GetComponent<CanvasGroup>().interactable = false;
			this.oggettoListaCase.GetComponent<CanvasGroup>().blocksRaycasts = false;
			this.oggettoListaStanze.GetComponent<CanvasGroup>().alpha = 0f;
			this.oggettoListaStanze.GetComponent<CanvasGroup>().interactable = false;
			this.oggettoListaStanze.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		if (this.battVelAggNemici)
		{
			this.battVelAggNemici = false;
			this.AggiornaNemici();
			this.battVelAggiungiNemico = false;
			this.battVelTogliNemico = false;
			this.resettaNemici = false;
		}
		if (this.battVelAggAlleati)
		{
			this.battVelAggAlleati = false;
			this.AggiornaAlleati();
			this.battVelAggiungiAlleato = false;
			this.battVelTogliAlleato = false;
			this.resettaAlleati = false;
		}
		if (this.battVelInfoNemico)
		{
			this.battVelInfoNemico = false;
			this.DettagliInfoNemico();
		}
		if (this.battVelInfoAlleato)
		{
			this.battVelInfoAlleato = false;
			this.DettagliInfoAlleato();
		}
		if (this.armiAperto)
		{
			this.VisualizzaArmi();
		}
		this.OpzioniBattVeloce();
		this.ControlloPerAvvio();
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x000EE300 File Offset: 0x000EC500
	private void AggiornaMissione()
	{
		this.descrMissione.GetComponent<Text>().text = this.ListaDescrMissioni[this.numListaMissioni].GetComponent<Text>().text;
		this.oggettoListaMissioni.transform.parent.GetChild(0).GetComponent<Text>().text = this.oggettoListaMissioni.transform.GetChild(this.numListaMissioni).transform.GetChild(0).GetComponent<Text>().text;
		if (this.clickSuListaMissioni)
		{
			if (this.oggettoListaMissioni.GetComponent<CanvasGroup>().alpha == 0f)
			{
				this.oggettoListaMissioni.GetComponent<CanvasGroup>().alpha = 1f;
				this.oggettoListaMissioni.GetComponent<CanvasGroup>().interactable = true;
				this.oggettoListaMissioni.GetComponent<CanvasGroup>().blocksRaycasts = true;
			}
			else
			{
				this.oggettoListaMissioni.GetComponent<CanvasGroup>().alpha = 0f;
				this.oggettoListaMissioni.GetComponent<CanvasGroup>().interactable = false;
				this.oggettoListaMissioni.GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
		}
		GestoreNeutroStrategia.tipoBattaglia = this.numListaMissioni;
		if (GestoreNeutroStrategia.tipoBattaglia == 3)
		{
			this.libertàDiReclAlleati = false;
			this.libertàDiSceltaNemici = true;
			this.ForzaturaAlleatiPerMappa();
			this.scrittaNonSceltaReclAlleati.GetComponent<CanvasGroup>().alpha = 1f;
			this.scrittaNonSceltaReclNemici.GetComponent<CanvasGroup>().alpha = 0f;
		}
		else if (GestoreNeutroStrategia.tipoBattaglia == 5)
		{
			this.libertàDiReclAlleati = false;
			this.libertàDiSceltaNemici = true;
			this.ForzaturaAlleatiPerMappa();
			this.scrittaNonSceltaReclAlleati.GetComponent<CanvasGroup>().alpha = 1f;
			this.scrittaNonSceltaReclNemici.GetComponent<CanvasGroup>().alpha = 0f;
		}
		else
		{
			this.libertàDiReclAlleati = true;
			this.libertàDiSceltaNemici = true;
			this.scrittaNonSceltaReclAlleati.GetComponent<CanvasGroup>().alpha = 0f;
			this.scrittaNonSceltaReclNemici.GetComponent<CanvasGroup>().alpha = 0f;
		}
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x000EE4FC File Offset: 0x000EC6FC
	private void ForzaturaAlleatiPerMappa()
	{
		for (int i = 0; i < this.ListaAlleati.Count; i++)
		{
			if (GestoreNeutroStrategia.tipoBattaglia == 3)
			{
				this.ListaAlleatiQuantitàBattVel[i] = 0;
			}
		}
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x000EE540 File Offset: 0x000EC740
	private void AggiornaDurataBattaglia()
	{
		if (this.clickSuListaDurata)
		{
			if (this.oggettoListaDurata.GetComponent<CanvasGroup>().alpha == 0f)
			{
				this.oggettoListaDurata.GetComponent<CanvasGroup>().alpha = 1f;
				this.oggettoListaDurata.GetComponent<CanvasGroup>().interactable = true;
				this.oggettoListaDurata.GetComponent<CanvasGroup>().blocksRaycasts = true;
			}
			else
			{
				this.oggettoListaDurata.GetComponent<CanvasGroup>().alpha = 0f;
				this.oggettoListaDurata.GetComponent<CanvasGroup>().interactable = false;
				this.oggettoListaDurata.GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
		}
		PlayerPrefs.SetInt("durata battaglia", this.numListaDurata);
		this.oggettoListaDurata.transform.parent.GetChild(0).GetComponent<Text>().text = this.oggettoListaDurata.transform.GetChild(this.numListaDurata).transform.GetChild(0).GetComponent<Text>().text;
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x000EE640 File Offset: 0x000EC840
	private void AggiornaMappa()
	{
		if (this.cambioCasa)
		{
			this.numeroStanza = 0;
		}
		if (this.clickSuListaCase)
		{
			if (this.oggettoListaCase.GetComponent<CanvasGroup>().alpha == 0f)
			{
				this.oggettoListaCase.GetComponent<CanvasGroup>().alpha = 1f;
				this.oggettoListaCase.GetComponent<CanvasGroup>().interactable = true;
				this.oggettoListaCase.GetComponent<CanvasGroup>().blocksRaycasts = true;
			}
			else
			{
				this.oggettoListaCase.GetComponent<CanvasGroup>().alpha = 0f;
				this.oggettoListaCase.GetComponent<CanvasGroup>().interactable = false;
				this.oggettoListaCase.GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
		}
		if (this.clickSuListaStanze)
		{
			if (this.oggettoListaStanze.GetComponent<CanvasGroup>().alpha == 0f)
			{
				this.oggettoListaStanze.GetComponent<CanvasGroup>().alpha = 1f;
				this.oggettoListaStanze.GetComponent<CanvasGroup>().interactable = true;
				this.oggettoListaStanze.GetComponent<CanvasGroup>().blocksRaycasts = true;
			}
			else
			{
				this.oggettoListaStanze.GetComponent<CanvasGroup>().alpha = 0f;
				this.oggettoListaStanze.GetComponent<CanvasGroup>().interactable = false;
				this.oggettoListaStanze.GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
		}
		for (int i = 0; i < this.oggettoListaCase.transform.childCount; i++)
		{
			if (i < this.ListaCase.Count)
			{
				this.oggettoListaCase.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = this.ListaCase[i].GetComponent<InfoCasa>().nomeCasa;
				this.oggettoListaCase.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 1f;
				this.oggettoListaCase.transform.GetChild(i).GetComponent<CanvasGroup>().interactable = true;
				this.oggettoListaCase.transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = true;
			}
			else
			{
				this.oggettoListaCase.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 0f;
				this.oggettoListaCase.transform.GetChild(i).GetComponent<CanvasGroup>().interactable = false;
				this.oggettoListaCase.transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
		}
		this.oggettoListaCase.transform.parent.GetChild(0).GetComponent<Text>().text = this.ListaCase[this.numeroCasa].GetComponent<InfoCasa>().nomeCasa;
		this.immagineCasa.GetComponent<Image>().sprite = this.ListaCase[this.numeroCasa].GetComponent<InfoCasa>().immagineCasa;
		for (int j = 0; j < this.oggettoListaStanze.transform.childCount; j++)
		{
			if (j < this.ListaCase[this.numeroCasa].GetComponent<InfoCasa>().ListaStanzeDiCasa.Count)
			{
				this.oggettoListaStanze.transform.GetChild(j).GetChild(0).GetComponent<Text>().text = this.ListaCase[this.numeroCasa].GetComponent<InfoCasa>().ListaStanzeDiCasa[j].GetComponent<InfoStanza>().nomeStanza;
				this.oggettoListaStanze.transform.GetChild(j).GetComponent<CanvasGroup>().alpha = 1f;
				this.oggettoListaStanze.transform.GetChild(j).GetComponent<CanvasGroup>().interactable = true;
				this.oggettoListaStanze.transform.GetChild(j).GetComponent<CanvasGroup>().blocksRaycasts = true;
			}
			else
			{
				this.oggettoListaStanze.transform.GetChild(j).GetComponent<CanvasGroup>().alpha = 0f;
				this.oggettoListaStanze.transform.GetChild(j).GetComponent<CanvasGroup>().interactable = false;
				this.oggettoListaStanze.transform.GetChild(j).GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
		}
		this.oggettoListaStanze.transform.parent.GetChild(0).GetComponent<Text>().text = this.ListaCase[this.numeroCasa].GetComponent<InfoCasa>().ListaStanzeDiCasa[this.numeroStanza].GetComponent<InfoStanza>().nomeStanza;
		this.immagineStanza.GetComponent<Image>().sprite = this.ListaCase[this.numeroCasa].GetComponent<InfoCasa>().ListaStanzeDiCasa[this.numeroStanza].GetComponent<InfoStanza>().immagineStanza;
		CaricaScene.nomeScenaDaCaricare = this.ListaCase[this.numeroCasa].GetComponent<InfoCasa>().ListaStanzeDiCasa[this.numeroStanza].name;
		if (GestoreNeutroStrategia.tipoBattaglia == 6 || GestoreNeutroStrategia.tipoBattaglia == 7)
		{
			if (!this.ListaCase[this.numeroCasa].GetComponent<InfoCasa>().ListaStanzeDiCasa[this.numeroStanza].GetComponent<InfoStanza>().convogliPossibili)
			{
				this.scrittaMappaNonHaConvoglio.GetComponent<CanvasGroup>().alpha = 1f;
			}
			else
			{
				this.scrittaMappaNonHaConvoglio.GetComponent<CanvasGroup>().alpha = 0f;
			}
		}
		else
		{
			this.scrittaMappaNonHaConvoglio.GetComponent<CanvasGroup>().alpha = 0f;
		}
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x000EEBB0 File Offset: 0x000ECDB0
	private void AggiornaNemici()
	{
		this.schedaNemici.transform.SetSiblingIndex(1);
		if (this.battVelAggiungiNemico)
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				List<int> listaNemiciQuantitàBattVel;
				List<int> expr_31 = listaNemiciQuantitàBattVel = this.ListaNemiciQuantitàBattVel;
				int num;
				int expr_39 = num = this.nemicoSelez;
				num = listaNemiciQuantitàBattVel[num];
				expr_31[expr_39] = num + 1;
			}
			else
			{
				List<int> listaNemiciQuantitàBattVel2;
				List<int> expr_5A = listaNemiciQuantitàBattVel2 = this.ListaNemiciQuantitàBattVel;
				int num;
				int expr_63 = num = this.nemicoSelez;
				num = listaNemiciQuantitàBattVel2[num];
				expr_5A[expr_63] = num + 10;
			}
		}
		else if (this.battVelTogliNemico && this.ListaNemiciQuantitàBattVel[this.nemicoSelez] > 0)
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				List<int> listaNemiciQuantitàBattVel3;
				List<int> expr_B7 = listaNemiciQuantitàBattVel3 = this.ListaNemiciQuantitàBattVel;
				int num;
				int expr_C0 = num = this.nemicoSelez;
				num = listaNemiciQuantitàBattVel3[num];
				expr_B7[expr_C0] = num - 1;
			}
			else if (this.ListaNemiciQuantitàBattVel[this.nemicoSelez] >= 10)
			{
				List<int> listaNemiciQuantitàBattVel4;
				List<int> expr_FA = listaNemiciQuantitàBattVel4 = this.ListaNemiciQuantitàBattVel;
				int num;
				int expr_103 = num = this.nemicoSelez;
				num = listaNemiciQuantitàBattVel4[num];
				expr_FA[expr_103] = num - 10;
			}
		}
		if (this.resettaAlleati)
		{
			for (int i = 0; i < this.ListaAlleati.Count; i++)
			{
				this.ListaAlleatiQuantitàBattVel[i] = 0;
			}
		}
		if (this.resettaNemici)
		{
			for (int j = 0; j < this.ListaNemici.Count; j++)
			{
				this.ListaNemiciQuantitàBattVel[j] = 0;
			}
		}
		this.totaleNemici = 0;
		for (int k = 0; k < this.ListaNemici.Count; k++)
		{
			this.contSchedaNemici.transform.GetChild(k).GetChild(2).GetComponent<Text>().text = this.ListaNemiciQuantitàBattVel[k].ToString();
			this.totaleNemici += this.ListaNemiciQuantitàBattVel[k];
			if (this.libertàDiSceltaNemici)
			{
				if (GestoreNeutroStrategia.tipoBattaglia == 5)
				{
					if (this.ListaNemici[k].GetComponent<PresenzaNemico>().insettoVolante)
					{
						this.contSchedaNemici.transform.GetChild(k).GetChild(3).GetComponent<Button>().interactable = true;
						this.contSchedaNemici.transform.GetChild(k).GetChild(4).GetComponent<Button>().interactable = true;
					}
					else
					{
						this.contSchedaNemici.transform.GetChild(k).GetChild(3).GetComponent<Button>().interactable = false;
						this.contSchedaNemici.transform.GetChild(k).GetChild(4).GetComponent<Button>().interactable = false;
						this.ListaNemiciQuantitàBattVel[k] = 0;
					}
				}
				else
				{
					this.contSchedaNemici.transform.GetChild(k).GetChild(3).GetComponent<Button>().interactable = true;
					this.contSchedaNemici.transform.GetChild(k).GetChild(4).GetComponent<Button>().interactable = true;
				}
			}
			else
			{
				this.contSchedaNemici.transform.GetChild(k).GetChild(3).GetComponent<Button>().interactable = false;
				this.contSchedaNemici.transform.GetChild(k).GetChild(4).GetComponent<Button>().interactable = false;
			}
		}
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x000EEF10 File Offset: 0x000ED110
	private void AggiornaAlleati()
	{
		this.schedaAlleati.transform.SetSiblingIndex(1);
		if (this.battVelAggiungiAlleato)
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				List<int> listaAlleatiQuantitàBattVel;
				List<int> expr_31 = listaAlleatiQuantitàBattVel = this.ListaAlleatiQuantitàBattVel;
				int num;
				int expr_39 = num = this.alleatoSelez;
				num = listaAlleatiQuantitàBattVel[num];
				expr_31[expr_39] = num + 1;
			}
			else
			{
				List<int> listaAlleatiQuantitàBattVel2;
				List<int> expr_56 = listaAlleatiQuantitàBattVel2 = this.ListaAlleatiQuantitàBattVel;
				int num;
				int expr_5F = num = this.alleatoSelez;
				num = listaAlleatiQuantitàBattVel2[num];
				expr_56[expr_5F] = num + 10;
			}
		}
		else if (this.battVelTogliAlleato && this.ListaAlleatiQuantitàBattVel[this.alleatoSelez] > 0)
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				List<int> listaAlleatiQuantitàBattVel3;
				List<int> expr_AF = listaAlleatiQuantitàBattVel3 = this.ListaAlleatiQuantitàBattVel;
				int num;
				int expr_B8 = num = this.alleatoSelez;
				num = listaAlleatiQuantitàBattVel3[num];
				expr_AF[expr_B8] = num - 1;
			}
			else if (this.ListaAlleatiQuantitàBattVel[this.alleatoSelez] >= 10)
			{
				List<int> listaAlleatiQuantitàBattVel4;
				List<int> expr_EE = listaAlleatiQuantitàBattVel4 = this.ListaAlleatiQuantitàBattVel;
				int num;
				int expr_F7 = num = this.alleatoSelez;
				num = listaAlleatiQuantitàBattVel4[num];
				expr_EE[expr_F7] = num - 10;
			}
		}
		if (this.resettaAlleati)
		{
			for (int i = 0; i < this.ListaAlleati.Count; i++)
			{
				this.ListaAlleatiQuantitàBattVel[i] = 0;
			}
		}
		this.totaleAlleati = 0;
		for (int j = 0; j < this.ListaAlleati.Count; j++)
		{
			this.contSchedaAlleati.transform.GetChild(j).GetChild(2).GetComponent<Text>().text = this.ListaAlleatiQuantitàBattVel[j].ToString();
			this.totaleAlleati += this.ListaAlleatiQuantitàBattVel[j];
			if (this.libertàDiReclAlleati)
			{
				this.contSchedaAlleati.transform.GetChild(j).GetChild(3).GetComponent<Button>().interactable = true;
				this.contSchedaAlleati.transform.GetChild(j).GetChild(4).GetComponent<Button>().interactable = true;
			}
			else
			{
				this.contSchedaAlleati.transform.GetChild(j).GetChild(3).GetComponent<Button>().interactable = false;
				this.contSchedaAlleati.transform.GetChild(j).GetChild(4).GetComponent<Button>().interactable = false;
			}
		}
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x000EF168 File Offset: 0x000ED368
	private void OpzioniBattVeloce()
	{
		this.valoreRealeMaxNemici = Mathf.FloorToInt(this.impMaxNemici.GetComponent<Slider>().value);
		this.valoreRealeMaxAlleati = Mathf.FloorToInt(this.impMaxAlleati.GetComponent<Slider>().value);
		this.scrittaMaxNemici.GetComponent<Text>().text = "MAX ENEMIES ON SCREEN:  " + this.valoreRealeMaxNemici.ToString();
		this.scrittaMaxAlleati.GetComponent<Text>().text = "MAX ALLIES ON SCREEN:  " + this.valoreRealeMaxAlleati.ToString();
		float num = Mathf.Floor(this.impVitaNemici.GetComponent<Slider>().value);
		this.scrittaVitaNemici.GetComponent<Text>().text = "ENEMIES' HEALTH:  " + num.ToString() + "%";
		float num2 = Mathf.Floor(this.impAttaccoNemici.GetComponent<Slider>().value);
		this.scrittaAttaccoNemici.GetComponent<Text>().text = "ENEMIES' ATTACK:  " + num2.ToString() + "%";
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x000EF270 File Offset: 0x000ED470
	private void DettagliInfoNemico()
	{
		GameObject gameObject = this.ListaNemici[this.nemicoSelezInfo];
		this.dettagliNemico.transform.GetChild(1).GetComponent<Text>().text = gameObject.GetComponent<PresenzaNemico>().nomeInsetto;
		this.dettagliNemico.transform.GetChild(2).GetComponent<Image>().sprite = gameObject.GetComponent<PresenzaNemico>().immagineInsetto;
		this.dettagliNemico.transform.GetChild(3).GetComponent<Text>().text = string.Concat(new string[]
		{
			"Health:  ",
			(gameObject.GetComponent<PresenzaNemico>().vita * this.impVitaNemici.GetComponent<Slider>().value / 100f).ToString("F0"),
			"\nArmor:  ",
			(gameObject.GetComponent<PresenzaNemico>().armatura * 100f).ToString(),
			"%\nDamage 1:  ",
			(gameObject.GetComponent<PresenzaNemico>().danno1 * this.impAttaccoNemici.GetComponent<Slider>().value / 100f).ToString("F0"),
			"\nDamage 2:  ",
			(gameObject.GetComponent<PresenzaNemico>().danno2 * this.impAttaccoNemici.GetComponent<Slider>().value / 100f).ToString("F0"),
			"\nVenom Damage:  ",
			(gameObject.GetComponent<PresenzaNemico>().dannoVeleno * this.impAttaccoNemici.GetComponent<Slider>().value / 100f).ToString("F0"),
			"\nAttack Rate:  ",
			gameObject.GetComponent<PresenzaNemico>().frequenzaAttacco.ToString(),
			"\nSpeed:  ",
			gameObject.GetComponent<PresenzaNemico>().velocitàInsetto,
			"\nFlying:  ",
			gameObject.GetComponent<PresenzaNemico>().insettoVolante.ToString(),
			"\nJumping:  ",
			gameObject.GetComponent<PresenzaNemico>().èSaltatore.ToString(),
			"\n Members for group:  ",
			gameObject.GetComponent<PresenzaNemico>().numMembriGruppo.ToString()
		});
		this.dettagliNemico.transform.GetChild(4).GetComponent<Text>().text = gameObject.GetComponent<PresenzaNemico>().oggettoDescrizione.GetComponent<Text>().text;
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x000EF4D0 File Offset: 0x000ED6D0
	private void DettagliInfoAlleato()
	{
		GameObject gameObject = this.ListaAlleati[this.alleatoSelezInfo];
		this.dettagliAlleato.transform.GetChild(1).GetComponent<Text>().text = gameObject.GetComponent<PresenzaAlleato>().nomeUnità;
		this.dettagliAlleato.transform.GetChild(2).GetComponent<Image>().sprite = gameObject.GetComponent<PresenzaAlleato>().immagineUnità;
		if (!gameObject.GetComponent<PresenzaAlleato>().èPerRifornimento)
		{
			if (gameObject.GetComponent<PresenzaAlleato>().èAereo)
			{
				this.dettagliAlleato.transform.GetChild(3).GetComponent<Text>().text = string.Concat(new object[]
				{
					"Health:  ",
					gameObject.GetComponent<PresenzaAlleato>().vita.ToString(),
					"\nCost in Refined Plastic: ",
					gameObject.GetComponent<PresenzaAlleato>().costoInPlastica,
					"\nCost in Battle Point: ",
					(gameObject.GetComponent<PresenzaAlleato>().costoInPlastica / 10f).ToString("F0"),
					"\nSpeed:  ",
					gameObject.GetComponent<PresenzaAlleato>().velocitàIndicativa,
					"\nVisual Range:  ",
					gameObject.GetComponent<PresenzaAlleato>().raggioVisivo.ToString(),
					"\nClimbing:  ",
					gameObject.GetComponent<PresenzaAlleato>().scalatrice.ToString(),
					"\nRepair Step:  ",
					gameObject.GetComponent<PresenzaAlleato>().velocitàRiparazione,
					"\nFuel:  ",
					gameObject.GetComponent<PresenzaAlleato>().carburante.ToString("F0")
				});
			}
			else
			{
				this.dettagliAlleato.transform.GetChild(3).GetComponent<Text>().text = string.Concat(new object[]
				{
					"Health:  ",
					gameObject.GetComponent<PresenzaAlleato>().vita.ToString(),
					"\nCost in Refined Plastic: ",
					gameObject.GetComponent<PresenzaAlleato>().costoInPlastica,
					"\nCost in Battle Point: ",
					(gameObject.GetComponent<PresenzaAlleato>().costoInPlastica / 10f).ToString("F0"),
					"\nSpeed:  ",
					gameObject.GetComponent<PresenzaAlleato>().velocitàIndicativa,
					"\nVisual Range:  ",
					gameObject.GetComponent<PresenzaAlleato>().raggioVisivo.ToString(),
					"\nClimbing:  ",
					gameObject.GetComponent<PresenzaAlleato>().scalatrice.ToString(),
					"\nRepair Step:  ",
					gameObject.GetComponent<PresenzaAlleato>().velocitàRiparazione,
					"\nFuel:  N.D."
				});
			}
		}
		else
		{
			this.dettagliAlleato.transform.GetChild(3).GetComponent<Text>().text = string.Concat(new object[]
			{
				"Health:  ",
				gameObject.GetComponent<PresenzaAlleato>().vita.ToString(),
				"\nCost in Battle Point: ",
				(gameObject.GetComponent<PresenzaAlleato>().costoInPlastica / 10f).ToString("F0"),
				"\nSpeed:  ",
				gameObject.GetComponent<PresenzaAlleato>().velocitàIndicativa,
				"\nSupply Capacity:  ",
				gameObject.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp.ToString(),
				"\nSupply Range:  ",
				gameObject.GetComponent<PresenzaAlleato>().raggioDiRifornimento.ToString(),
				"\nClimbing:  ",
				gameObject.GetComponent<PresenzaAlleato>().scalatrice.ToString(),
				"\nRepair Step:  ",
				gameObject.GetComponent<PresenzaAlleato>().velocitàRiparazione,
				"\nCost in Refined Plastic: ",
				gameObject.GetComponent<PresenzaAlleato>().costoInPlastica
			});
		}
		if (gameObject.GetComponent<PresenzaAlleato>().tipoTruppa == 10 || gameObject.GetComponent<PresenzaAlleato>().tipoTruppa == 11 || gameObject.GetComponent<PresenzaAlleato>().tipoTruppa == 16 || gameObject.GetComponent<PresenzaAlleato>().tipoTruppa == 33)
		{
			this.pulsanteArmi.GetComponent<Button>().interactable = false;
		}
		else
		{
			this.pulsanteArmi.GetComponent<Button>().interactable = true;
		}
		this.dettagliAlleato.transform.GetChild(4).GetComponent<Text>().text = gameObject.GetComponent<PresenzaAlleato>().oggettoDescrizione.GetComponent<Text>().text;
	}

	// Token: 0x060006CB RID: 1739 RVA: 0x000EF924 File Offset: 0x000EDB24
	private void VisualizzaArmi()
	{
		GameObject gameObject = this.ListaAlleati[this.alleatoSelezInfo];
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
		if (this.battVelAggArmi)
		{
			for (int i = 0; i < 4; i++)
			{
				this.ListaMatricePulsantiArmi[i] = 0;
			}
			this.pulsanteColonnaArma = 0;
			this.pulsanteRigaArma = 0;
		}
		else
		{
			for (int j = 0; j < 4; j++)
			{
				if (this.pulsanteColonnaArma == j)
				{
					this.ListaMatricePulsantiArmi[j] = this.pulsanteRigaArma;
				}
			}
		}
		this.battVelAggArmi = false;
		this.Armi.transform.GetChild(0).GetComponent<Text>().text = "WEAPONS of:  " + gameObject.GetComponent<PresenzaAlleato>().nomeUnità;
		for (int k = 0; k < 4; k++)
		{
			GameObject gameObject2 = this.Armi.transform.GetChild(k + 1).FindChild("lista pulsanti armi").gameObject;
			if (k >= gameObject.GetComponent<PresenzaAlleato>().numeroArmi)
			{
				this.Armi.transform.GetChild(k + 1).GetComponent<CanvasGroup>().alpha = 0f;
				this.Armi.transform.GetChild(k + 1).GetComponent<CanvasGroup>().interactable = false;
				this.Armi.transform.GetChild(k + 1).GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
			else
			{
				this.Armi.transform.GetChild(k + 1).GetComponent<CanvasGroup>().alpha = 1f;
				this.Armi.transform.GetChild(k + 1).GetComponent<CanvasGroup>().interactable = true;
				this.Armi.transform.GetChild(k + 1).GetComponent<CanvasGroup>().blocksRaycasts = true;
				this.Armi.transform.GetChild(k + 1).GetChild(0).GetComponent<Text>().text = "WEAPON " + (k + 1).ToString();
				if (this.ListaArmiUnitàConsiderata[k][0] != 0f)
				{
					this.Armi.transform.GetChild(k + 1).FindChild("pulsante salva armi").GetComponent<CanvasGroup>().interactable = false;
					for (int l = 0; l < 12; l++)
					{
						if (l < this.ListaTipiMunizioniArmiUnitàCons[k].Count)
						{
							gameObject2.transform.GetChild(l).GetChild(0).GetComponent<Text>().text = this.ListaTipiMunizioniArmiUnitàCons[k][l].GetComponent<DatiGeneraliMunizione>().nome;
							gameObject2.transform.GetChild(l).GetComponent<CanvasGroup>().alpha = 1f;
							gameObject2.transform.GetChild(l).GetComponent<CanvasGroup>().interactable = true;
							gameObject2.transform.GetChild(l).GetComponent<CanvasGroup>().blocksRaycasts = true;
							if (l == this.ListaMatricePulsantiArmi[k])
							{
								gameObject2.transform.GetChild(l).GetComponent<Image>().color = this.coloreArmaSel;
							}
							else
							{
								gameObject2.transform.GetChild(l).GetComponent<Image>().color = this.coloreArmaNonSel;
							}
						}
						else
						{
							gameObject2.transform.GetChild(l).GetComponent<CanvasGroup>().alpha = 0f;
							gameObject2.transform.GetChild(l).GetComponent<CanvasGroup>().interactable = false;
							gameObject2.transform.GetChild(l).GetComponent<CanvasGroup>().blocksRaycasts = false;
						}
					}
					this.Armi.transform.GetChild(k + 1).GetChild(1).GetComponent<Text>().text = gameObject.GetComponent<PresenzaAlleato>().ListaNomiArmi[k];
					this.Armi.transform.GetChild(k + 1).GetChild(2).GetComponent<Text>().text = string.Concat(new object[]
					{
						"Damage: ",
						this.ListaTipiMunizioniArmiUnitàCons[k][this.ListaMatricePulsantiArmi[k]].GetComponent<DatiGeneraliMunizione>().danno,
						"\nRange: ",
						this.ListaTipiMunizioniArmiUnitàCons[k][this.ListaMatricePulsantiArmi[k]].GetComponent<DatiGeneraliMunizione>().portataMassima,
						"\nRate: ",
						this.ListaArmiUnitàConsiderata[k][0],
						" (",
						this.ListaArmiUnitàConsiderata[k][1],
						")"
					});
					this.Armi.transform.GetChild(k + 1).GetChild(3).GetComponent<Text>().text = string.Concat(new object[]
					{
						"Piercing Value: ",
						this.ListaTipiMunizioniArmiUnitàCons[k][this.ListaMatricePulsantiArmi[k]].GetComponent<DatiGeneraliMunizione>().penetrazione,
						"\nBlast Radius: ",
						this.ListaTipiMunizioniArmiUnitàCons[k][this.ListaMatricePulsantiArmi[k]].GetComponent<DatiGeneraliMunizione>().raggioEffetto,
						"\nReload: ",
						this.ListaArmiUnitàConsiderata[k][2]
					});
					this.Armi.transform.GetChild(k + 1).GetChild(4).GetComponent<Text>().text = this.ListaTipiMunizioniArmiUnitàCons[k][this.ListaMatricePulsantiArmi[k]].GetComponent<DatiGeneraliMunizione>().descrizioneArma.GetComponent<Text>().text;
				}
				else
				{
					this.Armi.transform.GetChild(k + 1).FindChild("pulsante salva armi").GetComponent<CanvasGroup>().interactable = true;
					if (gameObject.GetComponent<PresenzaAlleato>().tipoTruppaVolante != 0)
					{
						for (int m = 0; m < 12; m++)
						{
							if (m < gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili.Count)
							{
								gameObject2.transform.GetChild(m).GetChild(0).GetComponent<Text>().text = gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili[m].GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().nome + "  x" + gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili[m].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche.Count * 2;
								gameObject2.transform.GetChild(m).GetComponent<CanvasGroup>().alpha = 1f;
								gameObject2.transform.GetChild(m).GetComponent<CanvasGroup>().interactable = true;
								gameObject2.transform.GetChild(m).GetComponent<CanvasGroup>().blocksRaycasts = true;
								if (this.pulsanteColonnaArma == k && this.pulsanteRigaArma == m)
								{
									gameObject2.transform.GetChild(m).GetComponent<Image>().color = this.coloreArmaSel;
								}
								else
								{
									gameObject2.transform.GetChild(m).GetComponent<Image>().color = this.coloreArmaNonSel;
								}
							}
							else
							{
								gameObject2.transform.GetChild(m).GetComponent<CanvasGroup>().alpha = 0f;
								gameObject2.transform.GetChild(m).GetComponent<CanvasGroup>().interactable = false;
								gameObject2.transform.GetChild(m).GetComponent<CanvasGroup>().blocksRaycasts = false;
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
								k
							}));
						}
						else
						{
							@int = PlayerPrefs.GetInt(string.Concat(new object[]
							{
								"tipo truppa volante ",
								tipoTruppaVolante,
								" ",
								k - 1
							}));
						}
						bool flag = false;
						int num = 0;
						while (num < this.ListaOrdigniTotali.Count && !flag)
						{
							if (this.ListaOrdigniTotali[num].GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == @int)
							{
								GameObject x = this.ListaOrdigniTotali[num];
								flag = true;
								for (int n = 0; n < gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili.Count; n++)
								{
									if (x == gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili[n])
									{
										gameObject2.transform.GetChild(n).GetComponent<Image>().color = this.coloreArmaSalvata;
									}
								}
							}
							num++;
						}
						GameObject gameObject3 = gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili[this.ListaMatricePulsantiArmi[k]];
						this.Armi.transform.GetChild(k + 1).GetChild(1).GetComponent<Text>().text = gameObject3.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().nome + "  x" + gameObject3.GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche.Count * 2;
						this.Armi.transform.GetChild(k + 1).GetChild(2).GetComponent<Text>().text = string.Concat(new object[]
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
						this.Armi.transform.GetChild(k + 1).GetChild(3).GetComponent<Text>().text = string.Concat(new object[]
						{
							"Piercing Value: ",
							gameObject3.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().penetrazione,
							"\nBlast Radius: ",
							gameObject3.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().raggioEffetto,
							"\nReload: ",
							gameObject3.GetComponent<DatiOrdignoEsterno>().ListaValoriOrdigno[2]
						});
						this.Armi.transform.GetChild(k + 1).GetChild(4).GetComponent<Text>().text = gameObject3.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().descrizioneArma.GetComponent<Text>().text;
					}
					else if (gameObject.GetComponent<PresenzaAlleato>().tipoTruppaTerrConOrdigni != 0)
					{
						for (int num2 = 0; num2 < 12; num2++)
						{
							if (num2 < gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili.Count)
							{
								gameObject2.transform.GetChild(num2).GetChild(0).GetComponent<Text>().text = gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili[num2].GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().nome + "  x" + gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili[num2].GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche.Count;
								gameObject2.transform.GetChild(num2).GetComponent<CanvasGroup>().alpha = 1f;
								gameObject2.transform.GetChild(num2).GetComponent<CanvasGroup>().interactable = true;
								gameObject2.transform.GetChild(num2).GetComponent<CanvasGroup>().blocksRaycasts = true;
								if (this.pulsanteColonnaArma == k && this.pulsanteRigaArma == num2)
								{
									gameObject2.transform.GetChild(num2).GetComponent<Image>().color = this.coloreArmaSel;
								}
								else
								{
									gameObject2.transform.GetChild(num2).GetComponent<Image>().color = this.coloreArmaNonSel;
								}
							}
							else
							{
								gameObject2.transform.GetChild(num2).GetComponent<CanvasGroup>().alpha = 0f;
								gameObject2.transform.GetChild(num2).GetComponent<CanvasGroup>().interactable = false;
								gameObject2.transform.GetChild(num2).GetComponent<CanvasGroup>().blocksRaycasts = false;
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
								k
							}));
						}
						else
						{
							int2 = PlayerPrefs.GetInt(string.Concat(new object[]
							{
								"tipo truppa terr con ordigno ",
								tipoTruppaTerrConOrdigni,
								" ",
								k - 1
							}));
						}
						bool flag2 = false;
						int num3 = 0;
						while (num3 < this.ListaOrdigniTotali.Count && !flag2)
						{
							if (this.ListaOrdigniTotali[num3].GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == int2)
							{
								GameObject x2 = this.ListaOrdigniTotali[num3];
								flag2 = true;
								for (int num4 = 0; num4 < gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili.Count; num4++)
								{
									if (x2 == gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili[num4])
									{
										gameObject2.transform.GetChild(num4).GetComponent<Image>().color = this.coloreArmaSalvata;
									}
								}
							}
							num3++;
						}
						GameObject gameObject4 = gameObject.GetComponent<PresenzaAlleato>().ListaOrdigniPossibili[this.ListaMatricePulsantiArmi[k]];
						this.Armi.transform.GetChild(k + 1).GetChild(1).GetComponent<Text>().text = gameObject4.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().nome + "  x" + gameObject4.GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche.Count;
						this.Armi.transform.GetChild(k + 1).GetChild(2).GetComponent<Text>().text = string.Concat(new object[]
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
						this.Armi.transform.GetChild(k + 1).GetChild(3).GetComponent<Text>().text = string.Concat(new object[]
						{
							"Piercing Value: ",
							gameObject4.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().penetrazione,
							"\nBlast Radius: ",
							gameObject4.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().raggioEffetto,
							"\nReload: ",
							gameObject4.GetComponent<DatiOrdignoEsterno>().ListaValoriOrdigno[2]
						});
						this.Armi.transform.GetChild(k + 1).GetChild(4).GetComponent<Text>().text = gameObject4.GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().descrizioneArma.GetComponent<Text>().text;
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

	// Token: 0x060006CC RID: 1740 RVA: 0x000F0BF0 File Offset: 0x000EEDF0
	private void ControlloPerAvvio()
	{
		bool flag = true;
		if ((GestoreNeutroStrategia.tipoBattaglia == 6 || GestoreNeutroStrategia.tipoBattaglia == 7) && !this.ListaCase[this.numeroCasa].GetComponent<InfoCasa>().ListaStanzeDiCasa[this.numeroStanza].GetComponent<InfoStanza>().convogliPossibili)
		{
			flag = false;
		}
		if (GestoreNeutroStrategia.tipoBattaglia == 3 || GestoreNeutroStrategia.tipoBattaglia == 5)
		{
			if (this.totaleNemici <= 0)
			{
				flag = false;
			}
		}
		else if (this.totaleAlleati <= 0 || this.totaleNemici <= 0)
		{
			flag = false;
		}
		if (flag)
		{
			this.pulsanteInizia.GetComponent<Button>().interactable = true;
		}
		else
		{
			this.pulsanteInizia.GetComponent<Button>().interactable = false;
		}
	}

	// Token: 0x0400190E RID: 6414
	private GameObject descrMissione;

	// Token: 0x0400190F RID: 6415
	private GameObject oggettoListaMissioni;

	// Token: 0x04001910 RID: 6416
	private GameObject oggettoListaDurata;

	// Token: 0x04001911 RID: 6417
	private GameObject immagineCasa;

	// Token: 0x04001912 RID: 6418
	private GameObject oggettoListaCase;

	// Token: 0x04001913 RID: 6419
	private GameObject immagineStanza;

	// Token: 0x04001914 RID: 6420
	private GameObject oggettoListaStanze;

	// Token: 0x04001915 RID: 6421
	private GameObject schedaAlleati;

	// Token: 0x04001916 RID: 6422
	private GameObject schedaNemici;

	// Token: 0x04001917 RID: 6423
	private GameObject contSchedaAlleati;

	// Token: 0x04001918 RID: 6424
	private GameObject contSchedaNemici;

	// Token: 0x04001919 RID: 6425
	private GameObject dettagliAlleato;

	// Token: 0x0400191A RID: 6426
	private GameObject pulsanteArmi;

	// Token: 0x0400191B RID: 6427
	private GameObject dettagliNemico;

	// Token: 0x0400191C RID: 6428
	private GameObject Armi;

	// Token: 0x0400191D RID: 6429
	private GameObject scrittaNonSceltaReclAlleati;

	// Token: 0x0400191E RID: 6430
	private GameObject scrittaNonSceltaReclNemici;

	// Token: 0x0400191F RID: 6431
	private GameObject scrittaMappaNonHaConvoglio;

	// Token: 0x04001920 RID: 6432
	private GameObject pulsanteInizia;

	// Token: 0x04001921 RID: 6433
	private GameObject impMaxAlleati;

	// Token: 0x04001922 RID: 6434
	private GameObject impMaxNemici;

	// Token: 0x04001923 RID: 6435
	public GameObject impVitaNemici;

	// Token: 0x04001924 RID: 6436
	public GameObject impAttaccoNemici;

	// Token: 0x04001925 RID: 6437
	private GameObject scrittaMaxAlleati;

	// Token: 0x04001926 RID: 6438
	private GameObject scrittaMaxNemici;

	// Token: 0x04001927 RID: 6439
	private GameObject scrittaVitaNemici;

	// Token: 0x04001928 RID: 6440
	private GameObject scrittaAttaccoNemici;

	// Token: 0x04001929 RID: 6441
	public int valoreRealeMaxAlleati;

	// Token: 0x0400192A RID: 6442
	public int valoreRealeMaxNemici;

	// Token: 0x0400192B RID: 6443
	public List<GameObject> ListaDescrMissioni;

	// Token: 0x0400192C RID: 6444
	public List<GameObject> ListaCase;

	// Token: 0x0400192D RID: 6445
	public List<GameObject> ListaAlleati;

	// Token: 0x0400192E RID: 6446
	public List<GameObject> ListaNemici;

	// Token: 0x0400192F RID: 6447
	public bool battVelAggMissione;

	// Token: 0x04001930 RID: 6448
	public bool battVelAggDurataBatt;

	// Token: 0x04001931 RID: 6449
	public bool battVelAggMappa;

	// Token: 0x04001932 RID: 6450
	public bool battVelAggAlleati;

	// Token: 0x04001933 RID: 6451
	public bool battVelAggiungiAlleato;

	// Token: 0x04001934 RID: 6452
	public bool battVelTogliAlleato;

	// Token: 0x04001935 RID: 6453
	public bool battVelAggNemici;

	// Token: 0x04001936 RID: 6454
	public bool battVelAggiungiNemico;

	// Token: 0x04001937 RID: 6455
	public bool battVelTogliNemico;

	// Token: 0x04001938 RID: 6456
	public bool cambioCasa;

	// Token: 0x04001939 RID: 6457
	public bool battVelInfoAlleato;

	// Token: 0x0400193A RID: 6458
	public bool battVelInfoNemico;

	// Token: 0x0400193B RID: 6459
	public int fattoreDurataBattaglia;

	// Token: 0x0400193C RID: 6460
	public int numeroCasa;

	// Token: 0x0400193D RID: 6461
	public int numeroStanza;

	// Token: 0x0400193E RID: 6462
	public int schedaAperta;

	// Token: 0x0400193F RID: 6463
	public List<int> ListaAlleatiQuantitàBattVel;

	// Token: 0x04001940 RID: 6464
	public List<int> ListaNemiciQuantitàBattVel;

	// Token: 0x04001941 RID: 6465
	public int alleatoSelez;

	// Token: 0x04001942 RID: 6466
	public int nemicoSelez;

	// Token: 0x04001943 RID: 6467
	public int alleatoSelezInfo;

	// Token: 0x04001944 RID: 6468
	public int nemicoSelezInfo;

	// Token: 0x04001945 RID: 6469
	private List<List<float>> ListaArmiUnitàConsiderata;

	// Token: 0x04001946 RID: 6470
	private List<List<GameObject>> ListaTipiMunizioniArmiUnitàCons;

	// Token: 0x04001947 RID: 6471
	public int pulsanteColonnaArma;

	// Token: 0x04001948 RID: 6472
	public int pulsanteRigaArma;

	// Token: 0x04001949 RID: 6473
	private List<int> ListaMatricePulsantiArmi;

	// Token: 0x0400194A RID: 6474
	public Color coloreArmaSel;

	// Token: 0x0400194B RID: 6475
	public Color coloreArmaNonSel;

	// Token: 0x0400194C RID: 6476
	public Color coloreArmaSalvata;

	// Token: 0x0400194D RID: 6477
	public List<GameObject> ListaOrdigniTotali;

	// Token: 0x0400194E RID: 6478
	public bool salvaArmaSelez;

	// Token: 0x0400194F RID: 6479
	public int numColonnaArmaDaSalvare;

	// Token: 0x04001950 RID: 6480
	public bool armiAperto;

	// Token: 0x04001951 RID: 6481
	public bool battVelAggArmi;

	// Token: 0x04001952 RID: 6482
	private bool libertàDiReclAlleati;

	// Token: 0x04001953 RID: 6483
	private bool libertàDiSceltaNemici;

	// Token: 0x04001954 RID: 6484
	public bool resettaAlleati;

	// Token: 0x04001955 RID: 6485
	public bool resettaNemici;

	// Token: 0x04001956 RID: 6486
	private int totaleAlleati;

	// Token: 0x04001957 RID: 6487
	private int totaleNemici;

	// Token: 0x04001958 RID: 6488
	public bool clickSuListaCase;

	// Token: 0x04001959 RID: 6489
	public bool clickSuListaStanze;

	// Token: 0x0400195A RID: 6490
	public bool clickSuListaDurata;

	// Token: 0x0400195B RID: 6491
	public bool clickSuListaMissioni;

	// Token: 0x0400195C RID: 6492
	public int numListaDurata;

	// Token: 0x0400195D RID: 6493
	public int numListaMissioni;
}
