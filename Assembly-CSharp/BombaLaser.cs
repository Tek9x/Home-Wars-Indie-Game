using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B0 RID: 176
public class BombaLaser : MonoBehaviour
{
	// Token: 0x06000662 RID: 1634 RVA: 0x000E37E0 File Offset: 0x000E19E0
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x000E381C File Offset: 0x000E1A1C
	private void Update()
	{
		if (this.avviaTimer)
		{
			this.timerImpatto += Time.deltaTime;
			this.Esplosione();
		}
		else
		{
			base.transform.position += -Vector3.up * Time.deltaTime * 200f;
			base.transform.Rotate(base.transform.forward * Time.deltaTime * 80f);
			base.transform.forward = -Vector3.up;
			base.transform.GetChild(0).GetComponent<SphereCollider>().radius = base.GetComponent<DatiGeneraliMunizione>().raggioEffetto;
		}
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x000E38E8 File Offset: 0x000E1AE8
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

	// Token: 0x06000665 RID: 1637 RVA: 0x000E3CB0 File Offset: 0x000E1EB0
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
			base.transform.GetChild(0).GetComponent<AudioSource>().Stop();
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
				List<float> expr_290 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int index;
				int expr_295 = index = 10;
				float num2 = listaDanniAlleati[index];
				expr_290[expr_295] = num2 + num;
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
				List<float> expr_48D = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int index;
				int expr_492 = index = 10;
				float num2 = listaDanniAlleati2[index];
				expr_48D[expr_492] = num2 + num3;
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
				List<float> expr_666 = listaDanniAlleati3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int index;
				int expr_66B = index = 10;
				float num2 = listaDanniAlleati3[index];
				expr_666[expr_66B] = num2 + num4;
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
				List<float> expr_824 = listaDanniAlleati4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int index;
				int expr_829 = index = 10;
				float num2 = listaDanniAlleati4[index];
				expr_824[expr_829] = num2 + num5;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num5;
			}
		}
	}

	// Token: 0x040017E7 RID: 6119
	private GameObject infoNeutreTattica;

	// Token: 0x040017E8 RID: 6120
	private GameObject terzaCamera;

	// Token: 0x040017E9 RID: 6121
	private GameObject oggettoColpito;

	// Token: 0x040017EA RID: 6122
	private bool avviaTimer;

	// Token: 0x040017EB RID: 6123
	private float timerImpatto;

	// Token: 0x040017EC RID: 6124
	private bool esplosioneAvvenuta;
}
