using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Twilio.AspNet.Core;
using Twilio.TwiML;

namespace wishes_app.Controllers
{
    [ApiController]
    [Route("api/")]
    public class WebhookController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public WebhookController(AppDbContext dbContext)
        {
            _dbContext = dbContext;   
        }
        [HttpPost("IncomingMessage")]
        public async Task<IActionResult> IncomingMessage()
        {
            var form = await Request.ReadFormAsync();
            var body = form["Body"];
            var sender =form["From"];

            _dbContext.WishItems.Add(new WishItem{Message = body, Sender = sender});
            _dbContext.SaveChanges();
           
            var response = new MessagingResponse();
            response.Message("Thank you for the wish. Happy Holidays!");

            return new TwiMLResult(response);

        }

        [HttpPost("WishPlayer")]
        public IActionResult WishPlayer()
        {
            int savedMessages = _dbContext.WishItems.Count();
            string wish = string.Empty;
            if (savedMessages > 0)
            {
                var rand = new Random().Next(_dbContext.WishItems.Count());
                wish = _dbContext.WishItems.AsEnumerable().ElementAt(rand).Message;
            }
            else
            {
                wish = "No wishes present yet, check back later. Happy holidays!";
            }

            var response = new VoiceResponse();
            response.Say($"Reading out a wish for you", voice: "alice");
            response.Say(wish);

            return new TwiMLResult(response);
        }
    }
}