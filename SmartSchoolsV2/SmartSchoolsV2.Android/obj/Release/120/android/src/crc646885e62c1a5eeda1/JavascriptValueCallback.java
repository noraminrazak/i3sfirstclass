package crc646885e62c1a5eeda1;


public class JavascriptValueCallback
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.webkit.ValueCallback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onReceiveValue:(Ljava/lang/Object;)V:GetOnReceiveValue_Ljava_lang_Object_Handler:Android.Webkit.IValueCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Plugin.HybridWebView.Droid.JavascriptValueCallback, Plugin.HybridWebView", JavascriptValueCallback.class, __md_methods);
	}


	public JavascriptValueCallback ()
	{
		super ();
		if (getClass () == JavascriptValueCallback.class) {
			mono.android.TypeManager.Activate ("Plugin.HybridWebView.Droid.JavascriptValueCallback, Plugin.HybridWebView", "", this, new java.lang.Object[] {  });
		}
	}

	public JavascriptValueCallback (crc646885e62c1a5eeda1.HybridWebViewRenderer p0)
	{
		super ();
		if (getClass () == JavascriptValueCallback.class) {
			mono.android.TypeManager.Activate ("Plugin.HybridWebView.Droid.JavascriptValueCallback, Plugin.HybridWebView", "Plugin.HybridWebView.Droid.HybridWebViewRenderer, Plugin.HybridWebView", this, new java.lang.Object[] { p0 });
		}
	}


	public void onReceiveValue (java.lang.Object p0)
	{
		n_onReceiveValue (p0);
	}

	private native void n_onReceiveValue (java.lang.Object p0);

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
