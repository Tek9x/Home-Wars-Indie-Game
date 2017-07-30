using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C6 RID: 198
public class ImpostazioniScript : MonoBehaviour
{
	// Token: 0x060006DE RID: 1758 RVA: 0x000F5984 File Offset: 0x000F3B84
	private void Start()
	{
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x000F5988 File Offset: 0x000F3B88
	private void Update()
	{
		if (!this.primoFrame)
		{
			this.primoFrame = true;
			if (!base.GetComponent<OltreScene>().scenaDiMenu)
			{
				this.sfondoImpostazioni = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Impostazioni").FindChild("sfondo impostazioni").gameObject;
				this.impostazioneGioco = this.sfondoImpostazioni.transform.FindChild("Impostazioni Gioco").gameObject;
				this.contenutoImpGioco = this.impostazioneGioco.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
				this.impostazioneVideo = this.sfondoImpostazioni.transform.FindChild("Impostazioni Video").gameObject;
				this.impostazioneAudio = this.sfondoImpostazioni.transform.FindChild("Impostazioni Audio").gameObject;
				this.impRisoluzione = this.impostazioneVideo.transform.FindChild("imp risoluzione").GetChild(1).gameObject;
				this.impFinestra = this.impostazioneVideo.transform.FindChild("imp finestra").GetChild(1).GetChild(0).gameObject;
				this.impQualità = this.impostazioneVideo.transform.FindChild("imp qualità").GetChild(1).gameObject;
				this.descrizQualità = this.impostazioneVideo.transform.FindChild("imp qualità").FindChild("descrizione").gameObject;
				this.scrittaVolumeGlobale = this.impostazioneAudio.transform.GetChild(1).GetChild(0).gameObject;
				this.scrittaVolumeMusicaStrategia = this.impostazioneAudio.transform.GetChild(2).GetChild(0).gameObject;
				this.scrittaVolumeMusicaTattica = this.impostazioneAudio.transform.GetChild(3).GetChild(0).gameObject;
				this.impVolumeGlobale = this.impostazioneAudio.transform.GetChild(1).GetChild(1).gameObject;
				this.impVolumeMusicaStrategia = this.impostazioneAudio.transform.GetChild(2).GetChild(1).gameObject;
				this.impVolumeMusicaTattica = this.impostazioneAudio.transform.GetChild(3).GetChild(1).gameObject;
				this.gruppoMaxElementi = this.contenutoImpGioco.transform.FindChild("imp max elementi").gameObject;
				this.scrittaMaxNemici = this.gruppoMaxElementi.transform.GetChild(0).GetChild(0).gameObject;
				this.scrittaMaxAlleati = this.gruppoMaxElementi.transform.GetChild(1).GetChild(0).gameObject;
				this.impMaxNemici = this.gruppoMaxElementi.transform.GetChild(0).GetChild(1).gameObject;
				this.impMaxAlleati = this.gruppoMaxElementi.transform.GetChild(1).GetChild(1).gameObject;
				this.scrittaVitaNemici = this.contenutoImpGioco.transform.FindChild("imp vita nemici").GetChild(0).gameObject;
				this.impVitaNemici = this.contenutoImpGioco.transform.FindChild("imp vita nemici").GetChild(1).gameObject;
				this.scrittaAttaccoNemici = this.contenutoImpGioco.transform.FindChild("imp danno nemici").GetChild(0).gameObject;
				this.impAttaccoNemici = this.contenutoImpGioco.transform.FindChild("imp danno nemici").GetChild(1).gameObject;
				this.oggettoMusica = GameObject.FindGameObjectWithTag("Musica");
				this.scrittaSensAerei = this.contenutoImpGioco.transform.FindChild("imp per volanti").GetChild(0).GetChild(0).gameObject;
				this.impSensAerei = this.contenutoImpGioco.transform.FindChild("imp per volanti").GetChild(0).GetChild(1).gameObject;
				this.spuntaComandiInversiVolanti = this.contenutoImpGioco.transform.FindChild("imp per volanti").GetChild(1).GetChild(1).GetChild(0).gameObject;
				this.scrittaSensRotazioneCam = this.contenutoImpGioco.transform.FindChild("imp sensibilità rot camera").GetChild(0).gameObject;
				this.impSensRotazioneCam = this.contenutoImpGioco.transform.FindChild("imp sensibilità rot camera").GetChild(1).gameObject;
				this.spuntaOrdiniAereiAria = this.contenutoImpGioco.transform.FindChild("imp attacchi aerei").GetChild(2).GetChild(0).gameObject;
				this.spuntaOrdiniAereiTerra = this.contenutoImpGioco.transform.FindChild("imp attacchi aerei").GetChild(4).GetChild(0).gameObject;
				this.spuntaOrdiniAereiBomb = this.contenutoImpGioco.transform.FindChild("imp attacchi aerei").GetChild(6).GetChild(0).gameObject;
				this.gruppoFattDiff = this.contenutoImpGioco.transform.FindChild("imp fattori diff").gameObject;
				this.impFatDiffFreshFood = this.gruppoFattDiff.transform.GetChild(0).GetChild(1).gameObject;
				this.scrittaFatDiffFreshFood = this.gruppoFattDiff.transform.GetChild(0).GetChild(0).gameObject;
				this.impFatDiffFreshFood = this.gruppoFattDiff.transform.GetChild(0).GetChild(1).gameObject;
				this.scrittaFatDiffRottenFood = this.gruppoFattDiff.transform.GetChild(1).GetChild(0).gameObject;
				this.impFatDiffRottenFood = this.gruppoFattDiff.transform.GetChild(1).GetChild(1).gameObject;
				this.scrittaFatDiffHighProteinFood = this.gruppoFattDiff.transform.GetChild(2).GetChild(0).gameObject;
				this.impFatDiffHighProteinFood = this.gruppoFattDiff.transform.GetChild(2).GetChild(1).gameObject;
				this.impFatDiffSpawnGruppi = this.gruppoFattDiff.transform.GetChild(3).GetChild(1).gameObject;
				this.impDurataBatt = this.contenutoImpGioco.transform.FindChild("imp durata batt").GetChild(1).gameObject;
				this.impColoreUnità = this.contenutoImpGioco.transform.FindChild("imp colore unità").gameObject;
				this.barraColoreRosso = this.impColoreUnità.transform.GetChild(2).gameObject;
				this.scrittaBarraRossa = this.impColoreUnità.transform.GetChild(1).gameObject;
				this.barraColoreVerde = this.impColoreUnità.transform.GetChild(6).gameObject;
				this.scrittaBarraVerde = this.impColoreUnità.transform.GetChild(5).gameObject;
				this.barraColoreBlu = this.impColoreUnità.transform.GetChild(10).gameObject;
				this.scrittaBarraBlu = this.impColoreUnità.transform.GetChild(9).gameObject;
				this.coloreFinaleUnità = this.impColoreUnità.transform.GetChild(14).gameObject;
				this.impRisoluzione.transform.GetChild(0).GetComponent<Text>().text = Screen.currentResolution.ToString();
				if (!Screen.fullScreen)
				{
					this.impFinestra.GetComponent<Image>().enabled = true;
				}
				else
				{
					this.impFinestra.GetComponent<Image>().enabled = false;
				}
				this.impQualità.GetComponent<Dropdown>().value = QualitySettings.GetQualityLevel();
				AudioListener.volume = PlayerPrefs.GetFloat("volume globale");
				this.impVolumeGlobale.GetComponent<Slider>().value = PlayerPrefs.GetFloat("volume globale") * 100f;
				this.impVolumeMusicaStrategia.GetComponent<Slider>().value = PlayerPrefs.GetFloat("volume musica strategia") * 100f;
				this.impVolumeMusicaTattica.GetComponent<Slider>().value = PlayerPrefs.GetFloat("volume musica tattica") * 100f;
				if (base.GetComponent<OltreScene>().èInStrategia)
				{
					this.cameraCasa = GameObject.FindGameObjectWithTag("MainCamera");
				}
				else
				{
					this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
					this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
					this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
				}
				this.impMaxNemici.GetComponent<Slider>().value = (float)PlayerPrefs.GetInt("max nemici");
				this.impMaxAlleati.GetComponent<Slider>().value = (float)PlayerPrefs.GetInt("max alleati");
				this.impVitaNemici.GetComponent<Slider>().value = PlayerPrefs.GetFloat("vita nemici");
				this.impAttaccoNemici.GetComponent<Slider>().value = PlayerPrefs.GetFloat("attacco nemici");
				this.impFatDiffFreshFood.GetComponent<Slider>().value = PlayerPrefs.GetFloat("fattore diff fresh food");
				this.impFatDiffRottenFood.GetComponent<Slider>().value = PlayerPrefs.GetFloat("fattore diff rotten food");
				this.impFatDiffHighProteinFood.GetComponent<Slider>().value = PlayerPrefs.GetFloat("fattore diff high protein food");
				this.impFatDiffSpawnGruppi.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("fattore diff spawn gruppi");
				this.impDurataBatt.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("durata battaglia");
				this.impSensAerei.GetComponent<Slider>().value = PlayerPrefs.GetFloat("sensibilità aerei");
				this.impSensRotazioneCam.GetComponent<Slider>().value = PlayerPrefs.GetFloat("sensibilità rotazione camera");
				if (PlayerPrefs.GetInt("comandi inversi volanti") == 0)
				{
					this.spuntaComandiInversiVolanti.GetComponent<Image>().enabled = false;
				}
				else
				{
					this.spuntaComandiInversiVolanti.GetComponent<Image>().enabled = true;
				}
				if (PlayerPrefs.GetInt("ordini aerei aria") == 0)
				{
					this.spuntaOrdiniAereiAria.GetComponent<Image>().enabled = false;
				}
				else
				{
					this.spuntaOrdiniAereiAria.GetComponent<Image>().enabled = true;
				}
				if (PlayerPrefs.GetInt("ordini aerei terra") == 0)
				{
					this.spuntaOrdiniAereiTerra.GetComponent<Image>().enabled = false;
				}
				else
				{
					this.spuntaOrdiniAereiTerra.GetComponent<Image>().enabled = true;
				}
				if (PlayerPrefs.GetInt("ordini aerei bomb") == 0)
				{
					this.spuntaOrdiniAereiBomb.GetComponent<Image>().enabled = false;
				}
				else
				{
					this.spuntaOrdiniAereiBomb.GetComponent<Image>().enabled = true;
				}
				this.barraColoreRosso.GetComponent<Slider>().value = PlayerPrefs.GetFloat("colore rosso unità");
				this.barraColoreVerde.GetComponent<Slider>().value = PlayerPrefs.GetFloat("colore verde unità");
				this.barraColoreBlu.GetComponent<Slider>().value = PlayerPrefs.GetFloat("colore blu unità");
				float @float = PlayerPrefs.GetFloat("colore rosso unità");
				float float2 = PlayerPrefs.GetFloat("colore verde unità");
				float float3 = PlayerPrefs.GetFloat("colore blu unità");
				this.materialeColoreUnità.color = new Color(@float, float2, float3);
				this.materialeColoreTrappole.color = new Color(@float, float2, float3);
				this.materialeColoreEdificiBatt.color = new Color(@float, float2, float3);
				this.materialeColoreUnitàCasa.color = new Color(@float, float2, float3);
				this.materialeColoreHeadquarters.color = new Color(@float, float2, float3);
				this.materialeColorePostiHeadquarters.color = new Color(@float, float2, float3);
				if (!base.GetComponent<OltreScene>().èInStrategia)
				{
					this.gruppoMaxElementi.GetComponent<CanvasGroup>().interactable = false;
					this.gruppoMaxElementi.GetComponent<Image>().color = Color.gray;
					this.impVitaNemici.transform.parent.GetComponent<CanvasGroup>().interactable = false;
					this.impVitaNemici.transform.parent.GetComponent<Image>().color = Color.gray;
					this.impAttaccoNemici.transform.parent.GetComponent<CanvasGroup>().interactable = false;
					this.impAttaccoNemici.transform.parent.GetComponent<Image>().color = Color.gray;
					this.impDurataBatt.transform.parent.GetComponent<CanvasGroup>().interactable = false;
					this.impDurataBatt.transform.parent.GetComponent<Image>().color = Color.gray;
				}
			}
			else if (GameObject.FindGameObjectWithTag("CanvasMenuIniz"))
			{
				this.primoSfondoImp = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo impostazioni").gameObject;
				this.sfondoImpostazioni = this.primoSfondoImp.transform.FindChild("Impostazioni").FindChild("secondo sfondo impostazioni").gameObject;
				this.impostazioneGioco = this.sfondoImpostazioni.transform.FindChild("Impostazioni Gioco").gameObject;
				this.contenutoImpGioco = this.impostazioneGioco.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
				this.impostazioneVideo = this.sfondoImpostazioni.transform.FindChild("Impostazioni Video").gameObject;
				this.impostazioneAudio = this.sfondoImpostazioni.transform.FindChild("Impostazioni Audio").gameObject;
				this.impRisoluzione = this.impostazioneVideo.transform.FindChild("imp risoluzione").GetChild(1).gameObject;
				this.impFinestra = this.impostazioneVideo.transform.FindChild("imp finestra").GetChild(1).GetChild(0).gameObject;
				this.impQualità = this.impostazioneVideo.transform.FindChild("imp qualità").GetChild(1).gameObject;
				this.descrizQualità = this.impostazioneVideo.transform.FindChild("imp qualità").FindChild("descrizione").gameObject;
				this.scrittaVolumeGlobale = this.impostazioneAudio.transform.GetChild(1).GetChild(0).gameObject;
				this.scrittaVolumeMusicaStrategia = this.impostazioneAudio.transform.GetChild(2).GetChild(0).gameObject;
				this.scrittaVolumeMusicaTattica = this.impostazioneAudio.transform.GetChild(3).GetChild(0).gameObject;
				this.impVolumeGlobale = this.impostazioneAudio.transform.GetChild(1).GetChild(1).gameObject;
				this.impVolumeMusicaStrategia = this.impostazioneAudio.transform.GetChild(2).GetChild(1).gameObject;
				this.impVolumeMusicaTattica = this.impostazioneAudio.transform.GetChild(3).GetChild(1).gameObject;
				this.gruppoMaxElementi = this.contenutoImpGioco.transform.FindChild("imp max elementi").gameObject;
				this.scrittaMaxNemici = this.gruppoMaxElementi.transform.GetChild(0).GetChild(0).gameObject;
				this.scrittaMaxAlleati = this.gruppoMaxElementi.transform.GetChild(1).GetChild(0).gameObject;
				this.impMaxNemici = this.gruppoMaxElementi.transform.GetChild(0).GetChild(1).gameObject;
				this.impMaxAlleati = this.gruppoMaxElementi.transform.GetChild(1).GetChild(1).gameObject;
				this.scrittaVitaNemici = this.contenutoImpGioco.transform.FindChild("imp vita nemici").GetChild(0).gameObject;
				this.impVitaNemici = this.contenutoImpGioco.transform.FindChild("imp vita nemici").GetChild(1).gameObject;
				this.scrittaAttaccoNemici = this.contenutoImpGioco.transform.FindChild("imp danno nemici").GetChild(0).gameObject;
				this.impAttaccoNemici = this.contenutoImpGioco.transform.FindChild("imp danno nemici").GetChild(1).gameObject;
				this.oggettoMusica = GameObject.FindGameObjectWithTag("Musica");
				this.scrittaSensAerei = this.contenutoImpGioco.transform.FindChild("imp per volanti").GetChild(0).GetChild(0).gameObject;
				this.impSensAerei = this.contenutoImpGioco.transform.FindChild("imp per volanti").GetChild(0).GetChild(1).gameObject;
				this.spuntaComandiInversiVolanti = this.contenutoImpGioco.transform.FindChild("imp per volanti").GetChild(1).GetChild(1).GetChild(0).gameObject;
				this.scrittaSensRotazioneCam = this.contenutoImpGioco.transform.FindChild("imp sensibilità rot camera").GetChild(0).gameObject;
				this.impSensRotazioneCam = this.contenutoImpGioco.transform.FindChild("imp sensibilità rot camera").GetChild(1).gameObject;
				this.spuntaOrdiniAereiAria = this.contenutoImpGioco.transform.FindChild("imp attacchi aerei").GetChild(2).GetChild(0).gameObject;
				this.spuntaOrdiniAereiTerra = this.contenutoImpGioco.transform.FindChild("imp attacchi aerei").GetChild(4).GetChild(0).gameObject;
				this.spuntaOrdiniAereiBomb = this.contenutoImpGioco.transform.FindChild("imp attacchi aerei").GetChild(6).GetChild(0).gameObject;
				this.gruppoFattDiff = this.contenutoImpGioco.transform.FindChild("imp fattori diff").gameObject;
				this.impFatDiffFreshFood = this.gruppoFattDiff.transform.GetChild(0).GetChild(1).gameObject;
				this.scrittaFatDiffFreshFood = this.gruppoFattDiff.transform.GetChild(0).GetChild(0).gameObject;
				this.impFatDiffFreshFood = this.gruppoFattDiff.transform.GetChild(0).GetChild(1).gameObject;
				this.scrittaFatDiffRottenFood = this.gruppoFattDiff.transform.GetChild(1).GetChild(0).gameObject;
				this.impFatDiffRottenFood = this.gruppoFattDiff.transform.GetChild(1).GetChild(1).gameObject;
				this.scrittaFatDiffHighProteinFood = this.gruppoFattDiff.transform.GetChild(2).GetChild(0).gameObject;
				this.impFatDiffHighProteinFood = this.gruppoFattDiff.transform.GetChild(2).GetChild(1).gameObject;
				this.impFatDiffSpawnGruppi = this.gruppoFattDiff.transform.GetChild(3).GetChild(1).gameObject;
				this.impDurataBatt = this.contenutoImpGioco.transform.FindChild("imp durata batt").GetChild(1).gameObject;
				this.impColoreUnità = this.contenutoImpGioco.transform.FindChild("imp colore unità").gameObject;
				this.barraColoreRosso = this.impColoreUnità.transform.GetChild(2).gameObject;
				this.scrittaBarraRossa = this.impColoreUnità.transform.GetChild(1).gameObject;
				this.barraColoreVerde = this.impColoreUnità.transform.GetChild(6).gameObject;
				this.scrittaBarraVerde = this.impColoreUnità.transform.GetChild(5).gameObject;
				this.barraColoreBlu = this.impColoreUnità.transform.GetChild(10).gameObject;
				this.scrittaBarraBlu = this.impColoreUnità.transform.GetChild(9).gameObject;
				this.coloreFinaleUnità = this.impColoreUnità.transform.GetChild(14).gameObject;
				this.impRisoluzione.transform.GetChild(0).GetComponent<Text>().text = Screen.currentResolution.ToString();
				if (!Screen.fullScreen)
				{
					this.impFinestra.GetComponent<Image>().enabled = true;
				}
				else
				{
					this.impFinestra.GetComponent<Image>().enabled = false;
				}
				this.impQualità.GetComponent<Dropdown>().value = QualitySettings.GetQualityLevel();
				AudioListener.volume = PlayerPrefs.GetFloat("volume globale");
				this.impVolumeGlobale.GetComponent<Slider>().value = PlayerPrefs.GetFloat("volume globale") * 100f;
				this.impVolumeMusicaStrategia.GetComponent<Slider>().value = PlayerPrefs.GetFloat("volume musica strategia") * 100f;
				this.impVolumeMusicaTattica.GetComponent<Slider>().value = PlayerPrefs.GetFloat("volume musica tattica") * 100f;
				this.impMaxNemici.GetComponent<Slider>().value = (float)PlayerPrefs.GetInt("max nemici");
				this.impMaxAlleati.GetComponent<Slider>().value = (float)PlayerPrefs.GetInt("max alleati");
				this.impVitaNemici.GetComponent<Slider>().value = PlayerPrefs.GetFloat("vita nemici");
				this.impAttaccoNemici.GetComponent<Slider>().value = PlayerPrefs.GetFloat("attacco nemici");
				this.impFatDiffFreshFood.GetComponent<Slider>().value = PlayerPrefs.GetFloat("fattore diff fresh food");
				this.impFatDiffRottenFood.GetComponent<Slider>().value = PlayerPrefs.GetFloat("fattore diff rotten food");
				this.impFatDiffHighProteinFood.GetComponent<Slider>().value = PlayerPrefs.GetFloat("fattore diff high protein food");
				this.impFatDiffSpawnGruppi.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("fattore diff spawn gruppi");
				this.impDurataBatt.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("durata battaglia");
				this.impSensAerei.GetComponent<Slider>().value = PlayerPrefs.GetFloat("sensibilità aerei");
				this.impSensRotazioneCam.GetComponent<Slider>().value = PlayerPrefs.GetFloat("sensibilità rotazione camera");
				if (PlayerPrefs.GetInt("comandi inversi volanti") == 0)
				{
					this.spuntaComandiInversiVolanti.GetComponent<Image>().enabled = false;
				}
				else
				{
					this.spuntaComandiInversiVolanti.GetComponent<Image>().enabled = true;
				}
				if (PlayerPrefs.GetInt("ordini aerei aria") == 0)
				{
					this.spuntaOrdiniAereiAria.GetComponent<Image>().enabled = false;
				}
				else
				{
					this.spuntaOrdiniAereiAria.GetComponent<Image>().enabled = true;
				}
				if (PlayerPrefs.GetInt("ordini aerei terra") == 0)
				{
					this.spuntaOrdiniAereiTerra.GetComponent<Image>().enabled = false;
				}
				else
				{
					this.spuntaOrdiniAereiTerra.GetComponent<Image>().enabled = true;
				}
				if (PlayerPrefs.GetInt("ordini aerei bomb") == 0)
				{
					this.spuntaOrdiniAereiBomb.GetComponent<Image>().enabled = false;
				}
				else
				{
					this.spuntaOrdiniAereiBomb.GetComponent<Image>().enabled = true;
				}
				this.barraColoreRosso.GetComponent<Slider>().value = PlayerPrefs.GetFloat("colore rosso unità");
				this.barraColoreVerde.GetComponent<Slider>().value = PlayerPrefs.GetFloat("colore verde unità");
				this.barraColoreBlu.GetComponent<Slider>().value = PlayerPrefs.GetFloat("colore blu unità");
			}
		}
		if (!base.GetComponent<OltreScene>().scenaDiMenu)
		{
			if (base.GetComponent<OltreScene>().menuAperto)
			{
				this.SceltaScheda();
				this.ImpostGioco();
				this.ImpostVideo();
				this.ImpostAudio();
				if (this.impApplicate)
				{
					PlayerPrefs.Save();
					if (this.oggettoMusica)
					{
						if (base.GetComponent<OltreScene>().èInStrategia)
						{
							this.oggettoMusica.GetComponent<MusicaScript>().musica.volume = PlayerPrefs.GetFloat("volume musica strategia");
						}
						else
						{
							this.oggettoMusica.GetComponent<MusicaScript>().musica.volume = PlayerPrefs.GetFloat("volume musica tattica");
						}
					}
				}
				this.impApplicate = false;
				this.impChiuse = false;
			}
		}
		else if (base.GetComponent<OltreScene>().èMenuIniziale && this.primoSfondoImp.GetComponent<CanvasGroup>().alpha == 1f)
		{
			this.SceltaScheda();
			this.ImpostGioco();
			this.ImpostVideo();
			this.ImpostAudio();
			if (this.impApplicate)
			{
				PlayerPrefs.Save();
			}
			this.impApplicate = false;
			this.impChiuse = false;
		}
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x000F7144 File Offset: 0x000F5344
	private void SceltaScheda()
	{
		if (this.impostGiàScelta)
		{
			this.impostGiàScelta = false;
			for (int i = 0; i < this.sfondoImpostazioni.transform.childCount; i++)
			{
				if (i == this.impostSelez)
				{
					this.sfondoImpostazioni.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 1f;
					this.sfondoImpostazioni.transform.GetChild(i).GetComponent<CanvasGroup>().interactable = true;
					this.sfondoImpostazioni.transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = true;
				}
				else
				{
					this.sfondoImpostazioni.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 0f;
					this.sfondoImpostazioni.transform.GetChild(i).GetComponent<CanvasGroup>().interactable = false;
					this.sfondoImpostazioni.transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
			}
		}
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x000F7248 File Offset: 0x000F5448
	private void ImpostGioco()
	{
		this.impMaxAlleati.GetComponent<Slider>().value = this.impMaxNemici.GetComponent<Slider>().value / this.impMaxNemici.GetComponent<Slider>().maxValue * this.impMaxAlleati.GetComponent<Slider>().maxValue;
		int value = Mathf.FloorToInt(this.impMaxNemici.GetComponent<Slider>().value);
		int value2 = Mathf.FloorToInt(this.impMaxAlleati.GetComponent<Slider>().value);
		this.scrittaMaxNemici.GetComponent<Text>().text = "MAX ENEMIES ON SCREEN:  " + value.ToString();
		this.scrittaMaxAlleati.GetComponent<Text>().text = "MAX ALLIES ON SCREEN:  " + value2.ToString();
		float value3 = Mathf.Floor(this.impVitaNemici.GetComponent<Slider>().value);
		this.scrittaVitaNemici.GetComponent<Text>().text = "ENEMIES' HEALTH:  " + value3.ToString() + "%";
		float value4 = Mathf.Floor(this.impAttaccoNemici.GetComponent<Slider>().value);
		this.scrittaAttaccoNemici.GetComponent<Text>().text = "ENEMIES' ATTACK:  " + value4.ToString() + "%";
		float num = Mathf.Floor(this.impSensAerei.GetComponent<Slider>().value);
		this.scrittaSensAerei.GetComponent<Text>().text = "PLANE'S SENSITIVITY:  " + num.ToString() + "%";
		float num2 = Mathf.Floor(this.impSensRotazioneCam.GetComponent<Slider>().value);
		this.scrittaSensRotazioneCam.GetComponent<Text>().text = "CAMERA ROTATION SENSITIVITY:  " + num2.ToString() + "%";
		float value5 = this.impFatDiffFreshFood.GetComponent<Slider>().value;
		this.scrittaFatDiffFreshFood.GetComponent<Text>().text = "FRESH FOOD ABUNDANCE:  x" + value5.ToString("F2");
		float value6 = this.impFatDiffRottenFood.GetComponent<Slider>().value;
		this.scrittaFatDiffRottenFood.GetComponent<Text>().text = "ROTTEN FOOD ABUNDANCE:  x" + value6.ToString("F2");
		float value7 = this.impFatDiffHighProteinFood.GetComponent<Slider>().value;
		this.scrittaFatDiffHighProteinFood.GetComponent<Text>().text = "HIGH PROTEIN FOOD ABUNDANCE:  x" + value7.ToString("F2");
		if (this.appColoreDefaultUnità)
		{
			this.appColoreDefaultUnità = false;
			this.barraColoreRosso.GetComponent<Slider>().value = 0.06f;
			this.barraColoreVerde.GetComponent<Slider>().value = 0.49f;
			this.barraColoreBlu.GetComponent<Slider>().value = 0.1f;
		}
		float value8 = this.barraColoreRosso.GetComponent<Slider>().value;
		float value9 = this.barraColoreVerde.GetComponent<Slider>().value;
		float value10 = this.barraColoreBlu.GetComponent<Slider>().value;
		this.scrittaBarraRossa.GetComponent<Text>().text = "Red:  " + value8.ToString("F2");
		this.scrittaBarraVerde.GetComponent<Text>().text = "Green:  " + value9.ToString("F2");
		this.scrittaBarraBlu.GetComponent<Text>().text = "Blue:  " + value10.ToString("F2");
		this.coloreFinaleUnità.GetComponent<Image>().color = new Color(value8, value9, value10);
		if (this.impApplicate && this.impostSelez == 0)
		{
			PlayerPrefs.SetInt("max nemici", value);
			PlayerPrefs.SetInt("max alleati", value2);
			PlayerPrefs.SetFloat("vita nemici", value3);
			PlayerPrefs.SetFloat("attacco nemici", value4);
			PlayerPrefs.SetFloat("sensibilità aerei", num);
			PlayerPrefs.SetFloat("sensibilità rotazione camera", num2);
			PlayerPrefs.SetFloat("fattore diff fresh food", this.impFatDiffFreshFood.GetComponent<Slider>().value);
			PlayerPrefs.SetFloat("fattore diff rotten food", this.impFatDiffRottenFood.GetComponent<Slider>().value);
			PlayerPrefs.SetFloat("fattore diff high protein food", this.impFatDiffHighProteinFood.GetComponent<Slider>().value);
			PlayerPrefs.SetInt("fattore diff spawn gruppi", this.impFatDiffSpawnGruppi.GetComponent<Dropdown>().value);
			PlayerPrefs.SetInt("durata battaglia", this.impDurataBatt.GetComponent<Dropdown>().value);
			PlayerPrefs.SetFloat("colore rosso unità", value8);
			PlayerPrefs.SetFloat("colore verde unità", value9);
			PlayerPrefs.SetFloat("colore blu unità", value10);
			this.materialeColoreUnità.color = new Color(value8, value9, value10);
			this.materialeColoreTrappole.color = new Color(value8, value9, value10);
			this.materialeColoreEdificiBatt.color = new Color(value8, value9, value10);
			this.materialeColoreUnitàCasa.color = new Color(value8, value9, value10);
			this.materialeColoreHeadquarters.color = new Color(value8, value9, value10);
			this.materialeColorePostiHeadquarters.color = new Color(value8, value9, value10);
			if (!this.spuntaComandiInversiVolanti.GetComponent<Image>().enabled)
			{
				PlayerPrefs.SetInt("comandi inversi volanti", 0);
			}
			else
			{
				PlayerPrefs.SetInt("comandi inversi volanti", 1);
			}
			if (!this.spuntaOrdiniAereiAria.GetComponent<Image>().enabled)
			{
				PlayerPrefs.SetInt("ordini aerei aria", 0);
			}
			else
			{
				PlayerPrefs.SetInt("ordini aerei aria", 1);
			}
			if (!this.spuntaOrdiniAereiTerra.GetComponent<Image>().enabled)
			{
				PlayerPrefs.SetInt("ordini aerei terra", 0);
			}
			else
			{
				PlayerPrefs.SetInt("ordini aerei terra", 1);
			}
			if (!this.spuntaOrdiniAereiBomb.GetComponent<Image>().enabled)
			{
				PlayerPrefs.SetInt("ordini aerei bomb", 0);
			}
			else
			{
				PlayerPrefs.SetInt("ordini aerei bomb", 1);
			}
			if (!base.GetComponent<OltreScene>().scenaDiMenu)
			{
				if (base.GetComponent<OltreScene>().èInStrategia)
				{
					this.cameraCasa.GetComponent<CameraCasa>().sensRotCam = num2 / 100f;
				}
				else
				{
					this.primaCamera.GetComponent<PrimaCamera>().sensRotCam = num2 / 100f;
					for (int i = 0; i < this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati.Count; i++)
					{
						if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[i] != null && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[i].GetComponent<PresenzaAlleato>().volante)
						{
							this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati[i].GetComponent<PresenzaAlleato>().sensAerei = num / 100f;
						}
					}
				}
			}
		}
		if (this.impChiuse)
		{
			this.impMaxNemici.GetComponent<Slider>().value = (float)PlayerPrefs.GetInt("max nemici");
			this.impMaxAlleati.GetComponent<Slider>().value = (float)PlayerPrefs.GetInt("max alleati");
			this.impVitaNemici.GetComponent<Slider>().value = PlayerPrefs.GetFloat("vita nemici");
			this.impAttaccoNemici.GetComponent<Slider>().value = PlayerPrefs.GetFloat("attacco nemici");
			this.impSensAerei.GetComponent<Slider>().value = PlayerPrefs.GetFloat("sensibilità aerei");
			this.impSensRotazioneCam.GetComponent<Slider>().value = PlayerPrefs.GetFloat("sensibilità rotazione camera");
			this.impFatDiffFreshFood.GetComponent<Slider>().value = PlayerPrefs.GetFloat("fattore diff fresh food");
			this.impFatDiffRottenFood.GetComponent<Slider>().value = PlayerPrefs.GetFloat("fattore diff rotten food");
			this.impFatDiffHighProteinFood.GetComponent<Slider>().value = PlayerPrefs.GetFloat("fattore diff high protein food");
			this.impFatDiffSpawnGruppi.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("fattore diff spawn gruppi");
			this.impDurataBatt.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("durata battaglia");
			this.barraColoreRosso.GetComponent<Slider>().value = PlayerPrefs.GetFloat("colore rosso unità");
			this.barraColoreVerde.GetComponent<Slider>().value = PlayerPrefs.GetFloat("colore verde unità");
			this.barraColoreBlu.GetComponent<Slider>().value = PlayerPrefs.GetFloat("colore blu unità");
			if (PlayerPrefs.GetInt("comandi inversi volanti") == 0)
			{
				this.spuntaComandiInversiVolanti.GetComponent<Image>().enabled = false;
			}
			else
			{
				this.spuntaComandiInversiVolanti.GetComponent<Image>().enabled = true;
			}
		}
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x000F7A98 File Offset: 0x000F5C98
	private void ImpostVideo()
	{
		for (int i = 0; i < this.ListaDescrQualità.Count; i++)
		{
			if (this.impQualità.GetComponent<Dropdown>().value == i)
			{
				this.descrizQualità.GetComponent<Text>().text = this.ListaDescrQualità[i].GetComponent<Text>().text;
			}
		}
		if (this.impApplicate && this.impostSelez == 1)
		{
			for (int j = 0; j < 7; j++)
			{
				if (this.impRisoluzione.GetComponent<Dropdown>().value == j)
				{
					if (j == 0)
					{
						Screen.SetResolution(640, 480, true);
					}
					else if (j == 1)
					{
						Screen.SetResolution(800, 600, true);
					}
					else if (j == 2)
					{
						Screen.SetResolution(1024, 768, true);
					}
					else if (j == 3)
					{
						Screen.SetResolution(1280, 800, true);
					}
					else if (j == 4)
					{
						Screen.SetResolution(1280, 1024, true);
					}
					else if (j == 5)
					{
						Screen.SetResolution(1366, 768, true);
					}
					else if (j == 6)
					{
						Screen.SetResolution(1920, 1080, true);
					}
				}
			}
			for (int k = 0; k < 5; k++)
			{
				if (this.impQualità.GetComponent<Dropdown>().value == k)
				{
					QualitySettings.SetQualityLevel(k, true);
				}
			}
			if (!this.impFinestra.GetComponent<Image>().enabled)
			{
				Screen.fullScreen = true;
			}
			else
			{
				Screen.fullScreen = false;
			}
		}
		if (this.impChiuse)
		{
			if (!Screen.fullScreen)
			{
				this.impFinestra.GetComponent<Image>().enabled = true;
			}
			else
			{
				this.impFinestra.GetComponent<Image>().enabled = false;
			}
			this.impRisoluzione.transform.GetChild(0).GetComponent<Text>().text = Screen.currentResolution.ToString();
			this.impQualità.GetComponent<Dropdown>().value = QualitySettings.GetQualityLevel();
		}
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x000F7CC8 File Offset: 0x000F5EC8
	private void ImpostAudio()
	{
		float num = Mathf.Floor(this.impVolumeGlobale.GetComponent<Slider>().value);
		this.scrittaVolumeGlobale.GetComponent<Text>().text = "MAIN VOLUME:  " + num.ToString() + "%";
		float num2 = Mathf.Floor(this.impVolumeMusicaStrategia.GetComponent<Slider>().value);
		this.scrittaVolumeMusicaStrategia.GetComponent<Text>().text = "HOME MUSIC VOLUME:  " + num2.ToString() + "%";
		float num3 = Mathf.Floor(this.impVolumeMusicaTattica.GetComponent<Slider>().value);
		this.scrittaVolumeMusicaTattica.GetComponent<Text>().text = "BATTLE MUSIC VOLUME:  " + num3.ToString() + "%";
		if (this.impApplicate && this.impostSelez == 2)
		{
			PlayerPrefs.SetFloat("volume globale", num / 100f);
			PlayerPrefs.SetFloat("volume musica strategia", num2 / 100f);
			PlayerPrefs.SetFloat("volume musica tattica", num3 / 100f);
			AudioListener.volume = num / 100f;
		}
		if (this.impChiuse)
		{
			this.impVolumeGlobale.GetComponent<Slider>().value = PlayerPrefs.GetFloat("volume globale") * 100f;
			this.impVolumeMusicaStrategia.GetComponent<Slider>().value = PlayerPrefs.GetFloat("volume musica strategia") * 100f;
			this.impVolumeMusicaTattica.GetComponent<Slider>().value = PlayerPrefs.GetFloat("volume musica tattica") * 100f;
		}
	}

	// Token: 0x0400198E RID: 6542
	private GameObject sfondoImpostazioni;

	// Token: 0x0400198F RID: 6543
	private GameObject impostazioneGioco;

	// Token: 0x04001990 RID: 6544
	private GameObject impostazioneVideo;

	// Token: 0x04001991 RID: 6545
	private GameObject impostazioneAudio;

	// Token: 0x04001992 RID: 6546
	private GameObject impRisoluzione;

	// Token: 0x04001993 RID: 6547
	private GameObject impFinestra;

	// Token: 0x04001994 RID: 6548
	private GameObject impQualità;

	// Token: 0x04001995 RID: 6549
	private GameObject descrizQualità;

	// Token: 0x04001996 RID: 6550
	private GameObject scrittaVolumeGlobale;

	// Token: 0x04001997 RID: 6551
	private GameObject scrittaVolumeMusicaStrategia;

	// Token: 0x04001998 RID: 6552
	private GameObject scrittaVolumeMusicaTattica;

	// Token: 0x04001999 RID: 6553
	private GameObject impVolumeGlobale;

	// Token: 0x0400199A RID: 6554
	private GameObject impVolumeMusicaStrategia;

	// Token: 0x0400199B RID: 6555
	private GameObject impVolumeMusicaTattica;

	// Token: 0x0400199C RID: 6556
	private GameObject scrittaMaxNemici;

	// Token: 0x0400199D RID: 6557
	private GameObject scrittaMaxAlleati;

	// Token: 0x0400199E RID: 6558
	private GameObject impMaxNemici;

	// Token: 0x0400199F RID: 6559
	private GameObject impMaxAlleati;

	// Token: 0x040019A0 RID: 6560
	private GameObject scrittaVitaNemici;

	// Token: 0x040019A1 RID: 6561
	private GameObject impVitaNemici;

	// Token: 0x040019A2 RID: 6562
	private GameObject scrittaAttaccoNemici;

	// Token: 0x040019A3 RID: 6563
	private GameObject impAttaccoNemici;

	// Token: 0x040019A4 RID: 6564
	private GameObject primoSfondoImp;

	// Token: 0x040019A5 RID: 6565
	private GameObject oggettoMusica;

	// Token: 0x040019A6 RID: 6566
	private GameObject scrittaSensAerei;

	// Token: 0x040019A7 RID: 6567
	private GameObject impSensAerei;

	// Token: 0x040019A8 RID: 6568
	private GameObject spuntaComandiInversiVolanti;

	// Token: 0x040019A9 RID: 6569
	private GameObject scrittaSensRotazioneCam;

	// Token: 0x040019AA RID: 6570
	private GameObject impSensRotazioneCam;

	// Token: 0x040019AB RID: 6571
	private GameObject contenutoImpGioco;

	// Token: 0x040019AC RID: 6572
	private GameObject gruppoMaxElementi;

	// Token: 0x040019AD RID: 6573
	private GameObject spuntaOrdiniAereiAria;

	// Token: 0x040019AE RID: 6574
	private GameObject spuntaOrdiniAereiTerra;

	// Token: 0x040019AF RID: 6575
	private GameObject spuntaOrdiniAereiBomb;

	// Token: 0x040019B0 RID: 6576
	private GameObject gruppoFattDiff;

	// Token: 0x040019B1 RID: 6577
	private GameObject impFatDiffFreshFood;

	// Token: 0x040019B2 RID: 6578
	private GameObject impFatDiffRottenFood;

	// Token: 0x040019B3 RID: 6579
	private GameObject impFatDiffHighProteinFood;

	// Token: 0x040019B4 RID: 6580
	private GameObject impFatDiffSpawnGruppi;

	// Token: 0x040019B5 RID: 6581
	private GameObject scrittaFatDiffFreshFood;

	// Token: 0x040019B6 RID: 6582
	private GameObject scrittaFatDiffRottenFood;

	// Token: 0x040019B7 RID: 6583
	private GameObject scrittaFatDiffHighProteinFood;

	// Token: 0x040019B8 RID: 6584
	private GameObject impDurataBatt;

	// Token: 0x040019B9 RID: 6585
	private GameObject impColoreUnità;

	// Token: 0x040019BA RID: 6586
	private GameObject barraColoreRosso;

	// Token: 0x040019BB RID: 6587
	private GameObject scrittaBarraRossa;

	// Token: 0x040019BC RID: 6588
	private GameObject barraColoreVerde;

	// Token: 0x040019BD RID: 6589
	private GameObject scrittaBarraVerde;

	// Token: 0x040019BE RID: 6590
	private GameObject barraColoreBlu;

	// Token: 0x040019BF RID: 6591
	private GameObject scrittaBarraBlu;

	// Token: 0x040019C0 RID: 6592
	private GameObject coloreFinaleUnità;

	// Token: 0x040019C1 RID: 6593
	public Material materialeColoreUnità;

	// Token: 0x040019C2 RID: 6594
	public Material materialeColoreTrappole;

	// Token: 0x040019C3 RID: 6595
	public Material materialeColoreEdificiBatt;

	// Token: 0x040019C4 RID: 6596
	public Material materialeColoreUnitàCasa;

	// Token: 0x040019C5 RID: 6597
	public Material materialeColoreHeadquarters;

	// Token: 0x040019C6 RID: 6598
	public Material materialeColorePostiHeadquarters;

	// Token: 0x040019C7 RID: 6599
	private GameObject cameraCasa;

	// Token: 0x040019C8 RID: 6600
	private GameObject infoNeutreTattica;

	// Token: 0x040019C9 RID: 6601
	private GameObject primaCamera;

	// Token: 0x040019CA RID: 6602
	private GameObject infoAlleati;

	// Token: 0x040019CB RID: 6603
	public List<GameObject> ListaDescrQualità;

	// Token: 0x040019CC RID: 6604
	public int impostSelez;

	// Token: 0x040019CD RID: 6605
	public bool impostGiàScelta;

	// Token: 0x040019CE RID: 6606
	public bool impApplicate;

	// Token: 0x040019CF RID: 6607
	public bool impChiuse;

	// Token: 0x040019D0 RID: 6608
	private bool primoFrame;

	// Token: 0x040019D1 RID: 6609
	public bool appColoreDefaultUnità;
}
