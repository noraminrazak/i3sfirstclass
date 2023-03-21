package mono.com.google.android.play.core.listener;


public class StateUpdatedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.play.core.listener.StateUpdatedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onStateUpdate:(Ljava/lang/Object;)V:GetOnStateUpdate_Ljava_lang_Object_Handler:Com.Google.Android.Play.Core.Listener.IStateUpdatedListenerInvoker, PlayCore\n" +
			"";
		mono.android.Runtime.register ("Com.Google.Android.Play.Core.Listener.IStateUpdatedListenerImplementor, PlayCore", StateUpdatedListenerImplementor.class, __md_methods);
	}


	public StateUpdatedListenerImplementor ()
	{
		super ();
		if (getClass () == StateUpdatedListenerImplementor.class) {
			mono.android.TypeManager.Activate ("Com.Google.Android.Play.Core.Listener.IStateUpdatedListenerImplementor, PlayCore", "", this, new java.lang.Object[] {  });
		}
	}


	public void onStateUpdate (java.lang.Object p0)
	{
		n_onStateUpdate (p0);
	}

	private native void n_onStateUpdate (java.lang.Object p0);

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
