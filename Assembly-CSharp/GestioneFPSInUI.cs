using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000007 RID: 7
public class GestioneFPSInUI : MonoBehaviour
{
	// Token: 0x06000032 RID: 50 RVA: 0x0000DB78 File Offset: 0x0000BD78
	private void Start()
	{
		this.CanvasFPS = GameObject.FindGameObjectWithTag("CanvasFPS");
		this.Arma1 = this.CanvasFPS.transform.GetChild(3).transform.GetChild(0).gameObject;
		this.Arma2 = this.CanvasFPS.transform.GetChild(3).transform.GetChild(1).gameObject;
		this.Arma3 = this.CanvasFPS.transform.GetChild(3).transform.GetChild(2).gameObject;
		this.Arma4 = this.CanvasFPS.transform.GetChild(3).transform.GetChild(3).gameObject;
		this.primaCamera = GameObject.FindGameObjectWithTag("MainCamera");
		this.terzaCamera = GameObject.FindGameObjectWithTag("Terza Camera");
		this.vitaPerFPS = this.CanvasFPS.transform.GetChild(5).gameObject;
		this.nomeUnità = this.CanvasFPS.transform.GetChild(5).GetChild(0).gameObject;
		this.barraVita = this.CanvasFPS.transform.GetChild(5).GetChild(1).GetChild(0).gameObject;
		this.numeriVita = this.CanvasFPS.transform.GetChild(5).GetChild(2).gameObject;
		this.carburantePerFPS = this.CanvasFPS.transform.FindChild("Carburante").gameObject;
		this.barraCarburante = this.CanvasFPS.transform.FindChild("Carburante").FindChild("sfondo barra carburante").GetChild(0).gameObject;
		this.numeriCarburante = this.CanvasFPS.transform.FindChild("Carburante").FindChild("carburante in numeri").gameObject;
		this.silhouetteVelivoli = this.CanvasFPS.transform.FindChild("Silhouette velivoli").GetChild(0).gameObject;
		this.immagineAvvelenato = this.CanvasFPS.transform.GetChild(5).FindChild("immagine avvelenato").gameObject;
		this.ListaArmiUIFPS = new List<GameObject>();
		this.ListaArmiUIFPS.Add(this.Arma1);
		this.ListaArmiUIFPS.Add(this.Arma2);
		this.ListaArmiUIFPS.Add(this.Arma3);
		this.ListaArmiUIFPS.Add(this.Arma4);
	}

	// Token: 0x06000033 RID: 51 RVA: 0x0000DDF0 File Offset: 0x0000BFF0
	private void Update()
	{
		if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva == 3)
		{
			this.unitàInFPS = this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera;
			if (this.unitàInFPS)
			{
				this.QuadratiInfoArmi();
				this.VitaInFPS();
				if (this.unitàInFPS.GetComponent<PresenzaAlleato>().èAereo && this.unitàInFPS.GetComponent<PresenzaAlleato>().tipoTruppaVolante != 8 && this.unitàInFPS.GetComponent<PresenzaAlleato>().tipoTruppaVolante != 10 && this.unitàInFPS.GetComponent<PresenzaAlleato>().tipoTruppaVolante != 14)
				{
					this.CarburanteInFPS();
					this.GestioneSilhouetteAerei();
				}
				else
				{
					this.carburantePerFPS.transform.GetComponent<CanvasGroup>().alpha = 0f;
					this.silhouetteVelivoli.GetComponent<CanvasGroup>().alpha = 0f;
				}
			}
		}
		else
		{
			this.Arma1.transform.GetComponent<CanvasGroup>().alpha = 0f;
			this.Arma2.transform.GetComponent<CanvasGroup>().alpha = 0f;
			this.Arma3.transform.GetComponent<CanvasGroup>().alpha = 0f;
			this.Arma4.transform.GetComponent<CanvasGroup>().alpha = 0f;
			this.vitaPerFPS.transform.GetComponent<CanvasGroup>().alpha = 0f;
			this.carburantePerFPS.transform.GetComponent<CanvasGroup>().alpha = 0f;
			this.silhouetteVelivoli.GetComponent<CanvasGroup>().alpha = 0f;
		}
	}

	// Token: 0x06000034 RID: 52 RVA: 0x0000DF94 File Offset: 0x0000C194
	private void VitaInFPS()
	{
		this.vitaPerFPS.transform.GetComponent<CanvasGroup>().alpha = 1f;
		this.nomeUnità.GetComponent<Text>().text = this.unitàInFPS.GetComponent<PresenzaAlleato>().nomeUnità;
		this.barraVita.GetComponent<Image>().fillAmount = this.unitàInFPS.GetComponent<PresenzaAlleato>().vita / this.unitàInFPS.GetComponent<PresenzaAlleato>().vitaIniziale;
		this.numeriVita.GetComponent<Text>().text = this.unitàInFPS.GetComponent<PresenzaAlleato>().vita.ToString("F0") + " / " + this.unitàInFPS.GetComponent<PresenzaAlleato>().vitaIniziale.ToString("F0");
		if (this.unitàInFPS.GetComponent<PresenzaAlleato>().ListaAvvelenamento[0][0] != 0f)
		{
			this.immagineAvvelenato.GetComponent<CanvasGroup>().alpha = 1f;
		}
		else
		{
			this.immagineAvvelenato.GetComponent<CanvasGroup>().alpha = 0f;
		}
	}

	// Token: 0x06000035 RID: 53 RVA: 0x0000E0B0 File Offset: 0x0000C2B0
	private void CarburanteInFPS()
	{
		this.carburantePerFPS.transform.GetComponent<CanvasGroup>().alpha = 1f;
		this.barraCarburante.GetComponent<Image>().fillAmount = this.unitàInFPS.GetComponent<PresenzaAlleato>().carburante / this.unitàInFPS.GetComponent<PresenzaAlleato>().carburanteIniziale;
		this.numeriCarburante.GetComponent<Text>().text = this.unitàInFPS.GetComponent<PresenzaAlleato>().carburante.ToString("F0") + " / " + this.unitàInFPS.GetComponent<PresenzaAlleato>().carburanteIniziale.ToString("F0");
	}

	// Token: 0x06000036 RID: 54 RVA: 0x0000E158 File Offset: 0x0000C358
	private void QuadratiInfoArmi()
	{
		for (int i = 0; i < 4; i++)
		{
			if (!this.unitàInFPS.GetComponent<PresenzaAlleato>().èPerRifornimento)
			{
				if (i < this.unitàInFPS.GetComponent<PresenzaAlleato>().numeroArmi)
				{
					this.ListaArmiUIFPS[i].transform.GetComponent<CanvasGroup>().alpha = 1f;
					this.ListaArmiUIFPS[i].transform.GetChild(0).GetComponent<Text>().text = this.unitàInFPS.GetComponent<PresenzaAlleato>().ListaNomiArmi[i];
					this.ListaArmiUIFPS[i].transform.GetChild(1).GetComponent<Text>().text = this.unitàInFPS.GetComponent<PresenzaAlleato>().ListaArmi[i][5].ToString() + "  / " + this.unitàInFPS.GetComponent<PresenzaAlleato>().ListaArmi[i][6].ToString();
					if (!this.unitàInFPS.GetComponent<PresenzaAlleato>().ListaRicaricheInCorso[i])
					{
						if (!this.unitàInFPS.GetComponent<PresenzaAlleato>().ListaFuoriPortataArmi[i])
						{
							this.ListaArmiUIFPS[i].transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0f;
						}
						else
						{
							this.ListaArmiUIFPS[i].transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 1f;
							this.ListaArmiUIFPS[i].transform.GetChild(2).GetComponent<Text>().color = this.scrittaRossa;
							this.ListaArmiUIFPS[i].transform.GetChild(2).GetComponent<Text>().text = "OUT OF REACH";
						}
					}
					else
					{
						this.ListaArmiUIFPS[i].transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 1f;
						this.ListaArmiUIFPS[i].transform.GetChild(2).GetComponent<Text>().color = this.scrittaVerde;
						this.ListaArmiUIFPS[i].transform.GetChild(2).GetComponent<Text>().text = "RELOADING...";
					}
				}
				else
				{
					this.ListaArmiUIFPS[i].GetComponent<CanvasGroup>().alpha = 0f;
				}
			}
			else
			{
				this.Arma1.transform.GetComponent<CanvasGroup>().alpha = 1f;
				this.Arma1.transform.GetChild(0).GetComponent<Text>().text = "AMMO SUPPLIES";
				this.Arma1.transform.GetChild(1).GetComponent<Text>().text = "Supplies Remaining: " + this.unitàInFPS.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp.ToString() + "/" + this.unitàInFPS.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp.ToString();
				this.Arma1.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0f;
				this.Arma2.transform.GetComponent<CanvasGroup>().alpha = 1f;
				this.Arma2.transform.GetChild(0).GetComponent<Text>().text = "REPAIR SUPPLIES";
				this.Arma2.transform.GetChild(1).GetComponent<Text>().text = "Supplies Remaining: " + this.unitàInFPS.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp.ToString() + "/" + this.unitàInFPS.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp.ToString();
				this.Arma2.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0f;
				this.Arma3.transform.GetComponent<CanvasGroup>().alpha = 0f;
				this.Arma4.transform.GetComponent<CanvasGroup>().alpha = 0f;
			}
		}
		if (!this.unitàInFPS.GetComponent<PresenzaAlleato>().èPerRifornimento)
		{
			for (int j = 0; j < this.unitàInFPS.GetComponent<PresenzaAlleato>().numeroArmi; j++)
			{
				if (this.unitàInFPS.GetComponent<PresenzaAlleato>().armaAttivaInFPS == j)
				{
					this.ListaArmiUIFPS[j].GetComponent<Image>().color = this.armaEvidenziata;
				}
				else
				{
					this.ListaArmiUIFPS[j].GetComponent<Image>().color = this.armaNonEvidenziata;
				}
			}
		}
	}

	// Token: 0x06000037 RID: 55 RVA: 0x0000E5F8 File Offset: 0x0000C7F8
	private void GestioneSilhouetteAerei()
	{
		if (Input.GetKeyUp(KeyCode.Q))
		{
			this.silhouetteVelivoli.GetComponent<CanvasGroup>().alpha = 1f;
			if (this.unitàInFPS.GetComponent<PresenzaAlleato>().tipoTruppaVolante == 2)
			{
				this.silhouetteVelivoli.GetComponent<Image>().sprite = this.aereoFighter;
			}
			else if (this.unitàInFPS.GetComponent<PresenzaAlleato>().tipoTruppaVolante == 3)
			{
				this.silhouetteVelivoli.GetComponent<Image>().sprite = this.aereoGroundAttack;
			}
			else if (this.unitàInFPS.GetComponent<PresenzaAlleato>().tipoTruppaVolante == 4)
			{
				this.silhouetteVelivoli.GetComponent<Image>().sprite = this.aereoBomber;
			}
			else if (this.unitàInFPS.GetComponent<PresenzaAlleato>().tipoTruppaVolante == 5)
			{
				this.silhouetteVelivoli.GetComponent<Image>().sprite = this.aereoStrategicBomber;
			}
			else if (this.unitàInFPS.GetComponent<PresenzaAlleato>().tipoTruppaVolante == 11)
			{
				this.silhouetteVelivoli.GetComponent<Image>().sprite = this.aereoStrike;
			}
			else if (this.unitàInFPS.GetComponent<PresenzaAlleato>().tipoTruppaVolante == 12)
			{
				this.silhouetteVelivoli.GetComponent<Image>().sprite = this.aereoInterceptor;
			}
			else if (this.unitàInFPS.GetComponent<PresenzaAlleato>().tipoTruppaVolante == 13)
			{
				this.silhouetteVelivoli.GetComponent<Image>().sprite = this.aereoLongRangeBomber;
			}
			for (int i = 0; i < 4; i++)
			{
				if (i < this.unitàInFPS.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi.Count)
				{
					this.silhouetteVelivoli.transform.GetChild(i * 2).GetComponent<Image>().sprite = this.unitàInFPS.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[i].GetComponent<DatiOrdignoEsterno>().ordignoSprite;
					this.silhouetteVelivoli.transform.GetChild(i * 2 + 1).GetComponent<Image>().sprite = this.unitàInFPS.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi[i].GetComponent<DatiOrdignoEsterno>().ordignoSprite;
					this.silhouetteVelivoli.transform.GetChild(i * 2).GetComponent<CanvasGroup>().alpha = 1f;
					this.silhouetteVelivoli.transform.GetChild(i * 2 + 1).GetComponent<CanvasGroup>().alpha = 1f;
				}
				else
				{
					this.silhouetteVelivoli.transform.GetChild(i * 2).GetComponent<CanvasGroup>().alpha = 0f;
					this.silhouetteVelivoli.transform.GetChild(i * 2 + 1).GetComponent<CanvasGroup>().alpha = 0f;
				}
			}
		}
		if (this.unitàInFPS.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi.Count == 3 || this.unitàInFPS.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi.Count == 2)
		{
			for (int j = 0; j < this.unitàInFPS.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi.Count; j++)
			{
				if (this.unitàInFPS.GetComponent<PresenzaAlleato>().ListaArmi[j + 1][5] != 0f)
				{
					this.silhouetteVelivoli.transform.GetChild(j * 2).GetComponent<CanvasGroup>().alpha = 1f;
					this.silhouetteVelivoli.transform.GetChild(j * 2 + 1).GetComponent<CanvasGroup>().alpha = 1f;
				}
				else
				{
					this.silhouetteVelivoli.transform.GetChild(j * 2).GetComponent<CanvasGroup>().alpha = 0f;
					this.silhouetteVelivoli.transform.GetChild(j * 2 + 1).GetComponent<CanvasGroup>().alpha = 0f;
				}
			}
		}
		else
		{
			for (int k = 0; k < this.unitàInFPS.GetComponent<PresenzaAlleato>().ListaOrdigniAttivi.Count; k++)
			{
				if (this.unitàInFPS.GetComponent<PresenzaAlleato>().ListaArmi[k][5] != 0f)
				{
					this.silhouetteVelivoli.transform.GetChild(k * 2).GetComponent<CanvasGroup>().alpha = 1f;
					this.silhouetteVelivoli.transform.GetChild(k * 2 + 1).GetComponent<CanvasGroup>().alpha = 1f;
				}
				else
				{
					this.silhouetteVelivoli.transform.GetChild(k * 2).GetComponent<CanvasGroup>().alpha = 0f;
					this.silhouetteVelivoli.transform.GetChild(k * 2 + 1).GetComponent<CanvasGroup>().alpha = 0f;
				}
			}
		}
	}

	// Token: 0x0400010C RID: 268
	private GameObject CanvasFPS;

	// Token: 0x0400010D RID: 269
	private GameObject Arma1;

	// Token: 0x0400010E RID: 270
	private GameObject Arma2;

	// Token: 0x0400010F RID: 271
	private GameObject Arma3;

	// Token: 0x04000110 RID: 272
	private GameObject Arma4;

	// Token: 0x04000111 RID: 273
	private List<GameObject> ListaArmiUIFPS;

	// Token: 0x04000112 RID: 274
	private GameObject vitaPerFPS;

	// Token: 0x04000113 RID: 275
	private GameObject nomeUnità;

	// Token: 0x04000114 RID: 276
	private GameObject barraVita;

	// Token: 0x04000115 RID: 277
	private GameObject numeriVita;

	// Token: 0x04000116 RID: 278
	private GameObject carburantePerFPS;

	// Token: 0x04000117 RID: 279
	private GameObject barraCarburante;

	// Token: 0x04000118 RID: 280
	private GameObject numeriCarburante;

	// Token: 0x04000119 RID: 281
	private GameObject primaCamera;

	// Token: 0x0400011A RID: 282
	private GameObject terzaCamera;

	// Token: 0x0400011B RID: 283
	private GameObject silhouetteVelivoli;

	// Token: 0x0400011C RID: 284
	private GameObject immagineAvvelenato;

	// Token: 0x0400011D RID: 285
	public Color scrittaVerde;

	// Token: 0x0400011E RID: 286
	public Color scrittaRossa;

	// Token: 0x0400011F RID: 287
	public Color armaEvidenziata;

	// Token: 0x04000120 RID: 288
	public Color armaNonEvidenziata;

	// Token: 0x04000121 RID: 289
	private GameObject unitàInFPS;

	// Token: 0x04000122 RID: 290
	public Sprite aereoFighter;

	// Token: 0x04000123 RID: 291
	public Sprite aereoGroundAttack;

	// Token: 0x04000124 RID: 292
	public Sprite aereoBomber;

	// Token: 0x04000125 RID: 293
	public Sprite aereoStrategicBomber;

	// Token: 0x04000126 RID: 294
	public Sprite aereoStrike;

	// Token: 0x04000127 RID: 295
	public Sprite aereoInterceptor;

	// Token: 0x04000128 RID: 296
	public Sprite aereoLongRangeBomber;
}
