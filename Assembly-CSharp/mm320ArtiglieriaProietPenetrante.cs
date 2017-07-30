using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public class mm320ArtiglieriaProietPenetrante : MonoBehaviour
{
	// Token: 0x06000633 RID: 1587 RVA: 0x000DA3C4 File Offset: 0x000D85C4
	private void Start()
	{
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.origine = base.transform.position;
		if (GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.moltiplicatoreAttaccoInFPS = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().moltiplicatoreFPSBattVeloce;
		}
		else
		{
			this.moltiplicatoreAttaccoInFPS = PlayerPrefs.GetFloat("moltiplicatore danni PP");
		}
		this.corpoRigido = base.GetComponent<Rigidbody>();
		this.layerPerMira = 4621568;
		this.locazioneTarget = base.GetComponent<DatiProiettile>().locazioneTarget;
		if (!base.GetComponent<DatiProiettile>().sparatoInFPS)
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
		this.incrementoVelocità = 1f;
		base.GetComponent<ParticleSystem>().Play();
		base.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
		base.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x000DA56C File Offset: 0x000D876C
	private void Update()
	{
		this.timerPartenza += Time.deltaTime;
		this.SensoreEsplosione();
		if (this.avviaTimer)
		{
			this.timerImpatto += Time.deltaTime;
		}
		if (this.timerPartenza > 0.15f)
		{
			base.transform.GetChild(0).GetComponent<SphereCollider>().enabled = true;
			base.transform.GetChild(0).GetComponent<SphereCollider>().radius = base.GetComponent<DatiGeneraliMunizione>().raggioEffetto;
		}
		if (!base.GetComponent<DatiProiettile>().sparatoInFPS)
		{
			this.MovimentoIndipendente();
		}
		else if (this.timerPartenza > 0.2f && !this.avviaTimer)
		{
			base.transform.forward = this.corpoRigido.velocity;
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

	// Token: 0x06000635 RID: 1589 RVA: 0x000DA68C File Offset: 0x000D888C
	private void SensoreEsplosione()
	{
		if (!this.avviaTimer && Physics.Raycast(base.transform.position, base.transform.forward, 45f, this.layerPerMira))
		{
			base.GetComponent<Rigidbody>().isKinematic = true;
			base.GetComponent<CapsuleCollider>().enabled = false;
			base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<ParticleSystem>().Stop();
			base.GetComponent<ParticleSystem>().Clear();
			base.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
			base.GetComponent<AudioSource>().Play();
			this.posizioneAdEsplosione = base.transform.position;
			this.avviaTimer = true;
			this.Esplosione();
		}
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x000DA76C File Offset: 0x000D896C
	private void MovimentoIndipendente()
	{
		if (!base.GetComponent<DatiProiettile>().sparatoInFPS)
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
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x000DA8B4 File Offset: 0x000D8AB4
	private void Esplosione()
	{
		if (!this.esplosioneAvvenuta)
		{
			base.transform.position = this.posizioneAdEsplosione;
			foreach (GameObject current in base.transform.GetChild(0).GetComponent<ColliderAreaEffetto>().ListaAeraEffetto)
			{
				if (current != null)
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
							List<float> expr_19A = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
							int truppaDiOrigine;
							int expr_1A8 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
							float num2 = listaDanniAlleati[truppaDiOrigine];
							expr_19A[expr_1A8] = num2 + num;
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
							List<float> expr_2F1 = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
							int truppaDiOrigine;
							int expr_2FF = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
							float num2 = listaDanniAlleati2[truppaDiOrigine];
							expr_2F1[expr_2FF] = num2 + num3;
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
						List<float> expr_49A = listaDanniAlleati3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
						int truppaDiOrigine;
						int expr_4A8 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
						float num2 = listaDanniAlleati3[truppaDiOrigine];
						expr_49A[expr_4A8] = num2 + num4;
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
						List<float> expr_651 = listaDanniAlleati4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
						int truppaDiOrigine;
						int expr_65F = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
						float num2 = listaDanniAlleati4[truppaDiOrigine];
						expr_651[expr_65F] = num2 + num5;
						this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num5;
					}
				}
			}
			this.esplosioneAvvenuta = true;
		}
	}

	// Token: 0x04001743 RID: 5955
	public float forzaImpulso;

	// Token: 0x04001744 RID: 5956
	public float velocitàAutoRotazione;

	// Token: 0x04001745 RID: 5957
	private float timerPartenza;

	// Token: 0x04001746 RID: 5958
	private float timerImpatto;

	// Token: 0x04001747 RID: 5959
	private bool avviaTimer;

	// Token: 0x04001748 RID: 5960
	private GameObject IANemico;

	// Token: 0x04001749 RID: 5961
	private GameObject infoNeutreTattica;

	// Token: 0x0400174A RID: 5962
	private GameObject terzaCamera;

	// Token: 0x0400174B RID: 5963
	private bool esplosioneAvvenuta;

	// Token: 0x0400174C RID: 5964
	private Vector3 origine;

	// Token: 0x0400174D RID: 5965
	private Vector3 puntoBersaglio;

	// Token: 0x0400174E RID: 5966
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x0400174F RID: 5967
	private Rigidbody corpoRigido;

	// Token: 0x04001750 RID: 5968
	private RaycastHit hitProiettile;

	// Token: 0x04001751 RID: 5969
	private int layerPerMira;

	// Token: 0x04001752 RID: 5970
	private Vector3 puntoDiMezzo;

	// Token: 0x04001753 RID: 5971
	private Vector3 vertice;

	// Token: 0x04001754 RID: 5972
	private Vector3 direzioneSparo;

	// Token: 0x04001755 RID: 5973
	private Vector3 posizioneMassima;

	// Token: 0x04001756 RID: 5974
	private bool verticeRaggiunto;

	// Token: 0x04001757 RID: 5975
	private Vector3 dirDiCaduta;

	// Token: 0x04001758 RID: 5976
	private float velocitàAlPicco;

	// Token: 0x04001759 RID: 5977
	private Vector3 locazioneTarget;

	// Token: 0x0400175A RID: 5978
	private Vector3 posizioneAdEsplosione;

	// Token: 0x0400175B RID: 5979
	private float distanzaDiMetà;

	// Token: 0x0400175C RID: 5980
	private float timerIncrementoVelocità;

	// Token: 0x0400175D RID: 5981
	private float incrementoVelocità;
}
