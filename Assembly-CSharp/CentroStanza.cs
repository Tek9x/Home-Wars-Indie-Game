using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000103 RID: 259
public class CentroStanza : MonoBehaviour
{
	// Token: 0x0600082D RID: 2093 RVA: 0x0011C090 File Offset: 0x0011A290
	private void Start()
	{
		this.cameraCasa = GameObject.FindGameObjectWithTag("MainCamera");
		this.corpoBandiera = base.transform.GetChild(0);
		this.CentroStanzaUI = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Centro Stanza").gameObject;
		this.settoriUI = this.CentroStanzaUI.transform.GetChild(1).gameObject;
		this.risorseAlleatiUI = this.CentroStanzaUI.transform.FindChild("risorse alleati").gameObject;
		this.risorseNemiciUI = this.CentroStanzaUI.transform.FindChild("risorse nemici").gameObject;
		this.Headquarters = GameObject.FindGameObjectWithTag("Headquarters");
		this.testoNumeroAlleati = this.CentroStanzaUI.transform.FindChild("numero alleati in stanza").gameObject;
		this.testoNumeroNemici = this.CentroStanzaUI.transform.FindChild("numero nemici in stanza").gameObject;
		this.schermataBattaglia = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Per Battaglia").gameObject;
		this.elencoAlleatiSchermBatt = this.schermataBattaglia.transform.FindChild("elenco alleati").GetChild(0).GetChild(0).GetChild(0).gameObject;
		this.elencoAlleatiSchermBattBarraVert = this.schermataBattaglia.transform.FindChild("elenco alleati").GetChild(0).GetChild(2).gameObject;
		this.elencoNemiciSchermBatt = this.schermataBattaglia.transform.FindChild("elenco nemici").GetChild(0).GetChild(0).GetChild(0).gameObject;
		this.elencoNemiciSchermBattBarraVert = this.schermataBattaglia.transform.FindChild("elenco nemici").GetChild(0).GetChild(2).gameObject;
		this.Nest = GameObject.FindGameObjectWithTag("Nest");
		this.pulsanteDifendi = this.CentroStanzaUI.transform.FindChild("pulsante difendi").gameObject;
		this.pulsanteAttacca = this.CentroStanzaUI.transform.FindChild("pulsante attacca").gameObject;
		this.pulsanteCombatti = this.schermataBattaglia.transform.FindChild("Combatti").gameObject;
		this.pulsanteVittoriaATavolino = this.schermataBattaglia.transform.FindChild("Vittoria a tavolino").gameObject;
		this.pulsanteRitirata = this.schermataBattaglia.transform.FindChild("Ritirata").gameObject;
		this.scherResocontoBatt = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Resoconto Battaglia").gameObject;
		this.elencoAlleatiResocontoBatt = this.scherResocontoBatt.transform.FindChild("elenco alleati").GetChild(0).GetChild(0).GetChild(0).gameObject;
		this.elencoAlleatiResocontoBattBarraVert = this.scherResocontoBatt.transform.FindChild("elenco alleati").GetChild(0).GetChild(2).gameObject;
		this.elencoNemiciResocontoBatt = this.scherResocontoBatt.transform.FindChild("elenco nemici").GetChild(0).GetChild(0).GetChild(0).gameObject;
		this.elencoNemiciResocontoBattBarraVert = this.scherResocontoBatt.transform.FindChild("elenco nemici").GetChild(0).GetChild(2).gameObject;
		this.ricompenseResocontoBatt = this.scherResocontoBatt.transform.FindChild("ricompense").GetChild(0).GetChild(0).gameObject;
		this.immagineEsitoResBatt = this.scherResocontoBatt.transform.FindChild("sfondo immagine esito").gameObject;
		this.schermataMissione = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Per Missione").gameObject;
		this.elencoAlleatiSchermMiss = this.schermataMissione.transform.FindChild("elenco alleati").GetChild(0).GetChild(0).GetChild(0).gameObject;
		this.elencoAlleatiSchermMissBarraVert = this.schermataMissione.transform.FindChild("elenco alleati").GetChild(0).GetChild(2).gameObject;
		this.scherResocontoMissione = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Resoconto Missione").gameObject;
		this.immagineEsitoResMiss = this.scherResocontoMissione.transform.FindChild("sfondo immagine esito").gameObject;
		this.elencoAlleatiResocontoMissione = this.scherResocontoMissione.transform.FindChild("elenco alleati").GetChild(0).GetChild(0).GetChild(0).gameObject;
		this.elencoAlleatiResocontoMissioneBarraVert = this.scherResocontoMissione.transform.FindChild("elenco alleati").GetChild(0).GetChild(2).gameObject;
		this.resocontoMissioneTesto = this.scherResocontoMissione.transform.FindChild("ricompense").GetChild(0).GetChild(0).gameObject;
		this.resocontoMissionePremi = this.scherResocontoMissione.transform.FindChild("ricompense").GetChild(0).GetChild(1).gameObject;
		this.pulsanteMissione = this.CentroStanzaUI.transform.FindChild("pulsante missione").gameObject;
		this.scrittaSolo1Battaglia = this.CentroStanzaUI.transform.FindChild("scritta solo 1 battaglia").gameObject;
		this.perditeInBatt = this.scherResocontoBatt.transform.FindChild("perdite").GetChild(0).gameObject;
		this.perditeinMiss = this.scherResocontoMissione.transform.FindChild("perdite").GetChild(0).gameObject;
		this.postiInElencoBattaglia = 55;
		this.numUnitàInEserAlleato = this.Headquarters.GetComponent<GestioneEsercitiAlleati>().numUnitàInUnEser;
		this.numAlleatiPossibili = this.Headquarters.GetComponent<GestioneEsercitiAlleati>().ListaUnitàAlleate.Count;
		this.numNemiciPossibili = this.Nest.GetComponent<IANemicoStrategia>().ListaTipiInsetti.Count;
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x0011C6A8 File Offset: 0x0011A8A8
	private void Update()
	{
		this.EsteticaBandiere();
		this.GenerazioneRisorse();
		this.ElementiPresentiInStanza();
		this.ProprietàBandiera();
		if (!GestoreNeutroStrategia.mostraResocontoBattaglia)
		{
			this.CalcoloBattaglie();
		}
		if (this.CentroStanzaUI.GetComponent<CanvasGroup>().alpha == 1f && this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata == base.gameObject)
		{
			this.VisualizzazioneInUI();
		}
		if (this.schermataBattaglia.GetComponent<CanvasGroup>().alpha == 1f && this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata == base.gameObject && GestoreNeutroStrategia.aggElencoBattaglia)
		{
			this.BattagliaNormaleOATavolino();
			this.VisualizSchermataBattaglia();
			GestoreNeutroStrategia.aggElencoBattaglia = false;
		}
		if (this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata == base.gameObject && GestoreNeutroStrategia.mostraResocontoBattaglia)
		{
			this.VisualizzaResocontoBattaglia();
		}
		if (this.schermataMissione.GetComponent<CanvasGroup>().alpha == 1f && this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata == base.gameObject && GestoreNeutroStrategia.aggElencoMissione)
		{
			GestoreNeutroStrategia.tipoBattaglia = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().tipoMissione;
			this.VisualizzaSchermataMissione();
			GestoreNeutroStrategia.aggElencoMissione = false;
		}
		if (this.cameraCasa.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata == base.gameObject && GestoreNeutroStrategia.mostraResocontoMissione)
		{
			this.VisualizzaResocontoMissione();
		}
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x0011C838 File Offset: 0x0011AA38
	private void EsteticaBandiere()
	{
		Vector3 normalized = Vector3.ProjectOnPlane(-this.cameraCasa.transform.forward, Vector3.up).normalized;
		this.corpoBandiera.right = -normalized;
		this.corpoBandiera.eulerAngles = new Vector3(270f, this.corpoBandiera.eulerAngles.y, this.corpoBandiera.eulerAngles.z);
		if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().aggMissioneExtra)
		{
			if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().missionePresente == 1 && base.gameObject == this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().stanzaDiMissione])
			{
				if (this.numTruppeAlleatePres > 0)
				{
					base.transform.GetChild(0).GetChild(0).GetComponent<TextMesh>().text = "EXTRA MISSION";
					base.transform.GetChild(0).GetChild(0).GetComponent<TextMesh>().color = Color.yellow;
				}
				else
				{
					base.transform.GetChild(0).GetChild(0).GetComponent<TextMesh>().text = "EXTRA MISSION";
					base.transform.GetChild(0).GetChild(0).GetComponent<TextMesh>().color = Color.red;
				}
			}
			else
			{
				base.transform.GetChild(0).GetChild(0).GetComponent<TextMesh>().text = string.Empty;
			}
		}
		if (this.quiNemicoStaAttaccando)
		{
			base.transform.GetChild(0).GetChild(1).GetComponent<TextMesh>().text = "DEFEND HERE!";
		}
		else
		{
			base.transform.GetChild(0).GetChild(1).GetComponent<TextMesh>().text = string.Empty;
		}
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x0011CA28 File Offset: 0x0011AC28
	private void GenerazioneRisorse()
	{
		if (this.settoriAlleati == 3)
		{
			this.guadagnoRealePlastica = this.valoreGuadPlastica;
			this.guadagnoRealeMetallo = this.valoreGuadMetallo;
			this.guadagnoRealeEnergia = this.valoreGuadEnergia;
			this.guadagnoRealeMatIncendiario = this.valoreGuadIncendiario;
			this.guadagnoRealeMatTossico = this.valoreGuadTossico;
		}
		else if (this.settoriAlleati == 2)
		{
			this.guadagnoRealePlastica = this.valoreGuadPlastica / 2f;
			this.guadagnoRealeMetallo = this.valoreGuadMetallo / 2f;
			this.guadagnoRealeEnergia = this.valoreGuadEnergia / 2f;
			this.guadagnoRealeMatIncendiario = this.valoreGuadIncendiario / 2f;
			this.guadagnoRealeMatTossico = this.valoreGuadTossico / 2f;
		}
		else if (this.settoriAlleati == 0 || this.settoriAlleati == 1)
		{
			this.guadagnoRealePlastica = 0f;
			this.guadagnoRealeMetallo = 0f;
			this.guadagnoRealeEnergia = 0f;
			this.guadagnoRealeMatIncendiario = 0f;
			this.guadagnoRealeMatTossico = 0f;
		}
		if (this.settoriNemici == 3)
		{
			this.guadagnoRealeFreshFood = this.valoreGuadFreshFood * PlayerPrefs.GetFloat("fattore diff fresh food");
			this.guadagnoRealeRottenFood = this.valoreGuadRottenFood * PlayerPrefs.GetFloat("fattore diff rotten food");
			this.guadagnoRealeHighProteinFood = this.valoreGuadHighProteinFood * PlayerPrefs.GetFloat("fattore diff high protein food");
		}
		else if (this.settoriNemici == 2)
		{
			this.guadagnoRealeFreshFood = this.valoreGuadFreshFood * PlayerPrefs.GetFloat("fattore diff fresh food") / 2f;
			this.guadagnoRealeRottenFood = this.valoreGuadRottenFood * PlayerPrefs.GetFloat("fattore diff rotten food") / 2f;
			this.guadagnoRealeHighProteinFood = this.valoreGuadHighProteinFood * PlayerPrefs.GetFloat("fattore diff high protein food") / 2f;
		}
		else if (this.settoriNemici == 0 || this.settoriNemici == 1)
		{
			this.guadagnoRealeFreshFood = 0f;
			this.guadagnoRealeRottenFood = 0f;
			this.guadagnoRealeHighProteinFood = 0f;
		}
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x0011CC34 File Offset: 0x0011AE34
	private void ElementiPresentiInStanza()
	{
		this.esercitiAlleatiPresenti = 0;
		this.esercitiNemiciPresenti = 0;
		this.numTruppeAlleatePres = 0;
		this.numInsettiNemciPres = 0;
		this.ListaEserAlleatiPres = new List<GameObject>();
		this.ListaEserNemiciPres = new List<GameObject>();
		foreach (GameObject current in this.ListaColliderStanza)
		{
			if (current.GetComponent<ColliderCentroStanza>().ListaEsercitiInStanza.Count > 0)
			{
				foreach (GameObject current2 in current.GetComponent<ColliderCentroStanza>().ListaEsercitiInStanza)
				{
					if (current2 != null && current2.tag == "Alleato" && !this.ListaEserAlleatiPres.Contains(current2))
					{
						this.esercitiAlleatiPresenti++;
						this.ListaEserAlleatiPres.Add(current2);
						for (int i = 0; i < 30; i++)
						{
							if (current2.GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[i] != 100)
							{
								this.numTruppeAlleatePres++;
							}
						}
					}
					else if (current2 != null && current2.tag == "Nemico" && !this.ListaEserNemiciPres.Contains(current2))
					{
						this.esercitiNemiciPresenti++;
						this.ListaEserNemiciPres.Add(current2);
						for (int j = 0; j < this.Nest.GetComponent<IANemicoStrategia>().ListaTipiInsetti.Count; j++)
						{
							if (current2.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[j][0] != 100)
							{
								this.numInsettiNemciPres += current2.GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[j][1];
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x0011CE78 File Offset: 0x0011B078
	private void ProprietàBandiera()
	{
		if (this.ListaSettori[0] == 0 && this.ListaSettori[1] == 0 && this.ListaSettori[2] == 0)
		{
			if (this.esercitiAlleatiPresenti != 0 && this.esercitiNemiciPresenti == 0)
			{
				for (int i = 0; i < 3; i++)
				{
					this.ListaSettori[i] = 1;
				}
			}
			else if (this.esercitiAlleatiPresenti == 0 && this.esercitiNemiciPresenti != 0)
			{
				for (int j = 0; j < 3; j++)
				{
					this.ListaSettori[j] = 2;
				}
			}
		}
		this.settoriAlleati = 0;
		this.settoriNemici = 0;
		for (int k = 0; k < 3; k++)
		{
			if (this.ListaSettori[k] == 1)
			{
				this.settoriAlleati++;
			}
			else if (this.ListaSettori[k] == 2)
			{
				this.settoriNemici++;
			}
		}
		if (this.settoriAlleati == 0 && this.settoriNemici == 0)
		{
			this.coloreBandiera = this.bandieraBianca;
			this.appartenenzaBandiera = 0;
		}
		else if (this.settoriAlleati >= 2)
		{
			this.corpoBandiera.gameObject.GetComponent<Renderer>().material = this.bandieraVerde;
			this.appartenenzaBandiera = 1;
		}
		else if (this.settoriNemici >= 2)
		{
			this.corpoBandiera.gameObject.GetComponent<Renderer>().material = this.bandieraRossa;
			this.appartenenzaBandiera = 2;
		}
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x0011D01C File Offset: 0x0011B21C
	private void VisualizzazioneInUI()
	{
		this.CentroStanzaUI.transform.GetChild(0).GetComponent<Text>().text = this.nomeStanza;
		for (int i = 0; i < 3; i++)
		{
			if (this.ListaSettori[i] == 0)
			{
				this.settoriUI.transform.GetChild(i).GetComponent<Image>().color = Color.white;
			}
			else if (this.ListaSettori[i] == 1)
			{
				this.settoriUI.transform.GetChild(i).GetComponent<Image>().color = this.coloreSettoreVerde;
			}
			else if (this.ListaSettori[i] == 2)
			{
				this.settoriUI.transform.GetChild(i).GetComponent<Image>().color = this.coloreSettoreRosso;
			}
		}
		if (this.appartenenzaBandiera == 1)
		{
			this.risorseAlleatiUI.GetComponent<CanvasGroup>().alpha = 1f;
			this.risorseNemiciUI.GetComponent<CanvasGroup>().alpha = 0f;
			this.risorseAlleatiUI.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+" + this.guadagnoRealePlastica.ToString("F1");
			this.risorseAlleatiUI.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "+" + this.guadagnoRealeMetallo.ToString("F1");
			this.risorseAlleatiUI.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "+" + this.guadagnoRealeEnergia.ToString("F1");
			this.risorseAlleatiUI.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "+" + this.guadagnoRealeMatIncendiario.ToString("F1");
			this.risorseAlleatiUI.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "+" + this.guadagnoRealeMatTossico.ToString("F1");
		}
		else if (this.appartenenzaBandiera == 2)
		{
			this.risorseAlleatiUI.GetComponent<CanvasGroup>().alpha = 0f;
			this.risorseNemiciUI.GetComponent<CanvasGroup>().alpha = 1f;
			this.risorseNemiciUI.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+" + (this.guadagnoRealeFreshFood * PlayerPrefs.GetFloat("fattore diff fresh food")).ToString("F1");
			this.risorseNemiciUI.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "+" + (this.guadagnoRealeRottenFood * PlayerPrefs.GetFloat("fattore diff rotten food")).ToString("F1");
			this.risorseNemiciUI.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "+" + (this.guadagnoRealeHighProteinFood * PlayerPrefs.GetFloat("fattore diff high protein food")).ToString("F1");
		}
		else if (this.appartenenzaBandiera == 0)
		{
			this.risorseAlleatiUI.GetComponent<CanvasGroup>().alpha = 0f;
			this.risorseNemiciUI.GetComponent<CanvasGroup>().alpha = 0f;
		}
		if (this.esercitiAlleatiPresenti > 0 || this.appartenenzaBandiera == 1)
		{
			this.testoNumeroAlleati.GetComponent<Text>().text = "Allied Units: " + this.numTruppeAlleatePres;
			this.testoNumeroNemici.GetComponent<Text>().text = "Enemy Bugs: " + this.numInsettiNemciPres;
		}
		else
		{
			this.testoNumeroAlleati.GetComponent<Text>().text = "Allied Units: " + this.numTruppeAlleatePres;
			this.testoNumeroNemici.GetComponent<Text>().text = "Enemy Bugs: unknown";
		}
		int num = 0;
		int num2 = 0;
		for (int j = 0; j < this.ListaSettori.Count; j++)
		{
			if (this.ListaSettori[j] == 1)
			{
				num++;
			}
			else if (this.ListaSettori[j] == 2)
			{
				num2++;
			}
		}
		if (this.quiNemicoStaAttaccando)
		{
			this.pulsanteDifendiAttivo = true;
			this.pulsanteAttaccaAttivo = false;
		}
		else if (num < 3)
		{
			if (this.ListaEserAlleatiPres.Count > 0)
			{
				this.pulsanteDifendiAttivo = false;
				this.pulsanteAttaccaAttivo = true;
			}
			else
			{
				this.pulsanteDifendiAttivo = false;
				this.pulsanteAttaccaAttivo = false;
			}
		}
		else
		{
			this.pulsanteDifendiAttivo = false;
			this.pulsanteAttaccaAttivo = false;
		}
		if (this.pulsanteDifendiAttivo)
		{
			if (!this.cameraCasa.GetComponent<GestoreNeutroStrategia>().turnoNemicoAttivo)
			{
				this.pulsanteDifendi.GetComponent<Button>().interactable = true;
			}
			else
			{
				this.pulsanteDifendi.GetComponent<Button>().interactable = false;
			}
		}
		else
		{
			this.pulsanteDifendi.GetComponent<Button>().interactable = false;
		}
		this.scrittaSolo1Battaglia.GetComponent<Text>().enabled = false;
		if (this.pulsanteAttaccaAttivo)
		{
			if (!this.cameraCasa.GetComponent<GestoreNeutroStrategia>().turnoNemicoAttivo && this.ListaEserAlleatiPres.Count > 0 && this.quiCèStataBattaglia != 1 && this.quiCèStataBattaglia != 3)
			{
				this.pulsanteAttacca.GetComponent<Button>().interactable = true;
			}
			else
			{
				this.pulsanteAttacca.GetComponent<Button>().interactable = false;
				this.scrittaSolo1Battaglia.GetComponent<Text>().enabled = true;
			}
		}
		else
		{
			this.pulsanteAttacca.GetComponent<Button>().interactable = false;
		}
		if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().missionePresente == 1 && base.gameObject == this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().stanzaDiMissione])
		{
			if (this.numTruppeAlleatePres <= 0)
			{
				this.pulsanteMissione.GetComponent<Button>().interactable = false;
				this.pulsanteMissione.transform.GetChild(1).GetComponent<Text>().enabled = true;
			}
			else
			{
				this.pulsanteMissione.GetComponent<Button>().interactable = true;
				this.pulsanteMissione.transform.GetChild(1).GetComponent<Text>().enabled = false;
			}
		}
		else
		{
			this.pulsanteMissione.GetComponent<Button>().interactable = false;
			this.pulsanteMissione.transform.GetChild(1).GetComponent<Text>().enabled = false;
		}
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x0011D6F8 File Offset: 0x0011B8F8
	private void BattagliaNormaleOATavolino()
	{
		if (GestoreNeutroStrategia.attaccante == 0)
		{
			if (this.ListaEserNemiciPres.Count > 0)
			{
				bool flag = false;
				for (int i = 0; i < this.ListaEserNemiciPres.Count; i++)
				{
					if (this.ListaEserNemiciPres[i].GetComponent<PresenzaNemicaStrategica>().tipoDiSwarm == 0)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					this.pulsanteCombatti.GetComponent<Button>().interactable = true;
					this.pulsanteVittoriaATavolino.GetComponent<Button>().interactable = false;
					this.pulsanteRitirata.GetComponent<Button>().interactable = false;
				}
				else
				{
					this.pulsanteCombatti.GetComponent<Button>().interactable = false;
					this.pulsanteVittoriaATavolino.GetComponent<Button>().interactable = true;
					this.pulsanteRitirata.GetComponent<Button>().interactable = false;
				}
			}
			else
			{
				this.pulsanteCombatti.GetComponent<Button>().interactable = false;
				this.pulsanteVittoriaATavolino.GetComponent<Button>().interactable = true;
				this.pulsanteRitirata.GetComponent<Button>().interactable = false;
			}
		}
		else if (this.ListaEserAlleatiPres.Count > 0)
		{
			this.pulsanteCombatti.GetComponent<Button>().interactable = true;
			this.pulsanteVittoriaATavolino.GetComponent<Button>().interactable = false;
			this.pulsanteRitirata.GetComponent<Button>().interactable = true;
		}
		else
		{
			this.pulsanteCombatti.GetComponent<Button>().interactable = false;
			this.pulsanteVittoriaATavolino.GetComponent<Button>().interactable = false;
			this.pulsanteRitirata.GetComponent<Button>().interactable = true;
		}
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x0011D88C File Offset: 0x0011BA8C
	private void CalcoloBattaglie()
	{
		bool flag = false;
		bool flag2 = false;
		List<GameObject> list = new List<GameObject>();
		this.quiNemicoStaAttaccando = false;
		if (this.esercitiNemiciPresenti > 0)
		{
			for (int i = 0; i < this.ListaEserNemiciPres.Count; i++)
			{
				if (this.ListaEserNemiciPres[i].GetComponent<PresenzaNemicaStrategica>().tipoDiSwarm == 0)
				{
					flag = true;
				}
				else
				{
					flag2 = true;
					list.Add(this.ListaEserNemiciPres[i]);
				}
			}
			if (flag)
			{
				if (this.quiCèStataBattaglia != 2 && this.quiCèStataBattaglia != 3)
				{
					if (this.settoriNemici < 3)
					{
						this.quiNemicoStaAttaccando = true;
						this.numIdentSwarmSpecialeInAtt = 0;
					}
					else
					{
						this.quiNemicoStaAttaccando = false;
					}
				}
				else
				{
					this.quiNemicoStaAttaccando = false;
				}
			}
			if (!this.quiNemicoStaAttaccando && flag2 && list.Count > 0 && !this.bloccoPerBatt)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].GetComponent<PresenzaNemicaStrategica>().swarmSpecialeHaAttaccato == 0)
					{
						if (this.settoriNemici < 3)
						{
							this.quiNemicoStaAttaccando = true;
							this.numIdentSwarmSpecialeInAtt = list[j].GetComponent<PresenzaNemicaStrategica>().numIdentitàNemico;
							break;
						}
						this.quiNemicoStaAttaccando = false;
						this.numIdentSwarmSpecialeInAtt = 0;
					}
					else
					{
						this.quiNemicoStaAttaccando = false;
						this.numIdentSwarmSpecialeInAtt = 0;
					}
				}
			}
		}
		else
		{
			this.quiNemicoStaAttaccando = false;
			this.numIdentSwarmSpecialeInAtt = 0;
		}
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x0011DA18 File Offset: 0x0011BC18
	private void VisualizSchermataBattaglia()
	{
		this.elencoAlleatiSchermBattBarraVert.GetComponent<Scrollbar>().value = 1f;
		this.elencoNemiciSchermBattBarraVert.GetComponent<Scrollbar>().value = 1f;
		this.schermataBattaglia.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "MAP: " + this.nomeStanza;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.ListaSettori.Count; i++)
		{
			if (this.ListaSettori[i] == 1)
			{
				num++;
			}
			else if (this.ListaSettori[i] == 2)
			{
				num2++;
			}
		}
		string str = string.Empty;
		if (GestoreNeutroStrategia.attaccante == 0)
		{
			if (num == 2)
			{
				GestoreNeutroStrategia.tipoBattaglia = 1;
				str = "Destroy the enemy Outpost";
			}
			else if (num == 1)
			{
				GestoreNeutroStrategia.tipoBattaglia = 2;
				str = "Conquer the Flags";
			}
			else if (num == 0)
			{
				GestoreNeutroStrategia.tipoBattaglia = 0;
				str = "Defend the allied Outpost";
			}
		}
		else if (num == 3)
		{
			GestoreNeutroStrategia.tipoBattaglia = 1;
			str = "Destroy the enemy Outpost";
		}
		else if (num == 2)
		{
			GestoreNeutroStrategia.tipoBattaglia = 2;
			str = "Conquer the Flags";
		}
		else if (num == 1)
		{
			GestoreNeutroStrategia.tipoBattaglia = 0;
			str = "Defend the allied Outpost";
		}
		this.schermataBattaglia.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "MISSION: " + str;
		this.schermataBattaglia.transform.GetChild(3).GetComponent<Image>().sprite = this.immagineStanza;
		this.ListaAlleatiInSchermBatt = new List<List<int>>();
		for (int j = 0; j < this.postiInElencoBattaglia; j++)
		{
			List<int> list = new List<int>();
			list.Add(100);
			list.Add(0);
			this.ListaAlleatiInSchermBatt.Add(list);
		}
		for (int k = 0; k < this.numAlleatiPossibili; k++)
		{
			this.ListaAlleatiInSchermBatt[k][0] = k;
			for (int l = 0; l < this.ListaEserAlleatiPres.Count; l++)
			{
				for (int m = 0; m < this.numUnitàInEserAlleato; m++)
				{
					if (this.ListaEserAlleatiPres[l].GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[m] == k)
					{
						List<int> list2;
						List<int> expr_23D = list2 = this.ListaAlleatiInSchermBatt[k];
						int num3;
						int expr_241 = num3 = 1;
						num3 = list2[num3];
						expr_23D[expr_241] = num3 + 1;
					}
				}
			}
		}
		for (int n = 0; n < this.numAlleatiPossibili; n++)
		{
			this.elencoAlleatiSchermBatt.transform.GetChild(n).GetComponent<CanvasGroup>().alpha = 0f;
			this.elencoAlleatiSchermBatt.transform.GetChild(n).GetComponent<CanvasGroup>().interactable = false;
			this.elencoAlleatiSchermBatt.transform.GetChild(n).GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		int num4 = 0;
		for (int num5 = 0; num5 < this.numAlleatiPossibili; num5++)
		{
			if (this.ListaAlleatiInSchermBatt[num5][1] != 0)
			{
				this.elencoAlleatiSchermBatt.transform.GetChild(num4).GetComponent<CanvasGroup>().alpha = 1f;
				this.elencoAlleatiSchermBatt.transform.GetChild(num4).GetComponent<CanvasGroup>().interactable = true;
				this.elencoAlleatiSchermBatt.transform.GetChild(num4).GetComponent<CanvasGroup>().blocksRaycasts = true;
				this.elencoAlleatiSchermBatt.transform.GetChild(num4).GetChild(0).GetComponent<Image>().sprite = this.Headquarters.GetComponent<GestioneEsercitiAlleati>().ListaUnitàAlleate[this.ListaAlleatiInSchermBatt[num5][0]].GetComponent<PresenzaAlleato>().immagineUnità;
				this.elencoAlleatiSchermBatt.transform.GetChild(num4).GetChild(1).GetComponent<Text>().text = this.Headquarters.GetComponent<GestioneEsercitiAlleati>().ListaUnitàAlleate[this.ListaAlleatiInSchermBatt[num5][0]].GetComponent<PresenzaAlleato>().nomeUnità;
				this.elencoAlleatiSchermBatt.transform.GetChild(num4).GetChild(0).GetChild(0).GetComponent<Text>().text = this.ListaAlleatiInSchermBatt[num5][1].ToString();
				num4++;
			}
		}
		this.ListaNemiciInSchermBatt = new List<List<int>>();
		for (int num6 = 0; num6 < this.postiInElencoBattaglia; num6++)
		{
			List<int> list3 = new List<int>();
			list3.Add(100);
			list3.Add(0);
			this.ListaNemiciInSchermBatt.Add(list3);
		}
		if (this.ListaEserNemiciPres.Count == 0)
		{
			for (int num7 = 0; num7 < this.numNemiciPossibili; num7++)
			{
				this.elencoNemiciSchermBatt.transform.GetChild(num7).GetComponent<CanvasGroup>().alpha = 0f;
				this.elencoNemiciSchermBatt.transform.GetChild(num7).GetComponent<CanvasGroup>().interactable = false;
				this.elencoNemiciSchermBatt.transform.GetChild(num7).GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
		}
		else
		{
			if (this.numIdentSwarmSpecialeInAtt == 0)
			{
				for (int num8 = 0; num8 < this.numNemiciPossibili; num8++)
				{
					this.ListaNemiciInSchermBatt[num8][0] = num8;
					for (int num9 = 0; num9 < this.ListaEserNemiciPres.Count; num9++)
					{
						if (this.ListaEserNemiciPres[num9].GetComponent<PresenzaNemicaStrategica>().tipoDiSwarm == 0)
						{
							for (int num10 = 0; num10 < this.ListaEserNemiciPres[num9].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser.Count; num10++)
							{
								if (this.ListaEserNemiciPres[num9].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num10][0] == num8)
								{
									List<int> list4;
									List<int> expr_5F1 = list4 = this.ListaNemiciInSchermBatt[num8];
									int num11;
									int expr_5F5 = num11 = 1;
									num11 = list4[num11];
									expr_5F1[expr_5F5] = num11 + this.ListaEserNemiciPres[num9].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num10][1];
								}
							}
						}
					}
				}
			}
			else
			{
				for (int num12 = 0; num12 < this.Nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici.Count; num12++)
				{
					if (this.Nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num12].GetComponent<PresenzaNemicaStrategica>().numIdentitàNemico == this.numIdentSwarmSpecialeInAtt)
					{
						this.ListaNemiciInSchermBatt[0][0] = this.Nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num12].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][0];
						this.ListaNemiciInSchermBatt[0][1] = this.Nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num12].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1];
						break;
					}
				}
			}
			for (int num13 = 0; num13 < this.numNemiciPossibili; num13++)
			{
				this.elencoNemiciSchermBatt.transform.GetChild(num13).GetComponent<CanvasGroup>().alpha = 0f;
				this.elencoNemiciSchermBatt.transform.GetChild(num13).GetComponent<CanvasGroup>().interactable = false;
				this.elencoNemiciSchermBatt.transform.GetChild(num13).GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
			int num14 = 0;
			for (int num15 = 0; num15 < this.numNemiciPossibili; num15++)
			{
				if (this.ListaNemiciInSchermBatt[num15][1] != 0)
				{
					this.elencoNemiciSchermBatt.transform.GetChild(num14).GetComponent<CanvasGroup>().alpha = 1f;
					this.elencoNemiciSchermBatt.transform.GetChild(num14).GetComponent<CanvasGroup>().interactable = true;
					this.elencoNemiciSchermBatt.transform.GetChild(num14).GetComponent<CanvasGroup>().blocksRaycasts = true;
					this.elencoNemiciSchermBatt.transform.GetChild(num14).GetChild(0).GetComponent<Image>().sprite = this.Nest.GetComponent<IANemicoStrategia>().ListaTipiInsetti[this.ListaNemiciInSchermBatt[num15][0]].GetComponent<PresenzaNemico>().immagineInsetto;
					this.elencoNemiciSchermBatt.transform.GetChild(num14).GetChild(1).GetComponent<Text>().text = this.Nest.GetComponent<IANemicoStrategia>().ListaTipiInsetti[this.ListaNemiciInSchermBatt[num15][0]].GetComponent<PresenzaNemico>().nomeInsetto;
					this.elencoNemiciSchermBatt.transform.GetChild(num14).GetChild(0).GetChild(0).GetComponent<Text>().text = this.ListaNemiciInSchermBatt[num15][1].ToString() + " G";
					num14++;
				}
			}
		}
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x0011E38C File Offset: 0x0011C58C
	private void VisualizzaResocontoBattaglia()
	{
		if (GestoreNeutroStrategia.ripristinaBarraVert)
		{
			GestoreNeutroStrategia.ripristinaBarraVert = false;
			this.elencoAlleatiResocontoBattBarraVert.GetComponent<Scrollbar>().value = 1f;
			this.elencoNemiciResocontoBattBarraVert.GetComponent<Scrollbar>().value = 1f;
		}
		this.scherResocontoBatt.GetComponent<CanvasGroup>().alpha = 1f;
		this.scherResocontoBatt.GetComponent<CanvasGroup>().interactable = true;
		this.scherResocontoBatt.GetComponent<CanvasGroup>().blocksRaycasts = true;
		if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().battagliaATavolino)
		{
			if (GestoreNeutroStrategia.vincitore == 1)
			{
				this.immagineEsitoResBatt.GetComponent<Image>().color = Color.blue;
				this.immagineEsitoResBatt.transform.GetChild(0).GetComponent<Image>().sprite = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().strisciaVittoria;
				this.ricompenseResocontoBatt.GetComponent<Text>().text = "Only the Generals who face the Enemy deserve a Reward!";
			}
			else if (GestoreNeutroStrategia.vincitore == 2)
			{
				this.immagineEsitoResBatt.GetComponent<Image>().color = Color.red;
				this.immagineEsitoResBatt.transform.GetChild(0).GetComponent<Image>().sprite = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().strisciaSconfitta;
				this.ricompenseResocontoBatt.GetComponent<Text>().text = string.Concat(new string[]
				{
					"No Reward for losers!\n\nSTOLEN FOOD:   \n\nFresh Food:\n",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ricompensaFreshFoodNemico.ToString(),
					"\n\nRotten Food:\n",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ricompensaRottenFoodNemico.ToString(),
					"\n\nHigh-Protein Food:\n",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ricompensaHighProteinFoodNemico.ToString()
				});
			}
		}
		else if (GestoreNeutroStrategia.vincitore == 1)
		{
			this.immagineEsitoResBatt.GetComponent<Image>().color = Color.blue;
			this.immagineEsitoResBatt.transform.GetChild(0).GetComponent<Image>().sprite = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().strisciaVittoria;
			this.ricompenseResocontoBatt.GetComponent<Text>().text = string.Concat(new string[]
			{
				"YOUR REWARD:\n\n",
				this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[0][0]].GetComponent<PresenzaRisorsa>().nomeRisorsa,
				":\n",
				this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[0][1].ToString(),
				"\n\n",
				this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[1][0]].GetComponent<PresenzaRisorsa>().nomeRisorsa,
				":\n",
				this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[1][1].ToString(),
				"\n\n",
				this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[2][0]].GetComponent<PresenzaRisorsa>().nomeRisorsa,
				":\n",
				this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[2][1].ToString(),
				"\n\n",
				this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[3][0]].GetComponent<PresenzaRisorsa>().nomeRisorsa,
				":\n",
				this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[3][1].ToString()
			});
		}
		else if (GestoreNeutroStrategia.vincitore == 2)
		{
			this.immagineEsitoResBatt.GetComponent<Image>().color = Color.red;
			this.immagineEsitoResBatt.transform.GetChild(0).GetComponent<Image>().sprite = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().strisciaSconfitta;
			this.ricompenseResocontoBatt.GetComponent<Text>().text = string.Concat(new string[]
			{
				"No Reward for losers!\n\nSTOLEN FOOD:   \n\nFresh Food:\n",
				this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ricompensaFreshFoodNemico.ToString(),
				"\n\nRotten Food:\n",
				this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ricompensaRottenFoodNemico.ToString(),
				"\n\nHigh-Protein Food:\n",
				this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ricompensaHighProteinFoodNemico.ToString()
			});
		}
		if (GestoreNeutroStrategia.mostraElencoResoconto)
		{
			GestoreNeutroStrategia.mostraElencoResoconto = false;
			if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().battagliaATavolino)
			{
				this.ListaAlleatiInSchermBatt = new List<List<int>>();
				for (int i = 0; i < this.postiInElencoBattaglia; i++)
				{
					List<int> list = new List<int>();
					list.Add(100);
					list.Add(0);
					this.ListaAlleatiInSchermBatt.Add(list);
				}
				for (int j = 0; j < this.numAlleatiPossibili; j++)
				{
					this.ListaAlleatiInSchermBatt[j][0] = j;
					for (int k = 0; k < this.ListaEserAlleatiPres.Count; k++)
					{
						for (int l = 0; l < this.numUnitàInEserAlleato; l++)
						{
							if (this.ListaEserAlleatiPres[k].GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[l] == j)
							{
								List<int> list2;
								List<int> expr_587 = list2 = this.ListaAlleatiInSchermBatt[j];
								int num;
								int expr_58B = num = 1;
								num = list2[num];
								expr_587[expr_58B] = num + 1;
							}
						}
					}
				}
				for (int m = 0; m < this.numAlleatiPossibili; m++)
				{
					this.elencoAlleatiResocontoBatt.transform.GetChild(m).GetComponent<CanvasGroup>().alpha = 0f;
					this.elencoAlleatiResocontoBatt.transform.GetChild(m).GetComponent<CanvasGroup>().interactable = false;
					this.elencoAlleatiResocontoBatt.transform.GetChild(m).GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
				int num2 = 0;
				for (int n = 0; n < this.numAlleatiPossibili; n++)
				{
					if (this.ListaAlleatiInSchermBatt[n][1] != 0)
					{
						this.elencoAlleatiResocontoBatt.transform.GetChild(num2).GetComponent<CanvasGroup>().alpha = 1f;
						this.elencoAlleatiResocontoBatt.transform.GetChild(num2).GetComponent<CanvasGroup>().interactable = true;
						this.elencoAlleatiResocontoBatt.transform.GetChild(num2).GetComponent<CanvasGroup>().blocksRaycasts = true;
						this.elencoAlleatiResocontoBatt.transform.GetChild(num2).GetChild(0).GetComponent<Image>().sprite = this.Headquarters.GetComponent<GestioneEsercitiAlleati>().ListaUnitàAlleate[this.ListaAlleatiInSchermBatt[n][0]].GetComponent<PresenzaAlleato>().immagineUnità;
						this.elencoAlleatiResocontoBatt.transform.GetChild(num2).GetChild(1).GetComponent<Text>().text = this.Headquarters.GetComponent<GestioneEsercitiAlleati>().ListaUnitàAlleate[this.ListaAlleatiInSchermBatt[n][0]].GetComponent<PresenzaAlleato>().nomeUnità;
						this.elencoAlleatiResocontoBatt.transform.GetChild(num2).GetChild(2).GetComponent<Text>().text = "REMAINING: " + this.ListaAlleatiInSchermBatt[n][1].ToString();
						this.elencoAlleatiResocontoBatt.transform.GetChild(num2).GetChild(3).GetComponent<Text>().text = "DMG: 0 HP";
						num2++;
					}
				}
				this.ListaNemiciInSchermBatt = new List<List<int>>();
				for (int num3 = 0; num3 < this.postiInElencoBattaglia; num3++)
				{
					List<int> list3 = new List<int>();
					list3.Add(100);
					list3.Add(0);
					this.ListaNemiciInSchermBatt.Add(list3);
				}
				if (this.ListaEserNemiciPres.Count == 0)
				{
					for (int num4 = 0; num4 < this.numNemiciPossibili; num4++)
					{
						this.elencoNemiciResocontoBatt.transform.GetChild(num4).GetComponent<CanvasGroup>().alpha = 0f;
						this.elencoNemiciResocontoBatt.transform.GetChild(num4).GetComponent<CanvasGroup>().interactable = false;
						this.elencoNemiciResocontoBatt.transform.GetChild(num4).GetComponent<CanvasGroup>().blocksRaycasts = false;
					}
				}
				else
				{
					if (this.numIdentSwarmSpecialeInAtt == 0)
					{
						for (int num5 = 0; num5 < this.numNemiciPossibili; num5++)
						{
							this.ListaNemiciInSchermBatt[num5][0] = num5;
							for (int num6 = 0; num6 < this.ListaEserNemiciPres.Count; num6++)
							{
								if (this.ListaEserNemiciPres[num6].GetComponent<PresenzaNemicaStrategica>().tipoDiSwarm == 0)
								{
									for (int num7 = 0; num7 < this.ListaEserNemiciPres[num6].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser.Count; num7++)
									{
										if (this.ListaEserNemiciPres[num6].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num7][0] == num5)
										{
											List<int> list4;
											List<int> expr_960 = list4 = this.ListaNemiciInSchermBatt[num5];
											int num8;
											int expr_964 = num8 = 1;
											num8 = list4[num8];
											expr_960[expr_964] = num8 + this.ListaEserNemiciPres[num6].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[num7][1];
										}
									}
								}
							}
						}
					}
					else
					{
						for (int num9 = 0; num9 < this.Nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici.Count; num9++)
						{
							if (this.Nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num9].GetComponent<PresenzaNemicaStrategica>().numIdentitàNemico == this.numIdentSwarmSpecialeInAtt)
							{
								this.ListaNemiciInSchermBatt[0][0] = this.Nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num9].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][0];
								this.ListaNemiciInSchermBatt[0][1] = this.Nest.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[num9].GetComponent<PresenzaNemicaStrategica>().ListaInsettiInEser[0][1];
							}
						}
					}
					for (int num10 = 0; num10 < this.numNemiciPossibili; num10++)
					{
						this.elencoNemiciResocontoBatt.transform.GetChild(num10).GetComponent<CanvasGroup>().alpha = 0f;
						this.elencoNemiciResocontoBatt.transform.GetChild(num10).GetComponent<CanvasGroup>().interactable = false;
						this.elencoNemiciResocontoBatt.transform.GetChild(num10).GetComponent<CanvasGroup>().blocksRaycasts = false;
					}
					int num11 = 0;
					for (int num12 = 0; num12 < this.numNemiciPossibili; num12++)
					{
						if (this.ListaNemiciInSchermBatt[num12][1] != 0)
						{
							this.elencoNemiciResocontoBatt.transform.GetChild(num11).GetComponent<CanvasGroup>().alpha = 1f;
							this.elencoNemiciResocontoBatt.transform.GetChild(num11).GetComponent<CanvasGroup>().interactable = true;
							this.elencoNemiciResocontoBatt.transform.GetChild(num11).GetComponent<CanvasGroup>().blocksRaycasts = true;
							this.elencoNemiciResocontoBatt.transform.GetChild(num11).GetChild(0).GetComponent<Image>().sprite = this.Nest.GetComponent<IANemicoStrategia>().ListaTipiInsetti[this.ListaNemiciInSchermBatt[num12][0]].GetComponent<PresenzaNemico>().immagineInsetto;
							this.elencoNemiciResocontoBatt.transform.GetChild(num11).GetChild(1).GetComponent<Text>().text = this.Nest.GetComponent<IANemicoStrategia>().ListaTipiInsetti[this.ListaNemiciInSchermBatt[num12][0]].GetComponent<PresenzaNemico>().nomeInsetto;
							this.elencoNemiciResocontoBatt.transform.GetChild(num11).GetChild(2).GetComponent<Text>().text = "REMAINING: " + this.ListaNemiciInSchermBatt[num12][1].ToString() + " G";
							this.elencoNemiciResocontoBatt.transform.GetChild(num11).GetChild(3).GetComponent<Text>().text = "DMG: 0 HP";
							num11++;
						}
					}
				}
			}
			else
			{
				this.ListaAlleatiInSchermBatt = new List<List<int>>();
				for (int num13 = 0; num13 < this.postiInElencoBattaglia; num13++)
				{
					List<int> list5 = new List<int>();
					list5.Add(100);
					list5.Add(0);
					this.ListaAlleatiInSchermBatt.Add(list5);
				}
				for (int num14 = 0; num14 < this.numAlleatiPossibili; num14++)
				{
					this.ListaAlleatiInSchermBatt[num14][0] = num14;
					if (this.ListaAllPresentiInBatt[num14] == 1)
					{
						this.ListaAlleatiInSchermBatt[num14][1] = this.ListaAllSopravv[num14];
					}
				}
				for (int num15 = 0; num15 < this.numAlleatiPossibili; num15++)
				{
					this.elencoAlleatiResocontoBatt.transform.GetChild(num15).GetComponent<CanvasGroup>().alpha = 0f;
					this.elencoAlleatiResocontoBatt.transform.GetChild(num15).GetComponent<CanvasGroup>().interactable = false;
					this.elencoAlleatiResocontoBatt.transform.GetChild(num15).GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
				int num16 = 0;
				for (int num17 = 0; num17 < this.numAlleatiPossibili; num17++)
				{
					if (this.ListaAllPresentiInBatt[this.ListaAlleatiInSchermBatt[num17][0]] != 0)
					{
						if (this.ListaAlleatiInSchermBatt[num17][1] == 0)
						{
							this.elencoAlleatiResocontoBatt.transform.GetChild(num16).GetComponent<CanvasGroup>().alpha = 0.7f;
						}
						else
						{
							this.elencoAlleatiResocontoBatt.transform.GetChild(num16).GetComponent<CanvasGroup>().alpha = 1f;
						}
						this.elencoAlleatiResocontoBatt.transform.GetChild(num16).GetComponent<CanvasGroup>().interactable = true;
						this.elencoAlleatiResocontoBatt.transform.GetChild(num16).GetComponent<CanvasGroup>().blocksRaycasts = true;
						this.elencoAlleatiResocontoBatt.transform.GetChild(num16).GetChild(0).GetComponent<Image>().sprite = this.Headquarters.GetComponent<GestioneEsercitiAlleati>().ListaUnitàAlleate[this.ListaAlleatiInSchermBatt[num17][0]].GetComponent<PresenzaAlleato>().immagineUnità;
						this.elencoAlleatiResocontoBatt.transform.GetChild(num16).GetChild(1).GetComponent<Text>().text = this.Headquarters.GetComponent<GestioneEsercitiAlleati>().ListaUnitàAlleate[this.ListaAlleatiInSchermBatt[num17][0]].GetComponent<PresenzaAlleato>().nomeUnità;
						this.elencoAlleatiResocontoBatt.transform.GetChild(num16).GetChild(2).GetComponent<Text>().text = "REMAINING: " + this.ListaAlleatiInSchermBatt[num17][1].ToString();
						this.elencoAlleatiResocontoBatt.transform.GetChild(num16).GetChild(3).GetComponent<Text>().text = "DMG: " + this.ListaDanniAlleati[num17].ToString("F0") + " HP";
						num16++;
					}
				}
				this.ListaNemiciInSchermBatt = new List<List<int>>();
				for (int num18 = 0; num18 < this.postiInElencoBattaglia; num18++)
				{
					List<int> list6 = new List<int>();
					list6.Add(100);
					list6.Add(0);
					this.ListaNemiciInSchermBatt.Add(list6);
				}
				for (int num19 = 0; num19 < this.numNemiciPossibili; num19++)
				{
					this.ListaNemiciInSchermBatt[num19][0] = num19;
					if (this.ListaNemPresentiInBatt[num19] == 1)
					{
						this.ListaNemiciInSchermBatt[num19][1] = this.ListaNemSopravv[num19];
					}
				}
				for (int num20 = 0; num20 < this.numNemiciPossibili; num20++)
				{
					this.elencoNemiciResocontoBatt.transform.GetChild(num20).GetComponent<CanvasGroup>().alpha = 0f;
					this.elencoNemiciResocontoBatt.transform.GetChild(num20).GetComponent<CanvasGroup>().interactable = false;
					this.elencoNemiciResocontoBatt.transform.GetChild(num20).GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
				int num21 = 0;
				for (int num22 = 0; num22 < this.numNemiciPossibili; num22++)
				{
					if (this.ListaNemPresentiInBatt[this.ListaNemiciInSchermBatt[num22][0]] != 0)
					{
						if (this.ListaNemiciInSchermBatt[num22][1] == 0)
						{
							this.elencoNemiciResocontoBatt.transform.GetChild(num21).GetComponent<CanvasGroup>().alpha = 0.7f;
						}
						else
						{
							this.elencoNemiciResocontoBatt.transform.GetChild(num21).GetComponent<CanvasGroup>().alpha = 1f;
						}
						this.elencoNemiciResocontoBatt.transform.GetChild(num21).GetComponent<CanvasGroup>().interactable = true;
						this.elencoNemiciResocontoBatt.transform.GetChild(num21).GetComponent<CanvasGroup>().blocksRaycasts = true;
						this.elencoNemiciResocontoBatt.transform.GetChild(num21).GetChild(0).GetComponent<Image>().sprite = this.Nest.GetComponent<IANemicoStrategia>().ListaTipiInsetti[this.ListaNemiciInSchermBatt[num22][0]].GetComponent<PresenzaNemico>().immagineInsetto;
						this.elencoNemiciResocontoBatt.transform.GetChild(num21).GetChild(1).GetComponent<Text>().text = this.Nest.GetComponent<IANemicoStrategia>().ListaTipiInsetti[this.ListaNemiciInSchermBatt[num22][0]].GetComponent<PresenzaNemico>().nomeInsetto;
						this.elencoNemiciResocontoBatt.transform.GetChild(num21).GetChild(2).GetComponent<Text>().text = "REMAINING: " + this.ListaNemiciInSchermBatt[num22][1].ToString() + " G";
						this.elencoNemiciResocontoBatt.transform.GetChild(num21).GetChild(3).GetComponent<Text>().text = "DMG: " + this.ListaDanniNemici[num22].ToString("F0") + " HP";
						num21++;
					}
				}
			}
		}
		this.perditeInBatt.transform.GetChild(0).GetComponent<Text>().text = "Dead Allies:  " + GestoreNeutroTattica.numAlleatiMorti;
		this.perditeInBatt.transform.GetChild(1).GetComponent<Text>().text = "Dead Enemies:  " + GestoreNeutroTattica.numNemiciMorti;
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x0011F778 File Offset: 0x0011D978
	private void VisualizzaSchermataMissione()
	{
		this.elencoAlleatiSchermMissBarraVert.GetComponent<Scrollbar>().value = 1f;
		this.schermataMissione.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "MAP: " + this.nomeStanza;
		string str = string.Empty;
		if (GestoreNeutroStrategia.tipoBattaglia == 3)
		{
			str = "Defend the recon squad until the extraction";
		}
		else if (GestoreNeutroStrategia.tipoBattaglia == 4)
		{
			str = "Defend the Supply Crate until the extraction";
		}
		else if (GestoreNeutroStrategia.tipoBattaglia == 5)
		{
			str = "Defend the Satellite";
		}
		else if (GestoreNeutroStrategia.tipoBattaglia == 6)
		{
			str = "Escort the Allied Convoy";
		}
		else if (GestoreNeutroStrategia.tipoBattaglia == 7)
		{
			str = "Destroy the Enemy Convoy";
		}
		this.schermataMissione.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "MISSION: " + str;
		this.schermataMissione.transform.GetChild(3).GetComponent<Image>().sprite = this.immagineStanza;
		this.schermataMissione.transform.GetChild(4).GetChild(0).GetChild(0).GetComponent<Text>().text = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaDescrizMissioni[GestoreNeutroStrategia.tipoBattaglia].GetComponent<Text>().text;
		this.ListaAlleatiInSchermBatt = new List<List<int>>();
		for (int i = 0; i < this.postiInElencoBattaglia; i++)
		{
			List<int> list = new List<int>();
			list.Add(100);
			list.Add(0);
			this.ListaAlleatiInSchermBatt.Add(list);
		}
		if (GestoreNeutroStrategia.tipoBattaglia == 4 || GestoreNeutroStrategia.tipoBattaglia == 6 || GestoreNeutroStrategia.tipoBattaglia == 7)
		{
			for (int j = 0; j < this.numAlleatiPossibili; j++)
			{
				this.ListaAlleatiInSchermBatt[j][0] = j;
				for (int k = 0; k < this.ListaEserAlleatiPres.Count; k++)
				{
					for (int l = 0; l < this.numUnitàInEserAlleato; l++)
					{
						if (this.ListaEserAlleatiPres[k].GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[l] == j)
						{
							List<int> list2;
							List<int> expr_20B = list2 = this.ListaAlleatiInSchermBatt[j];
							int num;
							int expr_20F = num = 1;
							num = list2[num];
							expr_20B[expr_20F] = num + 1;
						}
					}
				}
			}
		}
		else if (GestoreNeutroStrategia.tipoBattaglia == 3)
		{
			int num2 = 0;
			for (int m = 0; m < 8; m++)
			{
				bool flag = false;
				int num3 = UnityEngine.Random.Range(0, 14);
				int num4 = 0;
				while (num4 < this.ListaAlleatiInSchermBatt.Count && !flag)
				{
					if (this.ListaAlleatiInSchermBatt[num4][0] != 100)
					{
						if (this.ListaAlleatiInSchermBatt[num4][0] == num3)
						{
							List<int> list3;
							List<int> expr_2D2 = list3 = this.ListaAlleatiInSchermBatt[num4];
							int num;
							int expr_2D6 = num = 1;
							num = list3[num];
							expr_2D2[expr_2D6] = num + 1;
							flag = true;
						}
					}
					else
					{
						this.ListaAlleatiInSchermBatt[num4][0] = num3;
						List<int> list4;
						List<int> expr_317 = list4 = this.ListaAlleatiInSchermBatt[num4];
						int num;
						int expr_31B = num = 1;
						num = list4[num];
						expr_317[expr_31B] = num + 1;
						flag = true;
						num2 = num4;
					}
					num4++;
				}
			}
			this.ListaAlleatiInSchermBatt[num2 + 1][0] = 38;
			this.ListaAlleatiInSchermBatt[num2 + 1][1] = 4;
			this.ListaAlleatiInSchermBatt[num2 + 2][0] = 39;
			this.ListaAlleatiInSchermBatt[num2 + 2][1] = 4;
			this.ListaAlleatiInSchermBatt[num2 + 3][0] = 40;
			this.ListaAlleatiInSchermBatt[num2 + 3][1] = 4;
			this.ListaAlleatiInSchermBatt[num2 + 4][0] = 41;
			this.ListaAlleatiInSchermBatt[num2 + 4][1] = 4;
			this.ListaAlleatiInSchermBatt[num2 + 5][0] = 42;
			this.ListaAlleatiInSchermBatt[num2 + 5][1] = 4;
			this.ListaAlleatiInSchermBatt[num2 + 6][0] = 43;
			this.ListaAlleatiInSchermBatt[num2 + 6][1] = 1;
			this.ListaAlleatiInSchermBatt[num2 + 7][0] = 45;
			this.ListaAlleatiInSchermBatt[num2 + 7][1] = 1;
			this.ListaAlleatiInSchermBatt[num2 + 8][0] = 46;
			this.ListaAlleatiInSchermBatt[num2 + 8][1] = 1;
		}
		else if (GestoreNeutroStrategia.tipoBattaglia == 5)
		{
			this.ListaAlleatiInSchermBatt[0][0] = 38;
			this.ListaAlleatiInSchermBatt[0][1] = 20;
			this.ListaAlleatiInSchermBatt[1][0] = 39;
			this.ListaAlleatiInSchermBatt[1][1] = 20;
			this.ListaAlleatiInSchermBatt[2][0] = 45;
			this.ListaAlleatiInSchermBatt[2][1] = 1;
		}
		for (int n = 0; n < this.numAlleatiPossibili; n++)
		{
			this.elencoAlleatiSchermMiss.transform.GetChild(n).GetComponent<CanvasGroup>().alpha = 0f;
			this.elencoAlleatiSchermMiss.transform.GetChild(n).GetComponent<CanvasGroup>().interactable = false;
			this.elencoAlleatiSchermMiss.transform.GetChild(n).GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		int num5 = 0;
		for (int num6 = 0; num6 < this.numAlleatiPossibili; num6++)
		{
			if (this.ListaAlleatiInSchermBatt[num6][1] != 0)
			{
				this.elencoAlleatiSchermMiss.transform.GetChild(num5).GetComponent<CanvasGroup>().alpha = 1f;
				this.elencoAlleatiSchermMiss.transform.GetChild(num5).GetComponent<CanvasGroup>().interactable = true;
				this.elencoAlleatiSchermMiss.transform.GetChild(num5).GetComponent<CanvasGroup>().blocksRaycasts = true;
				this.elencoAlleatiSchermMiss.transform.GetChild(num5).GetChild(0).GetComponent<Image>().sprite = this.Headquarters.GetComponent<GestioneEsercitiAlleati>().ListaUnitàAlleate[this.ListaAlleatiInSchermBatt[num6][0]].GetComponent<PresenzaAlleato>().immagineUnità;
				this.elencoAlleatiSchermMiss.transform.GetChild(num5).GetChild(1).GetComponent<Text>().text = this.Headquarters.GetComponent<GestioneEsercitiAlleati>().ListaUnitàAlleate[this.ListaAlleatiInSchermBatt[num6][0]].GetComponent<PresenzaAlleato>().nomeUnità;
				this.elencoAlleatiSchermMiss.transform.GetChild(num5).GetChild(0).GetChild(0).GetComponent<Text>().text = this.ListaAlleatiInSchermBatt[num6][1].ToString();
				num5++;
			}
		}
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x0011FED4 File Offset: 0x0011E0D4
	private void VisualizzaResocontoMissione()
	{
		if (GestoreNeutroStrategia.ripristinaBarraVert)
		{
			GestoreNeutroStrategia.ripristinaBarraVert = false;
			this.elencoAlleatiResocontoMissioneBarraVert.GetComponent<Scrollbar>().value = 1f;
		}
		this.scherResocontoMissione.GetComponent<CanvasGroup>().alpha = 1f;
		this.scherResocontoMissione.GetComponent<CanvasGroup>().interactable = true;
		this.scherResocontoMissione.GetComponent<CanvasGroup>().blocksRaycasts = true;
		string text = string.Empty;
		if (GestoreNeutroStrategia.vincitore == 2)
		{
			this.immagineEsitoResMiss.GetComponent<Image>().color = Color.red;
			this.immagineEsitoResMiss.transform.GetChild(0).GetComponent<Image>().sprite = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().strisciaSconfitta;
			this.resocontoMissioneTesto.GetComponent<Text>().text = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaDescrPremioMissNemico[GestoreNeutroStrategia.tipoBattaglia - 3].GetComponent<Text>().text;
			if (GestoreNeutroStrategia.tipoBattaglia == 7)
			{
				text = string.Concat(new object[]
				{
					"STOLEN FOOD:   \n\nFresh Food:\n",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ricompensaFreshFoodNemico.ToString(),
					"  x",
					GestoreNeutroStrategia.convogliArrivati,
					"\n\nRotten Food:\n",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ricompensaRottenFoodNemico.ToString(),
					"  x",
					GestoreNeutroStrategia.convogliArrivati,
					"\n\nHigh-Protein Food:\n",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ricompensaHighProteinFoodNemico.ToString(),
					"  x",
					GestoreNeutroStrategia.convogliArrivati
				});
			}
			else
			{
				text = string.Concat(new string[]
				{
					"STOLEN FOOD:   \n\nFresh Food:\n",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ricompensaFreshFoodNemico.ToString(),
					"\n\nRotten Food:\n",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ricompensaRottenFoodNemico.ToString(),
					"\n\nHigh-Protein Food:\n",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ricompensaHighProteinFoodNemico.ToString()
				});
			}
		}
		else if (GestoreNeutroStrategia.vincitore == 1)
		{
			this.immagineEsitoResMiss.GetComponent<Image>().color = Color.blue;
			this.immagineEsitoResMiss.transform.GetChild(0).GetComponent<Image>().sprite = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().strisciaVittoria;
			this.resocontoMissioneTesto.GetComponent<Text>().text = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaDescrPremioMissAlleato[GestoreNeutroStrategia.tipoBattaglia - 3].GetComponent<Text>().text;
			if (GestoreNeutroStrategia.tipoBattaglia == 3)
			{
				text = string.Concat(new object[]
				{
					this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[0][0]].GetComponent<PresenzaRisorsa>().nomeRisorsa,
					":   ",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[0][1].ToString(),
					"\n\n",
					this.Headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseArmi[0][0]].GetComponent<QuantitàMunizione>().name,
					":   ",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseArmi[0][1].ToString(),
					"  x",
					GestoreNeutroStrategia.soldatiSalvatiInBatt3,
					"\n\n",
					this.Headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseArmi[1][0]].GetComponent<QuantitàMunizione>().name,
					":   ",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseArmi[1][1].ToString(),
					"  x",
					GestoreNeutroStrategia.soldatiSalvatiInBatt3,
					"\n\n",
					this.Headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseArmi[2][0]].GetComponent<QuantitàMunizione>().name,
					":   ",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseArmi[2][1].ToString(),
					"  x",
					GestoreNeutroStrategia.soldatiSalvatiInBatt3
				});
			}
			if (GestoreNeutroStrategia.tipoBattaglia == 4 || GestoreNeutroStrategia.tipoBattaglia == 5)
			{
				text = string.Concat(new string[]
				{
					this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[0][0]].GetComponent<PresenzaRisorsa>().nomeRisorsa,
					":   ",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[0][1].ToString(),
					"\n\n",
					this.Headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseArmi[0][0]].GetComponent<QuantitàMunizione>().name,
					":   ",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseArmi[0][1].ToString(),
					"\n\n",
					this.Headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseArmi[1][0]].GetComponent<QuantitàMunizione>().name,
					":   ",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseArmi[1][1].ToString(),
					"\n\n",
					this.Headquarters.GetComponent<GestioneMunizioniMercatoESupporto>().ListaTipiMunizioniBaseStrategia[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseArmi[2][0]].GetComponent<QuantitàMunizione>().name,
					":   ",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseArmi[2][1].ToString()
				});
			}
			else if (GestoreNeutroStrategia.tipoBattaglia == 6)
			{
				text = string.Concat(new object[]
				{
					this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[0][0]].GetComponent<PresenzaRisorsa>().nomeRisorsa,
					":\n",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[0][1].ToString(),
					"\n\n",
					this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[1][0]].GetComponent<PresenzaRisorsa>().nomeRisorsa,
					":\n",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[1][1].ToString(),
					"  x",
					GestoreNeutroStrategia.convogliArrivati,
					"\n\n",
					this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[2][0]].GetComponent<PresenzaRisorsa>().nomeRisorsa,
					":\n",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[2][1].ToString(),
					"  x",
					GestoreNeutroStrategia.convogliArrivati,
					"\n\n",
					this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[3][0]].GetComponent<PresenzaRisorsa>().nomeRisorsa,
					":\n",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[3][1].ToString(),
					"  x",
					GestoreNeutroStrategia.convogliArrivati
				});
			}
			else if (GestoreNeutroStrategia.tipoBattaglia == 7)
			{
				text = string.Concat(new string[]
				{
					this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[0][0]].GetComponent<PresenzaRisorsa>().nomeRisorsa,
					":\n",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[0][1].ToString(),
					"\n\n",
					this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[1][0]].GetComponent<PresenzaRisorsa>().nomeRisorsa,
					":\n",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[1][1].ToString(),
					"\n\n",
					this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[2][0]].GetComponent<PresenzaRisorsa>().nomeRisorsa,
					":\n",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[2][1].ToString(),
					"\n\n",
					this.Headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[3][0]].GetComponent<PresenzaRisorsa>().nomeRisorsa,
					":\n",
					this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaRicompenseRisorse[3][1].ToString()
				});
			}
		}
		this.resocontoMissionePremi.GetComponent<Text>().text = text;
		if (GestoreNeutroStrategia.mostraElencoResoconto)
		{
			GestoreNeutroStrategia.mostraElencoResoconto = false;
			if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().battagliaATavolino)
			{
				this.ListaAlleatiInSchermBatt = new List<List<int>>();
				for (int i = 0; i < this.postiInElencoBattaglia; i++)
				{
					List<int> list = new List<int>();
					list.Add(100);
					list.Add(0);
					this.ListaAlleatiInSchermBatt.Add(list);
				}
				for (int j = 0; j < this.numAlleatiPossibili; j++)
				{
					this.ListaAlleatiInSchermBatt[j][0] = j;
					for (int k = 0; k < this.ListaEserAlleatiPres.Count; k++)
					{
						for (int l = 0; l < this.numUnitàInEserAlleato; l++)
						{
							if (this.ListaEserAlleatiPres[k].GetComponent<PresenzaAlleataStrategica>().ListaTruppeInEser[l] == j)
							{
								List<int> list2;
								List<int> expr_BBE = list2 = this.ListaAlleatiInSchermBatt[j];
								int num;
								int expr_BC2 = num = 1;
								num = list2[num];
								expr_BBE[expr_BC2] = num + 1;
							}
						}
					}
				}
				for (int m = 0; m < this.numAlleatiPossibili; m++)
				{
					this.elencoAlleatiResocontoBatt.transform.GetChild(m).GetComponent<CanvasGroup>().alpha = 0f;
					this.elencoAlleatiResocontoBatt.transform.GetChild(m).GetComponent<CanvasGroup>().interactable = false;
					this.elencoAlleatiResocontoBatt.transform.GetChild(m).GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
				int num2 = 0;
				for (int n = 0; n < this.numAlleatiPossibili; n++)
				{
					if (this.ListaAlleatiInSchermBatt[n][1] != 0)
					{
						this.elencoAlleatiResocontoMissione.transform.GetChild(num2).GetComponent<CanvasGroup>().alpha = 1f;
						this.elencoAlleatiResocontoMissione.transform.GetChild(num2).GetComponent<CanvasGroup>().interactable = true;
						this.elencoAlleatiResocontoMissione.transform.GetChild(num2).GetComponent<CanvasGroup>().blocksRaycasts = true;
						this.elencoAlleatiResocontoMissione.transform.GetChild(num2).GetChild(0).GetComponent<Image>().sprite = this.Headquarters.GetComponent<GestioneEsercitiAlleati>().ListaUnitàAlleate[this.ListaAlleatiInSchermBatt[n][0]].GetComponent<PresenzaAlleato>().immagineUnità;
						this.elencoAlleatiResocontoMissione.transform.GetChild(num2).GetChild(1).GetComponent<Text>().text = this.Headquarters.GetComponent<GestioneEsercitiAlleati>().ListaUnitàAlleate[this.ListaAlleatiInSchermBatt[n][0]].GetComponent<PresenzaAlleato>().nomeUnità;
						this.elencoAlleatiResocontoMissione.transform.GetChild(num2).GetChild(2).GetComponent<Text>().text = "REMAINING: " + this.ListaAlleatiInSchermBatt[n][1].ToString();
						this.elencoAlleatiResocontoMissione.transform.GetChild(num2).GetChild(3).GetComponent<Text>().text = "DMG: 0 HP";
						num2++;
					}
				}
			}
			else
			{
				this.ListaAlleatiInSchermBatt = new List<List<int>>();
				for (int num3 = 0; num3 < this.postiInElencoBattaglia; num3++)
				{
					List<int> list3 = new List<int>();
					list3.Add(100);
					list3.Add(0);
					this.ListaAlleatiInSchermBatt.Add(list3);
				}
				for (int num4 = 0; num4 < this.numAlleatiPossibili; num4++)
				{
					this.ListaAlleatiInSchermBatt[num4][0] = num4;
					if (this.ListaAllPresentiInBatt[num4] == 1)
					{
						this.ListaAlleatiInSchermBatt[num4][1] = this.ListaAllSopravv[num4];
					}
				}
				for (int num5 = 0; num5 < this.numAlleatiPossibili; num5++)
				{
					this.elencoAlleatiResocontoMissione.transform.GetChild(num5).GetComponent<CanvasGroup>().alpha = 0f;
					this.elencoAlleatiResocontoMissione.transform.GetChild(num5).GetComponent<CanvasGroup>().interactable = false;
					this.elencoAlleatiResocontoMissione.transform.GetChild(num5).GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
				int num6 = 0;
				for (int num7 = 0; num7 < this.numAlleatiPossibili; num7++)
				{
					if (this.ListaAllPresentiInBatt[this.ListaAlleatiInSchermBatt[num7][0]] != 0)
					{
						if (this.ListaAlleatiInSchermBatt[num7][1] == 0)
						{
							this.elencoAlleatiResocontoMissione.transform.GetChild(num6).GetComponent<CanvasGroup>().alpha = 0.7f;
						}
						else
						{
							this.elencoAlleatiResocontoMissione.transform.GetChild(num6).GetComponent<CanvasGroup>().alpha = 1f;
						}
						this.elencoAlleatiResocontoMissione.transform.GetChild(num6).GetComponent<CanvasGroup>().interactable = true;
						this.elencoAlleatiResocontoMissione.transform.GetChild(num6).GetComponent<CanvasGroup>().blocksRaycasts = true;
						this.elencoAlleatiResocontoMissione.transform.GetChild(num6).GetChild(0).GetComponent<Image>().sprite = this.Headquarters.GetComponent<GestioneEsercitiAlleati>().ListaUnitàAlleate[this.ListaAlleatiInSchermBatt[num7][0]].GetComponent<PresenzaAlleato>().immagineUnità;
						this.elencoAlleatiResocontoMissione.transform.GetChild(num6).GetChild(1).GetComponent<Text>().text = this.Headquarters.GetComponent<GestioneEsercitiAlleati>().ListaUnitàAlleate[this.ListaAlleatiInSchermBatt[num7][0]].GetComponent<PresenzaAlleato>().nomeUnità;
						this.elencoAlleatiResocontoMissione.transform.GetChild(num6).GetChild(2).GetComponent<Text>().text = "REMAINING: " + this.ListaAlleatiInSchermBatt[num7][1].ToString();
						this.elencoAlleatiResocontoMissione.transform.GetChild(num6).GetChild(3).GetComponent<Text>().text = "DMG: " + this.ListaDanniAlleati[num7].ToString("F0") + " HP";
						if (GestoreNeutroStrategia.tipoBattaglia == 3)
						{
							this.elencoAlleatiResocontoMissione.transform.GetChild(num6).GetChild(2).GetComponent<Text>().text = "REMAINING: -";
						}
						num6++;
					}
				}
			}
		}
		if (GestoreNeutroStrategia.controlloSatellite)
		{
			GestoreNeutroStrategia.controlloSatellite = false;
			if (GestoreNeutroStrategia.vincitore == 2)
			{
				int index = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.IndexOf(base.gameObject);
				this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaSatelliti[index] = 0;
				UnityEngine.Object.Destroy(this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[index].transform.FindChild("satellite(Clone)").gameObject);
			}
		}
		this.perditeinMiss.transform.GetChild(0).GetComponent<Text>().text = "Dead Allies:  " + GestoreNeutroTattica.numAlleatiMorti;
		this.perditeinMiss.transform.GetChild(1).GetComponent<Text>().text = "Dead Enemies:  " + GestoreNeutroTattica.numNemiciMorti;
	}

	// Token: 0x04001E5E RID: 7774
	private GameObject cameraCasa;

	// Token: 0x04001E5F RID: 7775
	private GameObject CentroStanzaUI;

	// Token: 0x04001E60 RID: 7776
	private GameObject settoriUI;

	// Token: 0x04001E61 RID: 7777
	private GameObject risorseAlleatiUI;

	// Token: 0x04001E62 RID: 7778
	private GameObject risorseNemiciUI;

	// Token: 0x04001E63 RID: 7779
	private GameObject Headquarters;

	// Token: 0x04001E64 RID: 7780
	private GameObject testoNumeroAlleati;

	// Token: 0x04001E65 RID: 7781
	private GameObject testoNumeroNemici;

	// Token: 0x04001E66 RID: 7782
	private GameObject schermataBattaglia;

	// Token: 0x04001E67 RID: 7783
	private GameObject elencoAlleatiSchermBatt;

	// Token: 0x04001E68 RID: 7784
	private GameObject elencoAlleatiSchermBattBarraVert;

	// Token: 0x04001E69 RID: 7785
	private GameObject elencoNemiciSchermBatt;

	// Token: 0x04001E6A RID: 7786
	private GameObject elencoNemiciSchermBattBarraVert;

	// Token: 0x04001E6B RID: 7787
	private GameObject elencoAlleatiSchermMiss;

	// Token: 0x04001E6C RID: 7788
	private GameObject elencoAlleatiSchermMissBarraVert;

	// Token: 0x04001E6D RID: 7789
	private GameObject Nest;

	// Token: 0x04001E6E RID: 7790
	private GameObject pulsanteDifendi;

	// Token: 0x04001E6F RID: 7791
	private GameObject pulsanteAttacca;

	// Token: 0x04001E70 RID: 7792
	private GameObject pulsanteCombatti;

	// Token: 0x04001E71 RID: 7793
	private GameObject pulsanteVittoriaATavolino;

	// Token: 0x04001E72 RID: 7794
	private GameObject pulsanteRitirata;

	// Token: 0x04001E73 RID: 7795
	private GameObject scherResocontoBatt;

	// Token: 0x04001E74 RID: 7796
	private GameObject elencoAlleatiResocontoBatt;

	// Token: 0x04001E75 RID: 7797
	private GameObject elencoAlleatiResocontoBattBarraVert;

	// Token: 0x04001E76 RID: 7798
	private GameObject elencoNemiciResocontoBatt;

	// Token: 0x04001E77 RID: 7799
	private GameObject elencoNemiciResocontoBattBarraVert;

	// Token: 0x04001E78 RID: 7800
	private GameObject ricompenseResocontoBatt;

	// Token: 0x04001E79 RID: 7801
	private GameObject immagineEsitoResBatt;

	// Token: 0x04001E7A RID: 7802
	private GameObject schermataMissione;

	// Token: 0x04001E7B RID: 7803
	private GameObject scherResocontoMissione;

	// Token: 0x04001E7C RID: 7804
	private GameObject immagineEsitoResMiss;

	// Token: 0x04001E7D RID: 7805
	private GameObject elencoAlleatiResocontoMissione;

	// Token: 0x04001E7E RID: 7806
	private GameObject elencoAlleatiResocontoMissioneBarraVert;

	// Token: 0x04001E7F RID: 7807
	private GameObject resocontoMissioneTesto;

	// Token: 0x04001E80 RID: 7808
	private GameObject resocontoMissionePremi;

	// Token: 0x04001E81 RID: 7809
	private GameObject pulsanteMissione;

	// Token: 0x04001E82 RID: 7810
	private GameObject scrittaSolo1Battaglia;

	// Token: 0x04001E83 RID: 7811
	private GameObject perditeInBatt;

	// Token: 0x04001E84 RID: 7812
	private GameObject perditeinMiss;

	// Token: 0x04001E85 RID: 7813
	public string nomeStanza;

	// Token: 0x04001E86 RID: 7814
	public Sprite immagineStanza;

	// Token: 0x04001E87 RID: 7815
	public string nomeScenaPerTattica;

	// Token: 0x04001E88 RID: 7816
	public int appartenenzaBandiera;

	// Token: 0x04001E89 RID: 7817
	public List<int> ListaSettori;

	// Token: 0x04001E8A RID: 7818
	public int settoriAlleati;

	// Token: 0x04001E8B RID: 7819
	public int settoriNemici;

	// Token: 0x04001E8C RID: 7820
	public float valoreGuadFreshFood;

	// Token: 0x04001E8D RID: 7821
	public float valoreGuadRottenFood;

	// Token: 0x04001E8E RID: 7822
	public float valoreGuadHighProteinFood;

	// Token: 0x04001E8F RID: 7823
	public float valoreGuadPlastica;

	// Token: 0x04001E90 RID: 7824
	public float valoreGuadMetallo;

	// Token: 0x04001E91 RID: 7825
	public float valoreGuadEnergia;

	// Token: 0x04001E92 RID: 7826
	public float valoreGuadIncendiario;

	// Token: 0x04001E93 RID: 7827
	public float valoreGuadTossico;

	// Token: 0x04001E94 RID: 7828
	public float guadagnoRealePlastica;

	// Token: 0x04001E95 RID: 7829
	public float guadagnoRealeMetallo;

	// Token: 0x04001E96 RID: 7830
	public float guadagnoRealeEnergia;

	// Token: 0x04001E97 RID: 7831
	public float guadagnoRealeMatIncendiario;

	// Token: 0x04001E98 RID: 7832
	public float guadagnoRealeMatTossico;

	// Token: 0x04001E99 RID: 7833
	public float guadagnoRealeFreshFood;

	// Token: 0x04001E9A RID: 7834
	public float guadagnoRealeRottenFood;

	// Token: 0x04001E9B RID: 7835
	public float guadagnoRealeHighProteinFood;

	// Token: 0x04001E9C RID: 7836
	private Transform corpoBandiera;

	// Token: 0x04001E9D RID: 7837
	public Material bandieraBianca;

	// Token: 0x04001E9E RID: 7838
	public Material bandieraVerde;

	// Token: 0x04001E9F RID: 7839
	public Material bandieraRossa;

	// Token: 0x04001EA0 RID: 7840
	private Material coloreBandiera;

	// Token: 0x04001EA1 RID: 7841
	public Color coloreSettoreVerde;

	// Token: 0x04001EA2 RID: 7842
	public Color coloreSettoreRosso;

	// Token: 0x04001EA3 RID: 7843
	public List<GameObject> ListaColliderStanza;

	// Token: 0x04001EA4 RID: 7844
	public int esercitiAlleatiPresenti;

	// Token: 0x04001EA5 RID: 7845
	public int esercitiNemiciPresenti;

	// Token: 0x04001EA6 RID: 7846
	public List<GameObject> ListaEserAlleatiPres;

	// Token: 0x04001EA7 RID: 7847
	public List<GameObject> ListaEserNemiciPres;

	// Token: 0x04001EA8 RID: 7848
	public int numTruppeAlleatePres;

	// Token: 0x04001EA9 RID: 7849
	public int numInsettiNemciPres;

	// Token: 0x04001EAA RID: 7850
	public List<List<int>> ListaAlleatiInSchermBatt;

	// Token: 0x04001EAB RID: 7851
	public List<List<int>> ListaNemiciInSchermBatt;

	// Token: 0x04001EAC RID: 7852
	public List<int> ListaAllPresentiInBatt;

	// Token: 0x04001EAD RID: 7853
	public List<int> ListaNemPresentiInBatt;

	// Token: 0x04001EAE RID: 7854
	public List<float> ListaDanniAlleati;

	// Token: 0x04001EAF RID: 7855
	public List<float> ListaDanniNemici;

	// Token: 0x04001EB0 RID: 7856
	public List<int> ListaAllSopravv;

	// Token: 0x04001EB1 RID: 7857
	public List<int> ListaNemSopravv;

	// Token: 0x04001EB2 RID: 7858
	public bool quiNemicoStaAttaccando;

	// Token: 0x04001EB3 RID: 7859
	public int numIdentSwarmSpecialeInAtt;

	// Token: 0x04001EB4 RID: 7860
	private bool pulsanteDifendiAttivo;

	// Token: 0x04001EB5 RID: 7861
	private bool pulsanteAttaccaAttivo;

	// Token: 0x04001EB6 RID: 7862
	private int postiInElencoBattaglia;

	// Token: 0x04001EB7 RID: 7863
	private int numUnitàInEserAlleato;

	// Token: 0x04001EB8 RID: 7864
	private int numAlleatiPossibili;

	// Token: 0x04001EB9 RID: 7865
	private int numNemiciPossibili;

	// Token: 0x04001EBA RID: 7866
	public int numNemiciAmmassati;

	// Token: 0x04001EBB RID: 7867
	public int quiCèStataBattaglia;

	// Token: 0x04001EBC RID: 7868
	public bool bloccoPerBatt;
}
