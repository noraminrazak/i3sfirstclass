package crc644d647af8386f1ad1;


public class MonthYearPickerDialog
	extends androidx.fragment.app.DialogFragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreateDialog:(Landroid/os/Bundle;)Landroid/app/Dialog;:GetOnCreateDialog_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("SmartSchoolsV2.Droid.Renderers.MonthYearPickerDialog, SmartSchoolsV2.Android", MonthYearPickerDialog.class, __md_methods);
	}


	public MonthYearPickerDialog ()
	{
		super ();
		if (getClass () == MonthYearPickerDialog.class) {
			mono.android.TypeManager.Activate ("SmartSchoolsV2.Droid.Renderers.MonthYearPickerDialog, SmartSchoolsV2.Android", "", this, new java.lang.Object[] {  });
		}
	}


	public MonthYearPickerDialog (int p0)
	{
		super (p0);
		if (getClass () == MonthYearPickerDialog.class) {
			mono.android.TypeManager.Activate ("SmartSchoolsV2.Droid.Renderers.MonthYearPickerDialog, SmartSchoolsV2.Android", "System.Int32, mscorlib", this, new java.lang.Object[] { p0 });
		}
	}


	public android.app.Dialog onCreateDialog (android.os.Bundle p0)
	{
		return n_onCreateDialog (p0);
	}

	private native android.app.Dialog n_onCreateDialog (android.os.Bundle p0);

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
