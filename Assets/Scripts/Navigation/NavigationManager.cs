using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace RetroPlatform.Navigation
{
    public static class NavigationManager
    {
        public static Dictionary<string, Route> RouteInformation = new Dictionary<string, Route>() {
            { "Overworld", new Route { RouteDescription = "The big bad world", CanTravel = true} },
            { "EntryLevel", new Route { RouteDescription = "Start of the game", CanTravel = true} },
            { "Construction", new Route { RouteDescription = "The construction area", CanTravel = false}} 
        }; 

        public static string GetRouteInfo(string destination)
        {
            return RouteInformation.ContainsKey(destination) ? RouteInformation[destination].RouteDescription : null;
        }

        public static bool CanNavigate(string destination)
        {
            return RouteInformation.ContainsKey(destination) ? RouteInformation[destination].CanTravel : false;
        }

        public static void NavigateTo(string destination)
        {
            SceneManager.LoadScene(destination);
        }
    }
}
