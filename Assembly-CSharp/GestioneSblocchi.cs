using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000A RID: 10
public class GestioneSblocchi : MonoBehaviour
{
	// Token: 0x0600004B RID: 75 RVA: 0x00012138 File Offset: 0x00010338
	private void Start()
	{
		this.Schede = GameObject.FindGameObjectWithTag("Schede");
		this.scheda5 = this.Schede.transform.FindChild("scheda 5").gameObject;
		this.expDisponibile = this.scheda5.transform.FindChild("exp disponibile").GetChild(1).gameObject;
		this.sfondoInfoSblocchi = this.scheda5.transform.FindChild("sfondo info sblocchi").gameObject;
		this.costoSblocco = this.sfondoInfoSblocchi.transform.FindChild("costo sblocco").gameObject;
		this.pulsanteSblocca = this.sfondoInfoSblocchi.transform.FindChild("pulsante Sblocca").gameObject;
		this.sfondoAlberoSblocchi = this.scheda5.transform.FindChild("sfondo albero sblocchi").gameObject;
		this.pulsantiCategorie = this.scheda5.transform.FindChild("pulsanti categorie").gameObject;
		this.aggInfoSblocco = true;
		this.aggTuttiGliSblocchi = true;
		this.ListaSblocchi[0].GetComponent<PresenzaSblocco>().èSbloccato = 1;
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00012260 File Offset: 0x00010460
	private void Update()
	{
		if (this.Schede.transform.GetChild(6).name == "scheda 5")
		{
			this.expDisponibile.GetComponent<Text>().text = base.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[10].GetComponent<PresenzaRisorsa>().quantitàRisorsa.ToString("F0");
		}
		this.InfoSblocco();
		this.SbloccaElemento();
		this.AlberoSblocchiUI();
	}

	// Token: 0x0600004D RID: 77 RVA: 0x000122DC File Offset: 0x000104DC
	private void InfoSblocco()
	{
		if (this.aggInfoSblocco)
		{
			this.aggInfoSblocco = false;
			if (this.numeroSbloccoSel < 48)
			{
				this.sfondoInfoSblocchi.transform.GetChild(0).GetComponent<Text>().text = base.GetComponent<GestioneEsercitiAlleati>().ListaUnitàAlleate[this.numeroSbloccoSel].GetComponent<PresenzaAlleato>().nomeUnità;
				this.sfondoInfoSblocchi.transform.GetChild(1).GetComponent<Image>().sprite = base.GetComponent<GestioneEsercitiAlleati>().ListaUnitàAlleate[this.numeroSbloccoSel].GetComponent<PresenzaAlleato>().immagineUnità;
				this.sfondoInfoSblocchi.transform.GetChild(2).GetComponent<Text>().text = base.GetComponent<GestioneEsercitiAlleati>().ListaUnitàAlleate[this.numeroSbloccoSel].GetComponent<PresenzaAlleato>().oggettoDescrizione.GetComponent<Text>().text;
			}
			else
			{
				this.sfondoInfoSblocchi.transform.GetChild(0).GetComponent<Text>().text = this.ListaSblocchi[this.numeroSbloccoSel].GetComponent<PresenzaSblocco>().nomeSblocco;
				this.sfondoInfoSblocchi.transform.GetChild(1).GetComponent<Image>().sprite = this.ListaSblocchi[this.numeroSbloccoSel].GetComponent<PresenzaSblocco>().immagineSblocco;
				this.sfondoInfoSblocchi.transform.GetChild(2).GetComponent<Text>().text = this.ListaSblocchi[this.numeroSbloccoSel].GetComponent<Text>().text;
			}
			int num = 0;
			if (base.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[10].GetComponent<PresenzaRisorsa>().quantitàRisorsa >= this.ListaSblocchi[this.numeroSbloccoSel].GetComponent<PresenzaSblocco>().costoInExp)
			{
				for (int i = 0; i < this.ListaSblocchi[this.numeroSbloccoSel].GetComponent<PresenzaSblocco>().ListaSblocchiCheBloccano.Count; i++)
				{
					if (this.ListaSblocchi[this.ListaSblocchi[this.numeroSbloccoSel].GetComponent<PresenzaSblocco>().ListaSblocchiCheBloccano[i]].GetComponent<PresenzaSblocco>().èSbloccato == 1)
					{
						num++;
					}
				}
			}
			if (this.ListaSblocchi[this.numeroSbloccoSel].GetComponent<PresenzaSblocco>().èSbloccato == 1)
			{
				this.costoSblocco.GetComponent<CanvasGroup>().alpha = 0f;
				this.pulsanteSblocca.GetComponent<CanvasGroup>().alpha = 0f;
				this.pulsanteSblocca.GetComponent<CanvasGroup>().interactable = false;
				this.pulsanteSblocca.GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
			else if (num == this.ListaSblocchi[this.numeroSbloccoSel].GetComponent<PresenzaSblocco>().ListaSblocchiCheBloccano.Count)
			{
				this.costoSblocco.GetComponent<CanvasGroup>().alpha = 1f;
				this.costoSblocco.transform.GetChild(1).GetComponent<Text>().text = this.ListaSblocchi[this.numeroSbloccoSel].GetComponent<PresenzaSblocco>().costoInExp.ToString("F0");
				this.pulsanteSblocca.GetComponent<CanvasGroup>().alpha = 1f;
				this.pulsanteSblocca.GetComponent<CanvasGroup>().interactable = true;
				this.pulsanteSblocca.GetComponent<CanvasGroup>().blocksRaycasts = true;
			}
			else
			{
				this.costoSblocco.GetComponent<CanvasGroup>().alpha = 1f;
				this.costoSblocco.transform.GetChild(1).GetComponent<Text>().text = this.ListaSblocchi[this.numeroSbloccoSel].GetComponent<PresenzaSblocco>().costoInExp.ToString("F0");
				this.pulsanteSblocca.GetComponent<CanvasGroup>().alpha = 0f;
				this.pulsanteSblocca.GetComponent<CanvasGroup>().interactable = false;
				this.pulsanteSblocca.GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
		}
	}

	// Token: 0x0600004E RID: 78 RVA: 0x000126D0 File Offset: 0x000108D0
	private void SbloccaElemento()
	{
		if (this.eseguiSblocco)
		{
			this.eseguiSblocco = false;
			this.ListaSblocchi[this.numeroSbloccoSel].GetComponent<PresenzaSblocco>().èSbloccato = 1;
			this.aggTuttiGliSblocchi = true;
			this.aggInfoSblocco = true;
			base.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[10].GetComponent<PresenzaRisorsa>().quantitàRisorsa -= this.ListaSblocchi[this.numeroSbloccoSel].GetComponent<PresenzaSblocco>().costoInExp;
		}
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00012758 File Offset: 0x00010958
	private void AlberoSblocchiUI()
	{
		if (this.aggTuttiGliSblocchi)
		{
			this.aggTuttiGliSblocchi = false;
			for (int i = 0; i < 5; i++)
			{
				if (i == this.categoriaDiSblocchi)
				{
					this.sfondoAlberoSblocchi.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 1f;
					this.sfondoAlberoSblocchi.transform.GetChild(i).GetComponent<CanvasGroup>().interactable = true;
					this.sfondoAlberoSblocchi.transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = true;
					this.pulsantiCategorie.transform.GetChild(i).GetComponent<Image>().color = this.coloreSelezCategorie;
				}
				else
				{
					this.sfondoAlberoSblocchi.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 0f;
					this.sfondoAlberoSblocchi.transform.GetChild(i).GetComponent<CanvasGroup>().interactable = false;
					this.sfondoAlberoSblocchi.transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = false;
					this.pulsantiCategorie.transform.GetChild(i).GetComponent<Image>().color = Color.white;
				}
			}
			for (int j = 1; j < this.sfondoAlberoSblocchi.transform.GetChild(this.categoriaDiSblocchi).childCount; j++)
			{
				if (this.ListaSblocchi[int.Parse(this.sfondoAlberoSblocchi.transform.GetChild(this.categoriaDiSblocchi).GetChild(j).name)].GetComponent<PresenzaSblocco>().èSbloccato == 1)
				{
					this.sfondoAlberoSblocchi.transform.GetChild(this.categoriaDiSblocchi).GetChild(j).GetChild(0).GetComponent<Image>().color = Color.black;
				}
			}
		}
	}

	// Token: 0x040001A3 RID: 419
	private GameObject Schede;

	// Token: 0x040001A4 RID: 420
	private GameObject scheda5;

	// Token: 0x040001A5 RID: 421
	private GameObject expDisponibile;

	// Token: 0x040001A6 RID: 422
	private GameObject sfondoInfoSblocchi;

	// Token: 0x040001A7 RID: 423
	private GameObject costoSblocco;

	// Token: 0x040001A8 RID: 424
	private GameObject pulsanteSblocca;

	// Token: 0x040001A9 RID: 425
	private GameObject sfondoAlberoSblocchi;

	// Token: 0x040001AA RID: 426
	private GameObject pulsantiCategorie;

	// Token: 0x040001AB RID: 427
	public bool aggInfoSblocco;

	// Token: 0x040001AC RID: 428
	public bool aggTuttiGliSblocchi;

	// Token: 0x040001AD RID: 429
	public int numeroSbloccoSel;

	// Token: 0x040001AE RID: 430
	public bool eseguiSblocco;

	// Token: 0x040001AF RID: 431
	public int categoriaDiSblocchi;

	// Token: 0x040001B0 RID: 432
	public List<GameObject> ListaSblocchi;

	// Token: 0x040001B1 RID: 433
	public Color coloreSelezCategorie;
}
