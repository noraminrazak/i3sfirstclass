package crc646885e62c1a5eeda1;


public class HybridWebViewChromeClient
	extends android.webkit.WebChromeClient
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Plugin.HybridWebView.Droid.HybridWebViewChromeClient, Plugin.HybridWebView", HybridWebViewChromeClient.class, __md_methods);
	}


	public HybridWebViewChromeClient ()
	{
		super ();
		if (getClass () == HybridWebViewChromeClient.class) {
			mono.android.TypeManager.Activate ("Plugin.HybridWebView.Droid.HybridWebViewChromeClient, Plugin.HybridWebView", "", this, new java.lang.Object[] {  });
		}
	}

	public HybridWebViewChromeClient (crc646885e62c1a5eeda1.HybridWebViewRenderer p0)
	{
		super ();
		if (getClass () == HybridWebViewChromeClient.class) {
			mono.android.TypeManager.Activate ("Plugin.HybridWebView.Droid.HybridWebViewChromeClient, Plugin.HybridWebView", "Plugin.HybridWebView.Droid.HybridWebViewRenderer, Plugin.HybridWebView", this, new java.lang.Object[] { p0 });
		}
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
