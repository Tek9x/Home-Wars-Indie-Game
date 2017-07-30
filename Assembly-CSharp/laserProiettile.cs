using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A0 RID: 160
public class laserProiettile : MonoBehaviour
{
	// Token: 0x060005F9 RID: 1529 RVA: 0x000CF140 File Offset: 0x000CD340
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
		base.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
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
		else
		{
			this.target = base.GetComponent<DatiProiettile>().target;
		}
		this.corpoRigido = base.GetComponent<Rigidbody>();
		this.locazioneTarget = base.GetComponent<DatiProiettile>().locazioneTarget;
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x000CF2C4 File Offset: 0x000CD4C4
	private void Update()
	{
		this.Esplosione();
		this.timerPartenza += Time.deltaTime;
		if (this.avviaTimer)
		{
			this.timerImpatto += Time.deltaTime;
		}
		if (this.timerPartenza > 0.1f)
		{
			base.GetComponent<CapsuleCollider>().enabled = true;
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

	// Token: 0x060005FB RID: 1531 RVA: 0x000CF368 File Offset: 0x000CD568
	private void MovimentoIndipendente()
	{
		if (!this.colpoDirezionato)
		{
			this.colpoDirezionato = true;
			base.transform.forward = (base.GetComponent<DatiProiettile>().locazioneTarget - base.transform.position).normalized;
		}
		if (!this.avviaTimer)
		{
			base.transform.position += base.transform.forward * 400f * Time.deltaTime;
		}
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x000CF3F8 File Offset: 0x000CD5F8
	private void MovimentoFPS()
	{
		Ray ray = this.terzaCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		Vector3 a = Vector3.zero;
		if (this.timerPartenza > 0f && this.timerPartenza < 0.05f)
		{
			if (Physics.Raycast(ray, out this.hitProiettile, 99999f, this.layerPerMira))
			{
				a = this.hitProiettile.point;
			}
			this.traiettoriaColpo = a - this.origine;
		}
		if (!this.avviaTimer)
		{
			this.corpoRigido.AddForce(this.traiettoriaColpo.normalized * this.forzaImpulso * Time.deltaTime, ForceMode.VelocityChange);
		}
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x000CF4C4 File Offset: 0x000CD6C4
	private void TiroTeso()
	{
		float num = Vector3.Distance(base.transform.position, this.origine);
		if (num > this.distanzaTiroTeso)
		{
			this.corpoRigido.AddForce(Vector3.down * 25f, ForceMode.Acceleration);
		}
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x000CF510 File Offset: 0x000CD710
	private void Esplosione()
	{
		if (!this.esplosioneAvvenuta && this.timerImpatto > 0f)
		{
			this.esplosioneAvvenuta = true;
		}
		if (this.timerImpatto > 0f && this.timerImpatto < 0.1f)
		{
			base.GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(0).GetComponent<ParticleSystem>().Clear();
			base.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
		}
		if (this.timerImpatto > 2f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x000CF5B4 File Offset: 0x000CD7B4
	private void OnCollisionEnter(Collision collisione)
	{
		if (collisione.gameObject.tag == "Ambiente" || collisione.gameObject.tag == "Nemico" || collisione.gameObject.tag == "Nemico Testa" || collisione.gameObject.tag == "Nemico Coll Suppl" || collisione.gameObject.tag == "ObbiettivoTattico")
		{
			this.corpoRigido.isKinematic = true;
			base.GetComponent<CapsuleCollider>().enabled = false;
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
					List<float> expr_232 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_240 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati[truppaDiOrigine];
					expr_232[expr_240] = num2 + num;
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
					List<float> expr_3F6 = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_404 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati2[truppaDiOrigine];
					expr_3F6[expr_404] = num2 + num3;
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
					List<float> expr_5F7 = listaDanniAlleati3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_605 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati3[truppaDiOrigine];
					expr_5F7[expr_605] = num2 + num4;
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
					List<float> expr_7DF = listaDanniAlleati4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_7ED = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati4[truppaDiOrigine];
					expr_7DF[expr_7ED] = num2 + num5;
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
					List<float> expr_9C3 = listaDanniAlleati5 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_9D1 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati5[truppaDiOrigine];
					expr_9C3[expr_9D1] = num2 + num6;
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
					List<float> expr_B8F = listaDanniAlleati6 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_B9D = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati6[truppaDiOrigine];
					expr_B8F[expr_B9D] = num2 + num7;
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
					List<float> expr_D5A = listaDanniAlleati7 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_D68 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati7[truppaDiOrigine];
					expr_D5A[expr_D68] = num2 + num8;
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
					List<float> expr_EE1 = listaDanniAlleati8 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
					int truppaDiOrigine;
					int expr_EEF = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
					float num2 = listaDanniAlleati8[truppaDiOrigine];
					expr_EE1[expr_EEF] = num2 + num9;
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num9;
				}
			}
		}
		if (this.timerPartenza > 0.2f && collisione.gameObject.tag == "collider reale alleato")
		{
			this.corpoRigido.isKinematic = true;
			base.GetComponent<CapsuleCollider>().enabled = false;
			this.avviaTimer = true;
		}
	}

	// Token: 0x0400168A RID: 5770
	public float distanzaTiroTeso;

	// Token: 0x0400168B RID: 5771
	public float forzaImpulso;

	// Token: 0x0400168C RID: 5772
	private float timerPartenza;

	// Token: 0x0400168D RID: 5773
	private float timerImpatto;

	// Token: 0x0400168E RID: 5774
	private bool avviaTimer;

	// Token: 0x0400168F RID: 5775
	private bool esplosioneAvvenuta;

	// Token: 0x04001690 RID: 5776
	private Vector3 origine;

	// Token: 0x04001691 RID: 5777
	private Vector3 puntoBersaglio;

	// Token: 0x04001692 RID: 5778
	private GameObject oggettoColpito;

	// Token: 0x04001693 RID: 5779
	private Vector3 traiettoriaColpo;

	// Token: 0x04001694 RID: 5780
	private RaycastHit hitProiettile;

	// Token: 0x04001695 RID: 5781
	private Vector3 ultimaPosizione;

	// Token: 0x04001696 RID: 5782
	private Vector3 locazioneTarget;

	// Token: 0x04001697 RID: 5783
	private GameObject target;

	// Token: 0x04001698 RID: 5784
	private GameObject IANemico;

	// Token: 0x04001699 RID: 5785
	private GameObject terzaCamera;

	// Token: 0x0400169A RID: 5786
	private GameObject infoNeutreTattica;

	// Token: 0x0400169B RID: 5787
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x0400169C RID: 5788
	private Vector3 direzione;

	// Token: 0x0400169D RID: 5789
	private RaycastHit puntoDiMira;

	// Token: 0x0400169E RID: 5790
	private int layerPerMira;

	// Token: 0x0400169F RID: 5791
	private Rigidbody corpoRigido;

	// Token: 0x040016A0 RID: 5792
	private bool colpoDirezionato;
}
