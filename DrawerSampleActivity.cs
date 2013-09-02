using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace DrawerSample
{
    [Activity(Label = "Drawer Sample", MainLauncher = true, Icon = "@drawable/icon")]
    public class DrawerSampleActivity : Activity
    {
        private DrawerLayout _drawer;
        private MyActionBarDrawerToggle _drawerToggle;
        private ListView _drawerList;

        private string _drawerTitle;
        private string _title;
        private string[] _planetTitles;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _title = _drawerTitle = Title;
            _planetTitles = Resources.GetStringArray(Resource.Array.PlanetsArray);
            _drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            _drawerList = FindViewById<ListView>(Resource.Id.left_drawer);

            _drawer.SetDrawerShadow(Resource.Drawable.drawer_shadow_dark, (int)GravityFlags.Start);
            
            _drawerList.Adapter = new ArrayAdapter<string>(this,
                Resource.Layout.DrawerListItem, _planetTitles);
            _drawerList.ItemClick += (sender, args) => SelectItem(args.Position);


            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            //DrawerToggle is the animation that happens with the indicator next to the
            //ActionBar icon. You can choose not to use this.
            _drawerToggle = new MyActionBarDrawerToggle(this, _drawer,
                                                      Resource.Drawable.ic_drawer_light,
                                                      Resource.String.DrawerOpen,
                                                      Resource.String.DrawerClose);

            //You can alternatively use _drawer.DrawerClosed here
            _drawerToggle.DrawerClosed += delegate
            {
                ActionBar.Title = _title;
                InvalidateOptionsMenu();
            };

            //You can alternatively use _drawer.DrawerOpened here
            _drawerToggle.DrawerOpened += delegate
            {
                ActionBar.Title = _drawerTitle;
                InvalidateOptionsMenu();
            };

            _drawer.SetDrawerListener(_drawerToggle);

            if (null == savedInstanceState)
                SelectItem(0);
        }

        private void SelectItem(int position)
        {
            var fragment = new PlanetFragment();
            var arguments = new Bundle();
            arguments.PutInt(PlanetFragment.ArgPlanetNumber, position);
            fragment.Arguments = arguments;

            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();

            _drawerList.SetItemChecked(position, true);
            ActionBar.Title = _title = _planetTitles[position];
            _drawer.CloseDrawer(_drawerList);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            _drawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            _drawerToggle.OnConfigurationChanged(newConfig);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            var drawerOpen = _drawer.IsDrawerOpen(Resource.Id.left_drawer);
            menu.FindItem(Resource.Id.action_websearch).SetVisible(!drawerOpen);
            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (_drawerToggle.OnOptionsItemSelected(item))
                return true;

            switch (item.ItemId)
            {
                case Resource.Id.action_websearch:
                    {
                        var intent = new Intent(Intent.ActionWebSearch);
                        intent.PutExtra(SearchManager.Query, ActionBar.Title);

                        if ((intent.ResolveActivity(PackageManager)) != null)
                            StartActivity(intent);
                        else
                            Toast.MakeText(this, Resource.String.app_not_available, ToastLength.Long).Show();
                        return true;
                    }
                case Resource.Id.action_slidingpane:
                    {
                        var intent = new Intent(this, typeof(SlidingPaneLayoutActivity));
                        intent.AddFlags(ActivityFlags.ClearTop);
                        StartActivity(intent);
                        return true;
                    }
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}

