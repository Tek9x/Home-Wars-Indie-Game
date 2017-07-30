using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000EE RID: 238
public class InfoGenericheNemici : MonoBehaviour
{
	// Token: 0x060007B3 RID: 1971 RVA: 0x00112F9C File Offset: 0x0011119C
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		if (GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.livelloNest = 10;
		}
		this.ListaOrdinataNemiciDisp = new List<int>();
		for (int i = 0; i < this.ListaNemiciPossibili.Count; i++)
		{
			this.ListaOrdinataNemiciDisp.Add(0);
		}
		if ((this.tipoBattaglia == 0 | this.tipoBattaglia == 1) || this.tipoBattaglia == 2)
		{
			for (int j = 0; j < 48; j++)
			{
				if (this.ListaNemiciCaricati[j][0] != 100)
				{
					this.ListaOrdinataNemiciDisp[this.ListaNemiciCaricati[j][0]] = this.ListaNemiciCaricati[j][1];
				}
			}
		}
		else if (this.tipoBattaglia == 3 || this.tipoBattaglia == 4 || this.tipoBattaglia == 6 || this.tipoBattaglia == 7)
		{
			if (GestoreNeutroTattica.èBattagliaVeloce)
			{
				for (int k = 0; k < 48; k++)
				{
					if (this.ListaNemiciCaricati[k][0] != 100)
					{
						this.ListaOrdinataNemiciDisp[this.ListaNemiciCaricati[k][0]] = this.ListaNemiciCaricati[k][1];
					}
				}
			}
			else
			{
				for (int l = 0; l < this.ListaNemiciPossibili.Count; l++)
				{
					if (l <= this.ListaTipoMaxPerOgniLvNest[this.livelloNest - 1])
					{
						this.ListaOrdinataNemiciDisp[l] = 20;
					}
					else
					{
						this.ListaOrdinataNemiciDisp[l] = 0;
					}
				}
			}
		}
		else if (this.tipoBattaglia == 5)
		{
			if (GestoreNeutroTattica.èBattagliaVeloce)
			{
				for (int m = 0; m < 48; m++)
				{
					if (this.ListaNemiciCaricati[m][0] != 100)
					{
						this.ListaOrdinataNemiciDisp[this.ListaNemiciCaricati[m][0]] = this.ListaNemiciCaricati[m][1];
					}
				}
			}
			else
			{
				this.ListaTipiPerBatt5 = new List<int>();
				this.ListaTipiPerBatt5.Add(1);
				this.ListaTipiPerBatt5.Add(3);
				this.ListaTipiPerBatt5.Add(6);
				this.ListaTipiPerBatt5.Add(11);
				this.ListaTipiPerBatt5.Add(12);
				this.ListaTipiPerBatt5.Add(17);
				this.ListaTipiPerBatt5.Add(18);
				this.ListaTipiPerBatt5.Add(19);
				this.ListaTipiPerBatt5.Add(29);
				this.ListaTipiPerBatt5.Add(30);
				for (int n = 0; n < this.ListaNemiciPossibili.Count; n++)
				{
					bool flag = false;
					int num = 0;
					while (num < this.ListaTipiPerBatt5.Count && !flag)
					{
						if (n == this.ListaTipiPerBatt5[num] && n <= this.ListaTipoMaxPerOgniLvNest[this.livelloNest - 1])
						{
							this.ListaOrdinataNemiciDisp[n] = 30;
							flag = true;
						}
						else if (num == this.ListaTipiPerBatt5.Count - 1)
						{
							this.ListaOrdinataNemiciDisp[n] = 0;
						}
						num++;
					}
				}
			}
		}
		if (!GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.fattoreVitaNemici = PlayerPrefs.GetFloat("vita nemici");
			this.fattoreAttaccoNemici = PlayerPrefs.GetFloat("attacco nemici");
			this.numMaxNemici = PlayerPrefs.GetInt("max nemici");
		}
		for (int num2 = 0; num2 < 33; num2++)
		{
			if (this.ListaOrdinataNemiciDisp[num2] > 0)
			{
				this.ListaNemPresInBatt[num2] = 1;
			}
		}
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x001133F8 File Offset: 0x001115F8
	private void Update()
	{
		if (!this.quantitàStabilità && this.infoAlleati.GetComponent<GestioneComandanteInUI>().fineCountdown)
		{
			this.quantitàStabilità = true;
		}
		this.GestioneTipi();
		this.ListeEValutazioneNemiciDisponibili();
		if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().battagliaTerminata)
		{
			this.CompListaPostBattaglia();
		}
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x00113454 File Offset: 0x00111654
	private void GestioneTipi()
	{
		this.ListaTipoVolante = new List<int>();
		this.ListaTipoNonVolante = new List<int>();
		this.ListaTipoSaltante = new List<int>();
		this.numRinforziNemici = 0;
		for (int i = 0; i < this.ListaNemiciPossibili.Count; i++)
		{
			if (this.ListaOrdinataNemiciDisp[i] > 0)
			{
				if (this.ListaNemiciPossibili[i].GetComponent<PresenzaNemico>().insettoVolante)
				{
					this.ListaTipoVolante.Add(i);
				}
				else if (this.ListaNemiciPossibili[i].GetComponent<PresenzaNemico>().èSaltatore)
				{
					this.ListaTipoSaltante.Add(i);
					this.ListaTipoNonVolante.Add(i);
				}
				else
				{
					this.ListaTipoNonVolante.Add(i);
				}
				this.numRinforziNemici += this.ListaOrdinataNemiciDisp[i];
			}
		}
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x00113544 File Offset: 0x00111744
	private void ListeEValutazioneNemiciDisponibili()
	{
		this.ListaNemiciVolanti = new List<GameObject>();
		this.ListaNemiciNonVolanti = new List<GameObject>();
		foreach (GameObject current in this.ListaNemici)
		{
			if (current != null)
			{
				if (current.GetComponent<PresenzaNemico>().insettoVolante)
				{
					this.ListaNemiciVolanti.Add(current);
				}
				else
				{
					this.ListaNemiciNonVolanti.Add(current);
				}
			}
		}
		this.timerAggComport += Time.deltaTime;
		if (this.timerAggComport > 5f)
		{
			this.timerAggComport = 0f;
			this.numNemiciInComportAObb = 0;
			for (int i = 0; i < this.ListaNemici.Count; i++)
			{
				if (this.ListaNemici[i] && this.ListaNemici[i].GetComponent<PresenzaNemico>().tipoComportamento == 0)
				{
					this.numNemiciInComportAObb++;
				}
			}
		}
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x00113684 File Offset: 0x00111884
	private void CompListaPostBattaglia()
	{
		if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().battagliaTerminata && !this.contaRimanenti)
		{
			this.contaRimanenti = true;
			List<int> list = new List<int>();
			for (int i = 0; i < this.ListaNemiciPossibili.Count; i++)
			{
				list.Add(0);
			}
			foreach (GameObject current in this.ListaNemici)
			{
				if (current != null)
				{
					List<int> list2;
					List<int> expr_76 = list2 = list;
					int num;
					int expr_84 = num = current.GetComponent<PresenzaNemico>().tipoInsetto;
					num = list2[num];
					expr_76[expr_84] = num + 1;
				}
			}
			this.ListaNemiciPostBattaglia = new List<int>();
			for (int j = 0; j < this.ListaNemiciPossibili.Count; j++)
			{
				this.ListaNemiciPostBattaglia.Add(0);
			}
			bool flag = false;
			int num2 = 0;
			while (num2 < this.ListaNemiciPossibili.Count && !flag)
			{
				List<int> listaNemiciPostBattaglia;
				List<int> expr_100 = listaNemiciPostBattaglia = this.ListaNemiciPostBattaglia;
				int num;
				int expr_105 = num = num2;
				num = listaNemiciPostBattaglia[num];
				expr_100[expr_105] = num + this.ListaOrdinataNemiciDisp[num2];
				List<int> listaNemiciPostBattaglia2;
				List<int> expr_12E = listaNemiciPostBattaglia2 = this.ListaNemiciPostBattaglia;
				int expr_133 = num = num2;
				num = listaNemiciPostBattaglia2[num];
				expr_12E[expr_133] = num + Mathf.CeilToInt((float)(list[num2] / this.ListaNemiciPossibili[num2].GetComponent<PresenzaNemico>().numMembriGruppo));
				num2++;
			}
		}
	}

	// Token: 0x04001CDF RID: 7391
	private GameObject scrittaNemiciRimanentiUI;

	// Token: 0x04001CE0 RID: 7392
	private GameObject infoNeutreTattica;

	// Token: 0x04001CE1 RID: 7393
	private GameObject primaCamera;

	// Token: 0x04001CE2 RID: 7394
	private GameObject infoAlleati;

	// Token: 0x04001CE3 RID: 7395
	public List<GameObject> ListaNemici;

	// Token: 0x04001CE4 RID: 7396
	public List<GameObject> ListaNemiciVolanti;

	// Token: 0x04001CE5 RID: 7397
	public List<GameObject> ListaNemiciNonVolanti;

	// Token: 0x04001CE6 RID: 7398
	public int numMaxNemici;

	// Token: 0x04001CE7 RID: 7399
	public List<List<int>> ListaNemiciCaricati;

	// Token: 0x04001CE8 RID: 7400
	public List<GameObject> ListaNemiciPossibili;

	// Token: 0x04001CE9 RID: 7401
	public int numeroTipiNemiciDisp;

	// Token: 0x04001CEA RID: 7402
	public List<int> ListaOrdinataNemiciDisp;

	// Token: 0x04001CEB RID: 7403
	public List<int> ListaTipoVolante;

	// Token: 0x04001CEC RID: 7404
	public List<int> ListaTipoNonVolante;

	// Token: 0x04001CED RID: 7405
	public List<int> ListaTipoSaltante;

	// Token: 0x04001CEE RID: 7406
	public List<int> ListaNemiciPostBattaglia;

	// Token: 0x04001CEF RID: 7407
	private bool contaRimanenti;

	// Token: 0x04001CF0 RID: 7408
	public int numeroTotNemiciRimanenti;

	// Token: 0x04001CF1 RID: 7409
	public int tipoBattaglia;

	// Token: 0x04001CF2 RID: 7410
	private bool quantitàStabilità;

	// Token: 0x04001CF3 RID: 7411
	public List<AudioClip> ListaSuoniInsetti;

	// Token: 0x04001CF4 RID: 7412
	private List<int> ListaTipiPerBatt5;

	// Token: 0x04001CF5 RID: 7413
	public List<int> ListaTipoMaxPerOgniLvNest;

	// Token: 0x04001CF6 RID: 7414
	public int livelloNest;

	// Token: 0x04001CF7 RID: 7415
	public float fattoreVitaNemici;

	// Token: 0x04001CF8 RID: 7416
	public float fattoreAttaccoNemici;

	// Token: 0x04001CF9 RID: 7417
	public int numRinforziNemici;

	// Token: 0x04001CFA RID: 7418
	private float timerAggComport;

	// Token: 0x04001CFB RID: 7419
	public int numNemiciInComportAObb;

	// Token: 0x04001CFC RID: 7420
	public List<int> ListaNemPresInBatt;

	// Token: 0x04001CFD RID: 7421
	public float bonusSurvivalSalute;

	// Token: 0x04001CFE RID: 7422
	public float bonusSurvivalAttacco;

	// Token: 0x04001CFF RID: 7423
	public float totalePerExpBatt;
}
