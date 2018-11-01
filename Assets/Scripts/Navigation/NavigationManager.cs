using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace RetroPlatform.Navigation
{
    public static class NavigationManager
    {
        public static Dictionary<string, Route> RouteInformation = new Dictionary<string, Route>() {
            { "Overworld", new Route { RouteDescription = "Mapa com todas as aventuras", CanTravel = true} },
            { "EntryLevel", new Route { RouteDescription = "Início da aventura", CanTravel = true} },
            { "HalloweenLevel", new Route { RouteDescription = "A floresta das aranhas", CanTravel = true} },
            { "BattleScene", new Route { RouteDescription = "Batalha contra todos inimigos", CanTravel = true}}
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
