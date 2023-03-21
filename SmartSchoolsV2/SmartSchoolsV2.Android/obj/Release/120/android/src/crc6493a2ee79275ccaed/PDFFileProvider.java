package crc6493a2ee79275ccaed;


public class PDFFileProvider
	extends androidx.core.content.FileProvider
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("SmartSchoolsV2.Droid.Class.PDFFileProvider, SmartSchoolsV2.Android", PDFFileProvider.class, __md_methods);
	}


	public PDFFileProvider ()
	{
		super ();
		if (getClass () == PDFFileProvider.class) {
			mono.android.TypeManager.Activate ("SmartSchoolsV2.Droid.Class.PDFFileProvider, SmartSchoolsV2.Android", "", this, new java.lang.Object[] {  });
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
