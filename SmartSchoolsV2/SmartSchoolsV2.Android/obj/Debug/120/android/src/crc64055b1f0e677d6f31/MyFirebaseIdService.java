package crc64055b1f0e677d6f31;


public class MyFirebaseIdService
	extends com.google.firebase.iid.FirebaseInstanceIdService
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTokenRefresh:()V:GetOnTokenRefreshHandler\n" +
			"";
		mono.android.Runtime.register ("SmartSchoolsV2.Droid.Services.MyFirebaseIdService, SmartSchoolsV2.Android", MyFirebaseIdService.class, __md_methods);
	}


	public MyFirebaseIdService ()
	{
		super ();
		if (getClass () == MyFirebaseIdService.class) {
			mono.android.TypeManager.Activate ("SmartSchoolsV2.Droid.Services.MyFirebaseIdService, SmartSchoolsV2.Android", "", this, new java.lang.Object[] {  });
		}
	}


	public void onTokenRefresh ()
	{
		n_onTokenRefresh ();
	}

	private native void n_onTokenRefresh ();

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
