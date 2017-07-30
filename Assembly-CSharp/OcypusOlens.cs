using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000DB RID: 219
public class OcypusOlens : MonoBehaviour
{
	// Token: 0x0600074A RID: 1866 RVA: 0x001050D0 File Offset: 0x001032D0
	private void Start()
	{
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.insettoAnim = base.GetComponent<Animator>();
		this.layerAttacco = 540672;
		this.raggioInsettoNav = base.GetComponent<NavMeshAgent>().radius;
		this.danno1 = base.GetComponent<PresenzaNemico>().danno1;
		this.danno2 = base.GetComponent<PresenzaNemico>().danno2;
		this.frequenzaAttacco = base.GetComponent<PresenzaNemico>().frequenzaAttacco;
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x00105158 File Offset: 0x00103358
	private void Update()
	{
		this.centroInsetto = base.GetComponent<PresenzaNemico>().centroInsetto;
		this.centroBaseInsetto = base.GetComponent<PresenzaNemico>().centroBaseInsetto;
		this.Morte();
		this.Attacco();
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x00105194 File Offset: 0x00103394
	private void Morte()
	{
		if (base.GetComponent<PresenzaNemico>().morto)
		{
			this.insettoAnim.SetBool(this.attacco1Hash, false);
			this.insettoAnim.SetBool(this.attacco2Hash, false);
			this.insettoAnim.SetBool(this.morteHash, true);
			if (base.GetComponent<PresenzaNemico>().timerMorte > 3f)
			{
				this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici.Remove(base.gameObject);
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x00105224 File Offset: 0x00103424
	private void Attacco()
	{
		this.timerDiAttacco += Time.deltaTime;
		if (Physics.Raycast(this.centroBaseInsetto, base.transform.forward, out this.hitAttacco, this.distanzaDiAttacco, this.layerAttacco))
		{
			base.GetComponent<PresenzaNemico>().muoviti = false;
			if (this.timerDiAttacco > this.frequenzaAttacco)
			{
				this.timerDiAttacco = 0f;
				this.attaccoEffettuato = false;
				Vector3 position = this.hitAttacco.collider.gameObject.transform.position;
				base.transform.LookAt(new Vector3(position.x, base.transform.position.y, position.z));
			}
			else if (this.timerDiAttacco > 0.8f && !this.attaccoEffettuato)
			{
				if (!this.turnoAttacco2)
				{
					if (this.hitAttacco.collider.gameObject.tag == "Alleato")
					{
						GameObject gameObject = this.hitAttacco.collider.gameObject;
						float num = 0f;
						if (gameObject.GetComponent<PresenzaAlleato>().vita > this.danno1)
						{
							num = this.danno1;
						}
						else if (gameObject.GetComponent<PresenzaAlleato>().vita > 0f)
						{
							num = gameObject.GetComponent<PresenzaAlleato>().vita;
						}
						gameObject.GetComponent<PresenzaAlleato>().vita -= this.danno1;
						List<float> listaDanniNemici;
						List<float> expr_18C = listaDanniNemici = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_19A = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici[tipoInsetto];
						expr_18C[expr_19A] = num2 + num;
					}
					else if (this.hitAttacco.collider.gameObject.tag == "Trappola" && this.hitAttacco.collider.gameObject.GetComponent<PresenzaTrappola>())
					{
						GameObject gameObject2 = this.hitAttacco.collider.gameObject;
						float num3 = 0f;
						if (gameObject2.GetComponent<PresenzaTrappola>().vita > this.danno1)
						{
							num3 = this.danno1;
						}
						else if (gameObject2.GetComponent<PresenzaTrappola>().vita > 0f)
						{
							num3 = gameObject2.GetComponent<PresenzaTrappola>().vita;
						}
						gameObject2.GetComponent<PresenzaTrappola>().vita -= this.danno1;
						List<float> listaDanniNemici2;
						List<float> expr_27E = listaDanniNemici2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_28C = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici2[tipoInsetto];
						expr_27E[expr_28C] = num2 + num3;
					}
					else if (this.hitAttacco.collider.gameObject.name == "Avamposto Alleato(Clone)")
					{
						float num4 = 0f;
						if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita > this.danno1)
						{
							num4 = this.danno1;
						}
						else if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
						{
							num4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita;
						}
						this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno1;
						List<float> listaDanniNemici3;
						List<float> expr_37D = listaDanniNemici3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_38B = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici3[tipoInsetto];
						expr_37D[expr_38B] = num2 + num4;
					}
					else if (this.hitAttacco.collider.gameObject.name == "Cassa Supply(Clone)")
					{
						float num5 = 0f;
						if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita > this.danno1)
						{
							num5 = this.danno1;
						}
						else if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
						{
							num5 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita;
						}
						this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno1;
						List<float> listaDanniNemici4;
						List<float> expr_47C = listaDanniNemici4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_48A = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici4[tipoInsetto];
						expr_47C[expr_48A] = num2 + num5;
					}
					else if (this.hitAttacco.collider.gameObject.name == "Camion per Convoglio(Clone)")
					{
						GameObject gameObject3 = this.hitAttacco.collider.gameObject;
						float num6 = 0f;
						if (gameObject3.GetComponent<ObbiettivoTatticoScript>().vita > this.danno1)
						{
							num6 = this.danno1;
						}
						else if (gameObject3.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
						{
							num6 = gameObject3.GetComponent<ObbiettivoTatticoScript>().vita;
						}
						gameObject3.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno1;
						List<float> listaDanniNemici5;
						List<float> expr_555 = listaDanniNemici5 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_563 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici5[tipoInsetto];
						expr_555[expr_563] = num2 + num6;
					}
				}
				else
				{
					foreach (GameObject current in this.oggettoSpruzzo.GetComponent<ColliderAereaAttNemici>().ListaAeraEffetto)
					{
						if (current != null)
						{
							if (current.tag == "Alleato")
							{
								float num7 = 0f;
								if (current.GetComponent<PresenzaAlleato>().vita > this.danno2)
								{
									num7 = this.danno2;
								}
								else if (current.GetComponent<PresenzaAlleato>().vita > 0f)
								{
									num7 = current.GetComponent<PresenzaAlleato>().vita;
								}
								current.GetComponent<PresenzaAlleato>().vita -= this.danno2;
								List<float> listaDanniNemici6;
								List<float> expr_640 = listaDanniNemici6 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
								int tipoInsetto;
								int expr_64E = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
								float num2 = listaDanniNemici6[tipoInsetto];
								expr_640[expr_64E] = num2 + num7;
							}
							else if (current.tag == "Trappola" && current.GetComponent<PresenzaTrappola>())
							{
								float num8 = 0f;
								if (current.GetComponent<PresenzaTrappola>().vita > this.danno2)
								{
									num8 = this.danno2;
								}
								else if (current.GetComponent<PresenzaTrappola>().vita > 0f)
								{
									num8 = current.GetComponent<PresenzaTrappola>().vita;
								}
								current.GetComponent<PresenzaTrappola>().vita -= this.danno2;
								List<float> listaDanniNemici7;
								List<float> expr_70A = listaDanniNemici7 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
								int tipoInsetto;
								int expr_718 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
								float num2 = listaDanniNemici7[tipoInsetto];
								expr_70A[expr_718] = num2 + num8;
							}
							else if (current.name == "Avamposto Alleato(Clone)")
							{
								float num9 = 0f;
								if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita > this.danno2)
								{
									num9 = this.danno2;
								}
								else if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
								{
									num9 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita;
								}
								this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno2;
								List<float> listaDanniNemici8;
								List<float> expr_7FB = listaDanniNemici8 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
								int tipoInsetto;
								int expr_809 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
								float num2 = listaDanniNemici8[tipoInsetto];
								expr_7FB[expr_809] = num2 + num9;
							}
							else if (current.name == "Cassa Supply(Clone)")
							{
								float num10 = 0f;
								if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita > this.danno2)
								{
									num10 = this.danno2;
								}
								else if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
								{
									num10 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita;
								}
								this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno2;
								List<float> listaDanniNemici9;
								List<float> expr_8EC = listaDanniNemici9 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
								int tipoInsetto;
								int expr_8FA = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
								float num2 = listaDanniNemici9[tipoInsetto];
								expr_8EC[expr_8FA] = num2 + num10;
							}
							else if (current.name == "Camion per Convoglio(Clone)")
							{
								float num11 = 0f;
								if (current.GetComponent<ObbiettivoTatticoScript>().vita > this.danno2)
								{
									num11 = this.danno2;
								}
								else if (current.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
								{
									num11 = current.GetComponent<ObbiettivoTatticoScript>().vita;
								}
								current.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno2;
								List<float> listaDanniNemici10;
								List<float> expr_9A5 = listaDanniNemici10 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
								int tipoInsetto;
								int expr_9B3 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
								float num2 = listaDanniNemici10[tipoInsetto];
								expr_9A5[expr_9B3] = num2 + num11;
							}
						}
					}
					this.oggettoSpruzzo.GetComponent<ParticleSystem>().Play();
					foreach (GameObject current2 in this.oggettoSpruzzo.GetComponent<ColliderAereaAttNemici>().ListaAeraEffetto)
					{
						if (current2 != null && current2.tag == "Alleato")
						{
							int index = 0;
							bool flag = false;
							for (int i = 0; i < current2.GetComponent<PresenzaAlleato>().ListaAvvelenamento.Count; i++)
							{
								if (current2.GetComponent<PresenzaAlleato>().ListaAvvelenamento[i][0] == base.GetComponent<PresenzaNemico>().dannoVeleno)
								{
									current2.GetComponent<PresenzaAlleato>().ListaAvvelenamento[i][1] = current2.GetComponent<PresenzaAlleato>().durataAvvelenamento;
									break;
								}
								if (!flag && current2.GetComponent<PresenzaAlleato>().ListaAvvelenamento[i][0] == 0f)
								{
									index = i;
									flag = true;
								}
								if (i == current2.GetComponent<PresenzaAlleato>().ListaAvvelenamento.Count - 1)
								{
									current2.GetComponent<PresenzaAlleato>().ListaAvvelenamento[index][0] = base.GetComponent<PresenzaNemico>().dannoVeleno;
									current2.GetComponent<PresenzaAlleato>().ListaAvvelenamento[index][1] = current2.GetComponent<PresenzaAlleato>().durataAvvelenamento;
									current2.GetComponent<PresenzaAlleato>().ListaAvvelenamento[index][2] = (float)base.GetComponent<PresenzaNemico>().tipoInsetto;
								}
							}
						}
					}
				}
				this.attaccoEffettuato = true;
				this.cambiaAttacco = true;
			}
			if (this.timerDiAttacco < 1.8f)
			{
				if (!this.turnoAttacco2)
				{
					this.insettoAnim.SetBool(this.attacco1Hash, true);
				}
				else
				{
					this.insettoAnim.SetBool(this.attacco2Hash, true);
				}
			}
			else if (this.timerDiAttacco > 1.8f)
			{
				this.insettoAnim.SetBool(this.attacco1Hash, false);
				this.insettoAnim.SetBool(this.attacco2Hash, false);
				if (this.cambiaAttacco)
				{
					this.cambiaAttacco = false;
					this.turnoAttacco2 = !this.turnoAttacco2;
				}
			}
		}
		else
		{
			base.GetComponent<PresenzaNemico>().muoviti = true;
			this.insettoAnim.SetBool(this.attacco1Hash, false);
		}
	}

	// Token: 0x04001B37 RID: 6967
	private float danno1;

	// Token: 0x04001B38 RID: 6968
	private float danno2;

	// Token: 0x04001B39 RID: 6969
	private float frequenzaAttacco;

	// Token: 0x04001B3A RID: 6970
	public float distanzaDiAttacco;

	// Token: 0x04001B3B RID: 6971
	private Animator insettoAnim;

	// Token: 0x04001B3C RID: 6972
	private int morteHash = Animator.StringToHash("insetto-morte");

	// Token: 0x04001B3D RID: 6973
	private int attacco1Hash = Animator.StringToHash("insetto-attacco1");

	// Token: 0x04001B3E RID: 6974
	private int attacco2Hash = Animator.StringToHash("insetto-attacco2");

	// Token: 0x04001B3F RID: 6975
	private float timerMorte;

	// Token: 0x04001B40 RID: 6976
	private GameObject bersaglio;

	// Token: 0x04001B41 RID: 6977
	private GameObject IANemico;

	// Token: 0x04001B42 RID: 6978
	private GameObject infoNeutreTattica;

	// Token: 0x04001B43 RID: 6979
	private float timerDiAttacco;

	// Token: 0x04001B44 RID: 6980
	private bool attaccoEffettuato;

	// Token: 0x04001B45 RID: 6981
	private RaycastHit hitAttacco;

	// Token: 0x04001B46 RID: 6982
	private float raggioInsettoNav;

	// Token: 0x04001B47 RID: 6983
	private int layerAttacco;

	// Token: 0x04001B48 RID: 6984
	private Vector3 centroInsetto;

	// Token: 0x04001B49 RID: 6985
	private Vector3 centroBaseInsetto;

	// Token: 0x04001B4A RID: 6986
	public GameObject oggettoSpruzzo;

	// Token: 0x04001B4B RID: 6987
	private bool turnoAttacco2;

	// Token: 0x04001B4C RID: 6988
	private bool cambiaAttacco;
}
