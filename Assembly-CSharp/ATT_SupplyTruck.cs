using System;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class ATT_SupplyTruck : MonoBehaviour
{
	// Token: 0x0600038F RID: 911 RVA: 0x00092608 File Offset: 0x00090808
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.alleatoNav = base.GetComponent<NavMeshAgent>();
		this.velocitàAlleatoNav = base.GetComponent<NavMeshAgent>().speed;
		this.campoCameraIniziale = this.terzaCamera.GetComponent<Camera>().fieldOfView;
		base.GetComponent<PresenzaAlleato>().arma1Attivata = true;
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.suonoMotore.clip = this.motoreFermo;
		this.suonoMotore.Play();
		this.distFineOrdineMovimento = this.alleatoNav.stoppingDistance + 3f;
		this.frequenzaDiRifornimento = 3f;
		this.suonoRifor = base.transform.GetChild(1).GetComponent<AudioSource>();
		this.ruote1 = base.transform.GetChild(1).transform.GetChild(0).gameObject;
		this.ruote2 = base.transform.GetChild(1).transform.GetChild(1).gameObject;
		this.ruote3 = base.transform.GetChild(1).transform.GetChild(2).gameObject;
	}

	// Token: 0x06000390 RID: 912 RVA: 0x00092774 File Offset: 0x00090974
	private void Update()
	{
		this.RifornimentoTruppe();
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.GestioneRuote();
			this.GestioneSuoniIndipendenti();
			base.GetComponent<NavMeshAgent>().enabled = true;
			this.MovimentoIndipendente();
			if (this.strutturaFPS1)
			{
				UnityEngine.Object.Destroy(this.strutturaFPS1);
			}
		}
		else
		{
			this.GestioneVisuali();
			if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva == 3)
			{
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaFPS = false;
					base.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
				}
				if (this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().diventaTPS = false;
					base.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
				}
			}
		}
		if (this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0 && this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0] == base.gameObject)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona = false;
			this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera = null;
			this.timerPosizionamentoTPS = 0f;
			this.timerPosizionamentoFPS = 0f;
			base.GetComponent<NavMeshAgent>().enabled = true;
			base.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = this.campoCameraIniziale;
		}
	}

	// Token: 0x06000391 RID: 913 RVA: 0x00092954 File Offset: 0x00090B54
	private void GestioneRuote()
	{
		if (this.suonoMotore.clip == this.motorePartenza || this.suonoMotore.clip == this.motoreViaggio)
		{
			this.ruote1.transform.Rotate(Vector3.right * 4f);
			this.ruote2.transform.Rotate(Vector3.right * 4f);
			this.ruote3.transform.Rotate(Vector3.right * 4f);
		}
	}

	// Token: 0x06000392 RID: 914 RVA: 0x000929F4 File Offset: 0x00090BF4
	private void GestioneSuoniIndipendenti()
	{
		if (this.alleatoNav.velocity.magnitude > 0f)
		{
			this.timerPartenza += Time.deltaTime;
			this.timerStop = 0f;
			this.inStop = false;
			this.stopFinito = false;
		}
		if (!this.inPartenza && this.timerPartenza > 0f)
		{
			this.suonoMotore.clip = this.motorePartenza;
			this.suonoMotore.Play();
			this.inPartenza = true;
		}
		if (!this.partenzaFinita && this.timerPartenza > this.motorePartenza.length)
		{
			this.suonoMotore.clip = this.motoreViaggio;
			this.suonoMotore.Play();
			this.partenzaFinita = true;
		}
		if (this.alleatoNav.velocity.magnitude == 0f)
		{
			this.timerStop += Time.deltaTime;
			this.timerPartenza = 0f;
			this.inPartenza = false;
			this.partenzaFinita = false;
		}
		if (!this.inStop && this.timerStop > 0f)
		{
			this.suonoMotore.clip = this.motoreStop;
			this.suonoMotore.Play();
			this.inStop = true;
		}
		if (!this.stopFinito && this.timerStop > this.motoreStop.length)
		{
			this.suonoMotore.clip = this.motoreFermo;
			this.suonoMotore.Play();
			this.stopFinito = true;
		}
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00092B98 File Offset: 0x00090D98
	private void GestioneVisuali()
	{
		if (!this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
		{
			if (this.strutturaFPS1)
			{
				UnityEngine.Object.Destroy(this.strutturaFPS1);
			}
			this.CameraTPS();
			this.timerPosizionamentoFPS = 0f;
		}
		if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
		{
			if (!this.strutturaFPS1)
			{
				this.strutturaFPS1 = (UnityEngine.Object.Instantiate(this.strutturaPrefabFPS1, base.transform.position, this.terzaCamera.transform.rotation) as GameObject);
				this.strutturaFPS1.transform.parent = this.terzaCamera.transform;
				this.strutturaFPS1.transform.localPosition = this.aggiustamentoTraslVisualeFPS;
			}
			this.CameraFPS();
			this.timerPosizionamentoTPS = 0f;
		}
	}

	// Token: 0x06000394 RID: 916 RVA: 0x00092C80 File Offset: 0x00090E80
	private void CameraTPS()
	{
		this.timerPosizionamentoTPS += Time.deltaTime;
		if (this.timerPosizionamentoTPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraTPS;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = this.campoCameraIniziale;
		}
	}

	// Token: 0x06000395 RID: 917 RVA: 0x00092D0C File Offset: 0x00090F0C
	private void CameraFPS()
	{
		this.timerPosizionamentoFPS += Time.deltaTime;
		if (this.timerPosizionamentoFPS < 0.35f)
		{
			this.terzaCamera.GetComponent<TerzaCamera>().transform.parent = this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera.transform;
			this.terzaCamera.GetComponent<TerzaCamera>().transform.localPosition = this.posizionamentoCameraFPS;
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 56f;
		}
	}

	// Token: 0x06000396 RID: 918 RVA: 0x00092D98 File Offset: 0x00090F98
	private void MovimentoIndipendente()
	{
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

	// Token: 0x06000397 RID: 919 RVA: 0x00092E10 File Offset: 0x00091010
	private void RifornimentoTruppe()
	{
		this.timerRifornimento += Time.deltaTime;
		bool flag = false;
		if (this.timerRifornimento > this.frequenzaDiRifornimento && base.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp > 0)
		{
			foreach (GameObject current in this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaAlleati)
			{
				if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[0] && !current.GetComponent<PresenzaAlleato>().èPerRifornimento)
				{
					float num = Vector3.Distance(base.transform.position, current.transform.position);
					if (num < base.GetComponent<PresenzaAlleato>().raggioDiRifornimento)
					{
						for (int i = 0; i < current.GetComponent<PresenzaAlleato>().numeroArmi; i++)
						{
							if (current.GetComponent<PresenzaAlleato>().ListaArmi[i][6] < current.GetComponent<PresenzaAlleato>().ListaArmi[i][4] && this.infoAlleati.GetComponent<InfoMunizionamento>().ListaTipiMunizioniBaseTattica[current.GetComponent<PresenzaAlleato>().ListaMunizioniAttive[i].GetComponent<DatiGeneraliMunizione>().tipoMunizioneBase.GetComponent<QuantitàMunizione>().posInListaMunizioniBase].GetComponent<QuantitàMunizione>().quantità > 0f)
							{
								current.GetComponent<PresenzaAlleato>().rifornimentoAttivo = true;
								base.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp--;
								flag = true;
							}
						}
					}
				}
				float num2 = Vector3.Distance(base.transform.position, current.transform.position);
				if (base.GetComponent<PresenzaAlleato>().ListaArmiAttivate[1] && num2 < base.GetComponent<PresenzaAlleato>().raggioDiRifornimento && this.infoAlleati.GetComponent<InfoMunizionamento>().ListaTipiMunizioniBaseTattica[11].GetComponent<QuantitàMunizione>().quantità > 0f && current.GetComponent<PresenzaAlleato>().vita < current.GetComponent<PresenzaAlleato>().vitaIniziale)
				{
					current.GetComponent<PresenzaAlleato>().riparazioneAttiva = true;
					base.GetComponent<PresenzaAlleato>().puntiRifornimentoDisp--;
					this.infoAlleati.GetComponent<InfoMunizionamento>().ListaTipiMunizioniBaseTattica[11].GetComponent<QuantitàMunizione>().quantità -= 1f;
				}
			}
			this.timerRifornimento = 0f;
		}
		if (flag)
		{
			this.suonoRifor.Play();
		}
	}

	// Token: 0x04000EE7 RID: 3815
	private float frequenzaDiRifornimento;

	// Token: 0x04000EE8 RID: 3816
	private float timerRifornimento;

	// Token: 0x04000EE9 RID: 3817
	public GameObject strutturaPrefabFPS1;

	// Token: 0x04000EEA RID: 3818
	public Vector3 posizionamentoCameraTPS;

	// Token: 0x04000EEB RID: 3819
	public Vector3 posizionamentoCameraFPS;

	// Token: 0x04000EEC RID: 3820
	public Vector3 aggiustamentoTraslVisualeFPS;

	// Token: 0x04000EED RID: 3821
	public Vector3 aggiustamentoRotVisualeFPS;

	// Token: 0x04000EEE RID: 3822
	private GameObject strutturaFPS1;

	// Token: 0x04000EEF RID: 3823
	private float timerPosizionamentoTPS;

	// Token: 0x04000EF0 RID: 3824
	private float timerPosizionamentoFPS;

	// Token: 0x04000EF1 RID: 3825
	private float campoCameraIniziale;

	// Token: 0x04000EF2 RID: 3826
	private GameObject infoNeutreTattica;

	// Token: 0x04000EF3 RID: 3827
	private GameObject terzaCamera;

	// Token: 0x04000EF4 RID: 3828
	private GameObject primaCamera;

	// Token: 0x04000EF5 RID: 3829
	private GameObject infoAlleati;

	// Token: 0x04000EF6 RID: 3830
	private NavMeshAgent alleatoNav;

	// Token: 0x04000EF7 RID: 3831
	private float velocitàAlleatoNav;

	// Token: 0x04000EF8 RID: 3832
	private AudioSource suonoMotore;

	// Token: 0x04000EF9 RID: 3833
	public AudioClip motoreFermo;

	// Token: 0x04000EFA RID: 3834
	public AudioClip motorePartenza;

	// Token: 0x04000EFB RID: 3835
	public AudioClip motoreViaggio;

	// Token: 0x04000EFC RID: 3836
	public AudioClip motoreStop;

	// Token: 0x04000EFD RID: 3837
	private float timerPartenza;

	// Token: 0x04000EFE RID: 3838
	private float timerStop;

	// Token: 0x04000EFF RID: 3839
	private bool primaPartenza;

	// Token: 0x04000F00 RID: 3840
	private bool inPartenza;

	// Token: 0x04000F01 RID: 3841
	private bool partenzaFinita;

	// Token: 0x04000F02 RID: 3842
	private bool inStop;

	// Token: 0x04000F03 RID: 3843
	public bool stopFinito;

	// Token: 0x04000F04 RID: 3844
	private float distFineOrdineMovimento;

	// Token: 0x04000F05 RID: 3845
	private AudioSource suonoRifor;

	// Token: 0x04000F06 RID: 3846
	private GameObject ruote1;

	// Token: 0x04000F07 RID: 3847
	private GameObject ruote2;

	// Token: 0x04000F08 RID: 3848
	private GameObject ruote3;
}
