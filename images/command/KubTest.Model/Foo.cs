using System;
using KubTest.EventSourcing;

namespace KubTest.Model
{
	public class Foo : BaseEventSourceModel, IEventSource<FooCreated>, IEventSource<ColorChanged>
	{
		public string Color { get; set; }

		public void ChangeColor(string newColor)
		{
			if (string.IsNullOrWhiteSpace(newColor))
				throw new ArgumentNullException(nameof(newColor));

			if (!string.Equals(newColor, Color, StringComparison.OrdinalIgnoreCase))
				Raise(new ColorChanged(Color, newColor));
		}

        public static Foo Create(Guid id, string color)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            if (string.IsNullOrWhiteSpace(color))
                throw new ArgumentNullException(nameof(color));

            var foo = new Foo() { Id = id };
            foo.Raise(new FooCreated(color));
            return foo;
        }

		public void ApplyEvent(ColorChanged evt)
		{
			Color = evt.To;
		}

		public void ApplyEvent(FooCreated evt)
		{
			Color = evt.Color;
		}
	}
}