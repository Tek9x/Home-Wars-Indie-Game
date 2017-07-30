using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000034 RID: 52
public class ATT_Sapper : MonoBehaviour
{
	// Token: 0x06000277 RID: 631 RVA: 0x00069138 File Offset: 0x00067338
	private void Start()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.immagineDinamite = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Barra Unità Selezionate").FindChild("SopraRettangolo").FindChild("dinamite").GetChild(0).gameObject;
		this.alleatoNav = base.GetComponent<NavMeshAgent>();
		this.velocitàAlleatoNav = base.GetComponent<NavMeshAgent>().speed;
		this.suonoUnità = base.GetComponent<AudioSource>();
		this.suonoMicciaDina = base.transform.FindChild("uomo").GetComponent<AudioSource>();
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 6f;
		this.tempoDinamite = 3f;
		this.velocitàIniziale = this.alleatoNav.speed;
	}

	// Token: 0x06000278 RID: 632 RVA: 0x00069248 File Offset: 0x00067448
	private void Update()
	{
		if (!this.alleatoNav.isOnOffMeshLink)
		{
			this.calcoloJumpEffettuato = false;
		}
		else
		{
			this.InJump();
		}
		if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().posizTrapAttivo && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0 && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0] == base.gameObject)
		{
			this.PosizionamentoTrappola();
		}
		this.FunzioneCodaCostruzione();
		this.PiazzamentoDinamite();
		float num = Vector3.Distance(base.transform.position, this.alleatoNav.destination);
		if (num <= this.distFineOrdineMovimento)
		{
			base.GetComponent<PresenzaAlleato>().destinazioneOrdinata = false;
		}
		if (!base.GetComponent<PresenzaAlleato>().destinazioneOrdinata)
		{
			this.alleatoNav.speed = 0f;
		}
		else
		{
			this.alleatoNav.speed = this.velocitàAlleatoNav;
		}
	}

	// Token: 0x06000279 RID: 633 RVA: 0x0006934C File Offset: 0x0006754C
	private void InJump()
	{
		if (!this.calcoloJumpEffettuato)
		{
			this.calcoloDistJump = true;
		}
		if (this.calcoloDistJump)
		{
			this.calcoloDistJump = false;
			this.calcoloJumpEffettuato = true;
			float num = Mathf.Abs(this.alleatoNav.destination.y - base.transform.position.y);
			this.alleatoNav.speed = this.velocitàIniziale / (num / 80f) / 10f;
		}
	}

	// Token: 0x0600027A RID: 634 RVA: 0x000693D0 File Offset: 0x000675D0
	private void PosizionamentoTrappola()
	{
		if (!this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaCreata)
		{
			if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz != null)
			{
				UnityEngine.Object.Destroy(this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz);
			}
			this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaTrappolePossibili[this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaScelta];
			this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz = (UnityEngine.Object.Instantiate(this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz, this.primaCamera.GetComponent<Selezionamento>().posPerTrappola, Quaternion.Euler(this.infoAlleati.GetComponent<InfoGenericheAlleati>().ultimoOrientTrappola)) as GameObject);
			this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaCreata = true;
			this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz.GetComponent<PresenzaTrappola>().inPosizionamento = true;
		}
		if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaCreata)
		{
			if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz.GetComponent<PresenzaTrappola>().tipoTrappola == 5)
			{
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz.transform.position = this.primaCamera.GetComponent<Selezionamento>().posPerTrappola + Vector3.up * 60f;
			}
			else
			{
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz.transform.position = this.primaCamera.GetComponent<Selezionamento>().posPerTrappola;
			}
			if (Input.GetKey(KeyCode.LeftAlt))
			{
				float num = Input.GetAxis("Mouse ScrollWheel") * 40f;
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz.transform.eulerAngles = new Vector3(0f, this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz.transform.eulerAngles.y + num, 0f);
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().ultimoOrientTrappola = this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz.transform.eulerAngles;
			}
			if (Input.GetMouseButtonDown(1))
			{
				UnityEngine.Object.Destroy(this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz);
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaCreata = false;
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().posizTrapAttivo = false;
				this.infoAlleati.GetComponent<InfoGenericheAlleati>().timerAnnullTrap = 0.1f;
			}
			if (Input.GetMouseButtonDown(0) && this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz.GetComponent<PresenzaTrappola>().èPosizionabile && !EventSystem.current.IsPointerOverGameObject() && this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().puntiBattaglia >= this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz.GetComponent<PresenzaTrappola>().costoPuntiBattaglia)
			{
				bool flag = false;
				for (int i = 0; i < this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici.Count; i++)
				{
					if (this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici[i] != null && Vector3.Distance(this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici[i].transform.position, this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz.transform.position) < 40f)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					this.infoAlleati.GetComponent<GestioneComandanteInUI>().partenzaTimerScrAvvSapper = true;
				}
				else
				{
					GameObject trappolaDaPosiz = this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz;
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().puntiBattaglia -= trappolaDaPosiz.GetComponent<PresenzaTrappola>().costoPuntiBattaglia;
					trappolaDaPosiz.GetComponent<PresenzaTrappola>().inPosizionamento = false;
					trappolaDaPosiz.GetComponent<PresenzaTrappola>().inCostruzione = true;
					if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz.GetComponent<PresenzaTrappola>().tipoTrappola == 5)
					{
						this.alleatoNav.SetDestination(new Vector3(trappolaDaPosiz.transform.position.x, trappolaDaPosiz.transform.position.y - 60f, trappolaDaPosiz.transform.position.z));
					}
					else
					{
						this.alleatoNav.SetDestination(trappolaDaPosiz.transform.position);
					}
					for (int j = 0; j < this.ListaCodaTrappole.Count; j++)
					{
						if (this.ListaCodaTrappole[j] == null || !this.ListaCodaTrappole[j].GetComponent<PresenzaTrappola>().inCostruzione)
						{
							this.ListaCodaTrappole.RemoveAt(j);
						}
					}
					this.ListaCodaTrappole.Add(trappolaDaPosiz);
					base.GetComponent<PresenzaAlleato>().destinazioneOrdinata = true;
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz = null;
					this.suonoUnità.Play();
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz = this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaTrappolePossibili[this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaScelta];
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz = (UnityEngine.Object.Instantiate(this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz, this.primaCamera.GetComponent<Selezionamento>().posPerTrappola, Quaternion.Euler(this.infoAlleati.GetComponent<InfoGenericheAlleati>().ultimoOrientTrappola)) as GameObject);
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaCreata = true;
					this.infoAlleati.GetComponent<InfoGenericheAlleati>().trappolaDaPosiz.GetComponent<PresenzaTrappola>().inPosizionamento = true;
				}
			}
		}
	}

	// Token: 0x0600027B RID: 635 RVA: 0x000699A0 File Offset: 0x00067BA0
	private void FunzioneCodaCostruzione()
	{
		for (int i = 0; i < this.ListaCodaTrappole.Count; i++)
		{
			if (this.ListaCodaTrappole[i] != null && this.ListaCodaTrappole[i].GetComponent<PresenzaTrappola>().inCostruzione)
			{
				if (this.ListaCodaTrappole[i].GetComponent<PresenzaTrappola>().tipoTrappola == 5)
				{
					this.alleatoNav.SetDestination(new Vector3(this.ListaCodaTrappole[i].transform.position.x, this.ListaCodaTrappole[i].transform.position.y - 60f, this.ListaCodaTrappole[i].transform.position.z));
				}
				else
				{
					this.alleatoNav.SetDestination(this.ListaCodaTrappole[i].transform.position);
				}
				break;
			}
		}
	}

	// Token: 0x0600027C RID: 636 RVA: 0x00069AB4 File Offset: 0x00067CB4
	private void PiazzamentoDinamite()
	{
		if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaBersagliDinamite.Count > 0)
		{
			bool flag = false;
			int num = 0;
			while (num < this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaBersagliDinamite.Count && !flag)
			{
				float num2 = Vector3.Distance(base.transform.position, this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaBersagliDinamite[num].transform.position);
				if (num2 < this.distInnescoDinamite)
				{
					flag = true;
					this.dinamiteBersaglio = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaBersagliDinamite[num];
				}
				num++;
			}
			if (flag)
			{
				this.dinamitePossibile = true;
				if (this.piazzDinamiteAttivo)
				{
					this.timerDinamite += Time.deltaTime;
					if (this.timerDinamite > 0f && this.timerDinamite < 0.2f)
					{
						this.suonoMicciaDina.Play();
					}
					if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0 && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0] == base.gameObject)
					{
						float fillAmount = this.timerDinamite / this.tempoDinamite;
						this.immagineDinamite.GetComponent<Image>().fillAmount = fillAmount;
					}
					if (this.timerDinamite > this.tempoDinamite)
					{
						this.timerDinamite = 0f;
						this.dinamiteFittizia = (UnityEngine.Object.Instantiate(this.dinamiteFittPrefab, base.transform.position, Quaternion.identity) as GameObject);
						this.suonoMicciaDina.Stop();
						this.dinamiteFittizia.GetComponent<AudioSource>().Play();
						this.dinamiteBersaglio.GetComponent<ObbiettivoTatticoScript>().vita -= this.dannoDinamiteAOutpostNem;
						this.piazzDinamiteAttivo = false;
						this.partenzaTimerDistrDina = true;
					}
				}
				else
				{
					this.timerDinamite = 0f;
					this.suonoMicciaDina.Stop();
				}
			}
			else
			{
				this.dinamitePossibile = false;
				this.piazzDinamiteAttivo = false;
				this.timerDinamite = 0f;
				this.suonoMicciaDina.Stop();
			}
			if (this.partenzaTimerDistrDina)
			{
				this.timerDistrDinamite += Time.deltaTime;
				if (this.timerDistrDinamite > 5f)
				{
					this.timerDistrDinamite = 0f;
					this.partenzaTimerDistrDina = false;
					UnityEngine.Object.Destroy(this.dinamiteFittizia);
				}
			}
		}
	}

	// Token: 0x04000A9D RID: 2717
	private GameObject infoAlleati;

	// Token: 0x04000A9E RID: 2718
	private GameObject infoNeutreTattica;

	// Token: 0x04000A9F RID: 2719
	private GameObject primaCamera;

	// Token: 0x04000AA0 RID: 2720
	private GameObject immagineDinamite;

	// Token: 0x04000AA1 RID: 2721
	private GameObject IANemico;

	// Token: 0x04000AA2 RID: 2722
	public float dannoDinamiteAOutpostNem;

	// Token: 0x04000AA3 RID: 2723
	private AudioSource suonoUnità;

	// Token: 0x04000AA4 RID: 2724
	private NavMeshAgent alleatoNav;

	// Token: 0x04000AA5 RID: 2725
	private float velocitàAlleatoNav;

	// Token: 0x04000AA6 RID: 2726
	private float distFineOrdineMovimento;

	// Token: 0x04000AA7 RID: 2727
	private GameObject trapDaPosizionare;

	// Token: 0x04000AA8 RID: 2728
	public bool dinamitePossibile;

	// Token: 0x04000AA9 RID: 2729
	public float distInnescoDinamite;

	// Token: 0x04000AAA RID: 2730
	public bool piazzDinamiteAttivo;

	// Token: 0x04000AAB RID: 2731
	private float timerDinamite;

	// Token: 0x04000AAC RID: 2732
	private float tempoDinamite;

	// Token: 0x04000AAD RID: 2733
	private GameObject dinamiteBersaglio;

	// Token: 0x04000AAE RID: 2734
	public GameObject dinamiteFittPrefab;

	// Token: 0x04000AAF RID: 2735
	private GameObject dinamiteFittizia;

	// Token: 0x04000AB0 RID: 2736
	private bool partenzaTimerDistrDina;

	// Token: 0x04000AB1 RID: 2737
	private float timerDistrDinamite;

	// Token: 0x04000AB2 RID: 2738
	private AudioSource suonoMicciaDina;

	// Token: 0x04000AB3 RID: 2739
	private bool calcoloDistJump;

	// Token: 0x04000AB4 RID: 2740
	private bool calcoloJumpEffettuato;

	// Token: 0x04000AB5 RID: 2741
	private float velocitàIniziale;

	// Token: 0x04000AB6 RID: 2742
	public List<GameObject> ListaCodaTrappole;
}
