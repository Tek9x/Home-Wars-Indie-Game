using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000E RID: 14
public class PresenzaAlleato : MonoBehaviour
{
	// Token: 0x06000066 RID: 102 RVA: 0x00014EFC File Offset: 0x000130FC
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.secondaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[1];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.varieMappaLocale = GameObject.FindGameObjectWithTag("VarieMappaLocale");
		this.sfondoQuadroAerei = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Quadro Aerei").FindChild("sfondo quadrato").gameObject;
		this.sensAerei = PlayerPrefs.GetFloat("sensibilità aerei") / 100f;
		this.voloInvertito = PlayerPrefs.GetInt("comandi inversi volanti");
		this.ordineAereoAria = PlayerPrefs.GetInt("ordini aerei aria");
		this.ordineAereoTerra = PlayerPrefs.GetInt("ordini aerei terra");
		this.ordineAereoBomb = PlayerPrefs.GetInt("ordini aerei bomb");
		if (base.GetComponent<NavMeshAgent>())
		{
			this.alleatoNav = base.GetComponent<NavMeshAgent>();
		}
		this.vitaIniziale = this.vita;
		this.carburanteIniziale = this.carburante;
		this.ListaArmi = new List<List<float>>();
		this.ListaArmi.Add(this.ListaValoriArma1);
		this.ListaArmi.Add(this.ListaValoriArma2);
		this.ListaArmi.Add(this.ListaValoriArma3);
		this.ListaArmi.Add(this.ListaValoriArma4);
		this.ListaMunizioneArmi = new List<List<GameObject>>();
		this.ListaMunizioneArmi.Add(this.ListaTipiMunizioniArma1);
		this.ListaMunizioneArmi.Add(this.ListaTipiMunizioniArma2);
		this.ListaMunizioneArmi.Add(this.ListaTipiMunizioniArma3);
		this.ListaMunizioneArmi.Add(this.ListaTipiMunizioniArma4);
		this.ListaMunizioniAttive = new List<GameObject>();
		this.ListaMunizioniAttive.Add(null);
		this.ListaMunizioniAttive.Add(null);
		this.ListaMunizioniAttive.Add(null);
		this.ListaMunizioniAttive.Add(null);
		this.ListaArmiAttivate = new List<bool>();
		this.ListaArmiAttivate.Add(this.arma1Attivata);
		this.ListaArmiAttivate.Add(this.arma2Attivata);
		this.ListaArmiAttivate.Add(this.arma3Attivata);
		this.ListaArmiAttivate.Add(this.arma4Attivata);
		this.armaAttivaInFPS = 0;
		this.ListaNomiArmi = new List<string>();
		this.ListaNomiArmi.Add(this.nomeArma1);
		this.ListaNomiArmi.Add(this.nomeArma2);
		this.ListaNomiArmi.Add(this.nomeArma3);
		this.ListaNomiArmi.Add(this.nomeArma4);
		this.ListaNumeroTipiMunizioni = new List<int>();
		this.ListaNumeroTipiMunizioni.Add(this.numeroTipiMunizioniArma1);
		this.ListaNumeroTipiMunizioni.Add(this.numeroTipiMunizioniArma2);
		this.ListaNumeroTipiMunizioni.Add(this.numeroTipiMunizioniArma3);
		this.ListaNumeroTipiMunizioni.Add(this.numeroTipiMunizioniArma4);
		this.ListaRicaricheInCorso = new List<bool>();
		this.ListaRicaricheInCorso.Add(this.ricaricaInCorsoArma1);
		this.ListaRicaricheInCorso.Add(this.ricaricaInCorsoArma2);
		this.ListaRicaricheInCorso.Add(this.ricaricaInCorsoArma3);
		this.ListaRicaricheInCorso.Add(this.ricaricaInCorsoArma4);
		this.ListaFuoriPortataArmi = new List<bool>();
		this.ListaFuoriPortataArmi.Add(this.fuoriPortataArma1);
		this.ListaFuoriPortataArmi.Add(this.fuoriPortataArma2);
		this.ListaFuoriPortataArmi.Add(this.fuoriPortataArma3);
		this.ListaFuoriPortataArmi.Add(this.fuoriPortataArma4);
		if (this.tipoTruppaVolante != 0)
		{
			this.AssegnamentoOrdigniVolanti();
		}
		if (this.tipoTruppaTerrConOrdigni != 0)
		{
			this.AssegnamentoOrdigniTerrestri();
		}
		this.ListaNumReintegrazioniOrdigni = new List<int>();
		this.ListaNumReintegrazioniOrdigni.Add(this.numReintegrazione1);
		this.ListaNumReintegrazioniOrdigni.Add(this.numReintegrazione2);
		this.ListaNumReintegrazioniOrdigni.Add(this.numReintegrazione3);
		this.ListaNumReintegrazioniOrdigni.Add(this.numReintegrazione4);
		this.RifornimentoIniziale();
		this.AssegnamentoInizialeMunizioni();
		this.cerchioSel = base.transform.GetChild(0).gameObject;
		if (this.volante)
		{
			this.cerchioSel2 = base.transform.GetChild(1).gameObject;
		}
		this.dimensioneQuadSel = base.transform.GetChild(0).transform.localScale.x;
		this.materialeSelezione = this.infoAlleati.GetComponent<GestioneComandanteInUI>().coloreSelAlleati;
		this.materialeEvidenziazione = this.infoAlleati.GetComponent<GestioneComandanteInUI>().coloreEvidAlleati;
		if (this.èParà)
		{
			base.transform.forward = -base.transform.parent.transform.forward;
			this.limiteMaxScalaParacChiuso = 71f;
			this.paracadute = (UnityEngine.Object.Instantiate(this.paracaduteChiuso, base.transform.position, Quaternion.identity) as GameObject);
			this.paracaduteVero = this.paracadute.transform.GetChild(1).gameObject;
			this.paracadute.transform.parent = this.ossoArma.transform;
			this.paracadute.transform.localPosition = this.posZainoParac;
			this.paracadute.transform.localEulerAngles = this.rotZainoParac;
			this.suonoParacadute = this.paracadute.GetComponent<AudioSource>();
			this.alleatoNav.enabled = false;
			this.giàSchierato = true;
		}
		this.durataAvvelenamento = 10.5f;
		this.ListaAvvelenamento = new List<List<float>>();
		for (int i = 0; i < 15; i++)
		{
			List<float> list = new List<float>();
			list.Add(0f);
			list.Add(0f);
			list.Add(100f);
			this.ListaAvvelenamento.Add(list);
		}
		this.ListaNemInAttacco = new List<GameObject>();
		for (int j = 0; j <= 6; j++)
		{
			this.ListaNemInAttacco.Add(null);
		}
		if (this.èAereo)
		{
			if (GestoreNeutroStrategia.tipoBattaglia == 5)
			{
				this.ricercaAutomaticaBersaglio = true;
			}
			else
			{
				if ((this.tipoTruppaVolante == 2 || this.tipoTruppaVolante == 12) && this.ordineAereoAria == 1)
				{
					this.ricercaAutomaticaBersaglio = true;
				}
				if ((this.tipoTruppaVolante == 3 || this.tipoTruppaVolante == 11) && this.ordineAereoTerra == 1)
				{
					this.ricercaAutomaticaBersaglio = true;
				}
				if ((this.tipoTruppaVolante == 4 || this.tipoTruppaVolante == 5 || this.tipoTruppaVolante == 13) && this.ordineAereoBomb == 1)
				{
					this.ricercaAutomaticaBersaglio = true;
				}
			}
		}
		else if (this.èFanteria && this.tipoTruppa != 6 && this.tipoTruppa != 7 && this.tipoTruppa != 8 && this.tipoTruppa != 9 && this.tipoTruppa != 14 && this.tipoTruppa != 15)
		{
			this.ricercaAutomDifensivaBersVicino = true;
		}
		else
		{
			this.ricercaAutomDifensivaBers = true;
		}
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00015644 File Offset: 0x00013844
	private void Update()
	{
		this.cameraAttiva = this.primaCamera.GetComponent<PrimaCamera>().oggettoCameraAttiva;
		this.evidenziaAlleatiENemici = this.infoAlleati.GetComponent<GestioneComandanteInUI>().evidenziaAlleatiENemici;
		this.Selezione();
		this.Rifornimento();
		if (this.tipoTruppaVolante != 0 || this.tipoTruppaTerrConOrdigni != 0)
		{
			this.AggiornamentoDatiOrdigniInArmi();
		}
		if (this.èAereo)
		{
			this.ConsumoCarburante();
		}
		if (this.èFanteria)
		{
			if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
			{
				this.ContattoTerreno();
			}
			else if (this.èParà)
			{
				this.ContattoTerreno();
			}
		}
		if (this.èParà)
		{
			this.FunzioneParacadutista();
		}
		this.Avvelenamento();
		this.Morte();
	}

	// Token: 0x06000068 RID: 104 RVA: 0x0001571C File Offset: 0x0001391C
	private void AssegnamentoOrdigniVolanti()
	{
		for (int i = 0; i < this.infoAlleati.GetComponent<InfoGenericheAlleati>().numeroTotaleTipiTruppeVolanti; i++)
		{
			if (i == this.tipoTruppaVolante)
			{
				for (int j = 0; j < this.numeroCoppieOrdigni; j++)
				{
					bool flag = false;
					int num = 0;
					while (num < this.ListaOrdigniPossibili.Count && !flag)
					{
						int num2 = 1;
						while (num2 <= this.infoAlleati.GetComponent<InfoGenericheAlleati>().numeroTotaleTipiOrdigni && !flag)
						{
							if (num2 == PlayerPrefs.GetInt(string.Concat(new object[]
							{
								"tipo truppa volante ",
								i,
								" ",
								j
							})) && this.ListaOrdigniPossibili[num].GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == PlayerPrefs.GetInt(string.Concat(new object[]
							{
								"tipo truppa volante ",
								i,
								" ",
								j
							})))
							{
								this.ListaOrdigniAttivi[j] = this.ListaOrdigniPossibili[num];
								flag = true;
							}
							num2++;
						}
						num++;
					}
				}
			}
		}
		if (this.tipoTruppaVolante == 5 || this.tipoTruppaVolante == 13)
		{
			int num3 = 0;
			while (num3 < this.ListaOrdigniAttivi.Count && this.ListaOrdigniAttivi[num3] != null)
			{
				for (int k = 0; k < 10; k++)
				{
					this.ListaArmi[num3][k] = this.ListaOrdigniAttivi[num3].GetComponent<DatiOrdignoEsterno>().ListaValoriOrdigno[k];
				}
				this.ListaMunizioneArmi.Add(null);
				this.ListaMunizioneArmi[num3][0] = this.ListaOrdigniAttivi[num3].GetComponent<DatiOrdignoEsterno>().munizioneUsata;
				this.ListaMunizioniAttive[num3] = this.ListaOrdigniAttivi[num3].GetComponent<DatiOrdignoEsterno>().munizioneUsata;
				this.ListaNomiArmi[num3] = this.ListaOrdigniAttivi[num3].GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().nome;
				num3++;
			}
		}
		else
		{
			int num4 = 0;
			while (num4 < this.ListaOrdigniAttivi.Count && this.ListaOrdigniAttivi[num4] != null)
			{
				for (int l = 0; l < 10; l++)
				{
					this.ListaArmi[num4 + 1][l] = this.ListaOrdigniAttivi[num4].GetComponent<DatiOrdignoEsterno>().ListaValoriOrdigno[l];
				}
				this.ListaMunizioneArmi.Add(null);
				this.ListaMunizioneArmi[num4 + 1][0] = this.ListaOrdigniAttivi[num4].GetComponent<DatiOrdignoEsterno>().munizioneUsata;
				this.ListaMunizioniAttive[num4 + 1] = this.ListaOrdigniAttivi[num4].GetComponent<DatiOrdignoEsterno>().munizioneUsata;
				this.ListaNomiArmi[num4 + 1] = this.ListaOrdigniAttivi[num4].GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().nome;
				num4++;
			}
		}
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00015A94 File Offset: 0x00013C94
	private void AssegnamentoOrdigniTerrestri()
	{
		for (int i = 0; i < this.infoAlleati.GetComponent<InfoGenericheAlleati>().numeroTotaleTipiTruppeTerrConOrdigni; i++)
		{
			if (i == this.tipoTruppaTerrConOrdigni)
			{
				for (int j = 0; j < this.numeroCoppieOrdigni; j++)
				{
					bool flag = false;
					int num = 0;
					while (num < this.ListaOrdigniPossibili.Count && !flag)
					{
						int num2 = 1;
						while (num2 <= this.infoAlleati.GetComponent<InfoGenericheAlleati>().numeroTotaleTipiOrdigni && !flag)
						{
							if (num2 == PlayerPrefs.GetInt(string.Concat(new object[]
							{
								"tipo truppa terr con ordigno ",
								i,
								" ",
								j
							})) && this.ListaOrdigniPossibili[num].GetComponent<DatiOrdignoEsterno>().tipologiaOrdigno == PlayerPrefs.GetInt(string.Concat(new object[]
							{
								"tipo truppa terr con ordigno ",
								i,
								" ",
								j
							})))
							{
								this.ListaOrdigniAttivi[j] = this.ListaOrdigniPossibili[num];
								flag = true;
							}
							num2++;
						}
						num++;
					}
				}
			}
		}
		if (this.tipoTruppaTerrConOrdigni == 3)
		{
			int num3 = 0;
			while (num3 < this.ListaOrdigniAttivi.Count && this.ListaOrdigniAttivi[num3] != null)
			{
				for (int k = 0; k < 10; k++)
				{
					this.ListaArmi[num3][k] = this.ListaOrdigniAttivi[num3].GetComponent<DatiOrdignoEsterno>().ListaValoriOrdigno[k];
				}
				this.ListaMunizioneArmi.Add(null);
				this.ListaMunizioneArmi[num3][0] = this.ListaOrdigniAttivi[num3].GetComponent<DatiOrdignoEsterno>().munizioneUsata;
				this.ListaMunizioniAttive[num3] = this.ListaOrdigniAttivi[num3].GetComponent<DatiOrdignoEsterno>().munizioneUsata;
				this.ListaNomiArmi[num3] = this.ListaOrdigniAttivi[num3].GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().nome;
				num3++;
			}
		}
		else
		{
			int num4 = 0;
			while (num4 < this.ListaOrdigniAttivi.Count && this.ListaOrdigniAttivi[num4] != null)
			{
				for (int l = 0; l < 10; l++)
				{
					this.ListaArmi[num4 + 1][l] = this.ListaOrdigniAttivi[num4].GetComponent<DatiOrdignoEsterno>().ListaValoriOrdigno[l];
				}
				this.ListaMunizioneArmi.Add(null);
				this.ListaMunizioneArmi[num4 + 1][0] = this.ListaOrdigniAttivi[num4].GetComponent<DatiOrdignoEsterno>().munizioneUsata;
				this.ListaMunizioniAttive[num4 + 1] = this.ListaOrdigniAttivi[num4].GetComponent<DatiOrdignoEsterno>().munizioneUsata;
				this.ListaNomiArmi[num4 + 1] = this.ListaOrdigniAttivi[num4].GetComponent<DatiOrdignoEsterno>().munizioneUsata.GetComponent<DatiGeneraliMunizione>().nome;
				num4++;
			}
		}
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00015E00 File Offset: 0x00014000
	private void RifornimentoIniziale()
	{
		if (!this.èPerRifornimento)
		{
			for (int i = 0; i < this.ListaArmi.Count; i++)
			{
				if (i < this.numeroArmi)
				{
					if (this.ListaMunizioneArmi[i][0].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().quantità > this.ListaArmi[i][4])
					{
						this.ListaArmi[i][6] = this.ListaArmi[i][4];
						this.ListaMunizioneArmi[i][0].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().quantità -= this.ListaArmi[i][4];
					}
					else
					{
						this.ListaArmi[i][6] = this.ListaMunizioneArmi[i][0].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().quantità;
						this.ListaMunizioneArmi[i][0].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().quantità = 0f;
					}
					if (this.ListaArmi[i][9] == 1f)
					{
						this.ListaArmi[i][5] = this.ListaArmi[i][6];
					}
				}
			}
		}
		else
		{
			this.puntiRifornimentoDisp = this.puntiRifornimentoMax;
		}
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00015F9C File Offset: 0x0001419C
	private void AssegnamentoInizialeMunizioni()
	{
		if (!this.èPerRifornimento)
		{
			for (int i = 0; i < this.numeroArmi; i++)
			{
				this.ListaMunizioniAttive[i] = this.ListaMunizioneArmi[i][0];
			}
		}
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00015FEC File Offset: 0x000141EC
	private void Selezione()
	{
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			if (this.truppaSelezionata || this.infoAlleati.GetComponent<GestioneComandanteInUI>().evidenziaAlleatiENemici)
			{
				this.cerchioSel.GetComponent<MeshRenderer>().enabled = true;
				this.cerchioSel.GetComponent<MeshRenderer>().material = this.materialeSelezione;
				this.testoVita.GetComponent<MeshRenderer>().enabled = true;
				if (this.volante)
				{
					this.cerchioSel2.GetComponent<MeshRenderer>().enabled = true;
					this.cerchioSel2.GetComponent<MeshRenderer>().material = this.materialeSelezione;
				}
				if (this.èPerRifornimento)
				{
					this.quadRaggioRifor.GetComponent<MeshRenderer>().enabled = true;
				}
				float num;
				if (this.cameraAttiva == this.primaCamera || this.cameraAttiva == this.terzaCamera)
				{
					num = Vector3.Distance(base.transform.position, this.cameraAttiva.transform.position);
				}
				else
				{
					num = this.secondaCamera.GetComponent<Camera>().orthographicSize * 1.8f;
				}
				float num2 = num / 200f * (this.dimensioneQuadSel / 50f + 1f);
				if (num2 < this.dimensioneQuadSel)
				{
					this.cerchioSel.transform.localScale = new Vector3(this.dimensioneQuadSel, this.dimensioneQuadSel, 0f);
				}
				else
				{
					this.cerchioSel.transform.localScale = new Vector3(num2, num2, 0f);
				}
				if (this.truppaSelezionata)
				{
					this.cerchioSel.GetComponent<MeshRenderer>().material = this.materialeSelezione;
					if (this.volante)
					{
						this.cerchioSel2.GetComponent<MeshRenderer>().material = this.materialeSelezione;
					}
				}
				else if (this.evidenziaAlleatiENemici)
				{
					this.cerchioSel.GetComponent<MeshRenderer>().material = this.materialeEvidenziazione;
					if (this.volante)
					{
						this.cerchioSel2.GetComponent<MeshRenderer>().material = this.materialeEvidenziazione;
					}
				}
				this.testoVita.GetComponent<TextMesh>().text = (this.vita * 100f / this.vitaIniziale).ToString("F1") + "%";
				Vector3 normalized = (base.transform.position - this.cameraAttiva.transform.position).normalized;
				this.testoVita.transform.forward = normalized;
				float num3 = num / 1000f * 1.8f;
				float num4 = 0.2f;
				if (num3 > num4)
				{
					this.testoVita.transform.localScale = new Vector3(num3, num3, num3);
				}
				else
				{
					this.testoVita.transform.localScale = new Vector3(num4, num4, num4);
				}
			}
			else
			{
				this.cerchioSel.GetComponent<MeshRenderer>().enabled = false;
				this.testoVita.GetComponent<MeshRenderer>().enabled = false;
				if (this.volante)
				{
					this.cerchioSel2.GetComponent<MeshRenderer>().enabled = false;
				}
				if (this.èPerRifornimento)
				{
					this.quadRaggioRifor.GetComponent<MeshRenderer>().enabled = false;
				}
			}
		}
		else
		{
			this.cerchioSel.GetComponent<MeshRenderer>().enabled = false;
			this.testoVita.GetComponent<MeshRenderer>().enabled = false;
			if (this.volante)
			{
				this.cerchioSel2.GetComponent<MeshRenderer>().enabled = false;
			}
			if (this.èPerRifornimento)
			{
				this.quadRaggioRifor.GetComponent<MeshRenderer>().enabled = false;
			}
		}
	}

	// Token: 0x0600006D RID: 109 RVA: 0x000163B0 File Offset: 0x000145B0
	private void Rifornimento()
	{
		if (this.rifornimentoAttivo)
		{
			this.rifornimentoAttivo = false;
			if (!this.èPerRifornimento)
			{
				for (int i = 0; i < this.numeroArmi; i++)
				{
					float num = 0f;
					if (this.ListaArmi[i][4] - this.ListaArmi[i][6] > this.ListaArmi[i][7])
					{
						if (this.ListaArmi[i][7] < this.ListaMunizioneArmi[i][0].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().quantità)
						{
							if (this.ListaArmi[i][9] == 1f)
							{
								num = this.ListaArmi[i][7];
							}
							List<float> list;
							List<float> expr_DF = list = this.ListaArmi[i];
							int index;
							int expr_E3 = index = 6;
							float num2 = list[index];
							expr_DF[expr_E3] = num2 + this.ListaArmi[i][7];
							this.ListaMunizioneArmi[i][0].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().quantità -= this.ListaArmi[i][7];
							if (this.ListaArmi[i][9] == 1f && this.ListaArmi[i][5] < this.ListaArmi[i][6])
							{
								this.ListaArmi[i][5] = this.ListaArmi[i][6];
							}
						}
						else
						{
							if (this.ListaArmi[i][9] == 1f)
							{
								num = this.ListaMunizioneArmi[i][0].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().quantità;
							}
							List<float> list2;
							List<float> expr_209 = list2 = this.ListaArmi[i];
							int index;
							int expr_20D = index = 6;
							float num2 = list2[index];
							expr_209[expr_20D] = num2 + this.ListaMunizioneArmi[i][0].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().quantità;
							this.ListaMunizioneArmi[i][0].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().quantità = 0f;
							if (this.ListaArmi[i][9] == 1f && this.ListaArmi[i][5] < this.ListaArmi[i][6])
							{
								this.ListaArmi[i][5] = this.ListaArmi[i][6];
							}
						}
					}
					else if (this.ListaArmi[i][4] - this.ListaArmi[i][6] < this.ListaMunizioneArmi[i][0].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().quantità)
					{
						if (this.ListaArmi[i][9] == 1f)
						{
							num = this.ListaArmi[i][4] - this.ListaArmi[i][6];
						}
						this.ListaMunizioneArmi[i][0].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().quantità -= this.ListaArmi[i][4] - this.ListaArmi[i][6];
						List<float> list3;
						List<float> expr_3D4 = list3 = this.ListaArmi[i];
						int index;
						int expr_3D8 = index = 6;
						float num2 = list3[index];
						expr_3D4[expr_3D8] = num2 + (this.ListaArmi[i][4] - this.ListaArmi[i][6]);
						if (this.ListaArmi[i][9] == 1f && this.ListaArmi[i][5] < this.ListaArmi[i][6])
						{
							this.ListaArmi[i][5] = this.ListaArmi[i][6];
						}
					}
					else
					{
						if (this.ListaArmi[i][9] == 1f)
						{
							num = this.ListaMunizioneArmi[i][0].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().quantità;
						}
						List<float> list4;
						List<float> expr_4D2 = list4 = this.ListaArmi[i];
						int index;
						int expr_4D6 = index = 6;
						float num2 = list4[index];
						expr_4D2[expr_4D6] = num2 + this.ListaMunizioneArmi[i][0].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().quantità;
						this.ListaMunizioneArmi[i][0].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().quantità = 0f;
						if (this.ListaArmi[i][9] == 1f && this.ListaArmi[i][5] < this.ListaArmi[i][6])
						{
							this.ListaArmi[i][5] = this.ListaArmi[i][6];
						}
					}
					if (this.ListaArmi[i][9] == 1f && num != 0f)
					{
						this.ListaNumReintegrazioniOrdigni[i] = Mathf.RoundToInt(num);
						this.reintegrazioneNecessaria = true;
					}
				}
			}
		}
		if (this.riparazioneAttiva)
		{
			this.riparazioneAttiva = false;
			if (this.èFanteria)
			{
				this.velocitàRiparazione = this.vita / 10f;
			}
			if (this.èMezzo || this.èArtiglieria)
			{
				this.velocitàRiparazione = this.vita / 20f;
			}
			if (this.volante)
			{
				this.velocitàRiparazione = this.vita / 15f;
			}
			if (this.velocitàRiparazione > this.vitaIniziale - this.vita)
			{
				this.vita = this.vitaIniziale;
			}
			else
			{
				this.vita += this.velocitàRiparazione;
			}
		}
		this.timerQuadMuniz += Time.deltaTime;
		this.timerVoceSenzaMuniz += Time.deltaTime;
		if (this.timerQuadMuniz > 2f)
		{
			if (this.èPerRifornimento)
			{
				if (this.puntiRifornimentoDisp <= 0)
				{
					this.quadSenzaMunizioni.GetComponent<MeshRenderer>().enabled = true;
					if (this.volante)
					{
						this.quadSenzaMunizioni2.GetComponent<MeshRenderer>().enabled = true;
					}
					if (this.timerVoceSenzaMuniz > 60f)
					{
						this.timerVoceSenzaMuniz = 0f;
						this.primaCamera.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
						this.primaCamera.GetComponent<GestioneSuoniCamera>().numListaVoceSelez = 16;
					}
				}
				else
				{
					this.quadSenzaMunizioni.GetComponent<MeshRenderer>().enabled = false;
					if (this.volante)
					{
						this.quadSenzaMunizioni2.GetComponent<MeshRenderer>().enabled = false;
					}
				}
			}
			else if (this.tipoTruppa != 10 && this.tipoTruppa != 11 && this.tipoTruppa != 44)
			{
				int num3 = 0;
				for (int j = 0; j < this.numeroArmi; j++)
				{
					if (this.ListaArmi[j][6] <= 0f)
					{
						num3++;
					}
				}
				if (num3 == this.numeroArmi)
				{
					this.quadSenzaMunizioni.GetComponent<MeshRenderer>().enabled = true;
					if (this.volante)
					{
						this.quadSenzaMunizioni2.GetComponent<MeshRenderer>().enabled = true;
					}
					if (this.timerVoceSenzaMuniz > 60f)
					{
						this.timerVoceSenzaMuniz = 0f;
						this.primaCamera.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
						this.primaCamera.GetComponent<GestioneSuoniCamera>().numListaVoceSelez = 16;
					}
				}
				else
				{
					this.quadSenzaMunizioni.GetComponent<MeshRenderer>().enabled = false;
					if (this.volante)
					{
						this.quadSenzaMunizioni2.GetComponent<MeshRenderer>().enabled = false;
					}
				}
			}
		}
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00016C5C File Offset: 0x00014E5C
	private void AggiornamentoDatiOrdigniInArmi()
	{
		int num = 0;
		while (num < this.ListaOrdigniAttivi.Count && this.ListaOrdigniAttivi[num] != null)
		{
			for (int i = 0; i < 10; i++)
			{
				if (this.tipoTruppaVolante == 5 || this.tipoTruppaVolante == 13 || this.tipoTruppaTerrConOrdigni == 3)
				{
					this.ListaOrdigniAttivi[num].GetComponent<DatiOrdignoEsterno>().ListaValoriOrdigno[i] = this.ListaArmi[num][i];
				}
				else
				{
					this.ListaOrdigniAttivi[num].GetComponent<DatiOrdignoEsterno>().ListaValoriOrdigno[i] = this.ListaArmi[num + 1][i];
				}
			}
			num++;
		}
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00016D38 File Offset: 0x00014F38
	private void ConsumoCarburante()
	{
		if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAereiInVolo[this.posInQuadratoAerei])
		{
			if (this.richiamaAereo)
			{
				this.carburante = 0f;
				this.richiamaAereo = false;
			}
			if (this.carburante <= 0f)
			{
				this.tornaAllaBase = true;
			}
			if (this.ritornoEffettuato)
			{
				this.ritornoEffettuato = false;
				for (int i = 0; i < this.numeroArmi; i++)
				{
					this.ListaMunizioneArmi[i][0].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().quantità += this.ListaArmi[i][6];
				}
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAereiInRifor[this.posInQuadratoAerei] = true;
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAereiInVolo[this.posInQuadratoAerei] = false;
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ricreaAereoInQuadro = true;
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().tipoAereoDaRicreareInQuadro = this.tipoTruppa;
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().postoInCuiRicreare = this.posInQuadratoAerei;
				if (this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera == base.gameObject)
				{
					this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva = 1;
					this.primaCamera.GetComponent<PrimaCamera>().oggettoCameraAttiva = this.primaCamera;
					this.primaCamera.GetComponent<PrimaCamera>().morteAvvenuta = true;
					this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera = null;
					this.terzaCamera.transform.parent = null;
					this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
					this.terzaCamera.GetComponent<TerzaCamera>().terzaCameraPosizionata = false;
					this.terzaCamera.GetComponent<TerzaCamera>().èTPS = true;
					this.terzaCamera.GetComponent<TerzaCamera>().entraInPrimaPersona = false;
				}
				this.primaCamera.GetComponent<Selezionamento>().azzeramentoSelezione = true;
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati.Remove(base.gameObject);
				if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Contains(base.gameObject))
				{
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Remove(base.gameObject);
				}
				if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèAereo.Contains(base.gameObject))
				{
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèAereo.Remove(base.gameObject);
				}
				if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèVolante.Contains(base.gameObject))
				{
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèVolante.Remove(base.gameObject);
				}
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				this.carburante -= Time.deltaTime;
				GameObject gameObject = this.sfondoQuadroAerei.transform.GetChild(this.posInQuadratoAerei).FindChild("barra scorrevole").gameObject;
				float fillAmount = this.carburante / this.carburanteIniziale;
				gameObject.GetComponent<Image>().fillAmount = fillAmount;
			}
		}
	}

	// Token: 0x06000070 RID: 112 RVA: 0x0001707C File Offset: 0x0001527C
	private void ContattoTerreno()
	{
		this.timerContattoTerreno += Time.deltaTime;
		if (Physics.Raycast(base.transform.position, -Vector3.up, out this.hitTerreno, 3f, 256))
		{
			this.toccaTerreno = true;
			this.timerContattoTerreno = 0f;
			if (!this.allineatoConTerreno)
			{
				this.allineatoConTerreno = true;
				base.transform.position = new Vector3(base.transform.position.x, this.hitTerreno.point.y + this.altezzaCentroUnità + 0.25f, base.transform.position.z);
				this.alleatoNav.enabled = true;
			}
		}
		else if (this.giàSchierato && !this.alleatoNav.isOnOffMeshLink && !this.èParà && this.timerContattoTerreno > 1f)
		{
			this.toccaTerreno = false;
			this.allineatoConTerreno = false;
			this.alleatoNav.enabled = false;
			base.transform.position += -Vector3.up * 80f * Time.deltaTime;
		}
	}

	// Token: 0x06000071 RID: 113 RVA: 0x000171DC File Offset: 0x000153DC
	private void FunzioneParacadutista()
	{
		if (this.toccaTerreno)
		{
			this.èInLancio = false;
			this.èParà = false;
			UnityEngine.Object.Destroy(this.paracadute);
			if (base.GetComponent<Rigidbody>())
			{
				base.GetComponent<Rigidbody>().useGravity = true;
			}
		}
		else if (base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().useGravity = false;
		}
		if (this.èInLancio)
		{
			this.timerDalLancio += Time.deltaTime;
			if (this.timerDalLancio < 2.5f)
			{
				if (this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera == base.gameObject && !this.suonoParàPartito)
				{
					this.suonoParacadute.Play();
					this.suonoParàPartito = true;
				}
				this.velocitàLancio += 40f * Time.deltaTime;
				if (this.velocitàLancio > 60f)
				{
					this.velocitàLancio = 60f;
				}
				base.transform.position += -Vector3.up * this.velocitàLancio * Time.deltaTime;
			}
			else if (!this.paracèAperto)
			{
				base.transform.position += -Vector3.up * this.velocitàLancio * Time.deltaTime;
				float d = this.paracadute.transform.GetChild(1).transform.localScale.x + 180f * Time.deltaTime;
				this.paracadute.transform.GetChild(1).transform.localScale = new Vector3(1f, 1f, 1f) * d;
				if (this.paracadute.transform.GetChild(1).localScale.x > this.limiteMaxScalaParacChiuso)
				{
					UnityEngine.Object.Destroy(this.paracadute);
					this.paracadute = (UnityEngine.Object.Instantiate(this.paracaduteAperto, base.transform.position, Quaternion.identity) as GameObject);
					this.paracaduteVero = this.paracadute.transform.GetChild(1).gameObject;
					this.paracadute.transform.parent = this.ossoArma.transform;
					this.paracadute.transform.localPosition = this.posZainoParac;
					this.paracadute.transform.localEulerAngles = this.rotZainoParac;
					this.suonoParacadute = this.paracadute.GetComponent<AudioSource>();
					this.paracèAperto = true;
					this.suonoParàPartito = false;
				}
			}
			else
			{
				base.transform.position += -Vector3.up * 10f * Time.deltaTime;
				if (this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera == base.gameObject)
				{
					if (this.timerDalLancio < 8f && !this.suonoParàPartito)
					{
						this.suonoParacadute.Play();
						this.suonoParàPartito = true;
					}
					if (this.timerDalLancio > 8f && this.suonoParàPartito)
					{
						this.suonoParacadute.clip = this.effettoVentoParàAperto;
						this.suonoParacadute.Play();
						this.suonoParàPartito = false;
					}
				}
				else if (!this.suonoParàPartito)
				{
					this.paracadute.transform.GetChild(0).GetComponent<AudioSource>().Play();
					this.suonoParàPartito = true;
				}
			}
			this.paracaduteVero.transform.eulerAngles = new Vector3(270f, this.paracadute.transform.eulerAngles.y, this.paracadute.transform.eulerAngles.z);
		}
	}

	// Token: 0x06000072 RID: 114 RVA: 0x000175E4 File Offset: 0x000157E4
	private void Avvelenamento()
	{
		if (this.ListaAvvelenamento[0][0] != 0f)
		{
			this.timerAvvelenamento += Time.deltaTime;
			if (this.timerAvvelenamento > 2f)
			{
				this.timerAvvelenamento = 0f;
				for (int i = 0; i < this.ListaAvvelenamento.Count; i++)
				{
					if (this.ListaAvvelenamento[i][0] != 0f)
					{
						List<float> list;
						List<float> expr_78 = list = this.ListaAvvelenamento[i];
						int index;
						int expr_7C = index = 1;
						float num = list[index];
						expr_78[expr_7C] = num - 2f;
						if (this.ListaAvvelenamento[i][1] <= 0f)
						{
							this.ListaAvvelenamento[i][0] = 0f;
							this.ListaAvvelenamento[i][1] = 0f;
						}
						else
						{
							float num2 = 0f;
							if (this.vita > this.ListaAvvelenamento[i][0])
							{
								num2 = this.ListaAvvelenamento[i][0];
							}
							else if (this.vita > 0f)
							{
								num2 = this.vita;
							}
							this.vita -= this.ListaAvvelenamento[i][0];
							int num3 = Mathf.FloorToInt(this.ListaAvvelenamento[i][2]);
							List<float> listaDanniNemici;
							List<float> expr_17F = listaDanniNemici = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
							int expr_183 = index = num3;
							num = listaDanniNemici[index];
							expr_17F[expr_183] = num + num2;
						}
					}
				}
				for (int j = 0; j < this.ListaAvvelenamento.Count; j++)
				{
					if (this.ListaAvvelenamento[j][0] == 0f)
					{
						for (int k = j + 1; k < this.ListaAvvelenamento.Count; k++)
						{
							this.ListaAvvelenamento[k - 1][0] = this.ListaAvvelenamento[k][0];
							this.ListaAvvelenamento[k - 1][1] = this.ListaAvvelenamento[k][1];
						}
					}
				}
			}
		}
	}

	// Token: 0x06000073 RID: 115 RVA: 0x0001784C File Offset: 0x00015A4C
	private void Morte()
	{
		if (this.vita <= 0f || base.transform.position.y < -5f)
		{
			if (this.tipoTruppa == 44 && !this.eliminaParàDaAereo)
			{
				this.eliminaParàDaAereo = true;
			}
			else
			{
				this.morto = true;
			}
		}
		if (this.morto)
		{
			if (!this.estratto)
			{
				this.primaCamera.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
				this.primaCamera.GetComponent<GestioneSuoniCamera>().numListaVoceSelez = 13;
			}
			if (this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera == base.gameObject)
			{
				if (this.terzaCamera.transform.childCount > 0)
				{
					for (int i = 0; i < this.terzaCamera.transform.childCount; i++)
					{
						UnityEngine.Object.Destroy(this.terzaCamera.transform.GetChild(0).gameObject);
					}
				}
				this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva = 1;
				this.primaCamera.GetComponent<PrimaCamera>().oggettoCameraAttiva = this.primaCamera;
				this.primaCamera.GetComponent<PrimaCamera>().morteAvvenuta = true;
				this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera = null;
				this.terzaCamera.transform.parent = null;
				this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
				this.terzaCamera.GetComponent<TerzaCamera>().terzaCameraPosizionata = false;
				this.terzaCamera.GetComponent<TerzaCamera>().èTPS = true;
				this.terzaCamera.GetComponent<TerzaCamera>().entraInPrimaPersona = false;
				if (this.tipoTruppa == 10 && base.GetComponent<ATT_Observer>().mirinoCreato)
				{
					UnityEngine.Object.Destroy(base.GetComponent<ATT_Observer>().mirinoObserver);
				}
			}
			this.infoAlleati.GetComponent<GestioneComandanteInUI>().aggMorte = true;
			if (GestoreNeutroStrategia.tipoBattaglia != 3)
			{
				GestoreNeutroTattica.numAlleatiMorti++;
			}
			else if (!this.èFanteria)
			{
				GestoreNeutroTattica.numAlleatiMorti++;
			}
			if (this.tipoTruppa == 16 || this.tipoTruppa == 33)
			{
				this.infoAlleati.GetComponent<InfoMunizionamento>().ListaTipiMunizioniBaseTattica[11].GetComponent<QuantitàMunizione>().quantità += (float)this.puntiRifornimentoDisp;
			}
			else
			{
				for (int j = 0; j < this.numeroArmi; j++)
				{
					this.ListaMunizioneArmi[j][0].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().quantità += this.ListaArmi[j][6];
				}
			}
			if (this.èGeniere && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0 && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0] == base.gameObject)
			{
				if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz)
				{
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz.GetComponent<PresenzaTrappola>().inPosizionamento = false;
				}
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaCreata = false;
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().posizTrapAttivo = false;
				UnityEngine.Object.Destroy(this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz);
			}
			if (this.tipoTruppa == 10 && base.GetComponent<ATT_Observer>().mirinoCreato)
			{
				UnityEngine.Object.Destroy(base.GetComponent<ATT_Observer>().mirinoObserver);
				base.GetComponent<ATT_Observer>().mirinoCreato = false;
			}
			if (!this.estratto)
			{
				this.quadMorte.GetComponent<MeshRenderer>().enabled = true;
				this.quadMorte.GetComponent<QuadMorteScript>().attivo = true;
				this.quadMorte.transform.parent = null;
				if (this.volante)
				{
					this.quadMorte2.GetComponent<MeshRenderer>().enabled = true;
					this.quadMorte2.GetComponent<QuadMorteScript>().attivo = true;
					this.quadMorte2.transform.parent = null;
				}
			}
			this.primaCamera.GetComponent<Selezionamento>().azzeramentoSelezione = true;
			this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati.Remove(base.gameObject);
			if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Contains(base.gameObject))
			{
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Remove(base.gameObject);
			}
			if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèAereo.Contains(base.gameObject))
			{
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèAereo.Remove(base.gameObject);
			}
			if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèArtiglieria.Contains(base.gameObject))
			{
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèArtiglieria.Remove(base.gameObject);
			}
			if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèElicottero.Contains(base.gameObject))
			{
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèElicottero.Remove(base.gameObject);
			}
			if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèFanteria.Contains(base.gameObject))
			{
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèFanteria.Remove(base.gameObject);
			}
			if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèMezzo.Contains(base.gameObject))
			{
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèMezzo.Remove(base.gameObject);
			}
			if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèPerRifornimento.Contains(base.gameObject))
			{
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèPerRifornimento.Remove(base.gameObject);
			}
			if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèVolante.Contains(base.gameObject))
			{
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèVolante.Remove(base.gameObject);
			}
			if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante.Contains(base.gameObject))
			{
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèNonVolante.Remove(base.gameObject);
			}
			if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèGeniere.Contains(base.gameObject))
			{
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaèGeniere.Remove(base.gameObject);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04000227 RID: 551
	public float vita;

	// Token: 0x04000228 RID: 552
	public float velocitàRiparazione;

	// Token: 0x04000229 RID: 553
	public float costoInPlastica;

	// Token: 0x0400022A RID: 554
	public float carburante;

	// Token: 0x0400022B RID: 555
	public int tipoTruppa;

	// Token: 0x0400022C RID: 556
	public int tipoTruppaVolante;

	// Token: 0x0400022D RID: 557
	public int tipoTruppaTerrConOrdigni;

	// Token: 0x0400022E RID: 558
	public bool scalatrice;

	// Token: 0x0400022F RID: 559
	public bool volante;

	// Token: 0x04000230 RID: 560
	public bool èFanteria;

	// Token: 0x04000231 RID: 561
	public bool èMezzo;

	// Token: 0x04000232 RID: 562
	public bool èElicottero;

	// Token: 0x04000233 RID: 563
	public bool èAereo;

	// Token: 0x04000234 RID: 564
	public bool èArtiglieria;

	// Token: 0x04000235 RID: 565
	public float valoreInizialePrecisione;

	// Token: 0x04000236 RID: 566
	public float valorePerditaPrecisione;

	// Token: 0x04000237 RID: 567
	public bool èBombardiere;

	// Token: 0x04000238 RID: 568
	public bool èPerRifornimento;

	// Token: 0x04000239 RID: 569
	public int puntiRifornimentoMax;

	// Token: 0x0400023A RID: 570
	public int puntiRifornimentoDisp;

	// Token: 0x0400023B RID: 571
	public float raggioDiRifornimento;

	// Token: 0x0400023C RID: 572
	public GameObject quadRaggioRifor;

	// Token: 0x0400023D RID: 573
	public bool èGeniere;

	// Token: 0x0400023E RID: 574
	public float quotaDiPartenza;

	// Token: 0x0400023F RID: 575
	public GameObject oggettoDescrizione;

	// Token: 0x04000240 RID: 576
	public string velocitàIndicativa;

	// Token: 0x04000241 RID: 577
	public float raggioVisivo;

	// Token: 0x04000242 RID: 578
	public string nomeUnità;

	// Token: 0x04000243 RID: 579
	public Sprite immagineUnità;

	// Token: 0x04000244 RID: 580
	public GameObject copiaPerSchieramento;

	// Token: 0x04000245 RID: 581
	public bool comportamentoDifensivo;

	// Token: 0x04000246 RID: 582
	public bool ricercaAutomaticaBersaglio;

	// Token: 0x04000247 RID: 583
	public bool ricercaAutomDifensivaBers;

	// Token: 0x04000248 RID: 584
	public bool ricercaAutomDifensivaBersVicino;

	// Token: 0x04000249 RID: 585
	public int numeroArmi;

	// Token: 0x0400024A RID: 586
	public string nomeArma1;

	// Token: 0x0400024B RID: 587
	public string nomeArma2;

	// Token: 0x0400024C RID: 588
	public string nomeArma3;

	// Token: 0x0400024D RID: 589
	public string nomeArma4;

	// Token: 0x0400024E RID: 590
	public List<string> ListaNomiArmi;

	// Token: 0x0400024F RID: 591
	public List<bool> ListaArmiAttivate;

	// Token: 0x04000250 RID: 592
	public bool arma1Attivata;

	// Token: 0x04000251 RID: 593
	public bool arma2Attivata;

	// Token: 0x04000252 RID: 594
	public bool arma3Attivata;

	// Token: 0x04000253 RID: 595
	public bool arma4Attivata;

	// Token: 0x04000254 RID: 596
	public int armaAttivaInFPS;

	// Token: 0x04000255 RID: 597
	public List<float> ListaValoriArma1;

	// Token: 0x04000256 RID: 598
	public List<float> ListaValoriArma2;

	// Token: 0x04000257 RID: 599
	public List<float> ListaValoriArma3;

	// Token: 0x04000258 RID: 600
	public List<float> ListaValoriArma4;

	// Token: 0x04000259 RID: 601
	public List<List<float>> ListaArmi;

	// Token: 0x0400025A RID: 602
	public bool truppaSelezionata;

	// Token: 0x0400025B RID: 603
	public bool attaccoOrdinato;

	// Token: 0x0400025C RID: 604
	public bool destinazioneOrdinata;

	// Token: 0x0400025D RID: 605
	public bool attaccoZonaOrdinato;

	// Token: 0x0400025E RID: 606
	public Vector3 destinazioneSenzaNavMesh;

	// Token: 0x0400025F RID: 607
	private GameObject infoNeutreTattica;

	// Token: 0x04000260 RID: 608
	private GameObject terzaCamera;

	// Token: 0x04000261 RID: 609
	private GameObject infoAlleati;

	// Token: 0x04000262 RID: 610
	private GameObject primaCamera;

	// Token: 0x04000263 RID: 611
	private GameObject varieMappaLocale;

	// Token: 0x04000264 RID: 612
	private GameObject sfondoQuadroAerei;

	// Token: 0x04000265 RID: 613
	private GameObject secondaCamera;

	// Token: 0x04000266 RID: 614
	public int numeroTipiMunizioniArma1;

	// Token: 0x04000267 RID: 615
	public int numeroTipiMunizioniArma2;

	// Token: 0x04000268 RID: 616
	public int numeroTipiMunizioniArma3;

	// Token: 0x04000269 RID: 617
	public int numeroTipiMunizioniArma4;

	// Token: 0x0400026A RID: 618
	public List<int> ListaNumeroTipiMunizioni;

	// Token: 0x0400026B RID: 619
	public List<GameObject> ListaTipiMunizioniArma1;

	// Token: 0x0400026C RID: 620
	public List<GameObject> ListaTipiMunizioniArma2;

	// Token: 0x0400026D RID: 621
	public List<GameObject> ListaTipiMunizioniArma3;

	// Token: 0x0400026E RID: 622
	public List<GameObject> ListaTipiMunizioniArma4;

	// Token: 0x0400026F RID: 623
	public List<List<GameObject>> ListaMunizioneArmi;

	// Token: 0x04000270 RID: 624
	public GameObject tipoMunizioneAttivoArma1;

	// Token: 0x04000271 RID: 625
	public GameObject tipoMunizioneAttivoArma2;

	// Token: 0x04000272 RID: 626
	public GameObject tipoMunizioneAttivoArma3;

	// Token: 0x04000273 RID: 627
	public GameObject tipoMunizioneAttivoArma4;

	// Token: 0x04000274 RID: 628
	public List<GameObject> ListaMunizioniAttive;

	// Token: 0x04000275 RID: 629
	public bool ricaricaInCorsoArma1;

	// Token: 0x04000276 RID: 630
	public bool ricaricaInCorsoArma2;

	// Token: 0x04000277 RID: 631
	public bool ricaricaInCorsoArma3;

	// Token: 0x04000278 RID: 632
	public bool ricaricaInCorsoArma4;

	// Token: 0x04000279 RID: 633
	public List<bool> ListaRicaricheInCorso;

	// Token: 0x0400027A RID: 634
	public bool fuoriPortataArma1;

	// Token: 0x0400027B RID: 635
	public bool fuoriPortataArma2;

	// Token: 0x0400027C RID: 636
	public bool fuoriPortataArma3;

	// Token: 0x0400027D RID: 637
	public bool fuoriPortataArma4;

	// Token: 0x0400027E RID: 638
	public List<bool> ListaFuoriPortataArmi;

	// Token: 0x0400027F RID: 639
	public float vitaIniziale;

	// Token: 0x04000280 RID: 640
	public float carburanteIniziale;

	// Token: 0x04000281 RID: 641
	public bool richiamaAereo;

	// Token: 0x04000282 RID: 642
	public bool fuoriPortata;

	// Token: 0x04000283 RID: 643
	public bool rifornimentoAttivo;

	// Token: 0x04000284 RID: 644
	public bool riparazioneAttiva;

	// Token: 0x04000285 RID: 645
	public int numeroCoppieOrdigni;

	// Token: 0x04000286 RID: 646
	public List<GameObject> ListaOrdigniPossibili;

	// Token: 0x04000287 RID: 647
	public List<GameObject> ListaOrdigniAttivi;

	// Token: 0x04000288 RID: 648
	private GameObject CoppiaOrdigni01;

	// Token: 0x04000289 RID: 649
	private GameObject CoppiaOrdigni23;

	// Token: 0x0400028A RID: 650
	private GameObject CoppiaOrdigni45;

	// Token: 0x0400028B RID: 651
	private GameObject CoppiaOrdigni67;

	// Token: 0x0400028C RID: 652
	public List<float> ListaValoriOrdigno01;

	// Token: 0x0400028D RID: 653
	public List<float> ListaValoriOrdigno23;

	// Token: 0x0400028E RID: 654
	public List<float> ListaValoriOrdigno45;

	// Token: 0x0400028F RID: 655
	public List<float> ListaValoriOrdigno67;

	// Token: 0x04000290 RID: 656
	public List<int> ListaNumReintegrazioniOrdigni;

	// Token: 0x04000291 RID: 657
	public int numReintegrazione1;

	// Token: 0x04000292 RID: 658
	public int numReintegrazione2;

	// Token: 0x04000293 RID: 659
	public int numReintegrazione3;

	// Token: 0x04000294 RID: 660
	public int numReintegrazione4;

	// Token: 0x04000295 RID: 661
	public bool reintegrazioneNecessaria;

	// Token: 0x04000296 RID: 662
	public Vector3 luogoAttZonaBomb;

	// Token: 0x04000297 RID: 663
	public Vector3 luogoAttZonaArt;

	// Token: 0x04000298 RID: 664
	public GameObject unitàBersaglio;

	// Token: 0x04000299 RID: 665
	public GameObject testoVita;

	// Token: 0x0400029A RID: 666
	private GameObject cameraAttiva;

	// Token: 0x0400029B RID: 667
	private GameObject cerchioSel;

	// Token: 0x0400029C RID: 668
	private GameObject cerchioSel2;

	// Token: 0x0400029D RID: 669
	private float dimensioneQuadSel;

	// Token: 0x0400029E RID: 670
	private Material materialeSelezione;

	// Token: 0x0400029F RID: 671
	private Material materialeEvidenziazione;

	// Token: 0x040002A0 RID: 672
	private bool evidenziaAlleatiENemici;

	// Token: 0x040002A1 RID: 673
	private bool morto;

	// Token: 0x040002A2 RID: 674
	public bool giàSchierato;

	// Token: 0x040002A3 RID: 675
	public int posInQuadratoAerei;

	// Token: 0x040002A4 RID: 676
	private float timerRientroDaVolo;

	// Token: 0x040002A5 RID: 677
	private NavMeshAgent alleatoNav;

	// Token: 0x040002A6 RID: 678
	public bool èParà;

	// Token: 0x040002A7 RID: 679
	public bool èInLancio;

	// Token: 0x040002A8 RID: 680
	private bool paracèAperto;

	// Token: 0x040002A9 RID: 681
	public GameObject paracaduteChiuso;

	// Token: 0x040002AA RID: 682
	public GameObject paracaduteAperto;

	// Token: 0x040002AB RID: 683
	public GameObject paracadute;

	// Token: 0x040002AC RID: 684
	private GameObject paracaduteVero;

	// Token: 0x040002AD RID: 685
	public Vector3 posZainoParac;

	// Token: 0x040002AE RID: 686
	public Vector3 rotZainoParac;

	// Token: 0x040002AF RID: 687
	private float limiteMaxScalaParacChiuso;

	// Token: 0x040002B0 RID: 688
	private float timerDalLancio;

	// Token: 0x040002B1 RID: 689
	private float velocitàLancio;

	// Token: 0x040002B2 RID: 690
	public GameObject ossoArma;

	// Token: 0x040002B3 RID: 691
	private AudioSource suonoParacadute;

	// Token: 0x040002B4 RID: 692
	public AudioClip effettoVentoParàAperto;

	// Token: 0x040002B5 RID: 693
	private bool suonoParàPartito;

	// Token: 0x040002B6 RID: 694
	public bool ritornoEffettuato;

	// Token: 0x040002B7 RID: 695
	public bool toccaTerreno;

	// Token: 0x040002B8 RID: 696
	private bool allineatoConTerreno;

	// Token: 0x040002B9 RID: 697
	private RaycastHit hitTerreno;

	// Token: 0x040002BA RID: 698
	public float altezzaCentroUnità;

	// Token: 0x040002BB RID: 699
	private float timerContattoTerreno;

	// Token: 0x040002BC RID: 700
	public List<List<float>> ListaAvvelenamento;

	// Token: 0x040002BD RID: 701
	public float durataAvvelenamento;

	// Token: 0x040002BE RID: 702
	private float timerAvvelenamento;

	// Token: 0x040002BF RID: 703
	public List<GameObject> ListaNemInAttacco;

	// Token: 0x040002C0 RID: 704
	private bool eliminaParàDaAereo;

	// Token: 0x040002C1 RID: 705
	public GameObject quadSenzaMunizioni;

	// Token: 0x040002C2 RID: 706
	public GameObject quadMorte;

	// Token: 0x040002C3 RID: 707
	public GameObject quadSenzaMunizioni2;

	// Token: 0x040002C4 RID: 708
	public GameObject quadMorte2;

	// Token: 0x040002C5 RID: 709
	private float timerQuadMuniz;

	// Token: 0x040002C6 RID: 710
	private float timerVoceSenzaMuniz;

	// Token: 0x040002C7 RID: 711
	public bool estratto;

	// Token: 0x040002C8 RID: 712
	public float altezzaInSchier;

	// Token: 0x040002C9 RID: 713
	public float sensAerei;

	// Token: 0x040002CA RID: 714
	public int voloInvertito;

	// Token: 0x040002CB RID: 715
	public int ordineAereoAria;

	// Token: 0x040002CC RID: 716
	public int ordineAereoTerra;

	// Token: 0x040002CD RID: 717
	public int ordineAereoBomb;

	// Token: 0x040002CE RID: 718
	public bool tornaAllaBase;
}
