namespace RingSoft.DataEntryControls.Maui
{
    public class ControlValidatorControl
    {
        public VisualElement Control { get; }

        public string Name { get; }

        public Type Type { get; }

        public ControlValidatorControl(VisualElement control, string name, Type type)
        {
            Control = control;
            Name = name;
            Type = type;
        }
    }

    public class ControlValidator
    {
        public List<ControlValidatorControl> Controls { get; }

        public Type DeclaringType { get; }

        public ControlValidator(Type declaringType)
        {
            Controls = new List<ControlValidatorControl>();
            DeclaringType = declaringType;
        }

        public void Validate()
        {
            foreach (var controlValidatorControl in Controls)
            {
                if (controlValidatorControl.Control == null)
                {
                    var message = $"{controlValidatorControl.Name} name of type {controlValidatorControl.Type.Name} is missing from the {DeclaringType.Name} Control Template";
                    throw new ArgumentException(message);
                }
            }
        }
}
}
