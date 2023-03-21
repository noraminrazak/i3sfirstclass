package crc6433eabb2f1deddbd5;


public class StoreReviewImplementation
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
		mono.android.Runtime.register ("Plugin.StoreReview.StoreReviewImplementation, Plugin.StoreReview", StoreReviewImplementation.class, __md_methods);
	}


	public StoreReviewImplementation ()
	{
		super ();
		if (getClass () == StoreReviewImplementation.class) {
			mono.android.TypeManager.Activate ("Plugin.StoreReview.StoreReviewImplementation, Plugin.StoreReview", "", this, new java.lang.Object[] {  });
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
