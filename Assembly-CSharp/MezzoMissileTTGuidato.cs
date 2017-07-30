using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000097 RID: 151
public class MezzoMissileTTGuidato : MonoBehaviour
{
	// Token: 0x060005C5 RID: 1477 RVA: 0x000C4DD4 File Offset: 0x000C2FD4
	private void Start()
	{
		this.layerPerMira = 4359424;
		this.supportoOriginario = base.transform.parent.gameObject;
		this.truppaDiOrigineDelSupporto = this.supportoOriginario.transform.parent.parent.parent.GetComponent<PresenzaAlleato>().tipoTruppa;
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

	// Token: 0x060005C6 RID: 1478 RVA: 0x000C4E90 File Offset: 0x000C3090
	private void Update()
	{
		if (this.supportoOriginario)
		{
			if (base.GetComponent<DatiGeneraliMunizione>().ordignoLocaleAttivo)
			{
				if (!base.GetComponent<Rigidbody>())
				{
					base.gameObject.AddComponent<Rigidbody>();
					base.GetComponent<Rigidbody>().useGravity = false;
					base.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
				}
				this.timerPartenza += Time.deltaTime;
				if (this.timerPartenza > 0f && this.timerPartenza < 0.1f && this.timerImpatto == 0f)
				{
					base.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
				}
				if (this.timerPartenza < 0.2f)
				{
					this.lanciatoDaGiocatore = base.transform.parent.GetComponent<DatiOrdignoEsterno>().lanciatoInFPS;
					this.target = base.transform.parent.GetComponent<DatiOrdignoEsterno>().bersaglio;
				}
				if (!this.audioViaggioAttivo)
				{
					base.GetComponent<AudioSource>().Play();
					this.audioViaggioAttivo = true;
				}
				if (this.timerPartenza > 1f)
				{
					base.GetComponent<CapsuleCollider>().enabled = true;
					base.transform.GetChild(0).GetComponent<SphereCollider>().enabled = true;
					base.transform.GetChild(0).GetComponent<SphereCollider>().radius = base.GetComponent<DatiGeneraliMunizione>().raggioEffetto;
				}
				if (!this.lanciatoDaGiocatore)
				{
					if (this.timerPartenza > 0f && this.timerPartenza < 0.4f)
					{
						base.transform.Translate(-this.supportoOriginario.transform.up * 3f * Time.deltaTime);
					}
					if (this.timerPartenza > 0.3f)
					{
						if (!this.esplosioneAvvenuta)
						{
							this.NavigazioneAutomatica();
						}
						if (this.target)
						{
							float num = Vector3.Distance(base.transform.position, this.target.transform.position);
							if (num < this.raggioInnesco && !this.esplosioneAvvenuta)
							{
								base.GetComponent<Rigidbody>().isKinematic = true;
								base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
								base.GetComponent<CapsuleCollider>().enabled = false;
								base.GetComponent<ParticleSystem>().Play();
								base.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
								base.transform.GetChild(1).GetComponent<ParticleSystem>().Clear();
								base.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
								base.GetComponent<AudioSource>().Stop();
								base.transform.GetChild(0).GetComponent<AudioSource>().Play();
								this.avviaTimer = true;
								base.transform.position = this.target.transform.position;
							}
						}
					}
				}
				else if (this.timerPartenza > 0.2f && !this.esplosioneAvvenuta)
				{
					this.NavigazioneFPS();
				}
			}
			if (this.timerPartenza > 0.2f && this.timerPartenza < 0.3f && !this.cancellamentoDaLista)
			{
				base.transform.parent = null;
				int index = this.supportoOriginario.GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche.IndexOf(base.gameObject);
				this.supportoOriginario.GetComponent<DatiOrdignoEsterno>().ListaMunizioniFisiche[index] = null;
				this.cancellamentoDaLista = true;
			}
			if (this.avviaTimer)
			{
				this.timerImpatto += Time.deltaTime;
			}
			if (!this.esplosioneAvvenuta)
			{
				this.ultimaPosizione = base.transform.position;
			}
			if (this.timerImpatto > 0f)
			{
				this.Esplosione();
			}
		}
		else if (!base.GetComponent<DatiGeneraliMunizione>().ordignoLocaleAttivo)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x000C5290 File Offset: 0x000C3490
	private void NavigazioneAutomatica()
	{
		if (this.timerPartenza > 0.4f && this.timerPartenza < 0.9f)
		{
			base.transform.forward = this.supportoOriginario.transform.forward;
		}
		if (this.timerPartenza > 0.91f && this.target)
		{
			Vector3 forward = this.target.transform.position - base.transform.position;
			Quaternion to = Quaternion.LookRotation(forward);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.velocitàRotazione * Time.deltaTime);
		}
		base.transform.position += base.transform.forward * this.velocitàMissile * Time.deltaTime;
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x000C5380 File Offset: 0x000C3580
	private void NavigazioneFPS()
	{
		Vector3 forward = Vector3.zero;
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(ray, out this.puntoDiMira, 9999f, this.layerPerMira))
		{
			forward = (this.puntoDiMira.point - base.transform.position).normalized;
		}
		else
		{
			forward = base.transform.forward;
		}
		base.transform.forward = forward;
		base.transform.position += base.transform.forward * this.velocitàMissile * Time.deltaTime;
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x000C544C File Offset: 0x000C364C
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

	// Token: 0x060005CA RID: 1482 RVA: 0x000C5B48 File Offset: 0x000C3D48
	private void OnCollisionEnter(Collision collisione)
	{
		if (collisione.gameObject.tag == "Ambiente" || collisione.gameObject.tag == "Nemico" || collisione.gameObject.tag == "Nemico Testa" || collisione.gameObject.tag == "Nemico Coll Suppl" || collisione.gameObject.tag == "ObbiettivoTattico")
		{
			base.GetComponent<Rigidbody>().isKinematic = true;
			base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<CapsuleCollider>().enabled = false;
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Clear();
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
			base.GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
			base.GetComponent<AudioSource>().Stop();
			base.transform.GetChild(0).GetComponent<AudioSource>().Play();
			this.avviaTimer = true;
			base.transform.position = this.ultimaPosizione;
			base.transform.eulerAngles = Vector3.zero;
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
					List<float> expr_2DD = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int index;
					int expr_2E6 = index = this.truppaDiOrigineDelSupporto;
					float num2 = listaDanniAlleati[index];
					expr_2DD[expr_2E6] = num2 + num;
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
					List<float> expr_49C = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int index;
					int expr_4A5 = index = this.truppaDiOrigineDelSupporto;
					float num2 = listaDanniAlleati2[index];
					expr_49C[expr_4A5] = num2 + num3;
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
					List<float> expr_69D = listaDanniAlleati3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int index;
					int expr_6A6 = index = this.truppaDiOrigineDelSupporto;
					float num2 = listaDanniAlleati3[index];
					expr_69D[expr_6A6] = num2 + num4;
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
					List<float> expr_880 = listaDanniAlleati4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int index;
					int expr_889 = index = this.truppaDiOrigineDelSupporto;
					float num2 = listaDanniAlleati4[index];
					expr_880[expr_889] = num2 + num5;
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
					List<float> expr_A64 = listaDanniAlleati5 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int index;
					int expr_A6D = index = this.truppaDiOrigineDelSupporto;
					float num2 = listaDanniAlleati5[index];
					expr_A64[expr_A6D] = num2 + num6;
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
					List<float> expr_C2B = listaDanniAlleati6 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int index;
					int expr_C34 = index = this.truppaDiOrigineDelSupporto;
					float num2 = listaDanniAlleati6[index];
					expr_C2B[expr_C34] = num2 + num7;
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
					List<float> expr_DDC = listaDanniAlleati7 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int index;
					int expr_DE5 = index = this.truppaDiOrigineDelSupporto;
					float num2 = listaDanniAlleati7[index];
					expr_DDC[expr_DE5] = num2 + num8;
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
					List<float> expr_F44 = listaDanniAlleati8 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int index;
					int expr_F4D = index = this.truppaDiOrigineDelSupporto;
					float num2 = listaDanniAlleati8[index];
					expr_F44[expr_F4D] = num2 + num9;
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num9;
				}
			}
		}
		if (this.timerPartenza > 0.2f && collisione.gameObject.tag == "collider reale alleato")
		{
			base.GetComponent<Rigidbody>().isKinematic = true;
			base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<CapsuleCollider>().enabled = false;
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Clear();
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
			base.GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
			base.GetComponent<AudioSource>().Stop();
			base.transform.GetChild(0).GetComponent<AudioSource>().Play();
			this.avviaTimer = true;
			base.transform.position = this.ultimaPosizione;
			base.transform.eulerAngles = Vector3.zero;
		}
	}

	// Token: 0x040015AE RID: 5550
	public float velocitàMissile;

	// Token: 0x040015AF RID: 5551
	public float velocitàRotazione;

	// Token: 0x040015B0 RID: 5552
	public float raggioInnesco;

	// Token: 0x040015B1 RID: 5553
	private float timerPartenza;

	// Token: 0x040015B2 RID: 5554
	private GameObject target;

	// Token: 0x040015B3 RID: 5555
	private bool esplosioneAvvenuta;

	// Token: 0x040015B4 RID: 5556
	private Vector3 posizioneEsplosione;

	// Token: 0x040015B5 RID: 5557
	private float timerImpatto;

	// Token: 0x040015B6 RID: 5558
	private GameObject oggettoOrigine;

	// Token: 0x040015B7 RID: 5559
	private bool audioViaggioAttivo;

	// Token: 0x040015B8 RID: 5560
	private bool audioPartenzaAttivo;

	// Token: 0x040015B9 RID: 5561
	private bool lanciatoDaGiocatore;

	// Token: 0x040015BA RID: 5562
	private GameObject terzaCamera;

	// Token: 0x040015BB RID: 5563
	private GameObject infoNeutreTattica;

	// Token: 0x040015BC RID: 5564
	private Vector3 ultimaPosizione;

	// Token: 0x040015BD RID: 5565
	private bool avviaTimer;

	// Token: 0x040015BE RID: 5566
	private GameObject oggettoColpito;

	// Token: 0x040015BF RID: 5567
	private RaycastHit puntoDiMira;

	// Token: 0x040015C0 RID: 5568
	private int layerPerMira;

	// Token: 0x040015C1 RID: 5569
	private GameObject supportoOriginario;

	// Token: 0x040015C2 RID: 5570
	private bool cancellamentoDaLista;

	// Token: 0x040015C3 RID: 5571
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x040015C4 RID: 5572
	private int truppaDiOrigineDelSupporto;
}
