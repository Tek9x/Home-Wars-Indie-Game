using System;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class NavigazioneConCamminata : MonoBehaviour
{
	// Token: 0x060007B9 RID: 1977 RVA: 0x00113854 File Offset: 0x00111A54
	private void Start()
	{
		this.layerNavigazione = 256;
		this.insettoNav = base.GetComponent<NavMeshAgent>();
		this.velocitàIniziale = this.insettoNav.speed;
		this.insettoAnim = base.GetComponent<Animator>();
		this.tipoBattaglia = GestoreNeutroStrategia.tipoBattaglia;
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x001138A0 File Offset: 0x00111AA0
	private void Update()
	{
		this.bersaglio = base.GetComponent<PresenzaNemico>().bersaglio;
		this.timerControlloDest += Time.deltaTime;
		if (this.bersaglio && this.bersaglio.tag != "ObbiettivoTattico")
		{
			this.destPrincipale = this.bersaglio.transform.position;
			if (base.GetComponent<NavMeshAgent>().velocity.magnitude == 0f)
			{
				base.transform.LookAt(new Vector3(this.bersaglio.transform.position.x, base.transform.position.y, this.bersaglio.transform.position.z));
			}
			if (this.timerControlloDest > 2f)
			{
				this.insettoNav.SetDestination(this.destPrincipale);
				this.timerControlloDest = 0f;
			}
		}
		else
		{
			this.destPrincipale = base.GetComponent<PresenzaNemico>().destinazione;
			if (this.tipoBattaglia == 6)
			{
				if (this.timerControlloDest > 3f)
				{
					this.insettoNav.SetDestination(this.destPrincipale);
					this.timerControlloDest = 0f;
				}
			}
			else if (this.timerControlloDest > 15f)
			{
				this.insettoNav.SetDestination(this.destPrincipale);
				this.timerControlloDest = 0f;
			}
		}
		this.centroInsetto = base.GetComponent<PresenzaNemico>().centroInsetto;
		this.muoviti = base.GetComponent<PresenzaNemico>().muoviti;
		this.morto = base.GetComponent<PresenzaNemico>().morto;
		this.Morte();
		this.NavigazioneAutomatica();
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x00113A70 File Offset: 0x00111C70
	private void Morte()
	{
		if (this.morto)
		{
			this.insettoNav.speed = 0f;
			this.insettoAnim.SetBool(this.camminataHash, false);
		}
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x00113AA0 File Offset: 0x00111CA0
	private void NavigazioneAutomatica()
	{
		if (this.insettoNav.isOnOffMeshLink)
		{
			this.inJump = true;
			if (!this.calcoloJumpEffettuato)
			{
				this.calcoloDistJump = true;
			}
		}
		else
		{
			this.inJump = false;
			this.calcoloJumpEffettuato = false;
		}
		if (this.inJump)
		{
			if (this.calcoloDistJump)
			{
				this.calcoloDistJump = false;
				this.calcoloJumpEffettuato = true;
				float num = Mathf.Abs(this.destPrincipale.y - base.transform.position.y);
				if (this.muoviti && !this.morto)
				{
					this.insettoNav.speed = this.velocitàIniziale / (num / 20f) / 4f;
				}
				else
				{
					this.insettoNav.speed = 0f;
				}
			}
			float num2 = 9999f;
			Vector3 zero = Vector3.zero;
			for (int i = 0; i < 360; i += 45)
			{
				Quaternion rotation = Quaternion.AngleAxis((float)i, Vector3.up);
				Vector3 direction = rotation * -base.transform.forward;
				if (Physics.Raycast(this.centroInsetto, direction, out this.hitSuperficie, 15f, this.layerNavigazione))
				{
					float num3 = Vector3.Distance(this.centroInsetto, this.hitSuperficie.point);
					if (num3 < num2)
					{
					}
				}
			}
			base.transform.forward = this.insettoNav.velocity.normalized;
		}
		else if (this.muoviti && !this.morto)
		{
			if (!this.rallDaTrappola)
			{
				this.insettoNav.speed = this.velocitàIniziale;
			}
			else
			{
				this.insettoNav.speed = this.velocitàIniziale * this.effettoRallDaTrappola;
			}
		}
		else
		{
			this.insettoNav.speed = 0f;
		}
		if (this.insettoNav.velocity.magnitude == 0f || !this.muoviti)
		{
			this.insettoAnim.SetBool(this.camminataHash, false);
		}
		else
		{
			this.insettoAnim.SetBool(this.camminataHash, true);
		}
	}

	// Token: 0x04001D00 RID: 7424
	private GameObject bersaglio;

	// Token: 0x04001D01 RID: 7425
	private Vector3 destPrincipale;

	// Token: 0x04001D02 RID: 7426
	private int layerNavigazione;

	// Token: 0x04001D03 RID: 7427
	private RaycastHit hitSuperficie;

	// Token: 0x04001D04 RID: 7428
	private NavMeshAgent insettoNav;

	// Token: 0x04001D05 RID: 7429
	private float velocitàIniziale;

	// Token: 0x04001D06 RID: 7430
	private bool inJump;

	// Token: 0x04001D07 RID: 7431
	private bool calcoloDistJump;

	// Token: 0x04001D08 RID: 7432
	private bool calcoloJumpEffettuato;

	// Token: 0x04001D09 RID: 7433
	private bool muoviti;

	// Token: 0x04001D0A RID: 7434
	private bool morto;

	// Token: 0x04001D0B RID: 7435
	private Animator insettoAnim;

	// Token: 0x04001D0C RID: 7436
	private int camminataHash = Animator.StringToHash("insetto-camminata");

	// Token: 0x04001D0D RID: 7437
	private Vector3 centroInsetto;

	// Token: 0x04001D0E RID: 7438
	public bool rallDaTrappola;

	// Token: 0x04001D0F RID: 7439
	public float effettoRallDaTrappola;

	// Token: 0x04001D10 RID: 7440
	private float timerControlloDest;

	// Token: 0x04001D11 RID: 7441
	private float controlloDistDaDest;

	// Token: 0x04001D12 RID: 7442
	private int tipoBattaglia;
}
