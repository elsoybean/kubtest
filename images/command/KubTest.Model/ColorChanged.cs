using KubTest.EventSourcing;

namespace KubTest.Model
{
	public class ColorChanged : BaseEvent  
	{	
	    public string From { get; private set; }  
	    public string To { get; private set; }

	    public ColorChanged(string from, string to)  
	    {  
	        From = from;
	        To = to;
	    }
	}
}