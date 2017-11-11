using KubTest.EventSourcing;

namespace KubTest.Model
{
	public class FooCreated : BaseEvent
    {	
	    public string Color { get; private set; }  

	    public FooCreated(string color)
	    {  
	        Color = color;
	    }
	}
}