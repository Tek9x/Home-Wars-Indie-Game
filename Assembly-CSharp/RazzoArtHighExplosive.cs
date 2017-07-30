using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009B RID: 155
public class RazzoArtHighExplosive : MonoBehaviour
{
	// Token: 0x060005DE RID: 1502 RVA: 0x000C9DC8 File Offset: 0x000C7FC8
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
		base.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x000C9E9C File Offset: 0x000C809C
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
				else if (this.timerPartenza > 0.2f)
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

	// Token: 0x060005E0 RID: 1504 RVA: 0x000CA1E4 File Offset: 0x000C83E4
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

	// Token: 0x060005E1 RID: 1505 RVA: 0x000CA31C File Offset: 0x000C851C
	private void Esplosione()
	{
		if (!this.esplosioneAvvenuta && this.timerImpatto > 0.1f)
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
		if (this.timerImpatto > 3f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x000CAA18 File Offset: 0x000C8C18
	private void OnCollisionEnter(Collision collisione)
	{
		if (collisione.gameObject.tag == "Ambiente" || collisione.gameObject.tag == "Nemico" || collisione.gameObject.tag == "Nemico Testa" || collisione.gameObject.tag == "Nemico Coll Suppl" || collisione.gameObject.tag == "ObbiettivoTattico")
		{
			this.corpoRigido.isKinematic = true;
			base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<CapsuleCollider>().enabled = false;
			base.GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Clear();
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
			base.transform.GetChild(1).GetComponent<AudioSource>().Stop();
			base.GetComponent<AudioSource>().Play();
			this.avviaTimer = true;
			if (collisione.gameObject.tag == "Nemico")
			{
				this.oggettoColpito = collisione.gameObject;
				if (!this.supportoOriginario.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS)
				{
					float num = 0f;
					if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione)
					{
						num = base.GetComponent<DatiGeneraliMunizione>().penetrazione;
					}
					else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
					{
						num = this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
					}
					this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione;
					if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura))
					{
						num += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura);
					}
					else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
					{
						num += this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
					}
					this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura);
					List<float> listaDanniAlleati;
					List<float> expr_2BC = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int index;
					int expr_2C5 = index = this.truppaDiOrigineDelSupporto;
					float num2 = listaDanniAlleati[index];
					expr_2BC[expr_2C5] = num2 + num;
				}
				else
				{
					this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
					float num3 = 0f;
					if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
					{
						num3 = base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
					}
					else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
					{
						num3 = this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
					}
					this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
					if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS)
					{
						num3 += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
					}
					else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
					{
						num3 += this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
					}
					this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
					List<float> listaDanniAlleati2;
					List<float> expr_47B = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int index;
					int expr_484 = index = this.truppaDiOrigineDelSupporto;
					float num2 = listaDanniAlleati2[index];
					expr_47B[expr_484] = num2 + num3;
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num3;
				}
			}
			else if (collisione.gameObject.tag == "Nemico Testa")
			{
				this.oggettoColpito = collisione.transform.parent.gameObject;
				if (!this.supportoOriginario.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS)
				{
					float num4 = 0f;
					if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f)
					{
						num4 = base.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f;
					}
					else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
					{
						num4 = this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
					}
					this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f;
					if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura) * 2f)
					{
						num4 += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura) * 2f;
					}
					else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
					{
						num4 += this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
					}
					this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura) * 2f;
					List<float> listaDanniAlleati3;
					List<float> expr_67C = listaDanniAlleati3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int index;
					int expr_685 = index = this.truppaDiOrigineDelSupporto;
					float num2 = listaDanniAlleati3[index];
					expr_67C[expr_685] = num2 + num4;
				}
				else
				{
					this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
					float num5 = 0f;
					if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS * 2f)
					{
						num5 = base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS * 2f;
					}
					else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
					{
						num5 = this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
					}
					this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS * 2f;
					if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS * 2f)
					{
						num5 += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS * 2f;
					}
					else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
					{
						num5 += this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
					}
					this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS * 2f;
					List<float> listaDanniAlleati4;
					List<float> expr_85F = listaDanniAlleati4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int index;
					int expr_868 = index = this.truppaDiOrigineDelSupporto;
					float num2 = listaDanniAlleati4[index];
					expr_85F[expr_868] = num2 + num5;
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num5;
				}
			}
			else if (collisione.gameObject.tag == "Nemico Coll Suppl")
			{
				this.oggettoColpito = collisione.transform.parent.gameObject;
				if (!this.supportoOriginario.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS)
				{
					float num6 = 0f;
					if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione)
					{
						num6 = base.GetComponent<DatiGeneraliMunizione>().penetrazione;
					}
					else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
					{
						num6 = this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
					}
					this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione;
					if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura))
					{
						num6 += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura);
					}
					else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
					{
						num6 += this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
					}
					this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura);
					List<float> listaDanniAlleati5;
					List<float> expr_A43 = listaDanniAlleati5 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int index;
					int expr_A4C = index = this.truppaDiOrigineDelSupporto;
					float num2 = listaDanniAlleati5[index];
					expr_A43[expr_A4C] = num2 + num6;
				}
				else
				{
					this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
					float num7 = 0f;
					if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
					{
						num7 = base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
					}
					else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
					{
						num7 = this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
					}
					this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
					if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS)
					{
						num7 += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
					}
					else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
					{
						num7 += this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
					}
					this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
					List<float> listaDanniAlleati6;
					List<float> expr_C0A = listaDanniAlleati6 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int index;
					int expr_C13 = index = this.truppaDiOrigineDelSupporto;
					float num2 = listaDanniAlleati6[index];
					expr_C0A[expr_C13] = num2 + num7;
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num7;
				}
			}
			else if ((collisione.gameObject.tag == "ObbiettivoTattico" && collisione.gameObject.name == "Avamposto Nemico(Clone)") || collisione.gameObject.name == "Pane per Convoglio(Clone)")
			{
				this.oggettoColpito = collisione.gameObject;
				if (!this.supportoOriginario.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS)
				{
					float num8 = 0f;
					if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione)
					{
						num8 = base.GetComponent<DatiGeneraliMunizione>().penetrazione;
					}
					else if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
					{
						num8 = this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita;
					}
					this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione;
					if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().danno)
					{
						num8 += base.GetComponent<DatiGeneraliMunizione>().danno;
					}
					else
					{
						num8 += this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita;
					}
					this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno;
					List<float> listaDanniAlleati7;
					List<float> expr_DBB = listaDanniAlleati7 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int index;
					int expr_DC4 = index = this.truppaDiOrigineDelSupporto;
					float num2 = listaDanniAlleati7[index];
					expr_DBB[expr_DC4] = num2 + num8;
				}
				else if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
				{
					this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
					float num9;
					if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
					{
						num9 = base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
					}
					else
					{
						num9 = this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita;
					}
					this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
					if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * this.moltiplicatoreAttaccoInFPS)
					{
						num9 += base.GetComponent<DatiGeneraliMunizione>().danno * this.moltiplicatoreAttaccoInFPS;
					}
					else
					{
						num9 += this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita;
					}
					this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * this.moltiplicatoreAttaccoInFPS;
					List<float> listaDanniAlleati8;
					List<float> expr_F23 = listaDanniAlleati8 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int index;
					int expr_F2C = index = this.truppaDiOrigineDelSupporto;
					float num2 = listaDanniAlleati8[index];
					expr_F23[expr_F2C] = num2 + num9;
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num9;
				}
			}
		}
		if (this.timerPartenza > 0.2f && collisione.gameObject.tag == "collider reale alleato")
		{
			this.corpoRigido.isKinematic = true;
			base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<CapsuleCollider>().enabled = false;
			base.GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Clear();
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
			base.transform.GetChild(1).GetComponent<AudioSource>().Stop();
			base.GetComponent<AudioSource>().Play();
			this.avviaTimer = true;
		}
	}

	// Token: 0x0400160E RID: 5646
	private GameObject supportoOriginario;

	// Token: 0x0400160F RID: 5647
	public float forzaImpulso;

	// Token: 0x04001610 RID: 5648
	private float timerPartenza;

	// Token: 0x04001611 RID: 5649
	private float timerImpatto;

	// Token: 0x04001612 RID: 5650
	private bool avviaTimer;

	// Token: 0x04001613 RID: 5651
	private bool esplosioneAvvenuta;

	// Token: 0x04001614 RID: 5652
	private Vector3 origine;

	// Token: 0x04001615 RID: 5653
	private Vector3 puntoBersaglio;

	// Token: 0x04001616 RID: 5654
	private Vector3 traiettoriaColpo;

	// Token: 0x04001617 RID: 5655
	private bool cancellamentoDaLista;

	// Token: 0x04001618 RID: 5656
	private GameObject terzaCamera;

	// Token: 0x04001619 RID: 5657
	private GameObject infoNeutreTattica;

	// Token: 0x0400161A RID: 5658
	private GameObject oggettoColpito;

	// Token: 0x0400161B RID: 5659
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x0400161C RID: 5660
	private Vector3 direzione;

	// Token: 0x0400161D RID: 5661
	private RaycastHit puntoDiMira;

	// Token: 0x0400161E RID: 5662
	private Rigidbody corpoRigido;

	// Token: 0x0400161F RID: 5663
	public float velocitàAutoRotazione;

	// Token: 0x04001620 RID: 5664
	private GameObject IANemico;

	// Token: 0x04001621 RID: 5665
	private RaycastHit hitProiettile;

	// Token: 0x04001622 RID: 5666
	private Vector3 puntoDiMezzo;

	// Token: 0x04001623 RID: 5667
	private Vector3 vertice;

	// Token: 0x04001624 RID: 5668
	private Vector3 direzioneSparo;

	// Token: 0x04001625 RID: 5669
	private Vector3 posizioneMassima;

	// Token: 0x04001626 RID: 5670
	private bool verticeRaggiunto;

	// Token: 0x04001627 RID: 5671
	private Vector3 dirDiCaduta;

	// Token: 0x04001628 RID: 5672
	private float velocitàAlPicco;

	// Token: 0x04001629 RID: 5673
	private Vector3 locazioneTarget;

	// Token: 0x0400162A RID: 5674
	private float distanzaDiMetà;

	// Token: 0x0400162B RID: 5675
	private float timerIncrementoVelocità;

	// Token: 0x0400162C RID: 5676
	private float incrementoVelocità;

	// Token: 0x0400162D RID: 5677
	private int truppaDiOrigineDelSupporto;
}
