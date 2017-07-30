using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D4 RID: 212
public class HemipepsisUstulata : MonoBehaviour
{
	// Token: 0x06000727 RID: 1831 RVA: 0x00100BB8 File Offset: 0x000FEDB8
	private void Start()
	{
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.insettoAnim = base.GetComponent<Animator>();
		this.layerAttacco = 540672;
		this.danno = base.GetComponent<PresenzaNemico>().danno1;
		this.frequenzaAttacco = base.GetComponent<PresenzaNemico>().frequenzaAttacco;
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x00100C20 File Offset: 0x000FEE20
	private void Update()
	{
		this.centroInsetto = base.GetComponent<PresenzaNemico>().centroInsetto;
		this.Morte();
		this.Attacco();
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x00100C40 File Offset: 0x000FEE40
	private void Morte()
	{
		if (base.GetComponent<PresenzaNemico>().morto)
		{
			this.insettoAnim.SetBool(this.attaccoHash, false);
			this.insettoAnim.SetBool(this.morteHash, true);
			if (base.GetComponent<PresenzaNemico>().timerMorte > 3f)
			{
				this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici.Remove(base.gameObject);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else if (!Physics.Raycast(this.centroInsetto, -Vector3.up, 0.5f, 256))
			{
				base.transform.position += -Vector3.up * 50f * Time.deltaTime;
			}
		}
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x00100D1C File Offset: 0x000FEF1C
	private void Attacco()
	{
		this.timerDiAttacco += Time.deltaTime;
		if (!this.partenzaTimerAllont && Physics.Raycast(this.centroInsetto, base.transform.forward, out this.hitAttacco, this.distanzaDiAttacco, this.layerAttacco))
		{
			base.GetComponent<PresenzaNemico>().muoviti = false;
			if (this.timerDiAttacco > this.frequenzaAttacco)
			{
				this.timerDiAttacco = 0f;
				this.attaccoEffettuato = false;
				Vector3 position = this.hitAttacco.collider.gameObject.transform.position;
				base.transform.LookAt(new Vector3(position.x, base.transform.position.y, position.z));
			}
			else if (this.timerDiAttacco > 0.7f && !this.attaccoEffettuato)
			{
				if (this.hitAttacco.collider.gameObject.tag == "Alleato")
				{
					GameObject gameObject = this.hitAttacco.collider.gameObject;
					float num = 0f;
					if (gameObject.GetComponent<PresenzaAlleato>().vita > this.danno)
					{
						num = this.danno;
					}
					else if (gameObject.GetComponent<PresenzaAlleato>().vita > 0f)
					{
						num = gameObject.GetComponent<PresenzaAlleato>().vita;
					}
					gameObject.GetComponent<PresenzaAlleato>().vita -= this.danno;
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
					if (gameObject2.GetComponent<PresenzaTrappola>().vita > this.danno)
					{
						num3 = this.danno;
					}
					else if (gameObject2.GetComponent<PresenzaTrappola>().vita > 0f)
					{
						num3 = gameObject2.GetComponent<PresenzaTrappola>().vita;
					}
					gameObject2.GetComponent<PresenzaTrappola>().vita -= this.danno;
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
					if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita > this.danno)
					{
						num4 = this.danno;
					}
					else if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
					{
						num4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita;
					}
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno;
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
					if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita > this.danno)
					{
						num5 = this.danno;
					}
					else if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
					{
						num5 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita;
					}
					this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno;
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
					if (gameObject3.GetComponent<ObbiettivoTatticoScript>().vita > this.danno)
					{
						num6 = this.danno;
					}
					else if (gameObject3.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
					{
						num6 = gameObject3.GetComponent<ObbiettivoTatticoScript>().vita;
					}
					gameObject3.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno;
					List<float> listaDanniNemici5;
					List<float> expr_555 = listaDanniNemici5 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
					int tipoInsetto;
					int expr_563 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
					float num2 = listaDanniNemici5[tipoInsetto];
					expr_555[expr_563] = num2 + num6;
				}
				else if (this.hitAttacco.collider.gameObject.name == "Satellite(Clone)")
				{
					GameObject satellite = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().satellite;
					float num7 = 0f;
					if (satellite.GetComponent<ObbiettivoTatticoScript>().vita > this.danno)
					{
						num7 = this.danno;
					}
					else if (satellite.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
					{
						num7 = satellite.GetComponent<ObbiettivoTatticoScript>().vita;
					}
					satellite.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno;
					List<float> listaDanniNemici6;
					List<float> expr_62E = listaDanniNemici6 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
					int tipoInsetto;
					int expr_63C = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
					float num2 = listaDanniNemici6[tipoInsetto];
					expr_62E[expr_63C] = num2 + num7;
				}
				this.attaccoEffettuato = true;
				GameObject gameObject4 = this.hitAttacco.collider.gameObject;
				int index = 0;
				bool flag = false;
				if (gameObject4 != null && gameObject4.tag == "Alleato")
				{
					for (int i = 0; i < gameObject4.GetComponent<PresenzaAlleato>().ListaAvvelenamento.Count; i++)
					{
						if (gameObject4.GetComponent<PresenzaAlleato>().ListaAvvelenamento[i][0] == base.GetComponent<PresenzaNemico>().dannoVeleno)
						{
							gameObject4.GetComponent<PresenzaAlleato>().ListaAvvelenamento[i][1] = gameObject4.GetComponent<PresenzaAlleato>().durataAvvelenamento;
							break;
						}
						if (!flag && gameObject4.GetComponent<PresenzaAlleato>().ListaAvvelenamento[i][0] == 0f)
						{
							index = i;
							flag = true;
						}
						if (i == gameObject4.GetComponent<PresenzaAlleato>().ListaAvvelenamento.Count - 1)
						{
							gameObject4.GetComponent<PresenzaAlleato>().ListaAvvelenamento[index][0] = base.GetComponent<PresenzaNemico>().dannoVeleno;
							gameObject4.GetComponent<PresenzaAlleato>().ListaAvvelenamento[index][1] = gameObject4.GetComponent<PresenzaAlleato>().durataAvvelenamento;
							gameObject4.GetComponent<PresenzaAlleato>().ListaAvvelenamento[index][2] = (float)base.GetComponent<PresenzaNemico>().tipoInsetto;
						}
					}
				}
			}
			if (this.timerDiAttacco < 0.7f)
			{
				this.insettoAnim.SetBool(this.attaccoHash, true);
			}
			else if (this.timerDiAttacco > 0.7f)
			{
				this.insettoAnim.SetBool(this.attaccoHash, false);
				this.partenzaTimerAllont = true;
				base.GetComponent<PresenzaNemico>().allontanamentoPerAtt = true;
			}
		}
		else
		{
			base.GetComponent<PresenzaNemico>().muoviti = true;
			this.insettoAnim.SetBool(this.attaccoHash, false);
		}
		if (this.partenzaTimerAllont)
		{
			this.timerAllontanamento += Time.deltaTime;
			if (this.timerAllontanamento > this.frequenzaAttacco / 1.7f)
			{
				this.timerAllontanamento = 0f;
				base.GetComponent<PresenzaNemico>().allontanamentoPerAtt = false;
				this.partenzaTimerAllont = false;
			}
		}
	}

	// Token: 0x04001AB7 RID: 6839
	private float danno;

	// Token: 0x04001AB8 RID: 6840
	private float frequenzaAttacco;

	// Token: 0x04001AB9 RID: 6841
	public float distanzaDiAttacco;

	// Token: 0x04001ABA RID: 6842
	private Animator insettoAnim;

	// Token: 0x04001ABB RID: 6843
	private int morteHash = Animator.StringToHash("insetto-morte");

	// Token: 0x04001ABC RID: 6844
	private int attaccoHash = Animator.StringToHash("insetto-attacco");

	// Token: 0x04001ABD RID: 6845
	private float timerMorte;

	// Token: 0x04001ABE RID: 6846
	private GameObject bersaglio;

	// Token: 0x04001ABF RID: 6847
	private GameObject IANemico;

	// Token: 0x04001AC0 RID: 6848
	private GameObject infoNeutreTattica;

	// Token: 0x04001AC1 RID: 6849
	private float timerDiAttacco;

	// Token: 0x04001AC2 RID: 6850
	private bool attaccoEffettuato;

	// Token: 0x04001AC3 RID: 6851
	private RaycastHit hitAttacco;

	// Token: 0x04001AC4 RID: 6852
	private float timerAllontanamento;

	// Token: 0x04001AC5 RID: 6853
	private bool partenzaTimerAllont;

	// Token: 0x04001AC6 RID: 6854
	private int layerAttacco;

	// Token: 0x04001AC7 RID: 6855
	private Vector3 centroInsetto;
}
