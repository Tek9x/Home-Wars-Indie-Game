using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F2 RID: 242
public class PresenzaNemicaStrategica : MonoBehaviour
{
	// Token: 0x060007CA RID: 1994 RVA: 0x001150E4 File Offset: 0x001132E4
	private void Start()
	{
		this.insettoNav = base.GetComponent<NavMeshAgent>();
		this.velocitàIniziale = this.insettoNav.speed;
		this.InsettoAnim = base.GetComponent<Animator>();
		this.Nest = GameObject.FindGameObjectWithTag("Nest");
		this.Headquarters = GameObject.FindGameObjectWithTag("Headquarters");
		this.cameraCasa = GameObject.FindGameObjectWithTag("MainCamera");
		base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		foreach (GameObject current in this.ListaPartiDelCorpo)
		{
			current.GetComponent<SkinnedMeshRenderer>().enabled = false;
		}
		this.suonoInsetto = base.GetComponent<AudioSource>();
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x001151CC File Offset: 0x001133CC
	private void Update()
	{
		if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().turnoNemicoAttivo && !this.Nest.GetComponent<IANemicoStrategia>().VittoriaNemica)
		{
			this.MovimentoADestinazione();
		}
		else if (!this.cameraCasa.GetComponent<GestoreNeutroStrategia>().turnoNemicoAttivo)
		{
			this.InsettoAnim.SetBool(this.camminataHash, false);
		}
		this.Visibilità();
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x0011523C File Offset: 0x0011343C
	private void MovimentoADestinazione()
	{
		if (base.GetComponent<ColliderEsercito>().ListaPosizioneEsercito.Count > 0)
		{
			this.posizioneAttuale = base.GetComponent<ColliderEsercito>().ListaPosizioneEsercito[base.GetComponent<ColliderEsercito>().ListaPosizioneEsercito.Count - 1];
		}
		if (this.destinazione == null)
		{
			this.destinazione = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[0];
		}
		if (this.muoviti && this.posizioneAttuale == this.vecchiaPosizioneAttuale)
		{
			this.muoviti = false;
			this.insettoNav.speed = this.velocitàIniziale;
			this.insettoNav.SetDestination(this.destinazione.transform.position);
			int index = UnityEngine.Random.Range(0, this.ListaVersiInsetti.Count);
			GestoreNeutroStrategia.valoreRandomSeed++;
			UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
			this.suonoInsetto.clip = this.ListaVersiInsetti[index];
			this.suonoInsetto.Play();
		}
		if (this.tipoDiSwarm == 0 && this.posizioneAttuale != this.vecchiaPosizioneAttuale)
		{
			this.insettoNav.speed = this.velocitàIniziale;
			this.destinazione = this.posizioneAttuale;
			this.insettoNav.SetDestination(this.destinazione.transform.position);
		}
		if (this.insettoNav.velocity.magnitude > 0f)
		{
			this.InsettoAnim.SetBool(this.camminataHash, true);
		}
		float num = Vector3.Distance(base.transform.position, this.destinazione.transform.position);
		if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().timerTurnoNemici > 1f && this.insettoNav.velocity.magnitude < 0.1f && num < this.insettoNav.stoppingDistance + 1f)
		{
			this.insettoNav.speed = 0f;
			this.prontoPerFineTurnoNemico = true;
			this.InsettoAnim.SetBool(this.camminataHash, false);
		}
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x00115474 File Offset: 0x00113674
	private void Visibilità()
	{
		this.timerVisibilità += Time.deltaTime;
		if (this.timerVisibilità > 1f)
		{
			bool flag = false;
			int num = 0;
			while (num < this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze.Count && !flag)
			{
				if (this.posizioneAttuale == this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaCentroStanze[num])
				{
					if (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().ListaSatelliti[num] == 1 || this.posizioneAttuale.GetComponent<CentroStanza>().appartenenzaBandiera == 1 || this.posizioneAttuale.GetComponent<CentroStanza>().ListaEserAlleatiPres.Count > 0)
					{
						base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
						foreach (GameObject current in this.ListaPartiDelCorpo)
						{
							current.GetComponent<SkinnedMeshRenderer>().enabled = true;
						}
					}
					else
					{
						base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
						foreach (GameObject current2 in this.ListaPartiDelCorpo)
						{
							current2.GetComponent<SkinnedMeshRenderer>().enabled = false;
						}
					}
					flag = true;
				}
				num++;
			}
			this.timerVisibilità = 0f;
		}
	}

	// Token: 0x04001D56 RID: 7510
	public int numIdentitàNemico;

	// Token: 0x04001D57 RID: 7511
	public int tipoDiSwarm;

	// Token: 0x04001D58 RID: 7512
	private NavMeshAgent insettoNav;

	// Token: 0x04001D59 RID: 7513
	public GameObject destinazione;

	// Token: 0x04001D5A RID: 7514
	public bool muoviti;

	// Token: 0x04001D5B RID: 7515
	private float velocitàIniziale;

	// Token: 0x04001D5C RID: 7516
	public GameObject posizioneAttuale;

	// Token: 0x04001D5D RID: 7517
	public GameObject vecchiaPosizioneAttuale;

	// Token: 0x04001D5E RID: 7518
	public bool prontoPerFineTurnoNemico;

	// Token: 0x04001D5F RID: 7519
	public int tipoComportamentoGruppo;

	// Token: 0x04001D60 RID: 7520
	private GameObject Nest;

	// Token: 0x04001D61 RID: 7521
	private GameObject Headquarters;

	// Token: 0x04001D62 RID: 7522
	private GameObject cameraCasa;

	// Token: 0x04001D63 RID: 7523
	private Animator InsettoAnim;

	// Token: 0x04001D64 RID: 7524
	private int camminataHash = Animator.StringToHash("insetto-camminata");

	// Token: 0x04001D65 RID: 7525
	public List<List<int>> ListaInsettiInEser;

	// Token: 0x04001D66 RID: 7526
	public string nomeEserInsetti;

	// Token: 0x04001D67 RID: 7527
	public List<GameObject> ListaPartiDelCorpo;

	// Token: 0x04001D68 RID: 7528
	private float timerVisibilità;

	// Token: 0x04001D69 RID: 7529
	public int coinvoltoInBattNemico;

	// Token: 0x04001D6A RID: 7530
	public int tipoDiInsettoOrda;

	// Token: 0x04001D6B RID: 7531
	public List<AudioClip> ListaVersiInsetti;

	// Token: 0x04001D6C RID: 7532
	public AudioSource suonoInsetto;

	// Token: 0x04001D6D RID: 7533
	public int swarmSpecialeHaAttaccato;
}
