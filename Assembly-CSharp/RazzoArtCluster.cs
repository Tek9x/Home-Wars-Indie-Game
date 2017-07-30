using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009A RID: 154
public class RazzoArtCluster : MonoBehaviour
{
	// Token: 0x060005D7 RID: 1495 RVA: 0x000C8F38 File Offset: 0x000C7138
	private void Start()
	{
		this.supportoOriginario = base.transform.parent.gameObject;
		this.truppaDiOrigineDelSupporto = this.supportoOriginario.transform.parent.parent.parent.parent.GetComponent<PresenzaAlleato>().tipoTruppa;
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
		this.layerPerMira = 4359424;
		this.incrementoVelocità = 1f;
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x000C9004 File Offset: 0x000C7204
	private void Update()
	{
		if (this.supportoOriginario)
		{
			if (base.GetComponent<DatiGeneraliMunizione>().ordignoLocaleAttivo)
			{
				this.timerPartenza += Time.deltaTime;
				base.transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
				if (!base.GetComponent<Rigidbody>())
				{
					base.gameObject.AddComponent<Rigidbody>();
					this.corpoRigido = base.GetComponent<Rigidbody>();
					this.corpoRigido.useGravity = false;
					this.corpoRigido.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
					this.locazioneTarget = this.supportoOriginario.GetComponent<DatiOrdignoEsterno>().zonaTarget;
					if (!this.supportoOriginario.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS)
					{
						this.puntoDiMezzo = Vector3.Lerp(this.locazioneTarget, base.transform.position, 0.45f);
						this.distanzaDiMetà = Vector3.Distance(base.transform.position, this.puntoDiMezzo);
						this.corpoRigido.AddForce(base.transform.forward * this.forzaImpulso / 2f, ForceMode.VelocityChange);
					}
					else
					{
						this.corpoRigido.AddForce(base.transform.forward * this.forzaImpulso, ForceMode.VelocityChange);
						this.corpoRigido.useGravity = true;
					}
					this.supportoOriginario.GetComponent<AudioSource>().Play();
				}
				this.SensoreEsplosione();
				if (this.timerPartenza > 0f && this.timerPartenza < 0.1f)
				{
					if (this.timerImpatto == 0f)
					{
						base.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
						base.transform.GetChild(1).GetComponent<AudioSource>().Play();
					}
					this.origine = base.transform.position;
				}
				if (!this.cancellamentoDaLista)
				{
					base.transform.parent = null;
					int index = this.supportoOriginario.GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche.IndexOf(base.gameObject);
					this.supportoOriginario.GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[index] = null;
					this.cancellamentoDaLista = true;
				}
				this.Esplosione();
				if (!this.supportoOriginario.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS)
				{
					this.MovimentoIndipendente();
				}
				else if (this.timerPartenza > 0.2f)
				{
					base.transform.forward = this.corpoRigido.velocity;
				}
				if (this.avviaTimer)
				{
					this.timerImpatto += Time.deltaTime;
					base.transform.up = this.perpASuperfCollisione;
					if (this.timerImpatto > 0.4f && !this.audioSecondaEspl)
					{
						this.audioSecondaEspl = true;
						base.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
						base.transform.GetChild(2).GetComponent<AudioSource>().Play();
					}
				}
				if (this.timerPartenza > 1f)
				{
					base.transform.GetChild(0).GetComponent<SphereCollider>().enabled = true;
					base.transform.GetChild(0).GetComponent<SphereCollider>().radius = base.GetComponent<DatiGeneraliMunizione>().raggioEffetto;
					if (!this.avviaTimer && Physics.Raycast(base.transform.position, -Vector3.up, out this.hitSuperficie, 26f, 256))
					{
						this.corpoRigido.isKinematic = true;
						base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
						base.GetComponent<CapsuleCollider>().enabled = false;
						base.GetComponent<ParticleSystem>().Play();
						base.transform.GetChild(1).GetComponent<ParticleSystem>().Clear();
						base.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
						base.transform.GetChild(1).GetComponent<AudioSource>().Stop();
						base.GetComponent<AudioSource>().Play();
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
		else if (this.timerImpatto == 0f)
		{
			base.transform.position += base.transform.forward * this.velocitàAlPicco * this.incrementoVelocità * Time.deltaTime;
		}
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x000C949C File Offset: 0x000C769C
	private void SensoreEsplosione()
	{
		if (!this.avviaTimer && this.timerPartenza > 1f && Physics.Raycast(base.transform.position, -Vector3.up, out this.hitSensore, 25f, this.layerPerMira))
		{
			this.corpoRigido.isKinematic = true;
			base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<CapsuleCollider>().enabled = false;
			base.GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Clear();
			base.transform.GetChild(1).GetComponent<AudioSource>().Stop();
			base.GetComponent<AudioSource>().Play();
			this.avviaTimer = true;
			this.Esplosione();
		}
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x000C9588 File Offset: 0x000C7788
	private void MovimentoIndipendente()
	{
		if (!this.verticeRaggiunto && this.distanzaDiMetà < Vector3.Distance(base.transform.position, this.origine))
		{
			this.velocitàAlPicco = this.corpoRigido.velocity.magnitude;
			this.corpoRigido.velocity = Vector3.zero;
			this.verticeRaggiunto = true;
		}
		if (this.verticeRaggiunto && !this.avviaTimer)
		{
			Quaternion to = Quaternion.LookRotation(this.locazioneTarget - base.transform.position);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.velocitàAutoRotazione * Time.deltaTime);
			base.transform.position += base.transform.forward * this.velocitàAlPicco * this.incrementoVelocità * Time.deltaTime;
			this.timerIncrementoVelocità += Time.deltaTime;
			if (this.timerIncrementoVelocità > 0.2f)
			{
				this.incrementoVelocità *= 1.005f;
			}
		}
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x000C96C0 File Offset: 0x000C78C0
	private void Esplosione()
	{
		if (!this.esplosioneAvvenuta && this.timerImpatto > 0f)
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

	// Token: 0x060005DC RID: 1500 RVA: 0x000C9DBC File Offset: 0x000C7FBC
	private void OnCollisionEnter(Collision collisione)
	{
	}

	// Token: 0x040015E9 RID: 5609
	private GameObject supportoOriginario;

	// Token: 0x040015EA RID: 5610
	public float forzaImpulso;

	// Token: 0x040015EB RID: 5611
	private float timerPartenza;

	// Token: 0x040015EC RID: 5612
	private float timerImpatto;

	// Token: 0x040015ED RID: 5613
	private bool avviaTimer;

	// Token: 0x040015EE RID: 5614
	private bool esplosioneAvvenuta;

	// Token: 0x040015EF RID: 5615
	private Vector3 origine;

	// Token: 0x040015F0 RID: 5616
	private Vector3 puntoBersaglio;

	// Token: 0x040015F1 RID: 5617
	private Vector3 traiettoriaColpo;

	// Token: 0x040015F2 RID: 5618
	private bool cancellamentoDaLista;

	// Token: 0x040015F3 RID: 5619
	private GameObject terzaCamera;

	// Token: 0x040015F4 RID: 5620
	private GameObject infoNeutreTattica;

	// Token: 0x040015F5 RID: 5621
	private GameObject oggettoColpito;

	// Token: 0x040015F6 RID: 5622
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x040015F7 RID: 5623
	private Vector3 direzione;

	// Token: 0x040015F8 RID: 5624
	private RaycastHit puntoDiMira;

	// Token: 0x040015F9 RID: 5625
	private int layerPerMira;

	// Token: 0x040015FA RID: 5626
	private Rigidbody corpoRigido;

	// Token: 0x040015FB RID: 5627
	public float velocitàAutoRotazione;

	// Token: 0x040015FC RID: 5628
	private GameObject IANemico;

	// Token: 0x040015FD RID: 5629
	private RaycastHit hitProiettile;

	// Token: 0x040015FE RID: 5630
	private Vector3 puntoDiMezzo;

	// Token: 0x040015FF RID: 5631
	private Vector3 vertice;

	// Token: 0x04001600 RID: 5632
	private Vector3 direzioneSparo;

	// Token: 0x04001601 RID: 5633
	private Vector3 posizioneMassima;

	// Token: 0x04001602 RID: 5634
	private bool verticeRaggiunto;

	// Token: 0x04001603 RID: 5635
	private Vector3 dirDiCaduta;

	// Token: 0x04001604 RID: 5636
	private float velocitàAlPicco;

	// Token: 0x04001605 RID: 5637
	private Vector3 locazioneTarget;

	// Token: 0x04001606 RID: 5638
	private float distanzaDiMetà;

	// Token: 0x04001607 RID: 5639
	private float timerIncrementoVelocità;

	// Token: 0x04001608 RID: 5640
	private float incrementoVelocità;

	// Token: 0x04001609 RID: 5641
	private bool audioSecondaEspl;

	// Token: 0x0400160A RID: 5642
	private RaycastHit hitSensore;

	// Token: 0x0400160B RID: 5643
	private Vector3 perpASuperfCollisione;

	// Token: 0x0400160C RID: 5644
	private RaycastHit hitSuperficie;

	// Token: 0x0400160D RID: 5645
	private int truppaDiOrigineDelSupporto;
}
