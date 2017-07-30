using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class PresenzaAlleataStrategica : MonoBehaviour
{
	// Token: 0x06000060 RID: 96 RVA: 0x00014B40 File Offset: 0x00012D40
	private void Start()
	{
		this.headquarters = GameObject.FindGameObjectWithTag("Headquarters");
		this.navEsercito = base.GetComponent<NavMeshAgent>();
		this.cameraCasa = GameObject.FindGameObjectWithTag("MainCamera");
		this.materPerEvidAlleato = this.cameraCasa.GetComponent<SelezionamentoInStrategia>().materPerEvidAlleato;
		this.materPerSelAlleato = this.cameraCasa.GetComponent<SelezionamentoInStrategia>().materPerSelAlleato;
		this.quadDiSel = base.transform.GetChild(0).gameObject;
		this.suonoDaEsercito = base.GetComponent<AudioSource>();
		this.turniMaxPrimaDiCanc = 2;
		this.segnoAttenzione = base.transform.FindChild("segno attenzione").gameObject;
		this.turniPerCancell = 100;
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00014BF4 File Offset: 0x00012DF4
	private void Update()
	{
		this.MovimentoGruppo();
		this.Evidenziazione();
		this.SegnoDiAttenzione();
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00014C08 File Offset: 0x00012E08
	private void MovimentoGruppo()
	{
		if (base.GetComponent<ColliderEsercito>().ListaPosizioneEsercito.Count > 0)
		{
			this.posizioneAttuale = base.GetComponent<ColliderEsercito>().ListaPosizioneEsercito[0];
		}
		if (this.spostamentoAttivo)
		{
			this.navEsercito.SetDestination(this.destinazione.transform.position);
			if (Vector3.Distance(base.transform.position, this.destinazione.transform.position) < 0.3f)
			{
				this.spostamentoAttivo = false;
			}
		}
		if (this.navEsercito.velocity.magnitude <= 0.1f)
		{
			this.èFermo = true;
		}
		else
		{
			this.èFermo = false;
		}
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00014CCC File Offset: 0x00012ECC
	private void Evidenziazione()
	{
		if (this.alleatoSelezionato)
		{
			this.quadDiSel.GetComponent<MeshRenderer>().material = this.materPerSelAlleato;
		}
		else
		{
			this.quadDiSel.GetComponent<MeshRenderer>().material = this.materPerEvidAlleato;
		}
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00014D18 File Offset: 0x00012F18
	private void SegnoDiAttenzione()
	{
		if (this.posizioneAttuale != null)
		{
			if (this.posizioneAttuale.GetComponent<CentroStanza>().settoriNemici == 3)
			{
				if (this.turniPerCancell == 100)
				{
					this.turniPerCancell = this.turniMaxPrimaDiCanc;
					this.primoTurnoPerCanc = this.cameraCasa.GetComponent<GestoreNeutroStrategia>().numeroTurno;
					this.segnoAttenzione.GetComponent<MeshRenderer>().material = this.materSegnoAtt2;
				}
				else
				{
					this.segnoAttenzione.GetComponent<MeshRenderer>().enabled = true;
					Vector3 normalized = Vector3.ProjectOnPlane(-this.cameraCasa.transform.forward, Vector3.up).normalized;
					this.segnoAttenzione.transform.forward = -normalized;
					this.turniPerCancell = this.turniMaxPrimaDiCanc - (this.cameraCasa.GetComponent<GestoreNeutroStrategia>().numeroTurno - this.primoTurnoPerCanc);
					if (this.turniPerCancell == this.turniMaxPrimaDiCanc)
					{
						this.segnoAttenzione.GetComponent<MeshRenderer>().material = this.materSegnoAtt2;
					}
					else if (this.turniPerCancell == this.turniMaxPrimaDiCanc - 1)
					{
						this.segnoAttenzione.GetComponent<MeshRenderer>().material = this.materSegnoAtt1;
					}
					else if (this.turniPerCancell == this.turniMaxPrimaDiCanc - 2)
					{
						this.segnoAttenzione.GetComponent<MeshRenderer>().material = this.materSegnoAtt0;
					}
					if (this.turniPerCancell == -1)
					{
						for (int i = 0; i < this.ListaTruppeInEser.Count; i++)
						{
							this.ListaTruppeInEser[i] = 100;
						}
						this.headquarters.GetComponent<GestioneEsercitiAlleati>().controlloEserVuoti = true;
					}
				}
			}
			else
			{
				this.turniPerCancell = 100;
				this.segnoAttenzione.GetComponent<MeshRenderer>().enabled = false;
			}
		}
	}

	// Token: 0x0400020B RID: 523
	public int numIdentitàAlleato;

	// Token: 0x0400020C RID: 524
	public List<int> ListaTruppeInEser;

	// Token: 0x0400020D RID: 525
	public bool alleatoSelezionato;

	// Token: 0x0400020E RID: 526
	public string nomeEsercito;

	// Token: 0x0400020F RID: 527
	private GameObject headquarters;

	// Token: 0x04000210 RID: 528
	public GameObject destinazione;

	// Token: 0x04000211 RID: 529
	private NavMeshAgent navEsercito;

	// Token: 0x04000212 RID: 530
	public bool spostamentoAttivo;

	// Token: 0x04000213 RID: 531
	public bool attaccoAttivo;

	// Token: 0x04000214 RID: 532
	public GameObject posizioneAttuale;

	// Token: 0x04000215 RID: 533
	public GameObject vecchiaPosizioneAttuale;

	// Token: 0x04000216 RID: 534
	public bool puòAncoraMuoversi;

	// Token: 0x04000217 RID: 535
	public bool èFermo;

	// Token: 0x04000218 RID: 536
	private Material materPerEvidAlleato;

	// Token: 0x04000219 RID: 537
	private Material materPerSelAlleato;

	// Token: 0x0400021A RID: 538
	private GameObject quadDiSel;

	// Token: 0x0400021B RID: 539
	private GameObject cameraCasa;

	// Token: 0x0400021C RID: 540
	public int coinvoltoInBattAlleato;

	// Token: 0x0400021D RID: 541
	public AudioSource suonoDaEsercito;

	// Token: 0x0400021E RID: 542
	public List<AudioClip> ListaVoceEserAlleato;

	// Token: 0x0400021F RID: 543
	public AudioClip suonoSelezEsercito;

	// Token: 0x04000220 RID: 544
	public int turniPerCancell;

	// Token: 0x04000221 RID: 545
	private int turniMaxPrimaDiCanc;

	// Token: 0x04000222 RID: 546
	public int primoTurnoPerCanc;

	// Token: 0x04000223 RID: 547
	private GameObject segnoAttenzione;

	// Token: 0x04000224 RID: 548
	public Material materSegnoAtt0;

	// Token: 0x04000225 RID: 549
	public Material materSegnoAtt1;

	// Token: 0x04000226 RID: 550
	public Material materSegnoAtt2;
}
