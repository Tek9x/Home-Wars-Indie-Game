using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E3 RID: 227
public class TabanusBovinus : MonoBehaviour
{
	// Token: 0x06000772 RID: 1906 RVA: 0x0010A7DC File Offset: 0x001089DC
	private void Start()
	{
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.insettoAnim = base.GetComponent<Animator>();
		this.layerAttacco = 540672;
		this.danno = base.GetComponent<PresenzaNemico>().danno1;
		this.frequenzaAttacco = base.GetComponent<PresenzaNemico>().frequenzaAttacco;
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x0010A844 File Offset: 0x00108A44
	private void Update()
	{
		this.centroInsetto = base.GetComponent<PresenzaNemico>().centroInsetto;
		this.Morte();
		this.Attacco();
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x0010A864 File Offset: 0x00108A64
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

	// Token: 0x06000775 RID: 1909 RVA: 0x0010A940 File Offset: 0x00108B40
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
			else if (this.timerDiAttacco > 0.5f && !this.attaccoEffettuato)
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
				float num8 = base.GetComponent<PresenzaNemico>().vitaIniziale / 4f;
				if (base.GetComponent<PresenzaNemico>().vita < base.GetComponent<PresenzaNemico>().vitaIniziale - num8)
				{
					base.GetComponent<PresenzaNemico>().vita += num8;
				}
				else
				{
					base.GetComponent<PresenzaNemico>().vita = base.GetComponent<PresenzaNemico>().vitaIniziale;
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

	// Token: 0x04001BD0 RID: 7120
	private float danno;

	// Token: 0x04001BD1 RID: 7121
	private float frequenzaAttacco;

	// Token: 0x04001BD2 RID: 7122
	public float distanzaDiAttacco;

	// Token: 0x04001BD3 RID: 7123
	private Animator insettoAnim;

	// Token: 0x04001BD4 RID: 7124
	private int morteHash = Animator.StringToHash("insetto-morte");

	// Token: 0x04001BD5 RID: 7125
	private int attaccoHash = Animator.StringToHash("insetto-attacco");

	// Token: 0x04001BD6 RID: 7126
	private float timerMorte;

	// Token: 0x04001BD7 RID: 7127
	private GameObject bersaglio;

	// Token: 0x04001BD8 RID: 7128
	private GameObject IANemico;

	// Token: 0x04001BD9 RID: 7129
	private GameObject infoNeutreTattica;

	// Token: 0x04001BDA RID: 7130
	private float timerDiAttacco;

	// Token: 0x04001BDB RID: 7131
	private bool attaccoEffettuato;

	// Token: 0x04001BDC RID: 7132
	private RaycastHit hitAttacco;

	// Token: 0x04001BDD RID: 7133
	private float timerAllontanamento;

	// Token: 0x04001BDE RID: 7134
	private bool partenzaTimerAllont;

	// Token: 0x04001BDF RID: 7135
	private int layerAttacco;

	// Token: 0x04001BE0 RID: 7136
	private Vector3 centroInsetto;
}
