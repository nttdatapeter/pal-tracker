using Microsoft.AspNetCore.Mvc;

namespace PalTracker
{
    

    [Route("/")]
    public class WelcomeController : ControllerBase
    {
        private readonly WelcomeMessage _welcomeMessage;

        [HttpGet]
        public string SayHello() => _welcomeMessage.Message;

        public WelcomeController(WelcomeMessage welcomeMessage)
        {
            _welcomeMessage = welcomeMessage;
        }
    }

    
}