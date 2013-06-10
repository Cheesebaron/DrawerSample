using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace DrawerSample
{
    public class PlanetFragment: Fragment
    {
        public static string ArgPlanetNumber = "planet_number";

        public override View OnCreateView(LayoutInflater p0, ViewGroup p1, Bundle p2)
        {
            var rootView = p0.Inflate(Resource.Layout.FragmentPlanet, p1, false);
            var i = Arguments.GetInt(ArgPlanetNumber);
            var planet = Resources.GetStringArray(Resource.Array.PlanetsArray)[i];

            var imageId = Resources.GetIdentifier(
                "dk.ostebaronen.drawersample:drawable/" + planet.ToLowerInvariant(), 
                null, null);
            rootView.FindViewById<ImageView>(Resource.Id.image).SetImageResource(imageId);
            Activity.Title = planet;
            return rootView;
        }
    }
}