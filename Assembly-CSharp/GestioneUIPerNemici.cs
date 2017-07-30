using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020000EB RID: 235
public class GestioneUIPerNemici : MonoBehaviour
{
	// Token: 0x06000798 RID: 1944 RVA: 0x0010F148 File Offset: 0x0010D348
	private void Start()
	{
		this.CanvasComandante = GameObject.FindGameObjectWithTag("CanvasComandante");
		this.pannelloInfoNemico = this.CanvasComandante.transform.FindChild("Informazioni Nemico").gameObject;
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x0010F188 File Offset: 0x0010D388
	private void Update()
	{
		this.FunzionePannelloInfoNemico();
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x0010F190 File Offset: 0x0010D390
	private void FunzionePannelloInfoNemico()
	{
		if (!Selezionamento.selezioneInvalidata && this.mostraPannelloInfoNemico && this.nemicoPerPannelloInfo && this.nemicoPerPannelloInfo.GetComponent<PresenzaNemico>().vita > 0f && !EventSystem.current.IsPointerOverGameObject())
		{
			this.pannelloInfoNemico.GetComponent<CanvasGroup>().alpha = 1f;
			this.pannelloInfoNemico.transform.GetChild(0).GetComponent<Text>().text = this.nemicoPerPannelloInfo.GetComponent<PresenzaNemico>().nomeInsetto;
			this.pannelloInfoNemico.transform.GetChild(1).GetComponent<Text>().text = string.Concat(new string[]
			{
				"Health:  ",
				this.nemicoPerPannelloInfo.GetComponent<PresenzaNemico>().vita.ToString("F0"),
				"\nArmor:  ",
				(this.nemicoPerPannelloInfo.GetComponent<PresenzaNemico>().armatura * 100f).ToString(),
				"%\nDamage 1:  ",
				this.nemicoPerPannelloInfo.GetComponent<PresenzaNemico>().danno1.ToString("F0"),
				"\nDamage 2:  ",
				this.nemicoPerPannelloInfo.GetComponent<PresenzaNemico>().danno2.ToString("F0"),
				"\nVenom Damage:  ",
				this.nemicoPerPannelloInfo.GetComponent<PresenzaNemico>().dannoVeleno.ToString("F0"),
				"\nAttack Rate:  ",
				this.nemicoPerPannelloInfo.GetComponent<PresenzaNemico>().frequenzaAttacco.ToString(),
				"\nSpeed:  ",
				this.nemicoPerPannelloInfo.GetComponent<PresenzaNemico>().velocitàInsetto,
				"\nFlying:  ",
				this.nemicoPerPannelloInfo.GetComponent<PresenzaNemico>().insettoVolante.ToString(),
				"\nJumping:  ",
				this.nemicoPerPannelloInfo.GetComponent<PresenzaNemico>().èSaltatore.ToString(),
				"\n Members for group:  ",
				this.nemicoPerPannelloInfo.GetComponent<PresenzaNemico>().numMembriGruppo.ToString()
			});
			this.pannelloInfoNemico.transform.GetChild(2).GetComponent<Text>().text = this.nemicoPerPannelloInfo.GetComponent<PresenzaNemico>().oggettoDescrizione.GetComponent<Text>().text;
		}
		else
		{
			this.pannelloInfoNemico.GetComponent<CanvasGroup>().alpha = 0f;
			this.mostraPannelloInfoNemico = false;
		}
	}

	// Token: 0x04001C4C RID: 7244
	private GameObject CanvasComandante;

	// Token: 0x04001C4D RID: 7245
	private GameObject pannelloInfoNemico;

	// Token: 0x04001C4E RID: 7246
	public GameObject nemicoPerPannelloInfo;

	// Token: 0x04001C4F RID: 7247
	public bool mostraPannelloInfoNemico;
}
