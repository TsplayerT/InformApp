using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Com.OneSignal;
using InformApp.Controle;
using InformApp.Servico;
using InformApp.Utilidade;

namespace InformApp.Droid
{
    [Activity(Label = "InformApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                Xamarin.Forms.Forms.Init(this, savedInstanceState);
                OneSignal.Current.StartInit(Constantes.AppId).HandleNotificationReceived(Eventos.NotificacaoRecebida).EndInit();

                LoadApplication(new Principal());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}