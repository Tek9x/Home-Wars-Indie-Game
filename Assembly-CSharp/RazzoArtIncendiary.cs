using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009C RID: 156
public class RazzoArtIncendiary : MonoBehaviour
{
	// Token: 0x060005E4 RID: 1508 RVA: 0x000CBA58 File Offset: 0x000C9C58
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
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x000CBB18 File Offset: 0x000C9D18
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

	// Token: 0x060005E6 RID: 1510 RVA: 0x000CBE70 File Offset: 0x000CA070
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

	// Token: 0x060005E7 RID: 1511 RVA: 0x000CBFA8 File Offset: 0x000CA1A8
	private void Esplosione()
	{
		if (this.timerImpatto > 0f)
		{
			if (this.timerImpatto < 0.5f)
			{
				base.transform.up = this.perpASuperfCollisione;
				float num = Vector3.Dot(Vector3.up, this.perpASuperfCollisione);
				if (num < 0.8f)
				{
					base.transform.GetChild(2).localPosition = new Vector3(0f, 2f, 0f);
					base.transform.GetChild(3).localPosition = new Vector3(0f, 12f, 0f);
				}
			}
			base.transform.up = this.perpASuperfCollisione;
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
								float num2 = 0f;
								if (current.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione)
								{
									num2 = base.GetComponent<DatiGeneraliMunizione>().penetrazione;
								}
								else if (current.GetComponent<PresenzaNemico>().vita > 0f)
								{
									num2 = current.GetComponent<PresenzaNemico>().vita;
								}
								current.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione;
								if (current.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura))
								{
									num2 += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura);
								}
								else if (current.GetComponent<PresenzaNemico>().vita > 0f)
								{
									num2 += current.GetComponent<PresenzaNemico>().vita;
								}
								current.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura);
								List<float> listaDanniAlleati;
								List<float> expr_258 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
								int index;
								int expr_261 = index = this.truppaDiOrigineDelSupporto;
								float num3 = listaDanniAlleati[index];
								expr_258[expr_261] = num3 + num2;
							}
							else if (current.tag == "ObbiettivoTattico" && (current.name == "Avamposto Nemico(Clone)" || current.name == "Pane per Convoglio(Clone)"))
							{
								float num4 = 0f;
								if (current.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione)
								{
									num4 = base.GetComponent<DatiGeneraliMunizione>().penetrazione;
								}
								else if (current.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
								{
									num4 = current.GetComponent<ObbiettivoTatticoScript>().vita;
								}
								current.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione;
								if (current.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().danno)
								{
									num4 += base.GetComponent<DatiGeneraliMunizione>().danno;
								}
								else if (current.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
								{
									num4 += current.GetComponent<ObbiettivoTatticoScript>().vita;
								}
								current.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno;
								List<float> listaDanniAlleati2;
								List<float> expr_3B1 = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
								int index;
								int expr_3BA = index = this.truppaDiOrigineDelSupporto;
								float num3 = listaDanniAlleati2[index];
								expr_3B1[expr_3BA] = num3 + num4;
							}
						}
						else if (current.tag == "Nemico")
						{
							this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
							float num5 = 0f;
							if (current.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
							{
								num5 = base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
							}
							else if (current.GetComponent<PresenzaNemico>().vita > 0f)
							{
								num5 = current.GetComponent<PresenzaNemico>().vita;
							}
							current.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
							if (current.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS)
							{
								num5 += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
							}
							else if (current.GetComponent<PresenzaNemico>().vita > 0f)
							{
								num5 += current.GetComponent<PresenzaNemico>().vita;
							}
							current.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
							List<float> listaDanniAlleati3;
							List<float> expr_556 = listaDanniAlleati3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
							int index;
							int expr_55F = index = this.truppaDiOrigineDelSupporto;
							float num3 = listaDanniAlleati3[index];
							expr_556[expr_55F] = num3 + num5;
							this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num5;
						}
						else if (current.tag == "ObbiettivoTattico" && (current.name == "Avamposto Nemico(Clone)" || current.name == "Pane per Convoglio(Clone)"))
						{
							this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
							float num6 = 0f;
							if (current.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
							{
								num6 = base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
							}
							else if (current.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
							{
								num6 = current.GetComponent<ObbiettivoTatticoScript>().vita;
							}
							current.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
							if (current.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * this.moltiplicatoreAttaccoInFPS)
							{
								num6 += base.GetComponent<DatiGeneraliMunizione>().danno * this.moltiplicatoreAttaccoInFPS;
							}
							else if (current.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
							{
								num6 += current.GetComponent<ObbiettivoTatticoScript>().vita;
							}
							current.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * this.moltiplicatoreAttaccoInFPS;
							List<float> listaDanniAlleati4;
							List<float> expr_708 = listaDanniAlleati4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
							int index;
							int expr_711 = index = this.truppaDiOrigineDelSupporto;
							float num3 = listaDanniAlleati4[index];
							expr_708[expr_711] = num3 + num6;
							this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num6;
						}
					}
				}
			}
			if (!this.incendioPartito && this.timerImpatto > 0.2f)
			{
				base.transform.GetChild(2).GetComponent<AudioSource>().Play();
				this.incendioPartito = true;
			}
		}
		if (this.timerImpatto > this.lunghezzaVitaBomba)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x000CC784 File Offset: 0x000CA984
	private void OnCollisionEnter(Collision collisione)
	{
		if (collisione.gameObject.tag == "Ambiente")
		{
			this.corpoRigido.isKinematic = true;
			base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<CapsuleCollider>().enabled = false;
			base.GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(3).GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
			base.transform.GetChild(1).GetComponent<AudioSource>().Stop();
			base.GetComponent<AudioSource>().Play();
			this.avviaTimer = true;
			this.perpASuperfCollisione = collisione.contacts[0].normal.normalized;
		}
	}

	// Token: 0x0400162E RID: 5678
	private GameObject supportoOriginario;

	// Token: 0x0400162F RID: 5679
	public float forzaImpulso;

	// Token: 0x04001630 RID: 5680
	private float timerPartenza;

	// Token: 0x04001631 RID: 5681
	private float timerImpatto;

	// Token: 0x04001632 RID: 5682
	private bool avviaTimer;

	// Token: 0x04001633 RID: 5683
	private bool esplosioneAvvenuta;

	// Token: 0x04001634 RID: 5684
	private Vector3 origine;

	// Token: 0x04001635 RID: 5685
	private Vector3 puntoBersaglio;

	// Token: 0x04001636 RID: 5686
	private Vector3 traiettoriaColpo;

	// Token: 0x04001637 RID: 5687
	private bool cancellamentoDaLista;

	// Token: 0x04001638 RID: 5688
	private GameObject terzaCamera;

	// Token: 0x04001639 RID: 5689
	private GameObject infoNeutreTattica;

	// Token: 0x0400163A RID: 5690
	private GameObject oggettoColpito;

	// Token: 0x0400163B RID: 5691
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x0400163C RID: 5692
	private Vector3 direzione;

	// Token: 0x0400163D RID: 5693
	private RaycastHit puntoDiMira;

	// Token: 0x0400163E RID: 5694
	private Rigidbody corpoRigido;

	// Token: 0x0400163F RID: 5695
	public float velocitàAutoRotazione;

	// Token: 0x04001640 RID: 5696
	private GameObject IANemico;

	// Token: 0x04001641 RID: 5697
	private RaycastHit hitProiettile;

	// Token: 0x04001642 RID: 5698
	private Vector3 puntoDiMezzo;

	// Token: 0x04001643 RID: 5699
	private Vector3 vertice;

	// Token: 0x04001644 RID: 5700
	private Vector3 direzioneSparo;

	// Token: 0x04001645 RID: 5701
	private Vector3 posizioneMassima;

	// Token: 0x04001646 RID: 5702
	private bool verticeRaggiunto;

	// Token: 0x04001647 RID: 5703
	private Vector3 dirDiCaduta;

	// Token: 0x04001648 RID: 5704
	private float velocitàAlPicco;

	// Token: 0x04001649 RID: 5705
	private Vector3 locazioneTarget;

	// Token: 0x0400164A RID: 5706
	private float distanzaDiMetà;

	// Token: 0x0400164B RID: 5707
	private float timerIncrementoVelocità;

	// Token: 0x0400164C RID: 5708
	private float incrementoVelocità;

	// Token: 0x0400164D RID: 5709
	private bool incendioPartito;

	// Token: 0x0400164E RID: 5710
	public float lunghezzaVitaBomba;

	// Token: 0x0400164F RID: 5711
	private float timerDanno;

	// Token: 0x04001650 RID: 5712
	private Vector3 perpASuperfCollisione;

	// Token: 0x04001651 RID: 5713
	private int truppaDiOrigineDelSupporto;
}
