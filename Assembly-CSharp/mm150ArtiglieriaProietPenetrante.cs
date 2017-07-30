using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A2 RID: 162
public class mm150ArtiglieriaProietPenetrante : MonoBehaviour
{
	// Token: 0x06000609 RID: 1545 RVA: 0x000D1F70 File Offset: 0x000D0170
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

	// Token: 0x0600060A RID: 1546 RVA: 0x000D2118 File Offset: 0x000D0318
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

	// Token: 0x0600060B RID: 1547 RVA: 0x000D2238 File Offset: 0x000D0438
	private void SensoreEsplosione()
	{
		if (!this.avviaTimer && Physics.Raycast(base.transform.position, base.transform.forward, 30f, this.layerPerMira))
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

	// Token: 0x0600060C RID: 1548 RVA: 0x000D2318 File Offset: 0x000D0518
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

	// Token: 0x0600060D RID: 1549 RVA: 0x000D2460 File Offset: 0x000D0660
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

	// Token: 0x040016B3 RID: 5811
	public float forzaImpulso;

	// Token: 0x040016B4 RID: 5812
	public float velocitàAutoRotazione;

	// Token: 0x040016B5 RID: 5813
	private float timerPartenza;

	// Token: 0x040016B6 RID: 5814
	private float timerImpatto;

	// Token: 0x040016B7 RID: 5815
	private bool avviaTimer;

	// Token: 0x040016B8 RID: 5816
	private GameObject IANemico;

	// Token: 0x040016B9 RID: 5817
	private GameObject infoNeutreTattica;

	// Token: 0x040016BA RID: 5818
	private GameObject terzaCamera;

	// Token: 0x040016BB RID: 5819
	private bool esplosioneAvvenuta;

	// Token: 0x040016BC RID: 5820
	private Vector3 origine;

	// Token: 0x040016BD RID: 5821
	private Vector3 puntoBersaglio;

	// Token: 0x040016BE RID: 5822
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x040016BF RID: 5823
	private Rigidbody corpoRigido;

	// Token: 0x040016C0 RID: 5824
	private RaycastHit hitProiettile;

	// Token: 0x040016C1 RID: 5825
	private int layerPerMira;

	// Token: 0x040016C2 RID: 5826
	private Vector3 puntoDiMezzo;

	// Token: 0x040016C3 RID: 5827
	private Vector3 vertice;

	// Token: 0x040016C4 RID: 5828
	private Vector3 direzioneSparo;

	// Token: 0x040016C5 RID: 5829
	private Vector3 posizioneMassima;

	// Token: 0x040016C6 RID: 5830
	private bool verticeRaggiunto;

	// Token: 0x040016C7 RID: 5831
	private Vector3 dirDiCaduta;

	// Token: 0x040016C8 RID: 5832
	private float velocitàAlPicco;

	// Token: 0x040016C9 RID: 5833
	private Vector3 locazioneTarget;

	// Token: 0x040016CA RID: 5834
	private Vector3 posizioneAdEsplosione;

	// Token: 0x040016CB RID: 5835
	private float distanzaDiMetà;

	// Token: 0x040016CC RID: 5836
	private float timerIncrementoVelocità;

	// Token: 0x040016CD RID: 5837
	private float incrementoVelocità;
}
