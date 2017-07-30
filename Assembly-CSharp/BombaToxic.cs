using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000092 RID: 146
public class BombaToxic : MonoBehaviour
{
	// Token: 0x060005A5 RID: 1445 RVA: 0x000BD040 File Offset: 0x000BB240
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
		base.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x000BD108 File Offset: 0x000BB308
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

	// Token: 0x060005A7 RID: 1447 RVA: 0x000BD2A4 File Offset: 0x000BB4A4
	private void Movimento()
	{
		this.velocitàVerticale += Time.deltaTime * 25f;
		base.transform.position += Vector3.down * this.velocitàVerticale * Time.deltaTime;
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x000BD2FC File Offset: 0x000BB4FC
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
				base.transform.GetChild(1).GetComponent<AudioSource>().Play();
				this.gasPartito = true;
			}
		}
		if (this.timerImpatto > this.lunghezzaVitaBomba)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x000BDA38 File Offset: 0x000BBC38
	private void OnCollisionEnter(Collision collisione)
	{
		if (collisione.gameObject.tag == "Ambiente" || collisione.gameObject.tag == "Nemico" || collisione.gameObject.tag == "Nemico Testa" || collisione.gameObject.tag == "Nemico Coll Suppl" || collisione.gameObject.tag == "ObbiettivoTattico")
		{
			this.corpoRigido.isKinematic = true;
			base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<CapsuleCollider>().enabled = false;
			base.GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
			base.GetComponent<AudioSource>().Play();
			base.transform.GetChild(0).GetComponent<AudioSource>().Stop();
			base.transform.GetChild(1).GetComponent<AudioSource>().Play();
			this.avviaTimer = true;
		}
	}

	// Token: 0x04001546 RID: 5446
	private GameObject supportoOriginario;

	// Token: 0x04001547 RID: 5447
	public float forzaCaduta;

	// Token: 0x04001548 RID: 5448
	public float lunghezzaVitaBomba;

	// Token: 0x04001549 RID: 5449
	private float timerPartenza;

	// Token: 0x0400154A RID: 5450
	private float timerImpatto;

	// Token: 0x0400154B RID: 5451
	private bool avviaTimer;

	// Token: 0x0400154C RID: 5452
	private bool esplosioneAvvenuta;

	// Token: 0x0400154D RID: 5453
	private Vector3 origine;

	// Token: 0x0400154E RID: 5454
	private Vector3 puntoBersaglio;

	// Token: 0x0400154F RID: 5455
	private Vector3 traiettoriaColpo;

	// Token: 0x04001550 RID: 5456
	private bool cancellamentoDaLista;

	// Token: 0x04001551 RID: 5457
	private GameObject terzaCamera;

	// Token: 0x04001552 RID: 5458
	private GameObject infoNeutreTattica;

	// Token: 0x04001553 RID: 5459
	private float timerDanno;

	// Token: 0x04001554 RID: 5460
	private float forzaDiInerzia;

	// Token: 0x04001555 RID: 5461
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x04001556 RID: 5462
	private Rigidbody corpoRigido;

	// Token: 0x04001557 RID: 5463
	private bool gasPartito;

	// Token: 0x04001558 RID: 5464
	private float velocitàVerticale;

	// Token: 0x04001559 RID: 5465
	private int truppaDiOrigineDelSupporto;
}
