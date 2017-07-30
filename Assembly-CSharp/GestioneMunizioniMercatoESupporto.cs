using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000008 RID: 8
public class GestioneMunizioniMercatoESupporto : MonoBehaviour
{
	// Token: 0x06000039 RID: 57 RVA: 0x0000EAB4 File Offset: 0x0000CCB4
	private void Start()
	{
		this.headquarters = GameObject.FindGameObjectWithTag("Headquarters");
		this.Schede = GameObject.FindGameObjectWithTag("Schede");
		this.PulsanteFittStrategia = GameObject.FindGameObjectWithTag("PulsFittStrategia");
		this.elencoMunizioni = this.Schede.transform.FindChild("scheda 4").FindChild("Elenco Munizioni").gameObject;
		this.quadroInteratMuniz = this.Schede.transform.FindChild("scheda 4").FindChild("Quadro Interattivo").gameObject;
		this.scrittaNomeMuniz = this.quadroInteratMuniz.transform.FindChild("nome munizione").gameObject;
		this.scrittaQuantitàMuniz = this.quadroInteratMuniz.transform.FindChild("sfondo quantità").GetChild(0).gameObject;
		this.primoCostoSingolo = this.quadroInteratMuniz.transform.FindChild("primo costo singolo").gameObject;
		this.secondoCostoSingolo = this.quadroInteratMuniz.transform.FindChild("secondo costo singolo").gameObject;
		this.primoCostoTotale = this.quadroInteratMuniz.transform.FindChild("primo costo totale").gameObject;
		this.secondoCostoTotale = this.quadroInteratMuniz.transform.FindChild("secondo costo totale").gameObject;
		this.elencoRisorsePres = this.Schede.transform.FindChild("scheda 6").FindChild("colonna risorse presenti").gameObject;
		this.elencoValoriRis = this.Schede.transform.FindChild("scheda 6").FindChild("colonna valori").gameObject;
		this.elencoVendite = this.Schede.transform.FindChild("scheda 6").FindChild("colonna vendita").gameObject;
		this.elencoAcquisti = this.Schede.transform.FindChild("scheda 6").FindChild("colonna acquisto").gameObject;
		this.valoreDiScambioScritta = this.Schede.transform.FindChild("scheda 6").FindChild("valore di scambio").GetChild(1).gameObject;
		this.scheda7 = this.Schede.transform.FindChild("scheda 7").gameObject;
		this.bloccoSupporto = this.scheda7.transform.FindChild("blocco supporto").gameObject;
		this.aggiornaMunizioniPres = true;
		this.aggTuttiElenchiRisorse = true;
		for (int i = 0; i < 10; i++)
		{
			this.elencoValoriRis.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = this.ListaRapportiRisorse[i].ToString();
		}
		this.ListaVenditeTemp = new List<float>();
		this.ListaAcquistiTemp = new List<float>();
		for (int j = 0; j < 10; j++)
		{
			this.ListaVenditeTemp.Add(0f);
			this.ListaAcquistiTemp.Add(0f);
		}
		this.ListaElenchiSupporto = new List<GameObject>();
		this.ListaElenchiSupporto.Add(this.scheda7.transform.GetChild(1).GetChild(3).gameObject);
		this.ListaElenchiSupporto.Add(this.scheda7.transform.GetChild(2).GetChild(3).gameObject);
		this.ListaElenchiSupporto.Add(this.scheda7.transform.GetChild(3).GetChild(3).gameObject);
		this.ListaElenchiSupporto.Add(this.scheda7.transform.GetChild(4).GetChild(3).gameObject);
		this.ListaElenchiSupporto.Add(this.scheda7.transform.GetChild(5).GetChild(3).gameObject);
		this.ListaElenchiSupporto.Add(this.scheda7.transform.GetChild(6).GetChild(3).gameObject);
		this.ListaScritteCostiSupporto = new List<GameObject>();
		this.ListaScritteCostiSupporto.Add(this.scheda7.transform.GetChild(1).GetChild(4).gameObject);
		this.ListaScritteCostiSupporto.Add(this.scheda7.transform.GetChild(2).GetChild(4).gameObject);
		this.ListaScritteCostiSupporto.Add(this.scheda7.transform.GetChild(3).GetChild(4).gameObject);
		this.ListaScritteCostiSupporto.Add(this.scheda7.transform.GetChild(4).GetChild(4).gameObject);
		this.ListaScritteCostiSupporto.Add(this.scheda7.transform.GetChild(5).GetChild(4).gameObject);
		this.ListaScritteCostiSupporto.Add(this.scheda7.transform.GetChild(6).GetChild(4).gameObject);
		this.ListaCostiSupporto = new List<float>();
		this.ListaCostiSupporto.Add(15f);
		this.ListaCostiSupporto.Add(15f);
		this.ListaCostiSupporto.Add(30f);
		this.ListaCostiSupporto.Add(200f);
		this.ListaCostiSupporto.Add(40f);
		this.ListaCostiSupporto.Add(80f);
	}

	// Token: 0x0600003A RID: 58 RVA: 0x0000F018 File Offset: 0x0000D218
	private void Update()
	{
		if (this.PulsanteFittStrategia.GetComponent<PulsFitPerStrategia>().schedaAperta == 3)
		{
			this.AcquistoMunizioni();
		}
		else if (this.PulsanteFittStrategia.GetComponent<PulsFitPerStrategia>().schedaAperta == 5)
		{
			this.FunzioneMercato();
		}
		else if (this.PulsanteFittStrategia.GetComponent<PulsFitPerStrategia>().schedaAperta == 6)
		{
			if (base.GetComponent<GestioneSblocchi>().ListaSblocchi[10].GetComponent<PresenzaSblocco>().èSbloccato == 1)
			{
				this.bloccoSupporto.GetComponent<CanvasGroup>().alpha = 0f;
				this.bloccoSupporto.GetComponent<CanvasGroup>().blocksRaycasts = false;
				this.FunzioneSupporto();
			}
			else
			{
				this.bloccoSupporto.GetComponent<CanvasGroup>().alpha = 1f;
				this.bloccoSupporto.GetComponent<CanvasGroup>().blocksRaycasts = true;
			}
		}
	}

	// Token: 0x0600003B RID: 59 RVA: 0x0000F0F8 File Offset: 0x0000D2F8
	private void AcquistoMunizioni()
	{
		if (this.aggiornaMunizioniPres)
		{
			this.aggiornaMunizioniPres = false;
			this.munizioneConsiderata = this.ListaTipiMunizioniBaseStrategia[this.numMunizSelez];
			if (this.aggiungiMuniz)
			{
				this.aggiungiMuniz = false;
				this.munizioniInSospeso += this.quantitàMunizDaAggiungere;
			}
			this.scrittaNomeMuniz.GetComponent<Text>().text = this.munizioneConsiderata.name;
			this.scrittaQuantitàMuniz.GetComponent<Text>().text = this.munizioniInSospeso.ToString("F0");
			this.primoCostoSingolo.transform.GetChild(0).GetComponent<Image>().enabled = true;
			this.primoCostoSingolo.transform.GetChild(0).GetComponent<Image>().sprite = this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo1TipoRisorsa].GetComponent<PresenzaRisorsa>().immagineRisorsa;
			this.primoCostoSingolo.transform.GetChild(1).GetComponent<Text>().text = this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo1PrezzoSingolo.ToString("G");
			this.primoCostoTotale.transform.GetChild(0).GetComponent<Image>().enabled = true;
			this.primoCostoTotale.transform.GetChild(0).GetComponent<Image>().sprite = this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo1TipoRisorsa].GetComponent<PresenzaRisorsa>().immagineRisorsa;
			this.primoCostoTotale.transform.GetChild(1).GetComponent<Text>().text = (this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo1PrezzoSingolo * this.munizioniInSospeso).ToString("G6");
			if (this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo2PrezzoSingolo != 0f)
			{
				this.secondoCostoSingolo.GetComponent<CanvasGroup>().alpha = 1f;
				this.secondoCostoSingolo.transform.GetChild(0).GetComponent<Image>().sprite = this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo2TipoRisorsa].GetComponent<PresenzaRisorsa>().immagineRisorsa;
				this.secondoCostoSingolo.transform.GetChild(1).GetComponent<Text>().text = this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo2PrezzoSingolo.ToString("G");
				this.secondoCostoTotale.GetComponent<CanvasGroup>().alpha = 1f;
				this.secondoCostoTotale.transform.GetChild(0).GetComponent<Image>().sprite = this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo2TipoRisorsa].GetComponent<PresenzaRisorsa>().immagineRisorsa;
				this.secondoCostoTotale.transform.GetChild(1).GetComponent<Text>().text = (this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo2PrezzoSingolo * this.munizioniInSospeso).ToString("G6");
			}
			else
			{
				this.secondoCostoSingolo.GetComponent<CanvasGroup>().alpha = 0f;
				this.secondoCostoTotale.GetComponent<CanvasGroup>().alpha = 0f;
			}
			if (this.compra)
			{
				this.compra = false;
				if (this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo1PrezzoSingolo * this.munizioniInSospeso <= this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo1TipoRisorsa].GetComponent<PresenzaRisorsa>().quantitàRisorsa)
				{
					if (this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo2PrezzoSingolo != 0f)
					{
						if (this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo2PrezzoSingolo * this.munizioniInSospeso <= this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo2TipoRisorsa].GetComponent<PresenzaRisorsa>().quantitàRisorsa)
						{
							this.ListaTipiMunizioniBaseStrategia[this.numMunizSelez].GetComponent<QuantitàMunizione>().quantità += this.munizioniInSospeso;
							this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo1TipoRisorsa].GetComponent<PresenzaRisorsa>().quantitàRisorsa -= this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo1PrezzoSingolo * this.munizioniInSospeso;
							this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo2TipoRisorsa].GetComponent<PresenzaRisorsa>().quantitàRisorsa -= this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo2PrezzoSingolo * this.munizioniInSospeso;
						}
					}
					else
					{
						this.ListaTipiMunizioniBaseStrategia[this.numMunizSelez].GetComponent<QuantitàMunizione>().quantità += this.munizioniInSospeso;
						this.headquarters.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo1TipoRisorsa].GetComponent<PresenzaRisorsa>().quantitàRisorsa -= this.munizioneConsiderata.GetComponent<QuantitàMunizione>().costo1PrezzoSingolo * this.munizioniInSospeso;
					}
				}
			}
			for (int i = 0; i < this.elencoMunizioni.transform.childCount; i++)
			{
				this.elencoMunizioni.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = this.ListaTipiMunizioniBaseStrategia[i].GetComponent<QuantitàMunizione>().quantità.ToString("F0");
			}
		}
	}

	// Token: 0x0600003C RID: 60 RVA: 0x0000F6B0 File Offset: 0x0000D8B0
	private void FunzioneMercato()
	{
		if (this.aggTuttiElenchiRisorse)
		{
			this.aggTuttiElenchiRisorse = false;
			if (this.vendiPremuto)
			{
				this.vendiPremuto = false;
				float num = 100f;
				if (Input.GetKey(KeyCode.LeftAlt))
				{
					num = 1f;
				}
				else if (Input.GetKey(KeyCode.LeftControl))
				{
					num = 10f;
				}
				else if (Input.GetKey(KeyCode.LeftShift))
				{
					num = 1000f;
				}
				if (base.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[this.tipoRisorsaSelez].GetComponent<PresenzaRisorsa>().quantitàRisorsa >= num + this.ListaVenditeTemp[this.tipoRisorsaSelez] - this.ListaAcquistiTemp[this.tipoRisorsaSelez])
				{
					List<float> listaVenditeTemp;
					List<float> expr_C3 = listaVenditeTemp = this.ListaVenditeTemp;
					int index;
					int expr_CC = index = this.tipoRisorsaSelez;
					float num2 = listaVenditeTemp[index];
					expr_C3[expr_CC] = num2 + num;
					this.valoreScambio += num * this.ListaRapportiRisorse[this.tipoRisorsaSelez];
				}
			}
			if (this.compraPremuto)
			{
				this.compraPremuto = false;
				float num3 = 100f;
				if (Input.GetKey(KeyCode.LeftAlt))
				{
					num3 = 1f;
				}
				else if (Input.GetKey(KeyCode.LeftControl))
				{
					num3 = 10f;
				}
				else if (Input.GetKey(KeyCode.LeftShift))
				{
					num3 = 1000f;
				}
				if (this.valoreScambio >= num3 * this.ListaRapportiRisorse[this.tipoRisorsaSelez] * (1f + this.percRincaroAcquisto))
				{
					List<float> listaAcquistiTemp;
					List<float> expr_195 = listaAcquistiTemp = this.ListaAcquistiTemp;
					int index;
					int expr_19E = index = this.tipoRisorsaSelez;
					float num2 = listaAcquistiTemp[index];
					expr_195[expr_19E] = num2 + num3;
					this.valoreScambio -= num3 * this.ListaRapportiRisorse[this.tipoRisorsaSelez] * (1f + this.percRincaroAcquisto);
				}
			}
			if (this.eseguiScambioPremuto)
			{
				this.eseguiScambioPremuto = false;
				this.resettaPremuto = true;
				for (int i = 0; i < 10; i++)
				{
					base.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[i].GetComponent<PresenzaRisorsa>().quantitàRisorsa -= this.ListaVenditeTemp[i];
					base.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[i].GetComponent<PresenzaRisorsa>().quantitàRisorsa += this.ListaAcquistiTemp[i];
				}
			}
			if (this.resettaPremuto)
			{
				this.resettaPremuto = false;
				for (int j = 0; j < 10; j++)
				{
					this.ListaVenditeTemp[j] = 0f;
					this.ListaAcquistiTemp[j] = 0f;
					this.valoreScambio = 0f;
				}
			}
			for (int k = 0; k < 10; k++)
			{
				this.elencoRisorsePres.transform.GetChild(k).GetChild(1).GetComponent<Text>().text = base.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[k].GetComponent<PresenzaRisorsa>().quantitàRisorsa.ToString("F0");
				this.elencoVendite.transform.GetChild(k).GetChild(0).GetComponent<Text>().text = this.ListaVenditeTemp[k].ToString("F0");
				this.elencoAcquisti.transform.GetChild(k).GetChild(0).GetComponent<Text>().text = this.ListaAcquistiTemp[k].ToString("F0");
				this.valoreDiScambioScritta.GetComponent<Text>().text = this.valoreScambio.ToString("F0");
				if (k == this.tipoRisorsaSelez)
				{
					this.elencoRisorsePres.transform.GetChild(k).GetComponent<Image>().color = this.coloreRisorsaSelez;
				}
				else
				{
					this.elencoRisorsePres.transform.GetChild(k).GetComponent<Image>().color = Color.white;
				}
			}
		}
	}

	// Token: 0x0600003D RID: 61 RVA: 0x0000FAC8 File Offset: 0x0000DCC8
	private void FunzioneSupporto()
	{
		if (this.aggSupporto)
		{
			this.aggSupporto = false;
			if (this.supportoComprato && this.ListaQuantitàSupporto[this.numSupporto] < 8)
			{
				this.supportoComprato = false;
				List<int> listaQuantitàSupporto;
				List<int> expr_41 = listaQuantitàSupporto = this.ListaQuantitàSupporto;
				int num;
				int expr_49 = num = this.numSupporto;
				num = listaQuantitàSupporto[num];
				expr_41[expr_49] = num + 1;
				base.GetComponent<GestioneRisorseEHeadquartiers>().ListaRisorsePresenti[1].GetComponent<PresenzaRisorsa>().quantitàRisorsa -= this.ListaCostiSupporto[this.numSupporto];
			}
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if (j >= this.ListaQuantitàSupporto[i])
					{
						this.ListaElenchiSupporto[i].transform.GetChild(j).GetComponent<CanvasGroup>().alpha = 0.4f;
					}
					else
					{
						this.ListaElenchiSupporto[i].transform.GetChild(j).GetComponent<CanvasGroup>().alpha = 1f;
					}
				}
				this.ListaScritteCostiSupporto[i].transform.GetChild(0).GetComponent<Text>().text = this.ListaCostiSupporto[i].ToString();
			}
		}
	}

	// Token: 0x04000129 RID: 297
	private GameObject headquarters;

	// Token: 0x0400012A RID: 298
	private GameObject Schede;

	// Token: 0x0400012B RID: 299
	private GameObject PulsanteFittStrategia;

	// Token: 0x0400012C RID: 300
	private GameObject elencoMunizioni;

	// Token: 0x0400012D RID: 301
	private GameObject quadroInteratMuniz;

	// Token: 0x0400012E RID: 302
	private GameObject scrittaNomeMuniz;

	// Token: 0x0400012F RID: 303
	private GameObject scrittaQuantitàMuniz;

	// Token: 0x04000130 RID: 304
	private GameObject primoCostoSingolo;

	// Token: 0x04000131 RID: 305
	private GameObject secondoCostoSingolo;

	// Token: 0x04000132 RID: 306
	private GameObject primoCostoTotale;

	// Token: 0x04000133 RID: 307
	private GameObject secondoCostoTotale;

	// Token: 0x04000134 RID: 308
	private GameObject bloccoSupporto;

	// Token: 0x04000135 RID: 309
	public List<GameObject> ListaTipiMunizioniBaseStrategia;

	// Token: 0x04000136 RID: 310
	public List<float> ListaRapportiValoreArmi;

	// Token: 0x04000137 RID: 311
	public bool aggiornaMunizioniPres;

	// Token: 0x04000138 RID: 312
	public int numMunizSelez;

	// Token: 0x04000139 RID: 313
	public float quantitàMunizDaAggiungere;

	// Token: 0x0400013A RID: 314
	public float munizioniInSospeso;

	// Token: 0x0400013B RID: 315
	public bool aggiungiMuniz;

	// Token: 0x0400013C RID: 316
	private GameObject munizioneConsiderata;

	// Token: 0x0400013D RID: 317
	public bool compra;

	// Token: 0x0400013E RID: 318
	public Color coloreRisorsaSelez;

	// Token: 0x0400013F RID: 319
	private GameObject elencoRisorsePres;

	// Token: 0x04000140 RID: 320
	private GameObject elencoValoriRis;

	// Token: 0x04000141 RID: 321
	private GameObject elencoVendite;

	// Token: 0x04000142 RID: 322
	private GameObject elencoAcquisti;

	// Token: 0x04000143 RID: 323
	private GameObject valoreDiScambioScritta;

	// Token: 0x04000144 RID: 324
	public List<float> ListaRapportiRisorse;

	// Token: 0x04000145 RID: 325
	public float percRincaroAcquisto;

	// Token: 0x04000146 RID: 326
	private List<float> ListaVenditeTemp;

	// Token: 0x04000147 RID: 327
	private List<float> ListaAcquistiTemp;

	// Token: 0x04000148 RID: 328
	private float valoreScambio;

	// Token: 0x04000149 RID: 329
	public bool vendiPremuto;

	// Token: 0x0400014A RID: 330
	public bool compraPremuto;

	// Token: 0x0400014B RID: 331
	public bool resettaPremuto;

	// Token: 0x0400014C RID: 332
	public bool eseguiScambioPremuto;

	// Token: 0x0400014D RID: 333
	public bool aggTuttiElenchiRisorse;

	// Token: 0x0400014E RID: 334
	public int tipoRisorsaSelez;

	// Token: 0x0400014F RID: 335
	private GameObject scheda7;

	// Token: 0x04000150 RID: 336
	private List<GameObject> ListaElenchiSupporto;

	// Token: 0x04000151 RID: 337
	public List<int> ListaQuantitàSupporto;

	// Token: 0x04000152 RID: 338
	private List<GameObject> ListaScritteCostiSupporto;

	// Token: 0x04000153 RID: 339
	private List<float> ListaCostiSupporto;

	// Token: 0x04000154 RID: 340
	public bool aggSupporto;

	// Token: 0x04000155 RID: 341
	public int numSupporto;

	// Token: 0x04000156 RID: 342
	public bool supportoComprato;
}
