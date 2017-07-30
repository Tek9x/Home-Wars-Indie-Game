using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x0200010F RID: 271
public class PulsFitVarie : MonoBehaviour
{
	// Token: 0x0600090A RID: 2314 RVA: 0x0012FCD8 File Offset: 0x0012DED8
	public void MenuInizCreaCampagna()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
		SceneManager.LoadScene("Creazione Campagna");
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x0012FD08 File Offset: 0x0012DF08
	public void MenuInizApriCaricamento()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo caricamento").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo impostazioni").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject3.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
		CaricaDati.slotSuCuiCaricare = 0;
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0012FDC0 File Offset: 0x0012DFC0
	public void MenuInizBattagliVeloce()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
		SceneManager.LoadScene("Battaglia Veloce");
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x0012FDF0 File Offset: 0x0012DFF0
	public void MenuInizApriOpzioni()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo caricamento").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo impostazioni").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject2.GetComponent<CanvasGroup>().interactable = true;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = true;
		gameObject3.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x0012FEA4 File Offset: 0x0012E0A4
	public void MenuInizIndiceCarica(int numSlot)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
		CaricaDati.slotSuCuiCaricare = numSlot;
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x0012FEE0 File Offset: 0x0012E0E0
	public void MenuInizCarica()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
		CaricaDati.caricamentoAttivo = true;
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x0012FF0C File Offset: 0x0012E10C
	public void MenuInizCancellaSalvataggio()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo caricamento").FindChild("Finestra Cancella").gameObject;
		gameObject.GetComponent<CaricaDati>().cancellaSalvataggioSelez = true;
		gameObject3.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject3.GetComponent<CanvasGroup>().interactable = false;
		gameObject3.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x0012FFA0 File Offset: 0x0012E1A0
	public void MenuInizCancellaTutto()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo caricamento").FindChild("Finestra Cancella Tutto").gameObject;
		PlayerPrefs.DeleteAll();
		gameObject3.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject3.GetComponent<CanvasGroup>().interactable = false;
		gameObject3.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
		PlayerPrefs.SetInt("risoluzione schermo", 6);
		Screen.SetResolution(1920, 1080, true);
		PlayerPrefs.SetInt("in finestra", 0);
		Screen.fullScreen = true;
		PlayerPrefs.SetInt("qualità grafica", 4);
		QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualità grafica"), true);
		AudioListener.volume = 0.5f;
		PlayerPrefs.SetFloat("volume globale", 0.5f);
		PlayerPrefs.SetFloat("volume musica strategia", 0.5f);
		PlayerPrefs.SetFloat("volume musica tattica", 0.2f);
		PlayerPrefs.SetInt("max nemici", 400);
		PlayerPrefs.SetInt("max alleati", 60);
		PlayerPrefs.SetFloat("vita nemici", 100f);
		PlayerPrefs.SetFloat("attacco nemici", 100f);
		PlayerPrefs.SetInt("tipo truppa volante 1 0", 3);
		PlayerPrefs.SetInt("tipo truppa volante 1 1", 4);
		PlayerPrefs.SetInt("tipo truppa volante 2 0", 5);
		PlayerPrefs.SetInt("tipo truppa volante 2 1", 6);
		PlayerPrefs.SetInt("tipo truppa volante 2 2", 5);
		PlayerPrefs.SetInt("tipo truppa volante 3 0", 8);
		PlayerPrefs.SetInt("tipo truppa volante 3 1", 7);
		PlayerPrefs.SetInt("tipo truppa volante 3 2", 9);
		PlayerPrefs.SetInt("tipo truppa volante 4 0", 11);
		PlayerPrefs.SetInt("tipo truppa volante 4 1", 12);
		PlayerPrefs.SetInt("tipo truppa volante 4 2", 38);
		PlayerPrefs.SetInt("tipo truppa volante 5 0", 15);
		PlayerPrefs.SetInt("tipo truppa volante 5 1", 16);
		PlayerPrefs.SetInt("tipo truppa volante 5 2", 45);
		PlayerPrefs.SetInt("tipo truppa volante 5 3", 51);
		PlayerPrefs.SetInt("tipo truppa volante 6 0", 1);
		PlayerPrefs.SetInt("tipo truppa volante 7 0", 23);
		PlayerPrefs.SetInt("tipo truppa volante 7 1", 24);
		PlayerPrefs.SetInt("tipo truppa volante 11 0", 8);
		PlayerPrefs.SetInt("tipo truppa volante 11 1", 9);
		PlayerPrefs.SetInt("tipo truppa volante 12 0", 5);
		PlayerPrefs.SetInt("tipo truppa volante 12 1", 6);
		PlayerPrefs.SetInt("tipo truppa volante 13 0", 30);
		PlayerPrefs.SetInt("tipo truppa volante 13 1", 31);
		PlayerPrefs.SetInt("tipo truppa volante 13 2", 39);
		PlayerPrefs.SetInt("tipo truppa volante 13 3", 47);
		PlayerPrefs.SetInt("tipo truppa terr con ordigno 1 0", 25);
		PlayerPrefs.SetInt("tipo truppa terr con ordigno 1 1", 27);
		PlayerPrefs.SetInt("tipo truppa terr con ordigno 2 0", 29);
		PlayerPrefs.SetInt("tipo truppa terr con ordigno 2 1", 29);
		PlayerPrefs.SetInt("tipo truppa terr con ordigno 3 0", 34);
		PlayerPrefs.SetInt("tipo truppa terr con ordigno 3 1", 35);
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x00130258 File Offset: 0x0012E458
	public void MenuInizChiudiCaricamento()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo caricamento").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x001302C4 File Offset: 0x0012E4C4
	public void MenuApriDomandaCancellamento()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo caricamento").FindChild("Finestra Cancella").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x0013033C File Offset: 0x0012E53C
	public void MenuChiudiDomandaCancellamento()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo caricamento").FindChild("Finestra Cancella").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x001303B4 File Offset: 0x0012E5B4
	public void MenuApriDomandaCancellamentoTotale()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo caricamento").FindChild("Finestra Cancella Tutto").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x0013042C File Offset: 0x0012E62C
	public void MenuChiudiDomandaCancellamentoTotale()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo caricamento").FindChild("Finestra Cancella Tutto").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x001304A4 File Offset: 0x0012E6A4
	public void ImpostazioniScelta(int numero)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<ImpostazioniScript>().impostSelez = numero;
		gameObject.GetComponent<ImpostazioniScript>().impostGiàScelta = true;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x001304F0 File Offset: 0x0012E6F0
	public void ImpostazioniApplica()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<ImpostazioniScript>().impApplicate = true;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x00130530 File Offset: 0x0012E730
	public void ImpostazioniChiudi()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo impostazioni").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<ImpostazioniScript>().impostSelez = 0;
		gameObject2.GetComponent<ImpostazioniScript>().impostGiàScelta = true;
		gameObject2.GetComponent<ImpostazioniScript>().impChiuse = true;
		gameObject3.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x001305D0 File Offset: 0x0012E7D0
	public void ImpostazioniInFinestra()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo impostazioni").FindChild("Impostazioni").FindChild("secondo sfondo impostazioni").FindChild("Impostazioni Video").FindChild("imp finestra").GetChild(1).GetChild(0).gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<Image>().enabled = !gameObject.GetComponent<Image>().enabled;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x00130660 File Offset: 0x0012E860
	public void ImpostazioniVoloInverso()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello");
		if (!gameObject.GetComponent<OltreScene>().scenaDiMenu)
		{
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Impostazioni").FindChild("sfondo impostazioni").FindChild("Impostazioni Gioco").GetChild(1).GetChild(0).GetChild(0).FindChild("imp per volanti").GetChild(1).GetChild(1).GetChild(0).gameObject;
			gameObject2.GetComponent<Image>().enabled = !gameObject2.GetComponent<Image>().enabled;
			if (!gameObject.GetComponent<OltreScene>().èInStrategia)
			{
				GameObject gameObject3 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
				GameObject gameObject4 = gameObject3.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
				gameObject4.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
				gameObject4.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
				gameObject4.GetComponent<AudioSource>().clip = gameObject4.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
			}
			else
			{
				GameObject gameObject5 = GameObject.FindGameObjectWithTag("Headquarters");
				gameObject5.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
				gameObject5.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject5.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
			}
		}
		else
		{
			GameObject gameObject6 = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo impostazioni").FindChild("Impostazioni").FindChild("secondo sfondo impostazioni").FindChild("Impostazioni Gioco").GetChild(1).GetChild(0).GetChild(0).FindChild("imp per volanti").GetChild(1).GetChild(1).GetChild(0).gameObject;
			GameObject gameObject7 = GameObject.FindGameObjectWithTag("MainCamera");
			gameObject6.GetComponent<Image>().enabled = !gameObject6.GetComponent<Image>().enabled;
			gameObject7.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
		}
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x00130840 File Offset: 0x0012EA40
	public void ImpostazioniOrdAereiAria()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello");
		if (!gameObject.GetComponent<OltreScene>().scenaDiMenu)
		{
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Impostazioni").FindChild("sfondo impostazioni").FindChild("Impostazioni Gioco").GetChild(1).GetChild(0).GetChild(0).FindChild("imp attacchi aerei").GetChild(2).GetChild(0).gameObject;
			gameObject2.GetComponent<Image>().enabled = !gameObject2.GetComponent<Image>().enabled;
			if (!gameObject.GetComponent<OltreScene>().èInStrategia)
			{
				GameObject gameObject3 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
				GameObject gameObject4 = gameObject3.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
				gameObject4.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
				gameObject4.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
				gameObject4.GetComponent<AudioSource>().clip = gameObject4.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
			}
			else
			{
				GameObject gameObject5 = GameObject.FindGameObjectWithTag("Headquarters");
				gameObject5.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
				gameObject5.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject5.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
			}
		}
		else
		{
			GameObject gameObject6 = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo impostazioni").FindChild("Impostazioni").FindChild("secondo sfondo impostazioni").FindChild("Impostazioni Gioco").GetChild(1).GetChild(0).GetChild(0).FindChild("imp attacchi aerei").GetChild(2).GetChild(0).gameObject;
			GameObject gameObject7 = GameObject.FindGameObjectWithTag("MainCamera");
			gameObject6.GetComponent<Image>().enabled = !gameObject6.GetComponent<Image>().enabled;
			gameObject7.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
		}
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x00130A14 File Offset: 0x0012EC14
	public void ImpostazioniOrdAereiTerra()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello");
		if (!gameObject.GetComponent<OltreScene>().scenaDiMenu)
		{
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Impostazioni").FindChild("sfondo impostazioni").FindChild("Impostazioni Gioco").GetChild(1).GetChild(0).GetChild(0).FindChild("imp attacchi aerei").GetChild(4).GetChild(0).gameObject;
			gameObject2.GetComponent<Image>().enabled = !gameObject2.GetComponent<Image>().enabled;
			if (!gameObject.GetComponent<OltreScene>().èInStrategia)
			{
				GameObject gameObject3 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
				GameObject gameObject4 = gameObject3.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
				gameObject4.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
				gameObject4.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
				gameObject4.GetComponent<AudioSource>().clip = gameObject4.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
			}
			else
			{
				GameObject gameObject5 = GameObject.FindGameObjectWithTag("Headquarters");
				gameObject5.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
				gameObject5.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject5.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
			}
		}
		else
		{
			GameObject gameObject6 = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo impostazioni").FindChild("Impostazioni").FindChild("secondo sfondo impostazioni").FindChild("Impostazioni Gioco").GetChild(1).GetChild(0).GetChild(0).FindChild("imp attacchi aerei").GetChild(4).GetChild(0).gameObject;
			GameObject gameObject7 = GameObject.FindGameObjectWithTag("MainCamera");
			gameObject6.GetComponent<Image>().enabled = !gameObject6.GetComponent<Image>().enabled;
			gameObject7.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
		}
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x00130BE8 File Offset: 0x0012EDE8
	public void ImpostazioniOrdAereiBomb()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello");
		if (!gameObject.GetComponent<OltreScene>().scenaDiMenu)
		{
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Impostazioni").FindChild("sfondo impostazioni").FindChild("Impostazioni Gioco").GetChild(1).GetChild(0).GetChild(0).FindChild("imp attacchi aerei").GetChild(6).GetChild(0).gameObject;
			gameObject2.GetComponent<Image>().enabled = !gameObject2.GetComponent<Image>().enabled;
			if (!gameObject.GetComponent<OltreScene>().èInStrategia)
			{
				GameObject gameObject3 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
				GameObject gameObject4 = gameObject3.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
				gameObject4.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
				gameObject4.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
				gameObject4.GetComponent<AudioSource>().clip = gameObject4.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
			}
			else
			{
				GameObject gameObject5 = GameObject.FindGameObjectWithTag("Headquarters");
				gameObject5.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
				gameObject5.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject5.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
			}
		}
		else
		{
			GameObject gameObject6 = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo impostazioni").FindChild("Impostazioni").FindChild("secondo sfondo impostazioni").FindChild("Impostazioni Gioco").GetChild(1).GetChild(0).GetChild(0).FindChild("imp attacchi aerei").GetChild(6).GetChild(0).gameObject;
			GameObject gameObject7 = GameObject.FindGameObjectWithTag("MainCamera");
			gameObject6.GetComponent<Image>().enabled = !gameObject6.GetComponent<Image>().enabled;
			gameObject7.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
		}
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x00130DBC File Offset: 0x0012EFBC
	public void ImpostazioniColoreDefault()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello");
		gameObject.GetComponent<ImpostazioniScript>().appColoreDefaultUnità = true;
		if (!gameObject.GetComponent<OltreScene>().scenaDiMenu)
		{
			if (!gameObject.GetComponent<OltreScene>().èInStrategia)
			{
				GameObject gameObject2 = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
				GameObject gameObject3 = gameObject2.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
				gameObject3.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
				gameObject3.GetComponent<GestioneSuoniCamera>().nonèVoce = true;
				gameObject3.GetComponent<AudioSource>().clip = gameObject3.GetComponent<GestioneSuoniCamera>().suonoClickGenerico1;
			}
			else
			{
				GameObject gameObject4 = GameObject.FindGameObjectWithTag("Headquarters");
				gameObject4.GetComponent<GestioneSuoniCasa>().attivaSuono = true;
				gameObject4.GetComponent<GestioneSuoniCasa>().suoniDiCasa.clip = gameObject4.GetComponent<GestioneSuoniCasa>().suonoClickGenerico1;
			}
		}
		else
		{
			GameObject gameObject5 = GameObject.FindGameObjectWithTag("MainCamera");
			gameObject5.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
		}
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x00130EA0 File Offset: 0x0012F0A0
	public void BattVelAggiornaMissione(int numero)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggMissione = true;
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggAlleati = true;
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggMappa = true;
		gameObject.GetComponent<BattagliaVeloceScript>().numListaMissioni = numero;
		gameObject.GetComponent<BattagliaVeloceScript>().clickSuListaMissioni = true;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggNemici = true;
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x00130F18 File Offset: 0x0012F118
	public void BattVelAggiornaDurataBatt(int numero)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggDurataBatt = true;
		gameObject.GetComponent<BattagliaVeloceScript>().numListaDurata = numero;
		gameObject.GetComponent<BattagliaVeloceScript>().clickSuListaDurata = true;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x00130F6C File Offset: 0x0012F16C
	public void BattVelAggiornaMappa(int origine)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggMappa = true;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x00130FA8 File Offset: 0x0012F1A8
	public void BattVelCambioCasa()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		gameObject.GetComponent<BattagliaVeloceScript>().cambioCasa = true;
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x00130FCC File Offset: 0x0012F1CC
	public void BattVelApriLista(int origine)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggMissione = true;
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggAlleati = true;
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggMappa = true;
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggMappa = true;
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggDurataBatt = true;
		if (origine == 0)
		{
			gameObject.GetComponent<BattagliaVeloceScript>().clickSuListaCase = true;
		}
		else if (origine == 1)
		{
			gameObject.GetComponent<BattagliaVeloceScript>().clickSuListaStanze = true;
		}
		else if (origine != 2)
		{
			if (origine == 3)
			{
				gameObject.GetComponent<BattagliaVeloceScript>().clickSuListaMissioni = true;
			}
			else if (origine == 4)
			{
				gameObject.GetComponent<BattagliaVeloceScript>().clickSuListaDurata = true;
			}
		}
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x001310A0 File Offset: 0x0012F2A0
	public void BattVelSelezCasa(int numero)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		gameObject.GetComponent<BattagliaVeloceScript>().numeroCasa = numero;
		gameObject.GetComponent<BattagliaVeloceScript>().clickSuListaCase = true;
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x001310D0 File Offset: 0x0012F2D0
	public void BattVelSelezStanza(int numero)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		gameObject.GetComponent<BattagliaVeloceScript>().numeroStanza = numero;
		gameObject.GetComponent<BattagliaVeloceScript>().clickSuListaStanze = true;
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x00131100 File Offset: 0x0012F300
	public void BattVelApriAlleati()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggAlleati = true;
		gameObject.GetComponent<BattagliaVeloceScript>().schedaAperta = 0;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x00131148 File Offset: 0x0012F348
	public void BattVelAggiungiAlleato(int posizione)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggAlleati = true;
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggiungiAlleato = true;
		gameObject.GetComponent<BattagliaVeloceScript>().alleatoSelez = posizione;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x0013119C File Offset: 0x0012F39C
	public void BattVelTogliAlleato(int posizione)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggAlleati = true;
		gameObject.GetComponent<BattagliaVeloceScript>().battVelTogliAlleato = true;
		gameObject.GetComponent<BattagliaVeloceScript>().alleatoSelez = posizione;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x001311F0 File Offset: 0x0012F3F0
	public void BattVelApriNemici()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggNemici = true;
		gameObject.GetComponent<BattagliaVeloceScript>().schedaAperta = 1;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x00131238 File Offset: 0x0012F438
	public void BattVelAggiungiNemico(int posizione)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggNemici = true;
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggiungiNemico = true;
		gameObject.GetComponent<BattagliaVeloceScript>().nemicoSelez = posizione;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x0013128C File Offset: 0x0012F48C
	public void BattVelTogliNemico(int posizione)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggNemici = true;
		gameObject.GetComponent<BattagliaVeloceScript>().battVelTogliNemico = true;
		gameObject.GetComponent<BattagliaVeloceScript>().nemicoSelez = posizione;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x001312E0 File Offset: 0x0012F4E0
	public void BattVelInizia()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("InizioLivello").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<SalvaDati>().salvaPerBattVel = true;
		GestoreNeutroTattica.èBattagliaVeloce = true;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x00131328 File Offset: 0x0012F528
	public void BattVelInfoAlleati(int posizione)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		GameObject gameObject2 = gameObject.transform.FindChild("Dettagli Veloci Unità").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<BattagliaVeloceScript>().battVelInfoAlleato = true;
		gameObject.GetComponent<BattagliaVeloceScript>().alleatoSelezInfo = posizione;
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggArmi = true;
		gameObject2.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject2.GetComponent<CanvasGroup>().interactable = true;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = true;
		gameObject3.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x001313BC File Offset: 0x0012F5BC
	public void BattVelInfoNemici(int posizione)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		GameObject gameObject2 = gameObject.transform.FindChild("Dettagli Veloci Insetto").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<BattagliaVeloceScript>().battVelInfoNemico = true;
		gameObject.GetComponent<BattagliaVeloceScript>().nemicoSelezInfo = posizione;
		gameObject2.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject2.GetComponent<CanvasGroup>().interactable = true;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = true;
		gameObject3.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x00131444 File Offset: 0x0012F644
	public void BattVelChiudiInfoAlleato()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce").transform.FindChild("Dettagli Veloci Unità").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x001314B0 File Offset: 0x0012F6B0
	public void BattVelChiudiInfoNemico()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce").transform.FindChild("Dettagli Veloci Insetto").gameObject;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x0013151C File Offset: 0x0012F71C
	public void BattVelApriArmi()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		GameObject gameObject2 = gameObject.transform.FindChild("Armi").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject2.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject2.GetComponent<CanvasGroup>().interactable = true;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = true;
		gameObject.GetComponent<BattagliaVeloceScript>().armiAperto = true;
		gameObject3.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x00131598 File Offset: 0x0012F798
	public void BattVelChiudiArmi()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		GameObject gameObject2 = gameObject.transform.FindChild("Armi").gameObject;
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject2.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject2.GetComponent<CanvasGroup>().interactable = false;
		gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = false;
		gameObject.GetComponent<BattagliaVeloceScript>().armiAperto = false;
		gameObject3.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x00131614 File Offset: 0x0012F814
	public void BattVelPulsanteColonnaArma(int numColonna)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<BattagliaVeloceScript>().pulsanteColonnaArma = numColonna;
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x00131644 File Offset: 0x0012F844
	public void BattVelPulsanteRigaArma(int numRiga)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		gameObject.GetComponent<BattagliaVeloceScript>().pulsanteRigaArma = numRiga;
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x00131668 File Offset: 0x0012F868
	public void BattVelSalvaArma(int numArma)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<BattagliaVeloceScript>().numColonnaArmaDaSalvare = numArma;
		gameObject.GetComponent<BattagliaVeloceScript>().salvaArmaSelez = true;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x001316B0 File Offset: 0x0012F8B0
	public void BattVelResettaAlleati()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggAlleati = true;
		gameObject.GetComponent<BattagliaVeloceScript>().resettaAlleati = true;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x001316F8 File Offset: 0x0012F8F8
	public void BattVelResettaNemici()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasBattVeloce");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<BattagliaVeloceScript>().battVelAggNemici = true;
		gameObject.GetComponent<BattagliaVeloceScript>().resettaNemici = true;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x00131740 File Offset: 0x0012F940
	public void CreazCampCambiaCasa()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasCreazCamp");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<CreazioneCampagnaScript>().creazCampAggCasa = true;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x0013177C File Offset: 0x0012F97C
	public void CreazCampInizia()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasCreazCamp");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<CreazioneCampagnaScript>().creazCampIniziaCamp = true;
		gameObject2.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x001317B8 File Offset: 0x0012F9B8
	public void AlMenuIniziale()
	{
		GestoreNeutroStrategia.aggElencoBattaglia = false;
		GestoreNeutroStrategia.aggElencoMissione = false;
		GestoreNeutroStrategia.mostraResocontoBattaglia = false;
		GestoreNeutroStrategia.mostraResocontoMissione = false;
		GestoreNeutroStrategia.mostraElencoResoconto = false;
		GestoreNeutroStrategia.vincitore = 0;
		GestoreNeutroStrategia.inTattica = false;
		SceneManager.LoadScene("Menu Iniziale");
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x001317FC File Offset: 0x0012F9FC
	public void EsciDaGioco()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
		Application.Quit();
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x00131828 File Offset: 0x0012FA28
	public void SuonoClick()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("MainCamera");
		gameObject.GetComponent<GestioniSuoniMenuVari>().attivaSuono = true;
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x0013184C File Offset: 0x0012FA4C
	public void ManualeApri()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasManuale");
		gameObject.GetComponent<CanvasManualeScript>().attivaSuonoManuale = true;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x00131898 File Offset: 0x0012FA98
	public void ManualeChiudi()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasManuale");
		gameObject.GetComponent<CanvasManualeScript>().attivaSuonoManuale = true;
		gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		gameObject.GetComponent<CanvasGroup>().interactable = false;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x001318E4 File Offset: 0x0012FAE4
	public void ManualeSelezPagina(int numero)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasManuale");
		gameObject.GetComponent<CanvasManualeScript>().attivaSuonoManuale = true;
		gameObject.GetComponent<CanvasManualeScript>().paginaAperta = numero;
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x00131914 File Offset: 0x0012FB14
	public void ManualeApriPaginaPrecisa(int numero)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("CanvasManuale");
		gameObject.GetComponent<CanvasManualeScript>().attivaSuonoManuale = true;
		gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		gameObject.GetComponent<CanvasGroup>().interactable = true;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		gameObject.GetComponent<CanvasManualeScript>().paginaAperta = numero;
	}
}
