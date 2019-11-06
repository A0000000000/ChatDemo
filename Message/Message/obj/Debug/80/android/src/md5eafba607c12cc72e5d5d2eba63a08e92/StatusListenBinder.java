package md5eafba607c12cc72e5d5d2eba63a08e92;


public class StatusListenBinder
	extends android.os.Binder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Message.StatusListenBinder, Message", StatusListenBinder.class, __md_methods);
	}


	public StatusListenBinder ()
	{
		super ();
		if (getClass () == StatusListenBinder.class)
			mono.android.TypeManager.Activate ("Message.StatusListenBinder, Message", "", this, new java.lang.Object[] {  });
	}

	public StatusListenBinder (md5eafba607c12cc72e5d5d2eba63a08e92.StatusListenService p0)
	{
		super ();
		if (getClass () == StatusListenBinder.class)
			mono.android.TypeManager.Activate ("Message.StatusListenBinder, Message", "Message.StatusListenService, Message", this, new java.lang.Object[] { p0 });
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
