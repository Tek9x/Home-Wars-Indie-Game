using System;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public class NavigazioneConSalti : MonoBehaviour
{
	// Token: 0x060007BE RID: 1982 RVA: 0x00113D20 File Offset: 0x00111F20
	private void Start()
	{
		this.layerNavigazione = 524544;
		this.layerSalto = 524544;
		this.insettoNav = base.GetComponent<NavMeshAgent>();
		this.velocitàIniziale = this.insettoNav.speed;
		this.insettoAnim = base.GetComponent<Animator>();
		this.increVelSalto = 1f;
		this.distanzaMaxSaltoReale = UnityEngine.Random.Range(this.distanzaMaxDiSalto - 50f, this.distanzaMaxDiSalto + 50f);
		this.tipoBattaglia = GestoreNeutroStrategia.tipoBattaglia;
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x00113DA8 File Offset: 0x00111FA8
	private void Update()
	{
		this.bersaglio = base.GetComponent<PresenzaNemico>().bersaglio;
		if (this.bersaglio && this.bersaglio.tag != "ObbiettivoTattico")
		{
			this.destPrincipale = this.bersaglio.transform.position;
		}
		else
		{
			this.destPrincipale = base.GetComponent<PresenzaNemico>().destinazione;
		}
		this.muoviti = base.GetComponent<PresenzaNemico>().muoviti;
		this.morto = base.GetComponent<PresenzaNemico>().morto;
		this.centroInsetto = base.GetComponent<PresenzaNemico>().centroInsetto;
		this.Morte();
		this.distDestinazione = Vector3.Distance(this.centroInsetto, this.destPrincipale);
		if (this.inSalto)
		{
			this.Salto();
		}
		else
		{
			this.timerCammFraSalti += Time.deltaTime;
			if (this.distDestinazione > this.distanzaMinDiSalto && this.timerCammFraSalti > this.tempoCammFraSalti)
			{
				this.Salto();
			}
			else if (!this.inAria)
			{
				this.NavigazioneAutomatica();
			}
		}
		this.VerificaPosizione();
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x00113ED8 File Offset: 0x001120D8
	private void Morte()
	{
		if (base.transform.position.y < -50f)
		{
			base.GetComponent<PresenzaNemico>().morto = true;
		}
		if (this.morto)
		{
			this.insettoNav.speed = 0f;
			this.insettoAnim.SetBool(this.camminataHash, false);
		}
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x00113F3C File Offset: 0x0011213C
	private void Salto()
	{
		if (!this.inSalto)
		{
			this.valutaSalto = true;
		}
		else
		{
			this.valutaSalto = false;
		}
		if (this.valutaSalto)
		{
			if (this.distDestinazione <= this.distanzaMaxSaltoReale)
			{
				if (this.bersaglio != null)
				{
					bool flag = false;
					do
					{
						float num = UnityEngine.Random.Range(-1f, 1f);
						float num2 = UnityEngine.Random.Range(-1f, 1f);
						float num3;
						if (this.bersaglio.GetComponent<NavMeshAgent>())
						{
							num3 = this.bersaglio.GetComponent<NavMeshAgent>().radius;
						}
						else
						{
							num3 = 10f;
						}
						this.destSalto = new Vector3(this.destPrincipale.x + num * num3 * 2.5f, this.destPrincipale.y, this.destPrincipale.z + num2 * num3 * 2.5f);
						float num4 = Vector3.Distance(this.destPrincipale, this.destSalto);
						if (num4 > num3)
						{
							flag = true;
							this.cercaCoordinateSalto = true;
						}
					}
					while (!flag);
				}
				else
				{
					this.destSalto = this.destPrincipale;
					this.cercaCoordinateSalto = true;
				}
				Vector3 vector = Vector3.Lerp(this.centroInsetto, this.destSalto, 0.5f);
				this.verticeSalto = new Vector3(vector.x, this.destSalto.y + this.altezzaSalto, vector.z);
			}
			else
			{
				Ray ray = new Ray(this.centroInsetto, (this.destPrincipale + Vector3.up * 3f - this.centroInsetto).normalized);
				Vector3 zero = Vector3.zero;
				if (!Physics.Raycast(ray, out this.sensoreSalto, this.distanzaMaxSaltoReale, this.layerNavigazione))
				{
					if (Physics.Raycast(ray.GetPoint(this.distanzaMaxSaltoReale), -Vector3.up, out this.sensoreSalto, 999f, this.layerSalto))
					{
						this.destSalto = this.sensoreSalto.point;
						this.cercaCoordinateSalto = true;
						Vector3 vector2 = Vector3.Lerp(this.centroInsetto, this.destSalto, 0.5f);
						this.verticeSalto = new Vector3(vector2.x, this.destSalto.y + this.altezzaSalto, vector2.z);
					}
				}
				else if (Physics.Raycast(ray.GetPoint(this.distanzaMaxSaltoReale), -Vector3.up, out this.sensoreSalto, this.distDestinazione, this.layerSalto))
				{
					this.destSalto = this.sensoreSalto.point + (this.sensoreSalto.point - this.centroInsetto).normalized * 2f;
					this.cercaCoordinateSalto = true;
					Vector3 vector3 = Vector3.Lerp(this.centroInsetto, this.destSalto, 0.5f);
					this.verticeSalto = new Vector3(vector3.x, this.destSalto.y + this.altezzaSalto, vector3.z);
				}
			}
		}
		if (this.cercaCoordinateSalto)
		{
			if (!Physics.Linecast(this.centroInsetto, this.verticeSalto, this.layerNavigazione))
			{
				if (!Physics.Linecast(this.verticeSalto, this.destSalto, this.layerNavigazione))
				{
					this.dirSalto = (this.verticeSalto - this.centroInsetto).normalized;
					this.dirDiscesa = (this.destSalto - this.verticeSalto).normalized;
					this.inSalto = true;
				}
				else
				{
					this.NavigazioneAutomatica();
				}
			}
			else
			{
				this.NavigazioneAutomatica();
			}
			this.cercaCoordinateSalto = false;
		}
		if (this.inSalto)
		{
			this.timerSalto += Time.deltaTime;
			this.insettoAnim.SetBool(this.camminataHash, false);
			this.insettoAnim.SetBool(this.saltoHash, true);
			this.insettoNav.enabled = false;
			if (!this.distSaltoCalcolata)
			{
				this.distDestSalto = Vector3.Distance(base.transform.position, new Vector3(this.destSalto.x, base.transform.position.y, this.destSalto.z));
				this.distSaltoCalcolata = true;
				this.timerCammFraSalti = 0f;
			}
			if (this.timerSalto > this.tempoPreSalto)
			{
				this.inAria = true;
				if (!this.verticeRaggiunto)
				{
					float num5 = Vector3.Distance(base.transform.position, new Vector3(this.verticeSalto.x, base.transform.position.y, this.verticeSalto.z));
					if (num5 > 250f)
					{
						num5 = 250f;
					}
					this.increVelSalto = num5 / (this.distDestSalto / 2f) + 0.3f;
					if (Vector3.Distance(this.centroInsetto, this.verticeSalto) > 2f)
					{
						base.transform.position += this.dirSalto * 1f * this.velocitàSalto * Time.deltaTime * this.increVelSalto;
					}
					else
					{
						this.verticeRaggiunto = true;
					}
				}
				else
				{
					float num6 = Vector3.Distance(base.transform.position, new Vector3(this.destSalto.x, base.transform.position.y, this.destSalto.z));
					this.increVelSalto = 1f - num6 / (this.distDestSalto / 2f) + 0.3f;
					float maxDistance;
					if (Time.timeScale < 3f)
					{
						maxDistance = Time.timeScale * this.altezzaSensTerreno;
					}
					else
					{
						maxDistance = Time.timeScale * 3f * this.altezzaSensTerreno;
					}
					if (!Physics.Raycast(this.centroInsetto, -Vector3.up, out this.sensoreTerreno, maxDistance, this.layerSalto))
					{
						if (this.increVelSalto > 1f)
						{
							this.increVelSalto = 1f;
						}
						base.transform.position += this.dirDiscesa * 1f * this.velocitàSalto * Time.deltaTime * this.increVelSalto;
					}
					else
					{
						base.transform.position = this.sensoreTerreno.point;
						this.verticeRaggiunto = false;
						this.inSalto = false;
						this.timerSalto = 0f;
						this.insettoNav.enabled = true;
						this.inAria = false;
						base.transform.LookAt(new Vector3(this.destPrincipale.x, base.transform.position.y, this.destPrincipale.z));
						this.insettoAnim.SetBool(this.saltoHash, false);
						this.increVelSalto = 1f;
						this.distSaltoCalcolata = false;
						this.distanzaMaxSaltoReale = UnityEngine.Random.Range(this.distanzaMaxDiSalto - 50f, this.distanzaMaxDiSalto + 50f);
					}
				}
			}
		}
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x001146B0 File Offset: 0x001128B0
	private void NavigazioneAutomatica()
	{
		this.timerControlloDest += Time.deltaTime;
		if (this.insettoNav.isOnNavMesh)
		{
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
		}
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
				float num = this.destPrincipale.y - base.transform.position.y;
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
				Vector3 direction = rotation * base.transform.forward;
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
			this.insettoNav.speed = this.velocitàIniziale;
		}
		else
		{
			this.insettoNav.speed = 0f;
		}
		if (this.insettoNav.velocity.magnitude != 0f)
		{
			this.insettoAnim.SetBool(this.camminataHash, true);
		}
		else
		{
			this.insettoAnim.SetBool(this.camminataHash, false);
		}
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x00114A44 File Offset: 0x00112C44
	private void VerificaPosizione()
	{
		if (base.transform.position.y < -0.5f)
		{
			base.transform.position = new Vector3(base.transform.position.x, -0.08f, base.transform.position.z);
		}
	}

	// Token: 0x04001D13 RID: 7443
	private GameObject bersaglio;

	// Token: 0x04001D14 RID: 7444
	private Vector3 destPrincipale;

	// Token: 0x04001D15 RID: 7445
	private int layerNavigazione;

	// Token: 0x04001D16 RID: 7446
	private RaycastHit hitSuperficie;

	// Token: 0x04001D17 RID: 7447
	private NavMeshAgent insettoNav;

	// Token: 0x04001D18 RID: 7448
	private float velocitàIniziale;

	// Token: 0x04001D19 RID: 7449
	private bool inJump;

	// Token: 0x04001D1A RID: 7450
	private bool calcoloDistJump;

	// Token: 0x04001D1B RID: 7451
	private bool calcoloJumpEffettuato;

	// Token: 0x04001D1C RID: 7452
	private bool muoviti;

	// Token: 0x04001D1D RID: 7453
	private bool morto;

	// Token: 0x04001D1E RID: 7454
	private Animator insettoAnim;

	// Token: 0x04001D1F RID: 7455
	private int camminataHash = Animator.StringToHash("insetto-camminata");

	// Token: 0x04001D20 RID: 7456
	private bool valutaSalto;

	// Token: 0x04001D21 RID: 7457
	private Vector3 verticeSalto;

	// Token: 0x04001D22 RID: 7458
	public float distanzaMinDiSalto;

	// Token: 0x04001D23 RID: 7459
	public float distanzaMaxDiSalto;

	// Token: 0x04001D24 RID: 7460
	private float distanzaMaxSaltoReale;

	// Token: 0x04001D25 RID: 7461
	public float altezzaSalto;

	// Token: 0x04001D26 RID: 7462
	public float velocitàSalto;

	// Token: 0x04001D27 RID: 7463
	private bool inSalto;

	// Token: 0x04001D28 RID: 7464
	private float timerSalto;

	// Token: 0x04001D29 RID: 7465
	private int saltoHash = Animator.StringToHash("insetto-salto");

	// Token: 0x04001D2A RID: 7466
	private Vector3 dirSalto;

	// Token: 0x04001D2B RID: 7467
	private Vector3 dirDiscesa;

	// Token: 0x04001D2C RID: 7468
	private bool verticeRaggiunto;

	// Token: 0x04001D2D RID: 7469
	private RaycastHit sensoreTerreno;

	// Token: 0x04001D2E RID: 7470
	public float altezzaSensTerreno;

	// Token: 0x04001D2F RID: 7471
	private bool inAria;

	// Token: 0x04001D30 RID: 7472
	public float tempoPreSalto;

	// Token: 0x04001D31 RID: 7473
	private Vector3 destSalto;

	// Token: 0x04001D32 RID: 7474
	private int layerSalto;

	// Token: 0x04001D33 RID: 7475
	private float distDestinazione;

	// Token: 0x04001D34 RID: 7476
	private RaycastHit sensoreSalto;

	// Token: 0x04001D35 RID: 7477
	private bool cercaCoordinateSalto;

	// Token: 0x04001D36 RID: 7478
	private Vector3 centroInsetto;

	// Token: 0x04001D37 RID: 7479
	private float increVelSalto;

	// Token: 0x04001D38 RID: 7480
	private bool distSaltoCalcolata;

	// Token: 0x04001D39 RID: 7481
	private float distDestSalto;

	// Token: 0x04001D3A RID: 7482
	public float tempoCammFraSalti;

	// Token: 0x04001D3B RID: 7483
	private float timerCammFraSalti;

	// Token: 0x04001D3C RID: 7484
	private float timerControlloDest;

	// Token: 0x04001D3D RID: 7485
	private int tipoBattaglia;
}
