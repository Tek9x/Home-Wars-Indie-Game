using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000101 RID: 257
public class CanvasManualeScript : MonoBehaviour
{
	// Token: 0x06000828 RID: 2088 RVA: 0x0011BDD4 File Offset: 0x00119FD4
	private void Start()
	{
		this.elencoPagine = base.transform.GetChild(0).FindChild("sfondo manuale").GetChild(0).GetChild(0).GetChild(0).gameObject;
		this.numeroPagine = this.elencoPagine.transform.childCount;
		this.suonoManuale = base.GetComponent<AudioSource>();
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x0011BE38 File Offset: 0x0011A038
	private void Update()
	{
		if (this.attivaSuonoManuale)
		{
			this.attivaSuonoManuale = false;
			this.suonoManuale.Play();
			for (int i = 0; i < this.numeroPagine; i++)
			{
				this.elencoPagine.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 0f;
			}
			this.elencoPagine.transform.GetChild(this.paginaAperta).GetComponent<CanvasGroup>().alpha = 1f;
			this.barraVerticale.GetComponent<Scrollbar>().value = 1f;
		}
	}

	// Token: 0x04001E53 RID: 7763
	private AudioSource suonoManuale;

	// Token: 0x04001E54 RID: 7764
	public bool attivaSuonoManuale;

	// Token: 0x04001E55 RID: 7765
	public int paginaAperta;

	// Token: 0x04001E56 RID: 7766
	private GameObject elencoPagine;

	// Token: 0x04001E57 RID: 7767
	private int numeroPagine;

	// Token: 0x04001E58 RID: 7768
	public GameObject barraVerticale;
}
