using System;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace WebApiSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NodaTimeValuesController : ControllerBase
    {
        [HttpGet("[action]")]
        public ActionResult<NodaTimeModel> GetModel()
        {
            var dateTimeZone = DateTimeZoneProviders.Tzdb.GetSystemDefault();
            Instant instant = Instant.FromDateTimeUtc(DateTime.UtcNow);
            NodaTimeModel nodaTimeModel = new NodaTimeModel
            {
                DateTimeZone = DateTimeZone.Utc,
                Instant = instant,
                DateTime = DateTime.UtcNow,
                Interval = new Interval(instant, instant.Plus(Duration.FromHours(1))),
                Period = Period.FromHours(1),
                ZonedDateTime = instant.InZone(dateTimeZone)
            };
            return nodaTimeModel;
        }
    }
}
