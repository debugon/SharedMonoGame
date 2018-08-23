using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using SharedProject;

namespace forAndroid
{
    [Activity(Label = "forAndroid"
        , MainLauncher = true
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape     //Android�̉���ʐݒ�
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //�i�r�Q�[�V�����o�[�A���O���E��
            //Android P�Ń^�X�N�ύX��A�Ĕ����m�F
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)
                                               ( SystemUiFlags.LowProfile
                                               | SystemUiFlags.Fullscreen
                                               | SystemUiFlags.HideNavigation
                                               | SystemUiFlags.ImmersiveSticky
                                               );

            var g = new Game1();
            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }
    }
}

