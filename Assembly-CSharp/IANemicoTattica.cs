using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class IANemicoTattica : MonoBehaviour
{
	// Token: 0x060007AD RID: 1965 RVA: 0x001123CC File Offset: 0x001105CC
	private void Start()
	{
		this.varieMappaLocale = GameObject.FindGameObjectWithTag("VarieMappaLocale");
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.numMaxNemici = base.GetComponent<InfoGenericheNemici>().numMaxNemici;
		if (this.tipoBattaglia == 0)
		{
			if (this.tipoDiOrda == 0 || this.tipoDiOrda == 5 || this.tipoDiOrda == 13 || this.tipoDiOrda == 16 || this.tipoDiOrda == 27)
			{
				this.tempoDiSpawnIniziale = 0.08f;
			}
			else
			{
				this.tempoDiSpawnIniziale = 0.12f;
			}
			this.frazioneAObbiettivo = Mathf.FloorToInt((float)(this.numMaxNemici / 3));
		}
		else if (this.tipoBattaglia == 1)
		{
			if (this.tipoDiOrda == 0 || this.tipoDiOrda == 5 || this.tipoDiOrda == 13 || this.tipoDiOrda == 16 || this.tipoDiOrda == 27)
			{
				this.tempoDiSpawnIniziale = 0.1f;
			}
			else
			{
				this.tempoDiSpawnIniziale = 0.14f;
			}
			this.raggioPerRicercaDistDaLuogo = 100f;
			this.frazioneAObbiettivo = Mathf.FloorToInt((float)(this.numMaxNemici / 10));
		}
		else if (this.tipoBattaglia == 2)
		{
			if (this.tipoDiOrda == 0 || this.tipoDiOrda == 5 || this.tipoDiOrda == 13 || this.tipoDiOrda == 16 || this.tipoDiOrda == 27)
			{
				this.tempoDiSpawnIniziale = 0.14f;
			}
			else
			{
				this.tempoDiSpawnIniziale = 0.2f;
			}
			this.frazioneAObbiettivo = Mathf.FloorToInt((float)(this.numMaxNemici / 3));
		}
		else if (this.tipoBattaglia == 3)
		{
			this.tempoDiSpawnIniziale = 0.5f;
			this.frazioneAObbiettivo = Mathf.FloorToInt((float)(this.numMaxNemici / 3));
		}
		else if (this.tipoBattaglia == 4)
		{
			this.tempoDiSpawnIniziale = 0.4f;
			this.frazioneAObbiettivo = Mathf.FloorToInt((float)(this.numMaxNemici / 3));
		}
		else if (this.tipoBattaglia == 5)
		{
			this.tempoDiSpawnIniziale = 0.7f;
			this.frazioneAObbiettivo = Mathf.FloorToInt((float)(this.numMaxNemici / 15));
		}
		else if (this.tipoBattaglia == 6)
		{
			this.tempoDiSpawnIniziale = 0.15f;
			this.frazioneAObbiettivo = Mathf.FloorToInt((float)(this.numMaxNemici / 4));
		}
		else if (this.tipoBattaglia == 7)
		{
			this.tempoDiSpawnIniziale = 0.15f;
			this.raggioPerRicercaDistDaLuogo = 15f;
			this.frazioneAObbiettivo = Mathf.FloorToInt((float)(this.numMaxNemici / 8));
		}
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x00112698 File Offset: 0x00110898
	private void Update()
	{
		this.timerProssComport += Time.deltaTime;
		this.timerDiSpawn += Time.deltaTime;
		if (!this.spawnImpostati && this.infoAlleati.GetComponent<GestioneComandanteInUI>().fineCountdown)
		{
			this.spawnImpostati = true;
			this.InizializzazioneSpawn();
		}
		if (base.GetComponent<InfoGenericheNemici>().ListaNemici.Count >= this.numMaxNemici)
		{
			this.stopSpawn = true;
		}
		else if (base.GetComponent<InfoGenericheNemici>().ListaTipoNonVolante.Count == 0 && base.GetComponent<InfoGenericheNemici>().ListaTipoVolante.Count == 0)
		{
			this.stopSpawn = true;
		}
		else
		{
			this.stopSpawn = false;
		}
		if (this.infoAlleati.GetComponent<GestioneComandanteInUI>().fineCountdown && !this.stopSpawn)
		{
			this.SpawnGenerale();
		}
		if (this.tipoBattaglia == 1 || this.tipoBattaglia == 7)
		{
			this.timerAggCompilListeLuogo += Time.deltaTime;
			if (this.timerAggCompilListeLuogo > 2f)
			{
				this.timerAggCompilListeLuogo = 0f;
				this.CompilamentoListaAllViciniALuogo();
			}
		}
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x001127CC File Offset: 0x001109CC
	private void InizializzazioneSpawn()
	{
		this.ListaPuntiSpawn = new List<GameObject>();
		for (int i = 0; i < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento.Count; i++)
		{
			if (this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[i].transform.GetChild(0).tag == "AreaSchieramentoNemico")
			{
				for (int j = 1; j < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[i].transform.childCount; j++)
				{
					this.ListaPuntiSpawn.Add(this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[i].transform.GetChild(j).gameObject);
				}
			}
		}
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x001128A4 File Offset: 0x00110AA4
	private void CompilamentoListaAllViciniALuogo()
	{
		if (this.tipoBattaglia == 1)
		{
			this.ListaAlleatiSuObb = new List<GameObject>();
			foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati)
			{
				if (current != null && Vector3.Distance(current.transform.position, this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoNemico.transform.position) < this.raggioPerRicercaDistDaLuogo)
				{
					this.ListaAlleatiSuObb.Add(current);
				}
			}
		}
		else if (this.tipoBattaglia == 7)
		{
			this.ListaAlleatiSuObb = new List<GameObject>();
			GameObject gameObject = null;
			for (int i = 0; i < this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaPanePerConvoglio.Count; i++)
			{
				if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaPanePerConvoglio[i] != null && this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaPanePerConvoglio[i].GetComponent<ObbiettivoTatticoScript>().vita > 0f)
				{
					gameObject = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaPanePerConvoglio[i];
					break;
				}
			}
			if (gameObject != null)
			{
				foreach (GameObject current2 in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati)
				{
					if (current2 != null && Vector3.Distance(current2.transform.position, gameObject.transform.position) < this.raggioPerRicercaDistDaLuogo)
					{
						this.ListaAlleatiSuObb.Add(current2);
					}
				}
			}
		}
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x00112AC4 File Offset: 0x00110CC4
	private void SpawnGenerale()
	{
		if (this.timerDiSpawn > this.tempoDiSpawn)
		{
			this.percTipoSpawn = 0.55f;
			int index = UnityEngine.Random.Range(0, this.ListaPuntiSpawn.Count);
			GestoreNeutroStrategia.valoreRandomSeed++;
			UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
			Vector3 vector = this.ListaPuntiSpawn[index].transform.position;
			Vector3 euler = base.transform.TransformDirection(this.ListaPuntiSpawn[index].transform.forward);
			if (this.membriRimDelGrupInCorso <= 0)
			{
				if (!this.eliminoIlGruppo)
				{
					List<int> listaOrdinataNemiciDisp;
					List<int> expr_9F = listaOrdinataNemiciDisp = base.GetComponent<InfoGenericheNemici>().ListaOrdinataNemiciDisp;
					int num;
					int expr_A8 = num = this.tipoDelGruppoInCorso;
					num = listaOrdinataNemiciDisp[num];
					expr_9F[expr_A8] = num - 1;
					this.eliminoIlGruppo = true;
				}
				float num2 = UnityEngine.Random.Range(0f, 1f);
				if (num2 <= this.percTipoSpawn && base.GetComponent<InfoGenericheNemici>().ListaTipoNonVolante.Count > 0)
				{
					int index2 = UnityEngine.Random.Range(0, base.GetComponent<InfoGenericheNemici>().ListaTipoNonVolante.Count);
					if (base.GetComponent<InfoGenericheNemici>().ListaOrdinataNemiciDisp[base.GetComponent<InfoGenericheNemici>().ListaTipoNonVolante[index2]] > 0)
					{
						this.tipoDelGruppoInCorso = base.GetComponent<InfoGenericheNemici>().ListaTipoNonVolante[index2];
						this.tempoDiSpawn = this.tempoDiSpawnIniziale * base.GetComponent<InfoGenericheNemici>().ListaNemiciPossibili[this.tipoDelGruppoInCorso].GetComponent<PresenzaNemico>().tempoSpawnGruppo;
						this.membriRimDelGrupInCorso = base.GetComponent<InfoGenericheNemici>().ListaNemiciPossibili[this.tipoDelGruppoInCorso].GetComponent<PresenzaNemico>().numMembriGruppo;
					}
				}
				else if (base.GetComponent<InfoGenericheNemici>().ListaTipoVolante.Count > 0)
				{
					int index2 = UnityEngine.Random.Range(0, base.GetComponent<InfoGenericheNemici>().ListaTipoVolante.Count);
					if (base.GetComponent<InfoGenericheNemici>().ListaOrdinataNemiciDisp[base.GetComponent<InfoGenericheNemici>().ListaTipoVolante[index2]] > 0)
					{
						this.tipoDelGruppoInCorso = base.GetComponent<InfoGenericheNemici>().ListaTipoVolante[index2];
						this.tempoDiSpawn = this.tempoDiSpawnIniziale * base.GetComponent<InfoGenericheNemici>().ListaNemiciPossibili[this.tipoDelGruppoInCorso].GetComponent<PresenzaNemico>().tempoSpawnGruppo;
						this.membriRimDelGrupInCorso = base.GetComponent<InfoGenericheNemici>().ListaNemiciPossibili[this.tipoDelGruppoInCorso].GetComponent<PresenzaNemico>().numMembriGruppo;
					}
				}
			}
			else
			{
				if (base.GetComponent<InfoGenericheNemici>().ListaNemiciPossibili[this.tipoDelGruppoInCorso].GetComponent<PresenzaNemico>().insettoVolante)
				{
					float d = UnityEngine.Random.Range(50f, 120f);
					vector += Vector3.up * d;
				}
				this.insettoCreato = (UnityEngine.Object.Instantiate(base.GetComponent<InfoGenericheNemici>().ListaNemiciPossibili[this.tipoDelGruppoInCorso], vector, Quaternion.Euler(euler)) as GameObject);
				this.membriRimDelGrupInCorso--;
				this.timerDiSpawn = 0f;
				this.eliminoIlGruppo = false;
				if (this.tipoBattaglia == 0)
				{
					Vector3 a = Vector3.Lerp(vector, this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.transform.position, 0.5f);
					if (Vector3.Distance(a, Vector3.zero) < 400f)
					{
						this.insettoCreato.GetComponent<PresenzaNemico>().sparpagliato = true;
					}
				}
				if (this.tipoBattaglia == 1)
				{
					this.insettoCreato.GetComponent<PresenzaNemico>().tipoComportamento = 2;
				}
				else if (this.tipoBattaglia == 3)
				{
					this.insettoCreato.GetComponent<PresenzaNemico>().tipoComportamento = 1;
				}
				else if (this.insettoCreato.GetComponent<PresenzaNemico>().insettoVolante)
				{
					if (this.tipoBattaglia == 5)
					{
						if (base.GetComponent<InfoGenericheNemici>().numNemiciInComportAObb < this.frazioneAObbiettivo)
						{
							this.insettoCreato.GetComponent<PresenzaNemico>().tipoComportamento = 0;
						}
						else
						{
							this.insettoCreato.GetComponent<PresenzaNemico>().tipoComportamento = 1;
						}
					}
					else
					{
						float value = UnityEngine.Random.value;
						if (value < 0.2f)
						{
							this.insettoCreato.GetComponent<PresenzaNemico>().tipoComportamento = 0;
						}
						else
						{
							this.insettoCreato.GetComponent<PresenzaNemico>().tipoComportamento = 1;
						}
					}
				}
				else if (base.GetComponent<InfoGenericheNemici>().numNemiciInComportAObb < this.frazioneAObbiettivo)
				{
					GestoreNeutroStrategia.valoreRandomSeed++;
					UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
					float value2 = UnityEngine.Random.value;
					if (value2 < 0.75f)
					{
						this.insettoCreato.GetComponent<PresenzaNemico>().tipoComportamento = 0;
					}
					else
					{
						this.insettoCreato.GetComponent<PresenzaNemico>().tipoComportamento = 1;
					}
				}
				else
				{
					this.insettoCreato.GetComponent<PresenzaNemico>().tipoComportamento = 1;
				}
			}
		}
	}

	// Token: 0x04001CC3 RID: 7363
	private GameObject varieMappaLocale;

	// Token: 0x04001CC4 RID: 7364
	private GameObject infoNeutreTattica;

	// Token: 0x04001CC5 RID: 7365
	private GameObject infoAlleati;

	// Token: 0x04001CC6 RID: 7366
	public List<GameObject> ListaPuntiSpawn;

	// Token: 0x04001CC7 RID: 7367
	private bool spawnImpostati;

	// Token: 0x04001CC8 RID: 7368
	private float tempoDiSpawn;

	// Token: 0x04001CC9 RID: 7369
	private float tempoDiSpawnIniziale;

	// Token: 0x04001CCA RID: 7370
	private float timerDiSpawn;

	// Token: 0x04001CCB RID: 7371
	private GameObject insettoCreato;

	// Token: 0x04001CCC RID: 7372
	public int tipoBattaglia;

	// Token: 0x04001CCD RID: 7373
	private float tempoProssComport;

	// Token: 0x04001CCE RID: 7374
	private float timerProssComport;

	// Token: 0x04001CCF RID: 7375
	private float timerRiposoComportAntiAereo;

	// Token: 0x04001CD0 RID: 7376
	private float timerRiposoComportAntiArtiglieria;

	// Token: 0x04001CD1 RID: 7377
	private float timerRiposoComportAntiGeniere;

	// Token: 0x04001CD2 RID: 7378
	private int comportamentoTattico;

	// Token: 0x04001CD3 RID: 7379
	private int numMaxNemici;

	// Token: 0x04001CD4 RID: 7380
	public float raggioPerRicercaDistDaLuogo;

	// Token: 0x04001CD5 RID: 7381
	public List<GameObject> ListaAlleatiInAvamp;

	// Token: 0x04001CD6 RID: 7382
	public List<GameObject> ListaAlleatiSuObb;

	// Token: 0x04001CD7 RID: 7383
	private int tipoDelGruppoInCorso;

	// Token: 0x04001CD8 RID: 7384
	private int membriRimDelGrupInCorso;

	// Token: 0x04001CD9 RID: 7385
	private bool stopSpawn;

	// Token: 0x04001CDA RID: 7386
	private float percTipoSpawn;

	// Token: 0x04001CDB RID: 7387
	public int frazioneAObbiettivo;

	// Token: 0x04001CDC RID: 7388
	private float timerAggCompilListeLuogo;

	// Token: 0x04001CDD RID: 7389
	private bool eliminoIlGruppo;

	// Token: 0x04001CDE RID: 7390
	public int tipoDiOrda;
}
