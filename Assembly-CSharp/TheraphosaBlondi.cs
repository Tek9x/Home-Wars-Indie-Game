using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E5 RID: 229
public class TheraphosaBlondi : MonoBehaviour
{
	// Token: 0x0600077C RID: 1916 RVA: 0x0010BAC0 File Offset: 0x00109CC0
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

	// Token: 0x0600077D RID: 1917 RVA: 0x0010BB48 File Offset: 0x00109D48
	private void Update()
	{
		this.centroInsetto = base.GetComponent<PresenzaNemico>().centroInsetto;
		this.centroBaseInsetto = base.GetComponent<PresenzaNemico>().centroBaseInsetto;
		this.Morte();
		this.Attacco();
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x0010BB84 File Offset: 0x00109D84
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

	// Token: 0x0600077F RID: 1919 RVA: 0x0010BC14 File Offset: 0x00109E14
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
			else if (this.timerDiAttacco > 2.1f && !this.attaccoEffettuato)
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
					if (this.hitAttacco.collider.gameObject.tag == "Alleato")
					{
						GameObject gameObject4 = this.hitAttacco.collider.gameObject;
						float num7 = 0f;
						if (gameObject4.GetComponent<PresenzaAlleato>().vita > this.danno2)
						{
							num7 = this.danno2;
						}
						else if (gameObject4.GetComponent<PresenzaAlleato>().vita > 0f)
						{
							num7 = gameObject4.GetComponent<PresenzaAlleato>().vita;
						}
						gameObject4.GetComponent<PresenzaAlleato>().vita -= this.danno2;
						List<float> listaDanniNemici6;
						List<float> expr_62E = listaDanniNemici6 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_63C = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici6[tipoInsetto];
						expr_62E[expr_63C] = num2 + num7;
					}
					else if (this.hitAttacco.collider.gameObject.tag == "Trappola" && this.hitAttacco.collider.gameObject.GetComponent<PresenzaTrappola>())
					{
						GameObject gameObject5 = this.hitAttacco.collider.gameObject;
						float num8 = 0f;
						if (gameObject5.GetComponent<PresenzaTrappola>().vita > this.danno2)
						{
							num8 = this.danno2;
						}
						else if (gameObject5.GetComponent<PresenzaTrappola>().vita > 0f)
						{
							num8 = gameObject5.GetComponent<PresenzaTrappola>().vita;
						}
						gameObject5.GetComponent<PresenzaTrappola>().vita -= this.danno2;
						List<float> listaDanniNemici7;
						List<float> expr_726 = listaDanniNemici7 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_734 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici7[tipoInsetto];
						expr_726[expr_734] = num2 + num8;
					}
					else if (this.hitAttacco.collider.gameObject.name == "Avamposto Alleato(Clone)")
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
						List<float> expr_825 = listaDanniNemici8 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_833 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici8[tipoInsetto];
						expr_825[expr_833] = num2 + num9;
					}
					else if (this.hitAttacco.collider.gameObject.name == "Cassa Supply(Clone)")
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
						List<float> expr_924 = listaDanniNemici9 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_932 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici9[tipoInsetto];
						expr_924[expr_932] = num2 + num10;
					}
					else if (this.hitAttacco.collider.gameObject.name == "Camion per Convoglio(Clone)")
					{
						GameObject gameObject6 = this.hitAttacco.collider.gameObject;
						float num11 = 0f;
						if (gameObject6.GetComponent<ObbiettivoTatticoScript>().vita > this.danno2)
						{
							num11 = this.danno2;
						}
						else if (gameObject6.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
						{
							num11 = gameObject6.GetComponent<ObbiettivoTatticoScript>().vita;
						}
						gameObject6.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno2;
						List<float> listaDanniNemici10;
						List<float> expr_9FD = listaDanniNemici10 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_A0B = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici10[tipoInsetto];
						expr_9FD[expr_A0B] = num2 + num11;
					}
					GameObject gameObject7 = this.hitAttacco.collider.gameObject;
					int index = 0;
					bool flag = false;
					if (gameObject7 != null && gameObject7.tag == "Alleato")
					{
						for (int i = 0; i < gameObject7.GetComponent<PresenzaAlleato>().ListaAvvelenamento.Count; i++)
						{
							if (gameObject7.GetComponent<PresenzaAlleato>().ListaAvvelenamento[i][0] == base.GetComponent<PresenzaNemico>().dannoVeleno)
							{
								gameObject7.GetComponent<PresenzaAlleato>().ListaAvvelenamento[i][1] = gameObject7.GetComponent<PresenzaAlleato>().durataAvvelenamento;
								break;
							}
							if (!flag && gameObject7.GetComponent<PresenzaAlleato>().ListaAvvelenamento[i][0] == 0f)
							{
								index = i;
								flag = true;
							}
							if (i == gameObject7.GetComponent<PresenzaAlleato>().ListaAvvelenamento.Count - 1)
							{
								gameObject7.GetComponent<PresenzaAlleato>().ListaAvvelenamento[index][0] = base.GetComponent<PresenzaNemico>().dannoVeleno;
								gameObject7.GetComponent<PresenzaAlleato>().ListaAvvelenamento[index][1] = gameObject7.GetComponent<PresenzaAlleato>().durataAvvelenamento;
								gameObject7.GetComponent<PresenzaAlleato>().ListaAvvelenamento[index][2] = (float)base.GetComponent<PresenzaNemico>().tipoInsetto;
							}
						}
					}
				}
				this.attaccoEffettuato = true;
				this.cambiaAttacco = true;
			}
			if (this.timerDiAttacco < 4f)
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
			else if (this.timerDiAttacco > 4f)
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

	// Token: 0x04001BF2 RID: 7154
	private float danno1;

	// Token: 0x04001BF3 RID: 7155
	private float danno2;

	// Token: 0x04001BF4 RID: 7156
	private float frequenzaAttacco;

	// Token: 0x04001BF5 RID: 7157
	public float distanzaDiAttacco;

	// Token: 0x04001BF6 RID: 7158
	private Animator insettoAnim;

	// Token: 0x04001BF7 RID: 7159
	private int morteHash = Animator.StringToHash("insetto-morte");

	// Token: 0x04001BF8 RID: 7160
	private int attacco1Hash = Animator.StringToHash("insetto-attacco1");

	// Token: 0x04001BF9 RID: 7161
	private int attacco2Hash = Animator.StringToHash("insetto-attacco2");

	// Token: 0x04001BFA RID: 7162
	private float timerMorte;

	// Token: 0x04001BFB RID: 7163
	private GameObject bersaglio;

	// Token: 0x04001BFC RID: 7164
	private GameObject IANemico;

	// Token: 0x04001BFD RID: 7165
	private GameObject infoNeutreTattica;

	// Token: 0x04001BFE RID: 7166
	private float timerDiAttacco;

	// Token: 0x04001BFF RID: 7167
	private bool attaccoEffettuato;

	// Token: 0x04001C00 RID: 7168
	private RaycastHit hitAttacco;

	// Token: 0x04001C01 RID: 7169
	private float raggioInsettoNav;

	// Token: 0x04001C02 RID: 7170
	private int layerAttacco;

	// Token: 0x04001C03 RID: 7171
	private Vector3 centroInsetto;

	// Token: 0x04001C04 RID: 7172
	private Vector3 centroBaseInsetto;

	// Token: 0x04001C05 RID: 7173
	private bool turnoAttacco2;

	// Token: 0x04001C06 RID: 7174
	private bool cambiaAttacco;
}
