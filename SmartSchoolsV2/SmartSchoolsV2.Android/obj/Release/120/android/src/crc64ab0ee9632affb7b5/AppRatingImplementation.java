package crc64ab0ee9632affb7b5;


public class AppRatingImplementation
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
		mono.android.Runtime.register ("Plugin.XamarinAppRating.AppRatingImplementation, Plugin.XamarinAppRating", AppRatingImplementation.class, __md_methods);
	}


	public AppRatingImplementation ()
	{
		super ();
		if (getClass () == AppRatingImplementation.class) {
			mono.android.TypeManager.Activate ("Plugin.XamarinAppRating.AppRatingImplementation, Plugin.XamarinAppRating", "", this, new java.lang.Object[] {  });
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
