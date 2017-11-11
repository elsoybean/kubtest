using System;
using KubTest.EventSourcing;

namespace KubTest.Model
{
	public class FooCreated : BaseEventArgs  
	{	
	    public string Color { get; private set; }  

	    public FooCreated(string color)
	    {  
	        Color = color;
	    }
	}
}