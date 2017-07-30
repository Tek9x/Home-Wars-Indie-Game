using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B2 RID: 178
public class ColpoArtiglieriaDaObserver : MonoBehaviour
{
	// Token: 0x0600066B RID: 1643 RVA: 0x000E4A30 File Offset: 0x000E2C30
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.destinazione = new Vector3(this.destTeorica.x + UnityEngine.Random.Range(-25f, 25f), this.destTeorica.y, this.destTeorica.z + UnityEngine.Random.Range(-25f, 25f));
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x000E4AB8 File Offset: 0x000E2CB8
	private void Update()
	{
		if (this.avviaTimer)
		{
			this.timerImpatto += Time.deltaTime;
			this.Esplosione();
		}
		else
		{
			Vector3 forward = this.destinazione - base.transform.position;
			base.transform.forward = forward;
			base.transform.position += base.transform.forward * Time.deltaTime * 80f;
			base.transform.GetChild(0).GetComponent<SphereCollider>().radius = base.GetComponent<DatiGeneraliMunizione>().raggioEffetto;
		}
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x000E4B68 File Offset: 0x000E2D68
	private void Esplosione()
	{
		if (!this.esplosioneAvvenuta && this.timerImpatto > 0f)
		{
			foreach (GameObject current in base.transform.GetChild(0).GetComponent<ColliderAreaEffetto>().ListaAeraEffetto)
			{
				if (current != null && current != this.oggettoColpito)
				{
					if (current.tag == "Nemico")
					{
						this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
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
						int expr_1B4 = index = 10;
						float num2 = listaDanniAlleati[index];
						expr_1AF[expr_1B4] = num2 + num;
						this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num;
					}
					else if (current.tag == "ObbiettivoTattico" && (current.name == "Avamposto Nemico(Clone)" || current.name == "Pane per Convoglio(Clone)"))
					{
						this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
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
						List<float> expr_32A = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
						int index;
						int expr_32F = index = 10;
						float num2 = listaDanniAlleati2[index];
						expr_32A[expr_32F] = num2 + num3;
						this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num3;
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

	// Token: 0x0600066E RID: 1646 RVA: 0x000E4F30 File Offset: 0x000E3130
	private void OnCollisionEnter(Collision collisione)
	{
		if (collisione.gameObject.tag == "Ambiente" || collisione.gameObject.tag == "Nemico" || collisione.gameObject.tag == "Nemico Testa" || collisione.gameObject.tag == "Nemico Coll Suppl" || collisione.gameObject.tag == "ObbiettivoTattico")
		{
			base.GetComponent<Rigidbody>().isKinematic = true;
			base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<CapsuleCollider>().enabled = false;
			base.GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
			base.GetComponent<AudioSource>().Play();
			this.avviaTimer = true;
			if (collisione.gameObject.tag == "Nemico")
			{
				this.oggettoColpito = collisione.gameObject;
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
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
				List<float> expr_27A = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int index;
				int expr_27F = index = 10;
				float num2 = listaDanniAlleati[index];
				expr_27A[expr_27F] = num2 + num;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num;
			}
			else if (collisione.gameObject.tag == "Nemico Testa")
			{
				this.oggettoColpito = collisione.transform.parent.gameObject;
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
				float num3 = 0f;
				if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f)
				{
					num3 = base.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f;
				}
				else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num3 = this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
				}
				this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f;
				if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura) * 2f)
				{
					num3 += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura) * 2f;
				}
				else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num3 += this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
				}
				this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura) * 2f;
				List<float> listaDanniAlleati2;
				List<float> expr_477 = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int index;
				int expr_47C = index = 10;
				float num2 = listaDanniAlleati2[index];
				expr_477[expr_47C] = num2 + num3;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num3;
			}
			else if (collisione.gameObject.tag == "Nemico Coll Suppl")
			{
				this.oggettoColpito = collisione.transform.parent.gameObject;
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
				float num4 = 0f;
				if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione)
				{
					num4 = base.GetComponent<DatiGeneraliMunizione>().penetrazione;
				}
				else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num4 = this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
				}
				this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione;
				if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura))
				{
					num4 += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura);
				}
				else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num4 += this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
				}
				this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura);
				List<float> listaDanniAlleati3;
				List<float> expr_650 = listaDanniAlleati3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int index;
				int expr_655 = index = 10;
				float num2 = listaDanniAlleati3[index];
				expr_650[expr_655] = num2 + num4;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num4;
			}
			else if ((collisione.gameObject.tag == "ObbiettivoTattico" && collisione.gameObject.name == "Avamposto Nemico(Clone)") || collisione.gameObject.name == "Pane per Convoglio(Clone)")
			{
				this.oggettoColpito = collisione.gameObject;
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
				float num5 = 0f;
				if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione)
				{
					num5 = base.GetComponent<DatiGeneraliMunizione>().penetrazione;
				}
				else if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
				{
					num5 = this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita;
				}
				this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione;
				if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().danno)
				{
					num5 += base.GetComponent<DatiGeneraliMunizione>().danno;
				}
				else if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
				{
					num5 += this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita;
				}
				this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno;
				List<float> listaDanniAlleati4;
				List<float> expr_80E = listaDanniAlleati4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int index;
				int expr_813 = index = 10;
				float num2 = listaDanniAlleati4[index];
				expr_80E[expr_813] = num2 + num5;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num5;
			}
		}
	}

	// Token: 0x040017FD RID: 6141
	private GameObject infoNeutreTattica;

	// Token: 0x040017FE RID: 6142
	private GameObject terzaCamera;

	// Token: 0x040017FF RID: 6143
	public Vector3 destTeorica;

	// Token: 0x04001800 RID: 6144
	private Vector3 destinazione;

	// Token: 0x04001801 RID: 6145
	private float timerImpatto;

	// Token: 0x04001802 RID: 6146
	private bool avviaTimer;

	// Token: 0x04001803 RID: 6147
	private GameObject oggettoColpito;

	// Token: 0x04001804 RID: 6148
	private bool esplosioneAvvenuta;
}
