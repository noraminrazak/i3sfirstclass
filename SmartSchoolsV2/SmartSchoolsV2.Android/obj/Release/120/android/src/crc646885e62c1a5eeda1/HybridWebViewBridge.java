package crc646885e62c1a5eeda1;


public class HybridWebViewBridge
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_InvokeAction:(Ljava/lang/String;)V:__export__\n" +
			"";
		mono.android.Runtime.register ("Plugin.HybridWebView.Droid.HybridWebViewBridge, Plugin.HybridWebView", HybridWebViewBridge.class, __md_methods);
	}


	public HybridWebViewBridge ()
	{
		super ();
		if (getClass () == HybridWebViewBridge.class) {
			mono.android.TypeManager.Activate ("Plugin.HybridWebView.Droid.HybridWebViewBridge, Plugin.HybridWebView", "", this, new java.lang.Object[] {  });
		}
	}

	public HybridWebViewBridge (crc646885e62c1a5eeda1.HybridWebViewRenderer p0)
	{
		super ();
		if (getClass () == HybridWebViewBridge.class) {
			mono.android.TypeManager.Activate ("Plugin.HybridWebView.Droid.HybridWebViewBridge, Plugin.HybridWebView", "Plugin.HybridWebView.Droid.HybridWebViewRenderer, Plugin.HybridWebView", this, new java.lang.Object[] { p0 });
		}
	}

	@android.webkit.JavascriptInterface

	public void invokeAction (java.lang.String p0)
	{
		n_InvokeAction (p0);
	}

	private native void n_InvokeAction (java.lang.String p0);

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
