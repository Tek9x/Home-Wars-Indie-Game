using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000B RID: 11
public class InfoGenericheAlleati : MonoBehaviour
{
	// Token: 0x06000051 RID: 81 RVA: 0x00012934 File Offset: 0x00010B34
	private void Awake()
	{
		PlayerPrefs.SetFloat("moltiplicatore danni PP", 2f);
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00012948 File Offset: 0x00010B48
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.primaCamera = GameObject.FindGameObjectWithTag("MainCamera");
		this.varieMappaLocale = GameObject.FindGameObjectWithTag("VarieMappaLocale");
		this.sfondoQuadroAerei = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Quadro Aerei").FindChild("sfondo quadrato").gameObject;
		this.attacchiAlleatiSpeciali = GameObject.FindGameObjectWithTag("Attacchi Speciali Alleati");
		this.pulsanteRichiamaAereo = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Quadro Aerei").FindChild("richiamo aerei").gameObject;
		this.scrittaPuntiBattaglia = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Barra Superiore").FindChild("Punti Battaglia").GetChild(1).gameObject;
		this.barraUnitàSelez = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Barra Unità Selezionate").gameObject;
		this.insiemeGruppi = this.barraUnitàSelez.transform.FindChild("SopraRettangolo").FindChild("insieme Gruppi").gameObject;
		this.modalitàUnitàSingola = this.barraUnitàSelez.transform.GetChild(0).transform.GetChild(0).gameObject;
		this.modalitàUnitàMultipla = this.barraUnitàSelez.transform.GetChild(0).transform.GetChild(1).gameObject;
		this.scrittaCancellaTrappola = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Scritte Varie").FindChild("cancella trappola").gameObject;
		this.tipoBattaglia = GestoreNeutroStrategia.tipoBattaglia;
		this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().tipoBattaglia = this.tipoBattaglia;
		this.IANemico.GetComponent<InfoGenericheNemici>().tipoBattaglia = this.tipoBattaglia;
		this.IANemico.GetComponent<IANemicoTattica>().tipoBattaglia = this.tipoBattaglia;
		base.GetComponent<InfoMunizionamento>().tipoBattaglia = this.tipoBattaglia;
		base.GetComponent<GestioneComandanteInUI>().tipoBattaglia = this.tipoBattaglia;
		this.ListaDisponibilitàAerei = new List<bool>();
		for (int i = 0; i < 9; i++)
		{
			this.ListaDisponibilitàAerei.Add(false);
		}
		this.ListaAereiInRifor = new List<bool>();
		for (int j = 0; j < 9; j++)
		{
			this.ListaAereiInRifor.Add(false);
		}
		this.ListaAereiInVolo = new List<bool>();
		for (int k = 0; k < 9; k++)
		{
			this.ListaAereiInVolo.Add(false);
		}
		this.ListaTimerRiforAerei = new List<float>();
		for (int l = 0; l < 9; l++)
		{
			this.ListaTimerRiforAerei.Add(0f);
		}
		if (!GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.numMaxAlleati = PlayerPrefs.GetInt("max alleati");
		}
		this.ListaTimerSuoniArmi = new List<List<float>>();
		for (int m = 0; m < this.ListaAlleatiPossibili.Count; m++)
		{
			List<float> list = new List<float>();
			if (m == 15 || m == 17 || m == 18 || m == 23 || m == 24)
			{
				list.Add(0f);
				list.Add(0f);
			}
			else
			{
				list.Add(0f);
			}
			this.ListaTimerSuoniArmi.Add(list);
		}
		this.ListaGruppo0 = new List<GameObject>();
		this.ListaGruppo1 = new List<GameObject>();
		this.ListaGruppo2 = new List<GameObject>();
		this.ListaGruppo3 = new List<GameObject>();
		this.ListaGruppo4 = new List<GameObject>();
		this.ListaGruppo5 = new List<GameObject>();
		this.ListaGruppo6 = new List<GameObject>();
		this.ListaGruppo7 = new List<GameObject>();
		this.ListaGruppo8 = new List<GameObject>();
		this.ListaGruppo9 = new List<GameObject>();
		this.ListaDiGruppi = new List<List<GameObject>>();
		this.ListaDiGruppi.Add(this.ListaGruppo0);
		this.ListaDiGruppi.Add(this.ListaGruppo1);
		this.ListaDiGruppi.Add(this.ListaGruppo2);
		this.ListaDiGruppi.Add(this.ListaGruppo3);
		this.ListaDiGruppi.Add(this.ListaGruppo4);
		this.ListaDiGruppi.Add(this.ListaGruppo5);
		this.ListaDiGruppi.Add(this.ListaGruppo6);
		this.ListaDiGruppi.Add(this.ListaGruppo7);
		this.ListaDiGruppi.Add(this.ListaGruppo8);
		this.ListaDiGruppi.Add(this.ListaGruppo9);
		this.gruppoPremuto = 999;
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00012DF8 File Offset: 0x00010FF8
	private void Update()
	{
		if (!this.primoFramePassato)
		{
			this.primoFramePassato = true;
			this.PrimoFrame();
		}
		this.ArrayAlleati = GameObject.FindGameObjectsWithTag("Alleato");
		this.ListaAlleati = this.ArrayAlleati.ToList<GameObject>();
		if (this.ListaAlleati.Count > 0)
		{
			this.CompletamentoListeAlleati();
		}
		this.GestioneAerei();
		if (base.GetComponent<GestioneComandanteInUI>().partenzaAereoParà)
		{
			base.GetComponent<GestioneComandanteInUI>().partenzaAereoParà = false;
			this.PerParacadutisti();
		}
		if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().battagliaTerminata)
		{
			this.CompListaPostBattaglia();
		}
		if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva != 3)
		{
			this.GestioneGruppi();
		}
		this.GestioneListePerSuoni();
		this.Varie();
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00012EC0 File Offset: 0x000110C0
	private void PrimoFrame()
	{
		if (this.tipoBattaglia == 3 && GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.ListaAlleatiDisponibili = new List<List<int>>();
			for (int i = 0; i < 48; i++)
			{
				List<int> list = new List<int>();
				if (i == 38 || i == 39 || i == 40 || i == 41)
				{
					list.Add(i);
					list.Add(4);
					this.ListaAlleatiDisponibili.Add(list);
				}
				else if (i == 42)
				{
					list.Add(i);
					list.Add(3);
					this.ListaAlleatiDisponibili.Add(list);
				}
				else if (i == 43)
				{
					list.Add(i);
					list.Add(1);
					this.ListaAlleatiDisponibili.Add(list);
				}
				else if (i == 45 || i == 46)
				{
					list.Add(i);
					list.Add(1);
					this.ListaAlleatiDisponibili.Add(list);
				}
				else
				{
					list.Add(100);
					list.Add(0);
					this.ListaAlleatiDisponibili.Add(list);
				}
			}
		}
		else if (this.tipoBattaglia == 5 && GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.ListaAlleatiDisponibili = new List<List<int>>();
			for (int j = 0; j < 48; j++)
			{
				List<int> list2 = new List<int>();
				if (j == 38 || j == 39)
				{
					list2.Add(j);
					list2.Add(20);
					this.ListaAlleatiDisponibili.Add(list2);
				}
				if (j == 45)
				{
					list2.Add(j);
					list2.Add(1);
					this.ListaAlleatiDisponibili.Add(list2);
				}
				else
				{
					list2.Add(100);
					list2.Add(0);
					this.ListaAlleatiDisponibili.Add(list2);
				}
			}
		}
		for (int k = 0; k < 48; k++)
		{
			if (this.ListaAlleatiDisponibili[k][1] > 0)
			{
				this.ListaAllPresInBatt[this.ListaAlleatiDisponibili[k][0]] = 1;
			}
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x000130DC File Offset: 0x000112DC
	private void CompletamentoListeAlleati()
	{
		for (int i = 0; i < this.ListaAlleati.Count; i++)
		{
			if (this.ListaAlleati[i] == null)
			{
				this.ListaAlleati[i] = this.ListaAlleati[this.ListaAlleati.Count - 1];
				this.ListaAlleati.Remove(this.ListaAlleati[this.ListaAlleati.Count - 1]);
			}
		}
		this.ListaèVolante = new List<GameObject>();
		this.ListaèNonVolante = new List<GameObject>();
		this.ListaèFanteria = new List<GameObject>();
		this.ListaèMezzo = new List<GameObject>();
		this.ListaèElicottero = new List<GameObject>();
		this.ListaèAereo = new List<GameObject>();
		this.ListaèArtiglieria = new List<GameObject>();
		this.ListaèPerRifornimento = new List<GameObject>();
		this.ListaèGeniere = new List<GameObject>();
		foreach (GameObject current in this.ListaAlleati)
		{
			if (current != null)
			{
				if (current.GetComponent<PresenzaAlleato>().volante)
				{
					this.ListaèVolante.Add(current);
				}
				if (!current.GetComponent<PresenzaAlleato>().volante)
				{
					this.ListaèNonVolante.Add(current);
				}
				if (current.GetComponent<PresenzaAlleato>().èFanteria)
				{
					this.ListaèFanteria.Add(current);
				}
				if (current.GetComponent<PresenzaAlleato>().èMezzo)
				{
					this.ListaèMezzo.Add(current);
				}
				if (current.GetComponent<PresenzaAlleato>().èElicottero)
				{
					this.ListaèElicottero.Add(current);
				}
				if (current.GetComponent<PresenzaAlleato>().èAereo)
				{
					this.ListaèAereo.Add(current);
				}
				if (current.GetComponent<PresenzaAlleato>().èArtiglieria)
				{
					this.ListaèArtiglieria.Add(current);
				}
				if (current.GetComponent<PresenzaAlleato>().èPerRifornimento)
				{
					this.ListaèPerRifornimento.Add(current);
				}
				if (current.GetComponent<PresenzaAlleato>().èGeniere)
				{
					this.ListaèGeniere.Add(current);
				}
			}
		}
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00013324 File Offset: 0x00011524
	private void GestioneAerei()
	{
		if (this.ricreaAereoInQuadro)
		{
			this.ListaAereiInQuadrato[this.postoInCuiRicreare] = this.ListaAlleatiPossibili[this.tipoAereoDaRicreareInQuadro];
			this.ricreaAereoInQuadro = false;
		}
		if (this.cliccatoSuQuadroAerei)
		{
			if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().schieramentoAttivo)
			{
				if (!this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().faseSchierInizTerminata)
				{
					base.GetComponent<GestioneComandanteInUI>().tipoTruppaDaCancellare = this.ListaAereiInQuadrato[this.numAereoInQuadro].GetComponent<PresenzaAlleato>().tipoTruppa;
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().puntiBattaglia += this.ListaAereiInQuadrato[this.numAereoInQuadro].GetComponent<PresenzaAlleato>().costoInPlastica / 10f;
					List<int> list;
					List<int> expr_ED = list = this.ListaAlleatiDisponibili[this.ListaAereiInQuadrato[this.numAereoInQuadro].GetComponent<PresenzaAlleato>().tipoTruppa];
					int num;
					int expr_F1 = num = 1;
					num = list[num];
					expr_ED[expr_F1] = num + 1;
					this.ListaDisponibilitàAerei[this.numAereoInQuadro] = false;
					this.ListaAereiInQuadrato[this.numAereoInQuadro] = null;
					base.GetComponent<GestioneComandanteInUI>().cancRinforzoSchierato = true;
					base.GetComponent<GestioneComandanteInUI>().aggiornaElencoRinforzi = true;
				}
			}
			else if (this.ListaDisponibilitàAerei[this.numAereoInQuadro])
			{
				if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().faseSchierInizTerminata && this.ListaAereiInQuadrato[this.numAereoInQuadro].GetComponent<PresenzaAlleato>().tipoTruppaVolante != 10)
				{
					Vector3 position = Vector3.zero;
					Vector3 zero = Vector3.zero;
					Quaternion identity = Quaternion.identity;
					bool flag = false;
					int num2 = 0;
					while (num2 < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento.Count && !flag)
					{
						if (this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num2].transform.GetChild(0).tag == "AreaSchieramentoAlleato")
						{
							GameObject gameObject = this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num2];
							float d = UnityEngine.Random.Range(-75f, 75f);
							position = gameObject.transform.position + Vector3.up * this.ListaAereiInQuadrato[this.numAereoInQuadro].GetComponent<PresenzaAlleato>().quotaDiPartenza + gameObject.transform.right * d;
							this.dirSpawnAereo = gameObject.transform.forward;
							this.aereoAppenaSpawnato = true;
							flag = true;
						}
						num2++;
					}
					GameObject gameObject2 = UnityEngine.Object.Instantiate(this.ListaAereiInQuadrato[this.numAereoInQuadro], position, Quaternion.identity) as GameObject;
					gameObject2.GetComponent<PresenzaAlleato>().posInQuadratoAerei = this.numAereoInQuadro;
					this.ListaAereiInVolo[this.numAereoInQuadro] = true;
					this.primaCamera.GetComponent<Selezionamento>().aereoDaSelezionare = true;
					this.primaCamera.GetComponent<Selezionamento>().numPosAereoInQuadrato = this.numAereoInQuadro;
					this.primaCamera.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
					this.primaCamera.GetComponent<GestioneSuoniCamera>().numListaVoceSelez = 2;
				}
			}
			else if (this.ListaAereiInVolo[this.numAereoInQuadro])
			{
				this.primaCamera.GetComponent<Selezionamento>().aereoDaSelezionare = true;
				this.primaCamera.GetComponent<Selezionamento>().numPosAereoInQuadrato = this.numAereoInQuadro;
			}
			this.cliccatoSuQuadroAerei = false;
		}
		for (int i = 0; i < 9; i++)
		{
			GameObject gameObject3 = this.sfondoQuadroAerei.transform.GetChild(i).FindChild("barra fissa").gameObject;
			GameObject gameObject4 = this.sfondoQuadroAerei.transform.GetChild(i).FindChild("barra scorrevole").gameObject;
			if (this.ListaAereiInVolo[i])
			{
				gameObject3.GetComponent<CanvasGroup>().alpha = 1f;
				gameObject4.GetComponent<CanvasGroup>().alpha = 1f;
				gameObject3.GetComponent<Image>().color = this.colBarraFissaCarb;
				gameObject4.GetComponent<Image>().color = Color.green;
				bool flag2 = false;
				int num3 = 0;
				while (num3 < this.ListaèAereo.Count && !flag2)
				{
					if (this.ListaèAereo[num3].GetComponent<PresenzaAlleato>().posInQuadratoAerei == i)
					{
						this.ListaAereiInQuadrato[i] = this.ListaèAereo[num3];
					}
					num3++;
				}
				if (this.ListaAereiInQuadrato[i] != null && this.ListaAereiInQuadrato[i].GetComponent<PresenzaAlleato>().truppaSelezionata)
				{
					this.sfondoQuadroAerei.transform.GetChild(i).GetComponent<Image>().color = this.coloreAereoSel;
					if (this.ListaAereiInQuadrato[i].GetComponent<PresenzaAlleato>().èBombardiere)
					{
						this.attacchiAlleatiSpeciali.GetComponent<AttacchiAlleatiSpecialiScript>().bombardiereInLista = true;
					}
				}
				else
				{
					this.sfondoQuadroAerei.transform.GetChild(i).GetComponent<Image>().color = Color.black;
				}
			}
			else
			{
				this.sfondoQuadroAerei.transform.GetChild(i).GetComponent<Image>().color = Color.black;
				if (this.ListaAereiInRifor[i])
				{
					List<float> listaTimerRiforAerei;
					List<float> expr_56B = listaTimerRiforAerei = this.ListaTimerRiforAerei;
					int num;
					int expr_570 = num = i;
					float num4 = listaTimerRiforAerei[num];
					expr_56B[expr_570] = num4 + Time.deltaTime;
					if (this.ListaTimerRiforAerei[i] > this.tempoAereiRifor)
					{
						this.ListaAereiInRifor[i] = false;
						this.ListaTimerRiforAerei[i] = 0f;
					}
					gameObject3.GetComponent<CanvasGroup>().alpha = 1f;
					gameObject4.GetComponent<CanvasGroup>().alpha = 1f;
					gameObject3.GetComponent<Image>().color = this.colBarraFissaRifor;
					gameObject4.GetComponent<Image>().color = Color.red;
					float fillAmount = this.ListaTimerRiforAerei[i] / this.tempoAereiRifor;
					gameObject4.GetComponent<Image>().fillAmount = fillAmount;
				}
				else
				{
					gameObject3.GetComponent<CanvasGroup>().alpha = 0f;
					gameObject4.GetComponent<CanvasGroup>().alpha = 0f;
					this.ListaDisponibilitàAerei[i] = true;
				}
			}
		}
		if (!this.ListaDisponibilitàAerei[this.numAereoInQuadro] && !this.ListaAereiInRifor[this.numAereoInQuadro])
		{
			this.pulsanteRichiamaAereo.GetComponent<CanvasGroup>().alpha = 1f;
			this.pulsanteRichiamaAereo.GetComponent<CanvasGroup>().interactable = true;
			this.pulsanteRichiamaAereo.GetComponent<CanvasGroup>().blocksRaycasts = true;
			if (this.richiamoAereo && this.ListaAereiInQuadrato[this.numAereoInQuadro] != null && this.ListaAereiInQuadrato[this.numAereoInQuadro].GetComponent<PresenzaAlleato>().tipoTruppaVolante != 10)
			{
				this.ListaAereiInQuadrato[this.numAereoInQuadro].GetComponent<PresenzaAlleato>().richiamaAereo = true;
				this.primaCamera.GetComponent<GestioneSuoniCamera>().attivaVoce = true;
				this.primaCamera.GetComponent<GestioneSuoniCamera>().numListaVoceSelez = 3;
				this.richiamoAereo = false;
			}
		}
		else
		{
			this.pulsanteRichiamaAereo.GetComponent<CanvasGroup>().alpha = 0f;
			this.pulsanteRichiamaAereo.GetComponent<CanvasGroup>().interactable = false;
			this.pulsanteRichiamaAereo.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00013AD0 File Offset: 0x00011CD0
	private void PerParacadutisti()
	{
		GameObject original = null;
		for (int i = 0; i < 9; i++)
		{
			if (this.ListaAereiInQuadrato[i].GetComponent<PresenzaAlleato>().tipoTruppaVolante == 10 && this.ListaDisponibilitàAerei[i])
			{
				original = this.ListaAereiInQuadrato[i];
				break;
			}
		}
		Vector3 position = Vector3.zero;
		Vector3 zero = Vector3.zero;
		Quaternion identity = Quaternion.identity;
		bool flag = false;
		int num = 0;
		while (num < this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento.Count && !flag)
		{
			if (this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num].transform.GetChild(0).tag == "AreaSchieramentoAlleato")
			{
				GameObject gameObject = this.varieMappaLocale.GetComponent<VarieMappaLocale>().ListaDelleAreeDiSchieramento[num];
				position = gameObject.transform.position + Vector3.up * this.ListaAereiInQuadrato[this.numAereoParàDispInQuadro].GetComponent<PresenzaAlleato>().quotaDiPartenza;
				this.dirSpawnAereo = gameObject.transform.forward;
				this.aereoAppenaSpawnato = true;
				flag = true;
			}
			num++;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(original, position, Quaternion.identity) as GameObject;
		gameObject2.GetComponent<PresenzaAlleato>().posInQuadratoAerei = this.numAereoParàDispInQuadro;
		this.ListaAereiInVolo[this.numAereoParàDispInQuadro] = true;
		this.ListaDisponibilitàAerei[this.numAereoParàDispInQuadro] = false;
		this.primaCamera.GetComponent<Selezionamento>().aereoDaSelezionare = true;
		this.primaCamera.GetComponent<Selezionamento>().numPosAereoInQuadrato = this.numAereoParàDispInQuadro;
		Vector3 posPrimoParà = gameObject2.GetComponent<ATT_ParaTransport>().posPrimoParà;
		float distFraParà = gameObject2.GetComponent<ATT_ParaTransport>().distFraParà;
		float x = posPrimoParà.x;
		gameObject2.GetComponent<ATT_ParaTransport>().ListaParàPresenti = new List<GameObject>();
		for (int j = 0; j < 7; j++)
		{
			if (j <= base.GetComponent<GestioneComandanteInUI>().ListaParàPerLancio.Count - 1)
			{
				if (j == 0 || j == 2 || j == 4 || j == 6)
				{
					x = posPrimoParà.x;
				}
				else
				{
					x = -posPrimoParà.x;
				}
				Vector3 localPosition = new Vector3(x, posPrimoParà.y, posPrimoParà.z + distFraParà * (float)j);
				GameObject gameObject3 = UnityEngine.Object.Instantiate(base.GetComponent<GestioneComandanteInUI>().ListaParàPerLancio[j], gameObject2.transform.position, Quaternion.identity) as GameObject;
				gameObject3.transform.parent = gameObject2.transform;
				gameObject3.transform.localPosition = localPosition;
				gameObject3.GetComponent<PresenzaAlleato>().èParà = true;
				gameObject2.GetComponent<ATT_ParaTransport>().ListaParàPresenti.Add(gameObject3);
			}
		}
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00013DB8 File Offset: 0x00011FB8
	private void CompListaPostBattaglia()
	{
		if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().battagliaTerminata && !this.contaRimanenti)
		{
			this.contaRimanenti = true;
			this.numeroTotAlleatiRimanenti = 0;
			this.ListaAlleatiPostBattaglia = new List<int>();
			for (int i = 0; i < this.ListaAlleatiPossibili.Count; i++)
			{
				this.ListaAlleatiPostBattaglia.Add(0);
			}
			bool flag = false;
			int num = 0;
			while (num < this.ListaAlleatiDisponibili.Count && !flag)
			{
				if (this.ListaAlleatiDisponibili[num][0] != 100)
				{
					List<int> listaAlleatiPostBattaglia;
					List<int> expr_89 = listaAlleatiPostBattaglia = this.ListaAlleatiPostBattaglia;
					int num2;
					int expr_9E = num2 = this.ListaAlleatiDisponibili[num][0];
					num2 = listaAlleatiPostBattaglia[num2];
					expr_89[expr_9E] = num2 + this.ListaAlleatiDisponibili[num][1];
					this.numeroTotAlleatiRimanenti += this.ListaAlleatiDisponibili[num][1];
				}
				else
				{
					flag = true;
				}
				num++;
			}
			for (int j = 0; j < this.ListaAlleati.Count; j++)
			{
				if (this.ListaAlleati[j] != null)
				{
					List<int> listaAlleatiPostBattaglia2;
					List<int> expr_12B = listaAlleatiPostBattaglia2 = this.ListaAlleatiPostBattaglia;
					int num2;
					int expr_144 = num2 = this.ListaAlleati[j].GetComponent<PresenzaAlleato>().tipoTruppa;
					num2 = listaAlleatiPostBattaglia2[num2];
					expr_12B[expr_144] = num2 + 1;
					this.numeroTotAlleatiRimanenti++;
					for (int k = 0; k < this.ListaAlleati[j].GetComponent<PresenzaAlleato>().numeroArmi; k++)
					{
						if (!this.ListaAlleati[j].GetComponent<PresenzaAlleato>().èPerRifornimento)
						{
							this.ListaAlleati[j].GetComponent<PresenzaAlleato>().ListaMunizioniAttive[k].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().quantità += this.ListaAlleati[j].GetComponent<PresenzaAlleato>().ListaArmi[k][6];
						}
					}
				}
			}
			for (int l = 0; l < 9; l++)
			{
				if (this.ListaAereiInQuadrato[l] != null && !this.ListaAereiInVolo[l])
				{
					List<int> listaAlleatiPostBattaglia3;
					List<int> expr_257 = listaAlleatiPostBattaglia3 = this.ListaAlleatiPostBattaglia;
					int num2;
					int expr_271 = num2 = this.ListaAereiInQuadrato[l].GetComponent<PresenzaAlleato>().tipoTruppa;
					num2 = listaAlleatiPostBattaglia3[num2];
					expr_257[expr_271] = num2 + 1;
					this.numeroTotAlleatiRimanenti++;
				}
			}
		}
	}

	// Token: 0x06000059 RID: 89 RVA: 0x0001406C File Offset: 0x0001226C
	private void GestioneListePerSuoni()
	{
		GameObject oggettoCameraAttiva = this.primaCamera.GetComponent<PrimaCamera>().oggettoCameraAttiva;
		this.ListaAllPiùViciniPerTipo = new List<GameObject>();
		for (int i = 0; i < this.ListaAlleatiPossibili.Count; i++)
		{
			int num = 3;
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = null;
				float num2 = 99999f;
				float num3 = num2;
				for (int k = 0; k < this.ListaAlleati.Count; k++)
				{
					if (this.ListaAlleati[k].GetComponent<PresenzaAlleato>().tipoTruppa == i)
					{
						float num4 = Vector3.Distance(this.ListaAlleati[k].transform.position, oggettoCameraAttiva.transform.position);
						if (num4 < num3 && !this.ListaAllPiùViciniPerTipo.Contains(this.ListaAlleati[k]))
						{
							num3 = num4;
							gameObject = this.ListaAlleati[k];
						}
					}
				}
				if (gameObject != null)
				{
					this.ListaAllPiùViciniPerTipo.Add(gameObject);
				}
				else
				{
					j = num;
				}
			}
		}
		for (int l = 0; l < this.ListaAlleatiPossibili.Count; l++)
		{
			for (int m = 0; m < this.ListaTimerSuoniArmi[l].Count; m++)
			{
				List<float> list;
				List<float> expr_137 = list = this.ListaTimerSuoniArmi[l];
				int index;
				int expr_13C = index = m;
				float num5 = list[index];
				expr_137[expr_13C] = num5 + Time.deltaTime;
			}
		}
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00014208 File Offset: 0x00012408
	private void GestioneGruppi()
	{
		bool flag = false;
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.gruppoPremuto = 0;
			flag = Input.GetKey(KeyCode.LeftControl);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.gruppoPremuto = 1;
			flag = Input.GetKey(KeyCode.LeftControl);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			this.gruppoPremuto = 2;
			flag = Input.GetKey(KeyCode.LeftControl);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			this.gruppoPremuto = 3;
			flag = Input.GetKey(KeyCode.LeftControl);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			this.gruppoPremuto = 4;
			flag = Input.GetKey(KeyCode.LeftControl);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			this.gruppoPremuto = 5;
			flag = Input.GetKey(KeyCode.LeftControl);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			this.gruppoPremuto = 6;
			flag = Input.GetKey(KeyCode.LeftControl);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			this.gruppoPremuto = 7;
			flag = Input.GetKey(KeyCode.LeftControl);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			this.gruppoPremuto = 8;
			flag = Input.GetKey(KeyCode.LeftControl);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			this.gruppoPremuto = 9;
			flag = Input.GetKey(KeyCode.LeftControl);
		}
		if (this.gruppoPremuto != 999)
		{
			if (flag)
			{
				this.ListaDiGruppi[this.gruppoPremuto].Clear();
				for (int i = 0; i < this.ListaSelezionamento.Count; i++)
				{
					if (this.ListaSelezionamento[i] != null)
					{
						this.ListaDiGruppi[this.gruppoPremuto].Add(this.ListaSelezionamento[i]);
					}
				}
			}
			else
			{
				foreach (GameObject current in this.ListaSelezionamento)
				{
					current.GetComponent<PresenzaAlleato>().truppaSelezionata = false;
				}
				this.ListaSelezionamento.Clear();
				base.GetComponent<GestioneComandanteInUI>().ListaTipiTruppeSel.Clear();
				for (int j = 0; j < 10; j++)
				{
					base.GetComponent<GestioneComandanteInUI>().ListaPosizioni[j].Clear();
				}
				base.GetComponent<GestioneComandanteInUI>().numeroTipoDiverseSel = 0;
				this.modalitàUnitàSingola.GetComponent<CanvasGroup>().alpha = 0f;
				this.modalitàUnitàSingola.GetComponent<CanvasGroup>().interactable = false;
				this.modalitàUnitàSingola.GetComponent<CanvasGroup>().blocksRaycasts = false;
				this.modalitàUnitàMultipla.GetComponent<CanvasGroup>().alpha = 0f;
				this.modalitàUnitàMultipla.GetComponent<CanvasGroup>().interactable = false;
				this.modalitàUnitàMultipla.GetComponent<CanvasGroup>().blocksRaycasts = false;
				this.primaCamera.GetComponent<Selezionamento>().trappolaSelez = null;
				for (int k = 0; k < this.ListaDiGruppi[this.gruppoPremuto].Count; k++)
				{
					if (this.ListaDiGruppi[this.gruppoPremuto][k] != null)
					{
						this.ListaSelezionamento.Add(this.ListaDiGruppi[this.gruppoPremuto][k]);
					}
				}
			}
		}
		for (int l = 0; l < 10; l++)
		{
			int num = 0;
			for (int m = 0; m < this.ListaDiGruppi[l].Count; m++)
			{
				if (this.ListaDiGruppi[l][m] != null)
				{
					num++;
				}
			}
			this.insiemeGruppi.transform.GetChild(l).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = num.ToString();
			if (num > 0)
			{
				this.insiemeGruppi.transform.GetChild(l).GetChild(0).GetComponent<Image>().color = this.coloreGruppoNonVuoto;
			}
			else
			{
				this.insiemeGruppi.transform.GetChild(l).GetChild(0).GetComponent<Image>().color = Color.white;
			}
		}
		this.gruppoPremuto = 999;
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00014720 File Offset: 0x00012920
	private void Varie()
	{
		this.scrittaPuntiBattaglia.GetComponent<Text>().text = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().puntiBattaglia.ToString("F0");
		if (this.ListaSelezionamento.Count == 0)
		{
			this.posizTrapAttivo = false;
			this.trappolaCreata = false;
			UnityEngine.Object.Destroy(this.trappolaDaPosiz);
		}
		else if (!this.ListaSelezionamento[0].GetComponent<PresenzaAlleato>().èGeniere)
		{
			this.posizTrapAttivo = false;
			this.trappolaCreata = false;
			UnityEngine.Object.Destroy(this.trappolaDaPosiz);
		}
		if (this.posizTrapAttivo)
		{
			this.scrittaCancellaTrappola.GetComponent<CanvasGroup>().alpha = 1f;
		}
		else
		{
			this.scrittaCancellaTrappola.GetComponent<CanvasGroup>().alpha = 0f;
		}
		if (this.atterraSupplyHeli)
		{
			foreach (GameObject current in this.ListaSelezionamento)
			{
				if (current != null && current.GetComponent<PresenzaAlleato>().tipoTruppa == 33)
				{
					current.GetComponent<MOV_AUTOM_SupplyHelicopter>().atterraHeli = true;
				}
				else if (current != null && current.GetComponent<PresenzaAlleato>().tipoTruppa == 34)
				{
					current.GetComponent<MOV_AUTOM_LightHelicopter>().atterraHeli = true;
				}
				else if (current != null && current.GetComponent<PresenzaAlleato>().tipoTruppa == 35)
				{
					current.GetComponent<MOV_AUTOM_AttackHelicopter>().atterraHeli = true;
				}
				else if (current != null && current.GetComponent<PresenzaAlleato>().tipoTruppa == 36)
				{
					current.GetComponent<MOV_AUTOM_HeavyHelicopter>().atterraHeli = true;
				}
				else if (current != null && current.GetComponent<PresenzaAlleato>().tipoTruppa == 37)
				{
					current.GetComponent<MOV_AUTOM_ArmoredAirship>().atterraHeli = true;
				}
			}
			this.atterraSupplyHeli = false;
		}
		if (this.tipoFuocoSalvaRockArt != 0)
		{
			foreach (GameObject current2 in this.ListaSelezionamento)
			{
				if (current2 && current2.GetComponent<PresenzaAlleato>().tipoTruppaTerrConOrdigni == 3)
				{
					if (this.tipoFuocoSalvaRockArt == 1)
					{
						current2.GetComponent<ATT_RocketArtillery>().rafficaAttiva = false;
					}
					else
					{
						current2.GetComponent<ATT_RocketArtillery>().rafficaAttiva = true;
					}
				}
			}
			this.tipoFuocoSalvaRockArt = 0;
		}
		if (this.timerAnnullTrap > 0f)
		{
			this.timerAnnullTrap += Time.deltaTime;
			if (this.timerAnnullTrap > 0.3f)
			{
				this.timerAnnullTrap = 0f;
			}
		}
		this.alleatiDiRiserva = 0;
		for (int i = 0; i < this.ListaAlleatiPossibili.Count; i++)
		{
			this.alleatiDiRiserva += this.ListaAlleatiDisponibili[i][1];
		}
	}

	// Token: 0x040001B2 RID: 434
	private GameObject infoNeutreTattica;

	// Token: 0x040001B3 RID: 435
	private GameObject IANemico;

	// Token: 0x040001B4 RID: 436
	private GameObject primaCamera;

	// Token: 0x040001B5 RID: 437
	private GameObject varieMappaLocale;

	// Token: 0x040001B6 RID: 438
	private GameObject sfondoQuadroAerei;

	// Token: 0x040001B7 RID: 439
	private GameObject attacchiAlleatiSpeciali;

	// Token: 0x040001B8 RID: 440
	private GameObject pulsanteRichiamaAereo;

	// Token: 0x040001B9 RID: 441
	private GameObject scrittaPuntiBattaglia;

	// Token: 0x040001BA RID: 442
	private GameObject barraUnitàSelez;

	// Token: 0x040001BB RID: 443
	private GameObject insiemeGruppi;

	// Token: 0x040001BC RID: 444
	private GameObject modalitàUnitàSingola;

	// Token: 0x040001BD RID: 445
	private GameObject modalitàUnitàMultipla;

	// Token: 0x040001BE RID: 446
	private GameObject scrittaCancellaTrappola;

	// Token: 0x040001BF RID: 447
	public List<GameObject> ListaAlleati;

	// Token: 0x040001C0 RID: 448
	private GameObject[] ArrayAlleati;

	// Token: 0x040001C1 RID: 449
	public int numMaxAlleati;

	// Token: 0x040001C2 RID: 450
	public int numeroTotaleTipiTruppeVolanti;

	// Token: 0x040001C3 RID: 451
	public int numeroTotaleTipiTruppeTerrConOrdigni;

	// Token: 0x040001C4 RID: 452
	public int numeroTotaleTipiOrdigni;

	// Token: 0x040001C5 RID: 453
	public List<GameObject> ListaSelezionamento;

	// Token: 0x040001C6 RID: 454
	public List<GameObject> ListaèVolante;

	// Token: 0x040001C7 RID: 455
	public List<GameObject> ListaèNonVolante;

	// Token: 0x040001C8 RID: 456
	public List<GameObject> ListaèFanteria;

	// Token: 0x040001C9 RID: 457
	public List<GameObject> ListaèMezzo;

	// Token: 0x040001CA RID: 458
	public List<GameObject> ListaèElicottero;

	// Token: 0x040001CB RID: 459
	public List<GameObject> ListaèAereo;

	// Token: 0x040001CC RID: 460
	public List<GameObject> ListaèArtiglieria;

	// Token: 0x040001CD RID: 461
	public List<GameObject> ListaèPerRifornimento;

	// Token: 0x040001CE RID: 462
	public List<GameObject> ListaèGeniere;

	// Token: 0x040001CF RID: 463
	public List<GameObject> ListaAereiInQuadrato;

	// Token: 0x040001D0 RID: 464
	public List<bool> ListaDisponibilitàAerei;

	// Token: 0x040001D1 RID: 465
	public List<bool> ListaAereiInRifor;

	// Token: 0x040001D2 RID: 466
	public List<bool> ListaAereiInVolo;

	// Token: 0x040001D3 RID: 467
	private List<float> ListaTimerRiforAerei;

	// Token: 0x040001D4 RID: 468
	public List<GameObject> ListaAlleatiPossibili;

	// Token: 0x040001D5 RID: 469
	public List<List<int>> ListaAlleatiDisponibili;

	// Token: 0x040001D6 RID: 470
	public List<GameObject> ListaTrappolePossibili;

	// Token: 0x040001D7 RID: 471
	public bool cliccatoSuQuadroAerei;

	// Token: 0x040001D8 RID: 472
	public int numAereoInQuadro;

	// Token: 0x040001D9 RID: 473
	public Vector3 dirSpawnAereo;

	// Token: 0x040001DA RID: 474
	public bool aereoAppenaSpawnato;

	// Token: 0x040001DB RID: 475
	public Color colBarraFissaCarb;

	// Token: 0x040001DC RID: 476
	public Color colBarraFissaRifor;

	// Token: 0x040001DD RID: 477
	public bool ricreaAereoInQuadro;

	// Token: 0x040001DE RID: 478
	public int tipoAereoDaRicreareInQuadro;

	// Token: 0x040001DF RID: 479
	public int postoInCuiRicreare;

	// Token: 0x040001E0 RID: 480
	public Color coloreAereoSel;

	// Token: 0x040001E1 RID: 481
	public bool richiamoAereo;

	// Token: 0x040001E2 RID: 482
	public List<int> ListaAlleatiPostBattaglia;

	// Token: 0x040001E3 RID: 483
	private bool contaRimanenti;

	// Token: 0x040001E4 RID: 484
	public int numeroTotAlleatiRimanenti;

	// Token: 0x040001E5 RID: 485
	public int trappolaScelta;

	// Token: 0x040001E6 RID: 486
	public bool posizTrapAttivo;

	// Token: 0x040001E7 RID: 487
	public bool trappolaCreata;

	// Token: 0x040001E8 RID: 488
	public GameObject trappolaDaPosiz;

	// Token: 0x040001E9 RID: 489
	public float coeffMoltiplPerQuota;

	// Token: 0x040001EA RID: 490
	public bool aggiorQuota;

	// Token: 0x040001EB RID: 491
	public bool riparaTrappola;

	// Token: 0x040001EC RID: 492
	public bool demolisciTrappola;

	// Token: 0x040001ED RID: 493
	public float timerAnnullTrap;

	// Token: 0x040001EE RID: 494
	public bool atterraSupplyHeli;

	// Token: 0x040001EF RID: 495
	public int tipoFuocoSalvaRockArt;

	// Token: 0x040001F0 RID: 496
	public Vector3 puntoDiLancioParà;

	// Token: 0x040001F1 RID: 497
	public int numAereoParàDispInQuadro;

	// Token: 0x040001F2 RID: 498
	public int tipoBattaglia;

	// Token: 0x040001F3 RID: 499
	private bool primoFramePassato;

	// Token: 0x040001F4 RID: 500
	public int alleatiDiRiserva;

	// Token: 0x040001F5 RID: 501
	public List<int> ListaAllPresInBatt;

	// Token: 0x040001F6 RID: 502
	public List<List<float>> ListaTimerSuoniArmi;

	// Token: 0x040001F7 RID: 503
	public List<GameObject> ListaAllPiùViciniPerTipo;

	// Token: 0x040001F8 RID: 504
	public float tempoAereiRifor;

	// Token: 0x040001F9 RID: 505
	private List<GameObject> ListaGruppo0;

	// Token: 0x040001FA RID: 506
	private List<GameObject> ListaGruppo1;

	// Token: 0x040001FB RID: 507
	private List<GameObject> ListaGruppo2;

	// Token: 0x040001FC RID: 508
	private List<GameObject> ListaGruppo3;

	// Token: 0x040001FD RID: 509
	private List<GameObject> ListaGruppo4;

	// Token: 0x040001FE RID: 510
	private List<GameObject> ListaGruppo5;

	// Token: 0x040001FF RID: 511
	private List<GameObject> ListaGruppo6;

	// Token: 0x04000200 RID: 512
	private List<GameObject> ListaGruppo7;

	// Token: 0x04000201 RID: 513
	private List<GameObject> ListaGruppo8;

	// Token: 0x04000202 RID: 514
	private List<GameObject> ListaGruppo9;

	// Token: 0x04000203 RID: 515
	private List<List<GameObject>> ListaDiGruppi;

	// Token: 0x04000204 RID: 516
	public Color coloreGruppoNonVuoto;

	// Token: 0x04000205 RID: 517
	public int gruppoPremuto;

	// Token: 0x04000206 RID: 518
	public Vector3 ultimoOrientTrappola;
}
