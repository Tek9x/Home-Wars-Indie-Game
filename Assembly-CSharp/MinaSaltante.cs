using System;
using UnityEngine;

// Token: 0x020000FD RID: 253
public class MinaSaltante : MonoBehaviour
{
	// Token: 0x0600080D RID: 2061 RVA: 0x0011A650 File Offset: 0x00118850
	private void Start()
	{
		this.baseMina = base.transform.GetChild(2).gameObject;
		this.corpoMina = base.transform.GetChild(4).gameObject;
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x0011A68C File Offset: 0x0011888C
	private void Update()
	{
		if (!this.saltoAttivo)
		{
			if (base.GetComponent<PresenzaTrappola>().disposta)
			{
				this.Innesco();
			}
		}
		else if (!this.esplosioneAttiva)
		{
			this.Salto();
		}
		else
		{
			this.Esplosione();
		}
	}

	// Token: 0x0600080F RID: 2063 RVA: 0x0011A6DC File Offset: 0x001188DC
	private void Innesco()
	{
		foreach (GameObject current in this.corpoMina.GetComponent<ColliderAreaTrappole>().ListaAreaTrappola)
		{
			if (current != null)
			{
				float num = Vector3.Distance(current.transform.position, base.transform.position);
				if (num < this.distanzaInnesco)
				{
					this.saltoAttivo = true;
					this.baseMina.GetComponent<AudioSource>().Play();
				}
			}
		}
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x0011A790 File Offset: 0x00118990
	private void Salto()
	{
		if (this.corpoMina.transform.position.y < this.baseMina.transform.position.y + 15f)
		{
			this.corpoMina.transform.position += Vector3.up * 150f * Time.deltaTime;
		}
		else
		{
			this.esplosioneAttiva = true;
			this.corpoMina.GetComponent<AudioSource>().Play();
		}
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x0011A828 File Offset: 0x00118A28
	private void Esplosione()
	{
		this.timerPostEspl += Time.deltaTime;
		if (!this.dannoEffettuato)
		{
			foreach (GameObject current in this.corpoMina.GetComponent<ColliderAreaTrappole>().ListaAreaTrappola)
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
			this.corpoMina.GetComponent<ParticleSystem>().Play();
			this.corpoMina.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
			this.baseMina.GetComponent<MeshRenderer>().enabled = false;
			this.corpoMina.GetComponent<MeshRenderer>().enabled = false;
		}
		if (this.timerPostEspl > 3f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04001E12 RID: 7698
	public float distanzaInnesco;

	// Token: 0x04001E13 RID: 7699
	private GameObject baseMina;

	// Token: 0x04001E14 RID: 7700
	private GameObject corpoMina;

	// Token: 0x04001E15 RID: 7701
	private bool esplosioneAttiva;

	// Token: 0x04001E16 RID: 7702
	private bool dannoEffettuato;

	// Token: 0x04001E17 RID: 7703
	private float timerPostEspl;

	// Token: 0x04001E18 RID: 7704
	private bool saltoAttivo;
}
