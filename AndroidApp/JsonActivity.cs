using Android.App;
using Android.OS;
using Android.Text.Method;
using Android.Widget;

namespace AndroidApp
{
    [Activity(Label = "JsonActivity")]
    public class JsonActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.json_view);
            TextView jsonTextView = FindViewById<TextView>(Resource.Id.jsonTextView);
            jsonTextView.MovementMethod = new ScrollingMovementMethod();
            jsonTextView.Text = Data.JsonData;
        }
    }
}