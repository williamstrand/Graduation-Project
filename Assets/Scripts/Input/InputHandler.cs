namespace WSP.Input
{
    public static class InputHandler
    {
        public static Controls Controls
        {
            get
            {
                if (controls != null) return controls;

                controls = new Controls();
                controls.Enable();

                return controls;
            }
        }

        static Controls controls;
    }
}