using System;
using UnityEngine;

// Token: 0x020000AF RID: 175
public class ArtObserverPartenza : MonoBehaviour
{
	// Token: 0x0600065F RID: 1631 RVA: 0x000E36A4 File Offset: 0x000E18A4
	private void Start()
	{
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.frequenzaColpi = 1f;
		this.suonoColpi = base.GetComponent<AudioSource>();
		this.numColpiTotali = this.infoAlleati.GetComponent<InfoMunizionamento>().ListaQuantitàSupportoTattica[3];
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x000E36F4 File Offset: 0x000E18F4
	private void Update()
	{
		this.timerColpi += Time.deltaTime;
		this.timerScomparsa += Time.deltaTime;
		if (this.numColpiPartiti < this.numColpiTotali)
		{
			if (this.timerColpi > this.frequenzaColpi)
			{
				this.timerColpi = 0f;
				if (this.numColpiPartiti < this.numColpiTotali)
				{
					this.colpoArt = (UnityEngine.Object.Instantiate(this.colpoArtPrefab, base.transform.position, Quaternion.identity) as GameObject);
					this.colpoArt.GetComponent<ColpoArtiglieriaDaObserver>().destTeorica = this.destColpiArt;
					this.numColpiPartiti++;
					this.suonoColpi.Play();
				}
			}
		}
		else if (this.timerColpi > 5f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040017DC RID: 6108
	private GameObject infoAlleati;

	// Token: 0x040017DD RID: 6109
	public GameObject colpoArtPrefab;

	// Token: 0x040017DE RID: 6110
	private GameObject colpoArt;

	// Token: 0x040017DF RID: 6111
	private int numCannoniDispPerArt;

	// Token: 0x040017E0 RID: 6112
	private float timerColpi;

	// Token: 0x040017E1 RID: 6113
	private float frequenzaColpi;

	// Token: 0x040017E2 RID: 6114
	private int numColpiPartiti;

	// Token: 0x040017E3 RID: 6115
	public Vector3 destColpiArt;

	// Token: 0x040017E4 RID: 6116
	private AudioSource suonoColpi;

	// Token: 0x040017E5 RID: 6117
	private int numColpiTotali;

	// Token: 0x040017E6 RID: 6118
	private float timerScomparsa;
}
