using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000AB RID: 171
public class mm40GranataProiettile : MonoBehaviour
{
	// Token: 0x06000647 RID: 1607 RVA: 0x000DE4E8 File Offset: 0x000DC6E8
	private void Start()
	{
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.origine = base.transform.position;
		this.layerPerMira = 4359424;
		if (GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.moltiplicatoreAttaccoInFPS = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().moltiplicatoreFPSBattVeloce;
		}
		else
		{
			this.moltiplicatoreAttaccoInFPS = PlayerPrefs.GetFloat("moltiplicatore danni PP");
		}
		if (base.GetComponent<DatiProiettile>().sparatoInFPS)
		{
			Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			if (Physics.Raycast(ray, out this.puntoDiMira, 9999f, this.layerPerMira))
			{
				this.direzione = (this.puntoDiMira.point - base.transform.position).normalized;
			}
			else
			{
				this.direzione = base.transform.forward;
			}
			base.transform.forward = this.direzione;
		}
		this.corpoRigido = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x000DE630 File Offset: 0x000DC830
	private void Update()
	{
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
		if (!base.GetComponent<DatiProiettile>().sparatoInFPS)
		{
			this.MovimentoIndipendente();
		}
		else
		{
			this.MovimentoFPS();
			this.TiroTeso();
		}
		if (!this.esplosioneAvvenuta)
		{
			this.ultimaPosizione = base.transform.position;
		}
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x000DE70C File Offset: 0x000DC90C
	private void MovimentoIndipendente()
	{
		if (!this.colpoDirezionato)
		{
			this.colpoDirezionato = true;
			base.transform.forward = (base.GetComponent<DatiProiettile>().locazioneTarget - base.transform.position).normalized;
		}
		if (!this.avviaTimer)
		{
			base.transform.position += base.transform.forward * 100f * Time.deltaTime;
		}
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x000DE79C File Offset: 0x000DC99C
	private void MovimentoFPS()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		Vector3 a = Vector3.zero;
		if (this.timerPartenza > 0f && this.timerPartenza < 0.1f)
		{
			if (Physics.Raycast(ray, out this.hitProiettile, 99999f, this.layerPerMira))
			{
				a = this.hitProiettile.point;
			}
			this.traiettoriaColpo = a - this.origine;
		}
		if (!this.avviaTimer && this.timerPartenza < 0.2f)
		{
			this.corpoRigido.AddForce(this.traiettoriaColpo.normalized * this.forzaImpulso * Time.deltaTime, ForceMode.VelocityChange);
		}
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x000DE878 File Offset: 0x000DCA78
	private void TiroTeso()
	{
		float num = Vector3.Distance(base.transform.position, this.origine);
		if (num > this.distanzaTiroTeso)
		{
			base.GetComponent<Rigidbody>().AddForce(Vector3.down * 100f, ForceMode.Acceleration);
		}
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x000DE8C4 File Offset: 0x000DCAC4
	private void Esplosione()
	{
		if (!this.esplosioneAvvenuta && this.timerImpatto > 0f)
		{
			foreach (GameObject current in base.transform.GetChild(0).GetComponent<ColliderAreaEffetto>().ListaAeraEffetto)
			{
				if (current != null && current != this.oggettoColpito)
				{
					if (!base.GetComponent<DatiProiettile>().sparatoInFPS)
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
							List<float> expr_1AA = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
							int truppaDiOrigine;
							int expr_1B8 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
							float num2 = listaDanniAlleati[truppaDiOrigine];
							expr_1AA[expr_1B8] = num2 + num;
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
							int truppaDiOrigine;
							int expr_30F = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
							float num2 = listaDanniAlleati2[truppaDiOrigine];
							expr_301[expr_30F] = num2 + num3;
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
						List<float> expr_4AA = listaDanniAlleati3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
						int truppaDiOrigine;
						int expr_4B8 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
						float num2 = listaDanniAlleati3[truppaDiOrigine];
						expr_4AA[expr_4B8] = num2 + num4;
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
						List<float> expr_661 = listaDanniAlleati4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
						int truppaDiOrigine;
						int expr_66F = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
						float num2 = listaDanniAlleati4[truppaDiOrigine];
						expr_661[expr_66F] = num2 + num5;
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

	// Token: 0x0600064D RID: 1613 RVA: 0x000DEFCC File Offset: 0x000DD1CC
	private void OnCollisionEnter(Collision collisione)
	{
		if (collisione.gameObject.tag == "Ambiente" || collisione.gameObject.tag == "Nemico" || collisione.gameObject.tag == "Nemico Testa" || collisione.gameObject.tag == "Nemico Coll Suppl" || collisione.gameObject.tag == "ObbiettivoTattico")
		{
			this.corpoRigido.isKinematic = true;
			base.GetComponent<CapsuleCollider>().enabled = false;
			base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<ParticleSystem>().Play();
			base.GetComponent<AudioSource>().Play();
			this.avviaTimer = true;
			if (collisione.gameObject.tag == "Nemico")
			{
				this.oggettoColpito = collisione.gameObject;
				if (!base.GetComponent<DatiProiettile>().sparatoInFPS)
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
					List<float> expr_25F = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_26D = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati[truppaDiOrigine];
					expr_25F[expr_26D] = num2 + num;
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
					List<float> expr_423 = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_431 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati2[truppaDiOrigine];
					expr_423[expr_431] = num2 + num3;
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num3;
				}
			}
			else if (collisione.gameObject.tag == "Nemico Testa")
			{
				this.oggettoColpito = collisione.transform.parent.gameObject;
				if (!base.GetComponent<DatiProiettile>().sparatoInFPS)
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
					List<float> expr_624 = listaDanniAlleati3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_632 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati3[truppaDiOrigine];
					expr_624[expr_632] = num2 + num4;
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
					List<float> expr_80C = listaDanniAlleati4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_81A = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati4[truppaDiOrigine];
					expr_80C[expr_81A] = num2 + num5;
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num5;
				}
			}
			else if (collisione.gameObject.tag == "Nemico Coll Suppl")
			{
				this.oggettoColpito = collisione.transform.parent.gameObject;
				if (!base.GetComponent<DatiProiettile>().sparatoInFPS)
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
					List<float> expr_9F0 = listaDanniAlleati5 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_9FE = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati5[truppaDiOrigine];
					expr_9F0[expr_9FE] = num2 + num6;
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
					List<float> expr_BBC = listaDanniAlleati6 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_BCA = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati6[truppaDiOrigine];
					expr_BBC[expr_BCA] = num2 + num7;
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num7;
				}
			}
			else if ((collisione.gameObject.tag == "ObbiettivoTattico" && collisione.gameObject.name == "Avamposto Nemico(Clone)") || collisione.gameObject.name == "Pane per Convoglio(Clone)")
			{
				this.oggettoColpito = collisione.gameObject;
				if (!base.GetComponent<DatiProiettile>().sparatoInFPS)
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
					else if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
					{
						num8 += this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita;
					}
					this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno;
					List<float> listaDanniAlleati7;
					List<float> expr_D87 = listaDanniAlleati7 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_D95 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati7[truppaDiOrigine];
					expr_D87[expr_D95] = num2 + num8;
				}
				else
				{
					this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
					float num9 = 0f;
					if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
					{
						num9 = base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
					}
					else if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
					{
						num9 = this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita;
					}
					this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
					if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * this.moltiplicatoreAttaccoInFPS)
					{
						num9 += base.GetComponent<DatiGeneraliMunizione>().danno * this.moltiplicatoreAttaccoInFPS;
					}
					else if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
					{
						num9 += this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita;
					}
					this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * this.moltiplicatoreAttaccoInFPS;
					List<float> listaDanniAlleati8;
					List<float> expr_F0E = listaDanniAlleati8 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_F1C = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati8[truppaDiOrigine];
					expr_F0E[expr_F1C] = num2 + num9;
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num9;
				}
			}
		}
		if (this.timerPartenza > 0.2f && collisione.gameObject.tag == "collider reale alleato")
		{
			this.corpoRigido.isKinematic = true;
			base.GetComponent<CapsuleCollider>().enabled = false;
			base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<ParticleSystem>().Play();
			base.GetComponent<AudioSource>().Play();
			this.avviaTimer = true;
		}
	}

	// Token: 0x0400178D RID: 6029
	public float distanzaTiroTeso;

	// Token: 0x0400178E RID: 6030
	public float forzaImpulso;

	// Token: 0x0400178F RID: 6031
	private float timerPartenza;

	// Token: 0x04001790 RID: 6032
	private float timerImpatto;

	// Token: 0x04001791 RID: 6033
	private bool avviaTimer;

	// Token: 0x04001792 RID: 6034
	private bool esplosioneAvvenuta;

	// Token: 0x04001793 RID: 6035
	private Vector3 origine;

	// Token: 0x04001794 RID: 6036
	private Vector3 puntoBersaglio;

	// Token: 0x04001795 RID: 6037
	private GameObject oggettoColpito;

	// Token: 0x04001796 RID: 6038
	private Vector3 traiettoriaColpo;

	// Token: 0x04001797 RID: 6039
	private RaycastHit hitProiettile;

	// Token: 0x04001798 RID: 6040
	private Vector3 ultimaPosizione;

	// Token: 0x04001799 RID: 6041
	private GameObject IANemico;

	// Token: 0x0400179A RID: 6042
	private GameObject infoNeutreTattica;

	// Token: 0x0400179B RID: 6043
	private GameObject terzaCamera;

	// Token: 0x0400179C RID: 6044
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x0400179D RID: 6045
	private Vector3 direzione;

	// Token: 0x0400179E RID: 6046
	private RaycastHit puntoDiMira;

	// Token: 0x0400179F RID: 6047
	private int layerPerMira;

	// Token: 0x040017A0 RID: 6048
	private Rigidbody corpoRigido;

	// Token: 0x040017A1 RID: 6049
	private bool colpoDirezionato;
}
