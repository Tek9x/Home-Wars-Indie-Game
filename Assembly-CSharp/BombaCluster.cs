using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008F RID: 143
public class BombaCluster : MonoBehaviour
{
	// Token: 0x06000593 RID: 1427 RVA: 0x000BA158 File Offset: 0x000B8358
	private void Start()
	{
		this.supportoOriginario = base.transform.parent.gameObject;
		this.truppaDiOrigineDelSupporto = this.supportoOriginario.transform.parent.GetComponent<PresenzaAlleato>().tipoTruppa;
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		if (GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.moltiplicatoreAttaccoInFPS = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().moltiplicatoreFPSBattVeloce;
		}
		else
		{
			this.moltiplicatoreAttaccoInFPS = PlayerPrefs.GetFloat("moltiplicatore danni PP");
		}
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x000BA1FC File Offset: 0x000B83FC
	private void Update()
	{
		if (this.supportoOriginario)
		{
			if (base.GetComponent<DatiGeneraliMunizione>().ordignoLocaleAttivo)
			{
				if (!base.GetComponent<Rigidbody>())
				{
					base.gameObject.AddComponent<Rigidbody>();
					this.corpoRigido = base.GetComponent<Rigidbody>();
					this.corpoRigido.useGravity = false;
					this.corpoRigido.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
					base.transform.GetChild(0).GetComponent<AudioSource>().Play();
				}
				this.origine = base.transform.position;
				if (!this.cancellamentoDaLista)
				{
					base.transform.parent = null;
					int index = this.supportoOriginario.GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche.IndexOf(base.gameObject);
					this.supportoOriginario.GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[index] = null;
					this.cancellamentoDaLista = true;
				}
				this.timerPartenza += Time.deltaTime;
				if (this.avviaTimer)
				{
					this.timerImpatto += Time.deltaTime;
					this.Esplosione();
					base.transform.up = this.perpASuperfCollisione;
					if (this.timerImpatto > 0.4f && !this.audioSecondaEspl)
					{
						this.audioSecondaEspl = true;
						base.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
						base.transform.GetChild(1).GetComponent<AudioSource>().Play();
					}
				}
				else
				{
					this.Movimento();
				}
				if (this.timerPartenza > 0.5f)
				{
					base.transform.GetChild(0).GetComponent<SphereCollider>().enabled = true;
					base.transform.GetChild(0).GetComponent<SphereCollider>().radius = base.GetComponent<DatiGeneraliMunizione>().raggioEffetto;
					if (!this.avviaTimer && Physics.Raycast(base.transform.position, -Vector3.up, out this.hitSuperficie, 25f, 256))
					{
						this.corpoRigido.isKinematic = true;
						base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
						base.GetComponent<BoxCollider>().enabled = false;
						base.GetComponent<ParticleSystem>().Play();
						base.GetComponent<AudioSource>().Play();
						base.transform.GetChild(0).GetComponent<AudioSource>().Stop();
						this.avviaTimer = true;
						this.perpASuperfCollisione = this.hitSuperficie.normal.normalized;
					}
				}
			}
		}
		else if (!base.GetComponent<DatiGeneraliMunizione>().ordignoLocaleAttivo)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x000BA4A0 File Offset: 0x000B86A0
	private void Movimento()
	{
		this.velocitàVerticale += Time.deltaTime * 25f;
		base.transform.position += Vector3.down * this.velocitàVerticale * Time.deltaTime;
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x000BA4F8 File Offset: 0x000B86F8
	private void Esplosione()
	{
		if (!this.esplosioneAvvenuta && this.timerImpatto > 0.4f)
		{
			foreach (GameObject current in base.transform.GetChild(0).GetComponent<ColliderAreaEffetto>().ListaAeraEffetto)
			{
				if (current != null && current != this.oggettoColpito)
				{
					if (!this.supportoOriginario.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS)
					{
						if (current.tag == "Nemico")
						{
							float num = 0f;
							if (current.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione)
							{
								num = base.GetComponent<DatiGeneraliMunizione>().penetrazione;
							}
							else if (current.GetComponent<PresenzaNemico>().vita > 0f)
							{
								num = current.GetComponent<PresenzaNemico>().vita;
							}
							current.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione;
							if (current.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura))
							{
								num += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura);
							}
							else if (current.GetComponent<PresenzaNemico>().vita > 0f)
							{
								num += current.GetComponent<PresenzaNemico>().vita;
							}
							current.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura);
							List<float> listaDanniAlleati;
							List<float> expr_1AF = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
							int index;
							int expr_1B8 = index = this.truppaDiOrigineDelSupporto;
							float num2 = listaDanniAlleati[index];
							expr_1AF[expr_1B8] = num2 + num;
						}
						else if (current.tag == "ObbiettivoTattico" && (current.name == "Avamposto Nemico(Clone)" || current.name == "Pane per Convoglio(Clone)"))
						{
							float num3 = 0f;
							if (current.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione)
							{
								num3 = base.GetComponent<DatiGeneraliMunizione>().penetrazione;
							}
							else if (current.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
							{
								num3 = current.GetComponent<ObbiettivoTatticoScript>().vita;
							}
							current.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione;
							if (current.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().danno)
							{
								num3 += base.GetComponent<DatiGeneraliMunizione>().danno;
							}
							else if (current.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
							{
								num3 += current.GetComponent<ObbiettivoTatticoScript>().vita;
							}
							current.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno;
							List<float> listaDanniAlleati2;
							List<float> expr_301 = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
							int index;
							int expr_30A = index = this.truppaDiOrigineDelSupporto;
							float num2 = listaDanniAlleati2[index];
							expr_301[expr_30A] = num2 + num3;
						}
					}
					else if (current.tag == "Nemico")
					{
						this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
						float num4 = 0f;
						if (current.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
						{
							num4 = base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
						}
						else if (current.GetComponent<PresenzaNemico>().vita > 0f)
						{
							num4 = current.GetComponent<PresenzaNemico>().vita;
						}
						current.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
						if (current.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS)
						{
							num4 += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
						}
						else if (current.GetComponent<PresenzaNemico>().vita > 0f)
						{
							num4 += current.GetComponent<PresenzaNemico>().vita;
						}
						current.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
						List<float> listaDanniAlleati3;
						List<float> expr_4A5 = listaDanniAlleati3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
						int index;
						int expr_4AE = index = this.truppaDiOrigineDelSupporto;
						float num2 = listaDanniAlleati3[index];
						expr_4A5[expr_4AE] = num2 + num4;
						this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num4;
					}
					else if (current.tag == "ObbiettivoTattico" && (current.name == "Avamposto Nemico(Clone)" || current.name == "Pane per Convoglio(Clone)"))
					{
						this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
						float num5 = 0f;
						if (current.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
						{
							num5 = base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
						}
						else if (current.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
						{
							num5 = current.GetComponent<ObbiettivoTatticoScript>().vita;
						}
						current.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
						if (current.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * this.moltiplicatoreAttaccoInFPS)
						{
							num5 += base.GetComponent<DatiGeneraliMunizione>().danno * this.moltiplicatoreAttaccoInFPS;
						}
						else if (current.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
						{
							num5 += current.GetComponent<ObbiettivoTatticoScript>().vita;
						}
						current.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * this.moltiplicatoreAttaccoInFPS;
						List<float> listaDanniAlleati4;
						List<float> expr_657 = listaDanniAlleati4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
						int index;
						int expr_660 = index = this.truppaDiOrigineDelSupporto;
						float num2 = listaDanniAlleati4[index];
						expr_657[expr_660] = num2 + num5;
						this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num5;
					}
				}
			}
			this.esplosioneAvvenuta = true;
		}
		if (this.timerImpatto > 5f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x000BABF4 File Offset: 0x000B8DF4
	private void OnCollisionEnter(Collision collisione)
	{
	}

	// Token: 0x04001507 RID: 5383
	private GameObject supportoOriginario;

	// Token: 0x04001508 RID: 5384
	public float forzaCaduta;

	// Token: 0x04001509 RID: 5385
	private float timerPartenza;

	// Token: 0x0400150A RID: 5386
	private float timerImpatto;

	// Token: 0x0400150B RID: 5387
	private bool avviaTimer;

	// Token: 0x0400150C RID: 5388
	private bool esplosioneAvvenuta;

	// Token: 0x0400150D RID: 5389
	private Vector3 origine;

	// Token: 0x0400150E RID: 5390
	private Vector3 puntoBersaglio;

	// Token: 0x0400150F RID: 5391
	private Vector3 traiettoriaColpo;

	// Token: 0x04001510 RID: 5392
	private bool audioViaggioAttivo;

	// Token: 0x04001511 RID: 5393
	private bool cancellamentoDaLista;

	// Token: 0x04001512 RID: 5394
	private GameObject terzaCamera;

	// Token: 0x04001513 RID: 5395
	private GameObject infoNeutreTattica;

	// Token: 0x04001514 RID: 5396
	private GameObject oggettoColpito;

	// Token: 0x04001515 RID: 5397
	private float forzaDiInerzia;

	// Token: 0x04001516 RID: 5398
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x04001517 RID: 5399
	private Rigidbody corpoRigido;

	// Token: 0x04001518 RID: 5400
	private bool audioSecondaEspl;

	// Token: 0x04001519 RID: 5401
	private float velocitàVerticale;

	// Token: 0x0400151A RID: 5402
	private Vector3 perpASuperfCollisione;

	// Token: 0x0400151B RID: 5403
	private RaycastHit hitSuperficie;

	// Token: 0x0400151C RID: 5404
	private int truppaDiOrigineDelSupporto;
}
