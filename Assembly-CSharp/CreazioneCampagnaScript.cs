using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020000C5 RID: 197
public class CreazioneCampagnaScript : MonoBehaviour
{
	// Token: 0x060006D8 RID: 1752 RVA: 0x000F5158 File Offset: 0x000F3358
	private void Start()
	{
		this.immagineCasa = base.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject;
		this.descrCasa = base.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetChild(0).gameObject;
		this.dropdownCasa = base.transform.GetChild(0).GetChild(1).GetChild(2).gameObject;
		this.dropdownTurno = base.transform.GetChild(0).GetChild(1).GetChild(3).gameObject;
		this.dropdownAnno = base.transform.GetChild(0).GetChild(1).GetChild(4).gameObject;
		this.impostazioniComuni = base.transform.GetChild(0).GetChild(1).FindChild("impostazioni").gameObject;
		this.scrittaMaxNemici = this.impostazioniComuni.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
		this.scrittaMaxAlleati = this.impostazioniComuni.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
		this.impMaxNemici = this.impostazioniComuni.transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
		this.impMaxAlleati = this.impostazioniComuni.transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
		this.scrittaVitaNemici = this.impostazioniComuni.transform.GetChild(1).GetChild(0).gameObject;
		this.impVitaNemici = this.impostazioniComuni.transform.GetChild(1).GetChild(1).gameObject;
		this.scrittaAttaccoNemici = this.impostazioniComuni.transform.GetChild(2).GetChild(0).gameObject;
		this.impAttaccoNemici = this.impostazioniComuni.transform.GetChild(2).GetChild(1).gameObject;
		this.scrittaFatDiffFreshFood = this.impostazioniComuni.transform.GetChild(3).GetChild(0).gameObject;
		this.impFatDiffFreshFood = this.impostazioniComuni.transform.GetChild(3).GetChild(1).gameObject;
		this.scrittaFatDiffRottenFood = this.impostazioniComuni.transform.GetChild(4).GetChild(0).gameObject;
		this.impFatDiffRottenFood = this.impostazioniComuni.transform.GetChild(4).GetChild(1).gameObject;
		this.scrittaFatDiffHighProteinFood = this.impostazioniComuni.transform.GetChild(5).GetChild(0).gameObject;
		this.impFatDiffHighProteinFood = this.impostazioniComuni.transform.GetChild(5).GetChild(1).gameObject;
		this.impFatDiffSpawnGruppi = this.impostazioniComuni.transform.GetChild(6).GetChild(1).gameObject;
		this.impMaxNemici.GetComponent<Slider>().value = (float)PlayerPrefs.GetInt("max nemici");
		this.impMaxAlleati.GetComponent<Slider>().value = (float)PlayerPrefs.GetInt("max alleati");
		this.impVitaNemici.GetComponent<Slider>().value = PlayerPrefs.GetFloat("vita nemici");
		this.impAttaccoNemici.GetComponent<Slider>().value = PlayerPrefs.GetFloat("attacco nemici");
		this.impFatDiffFreshFood.GetComponent<Slider>().value = PlayerPrefs.GetFloat("fattore diff fresh food");
		this.impFatDiffRottenFood.GetComponent<Slider>().value = PlayerPrefs.GetFloat("fattore diff rotten food");
		this.impFatDiffHighProteinFood.GetComponent<Slider>().value = PlayerPrefs.GetFloat("fattore diff high protein food");
		this.impFatDiffSpawnGruppi.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("fattore diff spawn gruppi");
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x000F5538 File Offset: 0x000F3738
	private void Update()
	{
		if (this.creazCampAggCasa)
		{
			this.creazCampAggCasa = false;
			this.AggiornaMappa();
		}
		this.ImpostazioniComuni();
		if (this.creazCampIniziaCamp)
		{
			this.creazCampIniziaCamp = false;
			this.IniziaCampagna();
		}
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x000F557C File Offset: 0x000F377C
	private void AggiornaMappa()
	{
		int index = Mathf.FloorToInt((float)this.dropdownCasa.GetComponent<Dropdown>().value);
		this.immagineCasa.GetComponent<Image>().sprite = this.ListaCase[index].GetComponent<InfoCasa>().immagineCasa;
		this.descrCasa.GetComponent<Text>().text = this.ListaCase[index].GetComponent<InfoCasa>().oggDescrizioneCasa.GetComponent<Text>().text;
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x000F55F8 File Offset: 0x000F37F8
	private void ImpostazioniComuni()
	{
		this.impMaxAlleati.GetComponent<Slider>().value = this.impMaxNemici.GetComponent<Slider>().value / this.impMaxNemici.GetComponent<Slider>().maxValue * this.impMaxAlleati.GetComponent<Slider>().maxValue;
		int num = Mathf.FloorToInt(this.impMaxNemici.GetComponent<Slider>().value);
		int num2 = Mathf.FloorToInt(this.impMaxAlleati.GetComponent<Slider>().value);
		this.scrittaMaxNemici.GetComponent<Text>().text = "MAX ENEMIES ON SCREEN:  " + num.ToString();
		this.scrittaMaxAlleati.GetComponent<Text>().text = "MAX ALLIES ON SCREEN:  " + num2.ToString();
		float num3 = Mathf.Floor(this.impVitaNemici.GetComponent<Slider>().value);
		this.scrittaVitaNemici.GetComponent<Text>().text = "ENEMIES' HEALTH:  " + num3.ToString() + "%";
		float num4 = Mathf.Floor(this.impAttaccoNemici.GetComponent<Slider>().value);
		this.scrittaAttaccoNemici.GetComponent<Text>().text = "ENEMIES' ATTACK:  " + num4.ToString() + "%";
		float value = this.impFatDiffFreshFood.GetComponent<Slider>().value;
		this.scrittaFatDiffFreshFood.GetComponent<Text>().text = "FRESH FOOD ABUNDANCE:  x" + value.ToString("F2");
		float value2 = this.impFatDiffRottenFood.GetComponent<Slider>().value;
		this.scrittaFatDiffRottenFood.GetComponent<Text>().text = "ROTTEN FOOD ABUNDANCE:  x" + value2.ToString("F2");
		float value3 = this.impFatDiffHighProteinFood.GetComponent<Slider>().value;
		this.scrittaFatDiffHighProteinFood.GetComponent<Text>().text = "HIGH PROTEIN FOOD ABUNDANCE:  x" + value3.ToString("F2");
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x000F57D8 File Offset: 0x000F39D8
	private void IniziaCampagna()
	{
		this.impMaxAlleati.GetComponent<Slider>().value = this.impMaxNemici.GetComponent<Slider>().value / 1000f * 200f;
		int value = Mathf.FloorToInt(this.impMaxNemici.GetComponent<Slider>().value);
		int value2 = Mathf.FloorToInt(this.impMaxAlleati.GetComponent<Slider>().value);
		float value3 = Mathf.Floor(this.impVitaNemici.GetComponent<Slider>().value);
		float value4 = Mathf.Floor(this.impAttaccoNemici.GetComponent<Slider>().value);
		PlayerPrefs.SetInt("max nemici", value);
		PlayerPrefs.SetInt("max alleati", value2);
		PlayerPrefs.SetFloat("vita nemici", value3);
		PlayerPrefs.SetFloat("attacco nemici", value4);
		PlayerPrefs.SetFloat("fattore diff fresh food", this.impFatDiffFreshFood.GetComponent<Slider>().value);
		PlayerPrefs.SetFloat("fattore diff rotten food", this.impFatDiffRottenFood.GetComponent<Slider>().value);
		PlayerPrefs.SetFloat("fattore diff high protein food", this.impFatDiffHighProteinFood.GetComponent<Slider>().value);
		PlayerPrefs.SetInt("fattore diff spawn gruppi", this.impFatDiffSpawnGruppi.GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt("campagna frequenza turni", this.dropdownTurno.GetComponent<Dropdown>().value + 1);
		PlayerPrefs.SetInt("campagna anno partenza", this.dropdownAnno.GetComponent<Dropdown>().value + 2017);
		GestoreNeutroStrategia.campagnaAppenaIniziata = true;
		GestoreNeutroStrategia.stagione = 0;
		CaricaScene.nomeScenaDaCaricare = this.ListaCase[Mathf.FloorToInt((float)this.dropdownCasa.GetComponent<Dropdown>().value)].name;
		SceneManager.LoadScene("Scena Di Caricamento");
	}

	// Token: 0x04001976 RID: 6518
	public List<GameObject> ListaCase;

	// Token: 0x04001977 RID: 6519
	private GameObject immagineCasa;

	// Token: 0x04001978 RID: 6520
	private GameObject descrCasa;

	// Token: 0x04001979 RID: 6521
	private GameObject dropdownCasa;

	// Token: 0x0400197A RID: 6522
	private GameObject dropdownTurno;

	// Token: 0x0400197B RID: 6523
	private GameObject dropdownAnno;

	// Token: 0x0400197C RID: 6524
	private GameObject impostazioniComuni;

	// Token: 0x0400197D RID: 6525
	private GameObject scrittaMaxNemici;

	// Token: 0x0400197E RID: 6526
	private GameObject scrittaMaxAlleati;

	// Token: 0x0400197F RID: 6527
	private GameObject impMaxNemici;

	// Token: 0x04001980 RID: 6528
	private GameObject impMaxAlleati;

	// Token: 0x04001981 RID: 6529
	private GameObject scrittaVitaNemici;

	// Token: 0x04001982 RID: 6530
	private GameObject impVitaNemici;

	// Token: 0x04001983 RID: 6531
	private GameObject scrittaAttaccoNemici;

	// Token: 0x04001984 RID: 6532
	private GameObject impAttaccoNemici;

	// Token: 0x04001985 RID: 6533
	private GameObject scrittaFatDiffFreshFood;

	// Token: 0x04001986 RID: 6534
	private GameObject impFatDiffFreshFood;

	// Token: 0x04001987 RID: 6535
	private GameObject scrittaFatDiffRottenFood;

	// Token: 0x04001988 RID: 6536
	private GameObject impFatDiffRottenFood;

	// Token: 0x04001989 RID: 6537
	private GameObject scrittaFatDiffHighProteinFood;

	// Token: 0x0400198A RID: 6538
	private GameObject impFatDiffHighProteinFood;

	// Token: 0x0400198B RID: 6539
	private GameObject impFatDiffSpawnGruppi;

	// Token: 0x0400198C RID: 6540
	public bool creazCampAggCasa;

	// Token: 0x0400198D RID: 6541
	public bool creazCampIniziaCamp;
}
