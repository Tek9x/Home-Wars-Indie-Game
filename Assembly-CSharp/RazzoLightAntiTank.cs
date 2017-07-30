using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009E RID: 158
public class RazzoLightAntiTank : MonoBehaviour
{
	// Token: 0x060005F0 RID: 1520 RVA: 0x000CD654 File Offset: 0x000CB854
	private void Start()
	{
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.origine = base.transform.position;
		base.GetComponent<ParticleSystem>().Play();
		base.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
		base.GetComponent<AudioSource>().Play();
		if (GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.moltiplicatoreAttaccoInFPS = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().moltiplicatoreFPSBattVeloce;
		}
		else
		{
			this.moltiplicatoreAttaccoInFPS = PlayerPrefs.GetFloat("moltiplicatore danni PP");
		}
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x000CD710 File Offset: 0x000CB910
	private void Update()
	{
		if (!base.GetComponent<Rigidbody>())
		{
			base.gameObject.AddComponent<Rigidbody>();
			this.corpoRigido = base.GetComponent<Rigidbody>();
			this.corpoRigido.useGravity = false;
			this.corpoRigido.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		}
		this.Movimento();
		this.TiroTeso();
		this.Esplosione();
		this.timerPartenza += Time.deltaTime;
		if (this.avviaTimer)
		{
			this.timerImpatto += Time.deltaTime;
		}
		if (this.timerPartenza > 0.15f)
		{
			base.GetComponent<CapsuleCollider>().enabled = true;
			base.transform.GetChild(0).GetComponent<SphereCollider>().enabled = true;
			base.transform.GetChild(0).GetComponent<SphereCollider>().radius = base.GetComponent<DatiGeneraliMunizione>().raggioEffetto;
		}
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x000CD7F4 File Offset: 0x000CB9F4
	private void Movimento()
	{
		this.corpoRigido.AddForce(base.transform.forward * this.forzaImpulso, ForceMode.Impulse);
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x000CD824 File Offset: 0x000CBA24
	private void TiroTeso()
	{
		float num = Vector3.Distance(base.transform.position, this.origine);
		if (num > base.GetComponent<DatiGeneraliMunizione>().portataMassima)
		{
			this.corpoRigido.useGravity = true;
			this.corpoRigido.AddForce(Vector3.down * 20f, ForceMode.Force);
		}
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x000CD880 File Offset: 0x000CBA80
	private void Esplosione()
	{
		if (!this.esplosioneAvvenuta)
		{
			if (!this.esplosioneDaInnesco && !base.GetComponent<DatiOrdignoInterno>().lanciatoInFPS)
			{
				GameObject bersaglio = base.GetComponent<DatiOrdignoInterno>().bersaglio;
				float num = Vector3.Distance(base.transform.position, bersaglio.GetComponent<PresenzaNemico>().centroInsetto);
				if (num < this.raggioInnesco)
				{
					this.corpoRigido.isKinematic = true;
					base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
					base.GetComponent<ParticleSystem>().Stop();
					base.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
					base.transform.GetChild(1).GetComponent<AudioSource>().Play();
					this.avviaTimer = true;
					if (bersaglio.tag == "Nemico")
					{
						if (!base.GetComponent<DatiOrdignoInterno>().lanciatoInFPS)
						{
							float num2 = bersaglio.GetComponent<PresenzaNemico>().armatura - base.GetComponent<DatiGeneraliMunizione>().penetrazione;
							if (num2 <= 0f)
							{
								bersaglio.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno;
							}
							else
							{
								float num3 = num2 - base.GetComponent<DatiGeneraliMunizione>().danno;
								if (num3 < 0f)
								{
									bersaglio.GetComponent<PresenzaNemico>().vita += num3;
								}
							}
						}
						this.oggettoColpito = bersaglio;
						this.esplosioneDaInnesco = true;
					}
				}
			}
			if (this.timerImpatto > 0f)
			{
				foreach (GameObject current in base.transform.GetChild(0).GetComponent<ColliderAreaEffetto>().ListaAeraEffetto)
				{
					if (current != null && current != this.oggettoColpito)
					{
						if (!base.GetComponent<DatiOrdignoInterno>().lanciatoInFPS)
						{
							if (current.tag == "Nemico")
							{
								float num4 = 0f;
								if (current.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione)
								{
									num4 = base.GetComponent<DatiGeneraliMunizione>().penetrazione;
								}
								else if (current.GetComponent<PresenzaNemico>().vita > 0f)
								{
									num4 = current.GetComponent<PresenzaNemico>().vita;
								}
								current.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione;
								if (current.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura))
								{
									num4 += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura);
								}
								else if (current.GetComponent<PresenzaNemico>().vita > 0f)
								{
									num4 += current.GetComponent<PresenzaNemico>().vita;
								}
								current.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura);
								List<float> listaDanniAlleati;
								List<float> expr_315 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
								int truppaDiOrigine;
								int expr_323 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
								float num5 = listaDanniAlleati[truppaDiOrigine];
								expr_315[expr_323] = num5 + num4;
							}
							else if (current.tag == "ObbiettivoTattico" && (current.name == "Avamposto Nemico(Clone)" || current.name == "Pane per Convoglio(Clone)"))
							{
								float num6 = 0f;
								if (current.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione)
								{
									num6 = base.GetComponent<DatiGeneraliMunizione>().penetrazione;
								}
								else if (current.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
								{
									num6 = current.GetComponent<ObbiettivoTatticoScript>().vita;
								}
								current.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione;
								if (current.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().danno)
								{
									num6 += base.GetComponent<DatiGeneraliMunizione>().danno;
								}
								else if (current.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
								{
									num6 += current.GetComponent<ObbiettivoTatticoScript>().vita;
								}
								current.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno;
								List<float> listaDanniAlleati2;
								List<float> expr_47F = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
								int truppaDiOrigine;
								int expr_48D = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
								float num5 = listaDanniAlleati2[truppaDiOrigine];
								expr_47F[expr_48D] = num5 + num6;
							}
						}
						else if (current.tag == "Nemico")
						{
							this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
							float num7 = 0f;
							if (current.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
							{
								num7 = base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
							}
							else if (current.GetComponent<PresenzaNemico>().vita > 0f)
							{
								num7 = current.GetComponent<PresenzaNemico>().vita;
							}
							current.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
							if (current.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS)
							{
								num7 += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
							}
							else if (current.GetComponent<PresenzaNemico>().vita > 0f)
							{
								num7 += current.GetComponent<PresenzaNemico>().vita;
							}
							current.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
							List<float> listaDanniAlleati3;
							List<float> expr_635 = listaDanniAlleati3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
							int truppaDiOrigine;
							int expr_643 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
							float num5 = listaDanniAlleati3[truppaDiOrigine];
							expr_635[expr_643] = num5 + num7;
							this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num7;
						}
						else if (current.tag == "ObbiettivoTattico" && (current.name == "Avamposto Nemico(Clone)" || current.name == "Pane per Convoglio(Clone)"))
						{
							this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
							float num8 = 0f;
							if (current.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
							{
								num8 = base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
							}
							else if (current.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
							{
								num8 = current.GetComponent<ObbiettivoTatticoScript>().vita;
							}
							current.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
							if (current.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * this.moltiplicatoreAttaccoInFPS)
							{
								num8 += base.GetComponent<DatiGeneraliMunizione>().danno * this.moltiplicatoreAttaccoInFPS;
							}
							else if (current.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
							{
								num8 += current.GetComponent<ObbiettivoTatticoScript>().vita;
							}
							current.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * this.moltiplicatoreAttaccoInFPS;
							List<float> listaDanniAlleati4;
							List<float> expr_7F7 = listaDanniAlleati4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
							int truppaDiOrigine;
							int expr_805 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
							float num5 = listaDanniAlleati4[truppaDiOrigine];
							expr_7F7[expr_805] = num5 + num8;
							this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num8;
						}
					}
				}
				this.esplosioneAvvenuta = true;
			}
		}
		if (this.timerImpatto > 1f && base.GetComponent<MeshRenderer>())
		{
			base.GetComponent<MeshRenderer>().enabled = false;
		}
		if (this.timerImpatto > 3f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x000CE14C File Offset: 0x000CC34C
	private void OnCollisionEnter(Collision collisione)
	{
		if ((!this.avviaTimer && collisione.gameObject.tag == "Ambiente") || collisione.gameObject.tag == "Nemico" || collisione.gameObject.tag == "Nemico Testa" || collisione.gameObject.tag == "Nemico Coll Suppl" || collisione.gameObject.tag == "ObbiettivoTattico")
		{
			this.corpoRigido.isKinematic = true;
			base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<ParticleSystem>().Stop();
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(1).GetComponent<AudioSource>().Play();
			this.avviaTimer = true;
			if (collisione.gameObject.tag == "Nemico")
			{
				this.oggettoColpito = collisione.gameObject;
				if (!base.GetComponent<DatiOrdignoInterno>().lanciatoInFPS)
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
					List<float> expr_27F = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_28D = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati[truppaDiOrigine];
					expr_27F[expr_28D] = num2 + num;
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
					List<float> expr_443 = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_451 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati2[truppaDiOrigine];
					expr_443[expr_451] = num2 + num3;
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num3;
				}
			}
			else if (collisione.gameObject.tag == "Nemico Testa")
			{
				this.oggettoColpito = collisione.transform.parent.gameObject;
				if (!base.GetComponent<DatiOrdignoInterno>().lanciatoInFPS)
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
					List<float> expr_644 = listaDanniAlleati3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_652 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati3[truppaDiOrigine];
					expr_644[expr_652] = num2 + num4;
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
					List<float> expr_82C = listaDanniAlleati4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_83A = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati4[truppaDiOrigine];
					expr_82C[expr_83A] = num2 + num5;
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num5;
				}
			}
			else if (collisione.gameObject.tag == "Nemico Coll Suppl")
			{
				this.oggettoColpito = collisione.transform.parent.gameObject;
				if (!base.GetComponent<DatiOrdignoInterno>().lanciatoInFPS)
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
					List<float> expr_A10 = listaDanniAlleati5 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_A1E = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati5[truppaDiOrigine];
					expr_A10[expr_A1E] = num2 + num6;
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
					List<float> expr_BDC = listaDanniAlleati6 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_BEA = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati6[truppaDiOrigine];
					expr_BDC[expr_BEA] = num2 + num7;
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num7;
				}
			}
			else if ((collisione.gameObject.tag == "ObbiettivoTattico" && collisione.gameObject.name == "Avamposto Nemico(Clone)") || collisione.gameObject.name == "Pane per Convoglio(Clone)")
			{
				this.oggettoColpito = collisione.gameObject;
				if (!base.GetComponent<DatiOrdignoInterno>().lanciatoInFPS)
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
					List<float> expr_D8D = listaDanniAlleati7 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_D9B = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati7[truppaDiOrigine];
					expr_D8D[expr_D9B] = num2 + num8;
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
					List<float> expr_EFA = listaDanniAlleati8 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_F08 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati8[truppaDiOrigine];
					expr_EFA[expr_F08] = num2 + num9;
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num9;
				}
			}
		}
		if (this.timerPartenza > 0.06f && collisione.gameObject.tag == "collider reale alleato")
		{
			this.corpoRigido.isKinematic = true;
			base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<ParticleSystem>().Stop();
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(1).GetComponent<AudioSource>().Play();
			this.avviaTimer = true;
		}
	}

	// Token: 0x04001675 RID: 5749
	private GameObject infoNeutreTattica;

	// Token: 0x04001676 RID: 5750
	private GameObject terzaCamera;

	// Token: 0x04001677 RID: 5751
	public float forzaImpulso;

	// Token: 0x04001678 RID: 5752
	public float raggioInnesco;

	// Token: 0x04001679 RID: 5753
	private float timerPartenza;

	// Token: 0x0400167A RID: 5754
	private float timerImpatto;

	// Token: 0x0400167B RID: 5755
	private bool avviaTimer;

	// Token: 0x0400167C RID: 5756
	private GameObject IANemico;

	// Token: 0x0400167D RID: 5757
	private bool esplosioneAvvenuta;

	// Token: 0x0400167E RID: 5758
	private Vector3 origine;

	// Token: 0x0400167F RID: 5759
	private Vector3 puntoBersaglio;

	// Token: 0x04001680 RID: 5760
	private GameObject oggettoColpito;

	// Token: 0x04001681 RID: 5761
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x04001682 RID: 5762
	private bool esplosioneDaInnesco;

	// Token: 0x04001683 RID: 5763
	private Rigidbody corpoRigido;
}
