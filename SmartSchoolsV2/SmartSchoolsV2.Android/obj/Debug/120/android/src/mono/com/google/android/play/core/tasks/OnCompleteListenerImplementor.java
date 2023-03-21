package mono.com.google.android.play.core.tasks;


public class OnCompleteListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.play.core.tasks.OnCompleteListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onComplete:(Lcom/google/android/play/core/tasks/Task;)V:GetOnComplete_Lcom_google_android_play_core_tasks_Task_Handler:Com.Google.Android.Play.Core.Tasks.IOnCompleteListenerInvoker, PlayCore\n" +
			"";
		mono.android.Runtime.register ("Com.Google.Android.Play.Core.Tasks.IOnCompleteListenerImplementor, PlayCore", OnCompleteListenerImplementor.class, __md_methods);
	}


	public OnCompleteListenerImplementor ()
	{
		super ();
		if (getClass () == OnCompleteListenerImplementor.class) {
			mono.android.TypeManager.Activate ("Com.Google.Android.Play.Core.Tasks.IOnCompleteListenerImplementor, PlayCore", "", this, new java.lang.Object[] {  });
		}
	}


	public void onComplete (com.google.android.play.core.tasks.Task p0)
	{
		n_onComplete (p0);
	}

	private native void n_onComplete (com.google.android.play.core.tasks.Task p0);

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
