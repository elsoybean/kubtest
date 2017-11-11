using System;
using KubTest.EventSourcing;

namespace KubTest.Model
{
	public class Foo : AbstractEventSourceModel, IEventSource<FooCreated>, IEventSource<ColorChanged>
	{
		public string Color { get; set; }

		public void ChangeColor(string newColor)
		{
			if (string.IsNullOrWhiteSpace(newColor))
				throw new ArgumentNullException("newColor");

			if (!string.Equals(newColor, Color, StringComparison.OrdinalIgnoreCase))
				Raise(new ColorChanged(Color, newColor));
		}

		public void ApplyEvent(ColorChanged eventArgs)
		{
			Color = eventArgs.To;
		}

		public void ApplyEvent(FooCreated eventArgs)
		{
			Color = eventArgs.Color;
		}
	}
}