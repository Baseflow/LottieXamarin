namespace Example.Forms.Tizen
{
    internal class Program : global::Xamarin.Forms.Platform.Tizen.FormsApplication
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            LoadApplication(new App());
        }

        protected override void OnTerminate()
        {
            base.OnTerminate();
        }

        private static void Main(string[] args)
        {
            var app = new Program();
            Xamarin.Forms.Forms.Init(app, true);
            app.Run(args);
        }
    }
}
