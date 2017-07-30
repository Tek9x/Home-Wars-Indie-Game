using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000091 RID: 145
public class BombaIncendiary : MonoBehaviour
{
	// Token: 0x0600059F RID: 1439 RVA: 0x000BC4F4 File Offset: 0x000BA6F4
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
		this.corpoRigido = base.GetComponent<Rigidbody>();
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x000BC5A4 File Offset: 0x000BA7A4
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
				}
				else
				{
					this.Movimento();
				}
				if (this.timerPartenza > 1f)
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
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x000BC740 File Offset: 0x000BA940
	private void Movimento()
	{
		this.velocitàVerticale += Time.deltaTime * 25f;
		base.transform.position += Vector3.down * this.velocitàVerticale * Time.deltaTime;
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x000BC798 File Offset: 0x000BA998
	private void Esplosione()
	{
		if (this.timerImpatto > 0f)
		{
			base.transform.up = this.perpASuperfCollisione;
			this.timerDanno += Time.deltaTime;
			if (this.timerImpatto < 0.5f)
			{
				float num = Vector3.Dot(Vector3.up, this.perpASuperfCollisione);
				if (num < 0.8f)
				{
					base.transform.GetChild(1).localPosition = new Vector3(0f, 2f, 0f);
					base.transform.GetChild(2).localPosition = new Vector3(0f, 12f, 0f);
				}
			}
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
								List<float> expr_247 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
								int index;
								int expr_250 = index = this.truppaDiOrigineDelSupporto;
								float num3 = listaDanniAlleati[index];
								expr_247[expr_250] = num3 + num2;
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
								List<float> expr_3A0 = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
								int index;
								int expr_3A9 = index = this.truppaDiOrigineDelSupporto;
								float num3 = listaDanniAlleati2[index];
								expr_3A0[expr_3A9] = num3 + num4;
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
							List<float> expr_545 = listaDanniAlleati3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
							int index;
							int expr_54E = index = this.truppaDiOrigineDelSupporto;
							float num3 = listaDanniAlleati3[index];
							expr_545[expr_54E] = num3 + num5;
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
							List<float> expr_6F7 = listaDanniAlleati4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
							int index;
							int expr_700 = index = this.truppaDiOrigineDelSupporto;
							float num3 = listaDanniAlleati4[index];
							expr_6F7[expr_700] = num3 + num6;
							this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num6;
						}
					}
				}
			}
			if (!this.incendioPartito && this.timerImpatto > 0.2f)
			{
				base.transform.GetChild(1).GetComponent<AudioSource>().Play();
				this.incendioPartito = true;
			}
		}
		if (this.timerImpatto > this.lunghezzaVitaBomba)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x000BCF64 File Offset: 0x000BB164
	private void OnCollisionEnter(Collision collisione)
	{
		if (collisione.gameObject.tag == "Ambiente")
		{
			this.corpoRigido.isKinematic = true;
			base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<CapsuleCollider>().enabled = false;
			base.GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
			base.GetComponent<AudioSource>().Play();
			base.transform.GetChild(0).GetComponent<AudioSource>().Stop();
			this.avviaTimer = true;
			this.perpASuperfCollisione = collisione.contacts[0].normal.normalized;
		}
	}

	// Token: 0x04001531 RID: 5425
	private GameObject supportoOriginario;

	// Token: 0x04001532 RID: 5426
	public float forzaCaduta;

	// Token: 0x04001533 RID: 5427
	public float lunghezzaVitaBomba;

	// Token: 0x04001534 RID: 5428
	private float timerPartenza;

	// Token: 0x04001535 RID: 5429
	private float timerImpatto;

	// Token: 0x04001536 RID: 5430
	private bool avviaTimer;

	// Token: 0x04001537 RID: 5431
	private bool esplosioneAvvenuta;

	// Token: 0x04001538 RID: 5432
	private Vector3 origine;

	// Token: 0x04001539 RID: 5433
	private Vector3 puntoBersaglio;

	// Token: 0x0400153A RID: 5434
	private Vector3 traiettoriaColpo;

	// Token: 0x0400153B RID: 5435
	private bool cancellamentoDaLista;

	// Token: 0x0400153C RID: 5436
	private GameObject terzaCamera;

	// Token: 0x0400153D RID: 5437
	private float timerDanno;

	// Token: 0x0400153E RID: 5438
	private GameObject infoNeutreTattica;

	// Token: 0x0400153F RID: 5439
	private float forzaDiInerzia;

	// Token: 0x04001540 RID: 5440
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x04001541 RID: 5441
	private Rigidbody corpoRigido;

	// Token: 0x04001542 RID: 5442
	private bool incendioPartito;

	// Token: 0x04001543 RID: 5443
	private Vector3 perpASuperfCollisione;

	// Token: 0x04001544 RID: 5444
	private float velocitàVerticale;

	// Token: 0x04001545 RID: 5445
	private int truppaDiOrigineDelSupporto;
}
