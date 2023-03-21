package crc644d647af8386f1ad1;


public class NumberPickerDialog
	extends androidx.fragment.app.DialogFragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("SmartSchoolsV2.Droid.Renderers.NumberPickerDialog, SmartSchoolsV2.Android", NumberPickerDialog.class, __md_methods);
	}


	public NumberPickerDialog ()
	{
		super ();
		if (getClass () == NumberPickerDialog.class) {
			mono.android.TypeManager.Activate ("SmartSchoolsV2.Droid.Renderers.NumberPickerDialog, SmartSchoolsV2.Android", "", this, new java.lang.Object[] {  });
		}
	}


	public NumberPickerDialog (int p0)
	{
		super (p0);
		if (getClass () == NumberPickerDialog.class) {
			mono.android.TypeManager.Activate ("SmartSchoolsV2.Droid.Renderers.NumberPickerDialog, SmartSchoolsV2.Android", "System.Int32, mscorlib", this, new java.lang.Object[] { p0 });
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
