using System;
using UnityEngine;

// Token: 0x020000FE RID: 254
public class MinaSemplice : MonoBehaviour
{
	// Token: 0x06000813 RID: 2067 RVA: 0x0011A9A4 File Offset: 0x00118BA4
	private void Start()
	{
		this.corpoMina = base.transform.GetChild(2).gameObject;
		this.esplosione = base.transform.GetChild(4).gameObject;
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x0011A9E0 File Offset: 0x00118BE0
	private void Update()
	{
		if (!this.esplosioneAttiva)
		{
			if (base.GetComponent<PresenzaTrappola>().disposta)
			{
				this.Innesco();
			}
		}
		else
		{
			this.Esplosione();
		}
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x0011AA1C File Offset: 0x00118C1C
	private void Innesco()
	{
		foreach (GameObject current in base.GetComponent<ColliderAreaTrappole>().ListaAreaTrappola)
		{
			if (current != null)
			{
				float num = Vector3.Distance(current.transform.position, base.transform.position);
				if (num < this.distanzaInnesco)
				{
					this.esplosioneAttiva = true;
					base.GetComponent<AudioSource>().Play();
				}
			}
		}
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x0011AAC8 File Offset: 0x00118CC8
	private void Esplosione()
	{
		this.timerPostEspl += Time.deltaTime;
		if (!this.dannoEffettuato)
		{
			foreach (GameObject current in base.GetComponent<ColliderAreaTrappole>().ListaAreaTrappola)
			{
				if (current != null)
				{
					float num = current.GetComponent<PresenzaNemico>().armatura - base.GetComponent<PresenzaTrappola>().penetrazione;
					if (num <= 0f)
					{
						current.GetComponent<PresenzaNemico>().vita -= base.GetComponent<PresenzaTrappola>().danno;
					}
					else
					{
						float num2 = num - base.GetComponent<PresenzaTrappola>().danno;
						if (num2 < 0f)
						{
							current.GetComponent<PresenzaNemico>().vita += num2;
						}
					}
				}
			}
			this.dannoEffettuato = true;
			base.GetComponent<ParticleSystem>().Play();
			this.esplosione.GetComponent<ParticleSystem>().Play();
			this.corpoMina.GetComponent<MeshRenderer>().enabled = false;
		}
		if (this.timerPostEspl > 3f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04001E19 RID: 7705
	public float distanzaInnesco;

	// Token: 0x04001E1A RID: 7706
	private GameObject corpoMina;

	// Token: 0x04001E1B RID: 7707
	private GameObject esplosione;

	// Token: 0x04001E1C RID: 7708
	private bool esplosioneAttiva;

	// Token: 0x04001E1D RID: 7709
	private bool dannoEffettuato;

	// Token: 0x04001E1E RID: 7710
	private float timerPostEspl;
}
