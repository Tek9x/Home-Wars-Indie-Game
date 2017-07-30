using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class RazzoArtToxic : MonoBehaviour
{
	// Token: 0x060005EA RID: 1514 RVA: 0x000CC878 File Offset: 0x000CAA78
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
		this.incrementoVelocità = 1f;
		base.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x000CC94C File Offset: 0x000CAB4C
	private void Update()
	{
		if (this.supportoOriginario)
		{
			if (base.GetComponent<DatiGeneraliMunizione>().ordignoLocaleAttivo)
			{
				this.timerPartenza += Time.deltaTime;
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
				else if (this.timerPartenza > 0.2f && this.timerImpatto == 0f)
				{
					base.transform.forward = this.corpoRigido.velocity;
				}
				if (this.avviaTimer)
				{
					this.timerImpatto += Time.deltaTime;
				}
				if (this.timerPartenza > 0.4f)
				{
					base.GetComponent<CapsuleCollider>().enabled = true;
					base.transform.GetChild(0).GetComponent<SphereCollider>().enabled = true;
					base.transform.GetChild(0).GetComponent<SphereCollider>().radius = base.GetComponent<DatiGeneraliMunizione>().raggioEffetto;
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

	// Token: 0x060005EC RID: 1516 RVA: 0x000CCCA4 File Offset: 0x000CAEA4
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

	// Token: 0x060005ED RID: 1517 RVA: 0x000CCDDC File Offset: 0x000CAFDC
	private void Esplosione()
	{
		if (this.timerImpatto > 0f)
		{
			this.timerDanno += Time.deltaTime;
			if (this.timerDanno > 1f)
			{
				this.timerDanno = 0f;
				foreach (GameObject current in base.transform.GetChild(0).GetComponent<ColliderAreaEffetto>().ListaAeraEffetto)
				{
					if (current != null)
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
								List<float> expr_1C0 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
								int index;
								int expr_1C9 = index = this.truppaDiOrigineDelSupporto;
								float num2 = listaDanniAlleati[index];
								expr_1C0[expr_1C9] = num2 + num;
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
								List<float> expr_312 = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
								int index;
								int expr_31B = index = this.truppaDiOrigineDelSupporto;
								float num2 = listaDanniAlleati2[index];
								expr_312[expr_31B] = num2 + num3;
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
							List<float> expr_4B6 = listaDanniAlleati3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
							int index;
							int expr_4BF = index = this.truppaDiOrigineDelSupporto;
							float num2 = listaDanniAlleati3[index];
							expr_4B6[expr_4BF] = num2 + num4;
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
							List<float> expr_668 = listaDanniAlleati4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
							int index;
							int expr_671 = index = this.truppaDiOrigineDelSupporto;
							float num2 = listaDanniAlleati4[index];
							expr_668[expr_671] = num2 + num5;
							this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num5;
						}
					}
				}
			}
			if (!this.gasPartito && this.timerImpatto > 0.2f)
			{
				base.transform.GetChild(2).GetComponent<AudioSource>().Play();
				this.gasPartito = true;
			}
		}
		if (this.timerImpatto > this.lunghezzaVitaBomba)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x000CD518 File Offset: 0x000CB718
	private void OnCollisionEnter(Collision collisione)
	{
		if (collisione.gameObject.tag == "Ambiente" || collisione.gameObject.tag == "Nemico" || collisione.gameObject.tag == "Nemico Testa" || collisione.gameObject.tag == "Nemico Coll Suppl" || collisione.gameObject.tag == "ObbiettivoTattico")
		{
			this.corpoRigido.isKinematic = true;
			base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<CapsuleCollider>().enabled = false;
			base.GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(2).GetComponent<AudioSource>().Stop();
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
			base.transform.GetChild(1).GetComponent<AudioSource>().Stop();
			base.GetComponent<AudioSource>().Play();
			this.avviaTimer = true;
		}
	}

	// Token: 0x04001652 RID: 5714
	private GameObject supportoOriginario;

	// Token: 0x04001653 RID: 5715
	public float forzaImpulso;

	// Token: 0x04001654 RID: 5716
	private float timerPartenza;

	// Token: 0x04001655 RID: 5717
	private float timerImpatto;

	// Token: 0x04001656 RID: 5718
	private bool avviaTimer;

	// Token: 0x04001657 RID: 5719
	private bool esplosioneAvvenuta;

	// Token: 0x04001658 RID: 5720
	private Vector3 origine;

	// Token: 0x04001659 RID: 5721
	private Vector3 puntoBersaglio;

	// Token: 0x0400165A RID: 5722
	private Vector3 traiettoriaColpo;

	// Token: 0x0400165B RID: 5723
	private bool cancellamentoDaLista;

	// Token: 0x0400165C RID: 5724
	private GameObject terzaCamera;

	// Token: 0x0400165D RID: 5725
	private GameObject infoNeutreTattica;

	// Token: 0x0400165E RID: 5726
	private GameObject oggettoColpito;

	// Token: 0x0400165F RID: 5727
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x04001660 RID: 5728
	private Vector3 direzione;

	// Token: 0x04001661 RID: 5729
	private RaycastHit puntoDiMira;

	// Token: 0x04001662 RID: 5730
	private Rigidbody corpoRigido;

	// Token: 0x04001663 RID: 5731
	public float velocitàAutoRotazione;

	// Token: 0x04001664 RID: 5732
	private GameObject IANemico;

	// Token: 0x04001665 RID: 5733
	private RaycastHit hitProiettile;

	// Token: 0x04001666 RID: 5734
	private Vector3 puntoDiMezzo;

	// Token: 0x04001667 RID: 5735
	private Vector3 vertice;

	// Token: 0x04001668 RID: 5736
	private Vector3 direzioneSparo;

	// Token: 0x04001669 RID: 5737
	private Vector3 posizioneMassima;

	// Token: 0x0400166A RID: 5738
	private bool verticeRaggiunto;

	// Token: 0x0400166B RID: 5739
	private Vector3 dirDiCaduta;

	// Token: 0x0400166C RID: 5740
	private float velocitàAlPicco;

	// Token: 0x0400166D RID: 5741
	private Vector3 locazioneTarget;

	// Token: 0x0400166E RID: 5742
	private float distanzaDiMetà;

	// Token: 0x0400166F RID: 5743
	private float timerIncrementoVelocità;

	// Token: 0x04001670 RID: 5744
	private float incrementoVelocità;

	// Token: 0x04001671 RID: 5745
	private bool gasPartito;

	// Token: 0x04001672 RID: 5746
	public float lunghezzaVitaBomba;

	// Token: 0x04001673 RID: 5747
	private float timerDanno;

	// Token: 0x04001674 RID: 5748
	private int truppaDiOrigineDelSupporto;
}
