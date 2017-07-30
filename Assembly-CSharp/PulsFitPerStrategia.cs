using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x0200010E RID: 270
public class PulsFitPerStrategia : MonoBehaviour
{
	// Token: 0x060008A4 RID: 2212 RVA: 0x0012B3E8 File Offset: 0x001295E8
	private void Start()
	{
		this.Schede = GameObject.FindGameObjectWithTag("Schede").transform;
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x0012B400 File Offset: 0x00129600
	private void Update()
	{
		this.ControlloScheda();
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x0012B408 File Offset: 0x00129608
	private void ControlloScheda()
	{
		if (this.Schede.GetChild(6).name == "scheda 1")
		{
			this.schedaAperta = 0;
		}
		else if (this.Schede.GetChild(6).name == "scheda 2")
		{
			this.schedaAperta = 1;
		}
		else if (this.Schede.GetChild(6).name == "scheda 3")
		{
			this.schedaAperta = 2;
		}
		else if (this.Schede.GetChild(6).name == "scheda 4")
		{
			this.schedaAperta = 3;
		}
		else if (this.Schede.GetChild(6).name == "scheda 5")
		{
			this.schedaAperta = 4;
		}
		else if (this.Schede.GetChild(6).name == "scheda 6")
		{
			this.schedaAperta = 5;
		}
		else if (this.Schede.GetChild(6).name == "scheda 7")
		{
			this.schedaAperta = 6;
		}
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x0012B544 File Offset: 0x00129744
	public void SelezioneSchede(int numeroScheda)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickScheda;
		this.ListaSchede = new List<GameObject>();
		this.ListaSchede.Add(null);
		this.ListaSchede.Add(null);
		this.ListaSchede.Add(null);
		this.ListaSchede.Add(null);
		this.ListaSchede.Add(null);
		this.ListaSchede.Add(null);
		this.ListaSchede.Add(null);
		Transform transform = GameObject.FindGameObjectWithTag("Schede").transform;
		for (int i = 0; i < this.ListaSchede.Count; i++)
		{
			if (transform.GetChild(i).gameObject.name == "scheda 1")
			{
				this.ListaSchede[0] = transform.GetChild(i).gameObject;
			}
			else if (transform.GetChild(i).gameObject.name == "scheda 2")
			{
				this.ListaSchede[1] = transform.GetChild(i).gameObject;
			}
			else if (transform.GetChild(i).gameObject.name == "scheda 3")
			{
				this.ListaSchede[2] = transform.GetChild(i).gameObject;
			}
			else if (transform.GetChild(i).gameObject.name == "scheda 4")
			{
				this.ListaSchede[3] = transform.GetChild(i).gameObject;
			}
			else if (transform.GetChild(i).gameObject.name == "scheda 5")
			{
				this.ListaSchede[4] = transform.GetChild(i).gameObject;
			}
			else if (transform.GetChild(i).gameObject.name == "scheda 6")
			{
				this.ListaSchede[5] = transform.GetChild(i).gameObject;
			}
			else if (transform.GetChild(i).gameObject.name == "scheda 7")
			{
				this.ListaSchede[6] = transform.GetChild(i).gameObject;
			}
		}
		this.ListaSchede[numeroScheda].transform.SetSiblingIndex(6);
		if (numeroScheda == 2)
		{
			gameObject = GameObject.FindGameObjectWithTag("Headquarters");
			gameObject.GetComponent<GestioneEsercitiAlleati>().sblocchiAggiornati = true;
		}
		if (numeroScheda == 6)
		{
			gameObject = GameObject.FindGameObjectWithTag("Headquarters");
			gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().aggSupporto = true;
		}
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x0012B808 File Offset: 0x00129A08
	public void ApparizioneSchede(int schedaDaAprire)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Schede");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("Schede").transform.FindChild("scheda 1").FindChild("info risorsa").gameObject;
		GameObject gameObject4 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject4.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject4.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject4.GetComponent<GestioneSuoniCasa>().suonoClickScheda;
		if (gameObject.GetComponent<CanvasGroup>().alpha == 0f)
		{
			if (schedaDaAprire == 0)
			{
				bool flag = false;
				int num = 0;
				while (num < 6 && !flag)
				{
					if (gameObject.transform.GetChild(num).gameObject.name == "scheda 1")
					{
						flag = true;
						gameObject.transform.GetChild(num).transform.SetSiblingIndex(6);
					}
					num++;
				}
			}
			else if (schedaDaAprire == 1)
			{
				bool flag2 = false;
				int num2 = 0;
				while (num2 < 6 && !flag2)
				{
					if (gameObject.transform.GetChild(num2).gameObject.name == "scheda 2")
					{
						flag2 = true;
						gameObject.transform.GetChild(num2).transform.SetSiblingIndex(6);
					}
					num2++;
				}
			}
			gameObject.GetComponent<CanvasGroup>().alpha = 1f;
			gameObject.GetComponent<CanvasGroup>().interactable = true;
			gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
		else
		{
			gameObject.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject.GetComponent<CanvasGroup>().interactable = false;
			gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
			gameObject3.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject3.GetComponent<CanvasGroup>().interactable = false;
			gameObject3.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x0012B9EC File Offset: 0x00129BEC
	public void FineTurno()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("MainCamera");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<AudioSource>().clip = gameObject2.GetComponent<GestioneSuoniCasa>().fineTurno;
		gameObject.GetComponent<GestoreNeutroStrategia>().turnoNemicoAttivo = true;
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x0012BA40 File Offset: 0x00129C40
	public void ReclutaEsercito()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneEsercitiAlleati>().reclutamentoAttivo = true;
		gameObject.GetComponent<GestioneEsercitiAlleati>().resettaListaRecl = true;
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x0012BA98 File Offset: 0x00129C98
	public void ResettaReclutamentoEsercito()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneEsercitiAlleati>().resettaListaRecl = true;
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x0012BAE4 File Offset: 0x00129CE4
	public void CreaEsercito()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneEsercitiAlleati>().creaEsercitoAlleato = true;
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x0012BB30 File Offset: 0x00129D30
	public void SelezNonReclutato(int tipoTruppa)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneEsercitiAlleati>().tipoTruppaSelez = tipoTruppa;
		gameObject.GetComponent<GestioneEsercitiAlleati>().ListaTimerTipoTruppa[0] = Time.time;
		gameObject.GetComponent<GestioneEsercitiAlleati>().giàReclutato = false;
		gameObject.GetComponent<GestioneEsercitiAlleati>().aggiornaDettagliRecl = true;
		gameObject.GetComponent<GestioneEsercitiAlleati>().selezioneInGiàReclutati = false;
		gameObject.GetComponent<GestioneEsercitiAlleati>().selezVisibileRecl = false;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Schede").transform.FindChild("scheda 3").gameObject;
		GameObject gameObject3 = gameObject2.transform.FindChild("dettagli unità").transform.FindChild("aggiungi o togli").gameObject;
		gameObject3.transform.GetChild(0).GetComponent<Text>().text = "ADD UNIT";
		gameObject.GetComponent<GestioneEsercitiAlleati>().aggArmi = true;
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x0012BC2C File Offset: 0x00129E2C
	public void SelezGiàReclutato(int posizioneInLista)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneEsercitiAlleati>().posizioneInListaRecl = posizioneInLista;
		gameObject.GetComponent<GestioneEsercitiAlleati>().ListaTimerTipoTruppa[1] = Time.time;
		gameObject.GetComponent<GestioneEsercitiAlleati>().giàReclutato = true;
		gameObject.GetComponent<GestioneEsercitiAlleati>().aggiornaDettagliRecl = true;
		gameObject.GetComponent<GestioneEsercitiAlleati>().selezioneInGiàReclutati = true;
		gameObject.GetComponent<GestioneEsercitiAlleati>().selezVisibileRecl = true;
		gameObject.GetComponent<GestioneEsercitiAlleati>().aggiornaSelezReclVisibile = true;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Schede").transform.FindChild("scheda 3").gameObject;
		GameObject gameObject3 = gameObject2.transform.FindChild("dettagli unità").transform.FindChild("aggiungi o togli").gameObject;
		gameObject3.transform.GetChild(0).GetComponent<Text>().text = "REMOVE UNIT";
		gameObject.GetComponent<GestioneEsercitiAlleati>().aggArmi = true;
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x0012BD34 File Offset: 0x00129F34
	public void AggiungiOTogliRecluta()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneEsercitiAlleati>().aggiungiOTogliAttivo = true;
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x0012BD80 File Offset: 0x00129F80
	public void CambiaElenco(int tipoElenco)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneEsercitiAlleati>().tipoElenco = tipoElenco;
		gameObject.GetComponent<GestioneEsercitiAlleati>().sblocchiAggiornati = true;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Schede").transform.FindChild("scheda 3").FindChild("elenco per reclutare").gameObject;
		GameObject gameObject3 = gameObject2.transform.FindChild("elenco Fanteria").gameObject;
		GameObject gameObject4 = gameObject2.transform.FindChild("elenco Mezzi").gameObject;
		GameObject gameObject5 = gameObject2.transform.FindChild("elenco Artiglieria").gameObject;
		GameObject gameObject6 = gameObject2.transform.FindChild("elenco Aeronautica").gameObject;
		if (tipoElenco == 0)
		{
			gameObject3.GetComponent<CanvasGroup>().alpha = 1f;
			gameObject3.GetComponent<CanvasGroup>().interactable = true;
			gameObject3.GetComponent<CanvasGroup>().blocksRaycasts = true;
			gameObject4.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject4.GetComponent<CanvasGroup>().interactable = false;
			gameObject4.GetComponent<CanvasGroup>().blocksRaycasts = false;
			gameObject5.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject5.GetComponent<CanvasGroup>().interactable = false;
			gameObject5.GetComponent<CanvasGroup>().blocksRaycasts = false;
			gameObject6.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject6.GetComponent<CanvasGroup>().interactable = false;
			gameObject6.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		else if (tipoElenco == 1)
		{
			gameObject3.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject3.GetComponent<CanvasGroup>().interactable = false;
			gameObject3.GetComponent<CanvasGroup>().blocksRaycasts = false;
			gameObject4.GetComponent<CanvasGroup>().alpha = 1f;
			gameObject4.GetComponent<CanvasGroup>().interactable = true;
			gameObject4.GetComponent<CanvasGroup>().blocksRaycasts = true;
			gameObject5.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject5.GetComponent<CanvasGroup>().interactable = false;
			gameObject5.GetComponent<CanvasGroup>().blocksRaycasts = false;
			gameObject6.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject6.GetComponent<CanvasGroup>().interactable = false;
			gameObject6.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		else if (tipoElenco == 2)
		{
			gameObject3.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject3.GetComponent<CanvasGroup>().interactable = false;
			gameObject3.GetComponent<CanvasGroup>().blocksRaycasts = false;
			gameObject4.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject4.GetComponent<CanvasGroup>().interactable = false;
			gameObject4.GetComponent<CanvasGroup>().blocksRaycasts = false;
			gameObject5.GetComponent<CanvasGroup>().alpha = 1f;
			gameObject5.GetComponent<CanvasGroup>().interactable = true;
			gameObject5.GetComponent<CanvasGroup>().blocksRaycasts = true;
			gameObject6.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject6.GetComponent<CanvasGroup>().interactable = false;
			gameObject6.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		else if (tipoElenco == 3)
		{
			gameObject3.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject3.GetComponent<CanvasGroup>().interactable = false;
			gameObject3.GetComponent<CanvasGroup>().blocksRaycasts = false;
			gameObject4.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject4.GetComponent<CanvasGroup>().interactable = false;
			gameObject4.GetComponent<CanvasGroup>().blocksRaycasts = false;
			gameObject5.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject5.GetComponent<CanvasGroup>().interactable = false;
			gameObject5.GetComponent<CanvasGroup>().blocksRaycasts = false;
			gameObject6.GetComponent<CanvasGroup>().alpha = 1f;
			gameObject6.GetComponent<CanvasGroup>().interactable = true;
			gameObject6.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x0012C11C File Offset: 0x0012A31C
	public void InfoUnità(int posUnità)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Dettagli Veloci Unità").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		if (!gameObject2.GetComponent<GestioneEsercitiAlleati>().scambioFraEserciti)
		{
			gameObject.GetComponent<CanvasGroup>().alpha = 1f;
			gameObject.GetComponent<CanvasGroup>().interactable = true;
			gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
			gameObject2.GetComponent<GestioneEsercitiAlleati>().visualizzaDettagli = true;
			gameObject2.GetComponent<GestioneEsercitiAlleati>().aggiornaDettagliEser = true;
		}
		gameObject2.GetComponent<GestioneEsercitiAlleati>().numPosUnità = posUnità;
		gameObject2.GetComponent<GestioneEsercitiAlleati>().aggArmi = true;
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x0012C1E4 File Offset: 0x0012A3E4
	public void OrigineInfoUnità(int origine)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneEsercitiAlleati>().origineDeiDettagli = origine;
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x0012C230 File Offset: 0x0012A430
	public void ChiudiInfoUnità()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Dettagli Veloci Unità").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<GestioneEsercitiAlleati>().visualizzaDettagli = false;
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x0012C2C4 File Offset: 0x0012A4C4
	public void OrigineSelezioneUnità(int origine)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		if (origine == 2)
		{
			gameObject.GetComponent<GestioneEsercitiAlleati>().ListaTimerTipoTruppa[2] = Time.time;
		}
		else if (origine == 3)
		{
			gameObject.GetComponent<GestioneEsercitiAlleati>().ListaTimerTipoTruppa[3] = Time.time;
		}
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x0012C344 File Offset: 0x0012A544
	public void ChiudiVisualEser()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Visualizza Esercito").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Dettagli Veloci Unità").gameObject;
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject3.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject3.GetComponent<CanvasGroup>().interactable = false;
		gameObject3.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<GestioneEsercitiAlleati>().visualizzaEser = false;
		gameObject2.GetComponent<GestioneEsercitiAlleati>().scambioFraEserciti = false;
		gameObject2.GetComponent<GestioneEsercitiAlleati>().controlloEserVuoti = true;
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x0012C438 File Offset: 0x0012A638
	public void ApriDomandaCongeda()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Dettagli Veloci Unità").FindChild("Domanda congedo").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x0012C4C8 File Offset: 0x0012A6C8
	public void AccettaCongeda()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Dettagli Veloci Unità").FindChild("Domanda congedo").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<GestioneEsercitiAlleati>().congedaUnitàSel = true;
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x0012C564 File Offset: 0x0012A764
	public void AnnullaCongeda()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Dettagli Veloci Unità").FindChild("Domanda congedo").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x0012C5F4 File Offset: 0x0012A7F4
	public void ApriArmi()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Armi").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		gameObject2.GetComponent<GestioneEsercitiAlleati>().armiAperto = true;
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x0012C688 File Offset: 0x0012A888
	public void ChiudiArmi()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Armi").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<GestioneEsercitiAlleati>().armiAperto = false;
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x0012C71C File Offset: 0x0012A91C
	public void PulsanteColonnaArma(int numColonna)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneEsercitiAlleati>().pulsanteColonnaArma = numColonna;
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x0012C768 File Offset: 0x0012A968
	public void PulsanteRigaArma(int numRiga)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneEsercitiAlleati>().pulsanteRigaArma = numRiga;
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x0012C78C File Offset: 0x0012A98C
	public void SalvaArma(int numArma)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneEsercitiAlleati>().numColonnaArmaDaSalvare = numArma;
		gameObject.GetComponent<GestioneEsercitiAlleati>().salvaArmaSelez = true;
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x0012C7E4 File Offset: 0x0012A9E4
	public void ChiudiCentroStanza()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Centro Stanza").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x0012C86C File Offset: 0x0012AA6C
	public void DifendiInBattaglia()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Per Battaglia").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Centro Stanza").gameObject;
		GameObject gameObject4 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject4.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject4.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject4.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		gameObject3.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject3.GetComponent<CanvasGroup>().interactable = false;
		gameObject3.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		GestoreNeutroStrategia.aggElencoBattaglia = true;
		GestoreNeutroStrategia.attaccante = 1;
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x0012C958 File Offset: 0x0012AB58
	public void AttaccaInBattaglia()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Per Battaglia").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Centro Stanza").gameObject;
		GameObject gameObject4 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject4.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject4.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject4.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		gameObject3.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject3.GetComponent<CanvasGroup>().interactable = false;
		gameObject3.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		GestoreNeutroStrategia.aggElencoBattaglia = true;
		GestoreNeutroStrategia.attaccante = 0;
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x0012CA44 File Offset: 0x0012AC44
	public void ChiudiSchermataBattaglia()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Per Battaglia").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x0012CACC File Offset: 0x0012ACCC
	public void ChiudiVisualEserInsetti()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Visualizza Esercito Insetti").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Dettagli Veloci Insetto").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("Nest").gameObject;
		GameObject gameObject4 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject4.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject4.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject4.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject3.GetComponent<IANemicoStrategia>().visualizzaEser = false;
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x0012CBB8 File Offset: 0x0012ADB8
	public void InfoInsetto(int numInElenco)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Dettagli Veloci Insetto").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Nest").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject3.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject3.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject3.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		gameObject2.GetComponent<IANemicoStrategia>().numElencoPerVisualizInsetto = numInElenco;
		gameObject2.GetComponent<IANemicoStrategia>().aggiornaDettagliEser = true;
		gameObject2.GetComponent<IANemicoStrategia>().visualizzaDettagli = true;
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x0012CC74 File Offset: 0x0012AE74
	public void OrigineInfoInsetti(int origine)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Nest").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<IANemicoStrategia>().origineDeiDettagli = origine;
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x0012CCD0 File Offset: 0x0012AED0
	public void ChiudiInfoInsetti()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Dettagli Veloci Insetto").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Nest").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject3.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject3.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject3.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<IANemicoStrategia>().visualizzaDettagli = false;
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x0012CD74 File Offset: 0x0012AF74
	public void MenuContinua()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Menu Strategico").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Menu Tattico").gameObject;
		GameObject gameObject4 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject4.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject4.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject4.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		if (gameObject.GetComponent<OltreScene>().èInStrategia)
		{
			gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject2.GetComponent<CanvasGroup>().interactable = false;
			gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
			gameObject.GetComponent<OltreScene>().menuAperto = false;
		}
		else
		{
			gameObject3.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject3.GetComponent<CanvasGroup>().interactable = false;
			gameObject3.GetComponent<CanvasGroup>().blocksRaycasts = false;
			gameObject.GetComponent<OltreScene>().menuAperto = false;
		}
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x0012CE80 File Offset: 0x0012B080
	public void MenuApriSalva()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Salva").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x0012CF08 File Offset: 0x0012B108
	public void MenuChiudiSalva()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Salva").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x0012CF90 File Offset: 0x0012B190
	public void MenuApriCarica()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Carica In Strategia").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		CaricaDati.slotSuCuiCaricare = 0;
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x0012D01C File Offset: 0x0012B21C
	public void MenuChiudiCarica()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Carica In Strategia").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x0012D0A4 File Offset: 0x0012B2A4
	public void MenuIndiceSalva(int numSlot)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<SalvaDati>().slotSuCuiSalvare = numSlot;
		CaricaDati.slotSuCuiCaricare = numSlot;
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x0012D108 File Offset: 0x0012B308
	public void MenuIndiceCarica(int numSlot)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		CaricaDati.slotSuCuiCaricare = numSlot;
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x0012D160 File Offset: 0x0012B360
	public void MenuSalva()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Salva").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject3.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject3.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject3.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<SalvaDati>().salvataggioAttivo = true;
		gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x0012D204 File Offset: 0x0012B404
	public void MenuCarica()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Carica In Strategia").FindChild("domanda per caricamento").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject3.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject3.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject3.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		CaricaDati.caricamentoAttivo = true;
		gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x0012D2AC File Offset: 0x0012B4AC
	public void MenuApriSalvaConNome()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Salva").FindChild("domanda nome salvataggio").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x0012D33C File Offset: 0x0012B53C
	public void MenuChiudiSalvaConNome()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Salva").FindChild("domanda nome salvataggio").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x0012D3CC File Offset: 0x0012B5CC
	public void MenuSalvaConNome()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Salva").FindChild("domanda nome salvataggio").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject3.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject3.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject3.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<SalvaDati>().salvataggioAttivo = true;
		gameObject.GetComponent<SalvaDati>().èSalvaConNome = true;
		gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.transform.parent.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.transform.parent.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x0012D4CC File Offset: 0x0012B6CC
	public void MenuApriDomandaCaricamento()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Carica In Strategia").FindChild("domanda per caricamento").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x0012D55C File Offset: 0x0012B75C
	public void MenuChiudiDomandaCaricamento()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Carica In Strategia").FindChild("domanda per caricamento").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x0012D5EC File Offset: 0x0012B7EC
	public void MenuCancellaSalvataggio()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Cancella").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject3.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject3.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject3.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CaricaDati>().cancellaSalvataggioSelez = true;
		gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x0012D690 File Offset: 0x0012B890
	public void MenuApriDomandaCancellamento()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Cancella").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x0012D718 File Offset: 0x0012B918
	public void MenuChiudiDomandaCancellamento()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Finestra Cancella").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x0012D7A0 File Offset: 0x0012B9A0
	public void MenuApriChiudi()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Menu Strategico").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InizioLivello");
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject3.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject3.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject3.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		if (!gameObject2.GetComponent<OltreScene>().menuAperto)
		{
			gameObject.GetComponent<CanvasGroup>().alpha = 1f;
			gameObject.GetComponent<CanvasGroup>().interactable = true;
			gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
			gameObject2.GetComponent<OltreScene>().menuAperto = true;
		}
		else
		{
			gameObject.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject.GetComponent<CanvasGroup>().interactable = false;
			gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
			gameObject2.GetComponent<OltreScene>().menuAperto = false;
		}
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x0012D888 File Offset: 0x0012BA88
	public void MenuApriImpostazioni()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Impostazioni").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x0012D910 File Offset: 0x0012BB10
	public void CombattiBattagliaNormale()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject3.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject3.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject3.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<SalvaDati>().slotSuCuiSalvare = 9;
		CaricaDati.slotSuCuiCaricare = 9;
		gameObject.GetComponent<SalvaDati>().salvataggioAttivo = true;
		gameObject.GetComponent<SalvaDati>().salvataggioNascosto = true;
		gameObject2.GetComponent<GestoreNeutroStrategia>().missionePresente = 0;
		GestoreNeutroStrategia.inTattica = true;
		CaricaScene.nomeScenaDaCaricare = gameObject2.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().nomeScenaPerTattica;
		GestoreNeutroStrategia.indiceStanzaDiBattaglia = gameObject2.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.IndexOf(gameObject2.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata);
		CaricaScene.nomeScenaCasaPerRitornoAStrategia = SceneManager.GetActiveScene().name;
		gameObject2.GetComponent<GestoreNeutroStrategia>().battagliaATavolino = false;
		if (GestoreNeutroStrategia.attaccante == 0)
		{
			if (gameObject2.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().quiCèStataBattaglia == 2)
			{
				gameObject2.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().quiCèStataBattaglia = 3;
			}
			else
			{
				gameObject2.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().quiCèStataBattaglia = 1;
			}
		}
		else if (gameObject2.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().quiCèStataBattaglia == 1)
		{
			gameObject2.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().quiCèStataBattaglia = 3;
		}
		else
		{
			gameObject2.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().quiCèStataBattaglia = 2;
		}
		gameObject2.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().bloccoPerBatt = true;
		GestoreNeutroTattica.èBattagliaVeloce = false;
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x0012DAC4 File Offset: 0x0012BCC4
	public void VittoriaATavolino()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("MainCamera").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Per Battaglia").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Resoconto Battaglia").gameObject;
		GameObject gameObject4 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject4.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject4.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject4.GetComponent<GestioneSuoniCasa>().suonoVittoriaATavolino;
		GestoreNeutroStrategia.vincitore = 1;
		gameObject.GetComponent<GestoreNeutroStrategia>().battagliaATavolino = true;
		GestoreNeutroStrategia.mostraResocontoBattaglia = true;
		GestoreNeutroStrategia.mostraElencoResoconto = true;
		GestoreNeutroStrategia.indiceStanzaDiBattaglia = gameObject.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.IndexOf(gameObject.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata);
		GestoreNeutroStrategia.ripristinaBarraVert = true;
		gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject3.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject3.GetComponent<CanvasGroup>().interactable = true;
		gameObject3.GetComponent<CanvasGroup>().blocksRaycasts = true;
		if (gameObject.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().quiCèStataBattaglia == 2)
		{
			gameObject.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().quiCèStataBattaglia = 3;
		}
		else
		{
			gameObject.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().quiCèStataBattaglia = 1;
		}
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x0012DC34 File Offset: 0x0012BE34
	public void Ritirata()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("MainCamera").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Per Battaglia").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Resoconto Battaglia").gameObject;
		GameObject gameObject4 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject4.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject4.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject4.GetComponent<GestioneSuoniCasa>().suonoRitirata;
		GestoreNeutroStrategia.vincitore = 2;
		gameObject.GetComponent<GestoreNeutroStrategia>().battagliaATavolino = true;
		GestoreNeutroStrategia.mostraResocontoBattaglia = true;
		GestoreNeutroStrategia.mostraElencoResoconto = true;
		GestoreNeutroStrategia.indiceStanzaDiBattaglia = gameObject.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.IndexOf(gameObject.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata);
		GestoreNeutroStrategia.ripristinaBarraVert = true;
		gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject3.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject3.GetComponent<CanvasGroup>().interactable = true;
		gameObject3.GetComponent<CanvasGroup>().blocksRaycasts = true;
		if (gameObject.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().quiCèStataBattaglia == 1)
		{
			gameObject.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().quiCèStataBattaglia = 3;
		}
		else
		{
			gameObject.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().quiCèStataBattaglia = 2;
		}
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x0012DDA4 File Offset: 0x0012BFA4
	public void ApriGestioneHeadquarters()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Schede");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("MainCamera");
		GameObject gameObject4 = GameObject.FindGameObjectWithTag("VarieMappaStrategica");
		GameObject gameObject5 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Gestione Headquarters").FindChild("pulsante esci da gestione").gameObject;
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<BoxCollider>().enabled = false;
		gameObject3.GetComponent<CameraCasa>().posPrimaDiGestione = gameObject3.transform.position;
		gameObject3.GetComponent<CameraCasa>().rotPrimaDiGestione = gameObject3.transform.eulerAngles;
		gameObject3.transform.position = gameObject4.GetComponent<VarieMappaStrategicaCasa>().posCameraInGestHeadquarters;
		gameObject3.transform.rotation = Quaternion.Euler(gameObject4.GetComponent<VarieMappaStrategicaCasa>().rotCameraInGestHeadquarters);
		gameObject3.GetComponent<CameraCasa>().cameraèFerma = true;
		gameObject5.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject5.GetComponent<CanvasGroup>().interactable = true;
		gameObject5.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x0012DEF8 File Offset: 0x0012C0F8
	public void ChiudiGestioneHeadquarters()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Gestione Headquarters").FindChild("pannello costruzioni").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Gestione Headquarters").FindChild("info costruzioni").gameObject;
		GameObject gameObject4 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Gestione Headquarters").FindChild("pulsante esci da gestione").gameObject;
		GameObject gameObject5 = GameObject.FindGameObjectWithTag("MainCamera");
		GameObject gameObject6 = gameObject.transform.FindChild("lista posti").gameObject;
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<BoxCollider>().enabled = true;
		gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject3.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject3.GetComponent<CanvasGroup>().interactable = false;
		gameObject3.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject4.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject4.GetComponent<CanvasGroup>().interactable = false;
		gameObject4.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject5.transform.position = gameObject5.GetComponent<CameraCasa>().posPrimaDiGestione;
		gameObject5.transform.rotation = Quaternion.Euler(gameObject5.GetComponent<CameraCasa>().rotPrimaDiGestione);
		gameObject5.GetComponent<CameraCasa>().cameraèFerma = false;
		for (int i = 0; i < gameObject.GetComponent<GestioneRisorseEHeadquartiers>().ListaPostiInHeadquarters.Count; i++)
		{
			gameObject6.transform.GetChild(i).GetComponent<MeshRenderer>().material = gameObject.GetComponent<GestioneRisorseEHeadquartiers>().colorePostoEdificioNormale;
		}
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x0012E0E4 File Offset: 0x0012C2E4
	public void ChiudiListaEdifici()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Gestione Headquarters").FindChild("pannello costruzioni").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x0012E174 File Offset: 0x0012C374
	public void ChiudiInfoEdifici()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Gestione Headquarters").FindChild("info costruzioni").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x0012E204 File Offset: 0x0012C404
	public void SelezionaTipoEdificio(int tipoEdificio)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Gestione Headquarters").FindChild("info costruzioni").gameObject;
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneRisorseEHeadquartiers>().tipologiaEdificio = tipoEdificio;
		gameObject2.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject2.GetComponent<CanvasGroup>().interactable = true;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x0012E2A0 File Offset: 0x0012C4A0
	public void CostruisciEdificio()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Gestione Headquarters").FindChild("info costruzioni").FindChild("pulsante costruisci").gameObject;
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneRisorseEHeadquartiers>().avviaCostruzione = true;
		gameObject.GetComponent<GestioneRisorseEHeadquartiers>().aggiornaLavoroEdifici = true;
		gameObject2.GetComponent<Button>().interactable = false;
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x0012E338 File Offset: 0x0012C538
	public void DemolisciEdificio()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Gestione Headquarters").FindChild("info costruzioni").gameObject;
		GameObject gameObject3 = gameObject2.transform.FindChild("pulsante demolisci").gameObject;
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneRisorseEHeadquartiers>().avviaDemolizione = true;
		gameObject.GetComponent<GestioneRisorseEHeadquartiers>().aggiornaLavoroEdifici = true;
		gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject3.GetComponent<Button>().interactable = false;
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x0012E404 File Offset: 0x0012C604
	public void EdificioAccesoSpento()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoInterruttoreEdificio;
		gameObject.GetComponent<GestioneRisorseEHeadquartiers>().cambiaAccesoSpento = true;
		gameObject.GetComponent<GestioneRisorseEHeadquartiers>().aggiornaLavoroEdifici = true;
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x0012E45C File Offset: 0x0012C65C
	public void ApriDettagliRisorse(int numRisorsa)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Schede").transform.FindChild("scheda 1").FindChild("info risorsa").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		gameObject2.GetComponent<GestioneRisorseEHeadquartiers>().tipoRisorsa = numRisorsa;
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x0012E4F8 File Offset: 0x0012C6F8
	public void ChiudiDettagliRisorse()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Schede").transform.FindChild("scheda 1").FindChild("info risorsa").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x0012E588 File Offset: 0x0012C788
	public void ApriDomandaSatellite()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Centro Stanza").FindChild("domanda lanciare o abbattere satellite").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		gameObject2.GetComponent<GestioneRisorseEHeadquartiers>().domandaAperta = true;
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x0012E624 File Offset: 0x0012C824
	public void ChiudiDomandaSatellite()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Centro Stanza").FindChild("domanda lanciare o abbattere satellite").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<GestioneRisorseEHeadquartiers>().domandaAperta = false;
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x0012E6C0 File Offset: 0x0012C8C0
	public void LanciaOAbbattiSatellite()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Centro Stanza").FindChild("domanda lanciare o abbattere satellite").gameObject;
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneRisorseEHeadquartiers>().creaOAbbattiSatellite = true;
		gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x0012E75C File Offset: 0x0012C95C
	public void ChiudiResocontoBattaglia()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Resoconto Battaglia").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InizioLivello");
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("MainCamera");
		GameObject gameObject4 = GameObject.FindGameObjectWithTag("Headquarters");
		GameObject gameObject5 = GameObject.FindGameObjectWithTag("Nest");
		gameObject4.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject4.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject4.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		GestoreNeutroStrategia.mostraResocontoBattaglia = false;
		gameObject3.GetComponent<GestoreNeutroStrategia>().ricompenseDecise = false;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject3.GetComponent<GestoreNeutroStrategia>().ricompenseScelte = false;
		gameObject3.GetComponent<GestoreNeutroStrategia>().assegnaRicompense = true;
		gameObject3.GetComponent<GestoreNeutroStrategia>().premiAssegnati = 0;
		gameObject3.GetComponent<GestoreNeutroStrategia>().ricompenseDecise = false;
		for (int i = 0; i < gameObject5.GetComponent<IANemicoStrategia>().ListaEsercitiNemici.Count; i++)
		{
			if (gameObject5.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[i].GetComponent<PresenzaNemicaStrategica>().numIdentitàNemico == gameObject3.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().numIdentSwarmSpecialeInAtt)
			{
				gameObject5.GetComponent<IANemicoStrategia>().ListaEsercitiNemici[i].GetComponent<PresenzaNemicaStrategica>().swarmSpecialeHaAttaccato = 1;
				break;
			}
		}
		gameObject3.GetComponent<SelezionamentoInStrategia>().bandieraSelezionata.GetComponent<CentroStanza>().numIdentSwarmSpecialeInAtt = 0;
		gameObject4.GetComponent<GestioneEsercitiAlleati>().controlloEserVuoti = true;
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x0012E8E8 File Offset: 0x0012CAE8
	public void ApriSchermataMissione()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Per Missione").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Centro Stanza").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("Headquarters");
		GameObject gameObject4 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject3.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject3.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject3.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		GestoreNeutroStrategia.attaccante = 0;
		GestoreNeutroStrategia.aggElencoMissione = true;
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x0012E9D4 File Offset: 0x0012CBD4
	public void ChiudiSchermataMissione()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Per Missione").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x0012EA5C File Offset: 0x0012CC5C
	public void RifiutoMissione()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("MainCamera").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Per Missione").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Resoconto Missione").gameObject;
		GameObject gameObject4 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject4.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject4.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject4.GetComponent<GestioneSuoniCasa>().suonoRifiutoMissione;
		GestoreNeutroStrategia.vincitore = 2;
		gameObject.GetComponent<GestoreNeutroStrategia>().battagliaATavolino = true;
		GestoreNeutroStrategia.mostraResocontoMissione = true;
		GestoreNeutroStrategia.mostraElencoResoconto = true;
		gameObject.GetComponent<GestoreNeutroStrategia>().missionePresente = 0;
		gameObject.GetComponent<GestoreNeutroStrategia>().partenzaTimerAggMissioneExtra = true;
		GestoreNeutroStrategia.ripristinaBarraVert = true;
		gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject3.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject3.GetComponent<CanvasGroup>().interactable = true;
		gameObject3.GetComponent<CanvasGroup>().blocksRaycasts = true;
		if (GestoreNeutroStrategia.tipoBattaglia == 7)
		{
			GestoreNeutroStrategia.convogliArrivati = 5;
		}
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x0012EB88 File Offset: 0x0012CD88
	public void ChiudiResocontoMissione()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Resoconto Missione").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InizioLivello");
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("MainCamera");
		GameObject gameObject4 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject4.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject4.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject4.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		GestoreNeutroStrategia.mostraResocontoMissione = false;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject3.GetComponent<GestoreNeutroStrategia>().ricompenseScelte = false;
		gameObject3.GetComponent<GestoreNeutroStrategia>().assegnaRicompense = true;
		gameObject3.GetComponent<GestoreNeutroStrategia>().premiAssegnati = 0;
		gameObject3.GetComponent<GestoreNeutroStrategia>().ricompenseDecise = false;
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x0012EC5C File Offset: 0x0012CE5C
	public void SelezMunizPerComprare(int numPulsante)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().aggiornaMunizioniPres = true;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().numMunizSelez = numPulsante;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().munizioniInSospeso = 0f;
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x0012ECC4 File Offset: 0x0012CEC4
	public void AggiungiMunizioni(float quantità)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().aggiornaMunizioniPres = true;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().aggiungiMuniz = true;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().quantitàMunizDaAggiungere = quantità;
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x0012ED28 File Offset: 0x0012CF28
	public void ResettaMunizioni()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().aggiornaMunizioniPres = true;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().munizioniInSospeso = 0f;
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x0012ED84 File Offset: 0x0012CF84
	public void CompraMunizioni()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().aggiornaMunizioniPres = true;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().compra = true;
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x0012EDDC File Offset: 0x0012CFDC
	public void SelezCategoriaSblocco(int tipoCategoria)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneSblocchi>().aggTuttiGliSblocchi = true;
		gameObject.GetComponent<GestioneSblocchi>().categoriaDiSblocchi = tipoCategoria;
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x0012EE34 File Offset: 0x0012D034
	public void SbloccoSelezionato(int numeroSblocco)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneSblocchi>().aggInfoSblocco = true;
		gameObject.GetComponent<GestioneSblocchi>().numeroSbloccoSel = numeroSblocco;
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x0012EE8C File Offset: 0x0012D08C
	public void SbloccaSblocco()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneSblocchi>().eseguiSblocco = true;
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x0012EED8 File Offset: 0x0012D0D8
	public void MercatoSelezRisorsa(int tipoRisorsa)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().aggTuttiElenchiRisorse = true;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().tipoRisorsaSelez = tipoRisorsa;
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x0012EF30 File Offset: 0x0012D130
	public void MercatoVendi()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().aggTuttiElenchiRisorse = true;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().vendiPremuto = true;
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x0012EF88 File Offset: 0x0012D188
	public void MercatoCompra()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().aggTuttiElenchiRisorse = true;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().compraPremuto = true;
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x0012EFE0 File Offset: 0x0012D1E0
	public void MercatoResetta()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().aggTuttiElenchiRisorse = true;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().resettaPremuto = true;
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x0012F038 File Offset: 0x0012D238
	public void MercatoAffareConcluso()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().aggTuttiElenchiRisorse = true;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().eseguiScambioPremuto = true;
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x0012F090 File Offset: 0x0012D290
	public void CompraSupporto(int numSupporto)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().aggSupporto = true;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().supportoComprato = true;
		gameObject.GetComponent<GestioneMunizioniMercatoESupporto>().numSupporto = numSupporto;
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x0012F0F4 File Offset: 0x0012D2F4
	public void ImpostazioniScelta(int numero)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		if (gameObject.GetComponent<OltreScene>().èInStrategia)
		{
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
			gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
			gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		}
		else
		{
			GameObject gameObject3 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
			GameObject gameObject4 = gameObject3.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
			gameObject4.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
			gameObject4.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
			gameObject4.GetComponent<AudioSource>().clip = gameObject4.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		}
		gameObject.GetComponent<ImpostazioniScript>().impostSelez = numero;
		gameObject.GetComponent<ImpostazioniScript>().impostGiàScelta = true;
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x0012F1BC File Offset: 0x0012D3BC
	public void ImpostazioniApplica()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		if (gameObject.GetComponent<OltreScene>().èInStrategia)
		{
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
			gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
			gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		}
		else
		{
			GameObject gameObject3 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
			GameObject gameObject4 = gameObject3.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
			gameObject4.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
			gameObject4.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
			gameObject4.GetComponent<AudioSource>().clip = gameObject4.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		}
		gameObject.GetComponent<ImpostazioniScript>().impApplicate = true;
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x0012F278 File Offset: 0x0012D478
	public void ImpostazioniChiudi()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Impostazioni").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		if (gameObject2.GetComponent<OltreScene>().èInStrategia)
		{
			GameObject gameObject3 = GameObject.FindGameObjectWithTag("Headquarters");
			gameObject3.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
			gameObject3.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject3.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		}
		else
		{
			GameObject gameObject4 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
			GameObject gameObject5 = gameObject4.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
			gameObject5.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
			gameObject5.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
			gameObject5.GetComponent<AudioSource>().clip = gameObject5.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
		}
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<ImpostazioniScript>().impostSelez = 0;
		gameObject2.GetComponent<ImpostazioniScript>().impostGiàScelta = true;
		gameObject2.GetComponent<ImpostazioniScript>().impChiuse = true;
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0012F398 File Offset: 0x0012D598
	public void ScambioEserDiTruppaSel(int numero)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneEsercitiAlleati>().eserDellaTruppaSel = numero;
		gameObject.GetComponent<GestioneEsercitiAlleati>().aggScambioEserciti = true;
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x0012F3C8 File Offset: 0x0012D5C8
	public void ScambioEsercitiEffettuato()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestioneEsercitiAlleati>().effettuaScambio = true;
		gameObject2.GetComponent<SelezionamentoInStrategia>().esercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().puòAncoraMuoversi = false;
		gameObject2.GetComponent<SelezionamentoInStrategia>().secondoEsercitoSelezionato.GetComponent<PresenzaAlleataStrategica>().puòAncoraMuoversi = false;
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x0012F44C File Offset: 0x0012D64C
	public void RinominaApri(int tipoEserDaRinominare)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		GameObject gameObject2 = null;
		gameObject.GetComponent<GestioneEsercitiAlleati>().tipoEserDaRinominare = tipoEserDaRinominare;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		if (tipoEserDaRinominare == 0)
		{
			gameObject2 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Visualizza Esercito").FindChild("elenco esercito").FindChild("sfondo rinomina").gameObject;
		}
		else if (tipoEserDaRinominare == 1)
		{
			gameObject2 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Visualizza Esercito").FindChild("elenco secondo esercito").FindChild("sfondo rinomina").gameObject;
		}
		else if (tipoEserDaRinominare == 2)
		{
			gameObject2 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Visualizza Esercito Insetti").FindChild("elenco esercito nemico").FindChild("sfondo rinomina").gameObject;
		}
		gameObject2.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject2.GetComponent<CanvasGroup>().interactable = true;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = true;
		gameObject3.GetComponent<CameraCasa>().cameraèFerma = true;
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x0012F590 File Offset: 0x0012D790
	public void RinominaEffettuata(int tipoEserRinominato)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Nest");
		GameObject gameObject2 = null;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("MainCamera");
		GameObject gameObject4 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject4.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject4.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject4.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		if (tipoEserRinominato == 0)
		{
			gameObject2 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Visualizza Esercito").FindChild("elenco esercito").FindChild("sfondo rinomina").gameObject;
			gameObject4.GetComponent<GestioneEsercitiAlleati>().rinominaEser = true;
			gameObject4.GetComponent<GestioneEsercitiAlleati>().aggiornaEser = true;
		}
		else if (tipoEserRinominato == 1)
		{
			gameObject2 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Visualizza Esercito").FindChild("elenco secondo esercito").FindChild("sfondo rinomina").gameObject;
			gameObject4.GetComponent<GestioneEsercitiAlleati>().rinominaEser = true;
			gameObject4.GetComponent<GestioneEsercitiAlleati>().aggScambioEserciti = true;
		}
		else if (tipoEserRinominato == 2)
		{
			gameObject2 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Visualizza Esercito Insetti").FindChild("elenco esercito nemico").FindChild("sfondo rinomina").gameObject;
			gameObject4.GetComponent<GestioneEsercitiAlleati>().rinominaEser = true;
			gameObject.GetComponent<IANemicoStrategia>().aggiornaEser = true;
		}
		gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject3.GetComponent<CameraCasa>().cameraèFerma = false;
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x0012F71C File Offset: 0x0012D91C
	public void RinominaChiudi(int tipoEserDaRinominare)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("MainCamera");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		GameObject gameObject3 = null;
		if (tipoEserDaRinominare == 0)
		{
			gameObject3 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Visualizza Esercito").FindChild("elenco esercito").FindChild("sfondo rinomina").gameObject;
		}
		else if (tipoEserDaRinominare == 1)
		{
			gameObject3 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Visualizza Esercito").FindChild("elenco secondo esercito").FindChild("sfondo rinomina").gameObject;
		}
		else if (tipoEserDaRinominare == 2)
		{
			gameObject3 = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Visualizza Esercito Insetti").FindChild("elenco esercito nemico").FindChild("sfondo rinomina").gameObject;
		}
		gameObject3.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject3.GetComponent<CanvasGroup>().interactable = false;
		gameObject3.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject.GetComponent<CameraCasa>().cameraèFerma = false;
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x0012F854 File Offset: 0x0012DA54
	public void ChiudiMessInizioCampagna()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermate Inizio Campagna").FindChild("prima schermata").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x0012F8E4 File Offset: 0x0012DAE4
	public void ApriChiudiTornaAMenuIniz()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Domande Uscita Strategia").FindChild("domanda torna al menu").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		if (gameObject.GetComponent<CanvasGroup>().alpha == 0f)
		{
			gameObject.GetComponent<CanvasGroup>().alpha = 1f;
			gameObject.GetComponent<CanvasGroup>().interactable = true;
			gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
			gameObject.transform.parent.GetComponent<CanvasGroup>().alpha = 1f;
			gameObject.transform.parent.GetComponent<CanvasGroup>().interactable = true;
			gameObject.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
		else
		{
			gameObject.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject.GetComponent<CanvasGroup>().interactable = false;
			gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
			gameObject.transform.parent.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject.transform.parent.GetComponent<CanvasGroup>().interactable = false;
			gameObject.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x0012FA44 File Offset: 0x0012DC44
	public void ApriChiudiTornaADesktop()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Domande Uscita Strategia").FindChild("domanda torna al desktop").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		if (gameObject.GetComponent<CanvasGroup>().alpha == 0f)
		{
			gameObject.GetComponent<CanvasGroup>().alpha = 1f;
			gameObject.GetComponent<CanvasGroup>().interactable = true;
			gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
			gameObject.transform.parent.GetComponent<CanvasGroup>().alpha = 1f;
			gameObject.transform.parent.GetComponent<CanvasGroup>().interactable = true;
			gameObject.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
		else
		{
			gameObject.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject.GetComponent<CanvasGroup>().interactable = false;
			gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
			gameObject.transform.parent.GetComponent<CanvasGroup>().alpha = 0f;
			gameObject.transform.parent.GetComponent<CanvasGroup>().interactable = false;
			gameObject.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x0012FBA4 File Offset: 0x0012DDA4
	public void NuovaStagioneCampagna()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("MainCamera");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		gameObject.GetComponent<GestoreNeutroStrategia>().continuaStagioneCampagna = true;
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x0012FBFC File Offset: 0x0012DDFC
	public void EsciDaGioco()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
		Application.Quit();
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x0012FC40 File Offset: 0x0012DE40
	public void ChiudiConsigliIniziali()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermate Inizio Campagna").FindChild("schermata consigli").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Headquarters");
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
		gameObject2.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject2.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
	}

	// Token: 0x04001FD3 RID: 8147
	private Transform Schede;

	// Token: 0x04001FD4 RID: 8148
	private GameObject scheda1;

	// Token: 0x04001FD5 RID: 8149
	private GameObject scheda2;

	// Token: 0x04001FD6 RID: 8150
	private GameObject scheda3;

	// Token: 0x04001FD7 RID: 8151
	private GameObject scheda4;

	// Token: 0x04001FD8 RID: 8152
	private GameObject scheda5;

	// Token: 0x04001FD9 RID: 8153
	public List<GameObject> ListaSchede;

	// Token: 0x04001FDA RID: 8154
	public int schedaAperta;
}
